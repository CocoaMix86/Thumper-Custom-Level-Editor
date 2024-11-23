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

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_MasterEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_MasterEditor(dynamic load = null, string filepath = null)
        {
            InitializeComponent();
            InitializeMasterStuff();
            masterToolStrip.Renderer = new ToolStripOverride();
            TCLE.InitializeTracks(masterLvlList, false);

            if (load != null)
                LoadMaster(load, filepath);
            propertyGridMaster.SelectedObject = _properties;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public string _loadedmaster
        {
            get { return LoadedMaster; }
            set {
                if (value == null) {
                    if (LoadedMaster != null && TCLE.lockedfiles.ContainsKey(LoadedMaster)) {
                        TCLE.lockedfiles[LoadedMaster].Close();
                        TCLE.lockedfiles.Remove(LoadedMaster);
                    }
                    LoadedMaster = value;
                    ResetMaster();
                }
                else if (LoadedMaster != value) {
                    if (LoadedMaster != null && TCLE.lockedfiles.ContainsKey(LoadedMaster)) {
                        TCLE.lockedfiles[LoadedMaster].Close();
                        TCLE.lockedfiles.Remove(LoadedMaster);
                    }
                    LoadedMaster = value;

                    if (!File.Exists(LoadedMaster)) {
                        File.WriteAllText(LoadedMaster, "");
                    }
                    TCLE.lockedfiles.Add(LoadedMaster, new FileStream(LoadedMaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private static string LoadedMaster;
        private List<MasterLvlData> clipboardmaster = new();
        public ObservableCollection<MasterLvlData> _masterlvls { get { return _properties.masterlvls; } set { _properties.masterlvls = value; } }
        public MasterProperties _properties;
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
            if (e.ColumnIndex == -1 || e.RowIndex == -1 || e.RowIndex > _masterlvls.Count - 1)
                return;
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;
            _properties.sublevel = _masterlvls[e.RowIndex];
            propertyGridMaster.ExpandAllGridItems();
            propertyGridMaster.Refresh();
        }

        private void masterLvlList_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void masterLvlList_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode dragdropnode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            AddFiletoMaster($@"{Path.GetDirectoryName(TCLE.WorkingFolder)}\{dragdropnode.FullPath}");
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
                int beats = TCLE.CalculateSingleLvlRuntime(TCLE.WorkingFolder, _masterlvls[_in]);
                string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                masterLvlList.Rows.Insert(_in, new object[] { 0, (_masterlvls[_in].type == "lvl" ? Properties.Resources.editor_lvl : Properties.Resources.editor_gate), _masterlvls[_in].lvlname, $"{beats} beats -- {time}" });
            }
            //if action REMOVE, remove row from the master DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
            }
            //TCLE.HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => $@"{TCLE.WorkingFolder}\{(_masterlvls[x.Index].lvlname != "" ? "lvl" : "gate")}_{x.Cells[1].Value}.txt").ToList());
            ///TCLE.HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => (_masterlvls[x.Index].name)).ToList());
            //set selected index. Mainly used when moving items
            ///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
            //enable certain buttons if there are enough items for them
            btnMasterLvlDelete.Enabled = _masterlvls.Count > 0;
            btnMasterLvlUp.Enabled = _masterlvls.Count > 1;
            btnMasterLvlDown.Enabled = _masterlvls.Count > 1;
            btnMasterLvlCopy.Enabled = _masterlvls.Count > 0;

            foreach (DataGridViewRow dgvr in masterLvlList.Rows) {
                string levelnum = "";
                if (_masterlvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_CRAKHED" or "SECTION_BOSS_CRAKHED_FINAL")
                    levelnum = "Ω";
                else if (_masterlvls[dgvr.Index].gatesectiontype is "SECTION_BOSS_PYRAMID")
                    levelnum = "∞";
                else
                    levelnum = (dgvr.Index + 1).ToString();
                dgvr.Cells[0].Value = levelnum;
            }

            //set lvl save flag to false
            ///Save(false);
        }

        private void masteropenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Master File (*.master)|*.master";
                ofd.Title = "Load a Thumper Master file";
                ofd.InitialDirectory = TCLE.WorkingFolder ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    string filepath = TCLE.CopyToWorkingFolderCheck(ofd.FileName, TCLE.WorkingFolder);
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath);
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
            sfd.InitialDirectory = TCLE.WorkingFolder ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                _loadedmaster = $@"{storePath}\master_sequin.txt";
                BuildSave(_properties);
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
                todelete.Add(_masterlvls[dgvr.Index]);
            }
            int _in = masterLvlList.CurrentRow.Index;
            foreach (MasterLvlData mld in todelete)
                _masterlvls.Remove(mld);
            TCLE.PlaySound("UIobjectremove");
            masterLvlList_CellClick(null, new DataGridViewCellEventArgs(1, _in >= _masterlvls.Count ? _in - 1 : _in));
        }
        private void btnMasterLvlAdd_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Lvl/Gate File (*.txt)|*.txt";
            ofd.Title = "Load a Thumper Lvl/Gate file";
            ofd.InitialDirectory = TCLE.WorkingFolder ?? Application.StartupPath;
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
            if (!Path.GetDirectoryName(path).Contains(TCLE.WorkingFolder)) {
                if (MessageBox.Show("The item you chose does not exist in the project. Do you want to copy it to the project folder?", "Yhumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    if (!File.Exists($@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}"))
                        File.Copy(path, $@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}");
                    else
                        return;
            }
            TCLE.PlaySound("UIobjectadd");
            //add lvl/gate data to the list
            if (_load["obj_type"] == "SequinLevel")
                _masterlvls.Add(new MasterLvlData() {
                    type = "lvl",
                    name = (string)_load["obj_name"],
                    playplus = true,
                    checkpoint = true,
                    checkpoint_leader = "<none>",
                    rest = "<none>",
                    gatesectiontype = "",
                    id = TCLE.rng.Next(0, 1000000)
                });
            else if (_load["obj_type"] == "SequinGate")
                _masterlvls.Add(new MasterLvlData() {
                    type = "gate",
                    name = (string)_load["obj_name"],
                    playplus = true,
                    checkpoint = true,
                    checkpoint_leader = "<none>",
                    rest = "<none>",
                    gatesectiontype = (string)_load["section_type"],
                    id = TCLE.rng.Next(0, 1000000)
                });
        }

        private void btnMasterLvlUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                _masterlvls.Insert(dgvr - 1, _masterlvls[dgvr]);
                _masterlvls.RemoveAt(dgvr + 1);
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
                _masterlvls.Insert(dgvr + 2, _masterlvls[dgvr]);
                _masterlvls.RemoveAt(dgvr);
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
            clipboardmaster = _masterlvls.Where(x => selectedrows.Contains(_masterlvls.IndexOf(x))).ToList();
            clipboardmaster.Reverse();
            TCLE.PlaySound("UIkcopy");
            btnMasterLvlPaste.Enabled = true;
        }

        private void btnMasterLvlPaste_Click(object sender, EventArgs e)
        {
            int _in = masterLvlList.CurrentRow?.Index + 1 ?? 0;
            foreach (MasterLvlData mld in clipboardmaster)
                _masterlvls.Insert(_in, mld.Clone());
            TCLE.PlaySound("UIkpaste");
        }

        private void btnRevertMaster_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            LoadMaster(_properties.revertPoint, LoadedMaster);
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

        public void LoadMaster(dynamic _load, string filepath)
        {
            if (_load == null)
                return;
            if ((string)_load["obj_type"] != "SequinMaster") {
                MessageBox.Show("This does not appear to be a master file!");
                return;
            }
            _loadedmaster = filepath;
            //set some visual elements
            this.Text = $"sequin.master";

            //setup new master properties
            _properties = new(this, filepath) {
                skybox = (string)_load["skybox_name"] == "" ? "<none>" : (string)_load["skybox_name"],
                introlvl = (string)_load["intro_lvl_name"] == "" ? "<none>" : (string)_load["intro_lvl_name"],
                checkpointlvl = (string)_load["checkpoint_lvl_name"] == "" ? "<none>" : (string)_load["checkpoint_lvl_name"]
            };

            ///Clear form elements so new data can load
            _masterlvls.Clear();
            ///load lvls associated with this master
            foreach (dynamic _lvl in _load["groupings"]) {
                _masterlvls.Add(new MasterLvlData() {
                    type = ((string)_lvl["lvl_name"]) != String.Empty ? "lvl" : "gate",
                    name = ((string)_lvl["lvl_name"]) != String.Empty ? _lvl["lvl_name"] : _lvl["gate_name"],
                    checkpoint = _lvl["checkpoint"],
                    playplus = _lvl["play_plus"],
                    isolate = _lvl["isolate"] ?? false,
                    checkpoint_leader = _lvl["checkpoint_leader_lvl_name"],
                    rest = _lvl["rest_lvl_name"] == "" ? "<none>" : _lvl["rest_lvl_name"],
                    id = TCLE.rng.Next(0, 10000000)
                });
            }
            ///set save flag (master just loaded, has no changes)
            SaveCheckAndWrite(true);
        }

        public static void MasterLoadLvl(string path)
        {
            if ((/*!_mainform._savelvl && */MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)/* || _mainform._savelvl*/) {
                if (!path.Contains(".lvl"))
                    return;
                string _file = path.Replace(".lvl", "");
                dynamic _load;
                try {
                    _load = TCLE.LoadFileLock($@"{TCLE.WorkingFolder}\lvl_{_file}.txt");
                }
                catch {
                    MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this leaf from a different folder?");
                    return;
                }
                //load the selected lvl
                ///_mainform.LoadLvl(_load, $@"{TCLE.WorkingFolder}\lvl_{_file}.txt");
            }
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            //make the beeble emote
            TCLE.beeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                if (!this.Text.EndsWith('*')) this.Text += "*";
                //add current JSON to the undo list
                _properties.undoItems.Add(BuildSave(_properties));
            }
            else {
                //build the JSON to write to file
                JObject _saveJSON = BuildSave(_properties);
                _properties.revertPoint = _saveJSON;
                //if file is not locked, lock it
                if (!TCLE.lockedfiles.ContainsKey(LoadedMaster)) {
                    TCLE.lockedfiles.Add(LoadedMaster, new FileStream(LoadedMaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedMaster], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public void RecalcLvlRuntime()
        {
            foreach (MasterLvlData _lvl in _masterlvls) {
                int beats = TCLE.CalculateSingleLvlRuntime(TCLE.WorkingFolder, _lvl);
                string time = TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                masterLvlList.Rows[_masterlvls.IndexOf(_lvl)].Cells[3].Value = $"{beats} beats -- {time}";
            }
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
                    { "lvl_name", group.lvlname.Replace("<none>", "") ?? "" },
                    { "gate_name", group.gatename.Replace("<none>", "") ?? "" },
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
            _masterlvls.Clear();
            this.Text = "Master Editor";
            _properties.skybox = "";
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
