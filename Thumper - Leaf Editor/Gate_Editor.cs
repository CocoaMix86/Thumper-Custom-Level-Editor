﻿using System;
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
		#region Variables
		bool _savegate = true;
		string _loadedgate
		{
			get { return loadedgate; }
			set
			{
				if (loadedgate != value) {
					loadedgate = value;
					MasterEditorVisible();
					gatesaveAsToolStripMenuItem.Enabled = true;
					gatesaveToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string loadedgate;
		string _loadedgatetemp;
		List<string> boss_name = new List<string>() {
			"Level 1 - triangle 1",
			"Level 1 - triangle 2",
			"Level 1 - crakhed",
			"Level 2 - circle",
			"Level 2 - crakhed",
			"Level 3 - triangle",
			"Level 3 - array",
			"Level 3 - crakhed",
			"Level 4 - triangle",
			"Level 4 - zillapede",
			"Level 4 - crakhed",
			"Level 5 - spiral",
			"Level 5 - crakhed",
			"Level 6 - spirograph",
			"Level 6 - crakhed",
			"Level 7 - tube",
			"Level 7 - crakhed",
			"Level 8 - starfish",
			"Level 8 - crakhed",
			"Level 9 - fractal",
			"Level 9 - crakhed"
		};
		List<string> boss_spn = new List<string>() {
			"gate_tutorial_thumps.spn",
			"boss_gate.spn2",
			"gate_crakhed1.spn",
			"gate_jump.spn",
			"crakhed2.spn",
			"pound_tutorial_boss.spn",
			"boss_array.spn",
			"crakhed3.spn",
			"triangle_boss.spn",
			"zillapede.spn",
			"crakhed4.spn",
			"boss_spiral.spn",
			"crakhed5.spn",
			"spirograph.spn",
			"crakhed6.spn",
			"boss_tube.spn",
			"crakhed7.spn",
			"gate_starfish.spn",
			"crakhed8.spn",
			"gate_frac.spn",
			"crakhed9.spn"
		};
		List<string> boss_ent = new List<string>() {
			"Level 1 - triangle 1",
			"Level 1 - triangle 2",
			"Level 1 - crakhed",
			"Level 2 - circle",
			"Level 2 - crakhed",
			"Level 3 - triangle",
			"Level 3 - array",
			"Level 3 - crakhed",
			"Level 4 - triangle",
			"Level 4 - zillapede",
			"Level 4 - crakhed",
			"Level 5 - spiral",
			"Level 5 - crakhed",
			"Level 6 - spirograph",
			"Level 6 - crakhed",
			"Level 7 - tube",
			"Level 7 - crakhed",
			"Level 8 - starfish",
			"Level 8 - crakhed",
			"Level 9 - fractal",
			"Level 9 - crakhed"
		};
		List<BossData> bossdata = new List<BossData>();
		ObservableCollection<GateLvlData> _gatelvls = new ObservableCollection<GateLvlData>();
		#endregion

		#region EventHandlers
		///        ///
		/// EVENTS ///
		///        ///

		private void gateLvlList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if ((!_savelvl && MessageBox.Show("Current lvl is not saved. Do you want load this one?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savelvl) {
				string _file = (_gatelvls[e.RowIndex].lvlname).Replace(".lvl", "");
				dynamic _load;
				try {
					_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\lvl_{_file}.txt"), "#.*", ""));
				}
				catch {
					MessageBox.Show($@"Could not locate ""lvl_{_file}.txt"" in the same folder as this master. Did you add this leaf from a different folder?");
					return;
				}
				_loadedlvltemp = $@"{workingfolder}\lvl_{_file}.txt";
				//load the selected lvl
				LoadLvl(_load);
			}
		}

		public void gatelvls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//clear dgv
			gateLvlList.RowCount = 0;
			//repopulate dgv from list
			gateLvlList.RowEnter -= gateLvlList_RowEnter;
			foreach (GateLvlData _lvl in _gatelvls) {
				gateLvlList.Rows.Add(new object[] { _lvl.lvlname, _lvl.sentrytype });
				gateLvlList.Rows[_gatelvls.IndexOf(_lvl)].HeaderCell.Value = $"Phase {_gatelvls.IndexOf(_lvl) + 1}";
			}
			gateLvlList.RowEnter += gateLvlList_RowEnter;
			//set selected index. Mainly used when moving items
			///lvlLeafList.CurrentCell = _lvlleafs.Count > 0 ? lvlLeafList.Rows[selectedIndex].Cells[0] : null;
			//enable certain buttons if there are enough items for them
			btnGateLvlDelete.Enabled = _gatelvls.Count > 0;
			btnGateLvlUp.Enabled = _gatelvls.Count > 1;
			btnGateLvlDown.Enabled = _gatelvls.Count > 1;

			//set lvl save flag to false
			SaveGate(false);
		}
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnGateLvlDelete_Click(object sender, EventArgs e) => _gatelvls.RemoveAt(gateLvlList.CurrentRow.Index);
		private void btnGateLvlAdd_Click(object sender, EventArgs e)
		{
			//don't load new lvl if gate has 4 phases
			if (_gatelvls.Count == 4) {
				MessageBox.Show("You can only add 4 phases to a boss.");
				return;
			}
			//show file dialog
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "Thumper Gate File (*.txt)|lvl_*.txt";
				ofd.Title = "Load a Thumper Lvl file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					//parse leaf to JSON
					dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
					//check if file being loaded is actually a leaf. Can do so by checking the JSON key
					if ((string)_load["obj_type"] != "SequinLevel") {
						MessageBox.Show("This does not appear to be a lvl file!", "Lvl load error");
						return;
					}
					//check if lvl exists in the same folder as the gate. If not, allow user to copy file.
					//this is why I utilize workingfolder
					/*if (Path.GetDirectoryName(ofd.FileName) != workingfolder) {
						if (MessageBox.Show("The lvl you chose does not exist in the same folder as this gate. Do you want to copy it to this folder and load it?", "Lvl load error", MessageBoxButtons.YesNo) == DialogResult.Yes)
							File.Copy(ofd.FileName, $@"{workingfolder}\{Path.GetFileName(ofd.FileName)}");
						else
							return;
					}*/
					//add leaf data to the list
					_gatelvls.Add(new GateLvlData() {
						lvlname = (string)_load["obj_name"],
						sentrytype = "SENTRY_NONE"
					});
				}
			}
		}

		private void btnGateLvlUp_Click(object sender, EventArgs e)
		{

		}

		private void btnGateLvlDown_Click(object sender, EventArgs e)
		{

		}
		#endregion

		#region Methods
		///         ///
		/// Methods ///
		///         ///
		         
		public void InitializeGateStuff()
		{
			_gatelvls.CollectionChanged += gatelvls_CollectionChanged;
			///Customize Lvl list a bit
			gateLvlList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 12, GraphicsUnit.Pixel)
			};
			gateLvlList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			///populate boss data
			foreach (string s in boss_name) {
				bossdata.Add(new BossData() {
					boss_name = s,
					boss_spn = boss_spn[boss_name.IndexOf(s)],
					boss_ent = boss_ent[boss_name.IndexOf(s)]
				});
			}
			///add boss data to dropdown
			dropGateBoss.DataSource = bossdata;
			dropGateBoss.DisplayMember = "boss_name";
			dropGateBoss.ValueMember = "boss_spn";
		}

		public void SaveGate(bool save)
		{
			_savegate = save;
			if (!save) {
				if (!lblGateName.Text.Contains("(unsaved)"))
					lblGateName.Text += " (unsaved)";
			}
			else {
				lblGateName.Text = lblGateName.Text.Replace(" (unsaved)", "");
			}
		}
		#endregion
	}
}