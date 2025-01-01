namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_LeafEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_LeafEditor));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            this.toolTip1 = new ToolTip(this.components);
            this.trackZoomVert = new TrackBar();
            this.trackZoom = new TrackBar();
            this.btnRawImport = new Button();
            this.dropObjects = new ComboBox();
            this.dropParamPath = new ComboBox();
            this.dropTrackLane = new ComboBox();
            this.btnTrackApply = new Button();
            this.vScrollBarTrackEditor = new VScrollBar();
            this.panelZoom = new Panel();
            this.labelScrollH = new Label();
            this.labelScrollV = new Label();
            this.trackEditor = new DataGridView();
            this.LeafEnabled = new DataGridViewTextBoxColumn();
            this.LeafAudio = new DataGridViewTextBoxColumn();
            this.LeafMultilane = new DataGridViewTextBoxColumn();
            this.leafToolStrip = new ToolStrip();
            this.btnTrackAdd = new ToolStripButton();
            this.btnTrackDelete = new ToolStripButton();
            this.btnTrackUp = new ToolStripButton();
            this.btnTrackDown = new ToolStripButton();
            this.btnTrackCopy = new ToolStripButton();
            this.btnTrackPaste = new ToolStripButton();
            this.btnTrackClear = new ToolStripButton();
            this.btnTrackPlayback = new ToolStripButton();
            this.btnTrackColorExport = new ToolStripButton();
            this.btnTrackColorImport = new ToolStripButton();
            this.btnLeafRandom = new ToolStripButton();
            this.leaftoolsToolStrip = new ToolStrip();
            this.btnLeafColors = new ToolStripButton();
            this.btnLEafInterpLinear = new ToolStripButton();
            this.btnLeafSplit = new ToolStripButton();
            this.btnLeafRandomValues = new ToolStripButton();
            this.btnLeafZoom = new ToolStripButton();
            this.btnLeafAutoPlace = new ToolStripButton();
            this.splitContainer1 = new SplitContainer();
            this.propertyGridLeaf = new PropertyGrid();
            this.splitContainerLeafSide = new SplitContainer();
            this.splitContainerTopbar = new SplitContainer();
            this.labelSequencer = new Label();
            this.textEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)this.trackZoomVert).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.trackZoom).BeginInit();
            this.panelZoom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackEditor).BeginInit();
            this.leafToolStrip.SuspendLayout();
            this.leaftoolsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainerLeafSide).BeginInit();
            this.splitContainerLeafSide.Panel1.SuspendLayout();
            this.splitContainerLeafSide.Panel2.SuspendLayout();
            this.splitContainerLeafSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainerTopbar).BeginInit();
            this.splitContainerTopbar.Panel1.SuspendLayout();
            this.splitContainerTopbar.Panel2.SuspendLayout();
            this.splitContainerTopbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.textEditor).BeginInit();
            this.SuspendLayout();
            // 
            // trackZoomVert
            // 
            this.trackZoomVert.AutoSize = false;
            this.trackZoomVert.Cursor = Cursors.Hand;
            this.trackZoomVert.Location = new Point(46, 20);
            this.trackZoomVert.Margin = new Padding(0);
            this.trackZoomVert.Maximum = 100;
            this.trackZoomVert.Minimum = 1;
            this.trackZoomVert.Name = "trackZoomVert";
            this.trackZoomVert.Orientation = Orientation.Vertical;
            this.trackZoomVert.Size = new Size(22, 115);
            this.trackZoomVert.TabIndex = 43;
            this.trackZoomVert.TickStyle = TickStyle.None;
            this.toolTip1.SetToolTip(this.trackZoomVert, "Row zoom/height");
            this.trackZoomVert.Value = 20;
            this.trackZoomVert.Scroll += this.trackZoomVert_Scroll;
            // 
            // trackZoom
            // 
            this.trackZoom.AutoSize = false;
            this.trackZoom.Cursor = Cursors.Hand;
            this.trackZoom.Location = new Point(-4, 3);
            this.trackZoom.Margin = new Padding(0);
            this.trackZoom.Maximum = 100;
            this.trackZoom.Minimum = 1;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new Size(117, 22);
            this.trackZoom.TabIndex = 41;
            this.trackZoom.TickStyle = TickStyle.None;
            this.toolTip1.SetToolTip(this.trackZoom, "Cell zoom/width");
            this.trackZoom.Value = 40;
            this.trackZoom.Scroll += this.trackZoom_Scroll;
            // 
            // btnRawImport
            // 
            this.btnRawImport.BackColor = Color.DarkGreen;
            this.btnRawImport.Cursor = Cursors.Hand;
            this.btnRawImport.Dock = DockStyle.Left;
            this.btnRawImport.FlatStyle = FlatStyle.Flat;
            this.btnRawImport.Image = (Image)resources.GetObject("btnRawImport.Image");
            this.btnRawImport.ImageAlign = ContentAlignment.TopCenter;
            this.btnRawImport.Location = new Point(0, 0);
            this.btnRawImport.Margin = new Padding(0);
            this.btnRawImport.Name = "btnRawImport";
            this.btnRawImport.Size = new Size(54, 129);
            this.btnRawImport.TabIndex = 44;
            this.btnRawImport.Text = "Import Raw";
            this.toolTip1.SetToolTip(this.btnRawImport, "Imports all data in the textbox to\r\nthe current selected sequencer object.");
            this.btnRawImport.UseVisualStyleBackColor = false;
            this.btnRawImport.Click += this.btnRawImport_Click;
            // 
            // dropObjects
            // 
            this.dropObjects.BackColor = Color.FromArgb(40, 40, 40);
            this.dropObjects.DrawMode = DrawMode.OwnerDrawFixed;
            this.dropObjects.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropObjects.Enabled = false;
            this.dropObjects.FlatStyle = FlatStyle.Flat;
            this.dropObjects.ForeColor = Color.White;
            this.dropObjects.FormattingEnabled = true;
            this.dropObjects.Location = new Point(4, 410);
            this.dropObjects.Margin = new Padding(4, 3, 4, 3);
            this.dropObjects.Name = "dropObjects";
            this.dropObjects.RightToLeft = RightToLeft.No;
            this.dropObjects.Size = new Size(190, 24);
            this.dropObjects.TabIndex = 30;
            // 
            // dropParamPath
            // 
            this.dropParamPath.BackColor = Color.FromArgb(40, 40, 40);
            this.dropParamPath.DrawMode = DrawMode.OwnerDrawFixed;
            this.dropParamPath.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropParamPath.DropDownWidth = 180;
            this.dropParamPath.Enabled = false;
            this.dropParamPath.FlatStyle = FlatStyle.Flat;
            this.dropParamPath.ForeColor = Color.White;
            this.dropParamPath.FormattingEnabled = true;
            this.dropParamPath.Location = new Point(4, 445);
            this.dropParamPath.Margin = new Padding(4, 3, 4, 3);
            this.dropParamPath.Name = "dropParamPath";
            this.dropParamPath.RightToLeft = RightToLeft.No;
            this.dropParamPath.Size = new Size(190, 24);
            this.dropParamPath.TabIndex = 33;
            // 
            // dropTrackLane
            // 
            this.dropTrackLane.BackColor = Color.FromArgb(40, 40, 40);
            this.dropTrackLane.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropTrackLane.DropDownWidth = 180;
            this.dropTrackLane.Enabled = false;
            this.dropTrackLane.FlatStyle = FlatStyle.Flat;
            this.dropTrackLane.ForeColor = Color.White;
            this.dropTrackLane.FormattingEnabled = true;
            this.dropTrackLane.Location = new Point(13, 475);
            this.dropTrackLane.Margin = new Padding(4, 3, 4, 3);
            this.dropTrackLane.Name = "dropTrackLane";
            this.dropTrackLane.RightToLeft = RightToLeft.No;
            this.dropTrackLane.Size = new Size(109, 23);
            this.dropTrackLane.TabIndex = 65;
            // 
            // btnTrackApply
            // 
            this.btnTrackApply.BackColor = Color.Green;
            this.btnTrackApply.Cursor = Cursors.Hand;
            this.btnTrackApply.Enabled = false;
            this.btnTrackApply.FlatStyle = FlatStyle.Flat;
            this.btnTrackApply.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.btnTrackApply.ForeColor = Color.White;
            this.btnTrackApply.Location = new Point(104, 488);
            this.btnTrackApply.Margin = new Padding(4, 3, 4, 3);
            this.btnTrackApply.Name = "btnTrackApply";
            this.btnTrackApply.Size = new Size(112, 28);
            this.btnTrackApply.TabIndex = 52;
            this.btnTrackApply.Text = "Apply to Track";
            this.btnTrackApply.UseVisualStyleBackColor = false;
            // 
            // vScrollBarTrackEditor
            // 
            this.vScrollBarTrackEditor.Dock = DockStyle.Left;
            this.vScrollBarTrackEditor.Location = new Point(25, 30);
            this.vScrollBarTrackEditor.Name = "vScrollBarTrackEditor";
            this.vScrollBarTrackEditor.Size = new Size(15, 355);
            this.vScrollBarTrackEditor.TabIndex = 144;
            this.vScrollBarTrackEditor.Visible = false;
            // 
            // panelZoom
            // 
            this.panelZoom.BackColor = Color.Black;
            this.panelZoom.BorderStyle = BorderStyle.Fixed3D;
            this.panelZoom.Controls.Add(this.labelScrollH);
            this.panelZoom.Controls.Add(this.labelScrollV);
            this.panelZoom.Controls.Add(this.trackZoomVert);
            this.panelZoom.Controls.Add(this.trackZoom);
            this.panelZoom.Location = new Point(176, 25);
            this.panelZoom.Margin = new Padding(4, 3, 4, 3);
            this.panelZoom.Name = "panelZoom";
            this.panelZoom.Size = new Size(114, 133);
            this.panelZoom.TabIndex = 43;
            this.panelZoom.Visible = false;
            // 
            // labelScrollH
            // 
            this.labelScrollH.AutoSize = true;
            this.labelScrollH.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelScrollH.ForeColor = Color.White;
            this.labelScrollH.Location = new Point(-1, 16);
            this.labelScrollH.Margin = new Padding(4, 0, 4, 0);
            this.labelScrollH.Name = "labelScrollH";
            this.labelScrollH.RightToLeft = RightToLeft.No;
            this.labelScrollH.Size = new Size(36, 45);
            this.labelScrollH.TabIndex = 67;
            this.labelScrollH.Text = "Ctrl+\r\nscroll\r\n←→";
            this.labelScrollH.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelScrollV
            // 
            this.labelScrollV.AutoSize = true;
            this.labelScrollV.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelScrollV.ForeColor = Color.White;
            this.labelScrollV.Location = new Point(62, 55);
            this.labelScrollV.Margin = new Padding(4, 0, 4, 0);
            this.labelScrollV.Name = "labelScrollV";
            this.labelScrollV.RightToLeft = RightToLeft.No;
            this.labelScrollV.Size = new Size(38, 60);
            this.labelScrollV.TabIndex = 68;
            this.labelScrollV.Text = "↑\r\n↓\r\nShift+\r\nscroll";
            this.labelScrollV.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trackEditor
            // 
            this.trackEditor.AllowUserToAddRows = false;
            this.trackEditor.AllowUserToDeleteRows = false;
            this.trackEditor.AllowUserToResizeColumns = false;
            this.trackEditor.AllowUserToResizeRows = false;
            this.trackEditor.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.trackEditor.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.trackEditor.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.trackEditor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.trackEditor.ColumnHeadersHeight = 20;
            this.trackEditor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.trackEditor.Columns.AddRange(new DataGridViewColumn[] { this.LeafEnabled, this.LeafAudio, this.LeafMultilane });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Format = "0.###";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            this.trackEditor.DefaultCellStyle = dataGridViewCellStyle2;
            this.trackEditor.Dock = DockStyle.Fill;
            this.trackEditor.EnableHeadersVisualStyles = false;
            this.trackEditor.GridColor = Color.Black;
            this.trackEditor.Location = new Point(40, 30);
            this.trackEditor.Margin = new Padding(4, 3, 4, 3);
            this.trackEditor.Name = "trackEditor";
            this.trackEditor.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle3.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.trackEditor.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.trackEditor.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.trackEditor.RowTemplate.Height = 20;
            this.trackEditor.ScrollBars = ScrollBars.Horizontal;
            this.trackEditor.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.trackEditor.ShowCellErrors = false;
            this.trackEditor.ShowRowErrors = false;
            this.trackEditor.Size = new Size(668, 355);
            this.trackEditor.TabIndex = 40;
            this.trackEditor.Tag = "editorpaneldgv";
            this.trackEditor.RowHeadersWidthChanged += this.trackEditor_RowHeadersWidthChanged;
            this.trackEditor.CellFormatting += this.trackEditor_CellFormatting;
            this.trackEditor.CellMouseClick += this.trackEditor_CellMouseClick;
            this.trackEditor.CellMouseDown += this.trackEditor_CellMouseDown;
            this.trackEditor.CellMouseEnter += this.trackEditor_CellMouseEnter;
            this.trackEditor.CellMouseLeave += this.trackEditor_CellMouseLeave;
            this.trackEditor.CellPainting += this.trackEditor_CellPainting;
            this.trackEditor.CellParsing += this.trackEditor_CellParsing;
            this.trackEditor.DataError += this.trackEditor_DataError;
            this.trackEditor.RowEnter += this.trackEditor_RowEnter;
            this.trackEditor.RowHeaderMouseClick += this.trackEditor_RowHeaderMouseClick;
            this.trackEditor.RowPostPaint += this.trackEditor_RowPostPaint;
            this.trackEditor.RowsAdded += this.trackEditor_RowsAdded;
            this.trackEditor.SelectionChanged += this.trackEditor_SelectionChanged;
            this.trackEditor.Click += this.trackEditor_Click;
            this.trackEditor.KeyDown += this.trackEditor_KeyDown;
            this.trackEditor.KeyPress += this.trackEditor_KeyPress;
            this.trackEditor.KeyUp += this.trackEditor_KeyUp;
            this.trackEditor.PreviewKeyDown += this.AllowArrowMovement;
            this.trackEditor.Resize += this.trackEditor_Resize;
            // 
            // LeafEnabled
            // 
            this.LeafEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.LeafEnabled.Frozen = true;
            this.LeafEnabled.HeaderText = "";
            this.LeafEnabled.MinimumWidth = 20;
            this.LeafEnabled.Name = "LeafEnabled";
            this.LeafEnabled.ReadOnly = true;
            this.LeafEnabled.Resizable = DataGridViewTriState.False;
            this.LeafEnabled.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.LeafEnabled.ToolTipText = "Enable/Disable All";
            this.LeafEnabled.Width = 20;
            // 
            // LeafAudio
            // 
            this.LeafAudio.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.LeafAudio.Frozen = true;
            this.LeafAudio.HeaderText = "";
            this.LeafAudio.MinimumWidth = 20;
            this.LeafAudio.Name = "LeafAudio";
            this.LeafAudio.ReadOnly = true;
            this.LeafAudio.Resizable = DataGridViewTriState.False;
            this.LeafAudio.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.LeafAudio.ToolTipText = "Mute/Unmute All";
            this.LeafAudio.Width = 20;
            // 
            // LeafMultilane
            // 
            this.LeafMultilane.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.LeafMultilane.Frozen = true;
            this.LeafMultilane.HeaderText = "";
            this.LeafMultilane.MinimumWidth = 20;
            this.LeafMultilane.Name = "LeafMultilane";
            this.LeafMultilane.ReadOnly = true;
            this.LeafMultilane.Resizable = DataGridViewTriState.False;
            this.LeafMultilane.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.LeafMultilane.Width = 20;
            // 
            // leafToolStrip
            // 
            this.leafToolStrip.AutoSize = false;
            this.leafToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.leafToolStrip.Dock = DockStyle.Left;
            this.leafToolStrip.GripMargin = new Padding(0);
            this.leafToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.leafToolStrip.ImageScalingSize = new Size(20, 20);
            this.leafToolStrip.Items.AddRange(new ToolStripItem[] { this.btnTrackAdd, this.btnTrackDelete, this.btnTrackUp, this.btnTrackDown, this.btnTrackCopy, this.btnTrackPaste, this.btnTrackClear, this.btnTrackPlayback, this.btnTrackColorExport, this.btnTrackColorImport, this.btnLeafRandom });
            this.leafToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.leafToolStrip.Location = new Point(0, 30);
            this.leafToolStrip.Name = "leafToolStrip";
            this.leafToolStrip.Padding = new Padding(0);
            this.leafToolStrip.RenderMode = ToolStripRenderMode.System;
            this.leafToolStrip.Size = new Size(25, 355);
            this.leafToolStrip.Stretch = true;
            this.leafToolStrip.TabIndex = 142;
            // 
            // btnTrackAdd
            // 
            this.btnTrackAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnTrackAdd.ForeColor = Color.White;
            this.btnTrackAdd.Image = (Image)resources.GetObject("btnTrackAdd.Image");
            this.btnTrackAdd.ImageTransparentColor = Color.Magenta;
            this.btnTrackAdd.Margin = new Padding(0);
            this.btnTrackAdd.Name = "btnTrackAdd";
            this.btnTrackAdd.Size = new Size(24, 24);
            this.btnTrackAdd.ToolTipText = "Add track";
            // 
            // btnTrackDelete
            // 
            this.btnTrackDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackDelete.Enabled = false;
            this.btnTrackDelete.Image = (Image)resources.GetObject("btnTrackDelete.Image");
            this.btnTrackDelete.ImageTransparentColor = Color.Magenta;
            this.btnTrackDelete.Margin = new Padding(0);
            this.btnTrackDelete.Name = "btnTrackDelete";
            this.btnTrackDelete.Size = new Size(24, 24);
            this.btnTrackDelete.ToolTipText = "Delete selected track";
            // 
            // btnTrackUp
            // 
            this.btnTrackUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackUp.Enabled = false;
            this.btnTrackUp.Image = (Image)resources.GetObject("btnTrackUp.Image");
            this.btnTrackUp.ImageTransparentColor = Color.Magenta;
            this.btnTrackUp.Margin = new Padding(0);
            this.btnTrackUp.Name = "btnTrackUp";
            this.btnTrackUp.Size = new Size(24, 24);
            this.btnTrackUp.ToolTipText = "Move selected tracks up";
            // 
            // btnTrackDown
            // 
            this.btnTrackDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackDown.Enabled = false;
            this.btnTrackDown.Image = (Image)resources.GetObject("btnTrackDown.Image");
            this.btnTrackDown.ImageTransparentColor = Color.Magenta;
            this.btnTrackDown.Margin = new Padding(0);
            this.btnTrackDown.Name = "btnTrackDown";
            this.btnTrackDown.Size = new Size(24, 24);
            this.btnTrackDown.ToolTipText = "Move selected tracks down";
            // 
            // btnTrackCopy
            // 
            this.btnTrackCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackCopy.Enabled = false;
            this.btnTrackCopy.Image = (Image)resources.GetObject("btnTrackCopy.Image");
            this.btnTrackCopy.ImageTransparentColor = Color.Magenta;
            this.btnTrackCopy.Margin = new Padding(0);
            this.btnTrackCopy.Name = "btnTrackCopy";
            this.btnTrackCopy.Size = new Size(24, 24);
            this.btnTrackCopy.ToolTipText = "Copy selected track";
            this.btnTrackCopy.Click += this.btnTrackCopy_Click;
            // 
            // btnTrackPaste
            // 
            this.btnTrackPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackPaste.Enabled = false;
            this.btnTrackPaste.Image = (Image)resources.GetObject("btnTrackPaste.Image");
            this.btnTrackPaste.ImageTransparentColor = Color.Magenta;
            this.btnTrackPaste.Name = "btnTrackPaste";
            this.btnTrackPaste.Size = new Size(24, 24);
            this.btnTrackPaste.ToolTipText = "Paste the copied track";
            this.btnTrackPaste.Click += this.btnTrackPaste_Click;
            // 
            // btnTrackClear
            // 
            this.btnTrackClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackClear.Enabled = false;
            this.btnTrackClear.Image = (Image)resources.GetObject("btnTrackClear.Image");
            this.btnTrackClear.ImageTransparentColor = Color.Magenta;
            this.btnTrackClear.Name = "btnTrackClear";
            this.btnTrackClear.Size = new Size(24, 24);
            this.btnTrackClear.ToolTipText = "Clear all values on selected track";
            this.btnTrackClear.Click += this.btnTrackClear_Click;
            // 
            // btnTrackPlayback
            // 
            this.btnTrackPlayback.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackPlayback.Enabled = false;
            this.btnTrackPlayback.Image = (Image)resources.GetObject("btnTrackPlayback.Image");
            this.btnTrackPlayback.ImageTransparentColor = Color.Magenta;
            this.btnTrackPlayback.Name = "btnTrackPlayback";
            this.btnTrackPlayback.Size = new Size(24, 24);
            this.btnTrackPlayback.ToolTipText = resources.GetString("btnTrackPlayback.ToolTipText");
            // 
            // btnTrackColorExport
            // 
            this.btnTrackColorExport.Alignment = ToolStripItemAlignment.Right;
            this.btnTrackColorExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackColorExport.Enabled = false;
            this.btnTrackColorExport.Image = (Image)resources.GetObject("btnTrackColorExport.Image");
            this.btnTrackColorExport.ImageTransparentColor = Color.Magenta;
            this.btnTrackColorExport.Name = "btnTrackColorExport";
            this.btnTrackColorExport.Size = new Size(24, 24);
            this.btnTrackColorExport.ToolTipText = "Export a color profile";
            // 
            // btnTrackColorImport
            // 
            this.btnTrackColorImport.Alignment = ToolStripItemAlignment.Right;
            this.btnTrackColorImport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnTrackColorImport.Enabled = false;
            this.btnTrackColorImport.Image = (Image)resources.GetObject("btnTrackColorImport.Image");
            this.btnTrackColorImport.ImageTransparentColor = Color.Magenta;
            this.btnTrackColorImport.Name = "btnTrackColorImport";
            this.btnTrackColorImport.Size = new Size(24, 24);
            this.btnTrackColorImport.ToolTipText = "Import a color profile and apply to selected track";
            // 
            // btnLeafRandom
            // 
            this.btnLeafRandom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafRandom.Image = (Image)resources.GetObject("btnLeafRandom.Image");
            this.btnLeafRandom.ImageTransparentColor = Color.Magenta;
            this.btnLeafRandom.Name = "btnLeafRandom";
            this.btnLeafRandom.Size = new Size(24, 24);
            this.btnLeafRandom.ToolTipText = "Click to add a random object + values to the leaf";
            this.btnLeafRandom.Click += this.btnLeafRandom_Click;
            // 
            // leaftoolsToolStrip
            // 
            this.leaftoolsToolStrip.AutoSize = false;
            this.leaftoolsToolStrip.BackColor = Color.Black;
            this.leaftoolsToolStrip.GripMargin = new Padding(0);
            this.leaftoolsToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.leaftoolsToolStrip.ImageScalingSize = new Size(25, 25);
            this.leaftoolsToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLeafColors, this.btnLEafInterpLinear, this.btnLeafSplit, this.btnLeafRandomValues, this.btnLeafZoom, this.btnLeafAutoPlace });
            this.leaftoolsToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.leaftoolsToolStrip.Location = new Point(0, 0);
            this.leaftoolsToolStrip.Name = "leaftoolsToolStrip";
            this.leaftoolsToolStrip.Padding = new Padding(0);
            this.leaftoolsToolStrip.RenderMode = ToolStripRenderMode.System;
            this.leaftoolsToolStrip.Size = new Size(603, 30);
            this.leaftoolsToolStrip.Stretch = true;
            this.leaftoolsToolStrip.TabIndex = 143;
            // 
            // btnLeafColors
            // 
            this.btnLeafColors.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafColors.Image = (Image)resources.GetObject("btnLeafColors.Image");
            this.btnLeafColors.ImageTransparentColor = Color.Magenta;
            this.btnLeafColors.Name = "btnLeafColors";
            this.btnLeafColors.Size = new Size(29, 27);
            this.btnLeafColors.ToolTipText = "Insert color value into selected cells";
            this.btnLeafColors.Click += this.btnLeafColors_Click;
            // 
            // btnLEafInterpLinear
            // 
            this.btnLEafInterpLinear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLEafInterpLinear.Image = (Image)resources.GetObject("btnLEafInterpLinear.Image");
            this.btnLEafInterpLinear.ImageTransparentColor = Color.Magenta;
            this.btnLEafInterpLinear.Name = "btnLEafInterpLinear";
            this.btnLEafInterpLinear.Size = new Size(29, 27);
            this.btnLEafInterpLinear.ToolTipText = "Smoothly interpolate between 2 selected values.\r\nAutomatically fills in values.";
            this.btnLEafInterpLinear.Click += this.btnLEafInterpLinear_Click;
            // 
            // btnLeafSplit
            // 
            this.btnLeafSplit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafSplit.Image = (Image)resources.GetObject("btnLeafSplit.Image");
            this.btnLeafSplit.ImageTransparentColor = Color.Magenta;
            this.btnLeafSplit.Name = "btnLeafSplit";
            this.btnLeafSplit.Size = new Size(29, 27);
            this.btnLeafSplit.ToolTipText = "Split leaf at selected beat.\r\nCreates a new file.";
            this.btnLeafSplit.Click += this.btnLeafSplit_Click;
            // 
            // btnLeafRandomValues
            // 
            this.btnLeafRandomValues.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafRandomValues.Image = (Image)resources.GetObject("btnLeafRandomValues.Image");
            this.btnLeafRandomValues.ImageTransparentColor = Color.Magenta;
            this.btnLeafRandomValues.Name = "btnLeafRandomValues";
            this.btnLeafRandomValues.Size = new Size(29, 27);
            this.btnLeafRandomValues.ToolTipText = "Clears the current track and sets random values.";
            this.btnLeafRandomValues.Click += this.btnLeafRandomValues_Click;
            // 
            // btnLeafZoom
            // 
            this.btnLeafZoom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafZoom.Image = (Image)resources.GetObject("btnLeafZoom.Image");
            this.btnLeafZoom.ImageTransparentColor = Color.Magenta;
            this.btnLeafZoom.Name = "btnLeafZoom";
            this.btnLeafZoom.Size = new Size(29, 27);
            this.btnLeafZoom.ToolTipText = "Click to show zoom.\r\nCTRL+scroll = horizontal\r\nSHIFT+scroll = vertical";
            this.btnLeafZoom.Click += this.btnLeafZoom_Click;
            // 
            // btnLeafAutoPlace
            // 
            this.btnLeafAutoPlace.CheckOnClick = true;
            this.btnLeafAutoPlace.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLeafAutoPlace.Image = (Image)resources.GetObject("btnLeafAutoPlace.Image");
            this.btnLeafAutoPlace.ImageTransparentColor = Color.Magenta;
            this.btnLeafAutoPlace.Name = "btnLeafAutoPlace";
            this.btnLeafAutoPlace.Size = new Size(29, 27);
            this.btnLeafAutoPlace.ToolTipText = "Enable auto-insert on click\r\n(inserts \"1\" on kTraitBool and kTraitAction objects)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnTrackApply);
            this.splitContainer1.Panel1.Controls.Add(this.dropTrackLane);
            this.splitContainer1.Panel1.Controls.Add(this.dropParamPath);
            this.splitContainer1.Panel1.Controls.Add(this.dropObjects);
            this.splitContainer1.Panel1.Controls.Add(this.propertyGridLeaf);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerLeafSide);
            this.splitContainer1.Size = new Size(933, 519);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 119;
            // 
            // propertyGridLeaf
            // 
            this.propertyGridLeaf.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLeaf.CategoryForeColor = Color.White;
            this.propertyGridLeaf.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridLeaf.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridLeaf.Dock = DockStyle.Fill;
            this.propertyGridLeaf.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridLeaf.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLeaf.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridLeaf.HelpForeColor = Color.White;
            this.propertyGridLeaf.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridLeaf.Location = new Point(0, 0);
            this.propertyGridLeaf.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridLeaf.Name = "propertyGridLeaf";
            this.propertyGridLeaf.PropertySort = PropertySort.Categorized;
            this.propertyGridLeaf.RightToLeft = RightToLeft.No;
            this.propertyGridLeaf.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridLeaf.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridLeaf.Size = new Size(220, 519);
            this.propertyGridLeaf.TabIndex = 0;
            this.propertyGridLeaf.ToolbarVisible = false;
            this.propertyGridLeaf.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLeaf.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridLeaf.ViewForeColor = Color.White;
            // 
            // splitContainerLeafSide
            // 
            this.splitContainerLeafSide.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainerLeafSide.Dock = DockStyle.Fill;
            this.splitContainerLeafSide.FixedPanel = FixedPanel.Panel2;
            this.splitContainerLeafSide.Location = new Point(0, 0);
            this.splitContainerLeafSide.Margin = new Padding(4, 3, 4, 3);
            this.splitContainerLeafSide.Name = "splitContainerLeafSide";
            this.splitContainerLeafSide.Orientation = Orientation.Horizontal;
            // 
            // splitContainerLeafSide.Panel1
            // 
            this.splitContainerLeafSide.Panel1.Controls.Add(this.panelZoom);
            this.splitContainerLeafSide.Panel1.Controls.Add(this.trackEditor);
            this.splitContainerLeafSide.Panel1.Controls.Add(this.vScrollBarTrackEditor);
            this.splitContainerLeafSide.Panel1.Controls.Add(this.leafToolStrip);
            this.splitContainerLeafSide.Panel1.Controls.Add(this.splitContainerTopbar);
            // 
            // splitContainerLeafSide.Panel2
            // 
            this.splitContainerLeafSide.Panel2.Controls.Add(this.textEditor);
            this.splitContainerLeafSide.Panel2.Controls.Add(this.btnRawImport);
            this.splitContainerLeafSide.Size = new Size(708, 519);
            this.splitContainerLeafSide.SplitterDistance = 385;
            this.splitContainerLeafSide.SplitterWidth = 5;
            this.splitContainerLeafSide.TabIndex = 120;
            // 
            // splitContainerTopbar
            // 
            this.splitContainerTopbar.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainerTopbar.Dock = DockStyle.Top;
            this.splitContainerTopbar.FixedPanel = FixedPanel.Panel1;
            this.splitContainerTopbar.IsSplitterFixed = true;
            this.splitContainerTopbar.Location = new Point(0, 0);
            this.splitContainerTopbar.Margin = new Padding(4, 3, 4, 3);
            this.splitContainerTopbar.Name = "splitContainerTopbar";
            // 
            // splitContainerTopbar.Panel1
            // 
            this.splitContainerTopbar.Panel1.Controls.Add(this.labelSequencer);
            // 
            // splitContainerTopbar.Panel2
            // 
            this.splitContainerTopbar.Panel2.Controls.Add(this.leaftoolsToolStrip);
            this.splitContainerTopbar.Size = new Size(708, 30);
            this.splitContainerTopbar.SplitterDistance = 100;
            this.splitContainerTopbar.SplitterWidth = 5;
            this.splitContainerTopbar.TabIndex = 144;
            // 
            // labelSequencer
            // 
            this.labelSequencer.AutoSize = true;
            this.labelSequencer.BackColor = Color.FromArgb(10, 10, 10);
            this.labelSequencer.Dock = DockStyle.Bottom;
            this.labelSequencer.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelSequencer.ForeColor = Color.White;
            this.labelSequencer.Location = new Point(0, 17);
            this.labelSequencer.Margin = new Padding(4, 0, 4, 0);
            this.labelSequencer.MinimumSize = new Size(117, 0);
            this.labelSequencer.Name = "labelSequencer";
            this.labelSequencer.Size = new Size(117, 13);
            this.labelSequencer.TabIndex = 96;
            this.labelSequencer.Text = "Sequencer";
            this.labelSequencer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textEditor
            // 
            this.textEditor.AccessibleDescription = "Textbox control";
            this.textEditor.AccessibleName = "Fast Colored Text Box";
            this.textEditor.AccessibleRole = AccessibleRole.Text;
            this.textEditor.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            this.textEditor.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            this.textEditor.AutoScrollMinSize = new Size(170, 14);
            this.textEditor.BackBrush = null;
            this.textEditor.BackColor = Color.FromArgb(31, 31, 31);
            this.textEditor.CharHeight = 14;
            this.textEditor.CharWidth = 8;
            this.textEditor.DefaultMarkerSize = 8;
            this.textEditor.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            this.textEditor.Dock = DockStyle.Fill;
            this.textEditor.FindForm = null;
            this.textEditor.FoldingHighlightColor = Color.LightGray;
            this.textEditor.FoldingHighlightEnabled = false;
            this.textEditor.ForeColor = Color.White;
            this.textEditor.GoToForm = null;
            this.textEditor.Hotkeys = resources.GetString("textEditor.Hotkeys");
            this.textEditor.IndentBackColor = Color.Black;
            this.textEditor.IsReplaceMode = false;
            this.textEditor.Location = new Point(54, 0);
            this.textEditor.Name = "textEditor";
            this.textEditor.Paddings = new Padding(0);
            this.textEditor.ReplaceForm = null;
            this.textEditor.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            this.textEditor.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("textEditor.ServiceColors");
            this.textEditor.Size = new Size(654, 129);
            this.textEditor.TabIndex = 45;
            this.textEditor.Text = "sequencer object data";
            this.textEditor.ToolTipDelay = 100;
            this.textEditor.Zoom = 100;
            // 
            // Form_LeafEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(933, 519);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_LeafEditor";
            this.Text = "Leaf Editor";
            this.Shown += this.Form_LeafEditor_Shown;
            ((System.ComponentModel.ISupportInitialize)this.trackZoomVert).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.trackZoom).EndInit();
            this.panelZoom.ResumeLayout(false);
            this.panelZoom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.trackEditor).EndInit();
            this.leafToolStrip.ResumeLayout(false);
            this.leafToolStrip.PerformLayout();
            this.leaftoolsToolStrip.ResumeLayout(false);
            this.leaftoolsToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainerLeafSide.Panel1.ResumeLayout(false);
            this.splitContainerLeafSide.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainerLeafSide).EndInit();
            this.splitContainerLeafSide.ResumeLayout(false);
            this.splitContainerTopbar.Panel1.ResumeLayout(false);
            this.splitContainerTopbar.Panel1.PerformLayout();
            this.splitContainerTopbar.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainerTopbar).EndInit();
            this.splitContainerTopbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.textEditor).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelZoom;
        private System.Windows.Forms.Label labelScrollH;
        private System.Windows.Forms.Label labelScrollV;
        private System.Windows.Forms.TrackBar trackZoomVert;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.VScrollBar vScrollBarTrackEditor;
        private System.Windows.Forms.ToolStrip leafToolStrip;
        private System.Windows.Forms.ToolStripButton btnTrackAdd;
        private System.Windows.Forms.ToolStripButton btnTrackDelete;
        private System.Windows.Forms.ToolStripButton btnTrackUp;
        private System.Windows.Forms.ToolStripButton btnTrackDown;
        private System.Windows.Forms.ToolStripButton btnTrackCopy;
        private System.Windows.Forms.ToolStripButton btnTrackPaste;
        private System.Windows.Forms.ToolStripButton btnTrackClear;
        private System.Windows.Forms.ToolStripButton btnTrackPlayback;
        private System.Windows.Forms.ToolStripButton btnTrackColorExport;
        private System.Windows.Forms.ToolStripButton btnTrackColorImport;
        private System.Windows.Forms.ToolStripButton btnLeafRandom;
        private System.Windows.Forms.Button btnRawImport;
        private System.Windows.Forms.ToolStrip leaftoolsToolStrip;
        private System.Windows.Forms.ToolStripButton btnLeafColors;
        private System.Windows.Forms.ToolStripButton btnLEafInterpLinear;
        private System.Windows.Forms.ToolStripButton btnLeafSplit;
        private System.Windows.Forms.ToolStripButton btnLeafRandomValues;
        private System.Windows.Forms.ToolStripButton btnLeafZoom;
        private System.Windows.Forms.ToolStripButton btnLeafAutoPlace;
        private System.Windows.Forms.ComboBox dropTrackLane;
        private System.Windows.Forms.Button btnTrackApply;
        public ComboBox dropObjects;
        public ComboBox dropParamPath;
        private SplitContainer splitContainer1;
        private ToolStrip gateToolStrip;
        public PropertyGrid propertyGridLeaf;
        private SplitContainer splitContainerLeafSide;
        private SplitContainer splitContainerTopbar;
        private Label labelSequencer;
        private FastColoredTextBoxNS.FastColoredTextBox textEditor;
        public DataGridView trackEditor;
        private DataGridViewTextBoxColumn LeafEnabled;
        private DataGridViewTextBoxColumn LeafAudio;
        private DataGridViewTextBoxColumn LeafMultilane;
    }
}