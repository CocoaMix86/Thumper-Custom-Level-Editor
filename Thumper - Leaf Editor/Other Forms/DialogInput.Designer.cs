
namespace Thumper_Custom_Level_Editor
{
	partial class DialogInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogInput));
            this.txtCustomPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCustomFolder = new System.Windows.Forms.Button();
            this.btnCustomCancel = new System.Windows.Forms.Button();
            this.btnCustomSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCustomAuthor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCustomName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCustomDiff = new System.Windows.Forms.ComboBox();
            this.lblCustomDiffHelp = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkLevel2 = new System.Windows.Forms.CheckBox();
            this.chkLevel3 = new System.Windows.Forms.CheckBox();
            this.chkLevel4 = new System.Windows.Forms.CheckBox();
            this.chkLevel5 = new System.Windows.Forms.CheckBox();
            this.chkLevel9 = new System.Windows.Forms.CheckBox();
            this.chkLevel8 = new System.Windows.Forms.CheckBox();
            this.chkLevel7 = new System.Windows.Forms.CheckBox();
            this.chkLevel6 = new System.Windows.Forms.CheckBox();
            this.chkLevel1 = new System.Windows.Forms.CheckBox();
            this.chkMisc = new System.Windows.Forms.CheckBox();
            this.chkRests = new System.Windows.Forms.CheckBox();
            this.chkGlobal = new System.Windows.Forms.CheckBox();
            this.chkDissonance = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureDifficulty = new System.Windows.Forms.PictureBox();
            this.lblNameError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDifficulty)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCustomPath
            // 
            this.txtCustomPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCustomPath.Enabled = false;
            this.txtCustomPath.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomPath.ForeColor = System.Drawing.Color.White;
            this.txtCustomPath.Location = new System.Drawing.Point(12, 25);
            this.txtCustomPath.Name = "txtCustomPath";
            this.txtCustomPath.Size = new System.Drawing.Size(376, 22);
            this.txtCustomPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Where to save your project/level folder:";
            // 
            // btnCustomFolder
            // 
            this.btnCustomFolder.BackColor = System.Drawing.Color.Gray;
            this.btnCustomFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustomFolder.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomFolder.ForeColor = System.Drawing.Color.Black;
            this.btnCustomFolder.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_folder;
            this.btnCustomFolder.Location = new System.Drawing.Point(388, 24);
            this.btnCustomFolder.Name = "btnCustomFolder";
            this.btnCustomFolder.Size = new System.Drawing.Size(24, 24);
            this.btnCustomFolder.TabIndex = 0;
            this.btnCustomFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomFolder.UseVisualStyleBackColor = false;
            this.btnCustomFolder.Click += new System.EventHandler(this.btnCustomFolder_Click);
            // 
            // btnCustomCancel
            // 
            this.btnCustomCancel.BackColor = System.Drawing.Color.Red;
            this.btnCustomCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustomCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomCancel.ForeColor = System.Drawing.Color.White;
            this.btnCustomCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomCancel.Location = new System.Drawing.Point(352, 406);
            this.btnCustomCancel.Name = "btnCustomCancel";
            this.btnCustomCancel.Size = new System.Drawing.Size(60, 24);
            this.btnCustomCancel.TabIndex = 19;
            this.btnCustomCancel.Text = "Cancel";
            this.btnCustomCancel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomCancel.UseVisualStyleBackColor = false;
            this.btnCustomCancel.Click += new System.EventHandler(this.btnCustomCancel_Click);
            // 
            // btnCustomSave
            // 
            this.btnCustomSave.BackColor = System.Drawing.Color.Green;
            this.btnCustomSave.Enabled = false;
            this.btnCustomSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustomSave.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomSave.ForeColor = System.Drawing.Color.White;
            this.btnCustomSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomSave.Location = new System.Drawing.Point(289, 406);
            this.btnCustomSave.Name = "btnCustomSave";
            this.btnCustomSave.Size = new System.Drawing.Size(60, 24);
            this.btnCustomSave.TabIndex = 18;
            this.btnCustomSave.Text = "Save";
            this.btnCustomSave.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustomSave.UseVisualStyleBackColor = false;
            this.btnCustomSave.Click += new System.EventHandler(this.btnCustomSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(1431, 612);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 118;
            this.label2.Text = "egg";
            // 
            // txtCustomAuthor
            // 
            this.txtCustomAuthor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCustomAuthor.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomAuthor.ForeColor = System.Drawing.Color.White;
            this.txtCustomAuthor.Location = new System.Drawing.Point(12, 107);
            this.txtCustomAuthor.Name = "txtCustomAuthor";
            this.txtCustomAuthor.Size = new System.Drawing.Size(192, 22);
            this.txtCustomAuthor.TabIndex = 2;
            this.txtCustomAuthor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 120;
            this.label3.Text = "Author Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 122;
            this.label4.Text = "Level Name:";
            // 
            // txtCustomName
            // 
            this.txtCustomName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCustomName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomName.ForeColor = System.Drawing.Color.White;
            this.txtCustomName.Location = new System.Drawing.Point(12, 66);
            this.txtCustomName.Name = "txtCustomName";
            this.txtCustomName.Size = new System.Drawing.Size(192, 22);
            this.txtCustomName.TabIndex = 1;
            this.txtCustomName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCustomName.TextChanged += new System.EventHandler(this.txtCustomName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 123;
            this.label5.Text = "Expected Difficulty";
            // 
            // txtCustomDiff
            // 
            this.txtCustomDiff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCustomDiff.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.txtCustomDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtCustomDiff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtCustomDiff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomDiff.ForeColor = System.Drawing.Color.White;
            this.txtCustomDiff.FormattingEnabled = true;
            this.txtCustomDiff.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7"});
            this.txtCustomDiff.Location = new System.Drawing.Point(12, 148);
            this.txtCustomDiff.Name = "txtCustomDiff";
            this.txtCustomDiff.Size = new System.Drawing.Size(126, 23);
            this.txtCustomDiff.TabIndex = 3;
            this.txtCustomDiff.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combobox_DrawItem);
            this.txtCustomDiff.TextChanged += new System.EventHandler(this.txtCustomDiff_TextChanged);
            // 
            // lblCustomDiffHelp
            // 
            this.lblCustomDiffHelp.AutoSize = true;
            this.lblCustomDiffHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomDiffHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblCustomDiffHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomDiffHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCustomDiffHelp.Location = new System.Drawing.Point(123, 130);
            this.lblCustomDiffHelp.Name = "lblCustomDiffHelp";
            this.lblCustomDiffHelp.Size = new System.Drawing.Size(15, 16);
            this.lblCustomDiffHelp.TabIndex = 125;
            this.lblCustomDiffHelp.Text = "?";
            this.lblCustomDiffHelp.Click += new System.EventHandler(this.lblCustomDiffHelp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 126;
            this.label6.Text = "Level Description";
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtDesc.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.ForeColor = System.Drawing.Color.White;
            this.txtDesc.Location = new System.Drawing.Point(12, 190);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(400, 96);
            this.txtDesc.TabIndex = 4;
            this.txtDesc.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(12, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(221, 13);
            this.label7.TabIndex = 128;
            this.label7.Text = "Samples/Loop Track Packs (optional)";
            // 
            // chkLevel2
            // 
            this.chkLevel2.AutoSize = true;
            this.chkLevel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel2.ForeColor = System.Drawing.Color.White;
            this.chkLevel2.Location = new System.Drawing.Point(15, 331);
            this.chkLevel2.Name = "chkLevel2";
            this.chkLevel2.Size = new System.Drawing.Size(128, 20);
            this.chkLevel2.TabIndex = 6;
            this.chkLevel2.Text = "Level 2 (340bpm)";
            this.chkLevel2.UseVisualStyleBackColor = true;
            // 
            // chkLevel3
            // 
            this.chkLevel3.AutoSize = true;
            this.chkLevel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel3.ForeColor = System.Drawing.Color.White;
            this.chkLevel3.Location = new System.Drawing.Point(15, 350);
            this.chkLevel3.Name = "chkLevel3";
            this.chkLevel3.Size = new System.Drawing.Size(128, 20);
            this.chkLevel3.TabIndex = 7;
            this.chkLevel3.Text = "Level 3 (360bpm)";
            this.chkLevel3.UseVisualStyleBackColor = true;
            // 
            // chkLevel4
            // 
            this.chkLevel4.AutoSize = true;
            this.chkLevel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel4.ForeColor = System.Drawing.Color.White;
            this.chkLevel4.Location = new System.Drawing.Point(15, 369);
            this.chkLevel4.Name = "chkLevel4";
            this.chkLevel4.Size = new System.Drawing.Size(128, 20);
            this.chkLevel4.TabIndex = 8;
            this.chkLevel4.Text = "Level 4 (380bpm)";
            this.chkLevel4.UseVisualStyleBackColor = true;
            // 
            // chkLevel5
            // 
            this.chkLevel5.AutoSize = true;
            this.chkLevel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel5.ForeColor = System.Drawing.Color.White;
            this.chkLevel5.Location = new System.Drawing.Point(15, 388);
            this.chkLevel5.Name = "chkLevel5";
            this.chkLevel5.Size = new System.Drawing.Size(128, 20);
            this.chkLevel5.TabIndex = 9;
            this.chkLevel5.Text = "Level 5 (400bpm)";
            this.chkLevel5.UseVisualStyleBackColor = true;
            // 
            // chkLevel9
            // 
            this.chkLevel9.AutoSize = true;
            this.chkLevel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel9.ForeColor = System.Drawing.Color.White;
            this.chkLevel9.Location = new System.Drawing.Point(150, 369);
            this.chkLevel9.Name = "chkLevel9";
            this.chkLevel9.Size = new System.Drawing.Size(128, 20);
            this.chkLevel9.TabIndex = 13;
            this.chkLevel9.Text = "Level 9 (480bpm)";
            this.chkLevel9.UseVisualStyleBackColor = true;
            // 
            // chkLevel8
            // 
            this.chkLevel8.AutoSize = true;
            this.chkLevel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel8.ForeColor = System.Drawing.Color.White;
            this.chkLevel8.Location = new System.Drawing.Point(150, 350);
            this.chkLevel8.Name = "chkLevel8";
            this.chkLevel8.Size = new System.Drawing.Size(128, 20);
            this.chkLevel8.TabIndex = 12;
            this.chkLevel8.Text = "Level 8 (460bpm)";
            this.chkLevel8.UseVisualStyleBackColor = true;
            // 
            // chkLevel7
            // 
            this.chkLevel7.AutoSize = true;
            this.chkLevel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel7.ForeColor = System.Drawing.Color.White;
            this.chkLevel7.Location = new System.Drawing.Point(150, 331);
            this.chkLevel7.Name = "chkLevel7";
            this.chkLevel7.Size = new System.Drawing.Size(128, 20);
            this.chkLevel7.TabIndex = 11;
            this.chkLevel7.Text = "Level 7 (440bpm)";
            this.chkLevel7.UseVisualStyleBackColor = true;
            // 
            // chkLevel6
            // 
            this.chkLevel6.AutoSize = true;
            this.chkLevel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel6.ForeColor = System.Drawing.Color.White;
            this.chkLevel6.Location = new System.Drawing.Point(150, 312);
            this.chkLevel6.Name = "chkLevel6";
            this.chkLevel6.Size = new System.Drawing.Size(128, 20);
            this.chkLevel6.TabIndex = 10;
            this.chkLevel6.Text = "Level 6 (420bpm)";
            this.chkLevel6.UseVisualStyleBackColor = true;
            // 
            // chkLevel1
            // 
            this.chkLevel1.AutoSize = true;
            this.chkLevel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLevel1.ForeColor = System.Drawing.Color.White;
            this.chkLevel1.Location = new System.Drawing.Point(15, 312);
            this.chkLevel1.Name = "chkLevel1";
            this.chkLevel1.Size = new System.Drawing.Size(128, 20);
            this.chkLevel1.TabIndex = 5;
            this.chkLevel1.Text = "Level 1 (320bpm)";
            this.chkLevel1.UseVisualStyleBackColor = true;
            // 
            // chkMisc
            // 
            this.chkMisc.AutoSize = true;
            this.chkMisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMisc.ForeColor = System.Drawing.Color.White;
            this.chkMisc.Location = new System.Drawing.Point(286, 369);
            this.chkMisc.Name = "chkMisc";
            this.chkMisc.Size = new System.Drawing.Size(57, 20);
            this.chkMisc.TabIndex = 17;
            this.chkMisc.Text = "Misc.";
            this.chkMisc.UseVisualStyleBackColor = true;
            // 
            // chkRests
            // 
            this.chkRests.AutoSize = true;
            this.chkRests.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRests.ForeColor = System.Drawing.Color.White;
            this.chkRests.Location = new System.Drawing.Point(286, 350);
            this.chkRests.Name = "chkRests";
            this.chkRests.Size = new System.Drawing.Size(61, 20);
            this.chkRests.TabIndex = 16;
            this.chkRests.Text = "Rests";
            this.chkRests.UseVisualStyleBackColor = true;
            // 
            // chkGlobal
            // 
            this.chkGlobal.AutoSize = true;
            this.chkGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGlobal.ForeColor = System.Drawing.Color.White;
            this.chkGlobal.Location = new System.Drawing.Point(286, 331);
            this.chkGlobal.Name = "chkGlobal";
            this.chkGlobal.Size = new System.Drawing.Size(113, 20);
            this.chkGlobal.TabIndex = 15;
            this.chkGlobal.Text = "Global Drones";
            this.chkGlobal.UseVisualStyleBackColor = true;
            // 
            // chkDissonance
            // 
            this.chkDissonance.AutoSize = true;
            this.chkDissonance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDissonance.ForeColor = System.Drawing.Color.White;
            this.chkDissonance.Location = new System.Drawing.Point(286, 312);
            this.chkDissonance.Name = "chkDissonance";
            this.chkDissonance.Size = new System.Drawing.Size(98, 20);
            this.chkDissonance.TabIndex = 14;
            this.chkDissonance.Text = "Dissonance";
            this.chkDissonance.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Cursor = System.Windows.Forms.Cursors.Help;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label8.Location = new System.Drawing.Point(247, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 16);
            this.label8.TabIndex = 142;
            this.label8.Text = "?";
            this.toolTip1.SetToolTip(this.label8, "This is the root folder where the project folder will \r\nbe stored. After clicking" +
        " Save, a new folder will be \r\ncreated using the Level Name.");
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
            this.pictureDifficulty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureDifficulty.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.D0;
            this.pictureDifficulty.Location = new System.Drawing.Point(144, 139);
            this.pictureDifficulty.Name = "pictureDifficulty";
            this.pictureDifficulty.Size = new System.Drawing.Size(40, 40);
            this.pictureDifficulty.TabIndex = 143;
            this.pictureDifficulty.TabStop = false;
            // 
            // lblNameError
            // 
            this.lblNameError.AutoSize = true;
            this.lblNameError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameError.ForeColor = System.Drawing.Color.Red;
            this.lblNameError.Location = new System.Drawing.Point(96, 50);
            this.lblNameError.Name = "lblNameError";
            this.lblNameError.Size = new System.Drawing.Size(58, 13);
            this.lblNameError.TabIndex = 144;
            this.lblNameError.Text = "error text";
            this.lblNameError.Visible = false;
            // 
            // DialogInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(425, 436);
            this.ControlBox = false;
            this.Controls.Add(this.lblNameError);
            this.Controls.Add(this.pictureDifficulty);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkMisc);
            this.Controls.Add(this.chkRests);
            this.Controls.Add(this.chkGlobal);
            this.Controls.Add(this.chkDissonance);
            this.Controls.Add(this.chkLevel1);
            this.Controls.Add(this.chkLevel9);
            this.Controls.Add(this.chkLevel8);
            this.Controls.Add(this.chkLevel7);
            this.Controls.Add(this.chkLevel6);
            this.Controls.Add(this.chkLevel5);
            this.Controls.Add(this.chkLevel4);
            this.Controls.Add(this.chkLevel3);
            this.Controls.Add(this.chkLevel2);
            this.Controls.Add(this.label7);
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
            this.Controls.Add(this.txtCustomName);
            this.Controls.Add(this.txtCustomAuthor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogInput";
            this.ShowInTaskbar = false;
            this.Text = "Custom Level Details";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureDifficulty)).EndInit();
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
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.CheckBox chkLevel2;
		public System.Windows.Forms.CheckBox chkLevel3;
		public System.Windows.Forms.CheckBox chkLevel4;
		public System.Windows.Forms.CheckBox chkLevel5;
		public System.Windows.Forms.CheckBox chkLevel9;
		public System.Windows.Forms.CheckBox chkLevel8;
		public System.Windows.Forms.CheckBox chkLevel7;
		public System.Windows.Forms.CheckBox chkLevel6;
		public System.Windows.Forms.CheckBox chkLevel1;
		public System.Windows.Forms.CheckBox chkMisc;
		public System.Windows.Forms.CheckBox chkRests;
		public System.Windows.Forms.CheckBox chkGlobal;
		public System.Windows.Forms.CheckBox chkDissonance;
        public System.Windows.Forms.Button btnCustomSave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureDifficulty;
        private System.Windows.Forms.Label lblNameError;
    }
}