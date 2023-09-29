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

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
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
					ShowPanel(true, panelLeaf);
					leafsaveAsToolStripMenuItem.Enabled = true;
					leafsaveToolStripMenuItem.Enabled = true;
					trackEditor.RowHeadersVisible = true;
				}
			}
		}
		private string loadedleaf;
		string _loadedleaftemp;
		public string leafobj;
		public bool loadingleaf = false;

		//public List<List<string>> _tracks = new List<List<string>>();
		public List<Sequencer_Object> _tracks = new List<Sequencer_Object>();
		public List<Object_Params> _objects = new List<Object_Params>();
		public List<string> _timesig = new List<string>() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
		public List<string> _tracklane = new List<string>() { ".a01", ".a02", ".ent", ".z01", ".z02" };
		public List<Tuple<string, int, int>> _scrollpositions = new List<Tuple<string, int, int>>();
		public Sequencer_Object clipboard_track;
		public DataGridViewRow clipboard_row;
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
				if (_tracks[_selecttrack].friendly_param.Contains("right") || _tracks[_selecttrack].friendly_param.Contains("left") || _tracks[_selecttrack].friendly_param.Contains("center")) {
					_params = _tracks[_selecttrack].friendly_param.Split(new string[] { ", " }, StringSplitOptions.None).ToList();
				}
				else
					_params = new List<string>() { _tracks[_selecttrack].friendly_param, "center" };
				//set all controls to their values stored in _tracks
				dropObjects.SelectedIndex = dropObjects.FindStringExact(_tracks[_selecttrack].friendly_type);
				dropParamPath.SelectedIndex = dropParamPath.FindStringExact(_params[0]);
				//needs a different selection method if it's a sample
				if (_tracks[_selecttrack].param_path == "play")
					dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_tracks[_selecttrack].obj_name);
				else if (_params.Count >= 2)
					dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[1]);
				else //track lane only uses param[0]
					dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[0]);
				txtTrait.Text = _tracks[_selecttrack].trait_type;
				btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack].highlight_color));
				//remove event handlers from a few controls so they don't trigger when their values change
				NUD_TrackHighlight.ValueChanged -= new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged -= new EventHandler(txtDefault_ValueChanged);
				dropLeafStep.SelectedIndexChanged -= new EventHandler(dropLeafStep_SelectedIndexChanged);
				dropLeafInterp.SelectedIndexChanged -= new EventHandler(dropLeafInterp_SelectedIndexChanged);
				//set values from _tracks
				//NUD_TrackDoubleclick.Value = Decimal.Parse(_tracks[_selecttrack][8]);
				NUD_TrackHighlight.Value = (decimal)_tracks[_selecttrack].highlight_value;
				btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack].highlight_color));
				txtDefault.Value = (decimal)_tracks[_selecttrack]._default;
				dropLeafStep.SelectedItem = _tracks[_selecttrack].step;
				dropLeafInterp.SelectedItem = _tracks[_selecttrack].default_interp;
				//re-add event handlers
				NUD_TrackHighlight.ValueChanged += new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged += new EventHandler(txtDefault_ValueChanged);
				dropLeafStep.SelectedIndexChanged += new EventHandler(dropLeafStep_SelectedIndexChanged);
				dropLeafInterp.SelectedIndexChanged += new EventHandler(dropLeafInterp_SelectedIndexChanged);

				//check if the current track has param_path set. If not, disable some controls
				/*
				if (_tracks[_selecttrack].param_path != null)
					return;
				btnTrackColorDialog.Enabled = false;
				NUD_TrackDoubleclick.Enabled = false;
				NUD_TrackHighlight.Enabled = false;
				*/
				if (txtTrait.Text == "kTraitBool")
					toolTip1.SetToolTip(txtTrait, "BOOL: accepts values 1 (on) or 0 (off).");
				else if (txtTrait.Text == "kTraitAction")
					toolTip1.SetToolTip(txtTrait, "ACTION: accepts values 1 (activate).");
				else if (txtTrait.Text == "kTraitFloat")
					toolTip1.SetToolTip(txtTrait, "FLOAT: accepts decimal values from -32000.0000 to 32000.0000.");
				else if (txtTrait.Text == "kTraitInt")
					toolTip1.SetToolTip(txtTrait, "INT: accepts integer (no decimal) values from -32000 to 32000.");
				else if (txtTrait.Text == "kTraitColor")
					toolTip1.SetToolTip(txtTrait, "COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors.");
			}
			catch { }
		}
		//cell input sanitization
		private void trackEditor_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			//during editing of a cell in trackEditor, check and sanitize input so it's numeric only
			e.Control.KeyPress -= new KeyPressEventHandler(NumericInputSanitize);
			if (trackEditor.CurrentCell.ColumnIndex != -1) //Desired Column
			{
				TextBox tb = e.Control as TextBox;
				if (tb != null) {
					tb.KeyPress += new KeyPressEventHandler(NumericInputSanitize);
				}
			}
		}
		//Cell value changed
		private void trackEditor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			trackEditor.CellValueChanged -= trackEditor_CellValueChanged;
			try {
				var _val = trackEditor.CurrentCell.Value;
				//iterate over each cell in the selection
				foreach (DataGridViewCell _cell in trackEditor.SelectedCells) {
					//if cell does not have the value, set it
					if (_cell.Value != _val)
						_cell.Value = _val;
					if (_cell.Value.ToString() == "")
						_cell.Value = null;

					TrackUpdateHighlightingSingleCell(_cell);
				}
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			catch { }

			ShowRawTrackData();
			trackEditor.CellValueChanged += trackEditor_CellValueChanged;
		}
		//Cell double click
		private void trackEditor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//checks if the cell's value contains a 0. If so, change the value to write to 1. If not, assume the cell value is already 0 and keep the value to write to 0
			string val;
			if (trackEditor.CurrentCell.Value == null)
				val = NUD_TrackDoubleclick.Value.ToString();
			else {
				val = null;
				trackEditor.CurrentCell.Style = null;
			}

			trackEditor.CurrentCell.Value = val;
		}
		//Keypress Backspace - clear selected cells
		private void trackEditor_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Back) {
				foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
					dgvc.Value = null;
					TrackUpdateHighlightingSingleCell(dgvc);
				}
				SaveLeaf(false);
			}
			e.Handled = true;
		}
		private void trackEditor_KeyDown(object sender, KeyEventArgs e)
		{
			//Keypress Delete - clear selected cellss
			//delete cell value if Delete key is pressed
			if (e.KeyCode == Keys.Delete) {
				foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
					dgvc.Value = null;
					TrackUpdateHighlightingSingleCell(dgvc);
				}
				SaveLeaf(false);
			}
			//copies selected cells
			if (e.Control && e.KeyCode == Keys.C) {
				DataObject d = trackEditor.GetClipboardContent();
				Clipboard.SetDataObject(d, true);
				e.Handled = true;
			}
			//pastes cell data from clipboard
			if (e.Control && e.KeyCode == Keys.V) {
				string s = Clipboard.GetText().Replace("\r\n", "\n");
				string[] lines = s.Split('\n');
				int row = trackEditor.CurrentCell.RowIndex;
				int col = trackEditor.CurrentCell.ColumnIndex;
				for (int _line = 0; _line < lines.Length; _line++) {
					if (row + _line >= trackEditor.RowCount)
						break;
					string[] cells = lines[_line].Split('\t');
					for (int i = 0; i < cells.Length; i++) {
						if (col + i >= trackEditor.ColumnCount)
							break;
						//don't paste if cell is blank
						if (cells[i] != "")
							trackEditor[col + i, row + _line].Value = cells[i];
					}
					TrackUpdateHighlighting(trackEditor.Rows[row + _line]);
				}
			}
		}
		//Clicking row headers to select the row
		private void trackEditor_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			trackEditor.CurrentCell = trackEditor.Rows[e.RowIndex].Cells[0];
		}
		///LEAF LENGTH
		private void numericUpDown_LeafLength_ValueChanged(object sender, EventArgs e)
		{
			_beats = (int)numericUpDown_LeafLength.Value;
			trackEditor.ColumnCount = _beats;
			GenerateColumnStyle(trackEditor, _beats);
			//set cell zoom
			trackZoom_Scroll(null, null);
			//make sure new cells follow the time sig
			TrackTimeSigHighlighting();
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///DROPDOWN OBJECTS
		private void dropObjects_SelectedValueChanged(object sender, EventArgs e)
		{
			//this gets triggered when the program starts, when no rows exist, and then it throws an error
			//this is only here to stop that
			try {
				label11.Text = "Lane";
				dropTrackLane.DataSource = new List<string>() { "lane left 2", "lane left 1", "lane center", "lane right 1", "lane right 2" };
				//when an object is chosen, unlock the param_path options and set datasource
				dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
				//switch index back and forth to trigger event
				dropParamPath.SelectedIndex = -1;
				dropParamPath.SelectedIndex = 0;
				dropParamPath.Enabled = true;
				//set default lane to 'middle'
				dropTrackLane.SelectedIndex = 2;

				if ((string)dropObjects.SelectedValue == "PLAY SAMPLE") {
					label11.Text = "Samples";
					LvlReloadSamples();
					dropTrackLane.DataSource = null;
					dropTrackLane.DataSource = _lvlsamples;
					dropTrackLane.SelectedIndex = -1;
				}
			}
			catch { };
		}
		///DROPDOWN PARAM_PATHS
		private void dropParamPath_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (dropParamPath.SelectedIndex != -1 && dropParamPath.Enabled) {
				//if the param_path is .ent, enable lane choice
				if (_objects.Where(obj => obj.param_displayname == dropParamPath.Text).First().param_path.EndsWith(".ent") || (string)dropObjects.SelectedValue == "PLAY SAMPLE") {
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
			//if the the selected param path can be set to a lane, keep Apply button disabled until a lane is selected
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
			File.WriteAllText(_loadedleaf, JsonConvert.SerializeObject(_save, Formatting.Indented));
			SaveLeaf(true);
			lblTrackFileName.Text = $"Leaf Editor - {_save["obj_name"]}";
			//update beat counts in loaded lvl if need be
			if (_loadedlvl != null)
				btnLvlRefreshBeats.PerformClick();
		}
		///LEAF - SAVE AS
		private void leafsaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Leaf File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				sfd.InitialDirectory = workingfolder ?? Application.StartupPath;
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
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save, Formatting.Indented));
					//set a few visual elementsto show what file is being worked on
					lblTrackFileName.Text = $"Leaf Editor - {_save["obj_name"]}";
					_loadedleaf = sfd.FileName;
					SaveLeaf(true);
					//update beat counts in loaded lvl if need be
					if (_loadedlvl != null)
						btnLvlRefreshBeats.PerformClick();
				}
			}
		}
		///LEAF - LOAD FILE
		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Leaf File (*.txt)|leaf_*.txt";
					ofd.Title = "Load a Thumper Leaf file";
					ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedleaftemp = ofd.FileName;
						var _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadLeaf(_load);
					}
				}
			}
		}
		/// LEAF - LOAD TEMPLATE
		private void leafTemplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Leaf File (*.txt)|leaf_*.txt";
					ofd.Title = "Load a Thumper Leaf file";
					//set folder to the templates location
					ofd.InitialDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}templates";
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedleaftemp = ofd.FileName;
						var _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadLeaf(_load);
						//set this to null, as it's a template. Next time on save, the user can save the file elsewhere
						_loadedleaf = null;
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
		///INTERP CHANGED
		private void dropLeafInterp_SelectedIndexChanged(object sender, EventArgs e)
		{
			_tracks[trackEditor.CurrentRow.Index].default_interp = dropLeafInterp.Text;
			SaveLeaf(false);
		}

		///DETECT SCROLL
		private void trackEditor_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.Type != ScrollEventType.EndScroll)
				return;

			var match = _scrollpositions.FindIndex(x => x.Item1 == leafobj);
			if (match != -1)
				_scrollpositions[match] = new Tuple<string, int, int>(leafobj, trackEditor.FirstDisplayedScrollingRowIndex, trackEditor.FirstDisplayedScrollingColumnIndex);
			else
				_scrollpositions.Add(new Tuple<string, int, int>(leafobj, trackEditor.FirstDisplayedScrollingRowIndex, trackEditor.FirstDisplayedScrollingColumnIndex));
		}
		private void AddScrollListener(DataGridView dgv, ScrollEventHandler scrollEventHandler)
		{
			HScrollBar scrollBar = dgv.Controls.OfType<HScrollBar>().First();
			VScrollBar vscrollBar = dgv.Controls.OfType<VScrollBar>().First();
			scrollBar.Scroll += scrollEventHandler;
			vscrollBar.Scroll += scrollEventHandler;
		}
		#endregion
		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnRawImport_Click(object sender, EventArgs e)
		{
			if (_loadedleaf == null)
				return;
			TrackRawImport(trackEditor.CurrentRow, JObject.Parse($"{{{richRawTrackData.Text}}}"));
		}

		private void btnTrackDelete_Click(object sender, EventArgs e)
		{
			bool _empty = true;
			//iterate over current row to see if any cells have data
			foreach (DataGridViewCell dgvc in trackEditor.SelectedCells) {
				if (dgvc.Value != null) {
					_empty = false;
					break;
				}
			}
			//if row is not empty, show confirmation box. Otherwise just delete the row
			if ((!_empty && MessageBox.Show("Some cells in the selected tracks have data. Are you sure you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes) || _empty) {
				try {
					var selectedrows = trackEditor.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
					foreach (DataGridViewRow dgvr in selectedrows) {
						_tracks.RemoveAt(dgvr.Index);
						trackEditor.Rows.Remove(dgvr);
					}
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

			_tracks.Add(new Sequencer_Object() { highlight_color = "-8355585", highlight_value = 1 });
			trackEditor.RowCount++;
			trackEditor.CurrentCell = trackEditor.Rows[_tracks.Count - 1].Cells[0];
			//disable Apply button if object is not set
			//dropObjects.SelectedIndex = 0;
			if (dropObjects.SelectedIndex == -1 || dropParamPath.SelectedIndex == -1)
				btnTrackApply.Enabled = false;
			else btnTrackApply.Enabled = true;
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}

		private void btnTrackUp_Click(object sender, EventArgs e)
		{
			List<Tuple<Sequencer_Object, DataGridViewRow, int>> _selectedtracks = new List<Tuple<Sequencer_Object, DataGridViewRow, int>>();
			DataGridView dgv = trackEditor;
			try {
				//finds each distinct row across all selected cells
				var selectedrows = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
				selectedrows.Sort((row, row2) => row.Index.CompareTo(row2.Index));
				var selectedcells = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => new { cell.ColumnIndex, cell.RowIndex }).ToList();
				foreach (DataGridViewRow dgvr in selectedrows) {
					//check if one of the rows is the top row. If it is, stop
					if (dgvr.Index == 0)
						return;

					_selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(_tracks[dgvr.Index], dgvr, dgvr.Index));
				}
				//iterate over rows and shift them up 1 index
				foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
					_tracks.Remove(_newtrack.Item1);
					dgv.Rows.Remove(_newtrack.Item2);
					_tracks.Insert(_newtrack.Item3 - 1, _newtrack.Item1);
					dgv.Rows.Insert(_newtrack.Item3 - 1, _newtrack.Item2);
				}
				//clear selected cells and shift them up
				dgv.ClearSelection();
				foreach (var cell in selectedcells) {
					dgv[cell.ColumnIndex, cell.RowIndex - 1].Selected = true;
				}
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			catch (Exception ex) { MessageBox.Show("Something unexpected happened. Show this error to the dev.\n" + ex, "Track move error"); }
		}

		private void btnTrackDown_Click(object sender, EventArgs e)
		{
			List<Tuple<Sequencer_Object, DataGridViewRow, int>> _selectedtracks = new List<Tuple<Sequencer_Object, DataGridViewRow, int>>();
			DataGridView dgv = trackEditor;
			try {
				//finds each distinct row across all selected cells
				var selectedrows = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
				selectedrows.Sort((row, row2) => row2.Index.CompareTo(row.Index));
				var selectedcells = dgv.SelectedCells.Cast<DataGridViewCell>().Select(cell => new { cell.ColumnIndex, cell.RowIndex }).ToList();
				foreach (DataGridViewRow dgvr in selectedrows) {
					//check if one of the rows is the top row. If it is, stop
					if (dgvr.Index >= dgv.RowCount - 1)
						return;

					_selectedtracks.Add(new Tuple<Sequencer_Object, DataGridViewRow, int>(_tracks[dgvr.Index], dgvr, dgvr.Index));
				}
				//iterate over rows and shift them up 1 index
				foreach (Tuple<Sequencer_Object, DataGridViewRow, int> _newtrack in _selectedtracks) {
					_tracks.Remove(_newtrack.Item1);
					dgv.Rows.Remove(_newtrack.Item2);
					_tracks.Insert(_newtrack.Item3 + 1, _newtrack.Item1);
					dgv.Rows.Insert(_newtrack.Item3 + 1, _newtrack.Item2);
				}
				//clear selected cells and shift them up
				dgv.ClearSelection();
				foreach (var cell in selectedcells) {
					dgv[cell.ColumnIndex, cell.RowIndex + 1].Selected = true;
				}
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			catch (Exception ex) { MessageBox.Show("Something unexpected happened. Show this error to the dev.\n" + ex, "Track move error"); }
		}

		private void btnTrackCopy_Click(object sender, EventArgs e)
		{
			DataGridView dgv = trackEditor;
			try {
				int _index = trackEditor.CurrentCell.RowIndex;
				clipboard_track = _tracks[_index];
				clipboard_row = (DataGridViewRow)dgv.Rows[_index].Clone();
				for (int i = 0; i < clipboard_row.Cells.Count; i++) {
					clipboard_row.Cells[i].Value = dgv.Rows[_index].Cells[i].Value;
				}
				btnTrackPaste.Enabled = true;
			}
			catch (Exception ex) { MessageBox.Show("something went wrong with copying. Show this error to the dev.\n\n" + ex); }
		}

		private void btnTrackPaste_Click(object sender, EventArgs e)
		{
			DataGridView dgv = trackEditor;
			DataGridViewRow _temp = (DataGridViewRow)clipboard_row.Clone();
			try {
				int _index = trackEditor.CurrentRow.Index;
				//check if copied row is longer than the leaf beat length
				if (clipboard_row.Cells.Count > _beats) {
					DialogResult _paste = MessageBox.Show("Copied track is longer than this leaf's beat count. Do you want to extend this leaf's beat count?\nYES = extend leaf and paste\nNO = paste, do not extend leaf\nCANCEL = do not paste", "Pasting leaf track", MessageBoxButtons.YesNoCancel);
					//YES = extend the leaf and then paste
					if (_paste == DialogResult.Yes) {
						numericUpDown_LeafLength.Value = clipboard_row.Cells.Count;
					}
					//NO = do not extend leaf and then paste
					else if (_paste == DialogResult.No) {
						while (_temp.Cells.Count > _beats)
							_temp.Cells.RemoveAt(_temp.Cells.Count - 1);
					}
					//CANCEL = do nothing
					else if (_paste == DialogResult.Cancel) {
						return;
					}
				}
				//add copied Sequencer_Object to main _tracks list
				_tracks.Insert(_index + 1, clipboard_track);
				//copy over values from clipboard to temp row, since clone() doesn't clone values
				for (int i = 0; i < _temp.Cells.Count; i++) {
					_temp.Cells[i].Value = clipboard_row.Cells[i].Value;
				}
				//add row to DGV
				dgv.Rows.Insert(_index + 1, _temp);
			}
			catch (Exception ex){ MessageBox.Show("something went wrong with pasting. Show this error to the dev.\n\n" + ex); }
			SaveLeaf(false);
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
			var objmatch = _objects.Where(obj => obj.category == dropObjects.Text && obj.param_displayname == dropParamPath.Text).First();
			//fill object properties on the form
			txtDefault.Text = objmatch.def;
			dropLeafStep.Text = objmatch.step;
			txtTrait.Text = objmatch.trait_type;
			//enable track highlighting tools
			btnTrackColorDialog.Enabled = true;
			NUD_TrackDoubleclick.Enabled = true;
			NUD_TrackHighlight.Enabled = true;
			//add track to list and populate with values
			_tracks[_selecttrack] = new Sequencer_Object() {
				obj_name = objmatch.obj_name,
				friendly_type = objmatch.category,
				param_path = objmatch.param_path,
				friendly_param = objmatch.param_displayname,
				_default = float.Parse(objmatch.def),
				step = objmatch.step,
				trait_type = objmatch.trait_type,
				highlight_color = _tracks[_selecttrack] != null ? _tracks[_selecttrack].highlight_color : "-8355585",
				highlight_value = 1,
				footer = objmatch.footer,
				default_interp = "kTraitInterpLinear"
			};
			//alter the data if it's a sample object being added. Save the sample name instead
			if ((string)dropObjects.SelectedValue == "PLAY SAMPLE")
				_tracks[_selecttrack].obj_name = dropTrackLane.SelectedValue.ToString() + ".samp";
			//if lane is not middle, edit the param_path and friendly_param to match
			if (_tracks[_selecttrack].param_path.Contains(".ent")) {
				_tracks[_selecttrack].param_path = _tracks[_selecttrack].param_path.Replace(".ent", _tracklane[dropTrackLane.SelectedIndex]);
				_tracks[_selecttrack].friendly_param += ", " + dropTrackLane.Text;
			}
			//change row header to reflect what the track is
			ChangeTrackName();
			SaveLeaf(false);
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
		/// This grabs the highlighting color from each track and then exports them into a file for use in later imports
		private void btnTrackColorExport_Click(object sender, EventArgs e)
		{
			string _out = "";
			//iterate over each track in the editor, writing its highlighting color to the _out string
			foreach (Sequencer_Object seq in _tracks) {
				_out += seq.highlight_color.ToString() + '\n';
			}
			using (var sfd = new SaveFileDialog()) {
				sfd.Filter = "Thumper Color Profile (*.color)|*.color";
				sfd.FilterIndex = 1;
				sfd.InitialDirectory = workingfolder ?? Application.StartupPath;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//save _out to file with .color extension
					File.WriteAllText(sfd.FileName, _out);
				}
			}
		}
		/// Imports colors from file to the current loaded leaf
		private void btnTrackColorImport_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Color Profile (*.color)|*.color";
				ofd.FilterIndex = 1;
				ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
				if (ofd.ShowDialog() == DialogResult.OK) {
					//import all colors in the file to this array
					var _colors = File.ReadAllLines(ofd.FileName);
					//then iterate over each track in the editor, applying the colors in the array in order
					for (int x = 0; x < _tracks.Count; x++) {
						if (x < _colors.Length)
							_tracks[x].highlight_color = _colors[x];
						//call this method to update the colors once the value has been assigned
						TrackUpdateHighlighting(trackEditor.Rows[x]);
                    }
                }
			}
		}

		private void btnLEafInterpLinear_Click(object sender, EventArgs e)
		{
			var _cells = trackEditor.SelectedCells;
			//interpolation requires 2 cells only
			if (_cells.Count != 2) {
				MessageBox.Show("Interpolation works with only 2 cells selected", "Interpolation error");
				return;
			}
			//check if cells are in the same row
			if (_cells[0].RowIndex != _cells[1].RowIndex) {
				MessageBox.Show("Interpolation works only if selected cells are in the same row", "Interpolation error");
				return;
			}

			//temporarily disable this so it doesn't trigger when cells update
			trackEditor.CellValueChanged -= trackEditor_CellValueChanged;

			List<DataGridViewCell> _listcell = new List<DataGridViewCell>() { _cells[0], _cells[1] };
			//if cell with higher column index is in [0], we need to move it to [1].
			//The order changes depending which cell is selected first
			if (_cells[0].ColumnIndex > _cells[1].ColumnIndex) {
				_listcell = new List<DataGridViewCell>() { _cells[1], _cells[0] };
			}
			//basic math to figure out the rate of change across the amount of beats between selections
			decimal _start = decimal.TryParse($"{_listcell[0].Value}", out decimal i) ? i : 0;
			decimal _end = decimal.TryParse($"{_listcell[1].Value}", out decimal j) ? j : 0;
			decimal _inc = _start;
			int _beats = _listcell[1].ColumnIndex - _listcell[0].ColumnIndex;
			decimal _diff = (_end - _start) / _beats;

			for (int x = 1; x < _beats; x++) {
				_inc = Decimal.Round(_inc + _diff, 4);
				//if interpolating for Color, remove the decimals
				if (_tracks[_listcell[0].RowIndex].trait_type == "kTraitColor")
					_inc = Math.Truncate(_inc);
				trackEditor[_listcell[0].ColumnIndex + x, _listcell[0].RowIndex].Value = _inc;
			}
			trackEditor[_listcell[1].ColumnIndex, _listcell[1].RowIndex].Value = _end;
			trackEditor[_listcell[0].ColumnIndex, _listcell[0].RowIndex].Value = _start;
			//recolor cells after populating
			TrackUpdateHighlighting(trackEditor.Rows[_listcell[0].RowIndex]);
			//re-enable this
			trackEditor.CellValueChanged += trackEditor_CellValueChanged;
		}

		private void btnLeafColors_Click(object sender, EventArgs e)
		{
			//do nothing if no cells selected
			if (trackEditor.SelectedCells.Count == 0)
				return;
			DialogResult result = colorDialog1.ShowDialog();
			if (result == DialogResult.OK) {
				foreach (DataGridViewCell _cell in trackEditor.SelectedCells) {
					if (_tracks[_cell.RowIndex].trait_type == "kTraitColor") {
						_cell.Value = colorDialog1.Color.ToArgb();
						_cell.Style.BackColor = colorDialog1.Color;
						//sets flag that leaf has unsaved changes
						SaveLeaf(false);
					}
				}
			}
		}

		private void btnLeafSplit_Click(object sender, EventArgs e)
		{
			//do nothing if no cells selected
			if (trackEditor.SelectedCells.Count == 0)
				return;
			if (trackEditor.SelectedCells.Count > 1) {
				MessageBox.Show("Select only 1 cell to be the split point", "Leaf split error");
				return;
			}
			//split leaf into 2 leafs
			int splitindex = trackEditor.CurrentCell.ColumnIndex;
			if (MessageBox.Show(@$"Split this leaf at beat {splitindex}? THIS CHANGE CANNOT BE UNDONE!", "Split leaf", MessageBoxButtons.YesNo) == DialogResult.No)
				return;

			string newfilename = "";
			//create file renaming dialog and show it
			FileNameDialog filenamedialog = new FileNameDialog();
			filenamedialog.StartPosition = FormStartPosition.Manual;
			filenamedialog.Location = MousePosition;
			if (filenamedialog.ShowDialog() == DialogResult.Yes) {
				newfilename = filenamedialog.txtWorkingRename.Text;
				//check if the chosen name exists in the level folder
				if (File.Exists($@"{workingfolder}\leaf_{newfilename}.txt")) {
					MessageBox.Show("File name already exists.", "Leaf split error");
					return;
                }
			}
			//if NOT yes, return and skip everything else below
			else
				return;

			///SPLIT THAT LEAF
			//build the leaf JSON so we can manipulate it
			var _leafsplitbefore = LeafBuildSave(Path.GetFileName(_loadedleaf).Replace("leaf_", ""));
			//enumerate over each sequencer object and it's values to figure out which ones to keep
			foreach (JObject seq_obj in _leafsplitbefore["seq_objs"]) {             
				//data_points contains a list of all data points. By getting Properties() of it,
				//each point becomes its own index
				var data_points = ((JObject)seq_obj["data_points"]).Properties().ToList();
				//iterate over each data point. If it's less than the splitindex, add it to a new list
				JObject newdata = new JObject();
				foreach (JProperty data_point in data_points) {
					if (int.Parse(data_point.Name) < splitindex)
						newdata.Add(data_point.Name, data_point.Value);
				}
				seq_obj.Remove("data_points");
				seq_obj.Add("data_points", newdata);
			}
			//set new beat count
			_leafsplitbefore.Remove("beat_cnt");
			_leafsplitbefore.Add("beat_cnt", splitindex);
			//write data back to file
			File.WriteAllText(_loadedleaf, JsonConvert.SerializeObject(_leafsplitbefore, Formatting.Indented));

			///repeat all above for after split file
			var _leafsplitafter = LeafBuildSave(newfilename + ".txt");
			foreach (JObject seq_obj in _leafsplitafter["seq_objs"]) {
				var data_points = ((JObject)seq_obj["data_points"]).Properties().ToList();
				JObject newdata = new JObject();
				foreach (JProperty data_point in data_points) {
					if (int.Parse(data_point.Name) >= splitindex)
						//shift beats back to starting position
						newdata.Add((int.Parse(data_point.Name) - splitindex).ToString(), data_point.Value);
				}
				seq_obj.Remove("data_points");
				seq_obj.Add("data_points", newdata);
			}
			//set new beat count
			_leafsplitafter.Remove("beat_cnt");
			_leafsplitafter.Add("beat_cnt", numericUpDown_LeafLength.Value - splitindex);
			//write data back to file
			File.WriteAllText($@"{workingfolder}\leaf_{newfilename}.txt", JsonConvert.SerializeObject(_leafsplitafter, Formatting.Indented));

			//load new leaf that was just split
			workingfolderFiles.Rows.Insert(workingfolderFiles.CurrentRow.Index + 1, new[] { Properties.Resources.ResourceManager.GetObject("leaf"), "leaf_" + newfilename });
			workingfolderFiles.Rows[workingfolderFiles.CurrentRow.Index + 1].Cells[1].Selected = true;
			
			//update beat counts in loaded lvl if need be
			if (_loadedlvl != null)
				btnLvlRefreshBeats.PerformClick();
		}

		private void btnLeafObjRefresh_Click(object sender, EventArgs e)
		{
			ImportObjects();
		}

		private void lblRawData_Click(object sender, EventArgs e)
		{
			if (panelRawData.Height > 20) {
				panelRawData.Height = 20;
				panelRawData.Location = new Point(panelRawData.Location.X, panelLeaf.Height - 20);
				lblRawData.Text = "▲";
				trackEditor.Height += 48;
				foreach (Control c in panelRawData.Controls)
					c.Visible = false;
				lblRawData.Visible = true;
			}
			else {
				panelRawData.Height = 68;
				panelRawData.Location = new Point(panelRawData.Location.X, panelLeaf.Height - 68);
				lblRawData.Text = "▼";
				trackEditor.Height -= 48;
				foreach (Control c in panelRawData.Controls)
					c.Visible = true;
			}
		}

		/// These buttons exist on the Workingfolder panel
		private void btnLeafPanelNew_Click(object sender, EventArgs e) => leafnewToolStripMenuItem.PerformClick();
		private void btnLeafPanelOpen_Click(object sender, EventArgs e) => leafloadToolStripMenuItem.PerformClick();
		private void btnLeafPanelTemplate_Click(object sender, EventArgs e) => leafTemplateToolStripMenuItem.PerformClick();
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void SaveLeaf(bool save)
		{
			//make the beeble emote
			pictureBox1_Click(null, null);
			//skip method if leaf is loading
			if (loadingleaf)
				return;

			_saveleaf = save;
			if (!save) {
				if (!lblTrackFileName.Text.Contains("(unsaved)"))
					lblTrackFileName.Text += " (unsaved)";
				btnSaveLeaf.Location = new Point(lblTrackFileName.Location.X + lblTrackFileName.Size.Width, btnSaveLeaf.Location.Y);
				btnSaveLeaf.Enabled = true;
				lblTrackFileName.BackColor = Color.Maroon;
			}
			else {
				lblTrackFileName.Text = lblTrackFileName.Text.Replace(" (unsaved)", "");
				btnSaveLeaf.Location = new Point(lblTrackFileName.Location.X + lblTrackFileName.Size.Width, btnSaveLeaf.Location.Y);
				btnSaveLeaf.Enabled = false;
				lblTrackFileName.BackColor = Color.FromArgb(40, 40, 40);
			}
		}

		public void InitializeTracks(DataGridView grid, bool columnstyle)
		{
			//double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
			//used to significantly improve rendering performance
			if (!SystemInformation.TerminalServerSession) {
				Type dgvType = grid.GetType();
				PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
				pi.SetValue(grid, true, null);
			}

			if (columnstyle)
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
				grid.Columns[i].ValueType = typeof(decimal);
				grid.Columns[i].DefaultCellStyle.Format = "0.######";
			}
		}
		///Import raw text from rich text box to selected row
		public void TrackRawImport(DataGridViewRow r, JObject _rawdata)
		{
			trackEditor.CellValueChanged -= trackEditor_CellValueChanged;
			//_rawdata contains a list of all data points. By getting Properties() of it,
			//each point becomes its own index
			var data_points = _rawdata.Properties().ToList();
			//check if the last data point is beyond the beat count. If it is, it will crash or not be included in the track editor
			//Ask the user if they want to expand the leaf to accomadate the data point
			if (data_points.Count > 0 && int.Parse(((JProperty)data_points.Last()).Name) >= r.Cells.Count) {
				if (MessageBox.Show($"Your last data point is beyond the leaf's beat count. Do you want to lengthen the leaf? If you do not, the data point will be left out.\nObject: {r.HeaderCell.Value}\nData point: {data_points.Last()}", "Leaf too short", MessageBoxButtons.YesNo) == DialogResult.Yes)
					numericUpDown_LeafLength.Value = int.Parse(((JProperty)data_points.Last()).Name) + 1;
			}
			//iterate over each data point, and fill cells
			foreach (JProperty data_point in data_points) {
				try {
					r.Cells[int.Parse(data_point.Name)].Value = (decimal)data_point.Value;
				}
				catch (ArgumentOutOfRangeException) { }
			}
			trackEditor.CellValueChanged += trackEditor_CellValueChanged;
		}
		///Updates row headers to be the Object and Param_Path
		public void ChangeTrackName()
		{
			if ((string)dropObjects.SelectedValue == "PLAY SAMPLE")
				//show the sample name instead
				trackEditor.CurrentRow.HeaderCell.Value = _tracks[_selecttrack].friendly_type + " (" + _tracks[_selecttrack].obj_name + ")";
			else
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
						_out += $"{x}:{trackEditor.Rows[_selecttrack].Cells[x].Value},";
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
			//grab the first part of the time sig. This represents how many beats are in a bar
			int beats = int.Parse(dropTimeSig.SelectedValue.ToString().Split('/')[0]);
			for (int i = 0; i < _beats; i++) {
				//whenever `i` is a multiple of the time sig, switch colors
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
			//iterate over all cells in the row
			foreach (DataGridViewCell dgvc in r.Cells) {
				TrackUpdateHighlightingSingleCell(dgvc);
			}
		}
		public void TrackUpdateHighlightingSingleCell(DataGridViewCell dgvc)
		{
			var i = dgvc.Value;
			dgvc.Style = null;
			if (i == null)
				return;

			//if it is kTraitColor, color the background differently
			if (_tracks[dgvc.RowIndex].trait_type == "kTraitColor") {
				dgvc.Style.BackColor = Color.FromArgb(int.Parse(i.ToString()));
				return;
            }

			//if the cell value is greater than the criteria of the row, highlight it with that row's color
			if (Math.Abs(decimal.Parse(i.ToString())) >= (decimal)_tracks[dgvc.RowIndex].highlight_value) {
				dgvc.Style.BackColor = Color.FromArgb(int.Parse(_tracks[dgvc.RowIndex].highlight_color));
			}
			//change cell font color so text is readable on dark/light backgrounds
			Color _c = dgvc.Style.BackColor;
			if (_c.R < 150 && _c.G < 150 && _c.B < 150)
				dgvc.Style.ForeColor = Color.White;
			else
				dgvc.Style.ForeColor = Color.Black;
		}

		///Update DGV from _tracks
		public void LoadLeaf(dynamic _load /*List<string> _load*/)
		{
			//reset flag in case it got stuck previously
			loadingleaf = false;
			//if Leaf Editor is hidden, show it when a leaf is selected
			if (panelLeaf.Visible == false)
				leafEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Leaf or not
			if ((string)_load["obj_type"] != "SequinLeaf") {
				MessageBox.Show("This does not appear to be a leaf file!", "Leaf not loaded");
				return;
			}
			//if the check above succeeds, then set the _loadedleaf to the string temp saved from ofd.filename
			if (_load["obj_name"] == null) {
				MessageBox.Show("Leaf missing obj_name parameter. Please set it in the txt file and then reload.", "Leaf not loaded");
				return;
			}
			lblTrackFileName.Text = $@"Leaf Editor - {_load["obj_name"]}";
			leafobj = _load["obj_name"];
			//set flag that load is in progress. This skips SaveLeaf() method
			loadingleaf = true;
			workingfolder = Path.GetDirectoryName(_loadedleaftemp);
			_loadedleaf = _loadedleaftemp;
			//clear existing tracks
			_tracks.Clear();
			trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			//set beat_cnt and time_sig
			numericUpDown_LeafLength.Value = (int?)_load["beat_cnt"] ?? 0;
			var _time_sig = (string)_load["time_sig"] ?? "4/4";
			dropTimeSig.SelectedIndex = dropTimeSig.FindStringExact(_time_sig);
			//each object in the seq_objs[] list becomes a track
			foreach (var seq_obj in _load["seq_objs"]) {
				Sequencer_Object _s = new Sequencer_Object() {
					obj_name = seq_obj["obj_name"],
					trait_type = seq_obj["trait_type"],
					data_points = seq_obj["data_points"],
					step = seq_obj["step"],
					_default = seq_obj["default"],
					footer = seq_obj["footer"].GetType() == typeof(JArray) ? String.Join(",", ((JArray)seq_obj["footer"]).ToList()) : ((string)seq_obj["footer"]).Replace("[", "").Replace("]", "")
				};
				//if the leaf has definitions for these, add them. If not, set to defaults
				_s.param_path = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : (string)seq_obj["param_path"];
				_s.highlight_color = (string)seq_obj["editor_data"]?[0] ?? "-8355585";
				_s.highlight_value = (int?)seq_obj["editor_data"]?[1] ?? 1;
				_s.default_interp = (string)seq_obj["default_interp"] ?? "kTraitInterpLinear";
				//if object is a .samp, set the friendly_param and friendly_type since they don't exist in _objects
				if (_s.param_path == "play") {
					_s.friendly_type = "PLAY SAMPLE";
					_s.friendly_param = _s.param_path;
				}
				//otherwise, search _objects for the friendly names for display purposes
				else {
					var reg_param = Regex.Replace(_s.param_path, "[.].*", ".ent");
					var objmatch = _objects.Where(obj => obj.param_path == reg_param && obj.obj_name == _s.obj_name.Replace((string)_load["obj_name"], "leafname")).First();
					_s.friendly_param = objmatch.param_displayname;
					_s.friendly_type = objmatch.category;
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
			trackEditor.RowHeadersVisible = true;
			foreach (DataGridViewRow r in trackEditor.Rows) {
				try {
					if (_tracks[r.Index].friendly_param.Length > 1) {
						if (_tracks[r.Index].param_path == "play")
							r.HeaderCell.Value = $"{_tracks[r.Index].friendly_type} ({_tracks[r.Index].obj_name})";
						else
							r.HeaderCell.Value = $"{_tracks[r.Index].friendly_type} ({_tracks[r.Index].friendly_param})";
						//pass _griddata per row to be imported to the DGV
						TrackRawImport(r, _tracks[r.Index].data_points);
						TrackUpdateHighlighting(r);
					}
				}
				catch (Exception ex) { MessageBox.Show($"{_load["obj_name"]} contains an object that doesn't exist:\n{_tracks[r.Index].obj_name}\n\n{ex}"); }
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
			btnTrackClear.Enabled = _tracks.Count > 0;
			btnTrackCopy.Enabled = _tracks.Count > 0;
			//set save flag to true, since it just barely loaded
			loadingleaf = false;
			SaveLeaf(true);
			//re-set the zoom level
			trackZoom_Scroll(null, null);
			//set scrollbar positions (if set last time this leaf was open)
			trackEditor.RowHeadersWidth = trackEditor.RowHeadersWidth;
			trackEditor.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
			trackEditor.FirstDisplayedScrollingRowIndex = 0;
			trackEditor.FirstDisplayedScrollingColumnIndex = 0;
			var match = _scrollpositions.FindIndex(x => x.Item1 == leafobj);
			if (match != -1) {
				trackEditor.FirstDisplayedScrollingRowIndex = _scrollpositions[match].Item2;
				trackEditor.FirstDisplayedScrollingColumnIndex = _scrollpositions[match].Item3;
			}
		}

		public JObject LeafBuildSave(string _leafname)
		{
			_leafname = Regex.Replace(_leafname, "[.].*", ".leaf");
            ///start building JSON output
            JObject _save = new JObject
            {
                { "obj_type", "SequinLeaf" },
                { "obj_name", _leafname }
            };
			
            JArray seq_objs = new JArray();
			foreach (Sequencer_Object seq_obj in _tracks) {
				//skip blank tracks
				if (seq_obj.friendly_param == null)
					continue;
				JObject s = new JObject();
				//if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
				if (seq_obj.obj_name.Contains(".leaf"))
					seq_obj.obj_name = (string)_save["obj_name"];
				s.Add("obj_name", seq_obj.obj_name.Replace("leafname", (string)_save["obj_name"]));
				//write param_path or param_path_hash
				if (seq_obj.param_path.StartsWith("0x"))
					s.Add("param_path_hash", seq_obj.param_path.Replace("0x", ""));
				else
					s.Add("param_path", seq_obj.param_path);
				s.Add("trait_type", seq_obj.trait_type);
				s.Add("default_interp", seq_obj.default_interp);
				///start building all data points into an object
				JObject data_points = new JObject();
				for (int x = 0; x < trackEditor.ColumnCount; x++) {
					if (!string.IsNullOrEmpty(trackEditor[x, _tracks.IndexOf(seq_obj)].Value?.ToString()))
						data_points.Add(x.ToString(), decimal.Parse(trackEditor[x, _tracks.IndexOf(seq_obj)].Value.ToString()));
				}
				s.Add("data_points", data_points);
				///end
				//add the rest of the keys to this seq_obj
				s.Add("step", seq_obj.step);
				s.Add("default", seq_obj._default);
				s.Add("footer", seq_obj.footer);
				s.Add("editor_data", new JArray() { new object[] { seq_obj.highlight_color, seq_obj.highlight_value } });

				seq_objs.Add(s);
			}
			//add all seq_objs to the overall leaf
			_save.Add("seq_objs", seq_objs);
			//end leaf with final keys
			_save.Add("beat_cnt", (int)numericUpDown_LeafLength.Value);
			_save.Add("time_sig", dropTimeSig.Text);
			///end building JSON output

			return _save;
		}
		#endregion
	}
}