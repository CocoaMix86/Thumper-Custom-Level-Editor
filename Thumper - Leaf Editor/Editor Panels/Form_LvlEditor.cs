using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using Thumper_Custom_Level_Editor.Other_Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LvlEditor : DockContentEx
    {
        #region Form Construction
        public Form_LvlEditor(dynamic load = null, FileInfo filepath = null, bool saveonlynoload = false)
        {
            InitializeComponent();
            InitializeLvlStuff();
            ColorFormElements();
            SaveOnlyNoLoad = saveonlynoload;
            lvlToolStrip.Renderer = new ToolStripOverride();
            lvlPathsToolStrip.Renderer = new ToolStripOverride();
            lvlLoopToolStrip.Renderer = new ToolStripOverride();
            TCLE.DoubleBufferDGV(lvlLeafList, false);

            if (load != null) {
                LoadLvl(load, filepath);
                UndoList.Add(new SaveState() {
                    reason = "",
                    savestate = load
                });
            }

            btnLvlPathView.Checked = Properties.Settings.Default.PreviewTunnel;
        }
        private void Form_LvlEditor_Shown(object sender, EventArgs e)
        {
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!IsSaved()) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        public void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorLvlBG;
            lvlLeafList.BackgroundColor = Properties.Settings.Default.ColorLvlLeafBG;
            lvlLeafPaths.BackgroundColor = Properties.Settings.Default.ColorLvlTunnelBG;
            lvlLoopTracks.BackgroundColor = Properties.Settings.Default.ColorLvlLoopsBG;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        private bool EditorIsLoading;
        private bool SaveOnlyNoLoad;
        public FileInfo loadedlvl
        {
            get => LoadedLvl;
            set {
                if (LoadedLvl != value) {
                    TCLE.CloseFileLock(LoadedLvl);
                    LoadedLvl = value;
                    if (!LoadedLvl.Exists) {
                        using (StreamWriter sw = LoadedLvl.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(LoadedLvl);
                }
            }
        }
        public FileInfo LoadedLvl;
        public LvlProperties lvlProperties
        {
            get { return LvlProperties; }
            set {
                LvlProperties = value;
                SaveCheckAndWrite(false, "uuuuuhhhhhhhhhhhhhh");
            }
        }
        private LvlProperties LvlProperties;
        public ObservableCollection<LvlLeafData> LvlLeafs { get => LvlProperties.lvlleafs; set => LvlProperties.lvlleafs = value; }
        private List<string> clipboardpaths = new();
        public int SampChannel;
        private DeserializeDockContent m_deserializeDockContent;
        public DockContentEx contentTunnel = new() {
            TabText = "Paths/Tunnels",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentMain = new() {
            TabText = "Leaf List",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentLoop = new() {
            TabText = "Loop Tracks",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
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
            LvlUpdatePaths(LvlProperties.sublevel);
        }

        private void lvlLeafList_SelectionChanged(object sender, EventArgs e)
        {
            if (lvlLeafList.RowCount < 1 || lvlLeafList.SelectedRows.Count == 0)
                return;
            lvlProperties.sublevel = LvlLeafs[lvlLeafList.SelectedRows[^1].Index];
            LvlUpdatePaths(LvlProperties.sublevel);
        }

        private void lvlLeafList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || LvlLeafs.Count == 0 || e.RowIndex > LvlLeafs.Count - 1)
                return;
            TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"{LvlLeafs[e.RowIndex].leafname}")).Value?.File);
        }

        private Rectangle dragBoxFromMouseDown;
        private Rectangle dragBoxFromMouseDownPaths;
        private List<LvlLeafData> LeafsToMove;
        private int rowIndexFromMouseDown;
        private int rowIndexFromMouseDownPaths;
        private int rowIndexOfItemUnderMouseToDrop;
        private int previousDragOver = -2;
        private int TargetRowToPaint = -3;
        private void lvlLeafList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (TCLE.DragSource is "none" && (e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (LeafsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.
                    LeafsToMove = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => LvlLeafs[x.Index]).ToList();
                    TCLE.DragSource = "LeafList";
                    //RowToMove.DefaultCellStyle.BackColor = SelectColor;
                    DragDropEffects dropEffect = lvlLeafList.DoDragDrop(LeafsToMove, DragDropEffects.Move);
                    LeafsToMove = null;
                    TCLE.DragSource = "none";
                    TargetRowToPaint = -3;
                    previousDragOver = -2;
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

        private void lvlLeafList_DragOver(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LeafList" and not "FileExplorer")
                return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = lvlLeafList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = lvlLeafList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (LeafsToMove == null) {
                //if (e.Data.GetData(typeof(List<string>)) is TreeNode dragdropnode)
                //    return;
                //else {
                if (targetRow != previousDragOver) {
                    previousDragOver = targetRow;
                    TargetRowToPaint = targetRow;
                    if (TargetRowToPaint is -1)
                        TargetRowToPaint = lvlLeafList.RowCount;
                    lvlLeafList.Invalidate();
                }
                //}
            }
            else {
                if (LeafsToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                    foreach (LvlLeafData leaf in LeafsToMove) {
                        LvlLeafs.Remove(leaf);
                        LvlLeafs.Insert(targetRow, leaf);
                    }
                    previousDragOver = targetRow;
                    lvlLeafList.ClearSelection();
                    lvlLeafList.Rows[targetRow].Selected = true;
                }
            }
        }

        private void lvlLeafList_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LeafList" and not "FileExplorer")
                return;
            if (LeafsToMove != null)
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }

        private void lvlLeafList_DragDrop(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LeafList" and not "FileExplorer")
                return;
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = lvlLeafList.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = lvlLeafList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode) {
                AddFiletoLvl($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}", TargetRowToPaint);
            }
            else if (LeafsToMove != null) {
                SaveCheckAndWrite(false, "Reorder Leafs");
                LeafsToMove = null;
            }
            else if (e.Data.GetData(typeof(List<LvlLeafData>)) is List<LvlLeafData> leafs) {
                LvlLeafs.CollectionChanged -= lvlleaf_CollectionChanged;
                foreach (LvlLeafData leaf in leafs)
                    LvlLeafs.Insert(TargetRowToPaint, leaf.Clone());
                LvlLeafs.CollectionChanged += lvlleaf_CollectionChanged;
            }
            else if (e.Data.GetData(typeof(List<string>)) is List<string> leafs2) {
                LvlLeafs.CollectionChanged -= lvlleaf_CollectionChanged;
                foreach (string leaf in leafs2)
                    AddFiletoLvl(ProjectExplorer.Files.FirstOrDefault(x => x.Value.IsFile && x.Value.File.Name == leaf).Value.FullPath, TargetRowToPaint);
                LvlLeafs.CollectionChanged += lvlleaf_CollectionChanged;
            }
            lvlleaf_CollectionChanged(null, null);
            lvlLeafList.ClearSelection();
            lvlLeafList.Rows[previousDragOver is -1 ? lvlLeafList.RowCount - 1 : previousDragOver].Selected = true;
            TargetRowToPaint = -3;
            previousDragOver = -2;
            lvlLeafList.Invalidate();
        }
        ///
        private List<string> RowsToMove;
        private void lvlLeafPaths_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (TCLE.DragSource is "none" && (e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (RowsToMove == null && dragBoxFromMouseDownPaths != Rectangle.Empty && !dragBoxFromMouseDownPaths.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.                    
                    var SelectedRows = lvlLeafPaths.SelectedCells.Cast<DataGridViewCell>().ToList();
                    SelectedRows.Sort((row1, row2) => row1.RowIndex.CompareTo(row2.RowIndex));
                    RowsToMove = SelectedRows.Select(x => x.Value.ToString()).ToList();
                    TCLE.DragSource = "PathList";
                    //
                    DragDropEffects dropEffect = lvlLeafPaths.DoDragDrop(RowsToMove, DragDropEffects.Move);
                    RowsToMove = null;
                    TCLE.DragSource = "none";
                    TargetRowToPaint = -3;
                    previousDragOver = -2;
                }
            }
        }
        private void lvlLeafPaths_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            rowIndexFromMouseDownPaths = lvlLeafPaths.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDownPaths != -1) {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDownPaths = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                dragBoxFromMouseDownPaths = Rectangle.Empty;
        }
        private void lvlLeafPaths_DragOver(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "PathList")
                return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = lvlLeafPaths.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = lvlLeafPaths.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            if (RowsToMove == null) {
                if (targetRow != previousDragOver) {
                    previousDragOver = targetRow;
                    TargetRowToPaint = targetRow;
                    if (TargetRowToPaint is -1)
                        TargetRowToPaint = lvlLeafPaths.RowCount;
                    lvlLeafPaths.Invalidate();
                }
            }
            else {
                if (RowsToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                    foreach (string path in RowsToMove) {
                        lvlProperties.sublevel.paths.Remove(path);
                        lvlProperties.sublevel.paths.Insert(targetRow, path);
                        previousDragOver = targetRow;
                        lvlLeafPaths.Rows[targetRow].Selected = true;
                    }
                    LvlUpdatePaths(LvlProperties.sublevel);
                }
            }
        }

        private void lvlLeafPaths_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "PathList")
                return;
            if (RowsToMove != null) {
                e.Effect = DragDropEffects.Move;
            } else {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void lvlLeafPaths_DragLeave(object sender, EventArgs e)
        {
        }

        private void lvlLeafPaths_DragDrop(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "PathList")
                return;
            // If the drag operation was a move then remove and insert the row.
            if (RowsToMove != null) {
                LvlBuildPathList();
                SaveCheckAndWrite(false, "Reorder Paths on Leaf");
            }
            else {
                if (e.Data.GetData(typeof(List<string>)) is List<string>) {
                    foreach (string path in e.Data.GetData(typeof(List<string>)) as List<string>) {
                        LvlProperties.sublevel.paths.Remove(path);
                        LvlProperties.sublevel.paths.Insert(TargetRowToPaint, path);
                    }

                    LvlUpdatePaths(LvlProperties.sublevel);
                    SaveCheckAndWrite(false, "Reorder Paths on Leaf");
                }
            }
            TargetRowToPaint = -3;
            previousDragOver = -2;
        }
        ///

        private static SolidBrush ClearColor = new SolidBrush(Color.Black);
        private static SolidBrush BrushWhite = new SolidBrush(Color.White);
        private static Pen PenGreen = new Pen(Color.Green, 4);
        private void lvlLeafList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            Rectangle bounds = e.RowBounds;
            bounds.X += 2;
            bounds.Y += 2;
            bounds.Width -= 4;
            bounds.Height -= 4;
            e.Graphics.FillRectangle(ClearColor, e.RowBounds);
            DataGridView dgv = sender as DataGridView;

            if (dgv.Rows[e.RowIndex].Selected)
                e.Graphics.FillRoundedRectangle(BrushWhite, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 8);
            e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(e.InheritedRowStyle.BackColor, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);

            if (sender == lvlLeafPaths)
                e.PaintCells(e.RowBounds, DataGridViewPaintParts.All);
            else
                e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);

            if ((sender == lvlLeafPaths && TCLE.DragSource is "PathList") || (sender == lvlLeafList && TCLE.DragSource is "LeafList" or "FileExplorer")) {
                if (e.RowIndex == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Top, e.RowBounds.Right, e.RowBounds.Top);
                if (e.RowIndex + 1 == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Bottom, e.RowBounds.Right, e.RowBounds.Bottom);
            }
        }

        private void lvlLeafList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (e.RowIndex == -1)
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            else {
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentBackground);
                e.Paint(e.CellBounds, DataGridViewPaintParts.Focus);
            }
        }
        ///DGV LVLLEAFPATHS
        //Cell value changed
        private void lvlLeafPaths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (LvlProperties == null)
                return;
            //clear List storing the paths and repopulate it
            LvlBuildPathList();
            //Delete button enabled/disabled if rows exist
            btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //set lvl save flag to false
            SaveCheckAndWrite(false, "Tunnels Changed");
        }

        /// DGV LVLLOOPTRACKS
        //Cell value changed
        private void lvlLoopTracks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            LvlProperties.lvlloops[e.RowIndex].sample = $"{lvlLoopTracks.Rows[e.RowIndex].Cells[1].Value}";
            LvlProperties.lvlloops[e.RowIndex].beats = decimal.Parse(lvlLoopTracks.Rows[e.RowIndex].Cells[2].Value.ToString());
            lvlLoopTracks.Rows[e.RowIndex].Cells[2].Value = LvlProperties.lvlloops[e.RowIndex].beats;
            SaveCheckAndWrite(false, "Loop Track Sample/Beats Changed");
        }
        private void lvlLoopTracks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        ///_LVLLEAF - Triggers when the collection changes
        public void lvlleaf_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (LvlProperties.LeafReload)
                return;
            ///int _in = e.NewStartingIndex;

            lvlLeafList.Rows.Clear();
            foreach (LvlLeafData leaf in LvlLeafs) {
                lvlLeafList.Rows.Add(new object[] {
                    Properties.Resources.editor_leaf,
                    leaf.leafname,
                    0 });
            }
            /*
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
            */
            RecalculateRuntime();

            //enable certain buttons if there are enough items for them
            btnLvlLeafDelete.Enabled = LvlLeafs.Count > 0;
            btnLvlLeafUp.Enabled = LvlLeafs.Count > 1;
            btnLvlLeafDown.Enabled = LvlLeafs.Count > 1;
            btnLvlLeafCopy.Enabled = LvlLeafs.Count > 0;
            //enable/disable buttons if leaf exists or not
            if (LvlLeafs.Count == 0) {
                btnLvlPathAdd.Enabled = false;
                contentTunnel.TabText = $"Paths/Tunnels - <no leaf>";
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
        }
        public void lvlloop_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            lvlLoopTracks.RowCount = 0;
            foreach (LvlLoop loop in LvlProperties.lvlloops) {
                lvlLoopTracks.Rows.Add(new object[] {
                    null,
                    loop.sample,
                    loop.beats
                });
            }
            foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
                r.HeaderCell.Value = "Loop Track " + r.Index;
                r.HeaderCell.ToolTipText = "Edit volume levels in Sequencer with an [AUDIO] object";
            }
            btnLvlLoopDelete.Enabled = lvlLoopTracks.Rows.Count > 0;
            TCLE.ResizeHeaders(lvlLoopTracks);
        }

        private void btnLvlPathView_CheckedChanged(object sender, EventArgs e)
        {
            //save check state
            Properties.Settings.Default.PreviewTunnel = btnLvlPathView.Checked;
            //update every active lvl document with new state
            foreach (Form_LvlEditor lvl in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LvlEditor)))
                lvl.btnLvlPathView.Checked = Properties.Settings.Default.PreviewTunnel;
        }

        private void lvlLeafPaths_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (!btnLvlPathView.Checked)
                return;
            //calculate position to show the tunnel image
            Point mouse = TCLE.Instance.PointToClient(System.Windows.Forms.Cursor.Position);
            int height = mouse.Y + 150 > TCLE.Instance.Height ? TCLE.Instance.Height - 300 : mouse.Y - 150;
            //get image of tunnel
            string pathname = (string)(sender as DataGridView).Rows[e.RowIndex].Cells[0].GetEditedFormattedValue(e.RowIndex, DataGridViewDataErrorContexts.Commit);
            TCLE.Instance.pictureTunnelViewer.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject($"path_{pathname.Replace(".path", "")}");
            //show the image
            TCLE.Instance.pictureTunnelViewer.Visible = true;
            TCLE.Instance.pictureTunnelViewer.Location = new Point(mouse.X + 100, height);
            TCLE.Instance.pictureTunnelViewer.BringToFront();
        }

        private void lvlLeafPaths_CellMouseLeave(object sender, DataGridViewCellEventArgs e) => TCLE.Instance.pictureTunnelViewer.Visible = false;
        private void lvlLeafPaths_MouseLeave(object sender, EventArgs e) => TCLE.Instance.pictureTunnelViewer.Visible = false;
        private void pictureTunnelViewer_MouseEnter(object sender, EventArgs e) => TCLE.Instance.pictureTunnelViewer.Visible = false;

        private void propertyGridLvl_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveCheckAndWrite(false, "Change Lvl Property");
        }
        #endregion

        #region Buttons
        private void btnLvlLeafDelete_Click(object sender, EventArgs e)
        {
            List<LvlLeafData> todelete = new();
            foreach (DataGridViewRow dgvr in lvlLeafList.SelectedRows) {
                todelete.Add(LvlLeafs[dgvr.Index]);
            }
            int _in = lvlLeafList.CurrentRow.Index;
            //disable event so it doesn't trigger every time
            LvlLeafs.CollectionChanged -= lvlleaf_CollectionChanged;
            foreach (LvlLeafData lvd in todelete)
                LvlLeafs.Remove(lvd);
            //readd event and call it to populate dgv
            LvlLeafs.CollectionChanged += lvlleaf_CollectionChanged;
            lvlleaf_CollectionChanged(null, null);
            //
            TCLE.PlaySound("UIobjectremove");
            SaveCheckAndWrite(false, "Remove Leaf");
            lvlLeafList_CellClick(null, new DataGridViewCellEventArgs(0, _in >= LvlLeafs.Count ? _in - 1 : _in));
        }
        private void btnLvlLeafAdd_Click(object sender, EventArgs e)
        {
            /*using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Leaf File (*.leaf)|*.leaf";
            ofd.Title = "Load a Thumper Leaf file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            TCLE.PlaySound("UIfolderopen");
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddFiletoLvl(ofd.FileName);
            }*/
            if (TCLE.DragDropItems.Items is not "leaf" || !TCLE.DragDropItems.Visible) {
                TCLE.DragDropItems.Items = "leaf";
                TCLE.DragDropItems.Show();
                TCLE.DragDropItems.Location = new Point(System.Windows.Forms.Cursor.Position.X + 2, System.Windows.Forms.Cursor.Position.Y + 2);
                if (TCLE.DragDropItems.Location.X + TCLE.DragDropItems.Width > this.Width)
                    TCLE.DragDropItems.Location = new Point(this.Width - TCLE.DragDropItems.Width - 2, TCLE.DragDropItems.Location.Y);
            } else
                TCLE.DragDropItems.Hide();
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
            SaveCheckAndWrite(false, "Move Leaf Up");
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
            SaveCheckAndWrite(false, "Move Leaf Down");
        }

        ///COPY PASTE of leaf
        private void btnLvlLeafCopy_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardLvl = LvlLeafs.Where(x => selectedrows.Contains(LvlLeafs.IndexOf(x))).ToList();
            //We reverse the list because they will all paste at the same index. So the last one pasted would be at the top.
            TCLE.ClipboardLvl.Reverse();
            btnLvlLeafPaste.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }
        private void btnLvlLeafPaste_Click(object sender, EventArgs e)
        {
            int _in = lvlLeafList.CurrentRow?.Index + 1 ?? 0;
            foreach (LvlLeafData lld in TCLE.ClipboardLvl)
                LvlLeafs.Insert(_in, lld.Clone());
            TCLE.PlaySound("UIkpaste");
            SaveCheckAndWrite(false, "Paste Leaf");
        }

        private void btnLvlLeafRandom_Click(object sender, EventArgs e)
        {
            List<FileInfo> leafs = ProjectExplorer.Files.Select(x => x.Value.File).Where(x => x.Extension.Equals(".leaf", StringComparison.OrdinalIgnoreCase)).ToList();
            AddFiletoLvl(leafs[TCLE.rng.Next(0, leafs.Count)].FullName);
            SaveCheckAndWrite(false, "Add Random Leaf");
        }

        private void btnLvlPathDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in lvlLeafPaths.SelectedRows) {
                LvlProperties.sublevel.paths.Remove(dgvr.Cells[0].Value.ToString());
            }
            LvlUpdatePaths(LvlProperties.sublevel);
            TCLE.PlaySound("UItunnelremove");
            SaveCheckAndWrite(false, "Remove Tunnel");
        }

        private void btnLvlPathAdd_Click(object sender, EventArgs e)
        {
            if (TCLE.DragDropItems.Items is not "path" || !TCLE.DragDropItems.Visible) {
                TCLE.DragDropItems.Items = "path";
                TCLE.DragDropItems.Show();
                if (TCLE.DragDropItems.Location.X + TCLE.DragDropItems.Width > this.Width)
                    TCLE.DragDropItems.Location = new Point(this.Width - TCLE.DragDropItems.Width - 2, TCLE.DragDropItems.Location.Y);
                else
                    TCLE.DragDropItems.Location = new Point(lvlPathsToolStrip.Width + 2, TCLE.DragDropItems.Location.Y);
            } else
                TCLE.DragDropItems.Hide();
        }

        private void btnLvlPathUp_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == 0))
                return;
            List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                LvlProperties.sublevel.paths.Insert(dgvr - 1, LvlProperties.sublevel.paths[dgvr]);
                LvlProperties.sublevel.paths.RemoveAt(dgvr + 1);
            }
            LvlUpdatePaths(LvlProperties.sublevel);
            lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] - 1];
            lvlLeafPaths.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafPaths.Rows[dgvr - 1].Cells[0].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Tunnel Up");
        }

        private void btnLvlPathDown_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == lvlLeafPaths.Rows.Count - 1))
                return;
            List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                LvlProperties.sublevel.paths.Insert(dgvr + 2, LvlProperties.sublevel.paths[dgvr]);
                LvlProperties.sublevel.paths.RemoveAt(dgvr);
            }
            LvlUpdatePaths(LvlProperties.sublevel);
            lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] + 1];
            lvlLeafPaths.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafPaths.Rows[dgvr + 1].Cells[0].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Tunnel Down");
        }

        private void btnLvlPathClear_Click(object sender, EventArgs e)
        {
            if (LvlProperties.sublevel.paths.Count > 0) {
                if (MessageBox.Show("Are you sure you want to clear all?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            LvlProperties.sublevel.paths.Clear();
            LvlUpdatePaths(LvlProperties.sublevel);
            TCLE.PlaySound("UIdataerase");
            SaveCheckAndWrite(false, "Clear Tunnels on Leaf");
        }

        private void btnLvlRandomTunnel_Click(object sender, EventArgs e)
        {
            if (LvlLeafs.Count == 0)
                return;
            lvlLeafPaths.RowCount++;
            lvlLeafPaths.Rows[^1].Cells[0].Value = TCLE.LvlPaths[TCLE.rng.Next(1, TCLE.LvlPaths.Count)];
            btnLvlPathDelete.Enabled = true;
            TCLE.PlaySound("UItunneladd");
            SaveCheckAndWrite(false, "Add Random Tunnel to Leaf");
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
            LvlUpdatePaths(LvlProperties.sublevel);
            TCLE.PlaySound("UIkpaste");
            SaveCheckAndWrite(false, "Paste Tunnels");
        }

        private void btnLvlLoopAdd_Click(object sender, EventArgs e)
        {
            LvlProperties.lvlloops.Add(new LvlLoop());
            btnLvlLoopDelete.Enabled = true;
            TCLE.PlaySound("UIobjectadd");
            SaveCheckAndWrite(false, "Add New Loop Track");
        }

        private void btnLvlLoopDelete_Click(object sender, EventArgs e)
        {
            LvlProperties.lvlloops.RemoveAt(lvlLoopTracks.CurrentRow.Index);
            TCLE.PlaySound("UIobjectremove");
            //disable button if no more rows exist
            if (lvlLoopTracks.Rows.Count < 1)
                btnLvlLoopDelete.Enabled = false;
            SaveCheckAndWrite(false, "Delete Loop Track");
        }

        private void btnRevertLvl_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true, "");
            //LoadLvl(lvlProperties.revertPoint, LoadedLvl);
            TCLE.PlaySound("UIrevertchanges");
        }

        private void btnLvlSequencer_Click(object sender, EventArgs e)
        {
            //if the Sequencer is open already, attempt to locate it and open it
            IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText == LoadedLvl.Name + " [Sequencer]")));
            if (workspacehastab != null) {
                workspacehastab.DockHandler.Activate();
                (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText == LoadedLvl.Name + " [Sequencer]").DockHandler.Activate();
                return;
            }

            IEnumerable<Form_WorkSpace> workspacewithfloats = TCLE.Workspaces.Cast<Form_WorkSpace>().Where(w => w.dockMain.FloatWindows.Count > 0);
            foreach (Form_WorkSpace ws in workspacewithfloats)
            {
                IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText == LoadedLvl.Name + " [Sequencer]").FirstOrDefault();
                if (activate != null) {
                    activate.DockHandler.Activate();
                    return;
                }
            }
            //this finds a pane in the active workspace that has matching extensions already open on it
            var Panes = TCLE.ActiveWorkspace.dockMain.Panes;
            DockPane OpenHere = Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(".leaf")).Any());

            Form_LeafEditor leaf = new(LvlProperties) { DockAreas = DockAreas.Document | DockAreas.Float };
            if (OpenHere != null)
                leaf.Show(OpenHere, null);
            else
                leaf.Show(TCLE.ActiveWorkspace.dockMain, DockState.Document);
        }
        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public void InitializeLvlStuff()
        {
            ///customize Loop Track list a bit
            //custom column containing comboboxes per cell
            lvlLoopTracks.Columns[2].ValueType = typeof(decimal);
            lvlLoopTracks.Columns[2].DefaultCellStyle.Format = "0.##";
            ///
            dockPanel1.Theme = TCLE.DockTheme;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //
            contentMain.Controls.Add(panelMain);
            panelMain.Dock = DockStyle.Fill;
            //
            contentTunnel.Controls.Add(panelTunnel);
            panelTunnel.Dock = DockStyle.Fill;
            //
            contentLoop.Controls.Add(panelLoop);
            panelLoop.Dock = DockStyle.Fill;
            //
            if (File.Exists($@"{TCLE.AppLocation}\settings\layout_lvl.config"))
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_lvl.config", m_deserializeDockContent);
            else {
                contentMain.Show(dockPanel1, DockState.Document);
                contentTunnel.Show(contentMain.Pane, DockAlignment.Right, 0.4d);
                contentLoop.Show(contentTunnel.Pane, DockAlignment.Bottom, 0.4d);
            }
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_lvl.config");
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString.Contains("Paths/Tunnels"))
                return contentTunnel;
            if (persistString is "Leaf List")
                return contentMain;
            if (persistString is "Loop Tracks")
                return contentLoop;

            throw new NotImplementedException();
        }

        public object GetProperties()
        {
            return LvlProperties;
        }

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
            this.Text = LoadedLvl.Name;
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;

            lvlProperties = new(this, filepath) {
                approachbeats = (int)_load["approach_beats"],
                volume = (decimal)_load["volume"],
                allowinput = (string)_load["input_allowed"] == "True",
                tutorialtype = (string)_load["tutorial_type"],
                seqJSON = _load["seq_objs"]
            };

            //Clear DGVs so new data can load
            lvlLoopTracks.Rows.Clear();
            lvlLeafList.Rows.Clear();
            lvlLeafPaths.Rows.Clear();
            LvlLeafs.Clear();

            //load loop track names and paths to lvlLoopTracks DGV
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[1]).DataSource = TCLE.ProjectSamples.Select(x => x.obj_name).ToList();
            foreach (dynamic samp in _load["loops"]) {
                lvlProperties.lvlloops.Add(new LvlLoop() {
                    sample = (string)samp["samp_name"],
                    beats = (decimal?)samp["beats_per_loop"] == null ? 0 : (decimal)samp["beats_per_loop"]
                });
            }
            ///load leafs associated with this lvl
            lvlProperties.lvlleafs.CollectionChanged -= lvlleaf_CollectionChanged;
            foreach (dynamic leaf in _load["leaf_seq"]) {
                LvlLeafs.Add(new LvlLeafData() {
                    leafname = (string)leaf["leaf_name"],
                    beats = (int)leaf["beat_cnt"],
                    paths = leaf["sub_paths"].ToObject<List<string>>(),
                    id = TCLE.rng.Next(0, 1000000)
                });
            }
            lvlProperties.lvlleafs.CollectionChanged += lvlleaf_CollectionChanged;
            lvlleaf_CollectionChanged(null, null);

            btnLvlLeafRandom.Enabled = true;
            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            EditorIsSaved = true;
            btnLvlSequencer.Enabled = true;
            RecalculateRuntime();
        }

        public void AddFiletoLvl(string path, int index = -1)
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
                        ProjectExplorer.CreateTreeView();
                    }
                    else {
                        MessageBox.Show($"A file with that name already exists in \"{TCLE.WorkingFolder.FullName}\". File not copied over.", "Thumper Custom Level Editor");
                        return;
                    }
            }
            TCLE.PlaySound("UIobjectadd");
            //Setup list of tunnels if copy check is enabled
            List<string> copytunnels = new();
            if (chkTunnelCopy.Checked) {
                copytunnels = new List<string>(LvlLeafs.Last().paths);
            }
            //add leaf data to the list
            if (index is -1)
                LvlLeafs.Add(new LvlLeafData() {
                    leafname = (string)_load["obj_name"],
                    beats = (int)_load["beat_cnt"],
                    paths = new List<string>(copytunnels),
                    id = TCLE.rng.Next()
                });
            else
                LvlLeafs.Insert(index, new LvlLeafData() {
                    leafname = (string)_load["obj_name"],
                    beats = (int)_load["beat_cnt"],
                    paths = new List<string>(copytunnels),
                    id = TCLE.rng.Next()
                });
            SaveCheckAndWrite(false, "Add New Leaf");
        }

        public void LvlUpdatePaths(LvlLeafData leaf)
        {
            lvlLeafPaths.Rows.Clear();
            contentTunnel.TabText = $"Paths/Tunnels - {leaf.leafname}";
            //for each path in the selected leaf, populate the paths DGV
            foreach (string path in leaf.paths) {
                //path may have been manually added and could not exist
                if (TCLE.LvlPaths.Contains(path))
                    lvlLeafPaths.Rows.Add(path);
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

        public void LvlBuildPathList()
        {
            LvlProperties.sublevel.paths.Clear();
            LvlProperties.sublevel.paths = lvlLeafPaths.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value.ToString()).ToList();
            LvlUpdatePaths(LvlProperties.sublevel);
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedLvl.FullName);
            LoadLvl(_load, LoadedLvl);
            RecalculateRuntime();
            lvlLeafList.Invalidate();
        }

        public List<SaveState> UndoList = new();
        public List<SaveState> GetUndoList()
        {
            return UndoList;
        }

        public void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            LoadLvl(UndoList[undolistindex].savestate, LoadedLvl);
            UndoList.RemoveRange(0, undolistindex);
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (LoadedLvl == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Lvl File (*.lvl)|*.lvl";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedlvl = new FileInfo(sfd.FileName);

                lvlProperties ??= new(this, loadedlvl) {
                    approachbeats = 16,
                    volume = 1,
                    allowinput = true,
                    tutorialtype = "TUTORIAL_NONE"
                };

                SaveCheckAndWrite(true, "", true);
                if (isnew)
                    TCLE.CloseFileLock(loadedlvl);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return loadedlvl;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorIsLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(LvlProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedLvl.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    reason = Reason,
                    savestate = _saveJSON
                });
            }
            else {
                this.Text = LoadedLvl.Name;
                lvlProperties.seqJSON = _saveJSON["seq_objs"];
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedLvl], _saveJSON);
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedLvl.Name);
                TCLE.FindEditorRunMethod(typeof(Form_GateEditor), "RecalculateRuntime");
                TCLE.FindEditorRunMethod(typeof(Form_MasterEditor), "RecalculateRuntime");
                if (playsound) TCLE.PlaySound("UIsave");
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
                } catch (ArgumentOutOfRangeException) { }
            }
        }

        public int RecalculateRuntime()
        {
            if (EditorIsLoading || SaveOnlyNoLoad)
                return 0;
            int beattotal = 0;
            foreach (LvlLeafData _leaf in LvlLeafs) {
                FileInfo leaffile = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{_leaf.leafname}")).Value?.File;
                leaffile?.Refresh();
                int beats = (leaffile != null && leaffile.Exists) ? 0 : -1;
                if (beats == -1) {
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].DefaultCellStyle.BackColor = Color.Maroon;
                    lvlLeafList.Rows[LvlLeafs.IndexOf(_leaf)].Cells[2].Value = $"file not found";
                    _leaf.beats = 1;
                    _leaf.NotFound = true;
                }
                else {
                    beats = (int)TCLE.LoadFileLock(leaffile.FullName)["beat_cnt"];
                    beattotal += beats;
                    _leaf.beats = beats;
                    _leaf.NotFound = false;
                    string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
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
            foreach (Sequencer_Object seq_obj in _properties.seq_objs.Where(x => !x.isdefault)) {
                //skip blank tracks
                if (seq_obj.friendly_param == null)
                    continue;
                JObject s = new();
                //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
                if (seq_obj.obj_name.Contains(".leaf") || string.IsNullOrEmpty(seq_obj.obj_name))
                    seq_obj.obj_name = (string)_save["obj_name"];
                s.Add("obj_name", seq_obj.obj_name.Replace("leafname", (string)_save["obj_name"]));
                //write param_path or param_path_hash
                if (seq_obj.param_path.StartsWith("0x"))
                    s.Add("param_path_hash", seq_obj.param_path.Replace("0x", ""));
                else
                    s.Add("param_path", $"{seq_obj.param_path}{(seq_obj.param_path_lane != "none" ? "." + seq_obj.param_path_lane : "")}");
                s.Add("trait_type", seq_obj.trait_type);
                JArray datapoints = new();
                foreach (SeqDataPoint datapoint in seq_obj.data_points.Where(x => x != null && x.value is not null)) {
                    JObject d = new() {
                        { "beat", datapoint.beat },
                        { "value", decimal.Parse(datapoint.value.ToString()) },
                        { "interp", $"kTraitInterp{datapoint.interpolation ?? "Linear"}" },
                        { "ease", $"k{datapoint.ease?.Replace(" ", "") ?? "EaseInOut"}" }
                    };

                    datapoints.Add(d);
                }
                s.Add("data_points", datapoints);
                ///end
                //add the rest of the keys to this seq_obj
                s.Add("step", seq_obj.step.ToString());
                s.Add("default", seq_obj.defaultvalue);
                s.Add("footer", seq_obj.footer);
                s.Add("editor_data", new JArray() { new object[] { seq_obj.highlight_color.ToArgb(), seq_obj.highlight_value } });
                s.Add("enabled", seq_obj.enabled.ToString());

                seq_objs.Add(s);
            }
            //add all seq_objs to the overall leaf
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

        private void lvlLoopTracks_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            //button is in column 0, so that's where to draw the image
            if (e.ColumnIndex == 0) {
                CellPaint(e);
            }
        }
        private void CellPaint(DataGridViewCellPaintingEventArgs e)
        {
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            //get dimensions
            int w = Properties.Resources.icon_play.Width;
            int h = Properties.Resources.icon_play.Height;
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            if (TCLE.PlayingChannels.Any(x => x.Item2 == lvlLoopTracks[1, e.RowIndex].Value.ToString()))
                e.Graphics.DrawImage(Properties.Resources.icon_stop, new Rectangle(x, y, w, h));
            else
                e.Graphics.DrawImage(Properties.Resources.icon_play, new Rectangle(x, y, w, h));
            e.Handled = true;
        }

        private void lvlLoopTracks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            if (e.ColumnIndex == 0) {
                AudioPlayback(lvlLoopTracks[e.ColumnIndex, e.RowIndex]);
            }
        }

        private void AudioPlayback(DataGridViewCell CellToPlay)
        {
            if (TCLE.PlaySampleOneOff(CellToPlay, TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == (string)CellToPlay.OwningRow.Cells[1].Value), out SampChannel)) {
                lvlLoopTracks.InvalidateCell(CellToPlay);
            }
            else {
                lvlLoopTracks.InvalidateCell(CellToPlay);
            }
        }

        private void volumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            //Bass.BASS_SetVolume(volumeSlider1.Volume);
        }
    }
}
