using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thumper___Leaf_Editor
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
			btnPanelColor.BackColor = Properties.Settings.Default.custom_panelcolor;
			//invert text so it's readable
			btnBGColor.ForeColor = Color.FromArgb(255 - btnBGColor.BackColor.R, 255 - btnBGColor.BackColor.G, 255 - btnBGColor.BackColor.B);
			btnMenuColor.ForeColor = Color.FromArgb(255 - btnMenuColor.BackColor.R, 255 - btnMenuColor.BackColor.G, 255 - btnMenuColor.BackColor.B);
			btnPanelColor.ForeColor = Color.FromArgb(255 - btnPanelColor.BackColor.R, 255 - btnPanelColor.BackColor.G, 255 - btnPanelColor.BackColor.B);
		}

		private void btnBGColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				Color _c = colorDialog1.Color;
				btnBGColor.BackColor = _c;
				btnBGColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
			}
		}

		private void btnMenuColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				Color _c = colorDialog1.Color;
				btnMenuColor.BackColor = colorDialog1.Color;
				btnMenuColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
			}
		}

		private void btnPanelColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				Color _c = colorDialog1.Color;
				btnPanelColor.BackColor = colorDialog1.Color;
				btnPanelColor.ForeColor = Color.FromArgb(255 - _c.R, 255 - _c.G, 255 - _c.B);
			}
		}
	}
}
