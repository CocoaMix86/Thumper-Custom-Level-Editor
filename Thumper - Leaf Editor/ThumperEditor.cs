using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Windows.Shell;
using Cyotek.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;
using Un4seen.Bass;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
    {
        #region Variables
        public static bool IsClosing;
        public static TCLE Instance;
        public static DockPanel DockMain => Instance.dockMain;
        public static Form_WorkSpace ActiveWorkspace;
        public static IEnumerable<IDockContent> Workspaces => Instance.dockMain.Documents;
        public static IEnumerable<IDockContent> Documents => Instance.dockMain.Documents.SelectMany(x => (x as Form_WorkSpace).dockMain.Documents);
        public static ColorPickerDialog colorDialogNew = new() { BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.Black };
        public static ContextMenuStrip TabRightClickMenu;
        private Properties.Settings AppSettings = Properties.Settings.Default;
        public static DirectoryInfo WorkingFolder => ProjectProperties.WorkingFolder;
        public static decimal BPM => ProjectProperties.bpm;
        public static List<string> lvlsinworkfolder = new();
        public static Random rng = new();
        public static string AppLocation = Path.GetDirectoryName(Application.ExecutablePath);
        public static Dictionary<string, Keys> defaultkeybinds = Properties.Resources.defaultkeybinds.Split('\n').ToDictionary(g => g.Split(';')[0], g => (Keys)Enum.Parse(typeof(Keys), g.Split(';')[1], true));
        public static Dictionary<FileInfo, FileStream> lockedfiles = new();
        public static Beeble MainBeeble = new() { Visible = false };
        public ProjectProperties projectProperties
        {
            get => ProjectProperties;
            set => ProjectProperties = value;
        }
        public static ProjectProperties ProjectProperties;
        public static List<Sequencer_Object> ClipboardSequencer = new();
        public static bool Fullscreen;
        #endregion

        #region Form Construction
        public static Form_ProjectExplorer ProjectExplorer;
        public static Form_ProjectProperties dockProjectProperties;
        public static ulong Bits(ulong raw, int lowestBit, int numBits)
        {
            ulong mask = 1;
            for (var i = 1; i < numBits; i++) {
                mask = (mask << 1) | 1;
            }

            mask <<= lowestBit;

            return (raw & mask) >> lowestBit;
        }
        public TCLE(string LevelFromArg)
        {
            BinaryReader bin = new(new MemoryStream(new byte[] { 0x30, 0x00, 0x00, 0x00, 0xB0, 0x35, 0x63, 0x00 }));
            var encoded = bin.ReadUInt64();
            bool HasAnyChunks = (encoded & 1) == 1; //Bit 0
            uint FrequencyId = (uint)Bits(encoded, 1, 4); //Bits 1-4
            var pow2 = (int)Bits(encoded, 5, 2); //Bits 5-6
            int NumChannels = 1 << pow2;
            bool IsStereo = NumChannels == 2;
            ulong DataOffset = Bits(encoded, 7, 27) * 32;
            ulong SampleCount = Bits(encoded, 34, 30);


            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            dockMain.Theme = new VS2015DarkTheme();
            Instance = this;
            TabRightClickMenu = contextmenuTabClick;
            projectProperties = new() {
                projectname = "",
                description = "",
                authornames = "",
                bpm = 0,
                WorkingFolder = null
            };

            // Initialize Sound library
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_LATENCY, this.Handle);
            //set custom renderer
            toolStripTitle.Renderer = new ToolStripMainForm();
            toolStripMain.Renderer = new ToolStripOverride();
            contextmenuFile.Renderer = new ContextMenuColors();
            contextmenuEdit.Renderer = new ContextMenuColors();
            contextmenuView.Renderer = new ContextMenuColors();
            contextMenuProject.Renderer = new ContextMenuColors();
            contextmenuWindow.Renderer = new ContextMenuColors();
            contextmenuHelp.Renderer = new ContextMenuColors();
            contextmenuTabClick.Renderer = new ContextMenuColors();
            contextmenuSampPacks.Renderer = new ContextMenuColors();
            contextmenuMoveWorkspace.Renderer = new ContextMenuColors();
            contextMenuRecentProjects.Renderer = new ContextMenuColors();
            //set check states from saved settings
            leafoptionShowCategory.Checked = Properties.Settings.Default.LeafOptionShowCategory;
            leafoptionShowGrid.Checked = Properties.Settings.Default.LeafOptionShowGrid;
            leafoptionConnectBars.Checked = Properties.Settings.Default.LeafOptionConnectBars;
            leafoptionShowLanes.Checked = Properties.Settings.Default.LeafOptionShowLane;
            leafoptionEaseDots.Checked = Properties.Settings.Default.LeafOptionEaseDots;
            leafoptionThinValues.Checked = Properties.Settings.Default.LeafOptionThinBars;
            //
            if (AppSettings.Recentfiles == null)
                AppSettings.Recentfiles = new List<string>();
            //
            //Create directory for leaf templates and other default files
            if (!Directory.Exists($@"{AppLocation}\templates")) {
                toolstripFileTemplateRegen_Click(null, null);
            }
            if (!Directory.Exists($@"{AppLocation}\temp")) {
                Directory.CreateDirectory($@"{AppLocation}\temp");
            }
            //call methods to initialize various aspects of the editors
            ImportObjects();
            ColorFormElements();
            JumpListUpdate();
            LoadQuickValues();
            SetKeyBinds();
            //import default object colors
            colorDialog1.CustomColors = AppSettings.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //load recent levels or the level from input arg
            List<string> levellist = AppSettings.Recentfiles ?? new List<string>();
            FileInfo LevelToLoad = new(string.IsNullOrEmpty(LevelFromArg) ? "e" : LevelFromArg);
            if (levellist.Count > 0 && !LevelToLoad.Extension.Equals(".tcl", StringComparison.OrdinalIgnoreCase))
                RecentFiles(levellist);
            else if (LevelToLoad.Extension.Equals(".tcl", StringComparison.OrdinalIgnoreCase) && LevelToLoad.Exists) {
                OpenProject(LevelToLoad);
            }
        }
        #endregion
        #region Form Loading Closing
        ///FORM LOADING
        private void FormLeafEditor_Load(object sender, EventArgs e)
        {
            //finalize boot
            PlaySound("UIboot");
            ///version check
            /*
            if (AppSettings.version != "2.2release1") {
                ShowChangelog();
                if (MessageBox.Show($"2.2 contains many new objects to use! You will need to update the track_objects.txt file to use them. Do this now?", "NEW VERSION NOTICE!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    toolstripFileTemplateRegen_Click(null, null);
                else
                    MessageBox.Show("You can update later from the File menu.\nFile > Template Files > Regenerate", "ok", MessageBoxButtons.OK);
                AppSettings.version = "2.2release1";
            }
            */
            //finish loading
            AppSettings.firstrun = false;
            AppSettings.Save();
            //
            MainBeeble.Size = AppSettings.beeblesize;
            MainBeeble.Location = AppSettings.beebleloc;
        }

        private void JumpListUpdate()
        {
            if (AppSettings.Recentfiles == null)
                return;

            JumpList jml = new() {
                ShowRecentCategory = true,
                ShowFrequentCategory = true
            };

            foreach (string file in AppSettings.Recentfiles) {
                FileInfo tcl = new FileInfo(file);
                JumpTask jmp = new() {
                    Title = $"{tcl.Name}",
                    Arguments = file,
                    Description = $"{tcl.FullName}",
                    ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location
                };
                jml.JumpItems.Add(jmp);
            }
            jml.Apply();
            AppSettings.Save();
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
            //save panel sizes and locations
            AppSettings.beeblesize = MainBeeble.Size;
            AppSettings.beebleloc = MainBeeble.Location;
            //colors
            AppSettings.colordialogcustomcolors = colorDialog1.CustomColors.ToList();
            //write quick values to file
            File.WriteAllText($@"{TCLE.AppLocation}\templates\quickvalues.txt", $"{TCLE.LeafQuickValue0}\n{TCLE.LeafQuickValue1}\n{TCLE.LeafQuickValue2}\n{TCLE.LeafQuickValue3}\n{TCLE.LeafQuickValue4}\n{TCLE.LeafQuickValue5}\n{TCLE.LeafQuickValue6}\n{TCLE.LeafQuickValue7}\n{TCLE.LeafQuickValue8}\n{TCLE.LeafQuickValue9}");
            AppSettings.Save();
        }
        private static void SetKeyBinds()
        {
            if (File.Exists($@"{AppLocation}\templates\keybinds.txt")) {
                Dictionary<string, Keys> import = File.ReadAllLines($@"{AppLocation}\templates\keybinds.txt").ToDictionary(g => g.Split(';')[0], g => Enum.Parse<Keys>(g.Split(';')[1], true));
                import = import.Concat(defaultkeybinds.Where(x => !import.ContainsKey(x.Key))).ToDictionary(x => x.Key, x => x.Value);
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
        public void MaximizeScreenBounds()
        {
            if (Fullscreen) {
                this.MaximizedBounds = new Rectangle();
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
            }
            this.WindowState = FormWindowState.Normal;
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
            if (this.WindowState == FormWindowState.Minimized) {
                //if (MainBeeble != null) MainBeeble.Visible = false;
            }
            else {
                //if (MainBeeble != null) MainBeeble.Visible = true;
            }

            if (this.WindowState == FormWindowState.Normal)
                toolstripFormRestore.Image = Properties.Resources.icon_maximize;
        }
        #endregion
        #region Form Key Press
        private void TCLE_KeyDown(object sender, KeyEventArgs e)
        {
            //tab switch next
            if (!e.Shift && e.Control && e.KeyCode == Keys.Tab) {
                if (!ActiveWorkspace.dockMain.Documents.Any())
                    return;
                List<IDockContent> docs = ActiveWorkspace.dockMain.Documents.ToList();
                int docind = docs.IndexOf(ActiveWorkspace.dockMain.ActiveDocument);
                docs[(docind + 1) % docs.Count].DockHandler.Activate();
            }
            //tab switch previous
            else if (e.Shift && e.Control && e.KeyCode == Keys.Tab) {
                if (!ActiveWorkspace.dockMain.Documents.Any())
                    return;
                List<IDockContent> docs = ActiveWorkspace.dockMain.Documents.ToList();
                int docind = docs.IndexOf(ActiveWorkspace.dockMain.ActiveDocument);
                docs[mod(docind - 1, docs.Count)].DockHandler.Activate();
            }
            //move document to next/previous workspace
            else if (e.Alt && e.Control && e.KeyCode is Keys.PageUp or Keys.PageDown) {
                if (GlobalActiveDocument == null || !ActiveWorkspace.dockMain.Documents.Any())
                    return;
                List<IDockContent> docs = DockMain.Documents.ToList();
                //index of next workspace +1 or -1
                int docind = docs.IndexOf(ActiveWorkspace) + (e.KeyCode == Keys.PageUp ? 1 : -1);
                (GlobalActiveDocument as DockContent).Show((docs[mod(docind, docs.Count)] as Form_WorkSpace).dockMain, DockState.Document);
                docs[mod(docind, docs.Count)].DockHandler.Activate();
                docs[mod(docind, docs.Count)].DockHandler.Form.Focus();
            }
            //workspace switch next/previous
            else if (e.Control && e.KeyCode is Keys.PageUp or Keys.PageDown) {
                List<IDockContent> docs = DockMain.Documents.ToList();
                int docind = docs.IndexOf(ActiveWorkspace) + (e.KeyCode == Keys.PageUp ? 1 : -1);
                docs[mod(docind, docs.Count)].DockHandler.Activate();
            }
            //e.Handled = true;
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
            ProjectPropertiesForm customlevel = new();
            customlevel.ShowDialog();
            if (customlevel.DialogResult == DialogResult.Yes)
                OpenProject(customlevel.ProjectToLoad);
            customlevel.Dispose();
        }

        private void toolstripFileOpenProject_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Title = "Open Project";
            ofd.Filter = "Thumper Custom Level (*.TCL)|*.TCL";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = TCLE.WorkingFolder?.FullName ?? Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                FileInfo TCL = new FileInfo(ofd.FileName);
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
                lockedfiles.Add(TCL, new FileStream(TCL.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                ClearFileLock();
            }
            catch (Exception) {
                MessageBox.Show($"That project is open already in another instance of the Level Editor.", "Thumper Custom Level Editor");
                return;
            }
            //if this application already has a TCL loaded, open a new application and pass in the TCL name to load it
            if (WorkingFolder != null) {
                var info = new ProcessStartInfo(Application.ExecutablePath, TCL.FullName);
                Process.Start(info);
                return;
            }
            //load the properties of the TCL and create projectProperties
            dynamic ProjectJson = LoadFileLock(TCL.FullName);
            projectProperties = new() {
                projectname = (string)ProjectJson["level_name"] ?? "New Project",
                difficulty = (string)ProjectJson["difficulty"] ?? "D0",
                description = (string)ProjectJson["description"] ?? "Please add a description",
                authornames = (string)ProjectJson["author"] ?? "a person",
                bpm = (decimal?)ProjectJson["bpm"] ?? 400m,
                WorkingFolder = TCL.Directory,
                TCL = TCL
            };
            //load colors, with failover to White
            try {
                dynamic railcolor = ProjectJson["rails_color"];
                projectProperties.rail = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
                dynamic railglowcolor = ProjectJson["rails_glow_color"];
                projectProperties.railglow = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
                dynamic pathcolor = ProjectJson["path_color"];
                projectProperties.path = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
            }
            catch (Exception) {
                projectProperties.rail = Color.White;
                projectProperties.railglow = Color.White;
                projectProperties.path = Color.White;
            }
            //update some visual elements
            toolstripLevelName.Text = projectProperties.projectname;
            toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject($"difficulty_{projectProperties.difficulty}");
            //add to recent files
            AppSettings.Recentfiles.Remove(TCL.FullName);
            AppSettings.Recentfiles.Insert(0, TCL.FullName);
            JumpListUpdate();
            //load sample of the project
            ReloadProjectSamples();
            panelRecentFiles.Visible = false;

            //create Project Explorer and Project Property panels
            ProjectExplorer = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
            ProjectExplorer.Show(dockMain, DockState.DockRight);
            dockProjectProperties = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
            dockProjectProperties.Show(ProjectExplorer.Pane, DockAlignment.Bottom, 0.3);
            //Load the project''s files into Explorer
            ProjectExplorer.LoadProject();
            dockProjectProperties.LoadProjectProperties();
            //create a workspace
            Form_WorkSpace workspace1 = new() { Text = $"Workspace {Workspaces.Count() + 1}" };
            workspace1.Show(dockMain, DockState.Document);
            OpenFile(ProjectExplorer.projectfiles.FirstOrDefault(x => x.Value.Extension.Equals(".master", StringComparison.OrdinalIgnoreCase)).Value);

            toolstripAddScene.Enabled = true;
            toolstripProject.Enabled = true;
            toolstripEdit.Enabled = true;
            toolstripWindow.Enabled = true;
            toolstripViewExplorer.Enabled = true;
            toolstripViewProperties.Enabled = true;

            dockMain.Panes.First(x => x.DockState == DockState.Document).Resize += DockPanelDocumentArea_Resize;
            dockMain.DefaultFloatWindowSize = dockMain.Panes.First(x => x.DockState == DockState.Document).Size;
        }

        private void toolstripFileConvert_Click(object sender, EventArgs e)
        {
            ConvertProjectToNew();
        }

        private void toolstripFileSaveAs_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.GetType().GetMethod("SaveAs").Invoke(GlobalActiveDocument, new object[] { false });
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
            File.WriteAllText($@"{AppLocation}\templates\singletrack.leaf", Properties.Resources.leaf_singletrack);
            File.WriteAllText($@"{AppLocation}\templates\leaf_multitrack.leaf", Properties.Resources.leaf_multitrack);
            File.WriteAllText($@"{AppLocation}\templates\leaf_multitrack_ring&bar.leaf", Properties.Resources.leaf_multitrack_ring_bar);
            File.WriteAllText($@"{AppLocation}\templates\track_objects2.2.txt", Properties.Resources.track_objects);
            File.WriteAllText($@"{AppLocation}\templates\objects_defaultcolors2.2.txt", Properties.Resources.objects_defaultcolors);
        }

        private void contextMenuRecentProjects_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenuRecentProjects.Items.Clear();
            foreach (string path in AppSettings.Recentfiles) {
                FileInfo tcl = new FileInfo(path);
                ToolStripMenuItem item = new() {
                    Text = $"{tcl.Name} ({tcl.FullName})",
                    ForeColor = Color.White,
                    Image = Properties.Resources.icon_tcle
                };
                contextMenuRecentProjects.Items.Add(item);
            }
        }

        private void contextMenuRecentProjects_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            FileInfo tcl = new(e.ClickedItem.Text.Split('(')[1].TrimEnd(')'));
            OpenProject(tcl);
        }

        private void toolstripFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region Toolstrip Edit
        private void toolstripEditUndo_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditCut_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.GetType().GetMethod("Cut").Invoke(GlobalActiveDocument, null);
        }

        private void toolstripEditCopy_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.GetType().GetMethod("Copy").Invoke(GlobalActiveDocument, null);
        }

        private void toolstripEditPaste_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.GetType().GetMethod("Paste").Invoke(GlobalActiveDocument, null);
        }

        private void toolstripEditDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolstripEditPreferences_Click(object sender, EventArgs e)
        {
            //Show the CustomWorkspace form. If form OK, then save the settings to app properties
            //then call method to recolor the form elements immediately
            CustomizeWorkspace custom = new();
            //custom._objects = _objects;
            if (custom.ShowDialog() == DialogResult.OK) {
                ColorFormElements();
                ImportDefaultColors();
                SetKeyBinds();
                AppSettings.Save();
            }
            custom.Dispose();
        }
        #endregion
        #region Toolstrip View
        private void leafoptionShowCategory_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowCategory = leafoptionShowCategory.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                foreach (Sequencer_Object seq in leaf.leafProperties.seq_objs) {
                    Form_LeafEditor.ChangeTrackName(seq, Properties.Settings.Default.LeafOptionShowCategory ? $"[{seq.category}] " : "");
                }
            }
        }

        private void leafoptionShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowGrid = leafoptionShowGrid.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                leaf.trackEditor.Refresh();
            }
        }

        private void leafoptionConnectBars_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionConnectBars = leafoptionConnectBars.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                leaf.trackEditor.Refresh();
            }
        }

        private void leafoptionShowLanes_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionShowLane = leafoptionShowLanes.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                if (Properties.Settings.Default.LeafOptionShowLane) {
                    foreach (Sequencer_Object seq in leaf.leafProperties.seq_objs) {
                        seq.expandlanes = true;
                    }
                    leaf.trackEditor.Invalidate();
                }
                else
                    leaf.trackEditor.InvalidateColumn(2);
            }
        }

        private void leafoptionEaseDots_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionEaseDots = leafoptionEaseDots.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                leaf.trackEditor.Refresh();
            }
        }

        private void leafoptionThinValues_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LeafOptionThinBars = leafoptionThinValues.Checked;
            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                leaf.trackEditor.Refresh();
            }
        }

        private void toolstripViewExplorer_Click(object sender, EventArgs e)
        {
            if (ProjectExplorer.IsDisposed) {
                ProjectExplorer = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
            }
            ProjectExplorer.Show(dockMain, DockState.DockRight);
        }

        private void toolstripViewProperties_Click(object sender, EventArgs e)
        {
            if (dockProjectProperties.IsDisposed) {
                dockProjectProperties = new() { DockAreas = DockAreas.DockRight | DockAreas.DockLeft };
                dockProjectProperties.Show(ProjectExplorer.Pane, DockAlignment.Bottom, 0.3);
                dockProjectProperties.LoadProjectProperties();
            }
            else
                dockProjectProperties.Show(ProjectExplorer.Pane, DockAlignment.Bottom, 0.3);
        }

        private void toolstripViewFullscreen_Click(object sender, EventArgs e)
        {
            Fullscreen = true;
            MaximizeScreenBounds();
            toolstripExitFullscreen.Visible = true;
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
            if (GlobalActiveDocument.DockHandler.DockState != DockState.Float)
                GlobalActiveDocument.DockHandler.DockState = DockState.Float;
            //ActiveWorkspace.dockMain.ActiveDocument.DockHandler.DockState = DockState.Float;
        }
        private void toolstripWindowFloatAll_Click(object sender, EventArgs e)
        {
            foreach (IDockContent dc in ActiveWorkspace.dockMain.Documents) {
                dc.DockHandler.DockState = DockState.Float;
            }
        }
        private void toolstripWindowDock_Click(object sender, EventArgs e) => dockMain.ActiveDocument.DockHandler.DockState = DockState.Document;

        private void toolstripWindowCloseAll_Click(object sender, EventArgs e)
        {
            while (dockMain.Documents.Any())
                dockMain.Documents.First().DockHandler.Dispose();
        }

        private void toolstripWindowCloseEditors_Click(object sender, EventArgs e)
        {
            Form_WorkSpace fws = dockMain.ActiveDocument as Form_WorkSpace;
            while (fws.dockMain.Documents.Any())
                fws.dockMain.Documents.First().DockHandler.Dispose();
        }

        private void addNewWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_WorkSpace workspace1 = new() { Text = $"Workspace {Workspaces.Count() + 1}", DockAreas = DockAreas.Document };
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
            Form_WorkSpace workspace = Workspaces.First(x => x.DockHandler.TabText == e.ClickedItem.Text) as Form_WorkSpace;
            (GlobalActiveDocument as DockContent).Show(workspace.dockMain, DockState.Document);
        }
        #endregion
        #region Toolstrip Help
        private void toolstripHelpTentacles_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1dGkU9uqlr3Hp2oJiVFMHHpIKt8S_c0Vi27n47ZRD0_0", UseShellExecute = true });
        private void toolstripHelpObjects_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1JWk7TDn4ZuitclB-x7gOYxU-PsmGkooZuU9QEd_aw1A", UseShellExecute = true });
        private void toolstripHelpAudio_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q", UseShellExecute = true });
        private void toolstripHelpAbout_Click(object sender, EventArgs e) => new AboutThumperEditor().Show();
        private void toolstripHelpDiscord_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://discord.com/invite/gTQbquY", UseShellExecute = true });
        private void toolstripHelpGithub_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://github.com/CocoaMix86/Thumper-Custom-Level-Editor", UseShellExecute = true });
        private void toolstripHelpChangelog_Click(object sender, EventArgs e) => ShowChangelog();
        private void toolstripHelpKofi_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/I2I5ZZBRH", UseShellExecute = true });
        #endregion
        #region Toolstrip Project
        private void toolstripProjectLeaf_Click(object sender, EventArgs e)
        {
            OpenFile(new Form_LeafEditor().SaveAs(true));
        }

        private void toolstripProjectLvl_Click(object sender, EventArgs e)
        {
            OpenFile(new Form_LvlEditor().SaveAs(true));
        }

        private void toolstripProjectGate_Click(object sender, EventArgs e)
        {
            OpenFile(new Form_GateEditor().SaveAs(true));
        }

        private void toolstripProjectMaster_Click(object sender, EventArgs e)
        {
            OpenFile(new Form_MasterEditor().SaveAs(true));
        }

        private void toolstripProjectSample_Click(object sender, EventArgs e)
        {
            OpenFile(new Form_SampleEditor().SaveAs(true));
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

                TCLE.ProjectExplorer.CreateTreeView();
                OpenFile(projectfile);
            }
        }

        private void toolstripProjectRegen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will overwrite the \"default\" files in the working folder. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                File.WriteAllText($@"{WorkingFolder}\spn_default.txt", Properties.Resources.spn_default);
                File.WriteAllText($@"{WorkingFolder}\xfm_default.txt", Properties.Resources.xfm_default);
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
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level1_320bpm.samp"), toolstripSampLevel1.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level2_340bpm.samp"), toolstripSampLevel2.Checked, Properties.Resources.samp_level2_340bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level3_360bpm.samp"), toolstripSampLevel3.Checked, Properties.Resources.samp_level3_360bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level4_380bpm.samp"), toolstripSampLevel4.Checked, Properties.Resources.samp_level4_380bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level5_400bpm.samp"), toolstripSampLevel5.Checked, Properties.Resources.samp_level5_400bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level6_420bpm.samp"), toolstripSampLevel6.Checked, Properties.Resources.samp_level6_420bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level7_440bpm.samp"), toolstripSampLevel7.Checked, Properties.Resources.samp_level7_440bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level8_460bpm.samp"), toolstripSampLevel8.Checked, Properties.Resources.samp_level8_460bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\level9_480bpm.samp"), toolstripSampLevel9.Checked, Properties.Resources.samp_level9_480bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\dissonant.samp"), toolstripSampLevelDiss.Checked, Properties.Resources.samp_dissonant),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\globaldrones.samp"), toolstripSampLevelDrones.Checked, Properties.Resources.samp_globaldrones),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\rests.samp"), toolstripSampLevelRests.Checked, Properties.Resources.samp_rests),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{WorkingFolder}\misc.samp"), toolstripSampLevelMisc.Checked, Properties.Resources.samp_misc)
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
                        filesupdates = true;
                    }
                }
                else {
                    if (files.Any(x => x.Name == pack.Item1.Name)) {
                        TCLE.DeleteFileLock(files.First(x => x.Name == pack.Item1.Name));
                        filesupdates = true;
                    }
                }
            }

            if (filesupdates)
                ProjectExplorer.CreateTreeView();
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
            foreach (SampleData samp in ProjectSamples) {
                PCtoAudioFile(samp);
            }
        }
        #endregion

        #region Toolstrip Toolbar
        private void toolstripMainSave_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.GetType().GetMethod("Save").Invoke(GlobalActiveDocument, new object[] { true });
            //FindEditorRunMethod(GlobalActiveDocument.GetType(), "Save");
        }

        private void toolstripMainSaveAll_Click(object sender, EventArgs e)
        {
            foreach (Form_WorkSpace workspace in Workspaces) {
                foreach (IDockContent document in workspace.dockMain.Documents) {
                    document.GetType().GetMethod("Save").Invoke(document, new object[] { false });
                    //FindEditorRunMethod(document.GetType(), "Save");
                }
            }
            TCLE.PlaySound("UIsave");
        }
        #endregion

        #region DockPanel
        private void DockPanelDocumentArea_Resize(object sender, EventArgs e) => dockMain.DefaultFloatWindowSize = dockMain.Panes.First(x => x.DockState == DockState.Document).Size;

        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (dockMain.ActiveDocument == null)
                return;
            if (dockMain.ActiveDocument.DockHandler.TabText is not "Project Explorer" and not "Project Properties")
                ActiveWorkspace = dockMain.ActiveDocument as Form_WorkSpace;
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
        #endregion
        #region Dock Tab Rightclick
        public static IDockContent GlobalActiveDocument;
        private void contextmenuTabClick_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolstripTabSave.Text = "Save " + GlobalActiveDocument.DockHandler.TabText;
        }

        private void toolstripTabClose_Click(object sender, EventArgs e)
        {
            GlobalActiveDocument.DockHandler.Dispose();
        }

        private void toolstripTabCloseOther_Click(object sender, EventArgs e)
        {
            Form_WorkSpace fws = dockMain.ActiveDocument as Form_WorkSpace;
            foreach (IDockContent document in fws.dockMain.Documents.ToList()) {
                if (document != GlobalActiveDocument)
                    document.DockHandler.Dispose();
            }
        }

        private void toolstripTabCopyPath_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ProjectExplorer.projectfiles.First(x => x.Key.EndsWith($@"\{GlobalActiveDocument.DockHandler.TabText}")).Value.FullName);
        }

        private void toolstripTabOpenFolder_Click(object sender, EventArgs e)
        {
            FileInfo foldertoopen = ProjectExplorer.projectfiles.First(x => x.Key.EndsWith($@"\{GlobalActiveDocument.DockHandler.TabText}")).Value;
            if (foldertoopen.Directory.Exists)
                Process.Start("explorer.exe", $@"/select, ""{foldertoopen.FullName}""");
        }
        #endregion

        private void toolstripAddScene_Click(object sender, EventArgs e)
        {
            Form_DrawScene draw = new Form_DrawScene();
            draw.Show(dockMain, DockState.Document);
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            /*
            foreach (FileInfo file in new DirectoryInfo($@"{Properties.Settings.Default.game_dir}\cache").GetFiles()) {
                byte[] _bytes;
                //read the .pc file as bytes, and skip the first 4 header bytes
                _bytes = File.ReadAllBytes(file.FullName);
                _bytes = _bytes.Skip(4).ToArray();

                // credit to https://github.com/SamboyCoding/Fmod5Sharp
                try {
                    
                    FmodSoundBank bank = FsbLoader.LoadFsbFromByteArray(_bytes);
                    List<FmodSample> samples = bank.Samples;
                    samples[0].RebuildAsStandardFileFormat(out byte[] dataBytes, out string fileExtension);

                    string audioname = "";
                    for (int x = 0; x < _bytes.Length; x++) {
                        if (_bytes[x] == 0x4 && _bytes[x + 1] == 0x0 && _bytes[x + 2] == 0x0 && _bytes[x + 3] == 0x0) {
                            for (int _in = x + 4; _in < 500; _in++) {
                                if (_bytes[_in] != 0x0)
                                    audioname += (char)_bytes[_in];
                                else
                                    break;
                            }
                            break;
                        }
                    }
                    if (_bytes[0] == 'D' && _bytes[1] == 'D' && _bytes[2] == 'S')
                        File.WriteAllBytes($@"temp\{file.Name}.dds", _bytes);
                } catch (Exception ex) {
                    continue;
                }
            }
            */
        }
    }
}
