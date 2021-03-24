﻿using System;
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
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Rail Color Explanation";
			}
			if (path == "difficultyhelp") {
				this.BackgroundImage = Properties.Resources.difficultyhelp;
				this.BackgroundImageLayout = ImageLayout.Center;
				this.Text = "Difficulty Explanation";
			}
			this.Size = this.BackgroundImage.Size;
			this.Height += 40;
		}
	}
}
