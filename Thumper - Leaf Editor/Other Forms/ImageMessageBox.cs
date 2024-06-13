using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class ImageMessageBox : Form
	{
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
			}
			if (path == "difficultyhelp") {
				this.BackgroundImage = Properties.Resources.difficultyhelp;
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Difficulty Explanation";
			}
			if (path == "bosssectionhelp") {
				this.BackgroundImage = Properties.Resources.bosssectionhelp;
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Boss Section Explanation";
			}
			this.Size = this.BackgroundImage.Size;
			this.Height += 40;
		}
	}
}
