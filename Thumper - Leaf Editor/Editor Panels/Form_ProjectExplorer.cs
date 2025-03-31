using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Packaging;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectExplorer : DockContentEx
    {
        #region Form Construction
        public Form_ProjectExplorer()
        {
            InitializeComponent();
            //set custom renderer for some controls
            toolstripExplorer.Renderer = new ToolStripOverride();
            contextMenuAddFile.Renderer = new ContextMenuColors();
            contextMenuFilters.Renderer = new ContextMenuColors();
            contextMenuFileClick.Renderer = new ContextMenuColors();
            contextMenuFolderClick.Renderer = new ContextMenuColors();
            contextMenuMulti.Renderer = new ContextMenuColors();
            contextMenuProject.Renderer = new ContextMenuColors();
            //add events to some controls
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
        }
        public void LoadProject()
        {
            ProjectExplorer.CreateTreeView();
        }
        #endregion
        #region Variables
        private bool cutfile;
        private bool dontcancelifrename;
        private string renametype;
        private TreeNode NewNameNode;
        private string OldName;
        private static string[] notallowedchars = new string[] { "/", "?", ":", "&", "\\", "*", "\"", "<", ">", "|", "#", "%" };
        private TreeNode previousNode;
        private List<TreeNode> filestocopy;
        private List<TreeNode> selectedNodes = new();
        //string is obj_name, FileInfo is file itself
        #endregion
        #region Key press Handling
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) toolstripFileDelete_Click(null, null);
            if (e.KeyCode == Keys.F2 && selectedNodes.Count == 1) {
                dontcancelifrename = true;
                treeView1.SelectedNode = selectedNodes[0];
                selectedNodes[0].BeginEdit();
                treeView1.SelectedNode = null;
                dontcancelifrename = false;
            }
        }
        #endregion
        #region Physical Controls
        private void filter_CheckChanged(object sender, EventArgs e)
        {
            ProjectExplorer.FilterLeaf = filterLeaf.Checked;
            ProjectExplorer.FilterLvl = filterLvl.Checked;
            ProjectExplorer.FilterGate = filterGate.Checked;
            ProjectExplorer.FilterMaster = filterMaster.Checked;
            ProjectExplorer.FilterSamp = filterSample.Checked;
            ProjectExplorer.CreateTreeView();
        }

        private void btnFilter_ButtonClick(object sender, EventArgs e)
        {
            ProjectExplorer.filterenabled = !ProjectExplorer.filterenabled;
            //this style button doesn't have a Checked state, so we change its backcolor to show its enabled or not
            btnFilter.BackColor = ProjectExplorer.filterenabled ? Color.FromArgb(46, 46, 46) : Color.FromArgb(35, 35, 35);
            //recreate the tree when filter state changes
            ProjectExplorer.CreateTreeView();
        }

        private void contextMenuFilters_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            //this prevents the filter menu from closing when an option is chosen, allowing to select multiple before exiting the menu
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search Project Explorer (Ctrl+;)") {
                txtSearch.TextChanged -= txtSearch_TextChanged;
                txtSearch.Text = "";
                txtSearch.TextChanged += txtSearch_TextChanged;
            }
        }

        private void txtSearch_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text)) {
                txtSearch.TextChanged -= txtSearch_TextChanged;
                txtSearch.Text = "Search Project Explorer (Ctrl+;)";
                txtSearch.TextChanged += txtSearch_TextChanged;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ProjectExplorer.filtersearch = txtSearch.Text is not "" and not "Search Project Explorer (Ctrl+;)";
            ProjectExplorer.SearchString = txtSearch.Text;
            ProjectExplorer.CreateTreeView();
        }

        private void btnCollapse_Click(object sender, EventArgs e) => treeView1.CollapseAll();
        private void btnExpand_Click(object sender, EventArgs e) => treeView1.ExpandAll();
        private void btnRefresh_Click(object sender, EventArgs e) => ProjectExplorer.CreateTreeView();
        #endregion
        #region Context Menu File
        private void toolstripFileOpen_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(ProjectExplorer.Files[selectedNodes[0]].File);
        }

        private void contextMenuFileClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //depending on number of items selected, alter the contextmenu
            toolstripFileRename.Enabled = selectedNodes.Count == 1;
            toolstripFileCopyPath.Visible = selectedNodes.Count == 1;
        }
        private void copyFilePathToolStripMenuItem1_Click(object sender, EventArgs e) => Clipboard.SetText(ProjectExplorer.Files[selectedNodes[0]].File.FullName);

        private void toolstripFileRaw_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(ProjectExplorer.Files[selectedNodes[0]].File, true);
        }

        private void toolstripFileSearch_Click(object sender, EventArgs e)
        {
            MessageBox.Show(TCLE.SearchReferences(selectedNodes[0].Name), "Thumper Custom Level Editor");
        }
        private void toolstripFileExternal_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in selectedNodes) {
                if (File.Exists(ProjectExplorer.Files[tn].File.FullName))
                    Process.Start(new ProcessStartInfo(ProjectExplorer.Files[tn].File.FullName) { UseShellExecute = true });
            }
        }
        private void toolstripFileDelete_Click(object sender, EventArgs e)
        {
            if (selectedNodes.Count == 1) {
                if (MessageBox.Show($"'{selectedNodes[0].Name}' will be deleted permanently", "Thumper Custom Level Editor", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                    return;
                }
            }
            if (selectedNodes.Count > 1) {
                if (MessageBox.Show($"The selected items will be deleted permanently.", "Thumper Custom Level Editor", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                    return;
                }
            }

            foreach (TreeNode tn in selectedNodes) {
                if (tn.ImageKey == "folder") {
                    DirectoryInfo source = ProjectExplorer.Files[tn].Folder;
                    foreach (FileInfo file in source.GetFiles("*", SearchOption.AllDirectories)) {
                        TCLE.CloseFile(file);
                        TCLE.DeleteFileLock(file);
                    }
                    source.Delete(true);
                }
                else {
                    FileInfo source = ProjectExplorer.Files[tn].File;
                    TCLE.CloseFile(source);
                    TCLE.DeleteFileLock(source);
                    ///FindDuplicateFile(tn, Color.White);
                }
                tn.Remove();
            }
        }
        private void toolstripFileCopy_Click(object sender, EventArgs e)
        {
            filestocopy = selectedNodes.Cast<TreeNode>().ToList();
            toolstripFolderPaste.Enabled = true;
            toolstripProjectPaste.Enabled = true;
            if ((sender as ToolStripItem).Text == "Cut")
                cutfile = true;
        }

        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(ProjectExplorer.Files[selectedNodes[0]].File.FullName))
                Process.Start("explorer.exe", $@"/select, ""{ProjectExplorer.Files[selectedNodes[0]].File.FullName}""");
        }
        #region Rename Handling
        private void toolstripFileRename_Click(object sender, EventArgs e)
        {
            dontcancelifrename = true;
            treeView1.SelectedNode = selectedNodes[0];
            selectedNodes[0].BeginEdit();
            treeView1.SelectedNode = null;
            dontcancelifrename = false;
            //check for same name
            ///FindDuplicateFile(selectedNodes[0], Color.Red);
        }
        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            ///renamefile = e.Node.FullPath;
            NewNameNode = e.Node;
            OldName = e.Node.Name;
            renametype = e.Node.ImageKey == "folder" ? "folder" : "file";
        }
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            this.BeginInvoke(new Action(() => afterAfterEdit(e.Node)));
        }
        private void afterAfterEdit(TreeNode node)
        {
            //check for not allowed characters in file path
            if (notallowedchars.Any(c => node.Text.Contains(c)) || node.Text is "." or "..") {
                MessageBox.Show($"File and Folder names cannot:\n- contain any of the following characters: / ? : & \\ * \" < > | # %\n- be '.' or '..'\n\nPlease enter a valid name.", "Thumper Custom Level Editor");
                node.Text = OldName;
                return;
            }
            string source = renametype == "folder" ? ProjectExplorer.Files[NewNameNode].Folder.FullName : ProjectExplorer.Files[NewNameNode].File.FullName; //$@"{Path.GetDirectoryName(projectfolder.FullName)}\{renamefile}";
            string dest = $@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{node.FullPath}";
            //check if same name
            if (NewNameNode.Text == OldName) {
                return;
            }
            //check if name exists already
            if (File.Exists(dest) || Directory.Exists(dest)) {
                MessageBox.Show($"A file or folder with the name '{node.Text}' already exists on\ndisk at this location. Please choose another name.", "Thumper Custom Level Editor");
                node.Text = OldName;
                return;
            }
            //check for changing file extension
            if (node.ImageKey != "folder" && Path.GetExtension(source) != Path.GetExtension(dest)) {
                if (MessageBox.Show("If you change a file name extension, the file may become\nunusable. Are you sure you want to change it?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    node.Text = OldName;
                    return;
                }
            }
            //move the folder or file
            if (node.ImageKey == "folder") {
                Directory.Move(source, dest);
                ProjectExplorer.CreateTreeView();
            }
            else {
                if (TCLE.lockedfiles.Any(x => x.Key.FullName == source))
                    MessageBox.Show($"{source} is currently open and cannot be renamed.", "Thumper Custom Level Editor");
                else {
                    File.Move(source, dest);
                    dynamic towrite = TCLE.LoadFileLock(dest);
                    File.WriteAllText(dest, ((string)JsonConvert.SerializeObject(towrite, Formatting.Indented)).Replace(Path.GetFileName(source), Path.GetFileName(dest)));
                    ProjectExplorer.CreateTreeView();
                    //need to update the name in every other file that references it too
                    foreach (FileInfo file in TCLE.WorkingFolder.GetFilesByExtensions(".leaf", ".lvl", ".gate", ".master", ".samp")) {
                        dynamic _loadfile = TCLE.LoadFileLock(file.FullName);
                        //if load fails, skip
                        if (_loadfile == null)
                            continue;
                        string _output = JsonConvert.SerializeObject(_loadfile, Formatting.Indented);
                        //if files doesn't contain reference, skip
                        if (!_output.Contains(Path.GetFileName(source)))
                            continue;
                        //some files may be lock loaded, so we use different writing methods for those
                        //also force editor to reload the document
                        if (TCLE.lockedfiles.FirstOrDefault(x => x.Key.FullName == file.FullName) is KeyValuePair<FileInfo, FileStream> stream && stream.Value != null) {
                            TCLE.WriteFileLock(stream.Value, _output.Replace($"_name\": \"{Path.GetFileName(source)}\"", $"_name\": \"{Path.GetFileName(dest)}\""));
                            //a document might be open multiple times (normal and raw), so need to locate both of them
                            foreach (IDockContent doc in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(stream.Key.Name)))
                                doc.GetType().GetMethod("Reload").Invoke(doc, null);
                        }
                        else
                            File.WriteAllText(file.FullName, _output.Replace($"_name\": \"{Path.GetFileName(source)}\"", $"_name\": \"{Path.GetFileName(dest)}\""));
                    }
                }
            }
        }
        #endregion
        #endregion
        #region Context Menu Folder
        private void contextMenuFolderClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //depending on number of items selected, alter the contextmenu
            toolstripFolderRename.Enabled = selectedNodes.Count == 1;
            toolstripFolderNew.Enabled = selectedNodes.Count == 1;
            toolstripFolderCopyPath.Visible = selectedNodes.Count == 1;
        }
        private void toolstripFolderExplorer_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in selectedNodes) {
                if (ProjectExplorer.Files[tn].Folder.Exists)
                    Process.Start(new ProcessStartInfo(ProjectExplorer.Files[tn].Folder.FullName) { UseShellExecute = true });
            }
        }

        private void existingItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Title = "Copy Existing File to Project";
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = TCLE.WorkingFolder?.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                FileInfo filetocopy = new(ofd.FileName);
                if (File.Exists($"{ProjectExplorer.Files[selectedNodes[0]].File.FullName}\\{filetocopy.Name}")) {
                    MessageBox.Show($"A filed named {filetocopy.Name} already exists in folder {selectedNodes[0].Text}.", "Thumper Custom Level Editor");
                    return;
                }
                FileInfo projectfile = new($"{ProjectExplorer.Files[selectedNodes[0]].File.FullName}\\{filetocopy.Name}");
                File.Copy(ofd.FileName, projectfile.FullName);

                if (TCLE.fileextensions.Any(x => projectfile.Name.StartsWith(x))) {
                    if (MessageBox.Show("This appears to be a file from an older version of the editor.\nConvert it to the new TCLE 3.0 format?", "Editor Custom Thumper Level", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        string[] splitextension = projectfile.Name.Replace(".txt", "").Split('_', 2);
                        projectfile.MoveTo($"{projectfile.DirectoryName}\\{splitextension[1]}.{splitextension[0]}");
                    }
                }

                ProjectExplorer.CreateTreeView();
                TCLE.OpenFile(projectfile);
            }
        }


        private void toolstripFolderCopyPath_Click(object sender, EventArgs e) => Clipboard.SetText(ProjectExplorer.Files[selectedNodes[0]].Folder.FullName);
        #region Paste
        private void toolstripFolderPaste_Click(object sender, EventArgs e)
        {
            TreeNode targetnode = selectedNodes[0];
            List<TreeNode> parentnodestocopy = new();
            foreach (TreeNode tn in filestocopy) {
                bool found = IsAChildOfOtherNodes(tn, filestocopy);
                if (!found)
                    parentnodestocopy.Add(tn);
            }

            foreach (TreeNode tn in parentnodestocopy) {
                string source = ProjectExplorer.Files[tn].FullPath;/* GetFileOrFolderPath(tn.FullPath).FullName;*/
                string dest = $@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{tn.Name}";
                //check if the destination is within the copied node. If it is, skip this node.
                if (IsAChildOfOtherNodes(targetnode, tn)) {
                    MessageBox.Show($"Item '{tn.Name}' not pasted as it contains the destination.", "Thumper Custom Level Editor");
                    continue;
                }
                //check if each node exists at the destination and ask to overwrite it. If' No', skip this node.
                if (targetnode.Nodes.ContainsKey(ProjectExplorer.Files[tn].Name)) {
                    /*
                    if (MessageBox.Show($"Item '{tn.Name}' already exists at the destination. Overwrite it?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No)
                        continue;
                    */
                    FileInfo fullname = new($@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{tn.Name}");
                    string name = Path.GetFileNameWithoutExtension(fullname.Name);
                    ///int count = targetnode.Nodes.Cast<TreeNode>().Count(x => x.Name == tn.Name);
                    int count = Directory.GetFiles($@"{ProjectExplorer.Files[targetnode].Folder.FullName}", $"{name} - Copy*{fullname.Extension}").Length + 1;
                    dest = $@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{name}{(count > 0 ? $" - Copy ({count})" : "")}{fullname.Extension}";
                }

                if (cutfile) {
                    if (tn.ImageKey == "folder" && Directory.Exists(source)) {
                        Directory.Move(source, dest);
                    }
                    else if (File.Exists(source)) {
                        File.Move(source, dest);
                    }
                    cutfile = false;
                    toolstripFolderPaste.Enabled = false;
                    toolstripProjectPaste.Enabled = false;
                }
                else {
                    if (tn.ImageKey == "folder" && Directory.Exists(source)) {
                        TCLE.CopyDirectory(source, dest, true);
                    }
                    else if (File.Exists(source)) {
                        //File.Copy(source, dest);
                        dynamic towrite = TCLE.LoadFileLock(source);
                        File.WriteAllText(dest, ((string)JsonConvert.SerializeObject(towrite, Formatting.Indented)).Replace(Path.GetFileName(source), Path.GetFileName(dest)));
                    }
                }
            }
            ProjectExplorer.CreateTreeView();
        }

        private void toolstripFileDuplicate_Click(object sender, EventArgs e)
        {
            TreeNode targetnode = selectedNodes[0].Parent;
            TreeNode sourcenode = selectedNodes[0];

            string source = ProjectExplorer.Files[sourcenode].FullPath;/* GetFileOrFolderPath(tn.FullPath).FullName;*/
            string dest = $@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{sourcenode.Name}";
            //check if each node exists at the destination and ask to overwrite it. If' No', skip this node.
            if (targetnode.Nodes.ContainsKey(ProjectExplorer.Files[sourcenode].Name)) {
                //calculate how many copies exist and create new name appropriately
                FileInfo fullname = new($@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{sourcenode.Name}");
                string name = Path.GetFileNameWithoutExtension(fullname.Name);
                int count = Directory.GetFiles($@"{ProjectExplorer.Files[targetnode].Folder.FullName}", $"{name} - Copy*{fullname.Extension}").Length + 1;
                dest = $@"{ProjectExplorer.Files[targetnode].Folder.FullName}\{name}{(count > 0 ? $" - Copy ({count})" : "")}{fullname.Extension}";
            }            

            if (File.Exists(source)) {
                dynamic towrite = TCLE.LoadFileLock(source);
                File.WriteAllText(dest, ((string)JsonConvert.SerializeObject(towrite, Formatting.Indented)).Replace(Path.GetFileName(source), Path.GetFileName(dest)));
            }

            ProjectExplorer.CreateTreeView();
        }
        #endregion
        #endregion
        #region Multiselect Handling
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // cancel selection, the selection will be handled in MouseDown
            if (dontcancelifrename == false)
                e.Cancel = true;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode currentNode = treeView1.GetNodeAt(e.Location);
            if (currentNode == null) return;

            bool control = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;

            if (control && e.Button != MouseButtons.Right) {
                // the node clicked with control button pressed:
                // invert selection of the current node
                List<TreeNode> addedNodes = new();
                List<TreeNode> removedNodes = new();
                if (!selectedNodes.Contains(currentNode)) {
                    addedNodes.Add(currentNode);
                    previousNode = currentNode;
                }
                else {
                    removedNodes.Add(currentNode);
                }
                changeSelection(addedNodes, removedNodes);
            }
            else if (shift && previousNode != null && e.Button != MouseButtons.Right) {
                if (currentNode.Parent == previousNode.Parent) {
                    // the node clicked with shift button pressed:
                    // if current node and previously selected node
                    // belongs to the same parent,
                    // select range of nodes between these two
                    List<TreeNode> addedNodes = new();
                    List<TreeNode> removedNodes = new();
                    bool selection = false;
                    bool selectionEnd = false;

                    TreeNodeCollection nodes = null;
                    if (previousNode.Parent == null)
                        nodes = treeView1.Nodes;
                    else
                        nodes = previousNode.Parent.Nodes;

                    foreach (TreeNode n in nodes) {
                        if (n == currentNode || n == previousNode) {
                            if (selection)
                                selectionEnd = true;
                            if (!selection)
                                selection = true;
                        }
                        if (selection && !selectedNodes.Contains(n)) {
                            addedNodes.Add(n);
                        }
                        if (selectionEnd) {
                            break;
                        }
                    }

                    if (addedNodes.Count > 0) {
                        changeSelection(addedNodes, removedNodes);
                    }
                }
            }
            //if single click and holding on an already selected node, do nothing else
            else if (selectedNodes.Contains(currentNode)) return;
            //if right-clicking on a selected node, do nothing else
            else if (e.Button == MouseButtons.Right && selectedNodes.Contains(currentNode)) return;
            else {
                List<TreeNode> addedNodes = new();
                List<TreeNode> removedNodes = new();
                removedNodes.AddRange(selectedNodes);
                if (!removedNodes.Remove(currentNode)) {
                    addedNodes.Add(currentNode);
                }
                changeSelection(addedNodes, removedNodes);
                previousNode = currentNode;
            }
            //change contextmenus of nodes based on how many are selected
            //and what types are selected
            if (selectedNodes.Any(x => x.ImageKey == "folder") && selectedNodes.Any(x => x.ImageKey != "folder")) {
                foreach (TreeNode tn in selectedNodes) {
                    tn.ContextMenuStrip = contextMenuMulti;
                }
            }
            else {
                foreach (TreeNode tn in selectedNodes) {
                    if (tn.ImageKey is "folder")
                        tn.ContextMenuStrip = contextMenuFolderClick;
                    else if (tn.ImageKey is "project")
                        tn.ContextMenuStrip = contextMenuProject;
                    else {
                        tn.ContextMenuStrip = contextMenuFileClick;
                    }
                }
            }
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            TreeNode currentNode = treeView1.GetNodeAt(e.Location);
            bool control = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;

            if (control || shift || currentNode == null)
                return;
            /*
            if (currentNode == previousNodeMouseUp && e.Button != MouseButtons.Right) {
                currentNode.BeginEdit();
                return;
            }
            previousNodeMouseUp = currentNode;*/
            if (e.Button == MouseButtons.Right)
                return;

            List<TreeNode> addedNodes = new();
            List<TreeNode> removedNodes = new();
            removedNodes.AddRange(selectedNodes);
            if (!removedNodes.Remove(currentNode)) {
                addedNodes.Add(currentNode);
            }
            changeSelection(addedNodes, removedNodes);
            previousNode = currentNode;
        }
        #endregion
        #region Drag Drop node moving
        ///
        /// Drag Drop file moving
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TCLE.DragSource = "FileExplorer";
            DoDragDrop(e.Item, DragDropEffects.Copy);
            TCLE.DragSource = "none";
        }
        private void treeView1_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Copy;

        private TreeNode previousDragOver;
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (previousDragOver != targetNode && previousDragOver != null) {
                if (selectedNodes.Contains(previousDragOver))
                    previousDragOver.BackColor = Color.FromArgb(56, 56, 56);
                else
                    previousDragOver.BackColor = treeView1.BackColor;
            }
            if (targetNode != null && targetNode != previousDragOver) {
                targetNode.BackColor = Color.FromArgb(64, 53, 130);
                previousDragOver = targetNode;
            }
        }
        private void treeView1_DragLeave(object sender, EventArgs e)
        {
            if (previousDragOver == null)
                return;
            previousDragOver.BackColor = treeView1.BackColor;
        }
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode dragdropnode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (dragdropnode == null)
                return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
            // Don't allow drag to non-folders
            if (targetNode == null)
                return;
            if (targetNode.ImageKey is not "folder" and not "project") {
                if (selectedNodes.Contains(targetNode))
                    previousDragOver.BackColor = Color.FromArgb(56, 56, 56);
                else
                    previousDragOver.BackColor = treeView1.BackColor;
                return;
            }
            // Can't move a source to itself
            if (selectedNodes.Contains(targetNode)) {
                MessageBox.Show("Cannot move the selected items. The destination is included in the selection.", "Thumper Custom Level Editor");
                targetNode.BackColor = Color.FromArgb(56, 56, 56);
                return;
            }
            //check if destination contains any of the moved items
            //if so, cancel the whole operation
            foreach (TreeNode tn in selectedNodes) {
                string dest = $@"{ProjectExplorer.Files[targetNode].Folder.FullName}\{ProjectExplorer.Files[tn].Name}";
                if (File.Exists(dest) || Directory.Exists(dest)) {
                    MessageBox.Show($"Cannot move the item '{tn.Name}'. An item with that name already exists in the destination folder.", "Thumper Custom Level Editor");
                    targetNode.BackColor = Color.FromArgb(56, 56, 56);
                    return;
                }
            }
            //Finally, move each selected item to the destination
            string errorlog = "";
            foreach (TreeNode tn in selectedNodes) {
                string source = ProjectExplorer.Files[tn].FullPath;
                string dest = $@"{ProjectExplorer.Files[targetNode].Folder.FullName}\{ProjectExplorer.Files[tn].Name}";
                if (tn.ImageKey == "folder") {
                    if (TCLE.lockedfiles.Any(x => x.Key.FullName.Contains(source)))
                        errorlog += source + '\n';
                    else
                        Directory.Move(source, dest);
                }
                else {
                    if (TCLE.lockedfiles.Any(x => x.Key.FullName == source))
                        errorlog += source + '\n';
                    else
                        File.Move(source, dest);
                }

            }
            if (errorlog.Length > 1)
                MessageBox.Show($"Could not move these files/folders as they are currently open in a tab.\n{errorlog}", "Grumper Gustom Gevel Geditor");
            ProjectExplorer.CreateTreeView();
            // set destination folder backcolor back to normal to get rid of highlight
            targetNode.BackColor = treeView1.BackColor;
        }
        ///
        ///
        #endregion
        #region Functions and Methods (not event handlers)
        private void changeSelection(List<TreeNode> addedNodes, List<TreeNode> removedNodes)
        {
            foreach (TreeNode n in addedNodes) {
                n.BackColor = Properties.Settings.Default.ColorProjExpHighlight;
                selectedNodes.Add(n);
            }
            foreach (TreeNode n in removedNodes) {
                n.BackColor = treeView1.BackColor;
                selectedNodes.Remove(n);
            }
        }

        private static bool IsAChildOfOtherNodes(TreeNode nodetofind, TreeNode nodetosearch)
        {
            if (nodetosearch.Nodes.Contains(nodetofind))
                return true;
            foreach (TreeNode tn in nodetosearch.Nodes) {
                if (IsAChildOfOtherNodes(nodetofind, tn))
                    return true;
            }
            return false;
        }

        private static bool IsAChildOfOtherNodes(TreeNode nodetofind, List<TreeNode> nodetosearch)
        {
            foreach (TreeNode tn in nodetosearch) {
                if (tn.Nodes.Contains(nodetofind))
                    return true;
                if (IsAChildOfOtherNodes(nodetofind, tn))
                    return true;
            }
            return false;
        }
        #endregion

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (selectedNodes[0].FullPath == TCLE.WorkingFolder.Name)
                return;
            if (selectedNodes[0].ImageKey is "folder")
                return;
            TCLE.OpenFile(ProjectExplorer.Files[selectedNodes[0]].File);
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            if (btnOpenOnClick.Checked)
                TCLE.OpenFile(ProjectExplorer.Files[selectedNodes[0]].File);
        }
        /*
        private FileInfo GetFileOrFolderPath(string name)
        {
            return projectfiles.TryGetValue(name, out FileInfo file) ? file : new FileInfo(projectfolders[name].FullName);
        }
        */
        public static TreeNode FindNode(string path, TreeNodeCollection treeNodeCollection)
        {
            TreeNode found = null;
            foreach (TreeNode tn in treeNodeCollection) {
                if (tn.Text == path) {
                    found = tn;
                    break;
                }
                else
                    found = FindNode(path, tn.Nodes);
                if (found != null)
                    break;
            }

            return found;
        }

        public void FindDuplicateFile(TreeNode firstnode, Color color)
        {
            //check for same name
            TreeNode samename = FindNode(firstnode.Text, treeView1.Nodes[0].Nodes);
            if (samename != null) {
                samename.ForeColor = color;
                firstnode.ForeColor = color;
            }
        }

        private void toolstripProjectAddLeaf_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(new Form_LeafEditor().SaveAs(true));
            ProjectExplorer.CreateTreeView();
        }

        private void toolstripProjectAddLvl_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(new Form_LvlEditor().SaveAs(true));
            ProjectExplorer.CreateTreeView();
        }

        private void toolstripProjectAddGate_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(new Form_GateEditor().SaveAs(true));
            ProjectExplorer.CreateTreeView();
        }

        private void toolstripProjectAddMaster_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(new Form_MasterEditor().SaveAs(true));
            ProjectExplorer.CreateTreeView();
        }

        private void toolstripProjectAddSample_Click(object sender, EventArgs e)
        {
            TCLE.OpenFile(new Form_SampleEditor().SaveAs(true));
            ProjectExplorer.CreateTreeView();
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newfolders = ProjectExplorer.Files.Where(x => x.Key.Text.Contains("New Folder")).Count();
            TCLE.WorkingFolder.CreateSubdirectory($"New Folder{(newfolders > 1 ? $" {newfolders}" : "")}");
            ProjectExplorer.CreateTreeView();
        }

        public void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorProjectExplorerBG;
            toolstripExplorer.BackColor = Properties.Settings.Default.ColorProjectExplorerBG;
            treeView1.BackColor = Properties.Settings.Default.ColorProjectExplorerBG;
            ProjectExplorer.CreateTreeView();
        }

        private void filterLeaf_Click(object sender, EventArgs e)
        {

        }
    }
}
