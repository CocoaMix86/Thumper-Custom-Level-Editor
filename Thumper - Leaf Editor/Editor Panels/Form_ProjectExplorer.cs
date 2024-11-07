using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        private TCLE _mainform { get; set; }
        public Form_ProjectExplorer(TCLE form, string _projectfolder)
        {
            _mainform = form;
            projectfolder = new DirectoryInfo(_projectfolder);
            InitializeComponent();
            //set custom renderer for some controls
            toolstripExplorer.Renderer = new ToolStripOverride();
            contextMenuAddFile.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFilters.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFileClick.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFolderClick.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuMulti.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            //add events to some controls
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
            //populate treeview on first load
            CreateTreeView();
        }
        public Form_ProjectExplorer(string _projectfolder)
        {
            projectfolder = new DirectoryInfo(_projectfolder);
            CreateTreeView();
        }
        #endregion
        #region Variables
        bool cutfile;
        bool dontcancelifrename = false;
        bool filterenabled = false;
        bool filtersearch = false;
        string renamefile;
        string renamenode;
        string[] notallowedchars = new string[] { "/", "?", ":", "&", "\\", "*", "\"", "<", ">", "|", "#", "%" };
        DirectoryInfo projectfolder;
        TreeNode previousNode;
        List<TreeNode> filestocopy;
        List<TreeNode> selectedNodes = new();
        #endregion
        #region Create Tree
        private void CreateTreeView()
        {
            //clear existing treeview
            treeView1.Nodes.Clear();
            if (projectfolder.Exists) {
                //Build the tree
                BuildTree(projectfolder, treeView1.Nodes);
                //the root of the tree needs different properties
                TreeNode ProjectRoot = treeView1.Nodes[0];
                ProjectRoot.ImageKey = "project";
                ProjectRoot.SelectedImageKey = "project";
                ProjectRoot.NodeFont = new Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Bold);
                ProjectRoot.ContextMenuStrip = contextMenuFolderClick;
                //ProjectRoot.Text = $"Project '{ProjectRoot.Text}'";
            }
            //if using filters or search, expand all folders to show all results
            if (filterenabled || filtersearch)
                treeView1.ExpandAll();
            //otherwise expand root only
            else
                treeView1.Nodes[0].Expand();
        }
        private void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {
            ///BuildTree is a recursive function.
            //the very first node every time this function is called is a folder.
            TreeNode folder = new TreeNode() {
                Text = directoryInfo.Name,
                Name = directoryInfo.Name,
                ImageKey = "folder",
                SelectedImageKey = "folder",
                ContextMenuStrip = contextMenuFolderClick
            };
            addInMe.Add(folder);

            //Build subtree for each folder inside this folder
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories()) {
                BuildTree(subdir, folder.Nodes);
            }

            //add each file inside the folder to the tree
            foreach (FileInfo file in directoryInfo.GetFiles()) {
                TreeNode _tn = new TreeNode() {
                    Text = file.Name,
                    Name = file.Name,
                    ImageKey = file.Name.Split('_')[0],
                    SelectedImageKey = file.Name.Split('_')[0],
                    ContextMenuStrip = contextMenuFileClick
                };
                /*
                if (file.Name.Contains("xfm_") || file.Name.Contains("spn_") || file.Name.Contains("config_") || file.Name.Contains(".color"))
                    continue;*/
                //check for various filters being used
                if (filtersearch) {
                    if (file.Name.Contains(txtSearch.Text))
                        folder.Nodes.Add(_tn);
                }
                else if (!filterenabled)
                    folder.Nodes.Add(_tn);
                else if ((filterLeaf.Checked && file.Name.Contains("leaf_")) || (filterLvl.Checked && file.Name.Contains("lvl_")) || (filterGate.Checked && file.Name.Contains("gate_")) || (filterMaster.Checked && file.Name.Contains("master_")) || (filterSample.Checked && file.Name.Contains("samp_")))
                    folder.Nodes.Add(_tn);
            }
        }
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
        private void filter_CheckChanged(object sender, EventArgs e) => CreateTreeView();
        private void btnFilter_ButtonClick(object sender, EventArgs e)
        {
            filterenabled = !filterenabled;
            //this style button doesn't have a Checked state, so we change its backcolor to show its enabled or not
            btnFilter.BackColor = filterenabled ? Color.LightBlue : Color.FromArgb(35, 35, 35);
            //recreate the tree when filter state changes
            CreateTreeView();
        }

        private void contextMenuFilters_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            //this prevents the filter menu from closing when an option is chosen, allowing to select multiple before exiting the menu
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search Project Explorer (Ctrl+;)")
                txtSearch.Text = "";
        }

        private void txtSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
                txtSearch.Text = "Search Project Explorer (Ctrl+;)";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filtersearch = txtSearch.Text is not "" and not "Search Project Explorer (Ctrl+;)";
            CreateTreeView();
        }

        private void btnCollapse_Click(object sender, EventArgs e) => treeView1.CollapseAll();
        private void btnExpand_Click(object sender, EventArgs e) => treeView1.ExpandAll();
        private void btnRefresh_Click(object sender, EventArgs e) => CreateTreeView();
        #endregion
        #region Context Menu File
        private void contextMenuFileClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //depending on number of items selected, alter the contextmenu
            toolstripFileRename.Enabled = selectedNodes.Count == 1;
            toolstripFileCopyPath.Visible = selectedNodes.Count == 1;
        }
        private void copyFilePathToolStripMenuItem1_Click(object sender, EventArgs e) => Clipboard.SetText(GetNodeFilePath(selectedNodes[0]));
        private void toolstripFileExternal_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in selectedNodes) {
                if (File.Exists(GetNodeFilePath(tn)))
                    Process.Start(GetNodeFilePath(tn));
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
                string source = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{tn.FullPath}";
                if (tn.ImageKey == "folder" && Directory.Exists(source)) {
                    Directory.Delete(source, true);
                }
                else if (File.Exists(source))
                    File.Delete(source);
                tn.Remove();
            }
        }
        private void toolstripFileCopy_Click(object sender, EventArgs e)
        {
            filestocopy = selectedNodes.Cast<TreeNode>().ToList();
            toolstripFolderPaste.Enabled = true;
            if ((sender as ToolStripItem).Text == "Cut")
                cutfile = true;
        }
        #region Rename Handling
        private void toolstripFileRename_Click(object sender, EventArgs e)
        {
            dontcancelifrename = true;
            treeView1.SelectedNode = selectedNodes[0];
            selectedNodes[0].BeginEdit();
            treeView1.SelectedNode = null;
            dontcancelifrename = false;
        }
        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            renamefile = e.Node.FullPath;
            renamenode = e.Node.Name;
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
                node.Text = renamenode;
                return;
            }
            string source = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{renamefile}";
            string dest = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{node.FullPath}";
            //check if same name
            if (renamefile == node.FullPath) {
                return;
            }
            //check if name exists already
            if (File.Exists(dest) || Directory.Exists(dest)) {
                MessageBox.Show($"A file or folder with the name '{node.Text}' already exists on\ndisk at this location. Please choose another name.", "Thumper Custom Level Editor");
                node.Text = renamenode;
                return;
            }
            //check for changing file extension
            if (node.ImageKey != "folder" && Path.GetExtension(source) != Path.GetExtension(dest)) {
                if (MessageBox.Show("If you chaneg a file name extension, the file may become\nunusable. Are you sure you want to change it?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    node.Text = renamenode;
                    return;
                }
            }
            //move the folder or file
            if (node.ImageKey == "folder") {
                Directory.Move(source, dest);
            }
            else {
                File.Move(source, dest);
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
                if (Directory.Exists(GetNodeFilePath(tn)))
                    Process.Start(GetNodeFilePath(tn));
            }
        }
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
                string source = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{tn.FullPath}";
                string dest = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{targetnode.FullPath}\{tn.Name}";
                //check if the destination is within the copied node. If it is, skip this node.
                if (IsAChildOfOtherNodes(targetnode, tn)) {
                    MessageBox.Show($"Item '{tn.Name}' not pasted as it contains the destination.", "Thumper Custom Level Editor");
                    continue;
                }
                //check if each node exists at the destination and ask to overwrite it. If' No', skip this node.
                if (targetnode.Nodes.Contains(tn)) {
                    if (MessageBox.Show($"Item '{tn.Name}' already exists at the destination. Overwrite it?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No)
                        continue;
                }

                if (cutfile) {
                    if (tn.ImageKey == "folder" && Directory.Exists(source)) {
                        Directory.Move(source, dest);
                        tn.Remove();
                    }
                    else if (File.Exists(source)) {
                        File.Move(source, dest);
                        tn.Remove();
                    }
                    cutfile = false;
                    toolstripFolderPaste.Enabled = false;
                }
                else {
                    if (tn.ImageKey == "folder" && Directory.Exists(source)) {
                        TCLE.CopyDirectory(source, dest, true);
                    }
                    else if (File.Exists(source)) {
                        File.Copy(source, dest);
                    }
                }
            }
            targetnode.Expand();
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

            bool control = (ModifierKeys == Keys.Control);
            bool shift = (ModifierKeys == Keys.Shift);

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
                if (removedNodes.Contains(currentNode)) {
                    removedNodes.Remove(currentNode);
                }
                else {
                    addedNodes.Add(currentNode);
                }
                changeSelection(addedNodes, removedNodes);
                previousNode = currentNode;
            }
            //change contextmenus of nodes based on how many are selected
            //and what types are selected
            if (selectedNodes.Where(x => x.ImageKey == "folder").Count() > 0 && selectedNodes.Where(x => x.ImageKey != "folder").Count() > 0) {
                foreach (TreeNode tn in selectedNodes) {
                    tn.ContextMenuStrip = contextMenuMulti;
                }
            }
            else {
                foreach (TreeNode tn in selectedNodes) {
                    if (tn.ImageKey is "folder" or "project")
                        tn.ContextMenuStrip = contextMenuFolderClick;
                    else
                        tn.ContextMenuStrip = contextMenuFileClick;
                }
            }
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            TreeNode currentNode = treeView1.GetNodeAt(e.Location);
            bool control = (ModifierKeys == Keys.Control);
            bool shift = (ModifierKeys == Keys.Shift);

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
            if (removedNodes.Contains(currentNode)) {
                removedNodes.Remove(currentNode);
            }
            else {
                addedNodes.Add(currentNode);
            }
            changeSelection(addedNodes, removedNodes);
            previousNode = currentNode;
        }
        #endregion
        #region Drag Drop node moving
        ///
        /// Drag Drop file moving
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) => DoDragDrop(e.Item, DragDropEffects.Move);
        private void treeView1_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        TreeNode previousDragOver = null;
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
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
            // Don't allow drag to non-folders
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
                string dest = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{targetNode.FullPath}\{tn.Name}";
                if (File.Exists(dest) || Directory.Exists(dest)) {
                    MessageBox.Show($"Cannot move the item '{tn.Name}'. An item with that name already exists in the destination folder.", "Thumper Custom Level Editor");
                    targetNode.BackColor = Color.FromArgb(56, 56, 56);
                    return;
                }
            }
            //Finally, move each selected item to the destination
            foreach (TreeNode tn in selectedNodes) {
                string source = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{tn.FullPath}";
                string dest = $@"{Path.GetDirectoryName(projectfolder.FullName)}\{targetNode.FullPath}\{tn.Name}";
                if (tn.ImageKey == "folder") {
                    Directory.Move(source, dest);
                }
                else {
                    File.Move(source, dest);
                }

                tn.Remove();
                targetNode.Nodes.Add(tn);
                targetNode.Expand();
            }
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
                n.BackColor = Color.FromArgb(56, 56, 56);
                n.ForeColor = Color.White;
                selectedNodes.Add(n);
            }
            foreach (TreeNode n in removedNodes) {
                n.BackColor = treeView1.BackColor;
                n.ForeColor = treeView1.ForeColor;
                selectedNodes.Remove(n);
            }
        }

        private string GetNodeFilePath(TreeNode _node)
        {
            return $@"{Path.GetDirectoryName(projectfolder.FullName)}\{_node.FullPath}";
        }

        private bool IsAChildOfOtherNodes(TreeNode nodetofind, TreeNode nodetosearch)
        {
            if (nodetosearch.Nodes.Contains(nodetofind))
                return true;
            foreach (TreeNode tn in nodetosearch.Nodes) {
                if (IsAChildOfOtherNodes(nodetofind, tn))
                    return true;
            }
            return false;
        }

        private bool IsAChildOfOtherNodes(TreeNode nodetofind, List<TreeNode> nodetosearch)
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
    }
}
