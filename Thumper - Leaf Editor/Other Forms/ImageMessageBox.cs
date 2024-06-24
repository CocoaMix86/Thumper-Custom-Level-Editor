using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class ImageMessageBox : Form
	{
		private System.Drawing.Size _size;

		public ImageMessageBox()
		{
			InitializeComponent();
		}

		public ImageMessageBox(string path)
		{
			if (path == "railcolorhelp") {
				this.BackgroundImage = Properties.Resources.railcolorhelp;
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Rail Color Explanation";
				_size = this.BackgroundImage.Size;
			}
			if (path == "difficultyhelp") {
				this.BackgroundImage = Properties.Resources.difficultyhelp;
				this.BackgroundImageLayout = ImageLayout.Stretch;
				this.Text = "Difficulty Explanation";
				_size = new System.Drawing.Size(700, 690);
			}
			if (path == "bosssectionhelp") {
				this.BackgroundImage = Properties.Resources.bosssectionhelp;
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Boss Section Explanation";
				_size = this.BackgroundImage.Size;
			}
			this.Size = _size;
			this.Height += 40;
		}
	}
}
