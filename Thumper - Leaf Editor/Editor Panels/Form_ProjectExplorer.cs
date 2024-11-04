using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualBasic;

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
        string selectednodefilepath { get { return $@"{Path.GetDirectoryName(projectfolder.FullName)}\{treeView1.SelectedNode.FullPath}"; } }
        DirectoryInfo projectfolder = new DirectoryInfo(@"X:\Thumper\levels\Basics3");
        string filetocopy;
        bool copiedpathisfile;
        bool cutfile;
        List<TreeNode> affectednodes = new();

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
        private void copyFilePathToolStripMenuItem1_Click(object sender, EventArgs e) => Clipboard.SetText(selectednodefilepath);
        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(selectednodefilepath))
                Process.Start(selectednodefilepath);
            else {
                MessageBox.Show("That folder does not exist");
                //force refresh of treeview if the folder doesn't exist
                CreateTreeView();
            }
        }
        private void toolstripFileExternal_Click(object sender, EventArgs e) => Process.Start(selectednodefilepath);

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"'{treeView1.SelectedNode.Text}' will be deleted permanently", "Thumper Custom Level Editor", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                File.Delete(selectednodefilepath);
            }
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"'{treeView1.SelectedNode.Text}' and all its contents will be deleted permanently", "Thumper Custom Level Editor", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                Directory.Delete(selectednodefilepath, true);
            }
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {
                if (treeView1.SelectedNode.GetNodeCount(true) == 0) deleteToolStripMenuItem_Click(null, null);
                else toolStripMenuItem3_Click(null, null);
            }
        }

        private void toolstripFileCopy_Click(object sender, EventArgs e)
        {
            filetocopy = selectednodefilepath;
            toolstripFolderPaste.Enabled = true;
            copiedpathisfile = true;
            affectednodes.Clear();
            affectednodes.Add(treeView1.SelectedNode);
        }
        private void toolstripFolderCopy_Click(object sender, EventArgs e)
        {
            filetocopy = selectednodefilepath;
            toolstripFolderPaste.Enabled = true;
            copiedpathisfile = false;
            affectednodes.Clear();
            affectednodes.Add(treeView1.SelectedNode);
        }
        private void toolstripFileCut_Click(object sender, EventArgs e)
        {
            filetocopy = selectednodefilepath;
            toolstripFolderPaste.Enabled = true;
            cutfile = true;
            copiedpathisfile = true;
            affectednodes.Clear();
            affectednodes.Add(treeView1.SelectedNode);
        }
        private void toolstripFolderCut_Click(object sender, EventArgs e)
        {
            filetocopy = selectednodefilepath;
            toolstripFolderPaste.Enabled = true;
            cutfile = true;
            copiedpathisfile = false;
            affectednodes.Clear();
            affectednodes.Add(treeView1.SelectedNode);
        }
        private void toolstripFolderPaste_Click(object sender, EventArgs e)
        {
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

            CreateTreeView();
        }

    }
}
