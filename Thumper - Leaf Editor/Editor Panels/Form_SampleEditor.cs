using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using NAudio.Vorbis;
using NAudio.Wave;
using VarispeedDemo.SoundTouch;
using System.Collections.Generic;

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
            TCLE.DoubleBufferDGV(sampleList, false);

            if (load != null)
                LoadSample(load, filepath);
            propertyGridSample.SelectedObject = SampleProperties;
        }
        private void Form_SampleEditor_Shown(object sender, EventArgs e)
        {
            propertyGridSample.Focus();
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading = false;
        public FileInfo loadedsample
        {
            get { return LoadedSample; }
            set {
                if (LoadedSample != value) {
                    TCLE.CloseFileLock(LoadedSample);
                    LoadedSample = value;
                    if (!LoadedSample.Exists) {
                        using (StreamWriter sw = LoadedSample.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.lockedfiles.Add(LoadedSample, new FileStream(LoadedSample.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read));
                }
            }
        }
        private FileInfo LoadedSample;
        dynamic samplejson;
        public SampleProperties sampleproperties
        {
            get => SampleProperties;
            set {
                SampleProperties = value;
                SaveCheckAndWrite(false);
            }
        }
        private SampleProperties SampleProperties;
        public ObservableCollection<SampleData> SampleList { get => SampleProperties.samplelist; set => SampleProperties.samplelist = value; }
        public decimal BPM { get { return TCLE.dockProjectProperties.BPM; } }
        #endregion

        #region EventHandlers
        ///         ///
        /// EVENTS  ///
        ///         ///

        private void sampleList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            sampleproperties.sample = SampleList[e.RowIndex];
            propertyGridSample.ExpandAllGridItems();
            propertyGridSample.Refresh();

            if (e.ColumnIndex == 0) {
                AudioPlayback(sampleList[e.ColumnIndex, e.RowIndex]);
            }
        }

        private void sampleList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
        }

        private void sampleList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            //button is in column 0, so that's where to draw the image
            if (e.ColumnIndex == 0) {
                CellPaint(e, SampleIsPlaying);
            }
        }
        private void CellPaint(DataGridViewCellPaintingEventArgs e, bool isplaying)
        {
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            //get dimensions
            int w = Properties.Resources.icon_play.Width;
            int h = Properties.Resources.icon_play.Height;
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            if (isplaying && playingcell == sampleList[e.ColumnIndex, e.RowIndex])
                e.Graphics.DrawImage(Properties.Resources.icon_stop, new Rectangle(x, y, w, h));
            else
                e.Graphics.DrawImage(Properties.Resources.icon_play, new Rectangle(x, y, w, h));
            e.Handled = true;
        }

        public void _samplelist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //sort the list alphabetically
            SampleList = new ObservableCollection<SampleData>(SampleList.OrderBy(x => x.obj_name).ToList());
            SampleList.CollectionChanged += _samplelist_CollectionChanged;
            //clear dgv
            sampleList.RowCount = 0;
            //repopulate dgv from list
            foreach (SampleData _samp in SampleList) {
                sampleList.Rows.Add(new object[] { null, _samp.obj_name });
            }
            //enable certain buttons if there are enough items for them
            btnSampleAdd.Enabled = true;
            btnSampleDelete.Enabled = SampleList.Count > 0;

            //set lvl save flag to false
            SaveCheckAndWrite(false);
        }

        private void SamplenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((!EditorIsSaved && MessageBox.Show("Current Samples is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || EditorIsSaved) {
                //SamplesaveAsToolStripMenuItem_Click(null, null);
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
                    FileInfo filepath = new(TCLE.CopyToWorkingFolderCheck(ofd.FileName));
                    if (filepath == null)
                        return;
                    //load json from file into _load. The regex strips any comments from the text.
                    dynamic _load = TCLE.LoadFileLock(filepath.FullName);
                    LoadSample(_load, filepath);
                }
            }
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
                todelete.Add(SampleList[dgvr.Index]);
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
                    byte[] hashbytes = BitConverter.GetBytes(TCLE.Hash32($"A{sd.path}"));
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
                } catch (Exception ex) {
                    MessageBox.Show($"Unable to delete {TCLE.AppLocation}\\temp\\\\{SampleList[_in].obj_name}\n\n{ex}");
                }
                SampleList.Remove(sd);
            }

            if (customforcesave)
                //force save as this cannot be undone
                SaveCheckAndWrite(true);
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
            SampleList.Add(newsample);
            int _index = SampleList.IndexOf(newsample);
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

        private WaveOutEvent outputDevice = new();
        private VarispeedSampleProvider speedControl;
        private VorbisWaveReader vorbis;
        private AudioFileReader audioFile;
        private bool SampleIsPlaying;
        private DataGridViewCell playingcell;
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            timerSample.Enabled = false;
            outputDevice.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
            vorbis?.Dispose();
            vorbis = null;

            SampleIsPlaying = false;
            sampleList.InvalidateCell(playingcell);
        }
        private void AudioPlayback(DataGridViewCell cell)
        {
            if (!SampleIsPlaying) {
                SampleData _samp = sampleproperties.sample;
                string _filetype = "";
                //check if sample exists in temp folder. If not, create it
                if (!File.Exists($@"temp\{_samp.obj_name}.ogg") && !File.Exists($@"temp\{_samp.obj_name}.wav")) {
                    string _result = TCLE.PCtoOGG(_samp);
                    if (_result == null)
                        return;
                }
                //check extension of the sample to play
                if (File.Exists($@"temp\{_samp.obj_name}.ogg"))
                    _filetype = "ogg";
                if (File.Exists($@"temp\{_samp.obj_name}.wav"))
                    _filetype = "wav";

                //initialize the player and load the sample
                outputDevice = new WaveOutEvent();
                if (_filetype == "ogg") {
                    vorbis = new VorbisWaveReader($@"temp\{_samp.obj_name}.{_filetype}");
                    vorbis.CurrentTime = TimeSpan.FromMilliseconds(_samp.offset);
                    timerSample.Interval = (int)((float)(vorbis.TotalTime.TotalMilliseconds - _samp.offset) / (float)_samp.pitch);
                    speedControl = new(vorbis, 100, new SoundTouchProfile(false, false));
                }
                else {
                    audioFile = new AudioFileReader($@"temp\{_samp.obj_name}.{_filetype}");
                    audioFile.CurrentTime = TimeSpan.FromMilliseconds(_samp.offset);
                    timerSample.Interval = (int)((float)(audioFile.TotalTime.TotalMilliseconds - _samp.offset) / (float)_samp.pitch);
                    speedControl = new(audioFile, 100, new SoundTouchProfile(false, false));
                }
                //set playback rate equal to sample pitch
                speedControl.PlaybackRate = (float)_samp.pitch;
                outputDevice.Init(speedControl);
                outputDevice.Volume = volumeSlider1.Volume;
                outputDevice.PlaybackStopped += OnPlaybackStopped;
                SampleIsPlaying = true;
                //invalidate cell to repaint it. This will draw the stop icon
                sampleList.InvalidateCell(cell);
                //store cell for later reference
                playingcell = cell;
                outputDevice.Play();
                timerSample.Enabled = true;
            }
            else {
                outputDevice?.Stop();
            }
        }

        private void timerSample_Tick(object sender, EventArgs e)
        {
            outputDevice?.Stop();
            timerSample.Enabled = false;
        }

        private void volumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            outputDevice.Volume = volumeSlider1.Volume;
        }

        private void btnRevertSample_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SaveCheckAndWrite(true);
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
            sampleList.Columns[1].ValueType = typeof(string);
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
            //set flag that load is in progress. This skips Save method
            EditorLoading = true;

            sampleproperties = new(this, filepath);

            ///Clear form elements so new data can load
            SampleList.CollectionChanged -= _samplelist_CollectionChanged;
            SampleList.Clear();
            ///load lvls associated with this master
            foreach (dynamic _samp in _load["items"]) {
                SampleList.Add(new SampleData() {
                    obj_name = (string)_samp["obj_name"],
                    path = _samp["path"],
                    volume = _samp["volume"],
                    pitch = _samp["pitch"],
                    pan = _samp["pan"],
                    offset = _samp["offset"],
                    channel_group = _samp["channel_group"] == "" ? "sequin.ch" : _samp["channel_group"]
                });
            }
            SampleList.CollectionChanged += _samplelist_CollectionChanged;
            _samplelist_CollectionChanged(null, null);
            FSBtoSamp.Enabled = true;

            ///set save flag (samples just loaded, has no changes)
            SaveCheckAndWrite(true);
            EditorLoading = false;
            EditorIsSaved = true;
        }

        ///SAVE
        public void Save()
        {
            //if _loadedgate is somehow not set, force Save As instead
            if (loadedsample == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, true);
        }
        ///SAVE AS
        public FileInfo SaveAs(bool isnew = false)
        {
            using SaveFileDialog sfd = new();
            //filter .txt only
            sfd.Filter = "Thumper Sample File (*.samp)|*.samp";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = TCLE.WorkingFolder.FullName;
            if (sfd.ShowDialog() == DialogResult.OK) {
                loadedsample = new FileInfo(sfd.FileName);

                if (sampleproperties == null) {
                    sampleproperties = new(this, loadedsample) {

                    };
                }

                SaveCheckAndWrite(true, true);
                if (isnew)
                    TCLE.CloseFileLock(loadedsample);
                //after saving new file, refresh the project explorer
                TCLE.dockProjectExplorer.CreateTreeView();
            }
            return loadedsample;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, bool playsound = false)
        {
            if (EditorLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedSample.Name + "*";
                //add current JSON to the undo list
                sampleproperties.undoItems.Add(BuildSave(sampleproperties));
            }
            else {
                this.Text = LoadedSample.Name;
                //build the JSON to write to file
                JObject _saveJSON = BuildSave(sampleproperties);
                sampleproperties.revertPoint = _saveJSON;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedSample], _saveJSON);

                if (playsound) TCLE.PlaySound("UIsave");
                TCLE.LvlReloadSamples();
            }
        }

        public static JObject BuildSave(SampleProperties _properties)
        {
            JObject _save = new();
            JArray _items = new();
            foreach (SampleData _sample in _properties.samplelist) {
                JObject _samp = new() {
                    { "obj_type", "Sample"},
                    { "obj_name", _sample.obj_name },
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
            byte[] hashbytes = BitConverter.GetBytes(TCLE.Hash32($"Asamples/levels/custom/{_filename}.wav"));
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
            SampleList.Add(newsample);
            int _index = SampleList.IndexOf(newsample);
            sampleList.Rows[_index].Cells[0].Selected = true;
        }

        private void ResetSample()
        {
            //reset things to default values
            samplejson = null;
            SampleList.Clear();
            this.Text = "Sample Editor";
            //set saved flag to true, because nothing is loaded
            SaveCheckAndWrite(true);
            FSBtoSamp.Enabled = true;
        }
        #endregion

        private void propertyGridSample_Click(object sender, EventArgs e)
        {

        }

        private void sampleList_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}
