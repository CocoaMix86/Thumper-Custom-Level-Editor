﻿using Newtonsoft.Json;
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

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
	{
        #region Variables
        public bool _savemaster = true;
		public string _loadedmaster {
			get { return loadedmaster; }
			set
			{
				if (value == null) {
                    if (loadedmaster != null && lockedfiles.ContainsKey(loadedmaster)) {
                        lockedfiles[loadedmaster].Close();
                        lockedfiles.Remove(loadedmaster);
                    }
                    loadedmaster = value;
                    ResetMaster();
				}
				else if (loadedmaster != value) {
					if (loadedmaster != null && lockedfiles.ContainsKey(loadedmaster)) {
						lockedfiles[loadedmaster].Close();
						lockedfiles.Remove(loadedmaster);
					}
					loadedmaster = value;
					ShowPanel(true, panelMaster);
                    PanelEnableState(panelMaster, true);

                    if (!File.Exists(loadedmaster)) {
						File.WriteAllText(loadedmaster, "");
					}
					lockedfiles.Add(loadedmaster, new FileStream(loadedmaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
				}
			}
		}
		private string loadedmaster;
		dynamic masterjson;
		List<MasterLvlData> clipboardmaster = new();
		ObservableCollection<MasterLvlData> _masterlvls = new();
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

            string _file;
			dynamic _load = null;

			//show a different confirmation message if the selected item is gate or lvl
			if (_masterlvls[e.RowIndex].lvlname is "<none>" or "") {
				if ((!_savegate && MessageBox.Show("Current gate is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savegate) {
					_file = (_masterlvls[e.RowIndex].gatename).Replace(".gate", "");
					if (File.Exists($@"{workingfolder}\gate_{_file}.txt")) {
						_load = LoadFileLock($@"{workingfolder}\gate_{_file}.txt");
					}
					else {
						MessageBox.Show("This gate does not exist in the Level folder.");
						return;
					}
				}
				else
					return;
			}
			else if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				_file = (_masterlvls[e.RowIndex].lvlname).Replace(".lvl", "");
				if (File.Exists($@"{workingfolder}\lvl_{_file}.txt")) {
					_load = LoadFileLock($@"{workingfolder}\lvl_{_file}.txt");
				}
				else {
					MessageBox.Show("This lvl does not exist in the Level folder.");
					return;
				}
			}
			else
				return;

			//remove event handlers from a few controls so they don't trigger when their values change
			dropMasterLvlRest.SelectedIndexChanged -= new EventHandler(dropMasterLvlRest_SelectedIndexChanged);
			//load the selected item
			if ((string)_load["obj_type"] == "SequinLevel") 
				LoadLvl(_load, $@"{workingfolder}\lvl_{_file}.txt");
			else if ((string)_load["obj_type"] == "SequinGate") 
				LoadGate(_load, $@"{workingfolder}\gate_{_file}.txt");
			dropMasterLvlRest.SelectedItem = _masterlvls[e.RowIndex].rest;
			btnMasterOpenRest.Enabled = dropMasterLvlRest.SelectedIndex > 0;
			//re-add event handlers
			dropMasterLvlRest.SelectedIndexChanged += new EventHandler(dropMasterLvlRest_SelectedIndexChanged);
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
			} catch { }
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
				if (!String.IsNullOrEmpty(_masterlvls[_in].lvlname) && _masterlvls[_in].lvlname != "<none>") {
					int idx = _masterlvls[_in].lvlname.LastIndexOf('.');
					masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.lvl, _masterlvls[_in].lvlname[..idx], _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
				}
				else {
					int idx = _masterlvls[_in].gatename.LastIndexOf('.');
					masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.gate, _masterlvls[_in].gatename[..idx], _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
				}
			}
			//if action REMOVE, remove row from the master DGV
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
				masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
			}

			masterLvlList.RowEnter += masterLvlList_RowEnter;
            HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => $@"{workingfolder}\{(_masterlvls[x.Index].lvlname != "" ? "lvl" : "gate")}_{x.Cells[1].Value}.txt").ToList());
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
			btnMasterOpenIntro.Enabled = dropMasterIntro.SelectedIndex > 0;
			SaveMaster(false);
		}
		private void dropMasterCheck_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnMasterOpenCheckpoint.Enabled = dropMasterCheck.SelectedIndex > 0;
			SaveMaster(false);
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
			} catch (NullReferenceException) { }
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
                ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    string filepath = CopyToWorkingFolderCheck(ofd.FileName);
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = LoadFileLock(filepath);
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
				mastersaveAsToolStripMenuItem.PerformClick();
				return;
			}
			else
				WriteMaster();
		}
		///SAVE AS
		private void mastersaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//check if master exists already.
			if (File.Exists($@"{workingfolder}\master_sequin.txt")) {
				if (MessageBox.Show("You have a master file already for this Level. Proceeding will overwrite it.\nDo you want to continue?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
            }
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = workingfolder ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
				if (sender == null) {
					_loadedmaster = null;
				}
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                _loadedmaster = $@"{storePath}\master_sequin.txt";
                WriteMaster();
                //after saving new file, refresh the workingfolder
                btnWorkRefresh.PerformClick();
            }
        }
		public void WriteMaster()
		{
            //write contents direct to file without prompting save dialog
            JObject _save = MasterBuildSave();
			if (!lockedfiles.ContainsKey(loadedmaster)) {
				lockedfiles.Add(loadedmaster, new FileStream(loadedmaster, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
			}
			WriteFileLock(lockedfiles[loadedmaster], _save);
			SaveMaster(true, true);
			lblMasterName.Text = $"Master Editor - sequin.master";

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
			PlaySound("UIobjectremove");
			masterLvlList_CellClick(null, new DataGridViewCellEventArgs(1, _in >= _masterlvls.Count ? _in - 1 : _in));
		}
		private void btnMasterLvlAdd_Click(object sender, EventArgs e)
		{
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Lvl/Gate File (*.txt)|lvl_*.txt;gate_*.txt";
            ofd.Title = "Load a Thumper Lvl/Gate file";
            ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddFiletoMaster(ofd.FileName);
            }
        }

		private void AddFiletoMaster(string path)
        {
			//parse leaf to JSON
			dynamic _load = LoadFileLock(path);
			//check if file being loaded is actually a leaf. Can do so by checking the JSON key
			if ((string)_load["obj_type"] is not "SequinLevel" and not "SequinGate") {
				MessageBox.Show("This does not appear to be a lvl or a gate file!", "File load error");
				return;
			}
			//check if lvl exists in the same folder as the master. If not, allow user to copy file.
			//this is why I utilize workingfolder
			if (Path.GetDirectoryName(path) != workingfolder) {
				if (MessageBox.Show("The item you chose does not exist in the same folder as this master. Do you want to copy it to this folder and load it?", "File load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
					if (!File.Exists($@"{workingfolder}\{Path.GetFileName(path)}")) File.Copy(path, $@"{workingfolder}\{Path.GetFileName(path)}");
				else
					return;
			}
			PlaySound("UIobjectadd");
			//add lvl/gate data to the list
			if (_load["obj_type"] == "SequinLevel")
				_masterlvls.Add(new MasterLvlData() {
					lvlname = (string)_load["obj_name"],
					playplus = true,
					checkpoint = true,
					checkpoint_leader = "<none>",
					gatename = "",
					rest = "<none>",
					id = rng.Next(0, 1000000)
				});
			else if (_load["obj_type"] == "SequinGate")
				_masterlvls.Add(new MasterLvlData() {
					gatename = (string)_load["obj_name"],
					playplus = true,
					checkpoint = true,
					checkpoint_leader = "<none>",
					lvlname = "",
					rest = "<none>",
					id = rng.Next(0, 1000000)
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
			PlaySound("UIkcopy");
			btnMasterLvlPaste.Enabled = true;
		}

		private void btnMasterLvlPaste_Click(object sender, EventArgs e)
		{
			int _in = masterLvlList.CurrentRow?.Index + 1 ?? 0;
			foreach (MasterLvlData mld in clipboardmaster)
				_masterlvls.Insert(_in, mld.Clone());
			PlaySound("UIkpaste");
		}

		private void btnConfigColor_Click(object sender, EventArgs e)
		{
			PlaySound("UIcoloropen");
			Button button = (Button)sender;
			colorDialogNew.Color = button.BackColor;
            if (colorDialogNew.ShowDialog() == DialogResult.OK) {
				ColorButton(button, colorDialogNew.Color);
				PlaySound("UIcolorapply");
				SaveMaster(false);
			}
		}

		private void btnMasterRefreshLvl_Click(object sender, EventArgs e)
		{
			if (workingfolder == null)
				return;
			lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList() ?? new List<string>();
			lvlsinworkfolder.Add("<none>");
			lvlsinworkfolder.Sort();

			dropMasterCheck.SelectedIndexChanged -= dropMasterCheck_SelectedIndexChanged;
			dropMasterIntro.SelectedIndexChanged -= dropMasterIntro_SelectedIndexChanged;
			dropMasterLvlRest.SelectedIndexChanged -= dropMasterLvlRest_SelectedIndexChanged;
            ///add lvl list as datasources to dropdowns
            object _select = dropMasterCheck.SelectedItem;
			dropMasterCheck.DataSource = lvlsinworkfolder.ToList();
			dropMasterCheck.SelectedItem = _select;

			_select = dropMasterIntro.SelectedItem;
			dropMasterIntro.DataSource = lvlsinworkfolder.ToList();
			dropMasterIntro.SelectedItem = _select;

			_select = dropMasterLvlRest.SelectedItem;
			dropMasterLvlRest.DataSource = lvlsinworkfolder.ToList();
			dropMasterLvlRest.SelectedItem = _select;
			//
			dropMasterCheck.SelectedIndexChanged += dropMasterCheck_SelectedIndexChanged;
			dropMasterIntro.SelectedIndexChanged += dropMasterIntro_SelectedIndexChanged;
			dropMasterLvlRest.SelectedIndexChanged += dropMasterLvlRest_SelectedIndexChanged;
			PlaySound("UIrefresh");
		}

		private void btnRevertMaster_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			SaveMaster(true);
			LoadMaster(masterjson, loadedmaster);
			PlaySound("UIrevertchanges");
		}

		//buttons that click other buttons
		private void btnMasterPanelNew_Click(object sender, EventArgs e) => masternewToolStripMenuItem.PerformClick();
		//these all load a lvl
		private void btnMasterOpenIntro_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterIntro.SelectedItem.ToString());
		private void btnMasterOpenCheckpoint_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterCheck.SelectedItem.ToString());
		private void btnMasterOpenRest_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterLvlRest.SelectedItem.ToString());

		private void btnMasterRuntime_Click(object sender, EventArgs e) => CalculateMasterRuntime();

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
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(filepath);
            //check if the assign actually worked. If not, stop loading.
            if (workingfolder != Path.GetDirectoryName(filepath))
                return;
            _loadedmaster = filepath;
			//set some visual elements
			lblMasterName.Text = $"Master Editor - sequin.master";

			///Clear form elements so new data can load
			_masterlvls.Clear();

			///load lvls associated with this master
			foreach (dynamic _lvl in _load["groupings"]) {
				_masterlvls.Add(new MasterLvlData() {
					lvlname = _lvl["lvl_name"],
					gatename = _lvl["gate_name"],
					checkpoint = _lvl["checkpoint"],
					playplus = _lvl["play_plus"],
					isolate = _lvl["isolate"] ?? false,
					checkpoint_leader = _lvl["checkpoint_leader_lvl_name"],
					rest = _lvl["rest_lvl_name"] == "" ? "<none>" : _lvl["rest_lvl_name"],
					id = rng.Next(0, 10000000)
				});
            }
            dropMasterSkybox.SelectedIndex = dropMasterSkybox.Items.IndexOf((string)_load["skybox_name"] == "" ? "<none>" : (string)_load["skybox_name"]);
			dropMasterIntro.SelectedIndex = dropMasterIntro.Items.IndexOf((string)_load["intro_lvl_name"] == "" ? "<none>" : (string)_load["intro_lvl_name"]);
			dropMasterCheck.SelectedIndex = dropMasterCheck.Items.IndexOf((string)_load["checkpoint_lvl_name"] == "" ? "<none>" : (string)_load["checkpoint_lvl_name"]);
            ///load Config data (if file exists)
            LoadConfig();
			CalculateMasterRuntime();
			///set save flag (master just loaded, has no changes)
			SaveMaster(true);
			masterjson = _load;
			btnRevertMaster.Enabled = true;
			btnMasterRuntime.Enabled = true;
		}

		public void LoadConfig()
		{
            System.Collections.Generic.List<string> _configfile = Directory.GetFiles(workingfolder, "config_*.txt").ToList();
			if (_configfile.Count > 0) {
				dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_configfile[0]), "#.*", ""));
				NUD_ConfigBPM.Value = (int)_load["bpm"];
				ColorButton(btnConfigRailColor, Color.FromArgb((int)((float)_load["rails_color"][0] * 255), (int)((float)_load["rails_color"][1] * 255), (int)((float)_load["rails_color"][2] * 255)));
				ColorButton(btnConfigGlowColor, Color.FromArgb((int)((float)_load["rails_glow_color"][0] * 255), (int)((float)_load["rails_glow_color"][1] * 255), (int)((float)_load["rails_glow_color"][2] * 255)));
				ColorButton(btnConfigPathColor, Color.FromArgb((int)((float)_load["path_color"][0] * 255), (int)((float)_load["path_color"][1] * 255), (int)((float)_load["path_color"][2] * 255)));
			}
			else {
				NUD_ConfigBPM.Value = 420;
				btnConfigRailColor.BackColor = Color.White;
				btnConfigGlowColor.BackColor = Color.White;
				btnConfigPathColor.BackColor = Color.White;
			}
		}

		public void ColorButton(Control control, Color color)
        {
			control.BackColor = color;
			control.ForeColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);;
		}

		public void MasterLoadLvl(string path)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				if (!path.Contains(".lvl"))
					return;
				string _file = path.Replace(".lvl", "");
				dynamic _load;
				try {
					_load = LoadFileLock($@"{workingfolder}\lvl_{_file}.txt");
				}
				catch {
					MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this leaf from a different folder?");
					return;
				}
				//load the selected lvl
				LoadLvl(_load, $@"{workingfolder}\lvl_{_file}.txt");
			}
		}

		public void SaveMaster(bool save, bool playsound = false)
		{
			//make the beeble emote
			pictureBox1_Click(null, null);

			_savemaster = save;
			if (!save) {
				btnSaveMaster.Enabled = true;
				btnRevertMaster.Enabled = masterjson != null;
				btnRevertMaster.ToolTipText = masterjson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
				toolstripTitleMaster.BackColor = Color.Maroon;
			}
			else {
				btnSaveMaster.Enabled = false;
				btnRevertMaster.Enabled = false;
				toolstripTitleMaster.BackColor = Color.FromArgb(40, 40, 40);
				if (playsound) PlaySound("UIsave");
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
                { "skybox_name", dropMasterSkybox.Text.Replace("<none>", "") },
                { "intro_lvl_name", dropMasterIntro.Text.Replace("<none>", "") }
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
			_save.Add("checkpoint_lvl_name", dropMasterCheck.Text.Replace("<none>", ""));
			masterjson = _save;
			///end build
			///
			///begin building Config JSON object
			JObject _config = new() {
                { "obj_type", "LevelLib" },
                { "bpm", NUD_ConfigBPM.Value }
            };
            //for each lvl in Master that has checkpoint:True, Config requires a "SECTION_LINEAR"
            JArray level_sections = new();
			for (int x = 0; x < checkpoints; x++)
				level_sections.Add("SECTION_LINEAR");
			_config.Add("level_sections", level_sections);
			//
			//add rail color
			JArray rails_color = new() {
				Decimal.Round((decimal)btnConfigRailColor.BackColor.R / 255, 3),
				Decimal.Round((decimal)btnConfigRailColor.BackColor.G / 255, 3),
				Decimal.Round((decimal)btnConfigRailColor.BackColor.B / 255, 3),
				Decimal.Round((decimal)btnConfigRailColor.BackColor.A / 255, 3)
			};
			_config.Add("rails_color", rails_color);
			//
			//add rail glow color
			JArray rails_glow_color = new() {
				Decimal.Round((decimal)btnConfigGlowColor.BackColor.R / 255, 3),
				Decimal.Round((decimal)btnConfigGlowColor.BackColor.G / 255, 3),
				Decimal.Round((decimal)btnConfigGlowColor.BackColor.B / 255, 3),
				Decimal.Round((decimal)btnConfigGlowColor.BackColor.A / 255, 3)
			};
			_config.Add("rails_glow_color", rails_glow_color);
			//
			//add path color
			JArray path_color = new() {
				Decimal.Round((decimal)btnConfigPathColor.BackColor.R / 255, 3),
				Decimal.Round((decimal)btnConfigPathColor.BackColor.G / 255, 3),
				Decimal.Round((decimal)btnConfigPathColor.BackColor.B / 255, 3),
				Decimal.Round((decimal)btnConfigPathColor.BackColor.A / 255, 3)
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
			File.WriteAllText($@"{Path.GetDirectoryName(loadedmaster)}\config_{projectjson["level_name"]}.txt", JsonConvert.SerializeObject(_config, Formatting.Indented));

			///only need to return _save, since _config is written already
			return _save;
		}

		private void CalculateMasterRuntime()
		{
			dynamic _load;
			int _beatcount = 0;
			//loop through all entries in the master to get beat counts
			foreach (MasterLvlData _masterlvl in _masterlvls) {
				//this section handles lvl
				if (_masterlvl.lvlname is not null and not "") {
                    int idx = _masterlvl.lvlname.LastIndexOf('.');
					//load the lvl and then loop through its leafs to get beat counts
					_beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{_masterlvl.lvlname[..idx]}.txt");
				}
				//this section handles gate
				else {
					//load the gate to then loop through all lvls in it
					int idx = _masterlvl.gatename.LastIndexOf('.');
                    _load = LoadFileLock($"{workingfolder}\\gate_{_masterlvl.gatename[..idx]}.txt");
					if (_load == null)
						continue;
					foreach (dynamic _lvl in _load["boss_patterns"]) {
						//load the lvl and then loop through its leafs to get beat counts
						idx = ((string)_lvl["lvl_name"]).LastIndexOf('.');
                        _beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{((string)_lvl["lvl_name"])[..idx]}.txt");
					}
				}

				if (_masterlvl.rest is not "" and not "<none>" and not null)
					_beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{_masterlvl.rest[.._masterlvl.rest.LastIndexOf('.')]}.txt");
			}
			if (dropMasterIntro.Text != "<none>")
				_beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{dropMasterIntro.Text[..dropMasterIntro.Text.LastIndexOf('.')]}.txt");
			if (dropMasterCheck.Text != "<none>")
				_beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{dropMasterCheck.Text[..dropMasterCheck.Text.LastIndexOf('.')]}.txt");
			
			lblMAsterRuntimeBeats.Text = $"Beats: {_beatcount}";

			///Calculate min/sec based on beats and BPM
			lblMasterRuntime.Text = $"Time: {TimeSpan.FromMinutes(_beatcount / (double)NUD_ConfigBPM.Value).ToString("hh':'mm':'ss'.'fff")}";

		}
		private int LoadLvlGetBeatCounts(string path)
		{
			int _beatcount = 0;

			//load the lvl and then loop through its leafs to get beat counts
			dynamic _load = LoadFileLock(path);
            if (_load == null)
                return 0;
            foreach (dynamic leaf in _load["leaf_seq"]) {
				_beatcount += (int)leaf["beat_cnt"];
			}
			//every lvl has an approach beats to consider too
			//_beatcount += (int)_load["approach_beats"];

			return _beatcount;
		}

		private void ResetMaster()
        {
			//reset things to default values
			masterjson = null;
			_masterlvls.Clear();
			lblMasterName.Text = "Master Editor";
			btnConfigGlowColor.BackColor = Color.White;
			btnConfigPathColor.BackColor = Color.White;
			btnConfigRailColor.BackColor = Color.White;
			dropMasterSkybox.SelectedIndex = 1;
			NUD_ConfigBPM.Value = 400;
			//set saved flag to true, because nothing is loaded
			SaveMaster(true);
		}
		#endregion
	}
}