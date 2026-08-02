using System.Diagnostics;
using System.Windows.Shell;
using Cyotek.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;
using Un4seen.Bass; 
using System.Runtime.InteropServices;
using Thumper_Custom_Level_Editor.Other_Forms;
using System.Linq;
using System.Drawing.Text;
using System.Security.Permissions;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
    {
        #region Variables
        public static bool IsClosing;
        public static bool IsLoadingProject;
        //public static bool DontSwitchGAD;
        public static TCLE Instance;
        public static DockPanel DockMain => Instance.dockMain;
        public static DockWorkspace ActiveWorkspace;
        public static IEnumerable<IDockContent> Workspaces => Instance.dockMain.Documents;
        public static Dictionary<string, EditorBase> Documents = new();
        public static ColorPickerDialog colorDialogNew = new() { BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.Black };
        public static ContextMenuStrip TabRightClickMenu;
        public static DirectoryInfo WorkingFolder => ProjectProperties.WorkingFolder;
        public static decimal BPM => ProjectProperties.BPM;
        public static List<string> lvlsinworkfolder = new();
        public static Random rng = new();
        public static string AppLocation => Path.GetDirectoryName(Application.ExecutablePath);
        public static Dictionary<string, Keys> Keybinds = new();
        public static ProjectProperties ProjectProperties;
        public static SettingsUITheme settingsUITheme = new();
        public static bool Fullscreen;
        public static string DragSource = "none";
        public static PrivateFontCollection ImportedFonts = new PrivateFontCollection();
        //Active File Tracking
        public static EditorBase? GlobalActiveDocument
        {
            get => _GAD;
            set {
                if (value == null) {
                    dockProjectProperties.propertyGridProject.SelectedObject = ProjectProperties;
                    dockProjectProperties.TabText = $"Project Properties";
                    _GAD = value;
                    return;
                }
                if (value.WorkingFile is null)
                    return;
                _GAD = value;
                //for testing -> TCLE.Instance.toolstripLevelName.Text = GlobalActiveDocument.WorkingFile.Name;
                dockProjectProperties.propertyGridProject.SelectedObject = GlobalActiveDocument.GetProperties();
                dockProjectProperties.TabText = $"{GlobalActiveDocument.DockHandler.TabText} Properties";

                if (GlobalActiveDocument.GetType() == typeof(EditorLvl)) {
                    GlobalLastLvl = GlobalActiveDocument as EditorLvl;
                }
                else if (GlobalActiveDocument.GetType() == typeof(EditorGate)) {
                    GlobalLastGate = GlobalActiveDocument as EditorGate;
                }
                else if (GlobalActiveDocument.GetType() == typeof(EditorMaster)) {
                    GlobalLastMaster = GlobalActiveDocument as EditorMaster;
                }
            }
        }
        private static EditorBase? _GAD;
        public static EditorLvl? GlobalLastLvl { get; set; }
        public static EditorGate? GlobalLastGate { get; set; }
        public static EditorMaster? GlobalLastMaster { get; set; }
        //Public accessible clipboards
        public static List<Sequencer_Object> ClipboardSequencer = new();
        public static List<SeqDataPoint> ClipboardDataPoints;
        public static List<MasterLvlData> ClipboardMaster = new();
        public static List<LvlLeafData> ClipboardLvl = new();
        public static List<GateLvlData> ClipboardGate = new();
        public static List<LvlPath> ClipboardPaths = new();
        //Beeble things
        public static Beeble MainBeeble = new() { Visible = false };
        public static List<Beeble> ExistingBeebles = new();
        #endregion

        #region Form Construction
        public static DockProjectExplorer Explorer;
        public static DockProjectProperties dockProjectProperties;
        public static DragDropItemList DragDropItems = new("path", null);
        public static VS2015DarkTheme DockTheme = new();
        public TCLE(string LevelFromArg)
        {
            InitializeComponent();
            Instance = this;
            ProjectProperties = new() {
                ProjectName = "",
                Description = "",
                AuthorNames = "",
                BPM = 0,
                WorkingFile = null
            };
            InitializeUI();
            // Initialize Sound library
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_LATENCY, this.Handle);
            // Initialize recent files list. Set empty list if there are none
            Properties.Settings.Default.Recentfiles ??= new List<string>();
            //
            InitializeFolders();
            //call methods to initialize various aspects of the editors
            UtilImport.ImportInit();
            ColorFormElements(TCLE.Instance);
            JumpListUpdate();
            SetKeyBinds();
            SeqObjTreeBuilder.Initialize();
            //import last used custom colors
            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            this.Cursor = new Cursor(new MemoryStream(Properties.Resources.mousebeeble1));
            //load recent levels or the level from input arg
            FileInfo LevelToLoad = new(string.IsNullOrEmpty(LevelFromArg) ? "e" : LevelFromArg);
            if (LevelToLoad.Extension.Equals(".tcl", StringComparison.OrdinalIgnoreCase) && LevelToLoad.Exists) {
                OpenProject(LevelToLoad);
                return;
            }
            RecentFiles(Properties.Settings.Default.Recentfiles ?? new List<string>());
        }
        #endregion
        #region Form Loading Closing
        ///FORM LOADING
        private void TCLEMain_Load(object sender, EventArgs e)
        {
            //finalize boot
            UtilAudio.PlaySound("UIboot");
            //finish loading
            Properties.Settings.Default.firstrun = false;
            Properties.Settings.Default.Save();
            //
            MainBeeble.Visible = false;
            MainBeeble.Size = Properties.Settings.Default.beeblesize;
            MainBeeble.Location = Properties.Settings.Default.beebleloc;
        }

        private void TCLE_SizeChanged(object sender, EventArgs e)
        {
            panelRecentFiles.Location = new((this.Width / 2) - (panelRecentFiles.Width / 2), (this.Height / 2) - (panelRecentFiles.Height / 2));
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
                FileInfo tcl = new(file);
                JumpTask jmp = new() {
                    Title = $"{tcl.Name}",
                    Arguments = file,
                    Description = $"{tcl.FullName}",
                    ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location
                };
                jml.JumpItems.Add(jmp);
            }
            jml.Apply();
            Properties.Settings.Default.Save();
        }

        private void TCLE_FormClosing(object sender, FormClosingEventArgs e)
        {
            //check for unsaved files, cancel closing
            IsClosing = true;
            if (AnyUnsaved()) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to exit?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                    IsClosing = false;
                }
            }
            //save sequencer favs
            Properties.Settings.Default.SequencerFavorites = TCLE.LeafObjects.Values.Where(x => x.favorite).Select(x => $"{x.obj_name};{x.param_path}").ToList();
            //save panel sizes and locations
            Properties.Settings.Default.beeblesize = MainBeeble.Size;
            Properties.Settings.Default.beebleloc = MainBeeble.Location;
            //colors
            Properties.Settings.Default.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            //write quick values to file
            File.WriteAllText($@"{UtilPaths.Settings}\quickvalues.txt", string.Join('\n', TCLE.LeafQuickValues));
            Properties.Settings.Default.Save();
        }

        private void TCLE_FormClosed(object sender, FormClosedEventArgs e)
        {
            ImportedFonts.Dispose();
            alzheimer();
            //
            try {
                UtilPaths.DirTemp.Delete(true);
            } catch (Exception) {
                //this will always fail, as the font file and sequencer .sf2 will still be open
                //this is ok.
            }
        }

        public void SetKeyBinds()
        {
            Dictionary<string, Keys> _default = Properties.Resources.DefaultKeybinds.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
            //check if custom keybinds set
            if (Properties.Settings.Default.UserKeybinds != "-") {
                Dictionary<string, Keys> _user = Properties.Settings.Default.UserKeybinds.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
                //once user keys are loaded, iterate each and copy values over to _default, overwriting defaults
                _user.ToList().ForEach(x => _default[x.Key] = x.Value);
            }
            Keybinds = _default;
            ///
            toolstripFileNewProject.ShortcutKeys = Keybinds["New Project"];
            toolstripFileOpenProject.ShortcutKeys = Keybinds["Open Project"];
            toolstripFileSave.ShortcutKeys = Keybinds["Save File"];
            toolstripFileSaveAs.ShortcutKeys = Keybinds["Save File As"];
            toolstripFileSaveAll.ShortcutKeys = Keybinds["Save All"];
            toolstripFileExit.ShortcutKeys = Keybinds["Close App"];
            toolstripFileRestart.ShortcutKeys = Keybinds["Restart App"];
            ///
            toolstripViewFullscreen.ShortcutKeys = Keybinds["Fullscreen"];
            ///
            toolstripProjectLeaf.ShortcutKeys = Keybinds["New Leaf"];
            toolstripProjectLvl.ShortcutKeys = Keybinds["New Lvl"];
            toolstripProjectGate.ShortcutKeys = Keybinds["New Gate"];
            toolstripProjectMaster.ShortcutKeys = Keybinds["New Master"];
            toolstripProjectSample.ShortcutKeys = Keybinds["New Sample"];
            ///
            toolstripWindowFloat.ShortcutKeys = Keybinds["Float Current Tab"];
            toolstripWindowFloatAll.ShortcutKeys = Keybinds["Float All Tabs"];
            toolstripWindowDock.ShortcutKeys = Keybinds["Dock Floating Tab"];
            toolstripWindowWorkspace.ShortcutKeys = Keybinds["Add New Worksapce"];
            toolStripWindowCloseTab.ShortcutKeys = Keybinds["Close Current Tab"];
            toolStripWindowCloseWorkspace.ShortcutKeys = Keybinds["Close Current Workspace"];
            ///
            toolstripTabSave.ShortcutKeys = Keybinds["Save File"];
        }
        #endregion
        #region Form Moving and Control buttons
        private void toolStripTitle_DoubleClick(object sender, EventArgs e)
        {
            //toolstripFormRestore.PerformClick();
        }
        private void toolStripTitle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //toolstripFormRestore.PerformClick();
        }
        private void toolstripFormRestore_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                MaximizeScreenBounds();
            }
            else {
                this.LocationChanged -= TCLE_LocationChanged;
                this.WindowState = FormWindowState.Normal;
                toolstripFormRestore.Image = Properties.Resources.icon_maximize;
                this.Refresh();
                contextFormMax.Enabled = true;
                contextFormRestore.Enabled = false;
                this.LocationChanged += TCLE_LocationChanged;
                toolstripFormRestore.ToolTipText = "Maximize";
                Fullscreen = false;
            }
        }
        public void MaximizeScreenBounds()
        {
            if (Fullscreen) {
                this.MaximizedBounds = new Rectangle();
                toolstripExitFullscreen.Visible = true;
            }
            else {
                Rectangle bounds = Screen.FromHandle(this.Handle).WorkingArea;
                //Screen WorkingArea is shrunk a small bit compared to the actual display area
                //so the following 4 lines increases the bounds to cover whole screen
                bounds.X = -8;
                bounds.Y = -8;
                bounds.Width += 16;
                bounds.Height += 16;
                this.MaximizedBounds = bounds;
                toolstripExitFullscreen.Visible = false;
            }
            this.WindowState = FormWindowState.Normal;
            this.WindowState = FormWindowState.Maximized;
            this.Refresh();
            toolstripFormRestore.Image = Properties.Resources.icon_restore;
            contextFormMax.Enabled = false;
            contextFormRestore.Enabled = true;
            toolstripFormRestore.ToolTipText = "Restore Down";
        }
        private void TCLE_LocationChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal && this.Location.Y <= 1 && (Control.MouseButtons & MouseButtons.Left) == 0)
                MaximizeScreenBounds();
        }
        private void toolstripFormMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void toolstripFormClose_Click(object sender, EventArgs e)
        {
            DragDropItems.Dispose();
            this.Close();
        }

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
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void TCLE_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                //if (MainBeeble != null) MainBeeble.Visible = false;
            }
            else {
                //if (MainBeeble != null) MainBeeble.Visible = true;
            }

            if (this.WindowState == FormWindowState.Normal)
                toolstripFormRestore.Image = Properties.Resources.icon_maximize;
        }

        private void pictureTunnelViewer_MouseEnter(object sender, EventArgs e)
        {
            pictureTunnelViewer.Visible = false;
        }
        #endregion
        #region Form Key Press
        private void TCLE_KeyDown(object sender, KeyEventArgs e)
        {
            if (dockMain.ActiveContent is DockProjectExplorer or DockProjectProperties)
                return;
            //get the action name from the keypress and use that in the switch instead
            var action = Keybinds.FirstOrDefault(x => x.Value == e.KeyData).Key;
            switch (action) {
                case "Cut":
                    GlobalActiveDocument.Cut();
                    break;
                case "Copy":
                    GlobalActiveDocument.Copy();
                    break;
                case "Paste":
                    GlobalActiveDocument.Paste();
                    break;
                case "Next Tab":
                    SwitchTab(1);
                    break;
                case "Previous Tab":
                    SwitchTab(-1);
                    break;
                case "Move Tab to Next Workspace":
                    MoveTabToWorkspace(1);
                    break;
                case "Move Tab to Prev Workspace":
                    MoveTabToWorkspace(-1);
                    break;
                case "Next Workspace":
                    SwitchWorkspace(1);
                    break;
                case "Previous Workspace":
                    SwitchWorkspace(-1);
                    break;
                case "Undo":
                    UndoSystem.UndoFunction(1);
                    break;
            }
            //e.Handled = true;
        }
        #endregion

        #region Toolstrip Main (no submenu)
        private void toolstripAddScene_Click(object sender, EventArgs e)
        {
            Form_DrawScene draw = new();
            draw.Show(dockMain, DockState.Document);
        }

        private VolumeMaster? VolumeMaster;
        private void btnVolumeMixer_Click(object sender, EventArgs e)
        {
            if (VolumeMaster == null || VolumeMaster.IsDisposed)
                VolumeMaster = new();
            VolumeMaster.Show();
            VolumeMaster.Activate();
        }

        private void toolstripLevelName_Click(object sender, EventArgs e)
        {
            dockProjectProperties.propertyGridProject.SelectedObject = ProjectProperties;
            dockProjectProperties.TabText = $"Project Properties";
            UtilAudio.PlaySound("UIfolderopen");
        }

        private void toolstripStopAudio_Click(object sender, EventArgs e)
        {
            UtilAudio.StopAudio();
        }
        #endregion
        #region Toolstrip File
        private void contextmenuFile_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GlobalActiveDocument == null) {
                toolstripFileSave.Text = "Save";
                toolstripFileSaveAs.Text = "Save As...";
                toolstripFileSave.Enabled = toolstripFileSaveAs.Enabled = false;
            }
            else {
                toolstripFileSave.Text = "Save " + GlobalActiveDocument.DockHandler.TabText;
                toolstripFileSaveAs.Text = "Save " + GlobalActiveDocument.DockHandler.TabText + " As...";
                toolstripFileSave.Enabled = toolstripFileSaveAs.Enabled = true;
            }
        }

        private void toolstripFileNewProject_Click(object sender, EventArgs e)
        {
            MenuProjectNew customlevel = new() { Owner = this };
            UtilAudio.PlaySound("UIfolderopen");
            customlevel.ShowDialog();
            if (customlevel.DialogResult == DialogResult.Yes)
                OpenProject(customlevel.ProjectToLoad);
            customlevel.Dispose();
        }

        private void toolstripFileOpenProject_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            UtilAudio.PlaySound("UIfolderopen");
            ofd.Title = "Open Project";
            ofd.Filter = "Thumper Custom Level (*.TCL)|*.TCL";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = TCLE.WorkingFolder?.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                FileInfo TCL = new(ofd.FileName);
                OpenProject(TCL);
            }
        }

        public void OpenProject(FileInfo TCL)
        {
            if (TCL.DirectoryName == TCLE.WorkingFolder?.FullName)
                return;
            //Try locking the .TCL first. If it fails, the level is already open
            //in that case, return before doing anything
            try {
                var _testlock = new FileStream(TCL.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                _testlock.Close();
            } catch (Exception) {
                MessageBox.Show($"That project is open already in another instance of the Level Editor.", "Thumper Custom Level Editor");
                return;
            }
            //if this application already has a TCL loaded, open a new application and pass in the TCL name to load it
            if (WorkingFolder != null) {
                ProcessStartInfo info = new(Application.ExecutablePath, TCL.FullName);
                Process.Start(info);
                return;
            }
            //load the properties of the TCL and create projectProperties
            dynamic ProjectJson = UtilFile.LoadFileLock(TCL);
            Image Thumbnail = null;
            if (File.Exists($@"{TCL.Directory}\thumbnail.png")) {
                using (FileStream fs = new($@"{TCL.Directory}\thumbnail.png", FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    Thumbnail = Image.FromStream(fs);
                }
            }
            ProjectProperties = new() {
                ProjectName = (string)ProjectJson["level_name"] ?? "New Project",
                Difficulty = (string)ProjectJson["difficulty"] ?? "D0",
                Description = (string)ProjectJson["description"] ?? "Please add a description",
                AuthorNames = (string)ProjectJson["author"] ?? "a person",
                BPM = (decimal?)ProjectJson["bpm"] ?? 400m,
                WorkingFile = TCL,
                Thumbnail = Thumbnail
            };
            MenusVisible(true);
            this.Cursor = Cursors.Arrow;
            //load colors, with failover to White
            try {
                dynamic railcolor = ProjectJson["rails_color"];
                ProjectProperties.RailColor = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
                dynamic railglowcolor = ProjectJson["rails_glow_color"];
                ProjectProperties.RailGlowColor = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
                dynamic pathcolor = ProjectJson["path_color"];
                ProjectProperties.PathColor = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
            } catch (Exception) {
                ProjectProperties.RailColor = Color.White;
                ProjectProperties.RailGlowColor = Color.White;
                ProjectProperties.PathColor = Color.White;
            }
            //update some visual elements
            toolstripLevelName.Text = ProjectProperties.ProjectName;
            toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject($"difficulty_{ProjectProperties.Difficulty}");
            //add to recent files
            Properties.Settings.Default.Recentfiles.Remove(TCL.FullName);
            Properties.Settings.Default.Recentfiles.Insert(0, TCL.FullName);
            JumpListUpdate();
            //load sample of the project
            ReloadProjectSamples();

            //create Project Explorer and Project Property panels
            Explorer = new() { TabText = "Project Explorer", DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
            dockProjectProperties = new() { TabText = "Project Properties", DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
            //Load the project''s files into Explorer
            Explorer.LoadProject();
            dockProjectProperties.LoadProjectProperties();
            //create a workspace
            IsLoadingProject = true;
            if (!UtilPaths.DirCurrentProjectSettings.Exists)
                UtilPaths.DirCurrentProjectSettings.Create();
            DeserializeDockContent m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            try {
                dockMain.LoadFromXml($@"{UtilPaths.CurrentProjectSettings}\layout_workspace.config", m_deserializeDockContent);
            } catch {
                DockWorkspace workspace1 = new($"Workspace {Workspaces.Count() + 1}");
                workspace1.Show(dockMain, DockState.Document);
                Explorer.Show(dockMain, DockState.DockRight);
                dockProjectProperties.Show(Explorer.Pane, DockAlignment.Bottom, 0.35);
                OpenFile(ProjectExplorer.Files.Values.FirstOrDefault(x => x.FullName.EndsWith(".master", StringComparison.OrdinalIgnoreCase)));
            }
            foreach (DockWorkspace _ws in TCLE.Workspaces) {
                if (File.Exists($@"{UtilPaths.CurrentProjectSettings}\layout_{_ws.Text}.config"))
                    _ws.dockMain.SaveAsXml($@"{UtilPaths.CurrentProjectSettings}\layout_{_ws.Text}.config");
            }
            IsLoadingProject = false;
            //this will be the loading sound :D
            UtilAudio.PlaySound($"UIbeetleclick{rng.Next(1, 9)}");

            DockPane documentsPane = dockMain.Panes.FirstOrDefault(x => x.DockState == DockState.Document);
            if (documentsPane != null) {
                documentsPane.Resize += DockPanelDocumentArea_Resize;
                dockMain.DefaultFloatWindowSize = documentsPane.Size;
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString.Contains("Workspace"))
                return new DockWorkspace(persistString) { TabText = persistString };
            if (persistString is "Project Explorer")
                return Explorer;
            if (persistString.EndsWith("Properties"))
                return dockProjectProperties;

            throw new NotImplementedException();
        }

        private void toolstripFileConvert_Click(object sender, EventArgs e)
        {
            ConvertProjectToNew();
        }

        private void toolstripFileSaveAs_Click(object sender, EventArgs e)
        {
            if (GlobalActiveDocument == null)
                return;
            GlobalActiveDocument.SaveAs(false, null);
            TCLE.SaveTCL();
        }

        private void toolstripFileTemplateFolder_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new() {
                Arguments = UtilPaths.Temp,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

        private void toolstripFileTemplateRegen_Click(object sender, EventArgs e)
        {
            InitializeTemplateFiles();
        }

        private void toolstripFileRecent_Click(object sender, EventArgs e)
        {
            MenusVisible(false);
        }

        private void contextMenuRecentProjects_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenuRecentProjects.Items.Clear();
            //clear projects that no longer exist
            Properties.Settings.Default.Recentfiles.RemoveAll(path => !File.Exists(path));
            //build the menu
            foreach (string path in Properties.Settings.Default.Recentfiles) {
                FileInfo tcl = new(path);
                ToolStripMenuItem item = new() {
                    Text = $"{tcl.Name} ({tcl.FullName})",
                    ForeColor = Color.White,
                    Image = Properties.Resources.icon_tcle,
                    //tag is set to 'tcl' (the actual FileInfo) here so it's easy to grab later
                    Tag = tcl
                };
                contextMenuRecentProjects.Items.Add(item);
            }
        }

        private void contextMenuRecentProjects_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //FileInfo tcl = new(e.ClickedItem.Text.Split('(')[1].TrimEnd(')'));
            if (e.ClickedItem.Tag is FileInfo tcl)
                OpenProject(tcl);
        }

        private void toolstripFileClearTemp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all temp files?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            foreach (string file in Directory.GetFiles(UtilPaths.Temp, "*.*"))
                File.Delete(file);
        }

        private void toolstripFileExit_Click(object sender, EventArgs e)
        {
            DragDropItems.Dispose();
            this.Close();
        }

        private void toolstripFileRestart_Click(object sender, EventArgs e)
        {
            DragDropItems.Dispose();
            Application.Restart();
            Environment.Exit(0);
        }
        #endregion
        #region Toolstrip Edit
        private void toolstripEditCut_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.Cut();
        }

        private void toolstripEditCopy_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.Copy();
        }

        private void toolstripEditPaste_Click(object sender, EventArgs e)
        {
            if (GlobalActiveDocument is EditorRawText)
                return;
            GlobalActiveDocument.Paste();
        }

        private void toolstripEditPreferences_Click(object sender, EventArgs e)
        {
            //Show the CustomWorkspace form. If form OK, then save the settings to app properties
            //then call method to recolor the form elements immediately
            MenuPreferences custom = new() { Owner = this };
            UtilAudio.PlaySound("UIinterpolatewindow");
            //custom._objects = _objects;
            if (custom.ShowDialog() == DialogResult.OK) {

            }
            custom.Dispose();
        }
        #endregion
        #region Toolstrip View
        private void leafoptionShowCategory_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowCategory = leafoptionShowCategory.Checked;
            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>()) {
                leaf.RefreshHeaders();
            }
        }

        private void leafoptionShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowGrid = leafoptionShowGrid.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionConnectBars_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionConnectBars = leafoptionConnectBars.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionShowLanes_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowLane = leafoptionShowLanes.Checked;
            RefreshLeafEditors(true);
        }

        private void leafoptionEaseDots_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionEaseDots = leafoptionEaseDots.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionThinValues_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionThinBars = leafoptionThinValues.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionShowWave_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowWave = leafoptionShowWave.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionVerticalCells_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionVerticalCells = leafoptionVerticalCells.Checked;
            RefreshLeafEditors();
        }

        private void leafoptionPlaybackScroll_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionPlaybackScroll = leafoptionPlaybackScroll.Checked;
        }

        private void toolstripViewExplorer_Click(object sender, EventArgs e)
        {
            if (Explorer.IsDisposed) {
                Explorer = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
                ProjectExplorer.CreateTreeView();
            }

            if (dockProjectProperties.Pane != null)
                Explorer.Show(dockProjectProperties.Pane, DockAlignment.Top, 0.7);
            else
                Explorer.Show(dockMain, DockState.DockRight);
        }

        private void toolstripViewProperties_Click(object sender, EventArgs e)
        {
            if (dockProjectProperties.IsDisposed) {
                dockProjectProperties = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
                dockProjectProperties.LoadProjectProperties();
            }

            if (Explorer.Pane != null)
                dockProjectProperties.Show(Explorer.Pane, DockAlignment.Bottom, 0.3);
            else
                dockProjectProperties.Show(dockMain, DockState.DockRight);
        }

        private void toolstripViewFullscreen_Click(object sender, EventArgs e)
        {
            if (Fullscreen) {
                Fullscreen = false;
                MaximizeScreenBounds();
                toolstripExitFullscreen.Visible = false;
                toolstripViewFullscreen.Text = "Fullscreen";
            }
            else {
                Fullscreen = true;
                MaximizeScreenBounds();
                toolstripExitFullscreen.Visible = true;
                toolstripViewFullscreen.Text = "Exit Fullscreen";
            }
        }

        private void toolstripExitFullscreen_Click(object sender, EventArgs e)
        {
            Fullscreen = false;
            MaximizeScreenBounds();
            toolstripExitFullscreen.Visible = false;
        }
        #endregion
        #region Toolstrip Window
        private void toolstripWindowFloat_Click(object sender, EventArgs e)
        {
            if (GlobalActiveDocument is null)
                return;
            if (!string.IsNullOrEmpty(GlobalActiveDocument.Text) && GlobalActiveDocument.DockHandler.DockState != DockState.Float)
                GlobalActiveDocument.DockHandler.DockState = DockState.Float;
            //ActiveWorkspace.dockMain.ActiveDocument.DockHandler.DockState = DockState.Float;
        }
        private void toolstripWindowFloatAll_Click(object sender, EventArgs e)
        {
            foreach (IDockContent dc in ActiveWorkspace.dockMain.Documents) {
                dc.DockHandler.DockState = DockState.Float;
            }
        }
        private void toolstripWindowDock_Click(object sender, EventArgs e) => TCLE.GlobalActiveDocument.DockHandler.DockState = DockState.Document;

        private void toolStripWindowCloseWorkspace_Click(object sender, EventArgs e)
        {
            if (ActiveWorkspace == null)
                return;
            if (AnyUnsaved(ActiveWorkspace)) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to close them?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            ActiveWorkspace.Close();
            ActiveWorkspace.DockHandler.Dispose();
        }

        private void toolstripWindowCloseAll_Click(object sender, EventArgs e)
        {
            if (AnyUnsaved()) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to close them?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            while (dockMain.Documents.Any()) {
                dockMain.Documents.First().DockHandler.Close();
            }
        }

        private void toolstripWindowCloseEditors_Click(object sender, EventArgs e)
        {
            if (ActiveWorkspace == null)
                return;
            if (AnyUnsaved(ActiveWorkspace)) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to close them?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            while (ActiveWorkspace.dockMain.Documents.Any())
                ActiveWorkspace.dockMain.Documents.First().DockHandler.Close();
        }

        private void toolstripWindowCloseFiletype_Click(object sender, EventArgs e)
        {
            if (ActiveWorkspace == null)
                return;
            if (AnyUnsaved(ActiveWorkspace, GlobalActiveDocument.GetType())) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to close them?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            foreach (IDockContent document in ActiveWorkspace.dockMain.Documents.ToList()) {
                if (document != GlobalActiveDocument && document.GetType() == GlobalActiveDocument.GetType())
                    document.DockHandler.Close();
            }
        }

        private void addNewWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DockWorkspace workspace1 = new($"Workspace {Workspaces.Count() + 1}") { DockAreas = DockAreas.Document };
            workspace1.Show(dockMain, DockState.Document);
        }

        private void contextmenuMoveWorkspace_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextmenuMoveWorkspace.Items.Clear();
            foreach (IDockContent ws in Workspaces) {
                ToolStripMenuItem item = new() {
                    Text = ws.DockHandler.TabText,
                    ForeColor = Color.White,
                    Image = Properties.Resources.editor_workspace,
                    Checked = ws == ActiveWorkspace
                };
                contextmenuMoveWorkspace.Items.Add(item);
            }
        }

        private void contextmenuMoveWorkspace_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DockWorkspace workspace = Workspaces.First(x => x.DockHandler.TabText == e.ClickedItem.Text) as DockWorkspace;
            (GlobalActiveDocument as DockContent).Show(workspace.dockMain, DockState.Document);
        }
        #endregion
        #region Toolstrip Help
        private void toolstripHelpGameDir_Click(object sender, EventArgs e)
        {
            UtilImport.GetThumperCacheFolder();
        }
        private void toolstripHelpTentacles_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1dGkU9uqlr3Hp2oJiVFMHHpIKt8S_c0Vi27n47ZRD0_0", UseShellExecute = true });
        private void toolstripHelpObjects_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1JWk7TDn4ZuitclB-x7gOYxU-PsmGkooZuU9QEd_aw1A", UseShellExecute = true });
        private void toolstripHelpAudio_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q", UseShellExecute = true });
        private void toolstripHelpAbout_Click(object sender, EventArgs e) => new MenuAbout().Show();
        private void toolstripHelpDiscord_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://discord.com/invite/gTQbquY", UseShellExecute = true });
        private void toolstripHelpGithub_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://github.com/CocoaMix86/Thumper-Custom-Level-Editor", UseShellExecute = true });
        private void toolstripHelpKofi_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/I2I5ZZBRH", UseShellExecute = true });
        #endregion
        #region Toolstrip Project
        private void toolstripProjectLeaf_Click(object sender, EventArgs e)
        {
            OpenFile(new EditorLeaf().SaveAs(true));
        }

        private void toolstripProjectLvl_Click(object sender, EventArgs e)
        {
            OpenFile(new EditorLvl().SaveAs(true));
        }

        private void toolstripProjectGate_Click(object sender, EventArgs e)
        {
            OpenFile(new EditorGate().SaveAs(true));
        }

        private void toolstripProjectMaster_Click(object sender, EventArgs e)
        {
            if (TCLE.CheckForMaster()) {
                if (MessageBox.Show("This project already has a master file. It is not recommended to have more than 1 as it can mess up the mod loader.\nDo you still wish to continue?", "Bad Idea", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            OpenFile(new EditorMaster().SaveAs(true));
        }

        private void toolstripProjectSample_Click(object sender, EventArgs e)
        {
            OpenFile(new EditorSample().SaveAs(true));
        }
        public static string[] fileextensions = new string[] { "leaf_", "lvl_", "gate_", "master_", "samp_", "xfm_", "spn_" };
        private void toolstripProjectExisting_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Title = "Copy Existing File to Project";
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = TCLE.WorkingFolder?.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                FileInfo filetocopy = new(ofd.FileName);
                if (File.Exists($"{WorkingFolder.FullName}\\{filetocopy.Name}")) {
                    MessageBox.Show("A file with that name exists in the project folder already.", "Thumper Custom Level Editor");
                    return;
                }
                FileInfo projectfile = new($"{WorkingFolder.FullName}\\{filetocopy.Name}");
                File.Copy(ofd.FileName, projectfile.FullName);

                if (fileextensions.Any(x => projectfile.Name.StartsWith(x))) {
                    if (MessageBox.Show("This appears to be a file from an older version of the editor.\nConvert it to the new TCLE 3.0 format?", "Editor Custom Thumper Level", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        string[] splitextension = projectfile.Name.Replace(".txt", "").Split('_', 2);
                        projectfile.MoveTo($"{projectfile.DirectoryName}\\{splitextension[1]}.{splitextension[0]}");
                        //File.Move(projectfile.FullName, $"{projectfile.DirectoryName}\\{splitextension[1]}.{splitextension[0]}");
                        //projectfile = new($"{projectfile.DirectoryName}\\{splitextension[1]}.{splitextension[0]}");
                    }
                }

                ProjectExplorer.CreateTreeView();
                OpenFile(projectfile);
            }
        }

        private void toolstripProjectRegen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will overwrite the \"default\" files in the working folder. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                //spn
                FileInfo _locatespn = WorkingFolder.GetFiles("default.spn", SearchOption.AllDirectories).FirstOrDefault();
                if (_locatespn != null)
                    File.WriteAllText(_locatespn.FullName, Properties.Resources.spn_default);
                else
                    File.WriteAllText($@"{WorkingFolder}\default.spn", Properties.Resources.spn_default);
                //xfm
                FileInfo _locatexfm = WorkingFolder.GetFiles("default.xfm", SearchOption.AllDirectories).FirstOrDefault();
                if (_locatexfm != null)
                    File.WriteAllText(_locatexfm.FullName, Properties.Resources.xfm_default);
                else
                    File.WriteAllText($@"{WorkingFolder}\default.xfm", Properties.Resources.xfm_default);
            }
        }

        private void contextmenuSampPacks_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            //this prevents the menu from closing when an option is chosen, allowing to select multiple before exiting the menu
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) {
                e.Cancel = true;
                return;
            }

            Tuple<FileInfo, bool, string>[] samplePacks = {
                new(new FileInfo($@"{WorkingFolder}\level1_320bpm.samp"), toolstripSampLevel1.Checked, Properties.Resources.samp_level1_320bpm),
                new(new FileInfo($@"{WorkingFolder}\level2_340bpm.samp"), toolstripSampLevel2.Checked, Properties.Resources.samp_level2_340bpm),
                new(new FileInfo($@"{WorkingFolder}\level3_360bpm.samp"), toolstripSampLevel3.Checked, Properties.Resources.samp_level3_360bpm),
                new(new FileInfo($@"{WorkingFolder}\level4_380bpm.samp"), toolstripSampLevel4.Checked, Properties.Resources.samp_level4_380bpm),
                new(new FileInfo($@"{WorkingFolder}\level5_400bpm.samp"), toolstripSampLevel5.Checked, Properties.Resources.samp_level5_400bpm),
                new(new FileInfo($@"{WorkingFolder}\level6_420bpm.samp"), toolstripSampLevel6.Checked, Properties.Resources.samp_level6_420bpm),
                new(new FileInfo($@"{WorkingFolder}\level7_440bpm.samp"), toolstripSampLevel7.Checked, Properties.Resources.samp_level7_440bpm),
                new(new FileInfo($@"{WorkingFolder}\level8_460bpm.samp"), toolstripSampLevel8.Checked, Properties.Resources.samp_level8_460bpm),
                new(new FileInfo($@"{WorkingFolder}\level9_480bpm.samp"), toolstripSampLevel9.Checked, Properties.Resources.samp_level9_480bpm),
                new(new FileInfo($@"{WorkingFolder}\dissonant.samp"), toolstripSampLevelDiss.Checked, Properties.Resources.samp_dissonant),
                new(new FileInfo($@"{WorkingFolder}\globaldrones.samp"), toolstripSampLevelDrones.Checked, Properties.Resources.samp_globaldrones),
                new(new FileInfo($@"{WorkingFolder}\rests.samp"), toolstripSampLevelRests.Checked, Properties.Resources.samp_rests),
                new(new FileInfo($@"{WorkingFolder}\misc.samp"), toolstripSampLevelMisc.Checked, Properties.Resources.samp_misc)
            };

            bool filesupdates = false;
            FileInfo[] files = WorkingFolder.GetFiles("*", SearchOption.AllDirectories);
            ///create samp_ files if any boxes are checked
            foreach (Tuple<FileInfo, bool, string> pack in samplePacks) {
                if (pack.Item2) {
                    if (!files.Any(x => x.Name == pack.Item1.Name)) {
                        using (StreamWriter sw = pack.Item1.CreateText()) {
                            sw.Write(pack.Item3);
                        }
                        UpdateProjectSamplesFromFile(pack.Item1, true, false, out string _);
                        filesupdates = true;
                    }
                }
                else {
                    if (files.Any(x => x.Name == pack.Item1.Name)) {
                        TCLE.CloseFile(pack.Item1);
                        TCLE.RemoveProjectSamples(files.First(x => x.Name == pack.Item1.Name));
                        filesupdates = true;
                        pack.Item1.Delete();
                    }
                }
            }

            if (filesupdates) {
                UpdateEditorsWithSamples();
                ProjectExplorer.CreateTreeView();
            }
        }

        private void contextmenuSampPacks_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] files = Directory.GetFiles(WorkingFolder.FullName, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();
            toolstripSampLevel1.Checked = files.Contains($"level1_320bpm.samp");
            toolstripSampLevel2.Checked = files.Contains($"level2_340bpm.samp");
            toolstripSampLevel3.Checked = files.Contains($"level3_360bpm.samp");
            toolstripSampLevel4.Checked = files.Contains($"level4_380bpm.samp");
            toolstripSampLevel5.Checked = files.Contains($"level5_400bpm.samp");
            toolstripSampLevel6.Checked = files.Contains($"level6_420bpm.samp");
            toolstripSampLevel7.Checked = files.Contains($"level7_440bpm.samp");
            toolstripSampLevel8.Checked = files.Contains($"level8_460bpm.samp");
            toolstripSampLevel9.Checked = files.Contains($"level9_480bpm.samp");
            toolstripSampLevelDiss.Checked = files.Contains($"dissonant.samp");
            toolstripSampLevelDrones.Checked = files.Contains($"globaldrones.samp");
            toolstripSampLevelRests.Checked = files.Contains($"rests.samp");
            toolstripSampLevelMisc.Checked = files.Contains($"misc.samp");
        }

        private void toolstripProjectPreload_Click(object sender, EventArgs e)
        {
            UtilAudio.CalculateSampleRuntimes();
            foreach (SampleData samp in ProjectSamples.Values) {
                UtilAudio.PCtoAudioFile(samp);
            }
        }
        #endregion

        #region Toolstrip Toolbar
        private void toolstripMainSave_Click(object sender, EventArgs e)
        {
            if (GlobalActiveDocument == null)
                return;
            GlobalActiveDocument.Save(true);
            //FindEditorRunMethod(GlobalActiveDocument.GetType(), "Save");
            TCLE.SaveTCL();
        }

        private void toolstripMainSaveAll_Click(object sender, EventArgs e)
        {
            if (GlobalActiveDocument == null)
                return;
            foreach (DockWorkspace workspace in Workspaces) {
                foreach (EditorBase document in workspace.dockMain.Documents) {
                    document.Save(false);
                    //FindEditorRunMethod(document.GetType(), "Save");
                }
            }
            TCLE.SaveTCL();
            UtilAudio.PlaySound("UIsave");
        }
        #endregion

        #region DockPanel
        private void DockPanelDocumentArea_Resize(object sender, EventArgs e) => dockMain.DefaultFloatWindowSize = dockMain.Panes.First(x => x.DockState == DockState.Document).Size;

        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (dockMain.ActiveDocument == null)
                return;
            if (dockMain.ActiveDocument.DockHandler.TabText is not "Project Explorer" and not "Project Properties")
                ActiveWorkspace = dockMain.ActiveDocument as DockWorkspace;
            //carry along all float windows to the new active workspace
            //this allows them to be docked there.
            /*
            foreach (Form_WorkSpace work in Workspaces) {
                if (work.dockMain.FloatWindows.Count == 0) continue;
                var floats = work.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(x => x.Contents).ToList();
                for (int i = 0; i < floats.Count; i++) {
                    (floats[i] as DockContent).Show(ActiveWorkspace.dockMain, DockState.Float);
                }
            }*/
        }

        private void dockMain_ActiveContentChanged(object sender, EventArgs e)
        {
            if (TCLE.IsLoadingProject)
                return;
            if (!UtilPaths.DirCurrentProjectSettings.Exists)
                UtilPaths.DirCurrentProjectSettings.Create();
            dockMain.SaveAsXml($@"{UtilPaths.CurrentProjectSettings}\layout_workspace.config");
        }
        #endregion
        #region Dock Tab Rightclick
        private void contextmenuTabClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripTabSave.Text = "Save " + GlobalActiveDocument.DockHandler.TabText;
        }

        private void toolstripTabClose_Click(object sender, EventArgs e)
        {
            if (!GlobalActiveDocument.Saved) {
                if (MessageBox.Show("File is unsaved. Are you sure you want to close it?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            GlobalActiveDocument.DockHandler.Dispose();
        }

        private void toolstripTabCloseOther_Click(object sender, EventArgs e)
        {
            if (ActiveWorkspace == null)
                return;
            if (AnyUnsaved(ActiveWorkspace)) {
                if (MessageBox.Show("Some files are unsaved. Are you sure you want to close them?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }
            foreach (IDockContent document in ActiveWorkspace.dockMain.Documents.ToList()) {
                if (document != GlobalActiveDocument)
                    document.DockHandler.Dispose();
            }
        }

        private void toolstripTabCopyPath_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(ProjectExplorer.Files.First(x => x.FullName.EndsWith($@"\{GlobalActiveDocument.DockHandler.TabText.Replace("*", "").Split(" [")[0]}"))?.FullName);
            Clipboard.SetText(GlobalActiveDocument.WorkingFile.FullName);
        }

        private void toolstripTabOpenFolder_Click(object sender, EventArgs e)
        {
            DirectoryInfo foldertoopen = GlobalActiveDocument.WorkingFile.Directory;
            if (foldertoopen != null && foldertoopen.Exists)
                Process.Start("explorer.exe", $@"/select, ""{foldertoopen.FullName}""");
        }
        #endregion
        #region Undo System
        private void toolstripMainUndo_ButtonClick(object sender, EventArgs e)
        {
            if (GlobalActiveDocument is EditorRawText)
                return;
            UndoSystem.UndoFunction(1);
        }

        private void toolstripMainUndo_DropDownOpening(object sender, EventArgs e)
        {
            if (GlobalActiveDocument is EditorRawText)
                return;
            toolstripMainUndo.DropDown = UndoSystem.CreateUndoMenu(TCLE.GlobalActiveDocument.UndoList);
        }

        private void toolstripMainUndo_DropDownOpened(object sender, EventArgs e)
        {
        }
        #endregion

        //Forcefully garbage collect everything. Highly important to keep this app's memory usage low, especially while streaming audio
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public static void alzheimer()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _ = SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        //this handles the taskbar button
        protected override CreateParams CreateParams
        {
            get {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

                // Extend the CreateParams property of the Button class.
                CreateParams cp = base.CreateParams;
                // Update the button Style.
                cp.Style &= ~0xC00000; //WS_CAPTION;
                cp.Caption = $"Thumper-CLE {TCLE.VersionNumber}";

                return cp;
            }
        }
    }
}
