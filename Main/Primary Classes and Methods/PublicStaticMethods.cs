using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Other_Forms;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        #region Variable
        //Static
        public static string VersionNumber = "3.0.0-a71";
        public static decimal[] LeafQuickValues = new[] { 1.000m, 1.000m, 1.000m, 1.000m, 1.000m, 1.000m, 1.000m, 1.000m, 1.000m, 1.000m };
        public static List<string> LvlPaths = Properties.Resources.paths.Replace("\r\n", "\n").Split('\n').ToList();
        public static List<string> LvlsInProject = new();
        public static Dictionary<string, DefaultSequencerObject> LeafObjects = new();
        public static Dictionary<string, Bitmap> ColorIcons = new();
        //public static List<SampleData> ProjectSamples = new();
        public static Dictionary<string, SampleData> ProjectSamples = new();
        public static ToolStripOverride LeafToolStripOverride = new();
        public static ContextMenuColors LeafContextMenuColors = new();
        //Static Readonly
        public static readonly List<string> TimeSignatures = new() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
        public static readonly Dictionary<string, string> TrackLaneFriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" }, { "none", "none" } };
        public static readonly Dictionary<string, int> LaneOffsets = new() { ["a01"] = 0, ["a02"] = -1, ["ent"] = -2, ["z01"] = -3, ["z02"] = -4 };
        public static readonly Dictionary<string, string> Easings = new() { { "kEaseInOut", "Ease In Out" }, { "kEaseIn", "Ease In" }, { "kEaseOut", "Ease Out" } };
        public static readonly string[] ImageExtensions = new string[] { ".png", ".jpeg", ".jpg", ".gif", ".webp", ".bmp" };
        public static readonly string[] ProjectExtensions = new string[] { ".leaf", ".lvl", ".gate", ".master", ".samp" };
        //
        #endregion

        private static readonly PropertyInfo? DoubleBufferedProperty = typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void DoubleBufferDGV(DataGridView grid)
        {
            if (SystemInformation.TerminalServerSession)
                return;
            DoubleBufferedProperty?.SetValue(grid, true);
        }

        public static void UpdateLoadingMessage()
        {
            TCLE.Instance.lblLoadingLeaf.Invalidate();
            TCLE.Instance.lblLoadingLeaf.Update();
            TCLE.Instance.lblLoadingLeaf.Refresh();
            Application.DoEvents();
        }

        ///Color elements based on set properties
        public static void ColorFormElements(TCLE MainForm)
        {
            EditorLeaf.CellBackColorCache.TryAdd(Properties.Settings.Default.ColorLeafTimeSig1, new(Properties.Settings.Default.ColorLeafTimeSig1));
            EditorLeaf.CellBackColorCache.TryAdd(Properties.Settings.Default.ColorLeafTimeSig2, new(Properties.Settings.Default.ColorLeafTimeSig2));

            MainForm.toolStripTitle.BackColor = Properties.Settings.Default.ColorMainMenuBar;
            MainForm.panelToolStrips.BackColor = Properties.Settings.Default.ColorMainSubMenubar;
            MainForm.dockMain.BackColor = Properties.Settings.Default.ColorMainBG;

            TCLE.Explorer?.ColorFormElements();

            foreach (EditorBase editor in TCLE.Documents.Values)
                editor.ColorFormElements();
        }

        public void MenusVisible(bool visible)
        {
            panelRecentFiles.Visible = !visible;
            panelIntroTips.Visible = !visible;
            if (WorkingFolder == null)
                visible = false;
            panelToolStrips.Visible = visible;
            toolStripTitle.SendToBack();
            dockMain.Visible = visible;
            foreach (object? item in toolStripTitle.Items)
                (item as ToolStripItem).Visible = visible;
            toolstripFile.Visible = true;
            toolstripEdit.Visible = true;
            toolstripHelp.Visible = true;
            toolstripFormClose.Visible = true;
            toolstripFormMinimize.Visible = true;
            toolstripFormRestore.Visible = true;
            toolstripFormIcon.Visible = true;
            toolstripExitFullscreen.Visible = TCLE.Fullscreen;
            lblRuntime.Font = TCLE.RuntimeLabelFont;

            toolstripProject.Enabled = true;
            toolstripEdit.Enabled = true;
            toolstripWindow.Enabled = true;
            toolstripViewExplorer.Enabled = true;
            toolstripViewProperties.Enabled = true;

            MainBeeble.Visible = visible;
            if (MainBeeble.Visible) {
                MainBeeble.Size = Properties.Settings.Default.beeblesize;
                MainBeeble.Location = Properties.Settings.Default.beebleloc;
            }
        }

        public void InitializeUI()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            dockMain.Theme = DockTheme;
            TabRightClickMenu = contextmenuTabClick;
            MainBeeble.Owner = this;
            DragDropItems.Owner = this;
            MenusVisible(false);
            //set custom renderer
            toolStripTitle.Renderer = new ToolStripMainForm();
            toolStripMain.Renderer = LeafToolStripOverride;
            contextmenuFile.Renderer = LeafContextMenuColors;
            contextmenuEdit.Renderer = LeafContextMenuColors;
            contextmenuView.Renderer = LeafContextMenuColors;
            contextMenuProject.Renderer = LeafContextMenuColors;
            contextmenuWindow.Renderer = LeafContextMenuColors;
            contextmenuHelp.Renderer = LeafContextMenuColors;
            contextmenuTabClick.Renderer = LeafContextMenuColors;
            contextmenuSampPacks.Renderer = LeafContextMenuColors;
            contextmenuMoveWorkspace.Renderer = LeafContextMenuColors;
            contextMenuRecentProjects.Renderer = LeafContextMenuColors;
            //set check states from saved settings
            leafoptionShowCategory.Checked = Properties.Settings.Default.LeafOptionShowCategory;
            leafoptionShowGrid.Checked = Properties.Settings.Default.LeafOptionShowGrid;
            leafoptionConnectBars.Checked = Properties.Settings.Default.LeafOptionConnectBars;
            leafoptionShowLanes.Checked = Properties.Settings.Default.LeafOptionShowLane;
            leafoptionEaseDots.Checked = Properties.Settings.Default.LeafOptionEaseDots;
            leafoptionThinValues.Checked = Properties.Settings.Default.LeafOptionThinBars;
            leafoptionShowWave.Checked = Properties.Settings.Default.LeafOptionShowWave;
            leafoptionVerticalCells.Checked = Properties.Settings.Default.LeafOptionVerticalCells;
            leafoptionPlaybackScroll.Checked = Properties.Settings.Default.LeafOptionPlaybackScroll;
            leafoptionScaleHeader.Checked = Properties.Settings.Default.LeafOptionScaleHeader;
        }

        public static void InitializeFolders()
        {
            try {
                if (Properties.Settings.Default.version != TCLE.VersionNumber) {
                    Properties.Settings.Default.version = TCLE.VersionNumber;
                    //if (UtilPaths.DirTemp.Exists)
                    //    UtilPaths.DirTemp.Delete(true);
                    //if (UtilPaths.DirSettings.Exists)
                    //    UtilPaths.DirSettings.Delete(true);
                }
                //Create directory for leaf templates and other default files
                if (!UtilPaths.DirTemplates.Exists) {
                    InitializeTemplateFiles();
                }
                if (!UtilPaths.DirTemp.Exists) {
                    UtilPaths.DirTemp.Create();
                }
                if (!UtilPaths.DirSettings.Exists) {
                    UtilPaths.DirSettings.Create();
                    File.WriteAllText($@"{UtilPaths.Settings}\track_objects_v4.txt", Properties.Resources.trackobjects_v4);
                    File.WriteAllText($@"{UtilPaths.Settings}\objects_defaultcolors_v3.txt", Properties.Resources.objects_defaultcolors);
                }
                //load fonts
                if (!File.Exists($@"{UtilPaths.Temp}\JetBrainsMono_Medium.ttf"))
                    File.WriteAllBytes($@"{UtilPaths.Temp}\JetBrainsMono_Medium.ttf", Properties.Resources.JetBrainsMono_Medium);
                ImportedFonts.AddFontFile($@"{UtilPaths.Temp}\JetBrainsMono_Medium.ttf");
                RuntimeLabelFont = new(TCLE.ImportedFonts.Families[0], 12);
            } catch (Exception ex) {
                MessageBox.Show($"An error occurred during app load section 1. Please show this to CocoaMix\n\n{ex}", "Thumper Custom Level Editor");
            }
        }

        public static void InitializeTemplateFiles()
        {
            if (!UtilPaths.DirTemplates.Exists)
                UtilPaths.DirTemplates.Create();
            //write out default templates and settings files
            //File.WriteAllText($@"{UtilPaths.Templates}\singletrack.leaf", Properties.Resources.leaf_singletrack);
            //File.WriteAllText($@"{UtilPaths.Templates}\leaf_multitrack.leaf", Properties.Resources.leaf_multitrack);
            //File.WriteAllText($@"{UtilPaths.Templates}\leaf_multitrack_ring&bar.leaf", Properties.Resources.leaf_multitrack_ring_bar);
        }

        public static void ResizeHeaders(DataGridView dgv)
        {
            int biggestheader = 50;
            //foreach (Sequencer_Object seq in SequencerObjects) {
            foreach (DataGridViewRow dgvr in dgv.Rows) {
                //measure header and see if it's the biggest
                int tempsize = TextRenderer.MeasureText(dgvr.HeaderCell.Value?.ToString(), dgvr.HeaderCell.Style.Font).Width;
                if (tempsize > biggestheader)
                    biggestheader = tempsize;
            }
            //set header width manually and allow resizing
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgv.RowHeadersWidth = biggestheader + 15;
        }

        public static void ReloadProjectSamples()
        {
            if (WorkingFolder == null)
                return;
            ProjectSamples.Clear();
            //add default empty sample
            ProjectSamples.Add("", new SampleData { ObjName = "", Path = "", Volume = 0, Pitch = 0, Pan = 0, Offset = 0, ChannelGroup = "", File = null });
            string warning = "";
            //iterate over each file
            foreach (FileInfo sampfile in WorkingFolder.GetFiles("*.samp", SearchOption.AllDirectories).Where(x => x.Name != "?!?!default?!?!?!?.samp")) {
                UpdateProjectSamplesFromFile(sampfile, false, false, out string _warning);
                warning += _warning;
            }
            if (warning.Length > 2)
                MessageBox.Show($"Your sample files contain duplicate entries. These can break your level, and it is advised to rename 1 or both of them.\n\n{warning}", "Thumper Custom Level Editor");
            ProjectSamples = ProjectSamples.OrderBy(w => w.Value.ObjName).ToDictionary();
            /*
            if (Properties.Settings.Default.RuntimeAsk) {
                CheckboxDialog Ask = new();
                if (Ask.ShowDialog() == DialogResult.Yes) {
                    Properties.Settings.Default.RuntimeSkip = false;
                    Properties.Settings.Default.RuntimeAsk = !Ask.checkAsk.Checked;
                }
                else {
                    Properties.Settings.Default.RuntimeSkip = true;
                    Properties.Settings.Default.RuntimeAsk = !Ask.checkAsk.Checked;
                }
            }
            if (!Properties.Settings.Default.RuntimeSkip) {
                UtilAudio.CalculateSampleRuntimes();
                UtilAudio.StopAudio();
            }*/

            UpdateEditorsWithSamples();
            //File.WriteAllLines($@"{AppLocation}\templates\{TCLE.WorkingFolder.Name}_sample_runtimes.temp", ProjectSamples.Select(x => $"{x.obj_name};{x.time}"));
        }

        public static void UpdateProjectSamplesFromFile(FileInfo SampFile, bool preserveSamples, bool updateeditors, out string warning)
        {
            //remove samples that match the incoming sample file, so that they're rewritten
            foreach (KeyValuePair<string, SampleData> _samp in ProjectSamples.Where(x => x.Value.File?.FullName == SampFile.FullName).ToList())
                ProjectSamples.Remove(_samp.Key);
            //ProjectSamples.RemoveAll(x => x.File?.FullName == SampFile.FullName);
            //parse file to JSON
            dynamic _in = UtilFile.LoadFileLock(SampFile);
            warning = "";
            //skip if somehow empty
            if (_in == null || !_in.ContainsKey("items"))
                return;
            //iterate over items:[] list to get each sample and add names to list
            foreach (dynamic _samp in _in["items"]) {
                //if (ProjectSamples.Any(x => x.Value.obj_name == (string)_samp["obj_name"])) {
                if (ProjectSamples.ContainsKey((string)_samp["obj_name"])) {
                    if (!preserveSamples)
                        warning += $"{_samp["obj_name"]} in {SampFile.FullName}\n{_samp["obj_name"]} in {ProjectSamples[(string)_samp["obj_name"]].File.FullName}\n";
                    else
                        continue;
                }
                ProjectSamples.TryAdd((string)_samp["obj_name"], new SampleData {
                    ObjName = ((string)_samp["obj_name"]),
                    Path = _samp["path"],
                    Volume = _samp["volume"],
                    Pitch = _samp["pitch"],
                    Pan = _samp["pan"],
                    Offset = _samp["offset"],
                    ChannelGroup = _samp["channel_group"],
                    File = SampFile,
                    Runtime = 0
                });
            }

            if (updateeditors)
                UpdateEditorsWithSamples();
        }

        public static void RemoveProjectSamples(FileInfo SampFile)
        {
            //TCLE.ProjectSamples.RemoveAll(x => x.File?.FullName == SampFile.FullName);
            //remove samples that match the incoming sample file, so that they're rewritten
            foreach (KeyValuePair<string, SampleData> _samp in ProjectSamples.Where(x => x.Value.File?.FullName == SampFile.FullName).ToList())
                ProjectSamples.Remove(_samp.Key);
            UpdateEditorsWithSamples();
        }

        public static void UpdateEditorsWithSamples()
        {
            SeqObjTreeBuilder.BuildSampleNodes();
            //var _samples = TCLE.ProjectSamples.Select(x => x.obj_name).ToList();

            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>()) {
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.txtSearch.Text);
            }
            foreach (EditorLvl lvl in TCLE.Documents.Values.OfType<EditorLvl>()) {
                //load loop track names and paths to lvlLoopTracks DGV
                ((DataGridViewComboBoxColumn)lvl.lvlLoopTracks.Columns[1]).DataSource = new BindingSource(TCLE.ProjectSamples, null);
                ((DataGridViewComboBoxColumn)lvl.lvlLoopTracks.Columns[1]).DisplayMember = "Key";
                ((DataGridViewComboBoxColumn)lvl.lvlLoopTracks.Columns[1]).ValueMember = "Value";
            }
        }

        //check if at least 1 master file exists
        public static bool CheckForMaster()
        {
            return ProjectExplorer.Files.Keys.Any(x => string.Equals(x, ".master", StringComparison.OrdinalIgnoreCase));
        }

        public static EditorBase OpenFile(FileInfo filepath, bool openraw = false, bool ReturnContent = false)
        {
            if (filepath == null)
                return null;
            //if item is an image, open in image viewer instead of a DockContent
            if (TryOpenImage(filepath))
                return null;
            //if item is not an editor type, open raw
            if (!ProjectExtensions.Contains(filepath.Extension, StringComparer.OrdinalIgnoreCase)) {
                openraw = true;
            }

            //object _load = openraw ? UtilFile.LoadFileLockRaw(filepath) : UtilFile.LoadFileLock(filepath);
            if (ProjectExplorer.TryGetFile(filepath.Name, out ProjectItem Item))
                return null;
            JObject _load = Item.Load();
            if (_load == null)
                return null;
            //enter this block if we're not returning the form
            if (!ReturnContent) {
                //if there are no workspaces, add one
                if (!Workspaces.Any()) {
                    DockWorkspace workspace1 = new($"Workspace {Workspaces.Count() + 1}") { DockAreas = DockAreas.Document };
                    workspace1.Show(TCLE.Instance.dockMain, DockState.Document);
                }
                if (LocateTab(filepath, openraw))
                    return null;

                //open document in raw viewer if that option was selected
                if (openraw || !ProjectExtensions.Contains(filepath.Extension, StringComparer.OrdinalIgnoreCase)) {
                    EditorRawText rawtext = new(_load.ToString(), filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                    rawtext.Show(ActiveWorkspace.dockMain, DockState.Document);
                    return null;
                }
            }

            DockContent OpenFile = new() { DockAreas = DockAreas.Document | DockAreas.Float };
            if (openraw) {
                OpenFile = new EditorRawText(_load.ToString(), filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
            }
            else {
                switch (filepath.Extension) {
                    case ".master":
                        OpenFile = new EditorMaster(_load, filepath);
                        break;
                    case ".lvl":
                        OpenFile = new EditorLvl(_load, filepath, Playback.Generating);
                        break;
                    case ".gate":
                        OpenFile = new EditorGate(_load, filepath, Playback.Generating);
                        break;
                    case ".leaf":
                        OpenFile = new EditorLeaf(_load, filepath, Playback.Generating);
                        break;
                    case ".samp":
                        OpenFile = new EditorSample(_load, filepath);
                        break;
                }
            }

            TCLE.Instance.toolStripWindowCloseTab.Enabled = true;
            TCLE.Instance.toolstripWindowCloseEditors.Enabled = true;
            TCLE.Instance.toolStripMenuItem7.Enabled = true;
            TCLE.Instance.toolstripWindowCloseFiletype.Enabled = true;
            TCLE.Instance.toolstripWindowFloat.Enabled = true;
            TCLE.Instance.toolstripWindowFloatAll.Enabled = true;
            TCLE.Instance.toolstripWindowDock.Enabled = true;
            if (ReturnContent)
                return (EditorBase)OpenFile;
            //this finds a pane in the active workspace that has matching extensions already open on it
            DockPane OpenHere = ActiveWorkspace.dockMain.Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(filepath.Extension)).Any());
            if (OpenHere != null) 
                OpenFile.Show(OpenHere, null);
            else 
                OpenFile.Show(ActiveWorkspace.dockMain, DockState.Document);

            return null;
        }

        public static bool TryOpenImage(FileInfo file)
        {
            if (!ImageExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
                return false;

            Image theimage = null;
            using (FileStream fs = new(file.FullName, FileMode.Open)) {
                theimage = Image.FromStream(fs);
            }
            new ImageViewer(theimage) { Text = file.Name }.Show();
            
            return true;
        }

        public static bool LocateTab(FileInfo filepath, bool openraw)
        {
            if (TCLE.Documents.TryGetValue($"{filepath.Name}{(openraw ? "-raw" : "")}", out EditorBase tab)) {
                tab.DockHandler.Activate();
                return true;
            }
            return false;
        }

        public static void CloseFile(FileInfo filepath)
        {
            TCLE.Documents.TryGetValue(filepath.Name, out EditorBase _close);
            _close?.Close();
            _close?.Dispose();
            TCLE.Documents.TryGetValue(filepath.Name + "-raw", out _close);
            _close?.Close();
            _close?.Dispose();
        }

        public static void ReloadLvlsInProject()
        {
            if (WorkingFolder == null)
                return;
            LvlsInProject = ProjectExplorer.GetFilesByExtension(".lvl").Select(x => x.File.Name).ToList();
            LvlsInProject.Add("<none>");
            LvlsInProject.Sort();
        }

        public static void FindReloadRaw(string documentname)
        {
            //find if any raw text docs matching documentname are open and update them
            TCLE.Documents.TryGetValue(documentname + "-raw", out EditorBase _found);
            (_found as EditorRawText)?.Reload();
        }

        private static void SwitchTab(int direction)
        {
            if (!ActiveWorkspace.dockMain.Documents.Any())
                return;

            List<IDockContent> docs = ActiveWorkspace.dockMain.Documents.ToList();
            int index = docs.IndexOf(ActiveWorkspace.dockMain.ActiveDocument);

            docs[UtilMath.mod(index + direction, docs.Count)].DockHandler.Activate();
        }

        private static void MoveTabToWorkspace(int direction)
        {
            if (GlobalActiveDocument == null || !ActiveWorkspace.dockMain.Documents.Any())
                return;
            List<IDockContent> docs = DockMain.Documents.ToList();
            //index of next workspace +1 or -1
            int docind = docs.IndexOf(ActiveWorkspace) + direction;
            (GlobalActiveDocument as DockContent).Show((docs[UtilMath.mod(docind, docs.Count)] as DockWorkspace).dockMain, DockState.Document);
            docs[UtilMath.mod(docind, docs.Count)].DockHandler.Activate();
            docs[UtilMath.mod(docind, docs.Count)].DockHandler.Form.Focus();
        }

        private static void SwitchWorkspace(int direction)
        {
            List<IDockContent> docs = DockMain.Documents.ToList();
            int docind = docs.IndexOf(ActiveWorkspace) + direction;
            docs[UtilMath.mod(docind, docs.Count)].DockHandler.Activate();
        }

        public static void RefreshLeafEditors(bool ExpandLanes = false)
        {
            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>()) {
                if (ExpandLanes && Properties.Settings.Default.LeafOptionShowLane) {
                    foreach (Sequencer_Object seq in leaf.LeafProperties.SequencerObjects) {
                        seq.ExpandLanesInEditor = true;
                    }
                }
                leaf.trackEditor.Invalidate();
                leaf.dgvMasterView.Invalidate();
            }
        }

        public static bool AnyUnsaved(DockWorkspace work = null, Type type = null)
        {
            IEnumerable<EditorBase> _documentsToCheck;
            //if closing 1 specific workspace, compile the documents in it
            if (work != null) {
                _documentsToCheck = work.dockMain.Documents.Cast<EditorBase>();
                //if closing 1 specific file type, filter the documents to just that type.
                if (type != null)
                    _documentsToCheck = _documentsToCheck.Where(d => d.GetType() == type);
            }
            //otherwise compile all documents currently open
            else {
                _documentsToCheck = TCLE.Documents.Values;
            }
            //returns true if ANY is NOT saved
            return _documentsToCheck.Any(d => !d.Saved);
        }

        public void ConvertProjectToNew()
        {
            FileInfo LevelDetails;
            using OpenFileDialog ofd = new();
            ofd.Title = "Find a LEVEL DETAILS.txt file";
            ofd.Filter = "LEVEL DETAILS.txt|LEVEL DETAILS.txt";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = Application.StartupPath;
            if (ofd.ShowDialog() == DialogResult.OK) {
                LevelDetails = new FileInfo(ofd.FileName);
                if (!LevelDetails.Name.Equals("LEVEL DETAILS.TXT", StringComparison.OrdinalIgnoreCase)) {
                    MessageBox.Show("That's not the level details file");
                    return;
                }
            }
            else
                return;

            if (MessageBox.Show("This will convert the project to the new TCLE 3.0 format. This change CANNOT be undone.\nPlease make a backup of your project before continuing.", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            int countleaf = LevelDetails.Directory.GetFiles("leaf_*.txt", SearchOption.AllDirectories).Length;
            int countlvl = LevelDetails.Directory.GetFiles("lvl_*.txt", SearchOption.AllDirectories).Length;
            int countgate = LevelDetails.Directory.GetFiles("gate_*.txt", SearchOption.AllDirectories).Length;
            int countsamp = LevelDetails.Directory.GetFiles("samp_*.txt", SearchOption.AllDirectories).Length;
            int countmaster = LevelDetails.Directory.GetFiles("master_*.txt", SearchOption.AllDirectories).Length;
            bool sort = MessageBox.Show($"Sort files into subfolders?\n{countleaf} leaf files\n{countlvl} lvl files\n{countgate} gate files\n{countsamp} samp files\n{countmaster} master files", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.Yes;

            //load the properties of the TCL and create projectProperties
            dynamic ProjectJson = UtilFile.LoadFileLock(LevelDetails);
            dynamic ProjectConfig = UtilFile.LoadFileLock(LevelDetails.Directory.GetFiles("config_*.txt").FirstOrDefault());
            ProjectProperties Convert = new() {
                ProjectName = (string)ProjectJson["level_name"] ?? "New Project",
                Difficulty = (string)ProjectJson["difficulty"] ?? "D0",
                Description = (string)ProjectJson["description"] ?? "Please add a description",
                AuthorNames = (string)ProjectJson["author"] ?? "a person",
                BPM = (decimal?)ProjectConfig["bpm"] ?? 400m
            };
            //load colors, with failover to White
            try {
                Convert.BPM = (decimal?)ProjectConfig["bpm"] ?? 400m;
                dynamic railcolor = ProjectConfig["rails_color"];
                Convert.RailColor = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
                dynamic railglowcolor = ProjectConfig["rails_glow_color"];
                Convert.RailGlowColor = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
                dynamic pathcolor = ProjectConfig["path_color"];
                Convert.PathColor = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
            } catch (Exception) {
                Convert.RailColor = Color.White;
                Convert.RailGlowColor = Color.White;
                Convert.PathColor = Color.White;
            }

            foreach (FileInfo file in LevelDetails.Directory.GetFiles("*", SearchOption.AllDirectories)) {
                if (file.Name.Equals("LEVEL DETAILS.TXT", StringComparison.OrdinalIgnoreCase) || file.Name.StartsWith("config_", StringComparison.OrdinalIgnoreCase)) {
                    file.Delete();
                    continue;
                }
                else if (file.Directory.Name.ToLower() is "extras")
                    continue;
                if (file.Extension == ".pc" && file.Directory.Name != "extras") {
                    if (!Directory.Exists($@"{file.DirectoryName}\extras"))
                        Directory.CreateDirectory($@"{file.DirectoryName}\extras");
                    file.MoveTo($@"{file.DirectoryName}\extras\{file.Name}");
                }
                string[] splitextension = file.Name.Replace(".txt", "").Split('_', 2);
                if (sort) {
                    try {
                        Directory.CreateDirectory($@"{file.DirectoryName}\{splitextension[0]}");
                    } catch { continue; }
                }

                FileInfo newfile = new($@"{file.DirectoryName}\{(sort ? splitextension[0] + "\\" : "")}{splitextension[1]}.{splitextension[0].ToLower()}");
                File.Move(file.FullName, newfile.FullName);
                //show the loading message
                TCLE.Instance.panelLoadingMessage.Visible = true;
                TCLE.Instance.lblLoadingLeaf.Text = $"Processing: {newfile.Name}";
                TCLE.Instance.lblLoadingLeaf.Invalidate();
                TCLE.Instance.lblLoadingLeaf.Update();
                TCLE.Instance.lblLoadingLeaf.Refresh();
                Application.DoEvents();
                //
                //resave leafs and lvls to properly convert the datapoints
                JObject _save = null;
                if (newfile.Extension == ".leaf") {
                    dynamic _load = UtilFile.LoadFileLock(newfile);
                    EditorLeaf _leaf = new(_load, newfile, true);
                    _save = _leaf.LeafProperties.ConvertToJson();
                    //_leaf.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".lvl") {
                    dynamic _load = UtilFile.LoadFileLock(newfile);
                    EditorLvl _lvl = new(_load, newfile, true);
                    _save = EditorLvl.BuildSave(_lvl.LvlProperties);
                    //_lvl.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".master") {
                    dynamic _load = UtilFile.LoadFileLock(newfile);
                    EditorMaster _master = new(_load, newfile, true);
                    _save = EditorMaster.BuildSave(_master.MasterProperties);
                    //_master.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".samp") {
                    dynamic _load = UtilFile.LoadFileLock(newfile);
                    EditorSample _samp = new(_load, newfile, true);
                    _save = EditorSample.BuildSave(_samp.SampleProperties);
                    //_samp.SaveCheckAndWrite(true, "");
                }
                if (_save != null) {
                    UtilFile.WriteFileLock(newfile.FullName, _save);
                }
            }
            //
            TCLE.Instance.lblLoadingLeaf.Text = $"Finalizing";
            TCLE.Instance.lblLoadingLeaf.Invalidate();
            TCLE.Instance.lblLoadingLeaf.Update();
            TCLE.Instance.lblLoadingLeaf.Refresh();
            Application.DoEvents();
            //build the JSON to write to file
            JObject _saveJSON = BuildSave(Convert);
            //write JSON to file
            File.WriteAllText($@"{LevelDetails.DirectoryName}\{Convert.ProjectName}.TCL", JsonConvert.SerializeObject(_saveJSON, Formatting.Indented));
            //locate pyramid_outro
            FileInfo pyramid = LevelDetails.Directory.GetFiles("pyramid_outro.leaf", SearchOption.AllDirectories).FirstOrDefault();
            if (pyramid != null)
                UtilFile.WriteFileLock(pyramid.FullName, Properties.Resources.leaf_pyramid_outro);
            TCLE.Instance.panelLoadingMessage.Visible = false;

            OpenProject(new FileInfo($@"{LevelDetails.DirectoryName}\{Convert.ProjectName}.TCL"));
        }

        public static JObject BuildSave(ProjectProperties _properties)
        {
            JObject _save = new() {
                { "level_name", _properties.ProjectName },
                { "difficulty", _properties.Difficulty },
                { "description", _properties.Description },
                { "author", _properties.AuthorNames },
                { "bpm", _properties.BPM },
                { "level_sections", new JArray() {_properties.LevelSections} },
                { "rails_color", new JArray() { _properties.RailColor.R / 255f, _properties.RailColor.G / 255f, _properties.RailColor.B / 255f, 1 } },
                { "rails_glow_color", new JArray() { _properties.RailGlowColor.R / 255f, _properties.RailGlowColor.G / 255f, _properties.RailGlowColor.B / 255f, 1}},
                { "path_color", new JArray() { _properties.PathColor.R / 255f, _properties.PathColor.G / 255f, _properties.PathColor.B / 255f, 1 }},
                { "joy_color", new JArray() { 1f, 1f, 1f, 1f } }
            };
            return _save;
        }

        public static DateTime lastsave = new(0);
        public static void SaveTCL()
        {
            if (DateTime.Now < lastsave.AddSeconds(5))
                return;
            JObject _saveJSON = TCLE.BuildSave(TCLE.ProjectProperties);
            //write JSON to file
            UtilFile.WriteFileLock(TCLE.ProjectProperties.FileLock, _saveJSON);
            //File.WriteAllText($"{TCLE.ProjectProperties.WorkingFile.FullName}", JsonConvert.SerializeObject(_saveJSON, Formatting.Indented));

            lastsave = DateTime.Now;
        }
    }

    public static class DictExtensions
    {
        public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey oldKey, TKey newKey)
        {
            if (!dict.Remove(oldKey, out TValue value))
                return false;

            dict[newKey] = value;  // or dict.Add(newKey, value) depending on ur comfort
            return true;
        }

    }
}