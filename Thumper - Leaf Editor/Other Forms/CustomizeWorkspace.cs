using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        string AppLoc = Path.GetDirectoryName(Application.ExecutablePath);
        public List<Object_Params> _objects = new List<Object_Params>();
        private List<Tuple<string, string>> objectcolors = new List<Tuple<string, string>>();

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
                var items = line.Split(';');
                objectcolors.Add(new Tuple<string, string>(items[0], items[1]));
            }
            //
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
        }

        private void btnSetColor(object sender, EventArgs e)
        {
            FormLeafEditor.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                FormLeafEditor.PlaySound("UIcolorapply");
                Color _c = colorDialog1.Color;
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
    }
}
