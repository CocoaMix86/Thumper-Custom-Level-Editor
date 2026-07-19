using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Other_Forms;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        #region Variable
        //Static
        public static string VersionNumber = "3.0.0-a69";
        public static decimal LeafQuickValue1 = 1.000m;
        public static decimal LeafQuickValue2 = 1.000m;
        public static decimal LeafQuickValue3 = 1.000m;
        public static decimal LeafQuickValue4 = 1.000m;
        public static decimal LeafQuickValue5 = 1.000m;
        public static decimal LeafQuickValue6 = 1.000m;
        public static decimal LeafQuickValue7 = 1.000m;
        public static decimal LeafQuickValue8 = 1.000m;
        public static decimal LeafQuickValue9 = 1.000m;
        public static decimal LeafQuickValue0 = 1.000m;
        public static List<string> LvlPaths = Properties.Resources.paths.Replace("\r\n", "\n").Split('\n').ToList();
        public static Dictionary<string, Object_Params> LeafObjects = new();
        public static Dictionary<string, Bitmap> ColorIcons = new();
        public static List<SampleData> ProjectSamples = new();
        //Static Readonly
        public static readonly List<string> TimeSignatures = new() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
        public static readonly Dictionary<string, string> TrackLaneFriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" }, { "none", "none" } };
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
        /*
        public static void GenerateColumnStyle(IEnumerable<DataGridViewColumn> columns, int offset = 0)
        {
            foreach (DataGridViewColumn dgvc in columns) {
                dgvc.CellTemplate = new SeqDataPoint();
                dgvc.Name = (dgvc.Index - offset).ToString();
                dgvc.HeaderText = (dgvc.Index - offset).ToString();
                dgvc.Resizable = DataGridViewTriState.False;
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvc.DividerWidth = 0;
                dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvc.Frozen = false;
                dgvc.MinimumWidth = 2;
                dgvc.ReadOnly = false;
                dgvc.ValueType = typeof(decimal?);
                dgvc.DefaultCellStyle.Format = "0.###";
                dgvc.FillWeight = 0.001F;
                dgvc.DefaultCellStyle.Font = EditorLeaf.TuningFont;
                dgvc.Width = Properties.Settings.Default.ZoomHoriz;
            }
        }        
        */
        ///Color elements based on set properties
        public static void ColorFormElements(TCLE MainForm)
        {
            MainForm.toolStripTitle.BackColor = Properties.Settings.Default.ColorMainMenuBar;
            MainForm.panelToolStrips.BackColor = Properties.Settings.Default.ColorMainSubMenubar;
            MainForm.dockMain.BackColor = Properties.Settings.Default.ColorMainBG;

            TCLE.Explorer?.ColorFormElements();

            foreach (EditorBase editor in TCLE.Documents.Values)
                editor.ColorFormElements();
            /*
            foreach (EditorLeaf leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorLeaf)))
                leaf.ColorFormElements();
            foreach (EditorLvl lvl in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorLvl)))
                lvl.ColorFormElements();
            foreach (EditorGate gate in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorGate)))
                gate.ColorFormElements();
            foreach (EditorMaster master in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorMaster)))
                master.ColorFormElements();
            foreach (EditorSample sample in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorSample)))
                sample.ColorFormElements();
            foreach (EditorRawText raw in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorRawText)))
                raw.ColorFormElements();
            */
        }

        public void MenusVisible(bool visible)
        {
            panelRecentFiles.Visible = !visible;
            panelIntroTips.Visible = !visible;
            if (WorkingFolder == null)
                visible = false;
            panelToolStrips.Visible = visible;
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

            toolstripProject.Enabled = true;
            toolstripEdit.Enabled = true;
            toolstripWindow.Enabled = true;
            toolstripViewExplorer.Enabled = true;
            toolstripViewProperties.Enabled = true;

            MainBeeble.Visible = visible;
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
            ProjectSamples.Add(new SampleData { obj_name = "", path = "", volume = 0, pitch = 0, pan = 0, offset = 0, channel_group = "", File = null });
            string warning = "";
            //iterate over each file
            foreach (FileInfo sampfile in WorkingFolder.GetFiles("*.samp", SearchOption.AllDirectories).Where(x => x.Name != "?!?!default?!?!?!?.samp")) {
                UpdateProjectSamplesFromFile(sampfile, false, false, out string _warning);
                warning += _warning;
            }
            if (warning.Length > 2)
                MessageBox.Show($"Your sample files contain duplicate entries. These can break your level, and it is advised to rename 1 or both of them.\n\n{warning}", "Thumper Custom Level Editor");
            ProjectSamples = ProjectSamples.OrderBy(w => w.obj_name).ToList();
            //
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
            }

            UpdateEditorsWithSamples();
            //File.WriteAllLines($@"{AppLocation}\templates\{TCLE.WorkingFolder.Name}_sample_runtimes.temp", ProjectSamples.Select(x => $"{x.obj_name};{x.time}"));
        }

        public static void UpdateProjectSamplesFromFile(FileInfo SampFile, bool preserveSamples, bool updateeditors, out string warning)
        {
            //remove samples that match the incoming sample file, so that they're rewritten
            ProjectSamples.RemoveAll(x => x.File?.FullName == SampFile.FullName);
            //parse file to JSON
            dynamic _in = UtilFile.LoadFileLock(SampFile.FullName);
            warning = "";
            //skip if somehow empty
            if (_in == null || !_in.ContainsKey("items"))
                return;
            //iterate over items:[] list to get each sample and add names to list
            foreach (dynamic _samp in _in["items"]) {
                if (ProjectSamples.Any(x => x.obj_name == (string)_samp["obj_name"])) {
                    if (!preserveSamples)
                        warning += $"{_samp["obj_name"]} in {SampFile.FullName}\n{_samp["obj_name"]} in {ProjectSamples.First(x => x.obj_name == (string)_samp["obj_name"]).File.FullName}\n";
                    else
                        continue;
                }
                ProjectSamples.Add(new SampleData {
                    obj_name = ((string)_samp["obj_name"]),
                    path = _samp["path"],
                    volume = _samp["volume"],
                    pitch = _samp["pitch"],
                    pan = _samp["pan"],
                    offset = _samp["offset"],
                    channel_group = _samp["channel_group"],
                    File = SampFile,
                    time = 0
                });
            }

            if (updateeditors)
                UpdateEditorsWithSamples();
        }

        public static void RemoveProjectSamples(FileInfo SampFile)
        {
            TCLE.ProjectSamples.RemoveAll(x => x.File?.FullName == SampFile.FullName);
            UpdateEditorsWithSamples();
        }

        public static void UpdateEditorsWithSamples()
        {
            SeqObjTreeBuilder.BuildObjectTree(SeqObjTreeBuilder.GlobalObjectTree, "");
            var _samples = TCLE.ProjectSamples.Select(x => x.obj_name).ToList();

            foreach (EditorLeaf leaf in TCLE.Documents.Values.OfType<EditorLeaf>()) {
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.txtSearch.Text);
            }
            foreach (EditorLvl lvl in TCLE.Documents.Values.OfType<EditorLvl>()) {
                //load loop track names and paths to lvlLoopTracks DGV
                ((DataGridViewComboBoxColumn)lvl.lvlLoopTracks.Columns[1]).DataSource = _samples;
            }
        }

        //check if at least 1 master file exists
        public static bool CheckForMaster()
        {
            return ProjectExplorer.Files.Any(x => string.Equals(x.Extension, ".master", StringComparison.OrdinalIgnoreCase));
        }

        public static EditorBase OpenFile(FileInfo filepath, bool openraw = false, bool ReturnContent = false)
        {
            if (filepath == null)
                return null;
            //if item is an image, open in image viewer instead of a DockContent
            if (TryOpenImage(filepath))
                return null;
            //if item is not an editor type, open raw
            if (!ProjectExtensions.Contains(filepath.Extension.ToLower())) {
                openraw = true;
            }

            object _load = UtilFile.LoadFileLock(filepath.FullName, openraw);
            if (_load == null)
                return null;
            //if there are no workspaces, add one
            if (!ReturnContent) {
                if (!Workspaces.Any()) {
                    DockWorkspace workspace1 = new($"Workspace {Workspaces.Count() + 1}") { DockAreas = DockAreas.Document };
                    workspace1.Show(TCLE.Instance.dockMain, DockState.Document);
                }
                //find if the document is loaded already in a tab
                //if so, make it activate
                IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as DockWorkspace).dockMain.Documents.Any(y => y.DockHandler.TabText.Replace("*", "") == (filepath.Name + (openraw ? " [Raw]" : ""))));
                if (workspacehastab != null) {
                    workspacehastab.DockHandler.Activate();
                    (workspacehastab as DockWorkspace).dockMain.Documents.First(y => y.DockHandler.TabText.Replace("*", "") == (filepath.Name + (openraw ? " [Raw]" : ""))).DockHandler.Activate();
                    return null;
                }

                IEnumerable<DockWorkspace> workspacewithfloats = TCLE.Workspaces.Cast<DockWorkspace>().Where(w => w.dockMain.FloatWindows.Count > 0);
                foreach (DockWorkspace ws in workspacewithfloats) {
                    IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).FirstOrDefault();
                    if (activate != null) {
                        activate.DockHandler.Activate();
                        return null;
                    }
                }
                //open document in raw viewer if that option was selected
                if (openraw || !ProjectExtensions.Contains(filepath.Extension)) {
                    EditorRawText rawtext = new((string)_load, filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                    if (ReturnContent)
                        return rawtext;
                    rawtext.Show(ActiveWorkspace.dockMain, DockState.Document);
                    //TCLE.Documents.Add(rawtext.WorkingFile.Name + "-raw", rawtext);
                    return null;
                }
            }
            //this finds a pane in the active workspace that has matching extensions already open on it
            DockPane OpenHere = ReturnContent ? null : ActiveWorkspace.dockMain.Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(filepath.Extension)).Any());

            DockContent OpenFile = new() { DockAreas = DockAreas.Document | DockAreas.Float };
            if (filepath.Extension == ".master") {
                OpenFile = new EditorMaster(_load, filepath);
            }
            else if (filepath.Extension == ".lvl") {
                OpenFile = new EditorLvl(_load, filepath);
            }
            else if (filepath.Extension == ".gate") {
                OpenFile = new EditorGate(_load, filepath);
            }
            else if (filepath.Extension == ".leaf") {
                OpenFile = new EditorLeaf(_load, filepath, Playback.Generating);
            }
            else if (filepath.Extension == ".samp") {
                OpenFile = new EditorSample(_load, filepath);
            }

            TCLE.Instance.toolStripWindowCloseTab.Enabled = true;
            TCLE.Instance.toolstripWindowCloseEditors.Enabled = true;
            TCLE.Instance.toolStripMenuItem7.Enabled = true;
            TCLE.Instance.toolstripWindowCloseFiletype.Enabled = true;
            TCLE.Instance.toolstripWindowFloat.Enabled = true;
            TCLE.Instance.toolstripWindowFloatAll.Enabled = true;
            TCLE.Instance.toolstripWindowDock.Enabled = true;
            //TCLE.Documents.Add(OpenFile.WorkingFile.Name, OpenFile);
            if (ReturnContent)
                return (EditorBase)OpenFile;
            if (OpenHere != null) OpenFile.Show(OpenHere, null);
            else OpenFile.Show(ActiveWorkspace.dockMain, DockState.Document);

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

        public static void CloseFile(FileInfo filepath)
        {
            TCLE.Documents.TryGetValue(filepath.Name, out EditorBase _close);
            _close?.Close();
            _close?.Dispose();
            TCLE.Documents.TryGetValue(filepath.Name + "-raw", out _close);
            _close?.Close();
            _close?.Dispose();
            /*
            //check tabs in non float
            IDockContent workspacehastab = TCLE.Workspaces.SelectMany(x => (x as Form_WorkSpace).dockMain.Documents).FirstOrDefault(y => y.DockHandler.TabText.StartsWith(filepath.Name));
            if (workspacehastab != null) {
                (workspacehastab as DockContent).DockHandler.Dispose();
            }
            //check tabs in floats
            IEnumerable<Form_WorkSpace> workspacewithfloats = TCLE.Workspaces.Cast<Form_WorkSpace>().Where(w => w.dockMain.FloatWindows.Count > 0);
            foreach (Form_WorkSpace ws in workspacewithfloats) {
                IDockContent toclose = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).FirstOrDefault(z => z.DockHandler.TabText.StartsWith(filepath.Name));
                if (toclose != null) {
                    (toclose as DockContent).DockHandler.Dispose();
                }
            }
            */
        }

        public static void ReloadLvlsInProject()
        {
            if (WorkingFolder == null)
                return;
            lvlsinworkfolder = ProjectExplorer.Files.Where(x => x.Extension == ".lvl").Select(x => x.Name).ToList();
            lvlsinworkfolder.Add("<none>");
            lvlsinworkfolder.Sort();
        }

        public static void FindReloadRaw(string documentname)
        {
            //find if any raw text docs matching documentname are open and update them
            TCLE.Documents.TryGetValue(documentname + "-raw", out EditorBase _found);
            (_found as EditorRawText)?.Reload();
            /*
            foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(documentname) && x.GetType() == typeof(Form_RawText))) {
                (document as Form_RawText).Reload();
            }
            */
        }

        public static void FindEditorRunMethod(Type editorType, string methodName)
        {
            var method = editorType.GetMethod(methodName);
            if (method == null)
                return;

            foreach (var document in TCLE.Documents.Values.Where(x => editorType.IsInstanceOfType(x))) {
                method.Invoke(document, null);
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
            dynamic ProjectJson = UtilFile.LoadFileLock(LevelDetails.FullName);
            dynamic ProjectConfig = UtilFile.LoadFileLock(LevelDetails.Directory.GetFiles("config_*.txt").FirstOrDefault()?.FullName);
            ProjectProperties Convert = new() {
                ProjectName = (string)ProjectJson["level_name"] ?? "New Project",
                difficulty = (string)ProjectJson["difficulty"] ?? "D0",
                description = (string)ProjectJson["description"] ?? "Please add a description",
                authornames = (string)ProjectJson["author"] ?? "a person",
                BPM = (decimal?)ProjectConfig["bpm"] ?? 400m
            };
            //load colors, with failover to White
            try {
                Convert.BPM = (decimal?)ProjectConfig["bpm"] ?? 400m;
                dynamic railcolor = ProjectConfig["rails_color"];
                Convert.rail = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
                dynamic railglowcolor = ProjectConfig["rails_glow_color"];
                Convert.railglow = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
                dynamic pathcolor = ProjectConfig["path_color"];
                Convert.path = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
            } catch (Exception) {
                Convert.rail = Color.White;
                Convert.railglow = Color.White;
                Convert.path = Color.White;
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
                //resave leafs and lvls to properly convert the datapoints
                JObject _save = null;
                if (newfile.Extension == ".leaf") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    EditorLeaf _leaf = new(_load, newfile, true);
                    _save = _leaf.LeafProperties.ConvertToJson();
                    //_leaf.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".lvl") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    EditorLvl _lvl = new(_load, newfile, true);
                    _save = EditorLvl.BuildSave(_lvl.LvlProperties);
                    //_lvl.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".master") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    EditorMaster _master = new(_load, newfile, true);
                    _save = EditorMaster.BuildSave(_master.MasterProperties);
                    //_master.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".samp") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    EditorSample _samp = new(_load, newfile, true);
                    _save = EditorSample.BuildSave(_samp.SampleProperties);
                    //_samp.SaveCheckAndWrite(true, "");
                }
                if (_save != null) {
                    UtilFile.WriteFileLock(newfile.FullName, _save);
                }
            }
            //build the JSON to write to file
            JObject _saveJSON = BuildSave(Convert);
            //write JSON to file
            File.WriteAllText($@"{LevelDetails.DirectoryName}\{Convert.ProjectName}.TCL", JsonConvert.SerializeObject(_saveJSON, Formatting.Indented));
            //locate pyramid_outro
            FileInfo pyramid = LevelDetails.Directory.GetFiles("pyramid_outro.leaf", SearchOption.AllDirectories).FirstOrDefault();
            if (pyramid != null)
                UtilFile.WriteFileLock(pyramid.FullName, Properties.Resources.leaf_pyramid_outro);

            OpenProject(new FileInfo($@"{LevelDetails.DirectoryName}\{Convert.ProjectName}.TCL"));
        }

        public static JObject BuildSave(ProjectProperties _properties)
        {
            JObject _save = new() {
                { "level_name", _properties.ProjectName },
                { "difficulty", _properties.difficulty },
                { "description", _properties.description },
                { "author", _properties.authornames },
                { "bpm", _properties.BPM },
                { "level_sections", new JArray() {_properties.LevelSections} },
                { "rails_color", new JArray() { _properties.rail.R / 255f, _properties.rail.G / 255f, _properties.rail.B / 255f, 1 } },
                { "rails_glow_color", new JArray() { _properties.railglow.R / 255f, _properties.railglow.G / 255f, _properties.railglow.B / 255f, 1}},
                { "path_color", new JArray() { _properties.path.R / 255f, _properties.path.G / 255f, _properties.path.B / 255f, 1 }},
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
}