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

		private void workingfolderFiles_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			//workingfolderFiles_RowEnter(sender, e);
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
			}
			catch {
				//return method if parse fails
				MessageBox.Show("The selected file could not be parsed as JSON.");
				return;
			}
			///Send file off to different load methods based on the file type
			if ((string)_load["obj_type"] == "SequinMaster") {
				_loadedmastertemp = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadMaster(_load);
				if (panelMaster.Visible == false)
					masterEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinGate") {
				_loadedgatetemp = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadGate(_load);
				if (panelGate.Visible == false)
					gateEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLevel") {
				_loadedlvltemp = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadLvl(_load);
				if (panelLevel.Visible == false)
					levelEditorToolStripMenuItem.PerformClick();
			}
			else if ((string)_load["obj_type"] == "SequinLeaf") {
				_loadedleaf = $@"{workingfolder}\{workingfolderFiles[0, e.RowIndex].Value}";
				LoadLeaf(_load);
				if (panelLeaf.Visible == false)
					leafEditorToolStripMenuItem.PerformClick();
			}
			else if (workingfolderFiles[0, e.RowIndex].Value.ToString().Contains("LEVEL DETAILS")) {
				editLevelDetailsToolStripMenuItem_Click(null, null);
			}
			else
				MessageBox.Show("this is not a valid Custom Level file.");
		}
		private void workingfolderFiles_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_dgfocus != "workingfolderFiles") {
				_dgfocus = "workingfolderFiles";
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
			//filter for specific files
			foreach (string file in Directory.GetFiles(workingfolder).Where(x => !x.Contains("leaf_pyramid_outro.txt") && (x.Contains("leaf_") || x.Contains("lvl_") || x.Contains("gate_") || x.Contains("master_") || x.Contains("LEVEL DETAILS")))) {
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
