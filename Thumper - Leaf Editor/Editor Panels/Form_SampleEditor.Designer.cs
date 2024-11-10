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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SampleEditor));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.panelSample = new System.Windows.Forms.Panel();
            this.sampleList = new System.Windows.Forms.DataGridView();
            this.SampleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Channel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label54 = new System.Windows.Forms.Label();
            this.sampleToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSampleAdd = new System.Windows.Forms.ToolStripButton();
            this.btnSampleDelete = new System.Windows.Forms.ToolStripButton();
            this.FSBtoSamp = new System.Windows.Forms.ToolStripButton();
            this.btnSampEditorPlaySamp = new System.Windows.Forms.ToolStripButton();
            this.lblSampleFSBhelp = new System.Windows.Forms.ToolStripLabel();
            this.label50 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.txtSampPath = new System.Windows.Forms.TextBox();
            this.toolstripTitleSample = new System.Windows.Forms.ToolStrip();
            this.lblSampleEditor = new System.Windows.Forms.ToolStripLabel();
            this.btnSaveSample = new System.Windows.Forms.ToolStripButton();
            this.btnRevertSample = new System.Windows.Forms.ToolStripButton();
            this.panelSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleList)).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            this.toolstripTitleSample.SuspendLayout();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Cursor = System.Windows.Forms.Cursors.Help;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label14.Location = new System.Drawing.Point(780, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 16);
            this.label14.TabIndex = 151;
            this.label14.Text = "?";
            this.toolTip1.SetToolTip(this.label14, "V. (volume)\r\nP. (pitch)\r\nPan\r\nOf. (offset)\r\nCh. (channel)");
            // 
            // panelSample
            // 
            this.panelSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panelSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSample.Controls.Add(this.sampleList);
            this.panelSample.Controls.Add(this.label14);
            this.panelSample.Controls.Add(this.label54);
            this.panelSample.Controls.Add(this.sampleToolStrip);
            this.panelSample.Controls.Add(this.label50);
            this.panelSample.Controls.Add(this.label45);
            this.panelSample.Controls.Add(this.txtSampPath);
            this.panelSample.Controls.Add(this.toolstripTitleSample);
            this.panelSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSample.Location = new System.Drawing.Point(0, 0);
            this.panelSample.MinimumSize = new System.Drawing.Size(60, 60);
            this.panelSample.Name = "panelSample";
            this.panelSample.Size = new System.Drawing.Size(800, 450);
            this.panelSample.TabIndex = 135;
            this.panelSample.Tag = "editorpanel";
            // 
            // sampleList
            // 
            this.sampleList.AllowDrop = true;
            this.sampleList.AllowUserToAddRows = false;
            this.sampleList.AllowUserToDeleteRows = false;
            this.sampleList.AllowUserToResizeRows = false;
            this.sampleList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sampleList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.sampleList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sampleList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.sampleList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sampleList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampleList.ColumnHeadersHeight = 20;
            this.sampleList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.sampleList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SampleName,
            this.Volume,
            this.Pitch,
            this.Pan,
            this.Offset,
            this.Channel});
            this.sampleList.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sampleList.DefaultCellStyle = dataGridViewCellStyle7;
            this.sampleList.EnableHeadersVisualStyles = false;
            this.sampleList.GridColor = System.Drawing.Color.Black;
            this.sampleList.Location = new System.Drawing.Point(3, 40);
            this.sampleList.Name = "sampleList";
            this.sampleList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sampleList.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.sampleList.RowHeadersVisible = false;
            this.sampleList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sampleList.RowTemplate.Height = 20;
            this.sampleList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sampleList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.sampleList.Size = new System.Drawing.Size(791, 347);
            this.sampleList.TabIndex = 145;
            this.sampleList.Tag = "editorpaneldgv";
            this.sampleList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.sampleList_CellEnter);
            this.sampleList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.sampleList_CellValueChanged);
            this.sampleList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.sampleList_EditingControlShowing);
            this.sampleList.DragDrop += new System.Windows.Forms.DragEventHandler(this.sampleList_DragDrop);
            this.sampleList.DragEnter += new System.Windows.Forms.DragEventHandler(this.sampleList_DragEnter);
            // 
            // SampleName
            // 
            this.SampleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.SampleName.DefaultCellStyle = dataGridViewCellStyle2;
            this.SampleName.FillWeight = 8.913044F;
            this.SampleName.HeaderText = "Sample Name";
            this.SampleName.Name = "SampleName";
            this.SampleName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Volume
            // 
            this.Volume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Format = "0.00";
            dataGridViewCellStyle3.NullValue = null;
            this.Volume.DefaultCellStyle = dataGridViewCellStyle3;
            this.Volume.FillWeight = 1F;
            this.Volume.HeaderText = "V.";
            this.Volume.Name = "Volume";
            this.Volume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Volume.Width = 22;
            // 
            // Pitch
            // 
            this.Pitch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Format = "0.00";
            dataGridViewCellStyle4.NullValue = null;
            this.Pitch.DefaultCellStyle = dataGridViewCellStyle4;
            this.Pitch.FillWeight = 1F;
            this.Pitch.HeaderText = "P.";
            this.Pitch.Name = "Pitch";
            this.Pitch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Pitch.Width = 21;
            // 
            // Pan
            // 
            this.Pan.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Format = "0.00";
            dataGridViewCellStyle5.NullValue = null;
            this.Pan.DefaultCellStyle = dataGridViewCellStyle5;
            this.Pan.FillWeight = 1F;
            this.Pan.HeaderText = "Pan";
            this.Pan.Name = "Pan";
            this.Pan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Pan.Width = 32;
            // 
            // Offset
            // 
            this.Offset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Format = "0.00";
            dataGridViewCellStyle6.NullValue = null;
            this.Offset.DefaultCellStyle = dataGridViewCellStyle6;
            this.Offset.FillWeight = 1F;
            this.Offset.HeaderText = "Of.";
            this.Offset.Name = "Offset";
            this.Offset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Offset.Width = 27;
            // 
            // Channel
            // 
            this.Channel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Channel.FillWeight = 1.086957F;
            this.Channel.HeaderText = "Ch.";
            this.Channel.Name = "Channel";
            this.Channel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Channel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.White;
            this.label54.Location = new System.Drawing.Point(3, 27);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(54, 13);
            this.label54.TabIndex = 146;
            this.label54.Text = "Samples";
            // 
            // sampleToolStrip
            // 
            this.sampleToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sampleToolStrip.AutoSize = false;
            this.sampleToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.sampleToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.sampleToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.sampleToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSampleAdd,
            this.btnSampleDelete,
            this.FSBtoSamp,
            this.btnSampEditorPlaySamp,
            this.lblSampleFSBhelp});
            this.sampleToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.sampleToolStrip.Location = new System.Drawing.Point(3, 390);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.sampleToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new System.Drawing.Size(791, 25);
            this.sampleToolStrip.Stretch = true;
            this.sampleToolStrip.TabIndex = 150;
            // 
            // btnSampleAdd
            // 
            this.btnSampleAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSampleAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleAdd.ForeColor = System.Drawing.Color.White;
            this.btnSampleAdd.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_plus;
            this.btnSampleAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSampleAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnSampleAdd.Name = "btnSampleAdd";
            this.btnSampleAdd.Size = new System.Drawing.Size(24, 25);
            this.btnSampleAdd.ToolTipText = "Add new sample";
            this.btnSampleAdd.Click += new System.EventHandler(this.btnSampleAdd_Click);
            // 
            // btnSampleDelete
            // 
            this.btnSampleDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSampleDelete.Enabled = false;
            this.btnSampleDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.btnSampleDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSampleDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnSampleDelete.Name = "btnSampleDelete";
            this.btnSampleDelete.Size = new System.Drawing.Size(24, 25);
            this.btnSampleDelete.ToolTipText = "Delete selected phase";
            this.btnSampleDelete.Click += new System.EventHandler(this.btnSampleDelete_Click);
            // 
            // FSBtoSamp
            // 
            this.FSBtoSamp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FSBtoSamp.Enabled = false;
            this.FSBtoSamp.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_import;
            this.FSBtoSamp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FSBtoSamp.Name = "FSBtoSamp";
            this.FSBtoSamp.Size = new System.Drawing.Size(24, 22);
            this.FSBtoSamp.ToolTipText = "Import FSB files to Sample format";
            this.FSBtoSamp.Click += new System.EventHandler(this.FSBtoSamp_Click);
            // 
            // btnSampEditorPlaySamp
            // 
            this.btnSampEditorPlaySamp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSampEditorPlaySamp.Enabled = false;
            this.btnSampEditorPlaySamp.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_play2;
            this.btnSampEditorPlaySamp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSampEditorPlaySamp.Name = "btnSampEditorPlaySamp";
            this.btnSampEditorPlaySamp.Size = new System.Drawing.Size(24, 22);
            this.btnSampEditorPlaySamp.ToolTipText = "Play selected sample";
            this.btnSampEditorPlaySamp.Click += new System.EventHandler(this.btnSampEditorPlaySamp_Click);
            // 
            // lblSampleFSBhelp
            // 
            this.lblSampleFSBhelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblSampleFSBhelp.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleFSBhelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblSampleFSBhelp.Name = "lblSampleFSBhelp";
            this.lblSampleFSBhelp.Size = new System.Drawing.Size(161, 22);
            this.lblSampleFSBhelp.Text = "How to get a .FSB audio file";
            this.lblSampleFSBhelp.Click += new System.EventHandler(this.lblSampleFSBhelp_Click);
            // 
            // label50
            // 
            this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.Color.White;
            this.label50.Location = new System.Drawing.Point(5, 426);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(32, 15);
            this.label50.TabIndex = 147;
            this.label50.Text = "Path";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.Color.White;
            this.label45.Location = new System.Drawing.Point(142, 27);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(157, 13);
            this.label45.TabIndex = 149;
            this.label45.Text = "all fields in the table are editable";
            // 
            // txtSampPath
            // 
            this.txtSampPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtSampPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSampPath.ForeColor = System.Drawing.Color.White;
            this.txtSampPath.Location = new System.Drawing.Point(38, 423);
            this.txtSampPath.Name = "txtSampPath";
            this.txtSampPath.Size = new System.Drawing.Size(735, 22);
            this.txtSampPath.TabIndex = 148;
            this.txtSampPath.TextChanged += new System.EventHandler(this.txtSampPath_TextChanged);
            // 
            // toolstripTitleSample
            // 
            this.toolstripTitleSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.toolstripTitleSample.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolstripTitleSample.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripTitleSample.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSampleEditor,
            this.btnSaveSample,
            this.btnRevertSample});
            this.toolstripTitleSample.Location = new System.Drawing.Point(0, 0);
            this.toolstripTitleSample.MaximumSize = new System.Drawing.Size(0, 50);
            this.toolstripTitleSample.Name = "toolstripTitleSample";
            this.toolstripTitleSample.Padding = new System.Windows.Forms.Padding(0);
            this.toolstripTitleSample.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolstripTitleSample.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolstripTitleSample.Size = new System.Drawing.Size(798, 25);
            this.toolstripTitleSample.Stretch = true;
            this.toolstripTitleSample.TabIndex = 142;
            this.toolstripTitleSample.Text = "titlebar";
            // 
            // lblSampleEditor
            // 
            this.lblSampleEditor.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleEditor.ForeColor = System.Drawing.Color.Turquoise;
            this.lblSampleEditor.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.editor_sample;
            this.lblSampleEditor.Name = "lblSampleEditor";
            this.lblSampleEditor.Size = new System.Drawing.Size(101, 22);
            this.lblSampleEditor.Text = "Sample Editor";
            // 
            // btnSaveSample
            // 
            this.btnSaveSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveSample.Enabled = false;
            this.btnSaveSample.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_save2;
            this.btnSaveSample.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveSample.Name = "btnSaveSample";
            this.btnSaveSample.Size = new System.Drawing.Size(23, 22);
            this.btnSaveSample.ToolTipText = "Save sample file";
            this.btnSaveSample.Click += new System.EventHandler(this.SamplesaveToolStripMenuItem_Click);
            // 
            // btnRevertSample
            // 
            this.btnRevertSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRevertSample.Enabled = false;
            this.btnRevertSample.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_back;
            this.btnRevertSample.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRevertSample.Name = "btnRevertSample";
            this.btnRevertSample.Size = new System.Drawing.Size(23, 22);
            this.btnRevertSample.ToolTipText = "Revert changes to last save";
            this.btnRevertSample.Click += new System.EventHandler(this.btnRevertSample_Click);
            // 
            // Form_SampleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelSample);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_SampleEditor";
            this.Text = "Sample Editor";
            this.panelSample.ResumeLayout(false);
            this.panelSample.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleList)).EndInit();
            this.sampleToolStrip.ResumeLayout(false);
            this.sampleToolStrip.PerformLayout();
            this.toolstripTitleSample.ResumeLayout(false);
            this.toolstripTitleSample.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelSample;
        private System.Windows.Forms.DataGridView sampleList;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pan;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn Channel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.ToolStrip sampleToolStrip;
        private System.Windows.Forms.ToolStripButton btnSampleAdd;
        private System.Windows.Forms.ToolStripButton btnSampleDelete;
        private System.Windows.Forms.ToolStripButton FSBtoSamp;
        private System.Windows.Forms.ToolStripButton btnSampEditorPlaySamp;
        private System.Windows.Forms.ToolStripLabel lblSampleFSBhelp;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txtSampPath;
        private System.Windows.Forms.ToolStrip toolstripTitleSample;
        private System.Windows.Forms.ToolStripLabel lblSampleEditor;
        private System.Windows.Forms.ToolStripButton btnSaveSample;
        private System.Windows.Forms.ToolStripButton btnRevertSample;
    }
}