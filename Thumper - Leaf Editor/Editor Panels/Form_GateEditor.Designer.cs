namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_GateEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GateEditor));
            this.gateLvlList = new DataGridView();
            this.gatePhaseNum = new DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            this.GateLvlRuntime = new DataGridViewTextBoxColumn();
            this.gateToolStrip = new ToolStrip();
            this.btnGateLvlAdd = new ToolStripButton();
            this.btnGateLvlDelete = new ToolStripButton();
            this.btnGateLvlUp = new ToolStripButton();
            this.btnGateLvlDown = new ToolStripButton();
            this.btnGateCopy = new ToolStripButton();
            this.btnGatePaste = new ToolStripButton();
            this.label1 = new Label();
            this.propertyGridGate = new PropertyGrid();
            this.toolTip1 = new ToolTip(this.components);
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panelMain = new Panel();
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).BeginInit();
            this.gateToolStrip.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gateLvlList
            // 
            this.gateLvlList.AllowDrop = true;
            this.gateLvlList.AllowUserToAddRows = false;
            this.gateLvlList.AllowUserToDeleteRows = false;
            this.gateLvlList.AllowUserToResizeRows = false;
            this.gateLvlList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.gateLvlList.BorderStyle = BorderStyle.None;
            this.gateLvlList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gateLvlList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gateLvlList.ColumnHeadersHeight = 20;
            this.gateLvlList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gateLvlList.Columns.AddRange(new DataGridViewColumn[] { this.gatePhaseNum, this.dataGridViewImageColumn1, this.dataGridViewTextBoxColumn1, this.GateLvlRuntime });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.DefaultCellStyle = dataGridViewCellStyle3;
            this.gateLvlList.Dock = DockStyle.Fill;
            this.gateLvlList.EnableHeadersVisualStyles = false;
            this.gateLvlList.GridColor = Color.Black;
            this.gateLvlList.Location = new Point(24, 13);
            this.gateLvlList.Margin = new Padding(4, 3, 4, 3);
            this.gateLvlList.Name = "gateLvlList";
            this.gateLvlList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle4.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gateLvlList.RowHeadersVisible = false;
            this.gateLvlList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle5.BackColor = Color.Green;
            dataGridViewCellStyle5.Font = new Font("Relay-Medium", 8.249999F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            this.gateLvlList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.gateLvlList.RowTemplate.DefaultCellStyle.BackColor = Color.Green;
            this.gateLvlList.RowTemplate.DefaultCellStyle.Font = new Font("Relay-Medium", 8.249999F);
            this.gateLvlList.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.gateLvlList.RowTemplate.Height = 20;
            this.gateLvlList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.gateLvlList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gateLvlList.Size = new Size(269, 268);
            this.gateLvlList.TabIndex = 118;
            this.gateLvlList.Tag = "editorpaneldgv";
            this.gateLvlList.CellClick += this.gateLvlList_CellClick_1;
            this.gateLvlList.CellDoubleClick += this.gateLvlList_CellDoubleClick;
            this.gateLvlList.CellPainting += this.gateLvlList_CellPainting;
            this.gateLvlList.RowPrePaint += this.gateLvlList_RowPrePaint;
            this.gateLvlList.SelectionChanged += this.gateLvlList_SelectionChanged;
            this.gateLvlList.DragDrop += this.gateLvlList_DragDrop;
            this.gateLvlList.DragEnter += this.gateLvlList_DragEnter;
            this.gateLvlList.DragOver += this.gateLvlList_DragOver;
            this.gateLvlList.MouseDown += this.gateLvlList_MouseDown;
            this.gateLvlList.MouseMove += this.gateLvlList_MouseMove;
            // 
            // gatePhaseNum
            // 
            this.gatePhaseNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.gatePhaseNum.HeaderText = "";
            this.gatePhaseNum.Name = "gatePhaseNum";
            this.gatePhaseNum.ReadOnly = true;
            this.gatePhaseNum.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.gatePhaseNum.Width = 5;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewImageColumn1.FillWeight = 1F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Lvl";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // GateLvlRuntime
            // 
            this.GateLvlRuntime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.GateLvlRuntime.DefaultCellStyle = dataGridViewCellStyle2;
            this.GateLvlRuntime.FillWeight = 50F;
            this.GateLvlRuntime.HeaderText = "Runtime";
            this.GateLvlRuntime.Name = "GateLvlRuntime";
            this.GateLvlRuntime.ReadOnly = true;
            this.GateLvlRuntime.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // gateToolStrip
            // 
            this.gateToolStrip.AutoSize = false;
            this.gateToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.gateToolStrip.Dock = DockStyle.Left;
            this.gateToolStrip.GripMargin = new Padding(0);
            this.gateToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.gateToolStrip.ImageScalingSize = new Size(20, 20);
            this.gateToolStrip.Items.AddRange(new ToolStripItem[] { this.btnGateLvlAdd, this.btnGateLvlDelete, this.btnGateLvlUp, this.btnGateLvlDown, this.btnGateCopy, this.btnGatePaste });
            this.gateToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.gateToolStrip.Location = new Point(0, 13);
            this.gateToolStrip.Name = "gateToolStrip";
            this.gateToolStrip.Padding = new Padding(0);
            this.gateToolStrip.RenderMode = ToolStripRenderMode.System;
            this.gateToolStrip.Size = new Size(24, 268);
            this.gateToolStrip.Stretch = true;
            this.gateToolStrip.TabIndex = 143;
            // 
            // btnGateLvlAdd
            // 
            this.btnGateLvlAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGateLvlAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnGateLvlAdd.ForeColor = Color.White;
            this.btnGateLvlAdd.Image = Properties.Resources.icon_plus;
            this.btnGateLvlAdd.ImageTransparentColor = Color.Magenta;
            this.btnGateLvlAdd.Margin = new Padding(0);
            this.btnGateLvlAdd.Name = "btnGateLvlAdd";
            this.btnGateLvlAdd.Size = new Size(23, 24);
            this.btnGateLvlAdd.ToolTipText = "Add new phase";
            this.btnGateLvlAdd.Click += this.btnGateLvlAdd_Click;
            // 
            // btnGateLvlDelete
            // 
            this.btnGateLvlDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGateLvlDelete.Enabled = false;
            this.btnGateLvlDelete.Image = Properties.Resources.icon_remove2;
            this.btnGateLvlDelete.ImageTransparentColor = Color.Magenta;
            this.btnGateLvlDelete.Margin = new Padding(0);
            this.btnGateLvlDelete.Name = "btnGateLvlDelete";
            this.btnGateLvlDelete.Size = new Size(23, 24);
            this.btnGateLvlDelete.ToolTipText = "Delete selected phase";
            this.btnGateLvlDelete.Click += this.btnGateLvlDelete_Click;
            // 
            // btnGateLvlUp
            // 
            this.btnGateLvlUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGateLvlUp.Enabled = false;
            this.btnGateLvlUp.Image = Properties.Resources.icon_arrowup2;
            this.btnGateLvlUp.ImageTransparentColor = Color.Magenta;
            this.btnGateLvlUp.Margin = new Padding(0);
            this.btnGateLvlUp.Name = "btnGateLvlUp";
            this.btnGateLvlUp.Size = new Size(23, 24);
            this.btnGateLvlUp.ToolTipText = "Move selected phase up";
            this.btnGateLvlUp.Click += this.btnGateLvlUp_Click;
            // 
            // btnGateLvlDown
            // 
            this.btnGateLvlDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGateLvlDown.Enabled = false;
            this.btnGateLvlDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnGateLvlDown.ImageTransparentColor = Color.Magenta;
            this.btnGateLvlDown.Margin = new Padding(0);
            this.btnGateLvlDown.Name = "btnGateLvlDown";
            this.btnGateLvlDown.Size = new Size(23, 24);
            this.btnGateLvlDown.ToolTipText = "Move selected phase down";
            this.btnGateLvlDown.Click += this.btnGateLvlDown_Click;
            // 
            // btnGateCopy
            // 
            this.btnGateCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGateCopy.Enabled = false;
            this.btnGateCopy.Image = Properties.Resources.icon_copy2;
            this.btnGateCopy.ImageTransparentColor = Color.Magenta;
            this.btnGateCopy.Margin = new Padding(0);
            this.btnGateCopy.Name = "btnGateCopy";
            this.btnGateCopy.Size = new Size(23, 24);
            this.btnGateCopy.ToolTipText = "Copy selected sublevel";
            this.btnGateCopy.Click += this.btnGateCopy_Click;
            // 
            // btnGatePaste
            // 
            this.btnGatePaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnGatePaste.Enabled = false;
            this.btnGatePaste.Image = Properties.Resources.icon_paste2;
            this.btnGatePaste.ImageTransparentColor = Color.Magenta;
            this.btnGatePaste.Name = "btnGatePaste";
            this.btnGatePaste.Size = new Size(23, 24);
            this.btnGatePaste.ToolTipText = "Paste the copied sublevel";
            this.btnGatePaste.Click += this.btnGatePaste_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.FromArgb(10, 10, 10);
            this.label1.Dock = DockStyle.Top;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(0, 0);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(79, 13);
            this.label1.TabIndex = 117;
            this.label1.Text = "Boss Phases";
            // 
            // propertyGridGate
            // 
            this.propertyGridGate.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.CategoryForeColor = Color.White;
            this.propertyGridGate.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridGate.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridGate.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridGate.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridGate.HelpForeColor = Color.White;
            this.propertyGridGate.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridGate.Location = new Point(353, 12);
            this.propertyGridGate.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridGate.Name = "propertyGridGate";
            this.propertyGridGate.PropertySort = PropertySort.Categorized;
            this.propertyGridGate.RightToLeft = RightToLeft.No;
            this.propertyGridGate.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridGate.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridGate.Size = new Size(221, 279);
            this.propertyGridGate.TabIndex = 0;
            this.propertyGridGate.ToolbarVisible = false;
            this.propertyGridGate.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridGate.ViewForeColor = Color.White;
            this.propertyGridGate.PropertyValueChanged += this.propertyGridGate_PropertyValueChanged;
            // 
            // dockPanel1
            // 
            this.dockPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dockPanel1.Location = new Point(-4, -4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new Size(633, 372);
            this.dockPanel1.TabIndex = 119;
            this.dockPanel1.ActiveContentChanged += this.dockPanel1_ActiveContentChanged;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.gateLvlList);
            this.panelMain.Controls.Add(this.gateToolStrip);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Location = new Point(12, 12);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(293, 281);
            this.panelMain.TabIndex = 120;
            // 
            // Form_GateEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.ClientSize = new Size(625, 364);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.propertyGridGate);
            this.Controls.Add(this.dockPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_GateEditor";
            this.Text = "Gate Editor";
            this.Shown += this.Form_GateEditor_Shown;
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).EndInit();
            this.gateToolStrip.ResumeLayout(false);
            this.gateToolStrip.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
        public PropertyGrid propertyGridGate;
        private ToolStrip gateToolStrip;
        private ToolStripButton btnGateLvlAdd;
        private ToolStripButton btnGateLvlDelete;
        private ToolStripButton btnGateLvlUp;
        private ToolStripButton btnGateLvlDown;
        private DataGridView gateLvlList;
        private Label label1;
        private ToolTip toolTip1;
        private DataGridViewTextBoxColumn gatePhaseNum;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn GateLvlRuntime;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private Panel panelMain;
        private ToolStripButton btnGateCopy;
        private ToolStripButton btnGatePaste;
    }
}