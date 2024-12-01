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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GateEditor));
            this.panelGate = new Panel();
            this.gateLvlList = new DataGridView();
            this.gatefiletype = new DataGridViewImageColumn();
            this.Lvl = new DataGridViewTextBoxColumn();
            this.Sentry = new DataGridViewComboBoxColumn();
            this.dgvGateBucket = new DataGridViewComboBoxColumn();
            this.label36 = new Label();
            this.gateToolStrip = new ToolStrip();
            this.btnGateLvlAdd = new ToolStripButton();
            this.btnGateLvlDelete = new ToolStripButton();
            this.btnGateLvlUp = new ToolStripButton();
            this.btnGateLvlDown = new ToolStripButton();
            this.panel9 = new Panel();
            this.lblGatebuckethelp = new Label();
            this.label40 = new Label();
            this.label38 = new Label();
            this.checkGateRandom = new CheckBox();
            this.dropGateBoss = new ComboBox();
            this.label2 = new Label();
            this.label41 = new Label();
            this.label42 = new Label();
            this.label43 = new Label();
            this.btnGateOpenRestart = new Button();
            this.label44 = new Label();
            this.btnGateOpenPost = new Button();
            this.dropGatePre = new ComboBox();
            this.btnGateOpenPre = new Button();
            this.dropGatePost = new ComboBox();
            this.lblGateSectionHelp = new Label();
            this.dropGateRestart = new ComboBox();
            this.dropGateSection = new ComboBox();
            this.panelGate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).BeginInit();
            this.gateToolStrip.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGate
            // 
            this.panelGate.BackColor = Color.FromArgb(35, 35, 35);
            this.panelGate.BorderStyle = BorderStyle.FixedSingle;
            this.panelGate.Controls.Add(this.gateLvlList);
            this.panelGate.Controls.Add(this.label36);
            this.panelGate.Controls.Add(this.gateToolStrip);
            this.panelGate.Controls.Add(this.panel9);
            this.panelGate.Dock = DockStyle.Fill;
            this.panelGate.Location = new Point(0, 0);
            this.panelGate.Margin = new Padding(4, 3, 4, 3);
            this.panelGate.MinimumSize = new Size(70, 69);
            this.panelGate.Name = "panelGate";
            this.panelGate.Size = new Size(442, 500);
            this.panelGate.TabIndex = 117;
            this.panelGate.Tag = "editorpanel";
            // 
            // gateLvlList
            // 
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
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gateLvlList.ColumnHeadersHeight = 20;
            this.gateLvlList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gateLvlList.Columns.AddRange(new DataGridViewColumn[] { this.gatefiletype, this.Lvl, this.Sentry, this.dgvGateBucket });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.DefaultCellStyle = dataGridViewCellStyle2;
            this.gateLvlList.Dock = DockStyle.Fill;
            this.gateLvlList.EnableHeadersVisualStyles = false;
            this.gateLvlList.GridColor = Color.Black;
            this.gateLvlList.Location = new Point(0, 13);
            this.gateLvlList.Margin = new Padding(4, 3, 4, 3);
            this.gateLvlList.Name = "gateLvlList";
            this.gateLvlList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle3.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.gateLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gateLvlList.RowHeadersVisible = false;
            this.gateLvlList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gateLvlList.RowTemplate.Height = 20;
            this.gateLvlList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.gateLvlList.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.gateLvlList.Size = new Size(440, 273);
            this.gateLvlList.TabIndex = 80;
            this.gateLvlList.Tag = "editorpaneldgv";
            // 
            // gatefiletype
            // 
            this.gatefiletype.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.gatefiletype.FillWeight = 1F;
            this.gatefiletype.HeaderText = "";
            this.gatefiletype.Name = "gatefiletype";
            this.gatefiletype.ReadOnly = true;
            this.gatefiletype.Width = 5;
            // 
            // Lvl
            // 
            this.Lvl.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Lvl.HeaderText = "Lvl";
            this.Lvl.Name = "Lvl";
            this.Lvl.ReadOnly = true;
            this.Lvl.Resizable = DataGridViewTriState.False;
            this.Lvl.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Sentry
            // 
            this.Sentry.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Sentry.HeaderText = "Sentry";
            this.Sentry.Items.AddRange(new object[] { "None", "Single Lane", "Multi Lane" });
            this.Sentry.Name = "Sentry";
            this.Sentry.Resizable = DataGridViewTriState.False;
            this.Sentry.Width = 48;
            // 
            // dgvGateBucket
            // 
            this.dgvGateBucket.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvGateBucket.HeaderText = "Bucket";
            this.dgvGateBucket.Items.AddRange(new object[] { "0", "1", "2", "3" });
            this.dgvGateBucket.Name = "dgvGateBucket";
            this.dgvGateBucket.Resizable = DataGridViewTriState.False;
            this.dgvGateBucket.Visible = false;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = Color.FromArgb(10, 10, 10);
            this.label36.Dock = DockStyle.Top;
            this.label36.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label36.ForeColor = Color.White;
            this.label36.Location = new Point(0, 0);
            this.label36.Margin = new Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new Size(79, 13);
            this.label36.TabIndex = 116;
            this.label36.Text = "Boss Phases";
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
            this.gateToolStrip.Location = new Point(0, 286);
            this.gateToolStrip.Name = "gateToolStrip";
            this.gateToolStrip.Padding = new Padding(0);
            this.gateToolStrip.RenderMode = ToolStripRenderMode.System;
            this.gateToolStrip.Size = new Size(440, 29);
            this.gateToolStrip.Stretch = true;
            this.gateToolStrip.TabIndex = 142;
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
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.lblGatebuckethelp);
            this.panel9.Controls.Add(this.label40);
            this.panel9.Controls.Add(this.label38);
            this.panel9.Controls.Add(this.checkGateRandom);
            this.panel9.Controls.Add(this.dropGateBoss);
            this.panel9.Controls.Add(this.label2);
            this.panel9.Controls.Add(this.label41);
            this.panel9.Controls.Add(this.label42);
            this.panel9.Controls.Add(this.label43);
            this.panel9.Controls.Add(this.btnGateOpenRestart);
            this.panel9.Controls.Add(this.label44);
            this.panel9.Controls.Add(this.btnGateOpenPost);
            this.panel9.Controls.Add(this.dropGatePre);
            this.panel9.Controls.Add(this.btnGateOpenPre);
            this.panel9.Controls.Add(this.dropGatePost);
            this.panel9.Controls.Add(this.lblGateSectionHelp);
            this.panel9.Controls.Add(this.dropGateRestart);
            this.panel9.Controls.Add(this.dropGateSection);
            this.panel9.Dock = DockStyle.Bottom;
            this.panel9.Location = new Point(0, 315);
            this.panel9.Margin = new Padding(4, 3, 4, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new Size(440, 183);
            this.panel9.TabIndex = 146;
            // 
            // lblGatebuckethelp
            // 
            this.lblGatebuckethelp.AutoSize = true;
            this.lblGatebuckethelp.BackColor = Color.Transparent;
            this.lblGatebuckethelp.Cursor = Cursors.Help;
            this.lblGatebuckethelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblGatebuckethelp.ForeColor = Color.DodgerBlue;
            this.lblGatebuckethelp.Location = new Point(226, 155);
            this.lblGatebuckethelp.Margin = new Padding(4, 0, 4, 0);
            this.lblGatebuckethelp.Name = "lblGatebuckethelp";
            this.lblGatebuckethelp.Size = new Size(15, 16);
            this.lblGatebuckethelp.TabIndex = 145;
            this.lblGatebuckethelp.Text = "?";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label40.ForeColor = Color.Silver;
            this.label40.Location = new Point(110, 3);
            this.label40.Margin = new Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new Size(102, 15);
            this.label40.TabIndex = 121;
            this.label40.Text = "══Gate Options══";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label38.ForeColor = Color.White;
            this.label38.Location = new Point(58, 28);
            this.label38.Margin = new Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new Size(34, 15);
            this.label38.TabIndex = 117;
            this.label38.Text = "Boss";
            // 
            // checkGateRandom
            // 
            this.checkGateRandom.AutoSize = true;
            this.checkGateRandom.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.checkGateRandom.Location = new Point(211, 158);
            this.checkGateRandom.Margin = new Padding(4, 3, 4, 3);
            this.checkGateRandom.Name = "checkGateRandom";
            this.checkGateRandom.Size = new Size(15, 14);
            this.checkGateRandom.TabIndex = 144;
            this.checkGateRandom.UseVisualStyleBackColor = true;
            // 
            // dropGateBoss
            // 
            this.dropGateBoss.BackColor = Color.FromArgb(40, 40, 40);
            this.dropGateBoss.DisplayMember = "plos";
            this.dropGateBoss.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropGateBoss.FlatStyle = FlatStyle.Flat;
            this.dropGateBoss.ForeColor = Color.White;
            this.dropGateBoss.FormattingEnabled = true;
            this.dropGateBoss.Location = new Point(99, 27);
            this.dropGateBoss.Margin = new Padding(4, 3, 4, 3);
            this.dropGateBoss.Name = "dropGateBoss";
            this.dropGateBoss.Size = new Size(158, 23);
            this.dropGateBoss.TabIndex = 117;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.White;
            this.label2.Location = new Point(96, 156);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(97, 15);
            this.label2.TabIndex = 143;
            this.label2.Text = "Enable Random";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label41.ForeColor = Color.DodgerBlue;
            this.label41.Location = new Point(47, 53);
            this.label41.Margin = new Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new Size(44, 15);
            this.label41.TabIndex = 122;
            this.label41.Text = "Pre Lvl";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label42.ForeColor = Color.DodgerBlue;
            this.label42.Location = new Point(41, 80);
            this.label42.Margin = new Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new Size(49, 15);
            this.label42.TabIndex = 123;
            this.label42.Text = "Post Lvl";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label43.ForeColor = Color.DodgerBlue;
            this.label43.Location = new Point(23, 105);
            this.label43.Margin = new Padding(4, 0, 4, 0);
            this.label43.Name = "label43";
            this.label43.Size = new Size(64, 15);
            this.label43.TabIndex = 124;
            this.label43.Text = "Restart Lvl";
            // 
            // btnGateOpenRestart
            // 
            this.btnGateOpenRestart.BackColor = Color.Gray;
            this.btnGateOpenRestart.Cursor = Cursors.Hand;
            this.btnGateOpenRestart.FlatStyle = FlatStyle.Popup;
            this.btnGateOpenRestart.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnGateOpenRestart.ForeColor = Color.Black;
            this.btnGateOpenRestart.Image = (Image)resources.GetObject("btnGateOpenRestart.Image");
            this.btnGateOpenRestart.ImageAlign = ContentAlignment.MiddleRight;
            this.btnGateOpenRestart.Location = new Point(259, 102);
            this.btnGateOpenRestart.Margin = new Padding(4, 3, 4, 3);
            this.btnGateOpenRestart.Name = "btnGateOpenRestart";
            this.btnGateOpenRestart.Size = new Size(27, 27);
            this.btnGateOpenRestart.TabIndex = 133;
            this.btnGateOpenRestart.TextAlign = ContentAlignment.TopCenter;
            this.btnGateOpenRestart.UseVisualStyleBackColor = false;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label44.ForeColor = Color.White;
            this.label44.Location = new Point(8, 130);
            this.label44.Margin = new Padding(4, 0, 4, 0);
            this.label44.Name = "label44";
            this.label44.Size = new Size(77, 15);
            this.label44.TabIndex = 125;
            this.label44.Text = "Section Type";
            // 
            // btnGateOpenPost
            // 
            this.btnGateOpenPost.BackColor = Color.Gray;
            this.btnGateOpenPost.Cursor = Cursors.Hand;
            this.btnGateOpenPost.FlatStyle = FlatStyle.Popup;
            this.btnGateOpenPost.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnGateOpenPost.ForeColor = Color.Black;
            this.btnGateOpenPost.Image = (Image)resources.GetObject("btnGateOpenPost.Image");
            this.btnGateOpenPost.ImageAlign = ContentAlignment.MiddleRight;
            this.btnGateOpenPost.Location = new Point(259, 76);
            this.btnGateOpenPost.Margin = new Padding(4, 3, 4, 3);
            this.btnGateOpenPost.Name = "btnGateOpenPost";
            this.btnGateOpenPost.Size = new Size(27, 27);
            this.btnGateOpenPost.TabIndex = 132;
            this.btnGateOpenPost.TextAlign = ContentAlignment.TopCenter;
            this.btnGateOpenPost.UseVisualStyleBackColor = false;
            // 
            // dropGatePre
            // 
            this.dropGatePre.BackColor = Color.FromArgb(40, 40, 40);
            this.dropGatePre.DisplayMember = "plos";
            this.dropGatePre.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropGatePre.FlatStyle = FlatStyle.Flat;
            this.dropGatePre.ForeColor = Color.White;
            this.dropGatePre.FormattingEnabled = true;
            this.dropGatePre.Items.AddRange(new object[] { "<none>" });
            this.dropGatePre.Location = new Point(99, 52);
            this.dropGatePre.Margin = new Padding(4, 3, 4, 3);
            this.dropGatePre.Name = "dropGatePre";
            this.dropGatePre.Size = new Size(158, 23);
            this.dropGatePre.TabIndex = 126;
            // 
            // btnGateOpenPre
            // 
            this.btnGateOpenPre.BackColor = Color.Gray;
            this.btnGateOpenPre.Cursor = Cursors.Hand;
            this.btnGateOpenPre.FlatStyle = FlatStyle.Popup;
            this.btnGateOpenPre.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnGateOpenPre.ForeColor = Color.Black;
            this.btnGateOpenPre.Image = (Image)resources.GetObject("btnGateOpenPre.Image");
            this.btnGateOpenPre.ImageAlign = ContentAlignment.MiddleRight;
            this.btnGateOpenPre.Location = new Point(259, 51);
            this.btnGateOpenPre.Margin = new Padding(4, 3, 4, 3);
            this.btnGateOpenPre.Name = "btnGateOpenPre";
            this.btnGateOpenPre.Size = new Size(27, 27);
            this.btnGateOpenPre.TabIndex = 119;
            this.btnGateOpenPre.TextAlign = ContentAlignment.TopCenter;
            this.btnGateOpenPre.UseVisualStyleBackColor = false;
            // 
            // dropGatePost
            // 
            this.dropGatePost.BackColor = Color.FromArgb(40, 40, 40);
            this.dropGatePost.DisplayMember = "plos";
            this.dropGatePost.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropGatePost.FlatStyle = FlatStyle.Flat;
            this.dropGatePost.ForeColor = Color.White;
            this.dropGatePost.FormattingEnabled = true;
            this.dropGatePost.Items.AddRange(new object[] { "<none>" });
            this.dropGatePost.Location = new Point(99, 77);
            this.dropGatePost.Margin = new Padding(4, 3, 4, 3);
            this.dropGatePost.Name = "dropGatePost";
            this.dropGatePost.Size = new Size(158, 23);
            this.dropGatePost.TabIndex = 127;
            // 
            // lblGateSectionHelp
            // 
            this.lblGateSectionHelp.AutoSize = true;
            this.lblGateSectionHelp.BackColor = Color.Transparent;
            this.lblGateSectionHelp.Cursor = Cursors.Help;
            this.lblGateSectionHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblGateSectionHelp.ForeColor = Color.DodgerBlue;
            this.lblGateSectionHelp.Location = new Point(337, 129);
            this.lblGateSectionHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblGateSectionHelp.Name = "lblGateSectionHelp";
            this.lblGateSectionHelp.Size = new Size(15, 16);
            this.lblGateSectionHelp.TabIndex = 131;
            this.lblGateSectionHelp.Text = "?";
            this.lblGateSectionHelp.Click += this.lblGateSectionHelp_Click;
            // 
            // dropGateRestart
            // 
            this.dropGateRestart.BackColor = Color.FromArgb(40, 40, 40);
            this.dropGateRestart.DisplayMember = "plos";
            this.dropGateRestart.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropGateRestart.FlatStyle = FlatStyle.Flat;
            this.dropGateRestart.ForeColor = Color.White;
            this.dropGateRestart.FormattingEnabled = true;
            this.dropGateRestart.Items.AddRange(new object[] { "<none>" });
            this.dropGateRestart.Location = new Point(99, 103);
            this.dropGateRestart.Margin = new Padding(4, 3, 4, 3);
            this.dropGateRestart.Name = "dropGateRestart";
            this.dropGateRestart.Size = new Size(158, 23);
            this.dropGateRestart.TabIndex = 128;
            // 
            // dropGateSection
            // 
            this.dropGateSection.BackColor = Color.FromArgb(40, 40, 40);
            this.dropGateSection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.dropGateSection.FlatStyle = FlatStyle.Flat;
            this.dropGateSection.ForeColor = Color.White;
            this.dropGateSection.FormattingEnabled = true;
            this.dropGateSection.Items.AddRange(new object[] { "SECTION_LINEAR", "SECTION_BOSS_TRIANGLE", "SECTION_BOSS_CIRCLE", "SECTION_BOSS_MINI", "SECTION_BOSS_CRAKHED", "SECTION_BOSS_CRAKHED_FINAL", "SECTION_BOSS_PYRAMID" });
            this.dropGateSection.Location = new Point(99, 128);
            this.dropGateSection.Margin = new Padding(4, 3, 4, 3);
            this.dropGateSection.Name = "dropGateSection";
            this.dropGateSection.Size = new Size(237, 23);
            this.dropGateSection.TabIndex = 129;
            // 
            // Form_GateEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(442, 500);
            this.Controls.Add(this.panelGate);
            this.DoubleBuffered = true;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_GateEditor";
            this.Text = "Gate Editor";
            this.panelGate.ResumeLayout(false);
            this.panelGate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.gateLvlList).EndInit();
            this.gateToolStrip.ResumeLayout(false);
            this.gateToolStrip.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelGate;
        private System.Windows.Forms.ToolStrip gateToolStrip;
        private System.Windows.Forms.ToolStripButton btnGateLvlAdd;
        private System.Windows.Forms.ToolStripButton btnGateLvlDelete;
        private System.Windows.Forms.ToolStripButton btnGateLvlUp;
        private System.Windows.Forms.ToolStripButton btnGateLvlDown;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.DataGridView gateLvlList;
        private System.Windows.Forms.DataGridViewImageColumn gatefiletype;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lvl;
        private System.Windows.Forms.DataGridViewComboBoxColumn Sentry;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvGateBucket;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label lblGatebuckethelp;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.CheckBox checkGateRandom;
        private System.Windows.Forms.ComboBox dropGateBoss;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button btnGateOpenRestart;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button btnGateOpenPost;
        private System.Windows.Forms.ComboBox dropGatePre;
        private System.Windows.Forms.Button btnGateOpenPre;
        private System.Windows.Forms.ComboBox dropGatePost;
        private System.Windows.Forms.Label lblGateSectionHelp;
        private System.Windows.Forms.ComboBox dropGateRestart;
        private System.Windows.Forms.ComboBox dropGateSection;
    }
}