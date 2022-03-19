using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ControlManager;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		public string workingfolder {
			get { return _workingfolder; }
			set
			{
				//check if `set` value is different than current stored value
				if (_workingfolder != value) {
					//also only change workingfolders if user says yes to data loss
					if (!_saveleaf || !_savelvl || !_savemaster || !_savegate) {
						if (MessageBox.Show("Some files are unsaved. Are you sure you want to change working folders?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
							return;
					}
					//if different, set it, then repopulate lvls in workingfolder
					//these are used in the Master Editor panel
					_workingfolder = value;
					lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList();
					lvlsinworkfolder.Add("");
					lvlsinworkfolder.Sort();
					//add lvl list as datasources to dropdowns
					dropMasterCheck.DataSource = lvlsinworkfolder.ToList();
					dropMasterIntro.DataSource = lvlsinworkfolder.ToList();
					dropMasterLvlLeader.DataSource = lvlsinworkfolder.ToList();
					dropMasterLvlRest.DataSource = lvlsinworkfolder.ToList();
					dropGatePre.DataSource = lvlsinworkfolder.ToList();
					dropGatePost.DataSource = lvlsinworkfolder.ToList();
					dropGateRestart.DataSource = lvlsinworkfolder.ToList();

					//when workingfolder changes, reset panels
					LeafEditorVisible(false); SaveLeaf(true);
					LvlEditorVisible(false); SaveLvl(true);
					GateEditorVisible(false); SaveGate(true);
					MasterEditorVisible(false); SaveMaster(true);

					//set Working Folder panel data
					workingfolderFiles.Rows.Clear();
					workingfolderFiles.RowEnter -= new DataGridViewCellEventHandler(workingfolderFiles_RowEnter);
					foreach (string file in Directory.GetFiles(workingfolder).Where(x => !x.Contains("leaf_pyramid_outro.txt") && (x.Contains("leaf_") || x.Contains("lvl_") || x.Contains("gate_") || x.Contains("master_") || x.Contains("LEVEL DETAILS")))) {
						workingfolderFiles.Rows.Add(Path.GetFileName(file));
					}
					workingfolderFiles.RowEnter += new DataGridViewCellEventHandler(workingfolderFiles_RowEnter);
					lblWorkingFolder.Text = $"Working Folder - {value}";
					btnWorkRefresh.Enabled = true;
					editLevelDetailsToolStripMenuItem.Enabled = true;
					regenerateDefaultFilesToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string _workingfolder;
		public dynamic helptext;
		public List<string> lvlsinworkfolder = new List<string>();
		public string _dgfocus;
		#endregion


		public FormLeafEditor()
		{
			InitializeComponent();
			ColorFormElements();
		}
		///Color elements based on set properties
		private void ColorFormElements()
		{
			this.BackColor = Properties.Settings.Default.custom_bgcolor;
			menuStrip.BackColor = Properties.Settings.Default.custom_menucolor;
			panelLeaf.BackColor = Properties.Settings.Default.custom_panelcolor;
			panelLevel.BackColor = Properties.Settings.Default.custom_panelcolor;
			panelGate.BackColor = Properties.Settings.Default.custom_panelcolor;
			panelMaster.BackColor = Properties.Settings.Default.custom_panelcolor;
			panelSample.BackColor = Properties.Settings.Default.custom_panelcolor;
			panelWorkingFolder.BackColor = Properties.Settings.Default.custom_panelcolor;
		}

		///Repaints toolstrip separators to have gray backgrounds
		private void toolStripSeparator_Paint(object sender, PaintEventArgs e)
		{
			ToolStripSeparator sep = (ToolStripSeparator)sender;
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, 40, 40)), 0, 0, sep.Width, sep.Height);
			e.Graphics.DrawLine(new Pen(Color.White), 30, sep.Height / 2, sep.Width - 4, sep.Height / 2);
		}

		///Toolstrip - INTERPOLATOR
		private void interpolatorToolStripMenuItem_Click(object sender, EventArgs e) => new Interpolator().Show();

		///Toolstrip - VIEW MENU
		//Visible - LEaf Editor
		private void leafEditorToolStripMenuItem_Click(object sender, EventArgs e) => panelLeaf.Visible = leafEditorToolStripMenuItem.Checked;
		//Visible - Level Editor
		private void levelEditorToolStripMenuItem_Click(object sender, EventArgs e) => panelLevel.Visible = levelEditorToolStripMenuItem.Checked;
		//Visble - Gate Editor
		private void gateEditorToolStripMenuItem_Click(object sender, EventArgs e) => panelGate.Visible = gateEditorToolStripMenuItem.Checked;
		//Visible - Master Editor
		private void masterEditorToolStripMenuItem_Click(object sender, EventArgs e) => panelMaster.Visible = masterEditorToolStripMenuItem.Checked;
		//Visbile - Working Folder
		private void workingFolderToolStripMenuItem_Click(object sender, EventArgs e) => panelWorkingFolder.Visible = workingFolderToolStripMenuItem.Checked;
		//Visble - Sample Editor
		private void sampleEditorToolStripMenuItem_Click(object sender, EventArgs e) => panelSample.Visible = sampleEditorToolStripMenuItem.Checked;

		///Toolstrip - HELP
		//About...
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
		//Help
		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}
		//Tentacles, Paths...
		private void tentaclesPathsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1wG1Ik_50sd2KeUaX19H8e1jjgl-avf-d8SqZk7rxrkQ");

		///Toolstrip - BRING TO FRONT items
		private void bTFLeafToolStripMenuItem_Click(object sender, EventArgs e) => panelLeaf.BringToFront();
		private void bTFLvlToolStripMenuItem_Click(object sender, EventArgs e) => panelLevel.BringToFront();
		private void bTFGateToolStripMenuItem_Click(object sender, EventArgs e) => panelGate.BringToFront();
		private void bTFMasterToolStripMenuItem_Click(object sender, EventArgs e) => panelMaster.BringToFront();
		private void bTFFolderToolStripMenuItem_Click(object sender, EventArgs e) => panelWorkingFolder.BringToFront();
		private void bTFSampleToolStripMenuItem_Click(object sender, EventArgs e) => panelSample.BringToFront();

		/// NEW CUSTOM LEVEL FOLDER
		private void newLevelFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogInput customlevel = new DialogInput();
			//show the new level folder dialog box
			if (customlevel.ShowDialog() == DialogResult.OK) {
				//if all OK, populate new JObject with data from the form
				CreateCustomLevelFolder(customlevel);
			}
			customlevel.Dispose();
		}

		///EXIT APP
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		///FORM CLOSING - check if anything is unsaved
		private void FormLeafEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_saveleaf || !_savelvl || !_savemaster || !_savegate)
			{
				if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					e.Cancel = true;
			}
			//save panel sizes and locations
			Properties.Settings.Default.leafeditorloc = panelLeaf.Location;
			Properties.Settings.Default.leafeditorsize = panelLeaf.Size;
			Properties.Settings.Default.lvleditorloc = panelLevel.Location;
			Properties.Settings.Default.lvleditorsize = panelLevel.Size;
			Properties.Settings.Default.gateeditorloc = panelGate.Location;
			Properties.Settings.Default.gateeditorsize = panelGate.Size;
			Properties.Settings.Default.mastereditorloc = panelMaster.Location;
			Properties.Settings.Default.mastereditorsize = panelMaster.Size;
			Properties.Settings.Default.folderloc = panelWorkingFolder.Location;
			Properties.Settings.Default.foldersize = panelWorkingFolder.Size;
			Properties.Settings.Default.sampleeditorsize = panelSample.Size;
			Properties.Settings.Default.sampleeditorloc = panelSample.Location;

			Properties.Settings.Default.Save();
		}
		///FORM
		private void FormLeafEditor_Load(object sender, EventArgs e)
		{
			//setup datagrids with proper formatting
			InitializeTracks(trackEditor);
			InitializeTracks(lvlSeqObjs);
			InitializeTracks(lvlLeafList);
			InitializeTracks(masterLvlList);
			InitializeTracks(gateLvlList);
			InitializeTracks(workingfolderFiles);
			InitializeLvlStuff();
			InitializeMasterStuff();
			InitializeGateStuff();
			//set panels to be resizeable
			ControlMoverOrResizer.Init(panelLeaf);
			ControlMoverOrResizer.Init(panelLevel);
			ControlMoverOrResizer.Init(panelMaster);
			ControlMoverOrResizer.Init(panelGate);
			ControlMoverOrResizer.Init(panelWorkingFolder);
			ControlMoverOrResizer.Init(workingfolderFiles, panelWorkingFolder);
			ControlMoverOrResizer.Init(panelSample);
			ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize;

			///import help text
			helptext = JsonConvert.DeserializeObject(Properties.Resources.helptext);

			///Create directory for leaf templates
			if (!Directory.Exists(@"templates")) {
				Directory.CreateDirectory(@"templates");
				File.WriteAllText(@"templates\leaf_singletrack.txt", Properties.Resources.leaf_singletrack);
				File.WriteAllText(@"templates\leaf_multitrack.txt", Properties.Resources.leaf_multitrack);
			}

			///import selectable objects from file and parse them into lists for manipulation
			//splits input at "###". Each section is a collection of param_paths
			if (!File.Exists(@"templates\track_objects.txt"))
				File.WriteAllText(@"templates\track_objects.txt", Properties.Resources.track_objects);
			var import = (File.ReadAllText(@"templates\track_objects.txt")).Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
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
					footer = new List<JArray>()
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
						objpar.footer.Add(JArray.Parse(import3[6]));
					}
					catch {
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
			//
			SaveLeaf(true);

			///set a bunch of tool tips
			//Leaf tooltips
			toolTip1.SetToolTip(btnTrackClear, "Clears the selected track of data");
			//Lvl tooltips
			toolTip1.SetToolTip(btnLvlSeqClear, "Clears the selected track of data");
			//Master tooltips
			toolTip1.SetToolTip(btnMasterOpenCheckpoint, "Opens the selected checkpoint in the Lvl Editor");
			toolTip1.SetToolTip(btnMasterOpenIntro, "Opens the selected intro lvl in the Lvl Editor");
			toolTip1.SetToolTip(btnMasterOpenLeader, "Opens the selected leader lvl in the Lvl Editor");
			toolTip1.SetToolTip(btnMasterOpenRest, "Opens the selected rest lvl in the Lvl Editor");
			//Gate tooltips
			toolTip1.SetToolTip(btnGateOpenPre, "Opens the selected Pre lvl in the Lvl Editor");
			toolTip1.SetToolTip(btnGateOpenPost, "Opens the selected Post lvl in the Lvl Editor");
			toolTip1.SetToolTip(btnGateOpenRestart, "Opens the selected Restart lvl in the Lvl Editor");

			//load size and location data for panels
			panelLeaf.Size = Properties.Settings.Default.leafeditorsize;
			panelLeaf.Location = Properties.Settings.Default.leafeditorloc;
			panelLevel.Location = Properties.Settings.Default.lvleditorloc;
			panelLevel.Size = Properties.Settings.Default.lvleditorsize;
			panelGate.Location = Properties.Settings.Default.gateeditorloc;
			panelGate.Size = Properties.Settings.Default.gateeditorsize;
			panelMaster.Location = Properties.Settings.Default.mastereditorloc;
			panelMaster.Size = Properties.Settings.Default.mastereditorsize;
			panelWorkingFolder.Location = Properties.Settings.Default.folderloc;
			panelWorkingFolder.Size = Properties.Settings.Default.foldersize;
			panelSample.Size = Properties.Settings.Default.sampleeditorsize;
			panelSample.Location = Properties.Settings.Default.sampleeditorloc;
		}

		public void CreateCustomLevelFolder(DialogInput input)
		{
			JObject level_details = new JObject {
				{ "level_name", input.txtCustomName.Text },
				{ "difficulty", input.txtCustomDiff.Text },
				{ "description", input.txtDesc.Text },
				{ "author", input.txtCustomAuthor.Text }
			};
			//then write the file to the new folder that was created from the form
			File.WriteAllText($@"{input.txtCustomPath.Text}\LEVEL DETAILS.txt", JsonConvert.SerializeObject(level_details, Formatting.Indented));
			//these 4 files below are required defaults of new levels.
			//create them if they don't exist
			if (!File.Exists($@"{input.txtCustomPath.Text}\leaf_pyramid_outro.txt")) {
				File.WriteAllText($@"{input.txtCustomPath.Text}\leaf_pyramid_outro.txt", Properties.Resources.leaf_pyramid_outro);
			}
			if (!File.Exists($@"{input.txtCustomPath.Text}\samp_default.txt")) {
				File.WriteAllText($@"{input.txtCustomPath.Text}\samp_default.txt", Properties.Resources.samp_default);
			}
			if (!File.Exists($@"{input.txtCustomPath.Text}\spn_default.txt")) {
				File.WriteAllText($@"{input.txtCustomPath.Text}\spn_default.txt", Properties.Resources.spn_default);
			}
			if (!File.Exists($@"{input.txtCustomPath.Text}\xfm_default.txt")) {
				File.WriteAllText($@"{input.txtCustomPath.Text}\xfm_default.txt", Properties.Resources.xfm_default);
			}
			//finally, set workingfolder
			workingfolder = input.txtCustomPath.Text;
			///create samp_ files if any boxes are checked
			//level 1
			if (input.chkLevel1.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level1_320bpm.txt", Properties.Resources.samp_level1_320bpm);
			else if (File.Exists($@"{workingfolder}\samp_level1_320bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level1_320bpm.txt");
			//level 2
			if (input.chkLevel2.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level2_340bpm.txt", Properties.Resources.samp_level2_340bpm);
			else if (File.Exists($@"{workingfolder}\samp_level2_340bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level2_340bpm.txt");
			//level 3
			if (input.chkLevel3.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level3_360bpm.txt", Properties.Resources.samp_level3_360bpm);
			else if (File.Exists($@"{workingfolder}\samp_level3_360bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level3_360bpm.txt");
			//level 4
			if (input.chkLevel4.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level4_380bpm.txt", Properties.Resources.samp_level4_380bpm);
			else if (File.Exists($@"{workingfolder}\samp_level4_380bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level4_380bpm.txt");
			//level 5
			if (input.chkLevel5.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level5_400bpm.txt", Properties.Resources.samp_level5_400bpm);
			else if (File.Exists($@"{workingfolder}\samp_level5_400bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level5_400bpm.txt");
			//level 6
			if (input.chkLevel6.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level6_420bpm.txt", Properties.Resources.samp_level6_420bpm);
			else if (File.Exists($@"{workingfolder}\samp_level6_420bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level6_420bpm.txt");
			//level 7
			if (input.chkLevel7.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level7_440bpm.txt", Properties.Resources.samp_level7_440bpm);
			else if (File.Exists($@"{workingfolder}\samp_level7_440bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level7_440bpm.txt");
			//level 8
			if (input.chkLevel8.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level8_460bpm.txt", Properties.Resources.samp_level8_460bpm);
			else if (File.Exists($@"{workingfolder}\samp_level8_460bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level8_460bpm.txt");
			//level 9
			if (input.chkLevel9.Checked)
				File.WriteAllText($@"{workingfolder}\samp_level9_480bpm.txt", Properties.Resources.samp_level9_480bpm);
			else if (File.Exists($@"{workingfolder}\samp_level9_480bpm.txt"))
				File.Delete($@"{workingfolder}\samp_level9_480bpm.txt");
			//Dissonance
			if (input.chkDissonance.Checked)
				File.WriteAllText($@"{workingfolder}\samp_dissonant.txt", Properties.Resources.samp_dissonant);
			else if (File.Exists($@"{workingfolder}\samp_dissonant.txt"))
				File.Delete($@"{workingfolder}\samp_dissonant.txt");
			//Global Drones
			if (input.chkGlobal.Checked)
				File.WriteAllText($@"{workingfolder}\samp_globaldrones.txt", Properties.Resources.samp_globaldrones);
			else if (File.Exists($@"{workingfolder}\samp_globaldrones.txt"))
				File.Delete($@"{workingfolder}\samp_globaldrones.txt");
			//Rests
			if (input.chkRests.Checked)
				File.WriteAllText($@"{workingfolder}\samp_rests.txt", Properties.Resources.samp_rests);
			else if (File.Exists($@"{workingfolder}\samp_rests.txt"))
				File.Delete($@"{workingfolder}\samp_rests.txt");
			//Misc
			if (input.chkMisc.Checked)
				File.WriteAllText($@"{workingfolder}\samp_misc.txt", Properties.Resources.samp_misc);
			else if (File.Exists($@"{workingfolder}\samp_misc.txt"))
				File.Delete($@"{workingfolder}\samp_misc.txt");
		}

		
		///FORM RESIZE
		///PANEL LABELS - change size or close
		private void lblMasterClose_Click(object sender, EventArgs e) => masterEditorToolStripMenuItem.PerformClick();
		private void lblLvlClose_Click(object sender, EventArgs e) => levelEditorToolStripMenuItem.PerformClick();
		private void lblLeafClose_Click(object sender, EventArgs e) => leafEditorToolStripMenuItem.PerformClick();
		private void lblGateClose_Click(object sender, EventArgs e) => gateEditorToolStripMenuItem.PerformClick();
		private void lblWorkClose_Click(object sender, EventArgs e) => workingFolderToolStripMenuItem.PerformClick();
		private void lblSampleClose_Click(object sender, EventArgs e) => sampleEditorToolStripMenuItem.PerformClick();
		private void Close_MouseEnter(object sender, EventArgs e)
		{
			(sender as Label).BackColor = Color.Red;
			(sender as Label).BorderStyle = BorderStyle.Fixed3D;
		}

		private void Close_MouseLeave(object sender, EventArgs e)
		{
			(sender as Label).BackColor = Color.FromArgb(55, 55, 55);
			(sender as Label).BorderStyle = BorderStyle.FixedSingle;
		}

		/// 
		/// VARIOUS POPUPS FOR HELP TEXT
		/// 
		private void lblMasterlvllistHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterLvlList"], "Master Editor Help");
		private void lblMasterRestHelp_Click(object sender, EventArgs e) => MessageBox.Show((string) helptext["masterRest"], "Master Editor Help");
		private void lblMasterCheckpointLeaderHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterCheckLeader"], "Master Editor Help");
		private void lblConfigColorHelp_Click(object sender, EventArgs e) => new ImageMessageBox("railcolorhelp").Show();
		private void lblGatePreHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["gatePre"], "Gate Editor Help");
		private void lblGateSectionHelp_Click(object sender, EventArgs e) => new ImageMessageBox("bosssectionhelp").Show();

		private void editLevelDetailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dynamic _load = new JObject();
			try {
				//atempt to parse JSON of LEVEL DETAILS. This wil lalso take care of the situation if it doesn't exist
				_load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\LEVEL DETAILS.txt"), "#.*", ""));
			}
			catch {	}
			DialogInput customlevel = new DialogInput();
			//set the form text fields to whatever is in LEVEL DETAILS
			customlevel.txtCustomPath.Text = workingfolder;
			//if the LEVEL DETAILS file is missing, or missing parameters, this fill fill the blanks will empty space
			customlevel.txtCustomName.Text = _load.ContainsKey("level_name") ? (string)_load["level_name"] : "";
			customlevel.txtCustomDiff.Text = _load.ContainsKey("difficulty") ? (string)_load["difficulty"] : "";
			customlevel.txtDesc.Text = _load.ContainsKey("description") ? (string)_load["description"] : "";
			customlevel.txtCustomAuthor.Text = _load.ContainsKey("author") ? (string)_load["author"] : "";
			//set samp pack checks
			customlevel.chkLevel1.Checked = File.Exists($@"{workingfolder}\samp_level1_320bpm.txt");
			customlevel.chkLevel2.Checked = File.Exists($@"{workingfolder}\samp_level2_340bpm.txt");
			customlevel.chkLevel3.Checked = File.Exists($@"{workingfolder}\samp_level3_360bpm.txt");
			customlevel.chkLevel4.Checked = File.Exists($@"{workingfolder}\samp_level4_380bpm.txt");
			customlevel.chkLevel5.Checked = File.Exists($@"{workingfolder}\samp_level5_400bpm.txt");
			customlevel.chkLevel6.Checked = File.Exists($@"{workingfolder}\samp_level6_420bpm.txt");
			customlevel.chkLevel7.Checked = File.Exists($@"{workingfolder}\samp_level7_440bpm.txt");
			customlevel.chkLevel8.Checked = File.Exists($@"{workingfolder}\samp_level8_460bpm.txt");
			customlevel.chkLevel9.Checked = File.Exists($@"{workingfolder}\samp_level9_480bpm.txt");
			customlevel.chkDissonance.Checked = File.Exists($@"{workingfolder}\samp_dissonant.txt");
			customlevel.chkGlobal.Checked = File.Exists($@"{workingfolder}\samp_globaldrones.txt");
			customlevel.chkRests.Checked = File.Exists($@"{workingfolder}\samp_rests.txt");
			customlevel.chkMisc.Checked = File.Exists($@"{workingfolder}\samp_misc.txt");
			//show the new level folder dialog box
			if (customlevel.ShowDialog() == DialogResult.OK) {
				//if all OK, populate new JObject with data from the form
				CreateCustomLevelFolder(customlevel);
			}
			customlevel.Dispose();
		}

		private void regenerateDefaultFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("This will overwrite the \"default\" files in the working folder. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				File.WriteAllText($@"{workingfolder}\leaf_pyramid_outro.txt", Properties.Resources.leaf_pyramid_outro);
				File.WriteAllText($@"{workingfolder}\samp_default.txt", Properties.Resources.samp_default);
				File.WriteAllText($@"{workingfolder}\spn_default.txt", Properties.Resources.spn_default);
				File.WriteAllText($@"{workingfolder}\xfm_default.txt", Properties.Resources.xfm_default);
			}
		}

		private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CustomizeWorkspace custom = new CustomizeWorkspace();
			if (custom.ShowDialog() == DialogResult.OK) {
				ColorFormElements();
			}
		}
	}
}
