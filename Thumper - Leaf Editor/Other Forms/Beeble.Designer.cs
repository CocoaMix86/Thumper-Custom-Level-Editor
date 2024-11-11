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
            this.timerBeeble.Tick += new System.EventHandler(this.timerBeeble_Tick);
            // 
            // Beeble
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Thumper_Custom_Level_Editor.Properties.Resources.beeble;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(113, 94);
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "Beeble";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Beeble_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Beeble_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerBeeble;
    }
}