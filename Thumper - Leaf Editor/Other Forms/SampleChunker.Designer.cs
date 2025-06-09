namespace Thumper_Custom_Level_Editor.Other_Forms
{
    partial class SampleChunker
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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            this.pictureWave = new PictureBox();
            this.lblBpm = new Label();
            this.lblRuntime = new Label();
            this.radioTime = new RadioButton();
            this.radioBeats = new RadioButton();
            this.label2 = new Label();
            this.label3 = new Label();
            this.chkPosStart = new CheckBox();
            this.chkPosEnd = new CheckBox();
            this.txtTimeStart = new TextBox();
            this.toolTip1 = new ToolTip(this.components);
            this.chkLimit = new CheckBox();
            this.label12 = new Label();
            this.panelStart = new Panel();
            this.txtBeatStart = new NumericUpDown();
            this.panelEnd = new Panel();
            this.txtBeatEnd = new NumericUpDown();
            this.txtTimeEnd = new TextBox();
            this.lblBeats = new Label();
            this.numChunks = new NumericUpDown();
            this.button1 = new Button();
            this.sampleToolStrip = new ToolStrip();
            this.btnChunkName = new ToolStripButton();
            this.btnChunkBeats = new ToolStripButton();
            this.btnChunkTime = new ToolStripButton();
            this.txtTimeChunk = new NumericUpDown();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.numSplitSec = new NumericUpDown();
            this.btnAddSplit1 = new Button();
            this.dgvSplits = new DataGridView();
            this.LeafEnabled = new DataGridViewTextBoxColumn();
            this.LeafAudio = new DataGridViewTextBoxColumn();
            this.removesplit = new DataGridViewImageColumn();
            this.numSplitBeat = new NumericUpDown();
            this.txtBeatChunk = new NumericUpDown();
            this.btnHelp = new Button();
            this.label1 = new Label();
            this.txtChunkName = new TextBox();
            this.lblChunkTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).BeginInit();
            this.panelStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatStart).BeginInit();
            this.panelEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numChunks).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtTimeChunk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numSplitSec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvSplits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numSplitBeat).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatChunk).BeginInit();
            this.SuspendLayout();
            // 
            // pictureWave
            // 
            this.pictureWave.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pictureWave.BackColor = Color.Black;
            this.pictureWave.BackgroundImageLayout = ImageLayout.None;
            this.pictureWave.BorderStyle = BorderStyle.FixedSingle;
            this.pictureWave.Location = new Point(13, 27);
            this.pictureWave.Margin = new Padding(4, 3, 4, 3);
            this.pictureWave.Name = "pictureWave";
            this.pictureWave.Size = new Size(515, 100);
            this.pictureWave.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureWave.TabIndex = 152;
            this.pictureWave.TabStop = false;
            // 
            // lblBpm
            // 
            this.lblBpm.AutoSize = true;
            this.lblBpm.BackColor = Color.Transparent;
            this.lblBpm.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblBpm.ForeColor = Color.White;
            this.lblBpm.Location = new Point(13, 130);
            this.lblBpm.Margin = new Padding(4, 0, 4, 0);
            this.lblBpm.Name = "lblBpm";
            this.lblBpm.RightToLeft = RightToLeft.No;
            this.lblBpm.Size = new Size(161, 15);
            this.lblBpm.TabIndex = 154;
            this.lblBpm.Text = "CURRENT BPM = {} = 1 min";
            this.lblBpm.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRuntime
            // 
            this.lblRuntime.AutoSize = true;
            this.lblRuntime.BackColor = Color.Transparent;
            this.lblRuntime.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblRuntime.ForeColor = Color.White;
            this.lblRuntime.Location = new Point(357, 132);
            this.lblRuntime.Margin = new Padding(4, 0, 4, 0);
            this.lblRuntime.Name = "lblRuntime";
            this.lblRuntime.RightToLeft = RightToLeft.No;
            this.lblRuntime.Size = new Size(168, 14);
            this.lblRuntime.TabIndex = 155;
            this.lblRuntime.Text = "Runtime: 00:00:00.00000";
            this.lblRuntime.TextAlign = ContentAlignment.MiddleRight;
            // 
            // radioTime
            // 
            this.radioTime.AutoSize = true;
            this.radioTime.Checked = true;
            this.radioTime.ForeColor = Color.White;
            this.radioTime.Location = new Point(68, 186);
            this.radioTime.Name = "radioTime";
            this.radioTime.Size = new Size(51, 19);
            this.radioTime.TabIndex = 156;
            this.radioTime.TabStop = true;
            this.radioTime.Text = "Time";
            this.radioTime.UseVisualStyleBackColor = true;
            this.radioTime.CheckedChanged += this.radioTime_CheckedChanged;
            // 
            // radioBeats
            // 
            this.radioBeats.AutoSize = true;
            this.radioBeats.ForeColor = Color.White;
            this.radioBeats.Location = new Point(139, 186);
            this.radioBeats.Name = "radioBeats";
            this.radioBeats.Size = new Size(53, 19);
            this.radioBeats.TabIndex = 157;
            this.radioBeats.Text = "Beats";
            this.radioBeats.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(88, 168);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = RightToLeft.No;
            this.label2.Size = new Size(75, 15);
            this.label2.TabIndex = 158;
            this.label2.Text = "Auto Splits";
            this.label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.White;
            this.label3.Location = new Point(103, 208);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = RightToLeft.No;
            this.label3.Size = new Size(45, 15);
            this.label3.TabIndex = 160;
            this.label3.Text = "Every:";
            this.label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // chkPosStart
            // 
            this.chkPosStart.AutoSize = true;
            this.chkPosStart.ForeColor = Color.White;
            this.chkPosStart.Location = new Point(77, 257);
            this.chkPosStart.Name = "chkPosStart";
            this.chkPosStart.Size = new Size(96, 19);
            this.chkPosStart.TabIndex = 162;
            this.chkPosStart.Text = "Start Position";
            this.toolTip1.SetToolTip(this.chkPosStart, "Chunk 1 will begin at this position in the sample.\r\nLeave unchecked to start from the beginning.");
            this.chkPosStart.UseVisualStyleBackColor = true;
            this.chkPosStart.CheckedChanged += this.chkPosStart_CheckedChanged;
            // 
            // chkPosEnd
            // 
            this.chkPosEnd.AutoSize = true;
            this.chkPosEnd.ForeColor = Color.White;
            this.chkPosEnd.Location = new Point(77, 304);
            this.chkPosEnd.Name = "chkPosEnd";
            this.chkPosEnd.Size = new Size(92, 19);
            this.chkPosEnd.TabIndex = 163;
            this.chkPosEnd.Text = "End Position";
            this.toolTip1.SetToolTip(this.chkPosEnd, "Chunks will be created up until this position in the sample.\r\nLeave unchecked to go to end.");
            this.chkPosEnd.UseVisualStyleBackColor = true;
            this.chkPosEnd.CheckedChanged += this.chkPosEnd_CheckedChanged;
            // 
            // txtTimeStart
            // 
            this.txtTimeStart.Font = new Font("Consolas", 9F);
            this.txtTimeStart.Location = new Point(11, 2);
            this.txtTimeStart.Name = "txtTimeStart";
            this.txtTimeStart.PlaceholderText = "hh:mm:ss.ddddd";
            this.txtTimeStart.Size = new Size(109, 22);
            this.txtTimeStart.TabIndex = 165;
            this.txtTimeStart.TextAlign = HorizontalAlignment.Right;
            this.txtTimeStart.TextChanged += this.txtTimeStart_TextChanged;
            // 
            // chkLimit
            // 
            this.chkLimit.AutoSize = true;
            this.chkLimit.ForeColor = Color.White;
            this.chkLimit.Location = new Point(44, 351);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new Size(157, 19);
            this.chkLimit.TabIndex = 168;
            this.chkLimit.Text = "Limit Number of Chunks";
            this.toolTip1.SetToolTip(this.chkLimit, "From start position, create only this many chunks.\r\nLeave unchecked to chunk until end.");
            this.chkLimit.UseVisualStyleBackColor = true;
            this.chkLimit.CheckedChanged += this.chkLimit_CheckedChanged;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = Color.Transparent;
            this.label12.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label12.ForeColor = Color.Aqua;
            this.label12.Location = new Point(160, 440);
            this.label12.Margin = new Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.RightToLeft = RightToLeft.No;
            this.label12.Size = new Size(14, 15);
            this.label12.TabIndex = 198;
            this.label12.Text = "?";
            this.label12.TextAlign = ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.label12, "Insert {X} to have it replaced with the chunk number.\r\nThe name MUST include at least 1 {X}. otherwise all the\r\nchunk names would be the same.\r\n");
            // 
            // panelStart
            // 
            this.panelStart.Controls.Add(this.txtBeatStart);
            this.panelStart.Controls.Add(this.txtTimeStart);
            this.panelStart.Enabled = false;
            this.panelStart.Location = new Point(-1, 272);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new Size(247, 26);
            this.panelStart.TabIndex = 170;
            // 
            // txtBeatStart
            // 
            this.txtBeatStart.DecimalPlaces = 4;
            this.txtBeatStart.Location = new Point(140, 3);
            this.txtBeatStart.Name = "txtBeatStart";
            this.txtBeatStart.Size = new Size(77, 23);
            this.txtBeatStart.TabIndex = 174;
            this.txtBeatStart.ValueChanged += this.txtBeatStart_ValueChanged;
            // 
            // panelEnd
            // 
            this.panelEnd.Controls.Add(this.txtBeatEnd);
            this.panelEnd.Controls.Add(this.txtTimeEnd);
            this.panelEnd.Enabled = false;
            this.panelEnd.Location = new Point(-1, 320);
            this.panelEnd.Name = "panelEnd";
            this.panelEnd.Size = new Size(247, 26);
            this.panelEnd.TabIndex = 171;
            // 
            // txtBeatEnd
            // 
            this.txtBeatEnd.DecimalPlaces = 4;
            this.txtBeatEnd.Location = new Point(140, 2);
            this.txtBeatEnd.Name = "txtBeatEnd";
            this.txtBeatEnd.Size = new Size(77, 23);
            this.txtBeatEnd.TabIndex = 174;
            this.txtBeatEnd.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.txtBeatEnd.ValueChanged += this.txtBeatEnd_ValueChanged;
            // 
            // txtTimeEnd
            // 
            this.txtTimeEnd.Font = new Font("Consolas", 9F);
            this.txtTimeEnd.Location = new Point(11, 2);
            this.txtTimeEnd.Name = "txtTimeEnd";
            this.txtTimeEnd.PlaceholderText = "hh:mm:ss.ddddd";
            this.txtTimeEnd.Size = new Size(109, 22);
            this.txtTimeEnd.TabIndex = 165;
            this.txtTimeEnd.TextAlign = HorizontalAlignment.Right;
            this.txtTimeEnd.TextChanged += this.txtTimeEnd_TextChanged;
            // 
            // lblBeats
            // 
            this.lblBeats.AutoSize = true;
            this.lblBeats.BackColor = Color.Transparent;
            this.lblBeats.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblBeats.ForeColor = Color.White;
            this.lblBeats.Location = new Point(371, 146);
            this.lblBeats.Margin = new Padding(4, 0, 4, 0);
            this.lblBeats.Name = "lblBeats";
            this.lblBeats.RightToLeft = RightToLeft.No;
            this.lblBeats.Size = new Size(63, 14);
            this.lblBeats.TabIndex = 172;
            this.lblBeats.Text = "Beats: 0";
            this.lblBeats.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numChunks
            // 
            this.numChunks.Location = new Point(95, 371);
            this.numChunks.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numChunks.Name = "numChunks";
            this.numChunks.Size = new Size(53, 23);
            this.numChunks.TabIndex = 173;
            this.numChunks.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numChunks.ValueChanged += this.txtBeatChunk_TextChanged;
            // 
            // button1
            // 
            this.button1.BackColor = Color.Green;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.ForeColor = Color.White;
            this.button1.Location = new Point(232, 446);
            this.button1.Name = "button1";
            this.button1.Size = new Size(98, 38);
            this.button1.TabIndex = 174;
            this.button1.Text = "Chunk It";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += this.button1_Click;
            // 
            // sampleToolStrip
            // 
            this.sampleToolStrip.AutoSize = false;
            this.sampleToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.sampleToolStrip.GripMargin = new Padding(0);
            this.sampleToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new ToolStripItem[] { this.btnChunkName, this.btnChunkBeats, this.btnChunkTime });
            this.sampleToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.sampleToolStrip.Location = new Point(0, 0);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new Padding(0);
            this.sampleToolStrip.RenderMode = ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new Size(546, 24);
            this.sampleToolStrip.Stretch = true;
            this.sampleToolStrip.TabIndex = 176;
            // 
            // btnChunkName
            // 
            this.btnChunkName.Checked = true;
            this.btnChunkName.CheckOnClick = true;
            this.btnChunkName.CheckState = CheckState.Checked;
            this.btnChunkName.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnChunkName.ForeColor = Color.White;
            this.btnChunkName.ImageTransparentColor = Color.Magenta;
            this.btnChunkName.Name = "btnChunkName";
            this.btnChunkName.Size = new Size(43, 21);
            this.btnChunkName.Text = "Name";
            this.btnChunkName.Click += this.btnChunkName_Click;
            // 
            // btnChunkBeats
            // 
            this.btnChunkBeats.Checked = true;
            this.btnChunkBeats.CheckOnClick = true;
            this.btnChunkBeats.CheckState = CheckState.Checked;
            this.btnChunkBeats.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnChunkBeats.ForeColor = Color.White;
            this.btnChunkBeats.ImageTransparentColor = Color.Magenta;
            this.btnChunkBeats.Name = "btnChunkBeats";
            this.btnChunkBeats.Size = new Size(39, 21);
            this.btnChunkBeats.Text = "Beats";
            this.btnChunkBeats.Click += this.btnChunkName_Click;
            // 
            // btnChunkTime
            // 
            this.btnChunkTime.Checked = true;
            this.btnChunkTime.CheckOnClick = true;
            this.btnChunkTime.CheckState = CheckState.Checked;
            this.btnChunkTime.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnChunkTime.ForeColor = Color.White;
            this.btnChunkTime.ImageTransparentColor = Color.Magenta;
            this.btnChunkTime.Name = "btnChunkTime";
            this.btnChunkTime.Size = new Size(37, 21);
            this.btnChunkTime.Text = "Time";
            this.btnChunkTime.Click += this.btnChunkName_Click;
            // 
            // txtTimeChunk
            // 
            this.txtTimeChunk.DecimalPlaces = 5;
            this.txtTimeChunk.Location = new Point(10, 228);
            this.txtTimeChunk.Name = "txtTimeChunk";
            this.txtTimeChunk.Size = new Size(86, 23);
            this.txtTimeChunk.TabIndex = 177;
            this.txtTimeChunk.TextAlign = HorizontalAlignment.Right;
            this.txtTimeChunk.ValueChanged += this.txtBeatChunk_TextChanged;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.Transparent;
            this.label4.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.ForeColor = Color.White;
            this.label4.Location = new Point(95, 230);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = RightToLeft.No;
            this.label4.Size = new Size(26, 15);
            this.label4.TabIndex = 178;
            this.label4.Text = "sec";
            this.label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = Color.Transparent;
            this.label5.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label5.ForeColor = Color.White;
            this.label5.Location = new Point(218, 232);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = RightToLeft.No;
            this.label5.Size = new Size(37, 15);
            this.label5.TabIndex = 179;
            this.label5.Text = "beats";
            this.label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = Color.Transparent;
            this.label6.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.ForeColor = Color.White;
            this.label6.Location = new Point(349, 168);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.RightToLeft = RightToLeft.No;
            this.label6.Size = new Size(116, 15);
            this.label6.TabIndex = 180;
            this.label6.Text = "Add Manual Split";
            this.label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = Color.Transparent;
            this.label7.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label7.ForeColor = Color.White;
            this.label7.Location = new Point(489, 193);
            this.label7.Margin = new Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.RightToLeft = RightToLeft.No;
            this.label7.Size = new Size(31, 15);
            this.label7.TabIndex = 184;
            this.label7.Text = "beat";
            this.label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = Color.Transparent;
            this.label8.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label8.ForeColor = Color.White;
            this.label8.Location = new Point(384, 191);
            this.label8.Margin = new Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.RightToLeft = RightToLeft.No;
            this.label8.Size = new Size(26, 15);
            this.label8.TabIndex = 183;
            this.label8.Text = "sec";
            this.label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numSplitSec
            // 
            this.numSplitSec.DecimalPlaces = 5;
            this.numSplitSec.Location = new Point(298, 189);
            this.numSplitSec.Name = "numSplitSec";
            this.numSplitSec.Size = new Size(86, 23);
            this.numSplitSec.TabIndex = 182;
            this.numSplitSec.TextAlign = HorizontalAlignment.Right;
            this.numSplitSec.ValueChanged += this.numSplitSec_ValueChanged;
            // 
            // btnAddSplit1
            // 
            this.btnAddSplit1.BackColor = Color.Green;
            this.btnAddSplit1.FlatStyle = FlatStyle.Flat;
            this.btnAddSplit1.ForeColor = Color.White;
            this.btnAddSplit1.Location = new Point(371, 212);
            this.btnAddSplit1.Name = "btnAddSplit1";
            this.btnAddSplit1.Size = new Size(63, 24);
            this.btnAddSplit1.TabIndex = 185;
            this.btnAddSplit1.Text = "Add";
            this.btnAddSplit1.UseVisualStyleBackColor = false;
            this.btnAddSplit1.Click += this.btnAddSplit1_Click;
            // 
            // dgvSplits
            // 
            this.dgvSplits.AllowDrop = true;
            this.dgvSplits.AllowUserToAddRows = false;
            this.dgvSplits.AllowUserToDeleteRows = false;
            this.dgvSplits.AllowUserToResizeColumns = false;
            this.dgvSplits.AllowUserToResizeRows = false;
            this.dgvSplits.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.dgvSplits.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvSplits.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle7.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            this.dgvSplits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSplits.ColumnHeadersHeight = 20;
            this.dgvSplits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSplits.Columns.AddRange(new DataGridViewColumn[] { this.LeafEnabled, this.LeafAudio, this.removesplit });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle10.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = Color.White;
            dataGridViewCellStyle10.Format = "0.###";
            dataGridViewCellStyle10.NullValue = null;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            this.dgvSplits.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvSplits.EnableHeadersVisualStyles = false;
            this.dgvSplits.GridColor = Color.Black;
            this.dgvSplits.Location = new Point(298, 242);
            this.dgvSplits.Margin = new Padding(4, 3, 4, 3);
            this.dgvSplits.Name = "dgvSplits";
            this.dgvSplits.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle11.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle11.ForeColor = Color.White;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            this.dgvSplits.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvSplits.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvSplits.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvSplits.RowTemplate.Height = 20;
            this.dgvSplits.ScrollBars = ScrollBars.Horizontal;
            this.dgvSplits.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.dgvSplits.ShowCellErrors = false;
            this.dgvSplits.ShowRowErrors = false;
            this.dgvSplits.Size = new Size(227, 163);
            this.dgvSplits.TabIndex = 187;
            this.dgvSplits.Tag = "editorpaneldgv";
            this.dgvSplits.CellClick += this.dgvSplits_CellClick;
            // 
            // LeafEnabled
            // 
            this.LeafEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Format = "N5";
            dataGridViewCellStyle8.NullValue = "0";
            this.LeafEnabled.DefaultCellStyle = dataGridViewCellStyle8;
            this.LeafEnabled.HeaderText = "Sec.";
            this.LeafEnabled.MinimumWidth = 25;
            this.LeafEnabled.Name = "LeafEnabled";
            this.LeafEnabled.ReadOnly = true;
            this.LeafEnabled.Resizable = DataGridViewTriState.False;
            this.LeafEnabled.SortMode = DataGridViewColumnSortMode.Programmatic;
            this.LeafEnabled.ToolTipText = "Enable/Disable All";
            // 
            // LeafAudio
            // 
            this.LeafAudio.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = "0";
            this.LeafAudio.DefaultCellStyle = dataGridViewCellStyle9;
            this.LeafAudio.HeaderText = "Beat";
            this.LeafAudio.MinimumWidth = 25;
            this.LeafAudio.Name = "LeafAudio";
            this.LeafAudio.ReadOnly = true;
            this.LeafAudio.Resizable = DataGridViewTriState.False;
            this.LeafAudio.SortMode = DataGridViewColumnSortMode.Programmatic;
            this.LeafAudio.ToolTipText = "Mute/Unmute All";
            this.LeafAudio.Width = 55;
            // 
            // removesplit
            // 
            this.removesplit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.removesplit.HeaderText = "";
            this.removesplit.Image = Properties.Resources.icon_trash;
            this.removesplit.Name = "removesplit";
            this.removesplit.SortMode = DataGridViewColumnSortMode.Programmatic;
            this.removesplit.Width = 5;
            // 
            // numSplitBeat
            // 
            this.numSplitBeat.DecimalPlaces = 4;
            this.numSplitBeat.Location = new Point(416, 189);
            this.numSplitBeat.Name = "numSplitBeat";
            this.numSplitBeat.Size = new Size(73, 23);
            this.numSplitBeat.TabIndex = 188;
            this.numSplitBeat.TextAlign = HorizontalAlignment.Right;
            this.numSplitBeat.ValueChanged += this.numSplitBeat_ValueChanged;
            // 
            // txtBeatChunk
            // 
            this.txtBeatChunk.DecimalPlaces = 4;
            this.txtBeatChunk.Location = new Point(139, 228);
            this.txtBeatChunk.Name = "txtBeatChunk";
            this.txtBeatChunk.Size = new Size(77, 23);
            this.txtBeatChunk.TabIndex = 189;
            this.txtBeatChunk.TextAlign = HorizontalAlignment.Right;
            this.txtBeatChunk.ValueChanged += this.txtBeatChunk_TextChanged;
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = Color.Orange;
            this.btnHelp.FlatStyle = FlatStyle.Flat;
            this.btnHelp.ForeColor = Color.Black;
            this.btnHelp.Location = new Point(431, 458);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(111, 26);
            this.btnHelp.TabIndex = 190;
            this.btnHelp.Text = "More Info....";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += this.btnHelp_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(62, 440);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = RightToLeft.No;
            this.label1.Size = new Size(101, 15);
            this.label1.TabIndex = 191;
            this.label1.Text = "Chunk Naming";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtChunkName
            // 
            this.txtChunkName.Font = new Font("Consolas", 9F);
            this.txtChunkName.Location = new Point(12, 458);
            this.txtChunkName.Name = "txtChunkName";
            this.txtChunkName.Size = new Size(204, 22);
            this.txtChunkName.TabIndex = 194;
            this.txtChunkName.Text = "_chunk{X}";
            // 
            // lblChunkTotal
            // 
            this.lblChunkTotal.AutoSize = true;
            this.lblChunkTotal.BackColor = Color.Transparent;
            this.lblChunkTotal.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblChunkTotal.ForeColor = Color.White;
            this.lblChunkTotal.Location = new Point(230, 427);
            this.lblChunkTotal.Margin = new Padding(4, 0, 4, 0);
            this.lblChunkTotal.Name = "lblChunkTotal";
            this.lblChunkTotal.RightToLeft = RightToLeft.No;
            this.lblChunkTotal.Size = new Size(94, 15);
            this.lblChunkTotal.TabIndex = 199;
            this.lblChunkTotal.Text = "Total Chunks:";
            this.lblChunkTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SampleChunker
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ClientSize = new Size(546, 487);
            this.Controls.Add(this.lblChunkTotal);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtChunkName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.txtBeatChunk);
            this.Controls.Add(this.numSplitBeat);
            this.Controls.Add(this.dgvSplits);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numSplitSec);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTimeChunk);
            this.Controls.Add(this.sampleToolStrip);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numChunks);
            this.Controls.Add(this.lblBeats);
            this.Controls.Add(this.panelEnd);
            this.Controls.Add(this.panelStart);
            this.Controls.Add(this.chkLimit);
            this.Controls.Add(this.chkPosEnd);
            this.Controls.Add(this.chkPosStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioBeats);
            this.Controls.Add(this.radioTime);
            this.Controls.Add(this.lblRuntime);
            this.Controls.Add(this.lblBpm);
            this.Controls.Add(this.pictureWave);
            this.Controls.Add(this.btnAddSplit1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SampleChunker";
            this.ShowInTaskbar = false;
            this.Text = "Sample Chunker";
            this.ResizeEnd += this.SampleChunker_ResizeEnd;
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).EndInit();
            this.panelStart.ResumeLayout(false);
            this.panelStart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatStart).EndInit();
            this.panelEnd.ResumeLayout(false);
            this.panelEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.numChunks).EndInit();
            this.sampleToolStrip.ResumeLayout(false);
            this.sampleToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtTimeChunk).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.numSplitSec).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dgvSplits).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.numSplitBeat).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatChunk).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private PictureBox pictureWave;
        private Label lblBpm;
        private Label lblRuntime;
        private RadioButton radioTime;
        private RadioButton radioBeats;
        private Label label2;
        private Label label3;
        private CheckBox chkPosStart;
        private CheckBox chkPosEnd;
        private TextBox txtTimeStart;
        private ToolTip toolTip1;
        private CheckBox chkLimit;
        private Panel panelStart;
        private Panel panelEnd;
        private TextBox txtTimeEnd;
        private Label lblBeats;
        private NumericUpDown numChunks;
        private NumericUpDown txtBeatStart;
        private NumericUpDown txtBeatEnd;
        private Button button1;
        private ToolStrip sampleToolStrip;
        private NumericUpDown txtTimeChunk;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private NumericUpDown numSplitSec;
        private Button btnAddSplit1;
        public DataGridView dgvSplits;
        private NumericUpDown numSplitBeat;
        private NumericUpDown txtBeatChunk;
        private DataGridViewTextBoxColumn LeafEnabled;
        private DataGridViewTextBoxColumn LeafAudio;
        private DataGridViewImageColumn removesplit;
        private Button btnHelp;
        private Label label1;
        private TextBox txtChunkName;
        private Label label12;
        private ToolStripButton btnChunkName;
        private ToolStripButton btnChunkBeats;
        private ToolStripButton btnChunkTime;
        private Label lblChunkTotal;
    }
}