﻿namespace Thumper_Custom_Level_Editor.Editor_Panels
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GateEditor));
            this.panelGate = new System.Windows.Forms.Panel();
            this.gateToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnGateLvlAdd = new System.Windows.Forms.ToolStripButton();
            this.btnGateLvlDelete = new System.Windows.Forms.ToolStripButton();
            this.btnGateLvlUp = new System.Windows.Forms.ToolStripButton();
            this.btnGateLvlDown = new System.Windows.Forms.ToolStripButton();
            this.label40 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.gateLvlList = new System.Windows.Forms.DataGridView();
            this.gatefiletype = new System.Windows.Forms.DataGridViewImageColumn();
            this.Lvl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sentry = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvGateBucket = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lblGatebuckethelp = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.checkGateRandom = new System.Windows.Forms.CheckBox();
            this.dropGateBoss = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.btnGateOpenRestart = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.btnGateOpenPost = new System.Windows.Forms.Button();
            this.dropGatePre = new System.Windows.Forms.ComboBox();
            this.btnGateOpenPre = new System.Windows.Forms.Button();
            this.dropGatePost = new System.Windows.Forms.ComboBox();
            this.lblGateSectionHelp = new System.Windows.Forms.Label();
            this.dropGateRestart = new System.Windows.Forms.ComboBox();
            this.dropGateSection = new System.Windows.Forms.ComboBox();
            this.panelGate.SuspendLayout();
            this.gateToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gateLvlList)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGate
            // 
            this.panelGate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panelGate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGate.Controls.Add(this.gateLvlList);
            this.panelGate.Controls.Add(this.label36);
            this.panelGate.Controls.Add(this.gateToolStrip);
            this.panelGate.Controls.Add(this.panel9);
            this.panelGate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGate.Location = new System.Drawing.Point(0, 0);
            this.panelGate.MinimumSize = new System.Drawing.Size(60, 60);
            this.panelGate.Name = "panelGate";
            this.panelGate.Size = new System.Drawing.Size(379, 433);
            this.panelGate.TabIndex = 117;
            this.panelGate.Tag = "editorpanel";
            // 
            // gateToolStrip
            // 
            this.gateToolStrip.AutoSize = false;
            this.gateToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.gateToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gateToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.gateToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.gateToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.gateToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGateLvlAdd,
            this.btnGateLvlDelete,
            this.btnGateLvlUp,
            this.btnGateLvlDown});
            this.gateToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.gateToolStrip.Location = new System.Drawing.Point(0, 247);
            this.gateToolStrip.Name = "gateToolStrip";
            this.gateToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.gateToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.gateToolStrip.Size = new System.Drawing.Size(377, 25);
            this.gateToolStrip.Stretch = true;
            this.gateToolStrip.TabIndex = 142;
            // 
            // btnGateLvlAdd
            // 
            this.btnGateLvlAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGateLvlAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGateLvlAdd.ForeColor = System.Drawing.Color.White;
            this.btnGateLvlAdd.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_plus;
            this.btnGateLvlAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGateLvlAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnGateLvlAdd.Name = "btnGateLvlAdd";
            this.btnGateLvlAdd.Size = new System.Drawing.Size(24, 25);
            this.btnGateLvlAdd.ToolTipText = "Add new phase";
            // 
            // btnGateLvlDelete
            // 
            this.btnGateLvlDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGateLvlDelete.Enabled = false;
            this.btnGateLvlDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.btnGateLvlDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGateLvlDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnGateLvlDelete.Name = "btnGateLvlDelete";
            this.btnGateLvlDelete.Size = new System.Drawing.Size(24, 25);
            this.btnGateLvlDelete.ToolTipText = "Delete selected phase";
            // 
            // btnGateLvlUp
            // 
            this.btnGateLvlUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGateLvlUp.Enabled = false;
            this.btnGateLvlUp.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowup2;
            this.btnGateLvlUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGateLvlUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnGateLvlUp.Name = "btnGateLvlUp";
            this.btnGateLvlUp.Size = new System.Drawing.Size(24, 25);
            this.btnGateLvlUp.ToolTipText = "Move selected phase up";
            // 
            // btnGateLvlDown
            // 
            this.btnGateLvlDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGateLvlDown.Enabled = false;
            this.btnGateLvlDown.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowdown2;
            this.btnGateLvlDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGateLvlDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnGateLvlDown.Name = "btnGateLvlDown";
            this.btnGateLvlDown.Size = new System.Drawing.Size(24, 25);
            this.btnGateLvlDown.ToolTipText = "Move selected phase down";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.Silver;
            this.label40.Location = new System.Drawing.Point(94, 3);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(102, 15);
            this.label40.TabIndex = 121;
            this.label40.Text = "══Gate Options══";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.label36.Dock = System.Windows.Forms.DockStyle.Top;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.White;
            this.label36.Location = new System.Drawing.Point(0, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(79, 13);
            this.label36.TabIndex = 116;
            this.label36.Text = "Boss Phases";
            // 
            // gateLvlList
            // 
            this.gateLvlList.AllowUserToAddRows = false;
            this.gateLvlList.AllowUserToDeleteRows = false;
            this.gateLvlList.AllowUserToResizeRows = false;
            this.gateLvlList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.gateLvlList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gateLvlList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gateLvlList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gateLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gateLvlList.ColumnHeadersHeight = 20;
            this.gateLvlList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gateLvlList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gatefiletype,
            this.Lvl,
            this.Sentry,
            this.dgvGateBucket});
            this.gateLvlList.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gateLvlList.DefaultCellStyle = dataGridViewCellStyle8;
            this.gateLvlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gateLvlList.EnableHeadersVisualStyles = false;
            this.gateLvlList.GridColor = System.Drawing.Color.Black;
            this.gateLvlList.Location = new System.Drawing.Point(0, 13);
            this.gateLvlList.Name = "gateLvlList";
            this.gateLvlList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gateLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gateLvlList.RowHeadersVisible = false;
            this.gateLvlList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gateLvlList.RowTemplate.Height = 20;
            this.gateLvlList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gateLvlList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gateLvlList.Size = new System.Drawing.Size(377, 234);
            this.gateLvlList.TabIndex = 80;
            this.gateLvlList.Tag = "editorpaneldgv";
            // 
            // gatefiletype
            // 
            this.gatefiletype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.gatefiletype.FillWeight = 1F;
            this.gatefiletype.HeaderText = "";
            this.gatefiletype.Name = "gatefiletype";
            this.gatefiletype.ReadOnly = true;
            this.gatefiletype.Width = 5;
            // 
            // Lvl
            // 
            this.Lvl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Lvl.HeaderText = "Lvl";
            this.Lvl.Name = "Lvl";
            this.Lvl.ReadOnly = true;
            this.Lvl.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Lvl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Sentry
            // 
            this.Sentry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Sentry.HeaderText = "Sentry";
            this.Sentry.Items.AddRange(new object[] {
            "None",
            "Single Lane",
            "Multi Lane"});
            this.Sentry.Name = "Sentry";
            this.Sentry.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Sentry.Width = 48;
            // 
            // dgvGateBucket
            // 
            this.dgvGateBucket.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvGateBucket.HeaderText = "Bucket";
            this.dgvGateBucket.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.dgvGateBucket.Name = "dgvGateBucket";
            this.dgvGateBucket.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGateBucket.Visible = false;
            this.dgvGateBucket.Width = 51;
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
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 272);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(377, 159);
            this.panel9.TabIndex = 146;
            // 
            // lblGatebuckethelp
            // 
            this.lblGatebuckethelp.AutoSize = true;
            this.lblGatebuckethelp.BackColor = System.Drawing.Color.Transparent;
            this.lblGatebuckethelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblGatebuckethelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGatebuckethelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblGatebuckethelp.Location = new System.Drawing.Point(194, 134);
            this.lblGatebuckethelp.Name = "lblGatebuckethelp";
            this.lblGatebuckethelp.Size = new System.Drawing.Size(15, 16);
            this.lblGatebuckethelp.TabIndex = 145;
            this.lblGatebuckethelp.Text = "?";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.White;
            this.label38.Location = new System.Drawing.Point(50, 24);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(34, 15);
            this.label38.TabIndex = 117;
            this.label38.Text = "Boss";
            // 
            // checkGateRandom
            // 
            this.checkGateRandom.AutoSize = true;
            this.checkGateRandom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkGateRandom.Location = new System.Drawing.Point(181, 137);
            this.checkGateRandom.Name = "checkGateRandom";
            this.checkGateRandom.Size = new System.Drawing.Size(15, 14);
            this.checkGateRandom.TabIndex = 144;
            this.checkGateRandom.UseVisualStyleBackColor = true;
            // 
            // dropGateBoss
            // 
            this.dropGateBoss.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropGateBoss.DisplayMember = "plos";
            this.dropGateBoss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropGateBoss.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropGateBoss.ForeColor = System.Drawing.Color.White;
            this.dropGateBoss.FormattingEnabled = true;
            this.dropGateBoss.Location = new System.Drawing.Point(85, 23);
            this.dropGateBoss.Name = "dropGateBoss";
            this.dropGateBoss.Size = new System.Drawing.Size(136, 21);
            this.dropGateBoss.TabIndex = 117;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(82, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 143;
            this.label2.Text = "Enable Random";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label41.Location = new System.Drawing.Point(40, 46);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(44, 15);
            this.label41.TabIndex = 122;
            this.label41.Text = "Pre Lvl";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label42.Location = new System.Drawing.Point(35, 69);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(49, 15);
            this.label42.TabIndex = 123;
            this.label42.Text = "Post Lvl";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label43.Location = new System.Drawing.Point(20, 91);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(64, 15);
            this.label43.TabIndex = 124;
            this.label43.Text = "Restart Lvl";
            // 
            // btnGateOpenRestart
            // 
            this.btnGateOpenRestart.BackColor = System.Drawing.Color.Gray;
            this.btnGateOpenRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGateOpenRestart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGateOpenRestart.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGateOpenRestart.ForeColor = System.Drawing.Color.Black;
            this.btnGateOpenRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnGateOpenRestart.Image")));
            this.btnGateOpenRestart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGateOpenRestart.Location = new System.Drawing.Point(222, 88);
            this.btnGateOpenRestart.Name = "btnGateOpenRestart";
            this.btnGateOpenRestart.Size = new System.Drawing.Size(23, 23);
            this.btnGateOpenRestart.TabIndex = 133;
            this.btnGateOpenRestart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGateOpenRestart.UseVisualStyleBackColor = false;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.Color.White;
            this.label44.Location = new System.Drawing.Point(7, 113);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(77, 15);
            this.label44.TabIndex = 125;
            this.label44.Text = "Section Type";
            // 
            // btnGateOpenPost
            // 
            this.btnGateOpenPost.BackColor = System.Drawing.Color.Gray;
            this.btnGateOpenPost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGateOpenPost.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGateOpenPost.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGateOpenPost.ForeColor = System.Drawing.Color.Black;
            this.btnGateOpenPost.Image = ((System.Drawing.Image)(resources.GetObject("btnGateOpenPost.Image")));
            this.btnGateOpenPost.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGateOpenPost.Location = new System.Drawing.Point(222, 66);
            this.btnGateOpenPost.Name = "btnGateOpenPost";
            this.btnGateOpenPost.Size = new System.Drawing.Size(23, 23);
            this.btnGateOpenPost.TabIndex = 132;
            this.btnGateOpenPost.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGateOpenPost.UseVisualStyleBackColor = false;
            // 
            // dropGatePre
            // 
            this.dropGatePre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropGatePre.DisplayMember = "plos";
            this.dropGatePre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropGatePre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropGatePre.ForeColor = System.Drawing.Color.White;
            this.dropGatePre.FormattingEnabled = true;
            this.dropGatePre.Items.AddRange(new object[] {
            "<none>"});
            this.dropGatePre.Location = new System.Drawing.Point(85, 45);
            this.dropGatePre.Name = "dropGatePre";
            this.dropGatePre.Size = new System.Drawing.Size(136, 21);
            this.dropGatePre.TabIndex = 126;
            // 
            // btnGateOpenPre
            // 
            this.btnGateOpenPre.BackColor = System.Drawing.Color.Gray;
            this.btnGateOpenPre.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGateOpenPre.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGateOpenPre.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGateOpenPre.ForeColor = System.Drawing.Color.Black;
            this.btnGateOpenPre.Image = ((System.Drawing.Image)(resources.GetObject("btnGateOpenPre.Image")));
            this.btnGateOpenPre.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGateOpenPre.Location = new System.Drawing.Point(222, 44);
            this.btnGateOpenPre.Name = "btnGateOpenPre";
            this.btnGateOpenPre.Size = new System.Drawing.Size(23, 23);
            this.btnGateOpenPre.TabIndex = 119;
            this.btnGateOpenPre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGateOpenPre.UseVisualStyleBackColor = false;
            // 
            // dropGatePost
            // 
            this.dropGatePost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropGatePost.DisplayMember = "plos";
            this.dropGatePost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropGatePost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropGatePost.ForeColor = System.Drawing.Color.White;
            this.dropGatePost.FormattingEnabled = true;
            this.dropGatePost.Items.AddRange(new object[] {
            "<none>"});
            this.dropGatePost.Location = new System.Drawing.Point(85, 67);
            this.dropGatePost.Name = "dropGatePost";
            this.dropGatePost.Size = new System.Drawing.Size(136, 21);
            this.dropGatePost.TabIndex = 127;
            // 
            // lblGateSectionHelp
            // 
            this.lblGateSectionHelp.AutoSize = true;
            this.lblGateSectionHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblGateSectionHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblGateSectionHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGateSectionHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblGateSectionHelp.Location = new System.Drawing.Point(289, 112);
            this.lblGateSectionHelp.Name = "lblGateSectionHelp";
            this.lblGateSectionHelp.Size = new System.Drawing.Size(15, 16);
            this.lblGateSectionHelp.TabIndex = 131;
            this.lblGateSectionHelp.Text = "?";
            this.lblGateSectionHelp.Click += new System.EventHandler(this.lblGateSectionHelp_Click);
            // 
            // dropGateRestart
            // 
            this.dropGateRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropGateRestart.DisplayMember = "plos";
            this.dropGateRestart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropGateRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropGateRestart.ForeColor = System.Drawing.Color.White;
            this.dropGateRestart.FormattingEnabled = true;
            this.dropGateRestart.Items.AddRange(new object[] {
            "<none>"});
            this.dropGateRestart.Location = new System.Drawing.Point(85, 89);
            this.dropGateRestart.Name = "dropGateRestart";
            this.dropGateRestart.Size = new System.Drawing.Size(136, 21);
            this.dropGateRestart.TabIndex = 128;
            // 
            // dropGateSection
            // 
            this.dropGateSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropGateSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropGateSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropGateSection.ForeColor = System.Drawing.Color.White;
            this.dropGateSection.FormattingEnabled = true;
            this.dropGateSection.Items.AddRange(new object[] {
            "SECTION_LINEAR",
            "SECTION_BOSS_TRIANGLE",
            "SECTION_BOSS_CIRCLE",
            "SECTION_BOSS_MINI",
            "SECTION_BOSS_CRAKHED",
            "SECTION_BOSS_CRAKHED_FINAL",
            "SECTION_BOSS_PYRAMID"});
            this.dropGateSection.Location = new System.Drawing.Point(85, 111);
            this.dropGateSection.Name = "dropGateSection";
            this.dropGateSection.Size = new System.Drawing.Size(204, 21);
            this.dropGateSection.TabIndex = 129;
            // 
            // Form_GateEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 433);
            this.Controls.Add(this.panelGate);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_GateEditor";
            this.Text = "Gate Editor";
            this.panelGate.ResumeLayout(false);
            this.panelGate.PerformLayout();
            this.gateToolStrip.ResumeLayout(false);
            this.gateToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gateLvlList)).EndInit();
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