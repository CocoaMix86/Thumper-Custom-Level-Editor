﻿using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Runtime.CompilerServices;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
    {
        #region Variables
        public static TCLE Instance;
        public static DockPanel DockMain { get { return Instance.dockMain; } }
        public static IEnumerable<IDockContent> Documents { get { return Instance.dockMain.Documents; } }
        public static ColorPickerDialog colorDialogNew = new() { BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.Black };
        Properties.Settings settings = Properties.Settings.Default;
        public static dynamic ProjectJson;
        private DirectoryInfo workingfolder
        {
            get { return WorkingFolder; }
            set {
                //check if `set` value is different than current stored value
                if (WorkingFolder != value) {
                    //also only change workingfolders if user says yes to data loss
                    if (/*!_saveleaf || !_savelvl || !_savemaster || !_savegate || !_savesample*/false) {
                        if (MessageBox.Show("Some files are unsaved. Are you sure you want to change working folders?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                            return;
                        }
                    }
                    //check if the .TCL exists. If not, this is not a level folder
                    FileInfo ProjectFile = value.GetFiles("*.TCL", SearchOption.AllDirectories).FirstOrDefault();
                    if (ProjectFile == null) {
                        MessageBox.Show("This folder does not appear to be a Custom Level project. The .TCL file is missing.\nProject not loaded.", "Thumper Custom Level Editor");
                        return;
                    }
                    //Try locking the .TCL first. If it fails, the level is already open
                    //in that case, return before doing anything
                    try {
                        lockedfiles.Add(ProjectFile, new FileStream(ProjectFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                        ClearFileLock();
                    }
                    catch (Exception) {
                        MessageBox.Show($"That project is open already in another instance of the Level Editor.", "Level cannot be opened");
                        return;
                    }
                    //load Level Details into an object so it can be accessed later
                    ProjectJson = LoadFileLock(ProjectFile.FullName);
                    if (ProjectJson == null || !ProjectJson.ContainsKey("level_name") || !ProjectJson.ContainsKey("difficulty") || !ProjectJson.ContainsKey("description") || !ProjectJson.ContainsKey("author")) {
                        DialogResult result = MessageBox.Show("The Project .TCL file is missing information or is corrupt.\nCreate new Project .TCL?", "Failed to load", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes) {
                            JObject level_details = new() { { "level_name", $"{value.Name}" }, { "difficulty", "D0" }, { "description", "replace this text" }, { "author", "some guy" } };
                            File.WriteAllText($@"{value.FullName}\{value.Name}.TCL", JsonConvert.SerializeObject(level_details, Formatting.Indented));
                            ProjectJson = LoadFileLock($@"{value.FullName}\{value.Name}.TCL");
                        }
                        else if (result == DialogResult.No) {
                            MessageBox.Show("Level Folder not loaded");
                            return;
                        }
                    }
                    ClearFileLock();
                    //update working folder
                    WorkingFolder = value;
                    toolstripLevelName.Text = ProjectJson["level_name"];
                    toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject($"{ProjectJson["difficulty"]}");
                    //add to recent files
                    if (Properties.Settings.Default.Recentfiles.Contains(WorkingFolder.FullName))
                        Properties.Settings.Default.Recentfiles.Remove(WorkingFolder.FullName);
                    Properties.Settings.Default.Recentfiles.Insert(0, WorkingFolder.FullName);
                    JumpListUpdate();
                    panelRecentFiles.Visible = false;
                }
            }
        }
        public static DirectoryInfo WorkingFolder;
        public static List<string> lvlsinworkfolder = new();
        public static Random rng = new();
        public static string AppLocation = Path.GetDirectoryName(Application.ExecutablePath);
        public string LevelToLoad;
        public static Dictionary<string, Keys> defaultkeybinds = Properties.Resources.defaultkeybinds.Split('\n').ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
        public static Dictionary<FileInfo, FileStream> lockedfiles = new();
        public static Beeble beeble = new();
        #endregion

        #region Form Construction
        public static Form_ProjectExplorer dockProjectExplorer;
        public static Form_ProjectProperties dockProjectProperties;
        public TCLE(string LevelFromArg)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            dockMain.Theme = new VS2015DarkTheme();
            beeble.Show();
            Instance = this;

            MaximizeScreenBounds();
            ColorFormElements();
            JumpListUpdate();

            //set custom renderer
            toolStripTitle.Renderer = new ToolStripMainForm();
            toolStripMain.Renderer = new ToolStripOverride();
            contextmenuFile.Renderer = new ContextMenuColors();
            contextmenuEdit.Renderer = new ContextMenuColors();
            contextMenuProject.Renderer = new ContextMenuColors();
            contextmenuWindow.Renderer = new ContextMenuColors();
            contextmenuHelp.Renderer = new ContextMenuColors();
            //
            if (Properties.Settings.Default.Recentfiles == null)
                Properties.Settings.Default.Recentfiles = new List<string>();
            //event handler needed for dgv
            //
            LevelToLoad = LevelFromArg;
            //
            //
            //
            ///Create directory for leaf templates and other default files
            if (!Directory.Exists($@"{AppLocation}\templates")) {
                toolstripFileTemplateRegen_Click(null, null);
            }
            if (!Directory.Exists($@"{AppLocation}\temp")) {
                Directory.CreateDirectory($@"{AppLocation}\temp");
            }
            //setup datagrids with proper formatting
            //call method that imports objects from track_objects.txt (for Leaf editing)
            ImportObjects();
            //write required audio files for playback
            ///InitializeSounds();
            //keybinds
            SetKeyBinds();
            //colors
            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //load recent levels 
            List<string> levellist = Properties.Settings.Default.Recentfiles ?? new List<string>();
            if (levellist.Count > 0 && LevelToLoad.Length < 2)
                RecentFiles(levellist);
            else if (LevelToLoad.Length > 2) {
                if (Directory.Exists(LevelToLoad)) {
                    workingfolder = new DirectoryInfo(LevelToLoad);
                    panelRecentFiles.Visible = false;
                }
                else
                    MessageBox.Show($"Recent Level selected no longer exists at that location\n{LevelToLoad}", "Level load error");
            }

            dockProjectExplorer = new(this) { DockAreas = DockAreas.Document | DockAreas.DockRight | DockAreas.DockLeft };
            dockProjectExplorer.Show(dockMain, DockState.DockRight);
            dockProjectProperties = new() { DockAreas = DockAreas.Document | DockAreas.DockRight | DockAreas.DockLeft };
            dockProjectProperties.Show(dockProjectExplorer.Pane, DockAlignment.Bottom, 0.5);
        }
        #endregion
        #region Form Loading Closing
        ///FORM LOADING
        private void FormLeafEditor_Load(object sender, EventArgs e)
        {
            //finalize boot
            PlaySound("UIboot");
            ///version check
            if (Properties.Settings.Default.version != "2.2release1") {
                ShowChangelog();
                if (MessageBox.Show($"2.2 contains many new objects to use! You will need to update the track_objects.txt file to use them. Do this now?", "NEW VERSION NOTICE!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    toolstripFileTemplateRegen_Click(null, null);
                else
                    MessageBox.Show("You can update later from the File menu.\nFile > Template Files > Regenerate", "ok", MessageBoxButtons.OK);
                Properties.Settings.Default.version = "2.2release1";
            }

            //finish loading
            Properties.Settings.Default.firstrun = false;
            Properties.Settings.Default.Save();
        }
        private static void JumpListUpdate()
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
            if (/*!_saveleaf || !_savelvl || !_savemaster || !_savegate || !_savesample*/false) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
            //save panel sizes and locations
            ///Properties.Settings.Default.beeblesize = pictureBeeble.Size;
            ///Properties.Settings.Default.beebleloc = pictureBeeble.Location;
            //colors
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            //
            Properties.Settings.Default.Save();
        }
        private static void SetKeyBinds()
        {
            if (File.Exists($@"{AppLocation}\templates\keybinds.txt")) {
                Dictionary<string, Keys> import = File.ReadAllLines($@"{AppLocation}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
                import = import.Concat(defaultkeybinds.Where(x => !import.Keys.Contains(x.Key))).ToDictionary(x => x.Key, x => x.Value);
                defaultkeybinds = import;
            }
            /*
            SaveAllToolStripMenuItem.ShortcutKeys = defaultkeybinds["saveall"];
            newLevelFolderToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelnew"];
            openLevelFolderToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelopen"];
            recentLevelsToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelrecent"];
            openLevelInExplorerToolStripMenuItem.ShortcutKeys = defaultkeybinds["levelexplorer"];
            leafTemplateToolStripMenuItem.ShortcutKeys = defaultkeybinds["templateopen"];
            */
            ///btnUndoLeaf.ToolTipText = $"Undo ({String.Join("+", defaultkeybinds["leafundo"].ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList().Reverse<string>())})";
        }
        #endregion
        #region Form Moving and Control buttons
        private void toolstripFormRestore_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                MaximizeScreenBounds();
            }
            else {
                this.WindowState = FormWindowState.Normal;
                toolstripFormRestore.Image = Properties.Resources.icon_maximize;
                this.Refresh();
                contextFormMax.Enabled = true;
                contextFormRestore.Enabled = false;
            }
        }
        private void MaximizeScreenBounds()
        {
            var bounds = Screen.FromHandle(this.Handle).WorkingArea;
            //Screen WorkingArea is shrunk a small bit compared to the actual display area
            //so the following 4 lines increases the bounds to cover whole screen
            bounds.X = -8;
            bounds.Y = -8;
            bounds.Width += 16;
            bounds.Height += 16;
            this.MaximizedBounds = bounds;
            this.WindowState = FormWindowState.Maximized;
            this.Refresh();
            toolstripFormRestore.Image = Properties.Resources.icon_restore;
            contextFormMax.Enabled = false;
            contextFormRestore.Enabled = true;
        }
        private void toolstripFormMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void toolstripFormClose_Click(object sender, EventArgs e) => this.Close();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void toolStripTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void TCLE_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                beeble.Visible = false;
            else
                beeble.Visible = true;
            if (this.WindowState == FormWindowState.Normal)
                toolstripFormRestore.Image = Properties.Resources.icon_maximize;
        }
        #endregion

        #region Toolstrip File
        private void toolstripFileNewProject_Click(object sender, EventArgs e)
        {
            ProjectPropertiesForm customlevel = new(true);
            //show the new level folder dialog box
            if (customlevel.ShowDialog() == DialogResult.Yes) {
                customlevel.Dispose();
            }
        }

        private void toolstripFileNewFile_Click(object sender, EventArgs e)
        {

        }

        private void toolstripFileOpenProject_Click(object sender, EventArgs e)
        {

        }

        private void toolstripFileOpenFile_Click(object sender, EventArgs e)
        {

        }

        private void toolstripFileSave_Click(object sender, EventArgs e)
        {
            IDockContent _activedoc = dockMain.ActiveDocument;
            if (_activedoc.GetType() == typeof(Form_MasterEditor)) {
                ((Form_MasterEditor)_activedoc).SaveAs();
            }
        }

        private void toolstripFileSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void toolstripFileSaveAll_Click(object sender, EventArgs e)
        {

        }

        private void toolstripFileTemplateFolder_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new() {
                Arguments = $@"{Path.GetDirectoryName(Application.ExecutablePath)}\templates",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void toolstripFileTemplateRegen_Click(object sender, EventArgs e)
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

        private void toolstripFileExit_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Toolstrip Edit
        private void toolstripEditUndo_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditCut_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditCopy_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditPaste_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditPreferences_Click(object sender, EventArgs e)
        {
            //Show the CustomWorkspace form. If form OK, then save the settings to app properties
            //then call method to recolor the form elements immediately
            CustomizeWorkspace custom = new(LeafObjects, this);
            //custom._objects = _objects;
            if (custom.ShowDialog() == DialogResult.OK) {
                ColorFormElements();
                ImportDefaultColors();
                SetKeyBinds();
                Properties.Settings.Default.Save();
            }
            custom.Dispose();
        }
        #endregion
        #region Toolstrip Window
        private void toolstripWindowFloat_Click(object sender, EventArgs e) => dockMain.ActiveDocument.DockHandler.DockState = DockState.Float;
        private void toolstripWindowFloatAll_Click(object sender, EventArgs e) => dockMain.ActivePane.DockState = DockState.Float;
        private void toolstripWindowDock_Click(object sender, EventArgs e) => dockMain.ActiveDocument.DockHandler.DockState = DockState.Document;

        private void toolstripWindowCloseAll_Click(object sender, EventArgs e)
        {
            int contents = dockMain.Contents.Count;
            for (int x = 0; x < contents; x++) {
                dockMain.Contents[0].DockHandler.DockPanel = null;
            }
        }

        private void toolstripWindowCloseEditors_Click(object sender, EventArgs e)
        {
            int docs = dockMain.DocumentsCount;
            int floats = dockMain.FloatWindows.Count;
            for (int x = 0; x < docs; x++) {
                dockMain.Documents.ToList()[0].DockHandler.DockPanel = null;
            }
            for (int x = 0; x < floats; x++) {
                dockMain.FloatWindows[0].Close();
            }
        }
        #endregion
        #region Toolstrip Help
        private void toolstripHelpTentacles_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1dGkU9uqlr3Hp2oJiVFMHHpIKt8S_c0Vi27n47ZRD0_0");
        private void toolstripHelpObjects_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/1JWk7TDn4ZuitclB-x7gOYxU-PsmGkooZuU9QEd_aw1A");
        private void toolstripHelpAudio_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q");
        private void toolstripHelpAbout_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
        private void toolstripHelpDiscord_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://discord.com/invite/gTQbquY");
        private void toolstripHelpGithub_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/CocoaMix86/Thumper-Custom-Level-Editor");
        private void toolstripHelpChangelog_Click(object sender, EventArgs e) => ShowChangelog();
        private void toolstripHelpKofi_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://ko-fi.com/I2I5ZZBRH");
        #endregion
        #region Toolstrip Project
        private void toolstripProjectLeaf_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectLvl_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectGate_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectMaster_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectSample_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectExisting_Click(object sender, EventArgs e)
        {

        }

        private void toolstripProjectRegen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will overwrite the \"default\" files in the working folder. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                File.WriteAllText($@"{workingfolder}\spn_default.txt", Properties.Resources.spn_default);
                File.WriteAllText($@"{workingfolder}\xfm_default.txt", Properties.Resources.xfm_default);
            }
        }

        private void toolstripProjectProperties_Click(object sender, EventArgs e)
        {
            ProjectPropertiesForm customlevel = new(false);
            //set textboxes
            customlevel.txtCustomName.Text = ProjectJson["level_name"] ?? "LEVEL NAME";
            customlevel.txtCustomDiff.Text = ProjectJson["difficulty"] ?? "d0";
            customlevel.txtDesc.Text = ProjectJson["description"] ?? "ADD A DESCRIPTION";
            customlevel.txtCustomAuthor.Text = ProjectJson["author"] ?? "SOME PERSON";
            //show the new level folder dialog box
            customlevel.ShowDialog();
        }
        #endregion

        #region Toolstrip Toolbar
        #endregion

        private void toolstripOpenPanels_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtFilePath.Text)) {
                MessageBox.Show("that folder path doesn't exist");
                return;
            }
            workingfolder = new DirectoryInfo(txtFilePath.Text);
            dockProjectExplorer.LoadProject(WorkingFolder.FullName);
            dockProjectProperties.LoadProjectProperties(ProjectJson);

            Form_GateEditor dockGate = new() { DockAreas = DockAreas.Document | DockAreas.Float };
            Form_LvlEditor dockLvl = new() { DockAreas = DockAreas.Document | DockAreas.Float };
            Form_SampleEditor dockSample = new() { DockAreas = DockAreas.Document | DockAreas.Float };
            Form_LeafEditor dockLeaf = new() { DockAreas = DockAreas.Document | DockAreas.Float };

            dockGate.Show(dockMain, DockState.Document);
            dockLeaf.Show(dockGate.Pane, DockAlignment.Bottom, 0.6);
            dockLvl.Show(dockGate.Pane, DockAlignment.Right, 0.6);
            dockSample.Show(dockMain, DockState.Document);

            dockMain.Panes.First(x => x.DockState == DockState.Document).Resize += DockPanelDocumentArea_Resize;
            dockMain.DefaultFloatWindowSize = dockMain.Panes.First(x => x.DockState == DockState.Document).Size;
        }
        private void DockPanelDocumentArea_Resize(object sender, EventArgs e) => dockMain.DefaultFloatWindowSize = dockMain.Panes.First(x => x.DockState == DockState.Document).Size;

        public static DockPane lastclickedpane;
        private void dockMain_ActivePaneChanged(object sender, EventArgs e)
        {
            if (dockMain.ActivePane?.ActiveContent.DockHandler.TabText is not "Project Explorer" and not "Project Properties")
                lastclickedpane = dockMain.ActivePane;
        }

        private void toolstripMainSave_Click(object sender, EventArgs e)
        {
            var activedocument = dockMain.ActiveDocument;
            if (activedocument.GetType() == typeof(Form_MasterEditor)) {
                (activedocument as Form_MasterEditor).Save();
            }
            if (activedocument.GetType() == typeof(Form_RawText)) {
                (activedocument as Form_RawText).Save();
            }
        }
    }
}
