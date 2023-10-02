using ControlManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor : Form
    {
        #region Variables
        public readonly CommonOpenFileDialog cfd_lvl = new CommonOpenFileDialog() { IsFolderPicker = true, Multiselect = false };
        public string workingfolder
        {
            get { return _workingfolder; }
            set {
                //check if `set` value is different than current stored value
                if (_workingfolder != value) {
                    //also only change workingfolders if user says yes to data loss
                    if (!_saveleaf || !_savelvl || !_savemaster || !_savegate || !_savesample) {
                        if (MessageBox.Show("Some files are unsaved. Are you sure you want to change working folders?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                            return;
                        }
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
                    SaveLeaf(true);
                    SaveLvl(true);
                    SaveGate(true);
                    SaveMaster(true);
                    SaveSample(true);

                    //set Working Folder panel data
                    lblWorkingFolder.Text = $"Working Folder - {new DirectoryInfo(value).Name}";
                    toolTip1.SetToolTip(lblWorkingFolder, $"Working Folder - {value}");
                    btnWorkRefresh.Enabled = true;
                    btnWorkCopy.Enabled = true;
                    editLevelDetailsToolStripMenuItem.Enabled = true;
                    regenerateDefaultFilesToolStripMenuItem.Enabled = true;
                    btnWorkRefresh.PerformClick();
                    //set window name to the level name
                    this.Text = "Thumper Custom Level Editor - " + new DirectoryInfo(workingfolder).Name;
                }
            }
        }
        private string _workingfolder;
        public List<string> lvlsinworkfolder = new List<string>();
        public string _dgfocus;
        public Point _menuloc;
        #endregion


        public FormLeafEditor()
        {
            InitializeComponent();
            ColorFormElements();
            menuStrip.Renderer = new MyRenderer();
            contextMenuDock.Renderer = new MyRenderer();
            workingfolderRightClick.Renderer = new MyRenderer();
            masterToolStrip.Renderer = new ToolStripOverride();
            toolstripTitleMaster.Renderer = new ToolStripOverride();
        }
        ///
        ///THIS BLOCK DOUBLEBUFFERS ALL CONTROLS ON THE FORM, SO RESIZING IS SMOOTH
        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        ///END DOUBLEBUFFERING
        /// 

        ///Color elements based on set properties
        private void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.custom_bgcolor;
            menuStrip.BackColor = Properties.Settings.Default.custom_menucolor;
            panelLeaf.BackColor = Properties.Settings.Default.custom_leafcolor;
            panelLevel.BackColor = Properties.Settings.Default.custom_lvlcolor;
            panelGate.BackColor = Properties.Settings.Default.custom_gatecolor;
            panelMaster.BackColor = Properties.Settings.Default.custom_mastercolor;
            panelSample.BackColor = Properties.Settings.Default.custom_samplecolor;
        }

        ///Repaints toolstrip separators to have gray backgrounds
        private void toolStripSeparator_Paint(object sender, PaintEventArgs e)
        {
            ToolStripSeparator sep = (ToolStripSeparator)sender;
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, 40, 40)), 0, 0, sep.Width, sep.Height);
            e.Graphics.DrawLine(new Pen(Color.White), 30, sep.Height / 2, sep.Width - 4, sep.Height / 2);
        }

        ///Toolstrip - FILE
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Call all save methods for files with the save flag False
            if (!_savemaster) mastersaveToolStripMenuItem_Click(null, null);
            if (!_savegate) gatesaveToolStripMenuItem_Click(null, null);
            if (!_savelvl) saveToolStripMenuItem2_Click(null, null);
            if (!_saveleaf) saveToolStripMenuItem_Click(null, null);
            if (!_savesample) SamplesaveToolStripMenuItem_Click(null, null);
        }
        ///Toolstrip - INTERPOLATOR
        private void interpolatorToolStripMenuItem_Click(object sender, EventArgs e) => new Interpolator().Show();

        ///Toolstrip - VIEW MENU
        //Visible - LEaf Editor
        private void leafEditorToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelLeaf); panelLeaf.Visible = leafEditorToolStripMenuItem.Checked; panelLeaf.BringToFront(); }
        //Visible - Level Editor
        private void levelEditorToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelLevel) ; panelLevel.Visible = levelEditorToolStripMenuItem.Checked; panelLevel.BringToFront(); }
        //Visble - Gate Editor
        private void gateEditorToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelGate); panelGate.Visible = gateEditorToolStripMenuItem.Checked; panelGate.BringToFront(); }
        //Visible - Master Editor
        private void masterEditorToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelMaster); panelMaster.Visible = masterEditorToolStripMenuItem.Checked; panelMaster.BringToFront(); }
        //Visbile - Working Folder
        private void workingFolderToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelWorkingFolder); panelWorkingFolder.Visible = workingFolderToolStripMenuItem.Checked; panelWorkingFolder.BringToFront(); }
        //Visble - Sample Editor
        private void sampleEditorToolStripMenuItem_Click(object sender, EventArgs e) { UndockPanel(panelSample); panelSample.Visible = sampleEditorToolStripMenuItem.Checked; panelSample.BringToFront(); }

        ///Toolstrip - HELP
        //About...
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
        //Help
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        //Tentacles, Paths...
        private void tentaclesPathsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1dGkU9uqlr3Hp2oJiVFMHHpIKt8S_c0Vi27n47ZRD0_0");
        //Change Game Directory
        private void changeGameDirectoryToolStripMenuItem_Click(object sender, EventArgs e) => Read_Config();
        //How to create an FSB
        private void lblSampleFSBhelp_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q");

        ///Toolstrip - BRING TO FRONT items
        private void bTFLeafToolStripMenuItem_Click(object sender, EventArgs e) { panelLeaf.BringToFront(); panelLeaf.Visible = true; }
        private void bTFLvlToolStripMenuItem_Click(object sender, EventArgs e) { panelLevel.BringToFront(); panelLevel.Visible = true; }
        private void bTFGateToolStripMenuItem_Click(object sender, EventArgs e) { panelGate.BringToFront(); panelGate.Visible = true; }
        private void bTFMasterToolStripMenuItem_Click(object sender, EventArgs e) { panelMaster.BringToFront(); panelMaster.Visible = true; }
        private void bTFFolderToolStripMenuItem_Click(object sender, EventArgs e) { panelWorkingFolder.BringToFront(); panelWorkingFolder.Visible = true; }
        private void bTFSampleToolStripMenuItem_Click(object sender, EventArgs e) { panelSample.BringToFront(); panelSample.Visible = true; }

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
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();
        ///FORM CLOSING - check if anything is unsaved
        private void FormLeafEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_saveleaf || !_savelvl || !_savemaster || !_savegate || !_savesample) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
            if (Directory.Exists(@"temp")) {
                //Directory.Delete(@"temp", true);
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
            Properties.Settings.Default.leafeditorvisible = panelLeaf.Visible;
            Properties.Settings.Default.lvleditorvisible = panelLevel.Visible;
            Properties.Settings.Default.gateeditorvisible = panelGate.Visible;
            Properties.Settings.Default.mastereditorvisible = panelMaster.Visible;
            Properties.Settings.Default.sampleeditorvisible = panelSample.Visible;
            Properties.Settings.Default.workingfoldervisible = panelWorkingFolder.Visible;
            Properties.Settings.Default.beeblesize = panelBeeble.Size;
            Properties.Settings.Default.beebleloc = panelBeeble.Location;
            //splitter distances
            Properties.Settings.Default.splitterHorz1 = splitHorizontal.SplitterDistance;
            Properties.Settings.Default.splitterVert1 = splitTop1.SplitterDistance;
            Properties.Settings.Default.splitterVert2 = splitTop2.SplitterDistance;
            Properties.Settings.Default.splitterVert3 = splitBottom1.SplitterDistance;
            Properties.Settings.Default.splitterVert4 = splitBottom2.SplitterDistance;
            //zoom settings
            Properties.Settings.Default.leafzoom = trackZoom.Value;
            Properties.Settings.Default.lvlzoom = trackLvlVolumeZoom.Value;

            Properties.Settings.Default.Save();
        }
        ///FORM
        private void FormLeafEditor_Load(object sender, EventArgs e)
        {
            //setup datagrids with proper formatting
            InitializeTracks(trackEditor, true);
            InitializeTracks(lvlSeqObjs, true);
            InitializeTracks(lvlLeafList, false);
            InitializeTracks(masterLvlList, false);
            InitializeTracks(gateLvlList, false);
            InitializeTracks(workingfolderFiles, false);
            InitializeTracks(sampleList, false);
            InitializeLvlStuff();
            InitializeMasterStuff();
            InitializeGateStuff();
            InitializeSampleStuff();
            //set panels to be resizeable
            ControlMoverOrResizer.Init(pictureBox1);
            ControlMoverOrResizer.Init(panelLeaf);
            ControlMoverOrResizer.Init(lblTrackFileName);
            ControlMoverOrResizer.Init(panelLevel);
            ControlMoverOrResizer.Init(toolstripTitleLvl);
            ControlMoverOrResizer.Init(panelGate);
            ControlMoverOrResizer.Init(lblGateName);
            ControlMoverOrResizer.Init(panelMaster);
            ControlMoverOrResizer.Init(toolstripTitleMaster);
            ControlMoverOrResizer.Init(panelSample);
            ControlMoverOrResizer.Init(lblSampleEditor);
            ControlMoverOrResizer.Init(panelWorkingFolder);
            ControlMoverOrResizer.Init(lblWorkingFolder);
            //
            AddScrollListener(trackEditor, trackEditor_Scroll);

            ///Create directory for leaf templates
            if (!Directory.Exists(@"templates")) {
                Directory.CreateDirectory(@"templates");
                File.WriteAllText(@"templates\leaf_singletrack.txt", Properties.Resources.leaf_singletrack);
                File.WriteAllText(@"templates\leaf_multitrack.txt", Properties.Resources.leaf_multitrack);
            }
            if (!Directory.Exists(@"temp")) {
                Directory.CreateDirectory(@"temp");
            }
            //write required audio files for playback
            if (!File.Exists(@"temp\coin_collect.ogg")) File.WriteAllBytes(@"temp\coin_collect.ogg", Properties.Resources.coin_collect);
            if (!File.Exists(@"temp\ducker_ring_approach.ogg")) File.WriteAllBytes(@"temp\ducker_ring_approach.ogg", Properties.Resources.ducker_ring_approach);
            if (!File.Exists(@"temp\grindable_birth2.ogg")) File.WriteAllBytes(@"temp\grindable_birth2.ogg", Properties.Resources.grindable_birth2);
            if (!File.Exists(@"temp\hammer_two_handed_hit.ogg")) File.WriteAllBytes(@"temp\hammer_two_handed_hit.ogg", Properties.Resources.hammer_two_handed_hit);
            if (!File.Exists(@"temp\high_jump.ogg")) File.WriteAllBytes(@"temp\high_jump.ogg", Properties.Resources.high_jump);
            if (!File.Exists(@"temp\thump_birth1.ogg")) File.WriteAllBytes(@"temp\thump_birth1.ogg", Properties.Resources.thump_birth1);
            if (!File.Exists(@"temp\thump1b.ogg")) File.WriteAllBytes(@"temp\thump1b.ogg", Properties.Resources.thump1b);
            if (!File.Exists(@"temp\turn_birth.ogg")) File.WriteAllBytes(@"temp\turn_birth.ogg", Properties.Resources.turn_birth);
            if (!File.Exists(@"temp\turn_birth_lft.ogg")) File.WriteAllBytes(@"temp\turn_birth_lft.ogg", Properties.Resources.turn_birth_lft);
            if (!File.Exists(@"temp\turn_hit_perfect2.ogg")) File.WriteAllBytes(@"temp\turn_hit_perfect2.ogg", Properties.Resources.turn_hit_perfect2);
            if (!File.Exists(@"temp\turn_long_lft.ogg")) File.WriteAllBytes(@"temp\turn_long_lft.ogg", Properties.Resources.turn_long_lft);
            if (!File.Exists(@"temp\jumper_approach.ogg")) File.WriteAllBytes(@"temp\jumper_approach.ogg", Properties.Resources.jumper_approach);
            InitializeSounds();
            //call method that imports objects from track_objects.txt (for Leaf editing)
            ImportObjects();

            //set timesig datasource
            dropTimeSig.DataSource = _timesig;
            //
            SaveLeaf(true);
            //set panels to their last saved dock
            SetDockLocations();
            //load size and location data for panels
            panelLeaf.Size = Properties.Settings.Default.leafeditorsize;
            panelLeaf.Location = Properties.Settings.Default.leafeditorloc;
            panelLeaf.Visible = leafEditorToolStripMenuItem.Checked = Properties.Settings.Default.leafeditorvisible;
            panelLevel.Location = Properties.Settings.Default.lvleditorloc;
            panelLevel.Size = Properties.Settings.Default.lvleditorsize;
            panelLevel.Visible = levelEditorToolStripMenuItem.Checked = Properties.Settings.Default.lvleditorvisible;
            panelGate.Location = Properties.Settings.Default.gateeditorloc;
            panelGate.Size = Properties.Settings.Default.gateeditorsize;
            panelGate.Visible = gateEditorToolStripMenuItem.Checked = Properties.Settings.Default.gateeditorvisible;
            panelMaster.Location = Properties.Settings.Default.mastereditorloc;
            panelMaster.Size = Properties.Settings.Default.mastereditorsize;
            panelMaster.Visible = masterEditorToolStripMenuItem.Checked = Properties.Settings.Default.mastereditorvisible;
            panelWorkingFolder.Location = Properties.Settings.Default.folderloc;
            panelWorkingFolder.Size = Properties.Settings.Default.foldersize;
            panelWorkingFolder.Visible = workingFolderToolStripMenuItem.Checked = Properties.Settings.Default.workingfoldervisible;
            panelSample.Size = Properties.Settings.Default.sampleeditorsize;
            panelSample.Location = Properties.Settings.Default.sampleeditorloc;
            panelSample.Visible = sampleEditorToolStripMenuItem.Checked = Properties.Settings.Default.sampleeditorvisible;
            panelBeeble.Size = Properties.Settings.Default.beeblesize;
            panelBeeble.Location = Properties.Settings.Default.beebleloc;
            //zoom settings
            trackZoom.Value = Properties.Settings.Default.leafzoom;
            trackLvlVolumeZoom.Value = Properties.Settings.Default.lvlzoom;
        }

        public void ImportObjects()
        {
            _objects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists(@"templates\track_objects.txt")) {
                File.WriteAllText(@"templates\track_objects.txt", Properties.Resources.track_objects);
            }

            ///import selectable objects from file and parse them into lists for manipulation
            //splits input at "###". Each section is a collection of param_paths
            var import = (File.ReadAllText(@"templates\track_objects.txt")).Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
            for (int x = 0; x < import.Count; x++) {
                //split each section into individual lines
                var import2 = import[x].Split('\n').ToList();
                //initialise class so we can add to it
                /*
                Object_Params objpar = new Object_Params() {
                    //name of object is the first line of every set
                    obj_displayname = import2[0],
                    obj_name = new List<string>(),
                    param_displayname = new List<string>(),
                    param_path = new List<string>(),
                    trait_type = new List<string>(),
                    step = new List<string>(),
                    def = new List<string>(),
                    footer = new List<string>()
                };*/

                for (int y = 2; y < import2.Count - 1; y++) {
                    //split each line by ';'. Now each property is separated
                    var import3 = import2[y].Split(';');
                    try {
                        Object_Params objpar = new Object_Params() {
                            category = import2[0],
                            obj_name = import3[0],
                            param_displayname = import3[1],
                            param_path = import3[2],
                            trait_type = import3[3],
                            step = import3[4],
                            def = import3[5],
                            footer = import3[6].Replace("[", "").Replace("]", ""),
                        };
                        //finally, add complete object and values to list
                        _objects.Add(objpar);
                    }
                    catch {
                        _errorlog += "failed to import all properties of param_path " + import3[0] + " of object " + import2[0] + ".\n";
                    }
                }
            }
            //show errors to user if any imports failed
            if (_errorlog.Length > 1) {
                MessageBox.Show(_errorlog);
            }

            _errorlog = "";
            //customize combobox to display the correct content
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
            dropParamPath.Enabled = false;
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
            if (input.chkLevel1.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level1_320bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level1_320bpm.txt", Properties.Resources.samp_level1_320bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level1_320bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level1_320bpm.txt");
            }
            //level 2
            if (input.chkLevel2.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level2_340bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level2_340bpm.txt", Properties.Resources.samp_level2_340bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level2_340bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level2_340bpm.txt");
            }
            //level 3
            if (input.chkLevel3.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level3_360bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level3_360bpm.txt", Properties.Resources.samp_level3_360bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level3_360bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level3_360bpm.txt");
            }
            //level 4
            if (input.chkLevel4.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level4_380bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level4_380bpm.txt", Properties.Resources.samp_level4_380bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level4_380bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level4_380bpm.txt");
            }
            //level 5
            if (input.chkLevel5.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level5_400bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level5_400bpm.txt", Properties.Resources.samp_level5_400bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level5_400bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level5_400bpm.txt");
            }
            //level 6
            if (input.chkLevel6.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level6_420bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level6_420bpm.txt", Properties.Resources.samp_level6_420bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level6_420bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level6_420bpm.txt");
            }
            //level 7
            if (input.chkLevel7.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level7_440bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level7_440bpm.txt", Properties.Resources.samp_level7_440bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level7_440bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level7_440bpm.txt");
            }
            //level 8
            if (input.chkLevel8.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level8_460bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level8_460bpm.txt", Properties.Resources.samp_level8_460bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level8_460bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level8_460bpm.txt");
            }
            //level 9
            if (input.chkLevel9.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level9_480bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level9_480bpm.txt", Properties.Resources.samp_level9_480bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level9_480bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level9_480bpm.txt");
            }
            //Dissonance
            if (input.chkDissonance.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_dissonant.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_dissonant.txt", Properties.Resources.samp_dissonant);
            }
            else if (File.Exists($@"{workingfolder}\samp_dissonant.txt")) {
                File.Delete($@"{workingfolder}\samp_dissonant.txt");
            }
            //Global Drones
            if (input.chkGlobal.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_globaldrones.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_globaldrones.txt", Properties.Resources.samp_globaldrones);
            }
            else if (File.Exists($@"{workingfolder}\samp_globaldrones.txt")) {
                File.Delete($@"{workingfolder}\samp_globaldrones.txt");
            }
            //Rests
            if (input.chkRests.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_rests.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_rests.txt", Properties.Resources.samp_rests);
            }
            else if (File.Exists($@"{workingfolder}\samp_rests.txt")) {
                File.Delete($@"{workingfolder}\samp_rests.txt");
            }
            //Misc
            if (input.chkMisc.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_misc.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_misc.txt", Properties.Resources.samp_misc);
            }
            else if (File.Exists($@"{workingfolder}\samp_misc.txt")) {
                File.Delete($@"{workingfolder}\samp_misc.txt");
            }
        }


        ///FORM RESIZE
        ///PANEL LABELS - change size or close
        private void lblMasterClose_Click(object sender, EventArgs e) => masterEditorToolStripMenuItem.PerformClick();
        private void lblLvlClose_Click(object sender, EventArgs e) => levelEditorToolStripMenuItem.PerformClick();
        private void lblLeafClose_Click(object sender, EventArgs e) => leafEditorToolStripMenuItem.PerformClick();
        private void lblGateClose_Click(object sender, EventArgs e) => gateEditorToolStripMenuItem.PerformClick();
        private void lblWorkClose_Click(object sender, EventArgs e) => workingFolderToolStripMenuItem.PerformClick();
        private void lblSampleClose_Click(object sender, EventArgs e) => sampleEditorToolStripMenuItem.PerformClick();

        /// 
        /// VARIOUS POPUPS FOR HELP TEXT
        /// 
        private void lblConfigColorHelp_Click(object sender, EventArgs e) => new ImageMessageBox("railcolorhelp").Show();
        private void lblGateSectionHelp_Click(object sender, EventArgs e) => new ImageMessageBox("bosssectionhelp").Show();

        private void editLevelDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dynamic _load = new JObject();
            try {
                //atempt to parse JSON of LEVEL DETAILS. This wil lalso take care of the situation if it doesn't exist
                _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText($@"{workingfolder}\LEVEL DETAILS.txt"), "#.*", ""));
            }
            catch { }
            DialogInput customlevel = new DialogInput();
            //set the form text fields to whatever is in LEVEL DETAILS
            customlevel.txtCustomPath.Text = workingfolder;
            //if the LEVEL DETAILS file is missing, or missing parameters, this fill fill the blanks will empty space
            customlevel.txtCustomName.Text = _load.ContainsKey("level_name") ? (string)_load["level_name"] : "";
            customlevel.txtCustomDiff.Text = _load.ContainsKey("difficulty") ? (string)_load["difficulty"] : "";
            customlevel.txtDesc.Text = _load.ContainsKey("description") ? (string)_load["description"] : "";
            customlevel.txtCustomAuthor.Text = _load.ContainsKey("author") ? (string)_load["author"] : "";
            //set samp pack checkboxes
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
            //Show the CustomWorkspace form. If form OK, then save the settings to app properties
            //then call method to recolor the form elements immediately
            CustomizeWorkspace custom = new CustomizeWorkspace();
            if (custom.ShowDialog() == DialogResult.OK) {
                Properties.Settings.Default.custom_bgcolor = custom.btnBGColor.BackColor;
                Properties.Settings.Default.custom_menucolor = custom.btnMenuColor.BackColor;
                Properties.Settings.Default.custom_mastercolor = custom.btnMasterColor.BackColor;
                Properties.Settings.Default.custom_gatecolor = custom.btnGateColor.BackColor;
                Properties.Settings.Default.custom_lvlcolor = custom.btnLvlColor.BackColor;
                Properties.Settings.Default.custom_leafcolor = custom.btnLeafColor.BackColor;
                Properties.Settings.Default.custom_samplecolor = custom.btnSampleColor.BackColor;
                Properties.Settings.Default.custom_activecolor = custom.btnActiveColor.BackColor;
                ColorFormElements();
            }
            custom.Dispose();
        }

        /// Used to allow only numbers and a single decimal during input
        private void NumericInputSanitize(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-') {
                e.Handled = true;
            }

            //only allow `-` at beginning
            if (e.KeyChar == '-' && (sender as TextBox).SelectionStart != 0)
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) {
                e.Handled = true;
            }
        }

        private void Read_Config()
        {
            cfd_lvl.Title = "Select the folder where Thumper is installed (NOT the cache folder)";
            //check if the game_dir has been set before. It'll be empty if starting for the first time
            if (Properties.Settings.Default.game_dir == "none")
                cfd_lvl.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\Thumper";
            else
                //if it's not empty, initialize the FolderBrowser to be whatever was selected last
                cfd_lvl.InitialDirectory = Properties.Settings.Default.game_dir;
            //show FolderBrowser, and then set "game_dir" to whatever is chosen
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok)
                Properties.Settings.Default.game_dir = cfd_lvl.FileName;

            Properties.Settings.Default.Save();
        }

        /// 
        /// Collection of event handlers for moving the menustrip when the form scrolls
        private void FormLeafEditor_Scroll(object sender, ScrollEventArgs e) => menuStrip.Location = new Point(_menuloc.X, _menuloc.Y);
        private void FormLeafEditor_Scroll(object sender, MouseEventArgs e) => menuStrip.Location = _menuloc;
        private void menuStrip_MouseDown(object sender, MouseEventArgs e) => _menuloc = menuStrip.Location;
        private void menuStrip_MouseUp(object sender, MouseEventArgs e) => _menuloc = menuStrip.Location;
        private void resetMenuPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndockPanel(panelLeaf);
            UndockPanel(panelLevel);
            UndockPanel(panelGate);
            UndockPanel(panelMaster);
            UndockPanel(panelWorkingFolder);
            UndockPanel(panelSample);

            panelMaster.Location = new Point(0, 30);
            panelLevel.Location = new Point(this.Width / 3, 30);
            panelGate.Location = new Point((int)(this.Width * 0.66), 30);
            panelWorkingFolder.Location = new Point(0, this.Height / 2);
            panelLeaf.Location = new Point(this.Width / 3, this.Height / 2);
            panelSample.Location = new Point((int)(this.Width * 0.66), this.Height / 2);

            panelLeaf.Visible = panelLevel.Visible = panelGate.Visible = panelMaster.Visible = panelWorkingFolder.Visible = panelSample.Visible = true;

            panelBeeble.Location = new Point(25, 25);
            panelBeeble.Size = new Size(40, 40);
            panelBeeble.BringToFront();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int i = new Random().Next(0, 101);

            if (i >= 0 && i < 10)
                pictureBox1.Image = Properties.Resources.beeblehappy;
            else if (i >= 10 && i < 20)
                pictureBox1.Image = Properties.Resources.beebleconfuse;
            else if (i >= 20 && i < 30)
                pictureBox1.Image = Properties.Resources.beeblecool;
            else if (i >= 30 && i < 40)
                pictureBox1.Image = Properties.Resources.beeblederp;
            else if (i >= 40 && i < 50)
                pictureBox1.Image = Properties.Resources.beeblelaugh;
            else if (i >= 50 && i < 60)
                pictureBox1.Image = Properties.Resources.beeblestare;
            else if (i >= 60 && i < 70)
                pictureBox1.Image = Properties.Resources.beeblethink;
            else if (i >= 70 && i < 80)
                pictureBox1.Image = Properties.Resources.beebletiny;
            else if (i >= 80 && i < 90) 
                pictureBox1.Image = Properties.Resources.beeblelove;
            else if (i >= 90 && i < 100)
                pictureBox1.Image = Properties.Resources.beeblespin;
            else if (i == 100)
                pictureBox1.Image = Properties.Resources.beeblegold;

            timerBeeble.Start();
        }

        private void timerBeeble_Tick(object sender, EventArgs e)
        {
            timerBeeble.Stop();
            pictureBox1.Image = Properties.Resources.beeble;
        }

        private void splitPanel_Paint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(130, 70, 70, 70));
            g.FillRectangle(brush, 5, 5, c.Width - 10, c.Height - 10);
        }

        private void splitHorizontal_Paint(object sender, PaintEventArgs e)
        {/*
            SplitContainer s = sender as SplitContainer;
            e.Graphics.FillRectangle(Brushes.Gray, s.SplitterRectangle);
            if (s.SplitterRectangle.Width > s.SplitterRectangle.Height) {
                e.Graphics.FillRectangle(Brushes.Black, (float)(s.SplitterRectangle.Width * 0.5) - 20, s.SplitterRectangle.Location.Y, 40, s.SplitterRectangle.Height);
                e.Graphics.FillRectangle(Brushes.Black, (float)(s.SplitterRectangle.Width * 0.20) - 20, s.SplitterRectangle.Location.Y, 40, s.SplitterRectangle.Height);
                e.Graphics.FillRectangle(Brushes.Black, (float)(s.SplitterRectangle.Width * 0.80) - 20, s.SplitterRectangle.Location.Y, 40, s.SplitterRectangle.Height);
            }
            if (s.SplitterRectangle.Width < s.SplitterRectangle.Height)
                e.Graphics.FillRectangle(Brushes.Black, s.SplitterRectangle.Location.X, (s.SplitterRectangle.Height / 2) - 20, s.SplitterRectangle.Width, 40);*/
        }

        private void ShowPanel(bool visible, Control panel)
        {
            panel.Visible = visible;
            panel.BringToFront();
        }

        private void editorpanelFocus(object sender, EventArgs e)
        {
            Panel control = (Panel)sender;
            control.Paint += editorpanel_PaintBorder;
            control.BorderStyle = BorderStyle.FixedSingle;
            control.Refresh();
        }

        private void editorpanelUnfocus(object sender, EventArgs e)
        {
            Panel control = (Panel)sender;
            control.Paint -= editorpanel_PaintBorder;
            if (control.Dock == DockStyle.Fill)
                control.BorderStyle = BorderStyle.None;
            else
                control.BorderStyle = BorderStyle.FixedSingle;
            control.Refresh();
        }

        private void editorpanel_PaintBorder(object sender, PaintEventArgs e)
        {
            Panel control = (Panel)sender;
            Color col = Properties.Settings.Default.custom_activecolor;
            ButtonBorderStyle bbs = ButtonBorderStyle.Solid;
            int thickness = 2;
            ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, col, thickness, bbs, col, thickness, bbs, col, thickness, bbs, col, thickness, bbs);
        }

        private void datagrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null) {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0) {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }
    }
}
