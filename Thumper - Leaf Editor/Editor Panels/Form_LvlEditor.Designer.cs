namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_LvlEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_LvlEditor));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            this.toolTip1 = new ToolTip(this.components);
            this.label25 = new Label();
            this.label23 = new Label();
            this.label28 = new Label();
            this.trackLvlVolumeZoom = new TrackBar();
            this.label20 = new Label();
            this.lblLvlTunnels = new Label();
            this.panelLevel = new Panel();
            this.lvlToolStrip = new ToolStrip();
            this.btnLvlLeafAdd = new ToolStripButton();
            this.btnLvlLeafDelete = new ToolStripButton();
            this.btnLvlLeafUp = new ToolStripButton();
            this.btnLvlLeafDown = new ToolStripButton();
            this.btnLvlLeafCopy = new ToolStripButton();
            this.btnLvlLeafPaste = new ToolStripButton();
            this.btnLvlRefreshBeats = new ToolStripButton();
            this.btnLvlLeafRandom = new ToolStripButton();
            this.label39 = new Label();
            this.label29 = new Label();
            this.dropLvlTutorial = new ComboBox();
            this.label26 = new Label();
            this.dropLvlInput = new ComboBox();
            this.label24 = new Label();
            this.NUD_lvlVolume = new NumericUpDown();
            this.NUD_lvlApproach = new NumericUpDown();
            this.lvlLeafList = new DataGridView();
            this.lvlfiletype = new DataGridViewImageColumn();
            this.Leaf = new DataGridViewTextBoxColumn();
            this.Beats = new DataGridViewTextBoxColumn();
            this.splitContainer1 = new SplitContainer();
            this.lvlSeqObjs = new DataGridView();
            this.panel1 = new Panel();
            this.lvlVolumeToolStrip = new ToolStrip();
            this.btnLvlSeqAdd = new ToolStripButton();
            this.btnLvlSeqDelete = new ToolStripButton();
            this.btnLvlSeqClear = new ToolStripButton();
            this.lvlPathsToolStrip2 = new ToolStrip();
            this.btnLvlPathUp = new ToolStripButton();
            this.btnLvlPathDown = new ToolStripButton();
            this.btnLvlPathClear = new ToolStripButton();
            this.lvlLoopToolStrip = new ToolStrip();
            this.btnLvlLoopAdd = new ToolStripButton();
            this.btnLvlLoopDelete = new ToolStripButton();
            this.lvlLeafPaths = new DataGridView();
            this.lvlPathsToolStrip = new ToolStrip();
            this.btnLvlPathAdd = new ToolStripButton();
            this.btnLvlPathDelete = new ToolStripButton();
            this.btnLvlCopyTunnel = new ToolStripButton();
            this.btnLvlPasteTunnel = new ToolStripButton();
            this.chkTunnelCopy = new ToolStripButton();
            this.btnLvlRandomTunnel = new ToolStripButton();
            this.label22 = new Label();
            this.lvlLoopTracks = new DataGridView();
            this.LoopSample = new DataGridViewComboBoxColumn();
            this.BeatsPerLoop = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)this.trackLvlVolumeZoom).BeginInit();
            this.panelLevel.SuspendLayout();
            this.lvlToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.NUD_lvlVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.NUD_lvlApproach).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlSeqObjs).BeginInit();
            this.panel1.SuspendLayout();
            this.lvlVolumeToolStrip.SuspendLayout();
            this.lvlPathsToolStrip2.SuspendLayout();
            this.lvlLoopToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).BeginInit();
            this.lvlPathsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).BeginInit();
            this.SuspendLayout();
            // 
            // label25
            // 
            this.label25.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.label25.AutoSize = true;
            this.label25.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label25.ForeColor = Color.White;
            this.label25.Location = new Point(34, 460);
            this.label25.Margin = new Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new Size(66, 15);
            this.label25.TabIndex = 89;
            this.label25.Text = "Allow Input";
            this.toolTip1.SetToolTip(this.label25, "Can the player control the beetle during this lvl?");
            // 
            // label23
            // 
            this.label23.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.label23.AutoSize = true;
            this.label23.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label23.ForeColor = Color.White;
            this.label23.Location = new Point(2, 411);
            this.label23.Margin = new Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new Size(93, 15);
            this.label23.TabIndex = 68;
            this.label23.Text = "Approach Beats";
            this.toolTip1.SetToolTip(this.label23, "How many beats before the first leaf does the\r\nloop tracks and volume sequencing start.\r\n");
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label28.ForeColor = Color.White;
            this.label28.Image = Properties.Resources.icon_zoom;
            this.label28.Location = new Point(1, 5);
            this.label28.Margin = new Padding(4, 0, 4, 0);
            this.label28.MinimumSize = new Size(23, 23);
            this.label28.Name = "label28";
            this.label28.Size = new Size(23, 23);
            this.label28.TabIndex = 42;
            this.toolTip1.SetToolTip(this.label28, "Cell zoom/width");
            // 
            // trackLvlVolumeZoom
            // 
            this.trackLvlVolumeZoom.AutoSize = false;
            this.trackLvlVolumeZoom.Cursor = Cursors.Hand;
            this.trackLvlVolumeZoom.Location = new Point(21, 5);
            this.trackLvlVolumeZoom.Margin = new Padding(4, 3, 4, 3);
            this.trackLvlVolumeZoom.Maximum = 120;
            this.trackLvlVolumeZoom.Minimum = 2;
            this.trackLvlVolumeZoom.Name = "trackLvlVolumeZoom";
            this.trackLvlVolumeZoom.Size = new Size(117, 22);
            this.trackLvlVolumeZoom.TabIndex = 41;
            this.trackLvlVolumeZoom.TickStyle = TickStyle.None;
            this.toolTip1.SetToolTip(this.trackLvlVolumeZoom, "Cell zoom/width");
            this.trackLvlVolumeZoom.Value = 60;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = Color.FromArgb(10, 10, 10);
            this.label20.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label20.ForeColor = Color.White;
            this.label20.Location = new Point(31, 3);
            this.label20.Margin = new Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new Size(182, 13);
            this.label20.TabIndex = 68;
            this.label20.Text = "Loop Track Volume Sequencer";
            this.toolTip1.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
            // 
            // lblLvlTunnels
            // 
            this.lblLvlTunnels.AutoSize = true;
            this.lblLvlTunnels.BackColor = Color.FromArgb(10, 10, 10);
            this.lblLvlTunnels.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblLvlTunnels.ForeColor = Color.White;
            this.lblLvlTunnels.Location = new Point(64, 1);
            this.lblLvlTunnels.Margin = new Padding(4, 0, 4, 0);
            this.lblLvlTunnels.Name = "lblLvlTunnels";
            this.lblLvlTunnels.Size = new Size(90, 13);
            this.lblLvlTunnels.TabIndex = 80;
            this.lblLvlTunnels.Text = "Paths/Tunnels";
            this.toolTip1.SetToolTip(this.lblLvlTunnels, "Unique per leaf");
            // 
            // panelLevel
            // 
            this.panelLevel.BackColor = Color.FromArgb(35, 35, 35);
            this.panelLevel.BorderStyle = BorderStyle.FixedSingle;
            this.panelLevel.Controls.Add(this.lvlToolStrip);
            this.panelLevel.Controls.Add(this.label39);
            this.panelLevel.Controls.Add(this.label29);
            this.panelLevel.Controls.Add(this.dropLvlTutorial);
            this.panelLevel.Controls.Add(this.label26);
            this.panelLevel.Controls.Add(this.dropLvlInput);
            this.panelLevel.Controls.Add(this.label25);
            this.panelLevel.Controls.Add(this.label24);
            this.panelLevel.Controls.Add(this.NUD_lvlVolume);
            this.panelLevel.Controls.Add(this.label23);
            this.panelLevel.Controls.Add(this.NUD_lvlApproach);
            this.panelLevel.Controls.Add(this.lvlLeafList);
            this.panelLevel.Controls.Add(this.splitContainer1);
            this.panelLevel.Dock = DockStyle.Fill;
            this.panelLevel.Location = new Point(0, 0);
            this.panelLevel.Margin = new Padding(4, 3, 4, 3);
            this.panelLevel.MinimumSize = new Size(70, 69);
            this.panelLevel.Name = "panelLevel";
            this.panelLevel.Size = new Size(933, 519);
            this.panelLevel.TabIndex = 47;
            this.panelLevel.Tag = "editorpanel";
            // 
            // lvlToolStrip
            // 
            this.lvlToolStrip.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lvlToolStrip.AutoSize = false;
            this.lvlToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlToolStrip.Dock = DockStyle.None;
            this.lvlToolStrip.GripMargin = new Padding(0);
            this.lvlToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLeafAdd, this.btnLvlLeafDelete, this.btnLvlLeafUp, this.btnLvlLeafDown, this.btnLvlLeafCopy, this.btnLvlLeafPaste, this.btnLvlRefreshBeats, this.btnLvlLeafRandom });
            this.lvlToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlToolStrip.Location = new Point(4, 354);
            this.lvlToolStrip.Name = "lvlToolStrip";
            this.lvlToolStrip.Padding = new Padding(0);
            this.lvlToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlToolStrip.Size = new Size(275, 29);
            this.lvlToolStrip.Stretch = true;
            this.lvlToolStrip.TabIndex = 141;
            // 
            // btnLvlLeafAdd
            // 
            this.btnLvlLeafAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlLeafAdd.ForeColor = Color.White;
            this.btnLvlLeafAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlLeafAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafAdd.Margin = new Padding(0);
            this.btnLvlLeafAdd.Name = "btnLvlLeafAdd";
            this.btnLvlLeafAdd.Size = new Size(24, 29);
            this.btnLvlLeafAdd.ToolTipText = "Add new sublevel to the list";
            // 
            // btnLvlLeafDelete
            // 
            this.btnLvlLeafDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDelete.Enabled = false;
            this.btnLvlLeafDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLeafDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDelete.Margin = new Padding(0);
            this.btnLvlLeafDelete.Name = "btnLvlLeafDelete";
            this.btnLvlLeafDelete.Size = new Size(24, 29);
            this.btnLvlLeafDelete.ToolTipText = "Delete selected sublevel from this list";
            // 
            // btnLvlLeafUp
            // 
            this.btnLvlLeafUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafUp.Enabled = false;
            this.btnLvlLeafUp.Image = Properties.Resources.icon_arrowup2;
            this.btnLvlLeafUp.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafUp.Margin = new Padding(0);
            this.btnLvlLeafUp.Name = "btnLvlLeafUp";
            this.btnLvlLeafUp.Size = new Size(24, 29);
            this.btnLvlLeafUp.ToolTipText = "Move selected sublevel up";
            // 
            // btnLvlLeafDown
            // 
            this.btnLvlLeafDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDown.Enabled = false;
            this.btnLvlLeafDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlLeafDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDown.Margin = new Padding(0);
            this.btnLvlLeafDown.Name = "btnLvlLeafDown";
            this.btnLvlLeafDown.Size = new Size(24, 29);
            this.btnLvlLeafDown.ToolTipText = "Move selected sublevel down";
            // 
            // btnLvlLeafCopy
            // 
            this.btnLvlLeafCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafCopy.Enabled = false;
            this.btnLvlLeafCopy.Image = Properties.Resources.icon_copy2;
            this.btnLvlLeafCopy.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafCopy.Margin = new Padding(0);
            this.btnLvlLeafCopy.Name = "btnLvlLeafCopy";
            this.btnLvlLeafCopy.Size = new Size(24, 29);
            this.btnLvlLeafCopy.ToolTipText = "Copy selected sublevel";
            // 
            // btnLvlLeafPaste
            // 
            this.btnLvlLeafPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafPaste.Enabled = false;
            this.btnLvlLeafPaste.Image = Properties.Resources.icon_paste2;
            this.btnLvlLeafPaste.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafPaste.Name = "btnLvlLeafPaste";
            this.btnLvlLeafPaste.Size = new Size(24, 26);
            this.btnLvlLeafPaste.ToolTipText = "Paste the copied sublevel";
            // 
            // btnLvlRefreshBeats
            // 
            this.btnLvlRefreshBeats.Alignment = ToolStripItemAlignment.Right;
            this.btnLvlRefreshBeats.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlRefreshBeats.Image = Properties.Resources.icon_refresh2;
            this.btnLvlRefreshBeats.ImageTransparentColor = Color.Magenta;
            this.btnLvlRefreshBeats.Name = "btnLvlRefreshBeats";
            this.btnLvlRefreshBeats.Size = new Size(24, 26);
            this.btnLvlRefreshBeats.ToolTipText = "Recount beats of all leafs in this lvl";
            // 
            // btnLvlLeafRandom
            // 
            this.btnLvlLeafRandom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafRandom.Enabled = false;
            this.btnLvlLeafRandom.Image = Properties.Resources.icon_random;
            this.btnLvlLeafRandom.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafRandom.Name = "btnLvlLeafRandom";
            this.btnLvlLeafRandom.Size = new Size(24, 26);
            this.btnLvlLeafRandom.ToolTipText = "Add a random leaf";
            // 
            // label39
            // 
            this.label39.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.label39.AutoSize = true;
            this.label39.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label39.ForeColor = Color.Silver;
            this.label39.Location = new Point(80, 388);
            this.label39.Margin = new Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new Size(91, 15);
            this.label39.TabIndex = 117;
            this.label39.Text = "══Lvl Options══";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = Color.FromArgb(10, 10, 10);
            this.label29.Dock = DockStyle.Top;
            this.label29.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label29.ForeColor = Color.White;
            this.label29.Location = new Point(0, 0);
            this.label29.Margin = new Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new Size(56, 13);
            this.label29.TabIndex = 93;
            this.label29.Text = "Leaf List";
            // 
            // dropLvlTutorial
            // 
            this.dropLvlTutorial.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.dropLvlTutorial.BackColor = Color.FromArgb(40, 40, 40);
            this.dropLvlTutorial.DrawMode = DrawMode.OwnerDrawFixed;
            this.dropLvlTutorial.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropLvlTutorial.DropDownWidth = 200;
            this.dropLvlTutorial.FlatStyle = FlatStyle.Flat;
            this.dropLvlTutorial.ForeColor = Color.White;
            this.dropLvlTutorial.FormattingEnabled = true;
            this.dropLvlTutorial.Items.AddRange(new object[] { "TUTORIAL_NONE", "TUTORIAL_THUMP", "TUTORIAL_THUMP_REMINDER", "TUTORIAL_TURN_RIGHT", "TUTORIAL_TURN_LEFT", "TUTORIAL_GRIND", "TUTORIAL_POWER_GRIND", "TUTORIAL_POUND", "TUTORIAL_POUND_REMINDER", "TUTORIAL_LANES", "TUTORIAL_JUMP" });
            this.dropLvlTutorial.Location = new Point(113, 485);
            this.dropLvlTutorial.Margin = new Padding(4, 3, 4, 3);
            this.dropLvlTutorial.Name = "dropLvlTutorial";
            this.dropLvlTutorial.Size = new Size(165, 24);
            this.dropLvlTutorial.TabIndex = 90;
            // 
            // label26
            // 
            this.label26.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.label26.AutoSize = true;
            this.label26.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label26.ForeColor = Color.White;
            this.label26.Location = new Point(21, 486);
            this.label26.Margin = new Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new Size(77, 15);
            this.label26.TabIndex = 91;
            this.label26.Text = "Tutorial Type";
            // 
            // dropLvlInput
            // 
            this.dropLvlInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.dropLvlInput.BackColor = Color.FromArgb(40, 40, 40);
            this.dropLvlInput.DrawMode = DrawMode.OwnerDrawFixed;
            this.dropLvlInput.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropLvlInput.FlatStyle = FlatStyle.Flat;
            this.dropLvlInput.ForeColor = Color.White;
            this.dropLvlInput.FormattingEnabled = true;
            this.dropLvlInput.Items.AddRange(new object[] { "True", "False" });
            this.dropLvlInput.Location = new Point(113, 458);
            this.dropLvlInput.Margin = new Padding(4, 3, 4, 3);
            this.dropLvlInput.Name = "dropLvlInput";
            this.dropLvlInput.Size = new Size(83, 24);
            this.dropLvlInput.TabIndex = 68;
            // 
            // label24
            // 
            this.label24.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.label24.AutoSize = true;
            this.label24.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label24.ForeColor = Color.White;
            this.label24.Location = new Point(54, 435);
            this.label24.Margin = new Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new Size(49, 15);
            this.label24.TabIndex = 87;
            this.label24.Text = "Volume";
            // 
            // NUD_lvlVolume
            // 
            this.NUD_lvlVolume.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.NUD_lvlVolume.BackColor = Color.FromArgb(40, 40, 40);
            this.NUD_lvlVolume.DecimalPlaces = 2;
            this.NUD_lvlVolume.ForeColor = Color.White;
            this.NUD_lvlVolume.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            this.NUD_lvlVolume.Location = new Point(113, 433);
            this.NUD_lvlVolume.Margin = new Padding(4, 3, 4, 3);
            this.NUD_lvlVolume.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.NUD_lvlVolume.Name = "NUD_lvlVolume";
            this.NUD_lvlVolume.Size = new Size(84, 23);
            this.NUD_lvlVolume.TabIndex = 88;
            this.NUD_lvlVolume.TextAlign = HorizontalAlignment.Center;
            this.NUD_lvlVolume.Value = new decimal(new int[] { 50, 0, 0, 131072 });
            // 
            // NUD_lvlApproach
            // 
            this.NUD_lvlApproach.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.NUD_lvlApproach.BackColor = Color.FromArgb(40, 40, 40);
            this.NUD_lvlApproach.ForeColor = Color.White;
            this.NUD_lvlApproach.Location = new Point(113, 408);
            this.NUD_lvlApproach.Margin = new Padding(4, 3, 4, 3);
            this.NUD_lvlApproach.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.NUD_lvlApproach.Name = "NUD_lvlApproach";
            this.NUD_lvlApproach.Size = new Size(84, 23);
            this.NUD_lvlApproach.TabIndex = 69;
            this.NUD_lvlApproach.TextAlign = HorizontalAlignment.Center;
            this.NUD_lvlApproach.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // lvlLeafList
            // 
            this.lvlLeafList.AllowUserToAddRows = false;
            this.lvlLeafList.AllowUserToDeleteRows = false;
            this.lvlLeafList.AllowUserToResizeRows = false;
            this.lvlLeafList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.lvlLeafList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLeafList.BorderStyle = BorderStyle.None;
            this.lvlLeafList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLeafList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.lvlLeafList.ColumnHeadersHeight = 20;
            this.lvlLeafList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.lvlLeafList.Columns.AddRange(new DataGridViewColumn[] { this.lvlfiletype, this.Leaf, this.Beats });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.DefaultCellStyle = dataGridViewCellStyle2;
            this.lvlLeafList.EnableHeadersVisualStyles = false;
            this.lvlLeafList.GridColor = Color.Black;
            this.lvlLeafList.Location = new Point(4, 13);
            this.lvlLeafList.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafList.Name = "lvlLeafList";
            this.lvlLeafList.ReadOnly = true;
            this.lvlLeafList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.lvlLeafList.RowHeadersVisible = false;
            this.lvlLeafList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lvlLeafList.RowTemplate.Height = 20;
            this.lvlLeafList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafList.Size = new Size(275, 339);
            this.lvlLeafList.TabIndex = 74;
            this.lvlLeafList.Tag = "editorpaneldgv";
            this.lvlLeafList.CellDoubleClick += this.lvlLeafList_CellDoubleClick;
            // 
            // lvlfiletype
            // 
            this.lvlfiletype.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.lvlfiletype.HeaderText = "";
            this.lvlfiletype.Name = "lvlfiletype";
            this.lvlfiletype.ReadOnly = true;
            this.lvlfiletype.Width = 5;
            // 
            // Leaf
            // 
            this.Leaf.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Leaf.FillWeight = 30F;
            this.Leaf.HeaderText = "Leaf";
            this.Leaf.Name = "Leaf";
            this.Leaf.ReadOnly = true;
            this.Leaf.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Beats
            // 
            this.Beats.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Beats.FillWeight = 20F;
            this.Beats.HeaderText = "Beats";
            this.Beats.Name = "Beats";
            this.Beats.ReadOnly = true;
            this.Beats.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            this.splitContainer1.Location = new Point(288, -1);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvlSeqObjs);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.lvlVolumeToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.label20);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvlPathsToolStrip2);
            this.splitContainer1.Panel2.Controls.Add(this.lvlLoopToolStrip);
            this.splitContainer1.Panel2.Controls.Add(this.lvlLeafPaths);
            this.splitContainer1.Panel2.Controls.Add(this.lvlPathsToolStrip);
            this.splitContainer1.Panel2.Controls.Add(this.lblLvlTunnels);
            this.splitContainer1.Panel2.Controls.Add(this.label22);
            this.splitContainer1.Panel2.Controls.Add(this.lvlLoopTracks);
            this.splitContainer1.Size = new Size(644, 519);
            this.splitContainer1.SplitterDistance = 213;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 145;
            // 
            // lvlSeqObjs
            // 
            this.lvlSeqObjs.AllowUserToAddRows = false;
            this.lvlSeqObjs.AllowUserToDeleteRows = false;
            this.lvlSeqObjs.AllowUserToResizeColumns = false;
            this.lvlSeqObjs.AllowUserToResizeRows = false;
            this.lvlSeqObjs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.lvlSeqObjs.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlSeqObjs.BorderStyle = BorderStyle.None;
            this.lvlSeqObjs.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlSeqObjs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle4.Font = new Font("Consolas", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.Silver;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            this.lvlSeqObjs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.lvlSeqObjs.ColumnHeadersHeight = 20;
            this.lvlSeqObjs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle5.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle5.Format = "N3";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            this.lvlSeqObjs.DefaultCellStyle = dataGridViewCellStyle5;
            this.lvlSeqObjs.EnableHeadersVisualStyles = false;
            this.lvlSeqObjs.GridColor = Color.Black;
            this.lvlSeqObjs.Location = new Point(31, 18);
            this.lvlSeqObjs.Margin = new Padding(4, 3, 4, 3);
            this.lvlSeqObjs.Name = "lvlSeqObjs";
            this.lvlSeqObjs.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle6.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            this.lvlSeqObjs.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.lvlSeqObjs.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.lvlSeqObjs.RowTemplate.Height = 20;
            this.lvlSeqObjs.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlSeqObjs.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.lvlSeqObjs.Size = new Size(608, 192);
            this.lvlSeqObjs.TabIndex = 68;
            // 
            // panel1
            // 
            this.panel1.BackColor = Color.Black;
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.trackLvlVolumeZoom);
            this.panel1.Location = new Point(250, -7);
            this.panel1.Margin = new Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(141, 31);
            this.panel1.TabIndex = 92;
            // 
            // lvlVolumeToolStrip
            // 
            this.lvlVolumeToolStrip.AutoSize = false;
            this.lvlVolumeToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlVolumeToolStrip.Dock = DockStyle.None;
            this.lvlVolumeToolStrip.GripMargin = new Padding(0);
            this.lvlVolumeToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlVolumeToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlVolumeToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlSeqAdd, this.btnLvlSeqDelete, this.btnLvlSeqClear });
            this.lvlVolumeToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lvlVolumeToolStrip.Location = new Point(1, 18);
            this.lvlVolumeToolStrip.Name = "lvlVolumeToolStrip";
            this.lvlVolumeToolStrip.Padding = new Padding(0);
            this.lvlVolumeToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlVolumeToolStrip.Size = new Size(29, 89);
            this.lvlVolumeToolStrip.Stretch = true;
            this.lvlVolumeToolStrip.TabIndex = 142;
            // 
            // btnLvlSeqAdd
            // 
            this.btnLvlSeqAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlSeqAdd.Enabled = false;
            this.btnLvlSeqAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlSeqAdd.ForeColor = Color.White;
            this.btnLvlSeqAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlSeqAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlSeqAdd.Margin = new Padding(0);
            this.btnLvlSeqAdd.Name = "btnLvlSeqAdd";
            this.btnLvlSeqAdd.Size = new Size(28, 24);
            this.btnLvlSeqAdd.ToolTipText = "Add new volume track";
            // 
            // btnLvlSeqDelete
            // 
            this.btnLvlSeqDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlSeqDelete.Enabled = false;
            this.btnLvlSeqDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlSeqDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlSeqDelete.Margin = new Padding(0);
            this.btnLvlSeqDelete.Name = "btnLvlSeqDelete";
            this.btnLvlSeqDelete.Size = new Size(28, 24);
            this.btnLvlSeqDelete.ToolTipText = "Delete selected volume track";
            // 
            // btnLvlSeqClear
            // 
            this.btnLvlSeqClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlSeqClear.Enabled = false;
            this.btnLvlSeqClear.Image = Properties.Resources.icon_erase;
            this.btnLvlSeqClear.ImageTransparentColor = Color.Magenta;
            this.btnLvlSeqClear.Name = "btnLvlSeqClear";
            this.btnLvlSeqClear.Size = new Size(28, 24);
            this.btnLvlSeqClear.ToolTipText = "Erase all set values";
            // 
            // lvlPathsToolStrip2
            // 
            this.lvlPathsToolStrip2.AutoSize = false;
            this.lvlPathsToolStrip2.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlPathsToolStrip2.Dock = DockStyle.None;
            this.lvlPathsToolStrip2.GripMargin = new Padding(0);
            this.lvlPathsToolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlPathsToolStrip2.ImageScalingSize = new Size(20, 20);
            this.lvlPathsToolStrip2.Items.AddRange(new ToolStripItem[] { this.btnLvlPathUp, this.btnLvlPathDown, this.btnLvlPathClear });
            this.lvlPathsToolStrip2.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lvlPathsToolStrip2.Location = new Point(34, 16);
            this.lvlPathsToolStrip2.Name = "lvlPathsToolStrip2";
            this.lvlPathsToolStrip2.Padding = new Padding(0);
            this.lvlPathsToolStrip2.RenderMode = ToolStripRenderMode.System;
            this.lvlPathsToolStrip2.Size = new Size(28, 188);
            this.lvlPathsToolStrip2.Stretch = true;
            this.lvlPathsToolStrip2.TabIndex = 145;
            // 
            // btnLvlPathUp
            // 
            this.btnLvlPathUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathUp.Enabled = false;
            this.btnLvlPathUp.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlPathUp.ForeColor = Color.White;
            this.btnLvlPathUp.Image = Properties.Resources.icon_arrowup2;
            this.btnLvlPathUp.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathUp.Margin = new Padding(0);
            this.btnLvlPathUp.Name = "btnLvlPathUp";
            this.btnLvlPathUp.Size = new Size(27, 24);
            this.btnLvlPathUp.ToolTipText = "Move selected tunnel up";
            // 
            // btnLvlPathDown
            // 
            this.btnLvlPathDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDown.Enabled = false;
            this.btnLvlPathDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlPathDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDown.Margin = new Padding(0);
            this.btnLvlPathDown.Name = "btnLvlPathDown";
            this.btnLvlPathDown.Size = new Size(27, 24);
            this.btnLvlPathDown.ToolTipText = "Move selected tunnel down";
            // 
            // btnLvlPathClear
            // 
            this.btnLvlPathClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathClear.Enabled = false;
            this.btnLvlPathClear.Image = Properties.Resources.icon_erase;
            this.btnLvlPathClear.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathClear.Name = "btnLvlPathClear";
            this.btnLvlPathClear.Size = new Size(27, 24);
            this.btnLvlPathClear.Text = "toolStripButton2";
            this.btnLvlPathClear.ToolTipText = "Clear all tunnels";
            // 
            // lvlLoopToolStrip
            // 
            this.lvlLoopToolStrip.AutoSize = false;
            this.lvlLoopToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopToolStrip.Dock = DockStyle.None;
            this.lvlLoopToolStrip.GripMargin = new Padding(0);
            this.lvlLoopToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlLoopToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlLoopToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLoopAdd, this.btnLvlLoopDelete });
            this.lvlLoopToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lvlLoopToolStrip.Location = new Point(326, 17);
            this.lvlLoopToolStrip.Name = "lvlLoopToolStrip";
            this.lvlLoopToolStrip.Padding = new Padding(0);
            this.lvlLoopToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlLoopToolStrip.Size = new Size(29, 58);
            this.lvlLoopToolStrip.Stretch = true;
            this.lvlLoopToolStrip.TabIndex = 144;
            // 
            // btnLvlLoopAdd
            // 
            this.btnLvlLoopAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLoopAdd.Enabled = false;
            this.btnLvlLoopAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlLoopAdd.ForeColor = Color.White;
            this.btnLvlLoopAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlLoopAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlLoopAdd.Margin = new Padding(0);
            this.btnLvlLoopAdd.Name = "btnLvlLoopAdd";
            this.btnLvlLoopAdd.Size = new Size(28, 24);
            this.btnLvlLoopAdd.ToolTipText = "Add new loop track";
            // 
            // btnLvlLoopDelete
            // 
            this.btnLvlLoopDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLoopDelete.Enabled = false;
            this.btnLvlLoopDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLoopDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLoopDelete.Margin = new Padding(0);
            this.btnLvlLoopDelete.Name = "btnLvlLoopDelete";
            this.btnLvlLoopDelete.Size = new Size(28, 24);
            this.btnLvlLoopDelete.ToolTipText = "Delete selected loop track";
            // 
            // lvlLeafPaths
            // 
            this.lvlLeafPaths.AllowUserToAddRows = false;
            this.lvlLeafPaths.AllowUserToDeleteRows = false;
            this.lvlLeafPaths.AllowUserToResizeColumns = false;
            this.lvlLeafPaths.AllowUserToResizeRows = false;
            this.lvlLeafPaths.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.lvlLeafPaths.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLeafPaths.BorderStyle = BorderStyle.None;
            this.lvlLeafPaths.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLeafPaths.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle7.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle7.SelectionForeColor = Color.White;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.lvlLeafPaths.ColumnHeadersHeight = 20;
            this.lvlLeafPaths.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle8.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.DefaultCellStyle = dataGridViewCellStyle8;
            this.lvlLeafPaths.EnableHeadersVisualStyles = false;
            this.lvlLeafPaths.GridColor = Color.Black;
            this.lvlLeafPaths.Location = new Point(64, 16);
            this.lvlLeafPaths.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafPaths.Name = "lvlLeafPaths";
            this.lvlLeafPaths.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle9.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.lvlLeafPaths.RowHeadersVisible = false;
            this.lvlLeafPaths.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLeafPaths.RowTemplate.Height = 20;
            this.lvlLeafPaths.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafPaths.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafPaths.Size = new Size(255, 280);
            this.lvlLeafPaths.TabIndex = 79;
            // 
            // lvlPathsToolStrip
            // 
            this.lvlPathsToolStrip.AutoSize = false;
            this.lvlPathsToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlPathsToolStrip.Dock = DockStyle.None;
            this.lvlPathsToolStrip.GripMargin = new Padding(0);
            this.lvlPathsToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlPathsToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlPathsToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlPathAdd, this.btnLvlPathDelete, this.btnLvlCopyTunnel, this.btnLvlPasteTunnel, this.chkTunnelCopy, this.btnLvlRandomTunnel });
            this.lvlPathsToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lvlPathsToolStrip.Location = new Point(5, 16);
            this.lvlPathsToolStrip.Name = "lvlPathsToolStrip";
            this.lvlPathsToolStrip.Padding = new Padding(0);
            this.lvlPathsToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlPathsToolStrip.Size = new Size(29, 188);
            this.lvlPathsToolStrip.Stretch = true;
            this.lvlPathsToolStrip.TabIndex = 143;
            // 
            // btnLvlPathAdd
            // 
            this.btnLvlPathAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathAdd.Enabled = false;
            this.btnLvlPathAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlPathAdd.ForeColor = Color.White;
            this.btnLvlPathAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlPathAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathAdd.Margin = new Padding(0);
            this.btnLvlPathAdd.Name = "btnLvlPathAdd";
            this.btnLvlPathAdd.Size = new Size(28, 24);
            this.btnLvlPathAdd.ToolTipText = "Add new path/tunnel";
            // 
            // btnLvlPathDelete
            // 
            this.btnLvlPathDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDelete.Enabled = false;
            this.btnLvlPathDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlPathDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDelete.Margin = new Padding(0);
            this.btnLvlPathDelete.Name = "btnLvlPathDelete";
            this.btnLvlPathDelete.Size = new Size(28, 24);
            this.btnLvlPathDelete.ToolTipText = "Delete selected path";
            // 
            // btnLvlCopyTunnel
            // 
            this.btnLvlCopyTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlCopyTunnel.Enabled = false;
            this.btnLvlCopyTunnel.Image = Properties.Resources.icon_copy2;
            this.btnLvlCopyTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlCopyTunnel.Name = "btnLvlCopyTunnel";
            this.btnLvlCopyTunnel.Size = new Size(28, 24);
            this.btnLvlCopyTunnel.ToolTipText = "Copy all paths/tunnels";
            // 
            // btnLvlPasteTunnel
            // 
            this.btnLvlPasteTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPasteTunnel.Enabled = false;
            this.btnLvlPasteTunnel.Image = Properties.Resources.icon_paste2;
            this.btnLvlPasteTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlPasteTunnel.Name = "btnLvlPasteTunnel";
            this.btnLvlPasteTunnel.Size = new Size(28, 24);
            this.btnLvlPasteTunnel.ToolTipText = "Paste copied paths/tunnels";
            // 
            // chkTunnelCopy
            // 
            this.chkTunnelCopy.CheckOnClick = true;
            this.chkTunnelCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.chkTunnelCopy.Image = Properties.Resources.icon_sling;
            this.chkTunnelCopy.ImageTransparentColor = Color.Magenta;
            this.chkTunnelCopy.Name = "chkTunnelCopy";
            this.chkTunnelCopy.Size = new Size(28, 24);
            this.chkTunnelCopy.ToolTipText = "When enabled, new leafs added will copy the paths\r\nof the previous leaf.";
            // 
            // btnLvlRandomTunnel
            // 
            this.btnLvlRandomTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlRandomTunnel.Enabled = false;
            this.btnLvlRandomTunnel.Image = Properties.Resources.icon_random;
            this.btnLvlRandomTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlRandomTunnel.Name = "btnLvlRandomTunnel";
            this.btnLvlRandomTunnel.Size = new Size(28, 24);
            this.btnLvlRandomTunnel.ToolTipText = "Click to add a random tunnel";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = Color.FromArgb(10, 10, 10);
            this.label22.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label22.ForeColor = Color.White;
            this.label22.Location = new Point(357, 1);
            this.label22.Margin = new Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new Size(99, 13);
            this.label22.TabIndex = 82;
            this.label22.Text = "Lvl Loop Tracks";
            // 
            // lvlLoopTracks
            // 
            this.lvlLoopTracks.AllowUserToAddRows = false;
            this.lvlLoopTracks.AllowUserToDeleteRows = false;
            this.lvlLoopTracks.AllowUserToResizeColumns = false;
            this.lvlLoopTracks.AllowUserToResizeRows = false;
            this.lvlLoopTracks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.lvlLoopTracks.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopTracks.BorderStyle = BorderStyle.None;
            this.lvlLoopTracks.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLoopTracks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle10.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = Color.White;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.lvlLoopTracks.ColumnHeadersHeight = 20;
            this.lvlLoopTracks.Columns.AddRange(new DataGridViewColumn[] { this.LoopSample, this.BeatsPerLoop });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle11.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle11.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.DefaultCellStyle = dataGridViewCellStyle11;
            this.lvlLoopTracks.EnableHeadersVisualStyles = false;
            this.lvlLoopTracks.GridColor = Color.Black;
            this.lvlLoopTracks.Location = new Point(357, 16);
            this.lvlLoopTracks.Margin = new Padding(4, 3, 4, 3);
            this.lvlLoopTracks.MultiSelect = false;
            this.lvlLoopTracks.Name = "lvlLoopTracks";
            this.lvlLoopTracks.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle12.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.lvlLoopTracks.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLoopTracks.RowTemplate.Height = 20;
            this.lvlLoopTracks.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLoopTracks.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.lvlLoopTracks.Size = new Size(286, 280);
            this.lvlLoopTracks.TabIndex = 81;
            // 
            // LoopSample
            // 
            this.LoopSample.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.LoopSample.HeaderText = "Loop Sample";
            this.LoopSample.MaxDropDownItems = 20;
            this.LoopSample.Name = "LoopSample";
            // 
            // BeatsPerLoop
            // 
            this.BeatsPerLoop.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.BeatsPerLoop.HeaderText = "Beats Per Loop";
            this.BeatsPerLoop.Name = "BeatsPerLoop";
            this.BeatsPerLoop.Width = 115;
            // 
            // Form_LvlEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(933, 519);
            this.Controls.Add(this.panelLevel);
            this.DoubleBuffered = true;
            this.ForeColor = Color.FromArgb(150, 150, 255);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_LvlEditor";
            this.Text = "Lvl Editor";
            ((System.ComponentModel.ISupportInitialize)this.trackLvlVolumeZoom).EndInit();
            this.panelLevel.ResumeLayout(false);
            this.panelLevel.PerformLayout();
            this.lvlToolStrip.ResumeLayout(false);
            this.lvlToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.NUD_lvlVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.NUD_lvlApproach).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.lvlSeqObjs).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.lvlVolumeToolStrip.ResumeLayout(false);
            this.lvlVolumeToolStrip.PerformLayout();
            this.lvlPathsToolStrip2.ResumeLayout(false);
            this.lvlPathsToolStrip2.PerformLayout();
            this.lvlLoopToolStrip.ResumeLayout(false);
            this.lvlLoopToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).EndInit();
            this.lvlPathsToolStrip.ResumeLayout(false);
            this.lvlPathsToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelLevel;
        private System.Windows.Forms.ToolStrip lvlToolStrip;
        private System.Windows.Forms.ToolStripButton btnLvlLeafAdd;
        private System.Windows.Forms.ToolStripButton btnLvlLeafDelete;
        private System.Windows.Forms.ToolStripButton btnLvlLeafUp;
        private System.Windows.Forms.ToolStripButton btnLvlLeafDown;
        private System.Windows.Forms.ToolStripButton btnLvlLeafCopy;
        private System.Windows.Forms.ToolStripButton btnLvlLeafPaste;
        private System.Windows.Forms.ToolStripButton btnLvlRefreshBeats;
        private System.Windows.Forms.ToolStripButton btnLvlLeafRandom;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox dropLvlTutorial;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox dropLvlInput;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown NUD_lvlVolume;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown NUD_lvlApproach;
        private System.Windows.Forms.DataGridView lvlLeafList;
        private System.Windows.Forms.DataGridViewImageColumn lvlfiletype;
        private System.Windows.Forms.DataGridViewTextBoxColumn Leaf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Beats;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView lvlSeqObjs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TrackBar trackLvlVolumeZoom;
        private System.Windows.Forms.ToolStrip lvlVolumeToolStrip;
        private System.Windows.Forms.ToolStripButton btnLvlSeqAdd;
        private System.Windows.Forms.ToolStripButton btnLvlSeqDelete;
        private System.Windows.Forms.ToolStripButton btnLvlSeqClear;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ToolStrip lvlPathsToolStrip2;
        private System.Windows.Forms.ToolStripButton btnLvlPathUp;
        private System.Windows.Forms.ToolStripButton btnLvlPathDown;
        private System.Windows.Forms.ToolStripButton btnLvlPathClear;
        private System.Windows.Forms.ToolStrip lvlLoopToolStrip;
        private System.Windows.Forms.ToolStripButton btnLvlLoopAdd;
        private System.Windows.Forms.ToolStripButton btnLvlLoopDelete;
        private System.Windows.Forms.DataGridView lvlLeafPaths;
        private System.Windows.Forms.ToolStrip lvlPathsToolStrip;
        private System.Windows.Forms.ToolStripButton btnLvlPathAdd;
        private System.Windows.Forms.ToolStripButton btnLvlPathDelete;
        private System.Windows.Forms.ToolStripButton btnLvlCopyTunnel;
        private System.Windows.Forms.ToolStripButton btnLvlPasteTunnel;
        private System.Windows.Forms.ToolStripButton chkTunnelCopy;
        private System.Windows.Forms.ToolStripButton btnLvlRandomTunnel;
        private System.Windows.Forms.Label lblLvlTunnels;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridView lvlLoopTracks;
        private System.Windows.Forms.DataGridViewComboBoxColumn LoopSample;
        private System.Windows.Forms.DataGridViewTextBoxColumn BeatsPerLoop;
    }
}