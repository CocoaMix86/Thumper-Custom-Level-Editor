namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class EditorSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorSample));
            this.toolTip1 = new ToolTip(this.components);
            this.sampleList = new DataGridView();
            this.Columnplaybuttons = new DataGridViewButtonColumn();
            this.SampleName = new DataGridViewTextBoxColumn();
            this.sampleRuntime = new DataGridViewTextBoxColumn();
            this.sampleToolStrip = new ToolStrip();
            this.btnSampleAdd = new ToolStripButton();
            this.btnSampleDelete = new ToolStripButton();
            this.FSBtoSamp = new ToolStripButton();
            this.btnSampleChunk = new ToolStripButton();
            this.propertyGridSample = new PropertyGrid();
            this.lblLoading = new Label();
            this.label1 = new Label();
            this.pictureSpectrum = new PictureBox();
            this.pictureWave = new PictureBox();
            this.panelMain = new Panel();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            ((System.ComponentModel.ISupportInitialize)this.sampleList).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureSpectrum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).BeginInit();
            this.panelMain.SuspendLayout();
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
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.sampleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampleList.ColumnHeadersHeight = 20;
            this.sampleList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sampleList.Columns.AddRange(new DataGridViewColumn[] { this.Columnplaybuttons, this.SampleName, this.sampleRuntime });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.DarkBlue;
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
            this.sampleList.Location = new Point(24, 13);
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
            dataGridViewCellStyle5.BackColor = Color.DarkBlue;
            dataGridViewCellStyle5.Font = new Font("Relay-Medium", 8.249999F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            this.sampleList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.sampleList.RowTemplate.DefaultCellStyle.BackColor = Color.DarkBlue;
            this.sampleList.RowTemplate.DefaultCellStyle.Font = new Font("Relay-Medium", 8.249999F);
            this.sampleList.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.sampleList.RowTemplate.Height = 20;
            this.sampleList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.sampleList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.sampleList.Size = new Size(300, 396);
            this.sampleList.TabIndex = 145;
            this.sampleList.Tag = "editorpaneldgv";
            this.sampleList.CellClick += this.sampleList_CellClick;
            this.sampleList.CellPainting += this.sampleList_CellPainting;
            this.sampleList.RowPrePaint += this.sampleList_RowPrePaint;
            this.sampleList.SelectionChanged += this.sampleList_SelectionChanged;
            this.sampleList.DragDrop += this.sampleList_DragDrop;
            this.sampleList.DragEnter += this.sampleList_DragEnter;
            this.sampleList.DragOver += this.sampleList_DragOver;
            this.sampleList.MouseDown += this.sampleList_MouseDown;
            this.sampleList.MouseMove += this.sampleList_MouseMove;
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
            // sampleToolStrip
            // 
            this.sampleToolStrip.AutoSize = false;
            this.sampleToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.sampleToolStrip.Dock = DockStyle.Left;
            this.sampleToolStrip.GripMargin = new Padding(0);
            this.sampleToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new ToolStripItem[] { this.btnSampleAdd, this.btnSampleDelete, this.FSBtoSamp, this.btnSampleChunk });
            this.sampleToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.sampleToolStrip.Location = new Point(0, 0);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new Padding(0);
            this.sampleToolStrip.RenderMode = ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new Size(24, 409);
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
            // btnSampleChunk
            // 
            this.btnSampleChunk.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSampleChunk.Enabled = false;
            this.btnSampleChunk.Image = Properties.Resources.icon_split;
            this.btnSampleChunk.ImageTransparentColor = Color.Magenta;
            this.btnSampleChunk.Name = "btnSampleChunk";
            this.btnSampleChunk.Size = new Size(23, 24);
            this.btnSampleChunk.ToolTipText = "Chunk/split the selected sample into\r\nspecific beat/time lengths.";
            this.btnSampleChunk.Click += this.btnSampleChunk_Click;
            // 
            // propertyGridSample
            // 
            this.propertyGridSample.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.CategoryForeColor = Color.White;
            this.propertyGridSample.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridSample.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridSample.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridSample.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridSample.HelpForeColor = Color.White;
            this.propertyGridSample.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridSample.Location = new Point(365, 218);
            this.propertyGridSample.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridSample.Name = "propertyGridSample";
            this.propertyGridSample.PropertySort = PropertySort.Categorized;
            this.propertyGridSample.RightToLeft = RightToLeft.No;
            this.propertyGridSample.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridSample.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridSample.Size = new Size(499, 293);
            this.propertyGridSample.TabIndex = 0;
            this.propertyGridSample.ToolbarVisible = false;
            this.propertyGridSample.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridSample.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridSample.ViewForeColor = Color.White;
            this.propertyGridSample.PropertyValueChanged += this.propertyGridSample_PropertyValueChanged;
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
            this.lblLoading.Location = new Point(23, 92);
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
            this.label1.Location = new Point(24, 0);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(104, 13);
            this.label1.TabIndex = 153;
            this.label1.Text = "How to get a .fsb file";
            this.label1.Click += this.lblSampleFSBhelp_Click;
            // 
            // pictureSpectrum
            // 
            this.pictureSpectrum.BackColor = Color.Black;
            this.pictureSpectrum.BackgroundImageLayout = ImageLayout.None;
            this.pictureSpectrum.BorderStyle = BorderStyle.FixedSingle;
            this.pictureSpectrum.Location = new Point(365, 112);
            this.pictureSpectrum.Margin = new Padding(4, 3, 4, 3);
            this.pictureSpectrum.Name = "pictureSpectrum";
            this.pictureSpectrum.Size = new Size(456, 100);
            this.pictureSpectrum.TabIndex = 150;
            this.pictureSpectrum.TabStop = false;
            // 
            // pictureWave
            // 
            this.pictureWave.BackColor = Color.Black;
            this.pictureWave.BackgroundImageLayout = ImageLayout.None;
            this.pictureWave.BorderStyle = BorderStyle.FixedSingle;
            this.pictureWave.Location = new Point(365, 12);
            this.pictureWave.Margin = new Padding(4, 3, 4, 3);
            this.pictureWave.Name = "pictureWave";
            this.pictureWave.Size = new Size(456, 100);
            this.pictureWave.TabIndex = 151;
            this.pictureWave.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = Color.Black;
            this.panelMain.Controls.Add(this.lblLoading);
            this.panelMain.Controls.Add(this.sampleList);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.sampleToolStrip);
            this.panelMain.Location = new Point(12, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new Size(324, 409);
            this.panelMain.TabIndex = 137;
            // 
            // dockPanel1
            // 
            this.dockPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dockPanel1.BackColor = Color.Black;
            this.dockPanel1.Location = new Point(-4, -4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new Size(941, 527);
            this.dockPanel1.TabIndex = 152;
            this.dockPanel1.ActiveContentChanged += this.dockPanel1_ActiveContentChanged;
            // 
            // Form_SampleEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(933, 519);
            this.Controls.Add(this.propertyGridSample);
            this.Controls.Add(this.pictureSpectrum);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.pictureWave);
            this.Controls.Add(this.dockPanel1);
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
            ((System.ComponentModel.ISupportInitialize)this.pictureSpectrum).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip sampleToolStrip;
        private System.Windows.Forms.ToolStripButton btnSampleAdd;
        private System.Windows.Forms.ToolStripButton btnSampleDelete;
        private System.Windows.Forms.ToolStripButton FSBtoSamp;
        public PropertyGrid propertyGridSample;
        private Label label1;
        private PictureBox pictureSpectrum;
        private PictureBox pictureWave;
        private DataGridViewButtonColumn Columnplaybuttons;
        private DataGridViewTextBoxColumn SampleName;
        private DataGridViewTextBoxColumn sampleRuntime;
        public DataGridView sampleList;
        private Label lblLoading;
        private Panel panelMain;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        public ToolStripButton btnSampleChunk;
    }
}