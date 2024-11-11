namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_MasterEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MasterEditor));
            this.panelMaster = new System.Windows.Forms.Panel();
            this.masterLvlList = new System.Windows.Forms.DataGridView();
            this.masterfiletype = new System.Windows.Forms.DataGridViewImageColumn();
            this.masterLvl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterCheckpoint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterPlayplus = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterIsolate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnMasterLvlAdd = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlDelete = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlUp = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlDown = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlCopy = new System.Windows.Forms.ToolStripButton();
            this.btnMasterLvlPaste = new System.Windows.Forms.ToolStripButton();
            this.label30 = new System.Windows.Forms.Label();
            this.lblMasterlvllistHelp = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblMasterRuntime = new System.Windows.Forms.Label();
            this.lblMAsterRuntimeBeats = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.btnMasterRuntime = new System.Windows.Forms.Button();
            this.label37 = new System.Windows.Forms.Label();
            this.dropMasterLvlRest = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.btnMasterOpenRest = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.lblConfigColorHelp = new System.Windows.Forms.Label();
            this.panelMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterLvlList)).BeginInit();
            this.masterToolStrip.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMaster
            // 
            this.panelMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panelMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMaster.Controls.Add(this.masterLvlList);
            this.panelMaster.Controls.Add(this.masterToolStrip);
            this.panelMaster.Controls.Add(this.label30);
            this.panelMaster.Controls.Add(this.lblMasterlvllistHelp);
            this.panelMaster.Controls.Add(this.panel5);
            this.panelMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMaster.Location = new System.Drawing.Point(0, 0);
            this.panelMaster.MinimumSize = new System.Drawing.Size(60, 60);
            this.panelMaster.Name = "panelMaster";
            this.panelMaster.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelMaster.Size = new System.Drawing.Size(800, 450);
            this.panelMaster.TabIndex = 48;
            this.panelMaster.Tag = "editorpanel";
            // 
            // masterLvlList
            // 
            this.masterLvlList.AllowUserToAddRows = false;
            this.masterLvlList.AllowUserToDeleteRows = false;
            this.masterLvlList.AllowUserToResizeColumns = false;
            this.masterLvlList.AllowUserToResizeRows = false;
            this.masterLvlList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.masterLvlList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.masterLvlList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.masterLvlList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.masterLvlList.ColumnHeadersHeight = 20;
            this.masterLvlList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.masterLvlList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.masterfiletype,
            this.masterLvl,
            this.masterCheckpoint,
            this.masterPlayplus,
            this.masterIsolate});
            this.masterLvlList.Cursor = System.Windows.Forms.Cursors.Arrow;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.NullValue = null;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.DefaultCellStyle = dataGridViewCellStyle14;
            this.masterLvlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterLvlList.EnableHeadersVisualStyles = false;
            this.masterLvlList.GridColor = System.Drawing.Color.Black;
            this.masterLvlList.Location = new System.Drawing.Point(0, 13);
            this.masterLvlList.Name = "masterLvlList";
            this.masterLvlList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.masterLvlList.RowHeadersVisible = false;
            this.masterLvlList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.masterLvlList.RowTemplate.Height = 20;
            this.masterLvlList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.masterLvlList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.masterLvlList.Size = new System.Drawing.Size(507, 409);
            this.masterLvlList.TabIndex = 79;
            this.masterLvlList.Tag = "editorpaneldgv";
            // 
            // masterfiletype
            // 
            this.masterfiletype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.masterfiletype.HeaderText = "";
            this.masterfiletype.Name = "masterfiletype";
            this.masterfiletype.ReadOnly = true;
            this.masterfiletype.Width = 5;
            // 
            // masterLvl
            // 
            this.masterLvl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.masterLvl.HeaderText = "Sublevel";
            this.masterLvl.Name = "masterLvl";
            this.masterLvl.ReadOnly = true;
            this.masterLvl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // masterCheckpoint
            // 
            this.masterCheckpoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterCheckpoint.HeaderText = "Chkp.";
            this.masterCheckpoint.Name = "masterCheckpoint";
            this.masterCheckpoint.Width = 44;
            // 
            // masterPlayplus
            // 
            this.masterPlayplus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterPlayplus.HeaderText = "P+";
            this.masterPlayplus.Name = "masterPlayplus";
            this.masterPlayplus.Width = 25;
            // 
            // masterIsolate
            // 
            this.masterIsolate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.masterIsolate.HeaderText = "Iso.";
            this.masterIsolate.Name = "masterIsolate";
            this.masterIsolate.Width = 32;
            // 
            // masterToolStrip
            // 
            this.masterToolStrip.AutoSize = false;
            this.masterToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.masterToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.masterToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.masterToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.masterToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.masterToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMasterLvlAdd,
            this.btnMasterLvlDelete,
            this.btnMasterLvlUp,
            this.btnMasterLvlDown,
            this.btnMasterLvlCopy,
            this.btnMasterLvlPaste});
            this.masterToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.masterToolStrip.Location = new System.Drawing.Point(0, 422);
            this.masterToolStrip.Name = "masterToolStrip";
            this.masterToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.masterToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.masterToolStrip.Size = new System.Drawing.Size(507, 25);
            this.masterToolStrip.Stretch = true;
            this.masterToolStrip.TabIndex = 138;
            // 
            // btnMasterLvlAdd
            // 
            this.btnMasterLvlAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlAdd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterLvlAdd.ForeColor = System.Drawing.Color.White;
            this.btnMasterLvlAdd.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_plus;
            this.btnMasterLvlAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlAdd.Name = "btnMasterLvlAdd";
            this.btnMasterLvlAdd.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlAdd.ToolTipText = "Add new sublevel to the list";
            // 
            // btnMasterLvlDelete
            // 
            this.btnMasterLvlDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDelete.Enabled = false;
            this.btnMasterLvlDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.btnMasterLvlDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlDelete.Name = "btnMasterLvlDelete";
            this.btnMasterLvlDelete.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlDelete.ToolTipText = "Delete selected sublevel from this list";
            // 
            // btnMasterLvlUp
            // 
            this.btnMasterLvlUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlUp.Enabled = false;
            this.btnMasterLvlUp.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowup2;
            this.btnMasterLvlUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlUp.Name = "btnMasterLvlUp";
            this.btnMasterLvlUp.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlUp.ToolTipText = "Move selected sublevel up";
            // 
            // btnMasterLvlDown
            // 
            this.btnMasterLvlDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDown.Enabled = false;
            this.btnMasterLvlDown.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_arrowdown2;
            this.btnMasterLvlDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlDown.Name = "btnMasterLvlDown";
            this.btnMasterLvlDown.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlDown.ToolTipText = "Move selected sublevel down";
            // 
            // btnMasterLvlCopy
            // 
            this.btnMasterLvlCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlCopy.Enabled = false;
            this.btnMasterLvlCopy.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.btnMasterLvlCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlCopy.Margin = new System.Windows.Forms.Padding(0);
            this.btnMasterLvlCopy.Name = "btnMasterLvlCopy";
            this.btnMasterLvlCopy.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlCopy.ToolTipText = "Copy selected sublevel";
            // 
            // btnMasterLvlPaste
            // 
            this.btnMasterLvlPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlPaste.Enabled = false;
            this.btnMasterLvlPaste.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_paste2;
            this.btnMasterLvlPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMasterLvlPaste.Name = "btnMasterLvlPaste";
            this.btnMasterLvlPaste.Size = new System.Drawing.Size(24, 24);
            this.btnMasterLvlPaste.ToolTipText = "Paste the copied sublevel";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.label30.Dock = System.Windows.Forms.DockStyle.Top;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.White;
            this.label30.Location = new System.Drawing.Point(0, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 13);
            this.label30.TabIndex = 94;
            this.label30.Text = "Lvl/Gate List";
            // 
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblMasterlvllistHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterlvllistHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new System.Drawing.Point(492, 0);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new System.Drawing.Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // panel5
            // 
            this.panel5.AutoScroll = true;
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.propertyGrid1);
            this.panel5.Controls.Add(this.lblMasterRuntime);
            this.panel5.Controls.Add(this.lblMAsterRuntimeBeats);
            this.panel5.Controls.Add(this.label21);
            this.panel5.Controls.Add(this.btnMasterRuntime);
            this.panel5.Controls.Add(this.lblConfigColorHelp);
            this.panel5.Controls.Add(this.label37);
            this.panel5.Controls.Add(this.dropMasterLvlRest);
            this.panel5.Controls.Add(this.label35);
            this.panel5.Controls.Add(this.btnMasterOpenRest);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(507, 0);
            this.panel5.Name = "panel5";
            this.panel5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel5.Size = new System.Drawing.Size(291, 447);
            this.panel5.TabIndex = 147;
            // 
            // lblMasterRuntime
            // 
            this.lblMasterRuntime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMasterRuntime.AutoSize = true;
            this.lblMasterRuntime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterRuntime.ForeColor = System.Drawing.Color.White;
            this.lblMasterRuntime.Location = new System.Drawing.Point(150, 360);
            this.lblMasterRuntime.Name = "lblMasterRuntime";
            this.lblMasterRuntime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMasterRuntime.Size = new System.Drawing.Size(38, 15);
            this.lblMasterRuntime.TabIndex = 146;
            this.lblMasterRuntime.Text = "Time:";
            // 
            // lblMAsterRuntimeBeats
            // 
            this.lblMAsterRuntimeBeats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMAsterRuntimeBeats.AutoSize = true;
            this.lblMAsterRuntimeBeats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMAsterRuntimeBeats.ForeColor = System.Drawing.Color.White;
            this.lblMAsterRuntimeBeats.Location = new System.Drawing.Point(150, 345);
            this.lblMAsterRuntimeBeats.Name = "lblMAsterRuntimeBeats";
            this.lblMAsterRuntimeBeats.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMAsterRuntimeBeats.Size = new System.Drawing.Size(41, 15);
            this.lblMAsterRuntimeBeats.TabIndex = 145;
            this.lblMAsterRuntimeBeats.Text = "Beats:";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Silver;
            this.label21.Location = new System.Drawing.Point(159, 331);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(78, 15);
            this.label21.TabIndex = 144;
            this.label21.Text = "══Runtime══";
            // 
            // btnMasterRuntime
            // 
            this.btnMasterRuntime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterRuntime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterRuntime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMasterRuntime.ForeColor = System.Drawing.Color.Green;
            this.btnMasterRuntime.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterRuntime.Image")));
            this.btnMasterRuntime.Location = new System.Drawing.Point(129, 351);
            this.btnMasterRuntime.Name = "btnMasterRuntime";
            this.btnMasterRuntime.Padding = new System.Windows.Forms.Padding(0, 0, 2, 1);
            this.btnMasterRuntime.Size = new System.Drawing.Size(20, 20);
            this.btnMasterRuntime.TabIndex = 143;
            this.btnMasterRuntime.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Silver;
            this.label37.Location = new System.Drawing.Point(137, 384);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(123, 15);
            this.label37.TabIndex = 116;
            this.label37.Text = "══Sublevel Options══";
            // 
            // dropMasterLvlRest
            // 
            this.dropMasterLvlRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropMasterLvlRest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.dropMasterLvlRest.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.dropMasterLvlRest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropMasterLvlRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropMasterLvlRest.ForeColor = System.Drawing.Color.White;
            this.dropMasterLvlRest.FormattingEnabled = true;
            this.dropMasterLvlRest.Items.AddRange(new object[] {
            "<none>"});
            this.dropMasterLvlRest.Location = new System.Drawing.Point(131, 417);
            this.dropMasterLvlRest.Name = "dropMasterLvlRest";
            this.dropMasterLvlRest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dropMasterLvlRest.Size = new System.Drawing.Size(131, 21);
            this.dropMasterLvlRest.TabIndex = 105;
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(173, 401);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(50, 15);
            this.label35.TabIndex = 104;
            this.label35.Text = "Rest Lvl";
            // 
            // btnMasterOpenRest
            // 
            this.btnMasterOpenRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMasterOpenRest.BackColor = System.Drawing.Color.Gray;
            this.btnMasterOpenRest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMasterOpenRest.Enabled = false;
            this.btnMasterOpenRest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMasterOpenRest.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMasterOpenRest.ForeColor = System.Drawing.Color.Black;
            this.btnMasterOpenRest.Image = ((System.Drawing.Image)(resources.GetObject("btnMasterOpenRest.Image")));
            this.btnMasterOpenRest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMasterOpenRest.Location = new System.Drawing.Point(260, 416);
            this.btnMasterOpenRest.Name = "btnMasterOpenRest";
            this.btnMasterOpenRest.Size = new System.Drawing.Size(23, 23);
            this.btnMasterOpenRest.TabIndex = 118;
            this.btnMasterOpenRest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMasterOpenRest.UseVisualStyleBackColor = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.propertyGrid1.CategoryForeColor = System.Drawing.Color.White;
            this.propertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.propertyGrid1.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.propertyGrid1.HelpBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.propertyGrid1.HelpForeColor = System.Drawing.Color.White;
            this.propertyGrid1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.propertyGrid1.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(96)))), ((int)(((byte)(232)))));
            this.propertyGrid1.SelectedItemWithFocusForeColor = System.Drawing.Color.White;
            this.propertyGrid1.Size = new System.Drawing.Size(291, 328);
            this.propertyGrid1.TabIndex = 147;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.propertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.propertyGrid1.ViewForeColor = System.Drawing.Color.White;
            // 
            // lblConfigColorHelp
            // 
            this.lblConfigColorHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigColorHelp.AutoSize = true;
            this.lblConfigColorHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblConfigColorHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.lblConfigColorHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigColorHelp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblConfigColorHelp.Location = new System.Drawing.Point(260, 331);
            this.lblConfigColorHelp.Name = "lblConfigColorHelp";
            this.lblConfigColorHelp.Size = new System.Drawing.Size(15, 16);
            this.lblConfigColorHelp.TabIndex = 112;
            this.lblConfigColorHelp.Text = "?";
            this.lblConfigColorHelp.Click += new System.EventHandler(this.lblConfigColorHelp_Click);
            // 
            // Form_MasterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelMaster);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_MasterEditor";
            this.Text = "Master Editor";
            this.panelMaster.ResumeLayout(false);
            this.panelMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterLvlList)).EndInit();
            this.masterToolStrip.ResumeLayout(false);
            this.masterToolStrip.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMaster;
        private System.Windows.Forms.DataGridView masterLvlList;
        private System.Windows.Forms.DataGridViewImageColumn masterfiletype;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterLvl;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterCheckpoint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterPlayplus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn masterIsolate;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lblMasterlvllistHelp;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblMasterRuntime;
        private System.Windows.Forms.Label lblMAsterRuntimeBeats;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnMasterRuntime;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox dropMasterLvlRest;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Button btnMasterOpenRest;
        private System.Windows.Forms.ToolStrip masterToolStrip;
        private System.Windows.Forms.ToolStripButton btnMasterLvlAdd;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDelete;
        private System.Windows.Forms.ToolStripButton btnMasterLvlUp;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDown;
        private System.Windows.Forms.ToolStripButton btnMasterLvlCopy;
        private System.Windows.Forms.ToolStripButton btnMasterLvlPaste;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Label lblConfigColorHelp;
    }
}