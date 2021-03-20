using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ControlManager;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor : Form
	{
		#region Variables
		public string workingfolder {
			get { return _workingfolder; }
			set
			{
				_workingfolder = value;
				lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList();
				lvlsinworkfolder.Add("");
				lvlsinworkfolder.Sort();
				///add lvl list as datasources to dropdowns
				dropMasterCheck.DataSource = lvlsinworkfolder.ToList();
				dropMasterIntro.DataSource = lvlsinworkfolder.ToList();
				dropMasterLvlLeader.DataSource = lvlsinworkfolder.ToList();
				dropMasterLvlRest.DataSource = lvlsinworkfolder.ToList();
			}
		}
		private string _workingfolder;
		public dynamic helptext;
		public List<string> lvlsinworkfolder = new List<string>();
		Panel _formactive;
		#endregion


		public FormLeafEditor() => InitializeComponent();

		///Toolstrip - INTERPOLATOR
		private void interpolatorToolStripMenuItem_Click(object sender, EventArgs e) => new Interpolator().Show();
		///Toolstrip - VIEW MENU
		//Visible - LEaf Editor
		private void leafEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelLeaf.Visible = leafEditorToolStripMenuItem.Checked;
		}
		//Visible - Level Editor
		private void levelEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelLevel.Visible = levelEditorToolStripMenuItem.Checked;
		}
		//Visible - Master Editor
		private void masterEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panelMaster.Visible = masterEditorToolStripMenuItem.Checked;
		}
		///Toolstrip - HELP
		//About...
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
		//Help
		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}
		//Tentacles, Paths...
		private void tentaclesPathsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1wG1Ik_50sd2KeUaX19H8e1jjgl-avf-d8SqZk7rxrkQ");

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
			InitializeLvlStuff();
			InitializeMasterStuff();
			//_formactive is the panel that was last set to Max
			_formactive = panelLeaf;
			//set panels to be resizeable
			ControlMoverOrResizer.Init(panelLeaf);
			ControlMoverOrResizer.Init(panelLevel);
			ControlMoverOrResizer.Init(panelMaster);
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
			//ping the beat counter to update column count
			numericUpDown_LeafLength_ValueChanged(null, null);
			//tell the app that everything is saved, because it just initialized
			SaveLeaf(true);
		}

		//////
		///FORM RESIZE
		//////
		///PANEL LABELS - change size or close
		private void lblMasterClose_Click(object sender, EventArgs e) => masterEditorToolStripMenuItem.PerformClick();
		private void lblLvlClose_Click(object sender, EventArgs e) => levelEditorToolStripMenuItem.PerformClick();
		private void lblLeafClose_Click(object sender, EventArgs e) => leafEditorToolStripMenuItem.PerformClick();
		private void lblLeafMax_Click(object sender, EventArgs e)
		{
			/*
			panelMaster.Width = Math.Min(100, this.Width / 5);
			panelLevel.Width = Math.Min(100, this.Width / 5);
			PanelVisibleResize();
			_formactive = panelLeaf;
			*/
		}
		private void lblLevelMax_Click(object sender, EventArgs e)
		{
			/*
			panelMaster.Width = Math.Min(100, this.Width / 4);
			panelLevel.Width = (int)(this.Width * 0.8) - 12;
			PanelVisibleResize();
			_formactive = panelLevel;
			*/
		}
		private void lblMasterMax_Click(object sender, EventArgs e)
		{
			/*
			panelMaster.Width = (int)(this.Width * 0.8) - 12;
			panelLevel.Width = Math.Min(100, this.Width / 4);
			PanelVisibleResize();
			_formactive = panelMaster;
			*/
		}
		private void FormLeafEditor_Resize(object sender, EventArgs e)
		{
			/*
			panelMaster.Width = 100;
			panelLevel.Width = 100;
			panelLeaf.Width = 100;
			if (_formactive != panelLeaf && _formactive != null)
				_formactive.Width = (int)(this.Width * 0.8) - 12;
			PanelVisibleResize();
			*/
		}
		///Resizes and Repositions panels based on which ones are visible
		public void PanelVisibleResize()
		{
			//if Master is invisible, first move Leaf to Level, then move Level to Master
			if (!panelMaster.Visible) {
				panelLeaf.Location = panelLevel.Location;
				panelLevel.Location = panelMaster.Location;
			}
			//if Level is invisible, move Leaf to Level
			if (!panelLevel.Visible)
				panelLeaf.Location = panelLevel.Location;
			//if Master is visible, move Level over
			if (panelMaster.Visible)
				panelLevel.Location = new Point(panelMaster.Location.X + panelMaster.Size.Width + 6, panelMaster.Location.Y);
			//if Level is visible, move Leaf over. Else move Leaf to Level
			if (panelLevel.Visible)
				panelLeaf.Location = new Point(panelLevel.Location.X + panelLevel.Size.Width + 6, panelMaster.Location.Y);
			else
				panelLeaf.Location = panelLevel.Location;

			panelLeaf.Width = this.Width - panelLeaf.Location.X - 20;
		}


		/// 
		/// VARIOUS POPUPS FOR HELP TEXT
		/// 
		private void lblMasterlvllistHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterLvlList"], "Master Editor Help");
		private void lblMasterRestHelp_Click(object sender, EventArgs e) => MessageBox.Show((string) helptext["masterRest"], "Master Editor Help");
		private void lblMasterCheckpointLeaderHelp_Click(object sender, EventArgs e) => MessageBox.Show((string)helptext["masterCheckLeader"], "Master Editor Help");
		private void lblConfigColorHelp_Click(object sender, EventArgs e) => new ImageMessageBox("railcolorhelp").Show();
	}
}
