using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using Thumper_Custom_Level_Editor.Other_Forms;
using NAudio.Wave;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        #region Variable
        //Static
        public static string VersionNumber = "3.0.0-a66";
        public static string _errorlog = "";
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
        //public static Dictionary<string, Object_Params> ObjectFavorites = new();
        public static Dictionary<string, Bitmap> ColorIcons = new();
        public static List<SampleData> ProjectSamples = new();
        public static Dictionary<string, double> ProjectSampleRuntimes = new();
        //Static Readonly
        public static readonly List<string> TimeSignatures = new() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
        public static readonly Dictionary<string, string> TrackLaneFriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" }, { "none", "none" } };
        public static readonly Dictionary<string, string> Easings = new() { { "kEaseInOut", "Ease In Out" }, { "kEaseIn", "Ease In" }, { "kEaseOut", "Ease Out" } };
        public static readonly string[] ImageExtensions = new string[] { ".png", ".jpeg", ".jpg", ".gif", ".webp", ".bmp" };
        public static readonly string[] ProjectExtensions = new string[] { ".leaf", ".lvl", ".gate", ".master", ".samp" };
        //
        //Local basic vars
        //
        //Local custom class vars
        private DeserializeDockContent m_deserializeDockContent;
        //
        #endregion

        private static void LoadQuickValues()
        {
            if (!File.Exists($@"{TCLE.AppLocation}\settings\quickvalues.txt"))
                return;
            string[] _load = File.ReadAllLines($@"{TCLE.AppLocation}\settings\quickvalues.txt");

            LeafQuickValue0 = decimal.TryParse(_load[0], out decimal result) ? result : 1.000m;
            LeafQuickValue1 = decimal.TryParse(_load[1], out result) ? result : 1.000m;
            LeafQuickValue2 = decimal.TryParse(_load[2], out result) ? result : 1.000m;
            LeafQuickValue3 = decimal.TryParse(_load[3], out result) ? result : 1.000m;
            LeafQuickValue4 = decimal.TryParse(_load[4], out result) ? result : 1.000m;
            LeafQuickValue5 = decimal.TryParse(_load[5], out result) ? result : 1.000m;
            LeafQuickValue6 = decimal.TryParse(_load[6], out result) ? result : 1.000m;
            LeafQuickValue7 = decimal.TryParse(_load[7], out result) ? result : 1.000m;
            LeafQuickValue8 = decimal.TryParse(_load[8], out result) ? result : 1.000m;
            LeafQuickValue9 = decimal.TryParse(_load[9], out result) ? result : 1.000m;
        }

        public static void DoubleBufferDGV(DataGridView grid)
        {
            //double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
            //used to significantly improve rendering performance
            if (!SystemInformation.TerminalServerSession) {
                Type dgvType = grid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(grid, true, null);
            }
        }

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
                dgvc.DefaultCellStyle.Font = Form_LeafEditor.TuningFont;
                dgvc.Width = Properties.Settings.Default.ZoomHoriz;
            }
        }

        public static void ImportObjects()
        {
            LeafObjects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists($@"{AppLocation}\settings\track_objects_v4.txt")) {
                using (StreamWriter sw = File.CreateText($@"{AppLocation}\settings\track_objects_v4.txt")) {
                    sw.Write(Properties.Resources.trackobjects_v4);
                }
            }
            //import selectable objects from file and parse them into lists for manipulation
            string[] _importedObjects = File.ReadAllLines($@"{AppLocation}\settings\track_objects_v4.txt");
            LeafObjects = _importedObjects.Select(x => x.Split(';'))
                                        .Select(x => new KeyValuePair<string, Object_Params>(x[1]+";"+x[3], new Object_Params {
                                            category = x[0],
                                            obj_name = x[1],
                                            param_displayname = x[2],
                                            param_path = x[3],
                                            trait_type = x[4],
                                            step = x[5] == "True",
                                            default_value = decimal.TryParse(x[6], out decimal _result) ? _result : 0,
                                            footer = x[7].Replace("[", "").Replace("]", ""),
                                            defaultcolor = Color.Purple
                                        })).ToDictionary();
            
            LeafObjects.Add("_TuningLayerX;⮝ Tuning Layer X", new Object_Params {
                category = "",
                obj_name = "_TuningLayerX",
                param_displayname = "⮝ Tuning Layer X",
                param_path = "⮝ Tuning Layer X",
                trait_type = "",
                step = false,
                default_value = 0m,
                footer = "",
                defaultcolor = Color.FromArgb(40, 40, 40)
            });
            //import default colors per object
            ImportDefaultColors();
            //import favorites
            if (AppSettings.SequencerFavorites != null) {
                foreach (string key in AppSettings.SequencerFavorites)
                    LeafObjects[key].favorite = true;
            }
                //ObjectFavorites = LeafObjects.Where(x => AppSettings.SequencerFavorites.Contains(x.Key)).ToDictionary();
        }

        public static void ImportDefaultColors()
        {
            Dictionary<string, Color> ObjectColors = new();
            if (!File.Exists($@"{AppLocation}\settings\objects_defaultcolors_v3.txt")) {
                File.WriteAllText($@"{AppLocation}\settings\objects_defaultcolors_v3.txt", Properties.Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines($@"{AppLocation}\settings\objects_defaultcolors_v3.txt").ToDictionary(g => g.Split(';')[0], g => Color.FromArgb(int.Parse(g.Split(';')[1])));

            ///colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //once all the colors are processed, assign them directly to the objects
            foreach (Object_Params obj in LeafObjects.Select(x => x.Value)) {
                obj.defaultcolor = ObjectColors.TryGetValue(obj.param_displayname, out Color value) ? value : Color.Purple;
                Bitmap color = new(16, 16);
                using (Graphics g = Graphics.FromImage(color)) {
                    g.Clear(value);
                }
                ColorIcons.TryAdd(value.ToArgb().ToString(), color);
            }
        }

        ///Color elements based on set properties
        public static void ColorFormElements(TCLE MainForm)
        {
            MainForm.toolStripTitle.BackColor = AppSettings.ColorMainMenuBar;
            MainForm.panelToolStrips.BackColor = AppSettings.ColorMainSubMenubar;
            MainForm.dockMain.BackColor = AppSettings.ColorMainBG;

            TCLE.Explorer?.ColorFormElements();

            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor)))
                leaf.ColorFormElements();
            foreach (Form_LvlEditor lvl in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LvlEditor)))
                lvl.ColorFormElements();
            foreach (Form_GateEditor gate in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_GateEditor)))
                gate.ColorFormElements();
            foreach (Form_MasterEditor master in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_MasterEditor)))
                master.ColorFormElements();
            foreach (Form_SampleEditor sample in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_SampleEditor)))
                sample.ColorFormElements();
            foreach (Form_RawText raw in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_RawText)))
                raw.ColorFormElements();
        }

        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + (backColor.R * (1 - amount)));
            byte g = (byte)((color.G * amount) + (backColor.G * (1 - amount)));
            byte b = (byte)((color.B * amount) + (backColor.B * (1 - amount)));
            return Color.FromArgb(r, g, b);
        }

        public static void Read_Config()
        {
            CommonOpenFileDialog cfd_lvl = new() {
                IsFolderPicker = true,
                Multiselect = false,
                Title = "Select the folder where Thumper is installed (NOT the cache folder)"
            };
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

        public static string SearchReferences(string searchreference)
        {
            string referencefiles = "";
            //search all files in the project folder
            foreach (FileInfo file in WorkingFolder.GetFiles("*", SearchOption.AllDirectories).Where(x => ProjectExtensions.Contains(x.Extension))) {
                //skip self to not include self
                if (file.Name == searchreference)
                    continue;
                string text = ((JObject)UtilFile.LoadFileLock(file.FullName)).ToString(Formatting.None);
                //check if the file we're searching contains the obj_name
                if (text.Contains(searchreference)) {
                    referencefiles += file.Name + '\n';
                }
            }

            return referencefiles.Length > 1 ? referencefiles : "<none>";
        }

        public void ShowChangelog()
        {
            panelChangelog.Visible = true;
            panelChangelog.BringToFront();
            //lblChangelog.Text = Properties.Resources.changelog;
        }
        private void lblChangelogClose_Click(object sender, EventArgs e) => panelChangelog.Visible = false;

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
        }

        /// https://stackoverflow.com/questions/3143657/truncate-two-decimal-places-without-rounding#answer-43639947
        public static decimal? TruncateDecimal(decimal? d, byte decimals)
        {
            if (d == null)
                return null;
            decimal r = Math.Round((decimal)d, decimals);

            if (d > 0 && r > d) {
                return r - new decimal(1, 0, 0, false, decimals);
            }
            else if (d < 0 && r < d) {
                return r + new decimal(1, 0, 0, false, decimals);
            }

            return r;
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

            foreach (Form_LeafEditor leaf in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LeafEditor))) {
                SeqObjTreeBuilder.FilterTree(leaf.treeObjects, leaf.txtSearch.Text);
            }
            foreach (Form_LvlEditor lvl in TCLE.Documents.Values.Where(x => x.GetType() == typeof(Form_LvlEditor))) {
                //load loop track names and paths to lvlLoopTracks DGV
                ((DataGridViewComboBoxColumn)lvl.lvlLoopTracks.Columns[1]).DataSource = TCLE.ProjectSamples.Select(x => x.obj_name).ToList();
            }
        }        

        public static uint Hash32(string s)
        {
            //this hashes stuff. Don't know why it does it this why.
            //this is ripped directly from the game's code
            uint h = 0x811c9dc5;
            foreach (char c in s)
                h = ((h ^ c) * 0x1000193) & 0xffffffff;
            h = (h * 0x2001) & 0xffffffff;
            h = (h ^ (h >> 0x7)) & 0xffffffff;
            h = (h * 0x9) & 0xffffffff;
            h = (h ^ (h >> 0x11)) & 0xffffffff;
            h = (h * 0x21) & 0xffffffff;

            return h;
        }

        public static string HashPCName(string StringToHash)
        {
            string _hashedname = "";
            byte[] hashbytes = BitConverter.GetBytes(Hash32(StringToHash));
            Array.Reverse(hashbytes);
            foreach (byte b in hashbytes)
                _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
            //if the hashed name starts with a '0', remove it
            if (_hashedname[0] == '0')
                _hashedname = _hashedname[1..];
            return _hashedname;
        }

        public static int ByteSearch(byte[] src, byte[] pattern)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++) {
                if (src[i] != pattern[0]) // compare only first byte
                    continue;

                // found a match on first byte, now try to match rest of the pattern
                for (int j = pattern.Length - 1; j >= 1; j--) {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1) return i;
                }
            }
            return -1;
        }

        public static int CalculateSublevelRuntime(MasterLvlData _masterlvl)
        {
            int _beatcount = 0;
            if (_masterlvl.Type == "lvl") {
                FileInfo lvl = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{_masterlvl.name}"));
                if (lvl != null) _beatcount += CalculateLvlRuntime(lvl.FullName);
                else return -1;
            }
            //this section handles gate
            else {
                int gatebeats = CalculateGateRuntimeFromFile(_masterlvl.name);
                if (gatebeats == -1)
                    return -1;
                else
                    _beatcount += gatebeats;
            }
            FileInfo lvlrest = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{_masterlvl.rest}"));
            if (lvlrest != null) _beatcount += CalculateLvlRuntime(lvlrest.FullName);

            return _beatcount;
        }

        public static int CalculateGateRuntimeFromFile(string gatename)
        {
            dynamic _load;
            int _beatcount = 0;
            List<int> bucketscounted = new();
            bool israndom;
            //load the gate to then loop through all lvls in it
            FileInfo gate = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{gatename}"));
            if (gate != null) {
                _load = UtilFile.LoadFileLock(gate.FullName);
                //if gate not found, _load is null. Return -1 to denote this
                if (_load == null)
                    return -1;
                //check if random is enabled on this gate
                israndom = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
                //loop through each lvl in gate
                foreach (dynamic _lvl in _load["boss_patterns"]) {
                    //attempt to load lvl
                    FileInfo lvl = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{(string)_lvl["lvl_name"]}"));
                    if (lvl != null) {
                        //if random is enabled, count only the first entry in each bucket
                        if (israndom) {
                            if (!bucketscounted.Contains((int)_lvl["bucket_num"])) {
                                bucketscounted.Add((int)_lvl["bucket_num"]);
                                _beatcount += CalculateLvlRuntime(lvl.FullName);
                            }
                        }
                        //otherwise count each lvl
                        else
                            _beatcount += CalculateLvlRuntime(lvl.FullName);
                    }
                }
                //need to also count pre and post lvl
                FileInfo prelvl = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{(string)_load["pre_lvl_name"]}"));
                if (prelvl != null) {
                    _beatcount += CalculateLvlRuntime(prelvl.FullName);
                }
                FileInfo postlvl = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{(string)_load["post_lvl_name"]}"));
                if (postlvl != null) {
                    _beatcount += CalculateLvlRuntime(postlvl.FullName);
                }
            }
            else
                return -1;

            return _beatcount;
        }
        public static int CalculateLvlRuntime(string path)
        {
            int _beatcount = 0;

            //load the lvl and then loop through its leafs to get beat counts
            dynamic _load = UtilFile.LoadFileLock(path);
            if (_load == null)
                return 0;
            foreach (dynamic leaf in _load["leaf_seq"]) {
                FileInfo _leaf = ProjectExplorer.Files.FirstOrDefault(x => x.FullName.EndsWith($@"\{(leaf["leaf_name"])}"));
                if (_leaf != null && _leaf.Exists)
                    _beatcount += (int)UtilFile.LoadFileLock(_leaf.FullName)["beat_cnt"];
                ///_beatcount += (int)leaf["beat_cnt"];
            }
            //every lvl has an approach beats to consider too
            //_beatcount += (int)_load["approach_beats"];

            return _beatcount;
        }

        //check if at least 1 master file exists
        public static bool CheckForMaster()
        {
            return ProjectExplorer.Files.Any(x => x.Extension is ".master");
        }

        public static EditorBase OpenFile(FileInfo filepath, bool openraw = false, bool ReturnContent = false)
        {
            if (filepath == null)
                return null;
            //if item is an image, open in image viewer instead of a DockContent
            if (ImageExtensions.Contains(filepath.Extension.ToLower())) {
                Image theimage = null;
                using (FileStream fs = new(filepath.FullName, FileMode.Open)) {
                    theimage = Image.FromStream(fs);
                }
                ImageViewer image = new(theimage) { Text = filepath.Name};
                image.Show();
                return null;
            }
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
                    Form_WorkSpace workspace1 = new($"Workspace {Workspaces.Count() + 1}") { DockAreas = DockAreas.Document };
                    workspace1.Show(TCLE.Instance.dockMain, DockState.Document);
                }
                //find if the document is loaded already in a tab
                //if so, make it activate
                IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText.Replace("*", "") == (filepath.Name + (openraw ? " [Raw]" : ""))));
                if (workspacehastab != null) {
                    workspacehastab.DockHandler.Activate();
                    (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText.Replace("*", "") == (filepath.Name + (openraw ? " [Raw]" : ""))).DockHandler.Activate();
                    return null;
                }

                IEnumerable<Form_WorkSpace> workspacewithfloats = TCLE.Workspaces.Cast<Form_WorkSpace>().Where(w => w.dockMain.FloatWindows.Count > 0);
                foreach (Form_WorkSpace ws in workspacewithfloats) {
                    IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).FirstOrDefault();
                    if (activate != null) {
                        activate.DockHandler.Activate();
                        return null;
                    }
                }
                //open document in raw viewer if that option was selected
                if (openraw || !ProjectExtensions.Contains(filepath.Extension)) {
                    Form_RawText rawtext = new((string)_load, filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                    if (ReturnContent)
                        return rawtext;
                    rawtext.Show(ActiveWorkspace.dockMain, DockState.Document);
                    //TCLE.Documents.Add(rawtext.WorkingFile.Name + "-raw", rawtext);
                    return null;
                }
            }
            //this finds a pane in the active workspace that has matching extensions already open on it
            DockPane OpenHere = ReturnContent ? null : ActiveWorkspace.dockMain.Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(filepath.Extension)).Any());

            EditorBase OpenFile = new(null);
            if (filepath.Extension == ".master") {
                OpenFile = new Form_MasterEditor(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
            }
            else if (filepath.Extension == ".lvl") {
                OpenFile = new Form_LvlEditor(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
            }
            else if (filepath.Extension == ".gate") {
                OpenFile = new Form_GateEditor(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
            }
            else if (filepath.Extension == ".leaf") {
                OpenFile = new Form_LeafEditor(_load, filepath, Playback.Generating) { DockAreas = DockAreas.Document | DockAreas.Float };
            }
            else if (filepath.Extension == ".samp") {
                OpenFile = new Form_SampleEditor(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
            }
            //TCLE.Documents.Add(OpenFile.WorkingFile.Name, OpenFile);
            if (ReturnContent)
                return OpenFile;
            if (OpenHere != null) OpenFile.Show(OpenHere, null);
            else OpenFile.Show(ActiveWorkspace.dockMain, DockState.Document);

            return null;
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
            (_found as Form_RawText)?.Reload();
            /*
            foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(documentname) && x.GetType() == typeof(Form_RawText))) {
                (document as Form_RawText).Reload();
            }
            */
        }

        public static void FindEditorRunMethod(Type editor, string method)
        {
            foreach (IDockContent document in TCLE.Documents.Values.Where(x => x.GetType() == editor)) {
                document.GetType().GetMethod(method).Invoke(document, null);
            }
        }

        public static bool AnyUnsaved(Form_WorkSpace work = null, Type type = null)
        {
            //Different save check method depending on what files are to be closed

            //closing an entire workspace
            if (work != null) {
                //closing a specific file type
                if (type != null) {
                    foreach (EditorBase document in work.dockMain.Documents.Where(x => x.GetType() == type)) {
                        if (!document.Saved)
                            return true;
                    }
                }
                //closing all in workspace
                else {
                    foreach (EditorBase document in work.dockMain.Documents) {
                        if (!document.Saved)
                            return true;
                    }
                }
            }
            //closing everything
            else {
                foreach (EditorBase document in TCLE.Documents.Values) {
                    if (!document.Saved)
                        return true;
                }
            }
            return false;
        }

        /// This also works for negative numbers
        public static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
        public static decimal mod(decimal x, int m)
        {
            decimal r = x % m;
            return r < 0 ? r + m : r;
        }

        public void ConvertProjectToNew()
        {
            FileInfo LevelDetails;
            FileInfo ConfigFile;
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
                    Form_LeafEditor _leaf = new(_load, newfile, true);
                    _save = _leaf.LeafProperties.ConvertToJson();
                    //_leaf.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".lvl") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    Form_LvlEditor _lvl = new(_load, newfile, true);
                    _save = Form_LvlEditor.BuildSave(_lvl.LvlProperties);
                    //_lvl.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".master") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    Form_MasterEditor _master = new(_load, newfile, true);
                    _save = Form_MasterEditor.BuildSave(_master.MasterProperties);
                    //_master.SaveCheckAndWrite(true, "");
                }
                else if (newfile.Extension == ".samp") {
                    dynamic _load = UtilFile.LoadFileLock(newfile.FullName);
                    Form_SampleEditor _samp = new(_load, newfile, true);
                    _save = Form_SampleEditor.BuildSave(_samp.SampleProperties);
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
                { "rails_color", new JArray() { (float)_properties.rail.R / 255, (float)_properties.rail.G / 255, (float)_properties.rail.B / 255, 1 } },
                { "rails_glow_color", new JArray() { (float)_properties.railglow.R / 255, (float)_properties.railglow.G / 255, (float)_properties.railglow.B / 255, 1}},
                { "path_color", new JArray() { (float)_properties.path.R / 255, (float)_properties.path.G / 255, (float)_properties.path.B / 255, 1 }},
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