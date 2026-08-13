using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor 
{ 
    public static class SeqObjTreeBuilder
    {
        public static Dictionary<string, TreeNode> ObjectNodes = new();
        public static Dictionary<string, TreeNode> CategoryNodes = new();
        public static Dictionary<string, TreeNode> SampleNodes = new();
        //
        public static ContextMenuStrip contextMenuFav = new();
        public static ToolStripMenuItem toolStripFavAdd = new();
        public static ContextMenuStrip contextMenuFavRemove = new();
        public static ToolStripMenuItem toolStripFavRemove = new();
        public static ContextMenuStrip contextMenuFavClear = new();
        public static ToolStripMenuItem toolStripFavClear = new();
        //
        public static TreeNode FavoritesNode = new() {
            Text = "*FAVORITES*",
            ImageKey = "fav",
            SelectedImageKey = "fav",
            ContextMenuStrip = contextMenuFavClear
        };

        public static void Initialize()
        {
            CreateRightclickMenus(contextMenuFav, toolStripFavAdd);
            CreateRightclickMenus(contextMenuFavRemove, toolStripFavRemove);
            CreateRightclickMenus(contextMenuFavClear, toolStripFavClear);
            //
            CreateMenuItems(toolStripFavAdd, Properties.Resources.icon_fav, "toolStripFavAdd", "Add To Favorites", toolStripFavAdd_Click);
            CreateMenuItems(toolStripFavRemove, Properties.Resources.icon_remove2, "toolStripFavRemove", "Remove From Favorites", toolStripFavRemove_Click);
            CreateMenuItems(toolStripFavClear, Properties.Resources.icon_remove2, "toolStripFavClear", "Clear Favorites", toolStripFavClear_Click);
            //
            BuildMasterObjectTree();
        }

        private static void CreateRightclickMenus(ContextMenuStrip Menu, ToolStripItem Item)
        {
            Menu.BackColor = Color.FromArgb(46, 46, 46);
            Menu.Items.Add(Item);
            Menu.Name = "workingfolderRightClick";
            Menu.RenderMode = ToolStripRenderMode.System;
            Menu.Size = new Size(152, 26);
            Menu.Renderer = new ContextMenuColors();
        }
        private static void CreateMenuItems(ToolStripItem Item, Image Icon, string Name, string Text, EventHandler Handler)
        {
            Item.ForeColor = Color.White;
            Item.Image = Icon;
            Item.Name = Name;
            Item.Size = new Size(151, 22);
            Item.Text = Text;
            Item.Click += Handler;
        }

        public static void BuildMasterObjectTree()
        {
            ObjectNodes.Clear();
            CategoryNodes.Clear();
            //Add Favorites right at the top
            BuildTreeFavorites();
            //BuildTreeFavorites(TreeToBuild, txtSearch);

            IOrderedEnumerable<IGrouping<string, DefaultSequencerObject>> categories = TCLE.LeafObjects.Values.GroupBy(x => x.Category).OrderBy(x => x.Key);
            //make each category of objects its own node
            foreach (IGrouping<string, DefaultSequencerObject>? category in categories) {
                TreeNode CategoryNode = BuildNode(category.Key.ToUpper(), $"{category.Key.ToUpper().Replace("/", "")}.png");
                CategoryNodes.Add(category.Key, CategoryNode);
                //
                if (category.Key == "PLAY SAMPLE") {
                    BuildSampleNodes();
                }
                else {
                    //each object becomes its own node
                    foreach (DefaultSequencerObject obj in category) {
                        TreeNode _param = BuildNode(obj.ParamDisplayName, obj.Favorite ? "fav" : $"{obj.DefaultColor.ToArgb()}", null, obj.Favorite ? contextMenuFavRemove : contextMenuFav, obj);
                        CategoryNode.Nodes.Add(_param);
                        ObjectNodes.Add(obj.Name + ";" + obj.ParamPath, _param);
                    }
                }
            }
        }

        public static void BuildSampleNodes()
        {
            CategoryNodes["PLAY SAMPLE"].Nodes.Clear();
            //
            //samples are not stored in LeafObjects, so we loop over a different list to find them
            //seperate samples into sub-nodes by the file they came from
            IEnumerable<IGrouping<string, SampleData>> sampleGroups = TCLE.ProjectSamples.Values.Where(x => x.File != null).GroupBy(x => x.File.Name);
            foreach (IGrouping<string, SampleData> file in sampleGroups) {
                if (string.IsNullOrEmpty(file.Key))
                    continue;
                TreeNode sampfile = BuildNode(file.Key, "samp");
                //ObjectNodes.Add(file.Key, sampfile);
                CategoryNodes["PLAY SAMPLE"].Nodes.Add(sampfile);

                foreach (SampleData? samp in file) {
                    TreeNode _param = BuildNode(samp.obj_name, "none", $"Pitch: {samp.pitch}\nPan: {samp.pan}\nOffset: {samp.offset}\nSelect sample and then hold SPACE to play it", null, "sample.samp;play");
                    sampfile.Nodes.Add(_param);
                    //ObjectNodes.Add(samp.obj_name, _param);
                }
            }
        }

        public static TreeNode BuildNode(string Text, string ImageKey, string ToolTip = null, ContextMenuStrip ContextMenu = null, object Tag = null)
        {
            return new TreeNode()
            {
                Text = Text,
                ImageKey = ImageKey,
                SelectedImageKey = ImageKey,
                ToolTipText = ToolTip,
                ContextMenuStrip = ContextMenu,
                Tag = Tag
            };
        }

        public static void BuildTreeFavorites()
        {
            //clear the favorites node
            FavoritesNode.Nodes.Clear();
            //get all favorites and sort alphabetically
            foreach (DefaultSequencerObject obj in TCLE.LeafObjects.Values.Where(x => x.Favorite).OrderBy(x => x.ParamDisplayName)) {
                TreeNode _param = new() {
                    Text = obj.ParamDisplayName,
                    ImageKey = "fav",
                    SelectedImageKey = "fav",
                    ContextMenuStrip = contextMenuFavRemove,
                    Tag = obj
                };
                FavoritesNode.Nodes.Add(_param);
            }
        }

        public static void SetNodeFavorite(TreeNode FavNode, bool Favorite)
        {
            DefaultSequencerObject obj = (DefaultSequencerObject)FavNode.Tag;
            obj.Favorite = Favorite;
            string ImageKey = Favorite ? "fav" : $"{obj.DefaultColor.ToArgb()}";

            FavNode.ImageKey = ImageKey;
            FavNode.SelectedImageKey = ImageKey;
            FavNode.ContextMenuStrip = Favorite ? contextMenuFavRemove : contextMenuFav;
            //also set the node in the master tree to show favorite star
            ObjectNodes[obj.Name + ";" + obj.ParamPath].ImageKey = ImageKey;
            ObjectNodes[obj.Name + ";" + obj.ParamPath].SelectedImageKey = ImageKey;
            ObjectNodes[obj.Name + ";" + obj.ParamPath].ContextMenuStrip = Favorite ? contextMenuFavRemove : contextMenuFav;
        }

        public static void FilterTree(TreeView _tree, string txtSearch, bool Startup = false)
        {
            bool filtersearch = !string.IsNullOrWhiteSpace(txtSearch) && txtSearch != "Search Objects (Ctrl+;)";
            //store which node names were expanded before we clear the list
            //HashSet<string> ExpandNodes = _tree.Nodes.Cast<TreeNode>().Where(x => x.IsExpanded).Select(x => x.Text).ToHashSet();
            _tree.BeginUpdate();
            _tree.Nodes.Clear();
            _tree.Nodes.Add(CloneObjectNode(FavoritesNode, true));
            //clone the existing tree, then filter out nodes that dont match the search string
            foreach (TreeNode Category in CategoryNodes.Values) {
                TreeNode _clonecategory = CloneCategoryNode(Category);
                bool PlaySample = _clonecategory.Text == "PLAY SAMPLE";

                foreach (TreeNode Object in Category.Nodes) {
                    if (filtersearch && !Object.Text.Contains(txtSearch))
                        continue;
                    _clonecategory.Nodes.Add(CloneObjectNode(Object, PlaySample));
                }
                if (_clonecategory.Nodes.Count > 0)
                    _tree.Nodes.Add(_clonecategory);
            }
            //re-expand the nodes
            if (filtersearch)
                _tree.ExpandAll();
            else {
                /*foreach (TreeNode tn in _tree.Nodes) {
                    if (ExpandNodes.Contains(tn.Text))
                        tn.Expand();
                }*/
                _tree.Nodes[0].Expand();
            }
            _tree.EndUpdate();
        }
        private static TreeNode CloneCategoryNode(TreeNode Node)
        {
            return new TreeNode
            {
                Text = Node.Text,
                ImageKey = Node.ImageKey,
                SelectedImageKey = Node.SelectedImageKey,
            };
        }
        private static TreeNode CloneObjectNode(TreeNode Node, bool CloneSubNodes)
        {
            TreeNode NewNode = new() {
                Text = Node.Text,
                ImageKey = Node.ImageKey,
                SelectedImageKey = Node.SelectedImageKey,
                ContextMenuStrip = Node.ContextMenuStrip,
                ToolTipText = Node.ToolTipText,
                Tag = Node.Tag
            };
            if (CloneSubNodes) {
                foreach (TreeNode Object in Node.Nodes) {
                    NewNode.Nodes.Add(CloneObjectNode(Object, false));
                }
            }
            return NewNode;
        }
        /*
        public static bool FilterNode(TreeNode _node, string txtSearch)
        {
            if (_node.Nodes.Count == 0) {
                return _node.Text.Contains(txtSearch, StringComparison.OrdinalIgnoreCase);
            }

            bool keepthisnode = false;
            for (int i = _node.Nodes.Count - 1; i >= 0; i--) {
                if (!FilterNode(_node.Nodes[i], txtSearch))
                    _node.Nodes[i].Remove();
                else
                    keepthisnode = true;
            }

            return keepthisnode;
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
        */
        public static void UpdateTrees(bool IsDelete)
        {
            //SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            BuildTreeFavorites();
            UtilAudio.PlaySound(IsDelete ? "UIdelete" : "UIselect");

            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>())
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.treeObjects.Tag.ToString());
        }

        public static void toolStripFavAdd_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            if (Source.SelectedNode.ImageKey == "fav")
                return;
            SetNodeFavorite(Source.SelectedNode, true);
            UpdateTrees(false);
        }

        public static void toolStripFavRemove_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            SetNodeFavorite(Source.SelectedNode, false);
            UpdateTrees(false);
        }

        public static void toolStripFavClear_Click(object sender, EventArgs e)
        {
            TreeViewEx? Source = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TreeViewEx);
            foreach (DefaultSequencerObject obj in TCLE.LeafObjects.Values)
                obj.Favorite = false;
            UpdateTrees(true);
            /*
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            UtilAudio.PlaySound("UIdelete");

            foreach (EditorLeaf leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorLeaf)))
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.treeObjects.Tag.ToString());
            */
        }
    }
}
