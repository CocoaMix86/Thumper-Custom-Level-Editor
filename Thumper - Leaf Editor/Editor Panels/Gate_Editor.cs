using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _savegate = true;
		string _loadedgate
		{
			get { return loadedgate; }
			set {
				if (value == null) {
					ResetGate();
				}
				if (loadedgate != value) {
					if (loadedgate != null && lockedfiles.ContainsKey(loadedgate)) {
						lockedfiles[loadedgate].Close();
						lockedfiles.Remove(loadedgate);
					}
					loadedgate = value;
					ShowPanel(true, panelGate);
                    PanelEnableState(panelGate, true);
					dropGateSection.SelectedIndex = 0;

                    if (!File.Exists(loadedgate)) {
						File.WriteAllText(loadedgate, "");
					}
					lockedfiles.Add(loadedgate, new FileStream(loadedgate, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
				}
			}
		}
		private string loadedgate;
		readonly string[] node_name_hash = new string[] { "0c3025e2", "27e9f06d", "3c5c8436", "3428c8e3" };
        readonly List<BossData> bossdata = new() {
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
			new BossData() {boss_name = "Level 9 - fractal", boss_spn = "boss_frac.spn", boss_ent = "boss_gate_pellet.ent"},
			new BossData() {boss_name = "Level 9 - crakhed",  boss_spn = "crakhed9.spn", boss_ent = "crakhed.ent"},
			new BossData() {boss_name = "Level 9 - pyramid",  boss_spn = "pyramidboss.spn", boss_ent = "crakhed.ent"}
		};
		readonly List<string> _bucket0 = new() { "33caad90", "418d18a1", "1e84f4f0", "2e1b70cf" };
        readonly List<string> _bucket1 = new() { "41561eda", "347eebcb", "f8192c30", "0c9ddd9e" };
        readonly List<string> _bucket2 = new() { "fe617306", "3ee2811c", "d4f56308", "092f1784" };
		readonly List<string> _bucket3 = new() { "e790cc5a", "df4d10ff", "e7bc30f7", "1f30e67f" };
		readonly Dictionary<string, string> gatesentrynames = new() { { "SENTRY_NONE", "None" }, { "SENTRY_SINGLE_LANE", "Single Lane" }, { "SENTRY_MULTI_LANE", "Multi Lane" } };
		dynamic gatejson;
        readonly ObservableCollection<GateLvlData> _gatelvls = new();
		#endregion

		#region EventHandlers
		///        ///
		/// EVENTS ///
		///        ///

		private void gateLvlList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			//Do nothing if not selecting the lvl name
			if (e.ColumnIndex != 0 || e.RowIndex == -1)
				return;
			dynamic _load;
			//gateLvlList_RowEnter(sender, e);
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				string _file = (_gatelvls[e.RowIndex].lvlname).Replace(".lvl", "");
				if (File.Exists($@"{workingfolder}\lvl_{_file}.txt")) {
					_load = LoadFileLock($@"{workingfolder}\lvl_{_file}.txt");
				}
				else {
					MessageBox.Show("This lvl does not exist in the Level folder.");
					return;
				}
				//load the selected lvl
				LoadLvl(_load, $@"{workingfolder}\lvl_{_file}.txt");
			}
		}
		private void gateLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1)
				return;
			if (_dgfocus != "gateLvlList") {
				_dgfocus = "gateLvlList";
			}
		}

		private void gateLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				_gatelvls[e.RowIndex].sentrytype = (string)gateLvlList[1, e.RowIndex].Value;
				_gatelvls[e.RowIndex].bucket = int.Parse((string)gateLvlList[2, e.RowIndex].Value);
				SaveGate(false);
			} catch { }
		}

		public void gatelvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear dgv
			gateLvlList.RowCount = 0;
			//repopulate dgv from list
			gateLvlList.RowEnter -= gateLvlList_RowEnter;
			foreach (GateLvlData _lvl in _gatelvls) {
				gateLvlList.Rows.Add(new object[] { _lvl.lvlname, _lvl.sentrytype, _lvl.bucket.ToString() });
				//gateLvlList.Rows[_gatelvls.IndexOf(_lvl)].HeaderCell.Value = $"Phase {_gatelvls.IndexOf(_lvl) + 1}";
			}
			gateLvlList.RowEnter += gateLvlList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnGateLvlDelete.Enabled = _gatelvls.Count > 0;
			btnGateLvlUp.Enabled = _gatelvls.Count > 1;
			btnGateLvlDown.Enabled = _gatelvls.Count > 1;

			//set lvl save flag to false
			SaveGate(false);
		}

		private void gatenewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savegate && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savegate) {
				gatesaveAsToolStripMenuItem_Click(null, null);
			}
		}

		private void gateopenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savegate && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savegate) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Gate File (*.txt)|gate_*.txt";
                ofd.Title = "Load a Thumper Gate file";
                ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    string filepath = CopyToWorkingFolderCheck(ofd.FileName);
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = LoadFileLock(filepath);
                    LoadGate(_load, filepath);
                }
            }
		}
		///SAVE
		private void gatesaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if _loadedgate is somehow not set, force Save As instead
			if (_loadedgate == null) {
				gatesaveAsToolStripMenuItem.PerformClick();
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
            sfd.Filter = "Thumper Gate File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = workingfolder;
            if (sfd.ShowDialog() == DialogResult.OK) {
				if (sender == null)
					ResetGate();
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
                _loadedgate = $@"{storePath}\gate_{tempFileName}";
                WriteGate();
                //after saving new file, refresh the workingfolder
                btnWorkRefresh.PerformClick();
            }
        }
		private void WriteGate()
		{
            //write contents direct to file without prompting save dialog
            JObject _save = GateBuildSave(Path.GetFileName(_loadedgate).Replace("gate_", ""));
			if (!lockedfiles.ContainsKey(_loadedgate)) {
				lockedfiles.Add(_loadedgate, new FileStream(_loadedgate, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
			}
			WriteFileLock(lockedfiles[loadedgate], _save);
			SaveGate(true, true);
			lblGateName.Text = $"Gate Editor ⮞ {_save["obj_name"]}";
		}

		/// All dropdowns of Gate Editor call this
		private void dropGateBoss_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((sender as ComboBox).Name == "dropGateBoss" && dropGateBoss.Text.Contains("pyramid")) {
				MessageBox.Show("Pyramid requires 5 phases to function. 4 for the fight, 1 for the death sequence, otherwise the level will crash.", "Gate Info");
            }
			SaveGate(false);
		}
		private void dropGatePre_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnGateOpenPre.Enabled = dropGatePre.SelectedIndex > 0;
			SaveGate(false);
		}
		private void dropGatePost_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnGateOpenPost.Enabled = dropGatePost.SelectedIndex > 0;
			SaveGate(false);
		}
		private void dropGateRestart_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnGateOpenRestart.Enabled = dropGateRestart.SelectedIndex > 0;
			SaveGate(false);
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
                todelete.Add(_gatelvls[dgvr.Index]);
            }
            int _in = gateLvlList.CurrentRow.Index;
            foreach (GateLvlData gld in todelete)
                _gatelvls.Remove(gld);
            PlaySound("UIobjectremove");
            gateLvlList_CellClick(null, new DataGridViewCellEventArgs(1, _in >= _masterlvls.Count ? _in - 1 : _in));
        }
		private void btnGateLvlAdd_Click(object sender, EventArgs e)
		{
			//don't load new lvl if gate has 4 phases
			if (_gatelvls.Count == 4 && bossdata[dropGateBoss.SelectedIndex].boss_spn != "pyramidboss.spn" && !checkGateRandom.Checked) {
				MessageBox.Show("You can only add 4 phases to a boss (each lvl is a phase).", "Gate Info");
				return;
			}
			if (_gatelvls.Count == 5 && bossdata[dropGateBoss.SelectedIndex].boss_spn == "pyramidboss.spn") {
				MessageBox.Show("Pyramid requires only 5 phases (each lvl is a phase).", "Gate Info");
				return;
			}
            //show file dialog
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Gate File (*.txt)|lvl_*.txt";
            ofd.Title = "Load a Thumper Lvl file";
            ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
				if ((_gatelvls.Count == 4 && bossdata[dropGateBoss.SelectedIndex].boss_spn != "pyramidboss.spn" && !checkGateRandom.Checked) || (_gatelvls.Count == 5 && bossdata[dropGateBoss.SelectedIndex].boss_spn == "pyramidboss.spn"))
					return;
                //parse leaf to JSON
                dynamic _load = LoadFileLock(ofd.FileName);
                //check if file being loaded is actually a leaf. Can do so by checking the JSON key
                if ((string)_load["obj_type"] != "SequinLevel") {
                    MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
                    return;
                }
                //add leaf data to the list
                _gatelvls.Add(new GateLvlData() {
                    lvlname = (string)_load["obj_name"],
                    sentrytype = "None"
                });
                PlaySound("UIobjectadd");
            }
        }

		private void btnGateLvlUp_Click(object sender, EventArgs e)
		{
            List<int> selectedrows = gateLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                _gatelvls.Insert(dgvr - 1, _gatelvls[dgvr]);
                _gatelvls.RemoveAt(dgvr + 1);
            }
            gateLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                gateLvlList.Rows[dgvr - 1].Cells[1].Selected = true;
            }
            SaveGate(false);
        }

		private void btnGateLvlDown_Click(object sender, EventArgs e)
        {
            List<int> selectedrows = gateLvlList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().Select(x => x.Index).ToList();
            if (selectedrows.Any(r => r == 0))
                return;
            selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
            foreach (int dgvr in selectedrows) {
                _gatelvls.Insert(dgvr + 2, _gatelvls[dgvr]);
                _gatelvls.RemoveAt(dgvr);
            }
            gateLvlList.ClearSelection();
            foreach (int dgvr in selectedrows) {
                gateLvlList.Rows[dgvr + 1].Cells[1].Selected = true;
            }
            SaveGate(false);
        }

		private void btnGateRefresh_Click(object sender, EventArgs e)
		{
			if (workingfolder == null)
				return;
			lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList();
			lvlsinworkfolder.Add("");
			lvlsinworkfolder.Sort();
            ///add lvl list as datasources to dropdowns
            object _select = dropGatePre.SelectedItem;
			dropGatePre.DataSource = lvlsinworkfolder.ToList();
			dropGatePre.SelectedItem = _select;

			_select = dropGatePost.SelectedItem;
			dropGatePost.DataSource = lvlsinworkfolder.ToList();
			dropGatePost.SelectedItem = _select;

			_select = dropGateRestart.SelectedItem;
			dropGateRestart.DataSource = lvlsinworkfolder.ToList();
			dropGateRestart.SelectedItem = _select;
			SaveGate(true);
			PlaySound("UIrefresh");
		}

		private void btnRevertGate_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			SaveGate(true);
			LoadGate(gatejson, loadedgate);
			PlaySound("UIrevertchanges");
		}

		//buttons that click other buttons
		private void btnGatePanelNew_Click(object sender, EventArgs e) => gatenewToolStripMenuItem.PerformClick();
		//I use MasterLoadLvl on these because it's the exact same code to load a lvl
		private void btnGateOpenPre_Click(object sender, EventArgs e) => MasterLoadLvl(dropGatePre.Text);
		private void btnGateOpenPost_Click(object sender, EventArgs e) => MasterLoadLvl(dropGatePost.Text);
		private void btnGateOpenRestart_Click(object sender, EventArgs e) => MasterLoadLvl(dropGateRestart.Text);

		private void checkGateRandom_CheckedChanged(object sender, EventArgs e)
		{
			dgvGateBucket.Visible = checkGateRandom.Checked;
			dropGateBoss.Enabled = !checkGateRandom.Checked;
			dropGateBoss.SelectedItem = bossdata.Where(x => x.boss_name == "Level 6 - spirograph").First();
			SaveGate(false);
		}
		#endregion

		#region Methods
		///         ///
		/// Methods ///
		///         ///

		public void InitializeGateStuff()
		{
			_gatelvls.CollectionChanged += gatelvls_CollectionChanged;

			///add boss data to dropdown
			dropGateBoss.DisplayMember = "boss_name";
			dropGateBoss.ValueMember = "boss_spn";
			dropGateBoss.DataSource = bossdata;
			dropGateSection.SelectedIndex = 0;
			//
			dropGateSection.SelectedIndex = -1;
			SaveGate(true);
		}

		public void LoadGate(dynamic _load, string filepath)
		{
			if (_load == null)
				return;
			//if Gate Editor is hidden, show it when selecting a gate
			if (panelGate.Visible == false)
				gateEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Gate or not
			if ((string)_load["obj_type"] != "SequinGate") {
				MessageBox.Show("This does not appear to be a gate file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(filepath);
            //check if the assign actually worked. If not, stop loading.
            if (workingfolder != Path.GetDirectoryName(filepath))
                return;
            _loadedgate = filepath;
			//set some visual elements
			lblGateName.Text = $"Gate Editor ⮞ {_load["obj_name"]}";

			///Clear form elements so new data can load
			_gatelvls.Clear();
			///load lvls associated with this master
			gateLvlList.CellValueChanged -= gateLvlList_CellValueChanged;
			foreach (dynamic _lvl in _load["boss_patterns"]) {
				_gatelvls.Add(new GateLvlData() {
					lvlname = _lvl["lvl_name"],
					//sentrytype = ((string)_lvl["sentry_type"]).Replace("SENTRY_", "").Replace("_", " ").ToLower().ToTitleCase(),
					sentrytype = gatesentrynames[(string)_lvl["sentry_type"]],
					bucket = _lvl["bucket_num"]
				});
			}
			gateLvlList.CellValueChanged += gateLvlList_CellValueChanged;

			//populate dropdowns
			checkGateRandom.Checked = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
			dropGateBoss.SelectedValue = (string)_load["spn_name"];
			dropGatePre.SelectedItem = (string)_load["pre_lvl_name"] == "" ? "<none>" : (string)_load["pre_lvl_name"];
			dropGatePost.SelectedItem = (string)_load["post_lvl_name"] == "" ? "<none>" : (string)_load["post_lvl_name"];
			dropGateRestart.SelectedItem = (string)_load["restart_lvl_name"] == "" ? "<none>" : (string)_load["restart_lvl_name"];
			dropGateSection.SelectedItem = (string)_load["section_type"];

			///set save flag (gate just loaded, has no changes)
			gatejson = _load;
			SaveGate(true);
		}

		public void SaveGate(bool save, bool playsound = false)
		{
			//make the beeble emote
			pictureBox1_Click(null, null);

			_savegate = save;
			if (!save) {
				btnSaveGate.Enabled = true;
				btnRevertGate.Enabled = gatejson != null;
				btnRevertGate.ToolTipText = gatejson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
				toolstripTitleGate.BackColor = Color.Maroon;
			}
			else {
				btnSaveGate.Enabled = false;
				btnRevertGate.Enabled = false;
				toolstripTitleGate.BackColor = Color.FromArgb(40, 40, 40);
				if (playsound) PlaySound("UIsave");
			}
		}

		public JObject GateBuildSave(string _gatename)
		{
			int bucket0 = 0;
			int bucket1 = 0;
			int bucket2 = 0;
			int bucket3 = 0;
			_gatename = _gatename.Replace(".txt", ".gate");
			///being build Master JSON object
			JObject _save = new() {
				{ "obj_type", "SequinGate" },
				{ "obj_name", _gatename },
				{ "spn_name", dropGateBoss.SelectedValue.ToString() },
				{ "param_path", bossdata[dropGateBoss.SelectedIndex].boss_ent },
				{ "pre_lvl_name", dropGatePre.Text.Replace("<none>", "") },
				{ "post_lvl_name", dropGatePost.Text.Replace("<none>", "") },
				{ "restart_lvl_name", dropGateRestart.Text.Replace("<none>", "") },
				{ "section_type", dropGateSection.Text },
				{ "random_type", $"LEVEL_RANDOM_{(checkGateRandom.Checked ? "BUCKET" : "NONE")}" }
			};
			//setup boss_patterns
			JArray boss_patterns = new();
			//int lvlcount = checkGateRandom.Checked ? _gatelvls.Count : _save["spn_name"].ToString().Contains("pyramid") ? 5 : 4;
			for (int x = 0; x < _gatelvls.Count; x++) {
				JObject s = new() {
					{ "lvl_name", _gatelvls[x].lvlname },
					//{ "sentry_type", $"SENTRY_{_gatelvls[x].sentrytype.ToUpper().Replace(' ', '_')}" },
					{ "sentry_type", $"{gatesentrynames.First(e => e.Value == _gatelvls[x].sentrytype).Key}"},
					{ "bucket_num", _gatelvls[x].bucket }
				};
				//if using RANDOM, the buckets and hashes are all different per entry in each bucket
				if (checkGateRandom.Checked) {
					switch (_gatelvls[x].bucket) {
						case 0:
							s.Add("node_name_hash", _bucket0[bucket0]);
							bucket0++;
							break;
						case 1:
							s.Add("node_name_hash", _bucket1[bucket1]);
							bucket1++;
							break;
						case 2:
							s.Add("node_name_hash", _bucket2[bucket2]);
							bucket2++;
							break;
						case 3:
							s.Add("node_name_hash", _bucket3[bucket3]);
							bucket3++;
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
			loadedgate = null;
			_gatelvls.Clear();
			gateLvlList.Rows.Clear();
			checkGateRandom.Checked = false;
			dropGatePre.SelectedIndex = 0;
			dropGatePost.SelectedIndex = 0;
			dropGateRestart.SelectedIndex = 0;
			lblGateName.Text = "Gate Editor";
			//set saved flag to true, because nothing is loaded
			SaveGate(true);
		}
		#endregion
	}
}