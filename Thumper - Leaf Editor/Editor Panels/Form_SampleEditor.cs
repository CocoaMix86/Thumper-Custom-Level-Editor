using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_SampleEditor : DockContentEx
    {
        #region Form Construction
        public Form_SampleEditor(dynamic load = null, FileInfo filepath = null)
        {
            InitializeComponent();
            ColorFormElements();
            InitializeSampleStuff();
            TCLE.DoubleBufferDGV(sampleList, false);

            if (load != null) {
                LoadSample(load, filepath);
                UndoList.Add(new SaveState() {
                    reason = "",
                    savestate = load
                });
            }
            propertyGridSample.SelectedObject = SampleProperties;
        }
        private void Form_SampleEditor_Shown(object sender, EventArgs e)
        {
            propertyGridSample.Focus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!IsSaved()) {
                if (MessageBox.Show("File not saved. Are you sure you want to close it and discard changes?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        public void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.ColorSampleBG;
            sampleList.BackgroundColor = Properties.Settings.Default.ColorSampleListBG;
            pictureSpectrum.BackColor = Properties.Settings.Default.ColorWaveformBG;
            pictureWave.BackColor = Properties.Settings.Default.ColorWaveformBG;
        }
        #endregion

        #region Variables
        public bool EditorIsSaved = true;
        public bool EditorLoading;
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
                    TCLE.AddFileLock(LoadedSample);
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
                SaveCheckAndWrite(false, "woooooooooooooooooow");
            }
        }
        private SampleProperties SampleProperties;
        public ObservableCollection<SampleData> SampleList { get => SampleProperties.samplelist; set => SampleProperties.samplelist = value; }
        BASSTimer _updateTimer = new(50);
        public Visuals _vis = new();
        private DeserializeDockContent m_deserializeDockContent;
        public DockContentEx contentPropertyGrid = new() {
            TabText = "Properties",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentMain = new() {
            TabText = "Samples",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
        public DockContentEx contentWave = new() {
            TabText = "Waveform",
            DockAreas = DockAreas.Document | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom,
            HideOnClose = true,
            BackColor = Color.Black,
            CloseButtonVisible = false,
            CloseButton = false,
        };
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

        private static SolidBrush ClearColor = new SolidBrush(Color.Black);
        private static SolidBrush SampColorNotExist = new SolidBrush(Color.Maroon);
        private static Color SelectColor = Color.FromArgb(199, 69, 255);
        private static SolidBrush SampColorSelected = new SolidBrush(SelectColor);
        private static SolidBrush BrushWhite = new SolidBrush(Color.White);
        private static Pen PenBlack = new Pen(Color.Black, 1);
        private void sampleList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;
            if (e.RowIndex is -1)
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            //button is in column 0, so that's where to draw the image
            else if (e.ColumnIndex is 0) {
                CellPaintIcons(e);
            }
            else
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
        }
        private void CellPaintIcons(DataGridViewCellPaintingEventArgs e)
        {
            //e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            //get dimensions
            int w = Properties.Resources.icon_play.Width;
            int h = Properties.Resources.icon_play.Height;
            int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
            int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
            //paint the image
            if (TCLE.PlayingChannels.Any(x => x.Item2 == sampleList[1, e.RowIndex].Value.ToString()))
                e.Graphics.DrawImage(Properties.Resources.icon_stop, new Rectangle(x, y, w, h));
            else
                e.Graphics.DrawImage(Properties.Resources.icon_play, new Rectangle(x, y, w, h));
        }

        private void sampleList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            Rectangle bounds = e.RowBounds;
            bounds.X += 2;
            bounds.Y += 2;
            bounds.Width -= 4;
            bounds.Height -= 4;
            e.Graphics.FillRectangle(ClearColor, e.RowBounds);

            if (sampleList.Rows[e.RowIndex].Selected)
                e.Graphics.FillRoundedRectangle(BrushWhite, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 8);
            e.Graphics.FillRoundedRectangle(new SolidBrush(TCLE.Blend(e.InheritedRowStyle.BackColor, Color.Black, (sampleList.Rows[e.RowIndex].Selected ? 1 : 0.6))), bounds, 8);
            e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);
        }

        public void _samplelist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //sort the list alphabetically
            ///SampleList = new ObservableCollection<SampleData>(SampleList.OrderBy(x => x.obj_name).ToList());
            ///SampleList.CollectionChanged += _samplelist_CollectionChanged;
            //clear dgv
            sampleList.RowCount = 0;
            //repopulate dgv from list
            foreach (SampleData _samp in SampleList) {
                sampleList.Rows.Add(new object[] { null, _samp.obj_name, (_samp.time != 0 ? $"{_samp.beats.ToString("0.##")} beats -- {TimeSpan.FromSeconds(_samp.alteredtime).ToString(@"hh\:mm\:ss\.fff")}" : "play sample to get time") });
            }
            //enable certain buttons if there are enough items for them
            btnSampleAdd.Enabled = true;
            btnSampleDelete.Enabled = SampleList.Count > 0;
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

        private Rectangle dragBoxFromMouseDown;
        private DataGridViewRow RowToMove;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int previousDragOver = -1;
        private void sampleList_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (RowToMove == null && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    // Proceed with the drag and drop, passing in the list item.                    
                    ///DragDropEffects dropEffect = lvlLeafList.DoDragDrop(lvlLeafList.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                    RowToMove = sampleList.Rows[rowIndexFromMouseDown];
                    sampleList.ClearSelection();
                    //RowToMove.DefaultCellStyle.BackColor = SelectColor;
                    DragDropEffects dropEffect = sampleList.DoDragDrop(SampleList[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void sampleList_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = sampleList.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1) {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
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
            e.Effect = DragDropEffects.Move;
        }
        ///Detect dragon-and-drop of files and then load them to Sample files
        private void sampleList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                bool addedfile = false;
                foreach (string dir in data) {
                    if (File.Exists(dir) && Path.GetExtension(dir) is ".fsb" or ".wav") {
                        ImportAudioToSamp(dir);
                        TCLE.alzheimer();
                        addedfile = true;
                    }
                    else
                        MessageBox.Show($@"{dir} is not an .fsb file. It was {Path.GetExtension(dir)}. File not added to sample list.", "Sample load error");
                }
                if (addedfile)
                    SaveCheckAndWrite(false, "Add Sample");
                TCLE.PlaySound("UIobjectadd");
            }
            else {
                Point clientPoint = sampleList.PointToClient(new Point(e.X, e.Y));
                // Get the row index of the item the mouse is below. 
                rowIndexOfItemUnderMouseToDrop = sampleList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                // If the drag operation was a move then remove and insert the row.
                if (e.Effect == DragDropEffects.Move) {
                    if (e.Data.GetData(typeof(SampleData)) is SampleData rowToMove) {
                        if (rowIndexOfItemUnderMouseToDrop == -1)
                            return;
                        SampleList.Remove(rowToMove);
                        SampleList.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                        sampleList.ClearSelection();
                        sampleList.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                        SaveCheckAndWrite(false, "Change Sample Order");
                        RowToMove = null;
                    }
                }
            }
        }

        private void sampleList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = sampleList.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            int targetRow = sampleList.HitTest(targetPoint.X, targetPoint.Y).RowIndex;

            if (RowToMove != null && targetRow != -1 && targetRow != previousDragOver) {
                sampleList.Rows.Remove(RowToMove);
                sampleList.Rows.Insert(targetRow, RowToMove);
                sampleList.ClearSelection();
                previousDragOver = targetRow;
                sampleList.Rows[targetRow].Selected = true;
            }
        }

        private void propertyGridSample_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SaveCheckAndWrite(false, "Change Sample Property");
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
            ///outputDevice?.Stop();

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
                SaveCheckAndWrite(true, "");
            else
                SaveCheckAndWrite(false, "Remove Sample");
            //force save as this cannot be undone
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
                channel_group = "sequin.ch",
                time = 0,
                Editor = this
            };
            SampleList.Add(newsample);
            SaveCheckAndWrite(false, "Add Sample");
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
                bool addedfile = false;
                foreach (string _file in ofd.FileNames) {
                    if (_file.EndsWith(".wav") || _file.EndsWith(".fsb")) {
                        ImportAudioToSamp(_file);
                        TCLE.alzheimer();
                        addedfile = true;
                    }
                }
                if (addedfile)
                    SaveCheckAndWrite(false, "Add Sample");
                TCLE.PlaySound("UIobjectadd");
            }
        }
        //How to create an FSB
        private void lblSampleFSBhelp_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/14kSw3Hm-WKfADqOfuquf16lEUNKxtt9dpeWLWsX8y9Q", UseShellExecute = true });

        private void AudioPlayback(DataGridViewCell CellToPlay)
        {
            if (TCLE.PlaySampleOneOff(CellToPlay, SampleProperties.sample, out int SampChannel)) {
                TCLE.LastChannel = SampChannel;
                _updateTimer.Start();
                sampleList.InvalidateCell(CellToPlay);
            }
            else {
                sampleList.InvalidateCell(CellToPlay);
                TCLE.alzheimer();
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (!TCLE.PlayingChannels.Any(x => x.Item1 == this.sampleList)) {
                _updateTimer.Stop();
                return;
            }
            //these 2 show different spectrums visually while the sample plays
            pictureSpectrum.Image = _vis.CreateSpectrumWave(TCLE.PlayingChannels.Last(x => x.Item1 == this.sampleList).Item3, pictureSpectrum.Width, pictureSpectrum.Height, Color.Green, Color.Red, Properties.Settings.Default.ColorWaveformBG, 1, false, false, false);
            pictureWave.Image = _vis.CreateWaveForm(TCLE.PlayingChannels.Last(x => x.Item1 == this.sampleList).Item3, pictureSpectrum.Width, pictureSpectrum.Height, Color.Green, Color.Red, Color.Gray, Properties.Settings.Default.ColorWaveformBG, 1, false, true, false);
        }

        private void volumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            //Bass.BASS_SetVolume(volumeSlider1.Volume);
        }

        private void btnRevertSample_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Revert all changes to last save?", "Revert changes", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //SaveCheckAndWrite(true);
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
            _updateTimer.Tick += new EventHandler(timerUpdate_Tick);
            sampleToolStrip.Renderer = new ToolStripOverride();
            //
            sampleList.Columns[1].ValueType = typeof(string);
            //
            dockPanel1.Theme = TCLE.DockTheme;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //
            contentMain.Controls.Add(panelMain);
            panelMain.Dock = DockStyle.Fill;
            //
            contentWave.Controls.Add(pictureSpectrum);
            contentWave.Controls.Add(pictureWave);
            pictureSpectrum.Dock = DockStyle.Top;
            pictureWave.Dock = DockStyle.Top;
            contentWave.SizeChanged += contentWave_SizeChanged;
            //
            contentPropertyGrid.Controls.Add(propertyGridSample);
            propertyGridSample.Dock = DockStyle.Fill;
            //
            try {
                dockPanel1.LoadFromXml($@"{TCLE.AppLocation}\settings\layout_sample.config", m_deserializeDockContent);
            }
            catch {
                contentMain.Show(dockPanel1, DockState.Document);
                contentWave.Show(contentMain.Pane, DockAlignment.Right, 0.5);
                contentPropertyGrid.Show(contentWave.Pane, DockAlignment.Bottom, 0.7);
            }
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            dockPanel1.SaveAsXml($@"{TCLE.AppLocation}\settings\layout_sample.config");
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            if (persistString is "Properties")
                return contentPropertyGrid;
            if (persistString is "Samples")
                return contentMain;
            if (persistString is "Waveform")
                return contentWave;

            throw new NotImplementedException();
        }

        private void contentWave_SizeChanged(object sender, EventArgs e)
        {
            pictureSpectrum.Height = contentWave.Height / 2;
            pictureWave.Height = contentWave.Height / 2;
        }

        public object GetProperties()
        {
            return SampleProperties;
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
            propertyGridSample.SelectedObject = sampleproperties;

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
                    channel_group = _samp["channel_group"] == "" ? "sequin.ch" : _samp["channel_group"],
                    Editor = this,
                    time = TCLE.ProjectSamples.First(x => x.obj_name == (string)_samp["obj_name"]).time
                });
            }
            SampleList.CollectionChanged += _samplelist_CollectionChanged;
            _samplelist_CollectionChanged(null, null);
            FSBtoSamp.Enabled = true;

            ///set save flag (samples just loaded, has no changes)
            SaveCheckAndWrite(true, "");
            EditorLoading = false;
            EditorIsSaved = true;
        }

        public void Reload()
        {
            dynamic _load = TCLE.LoadFileLock(LoadedSample.FullName);
            LoadSample(_load, LoadedSample);
            this.Invalidate();
        }

        public List<SaveState> UndoList = new();
        public List<SaveState> GetUndoList()
        {
            return UndoList;
        }

        public void PerformUndo(int undolistindex)
        {
            if (undolistindex > UndoList.Count - 1)
                return;
            bool _trackNotSaved = EditorIsSaved;
            LoadSample(UndoList[undolistindex].savestate, LoadedSample);
            UndoList.RemoveRange(0, undolistindex);
            propertyGridSample.Refresh();

            if (!_trackNotSaved) {
                EditorIsSaved = false;
                if (!this.Text.EndsWith("*"))
                    this.Text += '*';
            }
        }

        ///SAVE
        public void Save(bool playsound = true)
        {
            //if _loadedgate is somehow not set, force Save As instead
            if (loadedsample == null) {
                SaveAs();
            }
            else
                SaveCheckAndWrite(true, "", playsound);
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

                sampleproperties ??= new(this, loadedsample) {

                };

                SaveCheckAndWrite(true, "", true);
                if (isnew)
                    TCLE.CloseFileLock(loadedsample);
                //after saving new file, refresh the project explorer
                ProjectExplorer.CreateTreeView();
            }
            return loadedsample;
        }

        public bool IsSaved()
        {
            return EditorIsSaved;
        }

        public void SaveCheckAndWrite(bool IsSaved, string Reason, bool playsound = false)
        {
            if (EditorLoading)
                return;
            //make the beeble emote
            TCLE.MainBeeble.MakeFace();

            EditorIsSaved = IsSaved;
            JObject _saveJSON = BuildSave(SampleProperties);
            //
            if (!IsSaved) {
                //denote editor tab is not saved
                this.Text = LoadedSample.Name + "*";
                //update the undo list
                UndoList.Insert(0, new SaveState() {
                    reason = Reason,
                    savestate = _saveJSON
                });
            }
            else {
                this.Text = LoadedSample.Name;
                //write JSON to file
                TCLE.WriteFileLock(TCLE.lockedfiles[LoadedSample], _saveJSON);
                TCLE.UpdateProjectSamplesFromFile(LoadedSample, true, out string _);
                if (playsound) TCLE.PlaySound("UIsave");
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

        private Dictionary<int, ulong> FrequencyID = new() {
            { 8000, 1 },
            { 11_000, 2 },
            { 11_025, 3 },
            { 16_000, 4 },
            { 22_050, 5 },
            { 24_000, 6 },
            { 32_000, 7 },
            { 44_100, 8 },
            { 48_000, 9 },
            { 96_000, 10 }};
        private byte[] nametable = new byte[] { 0x04, 0x00, 0x00, 0x00, 0x52, 0x54, 0x4C, 0x33, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] PCfileheader = new byte[] { 0x0d, 0x00, 0x00, 0x00 };
        private void ImportAudioToSamp(string filepath)
        {
            string _filename = Path.GetFileNameWithoutExtension(filepath);

            if (TCLE.ProjectSamples.Any(x => x.obj_name == $"{_filename}.samp")) {
                MessageBox.Show($"A sample with the name \"{_filename}\" already exists in {TCLE.ProjectSamples.First(x => x.obj_name == $"{_filename}.samp").File.FullName}", "Thumper Custom Level Editor");
                return;
            }

            byte[] bytesFromFSB;
            string _hashedname = "";
            bool convertedwav = false;
            //loading label
            lblLoading.Visible = true;
            lblLoading.Invalidate();
            lblLoading.Update();
            lblLoading.Refresh();
            Application.DoEvents();
            //
            if (filepath.EndsWith(".wav")) {
                byte[] wavbytes;
                //see if file is in use and can be read
                try {
                    wavbytes = File.ReadAllBytes(filepath);
                } catch (Exception) {
                    MessageBox.Show("File in use in another program. Import did not succeed.");
                    lblLoading.Visible = false;
                    return;
                }
                //catch non-WAV files  that just happen to have the wav extension
                if (Encoding.UTF8.GetString(wavbytes, 0, 4) != "RIFF") {
                    MessageBox.Show("This does not appear to be a proper .WAV file (header problems).", "Thumper Custom Level Editor");
                    lblLoading.Visible = false;
                    return;
                }
                //some WAV have a "JUNK" header. Need to bypass this
                if (Encoding.UTF8.GetString(wavbytes, 12, 4) == "JUNK") {
                    uint junktable = BitConverter.ToUInt32(wavbytes, 16);
                    byte[] wavbefore = wavbytes[0..12];
                    byte[] wavafter = wavbytes.AsSpan(20 + (int)junktable).ToArray();
                    wavbytes = wavbefore.Concat(wavafter).ToArray();
                }
                uint freq = BitConverter.ToUInt32(wavbytes, 24);
                ulong freqid = FrequencyID.TryGetValue((int)freq, out ulong value) ? value : 8;
                //lookup where data starts and then remove header
                int indexofdata = TCLE.ByteSearch(wavbytes, new byte[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });
                int datalength = BitConverter.ToInt32(wavbytes.AsSpan(indexofdata + 4, 4));
                indexofdata += 8;
                wavbytes = wavbytes.AsSpan(indexofdata, datalength).ToArray();

                if (!Directory.Exists($@"{TCLE.WorkingFolder}\extras"))
                    Directory.CreateDirectory($@"{TCLE.WorkingFolder}\extras");
                using (BinaryWriter sw = new(new FileStream($@"{TCLE.WorkingFolder}\extras\{_filename}.fsb", FileMode.OpenOrCreate))) {
                    sw.Write(Encoding.UTF8.GetBytes("FSB5")); //fsb5
                    sw.Write((UInt32)1); //version
                    sw.Write((UInt32)1); //how many tracks in fsb
                    sw.Write((UInt32)8); //size of sample header
                    sw.Write((UInt32)0x1c); //size of header table
                    sw.Write((UInt32)wavbytes.Length); //sample bytes
                    sw.Write((UInt32)2); //audio type
                    sw.Write((UInt32)0); //always 0, unknown
                    sw.Write((UInt32)0); //flags
                    sw.Write((UInt64)0); //hash1
                    sw.Write((UInt64)0); //hash2
                    sw.Write((UInt64)0); //hash3

                    UInt64 metadata = (UInt64)(wavbytes.Length / 4);//samples in audio (bytes div 4)
                    metadata <<= 27; //make room for next item
                    metadata |= 0; //data offset
                    metadata <<= 2; //make room for next item
                    metadata |= 1; //2^n channels in audio
                    metadata <<= 4; //make room for next item
                    metadata |= freqid; //frequency of audio
                    metadata <<= 1; //make room for next item
                    //the last bit of the metadata is always 0, so I don't need to manip it here.
                    sw.Write(metadata);
                    sw.Write(nametable, 0, nametable.Length);
                    sw.Write(wavbytes, 0, wavbytes.Length);
                }
                filepath = $@"{TCLE.WorkingFolder}\extras\{_filename}.fsb";
                convertedwav = true;
                wavbytes = null;
            }

            bytesFromFSB = File.ReadAllBytes(filepath);
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
                f.Write(PCfileheader, 0, PCfileheader.Length);
                f.Write(bytesFromFSB, 0, bytesFromFSB.Length);
            }
            bytesFromFSB = null;

            //Add new sample entry to the loaded samp_ file
            SampleData newsample = new() {
                obj_name = $"{_filename.Replace(" ", "")}.samp",
                volume = 1,
                pitch = 1,
                pan = 0,
                offset = 0,
                path = $"samples/levels/custom/{_filename}.wav",
                channel_group = "sequin.ch",
                time = -1,
                Editor = this
            };
            newsample.CalculateRuntime();
            SampleList.Add(newsample);
            newsample.UpdateRuntime();

            if (convertedwav)
                File.Delete(filepath);

            TCLE.UpdateProjectSamplesFromFile(LoadedSample, true, out string _);
            lblLoading.Visible = false;
        }

        private void ResetSample()
        {
            //reset things to default values
            samplejson = null;
            SampleList.Clear();
            this.Text = "Sample Editor";
            //set saved flag to true, because nothing is loaded
            SaveCheckAndWrite(true, "");
            FSBtoSamp.Enabled = true;
        }
        #endregion
    }
}
