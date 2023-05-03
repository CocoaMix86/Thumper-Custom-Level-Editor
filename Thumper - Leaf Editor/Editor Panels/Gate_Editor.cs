using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
	{
		#region Variables
		bool _savegate = true;
		string _loadedgate
		{
			get { return loadedgate; }
			set {
				if (loadedgate != value) {
					loadedgate = value;
					GateEditorVisible(true);
					gatesaveAsToolStripMenuItem.Enabled = true;
					gatesaveToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string loadedgate;
		string _loadedgatetemp;
		readonly string[] node_name_hash = new string[] { "0c3025e2", "27e9f06d", "3c5c8436", "3428c8e3" };
		List<BossData> bossdata = new List<BossData> {
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
			new BossData() {boss_name = "Level 9 - pyramid",  boss_spn = "pyramid.spn", boss_ent = "crakhed.ent"}
		};
		ObservableCollection<GateLvlData> _gatelvls = new ObservableCollection<GateLvlData>();
		#endregion

		#region EventHandlers
		///        ///
		/// EVENTS ///
		///        ///

		private void gateLvlList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			//Do nothing if not selecting the lvl name
			if (e.ColumnIndex != 0)
				return;
			//gateLvlList_RowEnter(sender, e);
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				string _file = (_gatelvls[e.RowIndex].lvlname).Replace(".lvl", "");
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
		private void gateLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_dgfocus != "gateLvlList") {
				_dgfocus = "gateLvlList";
			}
		}

		private void gateLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				_gatelvls[e.RowIndex].sentrytype = gateLvlList[1, e.RowIndex].Value.ToString();
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
				gateLvlList.Rows.Add(new object[] { _lvl.lvlname, _lvl.sentrytype });
				gateLvlList.Rows[_gatelvls.IndexOf(_lvl)].HeaderCell.Value = $"Phase {_gatelvls.IndexOf(_lvl) + 1}";
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
				//reset things to default values
				_gatelvls.Clear();
				lblGateName.Text = "Gate Editor";
				//set saved flag to true, because nothing is loaded
				SaveGate(true);
				if (e != null)
					gatesaveAsToolStripMenuItem_Click(null, null);
			}
		}

		private void gateopenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savegate && MessageBox.Show("Current Gate is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savegate) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Gate File (*.txt)|gate_*.txt";
					ofd.Title = "Load a Thumper Gate file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						//storing the filename in temp so it doesn't overwrite _loadedgate in case it fails the check in LoadGate()
						_loadedgatetemp = ofd.FileName;
						//load json from file into _load. The regex strips any comments from the text.
						dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadGate(_load);
					}
				}
			}
		}

		private void gatesaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if _loadedgate is somehow not set, force Save As instead
			if (_loadedgate == null) {
				gatesaveAsToolStripMenuItem.PerformClick();
				return;
			}
			//write contents direct to file without prompting save dialog
			var _save = GateBuildSave(Path.GetFileName(_loadedgate).Replace("gate_", ""));
			File.WriteAllText(_loadedgate, JsonConvert.SerializeObject(_save, Formatting.Indented));
			SaveGate(true);
			lblGateName.Text = $"Gate Editor - {_save["obj_name"]}";
		}

		private void gatesaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Gate File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				sfd.InitialDirectory = workingfolder;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//separate path and filename
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					//check if user input "gate_", and deny save if so
					if (Path.GetFileName(sfd.FileName).Contains("gate_")) {
						MessageBox.Show("File not saved. Do not include 'gate_' in your file name.", "File not saved");
						return;
					}
					//get contents to save
					var _save = GateBuildSave(Path.GetFileName(sfd.FileName));
					//serialize JSON object to a string, and write it to the file
					sfd.FileName = $@"{storePath}\gate_{tempFileName}";
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save, Formatting.Indented));
					//set a few visual elements to show what file is being worked on
					lblGateName.Text = $"Gate Editor - {_save["obj_name"]}";
					workingfolder = Path.GetDirectoryName(sfd.FileName);
					_loadedgate = sfd.FileName;
					//set save flag
					SaveGate(true);
				}
			}
		}

		/// All dropdowns of Gate Editor call this
		private void dropGateBoss_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (bossdata[dropGateBoss.SelectedIndex].boss_spn == "pyramid.spn")
				lblPyramidWarn.Visible = true;
			else
				lblPyramidWarn.Visible = false;
			SaveGate(false);
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnGateLvlDelete_Click(object sender, EventArgs e) => _gatelvls.RemoveAt(gateLvlList.CurrentRow.Index);
		private void btnGateLvlAdd_Click(object sender, EventArgs e)
		{
			//don't load new lvl if gate has 4 phases
			if (_gatelvls.Count == 4 && bossdata[dropGateBoss.SelectedIndex].boss_spn != "pyramid.spn") {
				MessageBox.Show("You can only add 4 phases to a boss.", "Gate Info");
				return;
			}
			if (_gatelvls.Count == 5 && bossdata[dropGateBoss.SelectedIndex].boss_spn == "pyramid.spn") {
				MessageBox.Show("Pyramid requires only 5 phases.", "Gate Info");
				return;
			}
			//show file dialog
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Gate File (*.txt)|lvl_*.txt";
				ofd.Title = "Load a Thumper Lvl file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					//parse leaf to JSON
					dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
					//check if file being loaded is actually a leaf. Can do so by checking the JSON key
					if ((string)_load["obj_type"] != "SequinLevel") {
						MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
						return;
					}
					//check if lvl exists in the same folder as the gate. If not, allow user to copy file.
					//this is why I utilize workingfolder
					/*if (Path.GetDirectoryName(ofd.FileName) != workingfolder) {
						if (MessageBox.Show("The lvl you chose does not exist in the same folder as this gate. Do you want to copy it to this folder and load it?", "Lvl load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
							File.Copy(ofd.FileName, $@"{workingfolder}\{Path.GetFileName(ofd.FileName)}");
						else
							return;
					}*/
					//add leaf data to the list
					_gatelvls.Add(new GateLvlData() {
						lvlname = (string)_load["obj_name"],
						sentrytype = "SENTRY_NONE"
					});
				}
			}
		}

		private void btnGateLvlUp_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = gateLvlList.CurrentRow.Index;
				if (rowIndex == 0)
					return;
				//move leaf in list
				var selectedLvl = _gatelvls[rowIndex];
				_gatelvls.Remove(selectedLvl);
				_gatelvls.Insert(rowIndex - 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				gateLvlList.Rows[rowIndex - 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveGate(false);
			}
			catch { }
		}

		private void btnGateLvlDown_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = gateLvlList.CurrentRow.Index;
				if (rowIndex == _gatelvls.Count - 1)
					return;
				//move lvl in list
				var selectedLvl = _gatelvls[rowIndex];
				_gatelvls.Remove(selectedLvl);
				_gatelvls.Insert(rowIndex + 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				gateLvlList.Rows[rowIndex + 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveGate(false);
			}
			catch { }
		}

		private void btnGateRefresh_Click(object sender, EventArgs e)
		{
			lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList();
			lvlsinworkfolder.Add("");
			lvlsinworkfolder.Sort();
			///add lvl list as datasources to dropdowns
			var _select = dropGatePre.SelectedItem;
			dropGatePre.DataSource = lvlsinworkfolder.ToList();
			dropGatePre.SelectedItem = _select;

			_select = dropGatePost.SelectedItem;
			dropGatePost.DataSource = lvlsinworkfolder.ToList();
			dropGatePost.SelectedItem = _select;

			_select = dropGateRestart.SelectedItem;
			dropGateRestart.DataSource = lvlsinworkfolder.ToList();
			dropGateRestart.SelectedItem = _select;
		}

		//buttons that click other buttons
		private void btnGatePanelNew_Click(object sender, EventArgs e) => gatenewToolStripMenuItem.PerformClick();
		private void btnGatePanelOpen_Click(object sender, EventArgs e) => gateopenToolStripMenuItem.PerformClick();
		//I use MasterLoadLvl on these because it's the exact same code to load a lvl
		private void btnGateOpenPre_Click(object sender, EventArgs e) => MasterLoadLvl(dropGatePre.Text);
		private void btnGateOpenPost_Click(object sender, EventArgs e) => MasterLoadLvl(dropGatePost.Text);
		private void btnGateOpenRestart_Click(object sender, EventArgs e) => MasterLoadLvl(dropGateRestart.Text);
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
			//
			dropGateSection.SelectedIndex = -1;
			SaveGate(true);
		}

		public void LoadGate(dynamic _load)
		{
			//if Gate Editor is hidden, show it when selecting a gate
			if (panelGate.Visible == false)
				gateEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Gate or not
			if ((string)_load["obj_type"] != "SequinGate") {
				MessageBox.Show("This does not appear to be a gate file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedgatetemp);
			_loadedgate = _loadedgatetemp;
			//set some visual elements
			lblGateName.Text = $"Gate Editor - {_load["obj_name"]}";

			///Clear form elements so new data can load
			_gatelvls.Clear();
			///load lvls associated with this master
			foreach (dynamic _lvl in _load["boss_patterns"]) {
				_gatelvls.Add(new GateLvlData() {
					lvlname = _lvl["lvl_name"],
					sentrytype = _lvl["sentry_type"]
				});
			}

			//populate dropdowns
			dropGateBoss.SelectedValue = (string)_load["spn_name"];
			dropGatePre.SelectedItem = (string)_load["pre_lvl_name"];
			dropGatePost.SelectedItem = (string)_load["post_lvl_name"];
			dropGateRestart.SelectedItem = (string)_load["restart_lvl_name"];
			dropGateSection.SelectedItem = (string)_load["section_type"];

			///set save flag (gate just loaded, has no changes)
			SaveGate(true);
		}

		public void SaveGate(bool save)
		{
			_savegate = save;
			if (!save) {
				if (!lblGateName.Text.Contains("(unsaved)"))
					lblGateName.Text += " (unsaved)";
				btnSaveGate.Location = new Point(lblGateName.Location.X + lblGateName.Size.Width, btnSaveGate.Location.Y);
				btnSaveGate.Enabled = true;
				lblGateName.BackColor = Color.Maroon;
			}
			else {
				lblGateName.Text = lblGateName.Text.Replace(" (unsaved)", "");
				btnSaveGate.Location = new Point(lblGateName.Location.X + lblGateName.Size.Width, btnSaveGate.Location.Y);
				btnSaveGate.Enabled = false;
				lblGateName.BackColor = Color.FromArgb(40, 40, 40);
			}
		}

		public void GateEditorVisible(bool visible)
		{
			panelGate.Visible = visible;
		}

		public JObject GateBuildSave(string _gatename)
		{
			_gatename = Regex.Replace(_gatename, "[.].*", ".gate");
			///being build Master JSON object
			JObject _save = new JObject {
				{ "obj_type", "SequinGate" },
				{ "obj_name", _gatename },
				{ "spn_name", dropGateBoss.SelectedValue.ToString() },
				{ "param_path", bossdata[dropGateBoss.SelectedIndex].boss_ent },
				{ "pre_lvl_name", dropGatePre.Text },
				{ "post_lvl_name", dropGatePost.Text },
				{ "restart_lvl_name", dropGateRestart.Text },
				{ "section_type", dropGateSection.Text },
				{ "random_type", "LEVEL_RANDOM_NONE" }
			};
			//setup boss_patterns
			JArray boss_patterns = new JArray();
			for (int x = 0; x < _gatelvls.Count; x++) {
				JObject s = new JObject {
					{ "lvl_name", _gatelvls[x].lvlname },
					{ "sentry_type", _gatelvls[x].sentrytype },
					{ "bucket_num", 0 }
				};
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

				boss_patterns.Add(s);
			}
			_save.Add("boss_patterns", boss_patterns);
			return _save;
		}
		#endregion
	}
}