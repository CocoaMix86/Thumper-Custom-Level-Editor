using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LvlEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_LvlEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            InitializeLvlStuff();
            lvlToolStrip.Renderer = new ToolStripOverride();
            lvlPathsToolStrip.Renderer = new ToolStripOverride();
            lvlLoopToolStrip.Renderer = new ToolStripOverride();
            TCLE.InitializeTracks(lvlLeafList, false);

            if (load != null)
                LoadLvl(load, filepath);
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        private bool EditorIsLoading = false;
        public FileInfo loadedlvl
        {
            get => LoadedLvl;
            set {
                if (LoadedLvl != value) {
                    LoadedLvl = value;
                    if (!LoadedLvl.Exists) {
                        LoadedLvl.CreateText();
                    }
                    TCLE.lockedfiles.Add(LoadedLvl, new FileStream(LoadedLvl.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        public static FileInfo LoadedLvl;
        public LvlProperties lvlProperties
        {
            get { return LvlProperties; }
            set {
                SaveCheckAndWrite(false);
                LvlProperties = value;
            }
        }
        private static LvlProperties LvlProperties;
        private static List<string> LvlPaths = Properties.Resources.paths.Replace("\r\n", "\n").Split('\n').ToList();
        public ObservableCollection<LvlLeafData> LvlLeafs { get => LvlProperties.lvlleafs; set => LvlProperties.lvlleafs = value; }
        private static decimal BPM => TCLE.dockProjectProperties.BPM;
        private List<LvlLeafData> clipboardleaf = new();
        private List<string> clipboardpaths = new();
        #endregion

        #region EventHandlers
        ///         ///
        /// EVENTS  ///
        ///         ///

        ///DGV LVLLEAFLIST
        private void lvlLeafList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || LvlLeafs.Count == 0 || e.RowIndex > LvlLeafs.Count - 1)
                return;
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;
            lvlProperties.sublevel = LvlLeafs[e.RowIndex];
            propertyGridLvl.ExpandAllGridItems();
            propertyGridLvl.Refresh();
            LvlUpdatePaths(e.RowIndex);
        }
        private void lvlLeafList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || LvlLeafs.Count == 0 || e.RowIndex > LvlLeafs.Count - 1)
                return;
            TCLE.OpenFile(TCLE.Instance, TCLE.dockProjectExplorer.projectfiles.First(x => x.Key.EndsWith($@"\{LvlLeafs[e.RowIndex].leafname}")).Value);

        }
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void lvlLeafList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y)) {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = lvlLeafList.DoDragDrop(lvlLeafList.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void lvlLeafList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = lvlLeafList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1) {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private int previousDragOver = -1;
        private void lvlLeafList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = lvlLeafList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = lvlLeafList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (previousDragOver != targetRow && previousDragOver != -1) {
                if (lvlLeafList.Rows[previousDragOver].Cells[2].Value.ToString() == "file not found")
                    lvlLeafList.Rows[previousDragOver].DefaultCellStyle.BackColor = Color.Maroon;
                else
                    lvlLeafList.Rows[previousDragOver].DefaultCellStyle = null;
            }
            if (targetRow != -1 && targetRow != previousDragOver) {
                lvlLeafList.Rows[targetRow].DefaultCellStyle.BackColor = Color.FromArgb(64, 53, 130);
                previousDragOver = targetRow;
            }
        }

        private void lvlLeafList_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void lvlLeafList_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = lvlLeafList.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = lvlLeafList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move) {
                if (e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow rowToMove) {
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                        return;
                    LvlLeafData tomove = LvlLeafs[rowToMove.Index];
                    LvlLeafs.RemoveAt(rowIndexFromMouseDown);
                    LvlLeafs.Insert(rowIndexOfItemUnderMouseToDrop, tomove);
                }
                if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode) {
                    AddFiletoLvl($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}");
                }
            }
        }
        ///DGV LVLLEAFPATHS
        //Cell value changed
        private void lvlLeafPaths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if a path is set to blank, remove the row
            if (lvlLeafPaths.CurrentCell.Value.ToString() == " ")
                lvlLeafPaths.Rows.Remove(lvlLeafPaths.CurrentRow);
            //clear List storing the paths and repopulate it
            LvlProperties.sublevel.paths.Clear();
            LvlProperties.sublevel.paths = lvlLeafPaths.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value.ToString()).ToList();
            /*for (int x = 0; x < lvlLeafPaths.Rows.Count; x++) {
                if (lvlLeafPaths.Rows[x].Cells[0].Value != null)
                    LvlLeafs[lvlLeafList.CurrentRow.Index].paths.Add(lvlLeafPaths.Rows[x].Cells[0].Value.ToString());
            }*/
            //Delete button enabled/disabled if rows exist
            btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //set lvl save flag to false
            SaveCheckAndWrite(false);
        }
        /// DGV LVLLOOPTRACKS
        //Cell value changed
        private void lvlLoopTracks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            LvlProperties.lvlloops[e.RowIndex] = new LvlLoop() {
                sample = $"{lvlLoopTracks.Rows[e.RowIndex].Cells[0].Value}",
                beats = decimal.Parse(lvlLoopTracks.Rows[e.RowIndex].Cells[1].Value.ToString())
            };
            lvlloop_CollectionChanged(null, null);
            SaveCheckAndWrite(false);
        }
        private void lvlLoopTracks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        ///_LVLLEAF - Triggers when the collection changes
        public void lvlleaf_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int _in = e.NewStartingIndex;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) {
                lvlLeafList.RowCount = 0;
            }
            //if action ADD, add new row to the lvl DGV
            //NewStartingIndex and OldStartingIndex track where the changes were made
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                lvlLeafList.Rows.Insert(e.NewStartingIndex, new object[] {
                    Properties.Resources.editor_leaf,
                    LvlLeafs[_in].leafname,
                    0 });
            }
            //if action REMOVE, remove row from the lvl DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                lvlLeafList.Rows.RemoveAt(e.OldStartingIndex);
            }

            RecalculateRuntime();

            //enable certain buttons if there are enough items for them
            btnLvlLeafDelete.Enabled = LvlLeafs.Count > 0;
            btnLvlLeafUp.Enabled = LvlLeafs.Count > 1;
            btnLvlLeafDown.Enabled = LvlLeafs.Count > 1;
            btnLvlLeafCopy.Enabled = LvlLeafs.Count > 0;
            //enable/disable buttons if leaf exists or not
            if (LvlLeafs.Count == 0) {
                btnLvlPathAdd.Enabled = false;
                lblLvlTunnels.Text = $"Paths/Tunnels - <no leaf>";
                btnLvlRandomTunnel.Enabled = false;
            }
            if (btnLvlPathAdd.Enabled == false) btnLvlPathDelete.Enabled = false;
            if (btnLvlPathAdd.Enabled == false) btnLvlCopyTunnel.Enabled = false;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlLoopAdd.Enabled = LvlLeafs.Count > 0;
            if (btnLvlLoopAdd.Enabled == false) btnLvlLoopDelete.Enabled = false;
            //
            if (!EditorIsLoading) {
                SaveCheckAndWrite(false);
            }
        }
        public void lvlloop_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            lvlLoopTracks.RowCount = 0;
            foreach (LvlLoop loop in LvlProperties.lvlloops) {
                lvlLoopTracks.Rows.Add(new object[] {
                    loop.sample,
                    loop.beats
                });
            }
            foreach (DataGridViewRow r in lvlLoopTracks.Rows)
                r.HeaderCell.Value = "Volume Track " + r.Index;
            btnLvlLoopDelete.Enabled = lvlLoopTracks.Rows.Count > 0;
            SaveCheckAndWrite(false);
        }
        /// LVL NEW
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if LVL not saved, have user confirm if they want to continue
            if ((!EditorIsSaved && MessageBox.Show("Current LVL is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                SaveAs();
            }
        }
        ///SAVE
        public void Save()
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (LoadedLvl == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, true);
        }
        ///SAVE AS
        public void SaveAs()
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Lvl File (*.lvl)|*.lvl";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedlvl = new FileInfo(sfd.FileName);
                SaveCheckAndWrite(true, true);
                //after saving new file, refresh the project explorer
                TCLE.dockProjectExplorer.CreateTreeView();
            }
        }
        /// LVL LOAD
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current lvl is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Editor Lvl File (*.txt)|lvl_*.txt";
                ofd.Title = "Load a Thumper Lvl file";
                ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    FileInfo filepath = new(TCLE.CopyToWorkingFolderCheck(ofd.FileName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadLvl(_load, filepath);
                }
            }
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///

        private void btnLvlLeafDelete_Click(object sender, EventArgs e)
        {
            List<LvlLeafData> todelete = new();
            foreach (DataGridViewRow dgvr in lvlLeafList.SelectedRows) {
                todelete.Add(LvlLeafs[dgvr.Index]);
            }
            int _in = lvlLeafList.CurrentRow.Index;
            //LvlLeafs.RemoveAt(_in);
            foreach (LvlLeafData lvd in todelete)
                LvlLeafs.Remove(lvd);
            TCLE.PlaySound("UIobjectremove");
            lvlLeafList_CellClick(null, new DataGridViewCellEventArgs(0, _in >= LvlLeafs.Count ? _in - 1 : _in));
        }
        private void btnLvlLeafAdd_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Leaf File (*.txt)|leaf_*.txt";
            ofd.Title = "Load a Thumper Leaf file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            TCLE.PlaySound("UIfolderopen");
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddFiletoLvl(ofd.FileName);
            }
        }

        private void btnLvlLeafUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            EditorIsLoading = true;
            lvlLeafList.ClearSelection();
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                LvlLeafs.Insert(dgvr - 1, LvlLeafs[dgvr]);
                LvlLeafs.RemoveAt(dgvr + 1);
            }
            lvlLeafList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafList.Rows[dgvr - 1].Selected = true;
            }
            EditorIsLoading = false;
            SaveCheckAndWrite(false);
        }

        private void btnLvlLeafDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == lvlLeafList.RowCount - 1))
                return;
            EditorIsLoading = true;
            lvlLeafList.ClearSelection();
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                LvlLeafs.Insert(dgvr + 2, LvlLeafs[dgvr]);
                LvlLeafs.RemoveAt(dgvr);
            }
            lvlLeafList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafList.Rows[dgvr + 1].Selected = true;
            }
            EditorIsLoading = false;
            SaveCheckAndWrite(false);
        }

        ///COPY PASTE of leaf
        private void btnLvlLeafCopy_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            clipboardleaf = LvlLeafs.Where(x => selectedrows.Contains(LvlLeafs.IndexOf(x))).ToList();
            clipboardleaf.Reverse();
            btnLvlLeafPaste.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }
        private void btnLvlLeafPaste_Click(object sender, EventArgs e)
        {
            int _in = lvlLeafList.CurrentRow?.Index + 1 ?? 0;
            foreach (LvlLeafData lld in clipboardleaf)
                LvlLeafs.Insert(_in, lld.Clone());
            TCLE.PlaySound("UIkpaste");
            SaveCheckAndWrite(false);
        }

        private void btnLvlLeafRandom_Click(object sender, EventArgs e)
        {
            /*
            List<string> leafs = TCLE.WorkingFolderFiles.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value.ToString().Contains("leaf_")).Select(x => x.Cells[1].Value.ToString()).ToList();
            string _selectedfilename = $@"{TCLE.WorkingFolder}\{leafs[_mainform.rng.Next(0, leafs.Count)]}.txt";
            AddLeaftoLvl(_selectedfilename);
            */
        }

        private void btnLvlPathDelete_Click(object sender, EventArgs e)
        {
            List<string> todelete = new();
            foreach (DataGridViewRow dgvr in lvlLeafPaths.SelectedRows) {
                todelete.Add(dgvr.Cells[0].Value.ToString());
            }
            foreach (string s in todelete)
                LvlLeafs[lvlLeafList.CurrentRow.Index].paths.Remove(s);
            LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
            TCLE.PlaySound("UItunnelremove");
            SaveCheckAndWrite(false);
        }

        private void btnLvlPathAdd_Click(object sender, EventArgs e)
        {
            LvlLeafs[lvlLeafList.CurrentRow.Index].paths.Add("");
            LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
            btnLvlPathDelete.Enabled = true;
            TCLE.PlaySound("UItunneladd");
            SaveCheckAndWrite(false);
        }

        private void btnLvlPathUp_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == 0))
                return;
            int idx = lvlLeafList.CurrentRow.Index;
            List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                LvlLeafs[idx].paths.Insert(dgvr - 1, LvlLeafs[idx].paths[dgvr]);
                LvlLeafs[idx].paths.RemoveAt(dgvr + 1);
            }
            LvlUpdatePaths(idx);
            lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] - 1];
            lvlLeafPaths.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafPaths.Rows[dgvr - 1].Cells[0].Selected = true;
            }
            SaveCheckAndWrite(false);
        }

        private void btnLvlPathDown_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == lvlLeafPaths.Rows.Count - 1))
                return;
            int idx = lvlLeafList.CurrentRow.Index;
            List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                LvlLeafs[idx].paths.Insert(dgvr + 2, LvlLeafs[idx].paths[dgvr]);
                LvlLeafs[idx].paths.RemoveAt(dgvr);
            }
            LvlUpdatePaths(idx);
            lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] + 1];
            lvlLeafPaths.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafPaths.Rows[dgvr + 1].Cells[0].Selected = true;
            }
            SaveCheckAndWrite(false);
        }

        private void btnLvlPathClear_Click(object sender, EventArgs e)
        {
            int idx = lvlLeafList.CurrentRow.Index;
            if (LvlLeafs[idx].paths.Count > 0) {
                if (MessageBox.Show("Are you sure you want to clear all?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            LvlLeafs[idx].paths.Clear();
            LvlUpdatePaths(idx);
            TCLE.PlaySound("UIdataerase");
            SaveCheckAndWrite(false);
        }

        private void btnLvlRandomTunnel_Click(object sender, EventArgs e)
        {
            if (LvlLeafs.Count == 0)
                return;
            lvlLeafPaths.RowCount++;
            lvlLeafPaths.Rows[^1].Cells[0].Value = LvlPaths[TCLE.rng.Next(1, LvlPaths.Count)];
            btnLvlPathDelete.Enabled = true;
            TCLE.PlaySound("UItunneladd");
            SaveCheckAndWrite(false);
        }

        private void btnLvlCopyTunnel_Click(object sender, EventArgs e)
        {
            if (loadedlvl == null)
                return;
            clipboardpaths = new List<string>(LvlLeafs[lvlLeafList.CurrentRow.Index].paths);
            btnLvlPasteTunnel.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }

        private void btnLvlPasteTunnel_Click(object sender, EventArgs e)
        {
            LvlLeafs[lvlLeafList.CurrentRow.Index].paths.AddRange(new List<string>(clipboardpaths));
            LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
            TCLE.PlaySound("UIkpaste");
            SaveCheckAndWrite(false);
        }

        private void btnLvlLoopAdd_Click(object sender, EventArgs e)
        {
            lvlLoopTracks.RowCount++;
            lvlLoopTracks.Rows[^1].HeaderCell.Value = "Volume Track " + (lvlLoopTracks.Rows.Count - 1);
            lvlLoopTracks.Rows[^1].Cells[1].Value = 0;
            lvlLoopTracks.Rows[^1].Cells[0].Value = "";
            btnLvlLoopDelete.Enabled = true;
            LvlProperties.lvlloops.Add(new LvlLoop());
            TCLE.PlaySound("UIobjectadd");
        }

        private void btnLvlLoopDelete_Click(object sender, EventArgs e)
        {
            lvlLoopTracks.Rows.RemoveAt(lvlLoopTracks.CurrentRow.Index);
            LvlProperties.lvlloops.RemoveAt(lvlLoopTracks.CurrentRow.Index);
            TCLE.PlaySound("UIobjectremove");
            //disable button if no more rows exist
            if (lvlLoopTracks.Rows.Count < 1)
                btnLvlLoopDelete.Enabled = false;
            //rename each header cell as the rows have moved and now are on different tracks
            foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
                r.HeaderCell.Value = "Volume Track " + r.Index;
            }
            SaveCheckAndWrite(false);
        }

        private void btnRevertLvl_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true);
            LoadLvl(lvlProperties.revertPoint, LoadedLvl);
            TCLE.PlaySound("UIrevertchanges");
        }

        private void btnlvlPanelNew_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripLvlNew.PerformClick();
        }
        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public void LoadLvl(dynamic _load, FileInfo filepath)
        {
            if (_load == null)
                return;
            //reset flag in case it got stuck previously
            EditorIsLoading = false;
            //detect if file is actually Lvl or not
            if ((string)_load["obj_type"] != "SequinLevel") {
                MessageBox.Show("This does not appear to be a lvl file!");
                return;
            }
            loadedlvl = filepath;
            //set some visual elements
            this.Text = LoadedLvl.Name;
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;

            lvlProperties = new(this, filepath) {
                approachbeats = (int)_load["approach_beats"],
                volume = (decimal)_load["volume"],
                allowinput = (string)_load["input_allowed"] == "True",
                tutorialtype = (string)_load["tutorial_type"]
            };

            ///Clear DGVs so new data can load
            lvlLoopTracks.Rows.Clear();
            lvlLeafList.Rows.Clear();
            lvlLeafPaths.Rows.Clear();
            LvlLeafs.Clear();

            ///load loop track names and paths to lvlLoopTracks DGV
            TCLE.LvlReloadSamples();
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DataSource = TCLE.LvlSamples.Select(x => x.obj_name).ToList();
            foreach (dynamic samp in _load["loops"]) {
                lvlProperties.lvlloops.Add(new LvlLoop() {
                    sample = (string)samp["samp_name"],
                    beats = (decimal?)samp["beats_per_loop"] == null ? 0 : (decimal)samp["beats_per_loop"]
                });
            }
            ///load leafs associated with this lvl
            foreach (dynamic leaf in _load["leaf_seq"]) {
                LvlLeafs.Add(new LvlLeafData() {
                    leafname = (string)leaf["leaf_name"],
                    beats = (int)leaf["beat_cnt"],
                    paths = leaf["sub_paths"].ToObject<List<string>>(),
                    id = TCLE.rng.Next(0, 1000000)
                });
            }

            btnLvlLeafRandom.Enabled = true;
            propertyGridLvl.SelectedObject = lvlProperties;
            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            EditorIsSaved = true;
            RecalculateRuntime();
        }

        public void InitializeLvlStuff()
        {
            LvlPaths.Sort();

            ///customize Paths List a bit
            //custom column containing comboboxes per cell
            DataGridViewComboBoxColumn _dgvlvlpaths = new() {
                DataSource = LvlPaths,
                HeaderText = "Path Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                DisplayStyleForCurrentCellOnly = true
            };
            lvlLeafPaths.Columns.Add(_dgvlvlpaths);
            ///

            ///customize Loop Track list a bit
            //custom column containing comboboxes per cell
            lvlLoopTracks.Columns[1].ValueType = typeof(decimal);
            lvlLoopTracks.Columns[1].DefaultCellStyle.Format = "0.##";
            ///
        }

        public void AddFiletoLvl(string path)
        {
            //parse leaf to JSON
            dynamic _load = TCLE.LoadFileLock(path);
            //check if file being loaded is actually a leaf. Can do so by checking the JSON key
            if ((string)_load["obj_type"] != "SequinLeaf") {
                MessageBox.Show("This does not appear to be a leaf file!", "Leaf load error");
                return;
            }
            //check if lvl exists in the same folder as the master. If not, allow user to copy file.
            //this is why I utilize workingfolder
            //if (Path.GetDirectoryName(path) != TCLE.WorkingFolder) {
            if (!Path.GetDirectoryName(path).Contains(TCLE.WorkingFolder.FullName)) {
                if (MessageBox.Show("The item you chose does not exist in the project. Do you want to copy it to the project folder?", "Yhumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    if (!File.Exists($@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}")) {
                        File.Copy(path, $@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}");
                        TCLE.dockProjectExplorer.CreateTreeView();
                    }
                    else
                        return;
            }
            TCLE.PlaySound("UIobjectadd");
            //Setup list of tunnels if copy check is enabled
            List<string> copytunnels = new();
            if (chkTunnelCopy.Checked) {
                copytunnels = new List<string>(LvlLeafs.Last().paths);
            }
            //add leaf data to the list
            LvlLeafs.Add(new LvlLeafData() {
                leafname = (string)_load["obj_name"],
                beats = (int)_load["beat_cnt"],
                paths = new List<string>(copytunnels),
                id = TCLE.rng.Next(0, 1000000)
            });
            propertyGridLvl.Refresh();
        }

        public void LvlUpdatePaths(int index)
        {
            lvlLeafPaths.Rows.Clear();
            lblLvlTunnels.Text = $"Paths/Tunnels - {LvlLeafs[index].leafname}";
            //for each path in the selected leaf, populate the paths DGV
            foreach (string path in LvlLeafs[index].paths) {
                //path may have been manually added and could not exist
                if (LvlPaths.Contains(path))
                    lvlLeafPaths.Rows.Add(new object[] { path });
                else
                    MessageBox.Show($"Tunnel \"{path}\" not found in program. If you think this is wrong, please report this to CocoaMix on the github page!");
            }
            //enable a bunch of buttons based on if paths exist or not
            btnLvlPathAdd.Enabled = true;
            btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlRandomTunnel.Enabled = btnLvlPathAdd.Enabled;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //monke
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            if (EditorIsLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedLvl.Name + "*";
                //add current JSON to the undo list
                lvlProperties.undoItems.Add(BuildSave(lvlProperties));
            }
            else {
                this.Text = LoadedLvl.Name;
                //build the JSON to write to file
                JObject _saveJSON = BuildSave(lvlProperties);
                lvlProperties.revertPoint = _saveJSON;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLvl], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedLvl.Name);
                TCLE.FindEditorRunMethod(typeof(Form_GateEditor), "RecalculateRuntime");
                TCLE.FindEditorRunMethod(typeof(Form_MasterEditor), "RecalculateRuntime");
            }
        }

        ///Import raw text from rich text box to selected row
        public static void LvlTrackRawImport(DataGridViewRow r, JObject _rawdata)
        {
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
            //set highlighting color
            Color _color = Color.Purple;
            //iterate over each data point, and fill cells
            foreach (JProperty data_point in data_points) {
                try {
                    r.Cells[int.Parse(data_point.Name)].Value = (float)data_point.Value;
                    r.Cells[int.Parse(data_point.Name)].Style.BackColor = _color;
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        public int RecalculateRuntime()
        {
            int beattotal = 0;
            foreach (LvlLeafData _leaf in LvlLeafs) {
                FileInfo leaffile = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Value.Name == _leaf.leafname).Value;
                leaffile?.Refresh();
                int beats = (leaffile != null && leaffile.Exists) ? _leaf.beats : -1;
                if (beats == -1) {
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].DefaultCellStyle.BackColor = Color.Maroon;
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].Cells[2].Value = $"file not found";
                }
                else {
                    beats = (int)TCLE.LoadFileLock(leaffile.FullName)["beat_cnt"];
                    beattotal += beats;
                    string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].DefaultCellStyle = null;
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].Cells[2].Value = $"{beats} beats -- {time}";
                }
            }
            lvlLeafList.Refresh();
            return beattotal;
        }

        public static JObject BuildSave(LvlProperties _properties)
        {
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLevel" },
                { "obj_name", _properties.FilePath.Name },
                { "approach_beats", _properties.approachbeats }
            };
            //this section adds all colume sequencer controls
            JArray seq_objs = new();
            /*
            foreach (DataGridViewRow seq_obj in lvlSeqObjs.Rows) {
                JObject s = new() {
                    { "obj_name", $"{_lvlname}" },
                    { "param_path", $"layer_volume,{seq_obj.Index}" },
                    { "trait_type", "kTraitFloat" }
                };

                JObject data_points = new();
                for (int x = 0; x < seq_obj.Cells.Count; x++) {
                    if (!string.IsNullOrEmpty(seq_obj.Cells[x].Value?.ToString()))
                        data_points.Add(x.ToString(), float.Parse(seq_obj.Cells[x].Value.ToString()));
                }
                s.Add("data_points", data_points);

                s.Add("step", "False");
                s.Add("default", "0");
                s.Add("footer", "1,1,2,1,2,'kIntensityScale','kIntensityScale',1,1,1,1,1,1,1,1,0,0,0");

                seq_objs.Add(s);
            }
            */
            _save.Add("seq_objs", seq_objs);
            //this section adds all leafs
            JArray leaf_seq = new();
            foreach (LvlLeafData _leaf in _properties.lvlleafs) {
                JObject s = new() {
                    { "beat_cnt", _leaf.beats },
                    { "leaf_name", _leaf.leafname },
                    { "main_path", "default.path" },
                    { "sub_paths", JArray.FromObject(_leaf.paths) },
                    { "pos", new JArray() { 0, 0, 0 } },
                    { "rot_x", new JArray() { 1, 0, 0 } },
                    { "rot_y", new JArray() { 0, 1, 0 } },
                    { "rot_z", new JArray() { 0, 0, 1 } },
                    { "scale", new JArray() { 1, 1, 1 } }
                };

                leaf_seq.Add(s);
            }
            _save.Add("leaf_seq", leaf_seq);
            //this section adds the loop tracks
            JArray loops = new();
            foreach (LvlLoop _loop in _properties.lvlloops) {
                if (_loop.sample == null)
                    continue;
                JObject s = new() {
                    { "samp_name", $"{_loop.sample}"},
                    { "beats_per_loop", _loop.beats }
                };

                loops.Add(s);
            }
            _save.Add("loops", loops);
            //final keys
            _save.Add("volume", _properties.volume);
            _save.Add("input_allowed", _properties.allowinput.ToString());
            _save.Add("tutorial_type", _properties.tutorialtype);
            _save.Add("start_angle_fracs", new JArray() { 1, 1, 1 });
            ///end building JSON output
            return _save;
        }

#endregion
    }
}
