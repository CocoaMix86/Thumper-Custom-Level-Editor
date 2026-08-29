using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Un4seen.Bass;
using WeifenLuo.WinFormsUI.Docking;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;
using System.ComponentModel;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class EditorLvl : EditorBase
    {
        #region Form Construction
        public EditorLvl()
        {
            InitializeComponent();
        }
        public EditorLvl(dynamic load = null, FileInfo filepath = null, bool simpleload = false) : base(filepath, false, simpleload)
        {
            SimpleLoad = simpleload;
            if (SimpleLoad) {
                LoadLvlSimple(load);
                return;
            }

            InitializeComponent();
            RenderForm();
            ColorFormElements();

            if (load != null) {
                LoadLvl(load);
                _ = new EditorLeaf(LvlProperties, filepath, true);
                UndoList.Add(new SaveState() {
                    Reason = "",
                    State = load
                });
            }
        }

        public void RenderForm()
        {
            if (SimpleLoad)
                return;
            ///customize Loop Track list a bit
            //custom column containing comboboxes per cell
            lvlLoopTracks.Columns[2].ValueType = typeof(decimal);
            lvlLoopTracks.Columns[2].DefaultCellStyle.Format = "0.##";
            btnLvlLoopAdd.Enabled = true;
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
            lvlToolStrip.Renderer = new ToolStripOverride();
            lvlPathsToolStrip.Renderer = new ToolStripOverride();
            lvlLoopToolStrip.Renderer = new ToolStripOverride();
            TCLE.DoubleBufferDGV(lvlLeafList);
            TCLE.DoubleBufferDGV(lvlLoopTracks);
            TCLE.DoubleBufferDGV(lvlLeafPaths);
            btnLvlPathView.Checked = Properties.Settings.Default.PreviewTunnel;
            //
            try {
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_lvl.config", m_deserializeDockContent);
            } catch {
                contentMain.Show(dockPanel1, DockState.Document);
                contentTunnel.Show(contentMain.Pane, DockAlignment.Right, 0.4d);
                contentLoop.Show(contentTunnel.Pane, DockAlignment.Bottom, 0.4d);
            }

            if (TCLE.ClipboardPaths.Count > 0)
                btnLvlPasteTunnel.Enabled = true;
        }

        public override void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorLvlBG;
            lvlLeafList.BackgroundColor = Properties.Settings.Default.ColorLvlLeafBG;
            lvlLeafPaths.BackgroundColor = Properties.Settings.Default.ColorLvlTunnelBG;
            lvlLoopTracks.BackgroundColor = Properties.Settings.Default.ColorLvlLoopsBG;
        }

        private void Form_LvlEditor_Shown(object sender, EventArgs e)
        {
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!this.Saved) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region Variables
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LvlProperties LvlProperties
        {
            get { return _lvlproperties; }
            set {
                _lvlproperties = value;
                SaveCheckAndWrite(false, "uuuuuhhhhhhhhhhhhhh");
            }
        }
        private LvlProperties _lvlproperties;
        private List<DataGridViewRow> SelectedRows = new();
        private List<DataGridViewRow> SelectedRowsPaths = new();
        public BindingList<LvlLeafData> LvlLeafs => LvlProperties.Leafs;
        public int SampChannel;
        private DeserializeDockContent m_deserializeDockContent;
        public EditorBaseSub contentTunnel = new() {
            TabText = "Paths/Tunnels",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public EditorBaseSub contentMain = new() {
            TabText = "Leaf List",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public EditorBaseSub contentLoop = new() {
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
        int CurrentRow;
        private void lvlLeafList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || LvlLeafs.Count == 0 || e.RowIndex > LvlLeafs.Count - 1)
                return;
            if (!IsUndoing && Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;
            CurrentRow = e.RowIndex;
            LvlProperties.SelectedLeaf = LvlLeafs[e.RowIndex];
            contentTunnel.TabText = $"Paths/Tunnels - {LvlProperties.SelectedLeaf.Leaf}";
            lvlLeafPaths.DataSource = null;
            lvlLeafPaths.DataSource = new BindingSource(LvlProperties.SelectedLeaf.Paths, null);
            LvlPaths_ListChanged(null, null);
        }

        bool MouseDown;
        int LastRow = -1;
        private void lvlLeafList_SelectionChanged(object sender, EventArgs e)
        {
            if (MouseDown) {
                lvlLeafList.SelectionChanged -= lvlLeafList_SelectionChanged;
                lvlLeafList.ClearSelection();
                foreach (DataGridViewRow dgvr in SelectedRows) {
                    if (dgvr.Index is not -1)
                        lvlLeafList.Rows[dgvr.Index].Selected = true;
                }
                lvlLeafList.SelectionChanged += lvlLeafList_SelectionChanged;
            }

            if (lvlLeafList.RowCount < 1 || lvlLeafList.SelectedRows.Count == 0)
                return;
            LvlProperties.SelectedLeaf = LvlLeafs[lvlLeafList.SelectedRows[^1].Index];
        }

        private void lvlLeafList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || e.RowIndex == LastRow)
                return;
            if (lvlLeafList.Rows[e.RowIndex].Selected) {
                SelectedRows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().ToList();
                MouseDown = true;
            }
            else {
                MouseDown = false;
                lvlLeafList.ClearSelection();
            }
        }

        private void lvlLeafList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || !MouseDown)
                return;
            SelectedRows = new() { lvlLeafList.Rows[e.RowIndex] };
            lvlLeafList.ClearSelection();
            MouseDown = false;
        }

        private void lvlLeafList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || LvlLeafs.Count == 0 || e.RowIndex > LvlLeafs.Count - 1)
                return;
            TCLE.OpenFile(ProjectExplorer.TryGetFile(LvlLeafs[e.RowIndex].Leaf, out ProjectItem leaf) ? leaf.File : null);
        }

        private void lvlLeafList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void lvlLeafPaths_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;
        }

        private void lvlLeafPaths_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || e.RowIndex == LastRow)
                return;
            if (lvlLeafPaths.Rows[e.RowIndex].Selected) {
                SelectedRowsPaths = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().ToList();
                MouseDown = true;
            }
            else {
                MouseDown = false;
                lvlLeafPaths.ClearSelection();
            }
        }

        private void lvlLeafPaths_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || !MouseDown)
                return;
            SelectedRowsPaths = new() { lvlLeafPaths.Rows[e.RowIndex] };
            lvlLeafPaths.ClearSelection();
            MouseDown = false;
        }

        private void lvlLeafPaths_SelectionChanged(object sender, EventArgs e)
        {
            if (lvlLeafPaths.RowCount < 1 || lvlLeafPaths.SelectedRows.Count == 0)
                return;
            if (MouseDown) {
                lvlLeafPaths.SelectionChanged -= lvlLeafPaths_SelectionChanged;
                lvlLeafPaths.ClearSelection();
                foreach (DataGridViewRow dgvr in SelectedRowsPaths) {
                    if (dgvr.Index is not -1)
                        lvlLeafPaths.Rows[dgvr.Index].Selected = true;
                }
                lvlLeafPaths.SelectionChanged += lvlLeafPaths_SelectionChanged;
            }
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
                    // Proceed with the drag and drop, passing in the list item.var SelectedRows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                    List<DataGridViewRow> _SelectedRows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().ToList();
                    _SelectedRows.Sort((row1, row2) => row2.Index.CompareTo(row1.Index));
                    LeafsToMove = _SelectedRows.Select(x => LvlLeafs[x.Index]).ToList();
                    //
                    TCLE.DragSource = "LeafList";
                    LogUndo = false;
                    //
                    DragDropEffects dropEffect = lvlLeafList.DoDragDrop(LeafsToMove, DragDropEffects.Move);
                    //
                    LeafsToMove = null;
                    LogUndo = true;
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
                if (targetRow != -1 && targetRow != previousDragOver) {
                    if (targetRow + LeafsToMove.Count > LvlLeafs.Count)
                        return;
                    lvlLeafList.SelectionChanged -= lvlLeafList_SelectionChanged;
                    foreach (LvlLeafData leaf in LeafsToMove) {
                        LvlLeafs.Remove(leaf);
                    }
                    lvlLeafList.ClearSelection();
                    for (int x = 0; x < LeafsToMove.Count; x++) {
                        try {
                            LvlLeafs.Insert(targetRow, LeafsToMove[x]);
                            if (x == 0)
                                lvlLeafList.CurrentCell = lvlLeafList[0, targetRow];
                            lvlLeafList.Rows[targetRow].Selected = true;
                        } catch (Exception) {
                            LvlLeafs.Add(LeafsToMove[x]);
                            if (x == 0)
                                lvlLeafList.CurrentCell = lvlLeafList[0, lvlLeafList.RowCount - 1];
                            lvlLeafList.Rows[lvlLeafList.RowCount - 1].Selected = true;
                        }
                    }
                    lvlLeafList.SelectionChanged += lvlLeafList_SelectionChanged;
                    previousDragOver = targetRow;
                }
            }
        }

        private void lvlLeafList_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LeafList" and not "FileExplorer")
                return;
            if (LeafsToMove != null)
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetData(typeof(TreeNode)) is TreeNode)
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
                AddFiletoLvl(new FileInfo($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}"), TargetRowToPaint);
            }
            else if (LeafsToMove != null) {
                LogUndo = true;
                SaveCheckAndWrite(false, "Reorder Leafs");
                LeafsToMove = null;
            }
            else if (e.Data.GetData(typeof(List<LvlLeafData>)) is List<LvlLeafData> leafs) {
                foreach (LvlLeafData leaf in leafs)
                    LvlLeafs.Insert(TargetRowToPaint, leaf.Clone());
            }
            else if (e.Data.GetData(typeof(List<string>)) is List<string> leafs2) {
                foreach (string leaf in leafs2)
                    AddFiletoLvl((ProjectExplorer.TryGetFile(leaf, out ProjectItem _leaf) ? _leaf.File : null), TargetRowToPaint);
            }
            TargetRowToPaint = -3;
            previousDragOver = -2;
            lvlLeafList.Invalidate();
        }
        ///
        private List<LvlPath> PathsToMove;
        private void lvlLeafPaths_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (TCLE.DragSource is "none" && (e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (PathsToMove == null && dragBoxFromMouseDownPaths != Rectangle.Empty && !dragBoxFromMouseDownPaths.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.                    
                    List<DataGridViewCell> SelectedRows = lvlLeafPaths.SelectedCells.Cast<DataGridViewCell>().ToList();
                    SelectedRows.Sort((row1, row2) => row2.RowIndex.CompareTo(row1.RowIndex));
                    PathsToMove = SelectedRows.Select(x => LvlProperties.SelectedLeaf.Paths[x.RowIndex]).ToList();
                    TCLE.DragSource = "PathList";
                    //
                    DragDropEffects dropEffect = lvlLeafPaths.DoDragDrop(PathsToMove, DragDropEffects.Move);
                    PathsToMove = null;
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
            if (PathsToMove == null) {
                if (targetRow != previousDragOver) {
                    previousDragOver = targetRow;
                    TargetRowToPaint = targetRow;
                    if (TargetRowToPaint is -1)
                        TargetRowToPaint = lvlLeafPaths.RowCount;
                    lvlLeafPaths.Invalidate();
                }
            }
            else {
                /*
                if (targetRow != -1 && targetRow != previousDragOver) {
                    foreach (LvlPath path in PathsToMove) {
                        LvlProperties.SelectedLeaf.Paths.Remove(path);
                        LvlProperties.SelectedLeaf.Paths.Insert(targetRow, path);
                        previousDragOver = targetRow;
                        lvlLeafPaths.Rows[targetRow].Selected = true;
                    }
                }*/
                if (targetRow != -1 && targetRow != previousDragOver) {
                    if (targetRow + PathsToMove.Count > LvlProperties.SelectedLeaf.Paths.Count)
                        return;
                    lvlLeafPaths.SelectionChanged -= lvlLeafPaths_SelectionChanged;
                    foreach (LvlPath leaf in PathsToMove) {
                        LvlProperties.SelectedLeaf.Paths.Remove(leaf);
                    }
                    lvlLeafPaths.ClearSelection();
                    for (int x = 0; x < PathsToMove.Count; x++) {
                        try {
                            LvlProperties.SelectedLeaf.Paths.Insert(targetRow, PathsToMove[x]);
                            if (x == 0)
                                lvlLeafPaths.CurrentCell = lvlLeafPaths[0, targetRow];
                            lvlLeafPaths.Rows[targetRow].Selected = true;
                        } catch (Exception) {
                            LvlProperties.SelectedLeaf.Paths.Add(PathsToMove[x]);
                            if (x == 0)
                                lvlLeafPaths.CurrentCell = lvlLeafPaths[0, lvlLeafPaths.RowCount - 1];
                            lvlLeafPaths.Rows[lvlLeafPaths.RowCount - 1].Selected = true;
                        }
                    }
                    lvlLeafPaths.SelectionChanged += lvlLeafPaths_SelectionChanged;
                    previousDragOver = targetRow;
                }
            }
        }

        private void lvlLeafPaths_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "PathList")
                return;
            if (PathsToMove != null) {
                e.Effect = DragDropEffects.Move;
            }
            else {
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
            if (PathsToMove != null) {
                SaveCheckAndWrite(false, "Reorder Paths on Leaf");
            }
            /*
            else {
                if (e.Data.GetData(typeof(List<string>)) is List<string>) {
                    foreach (string path in e.Data.GetData(typeof(List<string>)) as List<string>) {
                        LvlProperties.SelectedLeaf.Paths.Remove(path);
                        if (TargetRowToPaint >= LvlProperties.SelectedLeaf.Paths.Count)
                            LvlProperties.SelectedLeaf.Paths.Add(path);
                        else
                            LvlProperties.SelectedLeaf.Paths.Insert(TargetRowToPaint, path);
                    }

                    SaveCheckAndWrite(false, "Reorder Paths on Leaf");
                }
            }*/
            TargetRowToPaint = -3;
            previousDragOver = -2;
        }
        ///

        private static SolidBrush ClearColor = new(Color.Black);
        private static Pen PenGreen = new(Color.Green, 4);
        private static Pen PenViolet = new(new SolidBrush(Color.Violet), 3);
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
                e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 8);
            e.Graphics.FillRoundedRectangle(new SolidBrush(UtilMath.Blend(e.InheritedRowStyle.BackColor, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);

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

            if (Playback.IsPlaying && this.WorkingFile.Name == Playback.GlobalCurrentLvl) {
                //if (Playback.PlaybackBeat > LvlLeafs[e.RowIndex].beatstart + (LvlProperties.approachbeats < 8 ? 8 : 0) && (Playback.PlaybackBeat - LvlLeafs[e.RowIndex].beatstart + (LvlProperties.approachbeats < 8 ? 8 : 0)) < LvlLeafs[e.RowIndex].beats)
                if (LvlLeafs[e.RowIndex].Leaf == Playback.GlobalCurrentLeaf) {
                    double pixelsperbeat = (double)e.RowBounds.Width / (double)LvlLeafs[e.RowIndex].Beats;
                    double offset = Playback.PlaybackBeat - Playback.GlobalCurrentOffsetLvl - (LvlLeafs[e.RowIndex].BeatStart - LvlProperties.ApproachBeats) + /*(Playback.Type != "lvl" ? LvlProperties.ApproachBeats : 0) +*/ Playback.PlaybackSubBeat;
                    e.Graphics.DrawLine(PenViolet, (int)(pixelsperbeat * offset), e.RowBounds.Top, (int)(pixelsperbeat * offset), e.RowBounds.Bottom);
                }
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
        ///
        private void lvlLoopTracks_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = (DataGridView)sender;
            Rectangle headerBounds = new(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString($"Loop Track {e.RowIndex}", this.Font, Brushes.Black, headerBounds, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private void lvlLoopTracks_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            //button is in column 0, so that's where to draw the image
            if (e.ColumnIndex == 0)
                CellPaint(e);
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
            if (UtilAudio.PlayingChannels.Any(x => x.Item2 == lvlLoopTracks[1, e.RowIndex].Value.ToString()))
                e.Graphics.DrawImage(Properties.Resources.icon_stop, new Rectangle(x, y, w, h));
            else
                e.Graphics.DrawImage(Properties.Resources.icon_play, new Rectangle(x, y, w, h));
            e.Handled = true;
        }

        //Cell value changed
        private void lvlLeafPaths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (LvlProperties == null)
                return;
            //Delete button enabled/disabled if rows exist
            btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //set lvl save flag to false
            SaveCheckAndWrite(false, "Tunnels Changed");
            */
        }

        /// DGV LVLLOOPTRACKS
        //Cell value changed
        private void lvlLoopTracks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            else if (e.ColumnIndex == 1)
                LvlProperties.LvlLoops[e.RowIndex].SampleName = $"{lvlLoopTracks.Rows[e.RowIndex].Cells[1].Value}";
            else if (e.ColumnIndex == 2) {
                LvlProperties.LvlLoops[e.RowIndex].Beats = decimal.TryParse(lvlLoopTracks.Rows[e.RowIndex].Cells[2].Value.ToString(), out decimal _beats) ? _beats : 0;
            }
            SaveCheckAndWrite(false, "Loop Track Sample/Beats Changed");
        }
        private void lvlLoopTracks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        ///_LVLLEAF - Triggers when the collection changes
        public void LvlLeaf_CollectionChanged(object sender, ListChangedEventArgs e)
        {
            if (EditorIsLoading)
                return;
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
        public void LvlLoop_CollectionChanged(object sender, ListChangedEventArgs e)
        {
            if (SimpleLoad)
                return;
            btnLvlLoopDelete.Enabled = LvlProperties.LvlLoops.Count > 0;

            if (EditorIsLoading)
                return;
            UpdateLoopHeaders();
        }

        public void LvlPaths_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (EditorIsLoading)
                return;
            int _paths = LvlProperties.SelectedLeaf.Paths.Count;
            //enable a bunch of buttons based on if paths exist or not
            btnLvlPathAdd.Enabled = true;
            btnLvlPathDelete.Enabled = _paths > 0;
            btnLvlCopyTunnel.Enabled = _paths > 0;
            btnLvlRandomTunnel.Enabled = btnLvlPathAdd.Enabled;
            btnLvlPathUp.Enabled = _paths > 1;
            btnLvlPathDown.Enabled = _paths > 1;
            btnLvlPathClear.Enabled = _paths > 0;
            //monke
        }

        public void UpdateLoopHeaders()
        {
            btnLvlLoopDelete.Enabled = lvlLoopTracks.Rows.Count > 0;
            TCLE.ResizeHeaders(lvlLoopTracks);
            lvlLoopTracks.Invalidate();
        }

        private void btnLvlPathView_CheckedChanged(object sender, EventArgs e)
        {
            //save check state
            bool _checkstate = btnLvlPathView.Checked;
            Properties.Settings.Default.PreviewTunnel = _checkstate;
            //update every active lvl document with new state
            foreach (EditorLvl lvl in TCLE.Documents.Values.OfType<EditorLvl>())
                lvl.btnLvlPathView.Checked = _checkstate;
        }

        private void lvlLeafPaths_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || !btnLvlPathView.Checked) {
                TCLE.Instance.pictureTunnelViewer.Visible = false;
                return;
            }
            //get image of tunnel
            string pathname = LvlProperties.SelectedLeaf.Paths[e.RowIndex].Name;
            if (string.IsNullOrEmpty(pathname)) {
                TCLE.Instance.pictureTunnelViewer.Visible = false;
                return;
            }
            //calculate position to show the tunnel image
            //apply some clamping to viewer height
            Point mouse = TCLE.Instance.PointToClient(System.Windows.Forms.Cursor.Position);
            int height = mouse.Y + 150 > TCLE.Instance.Height ? TCLE.Instance.Height - 300 : mouse.Y - 150;
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
            LogUndo = false;
            foreach (LvlLeafData lvd in todelete)
                LvlLeafs.Remove(lvd);

            if (LvlLeafs.Count > 0) {
                if (lvlLeafList.SelectedRows.Count == 0)
                    lvlLeafList.Rows[^1].Selected = true;
            }

            LogUndo = true;
            UtilAudio.PlaySound("UIobjectremove");
            SaveCheckAndWrite(false, "Remove Leaf");
            lvlLeafList_CellClick(null, new DataGridViewCellEventArgs(0, _in >= LvlLeafs.Count ? _in - 1 : _in));
        }
        private void btnLvlLeafAdd_Click(object sender, EventArgs e)
        {
            if (TCLE.DragDropItems.Items is not "leaf" || !TCLE.DragDropItems.Visible) {
                TCLE.DragDropItems.Items = "leaf";
                TCLE.DragDropItems.Show();
                TCLE.DragDropItems.Location = new Point(System.Windows.Forms.Cursor.Position.X + 2, System.Windows.Forms.Cursor.Position.Y + 2);
                if (TCLE.DragDropItems.Location.X + TCLE.DragDropItems.Width > this.Width)
                    TCLE.DragDropItems.Location = new Point(this.Width - TCLE.DragDropItems.Width - 2, TCLE.DragDropItems.Location.Y);
            }
            else
                TCLE.DragDropItems.Hide();
        }

        private void btnLvlLeafUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
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
            SaveCheckAndWrite(false, "Move Leaf Up");
        }

        private void btnLvlLeafDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == lvlLeafList.RowCount - 1))
                return;
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
            SaveCheckAndWrite(false, "Move Leaf Down");
        }

        ///COPY PASTE of leaf
        private void btnLvlLeafCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void btnLvlLeafPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void btnLvlLeafRandom_Click(object sender, EventArgs e)
        {
            List<ProjectItem> leafs = ProjectExplorer.GetFilesByExtension(".leaf");
            AddFiletoLvl(leafs[TCLE.rng.Next(0, leafs.Count)].File);
            SaveCheckAndWrite(false, "Add Random Leaf");
        }

        private void btnLvlPathDelete_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Count == 0)
                return;
            int lastrow = lvlLeafPaths.SelectedRows[^1].Index;
            for (int x = lvlLeafPaths.RowCount - 1; x >= 0; x--) {
                if (lvlLeafPaths.Rows[x].Selected)
                    LvlProperties.SelectedLeaf.Paths.RemoveAt(x);
            }
            if (lvlLeafPaths.Rows.Count > 0) {
                if (lastrow >= lvlLeafPaths.Rows.Count)
                    lvlLeafPaths.Rows[^1].Selected = true;
                else
                    lvlLeafPaths.Rows[lastrow].Selected = true;
            }
            UtilAudio.PlaySound("UItunnelremove");
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
            }
            else
                TCLE.DragDropItems.Hide();
        }

        private void btnLvlPathUp_Click(object sender, EventArgs e)
        {
            if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == 0))
                return;
            List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                LvlProperties.SelectedLeaf.Paths.Insert(dgvr - 1, LvlProperties.SelectedLeaf.Paths[dgvr]);
                LvlProperties.SelectedLeaf.Paths.RemoveAt(dgvr + 1);
            }
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
                LvlProperties.SelectedLeaf.Paths.Insert(dgvr + 2, LvlProperties.SelectedLeaf.Paths[dgvr]);
                LvlProperties.SelectedLeaf.Paths.RemoveAt(dgvr);
            }
            lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] + 1];
            lvlLeafPaths.ClearSelection();
            foreach (int dgvr in selectedrows) {
                lvlLeafPaths.Rows[dgvr + 1].Cells[0].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Tunnel Down");
        }

        private void btnLvlPathClear_Click(object sender, EventArgs e)
        {
            if (LvlProperties.SelectedLeaf.Paths.Count > 0) {
                if (MessageBox.Show("Are you sure you want to clear all?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            LvlProperties.SelectedLeaf.Paths.Clear();
            UtilAudio.PlaySound("UIdataerase");
            SaveCheckAndWrite(false, "Clear Tunnels on Leaf");
        }

        private void btnLvlRandomTunnel_Click(object sender, EventArgs e)
        {
            if (LvlLeafs.Count == 0)
                return;
            LvlProperties.SelectedLeaf.Paths.Add(new(TCLE.LvlPaths[TCLE.rng.Next(1, TCLE.LvlPaths.Count)]));
            LvlPaths_ListChanged(null, null);
            UtilAudio.PlaySound("UItunneladd");
            SaveCheckAndWrite(false, "Add Random Tunnel to Leaf");
        }

        private void btnLvlCopyTunnel_Click(object sender, EventArgs e)
        {
            if (this.WorkingFile == null)
                return;
            if (Control.ModifierKeys == Keys.Shift)
                TCLE.ClipboardPaths = lvlLeafPaths.Rows.Cast<DataGridViewRow>().Select(x => LvlProperties.SelectedLeaf.Paths[x.Index]).ToList();
            else
                TCLE.ClipboardPaths = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => LvlProperties.SelectedLeaf.Paths[x.Index]).ToList();
            //enable the paste button everywhere
            foreach (EditorLvl lvl in TCLE.Documents.Values.OfType<EditorLvl>())
                lvl.btnLvlPasteTunnel.Enabled = true;
            UtilAudio.PlaySound("UIkcopy");
        }

        private void btnLvlPasteTunnel_Click(object sender, EventArgs e)
        {
            LvlLeafs[lvlLeafList.CurrentRow.Index].Paths.AddRange(TCLE.ClipboardPaths);
            UtilAudio.PlaySound("UIkpaste");
            SaveCheckAndWrite(false, "Paste Tunnels");
        }

        private void btnLvlLoopAdd_Click(object sender, EventArgs e)
        {
            LvlProperties.LvlLoops.Add(new LvlLoop());
            btnLvlLoopDelete.Enabled = true;
            UtilAudio.PlaySound("UIobjectadd");
            SaveCheckAndWrite(false, "Add New Loop Track");
        }

        private void btnLvlLoopDelete_Click(object sender, EventArgs e)
        {
            LvlProperties.LvlLoops.RemoveAt(lvlLoopTracks.CurrentRow.Index);
            UtilAudio.PlaySound("UIobjectremove");
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
            UtilAudio.PlaySound("UIrevertchanges");
        }

        private void btnLvlSequencer_Click(object sender, EventArgs e)
        {
            //if the Sequencer is open already, attempt to locate it and open it
            if (TCLE.Documents.TryGetValue($"{this.WorkingFile.Name} [Sequencer]", out EditorBase sequencer)) {
                sequencer.DockHandler.Activate();
                return;
            }
            /*IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as DockWorkspace).dockMain.Documents.Any(y => y.DockHandler.TabText.Replace("*", "") == this.WorkingFile.Name + " [Sequencer]"));
            if (workspacehastab != null) {
                workspacehastab.DockHandler.Activate();
                (workspacehastab as DockWorkspace).dockMain.Documents.First(y => y.DockHandler.TabText.Replace("*", "") == this.WorkingFile.Name + " [Sequencer]").DockHandler.Activate();
                return;
            }

            IEnumerable<DockWorkspace> workspacewithfloats = TCLE.Workspaces.Cast<DockWorkspace>().Where(w => w.dockMain.FloatWindows.Count > 0);
            foreach (DockWorkspace ws in workspacewithfloats) {
                IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText.Replace("*", "") == this.WorkingFile.Name + " [Sequencer]").FirstOrDefault();
                if (activate != null) {
                    activate.DockHandler.Activate();
                    return;
                }
            }*/
            //this finds a pane in the active workspace that has matching extensions already open on it
            DockPaneCollection Panes = TCLE.ActiveWorkspace.dockMain.Panes;
            DockPane OpenHere = Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(".leaf")).Any());

            EditorLeaf leaf = new(LvlProperties, this.WorkingFile, false) { DockAreas = DockAreas.Document | DockAreas.Float };
            TCLE.Documents.TryAdd($"{leaf.WorkingFile.Name} [Sequencer]", leaf);
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
        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_lvl.config");
        }

        private EditorBaseSub? GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString.Contains("Paths/Tunnels"))
                return contentTunnel;
            if (persistString is "Leaf List")
                return contentMain;
            if (persistString is "Loop Tracks")
                return contentLoop;

            return null;

            throw new NotImplementedException();
        }

        public override object GetProperties()
        {
            return LvlProperties;
        }

        public void LoadLvl(dynamic _load)
        {
            //reset flag in case it got stuck previously
            EditorIsLoading = false;
            if (_load == null)
                return;
            //detect if file is actually Lvl or not
            if ((string)_load["obj_type"] != "SequinLevel") {
                MessageBox.Show("This does not appear to be a lvl file!");
                return;
            }
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;

            LvlProperties = new(this) {
                ApproachBeats = (int)_load["approach_beats"],
                Volume = (decimal)_load["volume"],
                AllowInput = (string)_load["input_allowed"] == "True",
                TutorialType = (string)_load["tutorial_type"],
                seqJSON = _load["seq_objs"]
            };
            this.Text = this.WorkingFile.Name;

            //Clear DGVs so new data can load
            ///shouldn't have to do this since it's a new form. There's no data.
            //lvlLoopTracks.Rows.Clear();
            //lvlLeafList.Rows.Clear();
            //lvlLeafPaths.Rows.Clear();
            //LvlLeafs.Clear();

            //load loop track names and paths to lvlLoopTracks DGV
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[1]).DataSource = new BindingSource(TCLE.ProjectSamples, null)/*.Select(x => x.obj_name).ToList()*/;
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[1]).DisplayMember = "Key";
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[1]).ValueMember = "Value";

            foreach (dynamic samp in _load["loops"]) {
                LvlProperties.LvlLoops.Add(new LvlLoop() {
                    SampleName = (string)samp["samp_name"],
                    Beats = Decimal.TryParse((string)samp["beats_per_loop"], out decimal value) ? value : 0
                });
            }
            ///load leafs associated with this lvl
            foreach (dynamic leaf in _load["leaf_seq"]) {
                LvlLeafs.Add(new LvlLeafData(LvlProperties) {
                    Leaf = (string)leaf["leaf_name"],
                    //Beats = (int)leaf["beat_cnt"],
                    ImportPaths = leaf["sub_paths"].ToObject<List<string>>(),
                    id = TCLE.rng.Next()
                });
            }

            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            LvlLeaf_CollectionChanged(null, null);
            this.Saved = true;
            btnLvlSequencer.Enabled = true;

            lvlLeafList.AutoGenerateColumns = false;
            lvlLeafList.Columns[1].DataPropertyName = "Leaf";
            lvlLeafList.Columns[2].DataPropertyName = "Runtime";
            lvlLeafList.DataSource = new BindingSource(LvlLeafs, null);
            lvlLoopTracks.AutoGenerateColumns = false;
            lvlLoopTracks.Columns[1].DataPropertyName = "SampleName";
            lvlLoopTracks.Columns[2].DataPropertyName = "Beats";
            lvlLoopTracks.DataSource = new BindingSource(LvlProperties.LvlLoops, null);
            lvlLeafPaths.AutoGenerateColumns = false;
            lvlLeafPaths.Columns[0].DataPropertyName = "Name";
            RecalculateRuntime();
            UpdateLoopHeaders();
            if (lvlLeafList.RowCount > 0) {
                lvlLeafList.ClearSelection();
                lvlLeafList.Rows[CurrentRow].Selected = true;
                lvlLeafList_CellClick(null, new(0, CurrentRow));
            }
        }

        public void LoadLvlSimple(dynamic _load)
        {
            //set flag that load is in progress. This skips Save method
            EditorIsLoading = true;

            LvlProperties = new(this) {
                ApproachBeats = (int)_load["approach_beats"],
                Volume = (decimal)_load["volume"],
                AllowInput = (string)_load["input_allowed"] == "True",
                TutorialType = (string)_load["tutorial_type"],
                seqJSON = _load["seq_objs"]
            };
            //load loop tracks
            //LvlProperties.LvlLoops.CollectionChanged -= lvlloop_CollectionChanged;
            foreach (dynamic samp in _load["loops"]) {
                LvlProperties.LvlLoops.Add(new LvlLoop() {
                    SampleName = (string)samp["samp_name"],
                    Beats = (decimal?)samp["beats_per_loop"] == null ? 0 : (decimal)samp["beats_per_loop"]
                });
            }
            //load leafs associated with this lvl
            //LvlLeafs.CollectionChanged -= lvlleaf_CollectionChanged;
            foreach (dynamic leaf in _load["leaf_seq"]) {
                LvlLeafs.Add(new LvlLeafData(LvlProperties) {
                    Leaf = (string)leaf["leaf_name"],
                    //Beats = (int)leaf["beat_cnt"],
                    ImportPaths = leaf["sub_paths"].ToObject<List<string>>(),
                    id = TCLE.rng.Next()
                });
            }
            //LvlProperties.Beats = LvlProperties.Leafs.Sum(x => x.Beats);

            //mark that lvl is saved (just freshly loaded)
            EditorIsLoading = false;
            this.Saved = true;
            RecalculateRuntime();
        }

        public void AddFiletoLvl(FileInfo FileToAdd, int index = -1)
        {
            //parse leaf to JSON
            dynamic _load = UtilFile.LoadFileLock(FileToAdd);
            //check if file being loaded is actually a leaf. Can do so by checking the JSON key
            if ((string)_load["obj_type"] != "SequinLeaf") {
                MessageBox.Show("This does not appear to be a leaf file!", "Leaf load error");
                return;
            }
            //check if lvl exists in the same folder as the master. If not, allow user to copy file.
            UtilFile.CopyToWorkingFolderCheck(FileToAdd.FullName);
            //Setup list of tunnels if copy check is enabled
            List<LvlPath> copytunnels = new();
            if (chkTunnelCopy.Checked)
                copytunnels = new List<LvlPath>(LvlLeafs.Last().Paths);
            //add leaf data to the list
            LvlLeafData _toadd = new(LvlProperties) {
                Leaf = (string)_load["obj_name"],
                //Beats = (int)_load["beat_cnt"],
                Paths = new BindingList<LvlPath>(copytunnels),
                id = TCLE.rng.Next()
            };
            if (index is -1)
                LvlLeafs.Add(_toadd);
            else
                LvlLeafs.Insert(index, _toadd);

            SaveCheckAndWrite(false, "Add New Leaf");
            UtilAudio.PlaySound("UIobjectadd");
        }
        /*
        public void LvlUpdatePaths(LvlLeafData leaf)
        {
            lvlLeafPaths.Rows.Clear();
            contentTunnel.TabText = $"Paths/Tunnels - {leaf.Leaf}";
            //for each path in the selected leaf, populate the paths DGV
            foreach (string path in leaf.Paths) {
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
            LvlProperties.SelectedLeaf.Paths = lvlLeafPaths.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[0].Value.ToString()).ToList();
            LvlUpdatePaths(LvlProperties.SelectedLeaf);
        }
        */
        bool IsUndoing;
        public override void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            IsUndoing = true;
            bool _trackNotSaved = this.Saved;
            LoadLvl(UndoList[undolistindex].State);
            UndoList.RemoveRange(0, undolistindex);

            if (!_trackNotSaved) {
                this.Saved = false;
                if (!this.Text.EndsWith('*'))
                    this.Text += '*';
            }
            IsUndoing = false;
        }

        public override void Reload(string OldName, string NewName)
        {
            foreach (LvlLeafData leaf in LvlLeafs) {
                if (leaf.Leaf == OldName)
                    leaf.Leaf = NewName;
            }
        }

        ///SAVE
        public override void Save(bool playsound = true)
        {
            //if _loadedlvl is somehow not set, force Save As instead
            if (this.WorkingFile == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public override FileInfo SaveAs(bool FileIsNew = false, string InitialDir = null)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Lvl File (*.lvl)|*.lvl";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = InitialDir ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                this.WorkingFile = new FileInfo(sfd.FileName);
                EditorIsLoading = true;
                LvlProperties ??= new(this) {
                    ApproachBeats = 16,
                    Volume = 1,
                    AllowInput = true,
                    TutorialType = "TUTORIAL_NONE"
                };
                EditorIsLoading = false;
                SaveCheckAndWrite(true, "", true);
                this.ClearFileLock();
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return this.WorkingFile;
        }

        public override void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorIsLoading || !LogUndo || Playback.Generating)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            this.Saved = IsSaved;
            JObject _saveJSON = BuildSave(LvlProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = this.WorkingFile.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    Reason = Reason,
                    State = _saveJSON
                });
            }
            else {
                this.Text = this.WorkingFile.Name;
                LvlProperties.seqJSON = _saveJSON["seq_objs"];
                //write JSON to file
                UtilFile.WriteFileLock(this.FileLock, _saveJSON);
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(this.WorkingFile.Name);
                foreach (EditorBase doc in TCLE.Documents.Values) {
                    if (doc is EditorGate gate) gate.RecalculateRuntime();
                    if (doc is EditorMaster master) master.RecalculateRuntime();
                }

                if (playsound) UtilAudio.PlaySound("UIsave");

                if (!SimpleLoad) {
                    TCLE.SaveTCL();
                }
            }
        }

        public override void RecalculateRuntime()
        {
            if (EditorIsLoading || SimpleLoad)
                return;
            /*int beattotal = 0;
            foreach (LvlLeafData _leaf in LvlLeafs) {
                beattotal += RecalculateRuntimeLeaf(_leaf);
            }
            if (!Playback.Generating)
                lvlLeafList.Refresh();*/
            UpdateBeatPosition();
            if (!Playback.Generating) {
                lvlLeafList.Refresh();
                lvlLeafList.Invalidate();
            }
            //LvlProperties.Beats = beattotal;
            //return beattotal;
        }

        public int RecalculateRuntimeLeaf(LvlLeafData _leaf)
        {
            /*if (EditorIsLoading || SimpleLoad)
                return 0;

            if (!ProjectExplorer.TryGetFile(_leaf.Leaf, out FileInfo leaffile) || !leaffile.Exists)
                _leaf.Beats = -1;
            else {
                if (!TCLE.CachedRuntimes.TryGetValue(leaffile.Name, out int runtime) || runtime == -1) {
                    _leaf.Beats = (int?)UtilFile.LoadFileLock(leaffile)["beat_cnt"] ?? -1;
                    TCLE.CachedRuntimes[leaffile.Name] = _leaf.Beats;
                }
                else
                    _leaf.Beats = runtime;
            }
            */
            return _leaf.Beats;
        }

        public void UpdateBeatPosition()
        {
            int beatpos = LvlProperties.ApproachBeats;
            foreach (LvlLeafData _leaf in LvlLeafs) {
                _leaf.BeatStart = beatpos;
                beatpos += _leaf.Beats;
            }
        }

        public static JObject BuildSave(LvlProperties _properties)
        {
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLevel" },
                { "obj_name", _properties.ParentEditor.WorkingFile.Name },
                { "approach_beats", _properties.ApproachBeats }
            };
            //this section adds all colume sequencer controls
            JArray seq_objs = new();
            foreach (Sequencer_Object seq_obj in _properties.SequencerObjects.Where(x => !x.IsDefault)) {
                //skip blank tracks
                if (seq_obj.FriendlyParam == null)
                    continue;
                JObject s = new();
                //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
                if (seq_obj.ObjName.Contains(".leaf") || string.IsNullOrEmpty(seq_obj.ObjName))
                    seq_obj.ObjName = (string)_save["obj_name"];
                s.Add("obj_name", seq_obj.ObjName.Replace("leafname", (string)_save["obj_name"]));
                //write param_path or param_path_hash
                if (seq_obj.ParamPath.StartsWith("0x"))
                    s.Add("param_path_hash", seq_obj.ParamPath.Replace("0x", ""));
                else
                    s.Add("param_path", $"{seq_obj.ParamPath}");
                s.Add("trait_type", seq_obj.Default?.TraitTypeString);
                JArray datapoints = new();
                foreach (SeqDataPoint datapoint in seq_obj.Cells.Cast<SeqDataPoint>()) {
                    if (datapoint is null || datapoint.Value is null)
                        continue;
                    if (seq_obj.Default?.TraitType is DefaultSequencerObject.Trait.Float) {
                        JObject d = new() {
                            { "beat", datapoint.beat },
                            { "value", (decimal)datapoint.Value },
                            { "interp", $"kTraitInterp{datapoint.Interpolation ?? "Linear"}" },
                            { "ease", $"k{datapoint.Ease?.Replace(" ", "") ?? "EaseInOut"}" }
                        };
                        datapoints.Add(d);
                    }
                    else {
                        JObject d = new() {
                            { "beat", datapoint.beat },
                            { "value", (int)(decimal)datapoint.Value },
                            { "interp", $"kTraitInterp{datapoint.Interpolation ?? "Linear"}" },
                            { "ease", $"k{datapoint.Ease?.Replace(" ", "") ?? "EaseInOut"}" }
                        };
                        datapoints.Add(d);
                    }
                }
                s.Add("data_points", datapoints);
                ///end
                //add the rest of the keys to this seq_obj
                s.Add("step", seq_obj.Step);
                s.Add("default", seq_obj.DefaultValue);
                s.Add("footer", seq_obj.Default?.Footer);
                s.Add("editor_data", new JArray() { new object[] { seq_obj.HighlightColor.ToArgb(), seq_obj.highlight_value } });
                s.Add("enabled", seq_obj.EnabledInEditor);

                seq_objs.Add(s);
            }
            //add all seq_objs to the overall leaf
            if (_properties.SequencerObjects.Count > 0)
                _save.Add("seq_objs", seq_objs);
            else
                _save.Add("seq_objs", _properties.seqJSON);
            //this section adds all leafs
            JArray leaf_seq = new();
            foreach (LvlLeafData _leaf in _properties.Leafs) {
                JObject s = new() {
                    { "beat_cnt", _leaf.Beats },
                    { "leaf_name", _leaf.Leaf },
                    { "main_path", "default.path" },
                    { "sub_paths", JArray.FromObject(_leaf.Paths.Select(x => x.Name).ToList()) },
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
            foreach (LvlLoop _loop in _properties.LvlLoops) {
                if (_loop.SampleName == null)
                    continue;
                JObject s = new() {
                    { "samp_name", $"{_loop.SampleName}"},
                    { "beats_per_loop", _loop.Beats }
                };

                loops.Add(s);
            }
            _save.Add("loops", loops);
            //final keys
            _save.Add("volume", _properties.Volume);
            _save.Add("input_allowed", _properties.AllowInput);
            _save.Add("tutorial_type", _properties.TutorialType);
            _save.Add("start_angle_fracs", new JArray() { 1, 1, 1 });
            ///end building JSON output
            return _save;
        }

        public override void Copy()
        {
            List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardLvl = LvlLeafs.Where(x => selectedrows.Contains(LvlLeafs.IndexOf(x))).ToList();
            //We reverse the list because they will all paste at the same index. So the last one pasted would be at the top.
            TCLE.ClipboardLvl.Reverse();
            //enable the paste button everywhere
            btnLvlLeafPaste.Enabled = true;
            foreach (EditorLvl lvl in TCLE.Documents.Values.OfType<EditorLvl>())
                lvl.btnLvlLeafPaste.Enabled = true;
            UtilAudio.PlaySound("UIkcopy");
        }

        public override void Cut()
        {
            Copy();

            //delete the copied items from the lvl now
            //LvlLeafs.CollectionChanged -= lvlleaf_CollectionChanged;
            foreach (LvlLeafData leaf in TCLE.ClipboardLvl) {
                LvlLeafs.Remove(leaf);
            }
            //LvlLeafs.CollectionChanged += lvlleaf_CollectionChanged;
            //lvlleaf_CollectionChanged(null, null);
            SaveCheckAndWrite(false, "Cut Leafs");
        }

        public override void Paste()
        {
            int _in = lvlLeafList.CurrentRow?.Index + 1 ?? 0;
            foreach (LvlLeafData lld in TCLE.ClipboardLvl)
                LvlLeafs.Insert(_in, lld.Clone());
            UtilAudio.PlaySound("UIkpaste");
            SaveCheckAndWrite(false, "Paste Leaf");
        }

        #endregion

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
            if (UtilAudio.PlaySampleOneOff(CellToPlay, TCLE.ProjectSamples[(string)CellToPlay.OwningRow.Cells[1].Value], out SampChannel)) {
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

        private int PlaybackStart = -1;
        private int PlaybackEnd = -1;
        private bool PlaybackLoop;
        private bool ForceStop;
        private void btnLvlPlayback_Click(object sender, EventArgs e)
        {
            if (Playback.IsPlaying) {
                Playback.IsPlaying = false;
                ForceStop = true;
            }
            else {
                //timer interval twice as small as the bpm (*500ms, instead of *1000ms), so it can keep up with the Playback threading timer
                timer1.Interval = 30;
                btnLvlPlayback.Image = Properties.Resources.icon_stop;
                Playback.Initialize("lvl");
                Playback.CreatePlaybackFromLvl(LvlProperties);
                Playback.Play(lvlLeafList.SelectedRows.Count > 0 ? LvlLeafs[lvlLeafList.SelectedRows[^1].Index].BeatStart : -1, LvlProperties.Beats + LvlProperties.ApproachBeats, PlaybackLoop, LvlProperties.ApproachBeats);
                if (Playback.IsPlaying) {
                    timer1.Enabled = true;
                }
                else {
                    Bass.BASS_ChannelFree(Playback.MidiStream);
                    TCLE.alzheimer();
                    btnLvlPlayback.Image = Properties.Resources.icon_play2;
                }
            }
        }

        private string _playingleaf;
        private EditorLeaf _playingleafform;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Playback.PlaybackBeat < 0)
                return;
            if (Playback.IsPlaying && !ForceStop) {
                lvlLeafList.Invalidate();
                //show the leaf that's playing
                if (_playingleaf != Playback.GlobalCurrentLeaf) {
                    _playingleaf = Playback.GlobalCurrentLeaf;
                    _playingleafform?.trackEditor.ResetPlayback();
                    _playingleafform = TCLE.Documents.Values.FirstOrDefault(x => x.WorkingFile.Name.StartsWith(_playingleaf)) as EditorLeaf;
                    //switch to the leaf if it's open
                    _playingleafform?.DockHandler?.Activate();
                    _playingleafform?.trackEditor.HorizontalScrollingOffset = 0;
                }
                if (_playingleafform is not null) {
                    _playingleafform.trackEditor.PlaybackPosition = (double)(Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat);
                    _playingleafform.trackEditor.Invalidate();
                    _playingleafform.dgvMasterView.Invalidate();
                    if (Properties.Settings.Default.LeafOptionPlaybackScroll) {
                        int playheadx = (int)Math.Round((Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat) * _playingleafform.trackZoom.Value);
                        int margin = _playingleafform.trackEditor.Width / 3;
                        if (playheadx > _playingleafform.trackEditor.HorizontalScrollingOffset + margin)
                            _playingleafform.trackEditor.HorizontalScrollingOffset = playheadx - margin;
                    }
                }
            }
            else {
                ForceStop = false;
                timer1.Enabled = false;
                btnLvlPlayback.Image = Properties.Resources.icon_play2;
                Playback.StopPlayback();
                lvlLeafList.Invalidate();
                _playingleafform?.trackEditor.Invalidate();
            }
        }
    }
}
