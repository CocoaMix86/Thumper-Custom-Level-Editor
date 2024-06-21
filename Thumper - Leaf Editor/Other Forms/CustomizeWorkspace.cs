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
        KeyEventArgs lastpress;
        Label currentlabel;
        bool ignorekeys = true;
        string keybindname;
        private void LoadKeyBindInfo()
        {
            if (!File.Exists($@"{AppLoc}\templates\keybinds.txt")) 
                File.WriteAllText($@"{AppLoc}\templates\keybinds.txt", Properties.Resources.defaultkeybinds);
            keybinds = File.ReadAllLines($@"{AppLoc}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));

            keybindLeafNew.Text = $"Leaf New - {keybinds["leafnew"].ToString().Replace(",", " +")}";
            keybindLeafOpen.Text = $"Leaf Open - {keybinds["leafopen"].ToString().Replace(",", " +")}";
            keybindLeafSave.Text = $"Leaf Save - {keybinds["leafsave"].ToString().Replace(",", " +")}";
            keybindLeafSaveAs.Text = $"Leaf Save As - {keybinds["leafsaveas"].ToString().Replace(",", " +")}";
        }
        private void keybindLabel_Click(object sender, EventArgs e)
        {
            currentlabel = sender as Label;
            keybindname = (string)currentlabel.Tag;
            string[] lbltxt = currentlabel.Text.Split('-');
            ignorekeys = false;
            panelSetKeybind.Visible = true;
            labelKeybindName.Text = $"Set Keybind - {lbltxt[0]}";
            labelKeys.Text = lbltxt[1].Replace(",", " +");
        }
        private void CustomizeWorkspace_KeyDown(object sender, KeyEventArgs e)
        {
            if (ignorekeys)
                return;
            if (e != lastpress) {
                lastpress = e;
                labelKeys.Text = $"{e.KeyCode} + {e.Modifiers.ToString().Replace(",", " +")}";
            }
        }
        private void btnSetKeybind_Click(object sender, EventArgs e)
        {
            keybinds[keybindname] = lastpress.KeyData;
            currentlabel.Text = $"{currentlabel.Text.Split('-')[0]} - {keybinds[keybindname].ToString().Replace(",", " +")}";
            panelSetKeybind.Visible = false;
        }
        /// 
        /// This is all for handling keybinds
        /// 



    }
}
