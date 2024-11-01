﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
	{
		List<WorkingFolderFileItem> workingfiles = new();

		private void workingfolderFiles_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			LoadFileOnClick(e.RowIndex, e.ColumnIndex);
		}
		private void LoadFileOnClick(int rowindex, int columnindex)
        {
			//do nothing if no cell click
			if (rowindex == -1)
				return;

			dynamic _load;
			string _selectedfilename = "";
			//attempt to load file listed in the dGV
			try {
				//first check if it exists
				_selectedfilename = $@"{workingfolder}\{workingfolderFiles[1, rowindex].Value}.txt";
				if (!File.Exists(_selectedfilename)) {
					MessageBox.Show($"File {workingfolderFiles[1, rowindex].Value}.txt could not be found in the folder. Was it moved or deleted?", "File load error");
					return;
				}
				//atempt to parse JSON
				_load = LoadFileLock(_selectedfilename);
			}
			catch (Exception) {
				//return method if parse fails
				MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.", "File load error");
				return;
			}

			///Search for file reference. Return afterwards, do not attempt to load file into editor
			if (columnindex == 2) {
				//this check skips master and samp, since you can't reference those
				if (!new[] { "samp", "master" }.Any(c => workingfolderFiles.Rows[rowindex].Cells[1].Value.ToString().Contains(c))) {
					string _files = SearchReferences(_load, _selectedfilename);
					MessageBox.Show($"This file is referenced in these files:\n{_files}");
				}
				return;
			}

			if (_load == null) {
				MessageBox.Show("No data parsed in selected file. Try again or replace the file.", "File load error");
				return;
            }

			///Send file off to different load methods based on the file type
			//process SAMP first, since its JSON structure is different, and detectable
			if (_load.ContainsKey("items")) {
				if (_selectedfilename == _loadedsample)
					return;
				//Check if gate is saved or not
				if ((!_savesample && MessageBox.Show("Current sample is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
					return;
				LoadSample(_load, _selectedfilename);
				if (panelSample.Visible == false)
					sampleEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinMaster") {
				if (_selectedfilename == _loadedmaster)
					return;
				LoadMaster(_load, _selectedfilename);
				if (panelMaster.Visible == false)
					masterEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinGate") {
				if (_selectedfilename == _loadedgate)
					return;
				//Check if gate is saved or not
				if ((!_savegate && MessageBox.Show("Current gate is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
					return;
				LoadGate(_load, _selectedfilename);
				if (panelGate.Visible == false)
					gateEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				if (_selectedfilename == _loadedlvl)
					return;
				//Check if lvl is saved or not
				if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
					return;
				LoadLvl(_load, _selectedfilename);
				if (panelLevel.Visible == false)
					levelEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				//don't reload the file if its the same name
				if (_selectedfilename == _loadedleaf)
					return;
				//Check if leaf is saved or not
				if ((!_saveleaf && MessageBox.Show("Current leaf is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
					return;
				LoadLeaf(_load, _selectedfilename);
				//make panel visible if it isn't
				if (panelLeaf.Visible == false)
					leafEditorToolStripMenuItem.PerformClick();
			}
			else
				MessageBox.Show("this is not a valid Custom Level file.");
		}

		private void workingfolderFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == -1 || e.RowIndex == -1)
				return;
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
				_load = LoadFileLock(_selectedfilename);
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
			if (workingfolder == null)
				return;
			editLevelDetailsToolStripMenuItem_Click(null, null);
		}

		public void btnWorkRefresh_Click(object sender, EventArgs e)
		{
			if (workingfolder == null)
				return;
			if (sender != null)
				PlaySound("UIrefresh");
			//clear the dgv and reload files in the folder
			workingfolderFiles.Rows.Clear();
			workingfiles.Clear();
			//filter for specific files
			List<string> filesinfolder = Directory.GetFiles(workingfolder).Where(x => { 
				string file = Path.GetFileName(x);
				return file != "leaf_pyramid_outro.txt" && file != "samp_default.txt" && !file.StartsWith("config_") && (file.StartsWith("leaf_") || file.StartsWith("lvl_") || file.StartsWith("gate_") || file.StartsWith("master_") || file.StartsWith("samp_"));
			}).ToList();
			foreach (string file in filesinfolder) {
                string filetype = Path.GetFileName(file).Split('_')[0];
				//upon loading a level folder, immediately open the MASTER file
				if (filetype == "master") {
					if (_loadedmaster != file) {
						dynamic _load = LoadFileLock(file);
						LoadMaster(_load, file);
						if (!panelMaster.Visible && !masterEditorToolStripMenuItem.Checked)
							masterEditorToolStripMenuItem.PerformClick();
					}
				}

				//if no filters enabled, add the file to list
				if (filefilter == 0x0) {
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
				}
				//otherwise, compare the filetype to any active filter and then add it
				else if ((filetype == "leaf" && (filefilter & 1) == 1) || (filetype == "lvl" && (filefilter & 2) == 2) || (filetype == "gate" && (filefilter & 4) == 4) || (filetype == "master" && (filefilter & 8) == 8) || (filetype == "samp" && (filefilter & 16) == 16)) {
					workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
				}

				workingfiles.Add(new WorkingFolderFileItem() {
					type = filetype,
					filename = Path.GetFileNameWithoutExtension(file),
					index = workingfolderFiles.RowCount - 1
				});
			}
			//enable button
			btnWorkDelete.Enabled = workingfolderFiles.RowCount > 0;
			btnWorkCopy.Enabled = workingfolderFiles.RowCount > 0;
			btnWorkNewFile.Enabled = filesinfolder.Count > 0;
			btnWorkEditDetails.Enabled = filesinfolder.Count > 0;
			btnLeafPanelTemplate.Enabled = true;

            HighlightMissingFile(lvlLeafList, lvlLeafList.Rows.OfType<DataGridViewRow>().Select(x => $@"{workingfolder}\leaf_{x.Cells[1].Value}.txt").ToList());
            HighlightMissingFile(gateLvlList, gateLvlList.Rows.OfType<DataGridViewRow>().Select(x => $@"{workingfolder}\lvl_{x.Cells[1].Value}.txt").ToList());
            HighlightMissingFile(masterLvlList, masterLvlList.Rows.OfType<DataGridViewRow>().Select(x => $@"{workingfolder}\{(_masterlvls[x.Index].lvlname != "" ? "lvl" : "gate")}_{x.Cells[1].Value}.txt").ToList());
        }
		private void workingfolderFiles_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0)
				return;
			//buttons are in column 2, so that's where to draw the button images
			if (e.ColumnIndex == 2) {
				//skip drawing for samp and master
				if (new[] {"samp", "master" }.Any(c => workingfolderFiles.Rows[e.RowIndex].Cells[1].Value.ToString().Contains(c)))
					return;
				e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //get dimensions
                int w = Properties.Resources.icon_zoom.Width;
                int h = Properties.Resources.icon_zoom.Height;
                int x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
				//paint the image
				e.Graphics.DrawImage(Properties.Resources.icon_zoom, new Rectangle(x, y, w, h));
				e.Handled = true;
			}
		}

		private void btnWorkDelete_Click(object sender, EventArgs e)
		{
			//make user confirm file deletion
			if (workingfolderFiles.CurrentRow.Index != -1 && MessageBox.Show($"{workingfolderFiles.CurrentRow.Cells[1].Value}\nAre you sure you want to delete this file?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				//check if file being deleted is LEVEL DETAILS
				if (workingfolderFiles.CurrentRow.Cells[1].Value.ToString().Contains("LEVEL DETAILS") && MessageBox.Show("You are about to delete the LEVEL DETAILS file. This file is required for the mod loader tool to load the level. Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
				string filepath = $@"{workingfolder}\{workingfolderFiles[1, workingfolderFiles.CurrentRow.Index].Value}.txt";
				string filetype = Path.GetFileNameWithoutExtension(filepath).Split('_')[0];
				if (lockedfiles.ContainsKey(filepath)) {
					lockedfiles[filepath].Close();
					lockedfiles.Remove(filepath);
					ClearPanels(filetype);
				}
				if (File.Exists(filepath))
					FileSystem.DeleteFile(filepath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
			if (file[0] is "master") {
				MessageBox.Show("You may not duplicate that file", "You cannot do that");
				return;
			}

            //create file renaming dialog and show it
            FileNameDialog filenamedialog = new(workingfolder, file[0]) {
                StartPosition = FormStartPosition.Manual,
                Location = MousePosition
            };
            filenamedialog.lblRenameFileType.Image = (Image)Properties.Resources.ResourceManager.GetObject(file[0]);

            string newfilename;
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
			//after copy, replace instances of old file name with new file name.
			string filecontents = File.ReadAllText($@"{workingfolder}\{file[0]}_{newfilename}.txt").Replace($"{file[1]}.{file[0]}", $"{newfilename}.{file[0]}");
			File.WriteAllText($@"{workingfolder}\{file[0]}_{newfilename}.txt", filecontents);
			//add new file to workingfolder DGV
			btnWorkRefresh_Click(null, null);
		}

		public byte filefilter = 0x0;
        private void filter_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = sender as ToolStripButton;
			//file filters are handled in a flag system //leaf 1, lvl 2, gate 4, master 8, sample 16
			//the filter buttons have these numbers in their Tags
            filefilter ^= byte.Parse((string)tsb.Tag);
            PlaySound("UIselect");
            btnWorkRefresh_Click(null, null);
        }
		private void filterClear_Click(object sender, EventArgs e)
		{
			//clear all checks and reset flags to 0
			filterLeaf.Checked = filterLvl.Checked = filterGate.Checked = filterMaster.Checked = filterSamp.Checked = false;
            PlaySound("UIselect");
            filefilter = 0x0;
            btnWorkRefresh_Click(null, null);
        }

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
			DataGridViewCellMouseEventArgs dgvcme = new(workingfolderFiles.CurrentCell.ColumnIndex, workingfolderFiles.CurrentCell.RowIndex, 0, 0, new MouseEventArgs(MouseButtons.Right, 0, 0, 0, 0));
			workingfolderFiles_CellMouseClick(null, dgvcme);
			//set textbox with name of selected file
			string oldfilename = workingfolderFiles.SelectedCells[1].Value.ToString();
            string[] file = oldfilename.Split(new[] { '_' }, 2);
            filetype = file[0];
			//check if file is valid to be renamed
			if (filetype is "master" or "LEVEL DETAILS") {
				MessageBox.Show("You cannot rename this file.", "File error");
				return;
			}

            //setup file name dialog and then show it
            FileNameDialog filenamedialog = new(workingfolder, filetype);
            filenamedialog.txtWorkingRename.Text = file[1];
			filenamedialog.lblRenameFileType.Image = (Image)Properties.Resources.ResourceManager.GetObject(file[0]);
			filenamedialog.Location = PointToClient(Cursor.Position);
            string newfilename;

            string newfilepath;
            //if YES, rename file
            if (filenamedialog.ShowDialog() == DialogResult.Yes) {
                newfilename = filenamedialog.txtWorkingRename.Text;
                newfilepath = $@"{workingfolder}\{filetype}_{newfilename}.txt";
				if (lockedfiles.ContainsKey($@"{workingfolder}\{oldfilename}.txt")) {
					lockedfiles[$@"{workingfolder}\{oldfilename}.txt"].Close();
					lockedfiles.Remove($@"{workingfolder}\{oldfilename}.txt");
				}
                File.Move($@"{workingfolder}\{oldfilename}.txt", newfilepath);
            }
            //if NO, return and skip the rest below
            else
                return;

            FindInstancesAndRename(file[1], newfilename, filetype);
			SaveFileType(filetype, newfilepath);
			btnWorkRefresh_Click(null, null);
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
			if (!_saveleaf || !_savelvl || !_savemaster || !_savegate || !_savesample) {
				if (MessageBox.Show("Some files are unsaved. Are you sure you want to open a different folder?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No) {
					return;
				}
			}
			cfd_lvl.Title = "Select the level folder";
			//check if the game_dir has been set before. It'll be empty if starting for the first time
			cfd_lvl.InitialDirectory = workingfolder == null ? AppLocation : Path.GetDirectoryName(workingfolder);
			//show FolderBrowser, and then set "game_dir" to whatever is chosen
			PlaySound("UIfolderopen");
			if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok) {
				if (!File.Exists($@"{cfd_lvl.FileName}\LEVEL DETAILS.txt")) {
					MessageBox.Show("This folder did not contain a LEVEL DETAILS.txt", "Level load fail");
					return;
                }
				workingfolder = cfd_lvl.FileName;
				panelRecentFiles.Visible = false;
				PlaySound("UIfolderclose");
			}
		}

		private void btnShowRecentFile_Click(object sender, EventArgs e)
		{
			PlaySound("UIfolderopen");
			if (panelRecentFiles.Visible)
				panelRecentFiles.Visible = false;
			else
				RecentFiles(Properties.Settings.Default.Recentfiles);
		}

		private void btnExplorer_Click(object sender, EventArgs e)
		{
			ProcessStartInfo startInfo = new() {
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
				LoadFileOnClick(1, workingfolderFiles.SelectedCells[0].RowIndex);
			}

			else if (e.KeyData == Keys.Delete) {
				e.Handled = true;
				btnWorkDelete_Click(null, null);
			}
		}

		private void FindInstancesAndRename(string oldname, string newname, string filetype)
        {
			object _load;
			if (filetype == "leaf") {
				foreach (string file in Directory.GetFiles(workingfolder, "lvl_*.txt")) {
					string text = ((JObject)LoadFileLock(file)).ToString(Formatting.None)  .Replace($"{oldname}.leaf", $"{newname}.leaf");
					dynamic updated = JsonConvert.DeserializeObject(text);
					if (file == _loadedlvl)
						WriteFileLock(lockedfiles[loadedlvl], updated);
					else
						File.WriteAllText(file, text);
				}
				if (_loadedlvl != null) {
					_load = LoadFileLock(_loadedlvl);
					LoadLvl(_load, loadedlvl);
				}
			}
			else if (filetype == "lvl") {
				foreach (string file in Directory.GetFiles(workingfolder).Where(f => f.Contains("gate_") || f.Contains("master_"))) {
					string text = ((JObject)LoadFileLock(file)).ToString(Formatting.None)  .Replace($"{oldname}.lvl", $"{newname}.lvl");
					dynamic updated = JsonConvert.DeserializeObject(text);
					if (file == _loadedmaster)
						WriteFileLock(lockedfiles[loadedmaster], updated);
					else if (file == _loadedgate)
						WriteFileLock(lockedfiles[loadedgate], updated);
					else
						File.WriteAllText(file, text);
				}
				if (_loadedgate != null) {
					_load = LoadFileLock(_loadedgate);
					LoadGate(_load, _loadedgate);
				}
				if (_loadedmaster != null) {
					_load = LoadFileLock(_loadedmaster);
					LoadMaster(_load, _loadedmaster);
				}
			}
			else if (filetype == "gate") {
				foreach (string file in Directory.GetFiles(workingfolder, "master_*.txt")) {
					string text = ((JObject)LoadFileLock(file)).ToString(Formatting.None)  .Replace($"{oldname}.gate", $"{newname}.gate");
					dynamic updated = JsonConvert.DeserializeObject(text);
					if (file == _loadedmaster)
						WriteFileLock(lockedfiles[loadedmaster], updated);
					else
						File.WriteAllText(file, text);
				}
				if (_loadedmaster != null) {
					_load = LoadFileLock(_loadedmaster);
					LoadMaster(_load, _loadedmaster);
				}
			}
		}
	}
}
