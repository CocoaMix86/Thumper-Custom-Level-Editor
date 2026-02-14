using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Un4seen.Bass;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_MasterEditor : DockContentEx
    {
        #region Form Construction
        public Form_MasterEditor(dynamic load = null, FileInfo filepath = null, bool saveonlynoload = false) : base(filepath)
        {
            InitializeComponent();
            RenderForm();
            ColorFormElements();

            SimpleLoad = saveonlynoload;

            if (load != null) {
                LoadMaster(load);
                LoadEnd(load);
            }
        }

        public void RenderForm()
        {
            if (SimpleLoad)
                return;

            dockPanel1.Theme = TCLE.DockTheme;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //
            contentMain.Controls.Add(panelMain);
            panelMain.Dock = DockStyle.Fill;
            //
            contentPropertyGrid.Controls.Add(propertyGridMaster);
            propertyGridMaster.Dock = DockStyle.Fill;
            //
            try {
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_master.config", m_deserializeDockContent);
            } catch {
                contentMain.Show(dockPanel1, DockState.Document);
                contentPropertyGrid.Show(dockPanel1, DockState.DockRight);
            }

            masterToolStrip.Renderer = new ToolStripOverride();
            TCLE.DoubleBufferDGV(masterLvlList);
        }

        private void Form_MasterEditor_Shown(object sender, EventArgs e)
        {
            propertyGridMaster.Focus();
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
            this.BackColor = Properties.Settings.Default.ColorMasterBG;
            masterLvlList.BackgroundColor = Properties.Settings.Default.ColorMasterLvlBG;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading;
        private bool SimpleLoad;
        private bool IsAddingItems;
        private bool GlobalCheckpoint;
        private bool GlobalPlayPlus;
        private bool GlobalIsolate;
        private bool LogUndo = true;
        public ObservableCollection<MasterLvlData> MasterLvls => MasterProperties.masterlvls;
        public MasterProperties MasterProperties;
        public List<SaveState> UndoList = new();
        private List<DataGridViewRow> SelectedRows = new();
        private DeserializeDockContent m_deserializeDockContent;
        public DockContentEx contentPropertyGrid = new(null) {
            TabText = "Sublevel Props.",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentMain = new(null) {
            TabText = "Sublevels",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        #endregion

        #region EventHandlers
        ///         ///
        /// EVENTS ///
        ///         ///

        /// DGV MASTERLVLLIS
        private void masterLvlList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex > MasterLvls.Count - 1)
                return;
            if (e.RowIndex == -1) {
                if (e.ColumnIndex == 4) {
                    GlobalCheckpoint = !GlobalCheckpoint;
                    foreach (MasterLvlData lvl in MasterLvls) {
                        lvl.checkpoint = GlobalCheckpoint;
                    }
                }
                else if (e.ColumnIndex == 5) {
                    GlobalPlayPlus = !GlobalPlayPlus;
                    foreach (MasterLvlData lvl in MasterLvls) {
                        lvl.playplus = GlobalPlayPlus;
                    }
                }
                else if (e.ColumnIndex == 6) {
                    GlobalIsolate = !GlobalIsolate;
                    foreach (MasterLvlData lvl in MasterLvls) {
                        lvl.isolate = GlobalIsolate;
                    }
                }
                masterLvlList.Invalidate();
                return;
            }
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;

            if (e.ColumnIndex is 4) {
                MasterLvls[e.RowIndex].checkpoint = !MasterLvls[e.RowIndex].checkpoint;
                foreach (MasterLvlData lvl in propertyGridMaster.SelectedObjects) {
                    lvl.checkpoint = MasterLvls[e.RowIndex].checkpoint;
                }
            }
            else if (e.ColumnIndex is 5) {
                MasterLvls[e.RowIndex].playplus = !MasterLvls[e.RowIndex].playplus;
                foreach (MasterLvlData lvl in propertyGridMaster.SelectedObjects) {
                    lvl.playplus = MasterLvls[e.RowIndex].playplus;
                }
            }
            else if (e.ColumnIndex is 6) {
                MasterLvls[e.RowIndex].isolate = !MasterLvls[e.RowIndex].isolate;
                foreach (MasterLvlData lvl in propertyGridMaster.SelectedObjects) {
                    lvl.isolate = MasterLvls[e.RowIndex].isolate;
                }
            }
            masterLvlList.Invalidate();
        }

        bool MouseDown;
        int LastRow = -1;
        private void masterLvlList_SelectionChanged(object sender, EventArgs e)
        {
            if (MouseDown) {
                masterLvlList.SelectionChanged -= masterLvlList_SelectionChanged;
                masterLvlList.ClearSelection();
                foreach (DataGridViewRow dgvr in SelectedRows) {
                    if (dgvr.Index is not -1)
                        masterLvlList.Rows[dgvr.Index].Selected = true;
                }
                masterLvlList.SelectionChanged += masterLvlList_SelectionChanged;
            }

            propertyGridMaster.SelectedObjects = masterLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => MasterLvls[x.Index]).ToArray();
            propertyGridMaster.Refresh();
        }

        private void masterLvlList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || e.RowIndex == LastRow)
                return;
            if (masterLvlList.Rows[e.RowIndex].Selected) {
                SelectedRows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                MouseDown = true;
            }
            else {
                MouseDown = false;
                masterLvlList.ClearSelection();
            }
        }

        private void masterLvlList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void masterLvlList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || !MouseDown)
                return;
            SelectedRows = new() { masterLvlList.Rows[e.RowIndex] };
            masterLvlList.ClearSelection();
            MouseDown = false;
        }

        private void masterLvlList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > MasterLvls.Count - 1)
                return;
            TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{MasterLvls[e.RowIndex].name}")));
        }

        private Rectangle dragBoxFromMouseDown;
        private List<MasterLvlData> LvlsToMove;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int previousDragOver = -2;
        private int TargetRowToPaint = -3;
        private void masterLvlList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (TCLE.DragSource is "none" && (e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (LvlsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.
                    List<DataGridViewRow> _SelectedRows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                    _SelectedRows.Sort((row1, row2) => row2.Index.CompareTo(row1.Index));
                    LvlsToMove = _SelectedRows.Select(x => MasterLvls[x.Index]).ToList();
                    //
                    TCLE.DragSource = "MasterList";
                    IsAddingItems = true;
                    LogUndo = false;
                    //
                    DragDropEffects dropEffect = masterLvlList.DoDragDrop(LvlsToMove, DragDropEffects.Move);
                    //
                    LvlsToMove = null;
                    LogUndo = true;
                    IsAddingItems = false;
                    TCLE.DragSource = "none";
                    TargetRowToPaint = -3;
                    previousDragOver = -2;
                }
            }
        }

        private void masterLvlList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if selection is in the checkbox columns, do not dragdrop
            if (masterLvlList.HitTest(e.X, e.Y).ColumnIndex >= 4)
                return;
            rowIndexFromMouseDown = masterLvlList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown is -1) {
                dragBoxFromMouseDown = Rectangle.Empty;
                return;
            }

            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
        }

        private void masterLvlList_DragOver(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlGateList" and not "MasterList" and not "FileExplorer")
                return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = masterLvlList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = masterLvlList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (LvlsToMove == null) {
                if (targetRow != previousDragOver) {
                    previousDragOver = targetRow;
                    TargetRowToPaint = targetRow;
                    if (TargetRowToPaint is -1)
                        TargetRowToPaint = masterLvlList.RowCount;
                    masterLvlList.Invalidate();
                }
            }
            else {
                if (targetRow != -1 && targetRow != previousDragOver) {
                    if (targetRow + LvlsToMove.Count > MasterLvls.Count)
                        return;
                    masterLvlList.SelectionChanged -= masterLvlList_SelectionChanged;
                    foreach (MasterLvlData leaf in LvlsToMove) {
                        MasterLvls.Remove(leaf);
                    }
                    masterLvlList.ClearSelection();
                    for (int x = 0; x < LvlsToMove.Count; x++) {
                        try {
                            MasterLvls.Insert(targetRow, LvlsToMove[x]);
                            if (x == 0)
                                masterLvlList.CurrentCell = masterLvlList[0, targetRow];
                            masterLvlList.Rows[targetRow].Selected = true;
                        } catch (Exception) {
                            MasterLvls.Add(LvlsToMove[x]);
                            if (x == 0)
                                masterLvlList.CurrentCell = masterLvlList[0, masterLvlList.RowCount - 1];
                            masterLvlList.Rows[masterLvlList.RowCount - 1].Selected = true;
                        }
                    }
                    masterLvlList.SelectionChanged += masterLvlList_SelectionChanged;
                    previousDragOver = targetRow;
                }
            }
        }

        private void masterLvlList_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlGateList" and not "MasterList" and not "FileExplorer")
                return;
            if (LvlsToMove != null)
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetData(typeof(TreeNode)) is TreeNode)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }

        private void masterLvlList_DragDrop(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlGateList" and not "MasterList" and not "FileExplorer")
                return;
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = masterLvlList.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = masterLvlList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            IsAddingItems = true;

            if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode) {
                AddFiletoMaster($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}", TargetRowToPaint);
            }
            else if (LvlsToMove != null) {
                LogUndo = true;
                SaveCheckAndWrite(false, "Reorder Sublevels");
                LvlsToMove = null;
            }
            else if (e.Data.GetData(typeof(List<MasterLvlData>)) is List<MasterLvlData> sublevels) {
                LogUndo = false;
                foreach (MasterLvlData leaf in sublevels)
                    MasterLvls.Insert(TargetRowToPaint, leaf.Clone());
                LogUndo = true;
                SaveCheckAndWrite(false, "Add Lvls");
            }
            else if (e.Data.GetData(typeof(List<string>)) is List<string> sublevels2) {
                LogUndo = false;
                foreach (string leaf in sublevels2)
                    AddFiletoMaster(ProjectExplorer.Files.FirstOrDefault(x => x.Name == leaf)?.FullName, TargetRowToPaint);
                LogUndo = true;
                SaveCheckAndWrite(false, "Add Lvls");
            }
            IsAddingItems = false;
            TargetRowToPaint = -3;
            previousDragOver = -2;
            masterLvlList.Invalidate();
        }

        private static SolidBrush ClearColor = new(Color.Black);
        private static SolidBrush BrushWhite = new(Color.White);
        private static Pen PenBlack = new(Color.Black, 1);
        private static Pen PenGreen = new(Color.Green, 4);
        private static Pen PenViolet = new(new SolidBrush(Color.Violet), 3);
        private void masterLvlList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (e.RowIndex == -1) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            }
            else {
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                CellPaintIcons(e);
                if (e.ColumnIndex is > 3)
                    e.Graphics.DrawLine(PenBlack, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
            }
        }

        int w = 16;
        int h = 16;
        private void CellPaintIcons(DataGridViewCellPaintingEventArgs e)
        {
            //get dimensions
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            //Checkpoint
            if (e.ColumnIndex == 4) {
                if (e.RowIndex is not -1) {
                    if (MasterLvls[e.RowIndex].checkpoint)
                        e.Graphics.DrawImage(Properties.Resources.icon_check_blue, new Rectangle(x, y, w, h));
                }
            }
            //Play+
            if (e.ColumnIndex == 5) {
                if (e.RowIndex is not -1) {
                    if (MasterLvls[e.RowIndex].playplus)
                        e.Graphics.DrawImage(Properties.Resources.icon_check_blue, new Rectangle(x, y, w, h));
                }
            }
            //Isolate
            if (e.ColumnIndex == 6) {
                if (e.RowIndex is not -1) {
                    if (MasterLvls[e.RowIndex].isolate)
                        e.Graphics.DrawImage(Properties.Resources.icon_check_blue, new Rectangle(x, y, w, h));
                }
            }
        }

        private void masterLvlList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
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
            if (MasterLvls.Any(x => x.isolate)) {
                if (MasterLvls[e.RowIndex].isolate)
                    e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(e.InheritedRowStyle.BackColor, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);
                else
                    e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(Color.Gray, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);
            }
            else {
                e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(e.InheritedRowStyle.BackColor, Color.Black, (dgv.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);
            }

            e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);

            if (sender == masterLvlList && TCLE.DragSource is "LvlGateList" or "FileExplorer") {
                if (e.RowIndex == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Top, e.RowBounds.Right, e.RowBounds.Top);
                if (e.RowIndex + 1 == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Bottom, e.RowBounds.Right, e.RowBounds.Bottom);
            }

            if (Playback.IsPlaying) {
                if (MasterLvls[e.RowIndex].name == Playback.GlobalCurrentGate && MasterLvls[e.RowIndex].beatstart + MasterLvls[e.RowIndex].Beats > Playback.PlaybackBeat) {
                    double pixelsperbeat = (double)e.RowBounds.Width / (double)MasterLvls[e.RowIndex].Beats;
                    double offset = Playback.PlaybackBeat - Playback.GlobalCurrentOffsetGate + Playback.PlaybackSubBeat;//MasterLvls[e.RowIndex].beatstart - (MasterLvls[e.RowIndex].restlevelbeats) + 
                    e.Graphics.DrawLine(PenViolet, (int)(pixelsperbeat * offset), e.RowBounds.Top, (int)(pixelsperbeat * offset), e.RowBounds.Bottom);
                }
                else if (MasterLvls[e.RowIndex].name == Playback.GlobalCurrentLvl) {
                    double pixelsperbeat = (double)e.RowBounds.Width / (double)MasterLvls[e.RowIndex].Beats;
                    double offset = Playback.PlaybackBeat - Playback.GlobalCurrentOffsetLvl + Playback.PlaybackSubBeat;//MasterLvls[e.RowIndex].beatstart - (MasterLvls[e.RowIndex].restlevelbeats) + 
                    e.Graphics.DrawLine(PenViolet, (int)(pixelsperbeat * offset), e.RowBounds.Top, (int)(pixelsperbeat * offset), e.RowBounds.Bottom);
                }
            }
        }

        public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SimpleLoad)
                return;

            /*masterLvlList.Rows.Clear();
            foreach (MasterLvlData lvl in MasterLvls)
            {
                masterLvlList.Rows.Add(new object[] {
                    0,
                    (lvl.type == "lvl" ? Properties.Resources.editor_lvl : Properties.Resources.editor_gate),
                    lvl.name,
                    0
                });
            }*/

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) {
                masterLvlList.RowCount = 0;
            }
            //if action ADD, add new row to the master DGV
            //NewStartingIndex and OldStartingIndex track where the changes were made
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                int _in = e.NewStartingIndex;
                //get the runtime of the object
                masterLvlList.Rows.Insert(_in, new object[] {
                    0,
                    (MasterLvls[_in].Type == "lvl" ? Properties.Resources.editor_lvl : Properties.Resources.editor_gate),
                    MasterLvls[_in].name,
                    0
                });
                RecalculateRuntimeSublevel(MasterLvls[_in]);
            }
            //if action REMOVE, remove row from the master DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
            }
            //enable certain buttons if there are enough items for them
            btnMasterLvlDelete.Enabled = MasterLvls.Count > 0;
            btnMasterLvlUp.Enabled = MasterLvls.Count > 1;
            btnMasterLvlDown.Enabled = MasterLvls.Count > 1;
            btnMasterLvlCopy.Enabled = MasterLvls.Count > 0;

            foreach (DataGridViewRow dgvr in masterLvlList.Rows) {
                string levelnum;
                if (MasterLvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_CRAKHED" or "SECTION_BOSS_CRAKHED_FINAL")
                    levelnum = "Ω";
                else if (MasterLvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_PYRAMID")
                    levelnum = "∞";
                else
                    levelnum = (dgvr.Index + 1).ToString();
                dgvr.Cells[0].Value = levelnum;
            }
        }

        private void propertyGridMaster_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveCheckAndWrite(false, "Change Master Property");
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///

        private void btnMasterLvlDelete_Click(object sender, EventArgs e)
        {
            List<MasterLvlData> todelete = new();
            foreach (DataGridViewRow dgvr in masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList()) {
                todelete.Add(MasterLvls[dgvr.Index]);
            }
            int _in = masterLvlList.CurrentRow.Index;
            LogUndo = false;
            foreach (MasterLvlData mld in todelete)
                MasterLvls.Remove(mld);

            LogUndo = true;
            TCLE.PlaySound("UIobjectremove");
            SaveCheckAndWrite(false, "Remove Lvl");
            masterLvlList_CellClick(null, new DataGridViewCellEventArgs(1, _in >= MasterLvls.Count ? _in - 1 : _in));
        }
        private void btnMasterLvlAdd_Click(object sender, EventArgs e)
        {
            if (TCLE.DragDropItems.Items is not "lvlgate" || !TCLE.DragDropItems.Visible) {
                TCLE.DragDropItems.Items = "lvlgate";
                TCLE.DragDropItems.Show();
                TCLE.DragDropItems.Location = new Point(System.Windows.Forms.Cursor.Position.X + 2, System.Windows.Forms.Cursor.Position.Y + 2);
                if (TCLE.DragDropItems.Location.X + TCLE.DragDropItems.Width > this.Width)
                    TCLE.DragDropItems.Location = new Point(this.Width - TCLE.DragDropItems.Width - 2, TCLE.DragDropItems.Location.Y);
            }
            else
                TCLE.DragDropItems.Hide();
        }

        public void AddFiletoMaster(string path, int index = -1)
        {
            //parse leaf to JSON
            dynamic _load = TCLE.LoadFileLock(path);
            //check if file being loaded is actually a leaf. Can do so by checking the JSON key
            if ((string)_load["obj_type"] is not "SequinLevel" and not "SequinGate") {
                MessageBox.Show("That does not appear to be a lvl or a gate file.\nItem not added to master.", "Bumper Custom Level Editor");
                return;
            }
            //check if lvl exists in the same folder as the master. If not, allow user to copy file.
            TCLE.CopyToWorkingFolderCheck(path);
            //add lvl/gate data to the list
            MasterLvlData _import = new() {
                Type = (_load["obj_type"] == "SequinLevel") ? "lvl" : "gate",
                name = (string)_load["obj_name"],
                playplus = true,
                checkpoint = true,
                checkpoint_leader = "<none>",
                rest = "<none>",
                gatesectiontype = "",
                id = TCLE.rng.Next(0, 1000000)
            };
            if (index is -1) 
                MasterLvls.Add(_import);            
            else 
                MasterLvls.Insert(index, _import);
            
            if (!IsAddingItems)
                propertyGridMaster.Refresh();

            SaveCheckAndWrite(false, "Add New Lvl");
            TCLE.PlaySound("UIobjectadd");
        }

        private void btnMasterLvlUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                MasterLvls.Insert(dgvr - 1, MasterLvls[dgvr]);
                MasterLvls.RemoveAt(dgvr + 1);
            }
            masterLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                masterLvlList.Rows[dgvr - 1].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Lvl Up");
        }

        private void btnMasterLvlDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == masterLvlList.RowCount - 1))
                return;
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                MasterLvls.Insert(dgvr + 2, MasterLvls[dgvr]);
                MasterLvls.RemoveAt(dgvr);
            }
            masterLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                masterLvlList.Rows[dgvr + 1].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Lvl Down");
        }

        private void btnMasterLvlCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void btnMasterLvlPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void btnRevertMaster_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //LoadMaster(masterproperties.revertPoint, LoadedMaster);
            TCLE.PlaySound("UIrevertchanges");
        }
        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///
        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_master.config");
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString is "Sublevel Props.")
                return contentPropertyGrid;
            if (persistString is "Sublevels")
                return contentMain;

            throw new NotImplementedException();
        }

        public object GetProperties()
        {
            return MasterProperties;
        }

        public void LoadMaster(dynamic _load)
        {
            if (_load == null)
                return;
            if ((string)_load["obj_type"] != "SequinMaster") {
                MessageBox.Show("This does not appear to be a master file!");
                return;
            }
            EditorLoading = true;

            //setup new master properties
            masterLvlList.Rows.Clear();
            MasterProperties = new(this) {
                skybox = string.IsNullOrEmpty((string)_load["skybox_name"]) ? "<none>" : (string)_load["skybox_name"],
                introlvl = string.IsNullOrEmpty((string)_load["intro_lvl_name"]) ? "<none>" : (string)_load["intro_lvl_name"],
                checkpointlvl = string.IsNullOrEmpty((string)_load["checkpoint_lvl_name"]) ? "<none>" : (string)_load["checkpoint_lvl_name"]
            };
            this.Text = $"{this.WorkingFile.Name}";
            //calc intro lvl
            MasterProperties.introlevelbeats += TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == MasterProperties.introlvl)?.FullName);
            //calc checkpoint lvl
            MasterProperties.checkpointbeats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == MasterProperties.checkpointlvl)?.FullName);

            ///Clear form elements so new data can load
            MasterLvls.Clear();
            ///load lvls associated with this master
            foreach (dynamic _lvl in _load["groupings"]) {
                MasterLvls.Add(new MasterLvlData() {
                    Type = !string.IsNullOrEmpty(((string)_lvl["lvl_name"])) ? "lvl" : "gate",
                    name = !string.IsNullOrEmpty(((string)_lvl["lvl_name"])) ? _lvl["lvl_name"] : _lvl["gate_name"],
                    checkpoint = _lvl["checkpoint"],
                    playplus = _lvl["play_plus"],
                    isolate = _lvl["isolate"] ?? false,
                    checkpoint_leader = _lvl["checkpoint_leader_lvl_name"],
                    rest = _lvl["rest_lvl_name"] == "" ? "<none>" : _lvl["rest_lvl_name"],
                    id = TCLE.rng.Next(0, 10000000)
                });
            }
        }

        public void LoadEnd(dynamic savestate)
        {
            UndoList.Add(new SaveState() {
                reason = "",
                savestate = savestate
            });

            TCLE.ProjectProperties.LevelSections = new() { "SECTION_LINEAR" };
            foreach (MasterLvlData mld in MasterLvls.Where(x => x.checkpoint)) {
                TCLE.ProjectProperties.LevelSections.Add("SECTION_LINEAR");
            }
            ///set save flag (master just loaded, has no changes)
            EditorLoading = false;
            EditorIsSaved = true;
        }

        public List<SaveState> GetUndoList()
        {
            return UndoList;
        }

        public void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            bool _trackNotSaved = EditorIsSaved;
            LoadMaster(UndoList[undolistindex].savestate);
            UndoList.RemoveRange(0, undolistindex);
            propertyGridMaster.Refresh();

            if (!_trackNotSaved) {
                EditorIsSaved = false;
                if (!this.Text.EndsWith("*"))
                    this.Text += '*';
            }
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if LoadedMaster is somehow not set, force Save As instead
            if (this.WorkingFile == null)
                SaveAs();
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false, string startpath = null)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.master)|*.master";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = startpath ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                this.WorkingFile = new FileInfo(sfd.FileName);

                MasterProperties ??= new(this) {
                    skybox = "<none>",
                    introlvl = "<none>",
                    checkpointlvl = "<none>"
                };

                SaveCheckAndWrite(true, "", true);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return this.WorkingFile;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorLoading || !LogUndo || Playback.Generating)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(MasterProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = this.WorkingFile.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    reason = Reason,
                    savestate = _saveJSON
                });
            }
            else {
                this.Text = this.WorkingFile.Name;
                //write JSON to file
                TCLE.WriteFileLock(this.FileLock, _saveJSON);
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(this.WorkingFile.Name);
                //update level sections
                TCLE.ProjectProperties.LevelSections = new() { "SECTION_LINEAR" };
                foreach (MasterLvlData mld in MasterLvls.Where(x => x.checkpoint)) {
                    TCLE.ProjectProperties.LevelSections.Add("SECTION_LINEAR");
                }
                if (!SimpleLoad) {
                    TCLE.SaveTCL();
                }
                //
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public int RecalculateRuntime()
        {
            if (SimpleLoad)
                return 0;
            int beattotal = 0;
            //calc intro lvl
            MasterProperties.introlevelbeats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == MasterProperties.introlvl)?.FullName);
            //calc checkpoint lvl
            MasterProperties.checkpointbeats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == MasterProperties.checkpointlvl)?.FullName);
            //calc each lvl/gate
            foreach (MasterLvlData _lvl in MasterLvls) {
                beattotal += RecalculateRuntimeSublevel(_lvl, false);
            }
            UpdateBeatPosition();
            masterLvlList.Refresh();
            return beattotal + MasterProperties.introlevelbeats;
        }

        public int RecalculateRuntimeSublevel(MasterLvlData _lvl, bool updatebeats = true)
        {
            if (SimpleLoad)
                return 0;

            _lvl.Beats = TCLE.CalculateSublevelRuntime(_lvl);
            //include rest in lvl's runtime
            if (_lvl.rest is not "<none>" and not null)
                _lvl.restlevelbeats += TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == _lvl.rest)?.FullName);
            //uptime visuals to show if lvl found or not
            ColorRow(_lvl, MasterLvls.IndexOf(_lvl));
            if (updatebeats)
                UpdateBeatPosition();
            //index for previous lvl checkpoint status
            int index = MasterLvls.IndexOf(_lvl);
            return _lvl.Beats + _lvl.restlevelbeats + (index > 0 && MasterLvls[index - 1].checkpoint ? MasterProperties.checkpointbeats : 0);
        }

        public void UpdateBeatPosition()
        {
            int beatpos = MasterProperties.introlevelbeats;
            foreach (MasterLvlData _lvl in MasterLvls) {
                //if previous lvl checkpoint status, this lvl will start later
                int index = MasterLvls.IndexOf(_lvl);
                if (index > 0 && MasterLvls[MasterLvls.IndexOf(_lvl) - 1].checkpoint) {
                    beatpos += MasterProperties.checkpointbeats;
                }
                //beat position for rest
                _lvl.restlevelbeatstart = beatpos;
                beatpos += _lvl.restlevelbeats;
                //beat position for lvl
                _lvl.beatstart = beatpos;
                beatpos += _lvl.Beats;
            }
        }

        public void ColorRow(MasterLvlData _lvl, int index)
        {
            if (_lvl.Beats is -1) {
                masterLvlList.Rows[index].DefaultCellStyle.BackColor = Color.Maroon;
                masterLvlList.Rows[index].Cells[3].Value = $"file not found";
            }
            else {
                masterLvlList.Rows[index].DefaultCellStyle = null;
                masterLvlList.Rows[index].Cells[3].Value = $"{_lvl.Beats} beats -- {_lvl.runtime}";
            }
            masterLvlList.InvalidateRow(MasterLvls.IndexOf(_lvl));
        }

        public static JObject BuildSave(MasterProperties _properties)
        {
            int checkpoints = 0;
            bool isolate_tracks = false;
            ///being build Master JSON object
            JObject _save = new() {
                { "obj_type", "SequinMaster" },
                { "obj_name", "sequin.master" },
                { "skybox_name", _properties.skybox.Replace("<none>", "") },
                { "intro_lvl_name", _properties.introlvl.Replace("<none>", "") }
            };
            JArray groupings = new();
            foreach (MasterLvlData group in _properties.masterlvls) {
                JObject s = new() {
                    { "lvl_name", (group.Type == "lvl" ? group.name : "") },
                    { "gate_name", (group.Type == "gate" ? group.name : "") },
                    { "checkpoint", group.checkpoint },
                    { "checkpoint_leader_lvl_name", group.checkpoint_leader.Replace("<none>", "") ?? "" },
                    { "rest_lvl_name", group.rest.Replace("<none>", "") ?? "" },
                    { "play_plus", group.playplus },
                    { "isolate", group.isolate }
                };
                if (group.isolate == true)
                    isolate_tracks = true;
                //increment checkpoints if this lvl has "checkpoint" true
                if ((string)s["checkpoint"] == "True")
                    checkpoints++;

                groupings.Add(s);
            }
            _save.Add("groupings", groupings);
            _save.Add("isolate_tracks", isolate_tracks);
            _save.Add("checkpoint_lvl_name", _properties.checkpointlvl.Replace("<none>", ""));
            ///end build
            ///only need to return _save, since _config is written already
            return _save;
        }

        public void Cut()
        {
            Copy();

            MasterLvls.CollectionChanged -= masterlvls_CollectionChanged;
            foreach (MasterLvlData mld in TCLE.ClipboardMaster) {
                MasterLvls.Remove(mld);
            }
            MasterLvls.CollectionChanged += masterlvls_CollectionChanged;
            masterlvls_CollectionChanged(null, null);
            SaveCheckAndWrite(false, "Cut Sublevels");
        }

        public void Copy()
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardMaster = MasterLvls.Where(x => selectedrows.Contains(MasterLvls.IndexOf(x))).ToList();
            TCLE.ClipboardMaster.Reverse();
            TCLE.PlaySound("UIkcopy");
            //enable the paste button everywhere
            foreach (Form_MasterEditor master in TCLE.Documents.Values.Where(x => x.WorkingFile.Extension == ".master"))
                master.btnMasterLvlPaste.Enabled = true;
        }

        public void Paste()
        {
            int _in = masterLvlList.CurrentRow?.Index + 1 ?? 0;

            //MasterLvls.CollectionChanged -= masterlvls_CollectionChanged;
            foreach (MasterLvlData mld in TCLE.ClipboardMaster)
                MasterLvls.Insert(_in, mld.Clone());
            //MasterLvls.CollectionChanged += masterlvls_CollectionChanged;
            //masterlvls_CollectionChanged(null, null);

            SaveCheckAndWrite(false, "Paste Lvl");
            TCLE.PlaySound("UIkpaste");
        }
        #endregion


        private int PlaybackStart = -1;
        private int PlaybackEnd = -1;
        private bool PlaybackLoop;
        private bool ForceStop;
        private void btnMasterPlayback_Click(object sender, EventArgs e)
        {
            if (Playback.IsPlaying) {
                Playback.IsPlaying = false;
                ForceStop = true;
            }
            else {
                //timer interval twice as small as the bpm (*500ms, instead of *1000ms), so it can keep up with the Playback threading timer
                //timer1.Interval = (int)((60 / TCLE.BPM) * (1000 / Playback.BeatSubdivisions));
                timer1.Interval = 10;
                btnMasterPlayback.Image = Properties.Resources.icon_stop;
                Playback.Initialize("master");
                Playback.CreatePlaybackFromMaster(MasterProperties);
                Playback.Play(masterLvlList.SelectedRows.Count > 0 ? MasterLvls[masterLvlList.SelectedRows[^1].Index].beatstart : -1, MasterProperties.Beats, PlaybackLoop);
                if (Playback.IsPlaying) {
                    timer1.Enabled = true;
                }
                else {
                    Bass.BASS_ChannelFree(Playback.MidiStream);
                    TCLE.alzheimer();
                    btnMasterPlayback.Image = Properties.Resources.icon_play2;
                }
            }
        }

        private string _playingleaf;
        private Form_LeafEditor _playingleafform;
        private string _playinglvl;
        private Form_LvlEditor _playinglvlform;
        private string _playinggate;
        private Form_GateEditor _playinggateform;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Playback.PlaybackBeat < 0)
                return;
            if (Playback.IsPlaying && !ForceStop) {
                masterLvlList.Invalidate();
                //show the leaf that's playing
                if (_playingleaf != Playback.GlobalCurrentLeaf) {
                    _playingleaf = Playback.GlobalCurrentLeaf;
                    _playingleafform?.trackEditor.ResetPlayback();
                    _playingleafform = TCLE.Documents.Values.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(_playingleaf)) as Form_LeafEditor;
                    //switch to the leaf if it's open
                    _playingleafform?.DockHandler?.Activate();
                }
                if (_playingleafform is not null) {
                    _playingleafform.trackEditor.PlaybackPosition = (double)(Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat);
                    _playingleafform.trackEditor.Invalidate();
                    if (Properties.Settings.Default.LeafOptionPlaybackScroll)
                        _playingleafform.trackEditor.HorizontalScrollingOffset = (int)((Playback.PlaybackBeat - Playback.GlobalCurrentOffset + Playback.PlaybackSubBeat) * _playingleafform.trackZoom.Value);
                }
                //show the lvl that's playing
                if (_playinglvl != Playback.GlobalCurrentLvl) {
                    _playinglvl = Playback.GlobalCurrentLvl;
                    _playinglvlform = TCLE.Documents.Values.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(_playinglvl)) as Form_LvlEditor;
                    //switch to the lvl if it's open
                    _playinglvlform?.DockHandler?.Activate();
                }
                _playinglvlform?.lvlLeafList.Invalidate();
                //show the lvl that's playing
                if (_playinggate != Playback.GlobalCurrentGate) {
                    _playinggate = Playback.GlobalCurrentGate;
                    _playinggateform = TCLE.Documents.Values.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(_playinggate)) as Form_GateEditor;
                    //switch to the gate if it's open
                    _playinggateform?.DockHandler?.Activate();
                }
                _playinggateform?.gateLvlList.Invalidate();
            }
            else {
                ForceStop = false;
                timer1.Enabled = false;
                btnMasterPlayback.Image = Properties.Resources.icon_play2;
                Playback.StopPlayback();
                masterLvlList.Invalidate();
                _playingleafform?.trackEditor.Invalidate();
                _playinglvlform?.lvlLeafList.Invalidate();
                _playinggateform?.gateLvlList.Invalidate();
            }
        }
    }
}
