using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        string AppLoc = Path.GetDirectoryName(Application.ExecutablePath);
        public List<Object_Params> _objects = new();
        private List<Tuple<string, string>> objectcolors = new();
        private Dictionary<string, Keys> defaultkeybinds = Properties.Resources.defaultkeybinds.Split('\n').ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));

        public CustomizeWorkspace(List<Object_Params> thelist)
        {
            InitializeComponent();
            _objects = thelist;
            this.BackColor = Properties.Settings.Default.custom_bgcolor;
            //set button back colors to the set settings
            btnBGColor.BackColor = Properties.Settings.Default.custom_bgcolor;
            btnMenuColor.BackColor = Properties.Settings.Default.custom_menucolor;
            btnMasterColor.BackColor = Properties.Settings.Default.custom_mastercolor;
            btnGateColor.BackColor = Properties.Settings.Default.custom_gatecolor;
            btnLvlColor.BackColor = Properties.Settings.Default.custom_lvlcolor;
            btnLeafColor.BackColor = Properties.Settings.Default.custom_leafcolor;
            btnSampleColor.BackColor = Properties.Settings.Default.custom_samplecolor;
            btnActiveColor.BackColor = Properties.Settings.Default.custom_activecolor;
            checkMuteApp.Checked = Properties.Settings.Default.muteapplication;
            //
            toolstripCustomize.Renderer = new ToolStripOverride();
            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //
            string[] import = File.Exists($@"{AppLoc}\templates\objects_defaultcolors.txt") ? File.ReadAllLines($@"{AppLoc}\templates\objects_defaultcolors.txt") : null;
            foreach (string line in import) {
                string[] items = line.Split(';');
                objectcolors.Add(new Tuple<string, string>(items[0], items[1]));
            }
            //
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();

            LoadKeyBindInfo();
        }

        private void btnSetColor(object sender, EventArgs e)
        {
            FormLeafEditor.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                FormLeafEditor.PlaySound("UIcolorapply");
                btn.BackColor = colorDialog1.Color;
            }
        }

        private void btnCustomizeApply_Click(object sender, EventArgs e)
        {
            string[] export = objectcolors.Select(x => $@"{x.Item1};{x.Item2}").ToArray();
            File.WriteAllLines($@"{AppLoc}\templates\objects_defaultcolors.txt", export);

            Properties.Settings.Default.custom_bgcolor = btnBGColor.BackColor;
            Properties.Settings.Default.custom_menucolor = btnMenuColor.BackColor;
            Properties.Settings.Default.custom_mastercolor = btnMasterColor.BackColor;
            Properties.Settings.Default.custom_gatecolor = btnGateColor.BackColor;
            Properties.Settings.Default.custom_lvlcolor = btnLvlColor.BackColor;
            Properties.Settings.Default.custom_leafcolor = btnLeafColor.BackColor;
            Properties.Settings.Default.custom_samplecolor = btnSampleColor.BackColor;
            Properties.Settings.Default.custom_activecolor = btnActiveColor.BackColor;
            Properties.Settings.Default.muteapplication = checkMuteApp.Checked;

            File.WriteAllText($@"{AppLoc}\templates\UIcolorprefs.txt", $"{btnBGColor.BackColor.ToArgb()}\n{btnMenuColor.BackColor.ToArgb()}\n{btnMasterColor.BackColor.ToArgb()}\n{btnGateColor.BackColor.ToArgb()}\n{btnLvlColor.BackColor.ToArgb()}\n{btnLeafColor.BackColor.ToArgb()}\n{btnSampleColor.BackColor.ToArgb()}\n{btnActiveColor.BackColor.ToArgb()}");

            File.WriteAllLines($@"{AppLoc}\templates\keybinds.txt", keybinds.Select(x => $"{x.Key};{x.Value}"));

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dropObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
        }

        private void dropParamPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnObjectColor.BackColor = Color.FromArgb(int.Parse(objectcolors.FirstOrDefault(x => x.Item1 == dropParamPath.Text)?.Item2 ?? "-8355585"));
        }

        private void btnObjectColor_Click(object sender, EventArgs e)
        {
            FormLeafEditor.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                FormLeafEditor.PlaySound("UIcolorapply");
                Color _c = colorDialog1.Color;
                btn.BackColor = colorDialog1.Color;

                if (objectcolors.FirstOrDefault(x => x.Item1 == dropParamPath.Text) == null) {
                    objectcolors.Add(new Tuple<string, string>(dropParamPath.Text, $"{_c.ToArgb()}"));
                }
                else {
                    int _in = objectcolors.IndexOf(objectcolors.Where(x => x.Item1 == dropParamPath.Text).First());
                    objectcolors[_in] = new Tuple<string, string>(dropParamPath.Text, $"{_c.ToArgb()}");
                }
            }
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
            g.DrawRectangle(p, tabPage1.Bounds);
        }

        /// 
        /// This is all for handling keybinds
        /// 
        Dictionary<string, Keys> keybinds = new();
        Keys lastpress;
        Label currentlabel;
        bool ignorekeys = true;
        string keybindname;
        private void LoadKeyBindInfo()
        {
            //locate keybinds file. If not exist, create it from internal resource
            if (!File.Exists($@"{AppLoc}\templates\keybinds.txt")) 
                File.WriteAllText($@"{AppLoc}\templates\keybinds.txt", Properties.Resources.defaultkeybinds);
            //read keybinds to a dictionary for easier lookup
            keybinds = File.ReadAllLines($@"{AppLoc}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
            //loop through labels called "keybind" on form. Each has a TAG that is used to lookup its keybind from the dictionary
            foreach (Label _lbl in panel1.Controls.OfType<Label>().Where(x => x.Name.Contains("keybind"))) {
                //the "14" is a leftpad empty space
                _lbl.Text = $"{_lbl.Text.Split('.')[0],14}" + $".....{keybinds[(string)_lbl.Tag].ToString().Replace(",", " +")}";
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
            labelKeys.Text = lbltxt.Last().Replace(",", " +");
        }
        private void CustomizeWorkspace_KeyDown(object sender, KeyEventArgs e)
        {
            if (ignorekeys)
                return;
            //check if keydown is the same as last pressed. Don't process if it is
            if (e.KeyData != lastpress) {
                //store last press for when user accepts changes
                lastpress = e.KeyData;
                //check if the new keypress exists as a keybind
                //if it is, disable controls so it can't be set
                labelKeys.ForeColor = keybinds.ContainsValue(lastpress) ? Color.Red : Color.White;
                btnSetKeybind.Enabled = !keybinds.ContainsValue(lastpress);
                btnSetKeybind.BackColor = keybinds.ContainsValue(lastpress) ? Color.Gray : Color.Green;
                labelKeys.Text = $"{e.KeyCode} + {e.Modifiers.ToString().Replace(",", " +")}";
            }
        }
        private void btnSetKeybind_Click(object sender, EventArgs e)
        {
            //when user accepts keybind change, store lastpress into the keybind dictionary
            //using the saved "keybindname" stored from the Click function
            keybinds[keybindname] = lastpress;
            //update the keybind label
            //the "14" is a leftpad empty space
            currentlabel.Text = $"{currentlabel.Text.Split('.')[0],14}" + $".....{keybinds[keybindname].ToString().Replace(",", " +")}";
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

            File.WriteAllText($@"{AppLoc}\templates\keybinds.txt", Properties.Resources.defaultkeybinds);
            LoadKeyBindInfo();
        }
        private void btnSingleReset_Click(object sender, EventArgs e)
        {
            keybinds[keybindname] = defaultkeybinds[keybindname];
            lastpress = keybinds[keybindname];
            //the "14" is a leftpad empty space
            currentlabel.Text = $"{currentlabel.Text.Split('.')[0],14}" + $".....{keybinds[keybindname].ToString().Replace(",", " +")}";
            keybindLabel_Click(currentlabel, null);
        }
        private void btnCloseKeybind_Click(object sender, EventArgs e)
        {
            ignorekeys = true;
            panelSetKeybind.Visible = false;
            btnSetKeybind.Enabled = false;
            btnSetKeybind.BackColor = Color.Gray;
        }

        /// 
        /// This is all for handling keybinds
        /// 



    }
}
