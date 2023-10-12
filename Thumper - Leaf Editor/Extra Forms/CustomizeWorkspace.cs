using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class CustomizeWorkspace : Form
    {
        public List<Object_Params> _objects = new List<Object_Params>();

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
            //
            toolstripCustomize.Renderer = new ToolStripOverride();
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dropObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
        }
    }
}
