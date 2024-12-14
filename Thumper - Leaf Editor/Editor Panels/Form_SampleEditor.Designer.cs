namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_SampleEditor
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SampleEditor));
            this.toolTip1 = new ToolTip(this.components);
            this.sampleList = new DataGridView();
            this.Columnplaybuttons = new DataGridViewButtonColumn();
            this.SampleName = new DataGridViewTextBoxColumn();
            this.label54 = new Label();
            this.sampleToolStrip = new ToolStrip();
            this.btnSampleAdd = new ToolStripButton();
            this.btnSampleDelete = new ToolStripButton();
            this.FSBtoSamp = new ToolStripButton();
            this.lblSampleFSBhelp = new ToolStripLabel();
            this.propertyGridSample = new PropertyGrid();
            this.lblMasterlvllistHelp = new Label();
            this.splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)this.sampleList).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sampleList
            // 
            this.sampleList.AllowDrop = true;
            this.sampleList.AllowUserToAddRows = false;
            this.sampleList.AllowUserToDeleteRows = false;
            this.sampleList.AllowUserToResizeRows = false;
            this.sampleList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.sampleList.BorderStyle = BorderStyle.None;
            this.sampleList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.sampleList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.sampleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampleList.ColumnHeadersHeight = 20;
            this.sampleList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sampleList.Columns.AddRange(new DataGridViewColumn[] { this.Columnplaybuttons, this.SampleName });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.sampleList.DefaultCellStyle = dataGridViewCellStyle3;
            this.sampleList.Dock = DockStyle.Fill;
            this.sampleList.EnableHeadersVisualStyles = false;
            this.sampleList.GridColor = Color.Black;
            this.sampleList.Location = new Point(0, 13);
            this.sampleList.Margin = new Padding(4, 3, 4, 3);
            this.sampleList.Name = "sampleList";
            this.sampleList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle4.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.sampleList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.sampleList.RowHeadersVisible = false;
            this.sampleList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sampleList.RowTemplate.Height = 20;
            this.sampleList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.sampleList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.sampleList.Size = new Size(300, 477);
            this.sampleList.TabIndex = 145;
            this.sampleList.Tag = "editorpaneldgv";
            this.sampleList.CellClick += this.sampleList_CellClick;
            this.sampleList.CellPainting += this.sampleList_CellPainting;
            this.sampleList.DragDrop += this.sampleList_DragDrop;
            this.sampleList.DragEnter += this.sampleList_DragEnter;
            // 
            // Columnplaybuttons
            // 
            this.Columnplaybuttons.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Columnplaybuttons.FlatStyle = FlatStyle.Flat;
            this.Columnplaybuttons.HeaderText = "";
            this.Columnplaybuttons.Name = "Columnplaybuttons";
            this.Columnplaybuttons.ReadOnly = true;
            this.Columnplaybuttons.Width = 5;
            // 
            // SampleName
            // 
            this.SampleName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.SampleName.DefaultCellStyle = dataGridViewCellStyle2;
            this.SampleName.FillWeight = 8.913044F;
            this.SampleName.HeaderText = "Sample Name";
            this.SampleName.Name = "SampleName";
            this.SampleName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = Color.FromArgb(10, 10, 10);
            this.label54.Dock = DockStyle.Top;
            this.label54.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label54.ForeColor = Color.White;
            this.label54.Location = new Point(0, 0);
            this.label54.Margin = new Padding(4, 0, 4, 0);
            this.label54.Name = "label54";
            this.label54.Size = new Size(54, 13);
            this.label54.TabIndex = 146;
            this.label54.Text = "Samples";
            // 
            // sampleToolStrip
            // 
            this.sampleToolStrip.AutoSize = false;
            this.sampleToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.sampleToolStrip.Dock = DockStyle.Bottom;
            this.sampleToolStrip.GripMargin = new Padding(0);
            this.sampleToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new ToolStripItem[] { this.btnSampleAdd, this.btnSampleDelete, this.FSBtoSamp, this.lblSampleFSBhelp });
            this.sampleToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.sampleToolStrip.Location = new Point(0, 490);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new Padding(0);
            this.sampleToolStrip.RenderMode = ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new Size(300, 29);
            this.sampleToolStrip.Stretch = true;
            this.sampleToolStrip.TabIndex = 150;
            // 
            // btnSampleAdd
            // 
            this.btnSampleAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSampleAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnSampleAdd.ForeColor = Color.White;
            this.btnSampleAdd.Image = Properties.Resources.icon_plus;
            this.btnSampleAdd.ImageTransparentColor = Color.Magenta;
            this.btnSampleAdd.Margin = new Padding(0);
            this.btnSampleAdd.Name = "btnSampleAdd";
            this.btnSampleAdd.Size = new Size(24, 29);
            this.btnSampleAdd.ToolTipText = "Add new sample";
            this.btnSampleAdd.Click += this.btnSampleAdd_Click;
            // 
            // btnSampleDelete
            // 
            this.btnSampleDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSampleDelete.Enabled = false;
            this.btnSampleDelete.Image = Properties.Resources.icon_remove2;
            this.btnSampleDelete.ImageTransparentColor = Color.Magenta;
            this.btnSampleDelete.Margin = new Padding(0);
            this.btnSampleDelete.Name = "btnSampleDelete";
            this.btnSampleDelete.Size = new Size(24, 29);
            this.btnSampleDelete.ToolTipText = "Delete selected phase";
            this.btnSampleDelete.Click += this.btnSampleDelete_Click;
            // 
            // FSBtoSamp
            // 
            this.FSBtoSamp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.FSBtoSamp.Enabled = false;
            this.FSBtoSamp.Image = Properties.Resources.icon_import;
            this.FSBtoSamp.ImageTransparentColor = Color.Magenta;
            this.FSBtoSamp.Name = "FSBtoSamp";
            this.FSBtoSamp.Size = new Size(24, 26);
            this.FSBtoSamp.ToolTipText = "Import FSB files to Sample format";
            this.FSBtoSamp.Click += this.FSBtoSamp_Click;
            // 
            // lblSampleFSBhelp
            // 
            this.lblSampleFSBhelp.Alignment = ToolStripItemAlignment.Right;
            this.lblSampleFSBhelp.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblSampleFSBhelp.ForeColor = Color.DodgerBlue;
            this.lblSampleFSBhelp.Name = "lblSampleFSBhelp";
            this.lblSampleFSBhelp.Size = new Size(161, 26);
            this.lblSampleFSBhelp.Text = "How to get a .FSB audio file";
            this.lblSampleFSBhelp.Click += this.lblSampleFSBhelp_Click;
            // 
            // propertyGridSample
            // 
            this.propertyGridSample.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.CategoryForeColor = Color.White;
            this.propertyGridSample.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridSample.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridSample.Dock = DockStyle.Fill;
            this.propertyGridSample.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridSample.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridSample.HelpForeColor = Color.White;
            this.propertyGridSample.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridSample.Location = new Point(0, 0);
            this.propertyGridSample.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridSample.Name = "propertyGridSample";
            this.propertyGridSample.PropertySort = PropertySort.Categorized;
            this.propertyGridSample.RightToLeft = RightToLeft.No;
            this.propertyGridSample.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridSample.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridSample.Size = new Size(628, 519);
            this.propertyGridSample.TabIndex = 148;
            this.propertyGridSample.ToolbarVisible = false;
            this.propertyGridSample.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridSample.ViewForeColor = Color.White;
            // 
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = Cursors.Help;
            this.lblMasterlvllistHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblMasterlvllistHelp.ForeColor = Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new Point(954, -3);
            this.lblMasterlvllistHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sampleList);
            this.splitContainer1.Panel1.Controls.Add(this.sampleToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.label54);
            this.splitContainer1.Panel1.Controls.Add(this.lblMasterlvllistHelp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridSample);
            this.splitContainer1.Size = new Size(933, 519);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 136;
            // 
            // Form_SampleEditor
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
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_SampleEditor";
            this.Text = "Sample Editor";
            ((System.ComponentModel.ISupportInitialize)this.sampleList).EndInit();
            this.sampleToolStrip.ResumeLayout(false);
            this.sampleToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView sampleList;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.ToolStrip sampleToolStrip;
        private System.Windows.Forms.ToolStripButton btnSampleAdd;
        private System.Windows.Forms.ToolStripButton btnSampleDelete;
        private System.Windows.Forms.ToolStripButton FSBtoSamp;
        private System.Windows.Forms.ToolStripLabel lblSampleFSBhelp;
        public PropertyGrid propertyGridSample;
        private Label lblMasterlvllistHelp;
        private SplitContainer splitContainer1;
        private DataGridViewButtonColumn Columnplaybuttons;
        private DataGridViewTextBoxColumn SampleName;
    }
}