using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using Thumper_Custom_Level_Editor.Editor_Panels;
using System.Runtime.CompilerServices;
using WeifenLuo.WinFormsUI.Docking;
using System.Diagnostics.Metrics;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
    {
        public static void InitializeTracks(DataGridView grid, bool columnstyle)
        {
            //double buffering for DGV, found here: https://10tec.com/articles/why-datagridview-slow.aspx
            //used to significantly improve rendering performance
            if (!SystemInformation.TerminalServerSession) {
                Type dgvType = grid.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(grid, true, null);
            }

            if (columnstyle)
                GenerateColumnStyle(grid, grid.ColumnCount);
        }

        public static void GenerateColumnStyle(DataGridView grid, int _cells)
        {
            //stylize track grid/columns
            for (int i = 0; i < _cells; i++) {
                grid.Columns[i].Name = i.ToString();
                grid.Columns[i].Resizable = DataGridViewTriState.False;
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                grid.Columns[i].DividerWidth = 1;
                grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                grid.Columns[i].Frozen = false;
                grid.Columns[i].MinimumWidth = 2;
                grid.Columns[i].ReadOnly = false;
                grid.Columns[i].ValueType = typeof(decimal?);
                grid.Columns[i].DefaultCellStyle.Format = "0.###";
                grid.Columns[i].FillWeight = 0.001F;
                grid.Columns[i].DefaultCellStyle.Font = new Font("Consolas", 8);
            }
        }

        public static HashSet<Object_Params> LeafObjects = new();
        string _errorlog = "";
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
            List<string> import = (File.ReadAllText($@"{AppLocation}\templates\track_objects2.2.txt")).Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
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
                            step = import3[4],
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
            foreach (var dc in dockMain.Documents.Where(x => x.DockHandler.TabText.EndsWith(".leaf"))) {
                Form_LeafEditor fle = dc as Form_LeafEditor;
                fle.dropObjects.DataSource = LeafObjects.Select(x => x.category).Distinct().ToList();
                fle.dropObjects.SelectedIndex = -1;
                fle.dropParamPath.Enabled = false;
            }
        }

        public static Dictionary<string, string> ObjectColors = new();
        public void ImportDefaultColors()
        {
            ObjectColors.Clear();
            if (!File.Exists($@"{AppLocation}\templates\objects_defaultcolors2.2.txt")) {
                File.WriteAllText($@"{AppLocation}\templates\objects_defaultcolors2.2.txt", Properties.Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines($@"{AppLocation}\templates\objects_defaultcolors2.2.txt").ToDictionary(g => g.Split(';')[0], g => g.Split(';')[1]);

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
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
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
            CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
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
            lblChangelog.Text = Properties.Resources.changelog;
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
            if (lockedfiles.ContainsKey(filetodelete)) {
                lockedfiles[filetodelete].Close();
                lockedfiles.Remove(filetodelete);
            }
            filetodelete.Delete();
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


        public static string CopyToWorkingFolderCheck(string filepath, string workingfolder)
        {
            if (workingfolder == null)
                return filepath;

            string dir = Path.GetDirectoryName(filepath);
            string file = Path.GetFileName(filepath);
            if (dir != workingfolder) {
                DialogResult result = MessageBox.Show("That file is not in the current Working Folder. Do you want to copy it here?\nOr not, and open that level folder?\n\nYES = copy\nNO = open that level folder\nCANCEL = do nothing", "Confirm?", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    if (!File.Exists($@"{workingfolder}\{file}")) File.Copy(filepath, $@"{workingfolder}\{file}");
                    filepath = $@"{workingfolder}\{file}";
                }
                else if (result == DialogResult.No) {

                }
                else
                    filepath = null;
            }

            return filepath;
        }
        /*
        public static void HighlightMissingFile(DataGridView dgv, List<string> filelist)
        {
            List<dynamic> files = Directory.GetFiles(TCLE.WorkingFolder, "*.*", SearchOption.AllDirectories);
            foreach (DataGridViewRow dgvr in dgv.Rows) {
                string filename = $"{dgvr.Cells[1].Value.ToString()}.txt";
                if (Directory.GetFiles(TCLE.WorkingFolder, $"{dgvr.Cells[1].Value.ToString()}.txt", SearchOption.AllDirectories).Any()) { 
                    dgvr.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
                    dgvr.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                }
                else {
                    dgvr.DefaultCellStyle.BackColor = Color.Maroon;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gray;
                }
            }
        }
        */
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

        public static List<SampleData> _lvlsamples = new();
        public static void LvlReloadSamples()
        {
            if (WorkingFolder == null)
                return;
            _lvlsamples.Clear();
            //find all samp_ files in the level folder
            List<FileInfo> _sampfiles = WorkingFolder.GetFiles("*.samp", SearchOption.AllDirectories).Where(x => x.Name != "default.samp").ToList();
            //add default empty sample
            _lvlsamples.Add(new SampleData { obj_name = "", path = "", volume = 0, pitch = 0, pan = 0, offset = 0, channel_group = "" });
            //iterate over each file
            foreach (FileInfo sampfile in _sampfiles) {
                //parse file to JSON
                dynamic _in = TCLE.LoadFileLock(sampfile.FullName);
                //iterate over items:[] list to get each sample and add names to list
                foreach (dynamic _samp in _in["items"]) {
                    _lvlsamples.Add(new SampleData {
                        obj_name = ((string)_samp["obj_name"]).Replace(".samp", ""),
                        path = _samp["path"],
                        volume = _samp["volume"],
                        pitch = _samp["pitch"],
                        pan = _samp["pan"],
                        offset = _samp["offset"],
                        channel_group = _samp["channel_group"]
                    });
                }
            }
            _lvlsamples = _lvlsamples.OrderBy(w => w.obj_name).ToList();
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

        public static int CalculateMasterRuntime(Form_MasterEditor master)
        {
            int _beatcount = 0;
            //loop through all entries in the master to get beat counts
            foreach (MasterLvlData _masterlvl in master._masterlvls) {
                int _beats = CalculateSingleLvlRuntime(_masterlvl);
                if (_beats != -1) _beatcount += _beats;
            }
            if (master._properties.introlvl != "<none>") {
                var introlvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{master._properties.introlvl}"));
                if (introlvl.Key != null) _beatcount += LoadLvlGetBeatCounts(introlvl.Value.FullName);
            }
            if (master._properties.checkpointlvl != "<none>") {
                var checkpointlvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{master._properties.checkpointlvl}"));
                if (checkpointlvl.Key != null) _beatcount += LoadLvlGetBeatCounts(checkpointlvl.Value.FullName);
            }

            return _beatcount;

        }
        public static int CalculateSingleLvlRuntime(MasterLvlData _masterlvl)
        {
            dynamic _load;
            int _beatcount = 0;
            if (_masterlvl.type == "lvl") {
                var lvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{_masterlvl.lvlname}"));
                if (lvl.Key != null) _beatcount += LoadLvlGetBeatCounts(lvl.Value.FullName);
                else return -1;
            }
            //this section handles gate
            else {
                //load the gate to then loop through all lvls in it
                var gate = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{_masterlvl.gatename}"));
                if (gate.Key != null) {
                    _load = TCLE.LoadFileLock(gate.Value.FullName);
                    if (_load == null)
                        return -1;
                    foreach (dynamic _lvl in _load["boss_patterns"]) {
                        var lvl = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{(string)_lvl["lvl_name"]}"));
                        if (lvl.Key != null) _beatcount += LoadLvlGetBeatCounts(lvl.Value.FullName);
                    }

                }
            }
            var lvlrest = TCLE.dockProjectExplorer.projectfiles.FirstOrDefault(x => x.Key.EndsWith($@"\{_masterlvl.rest}"));
            if (lvlrest.Key != null) _beatcount += LoadLvlGetBeatCounts(lvlrest.Value.FullName);

            return _beatcount;
        }
        private static int LoadLvlGetBeatCounts(string path)
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
            if (form.dockMain.Documents.Where(x => x.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).Any()) {
                form.dockMain.Documents.Where(x => x.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).First().DockHandler.Activate();
                return;
            }
            /*
            if (form.dockMain.FloatWindows.Where(x => x.DockPanel.Documents.Where(x => x.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).Any()).Any()) {
                form.dockMain.FloatWindows.Where(x => x.DockPanel.Documents.Where(x => x.DockHandler.TabText == filepath.Name + (openraw ? " [Raw]" : "")).Any()).First().Activate();
                return;
            }*/
            //open document in raw viewer if that option was selected
            if (openraw) {
                Form_RawText rawtext = new(_load, filepath) { Text = filepath.Name + " [Raw]", DockAreas = DockAreas.Document | DockAreas.Float };
                rawtext.Show(lastclickedpane, null);
                return;
            }
            //otherwise, open a standard editor for the document type
            string filetype = filepath.Extension;
            if (filetype == ".master") {
                Form_MasterEditor master = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                master.Show(lastclickedpane, null);
            }
            else if (filetype == ".lvl") {
                Form_LvlEditor lvl = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                lvl.Show(lastclickedpane, null);
            }
            else if (filetype == ".gate") {
                Form_GateEditor gate = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                gate.Show(lastclickedpane, null);
            }
            else if (filetype == ".leaf") {
                Form_LeafEditor leaf = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                leaf.Show(lastclickedpane, null);
            }
            else if (filetype == ".samp") {
                Form_SampleEditor sample = new(_load, filepath) { DockAreas = DockAreas.Document | DockAreas.Float };
                sample.Show(lastclickedpane, null);
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
    }
}