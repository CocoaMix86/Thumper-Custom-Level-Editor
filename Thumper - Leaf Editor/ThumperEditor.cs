using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Global
		///Toolstrip - ABOUT
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
		///Toolstrip - INTERPOLATOR
		private void interpolatorToolStripMenuItem_Click(object sender, EventArgs e) => new Interpolator().Show();
		///Toolstrip - VIEW MENU
		//Visible - LEaf Editor
		private void leafEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelLeaf.Visible = leafEditorToolStripMenuItem.Checked;
			PanelVisibleResize();
		}
		//Visible - Level Editor
		private void levelEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelLevel.Visible = levelEditorToolStripMenuItem.Checked;
			PanelVisibleResize();
		}
		//Visible - Master Editor
		private void masterEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelMaster.Visible = masterEditorToolStripMenuItem.Checked;
			PanelVisibleResize();
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}
		///FORM CLOSING - check if anything is unsaved
		private void FormLeafEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_saveleaf || !_savelvl)
			{
				if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					e.Cancel = true;
			}
		}

		#endregion

		#region Leaf_Editor
		#region Variables
		bool _saveleaf = true;

		int _beats = 0;
		int _selecttrack = 0;

		string _errorlog = "";
		string _loadedleaf = "";

		public List<List<string>> _tracks = new List<List<string>>();
		public List<Object_Params> _objects = new List<Object_Params>();
		public List<string> _timesig = new List<string>() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
		public List<string> _tracklane = new List<string>() { ".a01", ".a02", ".ent", ".z01", ".z02"};
		#endregion

		public FormLeafEditor()
		{
			InitializeComponent();
		}
		#region EventHandlers
		///        ///
		/// EVENTS ///
		///        ///

		///FORM
		private void FormLeafEditor_Load(object sender, EventArgs e)
		{
			//setup datagrids with proper formatting
			InitializeTracks(trackEditor);
			InitializeTracks(lvlSeqObjs);
			InitializeTracks(lvlLeafList);
			InitializeLvlStuff();

			///import selectable objects from file and parse them into lists for manipulation
			//splits input at "###". Each section is a collection of param_paths
			var import = (Properties.Resources.track_objects).Replace("\r\n","\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
			for (int x = 0; x < import.Count; x++) {
				//split each section into individual lines
				var import2 = import[x].Split('\n').ToList();
				//initialise class so we can add to it
				Object_Params objpar = new Object_Params() { 
					//name of object is the first line of every set
					obj_displayname = import2[0],
					obj_name = new List<string>(),
					param_displayname = new List<string>(),
					param_path = new List<string>(),
					trait_type = new List<string>(),
					step = new List<string>(),
					def = new List<string>(),
					footer = new List<string>()
				};
				
				for (int y = 2; y < import2.Count - 1; y++) {
					//split each line by ';'. Now each property is separated
					var import3 = import2[y].Split(';');
					try {
						objpar.obj_name.Add(import3[0]);
						objpar.param_displayname.Add(import3[1]);
						objpar.param_path.Add(import3[2]);
						objpar.trait_type.Add(import3[3]);
						objpar.step.Add(import3[4]);
						objpar.def.Add(import3[5]);
						objpar.footer.Add(import3[6]);
					} catch {
						_errorlog += "failed to import all properties of param_path " + import3[0] + " of object " + objpar.obj_name + ".\n";
					}
				}
				//finally, add complete object and values to list
				_objects.Add(objpar);
			}
			//show errors to user if any imports failed
			if (_errorlog.Length > 1) MessageBox.Show(_errorlog);
			_errorlog = "";
			//customize combobox to display the correct content
			dropObjects.DisplayMember = "obj_displayname";
			dropObjects.ValueMember = "obj_displayname";
			dropObjects.DataSource = _objects;
			dropParamPath.Enabled = false;
			//set timesig datasource
			dropTimeSig.DataSource = _timesig;
			//ping the beat counter to update column count
			numericUpDown_LeafLength_ValueChanged(null, null);
			//tell the app that everything is saved, because it just initialized
			SaveLeaf(true);


			lblLevelMax_Click(null, null);
		}
		///FORM RESIZE
		private void FormLeafEditor_Resize(object sender, EventArgs e)
		{
		}
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
				if (_tracks[_selecttrack][3].Contains("right") || _tracks[_selecttrack][3].Contains("left") || _tracks[_selecttrack][3].Contains("middle")) {
					_params = _tracks[_selecttrack][3].Split(new string[] { ", " }, StringSplitOptions.None).ToList();
				}
				else
					_params = new List<string>() { _tracks[_selecttrack][3], "middle"};
				//set all controls to their values stored in _tracks
				dropObjects.SelectedIndex = dropObjects.FindStringExact(_tracks[_selecttrack][1]);
				dropParamPath.SelectedIndex = dropParamPath.FindStringExact(_params[0]);
				dropTrackLane.SelectedIndex = dropTrackLane.FindStringExact(_params[1]);
				txtStep.Text = _tracks[_selecttrack][5];
				txtTrait.Text = _tracks[_selecttrack][6];
				btnTrackColorDialog.BackColor = Color.FromArgb(int.Parse(_tracks[_selecttrack][7]));
				//remove event handlers from a few controls so they don't trigger when their values change
				NUD_TrackDoubleclick.ValueChanged -= new EventHandler(NUD_TrackDoubleclick_ValueChanged);
				NUD_TrackHighlight.ValueChanged -= new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged -= new EventHandler(txtDefault_ValueChanged);
				//set values from _tracks
				NUD_TrackDoubleclick.Value = Decimal.Parse(_tracks[_selecttrack][8]);
				NUD_TrackHighlight.Value = Decimal.Parse(_tracks[_selecttrack][9]);
				txtDefault.Value = Decimal.Parse(_tracks[_selecttrack][4]);
				//re-add event handlers
				NUD_TrackDoubleclick.ValueChanged += new EventHandler(NUD_TrackDoubleclick_ValueChanged);
				NUD_TrackHighlight.ValueChanged += new EventHandler(NUD_TrackHighlight_ValueChanged);
				txtDefault.ValueChanged += new EventHandler(txtDefault_ValueChanged);

				//check if the current track has param_path set. If not, disable some controls
				if (_tracks[_selecttrack][0] != null)
					return;
				btnTrackColorDialog.Enabled = false;
				NUD_TrackDoubleclick.Enabled = false;
				NUD_TrackHighlight.Enabled = false;
			} catch {  }
		}
		//Cell value changed
		private void trackEditor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			ShowRawTrackData();
			var _val = trackEditor.CurrentCell.Value;

			try {
				//iterate over each cell in the selection
				foreach (DataGridViewCell _cell in trackEditor.SelectedCells) {
					//if cell does not have the value, set it
					if (_cell.Value != _val)
						_cell.Value = _val;
					if (_cell.Value.ToString() == "")
						_cell.Value = null;

					decimal i = 0;
					//clear cell styling, in case it now falls out of highlighting scope
					_cell.Style = null;
					//try parsing the value. If it doesn't work, i = 0
					i = decimal.TryParse(_cell.Value.ToString(), out i) ? i : 0;
					//check Absolute value of cell against highlughting value of track
					//we check Absolute because this catches negatives too
					if (Math.Abs(i) >= decimal.Parse(_tracks[_cell.RowIndex][9]))
						_cell.Style.BackColor = Color.FromArgb(int.Parse(_tracks[_cell.RowIndex][7]));

					//sets flag that leaf has unsaved changes
					SaveLeaf(false);
				}
			} catch { }
		}
		//Cell double click
		private void trackEditor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//checks if the cell's value contains a 0. If so, change the value to write to 1. If not, assume the cell value is already 0 and keep the value to write to 0
			string val;
			if (trackEditor.CurrentCell.Value == null || trackEditor.CurrentCell.Value.ToString() != _tracks[_selecttrack][8])
				val = _tracks[_selecttrack][8];
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
		//Keypress Delete - clear selected cells
		private void trackEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) {
				foreach (DataGridViewCell dgvc in trackEditor.SelectedCells)
					dgvc.Value = null;
				TrackUpdateHighlighting(trackEditor.CurrentRow);
			}
			e.Handled = true;
		}
		///PANEL LABELS - change size or close
		private void lblMasterClose_Click(object sender, EventArgs e) => masterEditorToolStripMenuItem.PerformClick();
		private void lblLvlClose_Click(object sender, EventArgs e) => levelEditorToolStripMenuItem.PerformClick();
		private void lblLeafClose_Click(object sender, EventArgs e) => leafEditorToolStripMenuItem.PerformClick();
		private void lblLeafMax_Click(object sender, EventArgs e)
		{
			panelMaster.Width = Math.Min(270, this.Width / 4);
			panelLevel.Width = Math.Min(270, this.Width / 4);
			PanelVisibleResize();
		}
		private void lblLevelMax_Click(object sender, EventArgs e)
		{
			panelMaster.Width = Math.Min(270, this.Width / 4);
			panelLevel.Width = (this.Width / 2) - 12;
			PanelVisibleResize();
		}
		private void lblMasterMax_Click(object sender, EventArgs e)
		{
			panelMaster.Width = (this.Width / 2) - 12;
			panelLevel.Width = Math.Min(270, this.Width / 4);
			PanelVisibleResize();
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
			} catch { };
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
			_tracks[trackEditor.CurrentRow.Index][9] = NUD_TrackHighlight.Value.ToString();
			TrackUpdateHighlighting(trackEditor.CurrentRow);
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///NUMERIC_UPDOWN TRACK DOUBLECLICK VALUE
		private void NUD_TrackDoubleclick_ValueChanged(object sender, EventArgs e)
		{
			_tracks[_selecttrack][8] = NUD_TrackDoubleclick.Value.ToString();
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		///LEAF - SAVE FILE
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string _save = "";
			_save += numericUpDown_LeafLength.Value + ";" + dropTimeSig.SelectedValue + '\n';
			foreach (List<string> _track in _tracks) {
				string _out = "";
				_save += String.Join(";", _track.ToArray());
				_save += '#';

				for (int x = 0; x < trackEditor.ColumnCount; x++) {
					if (!string.IsNullOrEmpty(trackEditor.Rows[_tracks.IndexOf(_track)].Cells[x].Value?.ToString()))
						_out += x + ":" + trackEditor.Rows[_tracks.IndexOf(_track)].Cells[x].Value + ",";
				}
				_save += _out + '\n';
			}

			using (var sfd = new SaveFileDialog()) {
				sfd.Filter = "Thumper Editor Leaf Tracks (*.teleaf)|*.teleaf";
				sfd.FilterIndex = 1;

				if (sfd.ShowDialog() == DialogResult.OK) {
					File.WriteAllText(sfd.FileName, _save);
					lblTrackFileName.Text = "Leaf Editor - " + Path.GetFileName(sfd.FileName);
					_loadedleaf = Path.GetFileName(sfd.FileName).Replace(".teleaf", "");
					SaveLeaf(true);
				}
			}
		}
		///LEAF - LOAD FILE
		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				_tracks.Clear();
				List<string> _load;

				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Editor Leaf Tracks (*.teleaf)|*.teleaf";
					ofd.Title = "Load a Thumper Editor Leaf file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						_loadedleaf = Path.GetFileName(ofd.FileName);
						lblTrackFileName.Text = "Leaf Editor - " + _loadedleaf;
						_load = File.ReadAllLines(ofd.FileName).ToList();
						LoadLeaf(_load);
					}
				}
			}
		}
		///LEAF - EXPORT TO leaf_*.txt
		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_loadedleaf == "") {
				MessageBox.Show("Please save your leaf before exporting");
				return;
			}

			string _export = "[\n{\n" +
				"'obj_type': 'SequinLeaf',\n" +
				"'obj_name': '" + _loadedleaf.Replace(".teleaf","") + ".leaf',\n" +
				"'seq_objs': [\n\n";

			foreach (List<string> _ls in _tracks) {
				if (_ls[0].Length > 1) {
					_export += "{\n";
					_export += "'obj_name': '" + _ls[0] + "',\n";
					_export += "'param_path': '" + _ls[2] + "',\n";
					_export += "'trait_type': '" + _ls[6] + "',\n";
					_export += "'data_points': {\n";
					
					for (int x = 0; x < trackEditor.ColumnCount; x++) {
						if (!string.IsNullOrEmpty(trackEditor.Rows[_tracks.IndexOf(_ls)].Cells[x].Value?.ToString()))
							_export += x + ":" + trackEditor.Rows[_tracks.IndexOf(_ls)].Cells[x].Value + ",";
					}

					_export += "\n},\n";
					_export += "'step': " + _ls[5] + ",\n";
					_export += "'default': " + _ls[4] + ",\n";
					_export += "'footer': " + _ls[10];
					_export += "\n},\n";
				}
			}

			_export += "],\n\n" +
				"'beat_cnt': " + _beats + "\n}\n]";

			_export = _export.Replace("leafname", _loadedleaf.Replace(".teleaf","") + ".leaf");

			using (var sfd = new SaveFileDialog()) {
				sfd.Filter = "Thumper Editor Leaf File (*.txt)|*.txt";
				sfd.FilterIndex = 1;

				if (sfd.ShowDialog() == DialogResult.OK) {
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					if (tempFileName.Substring(0, 5) != "leaf_")
						sfd.FileName = storePath + "\\leaf_" + tempFileName;
					File.WriteAllText(sfd.FileName, _export);

					MessageBox.Show("Leaf successfully exported as '" + Path.GetFileName(sfd.FileName) + "'.");
				}
			}
		}
		///DEFAULT TRACK VALUE CHANGED
		private void txtDefault_ValueChanged(object sender, EventArgs e)
		{
			_tracks[trackEditor.CurrentRow.Index][4] = txtDefault.Value.ToString();
			//sets flag that leaf has unsaved changes
			SaveLeaf(false);
		}
		#endregion
		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnRawImport_Click(object sender, EventArgs e)
		{
			TrackRawImport(trackEditor.CurrentRow, richRawTrackData.Text);
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

			_tracks.Add(new List<string>() { "", "", "", "", "", "", "", "", "-8355585", "1", "1", ""});
			trackEditor.RowCount++;
			trackEditor.CurrentCell = trackEditor.Rows[_tracks.Count - 1].Cells[0];
			//disable Apply button if object is not set
			dropObjects.SelectedIndex = 0;
			if (dropObjects.SelectedIndex == -1 || dropParamPath.SelectedIndex == -1)
				btnTrackApply.Enabled = false;
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
				List<string> selectedTrack = _tracks[rowIndex];
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
				List<string> selectedTrack = _tracks[rowIndex];
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
			txtStep.Text = _objects[dropObjects.SelectedIndex].step[dropParamPath.SelectedIndex];
			txtTrait.Text = _objects[dropObjects.SelectedIndex].trait_type[dropParamPath.SelectedIndex];
			//enable track highlighting tools
			btnTrackColorDialog.Enabled = true;
			NUD_TrackDoubleclick.Enabled = true;
			NUD_TrackHighlight.Enabled = true;
			//add track to list and populate with values
			_tracks[_selecttrack] = new List<string>() { 
				_objects[dropObjects.SelectedIndex].obj_name[dropParamPath.SelectedIndex],
				_objects[dropObjects.SelectedIndex].obj_displayname,
				_objects[dropObjects.SelectedIndex].param_path[dropParamPath.SelectedIndex],
				_objects[dropObjects.SelectedIndex].param_displayname[dropParamPath.SelectedIndex],
				_objects[dropObjects.SelectedIndex].def[dropParamPath.SelectedIndex],
				_objects[dropObjects.SelectedIndex].step[dropParamPath.SelectedIndex],
				_objects[dropObjects.SelectedIndex].trait_type[dropParamPath.SelectedIndex],
				"-8355585",
				"1",
				"1",
				_objects[dropObjects.SelectedIndex].footer[dropParamPath.SelectedIndex]
			};
			if (_tracks[_selecttrack][2].Contains(".ent")) {
				_tracks[_selecttrack][2] = _tracks[_selecttrack][2].Replace(".ent", _tracklane[dropTrackLane.SelectedIndex]);
				_tracks[_selecttrack][3] += ", " + dropTrackLane.Text;
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
				_tracks[_selecttrack][7] = colorDialog1.Color.ToArgb().ToString();
				//sets flag that leaf has unsaved changes
				SaveLeaf(false);
			}
			//call method to update coloring of track
			TrackUpdateHighlighting(trackEditor.CurrentRow);
		}
		///Sets up new leaf file
		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				_tracks.Clear();
				trackEditor.Rows.Clear();
				lblTrackFileName.Text = "Leaf Editor";
			}
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
			grid.DefaultCellStyle.Font = new Font(new FontFamily("Arial"), 12, FontStyle.Bold);
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
		public void TrackRawImport(DataGridViewRow r, string _rawdata)
		{
			var _in = _rawdata.Replace("\n","").Split(',');

			float _f = float.Parse(_tracks[r.Index][9]);
			Color _color = Color.FromArgb(int.Parse(_tracks[r.Index][7]));

			//remove any empty lines in the array
			_in = _in.Where(x => !string.IsNullOrEmpty(x)).ToArray();
			foreach (string s in _in) {
				//track data is stored as 'beat:value'. By splitting on ':', we get the index for the cell (_data[0]), and the value for the cell (_data[1])
				var _data = s.Split(':');
				r.Cells[int.Parse(_data[0])].Value = _data[1];
				try {
					//for each cell being imported, highlight it if it matches track highlighting value
					if (Math.Abs(float.Parse(_data[1])) >= _f) {
						r.Cells[int.Parse(_data[0])].Style.BackColor = _color;
					}
				} catch { }
			}
		}
		///Updates row headers to be the Object and Param_Path
		public void ChangeTrackName()
		{
			trackEditor.CurrentRow.HeaderCell.Value = _tracks[_selecttrack][1] + " (" + _tracks[_selecttrack][3] + ")";
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
			for (int x = 0; x < trackEditor.ColumnCount; x++) {
				//remove any current styling, in case it now falls out of scope
				r.Cells[x].Style = null;
				try {
					//parse value. If not parseable, set to 0
					i = float.TryParse(r.Cells[x].Value.ToString(), out i) ? i : 0;
				} catch { i = 0; }
				//if the cell value is greater than the criteria of the row, highlight it with that row's color
				try {
					if (Math.Abs(i) >= float.Parse(_tracks[r.Index][9])) {
						r.Cells[x].Style.BackColor = Color.FromArgb(int.Parse(_tracks[r.Index][7]));
					}
				} catch { }
			}
		}
		///Update DGV from _tracks
		public void LoadLeaf(List<string> _load)
		{
			//beat count and time sig are stored at the top of the file, separated by ';'
			numericUpDown_LeafLength.Value = int.Parse(_load[0].Split(';')[0]);
			dropTimeSig.SelectedIndex = dropTimeSig.FindStringExact(_load[0].Split(';')[1]);
			//remove the entry storing beat and timesig so the next steps don't process it
			_load.RemoveAt(0);

			List<string> _griddata = new List<string>();
			foreach (string s in _load) {
				_tracks.Add(s.Split('#')[0].Split(';').ToList());
				//all data after '#' in the string is row/cell data
				_griddata.Add(s.Split('#')[1]);
			}

			trackEditor.Rows.Clear();
			trackEditor.RowCount = _tracks.Count;
			//update header cells for each row and import data to the grid
			foreach (DataGridViewRow r in trackEditor.Rows) {
				if (_tracks[r.Index][1].Length > 1) {
					r.HeaderCell.Value = _tracks[r.Index][1] + " (" + _tracks[r.Index][3] + ")";
					//pass _griddata per row to be imported to the DGV
					TrackRawImport(r, _griddata[r.Index]);
				}
			}

			dropObjects.Enabled = true;
			dropParamPath.Enabled = true;
			btnTrackColorDialog.Enabled = true;
			NUD_TrackDoubleclick.Enabled = true;
			NUD_TrackHighlight.Enabled = true;

			SaveLeaf(true);
		}
		///Resizes and Repositions panels based on which ones are visible
		public void PanelVisibleResize()
		{
			//if Master is invisible, first move Leaf to Level, then move Level to Master
			if (!panelMaster.Visible) {
				panelLeaf.Location = panelLevel.Location;
				panelLevel.Location = panelMaster.Location;
			}
			//if Level is invisible, move Leaf to Level
			if (!panelLevel.Visible)
				panelLeaf.Location = panelLevel.Location;
			//if Master is visible, move Level over
			if (panelMaster.Visible)
				panelLevel.Location = new Point(panelMaster.Location.X + panelMaster.Size.Width + 6, panelMaster.Location.Y);
			//if Level is visible, move Leaf over. Else move Leaf to Level
			if (panelLevel.Visible)
				panelLeaf.Location = new Point(panelLevel.Location.X + panelLevel.Size.Width + 6, panelMaster.Location.Y);
			else
				panelLeaf.Location = panelLevel.Location;
		}
		#endregion
		#endregion

		#region Lvl_Editor
		#region Variables
		bool _savelvl = true;

		int _lvllength;

		List<string> _lvlpaths = (Properties.Resources.paths).Split('\n').ToList();
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
			if (lvlLeafPaths.CurrentCell.Value.ToString() == " ")
				lvlLeafPaths.Rows.Remove(lvlLeafPaths.CurrentRow);

			_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Clear();
			for (int x = 0; x < lvlLeafPaths.Rows.Count; x++) {
				if (lvlLeafPaths.Rows[x].Cells[0].Value != null)
					_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Add(lvlLeafPaths.Rows[x].Cells[0].Value.ToString());
			}

			btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
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
						lvlLeafList.CurrentCell = lvlLeafList.Rows[_lvlleafs.Count - 1].Cells[0];
					//display any error if found when importing
					} catch (Exception ex) { MessageBox.Show("Unable to open file \"" + ofd.FileName + "\". Are you sure this was created by the editor?\n\n" + ex); }
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
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void LoadLvl()
		{

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
			DataGridViewComboBoxColumn _dgvlvlloops = new DataGridViewComboBoxColumn() {
				DataSource = _lvlpaths,
				HeaderText = "Loop Track"
			};
			lvlLoopTracks.Columns.Add(_dgvlvlloops);
			lvlLoopTracks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			///
		}

		public void LvlUpdatePaths(int index)
		{
			lvlLeafPaths.Rows.Clear();
			//for each path in the selected leaf, populate the paths DGV
			foreach (string path in _lvlleafs[index].paths)
				lvlLeafPaths.Rows.Add(new object[] { path });
		}

		private void btnLvlSeqAdd_Click(object sender, EventArgs e)
		{
			lvlSeqObjs.RowCount++;
			lvlSeqObjs.Rows[lvlSeqObjs.Rows.Count - 1].HeaderCell.Value = "Volume Track " + (lvlSeqObjs.Rows.Count - 1);
			btnLvlSeqDelete.Enabled = true;
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
			//disable this button if there are no more rows
			btnLvlSeqDelete.Enabled = lvlSeqObjs.Rows.Count != 0;
		}
		#endregion

		#endregion
	}
}
