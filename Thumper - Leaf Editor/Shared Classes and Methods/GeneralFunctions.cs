using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.Vorbis;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        public static List<string> TimeSignatures = new() { "2/4", "3/4", "4/4", "5/4", "5/8", "6/8", "7/8", "8/8", "9/8" };
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

        private void LoadQuickValues()
        {
            if (!File.Exists($@"{TCLE.AppLocation}\templates\quickvalues.txt"))
                return;
            string[] _load = File.ReadAllLines($@"{TCLE.AppLocation}\templates\quickvalues.txt");

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
                dgvc.DividerWidth = 1;
                dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvc.Frozen = false;
                dgvc.MinimumWidth = 2;
                dgvc.ReadOnly = false;
                dgvc.ValueType = typeof(decimal?);
                dgvc.DefaultCellStyle.Format = "0.###";
                dgvc.FillWeight = 0.001F;
                dgvc.DefaultCellStyle.Font = new Font("Consolas", 8);
                dgvc.ReadOnly = false;
            }
        }

        public static HashSet<Object_Params> LeafObjects = new();
        private string _errorlog = "";
        public void ImportObjects()
        {
            LeafObjects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists($@"{AppLocation}\templates\track_objects2.2.txt")) {
                File.WriteAllText($@"{AppLocation}\templates\track_objects2.2.txt", Properties.Resources.track_objects);
            }
            //import default colors per object
            ImportDefaultColors();

            ///import selectable objects from file and parse them into lists for manipulation
            //splits input at "###". Each section is a collection of param_paths
            List<string> import = File.ReadAllText($@"{AppLocation}\templates\track_objects2.2.txt").Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
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
                        };
                        //finally, add complete object and values to list
                        LeafObjects.Add(objpar);
                    }
                    catch {
                        _errorlog += "failed to import all properties of param_path " + import3[0] + " of object " + import2[0] + ".\n";
                    }
                }
            }
            //show errors to user if any imports failed
            if (_errorlog.Length > 1) {
                MessageBox.Show(_errorlog);
                _errorlog = "";
            }

            //iterate over each open tab to find leafs. Update their object lists.
            foreach (IDockContent? dc in dockMain.Documents.Where(x => x.DockHandler.TabText.EndsWith(".leaf"))) {
                Form_LeafEditor fle = dc as Form_LeafEditor;
                fle.dropObjects.DataSource = LeafObjects.Select(x => x.category).Distinct().ToList();
                fle.dropObjects.SelectedIndex = -1;
                fle.dropParamPath.Enabled = false;
            }
        }

        public static Dictionary<string, Color> ObjectColors = new();
        public void ImportDefaultColors()
        {
            ObjectColors.Clear();
            if (!File.Exists($@"{AppLocation}\templates\objects_defaultcolors2.2.txt")) {
                File.WriteAllText($@"{AppLocation}\templates\objects_defaultcolors2.2.txt", Properties.Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines($@"{AppLocation}\templates\objects_defaultcolors2.2.txt").ToDictionary(g => g.Split(';')[0], g => Color.FromArgb(int.Parse(g.Split(';')[1])));

            colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
        }

        ///Color elements based on set properties
        private static void ColorFormElements()
        {
            /*
            if (File.Exists($@"{AppLocation}\templates\UIcolorprefs.txt")) {
                string[] colors = File.ReadAllLines($@"{AppLocation}\templates\UIcolorprefs.txt");
                Properties.Settings.Default.custom_bgcolor = this.BackColor = Color.FromArgb(int.Parse(colors[0]));
                Properties.Settings.Default.custom_menucolor = toolStripTitle.BackColor = Color.FromArgb(int.Parse(colors[1]));
                Properties.Settings.Default.custom_mastercolor = panelMaster.BackColor = Color.FromArgb(int.Parse(colors[2]));
                Properties.Settings.Default.custom_gatecolor = panelGate.BackColor = Color.FromArgb(int.Parse(colors[3]));
                Properties.Settings.Default.custom_lvlcolor = panelLevel.BackColor = Color.FromArgb(int.Parse(colors[4]));
                Properties.Settings.Default.custom_leafcolor = panelLeaf.BackColor = Color.FromArgb(int.Parse(colors[5]));
                Properties.Settings.Default.custom_samplecolor = panelSample.BackColor = Color.FromArgb(int.Parse(colors[6]));
                Properties.Settings.Default.custom_activecolor = Color.FromArgb(int.Parse(colors[7]));
                Properties.Settings.Default.Save();
            }
            else {
                this.BackColor = Properties.Settings.Default.custom_bgcolor;
                toolStripTitle.BackColor = Properties.Settings.Default.custom_menucolor;
                panelLeaf.BackColor = Properties.Settings.Default.custom_leafcolor;
                panelLevel.BackColor = Properties.Settings.Default.custom_lvlcolor;
                panelGate.BackColor = Properties.Settings.Default.custom_gatecolor;
                panelMaster.BackColor = Properties.Settings.Default.custom_mastercolor;
                panelSample.BackColor = Properties.Settings.Default.custom_samplecolor;
            }
            */
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

        public static void PlaySound(string audiofile)
        {
            if (Properties.Settings.Default.muteapplication)
                return;
            Stream stream = new MemoryStream((byte[])Properties.Resources.ResourceManager.GetObject(audiofile));
            VorbisWaveReader vorbisStream = new(stream);
            WaveOut waveOut = new();
            waveOut.Init(vorbisStream);
            waveOut.Volume = 1;
            waveOut.Play();
        }

        /// Used to allow only numbers and a single decimal during input
        public static void NumericInputSanitize(object sender, KeyPressEventArgs e)
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

        private static List<string> extensions = new() { ".leaf", ".lvl", ".gate", ".master"};
        public static string SearchReferences(string searchreference)
        {
            string referencefiles = "";
            //search all files in the project folder
            foreach (FileInfo file in WorkingFolder.GetFiles("*", SearchOption.AllDirectories).Where(x => extensions.Contains(x.Extension))) {
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

        ///
        /// File Lock read/write methods
        /// 
        public static void WriteFileLock(FileStream fs, JObject _save)
        {
            string tosave = JsonConvert.SerializeObject(_save, Formatting.Indented);
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

        public static List<SampleData> LvlSamples = new();
        public static void LvlReloadSamples()
        {
            if (WorkingFolder == null)
                return;
            LvlSamples.Clear();
            //add default empty sample
            LvlSamples.Add(new SampleData { obj_name = "", path = "", volume = 0, pitch = 0, pan = 0, offset = 0, channel_group = "" });
            //iterate over each file
            foreach (FileInfo sampfile in WorkingFolder.GetFiles("*.samp", SearchOption.AllDirectories).Where(x => x.Name != "default.samp")) {
                //parse file to JSON
                dynamic _in = TCLE.LoadFileLock(sampfile.FullName);
                //skip if somehow empty
                if (_in == null || !_in.ContainsKey("items"))
                    continue;
                //iterate over items:[] list to get each sample and add names to list
                foreach (dynamic _samp in _in["items"]) {
                    LvlSamples.Add(new SampleData {
                        obj_name = ((string)_samp["obj_name"]),
                        path = _samp["path"],
                        volume = _samp["volume"],
                        pitch = _samp["pitch"],
                        pan = _samp["pan"],
                        offset = _samp["offset"],
                        channel_group = _samp["channel_group"]
                    });
                }
            }
            LvlSamples = LvlSamples.OrderBy(w => w.obj_name).ToList();
            /*
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DataSource = _lvlsamples.Select(x => x.obj_name).ToList();
            //this is for adjusting the dropdown width so that the full item can display
            int width = 0;
            Graphics g = lvlLoopTracks.CreateGraphics();
            Font font = lvlLoopTracks.DefaultCellStyle.Font;
            foreach (SampleData s in _lvlsamples) {
                int newWidth = (int)g.MeasureString(s.obj_name, font).Width;
                if (width < newWidth) {
                    width = newWidth;
                }
            }
            ((DataGridViewComboBoxColumn)lvlLoopTracks.Columns[0]).DropDownWidth = width + 20;
            */
        }

        public static string PCtoOGG(SampleData _samp)
        {
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
                    MessageBox.Show($@"Unable to locate file {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample. Is the custom audio file in the extras folder?");
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
            _bytes = _bytes.Skip(4).ToArray();

            // credit to https://github.com/SamboyCoding/Fmod5Sharp
            FmodSoundBank bank = FsbLoader.LoadFsbFromByteArray(_bytes);
            List<FmodSample> samples = bank.Samples;
            samples[0].RebuildAsStandardFileFormat(out byte[] dataBytes, out string fileExtension);

            File.WriteAllBytes($@"temp\{_samp.obj_name}.{fileExtension}", dataBytes);
            return fileExtension;
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

        public static int CalculateSublevelRuntime(MasterLvlData _masterlvl)
        {
            int _beatcount = 0;
            if (_masterlvl.type == "lvl") {
                KeyValuePair<string, FileInfo> lvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{_masterlvl.name}"));
                if (lvl.Key != null) _beatcount += CalculateLvlRuntime(lvl.Value.FullName);
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
            KeyValuePair<string, FileInfo> lvlrest = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{_masterlvl.rest}"));
            if (lvlrest.Key != null) _beatcount += CalculateLvlRuntime(lvlrest.Value.FullName);

            return _beatcount;
        }

        public static int CalculateGateRuntimeFromFile(string gatename)
        {
            dynamic _load;
            int _beatcount = 0;
            List<int> bucketscounted = new();
            bool israndom;
            //load the gate to then loop through all lvls in it
            KeyValuePair<string, FileInfo> gate = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{gatename}"));
            if (gate.Key != null) {
                _load = TCLE.LoadFileLock(gate.Value.FullName);
                //if gate not found, _load is null. Return -1 to denote this
                if (_load == null)
                    return -1;
                //check if random is enabled on this gate
                israndom = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
                //loop through each lvl in gate
                foreach (dynamic _lvl in _load["boss_patterns"]) {
                    //attempt to load lvl
                    KeyValuePair<string, FileInfo> lvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{(string)_lvl["lvl_name"]}"));
                    if (lvl.Key != null) {
                        //if random is enabled, count only the first entry in each bucket
                        if (israndom) {
                            if (!bucketscounted.Contains((int)_lvl["bucket_num"])) {
                                bucketscounted.Add((int)_lvl["bucket_num"]);
                                _beatcount += CalculateLvlRuntime(lvl.Value.FullName);
                            }
                        }
                        //otherwise count each lvl
                        else
                            _beatcount += CalculateLvlRuntime(lvl.Value.FullName);
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
                _beatcount += (int)leaf["beat_cnt"];
            }
            //every lvl has an approach beats to consider too
            //_beatcount += (int)_load["approach_beats"];

            return _beatcount;
        }

        public static void OpenFile(TCLE form, FileInfo filepath, bool openraw = false)
        {
            if (filepath == null)
                return;

            dynamic _load = LoadFileLock(filepath.FullName);
        //find if the document is loaded already in a tab
        //if so, make it activate
        openraw:
            IDockContent workspacehastab = TCLE.Workspaces.FirstOrDefault(x => (x as Form_WorkSpace).dockMain.Documents.Any(y => y.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")));
            if (workspacehastab != null) {
                workspacehastab.DockHandler.Activate();
                (workspacehastab as Form_WorkSpace).dockMain.Documents.First(y => y.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).DockHandler.Activate();
                return;
            }

            var workspacewithfloats = TCLE.Workspaces.Cast<Form_WorkSpace>().Where(w => w.dockMain.FloatWindows.Count > 0);
            foreach(Form_WorkSpace ws in workspacewithfloats) {
                IDockContent activate = ws.dockMain.FloatWindows.SelectMany(x => x.NestedPanes).SelectMany(y => y.Contents).Where(z => z.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).FirstOrDefault();
                if (activate != null) {
                    activate.DockHandler.Activate();
                    return;
                }
            }
            //open document in raw viewer if that option was selected
            if (openraw) {
                Form_RawText rawtext = new(_load, filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                rawtext.Show(ActiveWorkspace.dockMain, DockState.Document);
                return;
            }
            //otherwise, open a standard editor for the document type
            string filetype = filepath.Extension;
            if (filetype == ".master") {
                Form_MasterEditor master = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                master.Show(ActiveWorkspace.dockMain, DockState.Document);
            }
            else if (filetype == ".lvl") {
                Form_LvlEditor lvl = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                lvl.Show(ActiveWorkspace.dockMain, DockState.Document);
            }
            else if (filetype == ".gate") {
                Form_GateEditor gate = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                gate.Show(ActiveWorkspace.dockMain, DockState.Document);
            }
            else if (filetype == ".leaf") {
                Form_LeafEditor leaf = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                leaf.Show(ActiveWorkspace.dockMain, DockState.Document);
            }
            else if (filetype == ".samp") {
                Form_SampleEditor sample = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                sample.Show(ActiveWorkspace.dockMain, DockState.Document);
            }
            //if file type not supported, open raw
            else {
                openraw = true;
                goto openraw;
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

        public bool AnyUnsaved()
        {
            foreach (IDockContent document in TCLE.Documents) {
                bool save = (bool)document.GetType().GetMethod("IsSaved").Invoke(document, null);
                if (!save)
                    return true;
            }
            return false;
        }

        public int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
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
    }
}