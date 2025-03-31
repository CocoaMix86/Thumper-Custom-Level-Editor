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

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        private DeserializeDockContent m_deserializeDockContent;
        public static readonly List<string> TimeSignatures = new() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
        public static decimal LeafQuickValue0 = 1.000m;
        public static decimal LeafQuickValue1 = 1.000m;
        public static decimal LeafQuickValue2 = 1.000m;
        public static decimal LeafQuickValue3 = 1.000m;
        public static decimal LeafQuickValue4 = 1.000m;
        public static decimal LeafQuickValue5 = 1.000m;
        public static decimal LeafQuickValue6 = 1.000m;
        public static decimal LeafQuickValue7 = 1.000m;
        public static decimal LeafQuickValue8 = 1.000m;
        public static decimal LeafQuickValue9 = 1.000m;
        public static readonly Dictionary<string, string> TrackLaneFriendly = new() { { "a01", "lane left 2" }, { "a02", "lane left 1" }, { "ent", "lane center" }, { "z01", "lane right 1" }, { "z02", "lane right 2" }, { "none", "none" } };
        public static readonly Dictionary<string, string> Easings = new() { { "kEaseInOut", "Ease In Out" }, { "kEaseIn", "Ease In" }, { "kEaseOut", "Ease Out" } };
        public static readonly string[] ImageExtensions = new string[] { ".png", ".jpeg", ".jpg", ".gif", ".webp", ".bmp" };
        public static readonly string[] ProjectExtensions = new string[] { ".leaf", ".lvl", ".gate", ".master" };
        public static List<string> LvlPaths = Properties.Resources.paths.Replace("\r\n", "\n").Split('\n').ToList();
        public static Dictionary<int, int> Frequencys = new() {
            { 1, 8000 },
            { 2, 11_000 },
            { 3, 11_025 },
            { 4, 16_000 },
            { 5, 22_050 },
            { 6, 24_000 },
            { 7, 32_000 },
            { 8, 44_100 },
            { 9, 48_000 },
            { 10,96_000 }
        };
        public static DataObject ClipBoardDataPoints = new();

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

        public static void DoubleBufferDGV(DataGridView grid, bool columnstyle)
        {
            //double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
            //used to significantly improve rendering performance
            if (!SystemInformation.TerminalServerSession) {
                Type dgvType = grid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(grid, true, null);
            }
        }

        public static void GenerateColumnStyle(List<DataGridViewColumn> columns, int offset = 0)
        {
            foreach (DataGridViewColumn dgvc in columns) {
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
                dgvc.DefaultCellStyle.Font = new Font("Consolas", 8);
                dgvc.ReadOnly = false;
                dgvc.Width = Properties.Settings.Default.ZoomHoriz;
            }
        }

        public static HashSet<Object_Params> LeafObjects = new();
        public static HashSet<Object_Params> ObjectFavorites = new();
        private string _errorlog = "";
        public void ImportObjects()
        {
            LeafObjects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists($@"{AppLocation}\settings\track_objects_v3.txt")) {
                using (StreamWriter sw = File.CreateText($@"{AppLocation}\settings\track_objects_v3.txt")) {
                    sw.Write(Properties.Resources.track_objects);
                }
            }

            ///import selectable objects from file and parse them into lists for manipulation
            //splits input at "###". Each section is a collection of param_paths
            List<string> import = File.ReadAllText($@"{AppLocation}\settings\track_objects_v3.txt").Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
            for (int x = 0; x < import.Count; x++) {
                //split each section into individual lines
                List<string> import2 = import[x].Split('\n').ToList();

                for (int y = 1; y < import2.Count - 1; y++) {
                    //split each line by ';'. Now each property is separated
                    string[] import3 = import2[y].Split(';');
                    try {
                        Object_Params objpar = new() {
                            category = import2[0],
                            obj_name = import3[0],
                            param_displayname = import3[1],
                            param_path = import3[2],
                            trait_type = import3[3],
                            step = import3[4] == "True",
                            def = import3[5],
                            footer = import3[6].Replace("[", "").Replace("]", ""),
                            defaultcolor = Color.Purple
                        };
                        //finally, add complete object and values to list
                        LeafObjects.Add(objpar);
                    }
                    catch {
                        _errorlog += "failed to import all properties of param_path " + import3[0] + " of object " + import2[0] + ".\n";
                    }
                }
            }
            LeafObjects.Add(new Object_Params() {
                category = "AUDIO",
                obj_name = "leafname",
                param_displayname = "Loop Track x Volume",
                param_path = "layer_volume,x",
                trait_type = "kTraitFloat",
                step = false,
                def = "0.0",
                footer = "1,1,2,1,2,kIntensityScale,kIntensityScale,1,1,1,1,1,1,1,1,0,0,0",
                defaultcolor = Color.DarkMagenta
            });
            //show errors to user if any imports failed
            if (_errorlog.Length > 1) {
                MessageBox.Show(_errorlog);
                _errorlog = "";
            }
            //import default colors per object
            ImportDefaultColors();
            //import favorites
            if (AppSettings.SequencerFavorites != null)
                ObjectFavorites = LeafObjects.Where(x => AppSettings.SequencerFavorites.Contains(x.param_displayname)).ToHashSet();
        }

        public static Dictionary<string, Bitmap> ColorIcons = new();
        public void ImportDefaultColors()
        {
            Dictionary<string, Color> ObjectColors = new();
            if (!File.Exists($@"{AppLocation}\settings\settings\objects_defaultcolors_v3.txt")) {
                File.WriteAllText($@"{AppLocation}\settings\objects_defaultcolors_v3.txt", Properties.Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines($@"{AppLocation}\settings\objects_defaultcolors_v3.txt").ToDictionary(g => g.Split(';')[0], g => Color.FromArgb(int.Parse(g.Split(';')[1])));

            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //once all the colors are processed, assign them directly to the objects
            foreach (Object_Params obj in LeafObjects) {
                obj.defaultcolor = ObjectColors.TryGetValue(obj.param_displayname, out Color value) ? value : Color.Purple;
                Bitmap color = new(16, 16);
                using (Graphics g = Graphics.FromImage(color)) {
                    g.Clear(value);
                }
                ColorIcons.TryAdd(value.ToArgb().ToString(), color);
            }
        }

        ///Color elements based on set properties
        public void ColorFormElements()
        {
            toolStripTitle.BackColor = AppSettings.ColorMainMenuBar;
            panelToolStrips.BackColor = AppSettings.ColorMainSubMenubar;
            dockMain.BackColor = AppSettings.ColorMainBG;

            TCLE.Explorer?.ColorFormElements();

            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LeafEditor)))
                leaf.ColorFormElements();
            foreach (Form_LvlEditor lvl in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LvlEditor)))
                lvl.ColorFormElements();
            foreach (Form_GateEditor gate in TCLE.Documents.Where(x => x.GetType() == typeof(Form_GateEditor)))
                gate.ColorFormElements();
            foreach (Form_MasterEditor master in TCLE.Documents.Where(x => x.GetType() == typeof(Form_MasterEditor)))
                master.ColorFormElements();
            foreach (Form_SampleEditor sample in TCLE.Documents.Where(x => x.GetType() == typeof(Form_SampleEditor)))
                sample.ColorFormElements();
            foreach (Form_RawText raw in TCLE.Documents.Where(x => x.GetType() == typeof(Form_RawText)))
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
                string text = ((JObject)LoadFileLock(file.FullName)).ToString(Formatting.None);
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
            if (WorkingFolder == null)
                visible = false;
            panelToolStrips.Visible = visible;
            dockMain.Visible = visible;
            foreach (object? item in toolStripTitle.Items)
                (item as ToolStripItem).Visible = visible;
            toolstripFile.Visible = true;
            toolstripHelp.Visible = true;
            toolstripFormClose.Visible = true;
            toolstripFormMinimize.Visible = true;
            toolstripFormRestore.Visible = true;
            toolstripFormIcon.Visible = true;
            toolstripExitFullscreen.Visible = TCLE.Fullscreen;
        }

        /// https://stackoverflow.com/questions/3143657/truncate-two-decimal-places-without-rounding#answer-43639947
        public static decimal TruncateDecimal(decimal d, byte decimals)
        {
            decimal r = Math.Round(d, decimals);

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
            int tempsize = TextRenderer.MeasureText(dgvr.HeaderCell.Value.ToString(), dgvr.HeaderCell.Style.Font).Width;
                if (tempsize > biggestheader)
                    biggestheader = tempsize;
            }
            //set header width manually and allow resizing
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgv.RowHeadersWidth = biggestheader + 15;
        }

        ///
        /// File Lock read/write methods
        /// 
        public static void AddFileLock(FileInfo file)
        {
            if (file == null)
                return;
            if (!TCLE.lockedfiles.Any(x => x.Key.FullName == file.FullName)) {
                lockedfiles.Add(file, new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
        }

        public static void WriteFileLock(FileStream fs, JObject _save)
        {
            string tosave = JsonConvert.SerializeObject(_save, Formatting.Indented);
            using (StreamWriter sr = new(fs, System.Text.Encoding.UTF8, tosave.Length, true)) {
                fs.SetLength(0);
                sr.Write(tosave);
            }
        }

        public static void WriteFileLock(FileStream fs, string _save)
        {
            string tosave = _save;
            using (StreamWriter sr = new(fs, System.Text.Encoding.UTF8, tosave.Length, true)) {
                fs.SetLength(0);
                sr.Write(tosave);
            }
        }

        public static dynamic LoadFileLock(string _selectedfilename)
        {
            dynamic _load;
            if (!File.Exists(_selectedfilename))
                return null;
            ///reference:
            ///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
            using (FileStream fileStream = new(_selectedfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader textReader = new(fileStream)) {
                try {
                    _load = JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
                } catch (Exception) {
                    MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.", "File load error");
                    _load = null;
                }
            }

            return _load;
        }

        public static void DeleteFileLock(FileInfo filetodelete)
        {
            if (lockedfiles.TryGetValue(filetodelete, out FileStream? value)) {
                value.Close();
                lockedfiles.Remove(filetodelete);
            }
            filetodelete.Delete();
        }

        public static void CloseFileLock(FileInfo filetoclose)
        {
            if (filetoclose == null)
                return;
            if (lockedfiles.TryGetValue(filetoclose, out FileStream? value)) {
                value.Close();
                lockedfiles.Remove(filetoclose);
            }
        }

        public static void ClearFileLock()
        {
            //clear previously locked files
            foreach (KeyValuePair<FileInfo, FileStream> i in lockedfiles) {
                i.Value.Close();
            }
            lockedfiles.Clear();
        }
        /// 
        /// 
        /// 


        public static string CopyToWorkingFolderCheck(string filepath)
        {
            if (WorkingFolder == null)
                return filepath;

            string dir = Path.GetDirectoryName(filepath);
            string file = Path.GetFileName(filepath);
            if (dir != WorkingFolder.FullName) {
                DialogResult result = MessageBox.Show("That file is not in the current Working Folder. Do you want to copy it here?", "Bumper Custom Level Editor", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) {
                    if (!File.Exists($@"{WorkingFolder}\{file}")) 
                        File.Copy(filepath, $@"{WorkingFolder}\{file}");
                    filepath = $@"{WorkingFolder}\{file}";
                }
                else
                    filepath = null;
            }

            return filepath;
        }

        ///
        ///https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            DirectoryInfo dir = new(sourceDir);

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

        public static List<SampleData> ProjectSamples = new();
        public static Dictionary<string, double> ProjectSampleRuntimes = new();
        public static void ReloadProjectSamples()
        {
            if (WorkingFolder == null)
                return;
            ProjectSamples.Clear();
            //add default empty sample
            ProjectSamples.Add(new SampleData { obj_name = "", path = "", volume = 0, pitch = 0, pan = 0, offset = 0, channel_group = "", File = null });
            string warning = "";
            //iterate over each file
            foreach (FileInfo sampfile in WorkingFolder.GetFiles("*.samp", SearchOption.AllDirectories).Where(x => x.Name != "default.samp")) {
                UpdateProjectSamplesFromFile(sampfile, false, out string _warning);
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
                CalculateSampleRuntimes();
                StopAudio();
            }
            //File.WriteAllLines($@"{AppLocation}\templates\{TCLE.WorkingFolder.Name}_sample_runtimes.temp", ProjectSamples.Select(x => $"{x.obj_name};{x.time}"));
        }

        public static void UpdateProjectSamplesFromFile(FileInfo SampFile, bool preserveSamples, out string warning)
        {
            //remove samples that match the incoming sample file, so that they're rewritten
            ProjectSamples.RemoveAll(x => x.File?.FullName == SampFile.FullName);
            //parse file to JSON
            dynamic _in = TCLE.LoadFileLock(SampFile.FullName);
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

            foreach (Form_LeafEditor leaf in TCLE.Documents.Where(x => x.DockHandler.TabText.Contains(".leaf"))) {
                leaf.BuildObjectTree();
            }
            return;
        }

        public static void CalculateSampleRuntimes()
        {
            foreach (SampleData samp in ProjectSamples.Where(x => x.time == 0)) {
                byte[] _bytes;
                //get the hash of this filename. This will be used to locate the sample's .PC file
                string _hashedname = "";
                byte[] hashbytes = BitConverter.GetBytes(Hash32($"A{samp.path}"));
                Array.Reverse(hashbytes);
                foreach (byte b in hashbytes)
                    _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
                //if the hashed name starts with a '0', remove it
                if (_hashedname[0] == '0')
                    _hashedname = _hashedname[1..];

                //check if sample is custom or not. This changes where we load audio from
                string filetoread;
                try {
                    if (samp.path.Contains("custom"))
                        filetoread = $@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc";
                    else
                        filetoread = $@"{Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc";

                    using (BinaryReader reader = new(new FileStream(filetoread, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))) {
                        reader.ReadUInt32(); //pc header
                        reader.ReadUInt32(); //fsb5 header
                        reader.ReadUInt32(); //version
                        reader.ReadUInt32(); //# of tracks
                        reader.ReadUInt32(); //size of sample header
                        reader.ReadUInt32(); //size of header table
                        uint sampbytes = reader.ReadUInt32(); //sample bytes
                        uint type = reader.ReadUInt32(); //audio type
                        reader.ReadUInt32(); //unknown
                        reader.ReadUInt32(); //flags
                        reader.ReadUInt64(); //hash1
                        reader.ReadUInt64(); //hash2
                        reader.ReadUInt64(); //hash3
                        UInt64 metadata = reader.ReadUInt64(); //metadata

                        UInt64 freqid = (metadata & 0b11110) >> 1;
                        UInt64 samples = metadata >> 34;
                        int freq = Frequencys[(int)freqid];
                        samp.time = (double)(samples) / (double)freq;
                    }
                }
                catch (Exception ex) {
                    samp.time = 0;
                }
            }
        }

        public static void StopAudio()
        {
            Bass.BASS_Free();
            alzheimer();
            TCLE.PlayingChannels.Clear();
            foreach (Form_SampleEditor samp in TCLE.Documents.Where(x => x.GetType() == typeof(Form_SampleEditor))) {
                samp.sampleList.Refresh();
            }
            foreach (Form_LvlEditor lvl in TCLE.Documents.Where(x => x.GetType() == typeof(Form_LvlEditor))) {
                lvl.lvlLoopTracks.Refresh();
            }
            // Initialize Sound library
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_LATENCY, TCLE.Instance.Handle);
        }

        public static string PCtoAudioFile(SampleData _samp)
        {
            if (_samp == null || _samp.obj_name == ".samp")
                return null;
            //check if the gamedir has been set so the method can find the .pc files
            if (Properties.Settings.Default.game_dir == "none") {
                TCLE.Read_Config();
            }

            byte[] _bytes;
            //get the hash of this filename. This will be used to locate the sample's .PC file
            string _hashedname = "";
            byte[] hashbytes = BitConverter.GetBytes(Hash32($"A{_samp.path}"));
            Array.Reverse(hashbytes);
            foreach (byte b in hashbytes)
                _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
            //if the hashed name starts with a '0', remove it
            if (_hashedname[0] == '0')
                _hashedname = _hashedname[1..];

            //check if sample is custom or not. This changes where we load audio from
            if (_samp.path.Contains("custom")) {
                //attempt to locate file. But error and return safely if nothing found
                try {
                    //read the .pc file as bytes, and skip the first 4 header bytes
                    _bytes = File.ReadAllBytes($@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc");
                }
                catch {
                    MessageBox.Show($@"Unable to locate file {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample. Is the custom audio file in the extras folder? You may need to re-import the file.");
                    return null;
                }
            }
            else {
                //attempt to locate file. But error and return safely if nothing found
                try {
                    //read the .pc file as bytes, and skip the first 4 header bytes
                    _bytes = File.ReadAllBytes($@"{Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc");
                }
                catch {
                    MessageBox.Show($@"Unable to locate file {Properties.Settings.Default.game_dir}\{_hashedname}.pc to play sample. If you need to change your Game Directory, go to the the Help menu.");
                    return null;
                }
            }
            if (_bytes.Length == 0)
            {
                MessageBox.Show($@"Unable to properly parse {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample. You may need to re-import the file.");
                return null;
            }
            //check if file has been converted already. Ready the path if true
            if (Directory.GetFiles($@"temp\", $"{_samp.obj_name}.*", SearchOption.AllDirectories).Any()) {
                _samp.TempFile = Directory.GetFiles($@"temp\", $"{_samp.obj_name}.*", SearchOption.AllDirectories).First();
                return _samp.TempFile;
            }
            _bytes = _bytes.Skip(4).ToArray();

            try
            {
                // credit to https://github.com/SamboyCoding/Fmod5Sharp
                FmodSoundBank bank = FsbLoader.LoadFsbFromByteArray(_bytes);
                List<FmodSample> samples = bank.Samples;
                samples[0].RebuildAsStandardFileFormat(out byte[] dataBytes, out string fileExtension);

                string finalfilename = $@"temp\{_samp.obj_name}.{fileExtension}";
                File.WriteAllBytes(finalfilename, dataBytes);
                _samp.TempFile = finalfilename;
                return _samp.TempFile;
            } catch (Exception) {
                MessageBox.Show($@"Unable to properly parse {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample. You may need to re-import the file.");
                return null;
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
            if (_masterlvl.type == "lvl") {
                FileInfo lvl = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{_masterlvl.name}")).Value?.File;
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
            FileInfo lvlrest = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{_masterlvl.rest}")).Value?.File;
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
            FileInfo gate = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{gatename}")).Value?.File;
            if (gate != null) {
                _load = TCLE.LoadFileLock(gate.FullName);
                //if gate not found, _load is null. Return -1 to denote this
                if (_load == null)
                    return -1;
                //check if random is enabled on this gate
                israndom = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
                //loop through each lvl in gate
                foreach (dynamic _lvl in _load["boss_patterns"]) {
                    //attempt to load lvl
                    FileInfo lvl = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{(string)_lvl["lvl_name"]}")).Value?.File;
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

            }
            else
                return -1;

            return _beatcount;
        }
        public static int CalculateLvlRuntime(string path)
        {
            int _beatcount = 0;

            //load the lvl and then loop through its leafs to get beat counts
            dynamic _load = TCLE.LoadFileLock(path);
            if (_load == null)
                return 0;
            foreach (dynamic leaf in _load["leaf_seq"]) {
                FileInfo _leaf = ProjectExplorer.Files.FirstOrDefault(x => x.Value.FullPath.EndsWith($@"\{(leaf["leaf_name"])}")).Value?.File;
                if (_leaf != null && _leaf.Exists)
                    _beatcount += (int)TCLE.LoadFileLock(_leaf.FullName)["beat_cnt"];
                ///_beatcount += (int)leaf["beat_cnt"];
            }
            //every lvl has an approach beats to consider too
            //_beatcount += (int)_load["approach_beats"];

            return _beatcount;
        }

        public static DockContent OpenFile(FileInfo filepath, bool openraw = false, bool ReturnContent = false)
        {
            if (filepath == null)
                return null;

            if (ImageExtensions.Contains(filepath.Extension.ToLower())) {
                Image theimage = null;
                using (FileStream fs = new(filepath.FullName, FileMode.Open)) {
                    theimage = Image.FromStream(fs);
                }
                ImageViewer image = new(theimage) { Text = filepath.Name};
                image.Show();
                return null;
            }

            dynamic _load = LoadFileLock(filepath.FullName);
            if (_load == null)
                return null;
            //if there are no workspaces, add one
            if (!ReturnContent && !Workspaces.Any()) {
                Form_WorkSpace workspace1 = new() { Text = $"Workspace {Workspaces.Count() + 1}", DockAreas = DockAreas.Document };
                workspace1.Show(TCLE.Instance.dockMain, DockState.Document);
            }
            //All methods below this point return true. So we can paint the node green to show it is loaded
            ///TreeNode successNode = TCLE.ProjectExplorer.FindNode(filepath.Name, TCLE.ProjectExplorer.treeView1.Nodes[0].Nodes);
            ///successNode.ForeColor = Color.Green;
        //find if the document is loaded already in a tab
        //if so, make it activate
        openraw:
            IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText.StartsWith(filepath.Name + (openraw ? " [Raw]" : ""))));
            if (workspacehastab != null) {
                workspacehastab.DockHandler.Activate();
                (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText.StartsWith(filepath.Name + (openraw ? " [Raw]" : ""))).DockHandler.Activate();
                return null;
            }

            IEnumerable<Form_WorkSpace> workspacewithfloats = TCLE.Workspaces.Cast<Form_WorkSpace>().Where(w => w.dockMain.FloatWindows.Count > 0);
            foreach(Form_WorkSpace ws in workspacewithfloats) {
                IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).FirstOrDefault();
                if (activate != null) {
                    activate.DockHandler.Activate();
                    return null;
                }
            }
            //open document in raw viewer if that option was selected
            if (openraw) {
                Form_RawText rawtext = new(_load, filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return rawtext;
                rawtext.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            //otherwise, open a standard editor for the document type
            string filetype = filepath.Extension;
            //this finds a pane in the active workspace that has matching extensions already open on it
            DockPane OpenHere = ReturnContent ? null : ActiveWorkspace.dockMain.Panes.FirstOrDefault(x => x.Contents.Where(x => x.DockHandler.TabText.Contains(filetype)).Any());

            if (filetype == ".master") {
                Form_MasterEditor master = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return master;
                if (OpenHere != null) master.Show(OpenHere, null);
                else master.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            else if (filetype == ".lvl") {
                Form_LvlEditor lvl = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return lvl;
                if (OpenHere != null) lvl.Show(OpenHere, null);
                else lvl.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            else if (filetype == ".gate") {
                Form_GateEditor gate = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return gate;
                if (OpenHere != null) gate.Show(OpenHere, null);
                else gate.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            else if (filetype == ".leaf") {
                Form_LeafEditor leaf = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return leaf;
                if (OpenHere != null) leaf.Show(OpenHere, null);
                else leaf.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            else if (filetype == ".samp") {
                Form_SampleEditor sample = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                if (ReturnContent)
                    return sample;
                if (OpenHere != null) sample.Show(OpenHere, null);
                else sample.Show(ActiveWorkspace.dockMain, DockState.Document);
                return null;
            }
            //if file type not supported, open raw
            else {
                openraw = true;
                goto openraw;
            }
        }

        public static void CloseFile(FileInfo filepath)
        {
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
        }

        public static void ReloadLvlsInProject()
        {
            if (WorkingFolder == null)
                return;
            lvlsinworkfolder.Clear();
            foreach (FileInfo file in WorkingFolder.GetFiles("*.lvl", SearchOption.AllDirectories)) {
                dynamic loadfile = LoadFileLock(file.FullName);
                if (loadfile == null) continue;
                if ((string)loadfile["obj_type"] == "SequinLevel") {
                    lvlsinworkfolder.Add((string)loadfile["obj_name"]);
                }
            }
            lvlsinworkfolder.Add("<none>");
            lvlsinworkfolder.Sort();
        }

        public static void FindReloadRaw(string documentname)
        {
            //find if any raw text docs matching documentname are open and update them
            foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(documentname) && x.GetType() == typeof(Form_RawText))) {
                (document as Form_RawText).Reload();
            }
        }

        public static void FindEditorRunMethod(Type editor, string method)
        {
            foreach (IDockContent document in TCLE.Documents.Where(x => x.GetType() == editor)) {
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
                    foreach (IDockContent document in work.dockMain.Documents.Where(x => x.GetType() == type)) {
                        bool save = (bool)document.GetType().GetMethod("IsSaved").Invoke(document, null);
                        if (!save)
                            return true;
                    }
                }
                //closing all in workspace
                else {
                    foreach (IDockContent document in work.dockMain.Documents) {
                        bool save = (bool)document.GetType().GetMethod("IsSaved").Invoke(document, null);
                        if (!save)
                            return true;
                    }
                }
            }
            //closing everything
            else {
                foreach (IDockContent document in TCLE.Documents) {
                    bool save = (bool)document.GetType().GetMethod("IsSaved").Invoke(document, null);
                    if (!save)
                        return true;
                }
            }
            return false;
        }

        public static int mod(int x, int m)
        {
            int r = x % m;
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
                if (LevelDetails.Name.ToUpper() != "LEVEL DETAILS.TXT") {
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
            dynamic ProjectJson = LoadFileLock(LevelDetails.FullName);
            ProjectProperties Convert = new() {
                projectname = (string)ProjectJson["level_name"] ?? "New Project",
                difficulty = (string)ProjectJson["difficulty"] ?? "D0",
                description = (string)ProjectJson["description"] ?? "Please add a description",
                authornames = (string)ProjectJson["author"] ?? "a person",
                bpm = (decimal?)ProjectJson["bpm"] ?? 400m
            };
            ConfigFile = new($@"{LevelDetails.DirectoryName}\config_{Convert.projectname}.txt");
            dynamic ProjectConfig = LoadFileLock(ConfigFile.FullName);
            //load colors, with failover to White
            try {
                Convert.bpm = (decimal?)ProjectConfig["bpm"] ?? 400m;
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
                string[] splitextension = file.Name.Replace(".txt", "").Split('_', 2);
                if (sort)
                    Directory.CreateDirectory($@"{file.DirectoryName}\{splitextension[0]}");

                FileInfo newfile = new($@"{file.DirectoryName}\{(sort ? splitextension[0] + "\\" : "")}{splitextension[1]}.{splitextension[0].ToLower()}");
                File.Move(file.FullName, newfile.FullName);
                //resave leafs and lvls to properly convert the datapoints
                if (newfile.Extension == ".leaf") {
                    dynamic _load = LoadFileLock(newfile.FullName);
                    Form_LeafEditor _leaf = new(_load, newfile, true);
                    _leaf.SaveCheckAndWrite(true, "");
                    CloseFileLock(newfile);
                }
                else if (newfile.Extension == ".lvl") {
                    dynamic _load = LoadFileLock(newfile.FullName);
                    Form_LvlEditor _lvl = new(_load, newfile, true);
                    _lvl.SaveCheckAndWrite(true, "");
                    CloseFileLock(newfile);
                }
                else if (newfile.Extension == ".master") {
                    dynamic _load = LoadFileLock(newfile.FullName);
                    Form_MasterEditor _master = new(_load, newfile, true);
                    _master.SaveCheckAndWrite(true, "");
                    CloseFileLock(newfile);
                }
            }
            //build the JSON to write to file
            JObject _saveJSON = BuildSave(Convert);
            //write JSON to file
            File.WriteAllText($@"{LevelDetails.DirectoryName}\{Convert.projectname}.TCL", JsonConvert.SerializeObject(_saveJSON, Formatting.Indented));
            //locate pyramid_outro
            FileInfo pyramid = LevelDetails.Directory.GetFiles("pyramid_outro.leaf", SearchOption.AllDirectories).FirstOrDefault();
            if (pyramid != null)
                File.WriteAllText($@"{pyramid.FullName}", Properties.Resources.leaf_pyramid_outro);
            else
                File.WriteAllText($@"{LevelDetails.DirectoryName}\pyramid_outro.leaf", Properties.Resources.leaf_pyramid_outro);

            OpenProject(new FileInfo($@"{LevelDetails.DirectoryName}\{Convert.projectname}.TCL"));
        }

        public static List<string> LevelSections;
        public static JObject BuildSave(ProjectProperties _properties)
        {
            JObject _save = new() {
                { "level_name", _properties.projectname },
                { "difficulty", _properties.difficulty },
                { "description", _properties.description },
                {"author", _properties.authornames },
                { "bpm", _properties.bpm },
                { "level_sections", new JArray() {LevelSections} },
                { "rails_color", new JArray() { (float)_properties.rail.R / 255, (float)_properties.rail.G / 255, (float)_properties.rail.B / 255, 1 } },
                { "rails_glow_color", new JArray() { (float)_properties.railglow.R / 255, (float)_properties.railglow.G / 255, (float)_properties.railglow.B / 255, 1}},
                { "path_color", new JArray() { (float)_properties.path.R / 255, (float)_properties.path.G / 255, (float)_properties.path.B / 255, 1 }},
                { "joy_color", new JArray() { 1f, 1f, 1f, 1f } }
            };
            return _save;
        }


        public static void PlaySound(string audiofile)
        {
            if (Properties.Settings.Default.muteapplication)
                return;
            if (rng.Next(0, 1001) == 1000) {
                MemoryStream tempstream = new();
                Properties.Resources.duck.CopyTo(tempstream);
                byte[] duckbytes = tempstream.ToArray();
                PlaySampleOneOff("duck", duckbytes, out _);
            }
            else
                PlaySampleOneOff(audiofile, (byte[])Properties.Resources.ResourceManager.GetObject(audiofile), out _);
            TCLE.alzheimer();
        }
        public static List<Tuple<DataGridView, string, int>> PlayingChannels = new();
        public static int LastChannel;
        public static float initialfreq;
        public static SYNCPROC EndingProc = new(OnEnding);
        public static bool PlaySampleOneOff(DataGridViewCell cell, SampleData _samp, out int SampChannel)
        {
            if (Bass.BASS_ChannelIsActive(PlayingChannels.FirstOrDefault(x => x.Item1 == cell.DataGridView && x.Item2 == cell.DataGridView[1, cell.RowIndex].Value.ToString())?.Item3 ?? 0) == BASSActive.BASS_ACTIVE_STOPPED) {
                string SampleToPlay = TCLE.PCtoAudioFile(_samp);
                if (String.IsNullOrEmpty(SampleToPlay)) {
                    SampChannel = 0;
                    return false;
                }

                //initialize the player and load the sample
                SampChannel = Bass.BASS_StreamCreateFile($@"{SampleToPlay}", 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                _ = Bass.BASS_ChannelSetSync(SampChannel, BASSSync.BASS_SYNC_END, 0, EndingProc, 0);
                //pitch shift and pan
                Bass.BASS_ChannelGetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_FREQ, ref initialfreq);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_FREQ, initialfreq * (float)_samp.pitch);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_PAN, (float)_samp.pan);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_VOL, (float)Properties.Settings.Default.VolKey99 / 100f);
                Bass.BASS_ChannelSetPosition(SampChannel, (double)_samp.offset / 1000d);
                if (_samp.wave == null) {
                    _samp.CalculateRuntime(SampChannel, false);
                    _samp.UpdateRuntime();
                }
                //play the sample
                if (SampChannel != 0 && Bass.BASS_ChannelPlay(SampChannel, false)) {
                    PlayingChannels.Add(new Tuple<DataGridView, string, int>(cell.DataGridView, cell.DataGridView[1, cell.RowIndex].Value.ToString(), SampChannel));
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                Tuple<DataGridView, string, int> ItemToRemove = PlayingChannels.First(x => x.Item1 == cell.DataGridView && x.Item2 == cell.DataGridView[1, cell.RowIndex].Value.ToString());
                SampChannel = ItemToRemove.Item3;
                Bass.BASS_ChannelStop(ItemToRemove.Item3);
                Bass.BASS_ChannelFree(ItemToRemove.Item3);
                PlayingChannels.Remove(ItemToRemove);
                return false;
            }
        }
        public static int PlaySampleOneOff(string samplename, byte[] stream, out int SampChannel)
        {
            //initialize the player and load the sample
            SampChannel = Bass.BASS_SampleLoad(stream, 0, stream.Length, 10, BASSFlag.BASS_SAMPLE_FLOAT);
            SampChannel = Bass.BASS_SampleGetChannel(SampChannel, BASSFlag.BASS_SAMPLE_FLOAT);
            _ = Bass.BASS_ChannelSetSync(SampChannel, BASSSync.BASS_SYNC_END, 0, EndingProc, IntPtr.Zero);
            //play the sample
            if (SampChannel != 0 && Bass.BASS_ChannelPlay(SampChannel, false)) {
                return SampChannel;
            }
            else {
                return SampChannel = 0;
            }
        }

        private static void OnEnding(int handle, int channel, int data, IntPtr user)
        {
            bool free1 = Bass.BASS_ChannelStop(channel);
            bool free2 = Bass.BASS_ChannelFree(channel);
            Tuple<DataGridView, string, int>? ItemToRemove = PlayingChannels.FirstOrDefault(x => x.Item3 == channel);
            if (ItemToRemove != null) {
                ItemToRemove.Item1.InvalidateColumn(0);
                PlayingChannels.Remove(ItemToRemove);
                if (TCLE.PlayingChannels.Count > 0)
                    LastChannel = PlayingChannels.Last().Item3;
            }
            TCLE.alzheimer();
        }

        public static void GenerateSampWave(SampleData samp, int channel)
        {
            WaveForm wave = new(samp.TempFile) {
                DrawWaveForm = WaveForm.WAVEFORMDRAWTYPE.DualMono
            };
            //math to figure out how long the sample is, in seconds and dimensions
            long len = Bass.BASS_ChannelGetLength(channel, BASSMode.BASS_POS_BYTE);
            samp.time = Bass.BASS_ChannelBytes2Seconds(channel, len);/* - ((double)samp.offset / 1000d)) / (double)samp.pitch;*/
            //render wave
            wave.RenderStart(false, BASSFlag.BASS_SAMPLE_FLOAT);
            samp.wave = wave;
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
        
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
            return files.Where(f => extensions.Contains(f.Extension));
        }
    }

}