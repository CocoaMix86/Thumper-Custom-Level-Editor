namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_ProjectExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ProjectExplorer));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.toolstripExplorer = new System.Windows.Forms.ToolStrip();
            this.btnFilter = new System.Windows.Forms.ToolStripSplitButton();
            this.contextMenuFilters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.filterLeaf = new System.Windows.Forms.ToolStripMenuItem();
            this.filterLvl = new System.Windows.Forms.ToolStripMenuItem();
            this.filterGate = new System.Windows.Forms.ToolStripMenuItem();
            this.filterMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.filterSample = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCollapse = new System.Windows.Forms.ToolStripButton();
            this.btnExpand = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnEditProjectProperties = new System.Windows.Forms.ToolStripButton();
            this.toolstripExplorer.SuspendLayout();
            this.contextMenuFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageKey = "xfm";
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 20;
            this.treeView1.LabelEdit = true;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(0, 45);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageKey = "xfm";
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(357, 405);
            this.treeView1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, "LEVEL DETAILS");
            this.imageList1.Images.SetKeyName(2, "leaf");
            this.imageList1.Images.SetKeyName(3, "lvl");
            this.imageList1.Images.SetKeyName(4, "gate");
            this.imageList1.Images.SetKeyName(5, "master");
            this.imageList1.Images.SetKeyName(6, "samp");
            this.imageList1.Images.SetKeyName(7, "xfm");
            this.imageList1.Images.SetKeyName(8, "project");
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(0, 25);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(357, 20);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Text = "Search Project Explorer (Ctrl+;)";
            // 
            // toolstripExplorer
            // 
            this.toolstripExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.toolstripExplorer.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolstripExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFilter,
            this.btnCollapse,
            this.btnExpand,
            this.btnRefresh,
            this.btnEditProjectProperties});
            this.toolstripExplorer.Location = new System.Drawing.Point(0, 0);
            this.toolstripExplorer.MaximumSize = new System.Drawing.Size(0, 50);
            this.toolstripExplorer.Name = "toolstripExplorer";
            this.toolstripExplorer.Padding = new System.Windows.Forms.Padding(0);
            this.toolstripExplorer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolstripExplorer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolstripExplorer.Size = new System.Drawing.Size(357, 25);
            this.toolstripExplorer.Stretch = true;
            this.toolstripExplorer.TabIndex = 142;
            this.toolstripExplorer.Text = "titlebar";
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btnFilter.BackgroundImage = global::Thumper_Custom_Level_Editor.Properties.Resources.pixel;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFilter.DropDown = this.contextMenuFilters;
            this.btnFilter.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_filter;
            this.btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(32, 22);
            this.btnFilter.Text = "toolStripSplitButton1";
            this.btnFilter.ToolTipText = "Click to toggle filters";
            this.btnFilter.ButtonClick += new System.EventHandler(this.btnFilter_ButtonClick);
            // 
            // contextMenuFilters
            // 
            this.contextMenuFilters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterLeaf,
            this.filterLvl,
            this.filterGate,
            this.filterMaster,
            this.filterSample});
            this.contextMenuFilters.Name = "workingfolderRightClick";
            this.contextMenuFilters.OwnerItem = this.btnFilter;
            this.contextMenuFilters.Size = new System.Drawing.Size(114, 114);
            this.contextMenuFilters.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuFilters_Closing);
            // 
            // filterLeaf
            // 
            this.filterLeaf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.filterLeaf.CheckOnClick = true;
            this.filterLeaf.ForeColor = System.Drawing.Color.PaleGreen;
            this.filterLeaf.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.leaf;
            this.filterLeaf.Name = "filterLeaf";
            this.filterLeaf.Size = new System.Drawing.Size(113, 22);
            this.filterLeaf.Text = "Leaf";
            this.filterLeaf.CheckedChanged += new System.EventHandler(this.filter_CheckChanged);
            // 
            // filterLvl
            // 
            this.filterLvl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.filterLvl.CheckOnClick = true;
            this.filterLvl.ForeColor = System.Drawing.Color.Green;
            this.filterLvl.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.lvl;
            this.filterLvl.Name = "filterLvl";
            this.filterLvl.Size = new System.Drawing.Size(113, 22);
            this.filterLvl.Text = "Lvl";
            this.filterLvl.CheckedChanged += new System.EventHandler(this.filter_CheckChanged);
            // 
            // filterGate
            // 
            this.filterGate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.filterGate.CheckOnClick = true;
            this.filterGate.ForeColor = System.Drawing.Color.Orange;
            this.filterGate.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.gate;
            this.filterGate.Name = "filterGate";
            this.filterGate.Size = new System.Drawing.Size(113, 22);
            this.filterGate.Text = "Gate";
            this.filterGate.CheckedChanged += new System.EventHandler(this.filter_CheckChanged);
            // 
            // filterMaster
            // 
            this.filterMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.filterMaster.CheckOnClick = true;
            this.filterMaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.filterMaster.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.master;
            this.filterMaster.Name = "filterMaster";
            this.filterMaster.Size = new System.Drawing.Size(113, 22);
            this.filterMaster.Text = "Master";
            this.filterMaster.CheckedChanged += new System.EventHandler(this.filter_CheckChanged);
            // 
            // filterSample
            // 
            this.filterSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.filterSample.CheckOnClick = true;
            this.filterSample.ForeColor = System.Drawing.Color.Turquoise;
            this.filterSample.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.samp;
            this.filterSample.Name = "filterSample";
            this.filterSample.Size = new System.Drawing.Size(113, 22);
            this.filterSample.Text = "Sample";
            this.filterSample.CheckedChanged += new System.EventHandler(this.filter_CheckChanged);
            // 
            // btnCollapse
            // 
            this.btnCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCollapse.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_collapse;
            this.btnCollapse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(23, 22);
            this.btnCollapse.Text = "toolStripButton1";
            this.btnCollapse.ToolTipText = "Collapse All";
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExpand.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_expand;
            this.btnExpand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(23, 22);
            this.btnExpand.Text = "toolStripButton2";
            this.btnExpand.ToolTipText = "Expand All";
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_refresh2;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "toolStripButton3";
            this.btnRefresh.ToolTipText = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnEditProjectProperties
            // 
            this.btnEditProjectProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditProjectProperties.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_editdetails;
            this.btnEditProjectProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditProjectProperties.Name = "btnEditProjectProperties";
            this.btnEditProjectProperties.Size = new System.Drawing.Size(23, 22);
            this.btnEditProjectProperties.Text = "toolStripButton4";
            // 
            // Form_ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(357, 450);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.toolstripExplorer);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ProjectExplorer";
            this.Text = "Project Explorer";
            this.toolstripExplorer.ResumeLayout(false);
            this.toolstripExplorer.PerformLayout();
            this.contextMenuFilters.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStrip toolstripExplorer;
        private System.Windows.Forms.ContextMenuStrip contextMenuFilters;
        private System.Windows.Forms.ToolStripMenuItem filterLeaf;
        private System.Windows.Forms.ToolStripMenuItem filterLvl;
        private System.Windows.Forms.ToolStripMenuItem filterGate;
        private System.Windows.Forms.ToolStripMenuItem filterMaster;
        private System.Windows.Forms.ToolStripMenuItem filterSample;
        private System.Windows.Forms.ToolStripSplitButton btnFilter;
        private System.Windows.Forms.ToolStripButton btnCollapse;
        private System.Windows.Forms.ToolStripButton btnExpand;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnEditProjectProperties;
    }
}