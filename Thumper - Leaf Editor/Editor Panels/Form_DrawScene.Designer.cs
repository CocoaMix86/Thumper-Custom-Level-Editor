namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_DrawScene
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DrawScene));
            this.toolStripTitle = new ToolStripEx();
            this.btnDoTheThing = new ToolStripMenuItem();
            this.panel1 = new Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripTitle
            // 
            this.toolStripTitle.BackColor = Color.FromArgb(80, 0, 0);
            this.toolStripTitle.GripMargin = new Padding(0);
            this.toolStripTitle.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripTitle.Items.AddRange(new ToolStripItem[] { this.btnDoTheThing });
            this.toolStripTitle.Location = new Point(0, 0);
            this.toolStripTitle.MaximumSize = new Size(0, 31);
            this.toolStripTitle.MinimumSize = new Size(0, 31);
            this.toolStripTitle.Name = "toolStripTitle";
            this.toolStripTitle.Padding = new Padding(0);
            this.toolStripTitle.RenderMode = ToolStripRenderMode.System;
            this.toolStripTitle.Size = new Size(647, 31);
            this.toolStripTitle.TabIndex = 151;
            this.toolStripTitle.Text = "toolStripTitle";
            // 
            // btnDoTheThing
            // 
            this.btnDoTheThing.Font = new Font("Gadugi", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnDoTheThing.ForeColor = Color.White;
            this.btnDoTheThing.Margin = new Padding(0, 5, 0, 5);
            this.btnDoTheThing.Name = "btnDoTheThing";
            this.btnDoTheThing.Padding = new Padding(3, 0, 3, 0);
            this.btnDoTheThing.Size = new Size(173, 21);
            this.btnDoTheThing.Text = "Reload";
            this.btnDoTheThing.Click += this.btnDoTheThing_Click;
            // 
            // panel1
            // 
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(647, 368);
            this.panel1.TabIndex = 152;
            // 
            // timer1
            // 
            this.timer1.Interval = 16;
            this.timer1.Tick += this.timer1_Tick;
            this.timer1.Enabled = true;
            // 
            // Form_DrawScene
            // 
            this.ClientSize = new Size(647, 399);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStripTitle);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "Form_DrawScene";
            this.Text = "Scene Viewer";
            this.SizeChanged += this.Form_DrawScene_SizeChanged;
            this.toolStripTitle.ResumeLayout(false);
            this.toolStripTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ToolStripEx toolStripTitle;
        private ToolStripMenuItem btnDoTheThing;
        private Panel panel1;
        private System.Windows.Forms.Timer timer1;
    }
}