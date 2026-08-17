using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class MenuPreferences : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void toolStripTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void toolstripFormClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region Variables
        //private ColorPickerDialog colorDialog = new() { BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.White };
        //public static AdamsLair.WinForms.ColorControls.ColorPickerDialog colorDialog = new();
        public ColorDialog colorDialog = new();
        private Dictionary<string, Keys> DictKeybind = new();
        private Dictionary<string, Keys> DictRandomization = new();
        #endregion
        #region Form Construction and initialization
        public MenuPreferences()
        {
            InitializeComponent();
            toolstripCustomize.Renderer = new ToolStripOverride();
            //load custom colors from previous
            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //set propertygrid to the color settings
            propertyGridUIColors.SelectedObject = TCLE.settingsUITheme;
            //setup Sequencer colors
            foreach (KeyValuePair<string, Bitmap> bmp in TCLE.ColorIcons)
                imageList1.Images.Add(bmp.Key, bmp.Value);
            BuildObjectTree();
            //set mute
            checkMuteApp.Checked = Properties.Settings.Default.muteapplication;
            checkAntiDuck.Checked = Properties.Settings.Default.muteduck;
            //read keybinds to a dictionary for easier lookup
            if (Properties.Settings.Default.UserKeybinds == "-") {
                DictKeybind = Properties.Resources.DefaultKeybinds.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
            }
            else
                DictKeybind = Properties.Settings.Default.UserKeybinds.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
            propertyGridKeyBinds.SelectedObject = new DictionaryPropertyGridAdapter(DictKeybind);
            //randomization values
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //Get the working area of the TabControl main control
            Rectangle rec = tabControl1.ClientRectangle;
            //Create a StringFormat object to set the layout of the label text
            StringFormat StrFormat = new() {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            //Draw the background of the main control
            e.Graphics.FillRectangle(Brushes.Black, rec);

            //Draw label style
            Font fntTab = e.Font;
            Brush bshBack = new SolidBrush(Color.FromArgb(30, 30, 30));
            Brush bshBack2 = new SolidBrush(Color.FromArgb(55, 55, 55));

            for (int i = 0; i < tabControl1.TabPages.Count; i++) {
                bool bSelected = (tabControl1.SelectedIndex == i);
                Rectangle recBounds = tabControl1.GetTabRect(i);
                RectangleF tabTextArea = (RectangleF)tabControl1.GetTabRect(i);
                if (bSelected) {
                    recBounds.Inflate(-2, -2);
                    e.Graphics.FillRoundedRectangle(bshBack2, recBounds, 10);
                    recBounds.X += 8;
                    e.Graphics.FillRectangle(bshBack2, recBounds);
                    e.Graphics.DrawString(tabControl1.TabPages[i].Text, fntTab, Brushes.White, tabTextArea, StrFormat);
                }
                else {
                    recBounds.Inflate(-2, -2);
                    e.Graphics.FillRoundedRectangle(bshBack, recBounds, 10);
                    e.Graphics.DrawString(tabControl1.TabPages[i].Text, fntTab, Brushes.White, tabTextArea, StrFormat);
                }
            }
        }
        #endregion
        #region Form Closing
        private void btnCustomizeApply_Click(object sender, EventArgs e)
        {
            //save colors to settings
            TCLE.settingsUITheme.Save();
            TCLE.ColorFormElements(TCLE.Instance);

            //write sequencer colors to txt file
            File.WriteAllLines($@"{TCLE.AppLocation}\settings\objects_defaultcolors_v3.txt", TCLE.LeafObjects.Select(x => $"{x.Value.ParamDisplayName};{x.Value.DefaultColor.ToArgb()}"));
            UtilImport.ImportDefaultColors();
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            SeqObjTreeBuilder.BuildMasterObjectTree();
            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>()) {
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.txtSearch.Text);
                leaf.TrackTimeSigHighlighting();
            }

            //save mute to settings
            Properties.Settings.Default.muteapplication = checkMuteApp.Checked;
            Properties.Settings.Default.muteduck = checkAntiDuck.Checked;

            //write keybinds to txt file
            ///File.WriteAllLines($@"{TCLE.AppLocation}\settings\keybinds.txt", keybindfromfile.Select(x => $"{x.Key};{x.Value}"));
            Properties.Settings.Default.UserKeybinds = string.Join('\n', DictKeybind.Select(x => $"{x.Key};{x.Value}"));
            TCLE.Instance.SetKeyBinds();

            //save properties
            Properties.Settings.Default.Save();

            UtilAudio.PlaySound("UIinterpolate");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
        #region UI colors
        private void btnSetColor(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                UtilAudio.PlaySound("UIcolorapply");
                btn.BackColor = colorDialog.Color;
            }
        }

        private void btnObjectColor_Click(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                UtilAudio.PlaySound("UIcolorapply");
                btn.BackColor = colorDialog.Color;
            }
        }
        #endregion
        #region Audio
        private void checkMuteApp_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkMuteApp.Checked) {
                UtilAudio.PlaySound("UIselect");
            }
        }

        private void checkAntiDuck_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkAntiDuck.Checked) {
                UtilAudio.PlaySound("duck");
            }
        }
        #endregion
        #region Keybinds
        #endregion
        #region Sequencer object colors
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BuildObjectTree();
        }

        private void BuildObjectTree()
        {
            bool filtersearch = txtSearch.Text is not "" and not "Search Objects (Ctrl+;)";

            treeObjects.Nodes.Clear();
            //make each category of objects its own node
            foreach (string category in TCLE.LeafObjects.Select(x => x.Value.Category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = category.ToUpper(),
                    SelectedImageKey = category.ToUpper()
                };
                //each object becomes its own node
                foreach (DefaultSequencerObject obj in TCLE.LeafObjects.Where(x => x.Value.Category == category).Select(x => x.Value)) {
                    TreeNode _param = new() {
                        Text = obj.ParamDisplayName,
                        ImageKey = obj.DefaultColor.ToArgb().ToString(),
                        SelectedImageKey = obj.DefaultColor.ToArgb().ToString(),
                        Tag = obj.Name + ";" + obj.ParamPath
                    };
                    if ((filtersearch && _param.Text.Contains(txtSearch.Text)) || !filtersearch)
                        _node.Nodes.Add(_param);
                }

                if ((filtersearch && _node.Nodes.Count != 0) || !filtersearch)
                    treeObjects.Nodes.Add(_node);
            }

            if (filtersearch)
                treeObjects.ExpandAll();
        }

        private void treeObjects_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 || treeObjects.SelectedNode.Nodes.Count > 0)
                return;

            UtilAudio.PlaySound("UIcoloropen");
            colorDialog.Color = Color.FromArgb(int.Parse(e.Node.ImageKey));
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                UtilAudio.PlaySound("UIcolorapply");
                //create color and store it in the bitmap dictionary
                Bitmap color = new(16, 16);
                using (Graphics g = Graphics.FromImage(color)) {
                    g.Clear(colorDialog.Color);
                }
                string colorname = colorDialog.Color.ToArgb().ToString();
                TCLE.ColorIcons.TryAdd(colorname, color);
                imageList1.Images.Add(colorname, color);
                //apply color to object
                DefaultSequencerObject param = TCLE.LeafObjects[(string)e.Node.Tag];
                param.DefaultColor = colorDialog.Color;
                e.Node.ImageKey = colorname;
                e.Node.SelectedImageKey = colorname;
            }
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIselect");
        }

        private void MenuPreferences_SizeChanged(object sender, EventArgs e)
        {
            toolStripLabel1.Margin = new((this.Width / 2) - (toolStripLabel1.Width / 2), 0, 0, 0);
            btnCustomizeApply.Margin = new((this.Width / 2) - (btnCustomizeApply.Width / 2), 0, 0, 0);
        }
    }
}
