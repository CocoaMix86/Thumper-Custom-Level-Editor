namespace Thumper_Custom_Level_Editor
{
	partial class AboutThumperEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutThumperEditor));
            this.okButton = new Button();
            this.textBoxDescription = new TextBox();
            this.labelCompanyName = new Label();
            this.labelCopyright = new Label();
            this.labelVersion = new Label();
            this.labelProductName = new Label();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.labelSoundCredit = new Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.okButton.DialogResult = DialogResult.Cancel;
            this.okButton.ForeColor = Color.White;
            this.okButton.Location = new Point(241, 177);
            this.okButton.Margin = new Padding(4, 3, 4, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(118, 27);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            this.okButton.Click += this.okButton_Click;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = DockStyle.Fill;
            this.textBoxDescription.Location = new Point(7, 95);
            this.textBoxDescription.Margin = new Padding(7, 3, 4, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = ScrollBars.Both;
            this.textBoxDescription.Size = new Size(352, 52);
            this.textBoxDescription.TabIndex = 23;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.Text = "Description";
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.Dock = DockStyle.Fill;
            this.labelCompanyName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCompanyName.ForeColor = Color.White;
            this.labelCompanyName.Location = new Point(7, 69);
            this.labelCompanyName.Margin = new Padding(7, 0, 4, 0);
            this.labelCompanyName.MaximumSize = new Size(0, 20);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new Size(352, 20);
            this.labelCompanyName.TabIndex = 22;
            this.labelCompanyName.Text = "Company Name";
            this.labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelCopyright
            // 
            this.labelCopyright.Dock = DockStyle.Fill;
            this.labelCopyright.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCopyright.ForeColor = Color.White;
            this.labelCopyright.Location = new Point(7, 46);
            this.labelCopyright.Margin = new Padding(7, 0, 4, 0);
            this.labelCopyright.MaximumSize = new Size(0, 20);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new Size(352, 20);
            this.labelCopyright.TabIndex = 21;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelVersion
            // 
            this.labelVersion.Dock = DockStyle.Fill;
            this.labelVersion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelVersion.ForeColor = Color.White;
            this.labelVersion.Location = new Point(7, 23);
            this.labelVersion.Margin = new Padding(7, 0, 4, 0);
            this.labelVersion.MaximumSize = new Size(0, 20);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new Size(352, 20);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelProductName
            // 
            this.labelProductName.Dock = DockStyle.Fill;
            this.labelProductName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelProductName.ForeColor = Color.White;
            this.labelProductName.Location = new Point(7, 0);
            this.labelProductName.Margin = new Padding(7, 0, 4, 0);
            this.labelProductName.MaximumSize = new Size(0, 20);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new Size(352, 20);
            this.labelProductName.TabIndex = 19;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelSoundCredit, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.okButton, 0, 5);
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Location = new Point(10, 10);
            this.tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            this.tableLayoutPanel.Size = new Size(363, 207);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelSoundCredit
            // 
            this.labelSoundCredit.Dock = DockStyle.Fill;
            this.labelSoundCredit.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelSoundCredit.ForeColor = Color.White;
            this.labelSoundCredit.Location = new Point(7, 150);
            this.labelSoundCredit.Margin = new Padding(7, 0, 4, 0);
            this.labelSoundCredit.MaximumSize = new Size(0, 20);
            this.labelSoundCredit.Name = "labelSoundCredit";
            this.labelSoundCredit.Size = new Size(352, 20);
            this.labelSoundCredit.TabIndex = 26;
            this.labelSoundCredit.Text = "Sound effects by CaptainJul (Discord member)";
            this.labelSoundCredit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AboutThumperEditor
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.ClientSize = new Size(383, 227);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutThumperEditor";
            this.Padding = new Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelSoundCredit;
    }
}
