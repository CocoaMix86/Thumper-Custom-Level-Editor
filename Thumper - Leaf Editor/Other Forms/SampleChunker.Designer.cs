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
            this.pictureWave = new PictureBox();
            this.lblBpm = new Label();
            this.lblRuntime = new Label();
            this.radioTime = new RadioButton();
            this.radioBeats = new RadioButton();
            this.label2 = new Label();
            this.txtBeatChunk = new TextBox();
            this.label3 = new Label();
            this.chkPosStart = new CheckBox();
            this.chkPosEnd = new CheckBox();
            this.txtTimeStart = new TextBox();
            this.toolTip1 = new ToolTip(this.components);
            this.chkLimit = new CheckBox();
            this.panelStart = new Panel();
            this.txtBeatStart = new NumericUpDown();
            this.panelEnd = new Panel();
            this.txtBeatEnd = new NumericUpDown();
            this.txtTimeEnd = new TextBox();
            this.lblBeats = new Label();
            this.numChunks = new NumericUpDown();
            this.button1 = new Button();
            this.label1 = new Label();
            this.sampleToolStrip = new ToolStrip();
            this.btnSampleAdd = new ToolStripButton();
            this.btnSampleDelete = new ToolStripButton();
            this.FSBtoSamp = new ToolStripButton();
            this.btnSampleChunk = new ToolStripButton();
            this.txtTimeChunk = new NumericUpDown();
            this.label4 = new Label();
            this.label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)this.pictureWave).BeginInit();
            this.panelStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatStart).BeginInit();
            this.panelEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtBeatEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.numChunks).BeginInit();
            this.sampleToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.txtTimeChunk).BeginInit();
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
            this.pictureWave.Size = new Size(456, 100);
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
            this.lblRuntime.Location = new Point(306, 132);
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
            this.radioTime.Location = new Point(176, 186);
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
            this.radioBeats.Location = new Point(247, 186);
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
            this.label2.Location = new Point(206, 168);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = RightToLeft.No;
            this.label2.Size = new Size(59, 15);
            this.label2.TabIndex = 158;
            this.label2.Text = "Split By:";
            this.label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtBeatChunk
            // 
            this.txtBeatChunk.Enabled = false;
            this.txtBeatChunk.Font = new Font("Consolas", 9F);
            this.txtBeatChunk.Location = new Point(247, 229);
            this.txtBeatChunk.Name = "txtBeatChunk";
            this.txtBeatChunk.Size = new Size(62, 22);
            this.txtBeatChunk.TabIndex = 159;
            this.txtBeatChunk.Text = "0";
            this.txtBeatChunk.TextAlign = HorizontalAlignment.Right;
            this.txtBeatChunk.TextChanged += this.txtBeatChunk_TextChanged;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.ForeColor = Color.White;
            this.label3.Location = new Point(211, 208);
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
            this.chkPosStart.Location = new Point(185, 257);
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
            this.chkPosEnd.Location = new Point(185, 304);
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
            this.txtTimeStart.Size = new Size(109, 22);
            this.txtTimeStart.TabIndex = 165;
            this.txtTimeStart.Text = "00:00:00.00000";
            this.txtTimeStart.TextAlign = HorizontalAlignment.Right;
            // 
            // chkLimit
            // 
            this.chkLimit.AutoSize = true;
            this.chkLimit.ForeColor = Color.White;
            this.chkLimit.Location = new Point(152, 351);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new Size(157, 19);
            this.chkLimit.TabIndex = 168;
            this.chkLimit.Text = "Limit Number of Chunks";
            this.toolTip1.SetToolTip(this.chkLimit, "From start position, create only this many chunks.\r\nLeave unchecked to chunk until end.");
            this.chkLimit.UseVisualStyleBackColor = true;
            this.chkLimit.CheckedChanged += this.chkLimit_CheckedChanged;
            // 
            // panelStart
            // 
            this.panelStart.Controls.Add(this.txtBeatStart);
            this.panelStart.Controls.Add(this.txtTimeStart);
            this.panelStart.Enabled = false;
            this.panelStart.Location = new Point(107, 272);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new Size(247, 26);
            this.panelStart.TabIndex = 170;
            // 
            // txtBeatStart
            // 
            this.txtBeatStart.Location = new Point(140, 3);
            this.txtBeatStart.Name = "txtBeatStart";
            this.txtBeatStart.Size = new Size(62, 23);
            this.txtBeatStart.TabIndex = 174;
            // 
            // panelEnd
            // 
            this.panelEnd.Controls.Add(this.txtBeatEnd);
            this.panelEnd.Controls.Add(this.txtTimeEnd);
            this.panelEnd.Enabled = false;
            this.panelEnd.Location = new Point(107, 320);
            this.panelEnd.Name = "panelEnd";
            this.panelEnd.Size = new Size(247, 26);
            this.panelEnd.TabIndex = 171;
            // 
            // txtBeatEnd
            // 
            this.txtBeatEnd.Location = new Point(140, 2);
            this.txtBeatEnd.Name = "txtBeatEnd";
            this.txtBeatEnd.Size = new Size(62, 23);
            this.txtBeatEnd.TabIndex = 174;
            this.txtBeatEnd.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtTimeEnd
            // 
            this.txtTimeEnd.Font = new Font("Consolas", 9F);
            this.txtTimeEnd.Location = new Point(11, 2);
            this.txtTimeEnd.Name = "txtTimeEnd";
            this.txtTimeEnd.Size = new Size(109, 22);
            this.txtTimeEnd.TabIndex = 165;
            this.txtTimeEnd.Text = "00:00:00.00000";
            this.txtTimeEnd.TextAlign = HorizontalAlignment.Right;
            // 
            // lblBeats
            // 
            this.lblBeats.AutoSize = true;
            this.lblBeats.BackColor = Color.Transparent;
            this.lblBeats.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblBeats.ForeColor = Color.White;
            this.lblBeats.Location = new Point(320, 146);
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
            this.numChunks.Location = new Point(203, 371);
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
            this.button1.Location = new Point(256, 418);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 38);
            this.button1.TabIndex = 174;
            this.button1.Text = "Chunk It";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(6, 398);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = RightToLeft.No;
            this.label1.Size = new Size(337, 60);
            this.label1.TabIndex = 175;
            this.label1.Text = "Chunking a sample will split it where shown on the waveform\r\nand create new samples in the .samp file.\r\n\r\nThe original sample will not be altered.\r\n";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // sampleToolStrip
            // 
            this.sampleToolStrip.AutoSize = false;
            this.sampleToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.sampleToolStrip.GripMargin = new Padding(0);
            this.sampleToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.sampleToolStrip.ImageScalingSize = new Size(20, 20);
            this.sampleToolStrip.Items.AddRange(new ToolStripItem[] { this.btnSampleAdd, this.btnSampleDelete, this.FSBtoSamp, this.btnSampleChunk });
            this.sampleToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.sampleToolStrip.Location = new Point(0, 0);
            this.sampleToolStrip.Name = "sampleToolStrip";
            this.sampleToolStrip.Padding = new Padding(0);
            this.sampleToolStrip.RenderMode = ToolStripRenderMode.System;
            this.sampleToolStrip.Size = new Size(487, 24);
            this.sampleToolStrip.Stretch = true;
            this.sampleToolStrip.TabIndex = 176;
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
            this.btnSampleAdd.Size = new Size(24, 24);
            this.btnSampleAdd.ToolTipText = "Add new sample";
            // 
            // btnSampleDelete
            // 
            this.btnSampleDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSampleDelete.Enabled = false;
            this.btnSampleDelete.Image = Properties.Resources.icon_remove2;
            this.btnSampleDelete.ImageTransparentColor = Color.Magenta;
            this.btnSampleDelete.Margin = new Padding(0);
            this.btnSampleDelete.Name = "btnSampleDelete";
            this.btnSampleDelete.Size = new Size(24, 24);
            this.btnSampleDelete.ToolTipText = "Delete selected phase";
            // 
            // FSBtoSamp
            // 
            this.FSBtoSamp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.FSBtoSamp.Enabled = false;
            this.FSBtoSamp.Image = Properties.Resources.icon_import;
            this.FSBtoSamp.ImageTransparentColor = Color.Magenta;
            this.FSBtoSamp.Name = "FSBtoSamp";
            this.FSBtoSamp.Size = new Size(24, 21);
            this.FSBtoSamp.ToolTipText = "Import FSB files to Sample format";
            // 
            // btnSampleChunk
            // 
            this.btnSampleChunk.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSampleChunk.Enabled = false;
            this.btnSampleChunk.Image = Properties.Resources.icon_split;
            this.btnSampleChunk.ImageTransparentColor = Color.Magenta;
            this.btnSampleChunk.Name = "btnSampleChunk";
            this.btnSampleChunk.Size = new Size(24, 21);
            this.btnSampleChunk.ToolTipText = "Chunk/split the selected sample into\r\nspecific beat/time lengths.";
            // 
            // txtTimeChunk
            // 
            this.txtTimeChunk.DecimalPlaces = 5;
            this.txtTimeChunk.Location = new Point(118, 228);
            this.txtTimeChunk.Name = "txtTimeChunk";
            this.txtTimeChunk.Size = new Size(86, 23);
            this.txtTimeChunk.TabIndex = 177;
            this.txtTimeChunk.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.Transparent;
            this.label4.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.ForeColor = Color.White;
            this.label4.Location = new Point(203, 230);
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
            this.label5.Location = new Point(311, 232);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = RightToLeft.No;
            this.label5.Size = new Size(37, 15);
            this.label5.TabIndex = 179;
            this.label5.Text = "beats";
            this.label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SampleChunker
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ClientSize = new Size(487, 462);
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
            this.Controls.Add(this.txtBeatChunk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioBeats);
            this.Controls.Add(this.radioTime);
            this.Controls.Add(this.lblRuntime);
            this.Controls.Add(this.lblBpm);
            this.Controls.Add(this.pictureWave);
            this.Controls.Add(this.label1);
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
        private TextBox txtBeatChunk;
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
        private Label label1;
        private ToolStrip sampleToolStrip;
        private ToolStripButton btnSampleAdd;
        private ToolStripButton btnSampleDelete;
        private ToolStripButton FSBtoSamp;
        public ToolStripButton btnSampleChunk;
        private NumericUpDown txtTimeChunk;
        private Label label4;
        private Label label5;
    }
}