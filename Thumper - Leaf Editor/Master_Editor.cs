using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _savemaster = true;
		string _loadedmaster {
			get { return loadedmaster; }
			set
			{
				if (loadedmaster != value) {
					loadedmaster = value;
					MasterEditorVisible();
				}
			}
		}
		private string loadedmaster;
		string _loadedmastertemp;

		ObservableCollection<MasterLvlData> _masterlvls = new ObservableCollection<MasterLvlData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS ///
		///         ///

		/// DGV MASTERLVLLIST
		//Row Enter (load the selected lvl)
		private void masterLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				string _file = (_masterlvls[e.RowIndex].lvlname).Replace(".lvl", "");
				dynamic _load;
				try {
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\lvl_{_file}.txt"), "#.*", ""));
				}
				catch {
					MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this leaf from a different folder?");
					return;
				}

				_loadedlvltemp = $@"{workingfolder}\lvl_{_file}.txt";
				LoadLvl(_load);

				//load lvl specific things into dropdown menues
				dropMasterLvlLeader.SelectedItem = _masterlvls[e.RowIndex].checkpoint_leader;
				dropMasterLvlRest.SelectedItem = _masterlvls[e.RowIndex].rest;
			}
		}
		//Cell value changed (for checkboxes)
		private void masterLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				_masterlvls[e.RowIndex].checkpoint = bool.Parse(masterLvlList[1, e.RowIndex].Value.ToString());
				_masterlvls[e.RowIndex].playplus = bool.Parse(masterLvlList[2, e.RowIndex].Value.ToString());
			} catch { }
		}

		public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear dgv
			masterLvlList.RowCount = 0;
			//repopulate dgv from list
			masterLvlList.RowEnter -= masterLvlList_RowEnter;
			for (int x = 0; x < _masterlvls.Count; x++) {
				masterLvlList.Rows.Add(new object[] { _masterlvls[x].lvlname, _masterlvls[x].checkpoint, _masterlvls[x].playplus });
			}
			masterLvlList.RowEnter += masterLvlList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnMasterLvlDelete.Enabled = _masterlvls.Count > 0;
			btnMasterLvlUp.Enabled = _masterlvls.Count > 1;
			btnMasterLvlDown.Enabled = _masterlvls.Count > 1;

			//set lvl save flag to false
			SaveMaster(false);
		}

		private void masternewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savemaster && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savemaster) {
				//reset things to default values
				_masterlvls.Clear();
				lblMasterName.Text = "Master Editor";
				//set saved flag to true, because nothing is loaded
				SaveMaster(true);
				mastersaveAsToolStripMenuItem.PerformClick();
			}
		}

		private void masteropenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savemaster && MessageBox.Show("Current Master is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savemaster) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Master File (*.txt)|*.txt";
					ofd.Title = "Load a Thumper Master file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedmastertemp = ofd.FileName;
						//load json from file into _load. The regex strips any comments from the text.
						var _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadMaster(_load);
					}
				}
			}
		}

		private void mastersaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if _loadedmaster is somehow not set, force Save As instead
			if (_loadedmaster.Length < 1) {
				mastersaveAsToolStripMenuItem.PerformClick();
				return;
			}
			//write contents direct to file without prompting save dialog
			var _save = MasterBuildSave();
			File.WriteAllText(_loadedmaster, JsonConvert.SerializeObject(_save, Formatting.Indented));
			SaveMaster(true);
			lblMasterName.Text = $"Master Editor - sequin.master";
		}

		private void mastersaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Master File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//separate path and filename
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					//get contents to save
					var _save = MasterBuildSave();
					//serialize JSON object to a string, and write it to the file
					sfd.FileName = $@"{storePath}\master_sequin.txt";
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save, Formatting.Indented));
					//set a few visual elements to show what file is being worked on
					lblMasterName.Text = $"Master Editor - sequin.master";
					_loadedmaster = sfd.FileName;
					workingfolder = Path.GetDirectoryName(_loadedmaster);
					//set save flag
					SaveMaster(true);
				}
			}
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnMasterLvlDelete_Click(object sender, EventArgs e) => _masterlvls.RemoveAt(masterLvlList.CurrentRow.Index);
		private void btnMasterLvlAdd_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Lvl File (*.txt)|*.txt";
				ofd.Title = "Load a Thumper Lvl file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					//parse leaf to JSON
					dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
					//check if file being loaded is actually a leaf. Can do so by checking the JSON key
					if ((string)_load["obj_type"] != "SequinLevel") {
						MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
						return;
					}
					//check if lvl exists in the same folder as the master. If not, allow user to copy file.
					//this is why I utilize workingfolder
					
					if (Path.GetDirectoryName(ofd.FileName) != workingfolder) {
						if (MessageBox.Show("The lvl you chose does not exist in the same folder as this master. Do you want to copy it to this folder and load it?", "Lvl load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
							File.Copy(ofd.FileName, $@"{workingfolder}\{Path.GetFileName(ofd.FileName)}");
						else
							return;
					}
					//add leaf data to the list
					_masterlvls.Add(new MasterLvlData() {
						lvlname = (string)_load["obj_name"],
						playplus = true,
						checkpoint = true
					});
				}
			}
		}

		private void btnMasterLvlUp_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = masterLvlList.CurrentRow.Index;
				if (rowIndex == 0)
					return;
				//move leaf in list
				var selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex - 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex - 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveMaster(false);
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
				//move leaf in list
				var selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex + 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex + 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveMaster(false);
			}
			catch { }
		}

		private void btnMasterPanelNew_Click(object sender, EventArgs e)
		{
			masternewToolStripMenuItem.PerformClick();
		}

		private void btnMasterPanelOpen_Click(object sender, EventArgs e)
		{
			masteropenToolStripMenuItem.PerformClick();
		}
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void InitializeMasterStuff()
		{
			_masterlvls.CollectionChanged += masterlvls_CollectionChanged;

			///customize Lvl List a bit
			masterLvlList.ColumnCount = 1;
			masterLvlList.RowHeadersVisible = false;
			masterLvlList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 15, GraphicsUnit.Pixel)
			};
			masterLvlList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			masterLvlList.Columns[0].HeaderText = "Lvl";
			masterLvlList.Columns[0].ReadOnly = true;
			///
			///add checkbox column for denoting if a lvl has a checkpoint or not
			DataGridViewCheckBoxColumn _dgvmastercheckpoint = new DataGridViewCheckBoxColumn() {
				HeaderText = "Checkpoint",
				ReadOnly = false
			};
			masterLvlList.Columns.Add(_dgvmastercheckpoint);
			///add checkbox column for denoting if a lvl shows in play+ or not
			DataGridViewCheckBoxColumn _dgvmasterplayplus = new DataGridViewCheckBoxColumn() {
				HeaderText = "Play+",
				ReadOnly = false
			};
			masterLvlList.Columns.Add(_dgvmasterplayplus);
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
					checkpoint_leader = _lvl["checkpoint_leader_lvl_name"],
					rest = _lvl["rest_lvl_name"]
				});
			}
			///load stand-alone master data
			dropMasterSkybox.SelectedItem = _load["skybox_name"];
			dropMasterIntro.SelectedItem = _load["intro_lvl_name"];
			dropMasterCheck.SelectedItem = _load["checkpoint_lvl_name"];
			///set save flag (master just loaded, has no changes)
			SaveMaster(true);
			///select the first lvl
			if (_masterlvls.Count > 0)
				masterLvlList_RowEnter(null, new DataGridViewCellEventArgs(0, 0));
		}

		public void MasterEditorVisible()
		{
			if (workingfolder != null) {
				foreach (Control c in panelMaster.Controls)
					c.Visible = true;
				btnMasterPanelNew.Visible = false;
				btnMasterPanelOpen.Visible = false;
			}
		}

		public void SaveMaster(bool save)
		{
			_savemaster = save;
			if (!save) {
				if (!lblMasterName.Text.Contains("(unsaved)"))
					lblMasterName.Text += " (unsaved)";
			}
			else {
				lblMasterName.Text = lblMasterName.Text.Replace(" (unsaved)", "");
			}
		}

		public JObject MasterBuildSave()
		{
			JObject _save = new JObject();
			_save.Add("obj_type", "SequinMaster");
			_save.Add("obj_name", "sequin.master");
			_save.Add("skybox_name", dropMasterSkybox.SelectedText);
			_save.Add("intro_lvl_name", dropMasterIntro.SelectedText);
			JArray groupings = new JArray();
			foreach (MasterLvlData group in _masterlvls) {
				JObject s = new JObject();
				s.Add("lvl_name", group.lvlname ?? "");
				s.Add("gate_name", group.gatename ?? "");
				s.Add("checkpoint", group.checkpoint.ToString());
				s.Add("checkpoint_leader_lvl_name", group.checkpoint_leader ?? "");
				s.Add("rest_lvl_name", group.rest ?? "");
				s.Add("play_plus", group.playplus.ToString());

				groupings.Add(s);
			}
			_save.Add("groupings", groupings);
			_save.Add("checkpoint_lvl_name", dropMasterCheck.SelectedText);
			return _save;
		}
		#endregion
	}
}