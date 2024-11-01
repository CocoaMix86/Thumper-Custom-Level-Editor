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

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor : Form
    {
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
            //customize combobox to display the correct content
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropObjects.SelectedIndex = -1;
            //dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
            dropParamPath.Enabled = false;
        }
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
        private void ColorFormElements()
        {
            if (File.Exists($@"{AppLocation}\templates\UIcolorprefs.txt")) {
                string[] colors = File.ReadAllLines($@"{AppLocation}\templates\UIcolorprefs.txt");
                Properties.Settings.Default.custom_bgcolor = this.BackColor = Color.FromArgb(int.Parse(colors[0]));
                Properties.Settings.Default.custom_menucolor = menuStrip.BackColor = Color.FromArgb(int.Parse(colors[1]));
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
                menuStrip.BackColor = Properties.Settings.Default.custom_menucolor;
                panelLeaf.BackColor = Properties.Settings.Default.custom_leafcolor;
                panelLevel.BackColor = Properties.Settings.Default.custom_lvlcolor;
                panelGate.BackColor = Properties.Settings.Default.custom_gatecolor;
                panelMaster.BackColor = Properties.Settings.Default.custom_mastercolor;
                panelSample.BackColor = Properties.Settings.Default.custom_samplecolor;
            }
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

        private void ClearPanels(string panel = "all")
        {
            //clear lists used for storing level data
            if (panel is "all" or "leaf") {
                _loadedleaf = null;
                PanelEnableState(panelLeaf, false);
            }
            if (panel is "all" or "lvl") {
                _loadedlvl = null;
                PanelEnableState(panelLevel, false);
            }
            if (panel is "all" or "gate") {
                _loadedgate = null;
                PanelEnableState(panelGate, false);
            }
            if (panel is "all" or "master") {
                _loadedmaster = null;
                PanelEnableState(panelMaster, false);
            }
            if (panel is "all" or "samp") {
                _loadedsample = null;
                PanelEnableState(panelSample, false);
            }
        }

        public void PlaySound(string audiofile)
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
        private static void NumericInputSanitize(object sender, KeyPressEventArgs e)
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
        private void AllowArrowMovement(object sender, PreviewKeyDownEventArgs e)
        {
           if (e.KeyCode is Keys.Right or Keys.Left or Keys.Up or Keys.Down) {
                if (trackEditor.IsCurrentCellInEditMode) {
                    if ((string)trackEditor.CurrentCell.EditedFormattedValue == "") {
                        trackEditor.CurrentCell.Value = null;
                        trackEditor.CancelEdit();
                        if (e.KeyCode is Keys.Right or Keys.Left)
                            trackEditor.EndEdit();
                    }
                }
           }
        }
        private void trackEditor_Click(object sender, EventArgs e)
        {
            if (trackEditor.IsCurrentCellInEditMode) {
                if ((string)trackEditor.CurrentCell.EditedFormattedValue == "") {
                    trackEditor.CurrentCell.Value = null;
                    trackEditor.CancelEdit();
                    trackEditor.EndEdit();
                }
            }
        }

        private void Read_Config()
        {
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

        private static void RandomizeRowValues(DataGridViewRow dgvr, Sequencer_Object _seqobj)
        {
            Random rng = new Random();
            int rngchance;
            int rnglimit;
            int randomtype = 0;
            decimal valueiftrue = 0;
            
            if ((_seqobj.trait_type is "kTraitBool" or "kTraitAction") || (_seqobj.param_path is "visibla01" or "visibla02" or "visible" or "visiblz01" or "visiblz02")) {
                valueiftrue = 1;
                rngchance = 10;
                rnglimit = 9;
                if (_seqobj.obj_name == "sentry.spn") {
                    rngchance = 55;
                    rnglimit = 54;
                }
            }
            else if (_seqobj.trait_type == "kTraitColor") {
                randomtype = 7;
                rngchance = 10;
                rnglimit = 8;
            }
            else {
                rngchance = 10;
                rnglimit = 9;
                if (_seqobj.param_path == "sequin_speed")
                    randomtype = 2;
                else if (_seqobj.obj_name == "fade.pp")
                    randomtype = 3;
                else if (_seqobj.friendly_type == "CAMERA")
                    randomtype = 4;
                else if (_seqobj.friendly_type == "GAMMA")
                    randomtype = 5;
                else
                    randomtype = 6;
            }
            foreach (DataGridViewCell dgvc in dgvr.Cells) {
                switch (randomtype) {
                    case 2:
                        valueiftrue = TruncateDecimal((decimal)(rng.NextDouble() * 100) + 0.01m, 3) % 4;
                        break;
                    case 3:
                        valueiftrue = TruncateDecimal((decimal)rng.NextDouble(), 3);
                        break;
                    case 4:
                        valueiftrue = TruncateDecimal((decimal)(rng.NextDouble() * 100), 3) * (rng.Next(0, 1) == 0 ? 1 : -1);
                        break;
                    case 5:
                        valueiftrue = TruncateDecimal((decimal)(rng.NextDouble() * 100), 3);
                        break;
                    case 6:
                        valueiftrue = (TruncateDecimal((decimal)(rng.NextDouble() * 1000), 3) % 200) * (rng.Next(0, 1) == 0 ? 1 : -1);
                        break;
                    case 7:
                        valueiftrue = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)).ToArgb();
                        break;
                }

                dgvc.Value = rng.Next(0, rngchance) >= rnglimit ? valueiftrue : null;
            }
            TrackUpdateHighlighting(dgvr, _seqobj);
            GenerateDataPoints(dgvr, _seqobj);
        }

        /// <summary>
        /// UNDO FUNCTIONS
        /// </summary>
        readonly ToolStripDropDownMenu undomenu = new() {
            BackColor = Color.FromArgb(40, 40, 40),
            ShowCheckMargin = false,
            ShowImageMargin = false,
            ShowItemToolTips = false,
            MaximumSize = new Size(2000, 500)
        };
        private ToolStripDropDown CreateUndoMenu(List<SaveState> undolist)
        {
            undomenu.Items.Clear();

            foreach (SaveState s in undolist) {
                ToolStripMenuItem tmsi = new() {
                    Text = s.reason
                };
                tmsi.MouseEnter += undoMenu_MouseEnter;
                tmsi.Click += undoItem_Click;
                tmsi.BackColor = Color.FromArgb(40, 40, 40);
                tmsi.ForeColor = Color.White;
                undomenu.Items.Add(tmsi);
            }
            return undomenu;
        }
        private static void undoMenu_MouseEnter(object sender, EventArgs e)
        {
            Color backcolor = Color.FromArgb(40, 40, 40);
            ToolStrip parent = ((ToolStripMenuItem)sender).Owner;
            for (int x = parent.Items.Count - 1; x >= 0; x--) {
                parent.Items[x].BackColor = backcolor;
                if (parent.Items[x] == sender)
                    backcolor = Color.Maroon;
            }
        }
        private void undoItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;
            int index = (tmsi.Owner).Items.IndexOf(tmsi);

            if ((tmsi.Owner).Items.Count == 1 && (tmsi.Owner).Items[0].Text.Contains("No changes"))
                return;

            UndoFunction(index + 1);
            PlaySound("UIrevertchanges");
        }
        private void UndoFunction(int undoindex)
        {
            if (undoindex >= _undolistleaf.Count) {
                LoadLeaf(_undolistleaf.Last().savestate, loadedleaf, false);
                _undolistleaf.RemoveRange(0, _undolistleaf.Count - 1);
            }
            else {
                LoadLeaf(_undolistleaf[undoindex].savestate, loadedleaf, false);
                _undolistleaf.RemoveRange(0, undoindex);
            }
        }
        private void ClearReloadUndo(dynamic _load)
        {
            _undolistleaf.Clear();
            leafjson = _load;
            _undolistleaf.Insert(0, new SaveState() {
                reason = $"No changes",
                savestate = leafjson
            });
        }
        ///
        ///
        ///

        private void UpdateLevelLists()
        {
            lvlsinworkfolder = Directory.GetFiles(workingfolder, "lvl_*.txt").Select(x => Path.GetFileName(x).Replace("lvl_", "").Replace(".txt", ".lvl")).ToList() ?? new List<string>();
            lvlsinworkfolder.Add("<none>");
            lvlsinworkfolder.Sort();

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

        public void PanelEnableState(Control panel, bool enablestate)
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
        public void WriteFileLock(FileStream fs, JObject _save)
        {
            string tosave = JsonConvert.SerializeObject(_save, Formatting.Indented);
            using (StreamWriter sr = new StreamWriter(fs, System.Text.Encoding.UTF8, tosave.Length, true)) {
                fs.SetLength(0);
                sr.Write(tosave);
            }
        }

        public dynamic LoadFileLock(string _selectedfilename)
        {
            dynamic _load;
            if (!File.Exists(_selectedfilename))
                return null;
            ///reference:
            ///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
            using (FileStream fileStream = new FileStream(_selectedfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader textReader = new StreamReader(fileStream)) {
                try {
                    _load = JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
                } catch (Exception) {
                    MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.", "File load error");
                    _load = null;
                }
            }

            return _load;
        }

        public void DeleteFileLock(string _selectedfilename, string filetype)
        {
            if (lockedfiles.ContainsKey(_selectedfilename)) {
                lockedfiles[_selectedfilename].Close();
                lockedfiles.Remove(_selectedfilename);
                ClearPanels(filetype);
            }
            File.Delete(_selectedfilename);
        }

        public void ClearFileLock()
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


        public string CopyToWorkingFolderCheck(string filepath)
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

        public void HighlightMissingFile(DataGridView dgv, List<string> filelist)
        {
            foreach (DataGridViewRow dgvr in dgv.Rows) {
                if (!File.Exists(filelist[dgvr.Index])) {
                    dgvr.DefaultCellStyle.BackColor = Color.Maroon;
                    dgvr.DefaultCellStyle.SelectionBackColor = Color.Gray;
                }
                else {
                    dgvr.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
                    dgvr.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                }
            }
        }
    }
}