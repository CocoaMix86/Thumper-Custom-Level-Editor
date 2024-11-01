namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_MasterEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MasterEditor));
            this.panelMaster = new System.Windows.Forms.Panel();
            this.masterLvlList = new System.Windows.Forms.DataGridView();
            this.masterfiletype = new System.Windows.Forms.DataGridViewImageColumn();
            this.masterLvl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterCheckpoint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterPlayplus = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterIsolate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolstripTitleMaster = new System.Windows.Forms.ToolStrip();
            this.lblMasterName = new System.Windows.Forms.ToolStripLabel();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnDock = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnRevert = new System.Windows.Forms.ToolStripButton();
            this.label30 = new System.Windows.Forms.Label();
            this.lblMasterlvllistHelp = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblMasterRuntime = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblMAsterRuntimeBeats = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.btnMasterRuntime = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.lblConfigColorHelp = new System.Windows.Forms.Label();
            this.dropMasterSkybox = new System.Windows.Forms.ComboBox();
            this.dropMasterIntro = new System.Windows.Forms.ComboBox();
            this.dropMasterCheck = new System.Windows.Forms.ComboBox();
            this.btnConfigRailColor = new System.Windows.Forms.Button();
            this.label37 = new System.Windows.Forms.Label();
            this.btnConfigGlowColor = new System.Windows.Forms.Button();
            this.dropMasterLvlRest = new System.Windows.Forms.ComboBox();
            this.btnConfigPathColor = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.NUD_ConfigBPM = new System.Windows.Forms.NumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.btnMasterOpenIntro = new System.Windows.Forms.Button();
            this.btnMasterOpenCheckpoint = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.btnMasterOpenRest = new System.Windows.Forms.Button();
            this.masterToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnMasterLvlAdd = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlDelete = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlUp = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlDown = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlCopy = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlPaste = new System.Windows.Forms.ToolStripButton();
            this.panelMover = new System.Windows.Forms.Panel();
            this.panelDockOptions = new System.Windows.Forms.Panel();
            this.btnDock1 = new System.Windows.Forms.Button();
            this.btnDock2 = new System.Windows.Forms.Button();
            this.btnDock3 = new System.Windows.Forms.Button();
            this.btnDock4 = new System.Windows.Forms.Button();
            this.btnDock5 = new System.Windows.Forms.Button();
            this.btnDock7 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterLvlList)).BeginInit();
            this.toolstripTitleMaster.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ConfigBPM)).BeginInit();
            this.masterToolStrip.SuspendLayout();
            this.panelDockOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMaster
            // 
            this.panelMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.panelMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMaster.Controls.Add(this.panelDockOptions);
            this.panelMaster.Controls.Add(this.panelMover);
            this.panelMaster.Controls.Add(this.masterLvlList);
            this.panelMaster.Controls.Add(this.toolstripTitleMaster);
            this.panelMaster.Controls.Add(this.label30);
            this.panelMaster.Controls.Add(this.lblMasterlvllistHelp);
            this.panelMaster.Controls.Add(this.panel5);
            this.panelMaster.Controls.Add(this.masterToolStrip);
            this.panelMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMaster.Location = new System.Drawing.Point(0, 0);
            this.panelMaster.MinimumSize = new System.Drawing.Size(60, 60);
            this.panelMaster.Name = "panelMaster";
            this.panelMaster.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelMaster.Size = new System.Drawing.Size(800, 450);
            this.panelMaster.TabIndex = 48;
            this.panelMaster.Tag = "editorpanel";
            // 
            // masterLvlList
            // 
            this.masterLvlList.AllowUserToAddRows = false;
            this.masterLvlList.AllowUserToDeleteRows = false;
            this.masterLvlList.AllowUserToResizeColumns = false;
            this.masterLvlList.AllowUserToResizeRows = false;
            this.masterLvlList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterLvlList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.masterLvlList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.masterLvlList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.masterLvlList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.masterLvlList.ColumnHeadersHeight = 20;
            this.masterLvlList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.masterLvlList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.masterfiletype,
            this.masterLvl,
            this.masterCheckpoint,
            this.masterPlayplus,
            this.masterIsolate});
            this.masterLvlList.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle26.NullValue = null;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.DefaultCellStyle = dataGridViewCellStyle26;
            this.masterLvlList.EnableHeadersVisualStyles = false;
            this.masterLvlList.GridColor = System.Drawing.Color.Black;
            this.masterLvlList.Location = new System.Drawing.Point(3, 39);
            this.masterLvlList.Name = "masterLvlList";
            this.masterLvlList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.masterLvlList.RowHeadersVisible = false;
            this.masterLvlList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.masterLvlList.RowTemplate.Height = 20;
            this.masterLvlList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.masterLvlList.Size = new System.Drawing.Size(618, 378);
            this.masterLvlList.TabIndex = 79;
            this.masterLvlList.Tag = "editorpaneldgv";
            // 
            // masterfiletype
            // 
            this.masterfiletype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.masterfiletype.HeaderText = "";
            this.masterfiletype.Name = "masterfiletype";
            this.masterfiletype.ReadOnly = true;
            this.masterfiletype.Width = 5;
            // 
            // masterLvl
            // 
            this.masterLvl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.masterLvl.HeaderText = "Sublevel";
            this.masterLvl.Name = "masterLvl";
            this.masterLvl.ReadOnly = true;
            this.masterLvl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // masterCheckpoint
            // 
            this.masterCheckpoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterCheckpoint.HeaderText = "Chkp.";
            this.masterCheckpoint.Name = "masterCheckpoint";
            this.masterCheckpoint.Width = 44;
            // 
            // masterPlayplus
            // 
            this.masterPlayplus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterPlayplus.HeaderText = "P+";
            this.masterPlayplus.Name = "masterPlayplus";
            this.masterPlayplus.Width = 25;
            // 
            // masterIsolate
            // 
            this.masterIsolate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterIsolate.HeaderText = "Iso.";
            this.masterIsolate.Name = "masterIsolate";
            this.masterIsolate.Width = 32;
            // 
            // toolstripTitleMaster
            // 
            this.toolstripTitleMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.toolstripTitleMaster.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolstripTitleMaster.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripTitleMaster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMasterName,
            this.btnSave,
            this.btnClose,
            this.btnDock,
            this.btnRefresh,
            this.btnRevert});
            this.toolstripTitleMaster.Location = new System.Drawing.Point(0, 0);
            this.toolstripTitleMaster.MaximumSize = new System.Drawing.Size(0, 50);
            this.toolstripTitleMaster.Name = "toolstripTitleMaster";
            this.toolstripTitleMaster.Padding = new System.Windows.Forms.Padding(0);
            this.toolstripTitleMaster.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolstripTitleMaster.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolstripTitleMaster.Size = new System.Drawing.Size(798, 25);
            this.toolstripTitleMaster.Stretch = true;
            this.toolstripTitleMaster.TabIndex = 139;
            this.toolstripTitleMaster.Text = "titlebar";
            // 
            // lblMasterName
            // 
            this.lblMasterName.DoubleClickEnabled = true;
            this.lblMasterName.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.lblMasterName.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.master;
            this.lblMasterName.Name = "lblMasterName";
            this.lblMasterName.Size = new System.Drawing.Size(98, 22);
            this.lblMasterName.Text = "Master Editor";
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_save2;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.ToolTipText = "Save master file";
            // 
            // btnClose
            // 
            this.btnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Margin = new System.Windows.Forms.Padding(-2, 0, 0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 25);
            this.btnClose.Text = "toolStripButton6";
            this.btnClose.ToolTipText = "Close panel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDock
            // 
            this.btnDock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnDock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDock.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowupdock;
            this.btnDock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDock.Margin = new System.Windows.Forms.Padding(-2, 0, 0, 0);
            this.btnDock.Name = "btnDock";
            this.btnDock.Size = new System.Drawing.Size(23, 25);
            this.btnDock.Text = "Dock panel";
            this.btnDock.ToolTipText = "Dock panel";
            this.btnDock.Click += new System.EventHandler(this.lblPopoutMaster_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_refresh2;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.ToolTipText = "Reload the lvl lists on this panel";
            // 
            // btnRevert
            // 
            this.btnRevert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRevert.Enabled = false;
            this.btnRevert.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_back;
            this.btnRevert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(23, 22);
            this.btnRevert.ToolTipText = "Revert changes to last save";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.White;
            this.label30.Location = new System.Drawing.Point(3, 26);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 13);
            this.label30.TabIndex = 94;
            this.label30.Text = "Lvl/Gate List";
            // 
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblMasterlvllistHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterlvllistHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new System.Drawing.Point(606, 23);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new System.Drawing.Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.AutoScroll = true;
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.lblMasterRuntime);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.lblMAsterRuntimeBeats);
            this.panel5.Controls.Add(this.label31);
            this.panel5.Controls.Add(this.label21);
            this.panel5.Controls.Add(this.label32);
            this.panel5.Controls.Add(this.btnMasterRuntime);
            this.panel5.Controls.Add(this.label33);
            this.panel5.Controls.Add(this.lblConfigColorHelp);
            this.panel5.Controls.Add(this.dropMasterSkybox);
            this.panel5.Controls.Add(this.dropMasterIntro);
            this.panel5.Controls.Add(this.dropMasterCheck);
            this.panel5.Controls.Add(this.btnConfigRailColor);
            this.panel5.Controls.Add(this.label37);
            this.panel5.Controls.Add(this.btnConfigGlowColor);
            this.panel5.Controls.Add(this.dropMasterLvlRest);
            this.panel5.Controls.Add(this.btnConfigPathColor);
            this.panel5.Controls.Add(this.label35);
            this.panel5.Controls.Add(this.NUD_ConfigBPM);
            this.panel5.Controls.Add(this.label27);
            this.panel5.Controls.Add(this.btnMasterOpenIntro);
            this.panel5.Controls.Add(this.btnMasterOpenCheckpoint);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label56);
            this.panel5.Controls.Add(this.label47);
            this.panel5.Controls.Add(this.btnMasterOpenRest);
            this.panel5.Location = new System.Drawing.Point(623, 22);
            this.panel5.Name = "panel5";
            this.panel5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel5.Size = new System.Drawing.Size(174, 414);
            this.panel5.TabIndex = 147;
            // 
            // lblMasterRuntime
            // 
            this.lblMasterRuntime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMasterRuntime.AutoSize = true;
            this.lblMasterRuntime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterRuntime.ForeColor = System.Drawing.Color.White;
            this.lblMasterRuntime.Location = new System.Drawing.Point(39, 263);
            this.lblMasterRuntime.Name = "lblMasterRuntime";
            this.lblMasterRuntime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMasterRuntime.Size = new System.Drawing.Size(38, 15);
            this.lblMasterRuntime.TabIndex = 146;
            this.lblMasterRuntime.Text = "Time:";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(30, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 15);
            this.label13.TabIndex = 96;
            this.label13.Text = "══Master Options══";
            // 
            // lblMAsterRuntimeBeats
            // 
            this.lblMAsterRuntimeBeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMAsterRuntimeBeats.AutoSize = true;
            this.lblMAsterRuntimeBeats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMAsterRuntimeBeats.ForeColor = System.Drawing.Color.White;
            this.lblMAsterRuntimeBeats.Location = new System.Drawing.Point(39, 248);
            this.lblMAsterRuntimeBeats.Name = "lblMAsterRuntimeBeats";
            this.lblMAsterRuntimeBeats.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMAsterRuntimeBeats.Size = new System.Drawing.Size(41, 15);
            this.lblMAsterRuntimeBeats.TabIndex = 145;
            this.lblMAsterRuntimeBeats.Text = "Beats:";
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.White;
            this.label31.Location = new System.Drawing.Point(64, 20);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(46, 15);
            this.label31.TabIndex = 97;
            this.label31.Text = "Skybox";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Silver;
            this.label21.Location = new System.Drawing.Point(48, 234);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(78, 15);
            this.label21.TabIndex = 144;
            this.label21.Text = "══Runtime══";
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(63, 58);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(49, 15);
            this.label32.TabIndex = 99;
            this.label32.Text = "Intro Lvl";
            // 
            // btnMasterRuntime
            // 
            this.btnMasterRuntime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterRuntime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterRuntime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMasterRuntime.ForeColor = System.Drawing.Color.Green;
            this.btnMasterRuntime.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterRuntime.Image")));
            this.btnMasterRuntime.Location = new System.Drawing.Point(18, 254);
            this.btnMasterRuntime.Name = "btnMasterRuntime";
            this.btnMasterRuntime.Padding = new System.Windows.Forms.Padding(0, 0, 2, 1);
            this.btnMasterRuntime.Size = new System.Drawing.Size(20, 20);
            this.btnMasterRuntime.TabIndex = 143;
            this.btnMasterRuntime.UseVisualStyleBackColor = true;
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(44, 97);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(86, 15);
            this.label33.TabIndex = 101;
            this.label33.Text = "Checkpoint Lvl";
            // 
            // lblConfigColorHelp
            // 
            this.lblConfigColorHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigColorHelp.AutoSize = true;
            this.lblConfigColorHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblConfigColorHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblConfigColorHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigColorHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblConfigColorHelp.Location = new System.Drawing.Point(158, 166);
            this.lblConfigColorHelp.Name = "lblConfigColorHelp";
            this.lblConfigColorHelp.Size = new System.Drawing.Size(15, 16);
            this.lblConfigColorHelp.TabIndex = 112;
            this.lblConfigColorHelp.Text = "?";
            // 
            // dropMasterSkybox
            // 
            this.dropMasterSkybox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropMasterSkybox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropMasterSkybox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropMasterSkybox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropMasterSkybox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropMasterSkybox.ForeColor = System.Drawing.Color.White;
            this.dropMasterSkybox.FormattingEnabled = true;
            this.dropMasterSkybox.Items.AddRange(new object[] {
            "<none>",
            "skybox_cube"});
            this.dropMasterSkybox.Location = new System.Drawing.Point(20, 36);
            this.dropMasterSkybox.Name = "dropMasterSkybox";
            this.dropMasterSkybox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropMasterSkybox.Size = new System.Drawing.Size(131, 21);
            this.dropMasterSkybox.TabIndex = 95;
            // 
            // dropMasterIntro
            // 
            this.dropMasterIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropMasterIntro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropMasterIntro.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropMasterIntro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropMasterIntro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropMasterIntro.ForeColor = System.Drawing.Color.White;
            this.dropMasterIntro.FormattingEnabled = true;
            this.dropMasterIntro.Items.AddRange(new object[] {
            "<none>"});
            this.dropMasterIntro.Location = new System.Drawing.Point(20, 73);
            this.dropMasterIntro.Name = "dropMasterIntro";
            this.dropMasterIntro.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropMasterIntro.Size = new System.Drawing.Size(131, 21);
            this.dropMasterIntro.TabIndex = 98;
            // 
            // dropMasterCheck
            // 
            this.dropMasterCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropMasterCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropMasterCheck.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropMasterCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropMasterCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropMasterCheck.ForeColor = System.Drawing.Color.White;
            this.dropMasterCheck.FormattingEnabled = true;
            this.dropMasterCheck.Items.AddRange(new object[] {
            "<none>"});
            this.dropMasterCheck.Location = new System.Drawing.Point(20, 113);
            this.dropMasterCheck.Name = "dropMasterCheck";
            this.dropMasterCheck.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropMasterCheck.Size = new System.Drawing.Size(131, 21);
            this.dropMasterCheck.TabIndex = 100;
            // 
            // btnConfigRailColor
            // 
            this.btnConfigRailColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigRailColor.BackColor = System.Drawing.Color.White;
            this.btnConfigRailColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfigRailColor.Location = new System.Drawing.Point(104, 163);
            this.btnConfigRailColor.Name = "btnConfigRailColor";
            this.btnConfigRailColor.Size = new System.Drawing.Size(55, 23);
            this.btnConfigRailColor.TabIndex = 96;
            this.btnConfigRailColor.UseVisualStyleBackColor = false;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Silver;
            this.label37.Location = new System.Drawing.Point(26, 287);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(123, 15);
            this.label37.TabIndex = 116;
            this.label37.Text = "══Sublevel Options══";
            // 
            // btnConfigGlowColor
            // 
            this.btnConfigGlowColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigGlowColor.BackColor = System.Drawing.Color.White;
            this.btnConfigGlowColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfigGlowColor.Location = new System.Drawing.Point(104, 186);
            this.btnConfigGlowColor.Name = "btnConfigGlowColor";
            this.btnConfigGlowColor.Size = new System.Drawing.Size(55, 23);
            this.btnConfigGlowColor.TabIndex = 108;
            this.btnConfigGlowColor.UseVisualStyleBackColor = false;
            // 
            // dropMasterLvlRest
            // 
            this.dropMasterLvlRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropMasterLvlRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropMasterLvlRest.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropMasterLvlRest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropMasterLvlRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropMasterLvlRest.ForeColor = System.Drawing.Color.White;
            this.dropMasterLvlRest.FormattingEnabled = true;
            this.dropMasterLvlRest.Items.AddRange(new object[] {
            "<none>"});
            this.dropMasterLvlRest.Location = new System.Drawing.Point(20, 320);
            this.dropMasterLvlRest.Name = "dropMasterLvlRest";
            this.dropMasterLvlRest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropMasterLvlRest.Size = new System.Drawing.Size(131, 21);
            this.dropMasterLvlRest.TabIndex = 105;
            // 
            // btnConfigPathColor
            // 
            this.btnConfigPathColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigPathColor.BackColor = System.Drawing.Color.White;
            this.btnConfigPathColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfigPathColor.Location = new System.Drawing.Point(104, 209);
            this.btnConfigPathColor.Name = "btnConfigPathColor";
            this.btnConfigPathColor.Size = new System.Drawing.Size(55, 23);
            this.btnConfigPathColor.TabIndex = 109;
            this.btnConfigPathColor.UseVisualStyleBackColor = false;
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(62, 304);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(50, 15);
            this.label35.TabIndex = 104;
            this.label35.Text = "Rest Lvl";
            // 
            // NUD_ConfigBPM
            // 
            this.NUD_ConfigBPM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_ConfigBPM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.NUD_ConfigBPM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NUD_ConfigBPM.DecimalPlaces = 2;
            this.NUD_ConfigBPM.ForeColor = System.Drawing.Color.White;
            this.NUD_ConfigBPM.Location = new System.Drawing.Point(54, 140);
            this.NUD_ConfigBPM.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NUD_ConfigBPM.Name = "NUD_ConfigBPM";
            this.NUD_ConfigBPM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NUD_ConfigBPM.Size = new System.Drawing.Size(83, 20);
            this.NUD_ConfigBPM.TabIndex = 96;
            this.NUD_ConfigBPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NUD_ConfigBPM.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(18, 142);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(34, 15);
            this.label27.TabIndex = 111;
            this.label27.Text = "BPM";
            // 
            // btnMasterOpenIntro
            // 
            this.btnMasterOpenIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterOpenIntro.BackColor = System.Drawing.Color.Gray;
            this.btnMasterOpenIntro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterOpenIntro.Enabled = false;
            this.btnMasterOpenIntro.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMasterOpenIntro.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterOpenIntro.ForeColor = System.Drawing.Color.Black;
            this.btnMasterOpenIntro.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterOpenIntro.Image")));
            this.btnMasterOpenIntro.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMasterOpenIntro.Location = new System.Drawing.Point(149, 72);
            this.btnMasterOpenIntro.Name = "btnMasterOpenIntro";
            this.btnMasterOpenIntro.Size = new System.Drawing.Size(23, 23);
            this.btnMasterOpenIntro.TabIndex = 114;
            this.btnMasterOpenIntro.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMasterOpenIntro.UseVisualStyleBackColor = false;
            // 
            // btnMasterOpenCheckpoint
            // 
            this.btnMasterOpenCheckpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterOpenCheckpoint.BackColor = System.Drawing.Color.Gray;
            this.btnMasterOpenCheckpoint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterOpenCheckpoint.Enabled = false;
            this.btnMasterOpenCheckpoint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMasterOpenCheckpoint.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterOpenCheckpoint.ForeColor = System.Drawing.Color.Black;
            this.btnMasterOpenCheckpoint.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterOpenCheckpoint.Image")));
            this.btnMasterOpenCheckpoint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMasterOpenCheckpoint.Location = new System.Drawing.Point(149, 112);
            this.btnMasterOpenCheckpoint.Name = "btnMasterOpenCheckpoint";
            this.btnMasterOpenCheckpoint.Size = new System.Drawing.Size(23, 23);
            this.btnMasterOpenCheckpoint.TabIndex = 115;
            this.btnMasterOpenCheckpoint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMasterOpenCheckpoint.UseVisualStyleBackColor = false;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(39, 212);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 15);
            this.label12.TabIndex = 140;
            this.label12.Text = "Path Color";
            // 
            // label56
            // 
            this.label56.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label56.ForeColor = System.Drawing.Color.White;
            this.label56.Location = new System.Drawing.Point(12, 189);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(89, 15);
            this.label56.TabIndex = 142;
            this.label56.Text = "RailGlow Color";
            // 
            // label47
            // 
            this.label47.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.Color.White;
            this.label47.Location = new System.Drawing.Point(42, 167);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(61, 15);
            this.label47.TabIndex = 141;
            this.label47.Text = "Rail Color";
            // 
            // btnMasterOpenRest
            // 
            this.btnMasterOpenRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterOpenRest.BackColor = System.Drawing.Color.Gray;
            this.btnMasterOpenRest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterOpenRest.Enabled = false;
            this.btnMasterOpenRest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMasterOpenRest.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterOpenRest.ForeColor = System.Drawing.Color.Black;
            this.btnMasterOpenRest.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterOpenRest.Image")));
            this.btnMasterOpenRest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMasterOpenRest.Location = new System.Drawing.Point(149, 319);
            this.btnMasterOpenRest.Name = "btnMasterOpenRest";
            this.btnMasterOpenRest.Size = new System.Drawing.Size(23, 23);
            this.btnMasterOpenRest.TabIndex = 118;
            this.btnMasterOpenRest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMasterOpenRest.UseVisualStyleBackColor = false;
            // 
            // masterToolStrip
            // 
            this.masterToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterToolStrip.AutoSize = false;
            this.masterToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.masterToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.masterToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.masterToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.masterToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.masterToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMasterLvlAdd,
            this.btnMasterLvlDelete,
            this.btnMasterLvlUp,
            this.btnMasterLvlDown,
            this.btnMasterLvlCopy,
            this.btnMasterLvlPaste});
            this.masterToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.masterToolStrip.Location = new System.Drawing.Point(3, 419);
            this.masterToolStrip.Name = "masterToolStrip";
            this.masterToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.masterToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.masterToolStrip.Size = new System.Drawing.Size(618, 25);
            this.masterToolStrip.Stretch = true;
            this.masterToolStrip.TabIndex = 138;
            // 
            // btnMasterLvlAdd
            // 
            this.btnMasterLvlAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterLvlAdd.ForeColor = System.Drawing.Color.White;
            this.btnMasterLvlAdd.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_plus;
            this.btnMasterLvlAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlAdd.Name = "btnMasterLvlAdd";
            this.btnMasterLvlAdd.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlAdd.ToolTipText = "Add new sublevel to the list";
            // 
            // btnMasterLvlDelete
            // 
            this.btnMasterLvlDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDelete.Enabled = false;
            this.btnMasterLvlDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.btnMasterLvlDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlDelete.Name = "btnMasterLvlDelete";
            this.btnMasterLvlDelete.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlDelete.ToolTipText = "Delete selected sublevel from this list";
            // 
            // btnMasterLvlUp
            // 
            this.btnMasterLvlUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlUp.Enabled = false;
            this.btnMasterLvlUp.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowup2;
            this.btnMasterLvlUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlUp.Name = "btnMasterLvlUp";
            this.btnMasterLvlUp.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlUp.ToolTipText = "Move selected sublevel up";
            // 
            // btnMasterLvlDown
            // 
            this.btnMasterLvlDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDown.Enabled = false;
            this.btnMasterLvlDown.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowdown2;
            this.btnMasterLvlDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlDown.Name = "btnMasterLvlDown";
            this.btnMasterLvlDown.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlDown.ToolTipText = "Move selected sublevel down";
            // 
            // btnMasterLvlCopy
            // 
            this.btnMasterLvlCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlCopy.Enabled = false;
            this.btnMasterLvlCopy.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.btnMasterLvlCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlCopy.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlCopy.Name = "btnMasterLvlCopy";
            this.btnMasterLvlCopy.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlCopy.ToolTipText = "Copy selected sublevel";
            // 
            // btnMasterLvlPaste
            // 
            this.btnMasterLvlPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlPaste.Enabled = false;
            this.btnMasterLvlPaste.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_paste2;
            this.btnMasterLvlPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlPaste.Name = "btnMasterLvlPaste";
            this.btnMasterLvlPaste.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlPaste.ToolTipText = "Paste the copied sublevel";
            // 
            // panelMover
            // 
            this.panelMover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelMover.Location = new System.Drawing.Point(142, 0);
            this.panelMover.Name = "panelMover";
            this.panelMover.Size = new System.Drawing.Size(590, 23);
            this.panelMover.TabIndex = 147;
            // 
            // panelDockOptions
            // 
            this.panelDockOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDockOptions.BackColor = System.Drawing.Color.Black;
            this.panelDockOptions.Controls.Add(this.button1);
            this.panelDockOptions.Controls.Add(this.btnDock7);
            this.panelDockOptions.Controls.Add(this.btnDock5);
            this.panelDockOptions.Controls.Add(this.btnDock4);
            this.panelDockOptions.Controls.Add(this.btnDock3);
            this.panelDockOptions.Controls.Add(this.btnDock2);
            this.panelDockOptions.Controls.Add(this.btnDock1);
            this.panelDockOptions.Location = new System.Drawing.Point(697, 25);
            this.panelDockOptions.Name = "panelDockOptions";
            this.panelDockOptions.Size = new System.Drawing.Size(102, 54);
            this.panelDockOptions.TabIndex = 148;
            this.panelDockOptions.Visible = false;
            // 
            // btnDock1
            // 
            this.btnDock1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock1.ForeColor = System.Drawing.Color.White;
            this.btnDock1.Location = new System.Drawing.Point(4, 3);
            this.btnDock1.Name = "btnDock1";
            this.btnDock1.Size = new System.Drawing.Size(24, 24);
            this.btnDock1.TabIndex = 0;
            this.btnDock1.Text = "1";
            this.btnDock1.UseVisualStyleBackColor = true;
            this.btnDock1.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock1.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock1.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // btnDock2
            // 
            this.btnDock2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock2.ForeColor = System.Drawing.Color.White;
            this.btnDock2.Location = new System.Drawing.Point(27, 3);
            this.btnDock2.Name = "btnDock2";
            this.btnDock2.Size = new System.Drawing.Size(24, 24);
            this.btnDock2.TabIndex = 1;
            this.btnDock2.Text = "2";
            this.btnDock2.UseVisualStyleBackColor = true;
            this.btnDock2.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock2.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock2.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // btnDock3
            // 
            this.btnDock3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock3.ForeColor = System.Drawing.Color.White;
            this.btnDock3.Location = new System.Drawing.Point(50, 3);
            this.btnDock3.Name = "btnDock3";
            this.btnDock3.Size = new System.Drawing.Size(24, 24);
            this.btnDock3.TabIndex = 2;
            this.btnDock3.Text = "3";
            this.btnDock3.UseVisualStyleBackColor = true;
            this.btnDock3.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock3.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock3.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // btnDock4
            // 
            this.btnDock4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock4.ForeColor = System.Drawing.Color.White;
            this.btnDock4.Location = new System.Drawing.Point(4, 26);
            this.btnDock4.Name = "btnDock4";
            this.btnDock4.Size = new System.Drawing.Size(24, 24);
            this.btnDock4.TabIndex = 3;
            this.btnDock4.Text = "4";
            this.btnDock4.UseVisualStyleBackColor = true;
            this.btnDock4.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock4.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock4.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // btnDock5
            // 
            this.btnDock5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock5.ForeColor = System.Drawing.Color.White;
            this.btnDock5.Location = new System.Drawing.Point(27, 26);
            this.btnDock5.Name = "btnDock5";
            this.btnDock5.Size = new System.Drawing.Size(24, 24);
            this.btnDock5.TabIndex = 4;
            this.btnDock5.Text = "5";
            this.btnDock5.UseVisualStyleBackColor = true;
            this.btnDock5.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock5.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock5.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // btnDock7
            // 
            this.btnDock7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDock7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDock7.ForeColor = System.Drawing.Color.White;
            this.btnDock7.Location = new System.Drawing.Point(50, 26);
            this.btnDock7.Name = "btnDock7";
            this.btnDock7.Size = new System.Drawing.Size(24, 24);
            this.btnDock7.TabIndex = 5;
            this.btnDock7.Text = "6";
            this.btnDock7.UseVisualStyleBackColor = true;
            this.btnDock7.Click += new System.EventHandler(this.btnDock_Click);
            this.btnDock7.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.btnDock7.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(75, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "0";
            this.toolTip1.SetToolTip(this.button1, "Undock");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnDock_Click);
            this.button1.MouseEnter += new System.EventHandler(this.btnDock1_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.btnDock1_MouseLeave);
            // 
            // Form_LeafEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.panelMaster);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_LeafEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panelMaster.ResumeLayout(false);
            this.panelMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterLvlList)).EndInit();
            this.toolstripTitleMaster.ResumeLayout(false);
            this.toolstripTitleMaster.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ConfigBPM)).EndInit();
            this.masterToolStrip.ResumeLayout(false);
            this.masterToolStrip.PerformLayout();
            this.panelDockOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMaster;
        private System.Windows.Forms.DataGridView masterLvlList;
        private System.Windows.Forms.DataGridViewImageColumn masterfiletype;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterLvl;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterCheckpoint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterPlayplus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterIsolate;
        private System.Windows.Forms.ToolStrip toolstripTitleMaster;
        private System.Windows.Forms.ToolStripLabel lblMasterName;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripButton btnDock;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnRevert;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lblMasterlvllistHelp;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblMasterRuntime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblMAsterRuntimeBeats;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button btnMasterRuntime;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label lblConfigColorHelp;
        private System.Windows.Forms.ComboBox dropMasterSkybox;
        private System.Windows.Forms.ComboBox dropMasterIntro;
        private System.Windows.Forms.ComboBox dropMasterCheck;
        private System.Windows.Forms.Button btnConfigRailColor;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button btnConfigGlowColor;
        private System.Windows.Forms.ComboBox dropMasterLvlRest;
        private System.Windows.Forms.Button btnConfigPathColor;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown NUD_ConfigBPM;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnMasterOpenIntro;
        private System.Windows.Forms.Button btnMasterOpenCheckpoint;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button btnMasterOpenRest;
        private System.Windows.Forms.ToolStrip masterToolStrip;
        private System.Windows.Forms.ToolStripButton btnMasterLvlAdd;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDelete;
        private System.Windows.Forms.ToolStripButton btnMasterLvlUp;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDown;
        private System.Windows.Forms.ToolStripButton btnMasterLvlCopy;
        private System.Windows.Forms.ToolStripButton btnMasterLvlPaste;
        private System.Windows.Forms.Panel panelMover;
        private System.Windows.Forms.Panel panelDockOptions;
        private System.Windows.Forms.Button btnDock7;
        private System.Windows.Forms.Button btnDock5;
        private System.Windows.Forms.Button btnDock4;
        private System.Windows.Forms.Button btnDock3;
        private System.Windows.Forms.Button btnDock2;
        private System.Windows.Forms.Button btnDock1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}