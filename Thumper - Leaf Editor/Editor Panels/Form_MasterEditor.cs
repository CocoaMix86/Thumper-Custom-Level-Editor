using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Input;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_MasterEditor : DockContent
    {
        #region Form Construction
        public Form_MasterEditor(dynamic load = null, FileInfo filepath = null, bool saveonlynoload = false)
        {
            InitializeComponent();
            InitializeMasterStuff();
            ColorFormElements();
            ///propertyGridMaster.Controls[2].MouseClick += propertyGridMaster_MouseClick;
            SaveOnlyNoLoad = saveonlynoload;
            masterToolStrip.Renderer = new ToolStripOverride();
            TCLE.DoubleBufferDGV(masterLvlList, false);

            if (load != null) {
                LoadMaster(load, filepath);
                UndoList.Add(new SaveState() {
                    reason = "",
                    savestate = load
                });
            }
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
        private bool SaveOnlyNoLoad;
        private bool IsAddingItems;
        private bool LogUndo = true;
        public FileInfo loadedmaster
        {
            get { return LoadedMaster; }
            set {
                if (LoadedMaster != value) {
                    TCLE.CloseFileLock(LoadedMaster);
                    LoadedMaster = value;
                    if (!LoadedMaster.Exists) {
                        using (StreamWriter sw = LoadedMaster.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(LoadedMaster);
                }
            }
        }
        private FileInfo LoadedMaster;
        public ObservableCollection<MasterLvlData> MasterLvls { get { return masterproperties.masterlvls; } set { masterproperties.masterlvls = value; } }
        public MasterProperties masterproperties
        {
            get => MasterProperties;
            set {
                MasterProperties = value;
                SaveCheckAndWrite(false, "UUUUUUuuuuuuuuuhhhhhhhhhhhhHHHHHHH");
            }
        }
        public MasterProperties MasterProperties;
        private List<DataGridViewRow> SelectedRows = new();
        public DockContent contentPropertyGrid = new() {
            TabText = "Properties",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContent contentMain = new() {
            TabText = "Master",
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
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > MasterLvls.Count - 1)
                return;
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

        private void masterLvlList_SelectionChanged(object sender, EventArgs e)
        {
            if (!IsAddingItems) {
                foreach (DataGridViewRow dgvr in SelectedRows) {
                    if (dgvr.Index is not -1)
                        masterLvlList.Rows[dgvr.Index].Selected = true;
                }
            }

            propertyGridMaster.SelectedObjects = masterLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => MasterLvls[x.Index]).ToArray();
            propertyGridMaster.Refresh();
        }

        private void masterLvlList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > MasterLvls.Count - 1)
                return;
            TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{MasterLvls[e.RowIndex].name}")).Value?.File);
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
                    var SelectedRows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
                    SelectedRows.Sort((row1, row2) => row2.Index.CompareTo(row1.Index));
                    LvlsToMove = SelectedRows.Select(x => MasterLvls[x.Index]).ToList();
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
            rowIndexFromMouseDown = masterLvlList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown is -1) {
                dragBoxFromMouseDown = Rectangle.Empty;
                return;
            }
            if (masterLvlList.Rows[rowIndexFromMouseDown].Selected)
                SelectedRows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().ToList();
            else
                SelectedRows.Clear();

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
            else if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode)
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
                    AddFiletoMaster(ProjectExplorer.Files.FirstOrDefault(x => x.Value.IsFile && x.Value.File.Name == leaf).Value.FullPath, TargetRowToPaint);
                LogUndo = true;
                SaveCheckAndWrite(false, "Add Lvls");
            }
            IsAddingItems = false;
            TargetRowToPaint = -3;
            previousDragOver = -2;
            masterLvlList.Invalidate();
        }

        private static SolidBrush ClearColor = new SolidBrush(Color.Black);
        private static SolidBrush BrushWhite = new SolidBrush(Color.White);
        private static Pen PenBlack = new Pen(Color.Black, 1);
        private static Pen PenGreen = new Pen(Color.Green, 4);
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
        }

        public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SaveOnlyNoLoad)
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
                    (MasterLvls[_in].type == "lvl" ? Properties.Resources.editor_lvl : Properties.Resources.editor_gate),
                    MasterLvls[_in].name,
                    0
                });
                RecalculateRuntimeSublevel(MasterLvls[_in]);
                ColorRow(MasterLvls[_in], _in);
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
        /*
        private void propertyGridMaster_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var grid = propertyGridMaster.Controls[2];
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var invalidPoint = new Point(-2147483648, -2147483648);
            var FindPosition = grid.GetType().GetMethod("FindPosition", flags);
            var p = (Point)FindPosition.Invoke(grid, new object[] { e.X, e.Y });
            GridItem entry = null;
            if (p != invalidPoint) {
                var GetGridEntryFromRow = grid.GetType()
                                              .GetMethod("GetGridEntryFromRow", flags);
                entry = (GridItem)GetGridEntryFromRow.Invoke(grid, new object[] { p.Y });
            }
            if (entry != null && entry.Value != null) {
                object parent;
                if (entry.Parent != null && entry.Parent.Value != null)
                    parent = entry.Parent.Value;
                else
                    parent = propertyGridMaster.SelectedObjects.Length > 1 ? propertyGridMaster.SelectedObjects : propertyGridMaster.SelectedObject;
                if (entry.Value != null && entry.Value is bool) {
                    var ee = propertyGridMaster.SelectedObjects;
                    entry.PropertyDescriptor.SetValue(parent, !(bool)entry.Value);
                    propertyGridMaster.Refresh();
                    masterLvlList.Invalidate();
                }
            }
        }*/

        private void masteropenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Master File (*.master)|*.master";
                ofd.Title = "Load a Thumper Master file";
                ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    FileInfo filepath = new(TCLE.CopyToWorkingFolderCheck(ofd.FileName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadMaster(_load, filepath);
                }
            }
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
            /*using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Lvl/Gate File|*.lvl;*.gate";
            ofd.Title = "Load a Thumper Lvl/Gate file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AddFiletoMaster(ofd.FileName);
            }*/
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

        private void AddFiletoMaster(string path, int index = -1)
        {
            //parse leaf to JSON
            dynamic _load = TCLE.LoadFileLock(path);
            //check if file being loaded is actually a leaf. Can do so by checking the JSON key
            if ((string)_load["obj_type"] is not "SequinLevel" and not "SequinGate") {
                MessageBox.Show("That does not appear to be a lvl or a gate file.\nItem not added to master.", "Bumper Custom Level Editor");
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
            if (index is -1) {
                MasterLvls.Add(new MasterLvlData() {
                    type = (_load["obj_type"] == "SequinLevel") ? "lvl" : "gate",
                    name = (string)_load["obj_name"],
                    playplus = true,
                    checkpoint = true,
                    checkpoint_leader = "<none>",
                    rest = "<none>",
                    gatesectiontype = "",
                    id = TCLE.rng.Next(0, 1000000)
                });
            }
            else {
                MasterLvls.Insert(index, new MasterLvlData() {
                    type = (_load["obj_type"] == "SequinLevel") ? "lvl" : "gate",
                    name = (string)_load["obj_name"],
                    playplus = true,
                    checkpoint = true,
                    checkpoint_leader = "<none>",
                    rest = "<none>",
                    gatesectiontype = "",
                    id = TCLE.rng.Next(0, 1000000)
                });
            }
            if (!IsAddingItems)
                propertyGridMaster.Refresh();
            SaveCheckAndWrite(false, "Add New Lvl");
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

        public void InitializeMasterStuff()
        {
            dockPanel1.Theme = new VS2015DarkTheme();
            //
            contentMain.Controls.Add(panelMain);
            panelMain.Dock = DockStyle.Fill;
            contentMain.Show(dockPanel1, DockState.Document);
            //
            contentPropertyGrid.Controls.Add(propertyGridMaster);
            propertyGridMaster.Dock = DockStyle.Fill;
            contentPropertyGrid.Show(dockPanel1, DockState.DockRight);
        }

        public object GetProperties()
        {
            return MasterProperties;
        }

        public void LoadMaster(dynamic _load, FileInfo filepath)
        {
            if (_load == null)
                return;
            if ((string)_load["obj_type"] != "SequinMaster") {
                MessageBox.Show("This does not appear to be a master file!");
                return;
            }
            loadedmaster = filepath;
            //set some visual elements
            this.Text = $"sequin.master";
            EditorLoading = true;

            //setup new master properties
            masterLvlList.Rows.Clear();
            masterproperties = new(this, filepath) {
                skybox = (string)_load["skybox_name"] == "" ? "<none>" : (string)_load["skybox_name"],
                introlvl = (string)_load["intro_lvl_name"] == "" ? "<none>" : (string)_load["intro_lvl_name"],
                checkpointlvl = (string)_load["checkpoint_lvl_name"] == "" ? "<none>" : (string)_load["checkpoint_lvl_name"]
            };

            ///Clear form elements so new data can load
            MasterLvls.Clear();
            ///load lvls associated with this master
            foreach (dynamic _lvl in _load["groupings"]) {
                MasterLvls.Add(new MasterLvlData() {
                    type = !string.IsNullOrEmpty(((string)_lvl["lvl_name"])) ? "lvl" : "gate",
                    name = !string.IsNullOrEmpty(((string)_lvl["lvl_name"])) ? _lvl["lvl_name"] : _lvl["gate_name"],
                    checkpoint = _lvl["checkpoint"],
                    playplus = _lvl["play_plus"],
                    isolate = _lvl["isolate"] ?? false,
                    checkpoint_leader = _lvl["checkpoint_leader_lvl_name"],
                    rest = _lvl["rest_lvl_name"] == "" ? "<none>" : _lvl["rest_lvl_name"],
                    id = TCLE.rng.Next(0, 10000000)
                });
            }
            ///set save flag (master just loaded, has no changes)
            EditorLoading = false;
            EditorIsSaved = true;
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedMaster.FullName);
            LoadMaster(_load, LoadedMaster);
            RecalculateRuntime();
            masterLvlList.Invalidate();
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
            LoadMaster(UndoList[undolistindex].savestate, LoadedMaster);
            UndoList.RemoveRange(0, undolistindex);
            propertyGridMaster.Refresh();
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if LoadedMaster is somehow not set, force Save As instead
            if (LoadedMaster == null)
                SaveAs();
            else
                SaveCheckAndWrite(true, "", playsound);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.master)|*.master";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedmaster = new FileInfo(sfd.FileName);

                masterproperties ??= new(this, loadedmaster) {
                    skybox = "<none>",
                    introlvl = "<none>",
                    checkpointlvl = "<none>"
                };

                SaveCheckAndWrite(true, "", true);
                if (isnew)
                    TCLE.CloseFileLock(loadedmaster);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return loadedmaster;
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
            JObject _saveJSON = BuildSave(MasterProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedMaster.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    reason = Reason,
                    savestate = _saveJSON
                });
            }
            else {
                this.Text = LoadedMaster.Name;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedMaster], _saveJSON);
                //find if any raw text docs are open of this gate and update them
                TCLE.FindReloadRaw(LoadedMaster.Name);
                //update level sections
                TCLE.LevelSections = new();
                foreach (MasterLvlData mld in MasterProperties.masterlvls.Where(x => x.checkpoint)) {
                    TCLE.LevelSections.Add("SECTION_LINEAR");
                }
                if (!SaveOnlyNoLoad) {
                    dynamic _saveTCL = TCLE.BuildSave(TCLE.ProjectProperties);
                    File.WriteAllText($"{TCLE.ProjectProperties.TCL.FullName}", JsonConvert.SerializeObject(_saveTCL, Formatting.Indented));
                }
                //
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public int RecalculateRuntime()
        {
            if (SaveOnlyNoLoad)
                return 0;
            int beattotal = 0;
            foreach (MasterLvlData _lvl in MasterLvls) {
                beattotal += RecalculateRuntimeSublevel(_lvl);
                if (_lvl.rest is not "<none>" and not null)
                    beattotal += TCLE.CalculateLvlRuntime(ProjectExplorer.Files.First(x => x.Value.Name == _lvl.rest).Value.FullPath);
            }
            masterLvlList.Refresh();
            return beattotal;
        }

        public int RecalculateRuntimeSublevel(MasterLvlData _lvl)
        {
            if (SaveOnlyNoLoad)
                return 0;

            int beats = TCLE.CalculateSublevelRuntime(_lvl);
            _lvl.Beats = beats;
            ColorRow(_lvl, MasterLvls.IndexOf(_lvl));

            return beats;
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
                    { "lvl_name", (group.type == "lvl" ? group.name : "") },
                    { "gate_name", (group.type == "gate" ? group.name : "") },
                    { "checkpoint", group.checkpoint.ToString() },
                    { "checkpoint_leader_lvl_name", group.checkpoint_leader.Replace("<none>", "") ?? "" },
                    { "rest_lvl_name", group.rest.Replace("<none>", "") ?? "" },
                    { "play_plus", group.playplus.ToString() },
                    { "isolate", group.isolate.ToString() }
                };
                if (group.isolate == true)
                    isolate_tracks = true;
                //increment checkpoints if this lvl has "checkpoint" true
                if ((string)s["checkpoint"] == "True")
                    checkpoints++;

                groupings.Add(s);
            }
            _save.Add("groupings", groupings);
            _save.Add("isolate_tracks", isolate_tracks.ToString());
            _save.Add("checkpoint_lvl_name", _properties.checkpointlvl.Replace("<none>", ""));
            ///end build
            ///
            /*
            ///begin building Config JSON object
            JObject _config = new() {
                { "obj_type", "LevelLib" },
                { "bpm", BPM }
            };
            //for each lvl in Master that has checkpoint:True, Config requires a "SECTION_LINEAR"
            JArray level_sections = new();
            for (int x = 0; x < checkpoints; x++)
                level_sections.Add("SECTION_LINEAR");
            _config.Add("level_sections", level_sections);
            //
            //add rail color
            JArray rails_color = new() {
                Decimal.Round((decimal)_properties.rail.R / 255, 3),
                Decimal.Round((decimal)_properties.rail.G / 255, 3),
                Decimal.Round((decimal)_properties.rail.B / 255, 3),
                Decimal.Round((decimal)_properties.rail.A / 255, 3)
            };
            _config.Add("rails_color", rails_color);
            //
            //add rail glow color
            JArray rails_glow_color = new() {
                Decimal.Round((decimal)_properties.railglow.R / 255, 3),
                Decimal.Round((decimal)_properties.railglow.G / 255, 3),
                Decimal.Round((decimal)_properties.railglow.B / 255, 3),
                Decimal.Round((decimal)_properties.railglow.A / 255, 3)
            };
            _config.Add("rails_glow_color", rails_glow_color);
            //
            //add path color
            JArray path_color = new() {
                Decimal.Round((decimal)_properties.path.R / 255, 3),
                Decimal.Round((decimal)_properties.path.G / 255, 3),
                Decimal.Round((decimal)_properties.path.B / 255, 3),
                Decimal.Round((decimal)_properties.path.A / 255, 3)
            };
            _config.Add("path_color", path_color);
            //
            //add joy color
            JArray joy_color = new(new object[] { 1, 1, 1, 1 });
            _config.Add("joy_color", joy_color);
            //
            ///end build

            ///Delete extra config_ files in the folder, then write Config to file
            string[] _files = Directory.GetFiles(Path.GetDirectoryName(LoadedMaster), "config_*.txt");
            foreach (string s in _files)
                File.Delete(s);
            File.WriteAllText($@"{TCLE.WorkingFolder}\config_{TCLE.projectjson["level_name"]}.txt", JsonConvert.SerializeObject(_config, Formatting.Indented));
            */
            ///only need to return _save, since _config is written already
            return _save;
        }

        private void ResetMaster()
        {
            //reset things to default values
            MasterLvls.Clear();
            this.Text = "Master Editor";
            masterproperties.skybox = "";
            //set saved flag to true, because nothing is loaded
            SaveCheckAndWrite(true, "");
        }

        public void Cut()
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardMaster = MasterLvls.Where(x => selectedrows.Contains(MasterLvls.IndexOf(x))).ToList();
            TCLE.ClipboardMaster.Reverse();
            TCLE.PlaySound("UIkcopy");
            btnMasterLvlPaste.Enabled = true;

            MasterLvls.CollectionChanged -= masterlvls_CollectionChanged;
            foreach (MasterLvlData mld in TCLE.ClipboardMaster) {
                MasterLvls.Remove(mld);
            }
            MasterLvls.CollectionChanged += masterlvls_CollectionChanged;
            masterlvls_CollectionChanged(null, null);
        }

        public void Copy()
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            TCLE.ClipboardMaster = MasterLvls.Where(x => selectedrows.Contains(MasterLvls.IndexOf(x))).ToList();
            TCLE.ClipboardMaster.Reverse();
            TCLE.PlaySound("UIkcopy");
            btnMasterLvlPaste.Enabled = true;
        }

        public void Paste()
        {
            int _in = masterLvlList.CurrentRow?.Index + 1 ?? 0;

            MasterLvls.CollectionChanged -= masterlvls_CollectionChanged;
            foreach (MasterLvlData mld in TCLE.ClipboardMaster)
                MasterLvls.Insert(_in, mld.Clone());
            MasterLvls.CollectionChanged += masterlvls_CollectionChanged;
            masterlvls_CollectionChanged(null, null);

            SaveCheckAndWrite(false, "Paste Lvl");
            TCLE.PlaySound("UIkpaste");
        }
        #endregion
    }
}
