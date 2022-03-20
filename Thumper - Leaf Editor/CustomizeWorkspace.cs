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

			btnBGColor.BackColor = Properties.Settings.Default.custom_bgcolor;
			btnMenuColor.BackColor = Properties.Settings.Default.custom_menucolor;
			btnPanelColor.BackColor = Properties.Settings.Default.custom_panelcolor;
		}

		private void btnBGColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				btnBGColor.BackColor = colorDialog1.Color;
			}
		}

		private void btnMenuColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				btnMenuColor.BackColor = colorDialog1.Color;
			}
		}

		private void btnPanelColor_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				btnPanelColor.BackColor = colorDialog1.Color;
			}
		}
	}
}
