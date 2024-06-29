namespace Thumper_Custom_Level_Editor
{
    partial class FileNameDialog
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
            this.panelWorkRename = new System.Windows.Forms.Panel();
            this.lblExists = new System.Windows.Forms.Label();
            this.lblRenameFileType = new System.Windows.Forms.Label();
            this.btnWorkRenameNo = new System.Windows.Forms.Button();
            this.btnWorkRenameYes = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.txtWorkingRename = new System.Windows.Forms.TextBox();
            this.panelWorkRename.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkRename
            // 
            this.panelWorkRename.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWorkRename.Controls.Add(this.lblExists);
            this.panelWorkRename.Controls.Add(this.lblRenameFileType);
            this.panelWorkRename.Controls.Add(this.btnWorkRenameNo);
            this.panelWorkRename.Controls.Add(this.btnWorkRenameYes);
            this.panelWorkRename.Controls.Add(this.label47);
            this.panelWorkRename.Controls.Add(this.txtWorkingRename);
            this.panelWorkRename.Location = new System.Drawing.Point(0, 0);
            this.panelWorkRename.Margin = new System.Windows.Forms.Padding(0);
            this.panelWorkRename.Name = "panelWorkRename";
            this.panelWorkRename.Size = new System.Drawing.Size(212, 75);
            this.panelWorkRename.TabIndex = 137;
            // 
            // lblExists
            // 
            this.lblExists.AutoSize = true;
            this.lblExists.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExists.ForeColor = System.Drawing.Color.Red;
            this.lblExists.Location = new System.Drawing.Point(-1, 4);
            this.lblExists.Name = "lblExists";
            this.lblExists.Size = new System.Drawing.Size(173, 13);
            this.lblExists.TabIndex = 96;
            this.lblExists.Text = "That file name exists already!";
            this.lblExists.Visible = false;
            // 
            // lblRenameFileType
            // 
            this.lblRenameFileType.AutoSize = true;
            this.lblRenameFileType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRenameFileType.ForeColor = System.Drawing.Color.White;
            this.lblRenameFileType.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.leaf;
            this.lblRenameFileType.Location = new System.Drawing.Point(3, 24);
            this.lblRenameFileType.MinimumSize = new System.Drawing.Size(16, 16);
            this.lblRenameFileType.Name = "lblRenameFileType";
            this.lblRenameFileType.Size = new System.Drawing.Size(16, 16);
            this.lblRenameFileType.TabIndex = 95;
            // 
            // btnWorkRenameNo
            // 
            this.btnWorkRenameNo.BackColor = System.Drawing.Color.Red;
            this.btnWorkRenameNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnWorkRenameNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkRenameNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkRenameNo.ForeColor = System.Drawing.Color.White;
            this.btnWorkRenameNo.Location = new System.Drawing.Point(88, 44);
            this.btnWorkRenameNo.Name = "btnWorkRenameNo";
            this.btnWorkRenameNo.Size = new System.Drawing.Size(81, 24);
            this.btnWorkRenameNo.TabIndex = 94;
            this.btnWorkRenameNo.Text = "Cancel";
            this.btnWorkRenameNo.UseVisualStyleBackColor = false;
            // 
            // btnWorkRenameYes
            // 
            this.btnWorkRenameYes.BackColor = System.Drawing.Color.Green;
            this.btnWorkRenameYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnWorkRenameYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkRenameYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkRenameYes.ForeColor = System.Drawing.Color.White;
            this.btnWorkRenameYes.Location = new System.Drawing.Point(2, 44);
            this.btnWorkRenameYes.Name = "btnWorkRenameYes";
            this.btnWorkRenameYes.Size = new System.Drawing.Size(81, 24);
            this.btnWorkRenameYes.TabIndex = 93;
            this.btnWorkRenameYes.Text = "Confirm";
            this.btnWorkRenameYes.UseVisualStyleBackColor = false;
            this.btnWorkRenameYes.Click += new System.EventHandler(this.btnWorkRenameYes_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.Color.White;
            this.label47.Location = new System.Drawing.Point(4, 4);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(100, 13);
            this.label47.TabIndex = 92;
            this.label47.Text = "Enter new file name";
            // 
            // txtWorkingRename
            // 
            this.txtWorkingRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtWorkingRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWorkingRename.ForeColor = System.Drawing.Color.White;
            this.txtWorkingRename.Location = new System.Drawing.Point(21, 21);
            this.txtWorkingRename.Name = "txtWorkingRename";
            this.txtWorkingRename.Size = new System.Drawing.Size(149, 22);
            this.txtWorkingRename.TabIndex = 0;
            this.txtWorkingRename.TextChanged += new System.EventHandler(this.txtWorkingRename_TextChanged);
            this.txtWorkingRename.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileNameDialog_KeyDown);
            // 
            // FileNameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(210, 72);
            this.Controls.Add(this.panelWorkRename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileNameDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Choose New File Name";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileNameDialog_KeyDown);
            this.panelWorkRename.ResumeLayout(false);
            this.panelWorkRename.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelWorkRename;
        private System.Windows.Forms.Label label47;
        public System.Windows.Forms.Button btnWorkRenameNo;
        public System.Windows.Forms.Button btnWorkRenameYes;
        public System.Windows.Forms.TextBox txtWorkingRename;
        public System.Windows.Forms.Label lblRenameFileType;
        private System.Windows.Forms.Label lblExists;
    }
}