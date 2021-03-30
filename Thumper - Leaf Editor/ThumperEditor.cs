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

					//set Working Folder panel data
					workingfolderFiles.Rows.Clear();
					foreach (string file in Directory.GetFiles(workingfolder)) {
						workingfolderFiles.Rows.Add(Path.GetFileName(file));
					}
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
		Panel _formactive;
		#endregion


		public FormLeafEditor() => InitializeComponent();

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

		///Toolstrip - HELP
		//About...
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
		//Help
		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}
		//Tentacles, Paths...
		private void tentaclesPathsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1wG1Ik_50sd2KeUaX19H8e1jjgl-avf-d8SqZk7rxrkQ");

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
			if (!_saveleaf || !_savelvl || !_savemaster)
			{
				if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					e.Cancel = true;
			}
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
			//_formactive is the panel that was last set to Max
			_formactive = panelLeaf;
			//set panels to be resizeable
			ControlMoverOrResizer.Init(panelLeaf);
			ControlMoverOrResizer.Init(panelLevel);
			ControlMoverOrResizer.Init(panelMaster);
			ControlMoverOrResizer.Init(panelGate);
			ControlMoverOrResizer.Init(panelWorkingFolder);
			ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize;

			///import help text
			helptext = JsonConvert.DeserializeObject(Properties.Resources.helptext);

			///import selectable objects from file and parse them into lists for manipulation
			//splits input at "###". Each section is a collection of param_paths
			var import = (Properties.Resources.track_objects).Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
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

			///Create directory for leaf templates
			if (!Directory.Exists(@"templates")) {
				Directory.CreateDirectory(@"templates");
				File.WriteAllText(@"templates\leaf_singletrack.txt", Properties.Resources.leaf_singletrack);
				File.WriteAllText(@"templates\leaf_multitrack.txt", Properties.Resources.leaf_multitrack);
			}

			///set a bunch of tool tips
			//Leaf tooltips
			toolTip1.SetToolTip(btnTrackClear, "Clears the selected track of data");
			//Lvl tooltips
			toolTip1.SetToolTip(btnLvlSeqClear, "Clears the selected track of data");
			//Master tooltips
			toolTip1.SetToolTip(btnMasterOpenCheckpoint, "Opens the selected checkpoint in the Lvl Editor");
			toolTip1.SetToolTip(btnMasterOpenIntro, "Opens the selected intro in the Lvl Editor");
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
		}

		
		///FORM RESIZE
		///PANEL LABELS - change size or close
		private void lblMasterClose_Click(object sender, EventArgs e) => masterEditorToolStripMenuItem.PerformClick();
		private void lblLvlClose_Click(object sender, EventArgs e) => levelEditorToolStripMenuItem.PerformClick();
		private void lblLeafClose_Click(object sender, EventArgs e) => leafEditorToolStripMenuItem.PerformClick();
		private void lblGateClose_Click(object sender, EventArgs e) => gateEditorToolStripMenuItem.PerformClick();
		private void lblWorkClose_Click(object sender, EventArgs e) => workingFolderToolStripMenuItem.PerformClick();
		private void lblLeafMax_Click(object sender, EventArgs e)
		{
		}
		private void lblLevelMax_Click(object sender, EventArgs e)
		{
		}
		private void lblMasterMax_Click(object sender, EventArgs e)
		{
		}

		/// 
		/// VARIOUS POPUPS FOR HELP TEXT
		/// 
		private void lblMasterlvllistHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterLvlList"], "Master Editor Help");
		private void lblMasterRestHelp_Click(object sender, EventArgs e) => MessageBox.Show((string) helptext["masterRest"], "Master Editor Help");
		private void lblMasterCheckpointLeaderHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterCheckLeader"], "Master Editor Help");
		private void lblConfigColorHelp_Click(object sender, EventArgs e) => new ImageMessageBox("railcolorhelp").Show();

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
	}
}
