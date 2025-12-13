namespace Thumper_Custom_Level_Editor.Other_Forms
{
    partial class ExportProject
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
            this.btnCustomFolder = new Button();
            this.label1 = new Label();
            this.txtCustomPath = new TextBox();
            this.label9 = new Label();
            this.label2 = new Label();
            this.lblStuff = new Label();
            this.pictureThumb = new PictureBox();
            this.label3 = new Label();
            this.panelThumb = new Panel();
            this.btnExport = new Button();
            ((System.ComponentModel.ISupportInitialize)this.pictureThumb).BeginInit();
            this.panelThumb.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCustomFolder
            // 
            this.btnCustomFolder.BackColor = Color.Gray;
            this.btnCustomFolder.FlatStyle = FlatStyle.Popup;
            this.btnCustomFolder.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnCustomFolder.ForeColor = Color.Black;
            this.btnCustomFolder.Image = Properties.Resources.icon_folder;
            this.btnCustomFolder.Location = new Point(452, 28);
            this.btnCustomFolder.Margin = new Padding(4, 3, 4, 3);
            this.btnCustomFolder.Name = "btnCustomFolder";
            this.btnCustomFolder.Size = new Size(24, 24);
            this.btnCustomFolder.TabIndex = 2;
            this.btnCustomFolder.TextAlign = ContentAlignment.TopCenter;
            this.btnCustomFolder.UseVisualStyleBackColor = false;
            this.btnCustomFolder.Click += this.btnCustomFolder_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(13, 11);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(197, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Where to export your project ZIP:";
            // 
            // txtCustomPath
            // 
            this.txtCustomPath.BackColor = Color.FromArgb(40, 40, 40);
            this.txtCustomPath.Enabled = false;
            this.txtCustomPath.Font = new Font("Arial", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.txtCustomPath.ForeColor = Color.White;
            this.txtCustomPath.Location = new Point(13, 28);
            this.txtCustomPath.Margin = new Padding(4, 3, 4, 3);
            this.txtCustomPath.Name = "txtCustomPath";
            this.txtCustomPath.Size = new Size(438, 25);
            this.txtCustomPath.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = Color.Transparent;
            this.label9.BorderStyle = BorderStyle.FixedSingle;
            this.label9.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label9.ForeColor = Color.White;
            this.label9.Location = new Point(-13, 59);
            this.label9.Margin = new Padding(4, 0, 4, 0);
            this.label9.MaximumSize = new Size(0, 1);
            this.label9.MinimumSize = new Size(515, 0);
            this.label9.Name = "label9";
            this.label9.RightToLeft = RightToLeft.No;
            this.label9.Size = new Size(515, 1);
            this.label9.TabIndex = 201;
            this.label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(13, 63);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(129, 13);
            this.label2.TabIndex = 202;
            this.label2.Text = "This project includes:";
            // 
            // lblStuff
            // 
            this.lblStuff.AutoSize = true;
            this.lblStuff.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblStuff.ForeColor = Color.White;
            this.lblStuff.Location = new Point(13, 78);
            this.lblStuff.Margin = new Padding(4, 0, 4, 0);
            this.lblStuff.Name = "lblStuff";
            this.lblStuff.Size = new Size(13, 104);
            this.lblStuff.TabIndex = 203;
            this.lblStuff.Text = "-\r\n-\r\n-\r\n-\r\n-\r\n-\r\n\r\n-";
            // 
            // pictureThumb
            // 
            this.pictureThumb.BackColor = Color.Black;
            this.pictureThumb.BackgroundImageLayout = ImageLayout.None;
            this.pictureThumb.BorderStyle = BorderStyle.FixedSingle;
            this.pictureThumb.Location = new Point(0, 15);
            this.pictureThumb.Margin = new Padding(4, 3, 4, 3);
            this.pictureThumb.Name = "pictureThumb";
            this.pictureThumb.Size = new Size(118, 84);
            this.pictureThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureThumb.TabIndex = 204;
            this.pictureThumb.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.White;
            this.label3.Location = new Point(0, 0);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(65, 13);
            this.label3.TabIndex = 205;
            this.label3.Text = "Thumbnail";
            // 
            // panelThumb
            // 
            this.panelThumb.BackColor = Color.FromArgb(40, 40, 40);
            this.panelThumb.Controls.Add(this.pictureThumb);
            this.panelThumb.Controls.Add(this.label3);
            this.panelThumb.Location = new Point(13, 188);
            this.panelThumb.MinimumSize = new Size(60, 60);
            this.panelThumb.Name = "panelThumb";
            this.panelThumb.Size = new Size(200, 200);
            this.panelThumb.TabIndex = 206;
            this.panelThumb.Tag = "editorpanel";
            // 
            // btnExport
            // 
            this.btnExport.BackColor = Color.Gray;
            this.btnExport.Enabled = false;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.ForeColor = Color.White;
            this.btnExport.Location = new Point(195, 405);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(98, 38);
            this.btnExport.TabIndex = 207;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += this.btnExport_Click;
            // 
            // ExportProject
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ClientSize = new Size(488, 450);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.panelThumb);
            this.Controls.Add(this.lblStuff);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCustomFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomPath);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportProject";
            this.ShowIcon = false;
            this.Text = "Export Project For Public Release (ZIP)";
            this.Load += this.ExportProject_Load;
            ((System.ComponentModel.ISupportInitialize)this.pictureThumb).EndInit();
            this.panelThumb.ResumeLayout(false);
            this.panelThumb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btnCustomFolder;
        private Label label1;
        public TextBox txtCustomPath;
        private Label label9;
        private Label label2;
        private Label lblStuff;
        private PictureBox pictureThumb;
        private Label label3;
        private Panel panelThumb;
        private Button btnExport;
    }
}