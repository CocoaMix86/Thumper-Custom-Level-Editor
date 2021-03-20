using System;
using System.Windows.Forms;

namespace Thumper___Leaf_Editor
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
				this.Size = this.BackgroundImage.Size;
			}
		}
	}
}
