using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_MasterEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_MasterEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            InitializeMasterStuff();
            masterToolStrip.Renderer = new ToolStripOverride();
            TCLE.InitializeTracks(masterLvlList, false);

            if (load != null)
                LoadMaster(load, filepath);
            propertyGridMaster.SelectedObject = masterproperties;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading = false;
        public FileInfo loadedmaster
        {
            get { return LoadedMaster; }
            set {
                if (LoadedMaster != value) {
                    LoadedMaster = value;

                    if (!LoadedMaster.Exists) {
                        LoadedMaster.CreateText();
                    }
                    TCLE.lockedfiles.Add(LoadedMaster, new FileStream(LoadedMaster.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private static FileInfo LoadedMaster;
        private List<MasterLvlData> clipboardmaster = new();
        public ObservableCollection<MasterLvlData> MasterLvls { get { return masterproperties.masterlvls; } set { masterproperties.masterlvls = value; } }
        public MasterProperties masterproperties
        {
            get { return MasterProperties; }
            set {
                SaveCheckAndWrite(false);
                MasterProperties = value;
            }
        }
        public static MasterProperties MasterProperties;
        public decimal BPM { get { return TCLE.dockProjectProperties.BPM; } }
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
            masterproperties.sublevel = MasterLvls[e.RowIndex];
            propertyGridMaster.ExpandAllGridItems();
            propertyGridMaster.Refresh();
        }
        private void masterLvlList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > MasterLvls.Count - 1)
                return;
            TCLE.OpenFile(TCLE.Instance, TCLE.dockProjectExplorer.projectfiles.Where(x => x.Key.EndsWith($@"\{masterLvlList.Rows[e.RowIndex].Cells[2].Value}")).FirstOrDefault().Value);
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void masterLvlList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y)) {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = masterLvlList.DoDragDrop(masterLvlList.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void masterLvlList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {// Get the index of the item the mouse is below.
            rowIndexFromMouseDown = masterLvlList.HitTest(e.X, e.Y).RowIndex;
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
        private void masterLvlList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = masterLvlList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = masterLvlList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;
            //changing the hovered node backcolor to make it obvious where the destination will be
            if (previousDragOver != targetRow && previousDragOver != -1) {
                if (masterLvlList.Rows[previousDragOver].Cells[3].Value.ToString() == "file not found")
                    masterLvlList.Rows[previousDragOver].DefaultCellStyle.BackColor = Color.Maroon;
                else
                    masterLvlList.Rows[previousDragOver].DefaultCellStyle = null;
            }
            if (targetRow != -1 && targetRow != previousDragOver) {
                masterLvlList.Rows[targetRow].DefaultCellStyle.BackColor = Color.FromArgb(64, 53, 130);
                previousDragOver = targetRow;
            }
        }
        private void masterLvlList_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void masterLvlList_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = masterLvlList.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = masterLvlList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move) {
                if (e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow rowToMove) {
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                        return;
                    MasterLvlData tomove = MasterLvls[rowToMove.Index];
                    MasterLvls.RemoveAt(rowIndexFromMouseDown);
                    MasterLvls.Insert(rowIndexOfItemUnderMouseToDrop, tomove);
                }
                if (e.Data.GetData(typeof(TreeNode)) is TreeNode dragdropnode) {
                    AddFiletoMaster($@"{Path.GetDirectoryName(TCLE.WorkingFolder.FullName)}\{dragdropnode.FullPath}");
                }
            }
        }

        public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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
                    MasterLvls[_in].type == "lvl" ? MasterLvls[_in].lvlname : MasterLvls[_in].gatename,
                    0
                });
            }
            //if action REMOVE, remove row from the master DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
            }
            RecalculateRuntime();
            //enable certain buttons if there are enough items for them
            btnMasterLvlDelete.Enabled = MasterLvls.Count > 0;
            btnMasterLvlUp.Enabled = MasterLvls.Count > 1;
            btnMasterLvlDown.Enabled = MasterLvls.Count > 1;
            btnMasterLvlCopy.Enabled = MasterLvls.Count > 0;

            foreach (DataGridViewRow dgvr in masterLvlList.Rows) {
                string levelnum = "";
                if (MasterLvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_CRAKHED" or "SECTION_BOSS_CRAKHED_FINAL")
                    levelnum = "Ω";
                else if (MasterLvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_PYRAMID")
                    levelnum = "∞";
                else
                    levelnum = (dgvr.Index + 1).ToString();
                dgvr.Cells[0].Value = levelnum;
            }

            //set save flag to false
            SaveCheckAndWrite(false);
        }

        private void propertyGridMaster_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveCheckAndWrite(false);
        }

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
        ///SAVE
        public void Save()
        {
            //if _loadedmaster is somehow not set, force Save As instead
            if (LoadedMaster == null)
                SaveAs();
            else
                SaveCheckAndWrite(true, true);
        }
        ///SAVE AS
        public void SaveAs()
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.master)|*.master";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                loadedmaster = new FileInfo($@"{storePath}\sequin.master");
                BuildSave(masterproperties);
                //after saving new file, refresh the workingfolder
                ///_mainform.btnWorkRefresh.PerformClick();
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
            foreach (MasterLvlData mld in todelete)
                MasterLvls.Remove(mld);
            TCLE.PlaySound("UIobjectremove");
            masterLvlList_CellClick(null, new DataGridViewCellEventArgs(1, _in >= MasterLvls.Count ? _in - 1 : _in));
        }
        private void btnMasterLvlAdd_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Lvl/Gate File (*.txt)|*.txt";
            ofd.Title = "Load a Thumper Lvl/Gate file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddFiletoMaster(ofd.FileName);
            }
        }

        private void AddFiletoMaster(string path)
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
                        TCLE.dockProjectExplorer.CreateTreeView();
                    }
                    else
                        return;
            }
            TCLE.PlaySound("UIobjectadd");
            //add lvl/gate data to the list
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
            propertyGridMaster.Refresh();
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
            SaveCheckAndWrite(false);
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
            SaveCheckAndWrite(false);
        }

        private void btnMasterLvlCopy_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            selectedrows.Sort((row, row2) => row.CompareTo(row2));
            clipboardmaster = MasterLvls.Where(x => selectedrows.Contains(MasterLvls.IndexOf(x))).ToList();
            clipboardmaster.Reverse();
            TCLE.PlaySound("UIkcopy");
            btnMasterLvlPaste.Enabled = true;
        }

        private void btnMasterLvlPaste_Click(object sender, EventArgs e)
        {
            int _in = masterLvlList.CurrentRow?.Index + 1 ?? 0;
            foreach (MasterLvlData mld in clipboardmaster)
                MasterLvls.Insert(_in, mld.Clone());
            TCLE.PlaySound("UIkpaste");
        }

        private void btnRevertMaster_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            LoadMaster(masterproperties.revertPoint, LoadedMaster);
            TCLE.PlaySound("UIrevertchanges");
        }

        //buttons that click other buttons
        private void btnMasterPanelNew_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripMasterNew.PerformClick();
        }

        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public void InitializeMasterStuff()
        {
            //_masterlvls.CollectionChanged += masterlvls_CollectionChanged;
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
            SaveCheckAndWrite(true);
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedMaster.FullName);
            LoadMaster(_load, LoadedMaster);
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            if (EditorLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedMaster.Name + "*";
                //add current JSON to the undo list
                masterproperties.undoItems.Add(BuildSave(masterproperties));
            }
            else {
                this.Text = LoadedMaster.Name;
                //build the JSON to write to file
                JObject _saveJSON = BuildSave(masterproperties);
                masterproperties.revertPoint = _saveJSON;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedMaster], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");

                foreach (IDockContent dock in TCLE.Documents.Where(x => x.DockHandler.TabText.Contains(LoadedMaster.Name))) {
                    if (dock.GetType() == typeof(Form_RawText))
                        (dock as Form_RawText).Reload();
                }
            }
        }

        public void RecalculateRuntime()
        {
            foreach (MasterLvlData _lvl in MasterLvls) {
                int beats = TCLE.CalculateSublevelRuntime(_lvl);
                if (beats == -1) {
                    masterLvlList.Rows[MasterLvls.IndexOf(_lvl)].DefaultCellStyle.BackColor = Color.Maroon;
                    masterLvlList.Rows[MasterLvls.IndexOf(_lvl)].Cells[3].Value = $"file not found";
                }
                else {
                    string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    masterLvlList.Rows[MasterLvls.IndexOf(_lvl)].DefaultCellStyle = null;
                    masterLvlList.Rows[MasterLvls.IndexOf(_lvl)].Cells[3].Value = $"{beats} beats -- {time}";
                }
            }
            masterLvlList.Refresh();
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
                    { "lvl_name", (group.type == "lvl" ? group.lvlname : "") },
                    { "gate_name", (group.type == "gate" ? group.gatename : "") },
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
            SaveCheckAndWrite(true);
        }
        #endregion

        private void lblConfigColorHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("railcolorhelp").Show();
        }
    }
}
