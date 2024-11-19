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

        public HashSet<Object_Params> _objects = new();
        string _errorlog = "";
        public void ImportObjects()
        {
            _objects.Clear();
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
                        _objects.Add(objpar);
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
            /*
            //customize combobox to display the correct content
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropObjects.SelectedIndex = -1;
            //dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
            dropParamPath.Enabled = false;
            */
        }

        private Dictionary<string, string> objectcolors = new();
        public void ImportDefaultColors()
        {
            objectcolors.Clear();
            if (!File.Exists($@"{AppLocation}\templates\objects_defaultcolors2.2.txt")) {
                File.WriteAllText($@"{AppLocation}\templates\objects_defaultcolors2.2.txt", Properties.Resources.objects_defaultcolors);
            }
            objectcolors = File.ReadAllLines($@"{AppLocation}\templates\objects_defaultcolors2.2.txt").ToDictionary(g => g.Split(';')[0], g => g.Split(';')[1]);

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

        private void UpdateLevelLists()
        {
            lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt", SearchOption.AllDirectories).Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList() ?? new List<string>();
            lvlsinworkfolder.Add("<none>");
            lvlsinworkfolder.Sort();
            /*
            dropMasterCheck.SelectedIndexChanged -= dropMasterCheck_SelectedIndexChanged;
            dropMasterIntro.SelectedIndexChanged -= dropMasterIntro_SelectedIndexChanged;
            dropMasterLvlRest.SelectedIndexChanged -= dropMasterLvlRest_SelectedIndexChanged;
            dropGatePre.SelectedIndexChanged -= dropGatePre_SelectedIndexChanged;
            dropGatePost.SelectedIndexChanged -= dropGatePost_SelectedIndexChanged;
            dropGateRestart.SelectedIndexChanged -= dropGateRestart_SelectedIndexChanged;
            //add lvl list as datasources to dropdowns
            dropMasterCheck.DataSource = lvlsinworkfolder.ToList();
            dropMasterIntro.DataSource = lvlsinworkfolder.ToList();
            dropMasterLvlRest.DataSource = lvlsinworkfolder.ToList();
            dropGatePre.DataSource = lvlsinworkfolder.ToList();
            dropGatePost.DataSource = lvlsinworkfolder.ToList();
            dropGateRestart.DataSource = lvlsinworkfolder.ToList();
            //
            dropMasterCheck.SelectedIndexChanged += dropMasterCheck_SelectedIndexChanged;
            dropMasterIntro.SelectedIndexChanged += dropMasterIntro_SelectedIndexChanged;
            dropMasterLvlRest.SelectedIndexChanged += dropMasterLvlRest_SelectedIndexChanged;
            dropGatePre.SelectedIndexChanged += dropGatePre_SelectedIndexChanged;
            dropGatePost.SelectedIndexChanged += dropGatePost_SelectedIndexChanged;
            dropGateRestart.SelectedIndexChanged += dropGateRestart_SelectedIndexChanged;
            */
        }

        public string SearchReferences(dynamic _load, string filepath)
        {
            string referencefiles = "";
            //search all files in the project folder
            foreach (string file in Directory.GetFiles(workingfolder).Where(x => Path.GetFileName(x).StartsWith("leaf_") || Path.GetFileName(x).StartsWith("lvl_") || Path.GetFileName(x).StartsWith("gate_") || Path.GetFileName(x).StartsWith("master_"))) {
                //skip self to not include self
                if (file == filepath)
                    continue;
                string text = ((JObject)LoadFileLock(file)).ToString(Formatting.None);
                //check if the file we're searching contains the obj_name
                if (text.Contains($"{_load["obj_name"]}")) {
                    referencefiles += Path.GetFileNameWithoutExtension(file) + '\n';
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

        public static void PanelEnableState(Control panel, bool enablestate)
        {
            foreach (Control _c in panel.Controls.Cast<Control>().Where(x => x.GetType() != typeof(Label))) {
                if (_c.Text != "titlebar")
                    _c.Enabled = enablestate;
            }
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

        public static void DeleteFileLock(string _selectedfilename, string filetype)
        {
            if (lockedfiles.ContainsKey(_selectedfilename)) {
                lockedfiles[_selectedfilename].Close();
                lockedfiles.Remove(_selectedfilename);
            }
            File.Delete(_selectedfilename);
        }

        public static void ClearFileLock()
        {
            //clear previously locked files
            foreach (KeyValuePair<string, FileStream> i in lockedfiles) {
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
            List<string> _sampfiles = Directory.GetFiles(WorkingFolder, "samp_*.txt").Where(x => !x.Contains("samp_default")).ToList();
            //add default empty sample
            _lvlsamples.Add(new SampleData { obj_name = "", path = "", volume = 0, pitch = 0, pan = 0, offset = 0, channel_group = "" });
            //iterate over each file
            foreach (string f in _sampfiles) {
                //parse file to JSON
                dynamic _in = TCLE.LoadFileLock(f);
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

        public static int CalculateMasterRuntime(string workingfolder, Form_MasterEditor master)
        {
            int _beatcount = 0;
            //loop through all entries in the master to get beat counts
            foreach (MasterLvlData _masterlvl in master._masterlvls) {
                _beatcount += CalculateSingleLvlRuntime(workingfolder, _masterlvl);
            }
            if (master._properties.introlvl != "<none>")
                _beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{Path.GetFileNameWithoutExtension(master._properties.introlvl)}.txt");
            if (master._properties.checkpointlvl != "<none>")
                _beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{Path.GetFileNameWithoutExtension(master._properties.checkpointlvl)}.txt");

            ///lblMAsterRuntimeBeats.Text = $"Beats: {_beatcount}";

            ///Calculate min/sec based on beats and BPM
            ///lblMasterRuntime.Text = $"Time: {TimeSpan.FromMinutes(_beatcount / (double)_properties.bpm).ToString("hh':'mm':'ss'.'fff")}";
            return _beatcount;

        }
        public static int CalculateSingleLvlRuntime(string workingfolder, MasterLvlData _masterlvl)
        {
            dynamic _load;
            int _beatcount = 0;
            if (_masterlvl.type == "lvl") {
                string file = Directory.GetFiles(workingfolder, $"lvl_{_masterlvl.name}.txt", SearchOption.AllDirectories).FirstOrDefault();
                //load the lvl and then loop through its leafs to get beat counts
                if (file != null)
                    _beatcount += LoadLvlGetBeatCounts(file);
            }
            //this section handles gate
            else {
                //load the gate to then loop through all lvls in it
                _load = TCLE.LoadFileLock($"{workingfolder}\\gate_{_masterlvl.name}.txt");
                if (_load == null)
                    return 0;
                foreach (dynamic _lvl in _load["boss_patterns"]) {
                    //load the lvl and then loop through its leafs to get beat counts
                    int idx = ((string)_lvl["lvl_name"]).LastIndexOf('.');
                    _beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{((string)_lvl["lvl_name"])[..idx]}.txt");
                }
            }

            if (_masterlvl.rest is not "" and not "<none>" and not null)
                _beatcount += LoadLvlGetBeatCounts($"{workingfolder}\\lvl_{Path.GetFileNameWithoutExtension(_masterlvl.rest)}.txt");

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

        public static void OpenFile(TCLE form, string filepath)
        {
            dynamic _load = LoadFileLock(filepath);
            string name = _load["obj_name"];
            if (form.dockMain.Documents.Where(x => x.DockHandler.TabText == name).Any()) {
                form.dockMain.Documents.Where(x => x.DockHandler.TabText == name).First().DockHandler.Activate();
                return;
            }
            string _type = _load["obj_type"];
            if (_type == "SequinMaster") {
                Form_MasterEditor master = new(_load, filepath);
                master.Show(form.dockMain, DockState.Document);
            }
        }

        public static void ReloadLvlsInProject()
        {
            if (WorkingFolder == null)
                return;
            lvlsinworkfolder.Clear();
            foreach (string file in Directory.GetFiles(WorkingFolder, "*", SearchOption.AllDirectories)) {
                dynamic loadfile = LoadFileLock(file);
                if ((string)loadfile["obj_type"] == "SequinLevel") {
                    lvlsinworkfolder.Add((string)loadfile["obj_name"]);
                }
            }
            lvlsinworkfolder.Add("<none>");
            lvlsinworkfolder.Sort();

            PlaySound("UIrefresh");
        }
    }
}