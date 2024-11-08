using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Shell;
using Cyotek.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        #region Variables
        public ColorPickerDialog colorDialogNew = new ColorPickerDialog() { BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.Black };
        Properties.Settings settings = Properties.Settings.Default;
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
        public dynamic projectjson; 
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
                    //check if LEVEL DETAILS exists. If not, this is not a level folder
                    if (!File.Exists($@"{value}\LEVEL DETAILS.txt")) {
                        MessageBox.Show("This folder does not appear to be a Custom Level folder. The LEVEL DETAILS file is missing.\nFile not loaded.", "File not loaded");
                        return;
                    }
                    //Try locking LEVEL DETAILS first. If it fails, the level is already open
                    //in that case, return before doing anything
                    try {
                        lockedfiles.Add($@"{value}\LEVEL DETAILS.txt", new FileStream($@"{value}\LEVEL DETAILS.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                        ClearFileLock();
                    }
                    catch (Exception) {
                        MessageBox.Show($"That level is open already in another instance of the Level Editor.", "Level cannot be opened");
                        return;
                    }
                    //load Level Details into an object so it can be accessed later
                    projectjson = LoadFileLock($@"{value}\LEVEL DETAILS.txt");
                    if (projectjson == null || !projectjson.ContainsKey("level_name") || !projectjson.ContainsKey("difficulty") || !projectjson.ContainsKey("description") || !projectjson.ContainsKey("author"))
                    {
                        DialogResult result = MessageBox.Show("The LEVEL DETAILS.txt is missing information or is corrupt.\nCreate new LEVEL DETAILS?", "Failed to load", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            JObject level_details = new() { { "level_name", $"{Path.GetFileName(value)}" }, { "difficulty", "D0" }, { "description", "replace this text" }, { "author", "some guy" } };
                            File.WriteAllText($@"{value}\LEVEL DETAILS.txt", JsonConvert.SerializeObject(level_details, Formatting.Indented));
                            projectjson = LoadFileLock($@"{value}\LEVEL DETAILS.txt");
                        }
                        else if (result == DialogResult.No) {
                            MessageBox.Show("Level Folder not loaded");
                            return;
                        }
                    }
                    ClearFileLock();
                    //update working folder
                    lockedfiles.Add($@"{value}\LEVEL DETAILS.txt", new FileStream($@"{value}\LEVEL DETAILS.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                    _workingfolder = value;
                    toolstripLevelName.Text = projectjson["level_name"];
                    toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject($"{projectjson["difficulty"]}");
                    //since a new level folder is loading, all panels need to clear their data so we;re not showing data from other levels
                    ClearPanels();
                    //populate lvlsinworkfolder with all .lvl files in the project
                    //this is needed for some specific dropdowns.
                    UpdateLevelLists();
                    /*
                    //set Working Folder panel data
                    lblWorkingFolder.Text = $"Working Folder ⮞ {projectjson["level_name"]}";
                    lblWorkingFolder.ToolTipText = $"Working Folder ⮞ {workingfolder}";
                    //enable buttons
                    btnWorkRefresh.Enabled = true;
                    btnWorkCopy.Enabled = true;
                    ///editLevelDetailsToolStripMenuItem.Enabled = true;
                    ///regenerateDefaultFilesToolStripMenuItem.Enabled = true;
                    btnExplorer.Enabled = true;
                    //add to recent files
                    if (Properties.Settings.Default.Recentfiles.Contains(workingfolder))
                        Properties.Settings.Default.Recentfiles.Remove(workingfolder);
                    Properties.Settings.Default.Recentfiles.Insert(0, workingfolder);
                    JumpListUpdate();
                    panelRecentFiles.Visible = false;

                    //once all that is done, refresh the Working Folder list. This automatically opens the Master
                    btnWorkRefresh.PerformClick();
                    */
                }
            }
        }
        private string _workingfolder;
        public List<string> lvlsinworkfolder = new();
        public Random rng = new();
        public static string AppLocation = Path.GetDirectoryName(Application.ExecutablePath);
        public string LevelToLoad;
        public static Dictionary<string, Keys> defaultkeybinds = Properties.Resources.defaultkeybinds.Split('\n').ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
        public FileStream filelocklevel;
        public Dictionary<string, FileStream> lockedfiles = new();
        public Dictionary<string, Form> openfiles = new();
        #endregion

        public TCLE(string LevelFromArg)
        {
            InitializeComponent();
            dockMain.Theme = new VS2015DarkTheme();
            pictureBeeble.BringToFront();

            ColorFormElements();
            JumpListUpdate();

            //set custom renderer
            menuStrip.Renderer = new ToolStripOverride();
            contextmenuFile.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            contextMenuAddFile.Renderer = new ToolStripProfessionalRenderer(new ProjectExplorerRightClick());
            //
            if (Properties.Settings.Default.Recentfiles == null)
                Properties.Settings.Default.Recentfiles = new List<string>();
            //event handler needed for dgv
            trackEditor.MouseWheel += new MouseEventHandler(trackEditor_MouseWheel);
            DropDownMenuScrollWheelHandler.Enable(true);
            //
            LevelToLoad = LevelFromArg;
            //
            //
            //
            ///Create directory for leaf templates and other default files
            if (!Directory.Exists($@"{AppLocation}\templates")) {
                regenerateTemplateFilesToolStripMenuItem_Click(null, null);
            }
            if (!Directory.Exists($@"{AppLocation}\temp")) {
                Directory.CreateDirectory($@"{AppLocation}\temp");
            }
            //
            dropMasterSkybox.SelectedIndex = 1;
            dropMasterSkybox.SelectedIndexChanged += dropMasterIntro_SelectedIndexChanged;

            //setup datagrids with proper formatting
            InitializeTracks(trackEditor, true);
            InitializeTracks(lvlSeqObjs, true);
            InitializeTracks(lvlLeafList, false);
            InitializeTracks(masterLvlList, false);
            InitializeTracks(gateLvlList, false);
            InitializeTracks(workingfolderFiles, false);
            InitializeTracks(sampleList, false);
            //call method that imports objects from track_objects.txt (for Leaf editing)
            ImportObjects();
            InitializeLeafStuff();
            InitializeLvlStuff();
            InitializeMasterStuff();
            InitializeGateStuff();
            InitializeSampleStuff();
            //write required audio files for playback
            InitializeSounds();
            //keybinds
            SetKeyBinds();
            //pictureBeeble.Size = Properties.Settings.Default.beeblesize;
            //pictureBeeble.Location = Properties.Settings.Default.beebleloc;
            //zoom settings
            trackZoom.Value = Properties.Settings.Default.leafzoom;
            trackZoomVert.Value = Properties.Settings.Default.leafzoomvert;
            trackLvlVolumeZoom.Value = Properties.Settings.Default.lvlzoom;
            btnLeafAutoPlace.Checked = Properties.Settings.Default.leafautoinsert;
            //colors
            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //load recent levels 
            List<string> levellist = Properties.Settings.Default.Recentfiles ?? new List<string>();
            if (levellist.Count > 0 && LevelToLoad.Length < 2)
                RecentFiles(levellist);
            else if (LevelToLoad.Length > 2) {
                if (Directory.Exists(LevelToLoad)) {
                    workingfolder = LevelToLoad;
                    panelRecentFiles.Visible = false;
                }
                else
                    MessageBox.Show($"Recent Level selected no longer exists at that location\n{LevelToLoad}", "Level load error");
            }
        }
        ///FORM LOADING
        private void FormLeafEditor_Load(object sender, EventArgs e)
        {
            //finalize boot
            PlaySound("UIboot");
            ///version check
            if (Properties.Settings.Default.version != "2.2release1") {
                ShowChangelog();
                if (MessageBox.Show($"2.2 contains many new objects to use! You will need to update the track_objects.txt file to use them. Do this now?", "NEW VERSION NOTICE!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    regenerateTemplateFilesToolStripMenuItem_Click(null, null);
                else
                    MessageBox.Show("You can update later from the File menu.\nFile > Template Files > Regenerate", "ok", MessageBoxButtons.OK);
                Properties.Settings.Default.version = "2.2release1";
            }

            //finish loading
            Properties.Settings.Default.firstrun = false;
            Properties.Settings.Default.Save();
        }
        private void JumpListUpdate()
        {
            if (Properties.Settings.Default.Recentfiles == null)
                return;

            JumpList jml = new() {
                ShowRecentCategory = true,
                ShowFrequentCategory = true
            };

            foreach (string file in Properties.Settings.Default.Recentfiles) {
                JumpTask jmp = new() {
                    Title = Path.GetFileName(file),
                    Arguments = file,
                    Description = file,
                    ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location
                };
                jml.JumpItems.Add(jmp);
            }
            jml.Apply();
            Properties.Settings.Default.Save();
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
            if (Directory.Exists($@"{AppLocation}\temp")) {
                //Directory.Delete(@"temp", true);
            }
            //save panel sizes and locations
            Properties.Settings.Default.beeblesize = pictureBeeble.Size;
            Properties.Settings.Default.beebleloc = pictureBeeble.Location;
            //zoom settings
            Properties.Settings.Default.leafzoom = trackZoom.Value;
            Properties.Settings.Default.leafzoomvert = trackZoomVert.Value;
            Properties.Settings.Default.lvlzoom = trackLvlVolumeZoom.Value;
            Properties.Settings.Default.leafautoinsert = btnLeafAutoPlace.Checked;
            //colors
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            //
            Properties.Settings.Default.Save();
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

        private void regenerateTemplateFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists($@"{AppLocation}\templates")) {
                Directory.CreateDirectory($@"{AppLocation}\templates");
            }
            File.WriteAllText($@"{AppLocation}\templates\leaf_singletrack.txt", Properties.Resources.leaf_singletrack);
            File.WriteAllText($@"{AppLocation}\templates\leaf_multitrack.txt", Properties.Resources.leaf_multitrack);
            File.WriteAllText($@"{AppLocation}\templates\leaf_multitrack_ring&bar.txt", Properties.Resources.leaf_multitrack_ring_bar);
            File.WriteAllText($@"{AppLocation}\templates\track_objects2.2.txt", Properties.Resources.track_objects);
            File.WriteAllText($@"{AppLocation}\templates\objects_defaultcolors2.2.txt", Properties.Resources.objects_defaultcolors);
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

        private void ResetBeeble(object sender, EventArgs e)
        { }

        ///
        ///Toolstrip - HELP
        //About...
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
        //DOCUMENTATION
        //Tentacles, Paths...
        private void tentaclesPathsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1dGkU9uqlr3Hp2oJiVFMHHpIKt8S_c0Vi27n47ZRD0_0");
        //Track Objects
        private void trackObjectsToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1JWk7TDn4ZuitclB-x7gOYxU-PsmGkooZuU9QEd_aw1A");
        //Change Game Directory
        private void changeGameDirectoryToolStripMenuItem_Click(object sender, EventArgs e) => Read_Config();
        private void discordServerToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://discord.com/invite/gTQbquY");
        private void githubToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/CocoaMix86/Thumper-Custom-Level-Editor");
        private void changelogToolStripMenuItem_Click(object sender, EventArgs e) => ShowChangelog();
        private void donateTipToolStripMenuItem_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://ko-fi.com/I2I5ZZBRH");
        ///
        /// 
        

        /// NEW CUSTOM LEVEL FOLDER
        private void newLevelFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogInput customlevel = new(this, true);
            //show the new level folder dialog box
            if (customlevel.ShowDialog() == DialogResult.Yes) {
                customlevel.Dispose();
            }
        }

        /// 
        /// VARIOUS POPUPS FOR HELP TEXT
        /// 
        private void lblConfigColorHelp_Click(object sender, EventArgs e) => new ImageMessageBox("railcolorhelp").Show();
        private void lblGateSectionHelp_Click(object sender, EventArgs e) => new ImageMessageBox("bosssectionhelp").Show();

        private void editLevelDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogInput customlevel = new(this, false);
            //set textboxes
            customlevel.txtCustomName.Text = projectjson["level_name"] ?? "LEVEL NAME";
            customlevel.txtCustomDiff.Text = projectjson["difficulty"] ?? "d0";
            customlevel.txtDesc.Text = projectjson["description"] ?? "ADD A DESCRIPTION";
            customlevel.txtCustomAuthor.Text = projectjson["author"] ?? "SOME PERSON";
            //show the new level folder dialog box
            customlevel.ShowDialog();
        }

        private void regenerateDefaultFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will overwrite the \"default\" files in the working folder. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                File.WriteAllText($@"{workingfolder}\spn_default.txt", Properties.Resources.spn_default);
                File.WriteAllText($@"{workingfolder}\xfm_default.txt", Properties.Resources.xfm_default);
            }
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show the CustomWorkspace form. If form OK, then save the settings to app properties
            //then call method to recolor the form elements immediately
            CustomizeWorkspace custom = new(_objects, this);
            //custom._objects = _objects;
            if (custom.ShowDialog() == DialogResult.OK) {
                ColorFormElements();
                ImportDefaultColors();
                SetKeyBinds();
                Properties.Settings.Default.Save();
            }
            custom.Dispose();
        }

        ///
        ///BEEBLE FUNCTIONS
        static List<Image> beebleimages = new() { Properties.Resources.beeblehappy, Properties.Resources.beebleconfuse, Properties.Resources.beeblecool, Properties.Resources.beeblederp, Properties.Resources.beeblelaugh, Properties.Resources.beeblestare, Properties.Resources.beeblethink, Properties.Resources.beebletiny, Properties.Resources.beeblelove, Properties.Resources.beeblespin };
        public void pictureBox1_Click(object sender, EventArgs e)
        {
            BeebleClick();
        }
        public void BeebleClick()
        {
            int i = new Random().Next(0, 1001);
            if (i == 1000) {
                pictureBeeble.BackgroundImage = Properties.Resources.beeblegold;
                PlaySound("UIbeetleclickGOLD");
            }
            else {
                pictureBeeble.BackgroundImage = beebleimages[i % 10];
            }
            timerBeeble.Start();
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            PlaySound($"UIbeetleclick{rng.Next(1, 9)}");
            pictureBeeble.BackColor = Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
        }
        private void timerBeeble_Tick(object sender, EventArgs e)
        {
            timerBeeble.Stop();
            pictureBeeble.BackgroundImage = Properties.Resources.beeble;
        }
        ///
        ///

        //Repaints toolstrip separators to have gray backgrounds
        private void toolStripSeparator_Paint(object sender, PaintEventArgs e)
        {
            ToolStripSeparator sep = (ToolStripSeparator)sender;
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, 40, 40)), 0, 0, sep.Width, sep.Height);
            e.Graphics.DrawLine(new Pen(Color.White), 30, sep.Height / 2, sep.Width - 4, sep.Height / 2);
        }
        ///

        private void mastereditor_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            if (sender is ComboBox cbx) {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0) {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new() {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };

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

        private void openTemplateFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new() {
                Arguments = $@"{Path.GetDirectoryName(Application.ExecutablePath)}\templates",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void trackEditor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != 0 && e.Value != null && e.ColumnIndex != -1 && e.RowIndex != -1) {
                string cellText = e.Value.ToString();
                for (int fontSize = 1; fontSize < 25; fontSize++) {
                    Font font = new("Consolas", fontSize);
                    Size textSize = TextRenderer.MeasureText(cellText, font);
                    if (textSize.Width > e.CellBounds.Width + 2 || textSize.Height > e.CellBounds.Height || fontSize == 24) {
                        if (fontSize - 1 != 0)
                            font = new Font("Consolas", fontSize - 1);
                        e.CellStyle.Font = font;
                        e.Paint(e.ClipBounds, e.PaintParts);
                        e.Handled = true;
                        break;
                    }
                }
            }
        }

        private void dropTrackLane_DataSourceChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int maxWidth = 0;
            foreach (object obj in cb.Items) {
                int temp = TextRenderer.MeasureText(obj.ToString(), cb.Font).Width;
                if (temp > maxWidth) {
                    maxWidth = temp;
                }
            }
            cb.DropDownWidth = maxWidth != 0 ? maxWidth : cb.DropDownWidth;
        }

        private void FormLeafEditor_KeyDown(object sender, KeyEventArgs e)
        {
            //Next/Previous lvl keybind
            if (e.KeyData == defaultkeybinds["nextlvl"] || e.KeyData == defaultkeybinds["previouslvl"]) {
                if (workingfolder == null)
                    return;
                if (lvlSeqObjs.IsCurrentCellInEditMode)
                    return;
                //depending on key, go next or previous
                int offset = e.KeyData == defaultkeybinds["nextlvl"] ? 1 : -1;
                //search for the current lvl. This is used to get its index
                IEnumerable<WorkingFolderFileItem> wffilist = workingfiles.Where(x => x.filename.Contains("lvl_"));
                WorkingFolderFileItem wffi = _loadedlvl != null ? wffilist.First(x => _loadedlvl.Contains(x.filename)) : wffilist.First();
                //if its the first or last entry, need to loop around
                if (_loadedlvl == null || (offset == 1 && wffi == wffilist.Last()))
                    offset = wffilist.First().index;
                else if (offset == -1 && wffi == wffilist.First())
                    offset = wffilist.Last().index;
                else
                    offset = wffi.index + offset;
                //load and visually select the new row
                LoadFileOnClick(offset, 1);
                workingfolderFiles.Rows[offset].Selected = true;
            }
            //Next/Previous leaf keybind
            else if (e.KeyData == defaultkeybinds["nextleaf"] || e.KeyData == defaultkeybinds["previousleaf"]) {
                if (workingfolder == null)
                    return;
                if (trackEditor.IsCurrentCellInEditMode)
                    return;
                //depending on key, go next or previous
                int offset = e.KeyData == defaultkeybinds["nextleaf"] ? 1 : -1;
                //search for the current lvl. This is used to get its index
                IEnumerable<WorkingFolderFileItem> wffilist = workingfiles.Where(x => x.filename.Contains("leaf_"));
                WorkingFolderFileItem wffi = _loadedleaf != null ? wffilist.First(x => _loadedleaf.Contains(x.filename)) : wffilist.First();
                //if its the first or last entry, need to loop around
                if (offset == 1 && wffi == wffilist.Last())
                    offset = wffilist.First().index;
                else if (offset == -1 && wffi == wffilist.First())
                    offset = wffilist.Last().index;
                else
                    offset = wffi.index + offset;
                //load and visually select the new row
                LoadFileOnClick(offset, 1);
                workingfolderFiles.Rows[offset].Selected = true;
            }
            //Undo keybind
            else if (e.KeyData == defaultkeybinds["leafundo"]) {
                if (_undolistleaf.Count <= 1)
                    return;
                //UndoFunction(1);
            }
            else if (e.KeyData == defaultkeybinds["colordialog"]) {
                btnLeafColors.PerformClick();
            }
            else if (e.KeyData == defaultkeybinds["interpolate"]) {
                btnLEafInterpLinear.PerformClick();
            }
            else if (e.KeyData == defaultkeybinds["splitleaf"]) {
                btnLeafSplit.PerformClick();
            }
            else if (e.KeyData == defaultkeybinds["randomizerow"]) {
                btnLeafRandomValues.PerformClick();
            }
            else if (e.KeyData == defaultkeybinds["toggleautoplace"]) {
                btnLeafAutoPlace.PerformClick();
            }
        }

        private void SetKeyBinds()
        {
            if (File.Exists($@"{AppLocation}\templates\keybinds.txt")) {
                Dictionary<string, Keys> import = File.ReadAllLines($@"{AppLocation}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
                import = import.Concat(defaultkeybinds.Where(x => !import.Keys.Contains(x.Key))).ToDictionary(x => x.Key, x => x.Value);
                defaultkeybinds = import;
            }
            /*
            leafnewToolStripMenuItem.ShortcutKeys = defaultkeybinds["leafnew"];
            leafloadToolStripMenuItem.ShortcutKeys = defaultkeybinds["leafopen"];
            leafsaveToolStripMenuItem.ShortcutKeys = defaultkeybinds["leafsave"];
            leafsaveAsToolStripMenuItem.ShortcutKeys = defaultkeybinds["leafsaveas"];
            lvlnewToolStripMenuItem1.ShortcutKeys = defaultkeybinds["lvlnew"];
            lvlopenToolStripMenuItem.ShortcutKeys = defaultkeybinds["lvlopen"];
            lvlsaveToolStripMenuItem2.ShortcutKeys = defaultkeybinds["lvlsave"];
            lvlsaveAsToolStripMenuItem.ShortcutKeys = defaultkeybinds["lvlsaveas"];
            gatenewToolStripMenuItem.ShortcutKeys = defaultkeybinds["gatenew"];
            gateopenToolStripMenuItem.ShortcutKeys = defaultkeybinds["gateopen"];
            gatesaveToolStripMenuItem.ShortcutKeys = defaultkeybinds["gatesave"];
            gatesaveAsToolStripMenuItem.ShortcutKeys = defaultkeybinds["gatesaveas"];
            masternewToolStripMenuItem.ShortcutKeys = defaultkeybinds["masternew"];
            masteropenToolStripMenuItem.ShortcutKeys = defaultkeybinds["masteropen"];
            mastersaveToolStripMenuItem.ShortcutKeys = defaultkeybinds["mastersave"];
            mastersaveAsToolStripMenuItem.ShortcutKeys = defaultkeybinds["mastersaveas"];
            SamplenewToolStripMenuItem.ShortcutKeys = defaultkeybinds["samplenew"];
            SampleopenToolStripMenuItem.ShortcutKeys = defaultkeybinds["sampleopen"];
            SamplesaveToolStripMenuItem.ShortcutKeys = defaultkeybinds["samplesave"];
            SamplesaveAsToolStripMenuItem.ShortcutKeys = defaultkeybinds["samplesaveas"];
            SaveAllToolStripMenuItem.ShortcutKeys = defaultkeybinds["saveall"];
            newLevelFolderToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelnew"];
            openLevelFolderToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelopen"];
            recentLevelsToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelrecent"];
            openLevelInExplorerToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelexplorer"];
            leafTemplateToolStripMenuItem.ShortcutKeys = defaultkeybinds["templateopen"];
            */
            btnUndoLeaf.ToolTipText = $"Undo ({String.Join("+", defaultkeybinds["leafundo"].ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList().Reverse<string>())})";
        }

        private void dropTrackLane_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = 0;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            if (senderComboBox.Items[0].GetType() == typeof(SampleData)) {
                foreach (SampleData s in senderComboBox.Items) {
                    newWidth = (int)g.MeasureString(s.obj_name, font).Width;
                    if (width < newWidth) {
                        width = newWidth;
                    }
                }
            }
            else {
                foreach (var s in senderComboBox.Items) {
                    newWidth = (int)g.MeasureString(s.ToString(), font).Width;
                    if (width < newWidth) {
                        width = newWidth;
                    }
                }
            }
            senderComboBox.DropDownWidth = width + vertScrollBarWidth;
        }


        private void toolstripOpenPanels_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtFilePath.Text)) {
                MessageBox.Show("that folder path doesn't exist");
                return;
            }

            var dockProject = new Form_ProjectExplorer(this, txtFilePath.Text);
            dockProject.Show(dockMain, DockState.DockRight);

            var dockMaster = new Form_MasterEditor(this);
            var dockGate = new Form_GateEditor(this);
            var dockLvl = new Form_LvlEditor(this);
            var dockSample = new Form_SampleEditor(this, workingfolder);
            var dockLeaf = new Form_LeafEditor(this);
            dockMaster.Show(dockMain, DockState.Document);
            dockGate.Show(dockMain, DockState.Document);
            dockLvl.Show(dockMain, DockState.Document);
            dockSample.Show(dockMain, DockState.Document);
            dockLeaf.Show(dockMain, DockState.Document);
        }

        ///
        ///https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles()) {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive) {
                foreach (DirectoryInfo subDir in dirs) {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private void toolStripSeparator8_Click(object sender, EventArgs e)
        {

        }
    }
}
