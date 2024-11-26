namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_RawText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RawText));
            this.SuspendLayout();
            // 
            // Form_RawText
            // 
            this.BackColor = Color.FromArgb(31, 31, 31);
            this.ClientSize = new Size(539, 497);
            this.ForeColor = Color.White;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "Form_RawText";
            this.Text = "Raw Editor";
            this.ResumeLayout(false);
        }

        #endregion
    }
}