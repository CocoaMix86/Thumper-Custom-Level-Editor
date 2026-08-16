namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class ProjectNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectNotes));
            this.txtNotes = new RichTextBox();
            this.SuspendLayout();
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = Color.FromArgb(30, 30, 30);
            this.txtNotes.Dock = DockStyle.Fill;
            this.txtNotes.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtNotes.ForeColor = Color.White;
            this.txtNotes.Location = new Point(0, 0);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(476, 423);
            this.txtNotes.TabIndex = 0;
            this.txtNotes.Text = "Write your project notes here";
            // 
            // ProjectNotes
            // 
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ClientSize = new Size(476, 423);
            this.Controls.Add(this.txtNotes);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Name = "ProjectNotes";
            this.Text = "NOTES";
            this.FormClosing += this.Form_WorkSpace_FormClosing;
            this.FormClosed += this.Form_WorkSpace_FormClosed;
            this.ResumeLayout(false);
        }

        #endregion

        public RichTextBox txtNotes;
    }
}