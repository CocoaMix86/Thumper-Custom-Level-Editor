namespace Thumper_Custom_Level_Editor
{
    partial class ImageViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(134, 104);
            this.DoubleBuffered = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(1, 1);
            this.Name = "ImageViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.MouseDown += this.Beeble_MouseDown;
            this.ResumeLayout(false);
        }

        #endregion
    }
}