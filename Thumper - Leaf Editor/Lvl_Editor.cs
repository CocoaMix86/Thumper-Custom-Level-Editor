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

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _savelvl = true;
		int _lvllength;
		string _loadedlvl;

		List<string> _lvlpaths = (Properties.Resources.paths).Replace("\r\n", "\n").Split('\n').ToList();
		List<string> _SampleLevels = new List<string>();
		List<List<string>> _SampleSamples = new List<List<string>>();

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
				_tracks.Clear();
				_loadedleaf = Path.GetFileName(_lvlleafs[e.RowIndex].leafname);
				lblTrackFileName.Text = "Leaf Editor - " + _loadedleaf;
				List<string> _load = File.ReadAllLines(_lvlleafs[e.RowIndex].filepath).ToList();
				LoadLeaf(_load);
			}

			LvlUpdatePaths(e.RowIndex);
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
			if (e.ColumnIndex == 0) {
				DataGridViewComboBoxCell _combocell = lvlLoopTracks[1, e.RowIndex] as DataGridViewComboBoxCell;
				_combocell.DataSource = _SampleSamples[_SampleLevels.IndexOf(lvlLoopTracks[e.ColumnIndex, e.RowIndex].Value.ToString())];
			}
			if (e.ColumnIndex == 1) {
				lvlLoopTracks[2, e.RowIndex].ReadOnly = false;
				lvlLoopTracks[2, e.RowIndex].Style.BackColor = Color.FromArgb(40, 40, 40);
			}
			//set lvl save flag to false
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
				_lvllength += _lvlleafs[x].beats;
			}
			lvlLeafList.RowEnter += lvlLeafList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnLvlLeafDelete.Enabled = _lvlleafs.Count > 0;
			btnLvlLeafUp.Enabled = _lvlleafs.Count > 1;
			btnLvlLeafDown.Enabled = _lvlleafs.Count > 1;
			//enable/disable path buttons if leaf exists
			btnLvlPathAdd.Enabled = _lvlleafs.Count > 0;
			if (btnLvlPathAdd.Enabled == false) btnLvlPathDelete.Enabled = false;
			//enable/disable seqObjs buttons
			btnLvlSeqAdd.Enabled = _lvlleafs.Count > 0;

			lvlSeqObjs.ColumnCount = _lvllength;
			GenerateColumnStyle(lvlSeqObjs, _lvllength);
			//set lvl save flag to false
			SaveLvl(false);
		}
		/// Set "saved" flag to false for LVL when these events happen
		private void NUD_lvlApproach_ValueChanged(object sender, EventArgs e) => SaveLvl(false);
		private void NUD_lvlVolume_ValueChanged(object sender, EventArgs e) => SaveLvl(false);
		private void dropLvlInput_SelectedIndexChanged(object sender, EventArgs e) => SaveLvl(false);
		private void dropLvlTutorial_SelectedIndexChanged(object sender, EventArgs e) => SaveLvl(false);
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
			}
		}
		/// LVL SAVE
		private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			//start with Approach, Volume, Input, and Tutorial at the top of the file
			string _export = $"{NUD_lvlApproach.Value};{NUD_lvlVolume.Value};{dropLvlInput.Text};{dropLvlTutorial.SelectedItem}\n##\n";
			//add each row of Loop Tracks to the file. Empty cells are ignored and values are ';' separated
			foreach (DataGridViewRow r in lvlLoopTracks.Rows)
				_export += string.Join(";", r.Cells.Cast<DataGridViewCell>().Where(c => !string.IsNullOrEmpty(c?.Value?.ToString())).Select(c => c.Value.ToString()).ToArray()) + "\n";
			_export += "##\n";
			//add each row of seq_objs to the file. Empty cells are ignored. Data cells have their column index and value added in {index:value} format. Cells are ';' separated
			foreach (DataGridViewRow r in lvlSeqObjs.Rows)
				_export += string.Join(";", r.Cells.Cast<DataGridViewCell>().Where(c => !string.IsNullOrEmpty(c?.Value?.ToString())).Select(c => $"{c.ColumnIndex}:{c.Value}").ToArray()) + "\n";
			_export += "##\n";
			foreach (LvlLeafData l in _lvlleafs)
				_export += $"{l.filepath};{l.leafname};{l.beats}#{string.Join(";", l.paths)}\n";

			using (var sfd = new SaveFileDialog()) {
				sfd.Filter = "Thumper Editor Lvl File (*.telvl)|*.telvl";
				sfd.FilterIndex = 1;

				if (sfd.ShowDialog() == DialogResult.OK) {
					if (sfd.FileName.Contains("lvl_")) {
						MessageBox.Show("File not saved. Do not include 'lvl_' in your file name.", "File not saved");
						return;
					}
					File.WriteAllText(sfd.FileName, _export);
					lblLvlName.Text = "Level Editor - " + Path.GetFileName(sfd.FileName);
					_loadedlvl = Path.GetFileName(sfd.FileName).Replace(".telvl", "");
					SaveLvl(true);
				}
			}
		}
		/// LVL LOAD
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				string _load;

				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Editor Lvl File (*.telvl)|*.telvl";
					ofd.Title = "Load a Thumper Editor Lvl file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedlvl = Path.GetFileName(ofd.FileName);
						lblLvlName.Text = "Level Editor - " + _loadedlvl;
						_load = File.ReadAllText(ofd.FileName);
						LoadLvl(_load);
					}
				}
			}
		}
		/// LVL EXPORT
		private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			//check if lvl has been saved before exporting
			if (string.IsNullOrEmpty(_loadedlvl) || !_savelvl) {
				MessageBox.Show("Please save your leaf before exporting");
				return;
			}

			string _export = $@"[
{{
'obj_type': 'SequinLevel',
'obj_name': '{_loadedlvl.Replace(".telvl", "")}.lvl',
'approach_beats': {NUD_lvlApproach.Value},
'seq_objs': [";
			//this section adds the volume control data to the 'seq_objs: []' list, one row at a time.
			//each row gets it's own volume track, denoted by 'layer_volume,{x}'
			for (int x = 0; x < lvlSeqObjs.Rows.Count; x++) {
				_export += $@"
	{{
	'obj_name': '{_loadedlvl.Replace(".telvl", "")}.lvl',
	'param_path': 'layer_volume,{x}',
	'trait_type': 'kTraitFloat',
	'data_points': {{
		{string.Join(";", lvlSeqObjs.Rows[x].Cells.Cast<DataGridViewCell>().Where(c => !string.IsNullOrEmpty(c?.Value?.ToString())).Select(c => $"{c.ColumnIndex}:{c.Value}").ToArray())}
	}},
	'step': False,
	'default': 0,
	'footer': (1,1,2,1,2,'kIntensityScale','kIntensityScale',1,1,1,1,1,1,1,1,0,0,0)
	}},";
			}
			//close the 'seq_objs: []' section, and start 'leaf_seq': []
			_export += "\n],\n'leaf_seq': [";
			//this section adds each leaf associated with the lvl to the 'leaf_seq': [] list
			//a sub list sub_paths': [] gets filled with the tunnels/paths set too
			foreach (LvlLeafData l in _lvlleafs) {
				_export += $@"
	{{
	'beat_cnt': {l.beats},
	'leaf_name': '{l.leafname.Replace(".teleaf", ".leaf")}',
	'main_path': 'default.path',
	'sub_paths': ['{string.Join("','", l.paths)}'],
	'pos': (0, 0, 0),
	'rot_x': (1, 0, 0),
	'rot_y': (0, 1, 0),
	'rot_z': (0, 0, 1),
	'scale': (0, 0, 0)
	}},";
			}
			//close the 'leaf_seq: []' section, and start 'loops': []
			_export += "\n],\n'loops': [";
			//this section adds each loop track specified to the 'loops': [] list
			//while on the form it displays if "drums" or "drones", the lvl does not actually need that (that data exists in samp_ files)
			foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
				_export += $@"
	{{
	'samp_name': '{r.Cells[1].Value.ToString().Replace("drones\\", "").Replace("drums\\", "").Replace(".wav", ".samp")}',
	'beats_per_loop': {r.Cells[2].Value},
	}},";
			}
			//close the 'loops: []' section, and now finish up the lvl file
			_export += $@"
],
'volume': {NUD_lvlVolume.Value},
'input_allowed': {dropLvlInput.Text},
'tutorial_type': {dropLvlTutorial.SelectedItem},
'start_angle_fracs': (1, 1, 1)
}}
]";

			using (var sfd = new SaveFileDialog()) {
				sfd.Filter = "Thumper Editor Lvl File (*.txt)|*.txt";
				sfd.FilterIndex = 1;

				if (sfd.ShowDialog() == DialogResult.OK) {
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					if (tempFileName.Substring(0, 4) != "lvl_")
						sfd.FileName = storePath + "\\lvl_" + tempFileName;
					File.WriteAllText(sfd.FileName, _export);

					MessageBox.Show("Level successfully exported as '" + Path.GetFileName(sfd.FileName) + "'.");
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
				ofd.Filter = "Thumper Editor Leaf Tracks (*.teleaf)|*.teleaf";
				ofd.Title = "Load a Thumper Editor Leaf file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					try {
						//load leaf info into List, like full path, file name, and beats (taken from line 0 of file)
						var _lvlloadleafname = Path.GetFileName(ofd.FileName);
						var _lvlloadleafbeats = int.Parse(File.ReadAllLines(ofd.FileName)[0].Split(';')[0]);
						LvlLeafData _temp = new LvlLeafData() {
							filepath = ofd.FileName,
							leafname = _lvlloadleafname,
							beats = _lvlloadleafbeats,
							paths = new List<string>()
						};
						_lvlleafs.Add(_temp);
						//select the newly added leaf to load it
						lvlLeafList[0, _lvlleafs.Count - 1].Selected = true;
						//display any error if found when importing
					}
					catch (Exception ex) { MessageBox.Show("Unable to open file \"" + ofd.FileName + "\". Are you sure this was created by the editor?\n\n" + ex); }
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
				//sets flag that leaf has unsaved changes
				///SaveLeaf(false);
				lvlLeafList.CurrentCell = lvlLeafList.Rows[rowIndex - 1].Cells[0];
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
				//sets flag that leaf has unsaved changes
				///SaveLeaf(false);
				lvlLeafList.CurrentCell = lvlLeafList.Rows[rowIndex + 1].Cells[0];
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
			lvlLoopTracks[2, lvlLoopTracks.Rows.Count - 1].ReadOnly = true;
			lvlLoopTracks[2, lvlLoopTracks.Rows.Count - 1].Style.BackColor = Color.Black;
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
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void LoadLvl(string _lvl)
		{
			//Clear DGVs so new data can load
			lvlLoopTracks.Rows.Clear();
			_lvlleafs.Clear();
			lvlLeafList.Rows.Clear();
			lvlSeqObjs.Rows.Clear();

			var _load = _lvl.Split(new string[] { "##\n" }, StringSplitOptions.None).ToList();
			//populate the non-DGV elements on the form with info, stored in line 1 of the file
			NUD_lvlApproach.Value = decimal.Parse(_load[0].Split(';')[0]);
			NUD_lvlVolume.Value = decimal.Parse(_load[0].Split(';')[1]);
			dropLvlInput.Text = _load[0].Split(';')[2];
			dropLvlTutorial.Text = _load[0].Split(';')[3];
			//load loop track names and paths to lvlLoopTracks DGV
			var _loadlooptracks = _load[1].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			for (int x = 0; x < _loadlooptracks.Count; x++) {
				btnLvlLoopDelete.Enabled = true;
				var _loadloopdata = _loadlooptracks[x].Split(';');
				//first add "level source" cell and force the CellValueChanged event
				//Forcing the event is required to populate the second combobox with the correct options
				lvlLoopTracks.Rows.Add(_loadloopdata[0]);
				lvlLoopTracks_CellValueChanged(lvlLoopTracks, new DataGridViewCellEventArgs(0, x));
				//_loadloopdata[1] contains track name, if exists
				if (_loadloopdata.Length > 1)
					lvlLoopTracks[1, x].Value = _loadloopdata[1];
				//_loadloopdata[2] contains "beats per loop", if exists
				if (_loadloopdata.Length > 2)
					lvlLoopTracks[2, x].Value = _loadloopdata[2];
			}
			//load leafs associated with this lvl
			var _loadlvlleafs = _load[3].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			foreach (string s in _loadlvlleafs) {
				//splitting on '#' gives the primary leaf data in [0] and any associated tunnels/paths in [1]
				var ss = s.Split('#').ToList();
				_lvlleafs.Add(new LvlLeafData() {
					//splitting [0] on ';' gives the data in order
					filepath = ss[0].Split(';')[0],
					leafname = ss[0].Split(';')[1],
					beats = int.Parse(ss[0].Split(';')[2]),
					//splitting [1] on ';' gives each tunnel/path separaretly
					paths = ss[1].Split(';').ToList()
				});
			}
			//load volume sequencer data
			_loadlooptracks = _load[2].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			for (int x = 0; x < _loadlooptracks.Count; x++) {
				lvlSeqObjs.RowCount++;
				btnLvlSeqClear.Enabled = true;
				btnLvlSeqDelete.Enabled = true;
				//only cells containing data are stored, in {columnIndex:data} format. ';' separates each cell
				foreach (string ss in _loadlooptracks[x].Split(';')) {
					//splitting on ':' gives the index [0] and the data [1]
					lvlSeqObjs[ss.Split(':')[0], x].Value = ss.Split(':')[1];
				}
			}
			//add headers to rows after importing their data
			foreach (DataGridViewRow r in lvlSeqObjs.Rows)
				r.HeaderCell.Value = "Volume Track " + r.Index;
			foreach (DataGridViewRow r in lvlLoopTracks.Rows)
				r.HeaderCell.Value = "Volume Track " + r.Index;
			//mark that lvl is saved (just freshly loaded)
			SaveLvl(true);
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
			DataGridViewComboBoxColumn _dgvlvllooplevels = new DataGridViewComboBoxColumn() {
				DataSource = _SampleLevels,
				HeaderText = "Level"
			};
			DataGridViewComboBoxColumn _dgvlvlloopsamples = new DataGridViewComboBoxColumn() {
				DataSource = null,
				HeaderText = "Loop Track"
			};
			lvlLoopTracks.Columns.Add(_dgvlvllooplevels);
			lvlLoopTracks.Columns.Add(_dgvlvlloopsamples);
			lvlLoopTracks.ColumnCount = 3;
			lvlLoopTracks.Columns[2].HeaderText = "Beats per loop";
			lvlLoopTracks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			///

			///import sample names
			//split on ### to separate each set of levels
			var _import = Properties.Resources.samples.Replace("\r\n", "\n").Split(new string[] { "\n###\n" }, StringSplitOptions.None);
			foreach (string _s in _import) {
				//split on \n to separate each individual sample
				foreach (string _s2 in _s.Split('\n')) {
					//split on ',' to separate each data point of a sample
					var _s3 = _s2.Split(',');
					//for every new level name found, add it to the Levels list,
					//and create a new List<string> for its respective samples to be stored 
					if (!_SampleLevels.Contains(_s3[0])) {
						_SampleLevels.Add(_s3[0]);
						_SampleSamples.Add(new List<string>());
					}
					//_s3[1] and [2] store the sample type (drone or drum) and the sample name
					//getting IndexOf(_s3[0]) puts the sample into the correct list
					_SampleSamples[_SampleLevels.IndexOf(_s3[0])].Add(_s3[1] + "\\" + _s3[2]);
				}
			}
			///set Saved flag to true, since nothing is loaded
			SaveLvl(true);
		}

		public void LvlUpdatePaths(int index)
		{
			lvlLeafPaths.Rows.Clear();
			//for each path in the selected leaf, populate the paths DGV
			foreach (string path in _lvlleafs[index].paths)
				lvlLeafPaths.Rows.Add(new object[] { path });
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
		#endregion
	}
}