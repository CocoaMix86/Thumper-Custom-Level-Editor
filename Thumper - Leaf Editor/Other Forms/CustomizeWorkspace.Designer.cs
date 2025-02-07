
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeWorkspace));
            this.colorDialog1 = new ColorDialog();
            this.toolstripCustomize = new ToolStrip();
            this.btnCustomizeApply = new ToolStripButton();
            this.tabControl1 = new TabControl();
            this.tabUIColors = new TabPage();
            this.propertyGridUIColors = new PropertyGrid();
            this.tabSeq = new TabPage();
            this.treeObjects = new TreeViewEx();
            this.imageList1 = new ImageList(this.components);
            this.txtSearch = new TextBox();
            this.tabAudio = new TabPage();
            this.checkMuteApp = new CheckBox();
            this.tabKeybinds = new TabPage();
            this.btnKeybindReset = new Button();
            this.pictureBox1 = new PictureBox();
            this.panelSetKeybind = new Panel();
            this.btnCloseKeybind = new Button();
            this.btnSingleReset = new Button();
            this.btnSetKeybind = new Button();
            this.labelKeys = new Label();
            this.labelKeybindName = new Label();
            this.lblInvalid = new Label();
            this.panel1 = new Panel();
            this.keybindshowhidesample = new Label();
            this.keybindshowhidefolder = new Label();
            this.keybindshowmaster = new Label();
            this.keybindshowgate = new Label();
            this.keybindshowhidelvl = new Label();
            this.keybindshowhideleaf = new Label();
            this.keybindQuick9 = new Label();
            this.keybindQuick8 = new Label();
            this.keybindQuick7 = new Label();
            this.keybindQuick6 = new Label();
            this.keybindQuick5 = new Label();
            this.keybindQuick4 = new Label();
            this.keybindQuick3 = new Label();
            this.keybindQuick2 = new Label();
            this.keybindQuick1 = new Label();
            this.keybindToggleAutoPlace = new Label();
            this.keybindRandomizeRow = new Label();
            this.keybindSplitLeaf = new Label();
            this.keybindInterpolate = new Label();
            this.keybindColorDialog = new Label();
            this.keybindPreviousLeaf = new Label();
            this.keybindNextLeaf = new Label();
            this.keybindPreviousLvl = new Label();
            this.keybindNextLvl = new Label();
            this.keybindLevelExplorer = new Label();
            this.keybindLevelRecent = new Label();
            this.keybindLevelOpen = new Label();
            this.keybindLevelNew = new Label();
            this.keybindSaveAll = new Label();
            this.keybindSampleSaveAs = new Label();
            this.keybindSampleSave = new Label();
            this.keybindSampleOpen = new Label();
            this.keybindSampleNew = new Label();
            this.keybindMasterSaveAs = new Label();
            this.keybindMasterSave = new Label();
            this.keybindMasterOpen = new Label();
            this.keybindMasterNew = new Label();
            this.keybindGateSaveAs = new Label();
            this.keybindGateSave = new Label();
            this.keybindGateOpen = new Label();
            this.keybindGateNew = new Label();
            this.keybindLvlSaveAs = new Label();
            this.keybindLvlSave = new Label();
            this.keybindLvlOpen = new Label();
            this.keybindLvlNew = new Label();
            this.keybindTemplateOpen = new Label();
            this.keybindLeafUndo = new Label();
            this.keybindLeafSaveAs = new Label();
            this.keybindLeafSave = new Label();
            this.keybindLeafOpen = new Label();
            this.keybindLeafNew = new Label();
            this.txtKeybindSearch = new TextBox();
            this.toolTip1 = new ToolTip(this.components);
            this.toolstripCustomize.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabUIColors.SuspendLayout();
            this.tabSeq.SuspendLayout();
            this.tabAudio.SuspendLayout();
            this.tabKeybinds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
            this.panelSetKeybind.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // toolstripCustomize
            // 
            this.toolstripCustomize.BackColor = Color.FromArgb(40, 40, 40);
            this.toolstripCustomize.Dock = DockStyle.Bottom;
            this.toolstripCustomize.GripStyle = ToolStripGripStyle.Hidden;
            this.toolstripCustomize.Items.AddRange(new ToolStripItem[] { this.btnCustomizeApply });
            this.toolstripCustomize.Location = new Point(0, 345);
            this.toolstripCustomize.Name = "toolstripCustomize";
            this.toolstripCustomize.Size = new Size(367, 25);
            this.toolstripCustomize.TabIndex = 106;
            this.toolstripCustomize.Text = "toolStrip1";
            // 
            // btnCustomizeApply
            // 
            this.btnCustomizeApply.BackColor = Color.FromArgb(0, 192, 0);
            this.btnCustomizeApply.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnCustomizeApply.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnCustomizeApply.ForeColor = SystemColors.ControlText;
            this.btnCustomizeApply.Image = (Image)resources.GetObject("btnCustomizeApply.Image");
            this.btnCustomizeApply.ImageTransparentColor = Color.Magenta;
            this.btnCustomizeApply.Margin = new Padding(140, 1, 0, 2);
            this.btnCustomizeApply.Name = "btnCustomizeApply";
            this.btnCustomizeApply.Size = new Size(91, 22);
            this.btnCustomizeApply.Text = "Apply Changes";
            this.btnCustomizeApply.Click += this.btnCustomizeApply_Click;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabUIColors);
            this.tabControl1.Controls.Add(this.tabSeq);
            this.tabControl1.Controls.Add(this.tabAudio);
            this.tabControl1.Controls.Add(this.tabKeybinds);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(367, 345);
            this.tabControl1.TabIndex = 107;
            this.tabControl1.DrawItem += this.tabControl1_DrawItem;
            // 
            // tabUIColors
            // 
            this.tabUIColors.BackColor = Color.FromArgb(55, 55, 55);
            this.tabUIColors.Controls.Add(this.propertyGridUIColors);
            this.tabUIColors.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.tabUIColors.Location = new Point(4, 22);
            this.tabUIColors.Name = "tabUIColors";
            this.tabUIColors.Padding = new Padding(3);
            this.tabUIColors.Size = new Size(359, 319);
            this.tabUIColors.TabIndex = 0;
            this.tabUIColors.Text = "UI Theme";
            // 
            // propertyGridUIColors
            // 
            this.propertyGridUIColors.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.CategoryForeColor = Color.White;
            this.propertyGridUIColors.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridUIColors.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridUIColors.Dock = DockStyle.Fill;
            this.propertyGridUIColors.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridUIColors.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridUIColors.HelpForeColor = Color.White;
            this.propertyGridUIColors.HelpVisible = false;
            this.propertyGridUIColors.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridUIColors.Location = new Point(3, 3);
            this.propertyGridUIColors.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridUIColors.Name = "propertyGridUIColors";
            this.propertyGridUIColors.PropertySort = PropertySort.Categorized;
            this.propertyGridUIColors.RightToLeft = RightToLeft.No;
            this.propertyGridUIColors.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridUIColors.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridUIColors.Size = new Size(353, 313);
            this.propertyGridUIColors.TabIndex = 122;
            this.propertyGridUIColors.ToolbarVisible = false;
            this.propertyGridUIColors.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridUIColors.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridUIColors.ViewForeColor = Color.White;
            // 
            // tabSeq
            // 
            this.tabSeq.BackColor = Color.FromArgb(55, 55, 55);
            this.tabSeq.Controls.Add(this.treeObjects);
            this.tabSeq.Controls.Add(this.txtSearch);
            this.tabSeq.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tabSeq.Location = new Point(4, 22);
            this.tabSeq.Name = "tabSeq";
            this.tabSeq.Size = new Size(359, 319);
            this.tabSeq.TabIndex = 3;
            this.tabSeq.Text = "Sequencer Colors";
            // 
            // treeObjects
            // 
            this.treeObjects.BackColor = Color.FromArgb(31, 31, 31);
            this.treeObjects.BorderStyle = BorderStyle.None;
            this.treeObjects.Dock = DockStyle.Fill;
            this.treeObjects.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.treeObjects.ForeColor = Color.White;
            this.treeObjects.FullRowSelect = true;
            this.treeObjects.HideSelection = false;
            this.treeObjects.ImageKey = "other";
            this.treeObjects.ImageList = this.imageList1;
            this.treeObjects.ItemHeight = 16;
            this.treeObjects.LineColor = Color.White;
            this.treeObjects.Location = new Point(0, 22);
            this.treeObjects.Margin = new Padding(4, 3, 4, 3);
            this.treeObjects.Name = "treeObjects";
            this.treeObjects.SelectedImageKey = "other";
            this.treeObjects.ShowNodeToolTips = true;
            this.treeObjects.ShowRootLines = false;
            this.treeObjects.Size = new Size(359, 297);
            this.treeObjects.TabIndex = 101;
            this.treeObjects.NodeMouseDoubleClick += this.treeObjects_NodeMouseDoubleClick;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "category");
            this.imageList1.Images.SetKeyName(1, "none");
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = Color.FromArgb(56, 56, 56);
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearch.Dock = DockStyle.Top;
            this.txtSearch.ForeColor = Color.White;
            this.txtSearch.Location = new Point(0, 0);
            this.txtSearch.Margin = new Padding(4, 3, 4, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(359, 22);
            this.txtSearch.TabIndex = 100;
            this.txtSearch.Text = "Search Objects (Ctrl+;)";
            this.txtSearch.TextChanged += this.txtSearch_TextChanged;
            // 
            // tabAudio
            // 
            this.tabAudio.BackColor = Color.FromArgb(55, 55, 55);
            this.tabAudio.Controls.Add(this.checkMuteApp);
            this.tabAudio.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.tabAudio.Location = new Point(4, 22);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Padding = new Padding(3);
            this.tabAudio.Size = new Size(359, 319);
            this.tabAudio.TabIndex = 1;
            this.tabAudio.Text = "Audio";
            // 
            // checkMuteApp
            // 
            this.checkMuteApp.AutoSize = true;
            this.checkMuteApp.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.checkMuteApp.ForeColor = Color.White;
            this.checkMuteApp.Location = new Point(40, 32);
            this.checkMuteApp.Name = "checkMuteApp";
            this.checkMuteApp.Size = new Size(192, 19);
            this.checkMuteApp.TabIndex = 0;
            this.checkMuteApp.Text = "Mute application sound effects";
            this.checkMuteApp.UseVisualStyleBackColor = true;
            this.checkMuteApp.CheckedChanged += this.checkMuteApp_CheckedChanged;
            // 
            // tabKeybinds
            // 
            this.tabKeybinds.BackColor = Color.FromArgb(55, 55, 55);
            this.tabKeybinds.Controls.Add(this.btnKeybindReset);
            this.tabKeybinds.Controls.Add(this.pictureBox1);
            this.tabKeybinds.Controls.Add(this.panelSetKeybind);
            this.tabKeybinds.Controls.Add(this.panel1);
            this.tabKeybinds.Controls.Add(this.txtKeybindSearch);
            this.tabKeybinds.Location = new Point(4, 22);
            this.tabKeybinds.Name = "tabKeybinds";
            this.tabKeybinds.Padding = new Padding(3);
            this.tabKeybinds.Size = new Size(359, 319);
            this.tabKeybinds.TabIndex = 2;
            this.tabKeybinds.Text = "Key Binds";
            // 
            // btnKeybindReset
            // 
            this.btnKeybindReset.BackColor = Color.Orange;
            this.btnKeybindReset.FlatStyle = FlatStyle.Flat;
            this.btnKeybindReset.ForeColor = Color.Black;
            this.btnKeybindReset.Location = new Point(255, 2);
            this.btnKeybindReset.Name = "btnKeybindReset";
            this.btnKeybindReset.Size = new Size(111, 23);
            this.btnKeybindReset.TabIndex = 149;
            this.btnKeybindReset.Text = "Reset to Defaults";
            this.btnKeybindReset.UseVisualStyleBackColor = false;
            this.btnKeybindReset.Click += this.btnKeybindReset_Click;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = Properties.Resources.icon_zoom;
            this.pictureBox1.Location = new Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(17, 17);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 133;
            this.pictureBox1.TabStop = false;
            // 
            // panelSetKeybind
            // 
            this.panelSetKeybind.BackColor = Color.FromArgb(40, 40, 40);
            this.panelSetKeybind.BorderStyle = BorderStyle.Fixed3D;
            this.panelSetKeybind.Controls.Add(this.btnCloseKeybind);
            this.panelSetKeybind.Controls.Add(this.btnSingleReset);
            this.panelSetKeybind.Controls.Add(this.btnSetKeybind);
            this.panelSetKeybind.Controls.Add(this.labelKeys);
            this.panelSetKeybind.Controls.Add(this.labelKeybindName);
            this.panelSetKeybind.Controls.Add(this.lblInvalid);
            this.panelSetKeybind.Location = new Point(87, 80);
            this.panelSetKeybind.Name = "panelSetKeybind";
            this.panelSetKeybind.Size = new Size(190, 97);
            this.panelSetKeybind.TabIndex = 119;
            this.panelSetKeybind.Visible = false;
            // 
            // btnCloseKeybind
            // 
            this.btnCloseKeybind.Cursor = Cursors.Hand;
            this.btnCloseKeybind.FlatStyle = FlatStyle.Flat;
            this.btnCloseKeybind.Image = Properties.Resources.icon_remove2;
            this.btnCloseKeybind.Location = new Point(168, 0);
            this.btnCloseKeybind.Name = "btnCloseKeybind";
            this.btnCloseKeybind.Size = new Size(16, 16);
            this.btnCloseKeybind.TabIndex = 150;
            this.btnCloseKeybind.UseVisualStyleBackColor = true;
            this.btnCloseKeybind.Click += this.btnCloseKeybind_Click;
            // 
            // btnSingleReset
            // 
            this.btnSingleReset.BackColor = Color.Orange;
            this.btnSingleReset.FlatStyle = FlatStyle.Flat;
            this.btnSingleReset.ForeColor = Color.Black;
            this.btnSingleReset.Location = new Point(99, 65);
            this.btnSingleReset.Name = "btnSingleReset";
            this.btnSingleReset.Size = new Size(75, 23);
            this.btnSingleReset.TabIndex = 150;
            this.btnSingleReset.Text = "Reset";
            this.btnSingleReset.UseVisualStyleBackColor = false;
            this.btnSingleReset.Click += this.btnSingleReset_Click;
            // 
            // btnSetKeybind
            // 
            this.btnSetKeybind.BackColor = Color.Gray;
            this.btnSetKeybind.Enabled = false;
            this.btnSetKeybind.FlatStyle = FlatStyle.Flat;
            this.btnSetKeybind.ForeColor = Color.White;
            this.btnSetKeybind.Location = new Point(24, 65);
            this.btnSetKeybind.Name = "btnSetKeybind";
            this.btnSetKeybind.Size = new Size(75, 23);
            this.btnSetKeybind.TabIndex = 120;
            this.btnSetKeybind.Text = "Set";
            this.btnSetKeybind.UseVisualStyleBackColor = false;
            this.btnSetKeybind.Click += this.btnSetKeybind_Click;
            // 
            // labelKeys
            // 
            this.labelKeys.AutoSize = true;
            this.labelKeys.BackColor = Color.FromArgb(20, 20, 20);
            this.labelKeys.Cursor = Cursors.Hand;
            this.labelKeys.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelKeys.ForeColor = Color.PaleGreen;
            this.labelKeys.Location = new Point(3, 17);
            this.labelKeys.MinimumSize = new Size(180, 20);
            this.labelKeys.Name = "labelKeys";
            this.labelKeys.Size = new Size(180, 20);
            this.labelKeys.TabIndex = 118;
            this.labelKeys.Tag = "1";
            this.labelKeys.Text = "--";
            this.labelKeys.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelKeybindName
            // 
            this.labelKeybindName.AutoSize = true;
            this.labelKeybindName.Cursor = Cursors.Hand;
            this.labelKeybindName.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelKeybindName.ForeColor = Color.PaleGreen;
            this.labelKeybindName.Location = new Point(3, 0);
            this.labelKeybindName.Name = "labelKeybindName";
            this.labelKeybindName.Size = new Size(96, 15);
            this.labelKeybindName.TabIndex = 117;
            this.labelKeybindName.Tag = "1";
            this.labelKeybindName.Text = "Set Keybind - ";
            // 
            // lblInvalid
            // 
            this.lblInvalid.AutoSize = true;
            this.lblInvalid.Cursor = Cursors.Hand;
            this.lblInvalid.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblInvalid.ForeColor = Color.Red;
            this.lblInvalid.Location = new Point(6, 36);
            this.lblInvalid.Name = "lblInvalid";
            this.lblInvalid.Size = new Size(173, 26);
            this.lblInvalid.TabIndex = 151;
            this.lblInvalid.Tag = "1";
            this.lblInvalid.Text = "that keybind is in use already\r\nor is invalid\r\n";
            this.lblInvalid.TextAlign = ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblInvalid, "Due to Windows limitations, all keybinds MUST\r\nhave at least 1 modifier (CTRL, ALT, Shift).");
            this.lblInvalid.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.keybindshowhidesample);
            this.panel1.Controls.Add(this.keybindshowhidefolder);
            this.panel1.Controls.Add(this.keybindshowmaster);
            this.panel1.Controls.Add(this.keybindshowgate);
            this.panel1.Controls.Add(this.keybindshowhidelvl);
            this.panel1.Controls.Add(this.keybindshowhideleaf);
            this.panel1.Controls.Add(this.keybindQuick9);
            this.panel1.Controls.Add(this.keybindQuick8);
            this.panel1.Controls.Add(this.keybindQuick7);
            this.panel1.Controls.Add(this.keybindQuick6);
            this.panel1.Controls.Add(this.keybindQuick5);
            this.panel1.Controls.Add(this.keybindQuick4);
            this.panel1.Controls.Add(this.keybindQuick3);
            this.panel1.Controls.Add(this.keybindQuick2);
            this.panel1.Controls.Add(this.keybindQuick1);
            this.panel1.Controls.Add(this.keybindToggleAutoPlace);
            this.panel1.Controls.Add(this.keybindRandomizeRow);
            this.panel1.Controls.Add(this.keybindSplitLeaf);
            this.panel1.Controls.Add(this.keybindInterpolate);
            this.panel1.Controls.Add(this.keybindColorDialog);
            this.panel1.Controls.Add(this.keybindPreviousLeaf);
            this.panel1.Controls.Add(this.keybindNextLeaf);
            this.panel1.Controls.Add(this.keybindPreviousLvl);
            this.panel1.Controls.Add(this.keybindNextLvl);
            this.panel1.Controls.Add(this.keybindLevelExplorer);
            this.panel1.Controls.Add(this.keybindLevelRecent);
            this.panel1.Controls.Add(this.keybindLevelOpen);
            this.panel1.Controls.Add(this.keybindLevelNew);
            this.panel1.Controls.Add(this.keybindSaveAll);
            this.panel1.Controls.Add(this.keybindSampleSaveAs);
            this.panel1.Controls.Add(this.keybindSampleSave);
            this.panel1.Controls.Add(this.keybindSampleOpen);
            this.panel1.Controls.Add(this.keybindSampleNew);
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
            this.panel1.Controls.Add(this.keybindTemplateOpen);
            this.panel1.Controls.Add(this.keybindLeafUndo);
            this.panel1.Controls.Add(this.keybindLeafSaveAs);
            this.panel1.Controls.Add(this.keybindLeafSave);
            this.panel1.Controls.Add(this.keybindLeafOpen);
            this.panel1.Controls.Add(this.keybindLeafNew);
            this.panel1.Location = new Point(1, 27);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new Padding(5, 0, 0, 0);
            this.panel1.RightToLeft = RightToLeft.Yes;
            this.panel1.Size = new Size(364, 293);
            this.panel1.TabIndex = 132;
            // 
            // keybindshowhidesample
            // 
            this.keybindshowhidesample.AutoSize = true;
            this.keybindshowhidesample.Cursor = Cursors.Hand;
            this.keybindshowhidesample.Dock = DockStyle.Top;
            this.keybindshowhidesample.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowhidesample.ForeColor = Color.Aqua;
            this.keybindshowhidesample.Location = new Point(5, 850);
            this.keybindshowhidesample.Name = "keybindshowhidesample";
            this.keybindshowhidesample.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowhidesample.RightToLeft = RightToLeft.No;
            this.keybindshowhidesample.Size = new Size(119, 17);
            this.keybindshowhidesample.TabIndex = 182;
            this.keybindshowhidesample.Tag = "showhidesample";
            this.keybindshowhidesample.Text = "Show/Hide Sample";
            this.keybindshowhidesample.Click += this.keybindLabel_Click;
            // 
            // keybindshowhidefolder
            // 
            this.keybindshowhidefolder.AutoSize = true;
            this.keybindshowhidefolder.Cursor = Cursors.Hand;
            this.keybindshowhidefolder.Dock = DockStyle.Top;
            this.keybindshowhidefolder.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowhidefolder.ForeColor = Color.Aqua;
            this.keybindshowhidefolder.Location = new Point(5, 833);
            this.keybindshowhidefolder.Name = "keybindshowhidefolder";
            this.keybindshowhidefolder.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowhidefolder.RightToLeft = RightToLeft.No;
            this.keybindshowhidefolder.Size = new Size(119, 17);
            this.keybindshowhidefolder.TabIndex = 181;
            this.keybindshowhidefolder.Tag = "showhidefolder";
            this.keybindshowhidefolder.Text = "Show/Hide Folder";
            this.keybindshowhidefolder.Click += this.keybindLabel_Click;
            // 
            // keybindshowmaster
            // 
            this.keybindshowmaster.AutoSize = true;
            this.keybindshowmaster.Cursor = Cursors.Hand;
            this.keybindshowmaster.Dock = DockStyle.Top;
            this.keybindshowmaster.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowmaster.ForeColor = Color.Aqua;
            this.keybindshowmaster.Location = new Point(5, 816);
            this.keybindshowmaster.Name = "keybindshowmaster";
            this.keybindshowmaster.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowmaster.RightToLeft = RightToLeft.No;
            this.keybindshowmaster.Size = new Size(119, 17);
            this.keybindshowmaster.TabIndex = 180;
            this.keybindshowmaster.Tag = "showhidemaster";
            this.keybindshowmaster.Text = "Show/Hide Master";
            this.keybindshowmaster.Click += this.keybindLabel_Click;
            // 
            // keybindshowgate
            // 
            this.keybindshowgate.AutoSize = true;
            this.keybindshowgate.Cursor = Cursors.Hand;
            this.keybindshowgate.Dock = DockStyle.Top;
            this.keybindshowgate.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowgate.ForeColor = Color.Aqua;
            this.keybindshowgate.Location = new Point(5, 799);
            this.keybindshowgate.Name = "keybindshowgate";
            this.keybindshowgate.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowgate.RightToLeft = RightToLeft.No;
            this.keybindshowgate.Size = new Size(105, 17);
            this.keybindshowgate.TabIndex = 179;
            this.keybindshowgate.Tag = "showhidegate";
            this.keybindshowgate.Text = "Show/Hide Gate";
            this.keybindshowgate.Click += this.keybindLabel_Click;
            // 
            // keybindshowhidelvl
            // 
            this.keybindshowhidelvl.AutoSize = true;
            this.keybindshowhidelvl.Cursor = Cursors.Hand;
            this.keybindshowhidelvl.Dock = DockStyle.Top;
            this.keybindshowhidelvl.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowhidelvl.ForeColor = Color.Aqua;
            this.keybindshowhidelvl.Location = new Point(5, 782);
            this.keybindshowhidelvl.Name = "keybindshowhidelvl";
            this.keybindshowhidelvl.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowhidelvl.RightToLeft = RightToLeft.No;
            this.keybindshowhidelvl.Size = new Size(98, 17);
            this.keybindshowhidelvl.TabIndex = 178;
            this.keybindshowhidelvl.Tag = "showhidelvl";
            this.keybindshowhidelvl.Text = "Show/Hide Lvl";
            this.keybindshowhidelvl.Click += this.keybindLabel_Click;
            // 
            // keybindshowhideleaf
            // 
            this.keybindshowhideleaf.AutoSize = true;
            this.keybindshowhideleaf.Cursor = Cursors.Hand;
            this.keybindshowhideleaf.Dock = DockStyle.Top;
            this.keybindshowhideleaf.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindshowhideleaf.ForeColor = Color.Aqua;
            this.keybindshowhideleaf.Location = new Point(5, 765);
            this.keybindshowhideleaf.Name = "keybindshowhideleaf";
            this.keybindshowhideleaf.Padding = new Padding(0, 0, 0, 2);
            this.keybindshowhideleaf.RightToLeft = RightToLeft.No;
            this.keybindshowhideleaf.Size = new Size(105, 17);
            this.keybindshowhideleaf.TabIndex = 177;
            this.keybindshowhideleaf.Tag = "showhideleaf";
            this.keybindshowhideleaf.Text = "Show/Hide Leaf";
            this.keybindshowhideleaf.Click += this.keybindLabel_Click;
            // 
            // keybindQuick9
            // 
            this.keybindQuick9.AutoSize = true;
            this.keybindQuick9.Cursor = Cursors.Hand;
            this.keybindQuick9.Dock = DockStyle.Top;
            this.keybindQuick9.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick9.ForeColor = Color.Aqua;
            this.keybindQuick9.Location = new Point(5, 748);
            this.keybindQuick9.Name = "keybindQuick9";
            this.keybindQuick9.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick9.RightToLeft = RightToLeft.No;
            this.keybindQuick9.Size = new Size(105, 17);
            this.keybindQuick9.TabIndex = 174;
            this.keybindQuick9.Tag = "quick9";
            this.keybindQuick9.Text = "Quick Insert 9";
            this.keybindQuick9.Click += this.keybindLabel_Click;
            // 
            // keybindQuick8
            // 
            this.keybindQuick8.AutoSize = true;
            this.keybindQuick8.Cursor = Cursors.Hand;
            this.keybindQuick8.Dock = DockStyle.Top;
            this.keybindQuick8.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick8.ForeColor = Color.Aqua;
            this.keybindQuick8.Location = new Point(5, 731);
            this.keybindQuick8.Name = "keybindQuick8";
            this.keybindQuick8.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick8.RightToLeft = RightToLeft.No;
            this.keybindQuick8.Size = new Size(105, 17);
            this.keybindQuick8.TabIndex = 173;
            this.keybindQuick8.Tag = "quick8";
            this.keybindQuick8.Text = "Quick Insert 8";
            this.keybindQuick8.Click += this.keybindLabel_Click;
            // 
            // keybindQuick7
            // 
            this.keybindQuick7.AutoSize = true;
            this.keybindQuick7.Cursor = Cursors.Hand;
            this.keybindQuick7.Dock = DockStyle.Top;
            this.keybindQuick7.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick7.ForeColor = Color.Aqua;
            this.keybindQuick7.Location = new Point(5, 714);
            this.keybindQuick7.Name = "keybindQuick7";
            this.keybindQuick7.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick7.RightToLeft = RightToLeft.No;
            this.keybindQuick7.Size = new Size(105, 17);
            this.keybindQuick7.TabIndex = 172;
            this.keybindQuick7.Tag = "quick7";
            this.keybindQuick7.Text = "Quick Insert 7";
            this.keybindQuick7.Click += this.keybindLabel_Click;
            // 
            // keybindQuick6
            // 
            this.keybindQuick6.AutoSize = true;
            this.keybindQuick6.Cursor = Cursors.Hand;
            this.keybindQuick6.Dock = DockStyle.Top;
            this.keybindQuick6.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick6.ForeColor = Color.Aqua;
            this.keybindQuick6.Location = new Point(5, 697);
            this.keybindQuick6.Name = "keybindQuick6";
            this.keybindQuick6.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick6.RightToLeft = RightToLeft.No;
            this.keybindQuick6.Size = new Size(105, 17);
            this.keybindQuick6.TabIndex = 171;
            this.keybindQuick6.Tag = "quick6";
            this.keybindQuick6.Text = "Quick Insert 6";
            this.keybindQuick6.Click += this.keybindLabel_Click;
            // 
            // keybindQuick5
            // 
            this.keybindQuick5.AutoSize = true;
            this.keybindQuick5.Cursor = Cursors.Hand;
            this.keybindQuick5.Dock = DockStyle.Top;
            this.keybindQuick5.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick5.ForeColor = Color.Aqua;
            this.keybindQuick5.Location = new Point(5, 680);
            this.keybindQuick5.Name = "keybindQuick5";
            this.keybindQuick5.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick5.RightToLeft = RightToLeft.No;
            this.keybindQuick5.Size = new Size(105, 17);
            this.keybindQuick5.TabIndex = 170;
            this.keybindQuick5.Tag = "quick5";
            this.keybindQuick5.Text = "Quick Insert 5";
            this.keybindQuick5.Click += this.keybindLabel_Click;
            // 
            // keybindQuick4
            // 
            this.keybindQuick4.AutoSize = true;
            this.keybindQuick4.Cursor = Cursors.Hand;
            this.keybindQuick4.Dock = DockStyle.Top;
            this.keybindQuick4.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick4.ForeColor = Color.Aqua;
            this.keybindQuick4.Location = new Point(5, 663);
            this.keybindQuick4.Name = "keybindQuick4";
            this.keybindQuick4.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick4.RightToLeft = RightToLeft.No;
            this.keybindQuick4.Size = new Size(105, 17);
            this.keybindQuick4.TabIndex = 169;
            this.keybindQuick4.Tag = "quick4";
            this.keybindQuick4.Text = "Quick Insert 4";
            this.keybindQuick4.Click += this.keybindLabel_Click;
            // 
            // keybindQuick3
            // 
            this.keybindQuick3.AutoSize = true;
            this.keybindQuick3.Cursor = Cursors.Hand;
            this.keybindQuick3.Dock = DockStyle.Top;
            this.keybindQuick3.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick3.ForeColor = Color.Aqua;
            this.keybindQuick3.Location = new Point(5, 646);
            this.keybindQuick3.Name = "keybindQuick3";
            this.keybindQuick3.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick3.RightToLeft = RightToLeft.No;
            this.keybindQuick3.Size = new Size(105, 17);
            this.keybindQuick3.TabIndex = 168;
            this.keybindQuick3.Tag = "quick3";
            this.keybindQuick3.Text = "Quick Insert 3";
            this.keybindQuick3.Click += this.keybindLabel_Click;
            // 
            // keybindQuick2
            // 
            this.keybindQuick2.AutoSize = true;
            this.keybindQuick2.Cursor = Cursors.Hand;
            this.keybindQuick2.Dock = DockStyle.Top;
            this.keybindQuick2.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick2.ForeColor = Color.Aqua;
            this.keybindQuick2.Location = new Point(5, 629);
            this.keybindQuick2.Name = "keybindQuick2";
            this.keybindQuick2.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick2.RightToLeft = RightToLeft.No;
            this.keybindQuick2.Size = new Size(105, 17);
            this.keybindQuick2.TabIndex = 167;
            this.keybindQuick2.Tag = "quick2";
            this.keybindQuick2.Text = "Quick Insert 2";
            this.keybindQuick2.Click += this.keybindLabel_Click;
            // 
            // keybindQuick1
            // 
            this.keybindQuick1.AutoSize = true;
            this.keybindQuick1.Cursor = Cursors.Hand;
            this.keybindQuick1.Dock = DockStyle.Top;
            this.keybindQuick1.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindQuick1.ForeColor = Color.Aqua;
            this.keybindQuick1.Location = new Point(5, 612);
            this.keybindQuick1.Name = "keybindQuick1";
            this.keybindQuick1.Padding = new Padding(0, 0, 0, 2);
            this.keybindQuick1.RightToLeft = RightToLeft.No;
            this.keybindQuick1.Size = new Size(105, 17);
            this.keybindQuick1.TabIndex = 166;
            this.keybindQuick1.Tag = "quick1";
            this.keybindQuick1.Text = "Quick Insert 1";
            this.keybindQuick1.Click += this.keybindLabel_Click;
            // 
            // keybindToggleAutoPlace
            // 
            this.keybindToggleAutoPlace.AutoSize = true;
            this.keybindToggleAutoPlace.Cursor = Cursors.Hand;
            this.keybindToggleAutoPlace.Dock = DockStyle.Top;
            this.keybindToggleAutoPlace.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindToggleAutoPlace.ForeColor = Color.Aqua;
            this.keybindToggleAutoPlace.Location = new Point(5, 595);
            this.keybindToggleAutoPlace.Name = "keybindToggleAutoPlace";
            this.keybindToggleAutoPlace.Padding = new Padding(0, 0, 0, 2);
            this.keybindToggleAutoPlace.RightToLeft = RightToLeft.No;
            this.keybindToggleAutoPlace.Size = new Size(126, 17);
            this.keybindToggleAutoPlace.TabIndex = 165;
            this.keybindToggleAutoPlace.Tag = "toggleautoplace";
            this.keybindToggleAutoPlace.Text = "Toggle Auto Place";
            this.keybindToggleAutoPlace.Click += this.keybindLabel_Click;
            // 
            // keybindRandomizeRow
            // 
            this.keybindRandomizeRow.AutoSize = true;
            this.keybindRandomizeRow.Cursor = Cursors.Hand;
            this.keybindRandomizeRow.Dock = DockStyle.Top;
            this.keybindRandomizeRow.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindRandomizeRow.ForeColor = Color.Aqua;
            this.keybindRandomizeRow.Location = new Point(5, 578);
            this.keybindRandomizeRow.Name = "keybindRandomizeRow";
            this.keybindRandomizeRow.Padding = new Padding(0, 0, 0, 2);
            this.keybindRandomizeRow.RightToLeft = RightToLeft.No;
            this.keybindRandomizeRow.Size = new Size(98, 17);
            this.keybindRandomizeRow.TabIndex = 164;
            this.keybindRandomizeRow.Tag = "randomizerow";
            this.keybindRandomizeRow.Text = "Randomize Row";
            this.keybindRandomizeRow.Click += this.keybindLabel_Click;
            // 
            // keybindSplitLeaf
            // 
            this.keybindSplitLeaf.AutoSize = true;
            this.keybindSplitLeaf.Cursor = Cursors.Hand;
            this.keybindSplitLeaf.Dock = DockStyle.Top;
            this.keybindSplitLeaf.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSplitLeaf.ForeColor = Color.Aqua;
            this.keybindSplitLeaf.Location = new Point(5, 561);
            this.keybindSplitLeaf.Name = "keybindSplitLeaf";
            this.keybindSplitLeaf.Padding = new Padding(0, 0, 0, 2);
            this.keybindSplitLeaf.RightToLeft = RightToLeft.No;
            this.keybindSplitLeaf.Size = new Size(77, 17);
            this.keybindSplitLeaf.TabIndex = 163;
            this.keybindSplitLeaf.Tag = "splitleaf";
            this.keybindSplitLeaf.Text = "Split Leaf";
            this.keybindSplitLeaf.Click += this.keybindLabel_Click;
            // 
            // keybindInterpolate
            // 
            this.keybindInterpolate.AutoSize = true;
            this.keybindInterpolate.Cursor = Cursors.Hand;
            this.keybindInterpolate.Dock = DockStyle.Top;
            this.keybindInterpolate.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindInterpolate.ForeColor = Color.Aqua;
            this.keybindInterpolate.Location = new Point(5, 544);
            this.keybindInterpolate.Name = "keybindInterpolate";
            this.keybindInterpolate.Padding = new Padding(0, 0, 0, 2);
            this.keybindInterpolate.RightToLeft = RightToLeft.No;
            this.keybindInterpolate.Size = new Size(84, 17);
            this.keybindInterpolate.TabIndex = 162;
            this.keybindInterpolate.Tag = "interpolate";
            this.keybindInterpolate.Text = "Interpolate";
            this.keybindInterpolate.Click += this.keybindLabel_Click;
            // 
            // keybindColorDialog
            // 
            this.keybindColorDialog.AutoSize = true;
            this.keybindColorDialog.Cursor = Cursors.Hand;
            this.keybindColorDialog.Dock = DockStyle.Top;
            this.keybindColorDialog.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindColorDialog.ForeColor = Color.Aqua;
            this.keybindColorDialog.Location = new Point(5, 527);
            this.keybindColorDialog.Name = "keybindColorDialog";
            this.keybindColorDialog.Padding = new Padding(0, 0, 0, 2);
            this.keybindColorDialog.RightToLeft = RightToLeft.No;
            this.keybindColorDialog.Size = new Size(91, 17);
            this.keybindColorDialog.TabIndex = 161;
            this.keybindColorDialog.Tag = "colordialog";
            this.keybindColorDialog.Text = "Color Picker";
            this.keybindColorDialog.Click += this.keybindLabel_Click;
            // 
            // keybindPreviousLeaf
            // 
            this.keybindPreviousLeaf.AutoSize = true;
            this.keybindPreviousLeaf.Cursor = Cursors.Hand;
            this.keybindPreviousLeaf.Dock = DockStyle.Top;
            this.keybindPreviousLeaf.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindPreviousLeaf.ForeColor = Color.Aqua;
            this.keybindPreviousLeaf.Location = new Point(5, 510);
            this.keybindPreviousLeaf.Name = "keybindPreviousLeaf";
            this.keybindPreviousLeaf.Padding = new Padding(0, 0, 0, 2);
            this.keybindPreviousLeaf.RightToLeft = RightToLeft.No;
            this.keybindPreviousLeaf.Size = new Size(98, 17);
            this.keybindPreviousLeaf.TabIndex = 160;
            this.keybindPreviousLeaf.Tag = "previousleaf";
            this.keybindPreviousLeaf.Text = "Previous Leaf";
            this.keybindPreviousLeaf.Click += this.keybindLabel_Click;
            // 
            // keybindNextLeaf
            // 
            this.keybindNextLeaf.AutoSize = true;
            this.keybindNextLeaf.Cursor = Cursors.Hand;
            this.keybindNextLeaf.Dock = DockStyle.Top;
            this.keybindNextLeaf.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindNextLeaf.ForeColor = Color.Aqua;
            this.keybindNextLeaf.Location = new Point(5, 493);
            this.keybindNextLeaf.Name = "keybindNextLeaf";
            this.keybindNextLeaf.Padding = new Padding(0, 0, 0, 2);
            this.keybindNextLeaf.RightToLeft = RightToLeft.No;
            this.keybindNextLeaf.Size = new Size(70, 17);
            this.keybindNextLeaf.TabIndex = 159;
            this.keybindNextLeaf.Tag = "nextleaf";
            this.keybindNextLeaf.Text = "Next Leaf";
            this.keybindNextLeaf.Click += this.keybindLabel_Click;
            // 
            // keybindPreviousLvl
            // 
            this.keybindPreviousLvl.AutoSize = true;
            this.keybindPreviousLvl.Cursor = Cursors.Hand;
            this.keybindPreviousLvl.Dock = DockStyle.Top;
            this.keybindPreviousLvl.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindPreviousLvl.ForeColor = Color.Aqua;
            this.keybindPreviousLvl.Location = new Point(5, 476);
            this.keybindPreviousLvl.Name = "keybindPreviousLvl";
            this.keybindPreviousLvl.Padding = new Padding(0, 0, 0, 2);
            this.keybindPreviousLvl.RightToLeft = RightToLeft.No;
            this.keybindPreviousLvl.Size = new Size(91, 17);
            this.keybindPreviousLvl.TabIndex = 158;
            this.keybindPreviousLvl.Tag = "previouslvl";
            this.keybindPreviousLvl.Text = "Previous Lvl";
            this.keybindPreviousLvl.Click += this.keybindLabel_Click;
            // 
            // keybindNextLvl
            // 
            this.keybindNextLvl.AutoSize = true;
            this.keybindNextLvl.Cursor = Cursors.Hand;
            this.keybindNextLvl.Dock = DockStyle.Top;
            this.keybindNextLvl.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindNextLvl.ForeColor = Color.Aqua;
            this.keybindNextLvl.Location = new Point(5, 459);
            this.keybindNextLvl.Name = "keybindNextLvl";
            this.keybindNextLvl.Padding = new Padding(0, 0, 0, 2);
            this.keybindNextLvl.RightToLeft = RightToLeft.No;
            this.keybindNextLvl.Size = new Size(63, 17);
            this.keybindNextLvl.TabIndex = 157;
            this.keybindNextLvl.Tag = "nextlvl";
            this.keybindNextLvl.Text = "Next Lvl";
            this.keybindNextLvl.Click += this.keybindLabel_Click;
            // 
            // keybindLevelExplorer
            // 
            this.keybindLevelExplorer.AutoSize = true;
            this.keybindLevelExplorer.Cursor = Cursors.Hand;
            this.keybindLevelExplorer.Dock = DockStyle.Top;
            this.keybindLevelExplorer.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLevelExplorer.ForeColor = Color.Aqua;
            this.keybindLevelExplorer.Location = new Point(5, 442);
            this.keybindLevelExplorer.Name = "keybindLevelExplorer";
            this.keybindLevelExplorer.Padding = new Padding(0, 0, 0, 2);
            this.keybindLevelExplorer.RightToLeft = RightToLeft.No;
            this.keybindLevelExplorer.Size = new Size(105, 17);
            this.keybindLevelExplorer.TabIndex = 156;
            this.keybindLevelExplorer.Tag = "levelexplorer";
            this.keybindLevelExplorer.Text = "Level Explorer";
            this.keybindLevelExplorer.Click += this.keybindLabel_Click;
            // 
            // keybindLevelRecent
            // 
            this.keybindLevelRecent.AutoSize = true;
            this.keybindLevelRecent.Cursor = Cursors.Hand;
            this.keybindLevelRecent.Dock = DockStyle.Top;
            this.keybindLevelRecent.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLevelRecent.ForeColor = Color.Aqua;
            this.keybindLevelRecent.Location = new Point(5, 425);
            this.keybindLevelRecent.Name = "keybindLevelRecent";
            this.keybindLevelRecent.Padding = new Padding(0, 0, 0, 2);
            this.keybindLevelRecent.RightToLeft = RightToLeft.No;
            this.keybindLevelRecent.Size = new Size(91, 17);
            this.keybindLevelRecent.TabIndex = 155;
            this.keybindLevelRecent.Tag = "levelrecent";
            this.keybindLevelRecent.Text = "Level Recent";
            this.keybindLevelRecent.Click += this.keybindLabel_Click;
            // 
            // keybindLevelOpen
            // 
            this.keybindLevelOpen.AutoSize = true;
            this.keybindLevelOpen.Cursor = Cursors.Hand;
            this.keybindLevelOpen.Dock = DockStyle.Top;
            this.keybindLevelOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLevelOpen.ForeColor = Color.Aqua;
            this.keybindLevelOpen.Location = new Point(5, 408);
            this.keybindLevelOpen.Name = "keybindLevelOpen";
            this.keybindLevelOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindLevelOpen.RightToLeft = RightToLeft.No;
            this.keybindLevelOpen.Size = new Size(77, 17);
            this.keybindLevelOpen.TabIndex = 154;
            this.keybindLevelOpen.Tag = "levelopen";
            this.keybindLevelOpen.Text = "Level Open";
            this.keybindLevelOpen.Click += this.keybindLabel_Click;
            // 
            // keybindLevelNew
            // 
            this.keybindLevelNew.AutoSize = true;
            this.keybindLevelNew.Cursor = Cursors.Hand;
            this.keybindLevelNew.Dock = DockStyle.Top;
            this.keybindLevelNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLevelNew.ForeColor = Color.Aqua;
            this.keybindLevelNew.Location = new Point(5, 391);
            this.keybindLevelNew.Name = "keybindLevelNew";
            this.keybindLevelNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindLevelNew.RightToLeft = RightToLeft.No;
            this.keybindLevelNew.Size = new Size(70, 17);
            this.keybindLevelNew.TabIndex = 153;
            this.keybindLevelNew.Tag = "levelnew";
            this.keybindLevelNew.Text = "Level New";
            this.keybindLevelNew.Click += this.keybindLabel_Click;
            // 
            // keybindSaveAll
            // 
            this.keybindSaveAll.AutoSize = true;
            this.keybindSaveAll.Cursor = Cursors.Hand;
            this.keybindSaveAll.Dock = DockStyle.Top;
            this.keybindSaveAll.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSaveAll.ForeColor = Color.Aqua;
            this.keybindSaveAll.Location = new Point(5, 374);
            this.keybindSaveAll.Name = "keybindSaveAll";
            this.keybindSaveAll.Padding = new Padding(0, 0, 0, 2);
            this.keybindSaveAll.RightToLeft = RightToLeft.No;
            this.keybindSaveAll.Size = new Size(63, 17);
            this.keybindSaveAll.TabIndex = 152;
            this.keybindSaveAll.Tag = "saveall";
            this.keybindSaveAll.Text = "Save All";
            this.keybindSaveAll.Click += this.keybindLabel_Click;
            // 
            // keybindSampleSaveAs
            // 
            this.keybindSampleSaveAs.AutoSize = true;
            this.keybindSampleSaveAs.Cursor = Cursors.Hand;
            this.keybindSampleSaveAs.Dock = DockStyle.Top;
            this.keybindSampleSaveAs.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSampleSaveAs.ForeColor = Color.Aqua;
            this.keybindSampleSaveAs.Location = new Point(5, 357);
            this.keybindSampleSaveAs.Name = "keybindSampleSaveAs";
            this.keybindSampleSaveAs.Padding = new Padding(0, 0, 0, 2);
            this.keybindSampleSaveAs.RightToLeft = RightToLeft.No;
            this.keybindSampleSaveAs.Size = new Size(105, 17);
            this.keybindSampleSaveAs.TabIndex = 151;
            this.keybindSampleSaveAs.Tag = "samplesaveas";
            this.keybindSampleSaveAs.Text = "Sample Save As";
            this.keybindSampleSaveAs.Click += this.keybindLabel_Click;
            // 
            // keybindSampleSave
            // 
            this.keybindSampleSave.AutoSize = true;
            this.keybindSampleSave.Cursor = Cursors.Hand;
            this.keybindSampleSave.Dock = DockStyle.Top;
            this.keybindSampleSave.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSampleSave.ForeColor = Color.Aqua;
            this.keybindSampleSave.Location = new Point(5, 340);
            this.keybindSampleSave.Name = "keybindSampleSave";
            this.keybindSampleSave.Padding = new Padding(0, 0, 0, 2);
            this.keybindSampleSave.RightToLeft = RightToLeft.No;
            this.keybindSampleSave.Size = new Size(84, 17);
            this.keybindSampleSave.TabIndex = 150;
            this.keybindSampleSave.Tag = "samplesave";
            this.keybindSampleSave.Text = "Sample Save";
            this.keybindSampleSave.Click += this.keybindLabel_Click;
            // 
            // keybindSampleOpen
            // 
            this.keybindSampleOpen.AutoSize = true;
            this.keybindSampleOpen.Cursor = Cursors.Hand;
            this.keybindSampleOpen.Dock = DockStyle.Top;
            this.keybindSampleOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSampleOpen.ForeColor = Color.Aqua;
            this.keybindSampleOpen.Location = new Point(5, 323);
            this.keybindSampleOpen.Name = "keybindSampleOpen";
            this.keybindSampleOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindSampleOpen.RightToLeft = RightToLeft.No;
            this.keybindSampleOpen.Size = new Size(84, 17);
            this.keybindSampleOpen.TabIndex = 149;
            this.keybindSampleOpen.Tag = "sampleopen";
            this.keybindSampleOpen.Text = "Sample Open";
            this.keybindSampleOpen.Click += this.keybindLabel_Click;
            // 
            // keybindSampleNew
            // 
            this.keybindSampleNew.AutoSize = true;
            this.keybindSampleNew.Cursor = Cursors.Hand;
            this.keybindSampleNew.Dock = DockStyle.Top;
            this.keybindSampleNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindSampleNew.ForeColor = Color.Aqua;
            this.keybindSampleNew.Location = new Point(5, 306);
            this.keybindSampleNew.Name = "keybindSampleNew";
            this.keybindSampleNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindSampleNew.RightToLeft = RightToLeft.No;
            this.keybindSampleNew.Size = new Size(77, 17);
            this.keybindSampleNew.TabIndex = 148;
            this.keybindSampleNew.Tag = "samplenew";
            this.keybindSampleNew.Text = "Sample New";
            this.keybindSampleNew.Click += this.keybindLabel_Click;
            // 
            // keybindMasterSaveAs
            // 
            this.keybindMasterSaveAs.AutoSize = true;
            this.keybindMasterSaveAs.Cursor = Cursors.Hand;
            this.keybindMasterSaveAs.Dock = DockStyle.Top;
            this.keybindMasterSaveAs.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindMasterSaveAs.ForeColor = Color.Aqua;
            this.keybindMasterSaveAs.Location = new Point(5, 289);
            this.keybindMasterSaveAs.Name = "keybindMasterSaveAs";
            this.keybindMasterSaveAs.Padding = new Padding(0, 0, 0, 2);
            this.keybindMasterSaveAs.RightToLeft = RightToLeft.No;
            this.keybindMasterSaveAs.Size = new Size(105, 17);
            this.keybindMasterSaveAs.TabIndex = 147;
            this.keybindMasterSaveAs.Tag = "mastersaveas";
            this.keybindMasterSaveAs.Text = "Master Save As";
            this.keybindMasterSaveAs.Click += this.keybindLabel_Click;
            // 
            // keybindMasterSave
            // 
            this.keybindMasterSave.AutoSize = true;
            this.keybindMasterSave.Cursor = Cursors.Hand;
            this.keybindMasterSave.Dock = DockStyle.Top;
            this.keybindMasterSave.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindMasterSave.ForeColor = Color.Aqua;
            this.keybindMasterSave.Location = new Point(5, 272);
            this.keybindMasterSave.Name = "keybindMasterSave";
            this.keybindMasterSave.Padding = new Padding(0, 0, 0, 2);
            this.keybindMasterSave.RightToLeft = RightToLeft.No;
            this.keybindMasterSave.Size = new Size(84, 17);
            this.keybindMasterSave.TabIndex = 146;
            this.keybindMasterSave.Tag = "mastersave";
            this.keybindMasterSave.Text = "Master Save";
            this.keybindMasterSave.Click += this.keybindLabel_Click;
            // 
            // keybindMasterOpen
            // 
            this.keybindMasterOpen.AutoSize = true;
            this.keybindMasterOpen.Cursor = Cursors.Hand;
            this.keybindMasterOpen.Dock = DockStyle.Top;
            this.keybindMasterOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindMasterOpen.ForeColor = Color.Aqua;
            this.keybindMasterOpen.Location = new Point(5, 255);
            this.keybindMasterOpen.Name = "keybindMasterOpen";
            this.keybindMasterOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindMasterOpen.RightToLeft = RightToLeft.No;
            this.keybindMasterOpen.Size = new Size(84, 17);
            this.keybindMasterOpen.TabIndex = 145;
            this.keybindMasterOpen.Tag = "masteropen";
            this.keybindMasterOpen.Text = "Master Open";
            this.keybindMasterOpen.Click += this.keybindLabel_Click;
            // 
            // keybindMasterNew
            // 
            this.keybindMasterNew.AutoSize = true;
            this.keybindMasterNew.Cursor = Cursors.Hand;
            this.keybindMasterNew.Dock = DockStyle.Top;
            this.keybindMasterNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindMasterNew.ForeColor = Color.Aqua;
            this.keybindMasterNew.Location = new Point(5, 238);
            this.keybindMasterNew.Name = "keybindMasterNew";
            this.keybindMasterNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindMasterNew.RightToLeft = RightToLeft.No;
            this.keybindMasterNew.Size = new Size(77, 17);
            this.keybindMasterNew.TabIndex = 144;
            this.keybindMasterNew.Tag = "masternew";
            this.keybindMasterNew.Text = "Master New";
            this.keybindMasterNew.Click += this.keybindLabel_Click;
            // 
            // keybindGateSaveAs
            // 
            this.keybindGateSaveAs.AutoSize = true;
            this.keybindGateSaveAs.Cursor = Cursors.Hand;
            this.keybindGateSaveAs.Dock = DockStyle.Top;
            this.keybindGateSaveAs.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindGateSaveAs.ForeColor = Color.Aqua;
            this.keybindGateSaveAs.Location = new Point(5, 221);
            this.keybindGateSaveAs.Name = "keybindGateSaveAs";
            this.keybindGateSaveAs.Padding = new Padding(0, 0, 0, 2);
            this.keybindGateSaveAs.RightToLeft = RightToLeft.No;
            this.keybindGateSaveAs.Size = new Size(91, 17);
            this.keybindGateSaveAs.TabIndex = 143;
            this.keybindGateSaveAs.Tag = "gatesaveas";
            this.keybindGateSaveAs.Text = "Gate Save As";
            this.keybindGateSaveAs.Click += this.keybindLabel_Click;
            // 
            // keybindGateSave
            // 
            this.keybindGateSave.AutoSize = true;
            this.keybindGateSave.Cursor = Cursors.Hand;
            this.keybindGateSave.Dock = DockStyle.Top;
            this.keybindGateSave.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindGateSave.ForeColor = Color.Aqua;
            this.keybindGateSave.Location = new Point(5, 204);
            this.keybindGateSave.Name = "keybindGateSave";
            this.keybindGateSave.Padding = new Padding(0, 0, 0, 2);
            this.keybindGateSave.RightToLeft = RightToLeft.No;
            this.keybindGateSave.Size = new Size(70, 17);
            this.keybindGateSave.TabIndex = 142;
            this.keybindGateSave.Tag = "gatesave";
            this.keybindGateSave.Text = "Gate Save";
            this.keybindGateSave.Click += this.keybindLabel_Click;
            // 
            // keybindGateOpen
            // 
            this.keybindGateOpen.AutoSize = true;
            this.keybindGateOpen.Cursor = Cursors.Hand;
            this.keybindGateOpen.Dock = DockStyle.Top;
            this.keybindGateOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindGateOpen.ForeColor = Color.Aqua;
            this.keybindGateOpen.Location = new Point(5, 187);
            this.keybindGateOpen.Name = "keybindGateOpen";
            this.keybindGateOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindGateOpen.RightToLeft = RightToLeft.No;
            this.keybindGateOpen.Size = new Size(70, 17);
            this.keybindGateOpen.TabIndex = 141;
            this.keybindGateOpen.Tag = "gateopen";
            this.keybindGateOpen.Text = "Gate Open";
            this.keybindGateOpen.Click += this.keybindLabel_Click;
            // 
            // keybindGateNew
            // 
            this.keybindGateNew.AutoSize = true;
            this.keybindGateNew.Cursor = Cursors.Hand;
            this.keybindGateNew.Dock = DockStyle.Top;
            this.keybindGateNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindGateNew.ForeColor = Color.Aqua;
            this.keybindGateNew.Location = new Point(5, 170);
            this.keybindGateNew.Name = "keybindGateNew";
            this.keybindGateNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindGateNew.RightToLeft = RightToLeft.No;
            this.keybindGateNew.Size = new Size(63, 17);
            this.keybindGateNew.TabIndex = 140;
            this.keybindGateNew.Tag = "gatenew";
            this.keybindGateNew.Text = "Gate New";
            this.keybindGateNew.Click += this.keybindLabel_Click;
            // 
            // keybindLvlSaveAs
            // 
            this.keybindLvlSaveAs.AutoSize = true;
            this.keybindLvlSaveAs.Cursor = Cursors.Hand;
            this.keybindLvlSaveAs.Dock = DockStyle.Top;
            this.keybindLvlSaveAs.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLvlSaveAs.ForeColor = Color.Aqua;
            this.keybindLvlSaveAs.Location = new Point(5, 153);
            this.keybindLvlSaveAs.Name = "keybindLvlSaveAs";
            this.keybindLvlSaveAs.Padding = new Padding(0, 0, 0, 2);
            this.keybindLvlSaveAs.RightToLeft = RightToLeft.No;
            this.keybindLvlSaveAs.Size = new Size(84, 17);
            this.keybindLvlSaveAs.TabIndex = 139;
            this.keybindLvlSaveAs.Tag = "lvlsaveas";
            this.keybindLvlSaveAs.Text = "Lvl Save As";
            this.keybindLvlSaveAs.Click += this.keybindLabel_Click;
            // 
            // keybindLvlSave
            // 
            this.keybindLvlSave.AutoSize = true;
            this.keybindLvlSave.Cursor = Cursors.Hand;
            this.keybindLvlSave.Dock = DockStyle.Top;
            this.keybindLvlSave.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLvlSave.ForeColor = Color.Aqua;
            this.keybindLvlSave.Location = new Point(5, 136);
            this.keybindLvlSave.Name = "keybindLvlSave";
            this.keybindLvlSave.Padding = new Padding(0, 0, 0, 2);
            this.keybindLvlSave.RightToLeft = RightToLeft.No;
            this.keybindLvlSave.Size = new Size(63, 17);
            this.keybindLvlSave.TabIndex = 138;
            this.keybindLvlSave.Tag = "lvlsave";
            this.keybindLvlSave.Text = "Lvl Save";
            this.keybindLvlSave.Click += this.keybindLabel_Click;
            // 
            // keybindLvlOpen
            // 
            this.keybindLvlOpen.AutoSize = true;
            this.keybindLvlOpen.Cursor = Cursors.Hand;
            this.keybindLvlOpen.Dock = DockStyle.Top;
            this.keybindLvlOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLvlOpen.ForeColor = Color.Aqua;
            this.keybindLvlOpen.Location = new Point(5, 119);
            this.keybindLvlOpen.Name = "keybindLvlOpen";
            this.keybindLvlOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindLvlOpen.RightToLeft = RightToLeft.No;
            this.keybindLvlOpen.Size = new Size(63, 17);
            this.keybindLvlOpen.TabIndex = 137;
            this.keybindLvlOpen.Tag = "lvlopen";
            this.keybindLvlOpen.Text = "Lvl Open";
            this.keybindLvlOpen.Click += this.keybindLabel_Click;
            // 
            // keybindLvlNew
            // 
            this.keybindLvlNew.AutoSize = true;
            this.keybindLvlNew.Cursor = Cursors.Hand;
            this.keybindLvlNew.Dock = DockStyle.Top;
            this.keybindLvlNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLvlNew.ForeColor = Color.Aqua;
            this.keybindLvlNew.Location = new Point(5, 102);
            this.keybindLvlNew.Name = "keybindLvlNew";
            this.keybindLvlNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindLvlNew.RightToLeft = RightToLeft.No;
            this.keybindLvlNew.Size = new Size(56, 17);
            this.keybindLvlNew.TabIndex = 136;
            this.keybindLvlNew.Tag = "lvlnew";
            this.keybindLvlNew.Text = "Lvl New";
            this.keybindLvlNew.Click += this.keybindLabel_Click;
            // 
            // keybindTemplateOpen
            // 
            this.keybindTemplateOpen.AutoSize = true;
            this.keybindTemplateOpen.Cursor = Cursors.Hand;
            this.keybindTemplateOpen.Dock = DockStyle.Top;
            this.keybindTemplateOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindTemplateOpen.ForeColor = Color.Aqua;
            this.keybindTemplateOpen.Location = new Point(5, 85);
            this.keybindTemplateOpen.Name = "keybindTemplateOpen";
            this.keybindTemplateOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindTemplateOpen.RightToLeft = RightToLeft.No;
            this.keybindTemplateOpen.Size = new Size(98, 17);
            this.keybindTemplateOpen.TabIndex = 175;
            this.keybindTemplateOpen.Tag = "templateopen";
            this.keybindTemplateOpen.Text = "Template Open";
            this.keybindTemplateOpen.Click += this.keybindLabel_Click;
            // 
            // keybindLeafUndo
            // 
            this.keybindLeafUndo.AutoSize = true;
            this.keybindLeafUndo.Cursor = Cursors.Hand;
            this.keybindLeafUndo.Dock = DockStyle.Top;
            this.keybindLeafUndo.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLeafUndo.ForeColor = Color.Aqua;
            this.keybindLeafUndo.Location = new Point(5, 68);
            this.keybindLeafUndo.Name = "keybindLeafUndo";
            this.keybindLeafUndo.Padding = new Padding(0, 0, 0, 2);
            this.keybindLeafUndo.RightToLeft = RightToLeft.No;
            this.keybindLeafUndo.Size = new Size(70, 17);
            this.keybindLeafUndo.TabIndex = 176;
            this.keybindLeafUndo.Tag = "leafundo";
            this.keybindLeafUndo.Text = "Leaf Undo";
            this.keybindLeafUndo.Click += this.keybindLabel_Click;
            // 
            // keybindLeafSaveAs
            // 
            this.keybindLeafSaveAs.AutoSize = true;
            this.keybindLeafSaveAs.Cursor = Cursors.Hand;
            this.keybindLeafSaveAs.Dock = DockStyle.Top;
            this.keybindLeafSaveAs.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLeafSaveAs.ForeColor = Color.Aqua;
            this.keybindLeafSaveAs.Location = new Point(5, 51);
            this.keybindLeafSaveAs.Name = "keybindLeafSaveAs";
            this.keybindLeafSaveAs.Padding = new Padding(0, 0, 0, 2);
            this.keybindLeafSaveAs.RightToLeft = RightToLeft.No;
            this.keybindLeafSaveAs.Size = new Size(91, 17);
            this.keybindLeafSaveAs.TabIndex = 135;
            this.keybindLeafSaveAs.Tag = "leafsaveas";
            this.keybindLeafSaveAs.Text = "Leaf Save As";
            this.keybindLeafSaveAs.Click += this.keybindLabel_Click;
            // 
            // keybindLeafSave
            // 
            this.keybindLeafSave.AutoSize = true;
            this.keybindLeafSave.Cursor = Cursors.Hand;
            this.keybindLeafSave.Dock = DockStyle.Top;
            this.keybindLeafSave.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLeafSave.ForeColor = Color.Aqua;
            this.keybindLeafSave.Location = new Point(5, 34);
            this.keybindLeafSave.Name = "keybindLeafSave";
            this.keybindLeafSave.Padding = new Padding(0, 0, 0, 2);
            this.keybindLeafSave.RightToLeft = RightToLeft.No;
            this.keybindLeafSave.Size = new Size(70, 17);
            this.keybindLeafSave.TabIndex = 134;
            this.keybindLeafSave.Tag = "leafsave";
            this.keybindLeafSave.Text = "Leaf Save";
            this.keybindLeafSave.Click += this.keybindLabel_Click;
            // 
            // keybindLeafOpen
            // 
            this.keybindLeafOpen.AutoSize = true;
            this.keybindLeafOpen.Cursor = Cursors.Hand;
            this.keybindLeafOpen.Dock = DockStyle.Top;
            this.keybindLeafOpen.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLeafOpen.ForeColor = Color.Aqua;
            this.keybindLeafOpen.Location = new Point(5, 17);
            this.keybindLeafOpen.Name = "keybindLeafOpen";
            this.keybindLeafOpen.Padding = new Padding(0, 0, 0, 2);
            this.keybindLeafOpen.RightToLeft = RightToLeft.No;
            this.keybindLeafOpen.Size = new Size(70, 17);
            this.keybindLeafOpen.TabIndex = 133;
            this.keybindLeafOpen.Tag = "leafopen";
            this.keybindLeafOpen.Text = "Leaf Open";
            this.keybindLeafOpen.Click += this.keybindLabel_Click;
            // 
            // keybindLeafNew
            // 
            this.keybindLeafNew.AutoSize = true;
            this.keybindLeafNew.Cursor = Cursors.Hand;
            this.keybindLeafNew.Dock = DockStyle.Top;
            this.keybindLeafNew.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.keybindLeafNew.ForeColor = Color.Aqua;
            this.keybindLeafNew.Location = new Point(5, 0);
            this.keybindLeafNew.Name = "keybindLeafNew";
            this.keybindLeafNew.Padding = new Padding(0, 0, 0, 2);
            this.keybindLeafNew.RightToLeft = RightToLeft.No;
            this.keybindLeafNew.Size = new Size(63, 17);
            this.keybindLeafNew.TabIndex = 132;
            this.keybindLeafNew.Tag = "leafnew";
            this.keybindLeafNew.Text = "Leaf New";
            this.keybindLeafNew.Click += this.keybindLabel_Click;
            // 
            // txtKeybindSearch
            // 
            this.txtKeybindSearch.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtKeybindSearch.Location = new Point(22, 2);
            this.txtKeybindSearch.Name = "txtKeybindSearch";
            this.txtKeybindSearch.RightToLeft = RightToLeft.No;
            this.txtKeybindSearch.Size = new Size(122, 21);
            this.txtKeybindSearch.TabIndex = 148;
            this.txtKeybindSearch.Text = "search...";
            this.txtKeybindSearch.TextChanged += this.txtKeybindSearch_TextChanged;
            this.txtKeybindSearch.Enter += this.txtKeybindSearch_Enter;
            // 
            // CustomizeWorkspace
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(367, 370);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolstripCustomize);
            this.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomizeWorkspace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Customize Workspace";
            this.TopMost = true;
            this.KeyDown += this.CustomizeWorkspace_KeyDown;
            this.toolstripCustomize.ResumeLayout(false);
            this.toolstripCustomize.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabUIColors.ResumeLayout(false);
            this.tabSeq.ResumeLayout(false);
            this.tabSeq.PerformLayout();
            this.tabAudio.ResumeLayout(false);
            this.tabAudio.PerformLayout();
            this.tabKeybinds.ResumeLayout(false);
            this.tabKeybinds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
            this.panelSetKeybind.ResumeLayout(false);
            this.panelSetKeybind.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStrip toolstripCustomize;
        private System.Windows.Forms.ToolStripButton btnCustomizeApply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabUIColors;
        private System.Windows.Forms.TabPage tabAudio;
        public System.Windows.Forms.CheckBox checkMuteApp;
        private System.Windows.Forms.TabPage tabKeybinds;
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
        private System.Windows.Forms.Button btnKeybindReset;
        private System.Windows.Forms.Button btnSingleReset;
        private System.Windows.Forms.Button btnCloseKeybind;
        private System.Windows.Forms.Label keybindSampleSaveAs;
        private System.Windows.Forms.Label keybindSampleSave;
        private System.Windows.Forms.Label keybindSampleOpen;
        private System.Windows.Forms.Label keybindSampleNew;
        private System.Windows.Forms.Label keybindSaveAll;
        private System.Windows.Forms.Label keybindLevelExplorer;
        private System.Windows.Forms.Label keybindLevelRecent;
        private System.Windows.Forms.Label keybindLevelOpen;
        private System.Windows.Forms.Label keybindLevelNew;
        private System.Windows.Forms.Label keybindPreviousLeaf;
        private System.Windows.Forms.Label keybindNextLeaf;
        private System.Windows.Forms.Label keybindPreviousLvl;
        private System.Windows.Forms.Label keybindNextLvl;
        private System.Windows.Forms.Label lblInvalid;
        private System.Windows.Forms.Label keybindRandomizeRow;
        private System.Windows.Forms.Label keybindSplitLeaf;
        private System.Windows.Forms.Label keybindInterpolate;
        private System.Windows.Forms.Label keybindColorDialog;
        private System.Windows.Forms.Label keybindToggleAutoPlace;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label keybindQuick1;
        private System.Windows.Forms.Label keybindQuick9;
        private System.Windows.Forms.Label keybindQuick8;
        private System.Windows.Forms.Label keybindQuick7;
        private System.Windows.Forms.Label keybindQuick6;
        private System.Windows.Forms.Label keybindQuick5;
        private System.Windows.Forms.Label keybindQuick4;
        private System.Windows.Forms.Label keybindQuick3;
        private System.Windows.Forms.Label keybindQuick2;
        private System.Windows.Forms.Label keybindTemplateOpen;
        private System.Windows.Forms.Label keybindLeafUndo;
        private System.Windows.Forms.Label keybindshowhidesample;
        private System.Windows.Forms.Label keybindshowhidefolder;
        private System.Windows.Forms.Label keybindshowmaster;
        private System.Windows.Forms.Label keybindshowgate;
        private System.Windows.Forms.Label keybindshowhidelvl;
        private System.Windows.Forms.Label keybindshowhideleaf;
        private TabPage tabSeq;
        private TreeViewEx treeObjects;
        private TextBox txtSearch;
        private ImageList imageList1;
        public PropertyGrid propertyGridUIColors;
    }
}