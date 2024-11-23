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
            this.SuspendLayout();
            // 
            // timerBeeble
            // 
            this.timerBeeble.Interval = 300;
            this.timerBeeble.Tick += this.timerBeeble_Tick;
            // 
            // Beeble
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BackgroundImage = Properties.Resources.beeble;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(132, 100);
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(56, 55);
            this.Name = "Beeble";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += this.Beeble_Load;
            this.MouseDown += this.Beeble_MouseDown;
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timerBeeble;
    }
}