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
            //invert text so it's readable
            btnBGColor.ForeColor = Color.FromArgb(255 - btnBGColor.BackColor.R, 255 - btnBGColor.BackColor.G, 255 - btnBGColor.BackColor.B);
            btnMenuColor.ForeColor = Color.FromArgb(255 - btnMenuColor.BackColor.R, 255 - btnMenuColor.BackColor.G, 255 - btnMenuColor.BackColor.B);
            btnMasterColor.ForeColor = Color.FromArgb(255 - btnMasterColor.BackColor.R, 255 - btnMasterColor.BackColor.G, 255 - btnMasterColor.BackColor.B);
        }

        private void btnBGColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color _c = colorDialog1.Color;
                btnBGColor.BackColor = _c;
                btnBGColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnMenuColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color _c = colorDialog1.Color;
                btnMenuColor.BackColor = colorDialog1.Color;
                btnMenuColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnPanelColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color _c = colorDialog1.Color;
                btnMasterColor.BackColor = colorDialog1.Color;
                btnMasterColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnGateColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                Color _c = colorDialog1.Color;
                btnGateColor.BackColor = colorDialog1.Color;
                btnGateColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnLvlColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                Color _c = colorDialog1.Color;
                btnLvlColor.BackColor = colorDialog1.Color;
                btnLvlColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnLeafColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                Color _c = colorDialog1.Color;
                btnLeafColor.BackColor = colorDialog1.Color;
                btnLeafColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }

        private void btnSampleColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                Color _c = colorDialog1.Color;
                btnSampleColor.BackColor = colorDialog1.Color;
                btnSampleColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
            }
        }
    }
}
