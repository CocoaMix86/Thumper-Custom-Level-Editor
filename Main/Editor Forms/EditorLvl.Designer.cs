namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class EditorLvl
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorLvl));
            this.toolTip1 = new ToolTip(this.components);
            this.lvlLeafList = new DataGridView();
            this.lvlLeafIcon = new DataGridViewImageColumn();
            this.LeafName = new DataGridViewTextBoxColumn();
            this.Runtime = new DataGridViewTextBoxColumn();
            this.btnLvlSequencer = new Button();
            this.lvlToolStrip = new ToolStrip();
            this.btnLvlLeafAdd = new ToolStripButton();
            this.btnLvlLeafDelete = new ToolStripButton();
            this.btnLvlLeafUp = new ToolStripButton();
            this.btnLvlLeafDown = new ToolStripButton();
            this.btnLvlLeafCopy = new ToolStripButton();
            this.btnLvlLeafPaste = new ToolStripButton();
            this.btnLvlLeafRandom = new ToolStripButton();
            this.btnLvlPlayback = new ToolStripButton();
            this.lvlLeafPaths = new DataGridView();
            this.columnLvlLeafPaths = new DataGridViewTextBoxColumn();
            this.lvlPathsToolStrip = new ToolStrip();
            this.btnLvlPathAdd = new ToolStripButton();
            this.btnLvlPathDelete = new ToolStripButton();
            this.btnLvlPathUp = new ToolStripButton();
            this.btnLvlPathDown = new ToolStripButton();
            this.btnLvlCopyTunnel = new ToolStripButton();
            this.btnLvlPasteTunnel = new ToolStripButton();
            this.btnLvlPathClear = new ToolStripButton();
            this.chkTunnelCopy = new ToolStripButton();
            this.btnLvlRandomTunnel = new ToolStripButton();
            this.btnLvlPathView = new ToolStripButton();
            this.lvlLoopTracks = new DataGridView();
            this.LvlLoopPlay = new DataGridViewButtonColumn();
            this.LoopSample = new DataGridViewComboBoxColumn();
            this.BeatsPerLoop = new DataGridViewTextBoxColumn();
            this.lvlLoopToolStrip = new ToolStrip();
            this.btnLvlLoopAdd = new ToolStripButton();
            this.btnLvlLoopDelete = new ToolStripButton();
            this.panelMain = new Panel();
            this.panelTunnel = new Panel();
            this.panelLoop = new Panel();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).BeginInit();
            this.lvlToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).BeginInit();
            this.lvlPathsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).BeginInit();
            this.lvlLoopToolStrip.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelTunnel.SuspendLayout();
            this.panelLoop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvlLeafList
            // 
            this.lvlLeafList.AllowDrop = true;
            this.lvlLeafList.AllowUserToAddRows = false;
            this.lvlLeafList.AllowUserToDeleteRows = false;
            this.lvlLeafList.AllowUserToResizeRows = false;
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
            this.lvlLeafList.Columns.AddRange(new DataGridViewColumn[] { this.lvlLeafIcon, this.LeafName, this.Runtime });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle4.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.DefaultCellStyle = dataGridViewCellStyle4;
            this.lvlLeafList.Dock = DockStyle.Fill;
            this.lvlLeafList.EnableHeadersVisualStyles = false;
            this.lvlLeafList.GridColor = Color.Black;
            this.lvlLeafList.Location = new Point(24, 0);
            this.lvlLeafList.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafList.Name = "lvlLeafList";
            this.lvlLeafList.ReadOnly = true;
            this.lvlLeafList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.lvlLeafList.RowHeadersVisible = false;
            this.lvlLeafList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.BackColor = Color.Green;
            dataGridViewCellStyle6.Font = new Font("Relay-Medium", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.White;
            this.lvlLeafList.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.lvlLeafList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
            this.lvlLeafList.RowTemplate.DefaultCellStyle.Font = new Font("Relay-Medium", 8.249999F);
            this.lvlLeafList.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLeafList.RowTemplate.Height = 20;
            this.lvlLeafList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafList.Size = new Size(234, 219);
            this.lvlLeafList.TabIndex = 152;
            this.lvlLeafList.Tag = "editorpaneldgv";
            this.lvlLeafList.CellClick += this.lvlLeafList_CellClick;
            this.lvlLeafList.CellDoubleClick += this.lvlLeafList_CellDoubleClick;
            this.lvlLeafList.CellMouseDown += this.lvlLeafList_CellMouseDown;
            this.lvlLeafList.CellMouseUp += this.lvlLeafList_CellMouseUp;
            this.lvlLeafList.CellPainting += this.lvlLeafList_CellPainting;
            this.lvlLeafList.RowPrePaint += this.lvlLeafList_RowPrePaint;
            this.lvlLeafList.SelectionChanged += this.lvlLeafList_SelectionChanged;
            this.lvlLeafList.DragDrop += this.lvlLeafList_DragDrop;
            this.lvlLeafList.DragEnter += this.lvlLeafList_DragEnter;
            this.lvlLeafList.DragOver += this.lvlLeafList_DragOver;
            this.lvlLeafList.KeyDown += this.lvlLeafList_KeyDown;
            this.lvlLeafList.MouseDown += this.lvlLeafList_MouseDown;
            this.lvlLeafList.MouseMove += this.lvlLeafList_MouseMove;
            // 
            // lvlLeafIcon
            // 
            this.lvlLeafIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.lvlLeafIcon.HeaderText = "";
            this.lvlLeafIcon.Image = Properties.Resources.editor_leaf;
            this.lvlLeafIcon.MinimumWidth = 16;
            this.lvlLeafIcon.Name = "lvlLeafIcon";
            this.lvlLeafIcon.ReadOnly = true;
            this.lvlLeafIcon.Width = 16;
            // 
            // LeafName
            // 
            this.LeafName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            this.LeafName.DefaultCellStyle = dataGridViewCellStyle2;
            this.LeafName.FillWeight = 50F;
            this.LeafName.HeaderText = "Leaf";
            this.LeafName.Name = "LeafName";
            this.LeafName.ReadOnly = true;
            this.LeafName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Runtime
            // 
            this.Runtime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            this.Runtime.DefaultCellStyle = dataGridViewCellStyle3;
            this.Runtime.FillWeight = 50F;
            this.Runtime.HeaderText = "Runtime";
            this.Runtime.Name = "Runtime";
            this.Runtime.ReadOnly = true;
            this.Runtime.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // btnLvlSequencer
            // 
            this.btnLvlSequencer.BackColor = Color.DarkGreen;
            this.btnLvlSequencer.CausesValidation = false;
            this.btnLvlSequencer.Dock = DockStyle.Bottom;
            this.btnLvlSequencer.Enabled = false;
            this.btnLvlSequencer.FlatStyle = FlatStyle.Popup;
            this.btnLvlSequencer.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnLvlSequencer.ForeColor = Color.White;
            this.btnLvlSequencer.Image = Properties.Resources.icon_template;
            this.btnLvlSequencer.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnLvlSequencer.Location = new Point(24, 219);
            this.btnLvlSequencer.Margin = new Padding(4, 3, 4, 3);
            this.btnLvlSequencer.MaximumSize = new Size(200, 28);
            this.btnLvlSequencer.MinimumSize = new Size(200, 28);
            this.btnLvlSequencer.Name = "btnLvlSequencer";
            this.btnLvlSequencer.Size = new Size(200, 28);
            this.btnLvlSequencer.TabIndex = 155;
            this.btnLvlSequencer.Text = "Open Sequencer";
            this.btnLvlSequencer.UseVisualStyleBackColor = false;
            this.btnLvlSequencer.Click += this.btnLvlSequencer_Click;
            // 
            // lvlToolStrip
            // 
            this.lvlToolStrip.AutoSize = false;
            this.lvlToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlToolStrip.Dock = DockStyle.Left;
            this.lvlToolStrip.GripMargin = new Padding(0);
            this.lvlToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLeafAdd, this.btnLvlLeafDelete, this.btnLvlLeafUp, this.btnLvlLeafDown, this.btnLvlLeafCopy, this.btnLvlLeafPaste, this.btnLvlLeafRandom, this.btnLvlPlayback });
            this.lvlToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lvlToolStrip.Location = new Point(0, 0);
            this.lvlToolStrip.Name = "lvlToolStrip";
            this.lvlToolStrip.Padding = new Padding(0);
            this.lvlToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlToolStrip.Size = new Size(24, 247);
            this.lvlToolStrip.Stretch = true;
            this.lvlToolStrip.TabIndex = 154;
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
            this.btnLvlLeafAdd.Size = new Size(23, 24);
            this.btnLvlLeafAdd.ToolTipText = "Add new leaf to the list";
            this.btnLvlLeafAdd.Click += this.btnLvlLeafAdd_Click;
            // 
            // btnLvlLeafDelete
            // 
            this.btnLvlLeafDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDelete.Enabled = false;
            this.btnLvlLeafDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLeafDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDelete.Margin = new Padding(0);
            this.btnLvlLeafDelete.Name = "btnLvlLeafDelete";
            this.btnLvlLeafDelete.Size = new Size(23, 24);
            this.btnLvlLeafDelete.ToolTipText = "Delete selected sublevel from this list";
            this.btnLvlLeafDelete.Click += this.btnLvlLeafDelete_Click;
            // 
            // btnLvlLeafUp
            // 
            this.btnLvlLeafUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafUp.Enabled = false;
            this.btnLvlLeafUp.Image = Properties.Resources.icon_arrowup2;
            this.btnLvlLeafUp.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafUp.Margin = new Padding(0);
            this.btnLvlLeafUp.Name = "btnLvlLeafUp";
            this.btnLvlLeafUp.Size = new Size(23, 24);
            this.btnLvlLeafUp.ToolTipText = "Move selected sublevel up";
            this.btnLvlLeafUp.Click += this.btnLvlLeafUp_Click;
            // 
            // btnLvlLeafDown
            // 
            this.btnLvlLeafDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDown.Enabled = false;
            this.btnLvlLeafDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlLeafDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDown.Margin = new Padding(0);
            this.btnLvlLeafDown.Name = "btnLvlLeafDown";
            this.btnLvlLeafDown.Size = new Size(23, 24);
            this.btnLvlLeafDown.ToolTipText = "Move selected sublevel down";
            this.btnLvlLeafDown.Click += this.btnLvlLeafDown_Click;
            // 
            // btnLvlLeafCopy
            // 
            this.btnLvlLeafCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafCopy.Enabled = false;
            this.btnLvlLeafCopy.Image = Properties.Resources.icon_copy2;
            this.btnLvlLeafCopy.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafCopy.Margin = new Padding(0);
            this.btnLvlLeafCopy.Name = "btnLvlLeafCopy";
            this.btnLvlLeafCopy.Size = new Size(23, 24);
            this.btnLvlLeafCopy.ToolTipText = "Copy selected sublevel";
            this.btnLvlLeafCopy.Click += this.btnLvlLeafCopy_Click;
            // 
            // btnLvlLeafPaste
            // 
            this.btnLvlLeafPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafPaste.Enabled = false;
            this.btnLvlLeafPaste.Image = Properties.Resources.icon_paste2;
            this.btnLvlLeafPaste.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafPaste.Name = "btnLvlLeafPaste";
            this.btnLvlLeafPaste.Size = new Size(23, 24);
            this.btnLvlLeafPaste.ToolTipText = "Paste the copied sublevel";
            this.btnLvlLeafPaste.Click += this.btnLvlLeafPaste_Click;
            // 
            // btnLvlLeafRandom
            // 
            this.btnLvlLeafRandom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafRandom.Enabled = false;
            this.btnLvlLeafRandom.Image = Properties.Resources.icon_random;
            this.btnLvlLeafRandom.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafRandom.Name = "btnLvlLeafRandom";
            this.btnLvlLeafRandom.Size = new Size(23, 24);
            this.btnLvlLeafRandom.ToolTipText = "Add a random leaf";
            this.btnLvlLeafRandom.Click += this.btnLvlLeafRandom_Click;
            // 
            // btnLvlPlayback
            // 
            this.btnLvlPlayback.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPlayback.Image = Properties.Resources.icon_play2;
            this.btnLvlPlayback.ImageTransparentColor = Color.Magenta;
            this.btnLvlPlayback.Name = "btnLvlPlayback";
            this.btnLvlPlayback.Size = new Size(23, 24);
            this.btnLvlPlayback.Text = "toolStripButton1";
            this.btnLvlPlayback.ToolTipText = "Preview how the Lvl will sound.\r\n!!! NOTE !!!\r\nThis is only a preview, and may not be entirely accurate\r\nto how it will sound in-game.\r\n!!\r\nSelect 1 leaf to set playback to start at that position";
            this.btnLvlPlayback.Click += this.btnLvlPlayback_Click;
            // 
            // lvlLeafPaths
            // 
            this.lvlLeafPaths.AllowDrop = true;
            this.lvlLeafPaths.AllowUserToAddRows = false;
            this.lvlLeafPaths.AllowUserToDeleteRows = false;
            this.lvlLeafPaths.AllowUserToResizeColumns = false;
            this.lvlLeafPaths.AllowUserToResizeRows = false;
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
            this.lvlLeafPaths.Columns.AddRange(new DataGridViewColumn[] { this.columnLvlLeafPaths });
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = Color.DarkBlue;
            dataGridViewCellStyle9.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = null;
            dataGridViewCellStyle9.SelectionBackColor = Color.CornflowerBlue;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.DefaultCellStyle = dataGridViewCellStyle9;
            this.lvlLeafPaths.Dock = DockStyle.Fill;
            this.lvlLeafPaths.EnableHeadersVisualStyles = false;
            this.lvlLeafPaths.GridColor = Color.Black;
            this.lvlLeafPaths.Location = new Point(0, 25);
            this.lvlLeafPaths.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafPaths.Name = "lvlLeafPaths";
            this.lvlLeafPaths.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle10.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.lvlLeafPaths.RowHeadersVisible = false;
            this.lvlLeafPaths.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle11.BackColor = Color.DarkBlue;
            dataGridViewCellStyle11.ForeColor = Color.White;
            this.lvlLeafPaths.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.BackColor = Color.DarkBlue;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
            this.lvlLeafPaths.RowTemplate.Height = 20;
            this.lvlLeafPaths.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafPaths.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafPaths.Size = new Size(305, 194);
            this.lvlLeafPaths.TabIndex = 160;
            this.lvlLeafPaths.CellClick += this.lvlLeafPaths_CellClick;
            this.lvlLeafPaths.CellMouseDown += this.lvlLeafPaths_CellMouseDown;
            this.lvlLeafPaths.CellMouseEnter += this.lvlLeafPaths_CellMouseEnter;
            this.lvlLeafPaths.CellMouseLeave += this.lvlLeafPaths_CellMouseLeave;
            this.lvlLeafPaths.CellMouseUp += this.lvlLeafPaths_CellMouseUp;
            this.lvlLeafPaths.CellPainting += this.lvlLeafList_CellPainting;
            this.lvlLeafPaths.CellValueChanged += this.lvlLeafPaths_CellValueChanged;
            this.lvlLeafPaths.DataError += this.lvlLoopTracks_DataError;
            this.lvlLeafPaths.RowPrePaint += this.lvlLeafList_RowPrePaint;
            this.lvlLeafPaths.SelectionChanged += this.lvlLeafPaths_SelectionChanged;
            this.lvlLeafPaths.DragDrop += this.lvlLeafPaths_DragDrop;
            this.lvlLeafPaths.DragEnter += this.lvlLeafPaths_DragEnter;
            this.lvlLeafPaths.DragOver += this.lvlLeafPaths_DragOver;
            this.lvlLeafPaths.DragLeave += this.lvlLeafPaths_DragLeave;
            this.lvlLeafPaths.MouseDown += this.lvlLeafPaths_MouseDown;
            this.lvlLeafPaths.MouseLeave += this.lvlLeafPaths_MouseLeave;
            this.lvlLeafPaths.MouseMove += this.lvlLeafPaths_MouseMove;
            // 
            // columnLvlLeafPaths
            // 
            this.columnLvlLeafPaths.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = Color.DarkBlue;
            dataGridViewCellStyle8.ForeColor = Color.White;
            this.columnLvlLeafPaths.DefaultCellStyle = dataGridViewCellStyle8;
            this.columnLvlLeafPaths.HeaderText = "Paths/Tunnels";
            this.columnLvlLeafPaths.Name = "columnLvlLeafPaths";
            this.columnLvlLeafPaths.ReadOnly = true;
            this.columnLvlLeafPaths.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lvlPathsToolStrip
            // 
            this.lvlPathsToolStrip.AutoSize = false;
            this.lvlPathsToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlPathsToolStrip.GripMargin = new Padding(0);
            this.lvlPathsToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlPathsToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlPathsToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlPathAdd, this.btnLvlPathDelete, this.btnLvlPathUp, this.btnLvlPathDown, this.btnLvlCopyTunnel, this.btnLvlPasteTunnel, this.btnLvlPathClear, this.chkTunnelCopy, this.btnLvlRandomTunnel, this.btnLvlPathView });
            this.lvlPathsToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlPathsToolStrip.Location = new Point(0, 0);
            this.lvlPathsToolStrip.Name = "lvlPathsToolStrip";
            this.lvlPathsToolStrip.Padding = new Padding(0);
            this.lvlPathsToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlPathsToolStrip.Size = new Size(305, 25);
            this.lvlPathsToolStrip.Stretch = true;
            this.lvlPathsToolStrip.TabIndex = 162;
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
            this.btnLvlPathAdd.Size = new Size(24, 25);
            this.btnLvlPathAdd.ToolTipText = "Add new path/tunnel";
            this.btnLvlPathAdd.Click += this.btnLvlPathAdd_Click;
            // 
            // btnLvlPathDelete
            // 
            this.btnLvlPathDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDelete.Enabled = false;
            this.btnLvlPathDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlPathDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDelete.Margin = new Padding(0);
            this.btnLvlPathDelete.Name = "btnLvlPathDelete";
            this.btnLvlPathDelete.Size = new Size(24, 25);
            this.btnLvlPathDelete.ToolTipText = "Delete selected path";
            this.btnLvlPathDelete.Click += this.btnLvlPathDelete_Click;
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
            this.btnLvlPathUp.Size = new Size(24, 25);
            this.btnLvlPathUp.ToolTipText = "Move selected tunnel up";
            this.btnLvlPathUp.Click += this.btnLvlPathUp_Click;
            // 
            // btnLvlPathDown
            // 
            this.btnLvlPathDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDown.Enabled = false;
            this.btnLvlPathDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlPathDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDown.Margin = new Padding(0);
            this.btnLvlPathDown.Name = "btnLvlPathDown";
            this.btnLvlPathDown.Size = new Size(24, 25);
            this.btnLvlPathDown.ToolTipText = "Move selected tunnel down";
            this.btnLvlPathDown.Click += this.btnLvlPathDown_Click;
            // 
            // btnLvlCopyTunnel
            // 
            this.btnLvlCopyTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlCopyTunnel.Enabled = false;
            this.btnLvlCopyTunnel.Image = Properties.Resources.icon_copy2;
            this.btnLvlCopyTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlCopyTunnel.Name = "btnLvlCopyTunnel";
            this.btnLvlCopyTunnel.Size = new Size(24, 22);
            this.btnLvlCopyTunnel.ToolTipText = "Copy selected paths/tunnels.\r\nHold SHIFT to copy all.";
            this.btnLvlCopyTunnel.Click += this.btnLvlCopyTunnel_Click;
            // 
            // btnLvlPasteTunnel
            // 
            this.btnLvlPasteTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPasteTunnel.Enabled = false;
            this.btnLvlPasteTunnel.Image = Properties.Resources.icon_paste2;
            this.btnLvlPasteTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlPasteTunnel.Name = "btnLvlPasteTunnel";
            this.btnLvlPasteTunnel.Size = new Size(24, 22);
            this.btnLvlPasteTunnel.ToolTipText = "Paste copied paths/tunnels";
            this.btnLvlPasteTunnel.Click += this.btnLvlPasteTunnel_Click;
            // 
            // btnLvlPathClear
            // 
            this.btnLvlPathClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathClear.Enabled = false;
            this.btnLvlPathClear.Image = Properties.Resources.icon_erase;
            this.btnLvlPathClear.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathClear.Name = "btnLvlPathClear";
            this.btnLvlPathClear.Size = new Size(24, 22);
            this.btnLvlPathClear.Text = "toolStripButton2";
            this.btnLvlPathClear.ToolTipText = "Clear all tunnels";
            this.btnLvlPathClear.Click += this.btnLvlPathClear_Click;
            // 
            // chkTunnelCopy
            // 
            this.chkTunnelCopy.CheckOnClick = true;
            this.chkTunnelCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.chkTunnelCopy.Image = Properties.Resources.icon_sling;
            this.chkTunnelCopy.ImageTransparentColor = Color.Magenta;
            this.chkTunnelCopy.Name = "chkTunnelCopy";
            this.chkTunnelCopy.Size = new Size(24, 22);
            this.chkTunnelCopy.ToolTipText = "When enabled, new leafs added will copy the paths\r\nof the previous leaf.";
            // 
            // btnLvlRandomTunnel
            // 
            this.btnLvlRandomTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlRandomTunnel.Enabled = false;
            this.btnLvlRandomTunnel.Image = Properties.Resources.icon_random;
            this.btnLvlRandomTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlRandomTunnel.Name = "btnLvlRandomTunnel";
            this.btnLvlRandomTunnel.Size = new Size(24, 22);
            this.btnLvlRandomTunnel.ToolTipText = "Click to add a random tunnel";
            this.btnLvlRandomTunnel.Click += this.btnLvlRandomTunnel_Click;
            // 
            // btnLvlPathView
            // 
            this.btnLvlPathView.Checked = true;
            this.btnLvlPathView.CheckOnClick = true;
            this.btnLvlPathView.CheckState = CheckState.Checked;
            this.btnLvlPathView.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathView.Image = Properties.Resources.icon_view;
            this.btnLvlPathView.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathView.Name = "btnLvlPathView";
            this.btnLvlPathView.Size = new Size(24, 22);
            this.btnLvlPathView.Text = "toolStripButton1";
            this.btnLvlPathView.ToolTipText = "Show/Hide tunnel preview";
            this.btnLvlPathView.CheckedChanged += this.btnLvlPathView_CheckedChanged;
            // 
            // lvlLoopTracks
            // 
            this.lvlLoopTracks.AllowUserToAddRows = false;
            this.lvlLoopTracks.AllowUserToDeleteRows = false;
            this.lvlLoopTracks.AllowUserToResizeColumns = false;
            this.lvlLoopTracks.AllowUserToResizeRows = false;
            this.lvlLoopTracks.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopTracks.BorderStyle = BorderStyle.None;
            this.lvlLoopTracks.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLoopTracks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle12.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = Color.White;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.lvlLoopTracks.ColumnHeadersHeight = 20;
            this.lvlLoopTracks.Columns.AddRange(new DataGridViewColumn[] { this.LvlLoopPlay, this.LoopSample, this.BeatsPerLoop });
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle13.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle13.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle13.Format = "N2";
            dataGridViewCellStyle13.NullValue = null;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.DefaultCellStyle = dataGridViewCellStyle13;
            this.lvlLoopTracks.Dock = DockStyle.Fill;
            this.lvlLoopTracks.EnableHeadersVisualStyles = false;
            this.lvlLoopTracks.GridColor = Color.Black;
            this.lvlLoopTracks.Location = new Point(0, 25);
            this.lvlLoopTracks.Margin = new Padding(4, 3, 4, 3);
            this.lvlLoopTracks.MultiSelect = false;
            this.lvlLoopTracks.Name = "lvlLoopTracks";
            this.lvlLoopTracks.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle14.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLoopTracks.RowTemplate.Height = 20;
            this.lvlLoopTracks.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLoopTracks.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.lvlLoopTracks.Size = new Size(348, 185);
            this.lvlLoopTracks.TabIndex = 161;
            this.lvlLoopTracks.CellClick += this.lvlLoopTracks_CellClick;
            this.lvlLoopTracks.CellPainting += this.lvlLoopTracks_CellPainting;
            this.lvlLoopTracks.CellValueChanged += this.lvlLoopTracks_CellValueChanged;
            this.lvlLoopTracks.DataError += this.lvlLoopTracks_DataError;
            this.lvlLoopTracks.RowPostPaint += this.lvlLoopTracks_RowPostPaint;
            // 
            // LvlLoopPlay
            // 
            this.LvlLoopPlay.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.LvlLoopPlay.HeaderText = "";
            this.LvlLoopPlay.Name = "LvlLoopPlay";
            this.LvlLoopPlay.ReadOnly = true;
            this.LvlLoopPlay.Resizable = DataGridViewTriState.False;
            this.LvlLoopPlay.Width = 5;
            // 
            // LoopSample
            // 
            this.LoopSample.FlatStyle = FlatStyle.Flat;
            this.LoopSample.HeaderText = "Sample Name";
            this.LoopSample.MaxDropDownItems = 20;
            this.LoopSample.MinimumWidth = 50;
            this.LoopSample.Name = "LoopSample";
            this.LoopSample.Width = 365;
            // 
            // BeatsPerLoop
            // 
            this.BeatsPerLoop.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.BeatsPerLoop.HeaderText = "Beats";
            this.BeatsPerLoop.MaxInputLength = 6;
            this.BeatsPerLoop.Name = "BeatsPerLoop";
            this.BeatsPerLoop.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.BeatsPerLoop.Width = 43;
            // 
            // lvlLoopToolStrip
            // 
            this.lvlLoopToolStrip.AutoSize = false;
            this.lvlLoopToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopToolStrip.GripMargin = new Padding(0);
            this.lvlLoopToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlLoopToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlLoopToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLoopAdd, this.btnLvlLoopDelete });
            this.lvlLoopToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlLoopToolStrip.Location = new Point(0, 0);
            this.lvlLoopToolStrip.Name = "lvlLoopToolStrip";
            this.lvlLoopToolStrip.Padding = new Padding(0);
            this.lvlLoopToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlLoopToolStrip.Size = new Size(348, 25);
            this.lvlLoopToolStrip.Stretch = true;
            this.lvlLoopToolStrip.TabIndex = 163;
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
            this.btnLvlLoopAdd.Size = new Size(24, 25);
            this.btnLvlLoopAdd.ToolTipText = "Add new loop track";
            this.btnLvlLoopAdd.Click += this.btnLvlLoopAdd_Click;
            // 
            // btnLvlLoopDelete
            // 
            this.btnLvlLoopDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLoopDelete.Enabled = false;
            this.btnLvlLoopDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLoopDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLoopDelete.Margin = new Padding(0);
            this.btnLvlLoopDelete.Name = "btnLvlLoopDelete";
            this.btnLvlLoopDelete.Size = new Size(24, 25);
            this.btnLvlLoopDelete.ToolTipText = "Delete selected loop track";
            this.btnLvlLoopDelete.Click += this.btnLvlLoopDelete_Click;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = Color.Black;
            this.panelMain.Controls.Add(this.lvlLeafList);
            this.panelMain.Controls.Add(this.btnLvlSequencer);
            this.panelMain.Controls.Add(this.lvlToolStrip);
            this.panelMain.Location = new Point(12, 12);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(258, 247);
            this.panelMain.TabIndex = 152;
            // 
            // panelTunnel
            // 
            this.panelTunnel.BackColor = Color.Black;
            this.panelTunnel.Controls.Add(this.lvlLeafPaths);
            this.panelTunnel.Controls.Add(this.lvlPathsToolStrip);
            this.panelTunnel.Location = new Point(360, 12);
            this.panelTunnel.Name = "panelTunnel";
            this.panelTunnel.Size = new Size(305, 219);
            this.panelTunnel.TabIndex = 164;
            // 
            // panelLoop
            // 
            this.panelLoop.BackColor = Color.Black;
            this.panelLoop.Controls.Add(this.lvlLoopTracks);
            this.panelLoop.Controls.Add(this.lvlLoopToolStrip);
            this.panelLoop.Location = new Point(327, 251);
            this.panelLoop.Name = "panelLoop";
            this.panelLoop.Size = new Size(348, 210);
            this.panelLoop.TabIndex = 165;
            // 
            // dockPanel1
            // 
            this.dockPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dockPanel1.BackColor = Color.Black;
            this.dockPanel1.Location = new Point(-4, -4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new Size(715, 514);
            this.dockPanel1.TabIndex = 166;
            this.dockPanel1.ActiveContentChanged += this.dockPanel1_ActiveContentChanged;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += this.timer1_Tick;
            // 
            // EditorLvl
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(707, 506);
            this.Controls.Add(this.panelLoop);
            this.Controls.Add(this.panelTunnel);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.dockPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = Color.FromArgb(150, 150, 255);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "EditorLvl";
            this.Text = "Lvl Editor";
            this.Shown += this.Form_LvlEditor_Shown;
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).EndInit();
            this.lvlToolStrip.ResumeLayout(false);
            this.lvlToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).EndInit();
            this.lvlPathsToolStrip.ResumeLayout(false);
            this.lvlPathsToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).EndInit();
            this.lvlLoopToolStrip.ResumeLayout(false);
            this.lvlLoopToolStrip.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelTunnel.ResumeLayout(false);
            this.panelLoop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private ToolStrip lvlLoopToolStrip;
        private ToolStripButton btnLvlLoopAdd;
        private ToolStripButton btnLvlLoopDelete;
        private ToolStrip lvlPathsToolStrip;
        private ToolStripButton btnLvlPathAdd;
        private ToolStripButton btnLvlPathDelete;
        private ToolStripButton btnLvlPathUp;
        private ToolStripButton btnLvlPathDown;
        private ToolStripButton btnLvlCopyTunnel;
        private ToolStripButton btnLvlPasteTunnel;
        private ToolStripButton btnLvlPathClear;
        private ToolStripButton chkTunnelCopy;
        private ToolStripButton btnLvlRandomTunnel;
        private ToolStripButton btnLvlPathView;
        public DataGridView lvlLoopTracks;
        private DataGridViewButtonColumn LvlLoopPlay;
        private DataGridViewComboBoxColumn LoopSample;
        private DataGridViewTextBoxColumn BeatsPerLoop;
        private DataGridViewTextBoxColumn columnLvlLeafPaths;
        public Button btnLvlSequencer;
        private ToolStrip lvlToolStrip;
        private ToolStripButton btnLvlLeafAdd;
        private ToolStripButton btnLvlLeafDelete;
        private ToolStripButton btnLvlLeafUp;
        private ToolStripButton btnLvlLeafDown;
        private ToolStripButton btnLvlLeafCopy;
        private ToolStripButton btnLvlLeafPaste;
        private ToolStripButton btnLvlLeafRandom;
        public DataGridView lvlLeafPaths;
        private Panel panelMain;
        private Panel panelTunnel;
        private Panel panelLoop;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private ToolStripButton btnLvlPlayback;
        private System.Windows.Forms.Timer timer1;
        public DataGridView lvlLeafList;
        private DataGridViewImageColumn lvlLeafIcon;
        private DataGridViewTextBoxColumn LeafName;
        private DataGridViewTextBoxColumn Runtime;
    }
}