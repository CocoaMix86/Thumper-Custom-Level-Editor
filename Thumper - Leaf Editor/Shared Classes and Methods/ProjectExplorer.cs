using System.Runtime.InteropServices;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public static class ProjectExplorer
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        public static Point GetTreeViewScrollPos(TreeView treeView)
        {
            return new Point(
                GetScrollPos(treeView.Handle, SB_HORZ),
                GetScrollPos(treeView.Handle, SB_VERT));
        }

        public static void SetTreeViewScrollPos(TreeView treeView, Point scrollPosition)
        {
            SetScrollPos(treeView.Handle, SB_HORZ, scrollPosition.X, true);
            SetScrollPos(treeView.Handle, SB_VERT, scrollPosition.Y, true);
        }

        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        //public static Dictionary<string, FileInfo> Files = new();
        //public static Dictionary<string, DirectoryInfo> Folders = new();
        public static TreeNodeCollection ProjectTree => TCLE.Explorer.treeView1.Nodes;
        public static TreeNode ProjectRoot => ProjectTree[0];
        public static Dictionary<TreeNode, FileOrFolder> AllFiles = new();
        public static IEnumerable<FileInfo> Files = AllFiles.Where(x => x.Value.IsFile).Select(x => x.Value.File);
        public static IEnumerable<DirectoryInfo> Folders = AllFiles.Where(x => x.Value.IsFolder).Select(x => x.Value.Folder);
        public static List<string> expandednodes = new();
        public static bool filterenabled;
        public static bool filtersearch;
        public static string SearchString;
        public static bool FilterLeaf;
        public static bool FilterLvl;
        public static bool FilterGate;
        public static bool FilterMaster;
        public static bool FilterSamp;

        public static void CreateTreeView()
        {
            if (TCLE.WorkingFolder == null) return;
            expandednodes.Clear();
            expandednodes = GetExpandedNodes(ProjectTree);
            //clear existing treeview
            Point LastScrollPosition = GetTreeViewScrollPos(TCLE.Explorer.treeView1);
            ProjectTree.Clear();
            AllFiles.Clear();
            ///projectfiles.Clear();
            ///projectfolders.Clear();
            if (TCLE.WorkingFolder.Exists) {
                //Build the tree
                BuildTree(TCLE.WorkingFolder, ProjectTree);
                //the root of the tree needs different properties
                ProjectRoot.ImageKey = "project";
                ProjectRoot.SelectedImageKey = "project";
                ProjectRoot.NodeFont = new Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Bold);
                ProjectRoot.ContextMenuStrip = TCLE.Explorer.contextMenuProject;

                TCLE.ReloadLvlsInProject();
            }
            //if using filters or search, expand all folders to show all results
            if (filterenabled || filtersearch)
                TCLE.Explorer.treeView1.ExpandAll();
            //otherwise expand root only
            else {
                ProjectTree[0].Expand();
                RecurseNodesFindExpanded(ProjectTree);
            }
            //force each master to recalc runtime in case tree has new files
            foreach (WeifenLuo.WinFormsUI.Docking.IDockContent? dock in TCLE.Instance.dockMain.Documents) {
                if (dock.GetType() == typeof(Form_LvlEditor)) (dock as Form_LvlEditor).RecalculateRuntime();
                if (dock.GetType() == typeof(Form_GateEditor)) (dock as Form_GateEditor).RecalculateRuntime();
                if (dock.GetType() == typeof(Form_MasterEditor)) (dock as Form_MasterEditor).RecalculateRuntime();
            }
            //repopulate dragdrop list
            TCLE.DragDropItems.Populate();
            SetTreeViewScrollPos(TCLE.Explorer.treeView1, LastScrollPosition);
        }

        private static void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {
            ///BuildTree is a recursive function.
            //the very first node every time this function is called is a folder.
            TreeNode folder = new() {
                Text = directoryInfo.Name,
                Name = directoryInfo.Name,
                ImageKey = "folder",
                SelectedImageKey = "folder",
                ContextMenuStrip = TCLE.Explorer.contextMenuFolderClick,
                ForeColor = Properties.Settings.Default.ColorProjExpText
            };
            addInMe.Add(folder);
            AllFiles.Add(folder, new(directoryInfo));
            ///projectfolders.Add(folder.FullPath, directoryInfo);

            //Build subtree for each folder inside this folder
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories()) {
                BuildTree(subdir, folder.Nodes);
            }

            //add each file inside the folder to the tree
            foreach (FileInfo file in directoryInfo.GetFiles()) {
                if (file.Extension is ".TCL" || file.Name.Equals("default.samp", StringComparison.OrdinalIgnoreCase))
                    continue;
                TreeNode _tn = new() {
                    Text = file.Name,
                    Name = file.Name,
                    ImageKey = TCLE.ImageExtensions.Contains(file.Extension) ? "image" : file.Extension,
                    SelectedImageKey = TCLE.ImageExtensions.Contains(file.Extension) ? "image" : file.Extension,
                    ContextMenuStrip = TCLE.Explorer.contextMenuFileClick,
                    ForeColor = Properties.Settings.Default.ColorProjExpText
                };
                AllFiles.Add(_tn, new(file));
                ///projectfiles.Add(_tn.FullPath, file);
                //check for various filters being used
                if (filtersearch && !file.Name.Contains(SearchString)) { }
                else if (filterenabled) {
                    if ((FilterLeaf && file.Extension is ".leaf") || (FilterLvl && file.Extension is ".lvl") || (FilterGate && file.Extension is ".gate") || (FilterMaster && file.Extension is ".master") || (FilterSamp && file.Extension is ".samp"))
                        folder.Nodes.Add(_tn);
                }
                else
                    folder.Nodes.Add(_tn);
            }
        }

        private static List<string> GetExpandedNodes(TreeNodeCollection treeNodeCollection)
        {
            List<string> expandednodes = new();
            foreach (TreeNode tn in treeNodeCollection) {
                expandednodes.AddRange(GetExpandedNodes(tn.Nodes));
                if (tn.IsExpanded)
                    expandednodes.Add(tn.FullPath);
            }

            return expandednodes;
        }

        private static void RecurseNodesFindExpanded(TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode tn in treeNodeCollection) {
                if (expandednodes.Contains(tn.FullPath))
                    tn.Expand();
                RecurseNodesFindExpanded(tn.Nodes);
            }
        }
    }
}
