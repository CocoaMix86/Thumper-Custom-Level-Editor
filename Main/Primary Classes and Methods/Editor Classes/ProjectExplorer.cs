using System.Runtime.InteropServices;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class FileOrFolder
    {
        public FileInfo File { get; set; }
        public DirectoryInfo Folder { get; set; }
        public bool IsFile => File != null;
        public bool IsFolder => Folder != null;
        public string Name => File?.Name ?? Folder.Name;
        public string FullPath => File?.FullName ?? Folder.FullName;

        public FileOrFolder(DirectoryInfo _dir = null)
        {
            File = null;
            Folder = _dir;
        }

        public FileOrFolder(FileInfo _file = null)
        {
            File = _file;
            Folder = null;
        }
    }

    public static partial class ProjectExplorer
    {
        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        [LibraryImport("user32.dll")]
        public static partial int GetScrollPos(IntPtr hWnd, int nBar);
        [LibraryImport("user32.dll")]
        public static partial int SetScrollPos(IntPtr hWnd, int nBar, int nPos, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);
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

        public static TreeNodeCollection ProjectTree => TCLE.Explorer.treeView1.Nodes;
        public static TreeNode ProjectRoot => ProjectTree[0];
        //
        public static Dictionary<TreeNode, FileOrFolder> AllFiles = new();
        public static Dictionary<string, FileInfo> Files = new(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, DirectoryInfo> Folders = new(StringComparer.OrdinalIgnoreCase);
        public static bool TryGetFile(string name, out FileInfo file) => Files.TryGetValue(name, out file);
        public static FileInfo? GetFile(string name) => Files.GetValueOrDefault(name);
        public static bool TryGetFolder(string name, out DirectoryInfo folder) => Folders.TryGetValue(name, out folder);
        public static List<FileInfo> GetFilesByExtension(string extension) => Files.Values.Where(x => x.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase)).ToList();
        //public static IEnumerable<FileInfo> Files => AllFiles.Where(x => x.Value.IsFile).Select(x => x.Value.File);
        //public static IEnumerable<DirectoryInfo> Folders => AllFiles.Where(x => x.Value.IsFolder).Select(x => x.Value.Folder);
        //
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
            Point LastScrollPosition = GetTreeViewScrollPos(TCLE.Explorer.treeView1);
            //clear existing treeview
            ProjectTree.Clear();
            AllFiles.Clear();
            Files.Clear();
            Folders.Clear();
            //
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
                if (dock.GetType() == typeof(EditorLvl)) (dock as EditorLvl).RecalculateRuntime();
                if (dock.GetType() == typeof(EditorGate)) (dock as EditorGate).RecalculateRuntime();
                if (dock.GetType() == typeof(EditorMaster)) (dock as EditorMaster).RecalculateRuntime();
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
            Folders[directoryInfo.Name] = directoryInfo;
            ///projectfolders.Add(folder.FullPath, directoryInfo);

            //Build subtree for each folder inside this folder
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories()) {
                //skip aurora folder. TCLE does not need the files in it
                if (subdir.Name == "aurora")
                    continue;
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
                Files[file.Name] = file;
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
