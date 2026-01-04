using ABI.Windows.ApplicationModel.Activation;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Un4seen.Bass;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_GateEditor : DockContentEx
    {
        #region Form Construction
        public Form_GateEditor(dynamic load = null, FileInfo filepath = null)
        {
            if (Playback.Generating) {
                LoadGateSimple(load, filepath);
                return;
            }

            InitializeComponent();
            InitializeGateStuff();
            gateToolStrip.Renderer = new ToolStripOverride();
            TCLE.DoubleBufferDGV(gateLvlList);

            if (load != null) {
                LoadGate(load, filepath);
                UndoList.Add(new SaveState() {
                    reason = "",
                    savestate = load
                });
            }
        }
        private void Form_GateEditor_Shown(object sender, EventArgs e)
        {
            propertyGridGate.Focus();
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
            this.BackColor = Properties.Settings.Default.ColorGateBG;
            gateLvlList.BackgroundColor = Properties.Settings.Default.ColorGateLvlBG;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading;
        private bool LogUndo = true;
        private bool IsAddingItems;
        public bool IsAllowedToAddLvl => !((GateProperties.gatelvls.Count >= 4 && GateProperties.boss != "Level 9 - pyramid" && !GateProperties.random) || (GateProperties.gatelvls.Count >= 5 && GateProperties.boss == "Level 9 - pyramid") || (GateProperties.gatelvls.Count >= 16 && GateProperties.random));
        public FileInfo loadedgate
        {
            get => LoadedGate;
            set {
                if (LoadedGate != value) {
                    TCLE.CloseFileLock(LoadedGate);
                    LoadedGate = value;
                    if (!LoadedGate.Exists) {
                        using (StreamWriter sw = LoadedGate.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(LoadedGate);
                }
            }
        }
        private FileInfo LoadedGate;
        private List<DataGridViewRow> SelectedRows = new();
        private static readonly string[] node_name_hash = new string[] { "0c3025e2", "27e9f06d", "3c5c8436", "3428c8e3" };
        public static readonly List<BossData> bossdata = new() {
            new BossData() {boss_name = "Level 1 - circle", boss_spn = "boss_gate.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 1 - crakhed", boss_spn = "crakhed1.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 2 - circle", boss_spn = "boss_jump.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 2 - crakhed", boss_spn = "crakhed2.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 3 - array", boss_spn = "boss_array.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 3 - crakhed", boss_spn = "crakhed3.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 4 - triangle", boss_spn = "boss_triangle.spn", boss_ent = "tutorial_thumps.ent"},
            new BossData() {boss_name = "Level 4 - zillapede", boss_spn = "zillapede.spn", boss_ent = "zillapede_gate.ent"},
            new BossData() {boss_name = "Level 4 - crakhed", boss_spn = "crakhed4.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 5 - spiral", boss_spn = "boss_spiral.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 5 - crakhed", boss_spn = "crakhed5.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 6 - spirograph", boss_spn = "boss_spirograph.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 6 - crakhed", boss_spn = "crakhed6.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 7 - tube", boss_spn = "boss_tube.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 7 - crakhed", boss_spn = "crakhed7.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 8 - starfish", boss_spn = "boss_starfish.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 8 - crakhed", boss_spn = "crakhed8.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 9 - fractal", boss_spn = "boss_fractal.spn", boss_ent = "boss_gate_pellet.ent"},
            new BossData() {boss_name = "Level 9 - crakhed",  boss_spn = "crakhed9.spn", boss_ent = "crakhed.ent"},
            new BossData() {boss_name = "Level 9 - pyramid",  boss_spn = "pyramid.spn", boss_ent = "crakhed.ent"}
        };
        private static readonly List<string> _bucket0 = new() { "33caad90", "418d18a1", "1e84f4f0", "2e1b70cf" };
        private static readonly List<string> _bucket1 = new() { "41561eda", "347eebcb", "f8192c30", "0c9ddd9e" };
        private static readonly List<string> _bucket2 = new() { "fe617306", "3ee2811c", "d4f56308", "092f1784" };
        private static readonly List<string> _bucket3 = new() { "e790cc5a", "df4d10ff", "e7bc30f7", "1f30e67f" };
        private static readonly Dictionary<string, string> gatesentrynames = new() { { "None", "SENTRY_NONE" }, { "Single Lane", "SENTRY_SINGLE_LANE" }, { "Multi Lane", "SENTRY_MULTI_LANE" } };
        private static readonly Dictionary<string, string> gatesectiontypes = new() {
            { "SECTION_LINEAR", "None" },
            { "SECTION_BOSS_TRIANGLE", "Boss" },
            { "SECTION_BOSS_CIRCLE", "Boss" },
            { "SECTION_BOSS_MINI", "Boss" },
            { "SECTION_BOSS_CRAKHED", "Final Boss" },
            { "SECTION_BOSS_CRAKHED_FINAL", "Final Boss" },
            { "SECTION_BOSS_PYRAMID", "Infinity" }
        };
        private dynamic gatejson;
        public GateProperties gateproperties
        {
            get { return GateProperties; }
            set {
                GateProperties = value;
                SaveCheckAndWrite(false, "Unsure what this one tracks");
            }
        }
        private GateProperties GateProperties;
        public ObservableCollection<GateLvlData> GateLvls { get { return GateProperties.gatelvls; } set { GateProperties.gatelvls = value; } }
        private DeserializeDockContent m_deserializeDockContent;
        public DockContentEx contentPropertyGrid = new() {
            TabText = "Properties",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentMain = new() {
            TabText = "Lvl Phases",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        #endregion

        #region EventHandlers
        ///        ///
        /// EVENTS ///
        ///        ///
        private void gateLvlList_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > GateLvls.Count - 1)
                return;
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;

            propertyGridGate.SelectedObjects = gateLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => GateLvls[x.Index]).ToArray();
            propertyGridGate.ExpandAllGridItems();
            propertyGridGate.Refresh();
        }

        private void gateLvlList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > GateLvls.Count - 1)
                return;
            TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{GateLvls[e.RowIndex].lvlname}")));
        }

        bool MouseDown;
        int LastRow = -1;
        private void gateLvlList_SelectionChanged(object sender, EventArgs e)
        {
            if (MouseDown) {
                gateLvlList.SelectionChanged -= gateLvlList_SelectionChanged;
                gateLvlList.ClearSelection();
                foreach (DataGridViewRow dgvr in SelectedRows) {
                    if (dgvr.Index is not -1)
                        gateLvlList.Rows[dgvr.Index].Selected = true;
                }
                gateLvlList.SelectionChanged += gateLvlList_SelectionChanged;
            }

            propertyGridGate.SelectedObjects = gateLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => GateLvls[x.Index]).ToArray();
            propertyGridGate.ExpandAllGridItems();
            propertyGridGate.Refresh();
        }

        private void gateLvlList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || e.RowIndex == LastRow)
                return;
            if (gateLvlList.Rows[e.RowIndex].Selected) {
                SelectedRows = gateLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                MouseDown = true;
            }
            else {
                MouseDown = false;
                gateLvlList.ClearSelection();
            }
        }

        private void gateLvlList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control) || !MouseDown)
                return;
            SelectedRows = new() { gateLvlList.Rows[e.RowIndex] };
            gateLvlList.ClearSelection();
            MouseDown = false;
        }

        private Rectangle dragBoxFromMouseDown;
        private List<GateLvlData> LvlsToMove;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int previousDragOver = -2;
        private int TargetRowToPaint = -3;
        private void gateLvlList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (LvlsToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.
                    List<DataGridViewRow> SelectedRows = gateLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                    SelectedRows.Sort((row1, row2) => row2.Index.CompareTo(row1.Index));
                    LvlsToMove = SelectedRows.Select(x => GateLvls[x.Index]).ToList();
                    //
                    TCLE.DragSource = "LvlList";
                    IsAddingItems = true;
                    LogUndo = false;
                    //
                    DragDropEffects dropEffect = gateLvlList.DoDragDrop(LvlsToMove, DragDropEffects.Move);
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

        private void gateLvlList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            rowIndexFromMouseDown = gateLvlList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown is -1) {
                dragBoxFromMouseDown = Rectangle.Empty;
                return;
            }
            if (gateLvlList.Rows[rowIndexFromMouseDown].Selected)
                SelectedRows = gateLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
            else
                SelectedRows.Clear();

            Size dragSize = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
        }

        private void gateLvlList_DragOver(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlList" and not "GateList" and not "FileExplorer")
                return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = gateLvlList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = gateLvlList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (LvlsToMove == null) {
                if (targetRow != previousDragOver) {
                    previousDragOver = targetRow;
                    TargetRowToPaint = targetRow;
                    if (TargetRowToPaint is -1)
                        TargetRowToPaint = gateLvlList.RowCount;
                    gateLvlList.Invalidate();
                }
            }
            else {
                if (targetRow != -1 && targetRow != previousDragOver) {
                    if (targetRow + LvlsToMove.Count > GateLvls.Count)
                        return;
                    gateLvlList.SelectionChanged -= gateLvlList_SelectionChanged;
                    foreach (GateLvlData leaf in LvlsToMove) {
                        GateLvls.Remove(leaf);
                    }
                    gateLvlList.ClearSelection();
                    for (int x = 0; x < LvlsToMove.Count; x++) {
                        try {
                            GateLvls.Insert(targetRow, LvlsToMove[x]);
                            if (x == 0)
                                gateLvlList.CurrentCell = gateLvlList[0, targetRow];
                            gateLvlList.Rows[targetRow].Selected = true;
                        } catch (Exception) {
                            GateLvls.Add(LvlsToMove[x]);
                            if (x == 0)
                                gateLvlList.CurrentCell = gateLvlList[0, gateLvlList.RowCount - 1];
                            gateLvlList.Rows[gateLvlList.RowCount - 1].Selected = true;
                        }
                    }
                    gateLvlList.SelectionChanged += gateLvlList_SelectionChanged;
                    previousDragOver = targetRow;
                }
            }
        }

        private void gateLvlList_DragEnter(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlList" and not "GateList" and not "FileExplorer")
                return;
            if (LvlsToMove != null)
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetData(typeof(TreeNode)) is TreeNode)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }
        private void gateLvlList_DragDrop(object sender, DragEventArgs e)
        {
            if (TCLE.DragSource is not "LvlList" and not "GateList" and not "FileExplorer")
                return;
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = gateLvlList.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = gateLvlList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            IsAddingItems = true;

            if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode) {
                AddFileToGate($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}", TargetRowToPaint);
            }
            else if (LvlsToMove != null) {
                LogUndo = true;
                SaveCheckAndWrite(false, "Reorder Phases");
                LvlsToMove = null;
            }
            else if (e.Data.GetData(typeof(List<GateLvlData>)) is List<GateLvlData> phases) {
                LogUndo = false;
                foreach (GateLvlData lvl in phases)
                    GateLvls.Insert(TargetRowToPaint, lvl.Clone());
                LogUndo = true;
                SaveCheckAndWrite(false, "Add Phases");
            }
            else if (e.Data.GetData(typeof(List<string>)) is List<string> sublevels2) {
                LogUndo = false;
                foreach (string leaf in sublevels2)
                    AddFileToGate(ProjectExplorer.Files.FirstOrDefault(x => x.Name == leaf)?.FullName, TargetRowToPaint);
                LogUndo = true;
                SaveCheckAndWrite(false, "Add Phases");
            }
            IsAddingItems = false;
            TargetRowToPaint = -3;
            previousDragOver = -2;
            gateLvlList.Invalidate();
        }


        private static SolidBrush ClearColor = new(Color.Black);
        private static SolidBrush BrushWhite = new(Color.White);
        private static Pen PenBlack = new(Color.Black, 1);
        private static Pen PenGreen = new(Color.Green, 4);
        private static Pen PenViolet = new(new SolidBrush(Color.Violet), 3);
        private void gateLvlList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (e.RowIndex == -1)
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            else {
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                //e.Paint(e.CellBounds, DataGridViewPaintParts.)
            }
        }

        private void gateLvlList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
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
            e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);

            if (sender == gateLvlList && TCLE.DragSource is "LvlList" or "FileExplorer") {
                if (e.RowIndex == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Top, e.RowBounds.Right, e.RowBounds.Top);
                if (e.RowIndex + 1 == TargetRowToPaint)
                    e.Graphics.DrawLine(PenGreen, e.RowBounds.Left, e.RowBounds.Bottom, e.RowBounds.Right, e.RowBounds.Bottom);
            }

            if (Playback.IsPlaying) {
                if (GateProperties.FilePath.Name == Playback.GlobalCurrentGate && GateLvls[e.RowIndex].lvlname == Playback.GlobalCurrentLvl) {
                    double pixelsperbeat = (double)e.RowBounds.Width / (double)GateLvls[e.RowIndex].beats;
                    double offset = Playback.PlaybackBeat - Playback.GlobalCurrentOffsetLvl + Playback.PlaybackSubBeat;
                    e.Graphics.DrawLine(PenViolet, (int)(pixelsperbeat * offset), e.RowBounds.Top, (int)(pixelsperbeat * offset), e.RowBounds.Bottom);
                }
            }
        }

        public void gatelvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            /*
            //clear dgv
            gateLvlList.RowCount = 0;
            //repopulate dgv from list

            foreach (GateLvlData _lvl in GateLvls) {
                gateLvlList.Rows.Add(new object[] {
                    GateProperties.random ? _lvl.bucket : GateLvls.IndexOf(_lvl),
                    Properties.Resources.editor_lvl,
                    _lvl.lvlname,
                    0
                });
            }
            propertyGridGate.Refresh();
            RecalculateRuntime();*/

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) {
                gateLvlList.RowCount = 0;
            }
            //if action ADD, add new row to the master DGV
            //NewStartingIndex and OldStartingIndex track where the changes were made
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                int _in = e.NewStartingIndex;
                //get the runtime of the object
                gateLvlList.Rows.Insert(_in, new object[] {
                    GateProperties.random ? GateLvls[_in].bucket : GateLvls.IndexOf(GateLvls[_in]),
                    Properties.Resources.editor_lvl,
                    GateLvls[_in].lvlname,
                    0
                });
            }
            //if action REMOVE, remove row from the master DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                gateLvlList.Rows.RemoveAt(e.OldStartingIndex);
            }
            RecalculateRuntime();

            //set selected index. Mainly used when moving items
            //enable certain buttons if there are enough items for them
            btnGateLvlDelete.Enabled = GateProperties.gatelvls.Count > 0;
            btnGateLvlUp.Enabled = GateProperties.gatelvls.Count > 1;
            btnGateLvlDown.Enabled = GateProperties.gatelvls.Count > 1;

            //limit how many phases can be added
            btnGateLvlAdd.Enabled = IsAllowedToAddLvl;
        }

        private void propertyGridGate_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveCheckAndWrite(false, "Change Gate Property");
        }

        private void gatenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                //gatesaveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void gateopenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Gate File (*.gate)|*.gate";
                ofd.Title = "Load a Thumper Gate file";
                ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    FileInfo filepath = new(TCLE.CopyToWorkingFolderCheck(ofd.FileName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadGate(_load, filepath);
                }
            }
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        private void btnGateLvlDelete_Click(object sender, EventArgs e)
        {
            List<GateLvlData> todelete = new();
            foreach (DataGridViewRow dgvr in gateLvlList.SelectedRows) {
                todelete.Add(GateProperties.gatelvls[dgvr.Index]);
            }
            LogUndo = false;
            foreach (GateLvlData gld in todelete)
                GateProperties.gatelvls.Remove(gld);
            LogUndo = true;
            SaveCheckAndWrite(false, "Remove Phase");
            TCLE.PlaySound("UIobjectremove");
        }

        private void btnGateLvlAdd_Click(object sender, EventArgs e)
        {
            //show file dialog
            /*using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Gate File (*.lvl)|*.lvl";
            ofd.Title = "Load a Thumper Lvl file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddFileToGate(ofd.FileName);
            }*/
            if (TCLE.DragDropItems.Items is not "lvl" || !TCLE.DragDropItems.Visible) {
                TCLE.DragDropItems.Items = "lvl";
                TCLE.DragDropItems.Show();
                TCLE.DragDropItems.Location = new Point(System.Windows.Forms.Cursor.Position.X + 2, System.Windows.Forms.Cursor.Position.Y + 2);
                if (TCLE.DragDropItems.Location.X + TCLE.DragDropItems.Width > this.Width)
                    TCLE.DragDropItems.Location = new Point(this.Width - TCLE.DragDropItems.Width - 2, TCLE.DragDropItems.Location.Y);
            }
            else
                TCLE.DragDropItems.Hide();
        }

        private void btnGateLvlUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = gateLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                GateProperties.gatelvls.Insert(dgvr - 1, GateProperties.gatelvls[dgvr]);
                GateProperties.gatelvls.RemoveAt(dgvr + 1);
            }
            gateLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                gateLvlList.Rows[dgvr - 1].Cells[1].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Lvl Up");
        }

        private void btnGateLvlDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = gateLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == gateLvlList.Rows.Count - 1))
                return;
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                GateProperties.gatelvls.Insert(dgvr + 2, GateProperties.gatelvls[dgvr]);
                GateProperties.gatelvls.RemoveAt(dgvr);
            }
            gateLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                gateLvlList.Rows[dgvr + 1].Cells[1].Selected = true;
            }
            SaveCheckAndWrite(false, "Move Lvl Down");
        }

        private void btnGateCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void btnGatePaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void btnRevertGate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true, "");
            LoadGate(gatejson, LoadedGate);
            TCLE.PlaySound("UIrevertchanges");
        }
        #endregion

        #region Methods
        ///         ///
        /// Methods ///
        ///         ///

        public void InitializeGateStuff()
        {
            dockPanel1.Theme = TCLE.DockTheme;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //
            contentMain.Controls.Add(panelMain);
            panelMain.Dock = DockStyle.Fill;
            //
            contentPropertyGrid.Controls.Add(propertyGridGate);
            propertyGridGate.Dock = DockStyle.Fill;
            //
            try {
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_gate.config", m_deserializeDockContent);
            } catch {
                contentMain.Show(dockPanel1, DockState.Document);
                contentPropertyGrid.Show(dockPanel1, DockState.DockRight);
            }
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_gate.config");
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString is "Properties")
                return contentPropertyGrid;
            if (persistString is "Lvl Phases")
                return contentMain;

            throw new NotImplementedException();
        }

        public object GetProperties()
        {
            return GateProperties;
        }

        public void LoadGate(dynamic _load, FileInfo filepath)
        {
            if (_load == null)
                return;
            //detect if file is actually Gate or not
            if ((string)_load["obj_type"] != "SequinGate") {
                MessageBox.Show("This does not appear to be a gate file!");
                return;
            }
            loadedgate = filepath;
            //set some visual elements
            this.Text = LoadedGate.Name;
            //set flag that load is in progress. This skips Save method
            EditorLoading = true;

            gateproperties = new(this, filepath) {
                boss = bossdata.FirstOrDefault(x => x.boss_spn == (string)_load["spn_name"])?.boss_name ?? bossdata[0].boss_name,
                prelvl = string.IsNullOrEmpty((string)_load["pre_lvl_name"]) ? "<none>" : (string)_load["pre_lvl_name"],
                postlvl = string.IsNullOrEmpty((string)_load["post_lvl_name"]) ? "<none>" : (string)_load["post_lvl_name"],
                restartlvl = string.IsNullOrEmpty((string)_load["restart_lvl_name"]) ? "<none>" : (string)_load["restart_lvl_name"],
                sectiontype = gatesectiontypes.FirstOrDefault(x => x.Key == (string)_load["section_type"]).Value ?? "Boss",
                random = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET",
            };

            ///Clear form elements so new data can load
            GateLvls.Clear();
            ///load lvls associated with this master
            foreach (dynamic _lvl in _load["boss_patterns"]) {
                GateProperties.gatelvls.Add(new GateLvlData() {
                    _Lvlname = _lvl["lvl_name"],
                    sentrytype = gatesentrynames.First(x => x.Value == (string)_lvl["sentry_type"]).Key,
                    bucket = (int)_lvl["bucket_num"] is < 0 or > 3 ? 0 : (int)_lvl["bucket_num"]
                });
            }

            EditorLoading = false;
            EditorIsSaved = true;
            RecalculateRuntime();
        }

        public void LoadGateSimple(dynamic _load, FileInfo filepath)
        {
            EditorLoading = true;
            LoadedGate = filepath;

            gateproperties = new(this, filepath) {
                boss = bossdata.First(x => x.boss_spn == (string)_load["spn_name"]).boss_name,
                prelvl = string.IsNullOrEmpty((string)_load["pre_lvl_name"]) ? "<none>" : (string)_load["pre_lvl_name"],
                postlvl = string.IsNullOrEmpty((string)_load["post_lvl_name"]) ? "<none>" : (string)_load["post_lvl_name"],
                restartlvl = string.IsNullOrEmpty((string)_load["restart_lvl_name"]) ? "<none>" : (string)_load["restart_lvl_name"],
                sectiontype = gatesectiontypes.First(x => x.Key == (string)_load["section_type"]).Value,
                random = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET",
            };

            GateLvls.CollectionChanged -= gatelvls_CollectionChanged;
            foreach (dynamic _lvl in _load["boss_patterns"]) {
                GateProperties.gatelvls.Add(new GateLvlData() {
                    _Lvlname = _lvl["lvl_name"],
                    sentrytype = gatesentrynames.First(x => x.Value == (string)_lvl["sentry_type"]).Key,
                    bucket = (int)_lvl["bucket_num"] is < 0 or > 3 ? 0 : (int)_lvl["bucket_num"]
                });
            }

            EditorLoading = false;
            EditorIsSaved = true;
            RecalculateRuntime();
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedGate.FullName);
            LoadGate(_load, LoadedGate);
            RecalculateRuntime();
            gateLvlList.Invalidate();
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
            bool _trackNotSaved = EditorIsSaved;
            LoadGate(UndoList[undolistindex].savestate, LoadedGate);
            UndoList.RemoveRange(0, undolistindex);
            propertyGridGate.Refresh();

            if (!_trackNotSaved) {
                EditorIsSaved = false;
                if (!this.Text.EndsWith('*'))
                    this.Text += '*';
            }
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if LoadedGate is somehow not set, force Save As instead
            if (LoadedGate == null)
                SaveAs();
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false, string startpath = null)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = "Thumper Gate File (*.gate)|*.gate";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = startpath ?? TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedgate = new FileInfo(sfd.FileName);

                gateproperties ??= new(this, loadedgate) {
                    boss = "Level 1 - circle",
                    prelvl = "<none>",
                    postlvl = "<none>",
                    restartlvl = "<none>",
                    sectiontype = "None",
                    random = false,
                };

                SaveCheckAndWrite(true, "", true);
                if (isnew)
                    TCLE.CloseFileLock(loadedgate);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return loadedgate;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorLoading || !LogUndo)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(gateproperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedGate.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    reason = Reason,
                    savestate = _saveJSON
                });
            }
            else {
                this.Text = LoadedGate.Name;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedGate], _saveJSON);

                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedGate.Name);
                TCLE.FindEditorRunMethod(typeof(Form_MasterEditor), "RecalculateRuntime");
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public void AddFileToGate(string path, int index = -1)
        {
            //parse leaf to JSON
            dynamic _load = TCLE.LoadFileLock(path);
            //check if file being loaded is actually a leaf. Can do so by checking the JSON key
            if ((string)_load["obj_type"] is not "SequinLevel") {
                MessageBox.Show("That does not appear to be a lvl.\nItem not added to gate.", "Bumper Custom Level Editor");
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
                    else
                        return;
            }
            TCLE.PlaySound("UIobjectadd");
            //add lvl/gate data to the list
            if (index == -1) {
                GateLvls.Add(new GateLvlData() {
                    _Lvlname = (string)_load["obj_name"],
                    sentrytype = "None",
                    bucket = 0
                });
            }
            else {
                GateLvls.Insert(index, new GateLvlData() {
                    _Lvlname = (string)_load["obj_name"],
                    sentrytype = "None",
                    bucket = 0
                });
            }
            if (!IsAddingItems)
                propertyGridGate.Refresh();
            SaveCheckAndWrite(false, "Add New Phase");
        }

        public int RecalculateRuntime()
        {
            if (EditorLoading || GateProperties == null)
                return 0;
            //depending on the gate configuration, it can have a different amount of lvls in it
            GateProperties.MaximumRows = 0;
            if (GateProperties.boss != "Level 9 - pyramid" && !GateProperties.random)
                GateProperties.MaximumRows = 4;
            else if (GateProperties.boss == "Level 9 - pyramid")
                GateProperties.MaximumRows = 5;
            else if (GateProperties.random)
                GateProperties.MaximumRows = 16;

            int beattotal = 0;
            List<int> bucketscounted = new();
            //calc pre lvl beats
            GateProperties.prebeats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == GateProperties.prelvl)?.FullName);
            //calc post lvl beats
            GateProperties.postbeats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == GateProperties.postlvl)?.FullName);
            //loop over each lvl and update the grid with runtime or a warning
            foreach (GateLvlData _lvl in GateLvls) {
                RecalculateRuntimeSublevel(_lvl);
                if (GateProperties.random) {
                    if (!bucketscounted.Contains(_lvl.bucket)) {
                        beattotal += _lvl.beats;
                        bucketscounted.Add(_lvl.bucket);
                    }
                }
                else
                    beattotal += _lvl.beats;
            }

            if (!Playback.Generating)
                gateLvlList.Refresh();
            return beattotal;
        }

        public int RecalculateRuntimeSublevel(GateLvlData _lvl)
        {
            if (EditorLoading)
                return 0;

            FileInfo lvlfile = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{_lvl.lvlname}"));
            lvlfile?.Refresh();

            if (lvlfile == null || !lvlfile.Exists)
                _lvl.beats = -1;
            else
                _lvl.beats = TCLE.CalculateLvlRuntime(ProjectExplorer.Files.FirstOrDefault(x => x.Name == _lvl.lvlname)?.FullName);
            //if playback generating, this was reached during generation, and the form won't exist
            //ColorRow calls form objects which won't be initialized yet.
            if (!Playback.Generating)
                ColorRow(_lvl, GateLvls.IndexOf(_lvl));
            UpdateBeatPosition();

            return _lvl.beats;
        }

        public void UpdateBeatPosition()
        {
            int beatpos = GateProperties.prebeats + GateProperties.postbeats;
            foreach (GateLvlData _lvl in GateLvls) {
                _lvl.beatstart = beatpos;
                beatpos += _lvl.beats;
            }
        }

        public void ColorRow(GateLvlData _lvl, int index)
        {
            //if random, the phase counter will instead show bucket numbers
            gateLvlList.Rows[index].Cells[0].Value = GateProperties.random ? _lvl.bucket + 1 : index + 1;
            if (index >= GateProperties.MaximumRows) {
                gateLvlList.Rows[index].DefaultCellStyle.BackColor = Color.DarkOrange;
                gateLvlList.Rows[index].Cells[3].Value = $"too many lvls in list (max. {GateProperties.MaximumRows})";
            }
            //each bucket can have 4 lvls only. Show warning if more than 4.
            else if (GateProperties.random && GateLvls.Where(x => x.bucket == _lvl.bucket).Count() > 4) {
                gateLvlList.Rows[index].DefaultCellStyle.BackColor = Color.DarkOrange;
                gateLvlList.Rows[index].Cells[3].Value = $"too many lvls in bucket {_lvl.bucket + 1} (max. 4)";
            }
            else {
                if (_lvl.beats == -1) {
                    gateLvlList.Rows[index].DefaultCellStyle.BackColor = Color.Maroon;
                    gateLvlList.Rows[index].Cells[3].Value = $"file not found";
                }
                else {
                    gateLvlList.Rows[index].DefaultCellStyle = null;
                    gateLvlList.Rows[index].Cells[3].Value = $"{_lvl.beats} beats -- {_lvl.runtime}";
                }
            }
        }

        public static JObject BuildSave(GateProperties _properties)
        {
            int bucket0 = 0;
            int bucket1 = 0;
            int bucket2 = 0;
            int bucket3 = 0;
            ///being build Master JSON object
            JObject _save = new() {
                { "obj_type", "SequinGate" },
                { "obj_name", _properties.FilePath.Name },
                { "spn_name", bossdata.First(x => x.boss_name == _properties.boss).boss_spn },
                { "param_path", bossdata.First(x => x.boss_name == _properties.boss).boss_ent },
                { "pre_lvl_name", _properties.prelvl.Replace("<none>", "") },
                { "post_lvl_name", _properties.postlvl.Replace("<none>", "") },
                { "restart_lvl_name", _properties.restartlvl.Replace("<none>", "") },
                { "section_type", gatesectiontypes.First(x => x.Value == _properties.sectiontype).Key},
                { "random_type", $"LEVEL_RANDOM_{(_properties.random ? "BUCKET" : "NONE")}" }
            };
            //setup boss_patterns
            JArray boss_patterns = new();
            for (int x = 0; x < _properties.gatelvls.Count; x++) {
                JObject s = new() {
                    { "lvl_name", _properties.gatelvls[x].lvlname },
                    { "sentry_type", $"{gatesentrynames[_properties.gatelvls[x].sentrytype]}"},
                    { "bucket_num", _properties.gatelvls[x].bucket }
                };
                //if using RANDOM, the buckets and hashes are all different per entry in each bucket
                if (_properties.random) {
                    switch (_properties.gatelvls[x].bucket) {
                        case 0:
                            s.Add("node_name_hash", _bucket0[bucket0]);
                            bucket0 = (bucket0 + 1) % 4;
                            break;
                        case 1:
                            s.Add("node_name_hash", _bucket1[bucket1]);
                            bucket1 = (bucket1 + 1) % 4;
                            break;
                        case 2:
                            s.Add("node_name_hash", _bucket2[bucket2]);
                            bucket2 = (bucket2 + 1) % 4;
                            break;
                        case 3:
                            s.Add("node_name_hash", _bucket3[bucket3]);
                            bucket3 = (bucket3 + 1) % 4;
                            break;
                        default:
                            break;
                    }
                }
                //if not using RANDOM, use the regular hashes
                else if (x < 5) {
                    //hash of phase 4 needs to be different depending if its crakhed or not
                    if (x == 3) {
                        if (_save["spn_name"].ToString().Contains("crakhed") || _save["spn_name"].ToString().Contains("triangle") || _save["spn_name"].ToString().Contains("pyramid"))
                            s.Add("node_name_hash", "6b39151f");
                        else
                            s.Add("node_name_hash", "3428c8e3");
                    }
                    //for pyramid only, requires 5 phases
                    else if (x == 4)
                        s.Add("node_name_hash", "07f819c9");
                    else
                        s.Add("node_name_hash", node_name_hash[x]);
                }

                boss_patterns.Add(s);
            }
            _save.Add("boss_patterns", boss_patterns);
            return _save;
        }

        public void Cut()
        {
            Copy();

            GateLvls.CollectionChanged -= gatelvls_CollectionChanged;
            foreach (GateLvlData mld in TCLE.ClipboardGate) {
                GateLvls.Remove(mld);
            }
            GateLvls.CollectionChanged += gatelvls_CollectionChanged;
            gatelvls_CollectionChanged(null, null);
            SaveCheckAndWrite(false, "Cut Sublevels");
        }

        public void Copy()
        {
            List<int> selectedrows = gateLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardGate = GateLvls.Where(x => selectedrows.Contains(GateLvls.IndexOf(x))).ToList();
            //We reverse the list because they will all paste at the same index. So the last one pasted would be at the top.
            TCLE.ClipboardGate.Reverse();
            //enable the paste button everywhere
            foreach (Form_GateEditor gate in TCLE.Documents.Where(x => x.DockHandler.TabText.Replace("*", "").EndsWith(".gate")))
                gate.btnGatePaste.Enabled = true;
            TCLE.PlaySound("UIkcopy");
        }

        public void Paste()
        {
            int _in = gateLvlList.CurrentRow?.Index + 1 ?? 0;

            GateLvls.CollectionChanged -= gatelvls_CollectionChanged;
            foreach (GateLvlData mld in TCLE.ClipboardGate)
                GateLvls.Insert(_in, mld.Clone());
            GateLvls.CollectionChanged += gatelvls_CollectionChanged;
            gatelvls_CollectionChanged(null, null);

            SaveCheckAndWrite(false, "Paste Lvl");
            TCLE.PlaySound("UIkpaste");
        }
        #endregion

        private void lblGateSectionHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("bosssectionhelp").Show();
        }

        private int PlaybackStart = -1;
        private int PlaybackEnd = -1;
        private bool PlaybackLoop;
        private bool ForceStop;
        private void btnGatePlayback_Click(object sender, EventArgs e)
        {
            if (Playback.IsPlaying) {
                Playback.IsPlaying = false;
                ForceStop = true;
            }
            else {
                //timer interval twice as small as the bpm (*500ms, instead of *1000ms), so it can keep up with the Playback threading timer
                timer1.Interval = (int)((60 / TCLE.BPM) * (1000 / Playback.BeatSubdivisions));
                btnGatePlayback.Image = Properties.Resources.icon_stop;
                Playback.Initialize("gate");
                Playback.CreatePlaybackFromGate(GateProperties);
                Playback.Play(gateLvlList.SelectedRows.Count > 0 ? GateLvls[gateLvlList.SelectedRows[^1].Index].beatstart : -1, GateProperties.beats, PlaybackLoop);
                if (Playback.IsPlaying) {
                    timer1.Enabled = true;
                }
                else {
                    Bass.BASS_ChannelFree(Playback.MidiStream);
                    TCLE.alzheimer();
                    btnGatePlayback.Image = Properties.Resources.icon_play2;
                }
            }
        }

        private string _playingleaf;
        private Form_LeafEditor _playingleafform;
        private string _playinglvl;
        private Form_LvlEditor _playinglvlform;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Playback.PlaybackBeat < 0)
                return;
            if (Playback.IsPlaying && !ForceStop) {
                gateLvlList.Invalidate();
                //show the leaf that's playing
                if (_playingleaf != Playback.GlobalCurrentLeaf) {
                    _playingleaf = Playback.GlobalCurrentLeaf;
                    _playingleafform = TCLE.Documents.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(Playback.GlobalCurrentLeaf)) as Form_LeafEditor;
                    //switch to the leaf if it's open
                    IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText.Replace("*", "") == _playingleaf));
                    if (workspacehastab != null) {
                        workspacehastab.DockHandler.Activate();
                        (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText.Replace("*", "") == _playingleaf).DockHandler.Activate();
                    }
                }
                _playingleafform?.trackEditor.Invalidate();
                //show the lvl that's playing
                if (_playinglvl != Playback.GlobalCurrentLvl) {
                    _playinglvl = Playback.GlobalCurrentLvl;
                    _playinglvlform = TCLE.Documents.FirstOrDefault(x => x.DockHandler.TabText.StartsWith(Playback.GlobalCurrentLvl)) as Form_LvlEditor;
                    //switch to the leaf if it's open
                    IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText.Replace("*", "") == _playinglvl));
                    if (workspacehastab != null) {
                        workspacehastab.DockHandler.Activate();
                        (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText.Replace("*", "") == _playinglvl).DockHandler.Activate();
                    }
                }
                _playinglvlform?.lvlLeafList.Invalidate();
            }
            else {
                ForceStop = false;
                timer1.Enabled = false;
                btnGatePlayback.Image = Properties.Resources.icon_play2;
                Playback.StopPlayback();
                gateLvlList.Invalidate();
                _playingleafform?.trackEditor.Invalidate();
                _playinglvlform?.lvlLeafList.Invalidate();
            }
        }
    }
}
