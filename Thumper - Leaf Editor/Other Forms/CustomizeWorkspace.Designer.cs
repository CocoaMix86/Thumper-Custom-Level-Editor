
namespace Thumper_Custom_Level_Editor
{
	partial class CustomizeWorkspace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeWorkspace));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolstripCustomize = new System.Windows.Forms.ToolStrip();
            this.btnCustomizeApply = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnObjectColor = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dropObjects = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dropParamPath = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnActiveColor = new System.Windows.Forms.Button();
            this.btnSampleColor = new System.Windows.Forms.Button();
            this.btnLeafColor = new System.Windows.Forms.Button();
            this.btnLvlColor = new System.Windows.Forms.Button();
            this.btnGateColor = new System.Windows.Forms.Button();
            this.btnMasterColor = new System.Windows.Forms.Button();
            this.btnMenuColor = new System.Windows.Forms.Button();
            this.btnBGColor = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkMuteApp = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panelSetKeybind = new System.Windows.Forms.Panel();
            this.btnSetKeybind = new System.Windows.Forms.Button();
            this.labelKeys = new System.Windows.Forms.Label();
            this.labelKeybindName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.keybindMasterSaveAs = new System.Windows.Forms.Label();
            this.keybindMasterSave = new System.Windows.Forms.Label();
            this.keybindMasterOpen = new System.Windows.Forms.Label();
            this.keybindMasterNew = new System.Windows.Forms.Label();
            this.keybindGateSaveAs = new System.Windows.Forms.Label();
            this.keybindGateSave = new System.Windows.Forms.Label();
            this.keybindGateOpen = new System.Windows.Forms.Label();
            this.keybindGateNew = new System.Windows.Forms.Label();
            this.keybindLvlSaveAs = new System.Windows.Forms.Label();
            this.keybindLvlSave = new System.Windows.Forms.Label();
            this.keybindLvlOpen = new System.Windows.Forms.Label();
            this.keybindLvlNew = new System.Windows.Forms.Label();
            this.keybindLeafSaveAs = new System.Windows.Forms.Label();
            this.keybindLeafSave = new System.Windows.Forms.Label();
            this.keybindLeafOpen = new System.Windows.Forms.Label();
            this.keybindLeafNew = new System.Windows.Forms.Label();
            this.txtKeybindSearch = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolstripCustomize.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panelSetKeybind.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolstripCustomize
            // 
            this.toolstripCustomize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.toolstripCustomize.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolstripCustomize.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripCustomize.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCustomizeApply});
            this.toolstripCustomize.Location = new System.Drawing.Point(0, 270);
            this.toolstripCustomize.Name = "toolstripCustomize";
            this.toolstripCustomize.Size = new System.Drawing.Size(367, 25);
            this.toolstripCustomize.TabIndex = 106;
            this.toolstripCustomize.Text = "toolStrip1";
            // 
            // btnCustomizeApply
            // 
            this.btnCustomizeApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnCustomizeApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCustomizeApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomizeApply.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCustomizeApply.Image = ((System.Drawing.Image)(resources.GetObject("btnCustomizeApply.Image")));
            this.btnCustomizeApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCustomizeApply.Margin = new System.Windows.Forms.Padding(140, 1, 0, 2);
            this.btnCustomizeApply.Name = "btnCustomizeApply";
            this.btnCustomizeApply.Size = new System.Drawing.Size(91, 22);
            this.btnCustomizeApply.Text = "Apply Changes";
            this.btnCustomizeApply.Click += new System.EventHandler(this.btnCustomizeApply_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(-5, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(376, 306);
            this.tabControl1.TabIndex = 107;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.tabPage1.Controls.Add(this.btnObjectColor);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.dropObjects);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.dropParamPath);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnActiveColor);
            this.tabPage1.Controls.Add(this.btnSampleColor);
            this.tabPage1.Controls.Add(this.btnLeafColor);
            this.tabPage1.Controls.Add(this.btnLvlColor);
            this.tabPage1.Controls.Add(this.btnGateColor);
            this.tabPage1.Controls.Add(this.btnMasterColor);
            this.tabPage1.Controls.Add(this.btnMenuColor);
            this.tabPage1.Controls.Add(this.btnBGColor);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 280);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Colors";
            // 
            // btnObjectColor
            // 
            this.btnObjectColor.BackColor = System.Drawing.Color.White;
            this.btnObjectColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnObjectColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnObjectColor.FlatAppearance.BorderSize = 4;
            this.btnObjectColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnObjectColor.ForeColor = System.Drawing.Color.Black;
            this.btnObjectColor.Location = new System.Drawing.Point(306, 89);
            this.btnObjectColor.Name = "btnObjectColor";
            this.btnObjectColor.Size = new System.Drawing.Size(53, 26);
            this.btnObjectColor.TabIndex = 127;
            this.btnObjectColor.Tag = "customcolorbutton";
            this.btnObjectColor.UseMnemonic = false;
            this.btnObjectColor.UseVisualStyleBackColor = false;
            this.btnObjectColor.Click += new System.EventHandler(this.btnObjectColor_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(191, 94);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 15);
            this.label12.TabIndex = 128;
            this.label12.Text = "Object Default Color";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(186, 7);
            this.label11.MinimumSize = new System.Drawing.Size(0, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(2, 230);
            this.label11.TabIndex = 126;
            // 
            // dropObjects
            // 
            this.dropObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropObjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropObjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropObjects.ForeColor = System.Drawing.Color.White;
            this.dropObjects.FormattingEnabled = true;
            this.dropObjects.Location = new System.Drawing.Point(194, 22);
            this.dropObjects.Name = "dropObjects";
            this.dropObjects.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropObjects.Size = new System.Drawing.Size(163, 21);
            this.dropObjects.TabIndex = 123;
            this.dropObjects.SelectedIndexChanged += new System.EventHandler(this.dropObjects_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(190, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 15);
            this.label9.TabIndex = 122;
            this.label9.Text = "Track Object";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(190, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 15);
            this.label10.TabIndex = 124;
            this.label10.Text = "Type (param_path)";
            // 
            // dropParamPath
            // 
            this.dropParamPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropParamPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropParamPath.DropDownWidth = 180;
            this.dropParamPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropParamPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropParamPath.ForeColor = System.Drawing.Color.White;
            this.dropParamPath.FormattingEnabled = true;
            this.dropParamPath.Location = new System.Drawing.Point(194, 62);
            this.dropParamPath.Name = "dropParamPath";
            this.dropParamPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropParamPath.Size = new System.Drawing.Size(163, 21);
            this.dropParamPath.TabIndex = 125;
            this.dropParamPath.SelectedIndexChanged += new System.EventHandler(this.dropParamPath_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(20, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 15);
            this.label8.TabIndex = 121;
            this.label8.Text = "Leaf Editor Color";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(18, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 15);
            this.label7.TabIndex = 120;
            this.label7.Text = "Gate Editor Color";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(6, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 15);
            this.label6.TabIndex = 119;
            this.label6.Text = "Background Colour";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 15);
            this.label5.TabIndex = 118;
            this.label5.Text = "Active Panel Color";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(1, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 15);
            this.label4.TabIndex = 117;
            this.label4.Text = "Sample Editor Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(29, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 116;
            this.label3.Text = "Lvl Editor Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 115;
            this.label2.Text = "Master Editor Color";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 114;
            this.label1.Text = "Menu Colour";
            // 
            // btnActiveColor
            // 
            this.btnActiveColor.BackColor = System.Drawing.Color.White;
            this.btnActiveColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActiveColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnActiveColor.FlatAppearance.BorderSize = 4;
            this.btnActiveColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveColor.ForeColor = System.Drawing.Color.Black;
            this.btnActiveColor.Location = new System.Drawing.Point(119, 163);
            this.btnActiveColor.Name = "btnActiveColor";
            this.btnActiveColor.Size = new System.Drawing.Size(60, 20);
            this.btnActiveColor.TabIndex = 113;
            this.btnActiveColor.Tag = "customcolorbutton";
            this.btnActiveColor.UseMnemonic = false;
            this.btnActiveColor.UseVisualStyleBackColor = false;
            this.btnActiveColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnSampleColor
            // 
            this.btnSampleColor.BackColor = System.Drawing.Color.White;
            this.btnSampleColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSampleColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSampleColor.FlatAppearance.BorderSize = 4;
            this.btnSampleColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleColor.ForeColor = System.Drawing.Color.Black;
            this.btnSampleColor.Location = new System.Drawing.Point(119, 83);
            this.btnSampleColor.Name = "btnSampleColor";
            this.btnSampleColor.Size = new System.Drawing.Size(60, 20);
            this.btnSampleColor.TabIndex = 112;
            this.btnSampleColor.Tag = "customcolorbutton";
            this.btnSampleColor.UseMnemonic = false;
            this.btnSampleColor.UseVisualStyleBackColor = false;
            this.btnSampleColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnLeafColor
            // 
            this.btnLeafColor.BackColor = System.Drawing.Color.White;
            this.btnLeafColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLeafColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLeafColor.FlatAppearance.BorderSize = 4;
            this.btnLeafColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeafColor.ForeColor = System.Drawing.Color.Black;
            this.btnLeafColor.Location = new System.Drawing.Point(119, 136);
            this.btnLeafColor.Name = "btnLeafColor";
            this.btnLeafColor.Size = new System.Drawing.Size(60, 20);
            this.btnLeafColor.TabIndex = 111;
            this.btnLeafColor.Tag = "customcolorbutton";
            this.btnLeafColor.UseMnemonic = false;
            this.btnLeafColor.UseVisualStyleBackColor = false;
            this.btnLeafColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnLvlColor
            // 
            this.btnLvlColor.BackColor = System.Drawing.Color.White;
            this.btnLvlColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLvlColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLvlColor.FlatAppearance.BorderSize = 4;
            this.btnLvlColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLvlColor.ForeColor = System.Drawing.Color.Black;
            this.btnLvlColor.Location = new System.Drawing.Point(119, 57);
            this.btnLvlColor.Name = "btnLvlColor";
            this.btnLvlColor.Size = new System.Drawing.Size(60, 20);
            this.btnLvlColor.TabIndex = 110;
            this.btnLvlColor.Tag = "customcolorbutton";
            this.btnLvlColor.UseMnemonic = false;
            this.btnLvlColor.UseVisualStyleBackColor = false;
            this.btnLvlColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnGateColor
            // 
            this.btnGateColor.BackColor = System.Drawing.Color.White;
            this.btnGateColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGateColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnGateColor.FlatAppearance.BorderSize = 4;
            this.btnGateColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGateColor.ForeColor = System.Drawing.Color.Black;
            this.btnGateColor.Location = new System.Drawing.Point(119, 110);
            this.btnGateColor.Name = "btnGateColor";
            this.btnGateColor.Size = new System.Drawing.Size(60, 20);
            this.btnGateColor.TabIndex = 109;
            this.btnGateColor.Tag = "customcolorbutton";
            this.btnGateColor.UseMnemonic = false;
            this.btnGateColor.UseVisualStyleBackColor = false;
            this.btnGateColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnMasterColor
            // 
            this.btnMasterColor.BackColor = System.Drawing.Color.White;
            this.btnMasterColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMasterColor.FlatAppearance.BorderSize = 4;
            this.btnMasterColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMasterColor.ForeColor = System.Drawing.Color.Black;
            this.btnMasterColor.Location = new System.Drawing.Point(119, 31);
            this.btnMasterColor.Name = "btnMasterColor";
            this.btnMasterColor.Size = new System.Drawing.Size(60, 20);
            this.btnMasterColor.TabIndex = 108;
            this.btnMasterColor.Tag = "customcolorbutton";
            this.btnMasterColor.UseMnemonic = false;
            this.btnMasterColor.UseVisualStyleBackColor = false;
            this.btnMasterColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnMenuColor
            // 
            this.btnMenuColor.BackColor = System.Drawing.Color.White;
            this.btnMenuColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMenuColor.FlatAppearance.BorderSize = 4;
            this.btnMenuColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuColor.ForeColor = System.Drawing.Color.Black;
            this.btnMenuColor.Location = new System.Drawing.Point(119, 5);
            this.btnMenuColor.Name = "btnMenuColor";
            this.btnMenuColor.Size = new System.Drawing.Size(60, 20);
            this.btnMenuColor.TabIndex = 107;
            this.btnMenuColor.Tag = "customcolorbutton";
            this.btnMenuColor.UseMnemonic = false;
            this.btnMenuColor.UseVisualStyleBackColor = false;
            this.btnMenuColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // btnBGColor
            // 
            this.btnBGColor.BackColor = System.Drawing.Color.White;
            this.btnBGColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBGColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBGColor.FlatAppearance.BorderSize = 4;
            this.btnBGColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBGColor.ForeColor = System.Drawing.Color.Black;
            this.btnBGColor.Location = new System.Drawing.Point(119, 189);
            this.btnBGColor.Name = "btnBGColor";
            this.btnBGColor.Size = new System.Drawing.Size(60, 20);
            this.btnBGColor.TabIndex = 106;
            this.btnBGColor.Tag = "customcolorbutton";
            this.btnBGColor.UseMnemonic = false;
            this.btnBGColor.UseVisualStyleBackColor = false;
            this.btnBGColor.Click += new System.EventHandler(this.btnSetColor);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.tabPage2.Controls.Add(this.checkMuteApp);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(368, 280);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Audio";
            // 
            // checkMuteApp
            // 
            this.checkMuteApp.AutoSize = true;
            this.checkMuteApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkMuteApp.ForeColor = System.Drawing.Color.White;
            this.checkMuteApp.Location = new System.Drawing.Point(40, 32);
            this.checkMuteApp.Name = "checkMuteApp";
            this.checkMuteApp.Size = new System.Drawing.Size(151, 19);
            this.checkMuteApp.TabIndex = 0;
            this.checkMuteApp.Text = "Mute application audio";
            this.checkMuteApp.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Controls.Add(this.panelSetKeybind);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.txtKeybindSearch);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(368, 280);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Key Binds";
            // 
            // panelSetKeybind
            // 
            this.panelSetKeybind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelSetKeybind.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSetKeybind.Controls.Add(this.btnSetKeybind);
            this.panelSetKeybind.Controls.Add(this.labelKeys);
            this.panelSetKeybind.Controls.Add(this.labelKeybindName);
            this.panelSetKeybind.Location = new System.Drawing.Point(87, 80);
            this.panelSetKeybind.Name = "panelSetKeybind";
            this.panelSetKeybind.Size = new System.Drawing.Size(190, 65);
            this.panelSetKeybind.TabIndex = 119;
            this.panelSetKeybind.Visible = false;
            // 
            // btnSetKeybind
            // 
            this.btnSetKeybind.BackColor = System.Drawing.Color.Green;
            this.btnSetKeybind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetKeybind.ForeColor = System.Drawing.Color.White;
            this.btnSetKeybind.Location = new System.Drawing.Point(60, 40);
            this.btnSetKeybind.Name = "btnSetKeybind";
            this.btnSetKeybind.Size = new System.Drawing.Size(75, 23);
            this.btnSetKeybind.TabIndex = 120;
            this.btnSetKeybind.Text = "Set";
            this.btnSetKeybind.UseVisualStyleBackColor = false;
            this.btnSetKeybind.Click += new System.EventHandler(this.btnSetKeybind_Click);
            // 
            // labelKeys
            // 
            this.labelKeys.AutoSize = true;
            this.labelKeys.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.labelKeys.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeys.ForeColor = System.Drawing.Color.PaleGreen;
            this.labelKeys.Location = new System.Drawing.Point(3, 17);
            this.labelKeys.MinimumSize = new System.Drawing.Size(180, 20);
            this.labelKeys.Name = "labelKeys";
            this.labelKeys.Size = new System.Drawing.Size(180, 20);
            this.labelKeys.TabIndex = 118;
            this.labelKeys.Tag = "1";
            this.labelKeys.Text = "--";
            this.labelKeys.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelKeybindName
            // 
            this.labelKeybindName.AutoSize = true;
            this.labelKeybindName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelKeybindName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeybindName.ForeColor = System.Drawing.Color.PaleGreen;
            this.labelKeybindName.Location = new System.Drawing.Point(3, 0);
            this.labelKeybindName.Name = "labelKeybindName";
            this.labelKeybindName.Size = new System.Drawing.Size(96, 15);
            this.labelKeybindName.TabIndex = 117;
            this.labelKeybindName.Tag = "1";
            this.labelKeybindName.Text = "Set Keybind - ";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.keybindMasterSaveAs);
            this.panel1.Controls.Add(this.keybindMasterSave);
            this.panel1.Controls.Add(this.keybindMasterOpen);
            this.panel1.Controls.Add(this.keybindMasterNew);
            this.panel1.Controls.Add(this.keybindGateSaveAs);
            this.panel1.Controls.Add(this.keybindGateSave);
            this.panel1.Controls.Add(this.keybindGateOpen);
            this.panel1.Controls.Add(this.keybindGateNew);
            this.panel1.Controls.Add(this.keybindLvlSaveAs);
            this.panel1.Controls.Add(this.keybindLvlSave);
            this.panel1.Controls.Add(this.keybindLvlOpen);
            this.panel1.Controls.Add(this.keybindLvlNew);
            this.panel1.Controls.Add(this.keybindLeafSaveAs);
            this.panel1.Controls.Add(this.keybindLeafSave);
            this.panel1.Controls.Add(this.keybindLeafOpen);
            this.panel1.Controls.Add(this.keybindLeafNew);
            this.panel1.Location = new System.Drawing.Point(1, 27);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(271, 218);
            this.panel1.TabIndex = 132;
            // 
            // keybindMasterSaveAs
            // 
            this.keybindMasterSaveAs.AutoSize = true;
            this.keybindMasterSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindMasterSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindMasterSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindMasterSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindMasterSaveAs.Location = new System.Drawing.Point(5, 270);
            this.keybindMasterSaveAs.Name = "keybindMasterSaveAs";
            this.keybindMasterSaveAs.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindMasterSaveAs.Size = new System.Drawing.Size(102, 18);
            this.keybindMasterSaveAs.TabIndex = 147;
            this.keybindMasterSaveAs.Tag = "mastersaveas";
            this.keybindMasterSaveAs.Text = "Master Save As";
            this.keybindMasterSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindMasterSave
            // 
            this.keybindMasterSave.AutoSize = true;
            this.keybindMasterSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindMasterSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindMasterSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindMasterSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindMasterSave.Location = new System.Drawing.Point(5, 252);
            this.keybindMasterSave.Name = "keybindMasterSave";
            this.keybindMasterSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindMasterSave.Size = new System.Drawing.Size(83, 18);
            this.keybindMasterSave.TabIndex = 146;
            this.keybindMasterSave.Tag = "mastersave";
            this.keybindMasterSave.Text = "Master Save";
            this.keybindMasterSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindMasterOpen
            // 
            this.keybindMasterOpen.AutoSize = true;
            this.keybindMasterOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindMasterOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindMasterOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindMasterOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindMasterOpen.Location = new System.Drawing.Point(5, 234);
            this.keybindMasterOpen.Name = "keybindMasterOpen";
            this.keybindMasterOpen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindMasterOpen.Size = new System.Drawing.Size(84, 18);
            this.keybindMasterOpen.TabIndex = 145;
            this.keybindMasterOpen.Tag = "masteropen";
            this.keybindMasterOpen.Text = "Master Open";
            this.keybindMasterOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindMasterNew
            // 
            this.keybindMasterNew.AutoSize = true;
            this.keybindMasterNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindMasterNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindMasterNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindMasterNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindMasterNew.Location = new System.Drawing.Point(5, 216);
            this.keybindMasterNew.Name = "keybindMasterNew";
            this.keybindMasterNew.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindMasterNew.Size = new System.Drawing.Size(78, 18);
            this.keybindMasterNew.TabIndex = 144;
            this.keybindMasterNew.Tag = "masternew";
            this.keybindMasterNew.Text = "Master New";
            this.keybindMasterNew.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindGateSaveAs
            // 
            this.keybindGateSaveAs.AutoSize = true;
            this.keybindGateSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindGateSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindGateSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindGateSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindGateSaveAs.Location = new System.Drawing.Point(5, 198);
            this.keybindGateSaveAs.Name = "keybindGateSaveAs";
            this.keybindGateSaveAs.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindGateSaveAs.Size = new System.Drawing.Size(90, 18);
            this.keybindGateSaveAs.TabIndex = 143;
            this.keybindGateSaveAs.Tag = "gatesaveas";
            this.keybindGateSaveAs.Text = "Gate Save As";
            this.keybindGateSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindGateSave
            // 
            this.keybindGateSave.AutoSize = true;
            this.keybindGateSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindGateSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindGateSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindGateSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindGateSave.Location = new System.Drawing.Point(5, 180);
            this.keybindGateSave.Name = "keybindGateSave";
            this.keybindGateSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindGateSave.Size = new System.Drawing.Size(71, 18);
            this.keybindGateSave.TabIndex = 142;
            this.keybindGateSave.Tag = "gatesave";
            this.keybindGateSave.Text = "Gate Save";
            this.keybindGateSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindGateOpen
            // 
            this.keybindGateOpen.AutoSize = true;
            this.keybindGateOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindGateOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindGateOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindGateOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindGateOpen.Location = new System.Drawing.Point(5, 162);
            this.keybindGateOpen.Name = "keybindGateOpen";
            this.keybindGateOpen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindGateOpen.Size = new System.Drawing.Size(72, 18);
            this.keybindGateOpen.TabIndex = 141;
            this.keybindGateOpen.Tag = "gateopen";
            this.keybindGateOpen.Text = "Gate Open";
            this.keybindGateOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindGateNew
            // 
            this.keybindGateNew.AutoSize = true;
            this.keybindGateNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindGateNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindGateNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindGateNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindGateNew.Location = new System.Drawing.Point(5, 144);
            this.keybindGateNew.Name = "keybindGateNew";
            this.keybindGateNew.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindGateNew.Size = new System.Drawing.Size(66, 18);
            this.keybindGateNew.TabIndex = 140;
            this.keybindGateNew.Tag = "gatenew";
            this.keybindGateNew.Text = "Gate New";
            this.keybindGateNew.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlSaveAs
            // 
            this.keybindLvlSaveAs.AutoSize = true;
            this.keybindLvlSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlSaveAs.Location = new System.Drawing.Point(5, 126);
            this.keybindLvlSaveAs.Name = "keybindLvlSaveAs";
            this.keybindLvlSaveAs.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLvlSaveAs.Size = new System.Drawing.Size(78, 18);
            this.keybindLvlSaveAs.TabIndex = 139;
            this.keybindLvlSaveAs.Tag = "lvlsaveas";
            this.keybindLvlSaveAs.Text = "Lvl Save As";
            this.keybindLvlSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlSave
            // 
            this.keybindLvlSave.AutoSize = true;
            this.keybindLvlSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlSave.Location = new System.Drawing.Point(5, 108);
            this.keybindLvlSave.Name = "keybindLvlSave";
            this.keybindLvlSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLvlSave.Size = new System.Drawing.Size(59, 18);
            this.keybindLvlSave.TabIndex = 138;
            this.keybindLvlSave.Tag = "lvlsave";
            this.keybindLvlSave.Text = "Lvl Save";
            this.keybindLvlSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlOpen
            // 
            this.keybindLvlOpen.AutoSize = true;
            this.keybindLvlOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlOpen.Location = new System.Drawing.Point(5, 90);
            this.keybindLvlOpen.Name = "keybindLvlOpen";
            this.keybindLvlOpen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLvlOpen.Size = new System.Drawing.Size(60, 18);
            this.keybindLvlOpen.TabIndex = 137;
            this.keybindLvlOpen.Tag = "lvlopen";
            this.keybindLvlOpen.Text = "Lvl Open";
            this.keybindLvlOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlNew
            // 
            this.keybindLvlNew.AutoSize = true;
            this.keybindLvlNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlNew.Location = new System.Drawing.Point(5, 72);
            this.keybindLvlNew.Name = "keybindLvlNew";
            this.keybindLvlNew.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLvlNew.Size = new System.Drawing.Size(54, 18);
            this.keybindLvlNew.TabIndex = 136;
            this.keybindLvlNew.Tag = "lvlnew";
            this.keybindLvlNew.Text = "Lvl New";
            this.keybindLvlNew.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafSaveAs
            // 
            this.keybindLeafSaveAs.AutoSize = true;
            this.keybindLeafSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafSaveAs.Location = new System.Drawing.Point(5, 54);
            this.keybindLeafSaveAs.Name = "keybindLeafSaveAs";
            this.keybindLeafSaveAs.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLeafSaveAs.Size = new System.Drawing.Size(87, 18);
            this.keybindLeafSaveAs.TabIndex = 135;
            this.keybindLeafSaveAs.Tag = "leafsaveas";
            this.keybindLeafSaveAs.Text = "Leaf Save As";
            this.keybindLeafSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafSave
            // 
            this.keybindLeafSave.AutoSize = true;
            this.keybindLeafSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafSave.Location = new System.Drawing.Point(5, 36);
            this.keybindLeafSave.Name = "keybindLeafSave";
            this.keybindLeafSave.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLeafSave.Size = new System.Drawing.Size(68, 18);
            this.keybindLeafSave.TabIndex = 134;
            this.keybindLeafSave.Tag = "leafsave";
            this.keybindLeafSave.Text = "Leaf Save";
            this.keybindLeafSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafOpen
            // 
            this.keybindLeafOpen.AutoSize = true;
            this.keybindLeafOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafOpen.Location = new System.Drawing.Point(5, 18);
            this.keybindLeafOpen.Name = "keybindLeafOpen";
            this.keybindLeafOpen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLeafOpen.Size = new System.Drawing.Size(69, 18);
            this.keybindLeafOpen.TabIndex = 133;
            this.keybindLeafOpen.Tag = "leafopen";
            this.keybindLeafOpen.Text = "Leaf Open";
            this.keybindLeafOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafNew
            // 
            this.keybindLeafNew.AutoSize = true;
            this.keybindLeafNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafNew.Location = new System.Drawing.Point(5, 0);
            this.keybindLeafNew.Name = "keybindLeafNew";
            this.keybindLeafNew.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.keybindLeafNew.Size = new System.Drawing.Size(63, 18);
            this.keybindLeafNew.TabIndex = 132;
            this.keybindLeafNew.Tag = "leafnew";
            this.keybindLeafNew.Text = "Leaf New";
            this.keybindLeafNew.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // txtKeybindSearch
            // 
            this.txtKeybindSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeybindSearch.Location = new System.Drawing.Point(22, 2);
            this.txtKeybindSearch.Name = "txtKeybindSearch";
            this.txtKeybindSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtKeybindSearch.Size = new System.Drawing.Size(122, 21);
            this.txtKeybindSearch.TabIndex = 148;
            this.txtKeybindSearch.Text = "search...";
            this.txtKeybindSearch.TextChanged += new System.EventHandler(this.txtKeybindSearch_TextChanged);
            this.txtKeybindSearch.Enter += new System.EventHandler(this.txtKeybindSearch_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_zoom;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 17);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 133;
            this.pictureBox1.TabStop = false;
            // 
            // CustomizeWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(367, 295);
            this.Controls.Add(this.toolstripCustomize);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomizeWorkspace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Customize Workspace";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomizeWorkspace_KeyDown);
            this.toolstripCustomize.ResumeLayout(false);
            this.toolstripCustomize.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panelSetKeybind.ResumeLayout(false);
            this.panelSetKeybind.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStrip toolstripCustomize;
        private System.Windows.Forms.ToolStripButton btnCustomizeApply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.Button btnActiveColor;
        public System.Windows.Forms.Button btnSampleColor;
        public System.Windows.Forms.Button btnLeafColor;
        public System.Windows.Forms.Button btnLvlColor;
        public System.Windows.Forms.Button btnGateColor;
        public System.Windows.Forms.Button btnMasterColor;
        public System.Windows.Forms.Button btnMenuColor;
        public System.Windows.Forms.Button btnBGColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox checkMuteApp;
        private System.Windows.Forms.ComboBox dropObjects;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox dropParamPath;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.Button btnObjectColor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panelSetKeybind;
        private System.Windows.Forms.Label labelKeybindName;
        private System.Windows.Forms.Label labelKeys;
        private System.Windows.Forms.Button btnSetKeybind;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label keybindMasterSaveAs;
        private System.Windows.Forms.Label keybindMasterSave;
        private System.Windows.Forms.Label keybindMasterOpen;
        private System.Windows.Forms.Label keybindMasterNew;
        private System.Windows.Forms.Label keybindGateSaveAs;
        private System.Windows.Forms.Label keybindGateSave;
        private System.Windows.Forms.Label keybindGateOpen;
        private System.Windows.Forms.Label keybindGateNew;
        private System.Windows.Forms.Label keybindLvlSaveAs;
        private System.Windows.Forms.Label keybindLvlSave;
        private System.Windows.Forms.Label keybindLvlOpen;
        private System.Windows.Forms.Label keybindLvlNew;
        private System.Windows.Forms.Label keybindLeafSaveAs;
        private System.Windows.Forms.Label keybindLeafSave;
        private System.Windows.Forms.Label keybindLeafOpen;
        private System.Windows.Forms.Label keybindLeafNew;
        private System.Windows.Forms.TextBox txtKeybindSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}