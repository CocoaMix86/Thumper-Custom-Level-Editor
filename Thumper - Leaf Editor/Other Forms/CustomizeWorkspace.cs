using Cyotek.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        #region Variables
        private ColorPickerDialog colorDialog = new() { BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.White };
        private Dictionary<string, Keys> DictKeybind = new();
        private Dictionary<string, Keys> DictRandomization = new();
        #endregion
        #region Form Construction and initialization
        public CustomizeWorkspace()
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
            StringFormat StrFormat = new StringFormat() {
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
            TCLE.settingsUITheme.SaveSettings();
            TCLE.ColorFormElements(TCLE.Instance);

            //write sequencer colors to txt file
            File.WriteAllLines($@"{TCLE.AppLocation}\settings\objects_defaultcolors_v3.txt", TCLE.LeafObjects.Select(x => $"{x.Value.param_displayname};{x.Value.defaultcolor.ToArgb()}"));
            TCLE.ImportDefaultColors();
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.txtSearch.Text);
            }

            //save mute to settings
            Properties.Settings.Default.muteapplication = checkMuteApp.Checked;

            //write keybinds to txt file
            ///File.WriteAllLines($@"{TCLE.AppLocation}\settings\keybinds.txt", keybindfromfile.Select(x => $"{x.Key};{x.Value}"));
            Properties.Settings.Default.UserKeybinds = string.Join('\n', DictKeybind.Select(x => $"{x.Key};{x.Value}"));
            TCLE.Instance.SetKeyBinds();

            //save properties
            Properties.Settings.Default.Save();

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
            foreach (string category in TCLE.LeafObjects.Select(x => x.Value.category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = category.ToUpper(),
                    SelectedImageKey = category.ToUpper()
                };
                //each object becomes its own node
                foreach (Object_Params obj in TCLE.LeafObjects.Where(x => x.Value.category == category).Select(x => x.Value)) {
                    TreeNode _param = new() {
                        Text = obj.param_displayname,
                        ImageKey = obj.defaultcolor.ToArgb().ToString(),
                        SelectedImageKey = obj.defaultcolor.ToArgb().ToString(),
                        Tag = obj.obj_name + ";" + obj.param_path
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
                Object_Params param = TCLE.LeafObjects[(string)e.Node.Tag];
                param.defaultcolor = colorDialog.Color;
                e.Node.ImageKey = colorname;
                e.Node.SelectedImageKey = colorname;
            }
        }
        #endregion
    }
}
