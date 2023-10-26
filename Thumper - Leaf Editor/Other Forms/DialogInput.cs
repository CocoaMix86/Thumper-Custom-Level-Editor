using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Thumper_Custom_Level_Editor
{
	public partial class DialogInput : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new CommonOpenFileDialog() { IsFolderPicker = true, Multiselect = false };

        public DialogInput()
		{
			InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnCustomFolder_Click(object sender, EventArgs e)
        {
            cfd_lvl.InitialDirectory = Application.StartupPath;
            cfd_lvl.Title = "Choose where to save the custom level";
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok) {
                //if (Directory.EnumerateFileSystemEntries(cfd_lvl.FileName).Any() && MessageBox.Show("The selected path is not empty. Do you still wish to save level data to this location?", "Confirm choice", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    //return;
                txtCustomPath.Text = cfd_lvl.FileName;
                btnCustomSave.Enabled = true;
            }
        }

		private void lblCustomDiffHelp_Click(object sender, EventArgs e)
		{
			new ImageMessageBox("difficultyhelp").Show();
		}

        private void combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null) {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0) {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void btnCustomSave_Click(object sender, EventArgs e)
        {

        }

        private void txtCustomDiff_TextChanged(object sender, EventArgs e)
        {
            Image diff = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);
            pictureDifficulty.Image = diff;
        }
    }
}
