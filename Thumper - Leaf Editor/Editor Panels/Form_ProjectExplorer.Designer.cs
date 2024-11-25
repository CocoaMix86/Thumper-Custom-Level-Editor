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
            this.toolTip1 = new ToolTip(this.components);
            this.treeView1 = new TreeViewEx();
            this.imageList1 = new ImageList(this.components);
            this.txtSearch = new TextBox();
            this.toolstripExplorer = new ToolStrip();
            this.btnFilter = new ToolStripSplitButton();
            this.contextMenuFilters = new ContextMenuStrip(this.components);
            this.filterLeaf = new ToolStripMenuItem();
            this.filterLvl = new ToolStripMenuItem();
            this.filterGate = new ToolStripMenuItem();
            this.filterMaster = new ToolStripMenuItem();
            this.filterSample = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnRefresh = new ToolStripButton();
            this.btnCollapse = new ToolStripButton();
            this.btnExpand = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnOpenOnClick = new ToolStripButton();
            this.contextMenuFileClick = new ContextMenuStrip(this.components);
            this.toolstripFileOpen = new ToolStripMenuItem();
            this.toolstripFileRaw = new ToolStripMenuItem();
            this.toolstripFileExternal = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.toolstripFileCut = new ToolStripMenuItem();
            this.toolstripFileCopy = new ToolStripMenuItem();
            this.toolstripFileDelete = new ToolStripMenuItem();
            this.toolstripFileRename = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolstripFileCopyPath = new ToolStripMenuItem();
            this.contextMenuFolderClick = new ContextMenuStrip(this.components);
            this.toolstripFolderNew = new ToolStripMenuItem();
            this.contextMenuAddFile = new ContextMenuStrip(this.components);
            this.existingItemToolStripMenuItem = new ToolStripMenuItem();
            this.folderToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.toolStripMenuItem5 = new ToolStripMenuItem();
            this.toolStripMenuItem6 = new ToolStripMenuItem();
            this.toolStripMenuItem7 = new ToolStripMenuItem();
            this.toolStripMenuItem8 = new ToolStripMenuItem();
            this.toolStripMenuItem9 = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.toolstripFolderCut = new ToolStripMenuItem();
            this.toolstripFolderCopy = new ToolStripMenuItem();
            this.toolstripFolderPaste = new ToolStripMenuItem();
            this.toolstripFolderDelete = new ToolStripMenuItem();
            this.toolstripFolderRename = new ToolStripMenuItem();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.toolstripFolderCopyPath = new ToolStripMenuItem();
            this.toolstripFolderExplorer = new ToolStripMenuItem();
            this.contextMenuMulti = new ContextMenuStrip(this.components);
            this.toolStripMultiCut = new ToolStripMenuItem();
            this.toolStripMultiCopy = new ToolStripMenuItem();
            this.toolStripMultiDelete = new ToolStripMenuItem();
            this.contextMenuProject = new ContextMenuStrip(this.components);
            this.toolStripSeparator8 = new ToolStripSeparator();
            this.toolstripProjectPaste = new ToolStripMenuItem();
            this.toolStripMenuItem11 = new ToolStripMenuItem();
            this.toolStripSeparator9 = new ToolStripSeparator();
            this.toolStripMenuItem12 = new ToolStripMenuItem();
            this.toolStripMenuItem13 = new ToolStripMenuItem();
            this.toolstripFileSearch = new ToolStripMenuItem();
            this.toolStripSeparator10 = new ToolStripSeparator();
            this.toolstripExplorer.SuspendLayout();
            this.contextMenuFilters.SuspendLayout();
            this.contextMenuFileClick.SuspendLayout();
            this.contextMenuFolderClick.SuspendLayout();
            this.contextMenuAddFile.SuspendLayout();
            this.contextMenuMulti.SuspendLayout();
            this.contextMenuProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BackColor = Color.FromArgb(31, 31, 31);
            this.treeView1.BorderStyle = BorderStyle.None;
            this.treeView1.Dock = DockStyle.Fill;
            this.treeView1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.treeView1.ForeColor = Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageKey = "other";
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 20;
            this.treeView1.LabelEdit = true;
            this.treeView1.LineColor = Color.White;
            this.treeView1.Location = new Point(0, 48);
            this.treeView1.Margin = new Padding(4, 3, 4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageKey = "other";
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new Size(416, 471);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeLabelEdit += this.treeView1_BeforeLabelEdit;
            this.treeView1.AfterLabelEdit += this.treeView1_AfterLabelEdit;
            this.treeView1.ItemDrag += this.treeView1_ItemDrag;
            this.treeView1.BeforeSelect += this.treeView1_BeforeSelect;
            this.treeView1.Click += this.treeView1_Click;
            this.treeView1.DragDrop += this.treeView1_DragDrop;
            this.treeView1.DragEnter += this.treeView1_DragEnter;
            this.treeView1.DragOver += this.treeView1_DragOver;
            this.treeView1.DragLeave += this.treeView1_DragLeave;
            this.treeView1.DoubleClick += this.treeView1_DoubleClick;
            this.treeView1.KeyDown += this.treeView1_KeyDown;
            this.treeView1.MouseDown += this.treeView1_MouseDown;
            this.treeView1.MouseUp += this.treeView1_MouseUp;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, ".TCL");
            this.imageList1.Images.SetKeyName(2, ".lvl");
            this.imageList1.Images.SetKeyName(3, ".master");
            this.imageList1.Images.SetKeyName(4, "project");
            this.imageList1.Images.SetKeyName(5, "other");
            this.imageList1.Images.SetKeyName(6, ".leaf");
            this.imageList1.Images.SetKeyName(7, ".gate");
            this.imageList1.Images.SetKeyName(8, ".samp");
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = Color.FromArgb(56, 56, 56);
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearch.Dock = DockStyle.Top;
            this.txtSearch.ForeColor = Color.White;
            this.txtSearch.Location = new Point(0, 25);
            this.txtSearch.Margin = new Padding(4, 3, 4, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(416, 23);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Text = "Search Project Explorer (Ctrl+;)";
            this.txtSearch.TextChanged += this.txtSearch_TextChanged;
            // 
            // toolstripExplorer
            // 
            this.toolstripExplorer.BackColor = Color.FromArgb(31, 31, 31);
            this.toolstripExplorer.GripMargin = new Padding(0);
            this.toolstripExplorer.GripStyle = ToolStripGripStyle.Hidden;
            this.toolstripExplorer.Items.AddRange(new ToolStripItem[] { this.btnFilter, this.toolStripSeparator2, this.btnRefresh, this.btnCollapse, this.btnExpand, this.toolStripSeparator1, this.btnOpenOnClick });
            this.toolstripExplorer.Location = new Point(0, 0);
            this.toolstripExplorer.MaximumSize = new Size(0, 58);
            this.toolstripExplorer.Name = "toolstripExplorer";
            this.toolstripExplorer.Padding = new Padding(0);
            this.toolstripExplorer.RenderMode = ToolStripRenderMode.System;
            this.toolstripExplorer.RightToLeft = RightToLeft.No;
            this.toolstripExplorer.Size = new Size(416, 25);
            this.toolstripExplorer.Stretch = true;
            this.toolstripExplorer.TabIndex = 142;
            this.toolstripExplorer.Text = "titlebar";
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = Color.FromArgb(35, 35, 35);
            this.btnFilter.BackgroundImageLayout = ImageLayout.Center;
            this.btnFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnFilter.DropDown = this.contextMenuFilters;
            this.btnFilter.Image = Properties.Resources.icon_filter;
            this.btnFilter.ImageTransparentColor = Color.Magenta;
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new Size(32, 22);
            this.btnFilter.Text = "toolStripSplitButton1";
            this.btnFilter.ToolTipText = "Filter Files By Type";
            this.btnFilter.ButtonClick += this.btnFilter_ButtonClick;
            // 
            // contextMenuFilters
            // 
            this.contextMenuFilters.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuFilters.Items.AddRange(new ToolStripItem[] { this.filterLeaf, this.filterLvl, this.filterGate, this.filterMaster, this.filterSample });
            this.contextMenuFilters.Name = "workingfolderRightClick";
            this.contextMenuFilters.OwnerItem = this.btnFilter;
            this.contextMenuFilters.RenderMode = ToolStripRenderMode.System;
            this.contextMenuFilters.Size = new Size(114, 114);
            this.contextMenuFilters.Closing += this.contextMenuFilters_Closing;
            // 
            // filterLeaf
            // 
            this.filterLeaf.BackColor = Color.FromArgb(46, 46, 46);
            this.filterLeaf.CheckOnClick = true;
            this.filterLeaf.ForeColor = Color.PaleGreen;
            this.filterLeaf.Image = Properties.Resources.editor_leaf;
            this.filterLeaf.Name = "filterLeaf";
            this.filterLeaf.Size = new Size(113, 22);
            this.filterLeaf.Text = "Leaf";
            this.filterLeaf.CheckedChanged += this.filter_CheckChanged;
            // 
            // filterLvl
            // 
            this.filterLvl.BackColor = Color.FromArgb(46, 46, 46);
            this.filterLvl.CheckOnClick = true;
            this.filterLvl.ForeColor = Color.Green;
            this.filterLvl.Image = Properties.Resources.editor_lvl;
            this.filterLvl.Name = "filterLvl";
            this.filterLvl.Size = new Size(113, 22);
            this.filterLvl.Text = "Lvl";
            this.filterLvl.CheckedChanged += this.filter_CheckChanged;
            // 
            // filterGate
            // 
            this.filterGate.BackColor = Color.FromArgb(46, 46, 46);
            this.filterGate.CheckOnClick = true;
            this.filterGate.ForeColor = Color.Orange;
            this.filterGate.Image = Properties.Resources.editor_gate;
            this.filterGate.Name = "filterGate";
            this.filterGate.Size = new Size(113, 22);
            this.filterGate.Text = "Gate";
            this.filterGate.CheckedChanged += this.filter_CheckChanged;
            // 
            // filterMaster
            // 
            this.filterMaster.BackColor = Color.FromArgb(46, 46, 46);
            this.filterMaster.CheckOnClick = true;
            this.filterMaster.ForeColor = Color.FromArgb(150, 150, 255);
            this.filterMaster.Image = Properties.Resources.editor_master;
            this.filterMaster.Name = "filterMaster";
            this.filterMaster.Size = new Size(113, 22);
            this.filterMaster.Text = "Master";
            this.filterMaster.CheckedChanged += this.filter_CheckChanged;
            // 
            // filterSample
            // 
            this.filterSample.BackColor = Color.FromArgb(46, 46, 46);
            this.filterSample.CheckOnClick = true;
            this.filterSample.ForeColor = Color.Turquoise;
            this.filterSample.Image = Properties.Resources.editor_sample;
            this.filterSample.Name = "filterSample";
            this.filterSample.Size = new Size(113, 22);
            this.filterSample.Text = "Sample";
            this.filterSample.CheckedChanged += this.filter_CheckChanged;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = Properties.Resources.icon_refresh2;
            this.btnRefresh.ImageTransparentColor = Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(23, 22);
            this.btnRefresh.Text = "toolStripButton3";
            this.btnRefresh.ToolTipText = "Refresh";
            this.btnRefresh.Click += this.btnRefresh_Click;
            // 
            // btnCollapse
            // 
            this.btnCollapse.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnCollapse.Image = Properties.Resources.icon_collapse;
            this.btnCollapse.ImageTransparentColor = Color.Magenta;
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new Size(23, 22);
            this.btnCollapse.Text = "toolStripButton1";
            this.btnCollapse.ToolTipText = "Collapse All";
            this.btnCollapse.Click += this.btnCollapse_Click;
            // 
            // btnExpand
            // 
            this.btnExpand.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnExpand.Image = Properties.Resources.icon_expand;
            this.btnExpand.ImageTransparentColor = Color.Magenta;
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new Size(23, 22);
            this.btnExpand.Text = "toolStripButton2";
            this.btnExpand.ToolTipText = "Expand All";
            this.btnExpand.Click += this.btnExpand_Click;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnOpenOnClick
            // 
            this.btnOpenOnClick.CheckOnClick = true;
            this.btnOpenOnClick.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnOpenOnClick.Image = Properties.Resources.icon_view;
            this.btnOpenOnClick.ImageTransparentColor = Color.Magenta;
            this.btnOpenOnClick.Name = "btnOpenOnClick";
            this.btnOpenOnClick.Size = new Size(23, 22);
            this.btnOpenOnClick.Text = "toolStripButton1";
            this.btnOpenOnClick.ToolTipText = "Open file on single-click";
            // 
            // contextMenuFileClick
            // 
            this.contextMenuFileClick.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuFileClick.BackgroundImageLayout = ImageLayout.Center;
            this.contextMenuFileClick.Items.AddRange(new ToolStripItem[] { this.toolstripFileOpen, this.toolstripFileRaw, this.toolstripFileExternal, this.toolStripSeparator3, this.toolstripFileSearch, this.toolStripSeparator10, this.toolstripFileCut, this.toolstripFileCopy, this.toolstripFileDelete, this.toolstripFileRename, this.toolStripSeparator4, this.toolstripFileCopyPath });
            this.contextMenuFileClick.Name = "contextMenuFileClick";
            this.contextMenuFileClick.RenderMode = ToolStripRenderMode.System;
            this.contextMenuFileClick.Size = new Size(181, 242);
            this.contextMenuFileClick.Opening += this.contextMenuFileClick_Opening;
            // 
            // toolstripFileOpen
            // 
            this.toolstripFileOpen.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileOpen.ForeColor = Color.White;
            this.toolstripFileOpen.Image = Properties.Resources.icon_openfile;
            this.toolstripFileOpen.Name = "toolstripFileOpen";
            this.toolstripFileOpen.Size = new Size(180, 22);
            this.toolstripFileOpen.Text = "Open";
            // 
            // toolstripFileRaw
            // 
            this.toolstripFileRaw.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileRaw.ForeColor = Color.White;
            this.toolstripFileRaw.Image = (Image)resources.GetObject("toolstripFileRaw.Image");
            this.toolstripFileRaw.Name = "toolstripFileRaw";
            this.toolstripFileRaw.Size = new Size(180, 22);
            this.toolstripFileRaw.Text = "Open Raw Text";
            // 
            // toolstripFileExternal
            // 
            this.toolstripFileExternal.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileExternal.ForeColor = Color.White;
            this.toolstripFileExternal.Name = "toolstripFileExternal";
            this.toolstripFileExternal.Size = new Size(180, 22);
            this.toolstripFileExternal.Text = "Open Externally...";
            this.toolstripFileExternal.Click += this.toolstripFileExternal_Click;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator3.ForeColor = Color.White;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(177, 6);
            // 
            // toolstripFileCut
            // 
            this.toolstripFileCut.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileCut.ForeColor = Color.White;
            this.toolstripFileCut.Image = Properties.Resources.icon_cut;
            this.toolstripFileCut.Name = "toolstripFileCut";
            this.toolstripFileCut.ShortcutKeys = Keys.Control | Keys.X;
            this.toolstripFileCut.Size = new Size(180, 22);
            this.toolstripFileCut.Text = "Cut";
            this.toolstripFileCut.Click += this.toolstripFileCopy_Click;
            // 
            // toolstripFileCopy
            // 
            this.toolstripFileCopy.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileCopy.ForeColor = Color.White;
            this.toolstripFileCopy.Image = Properties.Resources.icon_copy2;
            this.toolstripFileCopy.Name = "toolstripFileCopy";
            this.toolstripFileCopy.ShortcutKeys = Keys.Control | Keys.C;
            this.toolstripFileCopy.Size = new Size(180, 22);
            this.toolstripFileCopy.Text = "Copy";
            this.toolstripFileCopy.Click += this.toolstripFileCopy_Click;
            // 
            // toolstripFileDelete
            // 
            this.toolstripFileDelete.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileDelete.ForeColor = Color.White;
            this.toolstripFileDelete.Image = Properties.Resources.icon_remove2;
            this.toolstripFileDelete.Name = "toolstripFileDelete";
            this.toolstripFileDelete.ShortcutKeys = Keys.Delete;
            this.toolstripFileDelete.Size = new Size(180, 22);
            this.toolstripFileDelete.Text = "Delete";
            this.toolstripFileDelete.Click += this.toolstripFileDelete_Click;
            // 
            // toolstripFileRename
            // 
            this.toolstripFileRename.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileRename.ForeColor = Color.White;
            this.toolstripFileRename.Image = Properties.Resources.icon_editdetails;
            this.toolstripFileRename.Name = "toolstripFileRename";
            this.toolstripFileRename.ShortcutKeys = Keys.F2;
            this.toolstripFileRename.Size = new Size(180, 22);
            this.toolstripFileRename.Text = "Rename";
            this.toolstripFileRename.Click += this.toolstripFileRename_Click;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator4.ForeColor = Color.White;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(177, 6);
            // 
            // toolstripFileCopyPath
            // 
            this.toolstripFileCopyPath.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFileCopyPath.ForeColor = Color.White;
            this.toolstripFileCopyPath.Image = Properties.Resources.icon_copy2;
            this.toolstripFileCopyPath.Name = "toolstripFileCopyPath";
            this.toolstripFileCopyPath.Size = new Size(180, 22);
            this.toolstripFileCopyPath.Text = "Copy File Path";
            this.toolstripFileCopyPath.Click += this.copyFilePathToolStripMenuItem1_Click;
            // 
            // contextMenuFolderClick
            // 
            this.contextMenuFolderClick.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuFolderClick.Items.AddRange(new ToolStripItem[] { this.toolstripFolderNew, this.toolStripSeparator5, this.toolstripFolderCut, this.toolstripFolderCopy, this.toolstripFolderPaste, this.toolstripFolderDelete, this.toolstripFolderRename, this.toolStripSeparator6, this.toolstripFolderCopyPath, this.toolstripFolderExplorer });
            this.contextMenuFolderClick.Name = "contextMenuFolderClick";
            this.contextMenuFolderClick.RenderMode = ToolStripRenderMode.System;
            this.contextMenuFolderClick.Size = new Size(163, 192);
            this.contextMenuFolderClick.Opening += this.contextMenuFolderClick_Opening;
            // 
            // toolstripFolderNew
            // 
            this.toolstripFolderNew.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderNew.DropDown = this.contextMenuAddFile;
            this.toolstripFolderNew.ForeColor = Color.White;
            this.toolstripFolderNew.Name = "toolstripFolderNew";
            this.toolstripFolderNew.Size = new Size(162, 22);
            this.toolstripFolderNew.Text = "Add";
            // 
            // contextMenuAddFile
            // 
            this.contextMenuAddFile.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuAddFile.Items.AddRange(new ToolStripItem[] { this.existingItemToolStripMenuItem, this.folderToolStripMenuItem, this.toolStripSeparator7, this.toolStripMenuItem5, this.toolStripMenuItem6, this.toolStripMenuItem7, this.toolStripMenuItem8, this.toolStripMenuItem9 });
            this.contextMenuAddFile.Name = "workingfolderRightClick";
            this.contextMenuAddFile.OwnerItem = this.toolstripFolderNew;
            this.contextMenuAddFile.RenderMode = ToolStripRenderMode.System;
            this.contextMenuAddFile.Size = new Size(152, 164);
            // 
            // existingItemToolStripMenuItem
            // 
            this.existingItemToolStripMenuItem.ForeColor = Color.White;
            this.existingItemToolStripMenuItem.Name = "existingItemToolStripMenuItem";
            this.existingItemToolStripMenuItem.Size = new Size(151, 22);
            this.existingItemToolStripMenuItem.Text = "Existing Item...";
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.BackColor = Color.FromArgb(46, 46, 46);
            this.folderToolStripMenuItem.ForeColor = Color.White;
            this.folderToolStripMenuItem.Image = Properties.Resources.icon_folder;
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new Size(151, 22);
            this.folderToolStripMenuItem.Text = "New Folder";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(148, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem5.ForeColor = Color.PaleGreen;
            this.toolStripMenuItem5.Image = Properties.Resources.editor_leaf;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(151, 22);
            this.toolStripMenuItem5.Text = "Leaf";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem6.ForeColor = Color.Green;
            this.toolStripMenuItem6.Image = Properties.Resources.editor_lvl;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(151, 22);
            this.toolStripMenuItem6.Text = "Lvl";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem7.ForeColor = Color.Orange;
            this.toolStripMenuItem7.Image = Properties.Resources.editor_gate;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(151, 22);
            this.toolStripMenuItem7.Text = "Gate";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem8.ForeColor = Color.FromArgb(150, 150, 255);
            this.toolStripMenuItem8.Image = Properties.Resources.editor_master;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(151, 22);
            this.toolStripMenuItem8.Text = "Master";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem9.ForeColor = Color.Turquoise;
            this.toolStripMenuItem9.Image = Properties.Resources.editor_sample;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Size(151, 22);
            this.toolStripMenuItem9.Text = "Sample";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem1.DropDown = this.contextMenuAddFile;
            this.toolStripMenuItem1.ForeColor = Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(162, 22);
            this.toolStripMenuItem1.Text = "Add";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(159, 6);
            // 
            // toolstripFolderCut
            // 
            this.toolstripFolderCut.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderCut.ForeColor = Color.White;
            this.toolstripFolderCut.Image = Properties.Resources.icon_cut;
            this.toolstripFolderCut.Name = "toolstripFolderCut";
            this.toolstripFolderCut.ShortcutKeys = Keys.Control | Keys.X;
            this.toolstripFolderCut.Size = new Size(162, 22);
            this.toolstripFolderCut.Text = "Cut";
            this.toolstripFolderCut.Click += this.toolstripFileCopy_Click;
            // 
            // toolstripFolderCopy
            // 
            this.toolstripFolderCopy.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderCopy.ForeColor = Color.White;
            this.toolstripFolderCopy.Image = Properties.Resources.icon_copy2;
            this.toolstripFolderCopy.Name = "toolstripFolderCopy";
            this.toolstripFolderCopy.ShortcutKeys = Keys.Control | Keys.C;
            this.toolstripFolderCopy.Size = new Size(162, 22);
            this.toolstripFolderCopy.Text = "Copy";
            this.toolstripFolderCopy.Click += this.toolstripFileCopy_Click;
            // 
            // toolstripFolderPaste
            // 
            this.toolstripFolderPaste.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderPaste.Enabled = false;
            this.toolstripFolderPaste.ForeColor = Color.White;
            this.toolstripFolderPaste.Image = Properties.Resources.icon_paste2;
            this.toolstripFolderPaste.Name = "toolstripFolderPaste";
            this.toolstripFolderPaste.ShortcutKeys = Keys.Control | Keys.V;
            this.toolstripFolderPaste.Size = new Size(162, 22);
            this.toolstripFolderPaste.Text = "Paste";
            this.toolstripFolderPaste.Click += this.toolstripFolderPaste_Click;
            // 
            // toolstripFolderDelete
            // 
            this.toolstripFolderDelete.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderDelete.ForeColor = Color.White;
            this.toolstripFolderDelete.Image = Properties.Resources.icon_remove2;
            this.toolstripFolderDelete.Name = "toolstripFolderDelete";
            this.toolstripFolderDelete.ShortcutKeys = Keys.Delete;
            this.toolstripFolderDelete.Size = new Size(162, 22);
            this.toolstripFolderDelete.Text = "Delete";
            this.toolstripFolderDelete.Click += this.toolstripFileDelete_Click;
            // 
            // toolstripFolderRename
            // 
            this.toolstripFolderRename.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderRename.ForeColor = Color.White;
            this.toolstripFolderRename.Image = Properties.Resources.icon_editdetails;
            this.toolstripFolderRename.Name = "toolstripFolderRename";
            this.toolstripFolderRename.ShortcutKeys = Keys.F2;
            this.toolstripFolderRename.Size = new Size(162, 22);
            this.toolstripFolderRename.Text = "Rename";
            this.toolstripFolderRename.Click += this.toolstripFileRename_Click;
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(159, 6);
            // 
            // toolstripFolderCopyPath
            // 
            this.toolstripFolderCopyPath.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderCopyPath.ForeColor = Color.White;
            this.toolstripFolderCopyPath.Image = Properties.Resources.icon_copy2;
            this.toolstripFolderCopyPath.Name = "toolstripFolderCopyPath";
            this.toolstripFolderCopyPath.Size = new Size(162, 22);
            this.toolstripFolderCopyPath.Text = "Copy File Path";
            this.toolstripFolderCopyPath.Click += this.toolstripFolderCopyPath_Click;
            // 
            // toolstripFolderExplorer
            // 
            this.toolstripFolderExplorer.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripFolderExplorer.ForeColor = Color.White;
            this.toolstripFolderExplorer.Image = Properties.Resources.icon_explorer;
            this.toolstripFolderExplorer.Name = "toolstripFolderExplorer";
            this.toolstripFolderExplorer.Size = new Size(162, 22);
            this.toolstripFolderExplorer.Text = "Open In Explorer";
            this.toolstripFolderExplorer.Click += this.toolstripFolderExplorer_Click;
            // 
            // contextMenuMulti
            // 
            this.contextMenuMulti.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuMulti.Items.AddRange(new ToolStripItem[] { this.toolStripMultiCut, this.toolStripMultiCopy, this.toolStripMultiDelete });
            this.contextMenuMulti.Name = "contextMenuFolderClick";
            this.contextMenuMulti.RenderMode = ToolStripRenderMode.System;
            this.contextMenuMulti.Size = new Size(145, 70);
            // 
            // toolStripMultiCut
            // 
            this.toolStripMultiCut.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMultiCut.ForeColor = Color.White;
            this.toolStripMultiCut.Image = Properties.Resources.icon_cut;
            this.toolStripMultiCut.Name = "toolStripMultiCut";
            this.toolStripMultiCut.ShortcutKeys = Keys.Control | Keys.X;
            this.toolStripMultiCut.Size = new Size(144, 22);
            this.toolStripMultiCut.Text = "Cut";
            this.toolStripMultiCut.Click += this.toolstripFileCopy_Click;
            // 
            // toolStripMultiCopy
            // 
            this.toolStripMultiCopy.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMultiCopy.ForeColor = Color.White;
            this.toolStripMultiCopy.Image = Properties.Resources.icon_copy2;
            this.toolStripMultiCopy.Name = "toolStripMultiCopy";
            this.toolStripMultiCopy.ShortcutKeys = Keys.Control | Keys.C;
            this.toolStripMultiCopy.Size = new Size(144, 22);
            this.toolStripMultiCopy.Text = "Copy";
            this.toolStripMultiCopy.Click += this.toolstripFileCopy_Click;
            // 
            // toolStripMultiDelete
            // 
            this.toolStripMultiDelete.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMultiDelete.ForeColor = Color.White;
            this.toolStripMultiDelete.Image = Properties.Resources.icon_remove2;
            this.toolStripMultiDelete.Name = "toolStripMultiDelete";
            this.toolStripMultiDelete.ShortcutKeys = Keys.Delete;
            this.toolStripMultiDelete.Size = new Size(144, 22);
            this.toolStripMultiDelete.Text = "Delete";
            this.toolStripMultiDelete.Click += this.toolstripFileDelete_Click;
            // 
            // contextMenuProject
            // 
            this.contextMenuProject.BackColor = Color.FromArgb(46, 46, 46);
            this.contextMenuProject.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItem1, this.toolStripSeparator8, this.toolstripProjectPaste, this.toolStripMenuItem11, this.toolStripSeparator9, this.toolStripMenuItem12, this.toolStripMenuItem13 });
            this.contextMenuProject.Name = "contextMenuFolderClick";
            this.contextMenuProject.RenderMode = ToolStripRenderMode.System;
            this.contextMenuProject.Size = new Size(163, 126);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new Size(159, 6);
            // 
            // toolstripProjectPaste
            // 
            this.toolstripProjectPaste.BackColor = Color.FromArgb(46, 46, 46);
            this.toolstripProjectPaste.Enabled = false;
            this.toolstripProjectPaste.ForeColor = Color.White;
            this.toolstripProjectPaste.Image = Properties.Resources.icon_paste2;
            this.toolstripProjectPaste.Name = "toolstripProjectPaste";
            this.toolstripProjectPaste.ShortcutKeys = Keys.Control | Keys.V;
            this.toolstripProjectPaste.Size = new Size(162, 22);
            this.toolstripProjectPaste.Text = "Paste";
            this.toolstripProjectPaste.Click += this.toolstripFolderPaste_Click;
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem11.ForeColor = Color.White;
            this.toolStripMenuItem11.Image = Properties.Resources.icon_editdetails;
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.ShortcutKeys = Keys.F2;
            this.toolStripMenuItem11.Size = new Size(162, 22);
            this.toolStripMenuItem11.Text = "Rename";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new Size(159, 6);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem12.ForeColor = Color.White;
            this.toolStripMenuItem12.Image = Properties.Resources.icon_copy2;
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new Size(162, 22);
            this.toolStripMenuItem12.Text = "Copy File Path";
            this.toolStripMenuItem12.Click += this.copyFilePathToolStripMenuItem1_Click;
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.BackColor = Color.FromArgb(46, 46, 46);
            this.toolStripMenuItem13.ForeColor = Color.White;
            this.toolStripMenuItem13.Image = Properties.Resources.icon_explorer;
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new Size(162, 22);
            this.toolStripMenuItem13.Text = "Open In Explorer";
            this.toolStripMenuItem13.Click += this.toolstripFolderExplorer_Click;
            // 
            // toolstripFileSearch
            // 
            this.toolstripFileSearch.ForeColor = Color.White;
            this.toolstripFileSearch.Image = Properties.Resources.icon_zoom;
            this.toolstripFileSearch.Name = "toolstripFileSearch";
            this.toolstripFileSearch.Size = new Size(180, 22);
            this.toolstripFileSearch.Text = "Find References";
            this.toolstripFileSearch.Click += this.toolstripFileSearch_Click;
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Size(177, 6);
            // 
            // Form_ProjectExplorer
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(31, 31, 31);
            this.ClientSize = new Size(416, 519);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.toolstripExplorer);
            this.DoubleBuffered = true;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_ProjectExplorer";
            this.Text = "Project Explorer";
            this.toolstripExplorer.ResumeLayout(false);
            this.toolstripExplorer.PerformLayout();
            this.contextMenuFilters.ResumeLayout(false);
            this.contextMenuFileClick.ResumeLayout(false);
            this.contextMenuFolderClick.ResumeLayout(false);
            this.contextMenuAddFile.ResumeLayout(false);
            this.contextMenuMulti.ResumeLayout(false);
            this.contextMenuProject.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private Thumper_Custom_Level_Editor.TreeViewEx treeView1;
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
        private System.Windows.Forms.ToolStripMenuItem toolstripFolderNew;
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
        private System.Windows.Forms.ContextMenuStrip contextMenuMulti;
        private System.Windows.Forms.ToolStripMenuItem toolStripMultiCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMultiCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMultiDelete;
        private System.Windows.Forms.ToolStripMenuItem existingItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private ContextMenuStrip contextMenuProject;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolstripProjectPaste;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem toolStripMenuItem13;
        private ToolStripMenuItem toolstripFileSearch;
        private ToolStripSeparator toolStripSeparator10;
    }
}