
namespace Thumper_Custom_Level_Editor
{
	partial class ProjectPropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectPropertiesForm));
            this.txtCustomPath = new TextBox();
            this.label1 = new Label();
            this.btnCustomFolder = new Button();
            this.btnCustomCancel = new Button();
            this.btnCustomSave = new Button();
            this.label2 = new Label();
            this.txtCustomAuthor = new TextBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.txtCustomName = new TextBox();
            this.label5 = new Label();
            this.txtCustomDiff = new ComboBox();
            this.lblCustomDiffHelp = new Label();
            this.label6 = new Label();
            this.txtDesc = new RichTextBox();
            this.label8 = new Label();
            this.toolTip1 = new ToolTip(this.components);
            this.pictureDifficulty = new PictureBox();
            this.lblNameError = new Label();
            ((System.ComponentModel.ISupportInitialize)this.pictureDifficulty).BeginInit();
            this.SuspendLayout();
            // 
            // txtCustomPath
            // 
            this.txtCustomPath.BackColor = Color.FromArgb(40, 40, 40);
            this.txtCustomPath.Enabled = false;
            this.txtCustomPath.Font = new Font("Arial", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.txtCustomPath.ForeColor = Color.White;
            this.txtCustomPath.Location = new Point(14, 29);
            this.txtCustomPath.Margin = new Padding(4, 3, 4, 3);
            this.txtCustomPath.Name = "txtCustomPath";
            this.txtCustomPath.Size = new Size(438, 25);
            this.txtCustomPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(14, 12);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(234, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Where to save your project/level folder:";
            // 
            // btnCustomFolder
            // 
            this.btnCustomFolder.BackColor = Color.Gray;
            this.btnCustomFolder.FlatStyle = FlatStyle.Popup;
            this.btnCustomFolder.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnCustomFolder.ForeColor = Color.Black;
            this.btnCustomFolder.Image = Properties.Resources.icon_folder;
            this.btnCustomFolder.Location = new Point(453, 29);
            this.btnCustomFolder.Margin = new Padding(4, 3, 4, 3);
            this.btnCustomFolder.Name = "btnCustomFolder";
            this.btnCustomFolder.Size = new Size(24, 24);
            this.btnCustomFolder.TabIndex = 0;
            this.btnCustomFolder.TextAlign = ContentAlignment.TopCenter;
            this.btnCustomFolder.UseVisualStyleBackColor = false;
            this.btnCustomFolder.Click += this.btnCustomFolder_Click;
            // 
            // btnCustomCancel
            // 
            this.btnCustomCancel.BackColor = Color.Red;
            this.btnCustomCancel.FlatStyle = FlatStyle.Popup;
            this.btnCustomCancel.Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Underline);
            this.btnCustomCancel.ForeColor = Color.White;
            this.btnCustomCancel.ImageAlign = ContentAlignment.TopCenter;
            this.btnCustomCancel.Location = new Point(252, 335);
            this.btnCustomCancel.Margin = new Padding(4, 3, 4, 3);
            this.btnCustomCancel.Name = "btnCustomCancel";
            this.btnCustomCancel.Size = new Size(104, 28);
            this.btnCustomCancel.TabIndex = 19;
            this.btnCustomCancel.Text = "Cancel";
            this.btnCustomCancel.TextAlign = ContentAlignment.TopCenter;
            this.btnCustomCancel.UseVisualStyleBackColor = false;
            this.btnCustomCancel.Click += this.btnCustomCancel_Click;
            // 
            // btnCustomSave
            // 
            this.btnCustomSave.BackColor = Color.Green;
            this.btnCustomSave.Enabled = false;
            this.btnCustomSave.FlatStyle = FlatStyle.Popup;
            this.btnCustomSave.Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Underline);
            this.btnCustomSave.ForeColor = Color.White;
            this.btnCustomSave.ImageAlign = ContentAlignment.TopCenter;
            this.btnCustomSave.Location = new Point(150, 335);
            this.btnCustomSave.Margin = new Padding(4, 3, 4, 3);
            this.btnCustomSave.Name = "btnCustomSave";
            this.btnCustomSave.Size = new Size(98, 28);
            this.btnCustomSave.TabIndex = 18;
            this.btnCustomSave.Text = "Create";
            this.btnCustomSave.TextAlign = ContentAlignment.TopCenter;
            this.btnCustomSave.UseVisualStyleBackColor = false;
            this.btnCustomSave.Click += this.btnCustomSave_Click;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(1670, 706);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(27, 15);
            this.label2.TabIndex = 118;
            this.label2.Text = "egg";
            // 
            // txtCustomAuthor
            // 
            this.txtCustomAuthor.BackColor = Color.FromArgb(40, 40, 40);
            this.txtCustomAuthor.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtCustomAuthor.ForeColor = Color.White;
            this.txtCustomAuthor.Location = new Point(14, 123);
            this.txtCustomAuthor.Margin = new Padding(4, 3, 4, 3);
            this.txtCustomAuthor.Name = "txtCustomAuthor";
            this.txtCustomAuthor.Size = new Size(223, 26);
            this.txtCustomAuthor.TabIndex = 2;
            this.txtCustomAuthor.Text = "Noname";
            this.txtCustomAuthor.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.White;
            this.label3.Location = new Point(14, 107);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(84, 13);
            this.label3.TabIndex = 120;
            this.label3.Text = "Author Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.ForeColor = Color.White;
            this.label4.Location = new Point(14, 60);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(78, 13);
            this.label4.TabIndex = 122;
            this.label4.Text = "Level Name:";
            // 
            // txtCustomName
            // 
            this.txtCustomName.BackColor = Color.FromArgb(40, 40, 40);
            this.txtCustomName.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtCustomName.ForeColor = Color.White;
            this.txtCustomName.Location = new Point(14, 76);
            this.txtCustomName.Margin = new Padding(4, 3, 4, 3);
            this.txtCustomName.Name = "txtCustomName";
            this.txtCustomName.Size = new Size(223, 26);
            this.txtCustomName.TabIndex = 1;
            this.txtCustomName.Text = "The Best Level";
            this.txtCustomName.TextAlign = HorizontalAlignment.Center;
            this.txtCustomName.TextChanged += this.txtCustomName_TextChanged;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.ForeColor = Color.White;
            this.label5.Location = new Point(14, 154);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(114, 13);
            this.label5.TabIndex = 123;
            this.label5.Text = "Expected Difficulty";
            // 
            // txtCustomDiff
            // 
            this.txtCustomDiff.BackColor = Color.FromArgb(40, 40, 40);
            this.txtCustomDiff.DrawMode = DrawMode.OwnerDrawFixed;
            this.txtCustomDiff.DropDownStyle = ComboBoxStyle.DropDownList;
            this.txtCustomDiff.FlatStyle = FlatStyle.Flat;
            this.txtCustomDiff.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtCustomDiff.ForeColor = Color.White;
            this.txtCustomDiff.FormattingEnabled = true;
            this.txtCustomDiff.Items.AddRange(new object[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7" });
            this.txtCustomDiff.Location = new Point(14, 171);
            this.txtCustomDiff.Margin = new Padding(4, 3, 4, 3);
            this.txtCustomDiff.Name = "txtCustomDiff";
            this.txtCustomDiff.Size = new Size(146, 27);
            this.txtCustomDiff.TabIndex = 3;
            this.txtCustomDiff.DrawItem += this.combobox_DrawItem;
            this.txtCustomDiff.SelectedIndexChanged += this.txtCustomDiff_SelectedIndexChanged;
            // 
            // lblCustomDiffHelp
            // 
            this.lblCustomDiffHelp.AutoSize = true;
            this.lblCustomDiffHelp.BackColor = Color.Transparent;
            this.lblCustomDiffHelp.Cursor = Cursors.Help;
            this.lblCustomDiffHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblCustomDiffHelp.ForeColor = Color.DodgerBlue;
            this.lblCustomDiffHelp.Location = new Point(127, 152);
            this.lblCustomDiffHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblCustomDiffHelp.Name = "lblCustomDiffHelp";
            this.lblCustomDiffHelp.Size = new Size(15, 16);
            this.lblCustomDiffHelp.TabIndex = 125;
            this.lblCustomDiffHelp.Text = "?";
            this.lblCustomDiffHelp.Click += this.lblCustomDiffHelp_Click;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.ForeColor = Color.White;
            this.label6.Location = new Point(14, 203);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(106, 13);
            this.label6.TabIndex = 126;
            this.label6.Text = "Level Description";
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = Color.FromArgb(40, 40, 40);
            this.txtDesc.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtDesc.ForeColor = Color.White;
            this.txtDesc.Location = new Point(14, 219);
            this.txtDesc.Margin = new Padding(4, 3, 4, 3);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new Size(466, 110);
            this.txtDesc.TabIndex = 4;
            this.txtDesc.Text = "the best level description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = Color.Transparent;
            this.label8.Cursor = Cursors.Help;
            this.label8.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label8.ForeColor = Color.DodgerBlue;
            this.label8.Location = new Point(247, 10);
            this.label8.Margin = new Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new Size(15, 16);
            this.label8.TabIndex = 142;
            this.label8.Text = "?";
            this.toolTip1.SetToolTip(this.label8, "This is the root folder where the project folder will \r\nbe stored. After clicking Save, a new folder will be \r\ncreated using the Level Name.");
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.AutoPopDelay = 0;
            this.toolTip1.InitialDelay = 1;
            this.toolTip1.ReshowDelay = 100;
            // 
            // pictureDifficulty
            // 
            this.pictureDifficulty.BackgroundImageLayout = ImageLayout.None;
            this.pictureDifficulty.Image = Properties.Resources.difficulty_D0;
            this.pictureDifficulty.Location = new Point(164, 145);
            this.pictureDifficulty.Margin = new Padding(4, 3, 4, 3);
            this.pictureDifficulty.Name = "pictureDifficulty";
            this.pictureDifficulty.Size = new Size(75, 74);
            this.pictureDifficulty.TabIndex = 143;
            this.pictureDifficulty.TabStop = false;
            // 
            // lblNameError
            // 
            this.lblNameError.AutoSize = true;
            this.lblNameError.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblNameError.ForeColor = Color.Red;
            this.lblNameError.Location = new Point(112, 60);
            this.lblNameError.Margin = new Padding(4, 0, 4, 0);
            this.lblNameError.Name = "lblNameError";
            this.lblNameError.Size = new Size(58, 13);
            this.lblNameError.TabIndex = 144;
            this.lblNameError.Text = "error text";
            this.lblNameError.Visible = false;
            // 
            // ProjectPropertiesForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(496, 375);
            this.ControlBox = false;
            this.Controls.Add(this.txtCustomName);
            this.Controls.Add(this.lblNameError);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblCustomDiffHelp);
            this.Controls.Add(this.txtCustomDiff);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCustomSave);
            this.Controls.Add(this.btnCustomCancel);
            this.Controls.Add(this.btnCustomFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomPath);
            this.Controls.Add(this.txtCustomAuthor);
            this.Controls.Add(this.pictureDifficulty);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "ProjectPropertiesForm";
            this.ShowInTaskbar = false;
            this.Text = "Custom Level Details";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)this.pictureDifficulty).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCustomFolder;
		private System.Windows.Forms.Button btnCustomCancel;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox txtCustomPath;
		public System.Windows.Forms.TextBox txtCustomAuthor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.TextBox txtCustomName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCustomDiffHelp;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.RichTextBox txtDesc;
		public System.Windows.Forms.ComboBox txtCustomDiff;
        public System.Windows.Forms.Button btnCustomSave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureDifficulty;
        private System.Windows.Forms.Label lblNameError;
    }
}