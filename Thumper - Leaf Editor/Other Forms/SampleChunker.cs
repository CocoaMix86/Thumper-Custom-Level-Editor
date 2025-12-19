using System.ComponentModel;
using System.Data;
using System.Text;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Un4seen.Bass;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class SampleChunker : Form
    {
        #region Variables
        SampleData SampleToChunk;
        Form_SampleEditor ReturnForm;

        //how many seconds pass for 1 beat
        double BeatTime => 60d / (double)TCLE.projectProperties.bpm;
        //how many beats 1 chunk is
        double ChunkSize;
        //total seconds for 1 chunk
        double ChunkTime => BeatTime * ChunkSize;
        //represents seconds
        double Starttime;
        //represents seconds
        double Endtime;
        int Chunklimit;

        int SampleChannel;
        int SampleHandle;
        BASS_SAMPLE SampleInfo;
        byte[] SampleBuffer;
        #endregion

        #region Form Construction
        public SampleChunker(SampleData _samp, Form_SampleEditor _return)
        {
            InitializeComponent();
            ReturnForm = _return;
            btnChunkName.Checked = Properties.Settings.Default.ChunkShowName;
            btnChunkBeats.Checked = Properties.Settings.Default.ChunkShowBeats;
            btnChunkTime.Checked = Properties.Settings.Default.ChunkShowTime;

            SampleToChunk = _samp;
            SampleToChunk.CalculateRuntime();
            SampleToChunk.wave.ColorBackground = Color.Black;
            SampleToChunk.wave.MarkerLength = 1;
            SampleToChunk.wave.ColorMarker = Color.LimeGreen;
            SampleToChunk.wave.DrawMarker = Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.Line | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.Name | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.NameBoxFilled | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.NamePositionTop;
            SampleToChunk.wave.BeatWidth = 2;
            SampleToChunk.wave.DetectBeats = true;
            SampleToChunk.wave.DrawBeat = Un4seen.Bass.Misc.WaveForm.BEATDRAWTYPE.Bottom;

            //initialize the sample
            SampleHandle = Bass.BASS_SampleLoad(SampleToChunk.TempFile, 0, 0, 1, BASSFlag.BASS_SAMPLE_8BITS);
            //get byte buffer
            SampleInfo = Bass.BASS_SampleGetInfo(SampleHandle);
            SampleBuffer = new byte[SampleInfo.length];
            Bass.BASS_SampleGetData(SampleHandle, SampleBuffer);
            SampleChannel = Bass.BASS_SampleGetChannel(SampleHandle, BASSFlag.BASS_SAMPLE_8BITS);
            SampleToChunk.wave.SyncPlayback(SampleChannel);

            Endtime = SampleToChunk.alteredtime;
            DrawWave();

            this.Text = $"Sample Chunker - {SampleToChunk.obj_name}";
            lblMousePos.Text = $"CURRENT BPM = {TCLE.projectProperties.bpm} = 1 min";
            lblRuntime.Text = $"Runtime: {TimeSpan.FromSeconds(_samp.alteredtime).ToString(@"hh\:mm\:ss\.fffff")}";
            lblBeats.Text = $"Beats: {SampleToChunk.beats.ToString("0.#####")}";
            lblBpm.Text = $"Current BPM = {TCLE.projectProperties.bpm} = 1 min";

            numSplitBeat.Maximum = (decimal)SampleToChunk.beats;
            numSplitSec.Maximum = (decimal)_samp.alteredtime;
            txtBeatStart.Maximum = (decimal)SampleToChunk.beats;
            txtBeatEnd.Maximum = (decimal)SampleToChunk.beats;
            txtBeatChunk.Maximum = (decimal)SampleToChunk.beats;

            txtChunkName.Text = SampleToChunk.obj_name.Replace(".samp", "") + "_chunk{X}";
        }
        private void SampleChunker_ResizeEnd(object sender, EventArgs e)
        {
            ParseInputs();
            DrawWave();
        }
        #endregion

        #region Checkboxes
        private void radioTime_CheckedChanged(object sender, EventArgs e)
        {
            txtTimeChunk.Enabled = txtTimeEnd.Enabled = txtTimeStart.Enabled = radioTime.Checked;
            txtBeatChunk.Enabled = txtBeatEnd.Enabled = txtBeatStart.Enabled = !radioTime.Checked;
            ParseInputs();
            DrawMarkers();
        }

        private void chkPosStart_CheckedChanged(object sender, EventArgs e)
        {
            panelStart.Enabled = chkPosStart.Checked;
            if (chkPosStart.Checked)
                if (radioBeats.Checked)
                    Starttime = (double)((txtBeatStart.Value / TCLE.projectProperties.bpm) * 60m);
                else {
                    if (TimeSpan.TryParse(txtTimeStart.Text, out TimeSpan _result))
                        Starttime = _result.TotalSeconds;
                }
            else
                Starttime = 0;
            ParseInputs();
            DrawMarkers();
        }

        private void chkPosEnd_CheckedChanged(object sender, EventArgs e)
        {
            panelEnd.Enabled = chkPosEnd.Checked;
            if (chkPosEnd.Checked) {
                if (radioBeats.Checked)
                    Endtime = (double)((txtBeatEnd.Value / TCLE.projectProperties.bpm) * 60m);
                else {
                    if (TimeSpan.TryParse(txtTimeEnd.Text, out TimeSpan _result))
                        Endtime = _result.TotalSeconds;
                }
            }
            else {
                Endtime = SampleToChunk.alteredtime;
            }
            ParseInputs();
            DrawMarkers();
        }

        private void chkLimit_CheckedChanged(object sender, EventArgs e)
        {
            numChunks.Enabled = chkLimit.Checked;
            ParseInputs();
            DrawMarkers();
        }
        #endregion

        #region Buttons
        private void txtBeatChunk_TextChanged(object sender, EventArgs e)
        {
            ParseInputs();
            DrawMarkers();
        }

        private void txtBeatStart_ValueChanged(object sender, EventArgs e)
        {
            txtBeatEnd.Value = txtBeatStart.Value + 1;
            Starttime = (double)((txtBeatStart.Value / TCLE.projectProperties.bpm) * 60m);
            ParseInputs();
            DrawMarkers();
        }

        private void txtBeatEnd_ValueChanged(object sender, EventArgs e)
        {
            if (chkPosStart.Checked)
                txtBeatEnd.Minimum = txtBeatStart.Value;
            else
                txtBeatEnd.Minimum = 0;
            ParseInputs();
            DrawMarkers();
        }

        private void txtTimeStart_TextChanged(object sender, EventArgs e)
        {
            if (TimeSpan.TryParse(txtTimeStart.Text, out TimeSpan _result)) {
                Starttime = _result.TotalSeconds;
                ParseInputs();
                DrawMarkers();
            }
        }

        private void txtTimeEnd_TextChanged(object sender, EventArgs e)
        {
            if (TimeSpan.TryParse(txtTimeEnd.Text, out TimeSpan _result)) {
                Endtime = _result.TotalSeconds;
                ParseInputs();
                DrawMarkers();
            }
        }

        private void numSplitSec_ValueChanged(object sender, EventArgs e)
        {
            numSplitBeat.ValueChanged -= numSplitBeat_ValueChanged;
            numSplitBeat.Value = (TCLE.projectProperties.bpm / 60m) * numSplitSec.Value;
            numSplitBeat.ValueChanged += numSplitBeat_ValueChanged;
        }

        private void numSplitBeat_ValueChanged(object sender, EventArgs e)
        {
            numSplitSec.ValueChanged -= numSplitSec_ValueChanged;
            numSplitSec.Value = (numSplitBeat.Value / TCLE.projectProperties.bpm) * 60m;
            numSplitSec.ValueChanged += numSplitSec_ValueChanged;
        }

        private void btnAddSplit1_Click(object sender, EventArgs e)
        {
            dgvSplits.Rows.Add(new object[] { (double)numSplitSec.Value, (double)numSplitBeat.Value, Properties.Resources.icon_trash });
            dgvSplits.Sort(dgvSplits.Columns[0], ListSortDirection.Ascending);
            DrawMarkers();
        }

        private void dgvSplits_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
                dgvSplits.Rows.RemoveAt(e.RowIndex);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show($@"Chunking a sample is useful to prevent desyncs in-game. When crashing or pounding, the playing audio can desync and will be off-beat.

By splitting up your sample into smaller chunks, even if 1 chunk desyncs, the remaining ones will start exactly on their designated beats. Smaller chunks make for more desync proof levels (hence why standard loop tracks are only a couple of seconds long).

Chunking a sample will split it where shown on the waveform and create new samples in the .samp file where the original came from. The original sample will not be altered.

If ""End Position"" or ""Limit Chunks"" is checked, the last chunk will end at the last marker. Otherwise, the last chunk will continue until end of the file.

If ""Start Position"" is checked, the first chunk will start at that position. Otherwise, the first chunk will start from the beginning of the file.

", "Cepheus deviL TrommEl tutor");
        }
        #endregion

        #region Methods Functions
        private void ParseInputs()
        {
            if (radioBeats.Checked) {
                double.TryParse(txtBeatChunk.Text, out ChunkSize);
            }
            else {
                double.TryParse(txtTimeChunk.Text, out double _ChunkTime);
                ChunkSize = _ChunkTime / BeatTime;
            }
            Chunklimit = (int)numChunks.Value;
        }

        private void DrawMarkers()
        {
            //clear stuff to prep for drawing new markers
            SampleToChunk.wave.ClearAllMarker();
            double markerpos = Starttime;
            int markernum = 0;
            //draw nothing if the chunk size is 0 (effectively making infinity)
            if (ChunkSize != 0) {
                //there will always be at least 1 split. So "do" first before the while
                do {
                    SampleToChunk.wave.AddMarker($"{(btnChunkName.Checked ? $"chunk {markernum}\n" : " ")}{(btnChunkBeats.Checked ? $"{Math.Round(markerpos / BeatTime, 3)}\n" : " ")}{(btnChunkTime.Checked ? TimeSpan.FromSeconds(markerpos).ToString(@"mm\:ss\.fff") : " ")}".Replace("   ", $"{markernum}"), markerpos);
                    markerpos += ChunkTime;
                    markernum++;
                    if (chkLimit.Checked && markernum > Chunklimit) {
                        SampleToChunk.wave.AddMarker($"chunk end", markerpos);
                        break;
                    }
                }
                while (markerpos < Endtime);
            }

            DrawManualSplits();
            //draw all the markers and the wave
            DrawWave();
            lblChunkTotal.Text = $"Total Chunks: {SampleToChunk.wave.Wave.marker?.Count}";
        }

        private void DrawManualSplits()
        {
            //draw all the manually set splits
            int markernum = 1;
            foreach (DataGridViewRow dgvr in dgvSplits.Rows) {
                SampleToChunk.wave.AddMarker($"m{markernum}", (double)dgvr.Cells[0].Value);
                markernum++;
            }
        }

        private void DrawWave()
        {
            Bitmap WaveToDraw = SampleToChunk.wave.CreateBitmap(pictureWave.Width, pictureWave.Height - 20, -1, -1, true);
            pictureWave.Image = WaveToDraw;
        }
        #endregion


        ///THE CHUNKER
        ///THE MOST IMPORTANT FUNCTION
        ///(of this file)
        private void button1_Click(object sender, EventArgs e)
        {
            if (SampleToChunk.wave.Wave.marker == null || SampleToChunk.wave.Wave.marker.Count == 0) {
                MessageBox.Show("No splits have been set yet.", "Nwolc Custom Level Editor");
                return;
            }
            if (!txtChunkName.Text.Contains("{X}")) {
                MessageBox.Show("Chunk name must include at least 1 {X}.", "ZWest Custom Level Editor");
                return;
            }

            //get markers
            SampleToChunk.wave.SyncPlayback(SampleChannel);
            string[] markers = SampleToChunk.wave.GetMarkers();
            List<long> markerpos = markers.Select(x => SampleToChunk.wave.GetMarker(x)).Order().ToList();
            //add a 0 marker if it doesn't exist
            if (markerpos.Count == 0)
                markerpos.Add(0);
            if (!chkPosStart.Checked && markerpos[0] != 0)
                markerpos.Insert(0, 0);
            //align markers to 4 byte increments (4bytes per channel)
            for (int x = 0; x < markerpos.Count; x++) {
                markerpos[x] = markerpos[x] - (markerpos[x] % (4 * SampleInfo.chans));
            }

            //loop over markers to start splitting them
            for (int x = 0; x < markerpos.Count; x++) {
                //pos and pos2 are the cutoffs for the split.
                long pos = markerpos[x];
                long pos2 = SampleBuffer.Length - 1;
                if (x + 1 < markerpos.Count) {
                    pos2 = markerpos[x + 1];
                }
                //if limiting chunks, break if we're at the last one.
                //otherwise, pos2 becomes buffer.length and final chunk goes to end of file.
                else if (chkLimit.Checked)
                    break;

                //get the portion of bytes that are for this chunk
                byte[] chunkbytes = SampleBuffer[(int)pos..(int)pos2];

                //get the hash of the FSB filename. This will be used to name the final .PC file
                string chunkname = $"{txtChunkName.Text.Replace("{X}", x.ToString())}";
                string _hashedname = "";
                byte[] hashbytes = BitConverter.GetBytes(TCLE.Hash32($"Asamples/levels/custom/{chunkname}.wav"));
                Array.Reverse(hashbytes);
                foreach (byte b in hashbytes)
                    _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
                //if the hashed name starts with a '0', remove it
                if (_hashedname[0] == '0')
                    _hashedname = _hashedname[1..];

                //write the bytes to a new .pc file
                if (!Directory.Exists($@"{TCLE.WorkingFolder}\extras"))
                    Directory.CreateDirectory($@"{TCLE.WorkingFolder}\extras");
                using (BinaryWriter sw = new(new FileStream($@"{TCLE.WorkingFolder}\extras\{_hashedname}.pc", FileMode.OpenOrCreate))) {
                    //write pc file header
                    sw.Write(Form_SampleEditor.PCfileheader);
                    //
                    sw.Write(Encoding.UTF8.GetBytes("FSB5")); //fsb5
                    sw.Write((UInt32)1); //version
                    sw.Write((UInt32)1); //how many tracks in fsb
                    sw.Write((UInt32)8); //size of sample header
                    sw.Write((UInt32)0x1c); //size of header table
                    sw.Write((UInt32)chunkbytes.Length); //sample bytes
                    sw.Write((UInt32)2); //audio type
                    sw.Write((UInt32)0); //always 0, unknown
                    sw.Write((UInt32)0); //flags
                    sw.Write((UInt64)0); //hash1
                    sw.Write((UInt64)0); //hash2
                    sw.Write((UInt64)0); //hash3

                    UInt64 metadata = (UInt64)(chunkbytes.Length / 4);//samples in audio
                    metadata <<= 27; //make room for next item
                    metadata |= 0; //data offset
                    metadata <<= 2; //make room for next item
                    metadata |= 1; //2^n channels in audio
                    metadata <<= 4; //make room for next item
                    metadata |= Form_SampleEditor.FrequencyID[SampleInfo.freq]; //frequency of audio
                    metadata <<= 1; //make room for next item
                    //the last bit of the metadata is always 0, so I don't need to manip it here.
                    sw.Write(metadata);
                    sw.Write(Form_SampleEditor.nametable, 0, Form_SampleEditor.nametable.Length);
                    foreach (byte val in chunkbytes)
                        sw.Write(val);
                }

                //add new entry to the sample file for the chunk
                ReturnForm.SampleList.Add(new() {
                    obj_name = $"{chunkname}.samp",
                    volume = SampleToChunk.volume,
                    pitch = SampleToChunk.pitch,
                    pan = SampleToChunk.pan,
                    offset = 0,
                    path = $"samples/levels/custom/{chunkname}.wav",
                    channel_group = "sequin.ch",
                    time = -1,
                    Editor = ReturnForm
                });
            }

            ReturnForm.SaveCheckAndWrite(false, "Sample chunking");
            if (MessageBox.Show("Chunking has finished. Close the chunker?", "Thlumper Clustom Llevel Elditor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }

        private void btnChunkName_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ChunkShowName = btnChunkName.Checked;
            Properties.Settings.Default.ChunkShowBeats = btnChunkBeats.Checked;
            Properties.Settings.Default.ChunkShowTime = btnChunkTime.Checked;
            Properties.Settings.Default.Save();
            DrawMarkers();
        }

        private void pictureWave_MouseMove(object sender, MouseEventArgs e)
        {
            long bytepos = SampleToChunk.wave.GetBytePositionFromX(e.Location.X, pictureWave.Width, -1, -1);
            double time = Bass.BASS_ChannelBytes2Seconds(SampleHandle, bytepos);
            double beats = time / BeatTime;
            lblMousePos.Text = $"Mouse Pos: {TimeSpan.FromSeconds(time).ToString(@"mm\:ss\.fffff")} ;; Beat {Math.Round(beats, 5)}";
        }
    }
}

