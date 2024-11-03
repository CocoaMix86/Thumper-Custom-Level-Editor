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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnCollapse = new System.Windows.Forms.ToolStripButton();
            this.btnExpand = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditProjectProperties = new System.Windows.Forms.ToolStripButton();
            this.btnOpenOnClick = new System.Windows.Forms.ToolStripButton();
            this.contextMenuFileClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolstripFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFileRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFileExternal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolstripFileCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFileCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFileRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolstripFileCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuFolderClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolstripFolderCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFolderCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFolderPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFolderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFolderRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolstripFolderCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripFolderExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstripExplorer.SuspendLayout();
            this.contextMenuFilters.SuspendLayout();
            this.contextMenuFileClick.SuspendLayout();
            this.contextMenuFolderClick.SuspendLayout();
            this.contextMenuAddFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
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
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, "LEVEL DETAILS.txt");
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
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // toolstripExplorer
            // 
            this.toolstripExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.toolstripExplorer.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolstripExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFilter,
            this.toolStripSeparator2,
            this.btnRefresh,
            this.btnCollapse,
            this.btnExpand,
            this.toolStripSeparator1,
            this.btnEditProjectProperties,
            this.btnOpenOnClick});
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
            this.btnFilter.ToolTipText = "Filter Files By Type";
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditProjectProperties
            // 
            this.btnEditProjectProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditProjectProperties.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_editdetails;
            this.btnEditProjectProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditProjectProperties.Name = "btnEditProjectProperties";
            this.btnEditProjectProperties.Size = new System.Drawing.Size(23, 22);
            this.btnEditProjectProperties.Text = "toolStripButton4";
            this.btnEditProjectProperties.ToolTipText = "Edit Level Properties";
            // 
            // btnOpenOnClick
            // 
            this.btnOpenOnClick.CheckOnClick = true;
            this.btnOpenOnClick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenOnClick.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_view;
            this.btnOpenOnClick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenOnClick.Name = "btnOpenOnClick";
            this.btnOpenOnClick.Size = new System.Drawing.Size(23, 22);
            this.btnOpenOnClick.Text = "toolStripButton1";
            this.btnOpenOnClick.ToolTipText = "Open file on single-click";
            // 
            // contextMenuFileClick
            // 
            this.contextMenuFileClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.contextMenuFileClick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.contextMenuFileClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripFileOpen,
            this.toolstripFileRaw,
            this.toolstripFileExternal,
            this.toolStripSeparator3,
            this.toolstripFileCut,
            this.toolstripFileCopy,
            this.toolstripFileDelete,
            this.toolstripFileRename,
            this.toolStripSeparator4,
            this.toolstripFileCopyPath});
            this.contextMenuFileClick.Name = "contextMenuFileClick";
            this.contextMenuFileClick.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuFileClick.Size = new System.Drawing.Size(167, 192);
            // 
            // toolstripFileOpen
            // 
            this.toolstripFileOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileOpen.ForeColor = System.Drawing.Color.White;
            this.toolstripFileOpen.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_openfile;
            this.toolstripFileOpen.Name = "toolstripFileOpen";
            this.toolstripFileOpen.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileOpen.Text = "Open";
            // 
            // toolstripFileRaw
            // 
            this.toolstripFileRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileRaw.ForeColor = System.Drawing.Color.White;
            this.toolstripFileRaw.Image = ((System.Drawing.Image)(resources.GetObject("toolstripFileRaw.Image")));
            this.toolstripFileRaw.Name = "toolstripFileRaw";
            this.toolstripFileRaw.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileRaw.Text = "Open Raw Text";
            // 
            // toolstripFileExternal
            // 
            this.toolstripFileExternal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileExternal.ForeColor = System.Drawing.Color.White;
            this.toolstripFileExternal.Name = "toolstripFileExternal";
            this.toolstripFileExternal.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileExternal.Text = "Open Externally...";
            this.toolstripFileExternal.Click += new System.EventHandler(this.toolstripFileExternal_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.White;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // toolstripFileCut
            // 
            this.toolstripFileCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileCut.ForeColor = System.Drawing.Color.White;
            this.toolstripFileCut.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_cut;
            this.toolstripFileCut.Name = "toolstripFileCut";
            this.toolstripFileCut.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileCut.Text = "Cut";
            this.toolstripFileCut.Click += new System.EventHandler(this.toolstripFileCut_Click);
            // 
            // toolstripFileCopy
            // 
            this.toolstripFileCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileCopy.ForeColor = System.Drawing.Color.White;
            this.toolstripFileCopy.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.toolstripFileCopy.Name = "toolstripFileCopy";
            this.toolstripFileCopy.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileCopy.Text = "Copy";
            this.toolstripFileCopy.Click += new System.EventHandler(this.toolstripFileCopy_Click);
            // 
            // toolstripFileDelete
            // 
            this.toolstripFileDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileDelete.ForeColor = System.Drawing.Color.White;
            this.toolstripFileDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.toolstripFileDelete.Name = "toolstripFileDelete";
            this.toolstripFileDelete.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileDelete.Text = "Delete";
            this.toolstripFileDelete.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolstripFileRename
            // 
            this.toolstripFileRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileRename.ForeColor = System.Drawing.Color.White;
            this.toolstripFileRename.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_editdetails;
            this.toolstripFileRename.Name = "toolstripFileRename";
            this.toolstripFileRename.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileRename.Text = "Rename";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripSeparator4.ForeColor = System.Drawing.Color.White;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // toolstripFileCopyPath
            // 
            this.toolstripFileCopyPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFileCopyPath.ForeColor = System.Drawing.Color.White;
            this.toolstripFileCopyPath.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.toolstripFileCopyPath.Name = "toolstripFileCopyPath";
            this.toolstripFileCopyPath.Size = new System.Drawing.Size(180, 22);
            this.toolstripFileCopyPath.Text = "Copy File Path";
            this.toolstripFileCopyPath.Click += new System.EventHandler(this.copyFilePathToolStripMenuItem1_Click);
            // 
            // contextMenuFolderClick
            // 
            this.contextMenuFolderClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.contextMenuFolderClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator5,
            this.toolstripFolderCut,
            this.toolstripFolderCopy,
            this.toolstripFolderPaste,
            this.toolstripFolderDelete,
            this.toolstripFolderRename,
            this.toolStripSeparator6,
            this.toolstripFolderCopyPath,
            this.toolstripFolderExplorer});
            this.contextMenuFolderClick.Name = "contextMenuFolderClick";
            this.contextMenuFolderClick.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuFolderClick.Size = new System.Drawing.Size(181, 214);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDown = this.contextMenuAddFile;
            this.newToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // contextMenuAddFile
            // 
            this.contextMenuAddFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.contextMenuAddFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.folderToolStripMenuItem,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.contextMenuAddFile.Name = "workingfolderRightClick";
            this.contextMenuAddFile.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuAddFile.Size = new System.Drawing.Size(114, 136);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.PaleGreen;
            this.toolStripMenuItem5.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.leaf;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem5.Text = "Leaf";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripMenuItem6.ForeColor = System.Drawing.Color.Green;
            this.toolStripMenuItem6.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.lvl;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem6.Text = "Lvl";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripMenuItem7.ForeColor = System.Drawing.Color.Orange;
            this.toolStripMenuItem7.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.gate;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem7.Text = "Gate";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripMenuItem8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem8.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.master;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem8.Text = "Master";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolStripMenuItem9.ForeColor = System.Drawing.Color.Turquoise;
            this.toolStripMenuItem9.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.samp;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(113, 22);
            this.toolStripMenuItem9.Text = "Sample";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(159, 6);
            // 
            // toolstripFolderCut
            // 
            this.toolstripFolderCut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFolderCut.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderCut.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_cut;
            this.toolstripFolderCut.Name = "toolstripFolderCut";
            this.toolstripFolderCut.Size = new System.Drawing.Size(180, 22);
            this.toolstripFolderCut.Text = "Cut";
            this.toolstripFolderCut.Click += new System.EventHandler(this.toolstripFolderCut_Click);
            // 
            // toolstripFolderCopy
            // 
            this.toolstripFolderCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFolderCopy.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderCopy.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.toolstripFolderCopy.Name = "toolstripFolderCopy";
            this.toolstripFolderCopy.Size = new System.Drawing.Size(180, 22);
            this.toolstripFolderCopy.Text = "Copy";
            this.toolstripFolderCopy.Click += new System.EventHandler(this.toolstripFolderCopy_Click);
            // 
            // toolstripFolderPaste
            // 
            this.toolstripFolderPaste.Enabled = false;
            this.toolstripFolderPaste.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderPaste.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_paste2;
            this.toolstripFolderPaste.Name = "toolstripFolderPaste";
            this.toolstripFolderPaste.Size = new System.Drawing.Size(180, 22);
            this.toolstripFolderPaste.Text = "Paste";
            this.toolstripFolderPaste.Click += new System.EventHandler(this.toolstripFolderPaste_Click);
            // 
            // toolstripFolderDelete
            // 
            this.toolstripFolderDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFolderDelete.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderDelete.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_remove2;
            this.toolstripFolderDelete.Name = "toolstripFolderDelete";
            this.toolstripFolderDelete.Size = new System.Drawing.Size(162, 22);
            this.toolstripFolderDelete.Text = "Delete";
            this.toolstripFolderDelete.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolstripFolderRename
            // 
            this.toolstripFolderRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.toolstripFolderRename.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderRename.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_editdetails;
            this.toolstripFolderRename.Name = "toolstripFolderRename";
            this.toolstripFolderRename.Size = new System.Drawing.Size(162, 22);
            this.toolstripFolderRename.Text = "Rename";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(159, 6);
            // 
            // toolstripFolderCopyPath
            // 
            this.toolstripFolderCopyPath.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderCopyPath.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_copy2;
            this.toolstripFolderCopyPath.Name = "toolstripFolderCopyPath";
            this.toolstripFolderCopyPath.Size = new System.Drawing.Size(162, 22);
            this.toolstripFolderCopyPath.Text = "Copy File Path";
            this.toolstripFolderCopyPath.Click += new System.EventHandler(this.copyFilePathToolStripMenuItem1_Click);
            // 
            // toolstripFolderExplorer
            // 
            this.toolstripFolderExplorer.ForeColor = System.Drawing.Color.White;
            this.toolstripFolderExplorer.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_explorer;
            this.toolstripFolderExplorer.Name = "toolstripFolderExplorer";
            this.toolstripFolderExplorer.Size = new System.Drawing.Size(162, 22);
            this.toolstripFolderExplorer.Text = "Open In Explorer";
            this.toolstripFolderExplorer.Click += new System.EventHandler(this.openInExplorerToolStripMenuItem_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.folderToolStripMenuItem.Image = global::Thumper_Custom_Level_Editor.Properties.Resources.icon_folder;
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.folderToolStripMenuItem.Text = "Folder";
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
            this.contextMenuFileClick.ResumeLayout(false);
            this.contextMenuFolderClick.ResumeLayout(false);
            this.contextMenuAddFile.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnOpenOnClick;
        private System.Windows.Forms.ContextMenuStrip contextMenuFileClick;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileOpen;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileExternal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileCut;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileCopy;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileDelete;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileRename;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileCopyPath;
        private System.Windows.Forms.ToolStripMenuItem toolstripFileRaw;
        private System.Windows.Forms.ContextMenuStrip contextMenuFolderClick;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderCut;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderCopy;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderDelete;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderRename;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderCopyPath;
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderExplorer;
        private System.Windows.Forms.ContextMenuStrip contextMenuAddFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
    }
}