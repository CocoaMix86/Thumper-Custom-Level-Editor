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
        public bool _savemaster = true;
        public string _loadedmaster
        {
            get { return loadedmaster; }
            set {
                if (value == null) {
                    if (loadedmaster != null && TCLE.lockedfiles.ContainsKey(loadedmaster)) {
                        TCLE.lockedfiles[loadedmaster].Close();
                        TCLE.lockedfiles.Remove(loadedmaster);
                    }
                    loadedmaster = value;
                    ResetMaster();
                }
                else if (loadedmaster != value) {
                    if (loadedmaster != null && TCLE.lockedfiles.ContainsKey(loadedmaster)) {
                        TCLE.lockedfiles[loadedmaster].Close();
                        TCLE.lockedfiles.Remove(loadedmaster);
                    }
                    loadedmaster = value;

                    if (!File.Exists(loadedmaster)) {
                        File.WriteAllText(loadedmaster, "");
                    }
                    TCLE.lockedfiles.Add(loadedmaster, new FileStream(loadedmaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private string loadedmaster;
        dynamic masterjson;
        public List<MasterLvlData> clipboardmaster = new();
        public ObservableCollection<MasterLvlData> _masterlvls = new();
        public MasterProperties _properties;
        #endregion

        #region EventHandlers
        ///         ///
        /// EVENTS ///
        ///         ///

        /// DGV MASTERLVLLIST
        //Row Enter (load the selected lvl)

        private void masterLvlList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if not selecting the file column, return and do nothing
            if (e.ColumnIndex == -1 || e.ColumnIndex > 1 || e.RowIndex == -1 || e.RowIndex > _masterlvls.Count - 1)
                return;
            if (Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
                return;

            propertyGridSublevel.SelectedObject = _masterlvls[e.RowIndex];
        }
        private void masterLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
        }
        //Cell value changed (for checkboxes)
        private void masterLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try {
                _masterlvls[e.RowIndex].checkpoint = bool.Parse(masterLvlList[2, e.RowIndex].Value.ToString());
                _masterlvls[e.RowIndex].playplus = bool.Parse(masterLvlList[3, e.RowIndex].Value.ToString());
                _masterlvls[e.RowIndex].isolate = bool.Parse(masterLvlList[4, e.RowIndex].Value.ToString());
                //set lvl save flag to false
                SaveMaster(false);
            }
            catch { }
        }

        public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            masterLvlList.RowEnter -= masterLvlList_RowEnter;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) {
                masterLvlList.RowCount = 0;
            }
            //if action ADD, add new row to the master DGV
            //NewStartingIndex and OldStartingIndex track where the changes were made
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                int _in = e.NewStartingIndex;
                //detect if lvl or a gate. If it's a gate, the lvlname won't be set
                if (_masterlvls[_in].type == "lvl") {
                    //int idx = _masterlvls[_in].lvlname.LastIndexOf('.');
                    masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.editor_lvl, _masterlvls[_in].lvlname, _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
                }
                else {
                    //int idx = _masterlvls[_in].gatename.LastIndexOf('.');
                    masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.editor_gate, _masterlvls[_in].gatename, _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
                }
            }
            //if action REMOVE, remove row from the master DGV
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
            }

            masterLvlList.RowEnter += masterLvlList_RowEnter;
            //TCLE.HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => $@"{TCLE.WorkingFolder}\{(_masterlvls[x.Index].lvlname != "" ? "lvl" : "gate")}_{x.Cells[1].Value}.txt").ToList());
            ///TCLE.HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => (_masterlvls[x.Index].name)).ToList());
            //set selected index. Mainly used when moving items
            ///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
            //enable certain buttons if there are enough items for them
            btnMasterLvlDelete.Enabled = _masterlvls.Count > 0;
            btnMasterLvlUp.Enabled = _masterlvls.Count > 1;
            btnMasterLvlDown.Enabled = _masterlvls.Count > 1;
            btnMasterLvlCopy.Enabled = _masterlvls.Count > 0;

            //set lvl save flag to false
            SaveMaster(false);
        }
        ///Other dropdowns on Master Editor
        private void dropMasterIntro_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void dropMasterCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void NUD_ConfigBPM_ValueChanged(object sender, EventArgs e) => SaveMaster(false);
        /// DROP-REST LEVEL Update
        private void dropMasterLvlRest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                _masterlvls[masterLvlList.CurrentRow.Index].rest = dropMasterLvlRest.Text;
                btnMasterOpenRest.Enabled = dropMasterLvlRest.SelectedIndex > 0;
                //set lvl save flag to false
                SaveMaster(false);
            }
            catch (NullReferenceException) { }
        }
        /// DROP-CHECKPOINT LEADER LEVEL Update
        private void dropMasterLvlLeader_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void masternewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!_savemaster && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savemaster) {

                mastersaveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void masteropenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!_savemaster && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savemaster) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Master File (*.txt)|master_*.txt";
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
                    //set lvl save flag to true.
                    SaveMaster(true);
                }
            }
        }
        ///SAVE
        private void mastersaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if _loadedmaster is somehow not set, force Save As instead
            if (_loadedmaster == null) {
                ///_mainform.toolstripMasterSaveAs.PerformClick();
                return;
            }
            else
                WriteMaster();
        }
        ///SAVE AS
        private void mastersaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if master exists already.
            if (File.Exists($@"{TCLE.WorkingFolder}\master_sequin.txt")) {
                if (MessageBox.Show("You have a master file already for this Level. Proceeding will overwrite it.\nDo you want to continue?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
                if (sender == null) {
                    _loadedmaster = null;
                }
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                _loadedmaster = $@"{storePath}\master_sequin.txt";
                WriteMaster();
                //after saving new file, refresh the workingfolder
                ///_mainform.btnWorkRefresh.PerformClick();
            }
        }
        public void WriteMaster()
        {
            //write contents direct to file without prompting save dialog
            JObject _save = MasterBuildSave();
            if (!TCLE.lockedfiles.ContainsKey(loadedmaster)) {
                TCLE.lockedfiles.Add(loadedmaster, new FileStream(loadedmaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
            TCLE.WriteFileLock(TCLE.lockedfiles[loadedmaster], _save);
            SaveMaster(true, true);
            this.Text = $"sequin.master";

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
            ofd.Filter = "Thumper Lvl/Gate File (*.txt)|lvl_*.txt;gate_*.txt";
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
                MessageBox.Show("This does not appear to be a lvl or a gate file!", "File load error");
                return;
            }
            //check if lvl exists in the same folder as the master. If not, allow user to copy file.
            //this is why I utilize workingfolder
            if (Path.GetDirectoryName(path) != TCLE.WorkingFolder) {
                if (MessageBox.Show("The item you chose does not exist in the same folder as this master. Do you want to copy it to this folder and load it?", "File load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    if (!File.Exists($@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}")) File.Copy(path, $@"{TCLE.WorkingFolder}\{Path.GetFileName(path)}");
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
                    id = TCLE.rng.Next(0, 1000000)
                });
        }

        private void btnMasterLvlUp_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                _masterlvls.Insert(dgvr - 1, _masterlvls[dgvr]);
                _masterlvls.RemoveAt(dgvr + 1);
            }
            masterLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                masterLvlList.Rows[dgvr - 1].Cells[1].Selected = true;
            }
            SaveMaster(false);
        }

        private void btnMasterLvlDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = masterLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == masterLvlList.RowCount - 1))
                return;
            selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
            foreach (int dgvr in selectedrows) {
                _masterlvls.Insert(dgvr + 2, _masterlvls[dgvr]);
                _masterlvls.RemoveAt(dgvr);
            }
            masterLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                masterLvlList.Rows[dgvr + 1].Cells[1].Selected = true;
            }
            SaveMaster(false);
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

        private void btnConfigColor_Click(object sender, EventArgs e)
        {
            TCLE.PlaySound("UIcoloropen");
            Button button = (Button)sender;
            TCLE.colorDialogNew.Color = button.BackColor;
            if (TCLE.colorDialogNew.ShowDialog() == DialogResult.OK) {
                ColorButton(button, TCLE.colorDialogNew.Color);
                TCLE.PlaySound("UIcolorapply");
                SaveMaster(false);
            }
        }

        private void btnMasterRefreshLvl_Click(object sender, EventArgs e)
        {
            if (TCLE.WorkingFolder == null)
                return;
            TCLE.lvlsinworkfolder = Directory.GetFiles(TCLE.WorkingFolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList() ?? new List<string>();
            TCLE.lvlsinworkfolder.Add("<none>");
            TCLE.lvlsinworkfolder.Sort();

            ///add lvl list as datasources to dropdowns
            /*
            object _select = dropMasterCheck.SelectedItem;
            dropMasterCheck.DataSource = _mainform.lvlsinworkfolder.ToList();
            dropMasterCheck.SelectedItem = _select;

            _select = dropMasterIntro.SelectedItem;
            dropMasterIntro.DataSource = _mainform.lvlsinworkfolder.ToList();
            dropMasterIntro.SelectedItem = _select;

            _select = dropMasterLvlRest.SelectedItem;
            dropMasterLvlRest.DataSource = _mainform.lvlsinworkfolder.ToList();
            dropMasterLvlRest.SelectedItem = _select;
            */
            TCLE.PlaySound("UIrefresh");
        }

        private void btnRevertMaster_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveMaster(true);
            LoadMaster(masterjson, loadedmaster);
            TCLE.PlaySound("UIrevertchanges");
        }

        //buttons that click other buttons
        private void btnMasterPanelNew_Click(object sender, EventArgs e)
        {
            ///_mainform.toolstripMasterNew.PerformClick();
        }
        //these all load a lvl
        private void btnMasterOpenRest_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterLvlRest.SelectedItem.ToString());

        private void btnMasterRuntime_Click(object sender, EventArgs e) => TCLE.CalculateMasterRuntime(TCLE.WorkingFolder, this);

        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public void InitializeMasterStuff()
        {
            _masterlvls.CollectionChanged += masterlvls_CollectionChanged;
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
            _properties = new(this, filepath) {
                skybox = (string)_load["skybox_name"] == "" ? "<none>" : (string)_load["skybox_name"],
                introlvl = (string)_load["intro_lvl_name"] == "" ? "<none>" : (string)_load["intro_lvl_name"],
                checkpointlvl = (string)_load["checkpoint_lvl_name"] == "" ? "<none>" : (string)_load["checkpoint_lvl_name"]
            };
            ///load Config data (if file exists)
            LoadConfig();
            ///set save flag (master just loaded, has no changes)
            SaveMaster(true);
            masterjson = _load;
            ///btnRevert.Enabled = true;
            ///btnMasterRuntime.Enabled = true;
        }

        public void LoadConfig()
        {
            List<string> _configfile = Directory.GetFiles(TCLE.WorkingFolder, "config_*.txt", SearchOption.AllDirectories).ToList();
            if (_configfile.Count > 0) {
                dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_configfile[0]), "#.*", ""));
                _properties.bpm = (decimal)_load["bpm"];
                dynamic railcolor = _load["rails_color"];
                _properties.rail = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
                dynamic railglowcolor = _load["rails_glow_color"];
                _properties.railglow = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
                dynamic pathcolor = _load["path_color"];
                _properties.path = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
            }
            else {
                _properties.bpm = 420;
                _properties.rail = Color.White;
                _properties.railglow = Color.White;
                _properties.path = Color.White;
            }
        }

        public static void ColorButton(Control control, Color color)
        {
            control.BackColor = color;
            control.ForeColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B); ;
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

        public void SaveMaster(bool save, bool playsound = false)
        {
            //make the beeble emote
            TCLE.beeble.MakeFace();

            _savemaster = save;
            if (!save) {
                /*
                btnSave.Enabled = true;
                btnRevert.Enabled = masterjson != null;
                btnRevert.ToolTipText = masterjson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
                toolstripTitleMaster.BackColor = Color.Maroon;
                */
            }
            else {
                /*
                btnSave.Enabled = false;
                btnRevert.Enabled = false;
                toolstripTitleMaster.BackColor = Color.FromArgb(40, 40, 40);
                */
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public JObject MasterBuildSave()
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
            foreach (MasterLvlData group in _masterlvls) {
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
            masterjson = _save;
            ///end build
            ///
            ///begin building Config JSON object
            JObject _config = new() {
                { "obj_type", "LevelLib" },
                { "bpm", _properties.bpm }
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
            string[] _files = Directory.GetFiles(Path.GetDirectoryName(loadedmaster), "config_*.txt");
            foreach (string s in _files)
                File.Delete(s);
            File.WriteAllText($@"{TCLE.WorkingFolder}\config_{TCLE.projectjson["level_name"]}.txt", JsonConvert.SerializeObject(_config, Formatting.Indented));

            ///only need to return _save, since _config is written already
            return _save;
        }

        private void ResetMaster()
        {
            //reset things to default values
            masterjson = null;
            _masterlvls.Clear();
            this.Text = "Master Editor";
            _properties.rail = Color.White;
            _properties.railglow = Color.White;
            _properties.path = Color.White;
            _properties.skybox = "1";
            _properties.bpm = 400;
            //set saved flag to true, because nothing is loaded
            SaveMaster(true);
        }
        #endregion

        private void lblConfigColorHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("railcolorhelp").Show();
        }
    }
}
