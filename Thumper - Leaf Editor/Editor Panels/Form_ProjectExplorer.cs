using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        private TCLE _mainform { get; set; }
        public Form_ProjectExplorer(TCLE form, string projectfolder)
        {
            _mainform = form;
            InitializeComponent();
            //set custom renderer for some controls
            toolstripExplorer.Renderer = new ToolStripOverride();
            contextMenuAddFile.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFilters.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFileClick.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuFolderClick.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuMulti.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            //add events to some controls
            treeView1.NodeMouseClick += (sender, args) => treeView1.SelectedNode = args.Node;
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
            //populate treeview on first load
            CreateTreeView();
        }
        #endregion

        bool filterenabled = false;
        bool filtersearch = false;
        DirectoryInfo projectfolder = new DirectoryInfo(@"C:\Users\austin.peters\source\repos\Thumper-Custom-Level-Editor\Thumper - Leaf Editor\bin\Debug\test");
        List<TreeNode> filestocopy;
        bool cutfile;

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

        ///
        ///Filters and Search handling
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
        ///
        ///

        private void btnCollapse_Click(object sender, EventArgs e) => treeView1.CollapseAll();
        private void btnExpand_Click(object sender, EventArgs e) => treeView1.ExpandAll();
        private void btnRefresh_Click(object sender, EventArgs e) => CreateTreeView();
        private void copyFilePathToolStripMenuItem1_Click(object sender, EventArgs e) => Clipboard.SetText(GetNodeFilePath(selectedNodes[0]));
        private void toolstripFolderExplorer_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in selectedNodes) {
                if (Directory.Exists(GetNodeFilePath(tn)))
                    Process.Start(GetNodeFilePath(tn));
            }
        }
        private void toolstripFileExternal_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in selectedNodes) {
                if (File.Exists(GetNodeFilePath(tn)))
                    Process.Start(GetNodeFilePath(tn));
            }
        }

        ///
        ///File Delete handling
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


        }
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) toolstripFileDelete_Click(null, null);
        }
        ///
        ///

        ///
        ///Copy and Cut Handling
        private void toolstripFileCopy_Click(object sender, EventArgs e)
        {
            filestocopy = selectedNodes;
            toolstripFolderPaste.Enabled = true;
            if ((sender as ToolStripItem).Text == "Cut")
                cutfile = true;
        }
        ///
        ///

        ///
        ///Paste handling
        private void toolstripFolderPaste_Click(object sender, EventArgs e)
        {
            /*
            if (!File.Exists(filetocopy) && !Directory.Exists(filetocopy)) {
                MessageBox.Show($"'{filetocopy}' does not exist.", "Thumper Custom Level Editor");
                return;
            }
            //this is the node being pasted to
            affectednodes.Add(treeView1.SelectedNode);

            if (cutfile) {
                if (copiedpathisfile)
                    File.Move(filetocopy, $@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}");
                else
                    Directory.Move(filetocopy, $@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}");
                cutfile = false;
                filetocopy = "";
                toolstripFolderPaste.Enabled = false;
            }
            else {
                if (copiedpathisfile) {
                    if (File.Exists($@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}")) {
                        MessageBox.Show($"'{Path.GetFileName(filetocopy)}' exists in that location already.", "Thumper Custom Level Editor");
                        return;
                    }
                    File.Copy(filetocopy, $@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}");
                }
                else {
                    if (Directory.Exists($@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}")) {
                        MessageBox.Show($"'{Path.GetFileName(filetocopy)}' exists in that location already.", "Thumper Custom Level Editor");
                        return;
                    }
                    TCLE.CopyDirectory(filetocopy, $@"{selectednodefilepath}\{Path.GetFileName(filetocopy)}", true);
                }
            }
            */
            foreach (TreeNode tn in filestocopy) {
                if (cutfile) {
                    if (tn.ImageKey == "folder") {
                        //Directory.Move();
                    }
                }
            }
            CreateTreeView();
        }
        ///
        ///

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // cancel selection, the selection will be handled in MouseDown
            e.Cancel = true;
        }

        List<TreeNode> selectedNodes = new();
        TreeNode previousNode;
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode currentNode = treeView1.GetNodeAt(e.Location);
            if (currentNode == null) return;
            if (e.Button != MouseButtons.Right) {
                //currentNode.BackColor = treeView1.BackColor;
                //currentNode.ForeColor = treeView1.ForeColor;
            }

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
                    if (tn.ImageKey == "folder")
                        tn.ContextMenuStrip = contextMenuFolderClick;
                    else
                        tn.ContextMenuStrip = contextMenuFileClick;
                }
            }
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            TreeNode currentNode = treeView1.GetNodeAt(e.Location);
            if (currentNode == null) return;
            if (selectedNodes.Contains(currentNode)) return;
            if (e.Button != MouseButtons.Right) {
                //currentNode.BackColor = treeView1.BackColor;
                //currentNode.ForeColor = treeView1.ForeColor;
            }

            bool control = (ModifierKeys == Keys.Control);
            bool shift = (ModifierKeys == Keys.Shift);*/
        }

        protected void changeSelection(List<TreeNode> addedNodes, List<TreeNode> removedNodes)
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

        private void contextMenuFileClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripFileRename.Enabled = selectedNodes.Count == 1;
            toolstripFileCopyPath.Visible = selectedNodes.Count == 1;
        }
        private void contextMenuFolderClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripFolderRename.Enabled = selectedNodes.Count == 1;
            toolstripFolderNew.Enabled = selectedNodes.Count == 1;
            toolstripFolderCopyPath.Visible = selectedNodes.Count == 1;
        }


        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) => DoDragDrop(e.Item, DragDropEffects.Move);
        private void treeView1_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        TreeNode previousDragOver = null;
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
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
            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            // Confirm that the node at the drop location is not 
            // the dragged node and that target node isn't null
            // (for example if you drag outside the control)
            if (!draggedNode.Equals(targetNode) && targetNode != null) {
                // Remove the node from its current 
                // location and add it to the node at the drop location.
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
                // Expand the node at the location 
                // to show the dropped node.
                targetNode.Expand();
            }
            targetNode.BackColor = treeView1.BackColor;
        }

        private string GetNodeFilePath(TreeNode _node)
        {
            return $@"{Path.GetDirectoryName(projectfolder.FullName)}\{_node.FullPath}";
        }
    }
}
