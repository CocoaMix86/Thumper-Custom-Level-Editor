using System;
using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        public CustomizeWorkspace()
        {
            InitializeComponent();
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
            //invert text so it's readable
            foreach (Button btn in this.Controls.Find("btnCustom", true)) {
                if (btn.Tag.ToString() == "customcolorbutton") {
                    btn.ForeColor = Color.FromArgb(255 - btn.BackColor.R, 255 - btn.BackColor.G, 255 - btn.BackColor.B);
                }
            }
            //
            toolstripCustomize.Renderer = new ToolStripOverride();
        }

        private void btnSetColor(object sender, EventArgs e)
        {
            FormLeafEditor.PlaySound("UIcoloropen");
            Button btn = (Button)sender;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                Color _c = colorDialog1.Color;
                btn.BackColor = colorDialog1.Color;
                btn.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnCustomizeApply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
