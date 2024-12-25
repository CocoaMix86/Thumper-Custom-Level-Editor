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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GateEditor));
            this.splitContainer1 = new SplitContainer();
            this.gateToolStrip = new ToolStrip();
            this.btnGateLvlAdd = new ToolStripButton();
            this.btnGateLvlDelete = new ToolStripButton();
            this.btnGateLvlUp = new ToolStripButton();
            this.btnGateLvlDown = new ToolStripButton();
            this.gateLvlList = new DataGridView();
            this.gatePhaseNum = new DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            this.GateLvlRuntime = new DataGridViewTextBoxColumn();
            this.label1 = new Label();
            this.lblMasterlvllistHelp = new Label();
            this.propertyGridGate = new PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gateToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gateToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.gateLvlList);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.lblMasterlvllistHelp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridGate);
            this.splitContainer1.Size = new Size(625, 364);
            this.splitContainer1.SplitterDistance = 378;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 118;
            // 
            // gateToolStrip
            // 
            this.gateToolStrip.AutoSize = false;
            this.gateToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.gateToolStrip.Dock = DockStyle.Bottom;
            this.gateToolStrip.GripMargin = new Padding(0);
            this.gateToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.gateToolStrip.ImageScalingSize = new Size(20, 20);
            this.gateToolStrip.Items.AddRange(new ToolStripItem[] { this.btnGateLvlAdd, this.btnGateLvlDelete, this.btnGateLvlUp, this.btnGateLvlDown });
            this.gateToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.gateToolStrip.Location = new Point(0, 335);
            this.gateToolStrip.Name = "gateToolStrip";
            this.gateToolStrip.Padding = new Padding(0);
            this.gateToolStrip.RenderMode = ToolStripRenderMode.System;
            this.gateToolStrip.Size = new Size(378, 29);
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
            this.btnGateLvlAdd.Size = new Size(24, 29);
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
            this.btnGateLvlDelete.Size = new Size(24, 29);
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
            this.btnGateLvlUp.Size = new Size(24, 29);
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
            this.btnGateLvlDown.Size = new Size(24, 29);
            this.btnGateLvlDown.ToolTipText = "Move selected phase down";
            this.btnGateLvlDown.Click += this.btnGateLvlDown_Click;
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
            this.gateLvlList.Location = new Point(0, 13);
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
            this.gateLvlList.RowTemplate.Height = 20;
            this.gateLvlList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.gateLvlList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gateLvlList.Size = new Size(378, 351);
            this.gateLvlList.TabIndex = 118;
            this.gateLvlList.Tag = "editorpaneldgv";
            this.gateLvlList.CellClick += this.gateLvlList_CellClick_1;
            this.gateLvlList.CellDoubleClick += this.gateLvlList_CellDoubleClick;
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
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.GateLvlRuntime.DefaultCellStyle = dataGridViewCellStyle2;
            this.GateLvlRuntime.FillWeight = 50F;
            this.GateLvlRuntime.HeaderText = "Runtime";
            this.GateLvlRuntime.Name = "GateLvlRuntime";
            this.GateLvlRuntime.ReadOnly = true;
            this.GateLvlRuntime.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = Cursors.Help;
            this.lblMasterlvllistHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblMasterlvllistHelp.ForeColor = Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new Point(700, -3);
            this.lblMasterlvllistHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // propertyGridGate
            // 
            this.propertyGridGate.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.CategoryForeColor = Color.White;
            this.propertyGridGate.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridGate.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridGate.Dock = DockStyle.Fill;
            this.propertyGridGate.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridGate.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridGate.HelpForeColor = Color.White;
            this.propertyGridGate.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridGate.Location = new Point(0, 0);
            this.propertyGridGate.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridGate.Name = "propertyGridGate";
            this.propertyGridGate.PropertySort = PropertySort.Categorized;
            this.propertyGridGate.RightToLeft = RightToLeft.No;
            this.propertyGridGate.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridGate.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridGate.Size = new Size(242, 364);
            this.propertyGridGate.TabIndex = 0;
            this.propertyGridGate.ToolbarVisible = false;
            this.propertyGridGate.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridGate.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridGate.ViewForeColor = Color.White;
            this.propertyGridGate.PropertyValueChanged += this.propertyGridGate_PropertyValueChanged;
            // 
            // Form_GateEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(625, 364);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_GateEditor";
            this.Text = "Gate Editor";
            this.Shown += this.Form_GateEditor_Shown;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gateToolStrip.ResumeLayout(false);
            this.gateToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private Label lblMasterlvllistHelp;
        public PropertyGrid propertyGridGate;
        private ToolStrip gateToolStrip;
        private ToolStripButton btnGateLvlAdd;
        private ToolStripButton btnGateLvlDelete;
        private ToolStripButton btnGateLvlUp;
        private ToolStripButton btnGateLvlDown;
        private DataGridView gateLvlList;
        private Label label1;
        private DataGridViewTextBoxColumn gatePhaseNum;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn GateLvlRuntime;
    }
}