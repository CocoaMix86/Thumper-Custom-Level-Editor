using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
		bool _savelvl = true;
		int _lvllength;
		public string _loadedlvl {
			get { return loadedlvl; }
			set
			{
				if (loadedlvl != value) {
					loadedlvl = value;
					LvlEditorVisible();
					lvlsaveAsToolStripMenuItem.Enabled = true;
					lvlsaveToolStripMenuItem2.Enabled = true;
				}
			}
		}
		private string loadedlvl;
		string _loadedlvltemp;

		List<string> _lvlpaths = (Properties.Resources.paths).Replace("\r\n", "\n").Split('\n').ToList();
		List<string> _lvlsamples = new List<string>();

		ObservableCollection<LvlLeafData> _lvlleafs = new ObservableCollection<LvlLeafData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS  ///
		///         ///

		///DGV LVLLEAFLIST
		//Selected row changed
		private void lvlLeafList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				string _file = (_lvlleafs[e.RowIndex].leafname).Replace(".leaf", "");
				dynamic _load;
				try {
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\leaf_{_file}.txt"), "#.*", ""));
				} catch { 
					MessageBox.Show($@"Could not locate ""leaf_{_file}.txt"" in the same folder as this lvl. Did you add this leaf from a different folder?");
					return;
				}

				_loadedleaf = $@"{workingfolder}\leaf_{_file}.txt";
				LoadLeaf(_load);
				LvlUpdatePaths(e.RowIndex);
			}
		}
		///DGV LVLLEAFPATHS
		//Cell value changed
		private void lvlLeafPaths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			//if a path is set to blank, remove the row
			if (lvlLeafPaths.CurrentCell.Value.ToString() == " ")
				lvlLeafPaths.Rows.Remove(lvlLeafPaths.CurrentRow);
			//clear List storing the paths and repopulate it
			_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Clear();
			for (int x = 0; x < lvlLeafPaths.Rows.Count; x++) {
				if (lvlLeafPaths.Rows[x].Cells[0].Value != null)
					_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Add(lvlLeafPaths.Rows[x].Cells[0].Value.ToString());
			}
			//Delete button enabled/disabled if rows exist
			btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
			//set lvl save flag to false
			SaveLvl(false);
		}
		/// DGV LVLLOOPTRACKS
		//Cell value changed
		private void lvlLoopTracks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			SaveLvl(false);
		}
		/// DGV LVLSEQOBJS
		//Cell value changed
		private void lvlSeqObjs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				var _cell = lvlSeqObjs[e.ColumnIndex, e.RowIndex];
				if (!string.IsNullOrEmpty(_cell?.Value?.ToString()))
					_cell.Style.BackColor = Color.Blue;
				else if (_cell != null)
					_cell.Style = null;

				//set lvl save flag to false
				SaveLvl(false);
			}
			catch { }
		}
		//Press Delete to clear cells
		private void lvlSeqObjs_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) {
				foreach (DataGridViewCell dgvc in lvlSeqObjs.SelectedCells)
					dgvc.Value = null;
			}
			e.Handled = true;
		}
		//Press Backspace to clear cells
		private void lvlSeqObjs_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Back) {
				foreach (DataGridViewCell dgvc in lvlSeqObjs.SelectedCells)
					dgvc.Value = null;
			}
			e.Handled = true;
		}
		///_LVLLEAF - Triggers when the collection changes
		public void lvlleaf_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear dgv
			lvlLeafList.RowCount = 0;
			_lvllength = 0;
			//repopulate dgv from list
			lvlLeafList.RowEnter -= lvlLeafList_RowEnter;
			for (int x = 0; x < _lvlleafs.Count; x++) {
				lvlLeafList.Rows.Add(new object[] { _lvlleafs[x].leafname, _lvlleafs[x].beats });
				//total length of all leafs. This value is used for the volume sequencer
				_lvllength += _lvlleafs[x].beats;
			}
			//add approach to lvl length
			_lvllength += +(int)NUD_lvlApproach.Value;
			lvlLeafList.RowEnter += lvlLeafList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnLvlLeafDelete.Enabled = _lvlleafs.Count > 0;
			btnLvlLeafUp.Enabled = _lvlleafs.Count > 1;
			btnLvlLeafDown.Enabled = _lvlleafs.Count > 1;
			//enable/disable buttons if leaf exists or not
			btnLvlPathAdd.Enabled = _lvlleafs.Count > 0;
			if (btnLvlPathAdd.Enabled == false) btnLvlPathDelete.Enabled = false;
			btnLvlSeqAdd.Enabled = _lvlleafs.Count > 0;
			//set volume sequencer column total to length of all leafs + approach
			lvlSeqObjs.ColumnCount = _lvllength;
			//some styles
			GenerateColumnStyle(lvlSeqObjs, _lvllength);
			SaveLvl(false);
		}
		/// Set "saved" flag to false for LVL when these events happen
		private void NUD_lvlApproach_ValueChanged(object sender, EventArgs e) => SaveLvl(false);
		private void NUD_lvlVolume_ValueChanged(object sender, EventArgs e) => SaveLvl(false);
		private void dropLvlInput_SelectedIndexChanged(object sender, EventArgs e) => SaveLvl(false);
		private void dropLvlTutorial_SelectedIndexChanged(object sender, EventArgs e) => SaveLvl(false);
		///VOLUME SEQUENCER CELL ZOOM
		private void trackLvlVolumeZoom_Scroll(object sender, EventArgs e)
		{
			for (int i = 0; i < lvlSeqObjs.ColumnCount; i++) {
				lvlSeqObjs.Columns[i].Width = trackLvlVolumeZoom.Value;
			}
		}
		/// LVL NEW
		private void newToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			//if LVL not saved, have user confirm if they want to continue
			if ((!_savelvl && MessageBox.Show("Current LVL is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				//reset things to default values
				_lvlleafs.Clear();
				lvlSeqObjs.Rows.Clear();
				lvlLoopTracks.Rows.Clear();
				NUD_lvlApproach.Value = 16;
				NUD_lvlVolume.Value = 0.5M;
				dropLvlInput.SelectedIndex = 0;
				dropLvlTutorial.SelectedIndex = 0;
				lblLvlName.Text = "Level Editor";
				//set saved flag to true, because nothing is loaded
				SaveLvl(true);
				lvlsaveAsToolStripMenuItem.PerformClick();
			}
		}
		/// LVL SAVE
		private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			//if _loadedlvl is somehow not set, force Save As instead
			if (_loadedlvl.Length < 1) {
				lvlsaveAsToolStripMenuItem.PerformClick();
				return;
			}
			//write contents direct to file without prompting save dialog
			var _save = LvlBuildSave(Path.GetFileName(_loadedlvl).Replace("lvl_", ""));
			File.WriteAllText(_loadedlvl, JsonConvert.SerializeObject(_save));
			SaveLvl(true);
			lblLvlName.Text = $"Level Editor - {_save["obj_name"]}";
			//reload samples on save
			LvlReloadSamples();
		}
		/// LVL SAVE AS
		private void lvlsaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Editor Lvl File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//separate path and filename
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					//check if user input "lvl_", and deny save if so
					if (Path.GetFileName(sfd.FileName).Contains("lvl_")) {
						MessageBox.Show("File not saved. Do not include 'lvl_' in your file name.", "File not saved");
						return;
					}
					//get contents to save
					var _save = LvlBuildSave(Path.GetFileName(sfd.FileName));
					//serialize JSON object to a string, and write it to the file
					sfd.FileName = $@"{storePath}\lvl_{tempFileName}";
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save));
					//set a few visual elements to show what file is being worked on
					lblLvlName.Text = $"Level Editor - {_save["obj_name"]}";
					workingfolder = Path.GetDirectoryName(sfd.FileName);
					_loadedlvl = sfd.FileName;
					//set save flag
					SaveLvl(true);
				}
			}
		}
		/// LVL LOAD
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Editor Lvl File (*.txt)|*.txt";
					ofd.Title = "Load a Thumper Lvl file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						//storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
						_loadedlvltemp = ofd.FileName;
						//load json from file into _load. The regex strips any comments from the text.
						var _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadLvl(_load);
					}
				}
			}
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnLvlLeafDelete_Click(object sender, EventArgs e) => _lvlleafs.RemoveAt(lvlLeafList.CurrentRow.Index);
		private void btnLvlLeafAdd_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Leaf File (*.txt)|*.txt";
				ofd.Title = "Load a Thumper Leaf file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					//parse leaf to JSON
					dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
					//check if file being loaded is actually a leaf. Can do so by checking the JSON key
					if ((string)_load["obj_type"] != "SequinLeaf") {
						MessageBox.Show("This does not appear to be a leaf file!", "Leaf load error");
						return;
					}
					//check if leaf exists in the same folder as the lvl. If not, allow user to copy file.
					//this is why I utilize workingfolder
					if (Path.GetDirectoryName(ofd.FileName) != workingfolder) {
						if (MessageBox.Show("The leaf you chose does not exist in the same folder as this lvl. Do you want to copy it to this folder and load it?", "Leaf load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
							File.Copy(ofd.FileName, $@"{workingfolder}\{Path.GetFileName(ofd.FileName)}");
						else
							return;
					}
					//add leaf data to the list
					_lvlleafs.Add(new LvlLeafData() {
						leafname = (string)_load["obj_name"],
						beats = (int)_load["beat_cnt"],
						paths = new List<string>()
					});
				}
			}
		}

		private void btnLvlLeafUp_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = lvlLeafList.CurrentRow.Index;
				if (rowIndex == 0)
					return;
				//move leaf in list
				var selectedLeaf = _lvlleafs[rowIndex];
				_lvlleafs.Remove(selectedLeaf);
				_lvlleafs.Insert(rowIndex - 1, selectedLeaf);
				//move selected cell up a row to follow the moved item
				lvlLeafList.Rows[rowIndex - 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveLvl(false);
			}
			catch { }
		}

		private void btnLvlLeafDown_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = lvlLeafList.CurrentRow.Index;
				if (rowIndex == _lvlleafs.Count - 1)
					return;
				//move leaf in list
				var selectedLeaf = _lvlleafs[rowIndex];
				_lvlleafs.Remove(selectedLeaf);
				_lvlleafs.Insert(rowIndex + 1, selectedLeaf);
				//move selected cell up a row to follow the moved item
				lvlLeafList.Rows[rowIndex + 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveLvl(false);
			}
			catch { }
		}

		private void btnLvlPathAdd_Click(object sender, EventArgs e) => lvlLeafPaths.RowCount++;
		private void btnLvlPathDelete_Click(object sender, EventArgs e)
		{
			_lvlleafs[lvlLeafList.CurrentRow.Index].paths.RemoveAt(lvlLeafPaths.CurrentRow.Index);
			LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
		}

		private void btnLvlLoopAdd_Click(object sender, EventArgs e)
		{
			lvlLoopTracks.RowCount++;
			lvlLoopTracks.Rows[lvlLoopTracks.Rows.Count - 1].HeaderCell.Value = "Volume Track " + (lvlLoopTracks.Rows.Count - 1);
			btnLvlLoopDelete.Enabled = true;
		}

		private void btnLvlLoopDelete_Click(object sender, EventArgs e)
		{
			lvlLoopTracks.Rows.RemoveAt(lvlLoopTracks.CurrentRow.Index);
			//disable button if no more rows exist
			if (lvlLoopTracks.Rows.Count < 1)
				btnLvlLoopDelete.Enabled = false;
			//rename each header cell as the rows have moved and now are on different tracks
			foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
				r.HeaderCell.Value = "Volume Track " + r.Index;
			}
		}

		private void btnLvlSeqAdd_Click(object sender, EventArgs e)
		{
			lvlSeqObjs.RowCount++;
			lvlSeqObjs.Rows[lvlSeqObjs.Rows.Count - 1].HeaderCell.Value = "Volume Track " + (lvlSeqObjs.Rows.Count - 1);
			btnLvlSeqDelete.Enabled = true;
			btnLvlSeqClear.Enabled = true;
		}

		private void btnLvlSeqDelete_Click(object sender, EventArgs e)
		{
			bool _empty = true;
			//iterate over cells in current row. If there is a value, set bool to false and break loop
			foreach (DataGridViewCell dgvc in lvlSeqObjs.CurrentRow.Cells) {
				if (!string.IsNullOrEmpty(dgvc?.Value?.ToString())) {
					_empty = false;
					break;
				}
			}
			//prompt user to say YES if row is not empty. Then delete selected track
			if (_empty || (!_empty && MessageBox.Show("This track has data. Do you still want to delete it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)) {
				lvlSeqObjs.Rows.Remove(lvlSeqObjs.CurrentRow);
				//after deleting, rename all headers so they're in order again
				foreach (DataGridViewRow r in lvlSeqObjs.Rows)
					r.HeaderCell.Value = "Volume Track " + r.Index;
			}
			//disable buttons if there are no more rows
			btnLvlSeqDelete.Enabled = lvlSeqObjs.Rows.Count != 0;
			btnLvlSeqClear.Enabled = lvlSeqObjs.Rows.Count != 0;
		}

		private void btnLvlSeqClear_Click(object sender, EventArgs e)
		{
			bool _empty = true;
			//iterate over current row to see if any cells have data
			foreach (DataGridViewCell dgvc in lvlSeqObjs.CurrentRow.Cells) {
				if (dgvc.Value != null) {
					_empty = false;
					break;
				}
			}
			//if YES, clear cell values in row and clear highlighting
			if ((!_empty && MessageBox.Show("This track has data. Are you sure you want to clear it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
				foreach (DataGridViewCell dgvc in lvlSeqObjs.CurrentRow.Cells) {
					dgvc.Value = null;
					dgvc.Style = null;
				}
			}
		}

		private void btnLvlLoopRefresh_Click(object sender, EventArgs e)
		{
			LvlReloadSamples();
			MessageBox.Show($"Found and loaded {_lvlsamples.Count} samples for the current working folder.");
		}

		private void btnlvlPanelOpen_Click(object sender, EventArgs e)
		{
			lvlopenToolStripMenuItem.PerformClick();
		}

		private void btnlvlPanelNew_Click(object sender, EventArgs e)
		{
			lvlnewToolStripMenuItem1.PerformClick();
		}
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void LoadLvl(dynamic _load)
		{
			if ((string)_load["obj_type"] != "SequinLevel") {
				MessageBox.Show("This does not appear to be a lvl file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedlvltemp);
			_loadedlvl = _loadedlvltemp;
			//set some visual elements
			lblLvlName.Text = $@"Lvl Editor - {_load["obj_name"]}";

			///Clear DGVs so new data can load
			lvlLoopTracks.Rows.Clear();
			_lvlleafs.Clear();
			lvlLeafList.Rows.Clear();
			lvlSeqObjs.Rows.Clear();

			///populate the non-DGV elements on the form with info from the JSON
			NUD_lvlApproach.Value = (decimal)_load["approach_beats"];
			NUD_lvlVolume.Value = (decimal)_load["volume"];
			dropLvlInput.Text = (string)_load["input_allowed"];
			dropLvlTutorial.Text = (string)_load["tutorial_type"];
			///load loop track names and paths to lvlLoopTracks DGV
			LvlReloadSamples();
			foreach (dynamic samp in _load["loops"]) {
				lvlLoopTracks.Rows.Add(new object[] { (string)samp["samp_name"], (int)samp["beats_per_loop"]});
			}
			///load leafs associated with this lvl
			foreach (dynamic leaf in _load["leaf_seq"]) {
				_lvlleafs.Add(new LvlLeafData() {
					leafname = (string)leaf["leaf_name"],
					beats = (int)leaf["beat_cnt"],
					paths = leaf["sub_paths"].ToObject<List<string>>()
				});
			}
			///load volume sequencer data
			foreach (dynamic seq_obj in _load["seq_objs"]) {
				lvlSeqObjs.RowCount++;
				btnLvlSeqClear.Enabled = true;
				btnLvlSeqDelete.Enabled = true;
				LvlTrackRawImport(lvlSeqObjs.Rows[lvlSeqObjs.RowCount - 1], seq_obj["data_points"]);
			}
			///add headers to rows after importing their data
			foreach (DataGridViewRow r in lvlSeqObjs.Rows)
				r.HeaderCell.Value = "Volume Track " + r.Index;
			foreach (DataGridViewRow r in lvlLoopTracks.Rows)
				r.HeaderCell.Value = "Volume Track " + r.Index;
			///mark that lvl is saved (just freshly loaded)
			SaveLvl(true);
			//select the first leaf
			if (_lvlleafs.Count > 0)
				lvlLeafList_RowEnter(null, new DataGridViewCellEventArgs(0, 0));
		}

		public void InitializeLvlStuff()
		{
			//add event handler to this collection
			_lvlleafs.CollectionChanged += lvlleaf_CollectionChanged;

			_lvlpaths.Sort();
			///customize Leaf List a bit
			lvlLeafList.ColumnCount = 2;
			lvlLeafList.RowHeadersVisible = false;
			lvlLeafList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 15, GraphicsUnit.Pixel)
			};
			lvlLeafList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			lvlLeafList.Columns[0].HeaderText = "Leaf";
			lvlLeafList.Columns[1].HeaderText = "Beats";
			lvlLeafList.ReadOnly = true;
			///

			///customize Paths List a bit
			//custom column containing comboboxes per cell
			DataGridViewComboBoxColumn _dgvlvlpaths = new DataGridViewComboBoxColumn() {
				DataSource = _lvlpaths,
				HeaderText = "Path Name"
			};
			lvlLeafPaths.Columns.Add(_dgvlvlpaths);
			lvlLeafPaths.RowHeadersVisible = false;
			lvlLeafPaths.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			///

			///customize Loop Track list a bit
			//custom column containing comboboxes per cell
			DataGridViewComboBoxColumn _dgvlvlloopsamples = new DataGridViewComboBoxColumn() {
				DataSource = _lvlsamples,
				HeaderText = "Loop Track Name"
			};
			lvlLoopTracks.Columns.Add(_dgvlvlloopsamples);
			lvlLoopTracks.ColumnCount = 2;
			lvlLoopTracks.Columns[1].HeaderText = "Beats per loop";
			lvlLoopTracks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			///
			
			///set Saved flag to true, since nothing is loaded
			SaveLvl(true);
		}

		public void LvlEditorVisible()
		{
			if (workingfolder != null) {
				foreach (Control c in panelLevel.Controls)
					c.Visible = true;
				btnlvlPanelNew.Visible = false;
				btnlvlPanelOpen.Visible = false;
			}
		}

		public void LvlUpdatePaths(int index)
		{
			lvlLeafPaths.Rows.Clear();
			//for each path in the selected leaf, populate the paths DGV
			foreach (string path in _lvlleafs[index].paths)
				lvlLeafPaths.Rows.Add(new object[] { path });
			//monke
		}

		public void LvlReloadSamples()
		{
			_lvlsamples.Clear();
			//find all samp_ files in the level folder
			var _sampfiles = Directory.GetFiles(workingfolder, "samp_*.txt");
			//iterate over each file
			foreach (string f in _sampfiles) {
				//parse file to JSON
				dynamic _in = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(f), "#.*", ""));
				//iterate over items:[] list to get each sample and add names to list
				foreach (dynamic samp in _in["items"]) {
					_lvlsamples.Add((string)samp["obj_name"]);
				}
			}
			_lvlsamples.Sort();
		}

		public void SaveLvl(bool save)
		{
			_savelvl = save;
			if (!save) {
				if (!lblLvlName.Text.Contains("(unsaved)"))
					lblLvlName.Text += " (unsaved)";
			}
			else {
				lblLvlName.Text = lblLvlName.Text.Replace(" (unsaved)", "");
			}
		}

		///Import raw text from rich text box to selected row
		public void LvlTrackRawImport(DataGridViewRow r, JObject _rawdata)
		{
			//_rawdata contains a list of all data points. By getting Properties() of it,
			//each point becomes its own index
			var data_points = _rawdata.Properties().ToList();
			//set highlighting color
			Color _color = Color.Purple;
			//iterate over each data point, and fill cells
			foreach (JProperty data_point in data_points) {
				try {
					r.Cells[int.Parse(data_point.Name)].Value = (float)data_point.Value;
					r.Cells[int.Parse(data_point.Name)].Style.BackColor = _color;
				}
				catch (ArgumentOutOfRangeException) { }
			}
		}

		public JObject LvlBuildSave(string _lvlname)
		{
			_lvlname = Regex.Replace(_lvlname, "[.].*", ".lvl");
			///start building JSON output
			JObject _save = new JObject();
			_save.Add("obj_type", "SequinLevel");
			_save.Add("obj_name", $"{_lvlname}");
			_save.Add("approach_beats", NUD_lvlApproach.Value);
			//this section adds all colume sequencer controls
			JArray seq_objs = new JArray();
			foreach (DataGridViewRow seq_obj in lvlSeqObjs.Rows) {
				JObject s = new JObject();
				s.Add("obj_name", $"{_lvlname}");
				s.Add("param_path", $"layer_volume,{seq_obj.Index}");
				s.Add("trait_type", "kTraitFloat");

				JObject data_points = new JObject();
				for (int x = 0; x < seq_obj.Cells.Count; x++) {
					if (!string.IsNullOrEmpty(seq_obj.Cells[x].Value?.ToString()))
						data_points.Add(x.ToString(), float.Parse(seq_obj.Cells[x].Value.ToString()));
				}
				s.Add("data_points", data_points);

				s.Add("step", "False");
				s.Add("default", "0");
				s.Add("footer", new JArray() { new object[] { 1, 1, 2, 1, 2, "kIntensityScale", "kIntensityScale", 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 } });

				seq_objs.Add(s);
			}
			_save.Add("seq_objs", seq_objs);
			//this section adds all leafs
			JArray leaf_seq = new JArray();
			foreach (LvlLeafData _leaf in _lvlleafs) {
				JObject s = new JObject();
				s.Add("beat_cnt", _leaf.beats);
				s.Add("leaf_name", _leaf.leafname);
				s.Add("main_path", "default.path");
				s.Add("sub_paths", JArray.FromObject(_leaf.paths));
				s.Add("pos", new JArray() { 0, 0, 0 });
				s.Add("rot_x", new JArray() { 1, 0, 0 });
				s.Add("rot_y", new JArray() { 0, 1, 0 });
				s.Add("rot_z", new JArray() { 0, 0, 1 });
				s.Add("scale", new JArray() { 1, 1, 1 });

				leaf_seq.Add(s);
			}
			_save.Add("leaf_seq", leaf_seq);
			//this section adds the loop tracks
			JArray loops = new JArray();
			foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
				JObject s = new JObject();
				s.Add("samp_name", (string)r.Cells[0].Value);
				s.Add("beats_per_loop", decimal.Parse(r.Cells[1].Value.ToString()));

				loops.Add(s);
			}
			_save.Add("loops", loops);
			//final keys
			_save.Add("volume", NUD_lvlVolume.Value);
			_save.Add("input_allowed", dropLvlInput.Text);
			_save.Add("tutorial_type", dropLvlTutorial.Text);
			_save.Add("start_angle_fracs", new JArray() { 1, 1, 1 });
			///end building JSON output

			return _save;
		}
		#endregion
	}
}