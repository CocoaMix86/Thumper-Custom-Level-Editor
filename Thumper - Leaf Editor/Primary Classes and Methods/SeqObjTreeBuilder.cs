using System.Collections.Generic;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor 
{ 
    public static class SeqObjTreeBuilder
    {
        public static TreeView GlobalObjectTree = new();
        public static List<TreeNode> GlobalObjectTreeNodes = new();
        //
        public static ContextMenuStrip contextMenuFav = new();
        public static ToolStripMenuItem toolStripFavAdd = new();
        public static ContextMenuStrip contextMenuFavRemove = new();
        public static ToolStripMenuItem toolStripFavRemove = new();
        public static ContextMenuStrip contextMenuFavClear = new();
        public static ToolStripMenuItem toolStripFavClear = new();

        public static void Initialize()
        {
            BuildObjectTree(GlobalObjectTree, "");
            // 
            // contextMenuFav
            // 
            contextMenuFav.BackColor = Color.FromArgb(46, 46, 46);
            contextMenuFav.Items.AddRange(new ToolStripItem[] { toolStripFavAdd });
            contextMenuFav.Name = "workingfolderRightClick";
            contextMenuFav.RenderMode = ToolStripRenderMode.System;
            contextMenuFav.Size = new Size(162, 26);
            // 
            // toolStripFavAdd
            // 
            toolStripFavAdd.ForeColor = Color.White;
            toolStripFavAdd.Image = Properties.Resources.icon_fav;
            toolStripFavAdd.Name = "toolStripFavAdd";
            toolStripFavAdd.Size = new Size(161, 22);
            toolStripFavAdd.Text = "Add To Favorites";
            toolStripFavAdd.Click += toolStripFavAdd_Click;
            // 
            // contextMenuFavRemove
            // 
            contextMenuFavRemove.BackColor = Color.FromArgb(46, 46, 46);
            contextMenuFavRemove.Items.AddRange(new ToolStripItem[] { toolStripFavRemove });
            contextMenuFavRemove.Name = "workingfolderRightClick";
            contextMenuFavRemove.RenderMode = ToolStripRenderMode.System;
            contextMenuFavRemove.Size = new Size(199, 26);
            // 
            // toolStripFavRemove
            // 
            toolStripFavRemove.ForeColor = Color.White;
            toolStripFavRemove.Image = Properties.Resources.icon_remove2;
            toolStripFavRemove.Name = "toolStripFavRemove";
            toolStripFavRemove.Size = new Size(198, 22);
            toolStripFavRemove.Text = "Remove From Favorites";
            toolStripFavRemove.Click += toolStripFavRemove_Click;
            // 
            // contextMenuFavClear
            // 
            contextMenuFavClear.BackColor = Color.FromArgb(46, 46, 46);
            contextMenuFavClear.Items.AddRange(new ToolStripItem[] { toolStripFavClear });
            contextMenuFavClear.Name = "workingfolderRightClick";
            contextMenuFavClear.RenderMode = ToolStripRenderMode.System;
            contextMenuFavClear.Size = new Size(152, 26);
            // 
            // toolStripFavClear
            // 
            toolStripFavClear.ForeColor = Color.White;
            toolStripFavClear.Image = Properties.Resources.icon_remove2;
            toolStripFavClear.Name = "toolStripFavClear";
            toolStripFavClear.Size = new Size(151, 22);
            toolStripFavClear.Text = "Clear Favorites";
            toolStripFavClear.Click += toolStripFavClear_Click;
            //
            contextMenuFav.Renderer = new ContextMenuColors();
            contextMenuFavClear.Renderer = new ContextMenuColors();
            contextMenuFavRemove.Renderer = new ContextMenuColors();
        }

        public static void BuildObjectTree(TreeView _tree, string txtSearch)
        {
            _tree.Nodes.Clear();
            //Add Favorites right at the top
            TreeNode fav = new() {
                Text = "*FAVORITES*",
                ImageKey = "fav",
                SelectedImageKey = "fav",
                ContextMenuStrip = contextMenuFavClear
            };
            _tree.Nodes.Add(fav);
            BuildTreeFavorites(_tree, txtSearch);

            //make each category of objects its own node
            foreach (string category in TCLE.LeafObjects.Select(x => x.Value.category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = $"{category.ToUpper().Replace("/", "")}.png",
                    SelectedImageKey = $"{category.ToUpper().Replace("/", "")}.png"
                };
                if (category == "PLAY SAMPLE") {
                    //samples are not stored in LeafObjects, so we loop over a different list to find them
                    //seperate samples into sub-nodes by the file they came from
                    foreach (string file in TCLE.ProjectSamples.Select(x => x.File?.Name).Distinct()) {
                        if (string.IsNullOrEmpty(file))
                            continue;
                        TreeNode sampfile = new() {
                            Text = file,
                            ImageKey = "samp",
                            SelectedImageKey = "samp"
                        };
                        foreach (SampleData samp in TCLE.ProjectSamples.Where(x => x.File?.Name == file)) {
                            TreeNode _param = new() {
                                Text = samp.obj_name,
                                ImageKey = "none",
                                SelectedImageKey = "none",
                                ToolTipText = $"Pitch: {samp.pitch}\nPan: {samp.pan}\nOffset: {samp.offset}\nSelect sample and then hold SPACE to play it",
                                Tag = "sample.samp;play"
                            };
                            sampfile.Nodes.Add(_param);
                        }
                         _node.Nodes.Add(sampfile);
                    }
                }
                else {
                    //each object becomes its own node
                    foreach (Object_Params obj in TCLE.LeafObjects.Where(x => x.Value.category == category).Select(x => x.Value)) {
                        TreeNode _param = new() {
                            Text = obj.param_displayname,
                            ImageKey = obj.favorite ? "fav" : $"{obj.defaultcolor.ToArgb()}",
                            SelectedImageKey = obj.favorite ? "fav" : $"{obj.defaultcolor.ToArgb()}",
                            ContextMenuStrip = obj.favorite ? contextMenuFavRemove : contextMenuFav,
                            Tag = obj.obj_name + ";" + obj.param_path
                        };
                        _node.Nodes.Add(_param);
                    }
                }
                _tree.Nodes.Add(_node);
            }
        }

        public static void BuildTreeFavorites(TreeView _tree, string txtSearch)
        {
            bool filtersearch = txtSearch is not "" and not "Search Objects (Ctrl+;)";

            _tree.Nodes[0].Nodes.Clear();
            foreach (Object_Params obj in TCLE.LeafObjects.Values.Where(x => x.favorite).Order()) {
                TreeNode _param = new() {
                    Text = obj.param_displayname,
                    ImageKey = "fav",
                    SelectedImageKey = "fav",
                    ContextMenuStrip = contextMenuFavRemove,
                    Tag = obj.obj_name + ";" + obj.param_path
                };
                if ((filtersearch && _param.Text.Contains(txtSearch)) || !filtersearch)
                    _tree.Nodes[0].Nodes.Add(_param);
            }
        }

        public static void FilterTree(TreeView _tree, string txtSearch)
        {
            List<string> ExpandNodes = _tree.Nodes.Cast<TreeNode>().Where(x => x.IsExpanded).Select(x => x.Text).ToList();
            _tree.Nodes.Clear();
            List<TreeNode> filternodes = GlobalObjectTree.Nodes.Cast<TreeNode>().Select(x => (TreeNode)x.Clone()).ToList();
            if (txtSearch is not "" and not "Search Objects (Ctrl+;)") {
                for (int x = 0; x < filternodes.Count; x++) {
                    if (!FilterNode(filternodes[x], txtSearch)) {
                        filternodes.RemoveAt(x);
                        x--;
                    }
                }
            }
            _tree.Nodes.AddRange(filternodes.ToArray());
            _tree.Refresh();
            if (txtSearch is not "" and not "Search Objects (Ctrl+;)")
                _tree.ExpandAll();
            else {
                foreach (TreeNode tn in _tree.Nodes) {
                    if (ExpandNodes.Contains(tn.Text))
                        tn.Expand();
                }
                _tree.Nodes[0].Expand();
            }
        }

        public static void UpdateFavorites(TreeView _tree)
        {
            bool expand = _tree.Nodes[0].IsExpanded;
            _tree.Nodes[0] = (TreeNode)GlobalObjectTree.Nodes[0].Clone();
            if (expand)
                _tree.Nodes[0].Expand();
        }

        public static bool FilterNode(TreeNode _node, string txtSearch)
        {
            if (_node.Nodes.Count == 0) {
                if (_node.Text.Contains(txtSearch))
                    return true;
                return false;
            }
            bool keepthisnode = false;
            for (int x = 0; x < _node.Nodes.Count; x++) {
                bool found = FilterNode(_node.Nodes[x], txtSearch);
                if (found)
                    keepthisnode = true;
                else {
                    _node.Nodes[x].Remove();
                    x--;
                }
            }
            if (keepthisnode) 
                return true;
            return false;            
        }

        public static TreeNode FindNode(string search, TreeNodeCollection nodes)
        {
            TreeNode foundnode = null;
            foreach (TreeNode tn in nodes) {
                if (tn.Text == search)
                    return tn;
                foundnode = FindNode(search, tn.Nodes);
                if (foundnode != null)
                    break;
            }
            return foundnode;
        }

        public static void toolStripFavAdd_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            if (Source.SelectedNode.ImageKey == "fav")
                return;
            TCLE.LeafObjects[(string)Source.SelectedNode.Tag].favorite = true;
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            UtilAudio.PlaySound("UIselect");

            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor)))
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.treeObjects.Tag.ToString());
        }

        public static void toolStripFavRemove_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            TCLE.LeafObjects[(string)Source.SelectedNode.Tag].favorite = false;
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            UtilAudio.PlaySound("UIselect");

            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor)))
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.treeObjects.Tag.ToString());
        }

        public static void toolStripFavClear_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            foreach (Object_Params obj in TCLE.LeafObjects.Values)
                obj.favorite = false;
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            UtilAudio.PlaySound("UIdelete");

            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor)))
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.treeObjects.Tag.ToString());
        }
    }
}
