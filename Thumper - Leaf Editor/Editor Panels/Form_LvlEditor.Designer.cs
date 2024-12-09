﻿namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_LvlEditor
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
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_LvlEditor));
            this.toolTip1 = new ToolTip(this.components);
            this.lvlToolStrip = new ToolStrip();
            this.btnLvlLeafAdd = new ToolStripButton();
            this.btnLvlLeafDelete = new ToolStripButton();
            this.btnLvlLeafUp = new ToolStripButton();
            this.btnLvlLeafDown = new ToolStripButton();
            this.btnLvlLeafCopy = new ToolStripButton();
            this.btnLvlLeafPaste = new ToolStripButton();
            this.btnLvlLeafRandom = new ToolStripButton();
            this.label29 = new Label();
            this.lvlLeafList = new DataGridView();
            this.lvlfiletype = new DataGridViewImageColumn();
            this.Leaf = new DataGridViewTextBoxColumn();
            this.Beats = new DataGridViewTextBoxColumn();
            this.splitContainer1 = new SplitContainer();
            this.lblMasterlvllistHelp = new Label();
            this.propertyGridLvl = new PropertyGrid();
            this.splitContainer2 = new SplitContainer();
            this.label22 = new Label();
            this.lvlLoopTracks = new DataGridView();
            this.lvlLoopToolStrip = new ToolStrip();
            this.btnLvlLoopAdd = new ToolStripButton();
            this.btnLvlLoopDelete = new ToolStripButton();
            this.lvlPathsToolStrip = new ToolStrip();
            this.btnLvlPathAdd = new ToolStripButton();
            this.btnLvlPathDelete = new ToolStripButton();
            this.btnLvlPathUp = new ToolStripButton();
            this.btnLvlPathDown = new ToolStripButton();
            this.btnLvlCopyTunnel = new ToolStripButton();
            this.btnLvlPasteTunnel = new ToolStripButton();
            this.btnLvlPathClear = new ToolStripButton();
            this.chkTunnelCopy = new ToolStripButton();
            this.btnLvlRandomTunnel = new ToolStripButton();
            this.lvlLeafPaths = new DataGridView();
            this.lblLvlTunnels = new Label();
            this.LoopSample = new DataGridViewComboBoxColumn();
            this.BeatsPerLoop = new DataGridViewTextBoxColumn();
            this.splitContainer3 = new SplitContainer();
            this.lvlToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).BeginInit();
            this.lvlLoopToolStrip.SuspendLayout();
            this.lvlPathsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer3).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvlToolStrip
            // 
            this.lvlToolStrip.AutoSize = false;
            this.lvlToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlToolStrip.Dock = DockStyle.Bottom;
            this.lvlToolStrip.GripMargin = new Padding(0);
            this.lvlToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLeafAdd, this.btnLvlLeafDelete, this.btnLvlLeafUp, this.btnLvlLeafDown, this.btnLvlLeafCopy, this.btnLvlLeafPaste, this.btnLvlLeafRandom });
            this.lvlToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlToolStrip.Location = new Point(0, 271);
            this.lvlToolStrip.Name = "lvlToolStrip";
            this.lvlToolStrip.Padding = new Padding(0);
            this.lvlToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlToolStrip.Size = new Size(350, 29);
            this.lvlToolStrip.Stretch = true;
            this.lvlToolStrip.TabIndex = 141;
            // 
            // btnLvlLeafAdd
            // 
            this.btnLvlLeafAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlLeafAdd.ForeColor = Color.White;
            this.btnLvlLeafAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlLeafAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafAdd.Margin = new Padding(0);
            this.btnLvlLeafAdd.Name = "btnLvlLeafAdd";
            this.btnLvlLeafAdd.Size = new Size(24, 29);
            this.btnLvlLeafAdd.ToolTipText = "Add new sublevel to the list";
            // 
            // btnLvlLeafDelete
            // 
            this.btnLvlLeafDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDelete.Enabled = false;
            this.btnLvlLeafDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLeafDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDelete.Margin = new Padding(0);
            this.btnLvlLeafDelete.Name = "btnLvlLeafDelete";
            this.btnLvlLeafDelete.Size = new Size(24, 29);
            this.btnLvlLeafDelete.ToolTipText = "Delete selected sublevel from this list";
            // 
            // btnLvlLeafUp
            // 
            this.btnLvlLeafUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafUp.Enabled = false;
            this.btnLvlLeafUp.Image = Properties.Resources.icon_arrowup2;
            this.btnLvlLeafUp.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafUp.Margin = new Padding(0);
            this.btnLvlLeafUp.Name = "btnLvlLeafUp";
            this.btnLvlLeafUp.Size = new Size(24, 29);
            this.btnLvlLeafUp.ToolTipText = "Move selected sublevel up";
            // 
            // btnLvlLeafDown
            // 
            this.btnLvlLeafDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafDown.Enabled = false;
            this.btnLvlLeafDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlLeafDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafDown.Margin = new Padding(0);
            this.btnLvlLeafDown.Name = "btnLvlLeafDown";
            this.btnLvlLeafDown.Size = new Size(24, 29);
            this.btnLvlLeafDown.ToolTipText = "Move selected sublevel down";
            // 
            // btnLvlLeafCopy
            // 
            this.btnLvlLeafCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafCopy.Enabled = false;
            this.btnLvlLeafCopy.Image = Properties.Resources.icon_copy2;
            this.btnLvlLeafCopy.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafCopy.Margin = new Padding(0);
            this.btnLvlLeafCopy.Name = "btnLvlLeafCopy";
            this.btnLvlLeafCopy.Size = new Size(24, 29);
            this.btnLvlLeafCopy.ToolTipText = "Copy selected sublevel";
            // 
            // btnLvlLeafPaste
            // 
            this.btnLvlLeafPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafPaste.Enabled = false;
            this.btnLvlLeafPaste.Image = Properties.Resources.icon_paste2;
            this.btnLvlLeafPaste.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafPaste.Name = "btnLvlLeafPaste";
            this.btnLvlLeafPaste.Size = new Size(24, 26);
            this.btnLvlLeafPaste.ToolTipText = "Paste the copied sublevel";
            // 
            // btnLvlLeafRandom
            // 
            this.btnLvlLeafRandom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLeafRandom.Enabled = false;
            this.btnLvlLeafRandom.Image = Properties.Resources.icon_random;
            this.btnLvlLeafRandom.ImageTransparentColor = Color.Magenta;
            this.btnLvlLeafRandom.Name = "btnLvlLeafRandom";
            this.btnLvlLeafRandom.Size = new Size(24, 26);
            this.btnLvlLeafRandom.ToolTipText = "Add a random leaf";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = Color.FromArgb(10, 10, 10);
            this.label29.Dock = DockStyle.Top;
            this.label29.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label29.ForeColor = Color.White;
            this.label29.Location = new Point(0, 0);
            this.label29.Margin = new Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new Size(56, 13);
            this.label29.TabIndex = 93;
            this.label29.Text = "Leaf List";
            // 
            // lvlLeafList
            // 
            this.lvlLeafList.AllowUserToAddRows = false;
            this.lvlLeafList.AllowUserToDeleteRows = false;
            this.lvlLeafList.AllowUserToResizeRows = false;
            this.lvlLeafList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLeafList.BorderStyle = BorderStyle.None;
            this.lvlLeafList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLeafList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle11.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle11.ForeColor = Color.White;
            dataGridViewCellStyle11.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle11.SelectionForeColor = Color.White;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.lvlLeafList.ColumnHeadersHeight = 20;
            this.lvlLeafList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.lvlLeafList.Columns.AddRange(new DataGridViewColumn[] { this.lvlfiletype, this.Leaf, this.Beats });
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle13.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle13.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle13.NullValue = null;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.DefaultCellStyle = dataGridViewCellStyle13;
            this.lvlLeafList.Dock = DockStyle.Fill;
            this.lvlLeafList.EnableHeadersVisualStyles = false;
            this.lvlLeafList.GridColor = Color.Black;
            this.lvlLeafList.Location = new Point(0, 13);
            this.lvlLeafList.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafList.Name = "lvlLeafList";
            this.lvlLeafList.ReadOnly = true;
            this.lvlLeafList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.False;
            this.lvlLeafList.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.lvlLeafList.RowHeadersVisible = false;
            this.lvlLeafList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lvlLeafList.RowTemplate.Height = 20;
            this.lvlLeafList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafList.Size = new Size(350, 258);
            this.lvlLeafList.TabIndex = 74;
            this.lvlLeafList.Tag = "editorpaneldgv";
            this.lvlLeafList.CellDoubleClick += this.lvlLeafList_CellDoubleClick;
            // 
            // lvlfiletype
            // 
            this.lvlfiletype.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.lvlfiletype.HeaderText = "";
            this.lvlfiletype.Name = "lvlfiletype";
            this.lvlfiletype.ReadOnly = true;
            this.lvlfiletype.Width = 5;
            // 
            // Leaf
            // 
            this.Leaf.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Leaf.FillWeight = 50F;
            this.Leaf.HeaderText = "Leaf";
            this.Leaf.Name = "Leaf";
            this.Leaf.ReadOnly = true;
            this.Leaf.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Beats
            // 
            this.Beats.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Beats.DefaultCellStyle = dataGridViewCellStyle12;
            this.Beats.FillWeight = 50F;
            this.Beats.HeaderText = "Runtime";
            this.Beats.Name = "Beats";
            this.Beats.ReadOnly = true;
            this.Beats.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblMasterlvllistHelp);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new Size(707, 506);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 119;
            // 
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = Cursors.Help;
            this.lblMasterlvllistHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblMasterlvllistHelp.ForeColor = Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new Point(1004, -3);
            this.lblMasterlvllistHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // propertyGridLvl
            // 
            this.propertyGridLvl.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLvl.CategoryForeColor = Color.White;
            this.propertyGridLvl.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridLvl.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridLvl.Dock = DockStyle.Fill;
            this.propertyGridLvl.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridLvl.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLvl.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridLvl.HelpForeColor = Color.White;
            this.propertyGridLvl.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridLvl.Location = new Point(0, 0);
            this.propertyGridLvl.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridLvl.Name = "propertyGridLvl";
            this.propertyGridLvl.PropertySort = PropertySort.Categorized;
            this.propertyGridLvl.RightToLeft = RightToLeft.No;
            this.propertyGridLvl.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridLvl.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridLvl.Size = new Size(352, 300);
            this.propertyGridLvl.TabIndex = 149;
            this.propertyGridLvl.ToolbarVisible = false;
            this.propertyGridLvl.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridLvl.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridLvl.ViewForeColor = Color.White;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvlLeafList);
            this.splitContainer2.Panel1.Controls.Add(this.lvlToolStrip);
            this.splitContainer2.Panel1.Controls.Add(this.label29);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.propertyGridLvl);
            this.splitContainer2.Size = new Size(707, 300);
            this.splitContainer2.SplitterDistance = 350;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 161;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = Color.FromArgb(10, 10, 10);
            this.label22.Dock = DockStyle.Top;
            this.label22.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label22.ForeColor = Color.White;
            this.label22.Location = new Point(0, 0);
            this.label22.Margin = new Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new Size(99, 13);
            this.label22.TabIndex = 162;
            this.label22.Text = "Lvl Loop Tracks";
            // 
            // lvlLoopTracks
            // 
            this.lvlLoopTracks.AllowUserToAddRows = false;
            this.lvlLoopTracks.AllowUserToDeleteRows = false;
            this.lvlLoopTracks.AllowUserToResizeColumns = false;
            this.lvlLoopTracks.AllowUserToResizeRows = false;
            this.lvlLoopTracks.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopTracks.BorderStyle = BorderStyle.None;
            this.lvlLoopTracks.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLoopTracks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle18.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle18.ForeColor = Color.White;
            dataGridViewCellStyle18.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.lvlLoopTracks.ColumnHeadersHeight = 20;
            this.lvlLoopTracks.Columns.AddRange(new DataGridViewColumn[] { this.LoopSample, this.BeatsPerLoop });
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle19.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle19.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle19.Format = "N2";
            dataGridViewCellStyle19.NullValue = null;
            dataGridViewCellStyle19.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.DefaultCellStyle = dataGridViewCellStyle19;
            this.lvlLoopTracks.Dock = DockStyle.Fill;
            this.lvlLoopTracks.EnableHeadersVisualStyles = false;
            this.lvlLoopTracks.GridColor = Color.Black;
            this.lvlLoopTracks.Location = new Point(0, 13);
            this.lvlLoopTracks.Margin = new Padding(4, 3, 4, 3);
            this.lvlLoopTracks.MultiSelect = false;
            this.lvlLoopTracks.Name = "lvlLoopTracks";
            this.lvlLoopTracks.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle20.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle20.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.False;
            this.lvlLoopTracks.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.lvlLoopTracks.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            this.lvlLoopTracks.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLoopTracks.RowTemplate.Height = 20;
            this.lvlLoopTracks.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLoopTracks.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.lvlLoopTracks.Size = new Size(452, 159);
            this.lvlLoopTracks.TabIndex = 161;
            // 
            // lvlLoopToolStrip
            // 
            this.lvlLoopToolStrip.AutoSize = false;
            this.lvlLoopToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlLoopToolStrip.Dock = DockStyle.Bottom;
            this.lvlLoopToolStrip.GripMargin = new Padding(0);
            this.lvlLoopToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlLoopToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlLoopToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlLoopAdd, this.btnLvlLoopDelete });
            this.lvlLoopToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlLoopToolStrip.Location = new Point(0, 172);
            this.lvlLoopToolStrip.Name = "lvlLoopToolStrip";
            this.lvlLoopToolStrip.Padding = new Padding(0);
            this.lvlLoopToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlLoopToolStrip.Size = new Size(452, 29);
            this.lvlLoopToolStrip.Stretch = true;
            this.lvlLoopToolStrip.TabIndex = 163;
            // 
            // btnLvlLoopAdd
            // 
            this.btnLvlLoopAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLoopAdd.Enabled = false;
            this.btnLvlLoopAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlLoopAdd.ForeColor = Color.White;
            this.btnLvlLoopAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlLoopAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlLoopAdd.Margin = new Padding(0);
            this.btnLvlLoopAdd.Name = "btnLvlLoopAdd";
            this.btnLvlLoopAdd.Size = new Size(24, 29);
            this.btnLvlLoopAdd.ToolTipText = "Add new loop track";
            // 
            // btnLvlLoopDelete
            // 
            this.btnLvlLoopDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlLoopDelete.Enabled = false;
            this.btnLvlLoopDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlLoopDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlLoopDelete.Margin = new Padding(0);
            this.btnLvlLoopDelete.Name = "btnLvlLoopDelete";
            this.btnLvlLoopDelete.Size = new Size(24, 29);
            this.btnLvlLoopDelete.ToolTipText = "Delete selected loop track";
            // 
            // lvlPathsToolStrip
            // 
            this.lvlPathsToolStrip.AutoSize = false;
            this.lvlPathsToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.lvlPathsToolStrip.Dock = DockStyle.Bottom;
            this.lvlPathsToolStrip.GripMargin = new Padding(0);
            this.lvlPathsToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.lvlPathsToolStrip.ImageScalingSize = new Size(20, 20);
            this.lvlPathsToolStrip.Items.AddRange(new ToolStripItem[] { this.btnLvlPathAdd, this.btnLvlPathDelete, this.btnLvlPathUp, this.btnLvlPathDown, this.btnLvlCopyTunnel, this.btnLvlPasteTunnel, this.btnLvlPathClear, this.chkTunnelCopy, this.btnLvlRandomTunnel });
            this.lvlPathsToolStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.lvlPathsToolStrip.Location = new Point(0, 172);
            this.lvlPathsToolStrip.Name = "lvlPathsToolStrip";
            this.lvlPathsToolStrip.Padding = new Padding(0);
            this.lvlPathsToolStrip.RenderMode = ToolStripRenderMode.System;
            this.lvlPathsToolStrip.Size = new Size(250, 29);
            this.lvlPathsToolStrip.Stretch = true;
            this.lvlPathsToolStrip.TabIndex = 162;
            // 
            // btnLvlPathAdd
            // 
            this.btnLvlPathAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathAdd.Enabled = false;
            this.btnLvlPathAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlPathAdd.ForeColor = Color.White;
            this.btnLvlPathAdd.Image = Properties.Resources.icon_plus;
            this.btnLvlPathAdd.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathAdd.Margin = new Padding(0);
            this.btnLvlPathAdd.Name = "btnLvlPathAdd";
            this.btnLvlPathAdd.Size = new Size(24, 29);
            this.btnLvlPathAdd.ToolTipText = "Add new path/tunnel";
            // 
            // btnLvlPathDelete
            // 
            this.btnLvlPathDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDelete.Enabled = false;
            this.btnLvlPathDelete.Image = Properties.Resources.icon_remove2;
            this.btnLvlPathDelete.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDelete.Margin = new Padding(0);
            this.btnLvlPathDelete.Name = "btnLvlPathDelete";
            this.btnLvlPathDelete.Size = new Size(24, 29);
            this.btnLvlPathDelete.ToolTipText = "Delete selected path";
            // 
            // btnLvlPathUp
            // 
            this.btnLvlPathUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathUp.Enabled = false;
            this.btnLvlPathUp.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnLvlPathUp.ForeColor = Color.White;
            this.btnLvlPathUp.Image = Properties.Resources.icon_arrowup2;
            this.btnLvlPathUp.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathUp.Margin = new Padding(0);
            this.btnLvlPathUp.Name = "btnLvlPathUp";
            this.btnLvlPathUp.Size = new Size(24, 29);
            this.btnLvlPathUp.ToolTipText = "Move selected tunnel up";
            // 
            // btnLvlPathDown
            // 
            this.btnLvlPathDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathDown.Enabled = false;
            this.btnLvlPathDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnLvlPathDown.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathDown.Margin = new Padding(0);
            this.btnLvlPathDown.Name = "btnLvlPathDown";
            this.btnLvlPathDown.Size = new Size(24, 29);
            this.btnLvlPathDown.ToolTipText = "Move selected tunnel down";
            // 
            // btnLvlCopyTunnel
            // 
            this.btnLvlCopyTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlCopyTunnel.Enabled = false;
            this.btnLvlCopyTunnel.Image = Properties.Resources.icon_copy2;
            this.btnLvlCopyTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlCopyTunnel.Name = "btnLvlCopyTunnel";
            this.btnLvlCopyTunnel.Size = new Size(24, 26);
            this.btnLvlCopyTunnel.ToolTipText = "Copy all paths/tunnels";
            // 
            // btnLvlPasteTunnel
            // 
            this.btnLvlPasteTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPasteTunnel.Enabled = false;
            this.btnLvlPasteTunnel.Image = Properties.Resources.icon_paste2;
            this.btnLvlPasteTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlPasteTunnel.Name = "btnLvlPasteTunnel";
            this.btnLvlPasteTunnel.Size = new Size(24, 26);
            this.btnLvlPasteTunnel.ToolTipText = "Paste copied paths/tunnels";
            // 
            // btnLvlPathClear
            // 
            this.btnLvlPathClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlPathClear.Enabled = false;
            this.btnLvlPathClear.Image = Properties.Resources.icon_erase;
            this.btnLvlPathClear.ImageTransparentColor = Color.Magenta;
            this.btnLvlPathClear.Name = "btnLvlPathClear";
            this.btnLvlPathClear.Size = new Size(24, 26);
            this.btnLvlPathClear.Text = "toolStripButton2";
            this.btnLvlPathClear.ToolTipText = "Clear all tunnels";
            // 
            // chkTunnelCopy
            // 
            this.chkTunnelCopy.CheckOnClick = true;
            this.chkTunnelCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.chkTunnelCopy.Image = Properties.Resources.icon_sling;
            this.chkTunnelCopy.ImageTransparentColor = Color.Magenta;
            this.chkTunnelCopy.Name = "chkTunnelCopy";
            this.chkTunnelCopy.Size = new Size(24, 26);
            this.chkTunnelCopy.ToolTipText = "When enabled, new leafs added will copy the paths\r\nof the previous leaf.";
            // 
            // btnLvlRandomTunnel
            // 
            this.btnLvlRandomTunnel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLvlRandomTunnel.Enabled = false;
            this.btnLvlRandomTunnel.Image = Properties.Resources.icon_random;
            this.btnLvlRandomTunnel.ImageTransparentColor = Color.Magenta;
            this.btnLvlRandomTunnel.Name = "btnLvlRandomTunnel";
            this.btnLvlRandomTunnel.Size = new Size(24, 26);
            this.btnLvlRandomTunnel.ToolTipText = "Click to add a random tunnel";
            // 
            // lvlLeafPaths
            // 
            this.lvlLeafPaths.AllowUserToAddRows = false;
            this.lvlLeafPaths.AllowUserToDeleteRows = false;
            this.lvlLeafPaths.AllowUserToResizeColumns = false;
            this.lvlLeafPaths.AllowUserToResizeRows = false;
            this.lvlLeafPaths.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.lvlLeafPaths.BorderStyle = BorderStyle.None;
            this.lvlLeafPaths.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.lvlLeafPaths.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle15.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle15.ForeColor = Color.White;
            dataGridViewCellStyle15.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle15.SelectionForeColor = Color.White;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.lvlLeafPaths.ColumnHeadersHeight = 20;
            this.lvlLeafPaths.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle16.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle16.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            dataGridViewCellStyle16.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.DefaultCellStyle = dataGridViewCellStyle16;
            this.lvlLeafPaths.Dock = DockStyle.Fill;
            this.lvlLeafPaths.EnableHeadersVisualStyles = false;
            this.lvlLeafPaths.GridColor = Color.Black;
            this.lvlLeafPaths.Location = new Point(0, 13);
            this.lvlLeafPaths.Margin = new Padding(4, 3, 4, 3);
            this.lvlLeafPaths.Name = "lvlLeafPaths";
            this.lvlLeafPaths.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle17.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle17.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = DataGridViewTriState.False;
            this.lvlLeafPaths.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.lvlLeafPaths.RowHeadersVisible = false;
            this.lvlLeafPaths.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            this.lvlLeafPaths.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.lvlLeafPaths.RowTemplate.Height = 20;
            this.lvlLeafPaths.RowTemplate.Resizable = DataGridViewTriState.False;
            this.lvlLeafPaths.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.lvlLeafPaths.Size = new Size(250, 159);
            this.lvlLeafPaths.TabIndex = 160;
            // 
            // lblLvlTunnels
            // 
            this.lblLvlTunnels.AutoSize = true;
            this.lblLvlTunnels.BackColor = Color.FromArgb(10, 10, 10);
            this.lblLvlTunnels.Dock = DockStyle.Top;
            this.lblLvlTunnels.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblLvlTunnels.ForeColor = Color.White;
            this.lblLvlTunnels.Location = new Point(0, 0);
            this.lblLvlTunnels.Margin = new Padding(4, 0, 4, 0);
            this.lblLvlTunnels.Name = "lblLvlTunnels";
            this.lblLvlTunnels.Size = new Size(90, 13);
            this.lblLvlTunnels.TabIndex = 161;
            this.lblLvlTunnels.Text = "Paths/Tunnels";
            this.toolTip1.SetToolTip(this.lblLvlTunnels, "Unique per leaf");
            // 
            // LoopSample
            // 
            this.LoopSample.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.LoopSample.HeaderText = "Sample Name";
            this.LoopSample.MaxDropDownItems = 20;
            this.LoopSample.Name = "LoopSample";
            // 
            // BeatsPerLoop
            // 
            this.BeatsPerLoop.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.BeatsPerLoop.HeaderText = "Beats";
            this.BeatsPerLoop.Name = "BeatsPerLoop";
            this.BeatsPerLoop.Width = 62;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = Color.FromArgb(55, 55, 55);
            this.splitContainer3.Dock = DockStyle.Fill;
            this.splitContainer3.Location = new Point(0, 0);
            this.splitContainer3.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lvlLeafPaths);
            this.splitContainer3.Panel1.Controls.Add(this.lvlPathsToolStrip);
            this.splitContainer3.Panel1.Controls.Add(this.lblLvlTunnels);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.AutoScroll = true;
            this.splitContainer3.Panel2.Controls.Add(this.lvlLoopTracks);
            this.splitContainer3.Panel2.Controls.Add(this.label22);
            this.splitContainer3.Panel2.Controls.Add(this.lvlLoopToolStrip);
            this.splitContainer3.Size = new Size(707, 201);
            this.splitContainer3.SplitterDistance = 250;
            this.splitContainer3.SplitterWidth = 5;
            this.splitContainer3.TabIndex = 163;
            // 
            // Form_LvlEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(707, 506);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.ForeColor = Color.FromArgb(150, 150, 255);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_LvlEditor";
            this.Text = "Lvl Editor";
            this.lvlToolStrip.ResumeLayout(false);
            this.lvlToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafList).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.lvlLoopTracks).EndInit();
            this.lvlLoopToolStrip.ResumeLayout(false);
            this.lvlLoopToolStrip.PerformLayout();
            this.lvlPathsToolStrip.ResumeLayout(false);
            this.lvlPathsToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.lvlLeafPaths).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer3).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip lvlToolStrip;
        private System.Windows.Forms.ToolStripButton btnLvlLeafAdd;
        private System.Windows.Forms.ToolStripButton btnLvlLeafDelete;
        private System.Windows.Forms.ToolStripButton btnLvlLeafUp;
        private System.Windows.Forms.ToolStripButton btnLvlLeafDown;
        private System.Windows.Forms.ToolStripButton btnLvlLeafCopy;
        private System.Windows.Forms.ToolStripButton btnLvlLeafPaste;
        private System.Windows.Forms.ToolStripButton btnLvlLeafRandom;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.DataGridView lvlLeafList;
        private DataGridViewImageColumn lvlfiletype;
        private DataGridViewTextBoxColumn Leaf;
        private DataGridViewTextBoxColumn Beats;
        private SplitContainer splitContainer1;
        private Label lblMasterlvllistHelp;
        public PropertyGrid propertyGridLvl;
        private SplitContainer splitContainer2;
        private DataGridView lvlLoopTracks;
        private Label label22;
        private ToolStrip lvlLoopToolStrip;
        private ToolStripButton btnLvlLoopAdd;
        private ToolStripButton btnLvlLoopDelete;
        private DataGridView lvlLeafPaths;
        private ToolStrip lvlPathsToolStrip;
        private ToolStripButton btnLvlPathAdd;
        private ToolStripButton btnLvlPathDelete;
        private ToolStripButton btnLvlPathUp;
        private ToolStripButton btnLvlPathDown;
        private ToolStripButton btnLvlCopyTunnel;
        private ToolStripButton btnLvlPasteTunnel;
        private ToolStripButton btnLvlPathClear;
        private ToolStripButton chkTunnelCopy;
        private ToolStripButton btnLvlRandomTunnel;
        private Label lblLvlTunnels;
        private DataGridViewComboBoxColumn LoopSample;
        private DataGridViewTextBoxColumn BeatsPerLoop;
        private SplitContainer splitContainer3;
    }
}