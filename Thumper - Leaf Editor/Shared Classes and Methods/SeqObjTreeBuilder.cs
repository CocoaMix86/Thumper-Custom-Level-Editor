using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor 
{ 
    public static class SeqObjTreeBuilder
    {
        public static TreeView GlobalObjectTree = new();
        public static List<TreeNode> GlobalObjectTreeNodes = new();

        public static void Initialize()
        {
            BuildObjectTree(GlobalObjectTree, "");
        }

        public static void BuildObjectTree(TreeView _tree, string txtSearch)
        {
            _tree.Nodes.Clear();
            //Add Favorites right at the top
            TreeNode fav = new() {
                Text = "*FAVORITES*",
                ImageKey = "fav",
                SelectedImageKey = "fav",
                ContextMenuStrip = Form_LeafEditor.ContextMenuFavClear
            };
            _tree.Nodes.Add(fav);
            BuildTreeFavorites(_tree, txtSearch);
            if (fav.Nodes.Count == 0)
                fav.Remove();

            //make each category of objects its own node
            foreach (string category in TCLE.LeafObjects.Select(x => x.category).Distinct().Order()) {
                TreeNode _node = new() {
                    Text = category.ToUpper(),
                    ImageKey = "category",
                    SelectedImageKey = "category"
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
                            };
                            sampfile.Nodes.Add(_param);
                        }
                         _node.Nodes.Add(sampfile);
                    }
                }
                else {
                    //each object becomes its own node
                    foreach (Object_Params obj in TCLE.LeafObjects.Where(x => x.category == category)) {
                        TreeNode _param = new() {
                            Text = obj.param_displayname,
                            ImageKey = TCLE.ObjectFavorites.Contains(obj) ? "fav" : "none",
                            SelectedImageKey = TCLE.ObjectFavorites.Contains(obj) ? "fav" : "none",
                            ContextMenuStrip = Form_LeafEditor.ContextMenuFav
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
            foreach (string obj in TCLE.ObjectFavorites.Select(x => x.param_displayname).Order()) {
                TreeNode _param = new() {
                    Text = obj,
                    ImageKey = "none",
                    SelectedImageKey = "none",
                    ContextMenuStrip = Form_LeafEditor.ContextMenuFavRemove
                };
                if ((filtersearch && _param.Text.Contains(txtSearch)) || !filtersearch)
                    _tree.Nodes[0].Nodes.Add(_param);
            }
        }

        public static void FilterTree(TreeView _tree, string txtSearch)
        {
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
    }
}
