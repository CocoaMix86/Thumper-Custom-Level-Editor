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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MasterEditor));
            this.masterLvlList = new DataGridView();
            this.SublevelNum = new DataGridViewTextBoxColumn();
            this.masterfiletype = new DataGridViewImageColumn();
            this.masterLvl = new DataGridViewTextBoxColumn();
            this.Runtime = new DataGridViewTextBoxColumn();
            this.masterToolStrip = new ToolStrip();
            this.btnMasterLvlAdd = new ToolStripButton();
            this.btnMasterLvlDelete = new ToolStripButton();
            this.btnMasterLvlUp = new ToolStripButton();
            this.btnMasterLvlDown = new ToolStripButton();
            this.btnMasterLvlCopy = new ToolStripButton();
            this.btnMasterLvlPaste = new ToolStripButton();
            this.label30 = new Label();
            this.lblMasterlvllistHelp = new Label();
            this.toolTip1 = new ToolTip(this.components);
            this.propertyGridMaster = new PropertyGrid();
            this.lblConfigColorHelp = new Label();
            this.splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)this.masterLvlList).BeginInit();
            this.masterToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterLvlList
            // 
            this.masterLvlList.AllowDrop = true;
            this.masterLvlList.AllowUserToAddRows = false;
            this.masterLvlList.AllowUserToDeleteRows = false;
            this.masterLvlList.AllowUserToResizeRows = false;
            this.masterLvlList.BackgroundColor = Color.FromArgb(10, 10, 10);
            this.masterLvlList.BorderStyle = BorderStyle.None;
            this.masterLvlList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.masterLvlList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.masterLvlList.ColumnHeadersHeight = 20;
            this.masterLvlList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.masterLvlList.Columns.AddRange(new DataGridViewColumn[] { this.SublevelNum, this.masterfiletype, this.masterLvl, this.Runtime });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.DefaultCellStyle = dataGridViewCellStyle3;
            this.masterLvlList.Dock = DockStyle.Fill;
            this.masterLvlList.EnableHeadersVisualStyles = false;
            this.masterLvlList.GridColor = Color.Black;
            this.masterLvlList.Location = new Point(0, 13);
            this.masterLvlList.Margin = new Padding(4, 3, 4, 3);
            this.masterLvlList.Name = "masterLvlList";
            this.masterLvlList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle4.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.masterLvlList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.masterLvlList.RowHeadersVisible = false;
            this.masterLvlList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.masterLvlList.RowTemplate.Height = 20;
            this.masterLvlList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.masterLvlList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.masterLvlList.Size = new Size(400, 477);
            this.masterLvlList.TabIndex = 79;
            this.masterLvlList.Tag = "editorpaneldgv";
            this.masterLvlList.CellClick += this.masterLvlList_CellClick;
            this.masterLvlList.CellDoubleClick += this.masterLvlList_CellDoubleClick;
            this.masterLvlList.DragDrop += this.masterLvlList_DragDrop;
            this.masterLvlList.DragEnter += this.masterLvlList_DragEnter;
            // 
            // SublevelNum
            // 
            this.SublevelNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SublevelNum.HeaderText = "";
            this.SublevelNum.Name = "SublevelNum";
            this.SublevelNum.ReadOnly = true;
            this.SublevelNum.Resizable = DataGridViewTriState.False;
            this.SublevelNum.Width = 18;
            // 
            // masterfiletype
            // 
            this.masterfiletype.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.masterfiletype.HeaderText = "";
            this.masterfiletype.Name = "masterfiletype";
            this.masterfiletype.ReadOnly = true;
            this.masterfiletype.Width = 5;
            // 
            // masterLvl
            // 
            this.masterLvl.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.masterLvl.FillWeight = 50F;
            this.masterLvl.HeaderText = "Sublevel";
            this.masterLvl.Name = "masterLvl";
            this.masterLvl.ReadOnly = true;
            this.masterLvl.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Runtime
            // 
            this.Runtime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Runtime.DefaultCellStyle = dataGridViewCellStyle2;
            this.Runtime.FillWeight = 50F;
            this.Runtime.HeaderText = "Runtime";
            this.Runtime.Name = "Runtime";
            this.Runtime.ReadOnly = true;
            this.Runtime.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // masterToolStrip
            // 
            this.masterToolStrip.AutoSize = false;
            this.masterToolStrip.BackColor = Color.FromArgb(10, 10, 10);
            this.masterToolStrip.Dock = DockStyle.Bottom;
            this.masterToolStrip.GripMargin = new Padding(0);
            this.masterToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.masterToolStrip.ImageScalingSize = new Size(20, 20);
            this.masterToolStrip.Items.AddRange(new ToolStripItem[] { this.btnMasterLvlAdd, this.btnMasterLvlDelete, this.btnMasterLvlUp, this.btnMasterLvlDown, this.btnMasterLvlCopy, this.btnMasterLvlPaste });
            this.masterToolStrip.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.masterToolStrip.Location = new Point(0, 490);
            this.masterToolStrip.Name = "masterToolStrip";
            this.masterToolStrip.Padding = new Padding(0);
            this.masterToolStrip.RenderMode = ToolStripRenderMode.System;
            this.masterToolStrip.Size = new Size(400, 29);
            this.masterToolStrip.Stretch = true;
            this.masterToolStrip.TabIndex = 138;
            // 
            // btnMasterLvlAdd
            // 
            this.btnMasterLvlAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMasterLvlAdd.ForeColor = Color.White;
            this.btnMasterLvlAdd.Image = Properties.Resources.icon_plus;
            this.btnMasterLvlAdd.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlAdd.Margin = new Padding(0);
            this.btnMasterLvlAdd.Name = "btnMasterLvlAdd";
            this.btnMasterLvlAdd.Size = new Size(24, 24);
            this.btnMasterLvlAdd.ToolTipText = "Add new sublevel to the list";
            this.btnMasterLvlAdd.Click += this.btnMasterLvlAdd_Click;
            // 
            // btnMasterLvlDelete
            // 
            this.btnMasterLvlDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDelete.Enabled = false;
            this.btnMasterLvlDelete.Image = Properties.Resources.icon_remove2;
            this.btnMasterLvlDelete.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlDelete.Margin = new Padding(0);
            this.btnMasterLvlDelete.Name = "btnMasterLvlDelete";
            this.btnMasterLvlDelete.Size = new Size(24, 24);
            this.btnMasterLvlDelete.ToolTipText = "Delete selected sublevel from this list";
            this.btnMasterLvlDelete.Click += this.btnMasterLvlDelete_Click;
            // 
            // btnMasterLvlUp
            // 
            this.btnMasterLvlUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlUp.Enabled = false;
            this.btnMasterLvlUp.Image = Properties.Resources.icon_arrowup2;
            this.btnMasterLvlUp.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlUp.Margin = new Padding(0);
            this.btnMasterLvlUp.Name = "btnMasterLvlUp";
            this.btnMasterLvlUp.Size = new Size(24, 24);
            this.btnMasterLvlUp.ToolTipText = "Move selected sublevel up";
            this.btnMasterLvlUp.Click += this.btnMasterLvlUp_Click;
            // 
            // btnMasterLvlDown
            // 
            this.btnMasterLvlDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlDown.Enabled = false;
            this.btnMasterLvlDown.Image = Properties.Resources.icon_arrowdown2;
            this.btnMasterLvlDown.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlDown.Margin = new Padding(0);
            this.btnMasterLvlDown.Name = "btnMasterLvlDown";
            this.btnMasterLvlDown.Size = new Size(24, 24);
            this.btnMasterLvlDown.ToolTipText = "Move selected sublevel down";
            this.btnMasterLvlDown.Click += this.btnMasterLvlDown_Click;
            // 
            // btnMasterLvlCopy
            // 
            this.btnMasterLvlCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlCopy.Enabled = false;
            this.btnMasterLvlCopy.Image = Properties.Resources.icon_copy2;
            this.btnMasterLvlCopy.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlCopy.Margin = new Padding(0);
            this.btnMasterLvlCopy.Name = "btnMasterLvlCopy";
            this.btnMasterLvlCopy.Size = new Size(24, 24);
            this.btnMasterLvlCopy.ToolTipText = "Copy selected sublevel";
            // 
            // btnMasterLvlPaste
            // 
            this.btnMasterLvlPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnMasterLvlPaste.Enabled = false;
            this.btnMasterLvlPaste.Image = Properties.Resources.icon_paste2;
            this.btnMasterLvlPaste.ImageTransparentColor = Color.Magenta;
            this.btnMasterLvlPaste.Name = "btnMasterLvlPaste";
            this.btnMasterLvlPaste.Size = new Size(24, 24);
            this.btnMasterLvlPaste.ToolTipText = "Paste the copied sublevel";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = Color.FromArgb(10, 10, 10);
            this.label30.Dock = DockStyle.Top;
            this.label30.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label30.ForeColor = Color.White;
            this.label30.Location = new Point(0, 0);
            this.label30.Margin = new Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new Size(91, 13);
            this.label30.TabIndex = 94;
            this.label30.Text = "Sublevel Order";
            // 
            // lblMasterlvllistHelp
            // 
            this.lblMasterlvllistHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblMasterlvllistHelp.AutoSize = true;
            this.lblMasterlvllistHelp.BackColor = Color.Transparent;
            this.lblMasterlvllistHelp.Cursor = Cursors.Help;
            this.lblMasterlvllistHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblMasterlvllistHelp.ForeColor = Color.DodgerBlue;
            this.lblMasterlvllistHelp.Location = new Point(372, -3);
            this.lblMasterlvllistHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblMasterlvllistHelp.Name = "lblMasterlvllistHelp";
            this.lblMasterlvllistHelp.Size = new Size(15, 16);
            this.lblMasterlvllistHelp.TabIndex = 95;
            this.lblMasterlvllistHelp.Text = "?";
            // 
            // propertyGridMaster
            // 
            this.propertyGridMaster.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.CategoryForeColor = Color.White;
            this.propertyGridMaster.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridMaster.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridMaster.Dock = DockStyle.Fill;
            this.propertyGridMaster.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridMaster.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridMaster.HelpForeColor = Color.White;
            this.propertyGridMaster.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridMaster.Location = new Point(0, 0);
            this.propertyGridMaster.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridMaster.Name = "propertyGridMaster";
            this.propertyGridMaster.PropertySort = PropertySort.Categorized;
            this.propertyGridMaster.RightToLeft = RightToLeft.No;
            this.propertyGridMaster.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridMaster.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridMaster.Size = new Size(248, 519);
            this.propertyGridMaster.TabIndex = 147;
            this.propertyGridMaster.ToolbarVisible = false;
            this.propertyGridMaster.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridMaster.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridMaster.ViewForeColor = Color.White;
            // 
            // lblConfigColorHelp
            // 
            this.lblConfigColorHelp.AutoSize = true;
            this.lblConfigColorHelp.BackColor = Color.Transparent;
            this.lblConfigColorHelp.Cursor = Cursors.Help;
            this.lblConfigColorHelp.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            this.lblConfigColorHelp.ForeColor = Color.DodgerBlue;
            this.lblConfigColorHelp.Location = new Point(183, -2);
            this.lblConfigColorHelp.Margin = new Padding(4, 0, 4, 0);
            this.lblConfigColorHelp.Name = "lblConfigColorHelp";
            this.lblConfigColorHelp.Size = new Size(15, 16);
            this.lblConfigColorHelp.TabIndex = 112;
            this.lblConfigColorHelp.Text = "?";
            this.lblConfigColorHelp.Click += this.lblConfigColorHelp_Click;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.masterLvlList);
            this.splitContainer1.Panel1.Controls.Add(this.masterToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.lblMasterlvllistHelp);
            this.splitContainer1.Panel1.Controls.Add(this.lblConfigColorHelp);
            this.splitContainer1.Panel1.Controls.Add(this.label30);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridMaster);
            this.splitContainer1.Size = new Size(653, 519);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 49;
            // 
            // Form_MasterEditor
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(55, 55, 55);
            this.ClientSize = new Size(653, 519);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.ForeColor = Color.FromArgb(150, 150, 255);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_MasterEditor";
            this.Text = "Master Editor";
            ((System.ComponentModel.ISupportInitialize)this.masterLvlList).EndInit();
            this.masterToolStrip.ResumeLayout(false);
            this.masterToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.DataGridView masterLvlList;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lblMasterlvllistHelp;
        private System.Windows.Forms.ToolStrip masterToolStrip;
        private System.Windows.Forms.ToolStripButton btnMasterLvlAdd;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDelete;
        private System.Windows.Forms.ToolStripButton btnMasterLvlUp;
        private System.Windows.Forms.ToolStripButton btnMasterLvlDown;
        private System.Windows.Forms.ToolStripButton btnMasterLvlCopy;
        private System.Windows.Forms.ToolStripButton btnMasterLvlPaste;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblConfigColorHelp;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public PropertyGrid propertyGridMaster;
        private DataGridViewTextBoxColumn SublevelNum;
        private DataGridViewImageColumn masterfiletype;
        private DataGridViewTextBoxColumn masterLvl;
        private DataGridViewTextBoxColumn Runtime;
    }
}