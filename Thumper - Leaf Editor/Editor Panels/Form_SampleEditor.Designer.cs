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
            this.labelCollapsePanel = new Label();
            this.sampleList = new DataGridView();
            this.Columnplaybuttons = new DataGridViewButtonColumn();
            this.SampleName = new DataGridViewTextBoxColumn();
            this.sampleRuntime = new DataGridViewTextBoxColumn();
            this.label54 = new Label();
            this.sampleToolStrip = new ToolStrip();
            this.btnSampleAdd = new ToolStripButton();
            this.btnSampleDelete = new ToolStripButton();
            this.FSBtoSamp = new ToolStripButton();
            this.propertyGridSample = new PropertyGrid();
            this.lblMasterlvllistHelp = new Label();
            this.splitContainer1 = new SplitContainer();
            this.lblLoading = new Label();
            this.label1 = new Label();
            this.label4 = new Label();
            this.pictureSpectrum = new PictureBox();
            this.pictureWave = new PictureBox();
            this.label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)this.sampleList).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureSpectrum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).BeginInit();
            this.SuspendLayout();
            // 
            // labelCollapsePanel
            // 
            this.labelCollapsePanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.labelCollapsePanel.BackColor = Color.Gray;
            this.labelCollapsePanel.BorderStyle = BorderStyle.FixedSingle;
            this.labelCollapsePanel.Cursor = Cursors.Hand;
            this.labelCollapsePanel.FlatStyle = FlatStyle.Popup;
            this.labelCollapsePanel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCollapsePanel.ForeColor = Color.White;
            this.labelCollapsePanel.Location = new Point(303, 0);
            this.labelCollapsePanel.Margin = new Padding(4, 0, 4, 0);
            this.labelCollapsePanel.MaximumSize = new Size(16, 16);
            this.labelCollapsePanel.MinimumSize = new Size(16, 16);
            this.labelCollapsePanel.Name = "labelCollapsePanel";
            this.labelCollapsePanel.Size = new Size(16, 16);
            this.labelCollapsePanel.TabIndex = 154;
            this.labelCollapsePanel.Text = ">";
            this.toolTip1.SetToolTip(this.labelCollapsePanel, "Hide/Reveal right panel");
            this.labelCollapsePanel.Click += this.labelCollapsePanel_Click;
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
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.sampleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampleList.ColumnHeadersHeight = 20;
            this.sampleList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sampleList.Columns.AddRange(new DataGridViewColumn[] { this.Columnplaybuttons, this.SampleName, this.sampleRuntime });
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
            this.sampleList.Location = new Point(24, 26);
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
            this.sampleList.Size = new Size(295, 493);
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
            this.SampleName.ReadOnly = true;
            this.SampleName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // sampleRuntime
            // 
            this.sampleRuntime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.sampleRuntime.HeaderText = "Runtime";
            this.sampleRuntime.Name = "sampleRuntime";
            this.sampleRuntime.ReadOnly = true;
            this.sampleRuntime.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.sampleRuntime.Width = 58;
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
            this.sampleToolStrip.Dock = DockStyle.Left;
            this.sampleToolStrip.GripMargin = new Padding(0);
            this.sampleToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new ToolStripItem[] { this.btnSampleAdd, this.btnSampleDelete, this.FSBtoSamp });
            this.sampleToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.sampleToolStrip.Location = new Point(0, 13);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new Padding(0);
            this.sampleToolStrip.RenderMode = ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new Size(24, 506);
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
            this.btnSampleAdd.Size = new Size(23, 24);
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
            this.btnSampleDelete.Size = new Size(23, 24);
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
            this.FSBtoSamp.Size = new Size(23, 24);
            this.FSBtoSamp.ToolTipText = "Import FSB files to Sample format";
            this.FSBtoSamp.Click += this.FSBtoSamp_Click;
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
            this.propertyGridSample.Location = new Point(0, 226);
            this.propertyGridSample.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridSample.Name = "propertyGridSample";
            this.propertyGridSample.PropertySort = PropertySort.Categorized;
            this.propertyGridSample.RightToLeft = RightToLeft.No;
            this.propertyGridSample.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridSample.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridSample.Size = new Size(609, 293);
            this.propertyGridSample.TabIndex = 0;
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
            this.lblMasterlvllistHelp.Location = new Point(973, -3);
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
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblLoading);
            this.splitContainer1.Panel1.Controls.Add(this.labelCollapsePanel);
            this.splitContainer1.Panel1.Controls.Add(this.sampleList);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.sampleToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.label54);
            this.splitContainer1.Panel1.Controls.Add(this.lblMasterlvllistHelp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridSample);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.pictureSpectrum);
            this.splitContainer1.Panel2.Controls.Add(this.pictureWave);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new Size(933, 519);
            this.splitContainer1.SplitterDistance = 319;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 136;
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.BackColor = Color.FromArgb(10, 10, 10);
            this.lblLoading.BorderStyle = BorderStyle.Fixed3D;
            this.lblLoading.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblLoading.ForeColor = Color.White;
            this.lblLoading.Image = Properties.Resources.beebledance;
            this.lblLoading.ImageAlign = ContentAlignment.MiddleLeft;
            this.lblLoading.Location = new Point(59, 166);
            this.lblLoading.Margin = new Padding(4, 0, 4, 0);
            this.lblLoading.MinimumSize = new Size(260, 60);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new Size(260, 60);
            this.lblLoading.TabIndex = 152;
            this.lblLoading.Text = "LOADING AUDIO";
            this.lblLoading.TextAlign = ContentAlignment.MiddleRight;
            this.lblLoading.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.FromArgb(10, 10, 10);
            this.label1.Dock = DockStyle.Top;
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label1.ForeColor = SystemColors.Highlight;
            this.label1.Location = new Point(24, 13);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(104, 13);
            this.label1.TabIndex = 153;
            this.label1.Text = "How to get a .fsb file";
            this.label1.Click += this.lblSampleFSBhelp_Click;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.FromArgb(10, 10, 10);
            this.label4.Dock = DockStyle.Top;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.ForeColor = Color.White;
            this.label4.Location = new Point(0, 213);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(64, 13);
            this.label4.TabIndex = 149;
            this.label4.Text = "Properties";
            // 
            // pictureSpectrum
            // 
            this.pictureSpectrum.BackColor = Color.Black;
            this.pictureSpectrum.BackgroundImageLayout = ImageLayout.None;
            this.pictureSpectrum.BorderStyle = BorderStyle.FixedSingle;
            this.pictureSpectrum.Dock = DockStyle.Top;
            this.pictureSpectrum.Location = new Point(0, 113);
            this.pictureSpectrum.Margin = new Padding(4, 3, 4, 3);
            this.pictureSpectrum.Name = "pictureSpectrum";
            this.pictureSpectrum.Size = new Size(609, 100);
            this.pictureSpectrum.TabIndex = 150;
            this.pictureSpectrum.TabStop = false;
            // 
            // pictureWave
            // 
            this.pictureWave.BackColor = Color.Black;
            this.pictureWave.BackgroundImageLayout = ImageLayout.None;
            this.pictureWave.BorderStyle = BorderStyle.FixedSingle;
            this.pictureWave.Dock = DockStyle.Top;
            this.pictureWave.Location = new Point(0, 13);
            this.pictureWave.Margin = new Padding(4, 3, 4, 3);
            this.pictureWave.Name = "pictureWave";
            this.pictureWave.Size = new Size(609, 100);
            this.pictureWave.TabIndex = 151;
            this.pictureWave.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.FromArgb(10, 10, 10);
            this.label2.Dock = DockStyle.Top;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(0, 0);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(64, 13);
            this.label2.TabIndex = 147;
            this.label2.Text = "Waveform";
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
            this.KeyPreview = true;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_SampleEditor";
            this.Text = "Sample Editor";
            this.Shown += this.Form_SampleEditor_Shown;
            ((System.ComponentModel.ISupportInitialize)this.sampleList).EndInit();
            this.sampleToolStrip.ResumeLayout(false);
            this.sampleToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.pictureSpectrum).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.ToolStrip sampleToolStrip;
        private System.Windows.Forms.ToolStripButton btnSampleAdd;
        private System.Windows.Forms.ToolStripButton btnSampleDelete;
        private System.Windows.Forms.ToolStripButton FSBtoSamp;
        public PropertyGrid propertyGridSample;
        private Label lblMasterlvllistHelp;
        private SplitContainer splitContainer1;
        private Label label1;
        private Label labelCollapsePanel;
        private Label label4;
        private Label label2;
        private PictureBox pictureSpectrum;
        private PictureBox pictureWave;
        private DataGridViewButtonColumn Columnplaybuttons;
        private DataGridViewTextBoxColumn SampleName;
        private DataGridViewTextBoxColumn sampleRuntime;
        public DataGridView sampleList;
        private Label lblLoading;
    }
}