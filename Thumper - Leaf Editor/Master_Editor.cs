using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _savemaster;
		string _loadedmaster;

		ObservableCollection<MasterLvlData> _masterlvls = new ObservableCollection<MasterLvlData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS ///
		///         ///

		/// DGV MASTERLVLLIST
		//Row Enter (load the selected lvl)
		private void masterLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{

		}
		//Cell value changed (for checkboxes)
		private void masterLvlList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try {
				_masterlvls[e.RowIndex].checkpoint = bool.Parse(masterLvlList[1, e.RowIndex].Value.ToString());
				_masterlvls[e.RowIndex].playplus = bool.Parse(masterLvlList[2, e.RowIndex].Value.ToString());
			} catch { }
		}

		public void masterlvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear dgv
			masterLvlList.RowCount = 0;
			//repopulate dgv from list
			masterLvlList.RowEnter -= masterLvlList_RowEnter;
			for (int x = 0; x < _masterlvls.Count; x++) {
				masterLvlList.Rows.Add(new object[] { _masterlvls[x].lvlname, _masterlvls[x].checkpoint, _masterlvls[x].playplus });
			}
			masterLvlList.RowEnter += masterLvlList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnMasterLvlDelete.Enabled = _masterlvls.Count > 0;
			btnMasterLvlUp.Enabled = _masterlvls.Count > 1;
			btnMasterLvlDown.Enabled = _masterlvls.Count > 1;

			//set lvl save flag to false
			SaveMaster(false);
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///
		        
		private void btnMasterLvlDelete_Click(object sender, EventArgs e) => _masterlvls.RemoveAt(masterLvlList.CurrentRow.Index);
		private void btnMasterLvlAdd_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Lvl File (*.txt)|*.txt";
				ofd.Title = "Load a Thumper Lvl file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					//parse leaf to JSON
					dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
					//check if file being loaded is actually a leaf. Can do so by checking the JSON key
					if ((string)_load["obj_type"] != "SequinLevel") {
						MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
						return;
					}
					//check if lvl exists in the same folder as the master. If not, allow user to copy file.
					//this is why I utilize workingfolder
					/*
					if (Path.GetDirectoryName(ofd.FileName) != workingfolder) {
						if (MessageBox.Show("The lvl you chose does not exist in the same folder as this master. Do you want to copy it to this folder and load it?", "Lvl load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
							File.Copy(ofd.FileName, $@"{workingfolder}\{Path.GetFileName(ofd.FileName)}");
						else
							return;
					}*/
					//add leaf data to the list
					_masterlvls.Add(new MasterLvlData() {
						lvlname = (string)_load["obj_name"],
						playplus = true,
						checkpoint = true
					});
				}
			}
		}

		private void btnMasterLvlUp_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = masterLvlList.CurrentRow.Index;
				if (rowIndex == 0)
					return;
				//move leaf in list
				var selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex - 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex - 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveMaster(false);
			}
			catch { }
		}

		private void btnMasterLvlDown_Click(object sender, EventArgs e)
		{
			try {
				// get index of the row for the selected cell
				int rowIndex = masterLvlList.CurrentRow.Index;
				if (rowIndex == _masterlvls.Count - 1)
					return;
				//move leaf in list
				var selectedLvl = _masterlvls[rowIndex];
				_masterlvls.Remove(selectedLvl);
				_masterlvls.Insert(rowIndex + 1, selectedLvl);
				//move selected cell up a row to follow the moved item
				masterLvlList.Rows[rowIndex + 1].Cells[0].Selected = true;
				//sets flag that lvl has unsaved changes
				SaveMaster(false);
			}
			catch { }
		}
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///
		///         
		public void InitializeMasterStuff()
		{
			_masterlvls.CollectionChanged += masterlvls_CollectionChanged;

			///customize Lvl List a bit
			masterLvlList.ColumnCount = 1;
			masterLvlList.RowHeadersVisible = false;
			masterLvlList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 15, GraphicsUnit.Pixel)
			};
			masterLvlList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			masterLvlList.Columns[0].HeaderText = "Lvl";
			masterLvlList.Columns[0].ReadOnly = true;
			///
			///add checkbox column for denoting if a lvl has a checkpoint or not
			DataGridViewCheckBoxColumn _dgvmastercheckpoint = new DataGridViewCheckBoxColumn() {
				HeaderText = "Checkpoint",
				ReadOnly = false
			};
			masterLvlList.Columns.Add(_dgvmastercheckpoint);
			///add checkbox column for denoting if a lvl shows in play+ or not
			DataGridViewCheckBoxColumn _dgvmasterplayplus = new DataGridViewCheckBoxColumn() {
				HeaderText = "Play+",
				ReadOnly = false
			};
			masterLvlList.Columns.Add(_dgvmasterplayplus);
		}

		public void SaveMaster(bool save)
		{
			_savemaster = save;
			if (!save) {
				if (!lblMasterName.Text.Contains("(unsaved)"))
					lblMasterName.Text += " (unsaved)";
			}
			else {
				lblMasterName.Text = lblMasterName.Text.Replace(" (unsaved)", "");
			}
		}
		#endregion
	}
}