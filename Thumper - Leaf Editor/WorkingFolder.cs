using System;
using System.Drawing;
using System.Collections.Generic;
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
		private void panelWorkingFolder_SizeChanged(object sender, EventArgs e)
		{
			lblWorkingFolder.MaximumSize = new Size(panelWorkingFolder.Width - 16, 0);
		}

		private void workingfolderFiles_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			dynamic _load = null;
			//attempt to load file listed in the dGV
			try {
				//first check if it exists
				if (!File.Exists($@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}")) {
					MessageBox.Show($"File {workingfolderFiles[0, e.RowIndex].Value} could not be found in the folder. Was it moved or deleted?", "File load error");
					return;
				}
				//atempt to parse JSON
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}"), "#.*", ""));
			} catch {
				//return method if parse fails
				MessageBox.Show("The selected file could not be parsed as JSON.");
				return;
			}
			///Send file off to different load methods based on the file type
			if ((string)_load["obj_type"] == "SequinMaster") {
				_loadedmastertemp = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadMaster(_load);
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				_loadedlvltemp = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadLvl(_load);
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				_loadedleaf = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadLeaf(_load);
			}
			else if (workingfolderFiles[0, e.RowIndex].Value.ToString().Contains("LEVEL DETAILS")) {
				DialogInput customlevel = new DialogInput();
				customlevel.txtCustomPath.Text = workingfolder;
				customlevel.txtCustomName.Text = (string)_load["level_name"];
				customlevel.txtCustomDiff.Text = (string)_load["difficulty"];
				customlevel.txtDesc.Text = (string)_load["description"];
				customlevel.txtCustomAuthor.Text = (string)_load["author"];
				//show the new level folder dialog box
				if (customlevel.ShowDialog() == DialogResult.OK) {
					//if all OK, populate new JObject with data from the form
					CreateCustomLevelFolder(customlevel);
				}
				customlevel.Dispose();
			}
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
			workingfolderFiles.RowEnter -= new DataGridViewCellEventHandler(workingfolderFiles_RowEnter);
			foreach (string file in Directory.GetFiles(workingfolder)) {
				workingfolderFiles.Rows.Add(Path.GetFileName(file));
			}
			workingfolderFiles.RowEnter += new DataGridViewCellEventHandler(workingfolderFiles_RowEnter);
		}

		private void btnWorkDelete_Click(object sender, EventArgs e)
		{
			//make user confirm file deletion
			if (workingfolderFiles.CurrentRow.Index != -1 && MessageBox.Show("Are you sure you want to delete this file?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				//check if file being deleted is LEVEL DETAILS
				if (workingfolderFiles.CurrentRow.Cells[0].Value.ToString().Contains("LEVEL DETAILS") && MessageBox.Show("You are about to delete the LEVEL DETAILS file. This file is required for the mod loader tool to load the level. Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
				File.Delete($@"{workingfolder}\{workingfolderFiles[0, workingfolderFiles.CurrentRow.Index].Value}");
				//call the refresh method so the dgv updates
				btnWorkRefresh_Click(null, null);
			}
		}
	}
}
