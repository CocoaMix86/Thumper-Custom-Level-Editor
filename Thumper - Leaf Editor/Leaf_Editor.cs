using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _saveleaf = true;

		int _beats = 0;
		int _selecttrack = 0;

		string _errorlog = "";
		public string _loadedleaf
		{
			get { return loadedleaf; }
			set
			{
				if (loadedleaf != value) {
					loadedleaf = value;
					LeafEditorVisible();
					leafsaveAsToolStripMenuItem.Enabled = true;
					leafsaveToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string loadedleaf;

		//public List<List<string>> _tracks = new List<List<string>>();
		public List<Sequencer_Object> _tracks = new List<Sequencer_Object>();
		public List<Object_Params> _objects = new List<Object_Params>();
		public List<string> _timesig = new List<string>() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
		public List<string> _tracklane = new List<string>() { ".a01", ".a02", ".ent", ".z01", ".z02" };
		#endregion
		#region EventHandlers
		///        ///
		/// EVENTS ///
		///        ///

		///LEAF EDITOR PANEL LOCATION CHANGED
		private void panelLeaf_LocationChanged(object sender, EventArgs e)
		{
			//stretches panel to fit window
			panelLeaf.Size = new Size(this.Size.Width - panelLeaf.Location.X - 15, panelLeaf.Size.Height);
		}
		///TRACKBAR ZOOM
		private void trackZoom_Scroll(object sender, EventArgs e)
		{
			for (int i = 0; i < _beats; i++) {
				trackEditor.Columns[i].Width = trackZoom.Value;
			}
		}
		///DATAGRIDVIEW - TRACK EDITOR
		//Row changed
		private void trackEditor_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			_selecttrack = e.RowIndex;
			ShowRawTrackData();
			List<string> _params = new List<string>();

			try {
				//if track is a multi-lane object, split param_path from lane so both values can be used to update their dropdown boxes
				if (_tracks[_selecttrack].friendly_param.Contains("right") || _tracks[_selecttrack].friendly_param.Contains("left") || _tracks[_selecttrack].friendly_param.Contains("middle")) {
					_params = _tracks[_selecttrack].friendly_param.Split(new string[] { ", " }, StringSplitOptions.None).ToList();
				}
				else
					_params = new List<string>() { _tracks[_selecttrack].friendly_param, "middle" };
				//set all controls to their values stored in _tracks
				dropObjects.SelectedIndex = dropObjects.FindStringExact(_tracks[_selecttrack].friendly_type);
				dropParamPath.SelectedIndex = dropParamPath.FindStringExact(_params[0]);
				dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[1]);
				txtTrait.Text = _tracks[_selecttrack].trait_type;
				btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack].highlight_color));
				//remove event handlers from a few controls so they don't trigger when their values change
				NUD_TrackHighlight.ValueChanged -= new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged -= new EventHandler(txtDefault_ValueChanged);
				dropLeafStep.SelectedIndexChanged -= new EventHandler(dropLeafStep_SelectedIndexChanged);
				//set values from _tracks
				//NUD_TrackDoubleclick.Value = Decimal.Parse(_tracks[_selecttrack][8]);
				NUD_TrackHighlight.Value = (decimal)_tracks[_selecttrack].highlight_value;
				txtDefault.Value = (decimal)_tracks[_selecttrack]._default;
				dropLeafStep.SelectedItem = _tracks[_selecttrack].step;
				//re-add event handlers
				NUD_TrackHighlight.ValueChanged += new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged += new EventHandler(txtDefault_ValueChanged);
				dropLeafStep.SelectedIndexChanged += new EventHandler(dropLeafStep_SelectedIndexChanged);

				//check if the current track has param_path set. If not, disable some controls
				if (_tracks[_selecttrack].param_path != null)
					return;
				btnTrackColorDialog.Enabled = false;
				NUD_TrackDoubleclick.Enabled = false;
				NUD_TrackHighlight.Enabled = false;
			}
			catch { }
		}
		//Cell value changed
		private void trackEditor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				var _val = trackEditor.CurrentCell.Value;
				//iterate over each cell in the selection
				foreach (DataGridViewCell _cell in trackEditor.SelectedCells) {
					//if cell does not have the value, set it
					if (_cell.Value != _val)
						_cell.Value = _val;
					if (_cell.Value.ToString() == "")
						_cell.Value = null;

					float i = 0;
					//clear cell styling, in case it now falls out of highlighting scope
					_cell.Style = null;
					//try parsing the value. If it doesn't work, i = 0
					i = float.TryParse(_cell.Value.ToString(), out i) ? i : 0;
					//check Absolute value of cell against highlughting value of track
					//we check Absolute because this catches negatives too
					if (Math.Abs(i) >= _tracks[_cell.RowIndex]._default)
						_cell.Style.BackColor = Color.FromArgb(int.Parse(_tracks[_cell.RowIndex].highlight_color));

					//sets flag that leaf has unsaved changes
					SaveLeaf(false);
				}
			}
			catch { }

			ShowRawTrackData();
		}
		//Cell double click
		private void trackEditor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//checks if the cell's value contains a 0. If so, change the value to write to 1. If not, assume the cell value is already 0 and keep the value to write to 0
			string val;
			if (trackEditor.CurrentCell.Value == null)
				val = NUD_TrackDoubleclick.Value.ToString();
			else
				val = null;

			trackEditor.CurrentCell.Value = val;
		}
		//Keypress Backspace - clear selected cells
		private void trackEditor_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Back) {
				foreach (DataGridViewCell dgvc in trackEditor.SelectedCells)
					dgvc.Value = null;
				TrackUpdateHighlighting(trackEditor.CurrentRow);
			}
			e.Handled = true;
		}
		//Keypress Delete - clear selected cellss
		private void trackEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) {
				foreach (DataGridViewCell dgvc in trackEditor.SelectedCells)
					dgvc.Value = null;
				TrackUpdateHighlighting(trackEditor.CurrentRow);
			}
			e.Handled = true;
		}
		///TOOLSTRIP
		///Toolstrip - TRACK
		//Add Track (row)
		private void addToolStripMenuItem_Click(object sender, EventArgs e) => btnTrackAdd.PerformClick();
		//Delete Track (row)
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => btnTrackDelete.PerformClick();
		//Move Track Up
		private void moveTrackUpToolStripMenuItem_Click(object sender, EventArgs e) => btnTrackUp.PerformClick();
		//Move Track Down
		private void moveTrackDownToolStripMenuItem_Click(object sender, EventArgs e) => btnTrackDown.PerformClick();
		//Set Track Color
		private void trackColorToolStripMenuItem_Click(object sender, EventArgs e) => btnTrackColorDialog.PerformClick();
		///LEAF LENGTH
		private void numericUpDown_LeafLength_ValueChanged(object sender, EventArgs e)
		{
			_beats = (int)numericUpDown_LeafLength.Value;
			trackEditor.ColumnCount = _beats;
			GenerateColumnStyle(trackEditor, _beats);
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///DROPDOWN OBJECTS
		private void dropObjects_SelectedValueChanged(object sender, EventArgs e)
		{
			//this gets triggered when the program starts, when no rows exist, and then it throws an error
			//this is only here to stop that
			try {
				//when an object is chosen, unlock the param_path options and set datasource
				dropParamPath.DataSource = _objects[dropObjects.SelectedIndex].param_displayname;
				//switch index back and forth to trigger event
				dropParamPath.SelectedIndex = -1;
				dropParamPath.SelectedIndex = 0;
				dropParamPath.Enabled = true;
				//set default lane to 'middle'
				dropTrackLane.SelectedIndex = 2;
			}
			catch { };
		}
		///DROPDOWN PARAM_PATHS
		private void dropParamPath_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (dropParamPath.SelectedIndex != -1 && dropParamPath.Enabled) {
				//if the param_path is .ent, enable lane choice
				if (_objects[dropObjects.SelectedIndex].param_path[dropParamPath.SelectedIndex].EndsWith(".ent")) {
					dropTrackLane.Enabled = true;
					btnTrackApply.Enabled = true;
				}
				//else set lane to middle and enable 'Apply' button
				else {
					dropTrackLane.Enabled = false;
					dropTrackLane.SelectedIndex = 2;
					btnTrackApply.Enabled = true;
				}
			}
		}
		///DROPDOWN TRACK LANE
		private void dropTrackLane_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (dropTrackLane.Enabled && dropTrackLane.SelectedIndex != -1) {
				btnTrackApply.Enabled = true;
			}
		}
		///DROPDOWN TIMESIGS
		private void dropTimeSig_SelectedIndexChanged(object sender, EventArgs e)
		{
			TrackTimeSigHighlighting();
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///NUMERIC_UPDOWN TRACK HIGHLIGHT VALUE
		private void NUD_TrackHighlight_ValueChanged(object sender, EventArgs e)
		{
			_tracks[trackEditor.CurrentRow.Index].highlight_value = (float)NUD_TrackHighlight.Value;
			TrackUpdateHighlighting(trackEditor.CurrentRow);
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///LEAF - NEW
		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				_tracks.Clear();
				trackEditor.Rows.Clear();
				lblTrackFileName.Text = "Leaf Editor";
				dropObjects.Enabled = dropParamPath.Enabled = btnTrackApply.Enabled = false;
				//
				SaveLeaf(true);
				leafsaveAsToolStripMenuItem_Click(null, null);
			}
		}
		///LEAF - SAVE FILE
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if _loadedlvl is somehow not set, force Save As instead
			if (_loadedleaf == null) {
				leafsaveAsToolStripMenuItem.PerformClick();
				return;
			}
			//write contents direct to file without prompting save dialog
			var _save = LeafBuildSave(Path.GetFileName(_loadedleaf).Replace("leaf_", ""));
			File.WriteAllText(_loadedleaf, JsonConvert.SerializeObject(_save));
			SaveLeaf(true);
			lblTrackFileName.Text = $"Leaf Editor - {_save["obj_name"]}";
		}
		///LEAF - SAVE AS
		private void leafsaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Leaf File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//separate path and filename
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					//check if user input "leaf_", and deny save if so
					if (Path.GetFileName(sfd.FileName).Contains("leaf_")) {
						MessageBox.Show("File not saved. Do not include 'leaf_' in your file name.", "File not saved");
						return;
					}
					//get contents to save
					var _save = LeafBuildSave(Path.GetFileName(sfd.FileName));
					//serialize JSON object to a string, and write it to the file
					sfd.FileName = $@"{storePath}\leaf_{tempFileName}";
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save));
					//set a few visual elementsto show what file is being worked on
					lblTrackFileName.Text = $"Leaf Editor - {_save["obj_name"]}";
					_loadedleaf = sfd.FileName;
					SaveLeaf(true);
				}
			}
		}
		///LEAF - LOAD FILE
		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Leaf File (*.txt)|*.txt";
					ofd.Title = "Load a Thumper Leaf file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedleaf = ofd.FileName;
						var _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadLeaf(_load);
					}
				}
			}
		}
		///DEFAULT TRACK VALUE CHANGED
		private void txtDefault_ValueChanged(object sender, EventArgs e)
		{
			_tracks[trackEditor.CurrentRow.Index]._default = (float)txtDefault.Value;
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///STEP CHANGED
		private void dropLeafStep_SelectedIndexChanged(object sender, EventArgs e)
		{
			_tracks[trackEditor.CurrentRow.Index].step = dropLeafStep.Text;
			SaveLeaf(false);
		}
		#endregion
		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnRawImport_Click(object sender, EventArgs e)
		{
			TrackRawImport(trackEditor.CurrentRow, JObject.Parse($"{{{richRawTrackData.Text}}}"));
		}

		private void btnTrackDelete_Click(object sender, EventArgs e)
		{
			bool _empty = true;
			//iterate over current row to see if any cells have data
			foreach (DataGridViewCell dgvc in trackEditor.CurrentRow.Cells) {
				if (dgvc.Value != null) {
					_empty = false;
					break;
				}
			}
			//if row is not empty, show confirmation box. Otherwise just delete the row
			if ((!_empty && MessageBox.Show("This track has data. Are you sure you want to delete it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
				try {
					_tracks.RemoveAt(trackEditor.CurrentRow.Index);
					trackEditor.Rows.Remove(trackEditor.CurrentRow);
					//sets flag that leaf has unsaved changes
					SaveLeaf(false);
				}
				catch { }
			}
			//disable elements if there are no tracks
			if (_tracks.Count == 0) {
				dropObjects.Enabled = false;
				dropParamPath.Enabled = false;
				btnTrackApply.Enabled = false;
				btnTrackColorDialog.Enabled = false;
				NUD_TrackDoubleclick.Enabled = false;
				NUD_TrackHighlight.Enabled = false;

				btnTrackDelete.Enabled = false;
				btnTrackUp.Enabled = false;
				btnTrackDown.Enabled = false;
				btnTrackClear.Enabled = false;

				deleteToolStripMenuItem.Enabled = false;
				moveTrackUpToolStripMenuItem.Enabled = false;
				moveTrackDownToolStripMenuItem.Enabled = false;
				trackColorToolStripMenuItem.Enabled = false;
			}
		}

		private void btnTrackAdd_Click(object sender, EventArgs e)
		{
			dropObjects.Enabled = true;
			dropParamPath.Enabled = true;

			btnTrackDelete.Enabled = true;
			btnTrackUp.Enabled = true;
			btnTrackDown.Enabled = true;
			btnTrackClear.Enabled = true;

			deleteToolStripMenuItem.Enabled = true;
			moveTrackUpToolStripMenuItem.Enabled = true;
			moveTrackDownToolStripMenuItem.Enabled = true;
			trackColorToolStripMenuItem.Enabled = true;

			_tracks.Add(new Sequencer_Object() { highlight_color = "-8355585", highlight_value = 1 });
			trackEditor.RowCount++;
			trackEditor.CurrentCell = trackEditor.Rows[_tracks.Count - 1].Cells[0];
			//disable Apply button if object is not set
			dropObjects.SelectedIndex = 0;
			if (dropObjects.SelectedIndex == -1 || dropParamPath.SelectedIndex == -1)
				btnTrackApply.Enabled = false;
			else btnTrackApply.Enabled = true;
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}

		private void btnTrackUp_Click(object sender, EventArgs e)
		{
			DataGridView dgv = trackEditor;
			try {
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
				if (rowIndex == 0)
					return;
				//move track in list
				Sequencer_Object selectedTrack = _tracks[rowIndex];
				_tracks.Remove(selectedTrack);
				_tracks.Insert(rowIndex - 1, selectedTrack);
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.Rows.Remove(selectedRow);
				dgv.Rows.Insert(rowIndex - 1, selectedRow);
				dgv.ClearSelection();
				dgv.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			catch { }
		}

		private void btnTrackDown_Click(object sender, EventArgs e)
		{
			DataGridView dgv = trackEditor;
			try {
				int totalRows = dgv.Rows.Count;
				// get index of the row for the selected cell
				int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
				if (rowIndex == totalRows - 1)
					return;
				//move track in list
				Sequencer_Object selectedTrack = _tracks[rowIndex];
				_tracks.Remove(selectedTrack);
				_tracks.Insert(rowIndex + 1, selectedTrack);
				// get index of the column for the selected cell
				int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
				DataGridViewRow selectedRow = dgv.Rows[rowIndex];
				dgv.Rows.Remove(selectedRow);
				dgv.Rows.Insert(rowIndex + 1, selectedRow);
				dgv.ClearSelection();
				dgv.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			catch { }
		}

		private void btnTrackClear_Click(object sender, EventArgs e)
		{
			bool _empty = true;
			//iterate over current row to see if any cells have data
			foreach (DataGridViewCell dgvc in trackEditor.CurrentRow.Cells) {
				if (dgvc.Value != null) {
					_empty = false;
					break;
				}
			}
			//if YES, clear cell values in row and clear highlighting
			if ((!_empty && MessageBox.Show("This track has data. Are you sure you want to clear it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
				foreach (DataGridViewCell dgvc in trackEditor.CurrentRow.Cells) {
					dgvc.Value = null;
					dgvc.Style = null;
				}
			}
		}

		private void btnTrackApply_Click(object sender, EventArgs e)
		{
			//fill object properties on the form
			txtDefault.Text = _objects[dropObjects.SelectedIndex].def[dropParamPath.SelectedIndex];
			dropLeafStep.Text = _objects[dropObjects.SelectedIndex].step[dropParamPath.SelectedIndex];
			txtTrait.Text = _objects[dropObjects.SelectedIndex].trait_type[dropParamPath.SelectedIndex];
			//enable track highlighting tools
			btnTrackColorDialog.Enabled = true;
			NUD_TrackDoubleclick.Enabled = true;
			NUD_TrackHighlight.Enabled = true;
			//add track to list and populate with values
			_tracks[_selecttrack] = new Sequencer_Object() {
				obj_name = _objects[dropObjects.SelectedIndex].obj_name[dropParamPath.SelectedIndex],
				friendly_type = _objects[dropObjects.SelectedIndex].obj_displayname,
				param_path = _objects[dropObjects.SelectedIndex].param_path[dropParamPath.SelectedIndex],
				friendly_param = _objects[dropObjects.SelectedIndex].param_displayname[dropParamPath.SelectedIndex],
				_default = float.Parse(_objects[dropObjects.SelectedIndex].def[dropParamPath.SelectedIndex]),
				step = _objects[dropObjects.SelectedIndex].step[dropParamPath.SelectedIndex],
				trait_type = _objects[dropObjects.SelectedIndex].trait_type[dropParamPath.SelectedIndex],
				highlight_color = "-8355585",
				highlight_value = 1,
				footer = _objects[dropObjects.SelectedIndex].footer[dropParamPath.SelectedIndex]
			};
			//if lane is not middle, edit the param_path and friendly_param to match
			if (_tracks[_selecttrack].param_path.Contains(".ent")) {
				_tracks[_selecttrack].param_path = _tracks[_selecttrack].param_path.Replace(".ent", _tracklane[dropTrackLane.SelectedIndex]);
				_tracks[_selecttrack].friendly_param += ", " + dropTrackLane.Text;
			}
			//change row header to reflect what the track is
			ChangeTrackName();
		}
		///Sets highlighting color of current track
		private void btnTrackColorDialog_Click(object sender, EventArgs e)
		{
			DialogResult result = colorDialog1.ShowDialog();
			if (result == DialogResult.OK) {
				btnTrackColorDialog.BackColor = colorDialog1.Color;
				_tracks[_selecttrack].highlight_color = colorDialog1.Color.ToArgb().ToString();
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			//call method to update coloring of track
			TrackUpdateHighlighting(trackEditor.CurrentRow);
		}

		private void btnLeafPanelNew_Click(object sender, EventArgs e)
		{
			leafnewToolStripMenuItem.PerformClick();
		}

		private void btnLeafPanelOpen_Click(object sender, EventArgs e)
		{
			leafloadToolStripMenuItem.PerformClick();
		}
		#endregion
		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void SaveLeaf(bool save)
		{
			_saveleaf = save;
			if (!save) {
				if (!lblTrackFileName.Text.Contains("(unsaved)"))
					lblTrackFileName.Text += " (unsaved)";
			}
			else {
				lblTrackFileName.Text = lblTrackFileName.Text.Replace(" (unsaved)", "");
			}
		}

		public void InitializeTracks(DataGridView grid)
		{
			//track editor cell formatting
			grid.DefaultCellStyle.Font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
			grid.DefaultCellStyle.ForeColor = Color.FromArgb(0, 0, 0);
			grid.DefaultCellStyle.SelectionBackColor = Color.FromName("Highlight");
			grid.DefaultCellStyle.SelectionForeColor = Color.FromName("HighlightText");
			grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
			grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			grid.ReadOnly = false;
			grid.RowTemplate.Height = 20;

			//double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
			//used to significantly improve rendering performance
			if (!SystemInformation.TerminalServerSession) {
				Type dgvType = grid.GetType();
				PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
				pi.SetValue(grid, true, null);
			}
			GenerateColumnStyle(grid, _beats);
		}

		public void GenerateColumnStyle(DataGridView grid, int _cells)
		{
			//stylize track grid/columns
			for (int i = 0; i < _cells; i++) {
				grid.Columns[i].Name = i.ToString();
				grid.Columns[i].Resizable = DataGridViewTriState.False;
				grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				grid.Columns[i].DividerWidth = 1;
				grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				grid.Columns[i].Frozen = false;
				grid.Columns[i].Width = 60;
				grid.Columns[i].MinimumWidth = 2;
				grid.Columns[i].ReadOnly = false;
				grid.Columns[i].ValueType = typeof(string);
			}
		}
		///Import raw text from rich text box to selected row
		public void TrackRawImport(DataGridViewRow r, JObject _rawdata)
		{
			//_rawdata contains a list of all data points. By getting Properties() of it,
			//each point becomes its own index
			var data_points = _rawdata.Properties().ToList();
			//get highlighting value and color
			float _f = _tracks[r.Index].highlight_value;
			Color _color = Color.FromArgb(int.Parse(_tracks[r.Index].highlight_color));
			//check if the last data point is beyond the beat count. If it is, it will crash or not be included in the track editor
			//Ask the user if they want to expand the leaf to accomadate the data point
			if (data_points.Count > 0 && int.Parse(((JProperty)data_points.Last()).Name) >= r.Cells.Count) {
				if (MessageBox.Show($"Your last data point is beyond the leaf's beat count. Do you want to lengthen the leaf? If you do not, the data point will be left out.\nObject: {r.HeaderCell.Value}\nData point: {data_points.Last()}", "Leaf too short", MessageBoxButtons.YesNo) == DialogResult.Yes)
					numericUpDown_LeafLength.Value = int.Parse(((JProperty)data_points.Last()).Name) + 1;
			}
			//iterate over each data point, and fill cells
			foreach (JProperty data_point in data_points) {
				try {
					r.Cells[int.Parse(data_point.Name)].Value = (float)data_point.Value;
					if (Math.Abs((float)data_point.Value) >= _f)
						r.Cells[int.Parse(data_point.Name)].Style.BackColor = _color;
				}
				catch (ArgumentOutOfRangeException) { /*MessageBox.Show($"Data point {data_point.Name} is beyond the leaf beatcount of {numericUpDown_LeafLength.Value} for object {r.HeaderCell.Value}");*/ }
			}
		}
		///Updates row headers to be the Object and Param_Path
		public void ChangeTrackName()
		{
			trackEditor.CurrentRow.HeaderCell.Value = _tracks[_selecttrack].friendly_type + " (" + _tracks[_selecttrack].friendly_param + ")";
		}
		///Takes values in a row and puts in them in the rich text box, condensed
		public void ShowRawTrackData()
		{
			string _out = "";
			try {
				//iterate over each cell of the selected row
				for (int x = 0; x < trackEditor.ColumnCount; x++) {
					//if no value, leave it out
					if (trackEditor.Rows[_selecttrack].Cells[x].Value != null && trackEditor.Rows[_selecttrack].Cells[x].Value.ToString() != "")
						_out += x + ":" + trackEditor.Rows[_selecttrack].Cells[x].Value + ",";
				}
				//output final result
				richRawTrackData.Text = _out.Remove(_out.Length - 1);
			}
			catch { richRawTrackData.Text = ""; }
		}
		///Updates column highlighting in the DGV based on time sig
		public void TrackTimeSigHighlighting()
		{
			bool _switch = true;
			int beats = int.Parse(dropTimeSig.SelectedValue.ToString().Split('/')[0]);
			for (int i = 0; i < _beats; i++) {
				if (i % beats == 0)
					_switch = !_switch;
				if (_switch)
					trackEditor.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
				else
					trackEditor.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
			}
		}
		///Updates cell highlighting in the DGV
		public void TrackUpdateHighlighting(DataGridViewRow r)
		{
			float i;
			//iterate over column count - that's how many cells there are in the row
			for (int x = 0; x < r.Cells.Count; x++) {
				//remove any current styling, in case it now falls out of scope
				r.Cells[x].Style = null;
				try {
					//parse value. If not parseable, set to 0
					i = float.TryParse(r.Cells[x].Value.ToString(), out i) ? i : 0;
				}
				catch { i = 0; }
				//if the cell value is greater than the criteria of the row, highlight it with that row's color
				try {
					if (Math.Abs(i) >= _tracks[r.Index].highlight_value) {
						r.Cells[x].Style.BackColor = Color.FromArgb(int.Parse(_tracks[r.Index].highlight_color));
					}
				}
				catch { }
			}
		}

		public void LeafEditorVisible()
		{
			foreach (Control c in panelLeaf.Controls)
				c.Visible = true;
			btnLeafPanelNew.Visible = false;
			btnLeafPanelOpen.Visible = false;
		}

		///Update DGV from _tracks
		public void LoadLeaf(dynamic _load /*List<string> _load*/)
		{
			if ((string)_load["obj_type"] != "SequinLeaf") {
				MessageBox.Show("This does not appear to be a leaf file!");
				return;
			}

			lblTrackFileName.Text = $@"Leaf Editor - {_load["obj_name"]}";
			//clear existing tracks
			_tracks.Clear();
			//set beat_cnt and time_sig
			numericUpDown_LeafLength.Value = (int)_load["beat_cnt"];
			var _time_sig = (string)_load["time_sig"] ?? "4/4";
			dropTimeSig.SelectedIndex = dropTimeSig.FindStringExact(_time_sig);
			//each object in the seq_objs[] list becomes a track
			foreach (var seq_obj in _load["seq_objs"]) {
				if (seq_obj.ContainsKey("param_path_hash")) {
					MessageBox.Show($"Skipping {seq_obj["obj_name"]} due to 'param_path_hash' being present.");
					continue;
				}
				Sequencer_Object _s = new Sequencer_Object() {
					obj_name = seq_obj["obj_name"],
					param_path = seq_obj["param_path"],
					trait_type = seq_obj["trait_type"],
					data_points = seq_obj["data_points"],
					step = seq_obj["step"],
					_default = seq_obj["default"],
					footer = seq_obj["footer"]
				};
				//if the leaf has definitions for these, add them. If not, set to defaults
				_s.highlight_color = seq_obj.ContainsKey("editor_data") ? (string)seq_obj["editor_data"][0] : "-8355585";
				_s.highlight_value = seq_obj.ContainsKey("editor_data") ? (float)seq_obj["editor_data"][1] : 1;
				//iterate over every _object to find where a param_path is located
				//this was the best way to do this I could come up with
				for (int x = 0; x < _objects.Count; x++) {
					//replace .z01 .z02 .a01 .a02 with .ent, so that it's found in the param list
					var reg_param = Regex.Replace(_s.param_path, "[.].*", ".ent");
					//if found, set the friendly names
					if (_objects[x].param_path.Contains(reg_param)) {
						_s.friendly_param = _objects[x].param_displayname[_objects[x].param_path.IndexOf(reg_param)];
						_s.friendly_type = _objects[x].obj_displayname;
					}
				}
				//if an object can be multi-lane, it will be an .ent. Check for "." to detect this
				if (_s.param_path.Contains("."))
					//get the index of the lane from _tracklane to get the item from dropTrackLane, and append that to the friendly_param
					_s.friendly_param += $", {dropTrackLane.Items[_tracklane.IndexOf($".{_s.param_path.Split('.')[1]}")]}";

				//finally, add the completed seq_obj to tracks
				_tracks.Add(_s);
			}
			//clear the DGV and prep for new data points
			trackEditor.Rows.Clear();
			trackEditor.RowCount = _tracks.Count;
			//foreach row, import data points associated with it
			foreach (DataGridViewRow r in trackEditor.Rows) {
				if (_tracks[r.Index].friendly_param.Length > 1) {
					r.HeaderCell.Value = _tracks[r.Index].friendly_type + " (" + _tracks[r.Index].friendly_param + ")";
					//pass _griddata per row to be imported to the DGV
					TrackRawImport(r, _tracks[r.Index].data_points);
				}
			}
			//enable a bunch of elements now that a leaf is loaded.
			dropObjects.Enabled = true;
			dropParamPath.Enabled = true;
			btnTrackColorDialog.Enabled = true;
			NUD_TrackDoubleclick.Enabled = true;
			NUD_TrackHighlight.Enabled = true;
			btnTrackDelete.Enabled = _tracks.Count > 0;
			btnTrackUp.Enabled = _tracks.Count > 1;
			btnTrackDown.Enabled = _tracks.Count > 1;
			SaveLeaf(true);
		}

		public JObject LeafBuildSave(string _leafname)
		{
			_leafname = Regex.Replace(_leafname, "[.].*", ".leaf");
			///start building JSON output
			JObject _save = new JObject();
			_save.Add("obj_type", "SequinLeaf");
			_save.Add("obj_name", _leafname);

			JArray seq_objs = new JArray();
			foreach (Sequencer_Object seq_obj in _tracks) {
				//skip blank tracks
				if (seq_obj.friendly_param == null)
					continue;
				JObject s = new JObject();
				s.Add("obj_name", seq_obj.obj_name.Replace("leafname", (string)_save["obj_name"]));
				s.Add("param_path", seq_obj.param_path);
				s.Add("trait_type", seq_obj.trait_type);

				JObject data_points = new JObject();
				for (int x = 0; x < trackEditor.ColumnCount; x++) {
					if (!string.IsNullOrEmpty(trackEditor[x, _tracks.IndexOf(seq_obj)].Value?.ToString()))
						data_points.Add(x.ToString(), float.Parse(trackEditor[x, _tracks.IndexOf(seq_obj)].Value.ToString()));
				}
				s.Add("data_points", data_points);

				s.Add("step", seq_obj.step);
				s.Add("default", seq_obj._default);
				s.Add("footer", seq_obj.footer);
				s.Add("editor_data", new JArray() { new object[] { seq_obj.highlight_color, seq_obj.highlight_value } });

				seq_objs.Add(s);
			}
			_save.Add("seq_objs", seq_objs);

			_save.Add("beat_cnt", (int)numericUpDown_LeafLength.Value);
			_save.Add("time_sig", dropTimeSig.Text);
			///end building JSON output
			
			return _save;
		}
		#endregion
	}
}