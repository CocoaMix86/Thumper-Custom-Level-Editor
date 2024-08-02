using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Thumper_Custom_Level_Editor
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
				if (value == null) {
					ResetLvl();
				}
				else if (loadedlvl != value) {
					if (loadedlvl != null && lockedfiles.ContainsKey(loadedlvl)) {
						lockedfiles[loadedlvl].Close();
						lockedfiles.Remove(loadedlvl);
					}
					loadedlvl = value;
					ShowPanel(true, panelLevel);
					panelLevel.Enabled = true;

					if (!File.Exists(loadedlvl)) {
						File.WriteAllText(loadedlvl, "");
					}
					lockedfiles.Add(_loadedlvl, new FileStream(_loadedlvl, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
				}
			}
		}
		private string loadedlvl;
		string _loadedlvltemp;
		public bool loadinglvl = false;

		List<string> _lvlpaths = Properties.Resources.paths.Replace("\r\n", "\n").Split('\n').ToList();
		List<SampleData> _lvlsamples = new();
		dynamic lvljson;
		ObservableCollection<LvlLeafData> _lvlleafs = new();

		List<LvlLeafData> clipboardleaf = new();
		List<string> clipboardpaths = new();
		List<int> idxtocolor = new();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS  ///
		///         ///

		///DGV LVLLEAFLIST
		//Selected row changed

		private void lvlLeafList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1 || _lvlleafs.Count == 0 || e.RowIndex > _lvlleafs.Count - 1)
				return;
			//lvlLeafList_RowEnter(sender, e);
			if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _saveleaf) {
				string _file = (_lvlleafs[e.RowIndex].leafname).Replace(".leaf", "");
				dynamic _load;
				if (File.Exists($@"{workingfolder}\leaf_{_file}.txt")) {
					_load = LoadFileLock($@"{workingfolder}\leaf_{_file}.txt");
					_loadedleaftemp = $@"{workingfolder}\leaf_{_file}.txt";
				}
				else {
					MessageBox.Show("This leaf does not exist in the Level folder.");
					return;
                }

				LoadLeaf(_load);
			}
			LvlUpdatePaths(e.RowIndex);
		}
		private void lvlLeafList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1)
				return;
			if (_dgfocus != "lvlLeafList") {
				_dgfocus = "lvlLeafList";
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
			btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
			btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
			btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //set lvl save flag to false
            SaveLvl(false);
		}
		/// DGV LVLLOOPTRACKS
		//Cell value changed
		private void lvlLoopTracks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == -1 || e.RowIndex == -1)
				return;
			DataGridViewCell dgvc = sender as DataGridViewCell;
			try { //try is here because this gets triggered upon app load, when there's no data
				if (e.ColumnIndex == 0) {
                    //search _lvlsamples for the value in the cell. Cell value is a string, so we need to apply the SampleData object instead
                    SampleData _samplocate = _lvlsamples.First(item => item.obj_name == ((string)lvlLoopTracks.Rows[e.RowIndex].Cells[0].Value));
					lvlLoopTracks.Rows[e.RowIndex].Cells[0].Value = _samplocate;
				}
			} catch { }
			//set lvl save flag to false
			SaveLvl(false);
		}
		private void lvlLoopTracks_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.ThrowException = false;
		}

		private void lvlSeqObjs_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == -1 || e.RowIndex == -1)
				return;
			DataGridView dgv = (DataGridView)sender;
			if (e.Button == MouseButtons.Right) {
				if (dgv[e.ColumnIndex, e.RowIndex].Selected == false && dgv[e.ColumnIndex, e.RowIndex].Value != null) {
					lvlSeqObjs.CellValueChanged -= lvlSeqObjs_CellValueChanged;
					dgv[e.ColumnIndex, e.RowIndex].Value = null;
					TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex]);
					SaveLvl(false);
					lvlSeqObjs.CellValueChanged += lvlSeqObjs_CellValueChanged;
				}
				else if (dgv[e.ColumnIndex, e.RowIndex].Selected) {
					if (dgv[e.ColumnIndex, e.RowIndex].Value == null && dgv.SelectedCells.Count == 1)
						return;
					CellValueChangedLvl(e.RowIndex, e.ColumnIndex, true);
				}
			}
        }

        private void lvlSeqObjs_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;
            DataGridView dgv = sender as DataGridView;
            if (Control.MouseButtons == MouseButtons.Right) {
                if (dgv[e.ColumnIndex, e.RowIndex].Selected == false && dgv[e.ColumnIndex, e.RowIndex].Value != null) {
                    lvlSeqObjs.CellValueChanged -= lvlSeqObjs_CellValueChanged;
                    dgv[e.ColumnIndex, e.RowIndex].Value = null;
                    TrackUpdateHighlightingSingleCell(dgv[e.ColumnIndex, e.RowIndex]);
                    //GenerateDataPoints(dgv.Rows[e.RowIndex]);
                    SaveLvl(false);
                    lvlSeqObjs.CellValueChanged += lvlSeqObjs_CellValueChanged;
                }
                else if (dgv[e.ColumnIndex, e.RowIndex].Selected == true) {
                    CellValueChangedLvl(e.RowIndex, e.ColumnIndex, true);
                }
            }
        }
        /// DGV LVLSEQOBJS
        private void lvlSeqObjs_SelectionChanged(object sender, EventArgs e)
		{
			bool enable = lvlSeqObjs.SelectedCells.Count > 0;
			btnLvlSeqDelete.Enabled = enable;
			btnLvlSeqClear.Enabled = enable;
		}
		//Cell value changed
		private void lvlSeqObjs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1 || e.ColumnIndex == -1)
				return;
			CellValueChangedLvl(e.ColumnIndex, e.RowIndex);
		}
		private void CellValueChangedLvl(int ColumnIndex, int RowIndex, bool setnull = false)
        {
			lvlSeqObjs.CellValueChanged -= lvlSeqObjs_CellValueChanged;
			try {
				bool changes = false;
                object _val = null;
                if (setnull)
                    _val = null;
                else if (Decimal.TryParse(lvlSeqObjs[ColumnIndex, RowIndex].EditedFormattedValue?.ToString(), out decimal _valtoset))
                    _val = TruncateDecimal(_valtoset, 3);
                //iterate over each cell in the selection
                foreach (DataGridViewCell _cell in lvlSeqObjs.SelectedCells) {
					//if cell does not have the value, set it
					if (_cell.Value != _val) {
						_cell.Value = _val;
						changes = true;
					}

					if (_val == null)
						_cell.Style = null;
					else
						_cell.Style.BackColor = Color.Purple;
				}
				//sets flag that lvl has unsaved changes
				if (changes)
					SaveLvl(false);
			}
			catch { }
			lvlSeqObjs.CellValueChanged += lvlSeqObjs_CellValueChanged;
		}
		//Press Delete to clear cells
		private void lvlSeqObjs_KeyDown(object sender, KeyEventArgs e)
		{
			controldown = e.Control;
			shiftdown = e.Shift;
			altdown = e.Alt;
			if (e.KeyCode == Keys.Delete) {
				CellValueChangedLvl(lvlSeqObjs.CurrentCell.ColumnIndex, lvlSeqObjs.CurrentCell.RowIndex, true);
				e.Handled = true;
			}
			else if (controldown) {
				///copies selected cells
				if (e.KeyCode == Keys.C) {
					DataObject d = lvlSeqObjs.GetClipboardContent();
					Clipboard.SetDataObject(d, true);
					e.Handled = true;
				}
				///cut and copies selected cells
				if (e.KeyCode == Keys.X) {
					DataObject d = lvlSeqObjs.GetClipboardContent();
					Clipboard.SetDataObject(d, true);
					CellValueChangedLvl(lvlSeqObjs.CurrentCell.ColumnIndex, lvlSeqObjs.CurrentCell.RowIndex, true);
					e.Handled = true;
					SaveLvl(false);
				}
				///pastes cell data from clipboard
				if (e.KeyCode == Keys.V) {
					lvlSeqObjs.CellValueChanged -= lvlSeqObjs_CellValueChanged;
					//get content on clipboard to string and then split it to rows
					string s = Clipboard.GetText().Replace("\r\n", "\n");
					string[] copiedrows = s.Split('\n');
					//set ints so we don't have to always call rowindex, columnindex
					int row = lvlSeqObjs.CurrentCell.RowIndex;
					int col = lvlSeqObjs.CurrentCell.ColumnIndex;
					for (int _line = 0; _line < copiedrows.Length; _line++) {
						//if paste will go outside grid bounds, skip
						if (row + _line >= lvlSeqObjs.RowCount)
							break;
						//split row into individual cells
						string[] cells = copiedrows[_line].Split('\t');
						for (int i = 0; i < cells.Length; i++) {
							//if paste will go outside grid bounds, skip
							if (col + i >= lvlSeqObjs.ColumnCount)
								break;
							//don't paste if cell is blank
							if (cells[i] != "") {
								lvlSeqObjs[col + i, row + _line].Value = decimal.Parse(cells[i]);
								lvlSeqObjs[col + i, row + _line].Style.BackColor = Color.Purple;
							}
						}
					}
					SaveLvl(false);
					lvlSeqObjs.CellValueChanged += lvlSeqObjs_CellValueChanged;
				}
			}
		}
		//Press Backspace to clear cells
		private void lvlSeqObjs_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Back) {
				CellValueChangedLvl(lvlSeqObjs.CurrentCell.ColumnIndex, lvlSeqObjs.CurrentCell.RowIndex, true);
				e.Handled = true;
			}
		}
		//Fill weight - allows for more columns
		private void lvlSeqObjs_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
		{
			e.Column.FillWeight = 1;
			e.Column.Width = trackLvlVolumeZoom.Value;
		}
		///_LVLLEAF - Triggers when the collection changes
		public void lvlleaf_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			lvlLeafList.RowEnter -= lvlLeafList_RowEnter;
			int _in = e.NewStartingIndex;

			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset) {
				lvlLeafList.RowCount = 0;
			}
			//if action ADD, add new row to the lvl DGV
			//NewStartingIndex and OldStartingIndex track where the changes were made
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
				string leafname = _lvlleafs[_in].leafname;
				lvlLeafList.Rows.Insert(e.NewStartingIndex, new object[] { Properties.Resources.leaf, leafname.Replace(".leaf", ""), _lvlleafs[_in].beats });
			}
			//if action REMOVE, remove row from the lvl DGV
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
				lvlLeafList.Rows.RemoveAt(e.OldStartingIndex);
			}
			lvlLeafList.RowEnter += lvlLeafList_RowEnter;


			//enable certain buttons if there are enough items for them
			btnLvlLeafDelete.Enabled = _lvlleafs.Count > 0;
			btnLvlLeafUp.Enabled = _lvlleafs.Count > 1;
			btnLvlLeafDown.Enabled = _lvlleafs.Count > 1;
			btnLvlRefreshBeats.Enabled = _lvlleafs.Count > 0;
			btnLvlLeafCopy.Enabled = _lvlleafs.Count > 0;
			//enable/disable buttons if leaf exists or not
			if (_lvlleafs.Count == 0) {
				btnLvlPathAdd.Enabled = false;
				lblLvlTunnels.Text = $"Paths/Tunnels - <no leaf>";
				btnLvlRandomTunnel.Enabled = false;
			}
			if (btnLvlPathAdd.Enabled == false) btnLvlPathDelete.Enabled = false;
			if (btnLvlPathAdd.Enabled == false) btnLvlCopyTunnel.Enabled = false;
            btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
            btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            btnLvlSeqAdd.Enabled = _lvlleafs.Count > 0;
			if (btnLvlSeqAdd.Enabled == false) btnLvlSeqDelete.Enabled = false;
			if (btnLvlSeqAdd.Enabled == false) btnLvlSeqClear.Enabled = false;
			btnLvlLoopAdd.Enabled = _lvlleafs.Count > 0;
			if (btnLvlLoopAdd.Enabled == false) btnLvlLoopDelete.Enabled = false;
			//
			if (!loadinglvl) {
				ColorLvlVolumeSequencer();
				SaveLvl(false);
			}
		}
		/// Set "saved" flag to false for LVL when these events happen
		private void NUD_lvlApproach_ValueChanged(object sender, EventArgs e)
		{
			ColorLvlVolumeSequencer();
			SaveLvl(false);
		}
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
				lvlsaveAsToolStripMenuItem_Click(null, null);
			}
		}
		/// LVL SAVE
		private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			//if _loadedlvl is somehow not set, force Save As instead
			if (_loadedlvl == null) {
				lvlsaveAsToolStripMenuItem.PerformClick();
				return;
			}
			else
				WriteLvl();
		}
		/// LVL SAVE AS
		private void lvlsaveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Editor Lvl File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = workingfolder ?? Application.StartupPath;
            if (sfd.ShowDialog() == DialogResult.OK) {
				if (sender == null) {
					ResetLvl();
				}
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                string tempFileName = Path.GetFileName(sfd.FileName);
				if (!tempFileName.EndsWith(".txt"))
					tempFileName += ".txt";
				//check if user input "lvl_", and deny save if so
				if (Path.GetFileName(sfd.FileName).Contains("lvl_")) {
                    MessageBox.Show("File not saved. Do not include 'lvl_' in your file name.", "File not saved");
                    return;
                }
                if (File.Exists($@"{storePath}\lvl_{tempFileName}")) {
                    MessageBox.Show("That file name exists already.", "File not saved");
                    return;
                }
                _loadedlvl = _loadedlvltemp = $@"{storePath}\lvl_{tempFileName}";
                WriteLvl();
                //after saving new file, refresh the workingfolder
                btnWorkRefresh.PerformClick();
            }
        }
		private void WriteLvl()
		{
            //serialize JSON object to a string, and write it to the file
            JObject _save = LvlBuildSave(Path.GetFileName(_loadedlvl).Replace("lvl_", ""));
			if (!lockedfiles.ContainsKey(_loadedlvl)) {
				lockedfiles.Add(_loadedlvl, new FileStream(_loadedlvl, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
			}
			WriteFileLock(lockedfiles[loadedlvl], _save);
			SaveLvl(true, true);
			lblLvlName.Text = $"Lvl Editor ⮞ {_save["obj_name"]}";
			//reload samples on save
			LvlReloadSamples();
		}
		/// LVL LOAD
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Editor Lvl File (*.txt)|lvl_*.txt";
                ofd.Title = "Load a Thumper Lvl file";
                ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    _loadedlvltemp = ofd.FileName;
                    //load json from file into _load. The regex strips any comments from the text.
                    object _load = LoadFileLock(ofd.FileName);
                    LoadLvl(_load);
                }
            }
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnLvlLeafDelete_Click(object sender, EventArgs e)
		{
			List<LvlLeafData> todelete = new();
			foreach (DataGridViewRow dgvr in lvlLeafList.SelectedRows) {
				todelete.Add(_lvlleafs[dgvr.Index]);
            }
			int _in = lvlLeafList.CurrentRow.Index;
			//_lvlleafs.RemoveAt(_in);
			foreach (LvlLeafData lvd in todelete)
				_lvlleafs.Remove(lvd);
			PlaySound("UIobjectremove");
			lvlLeafList_CellClick(null, new DataGridViewCellEventArgs(0, _in >= _lvlleafs.Count ? _in - 1 : _in));
		}
		private void btnLvlLeafAdd_Click(object sender, EventArgs e)
		{
            using OpenFileDialog ofd = new();
            ofd.Filter = "Thumper Leaf File (*.txt)|leaf_*.txt";
            ofd.Title = "Load a Thumper Leaf file";
            ofd.InitialDirectory = workingfolder ?? Application.StartupPath;
            PlaySound("UIfolderopen");
            if (ofd.ShowDialog() == DialogResult.OK) {
                AddLeaftoLvl(ofd.FileName);
            }
        }

		private void btnLvlLeafUp_Click(object sender, EventArgs e)
		{
			List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
			if (selectedrows.Any(r => r == 0))
				return;
			loadinglvl = true;
			lvlLeafList.ClearSelection();
			selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
			foreach (int dgvr in selectedrows) {
				_lvlleafs.Insert(dgvr - 1, _lvlleafs[dgvr]);
				_lvlleafs.RemoveAt(dgvr + 1);
			}
			lvlLeafList.ClearSelection();
			foreach (int dgvr in selectedrows) {
				lvlLeafList.Rows[dgvr - 1].Selected = true;
			}
			loadinglvl = false;
			ColorLvlVolumeSequencer();
			SaveLvl(false);
		}

		private void btnLvlLeafDown_Click(object sender, EventArgs e)
		{
			List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
			if (selectedrows.Any(r => r == lvlLeafList.RowCount - 1))
				return;
			loadinglvl = true;
			lvlLeafList.ClearSelection();
			selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
			foreach (int dgvr in selectedrows) {
				_lvlleafs.Insert(dgvr + 2, _lvlleafs[dgvr]);
				_lvlleafs.RemoveAt(dgvr);
			}
			lvlLeafList.ClearSelection();
			foreach (int dgvr in selectedrows) {
				lvlLeafList.Rows[dgvr + 1].Selected = true;
			}
			loadinglvl = false;
			ColorLvlVolumeSequencer();
			SaveLvl(false);
		}

		///COPY PASTE of leaf
		private void btnLvlLeafCopy_Click(object sender, EventArgs e)
		{
			List<int> selectedrows = lvlLeafList.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
			selectedrows.Sort((row, row2) => row.CompareTo(row2));
			clipboardleaf = _lvlleafs.Where(x => selectedrows.Contains(_lvlleafs.IndexOf(x))).ToList();
			clipboardleaf.Reverse();
			btnLvlLeafPaste.Enabled = true;
			PlaySound("UIkcopy");
		}
		private void btnLvlLeafPaste_Click(object sender, EventArgs e)
		{
			int _in = lvlLeafList.CurrentRow?.Index + 1 ?? 0;
			foreach (LvlLeafData lld in clipboardleaf)
				_lvlleafs.Insert(_in, lld.Clone());
			PlaySound("UIkpaste");
			SaveLvl(false);
		}

		private void btnLvlLeafRandom_Click(object sender, EventArgs e)
		{
			List<string> leafs = workingfolderFiles.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value.ToString().Contains("leaf_")).Select(x => x.Cells[1].Value.ToString()).ToList();
			string _selectedfilename = $@"{workingfolder}\{leafs[rng.Next(0, leafs.Count)]}.txt";
			AddLeaftoLvl(_selectedfilename);
		}

		private void btnLvlPathDelete_Click(object sender, EventArgs e)
		{
			List<string> todelete = new();
			foreach (DataGridViewRow dgvr in lvlLeafPaths.SelectedRows) {
				todelete.Add(dgvr.Cells[0].Value.ToString());
            }
			foreach (string s in todelete)
				_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Remove(s);
			LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
			PlaySound("UItunnelremove");
			SaveLvl(false);
		}

		private void btnLvlPathAdd_Click(object sender, EventArgs e)
		{
			_lvlleafs[lvlLeafList.CurrentRow.Index].paths.Add("");
			LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
			btnLvlPathDelete.Enabled = true;
			PlaySound("UItunneladd");
			SaveLvl(false);
		}

		private void btnLvlPathUp_Click(object sender, EventArgs e)
		{
			if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == 0))
				return;
			int idx = lvlLeafList.CurrentRow.Index;
			List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
			selectedrows.Sort((row1, row2) => row1.CompareTo(row2));
			foreach (int dgvr in selectedrows) {
				_lvlleafs[idx].paths.Insert(dgvr - 1, _lvlleafs[idx].paths[dgvr]);
				_lvlleafs[idx].paths.RemoveAt(dgvr + 1);
			}
			LvlUpdatePaths(idx);
			lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] - 1];
			lvlLeafPaths.ClearSelection();
			foreach (int dgvr in selectedrows) {
				lvlLeafPaths.Rows[dgvr - 1].Cells[0].Selected = true;
			}
			SaveLvl(false);
		}

		private void btnLvlPathDown_Click(object sender, EventArgs e)
		{
			if (lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Any(r => r.Index == lvlLeafPaths.Rows.Count - 1))
				return;
			int idx = lvlLeafList.CurrentRow.Index;
			List<int> selectedrows = lvlLeafPaths.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();
			selectedrows.Sort((row1, row2) => row2.CompareTo(row1));
			foreach (int dgvr in selectedrows) {
				_lvlleafs[idx].paths.Insert(dgvr + 2, _lvlleafs[idx].paths[dgvr]);
				_lvlleafs[idx].paths.RemoveAt(dgvr);
			}
			LvlUpdatePaths(idx);
			lvlLeafPaths.CurrentCell = lvlLeafPaths[0, selectedrows[0] + 1];
			lvlLeafPaths.ClearSelection();
			foreach (int dgvr in selectedrows) {
				lvlLeafPaths.Rows[dgvr + 1].Cells[0].Selected = true;
			}
			SaveLvl(false);
        }

        private void btnLvlPathClear_Click(object sender, EventArgs e)
        {
            int idx = lvlLeafList.CurrentRow.Index;
			if (_lvlleafs[idx].paths.Count > 0) {
				if (MessageBox.Show("Are you sure you want to clear all?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
            }
			_lvlleafs[idx].paths.Clear();
			LvlUpdatePaths(idx);
			PlaySound("UIdataerase");
			SaveLvl(false);
        }

        private void btnLvlRandomTunnel_Click(object sender, EventArgs e)
		{
			if (_lvlleafs.Count == 0)
				return;
			lvlLeafPaths.RowCount++;
			lvlLeafPaths.Rows[^1].Cells[0].Value = _lvlpaths[rng.Next(1, _lvlpaths.Count)];
			btnLvlPathDelete.Enabled = true;
			PlaySound("UItunneladd");
			SaveLvl(false);
		}

		private void btnLvlCopyTunnel_Click(object sender, EventArgs e)
		{
			if (_loadedlvl == null)
				return;
			clipboardpaths = new List<string>(_lvlleafs[lvlLeafList.CurrentRow.Index].paths);
			btnLvlPasteTunnel.Enabled = true;
			PlaySound("UIkcopy");
		}

		private void btnLvlPasteTunnel_Click(object sender, EventArgs e)
		{
			_lvlleafs[lvlLeafList.CurrentRow.Index].paths.AddRange(new List<string>(clipboardpaths));
			LvlUpdatePaths(lvlLeafList.CurrentRow.Index);
			PlaySound("UIkpaste");
			SaveLvl(false);
		}

		private void btnLvlLoopAdd_Click(object sender, EventArgs e)
		{
			lvlLoopTracks.RowCount++;
			lvlLoopTracks.Rows[^1].HeaderCell.Value = "Volume Track " + (lvlLoopTracks.Rows.Count - 1);
			lvlLoopTracks.Rows[^1].Cells[1].Value = 0;
			lvlLoopTracks.Rows[^1].Cells[0].Value = "";
			btnLvlLoopDelete.Enabled = true;
			PlaySound("UIobjectadd");
		}

		private void btnLvlLoopDelete_Click(object sender, EventArgs e)
		{
			lvlLoopTracks.Rows.RemoveAt(lvlLoopTracks.CurrentRow.Index);
			PlaySound("UIobjectremove");
			//disable button if no more rows exist
			if (lvlLoopTracks.Rows.Count < 1)
				btnLvlLoopDelete.Enabled = false;
			//rename each header cell as the rows have moved and now are on different tracks
			foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
				r.HeaderCell.Value = "Volume Track " + r.Index;
			}
			SaveLvl(false);
		}

		private void btnLvlSeqAdd_Click(object sender, EventArgs e)
		{
			lvlSeqObjs.RowCount++;
			PlaySound("UIobjectadd");
			lvlSeqObjs.Rows[^1].HeaderCell.Value = "Volume Track " + (lvlSeqObjs.Rows.Count - 1);
			btnLvlSeqDelete.Enabled = true;
			btnLvlSeqClear.Enabled = true;
			SaveLvl(false);
		}

		private void btnLvlSeqDelete_Click(object sender, EventArgs e)
		{
			bool _empty = true;
            List<DataGridViewRow> selectedrows = lvlSeqObjs.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
            //iterate over current row to see if any cells have data
            List<DataGridViewCell> filledcells = selectedrows.SelectMany(x => x.Cells.Cast<DataGridViewCell>()).Where(x => x.Value != null).ToList();
            if (filledcells.Count > 0)
                _empty = false;
            //prompt user to say YES if row is not empty. Then delete selected track
            if (!_empty && MessageBox.Show("Some cells in the selected tracks have data. Are you sure you want to delete?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No) {

                return;
            }

            foreach (DataGridViewRow dgvr in selectedrows) {
                lvlSeqObjs.Rows.Remove(dgvr);
            }
            PlaySound("UIobjectremove");
            SaveLvl(false);
            //after deleting, rename all headers so they're in order again
            foreach (DataGridViewRow r in lvlSeqObjs.Rows)
                r.HeaderCell.Value = "Volume Track " + r.Index;
            //disable buttons if there are no more rows
            btnLvlSeqDelete.Enabled = lvlSeqObjs.Rows.Count > 0;
			btnLvlSeqClear.Enabled = lvlSeqObjs.Rows.Count > 0;
		}

		private void btnLvlSeqClear_Click(object sender, EventArgs e)
		{
			//finds each distinct row across all selected cells
			List<DataGridViewRow> selectedrows = lvlSeqObjs.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList();
			if (MessageBox.Show($"{selectedrows.Count} rows selected.\nAre you sure you want to clear them?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			//then get all cells in the rows that have values
			List<DataGridViewCell> filledcells = selectedrows.SelectMany(x => x.Cells.Cast<DataGridViewCell>()).Where(x => x.Value != null).ToList();
			//select all of them
			foreach (DataGridViewCell dgvc in filledcells) {
				dgvc.Selected = true;
			}
			//then set a single one to null. The "cellvaluechanged" event will handle the rest
			filledcells[0].Value = null;

			PlaySound("UIdataerase");
			SaveLvl(false);
		}

		private void btnLvlLoopRefresh_Click(object sender, EventArgs e)
		{
			LvlReloadSamples();
			PlaySound("UIrefresh");
			MessageBox.Show($"Found and loaded {_lvlsamples.Count} samples for the current working folder.");
		}

		private void btnLvlRefreshBeats_Click(object sender, EventArgs e)
		{
			foreach (LvlLeafData _leaf in _lvlleafs) {
				string _file = (_leaf.leafname).Replace(".leaf", "");
				dynamic _load;
				try {
					if (!File.Exists($@"{workingfolder}\leaf_{_file}.txt"))
						continue;
					//I need to load the entire document to grab one field from it
					_load = LoadFileLock($@"{workingfolder}\leaf_{_file}.txt");
					//if beat_cnt is different than what is loaded, replace it and mark the save flag
					if (_leaf.beats != (int)_load["beat_cnt"]) {
						_leaf.beats = (int)_load["beat_cnt"];
						//and update the dgv cell with new beat value
						lvlLeafList.Rows[_lvlleafs.IndexOf(_leaf)].Cells[2].Value = _leaf.beats;
						SaveLvl(false);
					}
				}
				catch (Exception ex) { MessageBox.Show("Problem occured when refreshing lvl beats. Show this error to the dev.\n" + ex, "Lvl beat refresh error"); }
			}
		}

		private void btnRevertLvl_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
				return;
			SaveLvl(true);
			LoadLvl(lvljson);
			PlaySound("UIrevertchanges");
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
			if (_load == null)
				return;
			//reset flag in case it got stuck previously
			loadinglvl = false;
			//if Lvl Editor is hidden, show it when a lvl is selected from anywhere
			if (panelLevel.Visible == false)
				levelEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Lvl or not
			if ((string)_load["obj_type"] != "SequinLevel") {
				MessageBox.Show("This does not appear to be a lvl file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedlvltemp);
			_loadedlvl = _loadedlvltemp;
			//set some visual elements
			lblLvlName.Text = $@"Lvl Editor ⮞ {_load["obj_name"]}";
			//set flag that load is in progress. This skips SaveLvl() method
			loadinglvl = true;
			lvljson = _load;

			///Clear DGVs so new data can load
			lvlLoopTracks.Rows.Clear();
            lvlLeafList.Rows.Clear();
            lvlSeqObjs.Rows.Clear();
            lvlLeafPaths.Rows.Clear();
            _lvlleafs.Clear();

			///populate the non-DGV elements on the form with info from the JSON
			NUD_lvlApproach.Value = (decimal)_load["approach_beats"];
			NUD_lvlVolume.Value = (decimal)_load["volume"];
			dropLvlInput.Text = (string)_load["input_allowed"];
			dropLvlTutorial.Text = (string)_load["tutorial_type"];
			///load loop track names and paths to lvlLoopTracks DGV
			((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DataSource = _lvlsamples;
			LvlReloadSamples();
			/*foreach (dynamic samp in _load["loops"]) {
				SampleData _samplocate = _lvlsamples.FirstOrDefault(item => item.obj_name == ((string)samp["samp_name"])?.Replace(".samp", "")) ?? _lvlsamples[0];
				lvlLoopTracks.Rows.Add(new object[] { _samplocate, (int?)samp["beats_per_loop"] == null ? 0 : (int)samp["beats_per_loop"] });
			}*/
			/*foreach (DataGridViewRow r in lvlLoopTracks.Rows)
				r.HeaderCell.Value = "Volume Track " + r.Index;*/
			btnLvlLoopDelete.Enabled = lvlLoopTracks.Rows.Count > 0;
			///load leafs associated with this lvl
			foreach (dynamic leaf in _load["leaf_seq"]) {
				_lvlleafs.Add(new LvlLeafData() {
					leafname = (string)leaf["leaf_name"],
					beats = (int)leaf["beat_cnt"],
					paths = leaf["sub_paths"].ToObject<List<string>>(),
					id = rng.Next(0, 1000000)
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

			trackLvlVolumeZoom_Scroll(null, null);
			btnLvlLeafRandom.Enabled = true;
			//mark that lvl is saved (just freshly loaded)
			//SaveLvl(true);
			loadinglvl = false;
			SaveLvl(true);
			ColorLvlVolumeSequencer();
		}

		public void InitializeLvlStuff()
		{
			_lvlpaths.Sort();

			///customize Paths List a bit
			//custom column containing comboboxes per cell
			DataGridViewComboBoxColumn _dgvlvlpaths = new() {
				DataSource = _lvlpaths,
				HeaderText = "Path Name",
				AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
				DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
				DisplayStyleForCurrentCellOnly = true
			};
			lvlLeafPaths.Columns.Add(_dgvlvlpaths);
			///

			///customize Loop Track list a bit
			//custom column containing comboboxes per cell
			lvlLoopTracks.Columns[1].ValueType = typeof(decimal);
			lvlLoopTracks.Columns[1].DefaultCellStyle.Format = "0.#";
			((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DataSource = _lvlsamples;
			//lvlLoopTracks.Columns[0].ValueType = typeof(SampleData);
			///
			//add event handler to this collection
			_lvlleafs.CollectionChanged += lvlleaf_CollectionChanged;
		}

		public void AddLeaftoLvl(string path)
        {
			//parse leaf to JSON
			dynamic _load = LoadFileLock(path);
			//check if file being loaded is actually a leaf. Can do so by checking the JSON key
			if ((string)_load["obj_type"] != "SequinLeaf") {
				MessageBox.Show("This does not appear to be a leaf file!", "Leaf load error");
				return;
			}
			//check if leaf exists in the same folder as the lvl. If not, allow user to copy file.
			//this is why I utilize workingfolder
			if (Path.GetDirectoryName(path) != workingfolder) {
				if (MessageBox.Show("The leaf you chose does not exist in the same folder as this lvl. Do you want to copy it to this folder and load it?", "Leaf load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
					File.Copy(path, $@"{workingfolder}\{Path.GetFileName(path)}");
				else
					return;
			}
			PlaySound("UIobjectadd");
			//Setup list of tunnels if copy check is enabled
			List<string> copytunnels = new();
			if (chkTunnelCopy.Checked) {
				copytunnels = new List<string>(_lvlleafs.Last().paths);
			}
			//add leaf data to the list
			_lvlleafs.Add(new LvlLeafData() {
				leafname = (string)_load["obj_name"],
				beats = (int)_load["beat_cnt"],
				paths = new List<string>(copytunnels),
				id = rng.Next(0, 1000000)
			});
		}

		public void PaintLvlSeqDividers(int startidx, int endidx)
        {
			for (int idx = startidx; idx < endidx; idx++) {
				lvlSeqObjs.Columns[idx].DefaultCellStyle.BackColor = Color.FromArgb(0,0,0);
			}
        }

		public void LvlUpdatePaths(int index)
		{
			lvlLeafPaths.Rows.Clear();
			lblLvlTunnels.Text = $"Paths/Tunnels - {_lvlleafs[index].leafname}";
			//for each path in the selected leaf, populate the paths DGV
			foreach (string path in _lvlleafs[index].paths) {
				if (_lvlpaths.Contains(path))
					lvlLeafPaths.Rows.Add(new object[] { path });
				else
					MessageBox.Show($"Tunnel \"{path}\" not found in program. If you think this is wrong, please report this to CocoaMix on the github page!");
			}
			btnLvlPathAdd.Enabled = true;
			btnLvlPathDelete.Enabled = lvlLeafPaths.Rows.Count > 0;
			btnLvlCopyTunnel.Enabled = lvlLeafPaths.Rows.Count > 0;
			btnLvlRandomTunnel.Enabled = btnLvlPathAdd.Enabled;
			btnLvlPathUp.Enabled = lvlLeafPaths.Rows.Count > 1;
			btnLvlPathDown.Enabled = lvlLeafPaths.Rows.Count > 1;
			btnLvlPathClear.Enabled = lvlLeafPaths.Rows.Count > 0;
            //monke
        }

		public void LvlReloadSamples()
		{
			if (workingfolder == null)
				return;
			_lvlsamples.Clear();
            //find all samp_ files in the level folder
            List<string> _sampfiles = Directory.GetFiles(workingfolder, "samp_*.txt").Where(x => !x.Contains("samp_default")).ToList();
			//iterate over each file
			foreach (string f in _sampfiles) {
				//parse file to JSON
				dynamic _in = LoadFileLock(f);
				//iterate over items:[] list to get each sample and add names to list
				foreach (dynamic _samp in _in["items"]) {
					_lvlsamples.Add(new SampleData {
						obj_name = ((string)_samp["obj_name"]).Replace(".samp", ""),
						path = _samp["path"],
						volume = _samp["volume"],
						pitch = _samp["pitch"],
						pan = _samp["pan"],
						offset = _samp["offset"],
						channel_group = _samp["channel_group"]
					});
				}
			}
			_lvlsamples.Add(new SampleData {
				obj_name = "",
				path = "",
				volume = 0,
				pitch = 0,
				pan = 0,
				offset = 0,
				channel_group = ""
			});

			_lvlsamples = _lvlsamples.OrderBy(w => w.obj_name).ToList();
			((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DataSource = _lvlsamples;
			//after reloading samples, the dropdowns need to be repopulated
			if (lvljson != null) {
				lvlLoopTracks.Rows.Clear();
				foreach (dynamic samp in lvljson["loops"]) {
					SampleData _samplocate = _lvlsamples.FirstOrDefault(item => item.obj_name == ((string)samp["samp_name"])?.Replace(".samp", "")) ?? _lvlsamples[0];
					lvlLoopTracks.Rows.Add(new object[] { _samplocate, (int?)samp["beats_per_loop"] == null ? 0 : (int)samp["beats_per_loop"] });
				}
				foreach (DataGridViewRow r in lvlLoopTracks.Rows)
					r.HeaderCell.Value = "Volume Track " + r.Index;
			}
			SaveLvl(true);

			//this is for adjusting the dropdown width so that the full item can display
			int width = 0;
			Graphics g = lvlLoopTracks.CreateGraphics();
			Font font = lvlLoopTracks.DefaultCellStyle.Font;
			foreach (SampleData s in _lvlsamples) {
				int newWidth = (int)g.MeasureString(s.obj_name, font).Width;
				if (width < newWidth) {
					width = newWidth;
				}
			}
			((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DropDownWidth = width + 20;
		}

		public void SaveLvl(bool save, bool playsound = false)
		{
			if (loadinglvl)
				return;
			//make the beeble emote
			pictureBox1_Click(null, null);

			_savelvl = save;
			if (!save) {
				btnSaveLvl.Enabled = true;
				btnRevertLvl.Enabled = lvljson != null;
				btnRevertLvl.ToolTipText = lvljson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
				toolstripTitleLvl.BackColor = Color.Maroon;
			}
			else {
				btnSaveLvl.Enabled = false;
				btnRevertLvl.Enabled = false;
				toolstripTitleLvl.BackColor = Color.FromArgb(40, 40, 40);
				if (playsound) PlaySound("UIsave");
			}
		}

		///Import raw text from rich text box to selected row
		public void LvlTrackRawImport(DataGridViewRow r, JObject _rawdata)
		{
            //_rawdata contains a list of all data points. By getting Properties() of it,
            //each point becomes its own index
            List<JProperty> data_points = _rawdata.Properties().ToList();
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

		public void ColorLvlVolumeSequencer()
        {
			lvlSeqObjs.Visible = false;
			//calculate total length of all leafs. This value is used for the volume sequencer
			foreach (int idx in idxtocolor) {
				lvlSeqObjs.Columns[idx].DefaultCellStyle = null;
				lvlSeqObjs.Columns[idx].HeaderCell.Style = null;
			}
			idxtocolor.Clear();
			_lvllength = (int)NUD_lvlApproach.Value;
			foreach (LvlLeafData _leaf in _lvlleafs) {
				if (_leaf.beats > 0)
					idxtocolor.Add(_lvllength);
				_lvllength += _leaf.beats;
			}
			lvlSeqObjs.ColumnCount = _lvllength;
			GenerateColumnStyle(lvlSeqObjs, _lvllength);
			foreach (int idx in idxtocolor) {
				lvlSeqObjs.Columns[idx].DefaultCellStyle.BackColor = Color.LightGray;
				lvlSeqObjs.Columns[idx].HeaderCell.Style.BackColor = Color.LightGray;
				lvlSeqObjs.Columns[idx].HeaderCell.Style.ForeColor = Color.Black;
			}
			lvlSeqObjs.Visible = true;
		}

		public JObject LvlBuildSave(string _lvlname)
		{
			_lvlname = _lvlname.Replace(".txt", ".lvl");
            ///start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLevel" },
                { "obj_name", $"{_lvlname}" },
                { "approach_beats", NUD_lvlApproach.Value }
            };
            //this section adds all colume sequencer controls
            JArray seq_objs = new();
			foreach (DataGridViewRow seq_obj in lvlSeqObjs.Rows) {
                JObject s = new() {
                    { "obj_name", $"{_lvlname}" },
                    { "param_path", $"layer_volume,{seq_obj.Index}" },
                    { "trait_type", "kTraitFloat" }
                };

                JObject data_points = new();
				for (int x = 0; x < seq_obj.Cells.Count; x++) {
					if (!string.IsNullOrEmpty(seq_obj.Cells[x].Value?.ToString()))
						data_points.Add(x.ToString(), float.Parse(seq_obj.Cells[x].Value.ToString()));
				}
				s.Add("data_points", data_points);

				s.Add("step", "False");
				s.Add("default", "0");
				s.Add("footer", "1,1,2,1,2,'kIntensityScale','kIntensityScale',1,1,1,1,1,1,1,1,0,0,0");

				seq_objs.Add(s);
			}
			_save.Add("seq_objs", seq_objs);
			//this section adds all leafs
			JArray leaf_seq = new();
			foreach (LvlLeafData _leaf in _lvlleafs) {
				JObject s = new() {
					{ "beat_cnt", _leaf.beats },
					{ "leaf_name", _leaf.leafname },
					{ "main_path", "default.path" },
					{ "sub_paths", JArray.FromObject(_leaf.paths) },
					{ "pos", new JArray() { 0, 0, 0 } },
					{ "rot_x", new JArray() { 1, 0, 0 } },
					{ "rot_y", new JArray() { 0, 1, 0 } },
					{ "rot_z", new JArray() { 0, 0, 1 } },
					{ "scale", new JArray() { 1, 1, 1 } }
				};

				leaf_seq.Add(s);
			}
			_save.Add("leaf_seq", leaf_seq);
			//this section adds the loop tracks
			JArray loops = new();
			foreach (DataGridViewRow r in lvlLoopTracks.Rows) {
				if (r.Cells[0].Value == null)
					continue;
				JObject s = new() {
					{ "samp_name", ((SampleData)r.Cells[0].Value).obj_name + ".samp"},
					{ "beats_per_loop", decimal.Parse(r.Cells[1].Value.ToString()) }
				};

				loops.Add(s);
			}
			_save.Add("loops", loops);
			//final keys
			_save.Add("volume", NUD_lvlVolume.Value);
			_save.Add("input_allowed", dropLvlInput.Text);
			_save.Add("tutorial_type", dropLvlTutorial.Text);
			_save.Add("start_angle_fracs", new JArray() { 1, 1, 1 });
			///end building JSON output
			lvljson = _save;
			return _save;
		}

		private void ResetLvl()
        {
			//reset things to default values
			lvljson = null;
			loadedlvl = null;
			_lvlleafs.Clear();
			lvlLeafPaths.Rows.Clear();
			lvlSeqObjs.Rows.Clear();
			lvlLoopTracks.Rows.Clear();
			LvlReloadSamples();
			NUD_lvlApproach.Value = 16;
			NUD_lvlVolume.Value = 0.5M;
			dropLvlInput.SelectedIndex = 0;
			dropLvlTutorial.SelectedIndex = 0;
			lblLvlName.Text = "Lvl Editor";
			//set saved flag to true, because nothing is loaded
			SaveLvl(true);
		}
		#endregion
	}
}