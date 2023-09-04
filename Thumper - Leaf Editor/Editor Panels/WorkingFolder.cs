using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
	{
		private void panelWorkingFolder_SizeChanged(object sender, EventArgs e)
		{
			lblWorkingFolder.MaximumSize = new Size(panelWorkingFolder.Width - 16, 0);
		}

		private void workingfolderFiles_SelectionChanged(object sender, EventArgs e)
		{
			//do nothing if selection changes to 0
			if (workingfolderFiles.SelectedCells.Count == 0)
				return;

			dynamic _load;
			string _selectedfilename;
			//attempt to load file listed in the dGV
			try {
				//first check if it exists
				_selectedfilename = $@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.SelectedCells[0].RowIndex].Value}.txt";
				if (!File.Exists(_selectedfilename)) {
					MessageBox.Show($"File {workingfolderFiles[1, workingfolderFiles.SelectedCells[0].RowIndex].Value}.txt could not be found in the folder. Was it moved or deleted?", "File load error");
					return;
				}
				//atempt to parse JSON
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_selectedfilename), "#.*", ""));
			}
			catch {
				//return method if parse fails
				MessageBox.Show("The selected file could not be parsed as JSON.", "File load error");
				return;
			}
			///Send file off to different load methods based on the file type
			//process SAMP first, since its JSON structure is different, and detectable
			if (_load.ContainsKey("items")) {
				_loadedsampletemp = _selectedfilename;
				LoadSample(_load);
				if (panelSample.Visible == false)
					sampleEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinMaster") {
				_loadedmastertemp = _selectedfilename;
				LoadMaster(_load);
				if (panelMaster.Visible == false)
					masterEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinGate") {
				_loadedgatetemp = _selectedfilename;
				LoadGate(_load);
				if (panelGate.Visible == false)
					gateEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				_loadedlvltemp = _selectedfilename;
				LoadLvl(_load);
				if (panelLevel.Visible == false)
					levelEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				_loadedleaf = _selectedfilename;
				LoadLeaf(_load);
				if (panelLeaf.Visible == false)
					leafEditorToolStripMenuItem.PerformClick();
			}
			else if (workingfolderFiles[1, workingfolderFiles.SelectedCells[0].RowIndex].Value.ToString().Contains("LEVEL DETAILS")) {
				editLevelDetailsToolStripMenuItem_Click(null, null);
			}
			else
				MessageBox.Show("this is not a valid Custom Level file.");
		}

		private void workingfolderFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			dynamic _load;
			string _selectedfilename;
			//attempt to load file listed in the dGV
			try {
				//first check if it exists
				_selectedfilename = $@"{workingfolder}\{workingfolderFiles[1, e.RowIndex].Value}.txt";
				if (!File.Exists(_selectedfilename)) {
					MessageBox.Show($"File {workingfolderFiles[1, e.RowIndex].Value}.txt could not be found in the folder. Was it moved or deleted?", "File load error");
					return;
				}
				//atempt to parse JSON
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_selectedfilename), "#.*", ""));
			}
			catch {
				//return method if parse fails
				MessageBox.Show("The selected file could not be parsed as JSON.", "File load error");
				return;
			}
			///Send file off to different load methods based on the file type
			if ((string)_load["obj_type"] == "SequinGate") {

			}
			else if ((string)_load["obj_type"] == "SequinLevel") {

			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				if (_loadedlvl != null)
					AddLeaftoLvl(_selectedfilename);
			}
		}

		private void btnWorkRefresh_Click(object sender, EventArgs e)
		{
			//clear the dgv and reload files in the folder
			workingfolderFiles.Rows.Clear();
			//filter for specific files
			foreach (string file in Directory.GetFiles(workingfolder).Where(x => !x.Contains("leaf_pyramid_outro.txt") && (x.Contains("leaf_") || x.Contains("lvl_") || x.Contains("gate_") || x.Contains("master_") || x.Contains("LEVEL DETAILS") || x.Contains("samp_")))) {
				var filetype = Path.GetFileName(file).Split('_')[0];

				if (!filterleaf && !filterlvl && !filtergate && !filtermaster && !filtersamp)
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
				else if ((filetype == "leaf" && filterleaf) || (filetype == "lvl" && filterlvl) || (filetype == "gate" && filtergate) || (filetype == "master" && filtermaster) || (filetype == "samp" && filtersamp))
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
			}
			//enable button
			btnWorkDelete.Enabled = workingfolderFiles.RowCount > 0;
		}

		private void btnWorkDelete_Click(object sender, EventArgs e)
		{
			//make user confirm file deletion
			if (workingfolderFiles.CurrentRow.Index != -1 && MessageBox.Show("Are you sure you want to delete this file?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				//check if file being deleted is LEVEL DETAILS
				if (workingfolderFiles.CurrentRow.Cells[1].Value.ToString().Contains("LEVEL DETAILS") && MessageBox.Show("You are about to delete the LEVEL DETAILS file. This file is required for the mod loader tool to load the level. Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
				File.Delete($@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.CurrentRow.Index].Value}.txt");
				//call the refresh method so the dgv updates
				btnWorkRefresh_Click(null, null);
			}
		}

		private void btnWorkCopy_Click(object sender, EventArgs e)
		{
			//make user confirm file duplication
			if (workingfolderFiles.CurrentRow.Index != -1 && MessageBox.Show("Copy selected file?", "Confirm duplication", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				//check if file being copied is LEVEL DETAILS
				if (workingfolderFiles.CurrentRow.Cells[1].Value.ToString().Contains("LEVEL DETAILS")) {
					MessageBox.Show("You may not duplicate Level Details", "You cannot do that");
					return;
				}
				File.Copy($@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.CurrentRow.Index].Value}.txt", $@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.CurrentRow.Index].Value} (2).txt");
				//call the refresh method so the dgv updates
				btnWorkRefresh_Click(null, null);
			}
		}

		bool filterleaf, filterlvl, filtergate, filtermaster, filtersamp = false;
		private void filterLeaf_CheckedChanged(object sender, EventArgs e) {filterleaf = filterLeaf.Checked; btnWorkRefresh_Click(null, null); }
		private void filterLvl_CheckedChanged(object sender, EventArgs e) { filterlvl = filterLvl.Checked; btnWorkRefresh_Click(null, null); }
		private void filterGate_CheckedChanged(object sender, EventArgs e) { filtergate = filterGate.Checked; btnWorkRefresh_Click(null, null); }
		private void filterMaster_CheckedChanged(object sender, EventArgs e) { filtermaster = filterMaster.Checked; btnWorkRefresh_Click(null, null); }
		private void filterSamp_CheckedChanged(object sender, EventArgs e) { filtersamp = filterSamp.Checked; btnWorkRefresh_Click(null, null); }


		//Handles right-click of cells to bring up context menu
		private void workingfolderFiles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right) {
				DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
				if (!c.Selected) {
					c.DataGridView.CurrentCell = c;
					c.Selected = true;
				}
			}
		}

		///Contect menu actions
		//Rename
		string filetype = "";
		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (workingfolderFiles.SelectedCells.Count < 1)
				return;
			//set textbox with name of selected file
			string[] file = workingfolderFiles.SelectedCells[1].Value.ToString().Split(new[] { '_' }, 2);
			filetype = file[0];
			//check if file is valid to be renamed
			if (filetype == "master" || filetype == "LEVEL DETAILS") {
				MessageBox.Show("You cannot rename this file.", "File error");
				return;
			}
			txtWorkingRename.Text = file[1];
			lblRenameFileType.Image = (Image)Properties.Resources.ResourceManager.GetObject(file[0]);
			//show the panel
			panelWorkRename.Location = workingfolderFiles.Location;
			panelWorkRename.BringToFront();
			panelWorkRename.Visible = true;
		}
		//Duplicate file
		private void duplicateToolStripMenuItem_Click(object sender, EventArgs e) => btnWorkCopy_Click(null, null);
		//Delete file
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => btnWorkDelete_Click(null, null);

		private void btnWorkRenameYes_Click(object sender, EventArgs e)
		{
			string newfilepath = $@"{workingfolder}\{filetype}_{txtWorkingRename.Text}.txt";
			File.Move(_loadedleaf, newfilepath);
			workingfolderFiles.SelectedCells[1].Value = $@"{filetype}_{txtWorkingRename.Text}";

			if (filetype == "leaf") {
				//change current leaf's loaded path and then save it to make sure new name is in fact saved
				_loadedleaf = newfilepath;
				saveToolStripMenuItem_Click(null, null);
			}
			if (filetype == "lvl") {
				_loadedlvl = newfilepath;
				saveToolStripMenuItem2_Click(null, null);
			}
			if (filetype == "gate") {
				_loadedgate = newfilepath;
				gatesaveToolStripMenuItem_Click(null, null);
            }
			if (filetype == "samp") {
				_loadedsample = newfilepath;
				SamplesaveToolStripMenuItem_Click(null, null);
            }
			panelWorkRename.Visible = false;
		}

		private void btnWorkRenameNo_Click(object sender, EventArgs e)
		{
			panelWorkRename.Visible = false;
		}
	}
}
