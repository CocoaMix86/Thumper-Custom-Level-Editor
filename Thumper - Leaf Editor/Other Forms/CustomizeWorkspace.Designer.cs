
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
            this.keybindLvlSaveAs = new System.Windows.Forms.Label();
            this.keybindLvlSave = new System.Windows.Forms.Label();
            this.keybindLvlOpen = new System.Windows.Forms.Label();
            this.keybindLvlNew = new System.Windows.Forms.Label();
            this.panelSetKeybind = new System.Windows.Forms.Panel();
            this.btnSetKeybind = new System.Windows.Forms.Button();
            this.labelKeys = new System.Windows.Forms.Label();
            this.labelKeybindName = new System.Windows.Forms.Label();
            this.keybindLeafSaveAs = new System.Windows.Forms.Label();
            this.keybindLeafSave = new System.Windows.Forms.Label();
            this.keybindLeafOpen = new System.Windows.Forms.Label();
            this.keybindLeafNew = new System.Windows.Forms.Label();
            this.toolstripCustomize.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panelSetKeybind.SuspendLayout();
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
            this.tabPage3.Controls.Add(this.keybindLvlSaveAs);
            this.tabPage3.Controls.Add(this.keybindLvlSave);
            this.tabPage3.Controls.Add(this.keybindLvlOpen);
            this.tabPage3.Controls.Add(this.keybindLvlNew);
            this.tabPage3.Controls.Add(this.panelSetKeybind);
            this.tabPage3.Controls.Add(this.keybindLeafSaveAs);
            this.tabPage3.Controls.Add(this.keybindLeafSave);
            this.tabPage3.Controls.Add(this.keybindLeafOpen);
            this.tabPage3.Controls.Add(this.keybindLeafNew);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(368, 280);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Key Binds";
            // 
            // keybindLvlSaveAs
            // 
            this.keybindLvlSaveAs.AutoSize = true;
            this.keybindLvlSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlSaveAs.Location = new System.Drawing.Point(3, 108);
            this.keybindLvlSaveAs.Name = "keybindLvlSaveAs";
            this.keybindLvlSaveAs.Size = new System.Drawing.Size(68, 15);
            this.keybindLvlSaveAs.TabIndex = 123;
            this.keybindLvlSaveAs.Tag = "lvlsaveas";
            this.keybindLvlSaveAs.Text = "Lvl Save As";
            this.keybindLvlSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlSave
            // 
            this.keybindLvlSave.AutoSize = true;
            this.keybindLvlSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlSave.Location = new System.Drawing.Point(3, 93);
            this.keybindLvlSave.Name = "keybindLvlSave";
            this.keybindLvlSave.Size = new System.Drawing.Size(52, 15);
            this.keybindLvlSave.TabIndex = 122;
            this.keybindLvlSave.Tag = "lvlsave";
            this.keybindLvlSave.Text = "Lvl Save";
            this.keybindLvlSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlOpen
            // 
            this.keybindLvlOpen.AutoSize = true;
            this.keybindLvlOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlOpen.Location = new System.Drawing.Point(3, 78);
            this.keybindLvlOpen.Name = "keybindLvlOpen";
            this.keybindLvlOpen.Size = new System.Drawing.Size(55, 15);
            this.keybindLvlOpen.TabIndex = 121;
            this.keybindLvlOpen.Tag = "lvlopen";
            this.keybindLvlOpen.Text = "Lvl Open";
            this.keybindLvlOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLvlNew
            // 
            this.keybindLvlNew.AutoSize = true;
            this.keybindLvlNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLvlNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLvlNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLvlNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLvlNew.Location = new System.Drawing.Point(3, 63);
            this.keybindLvlNew.Name = "keybindLvlNew";
            this.keybindLvlNew.Size = new System.Drawing.Size(50, 15);
            this.keybindLvlNew.TabIndex = 120;
            this.keybindLvlNew.Tag = "lvlnew";
            this.keybindLvlNew.Text = "Lvl New";
            this.keybindLvlNew.Click += new System.EventHandler(this.keybindLabel_Click);
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
            // keybindLeafSaveAs
            // 
            this.keybindLeafSaveAs.AutoSize = true;
            this.keybindLeafSaveAs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafSaveAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafSaveAs.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafSaveAs.Location = new System.Drawing.Point(3, 48);
            this.keybindLeafSaveAs.Name = "keybindLeafSaveAs";
            this.keybindLeafSaveAs.Size = new System.Drawing.Size(77, 15);
            this.keybindLeafSaveAs.TabIndex = 118;
            this.keybindLeafSaveAs.Tag = "leafsaveas";
            this.keybindLeafSaveAs.Text = "Leaf Save As";
            this.keybindLeafSaveAs.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafSave
            // 
            this.keybindLeafSave.AutoSize = true;
            this.keybindLeafSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafSave.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafSave.Location = new System.Drawing.Point(3, 33);
            this.keybindLeafSave.Name = "keybindLeafSave";
            this.keybindLeafSave.Size = new System.Drawing.Size(61, 15);
            this.keybindLeafSave.TabIndex = 117;
            this.keybindLeafSave.Tag = "leafsave";
            this.keybindLeafSave.Text = "Leaf Save";
            this.keybindLeafSave.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafOpen
            // 
            this.keybindLeafOpen.AutoSize = true;
            this.keybindLeafOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafOpen.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafOpen.Location = new System.Drawing.Point(3, 18);
            this.keybindLeafOpen.Name = "keybindLeafOpen";
            this.keybindLeafOpen.Size = new System.Drawing.Size(64, 15);
            this.keybindLeafOpen.TabIndex = 116;
            this.keybindLeafOpen.Tag = "leafopen";
            this.keybindLeafOpen.Text = "Leaf Open";
            this.keybindLeafOpen.Click += new System.EventHandler(this.keybindLabel_Click);
            // 
            // keybindLeafNew
            // 
            this.keybindLeafNew.AutoSize = true;
            this.keybindLeafNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keybindLeafNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.keybindLeafNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keybindLeafNew.ForeColor = System.Drawing.Color.Aqua;
            this.keybindLeafNew.Location = new System.Drawing.Point(3, 3);
            this.keybindLeafNew.Name = "keybindLeafNew";
            this.keybindLeafNew.Size = new System.Drawing.Size(59, 15);
            this.keybindLeafNew.TabIndex = 115;
            this.keybindLeafNew.Tag = "leafnew";
            this.keybindLeafNew.Text = "Leaf New";
            this.keybindLeafNew.Click += new System.EventHandler(this.keybindLabel_Click);
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
        private System.Windows.Forms.Label keybindLeafSaveAs;
        private System.Windows.Forms.Label keybindLeafSave;
        private System.Windows.Forms.Label keybindLeafOpen;
        private System.Windows.Forms.Label keybindLeafNew;
        private System.Windows.Forms.Panel panelSetKeybind;
        private System.Windows.Forms.Label labelKeybindName;
        private System.Windows.Forms.Label labelKeys;
        private System.Windows.Forms.Button btnSetKeybind;
        private System.Windows.Forms.Label keybindLvlSaveAs;
        private System.Windows.Forms.Label keybindLvlSave;
        private System.Windows.Forms.Label keybindLvlOpen;
        private System.Windows.Forms.Label keybindLvlNew;
    }
}