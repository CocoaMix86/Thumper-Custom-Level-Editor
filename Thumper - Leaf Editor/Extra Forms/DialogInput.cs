using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Thumper___Leaf_Editor
{
	public partial class DialogInput : Form
	{
		public DialogInput()
		{
			InitializeComponent();
		}

		private void btnCustomFolder_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog()) {
				fbd.SelectedPath = Application.StartupPath;
				fbd.Description = "Choose where to save the custom level";
				if (fbd.ShowDialog() == DialogResult.OK) {
					if (Directory.EnumerateFileSystemEntries(fbd.SelectedPath).Any() && MessageBox.Show("The selected path is not empty. Do you still wish to save level data to this location?", "Confirm choice", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;
					txtCustomPath.Text = fbd.SelectedPath;
					txtCustomName.Text = Path.GetFileName(fbd.SelectedPath);
				}
			}
		}

		private void lblCustomDiffHelp_Click(object sender, EventArgs e)
		{
			new ImageMessageBox("difficultyhelp").Show();
		}
	}
}
