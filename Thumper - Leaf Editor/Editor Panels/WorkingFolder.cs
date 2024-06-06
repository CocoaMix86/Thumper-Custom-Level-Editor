using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
	{
		private void workingfolderFiles_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//do nothing if no cell click
			if (e.RowIndex == -1)
				return;

			dynamic _load;
			string _selectedfilename = "";
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
			catch (Exception ex) {
				//return method if parse fails
				MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.\n\n{ex}", "File load error");
				return;
			}
			///Send file off to different load methods based on the file type
			//process SAMP first, since its JSON structure is different, and detectable
			if (_load.ContainsKey("items")) {
				_loadedsampletemp = _selectedfilename;
				if (_loadedsampletemp == _loadedsample)
					return;
				LoadSample(_load);
				if (panelSample.Visible == false)
					sampleEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinMaster") {
				_loadedmastertemp = _selectedfilename;
				if (_loadedmastertemp == _loadedmaster)
					return;
				LoadMaster(_load);
				if (panelMaster.Visible == false)
					masterEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinGate") {
				_loadedgatetemp = _selectedfilename;
				if (_loadedgatetemp == _loadedgate)
					return;
				LoadGate(_load);
				if (panelGate.Visible == false)
					gateEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				_loadedlvltemp = _selectedfilename;
				if (_loadedlvltemp == _loadedlvl)
					return;
				LoadLvl(_load);
				if (panelLevel.Visible == false)
					levelEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				_loadedleaftemp = _selectedfilename;
				//don't reload the file if its the same name
				if (_loadedleaftemp == _loadedleaf)
					return;
				//Check if leaf is saved or not
				if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
					return;
				LoadLeaf(_load);
				//make panel visible if it isn't
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
				if (_loadedmaster != null)
					AddFiletoMaster(_selectedfilename);
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				if (_loadedmaster != null)
					AddFiletoMaster(_selectedfilename);
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				if (_loadedlvl != null)
					AddLeaftoLvl(_selectedfilename);
			}
		}

		private void btnWorkEditDetails_Click(object sender, EventArgs e)
		{
			editLevelDetailsToolStripMenuItem_Click(null, null);
		}

		public void btnWorkRefresh_Click(object sender, EventArgs e)
		{
			if (workingfolder == null)
				return;
			PlaySound("UIrefresh");
			//clear the dgv and reload files in the folder
			workingfolderFiles.Rows.Clear();
			//filter for specific files
			foreach (string file in Directory.GetFiles(workingfolder).Where(x => !x.Contains("leaf_pyramid_outro.txt") && (x.Contains("leaf_") || x.Contains("lvl_") || x.Contains("gate_") || x.Contains("master_") /*|| x.Contains("LEVEL DETAILS")*/ || x.Contains("samp_")))) {
				var filetype = Path.GetFileName(file).Split('_')[0];
				//upon loading a level folder, immediately open the MASTER file
				if (filetype == "master") {
					if (_loadedmaster != file) {
						dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(file), "#.*", ""));
						_loadedmastertemp = file;
						LoadMaster(_load);
						if (panelMaster.Visible == false)
							masterEditorToolStripMenuItem.PerformClick();
					}
				}

				if (!filterleaf && !filterlvl && !filtergate && !filtermaster && !filtersamp) {
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
				}
				else if ((filetype == "leaf" && filterleaf) || (filetype == "lvl" && filterlvl) || (filetype == "gate" && filtergate) || (filetype == "master" && filtermaster) || (filetype == "samp" && filtersamp)) {
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
				}
			}
			//enable button
			btnWorkDelete.Enabled = workingfolderFiles.RowCount > 0;
			btnWorkCopy.Enabled = workingfolderFiles.RowCount > 0;
			btnWorkNewFile.Enabled = workingfolderFiles.RowCount > 0;
			btnWorkEditDetails.Enabled = workingfolderFiles.RowCount > 0;
		}

		private void btnWorkDelete_Click(object sender, EventArgs e)
		{
			//make user confirm file deletion
			if (workingfolderFiles.CurrentRow.Index != -1 && MessageBox.Show("Are you sure you want to delete this file?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				//check if file being deleted is LEVEL DETAILS
				if (workingfolderFiles.CurrentRow.Cells[1].Value.ToString().Contains("LEVEL DETAILS") && MessageBox.Show("You are about to delete the LEVEL DETAILS file. This file is required for the mod loader tool to load the level. Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
				File.Delete($@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.CurrentRow.Index].Value}.txt");
				PlaySound("UIdelete");
				//call the refresh method so the dgv updates
				btnWorkRefresh_Click(null, null);
			}
		}

		private void btnWorkCopy_Click(object sender, EventArgs e)
		{
			//split file type from file name
			//check if it is copy-able
			string[] file = workingfolderFiles.SelectedCells[1].Value.ToString().Split(new[] { '_' }, 2);
			if (file[0] == "LEVEL DETAILS" || file[0] == "master") {
				MessageBox.Show("You may not duplicate that file", "You cannot do that");
				return;
			}

			string newfilename = "";
			//create file renaming dialog and show it
			FileNameDialog filenamedialog = new FileNameDialog();
			filenamedialog.StartPosition = FormStartPosition.Manual;
			filenamedialog.Location = MousePosition;
			filenamedialog.lblRenameFileType.Image = (Image)Properties.Resources.ResourceManager.GetObject(file[0]);
			if (filenamedialog.ShowDialog() == DialogResult.Yes) {
				newfilename = filenamedialog.txtWorkingRename.Text;
			}
			//if NOT yes, return and skip everything else below
			else
				return;
			//check if the chosen name exists in the level folder
			if (File.Exists($@"{workingfolder}\{file[0]}_{newfilename}.txt")) {
				MessageBox.Show("File name already exists in the Working Folder", "File duplicate error");
				return;
			}


			File.Copy($@"{workingfolder}\{file[0]}_{file[1]}.txt", $@"{workingfolder}\{file[0]}_{newfilename}.txt");
			PlaySound("UIkcopy");
			//add new file to workingfolder DGV
			workingfolderFiles.Rows.Insert(workingfolderFiles.CurrentRow.Index + 1, new[] { Properties.Resources.ResourceManager.GetObject(file[0]), $@"{file[0]}_{newfilename}"});
			workingfolderFiles.Rows[workingfolderFiles.CurrentRow.Index + 1].Cells[1].Selected = true;
			SaveFileType(file[0], $@"{workingfolder}\{file[0]}_{newfilename}.txt");
		}

		bool filterleaf, filterlvl, filtergate, filtermaster, filtersamp = false;
		private void filterLeaf_CheckedChanged(object sender, EventArgs e) {filterleaf = filterLeaf.Checked; btnWorkRefresh_Click(null, null); PlaySound("UIselect"); }
		private void filterLvl_CheckedChanged(object sender, EventArgs e) { filterlvl = filterLvl.Checked; btnWorkRefresh_Click(null, null); PlaySound("UIselect"); }
		private void filterGate_CheckedChanged(object sender, EventArgs e) { filtergate = filterGate.Checked; btnWorkRefresh_Click(null, null); PlaySound("UIselect"); }
		private void filterMaster_CheckedChanged(object sender, EventArgs e) { filtermaster = filterMaster.Checked; btnWorkRefresh_Click(null, null); PlaySound("UIselect"); }
		private void filterSamp_CheckedChanged(object sender, EventArgs e) { filtersamp = filterSamp.Checked; btnWorkRefresh_Click(null, null); PlaySound("UIselect"); }

		///Drag and drop files to copy them to the workingfolder
		private void workingfolderFiles_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (File.Exists(data[0])) {
					e.Effect = DragDropEffects.Copy;
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}
		private void workingfolderFiles_DragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string filepath in data) {
				if (!File.Exists($@"{workingfolder}\{Path.GetFileName(filepath)}")) {
					File.Copy(filepath, $@"{workingfolder}\{Path.GetFileName(filepath)}");
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(Path.GetFileNameWithoutExtension(filepath).Split('_')[0]), Path.GetFileNameWithoutExtension(filepath));
					workingfolderFiles.Sort(workingfolderFiles.Columns[1], System.ComponentModel.ListSortDirection.Ascending);
				}
				else
					MessageBox.Show($@"{Path.GetFileName(filepath)} exists in the working folder. File not added.", "Load file error");
			}
		}

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
			//call click method first so the file is loaded before renaming
			DataGridViewCellMouseEventArgs dgvcme = new DataGridViewCellMouseEventArgs(workingfolderFiles.CurrentCell.ColumnIndex, workingfolderFiles.CurrentCell.RowIndex, 0, 0, new MouseEventArgs(MouseButtons.Right, 0, 0, 0, 0));
			workingfolderFiles_CellMouseClick(null, dgvcme);
			//set textbox with name of selected file
			string oldfilename = workingfolderFiles.SelectedCells[1].Value.ToString();
			string newfilename = "";
			string[] file = oldfilename.Split(new[] { '_' }, 2);
			filetype = file[0];
			//check if file is valid to be renamed
			if (filetype == "master" || filetype == "LEVEL DETAILS") {
				MessageBox.Show("You cannot rename this file.", "File error");
				return;
			}

			string newfilepath = "";
			//setup file name dialog and then show it
			FileNameDialog filenamedialog = new FileNameDialog(workingfolder, filetype);
			filenamedialog.txtWorkingRename.Text = file[1];
			filenamedialog.lblRenameFileType.Image = (Image)Properties.Resources.ResourceManager.GetObject(file[0]);
			filenamedialog.Location = PointToClient(Cursor.Position);
			//if YES, rename file
			if (filenamedialog.ShowDialog() == DialogResult.Yes) {
				newfilename = filenamedialog.txtWorkingRename.Text;
				newfilepath = $@"{workingfolder}\{filetype}_{newfilename}.txt";
				File.Move($@"{workingfolder}\{oldfilename}.txt", newfilepath);
				workingfolderFiles.SelectedCells[1].Value = $@"{filetype}_{filenamedialog.txtWorkingRename.Text}";
			}
			//if NO, return and skip the rest below
			else
				return;

			FindInstancesAndRename(file[1], newfilename, filetype);
			SaveFileType(filetype, newfilepath);
		}
		//Duplicate file
		private void duplicateToolStripMenuItem_Click(object sender, EventArgs e) => btnWorkCopy_Click(null, null);
		//Delete file
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) => btnWorkDelete_Click(null, null);

		private void SaveFileType(string filetype, string newfilepath)
        {
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
		}

		private void btnLevelFolder_Click(object sender, EventArgs e)
		{
			cfd_lvl.Title = "Select the level folder";
			//check if the game_dir has been set before. It'll be empty if starting for the first time
			cfd_lvl.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
			//show FolderBrowser, and then set "game_dir" to whatever is chosen
			PlaySound("UIfolderopen");
			if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok) {
				ClearPanels();
				workingfolder = cfd_lvl.FileName;
				panelRecentFiles.Visible = false;
				PlaySound("UIfolderclose");
			}
		}

		private void btnShowRecentFile_Click(object sender, EventArgs e)
		{
			PlaySound("UIfolderopen");
			RecentFiles(Properties.Settings.Default.Recentfiles);
		}

		private void btnExplorer_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo {
				Arguments = workingfolder,
				FileName = "explorer.exe"
			};
			Process.Start(startInfo);
		}

		//handle ENTER and click cell instead.
		//Default behaviour is to go to next cell.
		private void workingfolderFiles_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				e.Handled = true;
				DataGridViewCellMouseEventArgs dgvcme = new DataGridViewCellMouseEventArgs(1, workingfolderFiles.SelectedCells[0].RowIndex, 0, 0, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
				workingfolderFiles_CellMouseClick(null, dgvcme);
			}
		}

		private void FindInstancesAndRename(string oldname, string newname, string filetype)
        {
			object? _load;
			if (filetype == "leaf") {
				foreach (string file in Directory.GetFiles(workingfolder, "lvl_*.txt")) {
					string text = File.ReadAllText(file);
					text = text.Replace($"{oldname}.leaf", $"{newname}.leaf");
					File.WriteAllText(file, text);
				}
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_loadedlvl), "#.*", ""));
				LoadLvl(_load);
			}
			else if (filetype == "lvl") {
				foreach (string file in Directory.GetFiles(workingfolder).Where(f => f.Contains("gate_") || f.Contains("master_"))) {
					string text = File.ReadAllText(file);
					text = text.Replace($"{oldname}.lvl", $"{newname}.lvl");
					File.WriteAllText(file, text);
				}
				if (_loadedgate != null) {
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_loadedgate), "#.*", ""));
					LoadGate(_load);
				}
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_loadedmaster), "#.*", ""));
				LoadMaster(_load);
			}
			else if (filetype == "gate") {
				foreach (string file in Directory.GetFiles(workingfolder, "master_*.txt")) {
					string text = File.ReadAllText(file);
					text = text.Replace($"{oldname}.gate", $"{newname}.gate");
					File.WriteAllText(file, text);
				}
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(_loadedmaster), "#.*", ""));
				LoadMaster(_load);
			}
		}
	}
}
