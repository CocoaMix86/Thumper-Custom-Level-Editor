namespace Thumper_Custom_Level_Editor
{
    partial class Beeble
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
            this.components = new System.ComponentModel.Container();
            this.timerBeeble = new System.Windows.Forms.Timer(this.components);
            this.pictureBeeble = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.pictureBeeble).BeginInit();
            this.SuspendLayout();
            // 
            // timerBeeble
            // 
            this.timerBeeble.Interval = 300;
            this.timerBeeble.Tick += this.timerBeeble_Tick;
            // 
            // pictureBeeble
            // 
            this.pictureBeeble.Dock = DockStyle.Fill;
            this.pictureBeeble.Image = Properties.Resources.beeble;
            this.pictureBeeble.Location = new Point(0, 0);
            this.pictureBeeble.Name = "pictureBeeble";
            this.pictureBeeble.Size = new Size(134, 104);
            this.pictureBeeble.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBeeble.TabIndex = 0;
            this.pictureBeeble.TabStop = false;
            this.pictureBeeble.MouseDown += this.Beeble_MouseDown;
            // 
            // Beeble
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(134, 104);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBeeble);
            this.DoubleBuffered = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(1, 1);
            this.Name = "Beeble";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.MouseDown += this.Beeble_MouseDown;
            ((System.ComponentModel.ISupportInitialize)this.pictureBeeble).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timerBeeble;
        private PictureBox pictureBeeble;
    }
}