using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor
	{
		#region Variables
		bool _savemaster = true;
		public string _loadedmaster {
			get { return loadedmaster; }
			set
			{
				if (value == null) {
					loadedmaster = value;
					lblMasterName.Text = "Master Editor";
					SaveMaster(true);
				}
				if (loadedmaster != value) {
					loadedmaster = value;
					ShowPanel(true, panelMaster);
				}
			}
		}
		private string loadedmaster;
		string _loadedmastertemp;
		dynamic masterjson;
		MasterLvlData clipboardmaster = new();
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
			if (e.ColumnIndex == -1 || e.ColumnIndex > 1 || e.RowIndex == -1)
				return;

			string _file;
			dynamic _load = null;

			//show a different confirmation message if the selected item is gate or lvl
			if (String.IsNullOrEmpty(_masterlvls[e.RowIndex].lvlname)) {
				if ((!_savegate && MessageBox.Show("Current gate is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savegate) {
					_file = (_masterlvls[e.RowIndex].gatename).Replace(".gate", "");
					try {
						_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\gate_{_file}.txt"), "#.*", ""));
					}
					catch {
						MessageBox.Show($@"Could not locate ""gate_{_file}.txt"" in the same folder as this master. Did you add this gate from a different folder?");
						return;
					}
					_loadedgatetemp = $@"{workingfolder}\gate_{_file}.txt";
				}
				else
					return;
			}
			else if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				_file = (_masterlvls[e.RowIndex].lvlname).Replace(".lvl", "");
				try {
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\lvl_{_file}.txt"), "#.*", ""));
				}
				catch {
					MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this lvl from a different folder?");
					return;
				}
				_loadedlvltemp = $@"{workingfolder}\lvl_{_file}.txt";
			}
			else
				return;

			//remove event handlers from a few controls so they don't trigger when their values change
			dropMasterLvlLeader.SelectedIndexChanged -= new EventHandler(dropMasterLvlLeader_SelectedIndexChanged);
			dropMasterLvlRest.SelectedIndexChanged -= new EventHandler(dropMasterLvlRest_SelectedIndexChanged);
			//load the selected item
			if ((string)_load["obj_type"] == "SequinLevel") 
				LoadLvl(_load);
			else if ((string)_load["obj_type"] == "SequinGate") 
				LoadGate(_load);
			dropMasterLvlLeader.SelectedItem = _masterlvls[e.RowIndex].checkpoint_leader;
			dropMasterLvlRest.SelectedItem = _masterlvls[e.RowIndex].rest;
			btnMasterOpenRest.Enabled = dropMasterLvlRest.SelectedIndex > 0;
			//re-add event handlers
			dropMasterLvlLeader.SelectedIndexChanged += new EventHandler(dropMasterLvlLeader_SelectedIndexChanged);
			dropMasterLvlRest.SelectedIndexChanged += new EventHandler(dropMasterLvlRest_SelectedIndexChanged);
		}
		private void masterLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_dgfocus != "masterLvlList") {
				_dgfocus = "masterLvlList";
			}
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
				if (!String.IsNullOrEmpty(_masterlvls[_in].lvlname))
					masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.ResourceManager.GetObject(_masterlvls[_in].lvlname.Split('.')[1]), _masterlvls[_in].lvlname.Split('.')[0], _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
				else
					masterLvlList.Rows.Insert(_in, new object[] { Properties.Resources.ResourceManager.GetObject(_masterlvls[_in].gatename.Split('.')[1]), _masterlvls[_in].gatename.Split('.')[0], _masterlvls[_in].checkpoint, _masterlvls[_in].playplus, _masterlvls[_in].isolate });
			}
			//if action REMOVE, remove row from the master DGV
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
				masterLvlList.Rows.RemoveAt(e.OldStartingIndex);
			}

			masterLvlList.RowEnter += masterLvlList_RowEnter;
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
				//reset things to default values
				_masterlvls.Clear();
				lblMasterName.Text = "Master Editor";
				//set saved flag to true, because nothing is loaded
				SaveMaster(true);
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
                    //storing the filename in temp so it doesn't overwrite _loadedmaster in case it fails the check in LoadMaster()
                    _loadedmastertemp = ofd.FileName;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
                    LoadMaster(_load);
                    ///load stand-alone master data
                    //I have to do this here instead of in LoadMaster() because the DataSource for the dropdowns doesn't update until that method exits
                    dropMasterSkybox.Text = (string)_load["skybox_name"];
                    dropMasterIntro.Text = (string)_load["intro_lvl_name"];
                    dropMasterCheck.Text = (string)_load["checkpoint_lvl_name"];
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
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Master File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = workingfolder ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
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
			File.WriteAllText(_loadedmaster, JsonConvert.SerializeObject(_save, Formatting.Indented));
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
			_masterlvls.RemoveAt(masterLvlList.CurrentRow.Index);
			PlaySound("UIobjectremove");
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
			dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(path), "#.*", ""));
			//check if file being loaded is actually a leaf. Can do so by checking the JSON key
			if ((string)_load["obj_type"] is not "SequinLevel" and not "SequinGate") {
				MessageBox.Show("This does not appear to be a lvl or a gate file!", "File load error");
				return;
			}
			//check if lvl exists in the same folder as the master. If not, allow user to copy file.
			//this is why I utilize workingfolder
			if (Path.GetDirectoryName(path) != workingfolder) {
				if (MessageBox.Show("The item you chose does not exist in the same folder as this master. Do you want to copy it to this folder and load it?", "File load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
					File.Copy(path, $@"{workingfolder}\{Path.GetFileName(path)}");
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
					gatename = "<none>",
					rest = "<none>"
				});
			else if (_load["obj_type"] == "SequinGate")
				_masterlvls.Add(new MasterLvlData() {
					gatename = (string)_load["obj_name"],
					playplus = true,
					checkpoint = true,
					checkpoint_leader = "<none>",
					lvlname = "<none>",
					rest = "<none>"
				});
		}

		private void btnMasterLvlUp_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = masterLvlList.CurrentRow.Index;
				if (rowIndex == 0)
					return;
                //move lvl in list
                MasterLvlData selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex - 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex - 1].Cells[0].Selected = true;
			}
			catch { }
		}

		private void btnMasterLvlDown_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = masterLvlList.CurrentRow.Index;
				if (rowIndex == _masterlvls.Count - 1)
					return;
                //move lvl in list
                MasterLvlData selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex + 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex + 1].Cells[0].Selected = true;
			}
			catch { }
		}

		private void btnMasterLvlCopy_Click(object sender, EventArgs e)
		{
			MasterLvlData selectedlvl = _masterlvls[masterLvlList.CurrentRow.Index];
			clipboardmaster = selectedlvl.Clone();
			PlaySound("UIkcopy");
			btnMasterLvlPaste.Enabled = true;
		}

		private void btnMasterLvlPaste_Click(object sender, EventArgs e)
		{
			_masterlvls.Insert(masterLvlList.CurrentRow.Index + 1, clipboardmaster);
			PlaySound("UIkpaste");
		}

		private void btnConfigColor_Click(object sender, EventArgs e)
		{
			PlaySound("UIcoloropen");
			Button button = (Button)sender;
			if (colorDialog1.ShowDialog() == DialogResult.OK) {
				ColorButton(button, colorDialog1.Color);
				PlaySound("UIcolorapply");
				SaveMaster(false);
			}
		}

		private void btnMasterRefreshLvl_Click(object sender, EventArgs e)
		{
			if (workingfolder == null)
				return;
			lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList();
			lvlsinworkfolder.Add("<none>");
			lvlsinworkfolder.Sort();
            ///add lvl list as datasources to dropdowns
            object _select = dropMasterCheck.SelectedItem;
			dropMasterCheck.DataSource = lvlsinworkfolder.ToList();
			dropMasterCheck.SelectedItem = _select;

			_select = dropMasterIntro.SelectedItem;
			dropMasterIntro.DataSource = lvlsinworkfolder.ToList();
			dropMasterIntro.SelectedItem = _select;

			_select = dropMasterLvlLeader.SelectedItem;
			dropMasterLvlLeader.DataSource = lvlsinworkfolder.ToList();
			dropMasterLvlLeader.SelectedItem = _select;

			_select = dropMasterLvlRest.SelectedItem;
			dropMasterLvlRest.DataSource = lvlsinworkfolder.ToList();
			dropMasterLvlRest.SelectedItem = _select;
			SaveMaster(true);
			PlaySound("UIrefresh");
		}

		private void btnRevertMaster_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			SaveMaster(true);
			LoadMaster(masterjson);
			PlaySound("UIrevertchanges");
		}

		//buttons that click other buttons
		private void btnMasterPanelNew_Click(object sender, EventArgs e) => masternewToolStripMenuItem.PerformClick();
		//these all load a lvl
		private void btnMasterOpenIntro_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterIntro.SelectedItem.ToString());
		private void btnMasterOpenCheckpoint_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterCheck.SelectedItem.ToString());
		private void btnMasterOpenLeader_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterLvlLeader.SelectedItem.ToString());
		private void btnMasterOpenRest_Click(object sender, EventArgs e) => MasterLoadLvl(dropMasterLvlRest.SelectedItem.ToString());
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void InitializeMasterStuff()
		{
			_masterlvls.CollectionChanged += masterlvls_CollectionChanged;
		}

		public void LoadMaster(dynamic _load)
		{
			if ((string)_load["obj_type"] != "SequinMaster") {
				MessageBox.Show("This does not appear to be a master file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedmastertemp);
			_loadedmaster = _loadedmastertemp;
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
					rest = _lvl["rest_lvl_name"] == "" ? "<none>" : _lvl["rest_lvl_name"]
				});
			}
			dropMasterSkybox.SelectedIndex = dropMasterSkybox.Items.IndexOf((string)_load["skybox_name"] == "" ? "<none>" : (string)_load["skybox_name"]);
			dropMasterIntro.SelectedIndex = dropMasterIntro.Items.IndexOf((string)_load["intro_lvl_name"] == "" ? "<none>" : (string)_load["intro_lvl_name"]);
			dropMasterCheck.SelectedIndex = dropMasterCheck.Items.IndexOf((string)_load["checkpoint_lvl_name"] == "" ? "<none>" : (string)_load["checkpoint_lvl_name"]);
			///load Config data (if file exists)
			LoadConfig();
			///set save flag (master just loaded, has no changes)
			SaveMaster(true);
			masterjson = _load;
			btnRevertMaster.Enabled = true;
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
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\lvl_{_file}.txt"), "#.*", ""));
				}
				catch {
					MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this leaf from a different folder?");
					return;
				}
				_loadedlvltemp = $@"{workingfolder}\lvl_{_file}.txt";
				//load the selected lvl
				LoadLvl(_load);
			}
		}

		public void SaveMaster(bool save, bool playsound = false)
		{
			//make the beeble emote
			pictureBox1_Click(null, null);

			_savemaster = save;
			if (!save) {
				btnSaveMaster.Enabled = true;
				btnRevertMaster.Enabled = true;
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
                { "skybox_name", dropMasterSkybox.Text },
                { "intro_lvl_name", dropMasterIntro.Text.Replace("<none>", "") }
            };
            JArray groupings = new();
			foreach (MasterLvlData group in _masterlvls) {
                JObject s = new() {
                    { "lvl_name", group.lvlname ?? "" },
                    { "gate_name", group.gatename ?? "" },
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
            string[] _files = Directory.GetFiles(workingfolder, "config_*.txt");
			foreach (string s in _files)
				File.Delete(s);
			File.WriteAllText($@"{workingfolder}\config_{Path.GetFileName(workingfolder)}.txt", JsonConvert.SerializeObject(_config, Formatting.Indented));

			///only need to return _save, since _config is written already
			return _save;
		}
		#endregion
	}
}