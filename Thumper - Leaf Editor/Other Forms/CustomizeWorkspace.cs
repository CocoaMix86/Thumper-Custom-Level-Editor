using Cyotek.Windows.Forms;
using System.Collections.Generic;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        #region Variables
        private ColorPickerDialog colorDialog = new() { BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.White };
        private List<Keys> mandatorykeys = new() { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.Shift | Keys.Control | Keys.Alt, Keys.Alt, Keys.Control, Keys.Control | Keys.Alt, Keys.Control | Keys.Shift, Keys.Alt | Keys.Shift };
        private Dictionary<string, Keys> defaultkeybinds = Properties.Resources.defaultkeybinds.Split('\n').ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
        private Dictionary<string, Keys> keybindfromfile = new();
        private Keys lastpress;
        private Label currentlabel;
        private bool ignorekeys = true;
        private string keybindname;
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
            foreach (var bmp in TCLE.ColorIcons)
                imageList1.Images.Add(bmp.Key, bmp.Value);
            BuildObjectTree();
            //set mute
            checkMuteApp.Checked = Properties.Settings.Default.muteapplication;
            //locate keybinds file. If not exist, create it from internal resource
            if (!File.Exists($@"{TCLE.AppLocation}\templates\keybinds.txt"))
                File.WriteAllText($@"{TCLE.AppLocation}\templates\keybinds.txt", Properties.Resources.defaultkeybinds);
            //read keybinds to a dictionary for easier lookup
            keybindfromfile = File.ReadAllLines($@"{TCLE.AppLocation}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
            keybindfromfile = keybindfromfile.Concat(defaultkeybinds.Where(x => !keybindfromfile.ContainsKey(x.Key))).ToDictionary(x => x.Key, x => x.Value);
            LoadKeyBindInfo(keybindfromfile);
            propertyGridKeyBinds.SelectedObject = new DictionaryPropertyGridAdapter(keybindfromfile);
        }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Set Border header  
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(55, 55, 55)), e.Bounds);
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.HighlightText, paddedBounds);

            //set  Tabcontrol border  
            Graphics g = e.Graphics;
            Pen p = new(Color.FromArgb(55, 55, 55), 10);
            g.DrawRectangle(p, tabUIColors.Bounds);
        }
        #endregion
        #region Form Closing
        private void btnCustomizeApply_Click(object sender, EventArgs e)
        {
            //save colors to settings
            TCLE.settingsUITheme.SaveSettings();
            //write sequencer colors to txt file
            File.WriteAllLines($@"{TCLE.AppLocation}\templates\objects_defaultcolors2.2.txt", TCLE.LeafObjects.Select(x => $"{x.param_displayname};{x.defaultcolor.ToArgb()}"));
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            //save mute to settings
            Properties.Settings.Default.muteapplication = checkMuteApp.Checked;
            //write keybinds to txt file
            File.WriteAllLines($@"{TCLE.AppLocation}\templates\keybinds.txt", keybindfromfile.Select(x => $"{x.Key};{x.Value}"));
            //save properties
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
        #region UI colors
        private void btnSetColor(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                TCLE.PlaySound("UIcolorapply");
                btn.BackColor = colorDialog.Color;
            }
        }

        private void btnObjectColor_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                TCLE.PlaySound("UIcolorapply");
                Color _c = colorDialog.Color;
                btn.BackColor = colorDialog.Color;
            }
        }
        #endregion
        #region Audio
        private void checkMuteApp_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkMuteApp.Checked) {
                TCLE.PlaySound("UIselect");
            }
        }
        #endregion
        #region Keybinds
        private void LoadKeyBindInfo(Dictionary<string, Keys> loadthesekeys)
        {
            //loop through labels called "keybind" on form. Each has a TAG that is used to lookup its keybind from the dictionary
            foreach (Label _lbl in panel1.Controls.OfType<Label>().Where(x => x.Name.Contains("keybind"))) {
                //the "14" is a leftpad empty space
                List<string> mod = loadthesekeys[(string)_lbl.Tag].ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList();
                mod.Reverse();
                if (mod.Remove("Alt"))
                    mod.Insert(0, "Alt");
                if (mod.Remove("Control"))
                    mod.Insert(0, "Control");
                _lbl.Text = $"{_lbl.Text.Split('.')[0],17}" + $".....{String.Join(" + ", mod)}";
            }
        }
        private void keybindLabel_Click(object sender, EventArgs e)
        {
            ///all keybind labels call this function
            //storw which label was clicked
            currentlabel = sender as Label;
            currentlabel.Focus();
            keybindname = (string)currentlabel.Tag;
            string[] lbltxt = currentlabel.Text.Split('.');
            //set to false so the KeyDown event can start picking up our key presses
            ignorekeys = false;
            //make the keybind setting panel show up
            panelSetKeybind.Visible = true;
            labelKeybindName.Text = $"Set Keybind - {lbltxt[0].Trim()}";
            labelKeys.Text = lbltxt.Last();
        }
        private void CustomizeWorkspace_KeyDown(object sender, KeyEventArgs e)
        {
            if (ignorekeys)
                return;
            //check if keydown is the same as last pressed. Don't process if it is
            if (e.KeyData != lastpress) {
                lblInvalid.Visible = false;
                //store last press for when user accepts changes
                bool cantusethiskey = false;
                lastpress = e.KeyData;
                if (keybindfromfile.ContainsValue(lastpress) || (!mandatorykeys.Contains(e.KeyCode) && !mandatorykeys.Contains(e.Modifiers)) || (e.KeyCode is Keys.ControlKey or Keys.ShiftKey or Keys.Menu)) {
                    cantusethiskey = true;
                    lblInvalid.Visible = true;
                }
                //check if the new keypress exists as a keybind
                //if it is, disable controls so it can't be set
                labelKeys.ForeColor = cantusethiskey ? Color.Red : Color.White;
                btnSetKeybind.Enabled = !cantusethiskey;
                btnSetKeybind.BackColor = cantusethiskey ? Color.Gray : Color.Green;
                List<string> mod = e.Modifiers.ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList();

                if (mod.Remove("Alt"))
                    mod.Insert(0, "Alt");
                if (mod.Last() == "Control" && mod.Count > 1) {
                    mod.Remove("Control");
                    mod.Insert(0, "Control");
                }
                labelKeys.Text = $"{string.Join(" + ", mod)} + {e.KeyCode}";
            }
        }
        private void btnSetKeybind_Click(object sender, EventArgs e)
        {
            //when user accepts keybind change, store lastpress into the keybind dictionary
            //using the saved "keybindname" stored from the Click function
            keybindfromfile[keybindname] = lastpress;
            //update the keybind label
            //the "14" is a leftpad empty space
            List<string> mod = keybindfromfile[keybindname].ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList();
            mod.Reverse();
            currentlabel.Text = $"{currentlabel.Text.Split('.')[0],17}" + $".....{String.Join(" + ", mod)}";
            panelSetKeybind.Visible = false;
            ignorekeys = true;
        }
        private void txtKeybindSearch_Enter(object sender, EventArgs e)
        {
            txtKeybindSearch.Text = txtKeybindSearch.Text.Replace("search...", "");
        }
        private void txtKeybindSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (Label _lbl in panel1.Controls.OfType<Label>())
                _lbl.Visible = false;
            //find all labels with text that matches the search. Since keybind name AND Keys are in the same string,
            //the search can look up both at the same time
            foreach (Label _lbl in panel1.Controls.OfType<Label>().Where(x => x.Text.ToLower().Contains(txtKeybindSearch.Text.ToLower())))
                _lbl.Visible = true;
        }
        private void btnKeybindReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset all keybinds to default?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            LoadKeyBindInfo(defaultkeybinds);
            keybindfromfile = defaultkeybinds;
        }
        private void btnSingleReset_Click(object sender, EventArgs e)
        {
            CustomizeWorkspace_KeyDown(null, new KeyEventArgs(defaultkeybinds[keybindname]));
        }
        private void btnCloseKeybind_Click(object sender, EventArgs e)
        {
            ignorekeys = true;
            panelSetKeybind.Visible = false;
            btnSetKeybind.Enabled = false;
            btnSetKeybind.BackColor = Color.Gray;
            lastpress = Keys.None;
        }
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
            foreach (string category in TCLE.LeafObjects.Select(x => x.category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = "category",
                    SelectedImageKey = "category"
                };
                //each object becomes its own node
                foreach (Object_Params obj in TCLE.LeafObjects.Where(x => x.category == category)) {
                    TreeNode _param = new() {
                        Text = obj.param_displayname,
                        ImageKey = obj.defaultcolor.ToArgb().ToString(),
                        SelectedImageKey = obj.defaultcolor.ToArgb().ToString()
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

            TCLE.PlaySound("UIcoloropen");
            colorDialog.Color = Color.FromArgb(int.Parse(e.Node.ImageKey));
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                TCLE.PlaySound("UIcolorapply");
                //create color and store it in the bitmap dictionary
                Bitmap color = new(16, 16);
                using (Graphics g = Graphics.FromImage(color)) {
                    g.Clear(colorDialog.Color);
                }
                string colorname = colorDialog.Color.ToArgb().ToString();
                TCLE.ColorIcons.TryAdd(colorname, color);
                imageList1.Images.Add(colorname, color);
                //apply color to object
                TCLE.LeafObjects.First(x => x.param_displayname == e.Node.Text).defaultcolor = colorDialog.Color;
                e.Node.ImageKey = colorname;
                e.Node.SelectedImageKey = colorname;
            }
        }
        #endregion
    }
}
