using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_SampleEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_SampleEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            sampleToolStrip.Renderer = new ToolStripOverride();
            InitializeSampleStuff();
            TCLE.InitializeTracks(sampleList, false);
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        FileInfo loadedsample
        {
            get { return loadedsample; }
            set {
                if (loadedsample != value) {
                    loadedsample = value;
                    if (!LoadedSample.Exists) {
                        LoadedSample.CreateText();
                    }
                    TCLE.lockedfiles.Add(LoadedSample, new FileStream(LoadedSample.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private FileInfo LoadedSample;
        dynamic samplejson;
        ObservableCollection<SampleData> _samplelist = new();
        #endregion

        #region EventHandlers
        ///         ///
        /// EVENTS  ///
        ///         ///

        private void sampleList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //remove event handlers so they don't trigger when the values change
            txtSampPath.TextChanged -= txtSampPath_TextChanged;
            //update values with selected data
            txtSampPath.Text = _samplelist[e.RowIndex].path;
            btnSampEditorPlaySamp.Enabled = true;
            //re-add the event handlers
            txtSampPath.TextChanged += txtSampPath_TextChanged;
        }

        private void sampleList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            sampleList.CellValueChanged -= sampleList_CellValueChanged;
            try {
                List<DataGridViewRow> editedrows = new();
                object _val = sampleList[e.ColumnIndex, e.RowIndex].Value;
                //iterate over each cell in the selection
                foreach (DataGridViewCell _cell in sampleList.SelectedCells) {
                    //skip name column
                    if (_val is string && (_cell.ColumnIndex is 1 or 2 or 3 or 4))
                        continue;
                    if (_cell.ColumnIndex == 0)
                        continue;
                    //if cell does not have the value, set it
                    if (_cell.Value != _val)
                        _cell.Value = _val;
                    if (!editedrows.Contains(_cell.OwningRow))
                        editedrows.Add(_cell.OwningRow);
                }
                foreach (DataGridViewRow dgvr in editedrows) {
                    int _index = dgvr.Index;
                    _samplelist[_index].obj_name = (string)dgvr.Cells[0].Value;
                    _samplelist[_index].volume = (decimal)dgvr.Cells[1].Value;
                    _samplelist[_index].pitch = (decimal)dgvr.Cells[2].Value;
                    _samplelist[_index].pan = (decimal)dgvr.Cells[3].Value;
                    _samplelist[_index].offset = (decimal)dgvr.Cells[4].Value;
                    _samplelist[_index].channel_group = dgvr.Cells[5].Value.ToString();
                }
                SaveSample(false);
            }
            catch { }
            sampleList.CellValueChanged += sampleList_CellValueChanged;
        }

        private void sampleList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //during editing of a cell in sampleList, check and sanitize input so it's numeric only
            e.Control.KeyPress -= new KeyPressEventHandler(TCLE.NumericInputSanitize);
            if (sampleList.CurrentCell.ColumnIndex is 1 or 2 or 3 or 4) //Desired Column
            {
                if (e.Control is TextBox tb) {
                    tb.KeyPress += new KeyPressEventHandler(TCLE.NumericInputSanitize);
                }
            }
        }

        public void _samplelist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //sort the list alphabetically
            _samplelist = new ObservableCollection<SampleData>(_samplelist.OrderBy(x => x.obj_name).ToList());
            _samplelist.CollectionChanged += _samplelist_CollectionChanged;
            //clear dgv
            sampleList.RowCount = 0;
            //repopulate dgv from list
            foreach (SampleData _samp in _samplelist) {
                sampleList.Rows.Add(new object[] { _samp.obj_name, _samp.volume, _samp.pitch, _samp.pan, _samp.offset, _samp.channel_group });
            }
            //enable certain buttons if there are enough items for them
            btnSampleAdd.Enabled = true;
            btnSampleDelete.Enabled = _samplelist.Count > 0;
            btnSampEditorPlaySamp.Enabled = _samplelist.Count > 0;

            //set lvl save flag to false
            SaveSample(false);
        }

        private void txtSampPath_TextChanged(object sender, EventArgs e)
        {
            _samplelist[sampleList.CurrentRow.Index].path = txtSampPath.Text;
        }

        private void SamplenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Samples is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                SamplesaveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void SampleopenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Samples is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                using OpenFileDialog ofd = new();
                ofd.Filter = "Thumper Sample File (*.txt)|samp_*.txt";
                ofd.Title = "Load a Thumper Sample file";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    //storing the filename in temp so it doesn't overwrite _loadedlvl in case it fails the check in LoadLvl()
                    FileInfo filepath = new FileInfo(TCLE.CopyToWorkingFolderCheck(ofd.FileName, TCLE.WorkingFolder.FullName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadSample(_load, filepath);
                }
            }
        }
        ///SAVE
        private void SamplesaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if _loadedgate is somehow not set, force Save As instead
            if (loadedsample == null) {
                SamplesaveAsToolStripMenuItem_Click(1, null);
                return;
            }
            else
                WriteSample();
        }
        ///SAVE AS
        private void SamplesaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Sample File (*.txt)|*.txt";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName;
            if (sfd.ShowDialog() == DialogResult.OK) {
                if (sender == null)
                    loadedsample = null;
                //separate path and filename
                string storePath = Path.GetDirectoryName(sfd.FileName);
                string tempFileName = Path.GetFileName(sfd.FileName);
                if (!tempFileName.EndsWith(".txt"))
                    tempFileName += ".txt";
                //check if user input "gate_", and deny save if so
                if (Path.GetFileName(sfd.FileName).Contains("samp_")) {
                    MessageBox.Show("File not saved. Do not include 'samp_' in your file name.", "File not saved");
                    return;
                }
                if (File.Exists($@"{storePath}\{tempFileName}.samp")) {
                    MessageBox.Show("That file name exists already.", "File not saved");
                    return;
                }
                loadedsample = new FileInfo($@"{storePath}\{tempFileName}.samp");
                WriteSample();
                //after saving new file, refresh the workingfolder
                ///_mainform.btnWorkRefresh_Click(null, null);
            }
        }
        private void WriteSample()
        {
            //write contents direct to file without prompting save dialog
            JObject _save = SampleBuildSave();
            if (!TCLE.lockedfiles.ContainsKey(loadedsample)) {
                TCLE.lockedfiles.Add(loadedsample, new FileStream(LoadedSample.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
            }
            TCLE.WriteFileLock(TCLE.lockedfiles[loadedsample], _save);
            SaveSample(true, true);
            this.Text = LoadedSample.Name;
        }

        ///Detect dragon-and-drop of files and then load them to Sample files
        private void sampleList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (File.Exists(data[0])) {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }
        ///Detect dragon-and-drop of files and then load them to Sample files
        private void sampleList_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string dir in data) {
                if (File.Exists(dir) && Path.GetExtension(dir) == ".fsb")
                    FSBtoSAMP(dir);
                else
                    MessageBox.Show($@"{dir} is not an .fsb file. It was {Path.GetExtension(dir)}. File not added to sample list.", "Sample load error");
            }
            TCLE.PlaySound("UIobjectadd");
        }

        #endregion

        #region Buttons
        ///         ///
        /// BUTTONS ///
        ///         ///
        //add and remove sample entries
        private void btnSampleDelete_Click(object sender, EventArgs e)
        {
            List<SampleData> todelete = new();
            foreach (DataGridViewRow dgvr in sampleList.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToList()) {
                todelete.Add(_samplelist[dgvr.Index]);
            }
            int _in = sampleList.CurrentRow.Index;
            bool customforcesave = false;
            outputDevice?.Stop();

            if (todelete.Any(x => x.path.Contains("custom"))) {
                if (MessageBox.Show("At least 1 sample selected is a custom sample and it will be removed from the \"extras\" folder. This deletion cannot be undone.\nContinue?", "Confirm?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            foreach (SampleData sd in todelete) {
                if (sd.path.Contains("custom")) {
                    customforcesave = true;
                    string _hashedname = null;
                    byte[] hashbytes = BitConverter.GetBytes(Hash32($"A{sd.path}"));
                    Array.Reverse(hashbytes);
                    foreach (byte b in hashbytes)
                        _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
                    //if the hashed name starts with a '0', remove it
                    if (_hashedname[0] == '0')
                        _hashedname = _hashedname[1..];

                    if (File.Exists($@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc")) {
                        File.Delete($@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc");
                    }
                }
                //delete file from temp folder too. If it isn't removed and then a new sample is added with the same name, the old sample will play
                try {
                    if (File.Exists($@"{TCLE.AppLocation}\temp\{sd.obj_name}.ogg"))
                        File.Delete($@"{TCLE.AppLocation}\temp\{sd.obj_name}.ogg");
                    if (File.Exists($@"{TCLE.AppLocation}\temp\{sd.obj_name}.wav"))
                        File.Delete($@"{TCLE.AppLocation}\temp\{sd.obj_name}.wav");
                }
                catch (Exception ex) {
                    MessageBox.Show($"Unable to delete {TCLE.AppLocation}\\temp\\\\{_samplelist[_in].obj_name}\n\n{ex}");
                }
                _samplelist.Remove(sd);
            }

            if (customforcesave)
                //force save as this cannot be undone
                SamplesaveToolStripMenuItem_Click(null, null);
            TCLE.PlaySound("UIobjectremove");
        }
        private void btnSampleAdd_Click(object sender, EventArgs e)
        {
            SampleData newsample = new() {
                obj_name = "new",
                volume = 1,
                pitch = 1,
                pan = 0,
                offset = 0,
                path = "samples/levels/custom/new.wav",
                channel_group = "sequin.ch"
            };
            _samplelist.Add(newsample);
            int _index = _samplelist.IndexOf(newsample);
            sampleList.Rows[_index].Cells[0].Selected = true;
            TCLE.PlaySound("UIobjectadd");
        }

        //Opens an .FSB audio file, hashes the name, and adds it to the loaded SAMP_ file
        private void FSBtoSamp_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "FSB Audio File (*.fsb)|*.fsb";
            ofd.Title = "Load a FSB Audio file";
            ofd.InitialDirectory = TCLE.WorkingFolder.FullName ?? Application.StartupPath;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK) {
                foreach (string _file in ofd.FileNames)
                    FSBtoSAMP(_file);
                TCLE.PlaySound("UIobjectadd");
            }
        }
        //How to create an FSB
        private void lblSampleFSBhelp_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q");

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private void btnSampEditorPlaySamp_Click(object sender, EventArgs e)
        {
            SampleData _samp = _samplelist[sampleList.CurrentRow.Index];
            string _filetype = "";
            //check if sample exists in temp folder. If not, create it
            if (!File.Exists($@"temp\{_samp.obj_name}.ogg") && !File.Exists($@"temp\{_samp.obj_name}.wav")) {
                string _result = PCtoOGG(_samp);
                if (_result == null)
                    return;
            }
            //check extension of the sample to play
            if (File.Exists($@"temp\{_samp.obj_name}.ogg"))
                _filetype = "ogg";
            if (File.Exists($@"temp\{_samp.obj_name}.wav"))
                _filetype = "wav";


            if (outputDevice == null) {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null) {
                if (_filetype == "ogg") {
                    VorbisWaveReader vorbis = new($@"temp\{_samp.obj_name}.{_filetype}");
                    outputDevice.Init(vorbis);
                }
                else {
                    audioFile = new AudioFileReader($@"temp\{_samp.obj_name}.{_filetype}");
                    outputDevice.Init(audioFile);
                }
            }

            btnSampEditorPlaySamp.Image = Properties.Resources.icon_stop;
            btnSampEditorPlaySamp.Click -= btnSampEditorPlaySamp_Click;
            btnSampEditorPlaySamp.Click += OnButtonStopClick;
            outputDevice.Play();
        }
        private void OnButtonStopClick(object sender, EventArgs args) => outputDevice?.Stop();
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
            btnSampEditorPlaySamp.Click += btnSampEditorPlaySamp_Click;
            btnSampEditorPlaySamp.Click -= OnButtonStopClick;
            btnSampEditorPlaySamp.Image = Properties.Resources.icon_play2;
        }

        private void btnRevertSample_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveSample(true);
            LoadSample(samplejson, loadedsample);
            TCLE.PlaySound("UIrevertchanges");
        }
        #endregion

        #region Methods
        ///         ///
        /// METHODS ///
        ///         ///

        public void InitializeSampleStuff()
        {
            ///Customize Lvl list a bit
            sampleList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
                ForeColor = Color.White,
                Font = new Font("Arial", 12, GraphicsUnit.Pixel)
            };
            sampleList.Columns[0].ValueType = typeof(string);
            sampleList.Columns[1].ValueType = typeof(decimal);
            sampleList.Columns[2].ValueType = typeof(decimal);
            sampleList.Columns[3].ValueType = typeof(decimal);
            sampleList.Columns[4].ValueType = typeof(decimal);
            sampleList.Columns[5].ValueType = typeof(string);

            _samplelist.CollectionChanged += _samplelist_CollectionChanged;
        }

        public void LoadSample(dynamic _load, FileInfo filepath)
        {
            if (_load == null)
                return;
            //detect if file is actually Gate or not
            if (!_load.ContainsKey("items")) {
                MessageBox.Show("This does not appear to be a sample file!");
                return;
            }
            loadedsample = filepath;
            //set some visual elements
            this.Text = LoadedSample.Name;

            ///Clear form elements so new data can load
            _samplelist.CollectionChanged -= _samplelist_CollectionChanged;
            _samplelist.Clear();
            ///load lvls associated with this master
            foreach (dynamic _samp in _load["items"]) {
                _samplelist.Add(new SampleData() {
                    obj_name = ((string)_samp["obj_name"]).Replace(".samp", ""),
                    path = _samp["path"],
                    volume = _samp["volume"],
                    pitch = _samp["pitch"],
                    pan = _samp["pan"],
                    offset = _samp["offset"],
                    channel_group = _samp["channel_group"]
                });
            }
            _samplelist.CollectionChanged += _samplelist_CollectionChanged;
            _samplelist_CollectionChanged(null, null);
            FSBtoSamp.Enabled = true;
            ///set save flag (samples just loaded, has no changes)
            samplejson = _load;
            SaveSample(true);
        }

        public void SaveSample(bool save, bool playsound = false)
        {
            //make the beeble emote
            TCLE.beeble.MakeFace();

            EditorIsSaved = save;
            if (!save) {
                /*
                btnSaveSample.Enabled = true;
                btnRevertSample.Enabled = samplejson != null;
                btnRevertSample.ToolTipText = samplejson != null ? "Revert changes to last save" : "You cannot revert with no file saved";
                toolstripTitleSample.BackColor = Color.Maroon;
                */
            }
            else {
                /*
                btnSaveSample.Enabled = false;
                btnRevertSample.Enabled = false;
                toolstripTitleSample.BackColor = Color.FromArgb(40, 40, 40);
                */
                if (playsound) TCLE.PlaySound("UIsave");
            }
        }

        public JObject SampleBuildSave()
        {
            JObject _save = new();
            JArray _items = new();
            foreach (DataGridViewRow dgvr in sampleList.Rows) {
                int _index = dgvr.Index;
                _samplelist[_index].obj_name = (string)dgvr.Cells[0].Value;
                _samplelist[_index].volume = (decimal)dgvr.Cells[1].Value;
                _samplelist[_index].pitch = (decimal)dgvr.Cells[2].Value;
                _samplelist[_index].pan = (decimal)dgvr.Cells[3].Value;
                _samplelist[_index].offset = (decimal)dgvr.Cells[4].Value;
                _samplelist[_index].channel_group = dgvr.Cells[5].Value.ToString();
            }
            foreach (SampleData _sample in _samplelist) {
                JObject _samp = new() {
                    { "obj_type", "Sample"},
                    { "obj_name", _sample.obj_name + ".samp" },
                    { "mode", "kSampleOneOff" },
                    { "path", _sample.path },
                    { "volume", _sample.volume },
                    { "pitch", _sample.pitch },
                    { "pan", _sample.pan },
                    { "offset", _sample.offset },
                    { "channel_group", _sample.channel_group }
                };
                _items.Add(_samp);
            }
            _save.Add("items", _items);

            samplejson = _save;
            return _save;
        }

        private void FSBtoSAMP(string filepath)
        {
            string _filename;
            byte[] _bytes;
            byte[] _header = new byte[] { 0x0d, 0x00, 0x00, 0x00 };
            string _hashedname = "";

            //save relevant data of the chosen file
            _filename = Path.GetFileNameWithoutExtension(filepath);
            _bytes = File.ReadAllBytes(filepath);
            //get the hash of the FSB filename. This will be used to name the final .PC file
            byte[] hashbytes = BitConverter.GetBytes(Hash32($"Asamples/levels/custom/{_filename}.wav"));
            Array.Reverse(hashbytes);
            foreach (byte b in hashbytes)
                _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
            //if the hashed name starts with a '0', remove it
            if (_hashedname[0] == '0')
                _hashedname = _hashedname[1..];

            ///With hashing complete, can now save the file to a .PC
            //if the `extras` folder doesn't exist, make it
            Directory.CreateDirectory($@"{TCLE.WorkingFolder.FullName}\extras");
            //write header and bytes of fsb to new file
            using (FileStream f = File.Open($@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc", FileMode.Create, FileAccess.Write, FileShare.None)) {
                f.Write(_header, 0, _header.Length);
                f.Write(_bytes, 0, _bytes.Length);
            }

            //Add new sample entry to the loaded samp_ file
            SampleData newsample = new() {
                obj_name = $"{_filename}",
                volume = 1,
                pitch = 1,
                pan = 0,
                offset = 0,
                path = $"samples/levels/custom/{_filename}.wav",
                channel_group = "sequin.ch"
            };
            _samplelist.Add(newsample);
            int _index = _samplelist.IndexOf(newsample);
            sampleList.Rows[_index].Cells[0].Selected = true;
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

        public string PCtoOGG(SampleData _samp)
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

        private void ResetSample()
        {
            //reset things to default values
            samplejson = null;
            _samplelist.Clear();
            this.Text = "Sample Editor";
            //set saved flag to true, because nothing is loaded
            SaveSample(true);
            FSBtoSamp.Enabled = true;
        }
        #endregion
    }
}
