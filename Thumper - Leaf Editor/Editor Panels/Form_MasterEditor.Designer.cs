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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MasterEditor));
            this.toolTip1 = new ToolTip(this.components);
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.propertyGridMaster = new PropertyGrid();
            this.panelMain = new Panel();
            this.masterLvlList = new DataGridView();
            this.SublevelNum = new DataGridViewTextBoxColumn();
            this.masterfiletype = new DataGridViewImageColumn();
            this.masterLvl = new DataGridViewTextBoxColumn();
            this.Runtime = new DataGridViewTextBoxColumn();
            this.masterCheckpoint = new DataGridViewTextBoxColumn();
            this.masterPlayPlus = new DataGridViewTextBoxColumn();
            this.masterIsolate = new DataGridViewTextBoxColumn();
            this.masterToolStrip = new ToolStrip();
            this.btnMasterLvlAdd = new ToolStripButton();
            this.btnMasterLvlDelete = new ToolStripButton();
            this.btnMasterLvlUp = new ToolStripButton();
            this.btnMasterLvlDown = new ToolStripButton();
            this.btnMasterLvlCopy = new ToolStripButton();
            this.btnMasterLvlPaste = new ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnMasterPlayback = new ToolStripButton();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.masterLvlList).BeginInit();
            this.masterToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dockPanel1.BackColor = Color.Black;
            this.dockPanel1.Location = new Point(-4, -4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new Size(661, 527);
            this.dockPanel1.TabIndex = 147;
            this.dockPanel1.ActiveContentChanged += this.dockPanel1_ActiveContentChanged;
            // 
            // propertyGridMaster
            // 
            this.propertyGridMaster.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.CategoryForeColor = Color.White;
            this.propertyGridMaster.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridMaster.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridMaster.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridMaster.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridMaster.HelpForeColor = Color.White;
            this.propertyGridMaster.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridMaster.Location = new Point(423, 12);
            this.propertyGridMaster.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridMaster.Name = "propertyGridMaster";
            this.propertyGridMaster.PropertySort = PropertySort.Categorized;
            this.propertyGridMaster.RightToLeft = RightToLeft.No;
            this.propertyGridMaster.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridMaster.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridMaster.Size = new Size(209, 341);
            this.propertyGridMaster.TabIndex = 148;
            this.propertyGridMaster.ToolbarVisible = false;
            this.propertyGridMaster.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridMaster.ViewForeColor = Color.White;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = Color.Black;
            this.panelMain.Controls.Add(this.masterLvlList);
            this.panelMain.Controls.Add(this.masterToolStrip);
            this.panelMain.Location = new Point(24, 12);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(354, 341);
            this.panelMain.TabIndex = 149;
            // 
            // masterLvlList
            // 
            this.masterLvlList.AllowDrop = true;
            this.masterLvlList.AllowUserToAddRows = false;
            this.masterLvlList.AllowUserToDeleteRows = false;
            this.masterLvlList.AllowUserToResizeRows = false;
            this.masterLvlList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.masterLvlList.BorderStyle = BorderStyle.None;
            this.masterLvlList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.masterLvlList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.masterLvlList.ColumnHeadersHeight = 20;
            this.masterLvlList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.masterLvlList.Columns.AddRange(new DataGridViewColumn[] { this.SublevelNum, this.masterfiletype, this.masterLvl, this.Runtime, this.masterCheckpoint, this.masterPlayPlus, this.masterIsolate });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.Green;
            dataGridViewCellStyle3.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.DefaultCellStyle = dataGridViewCellStyle3;
            this.masterLvlList.Dock = DockStyle.Fill;
            this.masterLvlList.EnableHeadersVisualStyles = false;
            this.masterLvlList.GridColor = Color.Black;
            this.masterLvlList.Location = new Point(24, 0);
            this.masterLvlList.Margin = new Padding(4, 3, 4, 3);
            this.masterLvlList.Name = "masterLvlList";
            this.masterLvlList.ReadOnly = true;
            this.masterLvlList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle4.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.masterLvlList.RowHeadersVisible = false;
            this.masterLvlList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.BackColor = Color.Green;
            dataGridViewCellStyle5.Font = new Font("Relay-Medium", 8.249999F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            this.masterLvlList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.masterLvlList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
            this.masterLvlList.RowTemplate.DefaultCellStyle.Font = new Font("Relay-Medium", 8.249999F);
            this.masterLvlList.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.masterLvlList.RowTemplate.Height = 20;
            this.masterLvlList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.masterLvlList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.masterLvlList.Size = new Size(330, 341);
            this.masterLvlList.TabIndex = 147;
            this.masterLvlList.Tag = "editorpaneldgv";
            this.masterLvlList.CellClick += this.masterLvlList_CellClick;
            this.masterLvlList.CellDoubleClick += this.masterLvlList_CellDoubleClick;
            this.masterLvlList.CellMouseDown += this.masterLvlList_CellMouseDown;
            this.masterLvlList.CellMouseUp += this.masterLvlList_CellMouseUp;
            this.masterLvlList.CellPainting += this.masterLvlList_CellPainting;
            this.masterLvlList.RowPrePaint += this.masterLvlList_RowPrePaint;
            this.masterLvlList.SelectionChanged += this.masterLvlList_SelectionChanged;
            this.masterLvlList.DragDrop += this.masterLvlList_DragDrop;
            this.masterLvlList.DragEnter += this.masterLvlList_DragEnter;
            this.masterLvlList.DragOver += this.masterLvlList_DragOver;
            this.masterLvlList.MouseDown += this.masterLvlList_MouseDown;
            this.masterLvlList.MouseMove += this.masterLvlList_MouseMove;
            this.masterLvlList.MouseUp += this.masterLvlList_MouseUp;
            // 
            // SublevelNum
            // 
            this.SublevelNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.SublevelNum.HeaderText = "";
            this.SublevelNum.Name = "SublevelNum";
            this.SublevelNum.ReadOnly = true;
            this.SublevelNum.Resizable = DataGridViewTriState.False;
            this.SublevelNum.Width = 18;
            // 
            // masterfiletype
            // 
            this.masterfiletype.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.masterfiletype.HeaderText = "";
            this.masterfiletype.Name = "masterfiletype";
            this.masterfiletype.ReadOnly = true;
            this.masterfiletype.Width = 5;
            // 
            // masterLvl
            // 
            this.masterLvl.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.masterLvl.FillWeight = 50F;
            this.masterLvl.HeaderText = "Sublevel";
            this.masterLvl.Name = "masterLvl";
            this.masterLvl.ReadOnly = true;
            this.masterLvl.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Runtime
            // 
            this.Runtime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Runtime.DefaultCellStyle = dataGridViewCellStyle2;
            this.Runtime.FillWeight = 50F;
            this.Runtime.HeaderText = "Runtime";
            this.Runtime.Name = "Runtime";
            this.Runtime.ReadOnly = true;
            this.Runtime.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // masterCheckpoint
            // 
            this.masterCheckpoint.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.masterCheckpoint.HeaderText = "Ch.";
            this.masterCheckpoint.MinimumWidth = 16;
            this.masterCheckpoint.Name = "masterCheckpoint";
            this.masterCheckpoint.ReadOnly = true;
            this.masterCheckpoint.Resizable = DataGridViewTriState.True;
            this.masterCheckpoint.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.masterCheckpoint.ToolTipText = "Spawn checkpoint after lvl";
            this.masterCheckpoint.Width = 30;
            // 
            // masterPlayPlus
            // 
            this.masterPlayPlus.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.masterPlayPlus.HeaderText = "P+";
            this.masterPlayPlus.MinimumWidth = 16;
            this.masterPlayPlus.Name = "masterPlayPlus";
            this.masterPlayPlus.ReadOnly = true;
            this.masterPlayPlus.Resizable = DataGridViewTriState.True;
            this.masterPlayPlus.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.masterPlayPlus.ToolTipText = "Play/Hide lvl in Play+ (useful for tutorials)";
            this.masterPlayPlus.Width = 25;
            // 
            // masterIsolate
            // 
            this.masterIsolate.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.masterIsolate.HeaderText = "Iso.";
            this.masterIsolate.MinimumWidth = 16;
            this.masterIsolate.Name = "masterIsolate";
            this.masterIsolate.ReadOnly = true;
            this.masterIsolate.Resizable = DataGridViewTriState.True;
            this.masterIsolate.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.masterIsolate.ToolTipText = "Isolate lvl. If enabled, only isolated lvls will play when testing in game";
            this.masterIsolate.Width = 32;
            // 
            // masterToolStrip
            // 
            this.masterToolStrip.AutoSize = false;
            this.masterToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.masterToolStrip.Dock = DockStyle.Left;
            this.masterToolStrip.GripMargin = new Padding(0);
            this.masterToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.masterToolStrip.ImageScalingSize = new Size(20, 20);
            this.masterToolStrip.Items.AddRange(new ToolStripItem[] { this.btnMasterLvlAdd, this.btnMasterLvlDelete, this.btnMasterLvlUp, this.btnMasterLvlDown, this.btnMasterLvlCopy, this.btnMasterLvlPaste, this.btnMasterPlayback });
            this.masterToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.masterToolStrip.Location = new Point(0, 0);
            this.masterToolStrip.Name = "masterToolStrip";
            this.masterToolStrip.Padding = new Padding(0);
            this.masterToolStrip.RenderMode = ToolStripRenderMode.System;
            this.masterToolStrip.Size = new Size(24, 341);
            this.masterToolStrip.Stretch = true;
            this.masterToolStrip.TabIndex = 149;
            // 
            // btnMasterLvlAdd
            // 
            this.btnMasterLvlAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMasterLvlAdd.ForeColor = Color.White;
            this.btnMasterLvlAdd.Image = Properties.Resources.icon_plus;
            this.btnMasterLvlAdd.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlAdd.Margin = new Padding(0);
            this.btnMasterLvlAdd.Name = "btnMasterLvlAdd";
            this.btnMasterLvlAdd.Size = new Size(23, 24);
            this.btnMasterLvlAdd.ToolTipText = "Add new sublevel to the list";
            this.btnMasterLvlAdd.Click += this.btnMasterLvlAdd_Click;
            // 
            // btnMasterLvlDelete
            // 
            this.btnMasterLvlDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDelete.Enabled = false;
            this.btnMasterLvlDelete.Image = Properties.Resources.icon_remove2;
            this.btnMasterLvlDelete.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlDelete.Margin = new Padding(0);
            this.btnMasterLvlDelete.Name = "btnMasterLvlDelete";
            this.btnMasterLvlDelete.Size = new Size(23, 24);
            this.btnMasterLvlDelete.ToolTipText = "Delete selected sublevel from this list";
            this.btnMasterLvlDelete.Click += this.btnMasterLvlDelete_Click;
            // 
            // btnMasterLvlUp
            // 
            this.btnMasterLvlUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlUp.Enabled = false;
            this.btnMasterLvlUp.Image = Properties.Resources.icon_arrowup2;
            this.btnMasterLvlUp.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlUp.Margin = new Padding(0);
            this.btnMasterLvlUp.Name = "btnMasterLvlUp";
            this.btnMasterLvlUp.Size = new Size(23, 24);
            this.btnMasterLvlUp.ToolTipText = "Move selected sublevel up";
            this.btnMasterLvlUp.Click += this.btnMasterLvlUp_Click;
            // 
            // btnMasterLvlDown
            // 
            this.btnMasterLvlDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDown.Enabled = false;
            this.btnMasterLvlDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnMasterLvlDown.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlDown.Margin = new Padding(0);
            this.btnMasterLvlDown.Name = "btnMasterLvlDown";
            this.btnMasterLvlDown.Size = new Size(23, 24);
            this.btnMasterLvlDown.ToolTipText = "Move selected sublevel down";
            this.btnMasterLvlDown.Click += this.btnMasterLvlDown_Click;
            // 
            // btnMasterLvlCopy
            // 
            this.btnMasterLvlCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlCopy.Enabled = false;
            this.btnMasterLvlCopy.Image = Properties.Resources.icon_copy2;
            this.btnMasterLvlCopy.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlCopy.Margin = new Padding(0);
            this.btnMasterLvlCopy.Name = "btnMasterLvlCopy";
            this.btnMasterLvlCopy.Size = new Size(23, 24);
            this.btnMasterLvlCopy.ToolTipText = "Copy selected sublevel";
            this.btnMasterLvlCopy.Click += this.btnMasterLvlCopy_Click;
            // 
            // btnMasterLvlPaste
            // 
            this.btnMasterLvlPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlPaste.Enabled = false;
            this.btnMasterLvlPaste.Image = Properties.Resources.icon_paste2;
            this.btnMasterLvlPaste.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlPaste.Name = "btnMasterLvlPaste";
            this.btnMasterLvlPaste.Size = new Size(23, 24);
            this.btnMasterLvlPaste.ToolTipText = "Paste the copied sublevel";
            this.btnMasterLvlPaste.Click += this.btnMasterLvlPaste_Click;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += this.timer1_Tick;
            // 
            // btnMasterPlayback
            // 
            this.btnMasterPlayback.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterPlayback.Image = Properties.Resources.icon_play2;
            this.btnMasterPlayback.ImageTransparentColor = Color.Magenta;
            this.btnMasterPlayback.Name = "btnMasterPlayback";
            this.btnMasterPlayback.Size = new Size(23, 24);
            this.btnMasterPlayback.Text = "toolStripButton1";
            this.btnMasterPlayback.ToolTipText = "Preview how the Lvl will sound.\r\n!!! NOTE !!!\r\nThis is only a preview, and may not be entirely accurate\r\nto how it will sound in-game.\r\n!!\r\nSelect 1 leaf to set playback to start at that position";
            this.btnMasterPlayback.Click += this.btnMasterPlayback_Click;
            // 
            // Form_MasterEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(653, 519);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.propertyGridMaster);
            this.Controls.Add(this.dockPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = Color.FromArgb(150, 150, 255);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_MasterEditor";
            this.Text = "Master Editor";
            this.Shown += this.Form_MasterEditor_Shown;
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.masterLvlList).EndInit();
            this.masterToolStrip.ResumeLayout(false);
            this.masterToolStrip.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        public PropertyGrid propertyGridMaster;
        private Panel panelMain;
        private DataGridView masterLvlList;
        private DataGridViewTextBoxColumn SublevelNum;
        private DataGridViewImageColumn masterfiletype;
        private DataGridViewTextBoxColumn masterLvl;
        private DataGridViewTextBoxColumn Runtime;
        private DataGridViewTextBoxColumn masterCheckpoint;
        private DataGridViewTextBoxColumn masterPlayPlus;
        private DataGridViewTextBoxColumn masterIsolate;
        private ToolStrip masterToolStrip;
        private ToolStripButton btnMasterLvlAdd;
        private ToolStripButton btnMasterLvlDelete;
        private ToolStripButton btnMasterLvlUp;
        private ToolStripButton btnMasterLvlDown;
        private ToolStripButton btnMasterLvlCopy;
        private ToolStripButton btnMasterLvlPaste;
        private System.Windows.Forms.Timer timer1;
        private ToolStripButton btnMasterPlayback;
    }
}