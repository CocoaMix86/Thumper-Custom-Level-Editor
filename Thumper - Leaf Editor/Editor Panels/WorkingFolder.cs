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

		private void workingfolderFiles_CellClick(object sender, DataGridViewCellEventArgs e)
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
			else if (workingfolderFiles[1, e.RowIndex].Value.ToString().Contains("LEVEL DETAILS")) {
				editLevelDetailsToolStripMenuItem_Click(null, null);
			}
			else
				MessageBox.Show("this is not a valid Custom Level file.");
		}

		private void workingfolderFiles_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			btnWorkDelete.Enabled = true;
		}

		private void workingfolderFiles_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (workingfolderFiles.RowCount == 0) btnWorkDelete.Enabled = false;
		}

		private void btnWorkRefresh_Click(object sender, EventArgs e)
		{
			//clear the dgv and reload files in the folder
			workingfolderFiles.Rows.Clear();
			//filter for specific files
			foreach (string file in Directory.GetFiles(workingfolder).Where(x => !x.Contains("leaf_pyramid_outro.txt") && (x.Contains("leaf_") || x.Contains("lvl_") || x.Contains("gate_") || x.Contains("master_") || x.Contains("LEVEL DETAILS") || x.Contains("samp_")))) {
				var filetype = Path.GetFileName(file).Split('_')[0];
				workingfolderFiles.Rows.Add(Properties.Resources.ResourceManager.GetObject(filetype), Path.GetFileNameWithoutExtension(file));
			}
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
	}
}
