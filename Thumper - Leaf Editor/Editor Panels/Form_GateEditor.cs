﻿using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_GateEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_GateEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            gateToolStrip.Renderer = new ToolStripOverride();
            TCLE.InitializeTracks(gateLvlList, false);

            if (load != null)
                LoadGate(load, filepath);
            propertyGridGate.SelectedObject = GateProperties;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading = false;
        public FileInfo loadedgate
        {
            get => LoadedGate;
            set {
                LoadedGate = value;
                if (!LoadedGate.Exists) {
                    LoadedGate.CreateText();
                }
                TCLE.lockedfiles.Add(LoadedGate, new FileStream(LoadedGate.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
        }
        private static FileInfo LoadedGate;
        private readonly string[] node_name_hash = new string[] { "0c3025e2", "27e9f06d", "3c5c8436", "3428c8e3" };
        public readonly List<BossData> bossdata = new() {
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
        private readonly List<string> _bucket0 = new() { "33caad90", "418d18a1", "1e84f4f0", "2e1b70cf" };
        private readonly List<string> _bucket1 = new() { "41561eda", "347eebcb", "f8192c30", "0c9ddd9e" };
        private readonly List<string> _bucket2 = new() { "fe617306", "3ee2811c", "d4f56308", "092f1784" };
        private readonly List<string> _bucket3 = new() { "e790cc5a", "df4d10ff", "e7bc30f7", "1f30e67f" };
        private readonly Dictionary<string, string> gatesentrynames = new() { { "None", "SENTRY_NONE" }, { "Single Lane", "SENTRY_SINGLE_LANE" }, { "Multi Lane", "SENTRY_MULTI_LANE" } };
        private readonly Dictionary<string, string> gatesectiontypes = new() {
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
                SaveCheckAndWrite(false);
                GateProperties = value;
            }
        }
        private static GateProperties GateProperties;
        public ObservableCollection<GateLvlData> GateLvls { get { return GateProperties.gatelvls; } set { GateProperties.gatelvls = value; } }
        public decimal BPM { get { return TCLE.dockProjectProperties.BPM; } }
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
            gateproperties.sublevel = GateLvls[e.RowIndex];
            propertyGridGate.ExpandAllGridItems();
            propertyGridGate.Refresh();
        }

        private void gateLvlList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > GateLvls.Count - 1)
                return;
            TCLE.OpenFile(TCLE.Instance, TCLE.dockProjectExplorer.projectfiles.Where(x => x.Key.EndsWith($@"\{gateLvlList.Rows[e.RowIndex].Cells[1].Value}")).FirstOrDefault().Value);
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void gateLvlList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y)) {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = gateLvlList.DoDragDrop(gateLvlList.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void gateLvlList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = gateLvlList.HitTest(e.X, e.Y).RowIndex;
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
        private void gateLvlList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = gateLvlList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = gateLvlList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (previousDragOver != targetRow && previousDragOver != -1) {
                if (gateLvlList.Rows[previousDragOver].Cells[2].Value.ToString() == "file not found")
                    gateLvlList.Rows[previousDragOver].DefaultCellStyle.BackColor = Color.Maroon;
                else
                    gateLvlList.Rows[previousDragOver].DefaultCellStyle = null;
            }
            if (targetRow != -1 && targetRow != previousDragOver) {
                gateLvlList.Rows[targetRow].DefaultCellStyle.BackColor = Color.FromArgb(64, 53, 130);
                previousDragOver = targetRow;
            }
        }

        private void gateLvlList_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void gateLvlList_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = gateLvlList.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = gateLvlList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move) {
                if (e.Data.GetData(typeof(DataGridViewRow)) is not DataGridViewRow rowToMove || rowIndexOfItemUnderMouseToDrop == -1)
                    return;
                GateLvlData tomove = GateLvls[rowToMove.Index];
                GateLvls.RemoveAt(rowIndexFromMouseDown);
                GateLvls.Insert(rowIndexOfItemUnderMouseToDrop, tomove);
            }

            TreeNode dragdropnode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (dragdropnode != null)
                AddFileToGate($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}");
        }

        private void gateLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex <= 0)
                return;
            if (e.ColumnIndex == 2)
                GateProperties.gatelvls[e.RowIndex].sentrytype = gateLvlList[2, e.RowIndex].Value.ToString();
            if (e.ColumnIndex == 3)
                GateProperties.gatelvls[e.RowIndex].bucket = int.Parse((string)gateLvlList[3, e.RowIndex].Value);
            SaveCheckAndWrite(false);
        }

        public void gatelvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //clear dgv
            gateLvlList.RowCount = 0;
            //repopulate dgv from list
            foreach (GateLvlData _lvl in GateProperties.gatelvls) {
                gateLvlList.Rows.Add(new object[] { Properties.Resources.editor_lvl, _lvl.lvlname.Replace(".lvl", ""), _lvl.sentrytype, _lvl.bucket.ToString() });
                //gateLvlList.Rows[GateProperties.gatelvls.IndexOf(_lvl)].HeaderCell.Value = $"Phase {GateProperties.gatelvls.IndexOf(_lvl) + 1}";
            }
            //set selected index. Mainly used when moving items
            //enable certain buttons if there are enough items for them
            btnGateLvlDelete.Enabled = GateProperties.gatelvls.Count > 0;
            btnGateLvlUp.Enabled = GateProperties.gatelvls.Count > 1;
            btnGateLvlDown.Enabled = GateProperties.gatelvls.Count > 1;

            //limit how many phases can be added
            if ((GateProperties.gatelvls.Count >= 4 && GateProperties.boss != "Level 9 - pyramid" && !GateProperties.random) || (GateProperties.gatelvls.Count >= 5 && GateProperties.boss != "Level 9 - pyramid") || (GateProperties.gatelvls.Count >= 16 && GateProperties.random))
                btnGateLvlAdd.Enabled = false;
            else
                btnGateLvlAdd.Enabled = true;

            //set lvl save flag to false
            SaveCheckAndWrite(false);
        }

        private void gatenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                gatesaveAsToolStripMenuItem_Click(null, null);
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
        ///SAVE
        public void Save()
        {
            //if _loadedgate is somehow not set, force Save As instead
            if (loadedgate == null) {
                ///_mainform.toolstripGateSaveAs.PerformClick();
                return;
            }
            else
                WriteGate();
        }
        ///SAVE AS
        private void gatesaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Gate File (*.gate)|*.gate";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName;
            if (sfd.ShowDialog() == DialogResult.OK) {
                if (sender == null)
                    loadedgate = null;
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                string tempFileName = Path.GetFileName(sfd.FileName);
                if (!tempFileName.EndsWith(".txt"))
                    tempFileName += ".txt";
                //check if user input "gate_", and deny save if so
                if (Path.GetFileName(sfd.FileName).Contains("gate_")) {
                    MessageBox.Show("File not saved. Do not include 'gate_' in your file name.", "File not saved");
                    return;
                }
                if (File.Exists($@"{storePath}\gate_{tempFileName}")) {
                    MessageBox.Show("That file name exists already.", "File not saved");
                    return;
                }
                loadedgate = new FileInfo($@"{storePath}\{tempFileName}.gate");
                WriteGate();
                //after saving new file, refresh the workingfolder
                ///_mainform.btnWorkRefresh.PerformClick();
            }
        }
        private void WriteGate()
        {
            //write contents direct to file without prompting save dialog
            JObject _save = GateBuildSave(LoadedGate.Name);
            if (!TCLE.lockedfiles.ContainsKey(loadedgate)) {
                TCLE.lockedfiles.Add(loadedgate, new FileStream(LoadedGate.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
            TCLE.WriteFileLock(TCLE.lockedfiles[LoadedGate], _save);
            SaveCheckAndWrite(true, true);
            this.Text = LoadedGate.Name;
        }
        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        private void btnGateLvlDelete_Click(object sender, EventArgs e)
        {
            List<GateLvlData> todelete = new();
            foreach (DataGridViewRow dgvr in gateLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList()) {
                todelete.Add(GateProperties.gatelvls[dgvr.Index]);
            }
            int _in = gateLvlList.CurrentRow.Index;
            foreach (GateLvlData gld in todelete)
                GateProperties.gatelvls.Remove(gld);
            TCLE.PlaySound("UIobjectremove");
        }

        private void btnGateLvlAdd_Click(object sender, EventArgs e)
        {
            //show file dialog
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Gate File (*.lvl)|*.lvl";
            ofd.Title = "Load a Thumper Lvl file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                //parse leaf to JSON
                dynamic _load = TCLE.LoadFileLock(ofd.FileName);
                //check if file being loaded is actually a leaf. Can do so by checking the JSON key
                if ((string)_load["obj_type"] != "SequinLevel") {
                    MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
                    return;
                }
                //add leaf data to the list
                GateProperties.gatelvls.Add(new GateLvlData() {
                    lvlname = (string)_load["obj_name"],
                    sentrytype = "None",
                    bucket = 0
                });
                TCLE.PlaySound("UIobjectadd");
            }
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
            SaveCheckAndWrite(false);
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
            SaveCheckAndWrite(false);
        }

        private void btnRevertGate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true);
            LoadGate(gatejson, LoadedGate);
            TCLE.PlaySound("UIrevertchanges");
        }

        //buttons that click other buttons
        private void btnGatePanelNew_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripGateNew.PerformClick();
        }
        #endregion

        #region Methods
        ///         ///
        /// Methods ///
        ///         ///

        public void InitializeGateStuff()
        {
            GateProperties.gatelvls.CollectionChanged += gatelvls_CollectionChanged;
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
            EditorLoading = true;

            gateproperties = new(this, filepath) {
                boss = bossdata.First(x => x.boss_spn == (string)_load["spn_name"]).boss_name,
                prelvl = string.IsNullOrEmpty((string)_load["pre_lvl_name"]) ? "<none>" : (string)_load["pre_lvl_name"],
                postlvl = string.IsNullOrEmpty((string)_load["post_lvl_name"]) ? "<none>" : (string)_load["post_lvl_name"],
                restartlvl = string.IsNullOrEmpty((string)_load["restart_lvl_name"]) ? "<none>" : (string)_load["restart_lvl_name"],
                sectiontype = gatesectiontypes.First(x => x.Key == (string)_load["section_type"]).Value,
                random = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET",
            };

            ///Clear form elements so new data can load
            GateProperties.gatelvls.Clear();
            ///load lvls associated with this master
            foreach (dynamic _lvl in _load["boss_patterns"]) {
                GateProperties.gatelvls.Add(new GateLvlData() {
                    lvlname = _lvl["lvl_name"],
                    sentrytype = gatesentrynames.First(x => x.Value == (string)_lvl["sentry_type"]).Key,
                    bucket = _lvl["bucket_num"]
                });
                if ((string)_load["spn_name"] == "pyramid.spn" && GateProperties.gatelvls.Count == 5)
                    break;
                else if (gateproperties.random) {
                    if (gateproperties.gatelvls.Count == 16)
                        break;
                }
                else if (gateproperties.boss == "Level 9 - pyramid" && GateProperties.gatelvls.Count == 4)
                    break;
            }

            ///set save flag (gate just loaded, has no changes)
            gatejson = _load;
            SaveCheckAndWrite(true);
        }

        public void SaveCheckAndWrite(bool save, bool playsound = false)
        {
            //make the beeble emote
            TCLE.beeble.MakeFace();

            EditorIsSaved = save;
            if (!save) {
                /*
                btnSaveGate.Enabled = true;
                btnRevertGate.Enabled = gatejson != null;
                btnRevertGate.ToolTipText = gatejson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
                toolstripTitleGate.BackColor = Color.Maroon;
                */
            }
            else {
                /*
                btnSaveGate.Enabled = false;
                btnRevertGate.Enabled = false;
                toolstripTitleGate.BackColor = Color.FromArgb(40, 40, 40);
                */
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        private void AddFileToGate(string path)
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
                        TCLE.dockProjectExplorer.CreateTreeView();
                    }
                    else
                        return;
            }
            TCLE.PlaySound("UIobjectadd");
            //add lvl/gate data to the list
            GateLvls.Add(new GateLvlData() {
                lvlname = (string)_load["lvl_name"],
                sentrytype = gatesentrynames.First(x => x.Value == (string)_load["sentry_type"]).Key,
                bucket = _load["bucket_num"]
            });
            propertyGridGate.Refresh();
        }

        public void RecalculateRuntime()
        {
            foreach (GateLvlData _lvl in GateProperties.gatelvls) {
                KeyValuePair<string, FileInfo> lvlfile = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith(_lvl.lvlname));
                int beats = lvlfile.Key == null ? -1 : TCLE.CalculateLvlRuntime(lvlfile.Value.FullName);
                if (beats == -1) {
                    gateLvlList.Rows[GateProperties.gatelvls.IndexOf(_lvl)].DefaultCellStyle.BackColor = Color.Maroon;
                    gateLvlList.Rows[GateProperties.gatelvls.IndexOf(_lvl)].Cells[2].Value = $"file not found";
                }
                else {
                    string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    gateLvlList.Rows[GateProperties.gatelvls.IndexOf(_lvl)].DefaultCellStyle = null;
                    gateLvlList.Rows[GateProperties.gatelvls.IndexOf(_lvl)].Cells[2].Value = $"{beats} beats -- {time}";
                }
            }
            gateLvlList.Refresh();
        }

        public JObject GateBuildSave(string _gatename)
        {
            int bucket0 = 0;
            int bucket1 = 0;
            int bucket2 = 0;
            int bucket3 = 0;
            ///being build Master JSON object
            JObject _save = new() {
                { "obj_type", "SequinGate" },
                { "obj_name", _gatename },
                { "spn_name", bossdata.First(x => x.boss_name == GateProperties.boss).boss_spn },
                { "param_path", bossdata.First(x => x.boss_name == GateProperties.boss).boss_ent },
                { "pre_lvl_name", GateProperties.prelvl.Replace("<none>", "") },
                { "post_lvl_name", GateProperties.postlvl.Replace("<none>", "") },
                { "restart_lvl_name", GateProperties.restartlvl.Replace("<none>", "") },
                { "section_type", gatesectiontypes[GateProperties.sectiontype]},
                { "random_type", $"LEVEL_RANDOM_{(GateProperties.random ? "BUCKET" : "NONE")}" }
            };
            //setup boss_patterns
            JArray boss_patterns = new();
            //int lvlcount = checkGateRandom.Checked ? GateProperties.gatelvls.Count : _save["spn_name"].ToString().Contains("pyramid") ? 5 : 4;
            for (int x = 0; x < GateProperties.gatelvls.Count; x++) {
                JObject s = new() {
                    { "lvl_name", GateProperties.gatelvls[x].lvlname },
					//{ "sentry_type", $"SENTRY_{GateProperties.gatelvls[x].sentrytype.ToUpper().Replace(' ', '_')}" },
					{ "sentry_type", $"{gatesentrynames.First(e => e.Value == GateProperties.gatelvls[x].sentrytype).Key}"},
                    { "bucket_num", GateProperties.gatelvls[x].bucket }
                };
                //if using RANDOM, the buckets and hashes are all different per entry in each bucket
                if (GateProperties.random) {
                    switch (GateProperties.gatelvls[x].bucket) {
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
            gatejson = _save;
            return _save;
        }

        private void ResetGate()
        {
            //reset things to default values
            gatejson = null;
            GateProperties.gatelvls.Clear();
            gateLvlList.Rows.Clear();
            this.Text = "Gate Editor";
            //set saved flag to true, because nothing is loaded
            SaveCheckAndWrite(true);
        }
        #endregion

        private void lblGateSectionHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("bosssectionhelp").Show();
        }
    }
}
