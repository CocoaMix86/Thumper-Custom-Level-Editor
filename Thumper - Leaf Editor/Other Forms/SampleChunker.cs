using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Un4seen.Bass;
using Windows.Devices.Lights;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class SampleChunker : Form
    {
        #region Variables
        SampleData SampleToChunk;
        Form_SampleEditor ReturnForm;

        double BeatTime => 60d / (double)TCLE.projectProperties.bpm;
        double ChunkSize = 0;
        double ChunkTime => BeatTime * ChunkSize;
        double Starttime = 0;
        double Endtime = 0;
        int Chunklimit = 0;
        #endregion

        #region Form Construction
        public SampleChunker(SampleData _samp, Form_SampleEditor _return)
        {
            InitializeComponent();
            ReturnForm = _return;

            SampleToChunk = _samp;
            SampleToChunk.CalculateRuntime();
            SampleToChunk.wave.ColorBackground = Color.Black;
            SampleToChunk.wave.MarkerLength = 1;
            SampleToChunk.wave.ColorMarker = Color.LimeGreen;
            SampleToChunk.wave.DrawMarker = Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.Line | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.Name | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.NamePositionTop | Un4seen.Bass.Misc.WaveForm.MARKERDRAWTYPE.NameBoxFilled;
            SampleToChunk.wave.BeatWidth = 2;
            SampleToChunk.wave.DetectBeats = true;
            SampleToChunk.wave.DrawBeat = Un4seen.Bass.Misc.WaveForm.BEATDRAWTYPE.Bottom;

            DrawWave();

            this.Text = $"Sample Chunker - {SampleToChunk.obj_name}";
            lblBpm.Text = $"CURRENT BPM = {TCLE.projectProperties.bpm} = 1 min";
            lblRuntime.Text = $"Runtime: {TimeSpan.FromSeconds(_samp.alteredtime).ToString(@"hh\:mm\:ss\.fffff")}";
            lblBeats.Text = $"Beats: {SampleToChunk.beats.ToString("0.#####")}";

            numSplitBeat.Maximum = (decimal)SampleToChunk.beats;
            numSplitSec.Maximum = (decimal)_samp.alteredtime;
            txtBeatStart.Maximum = (decimal)SampleToChunk.beats;
            txtBeatEnd.Maximum = (decimal)SampleToChunk.beats;
            txtBeatChunk.Maximum = (decimal)SampleToChunk.beats;
        }
        private void SampleChunker_ResizeEnd(object sender, EventArgs e)
        {
            DrawWave();
        }
        #endregion

        #region Checkboxes
        private void radioTime_CheckedChanged(object sender, EventArgs e)
        {
            txtTimeChunk.Enabled = txtTimeEnd.Enabled = txtTimeStart.Enabled = radioTime.Checked;
            txtBeatChunk.Enabled = txtBeatEnd.Enabled = txtBeatStart.Enabled = !radioTime.Checked;
        }

        private void chkPosStart_CheckedChanged(object sender, EventArgs e)
        {
            panelStart.Enabled = chkPosStart.Checked;
            if (chkPosStart.Checked)
                Starttime = (double)((txtBeatStart.Value / TCLE.projectProperties.bpm) * 60m);
            else
                Starttime = 0;
        }

        private void chkPosEnd_CheckedChanged(object sender, EventArgs e)
        {
            panelEnd.Enabled = chkPosEnd.Checked;
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
        #endregion

        #region Methods Functions
        private void ParseInputs()
        {
            if (radioBeats.Checked) {
                double.TryParse(txtBeatChunk.Text, out ChunkSize);
            }
            Chunklimit = (int)numChunks.Value;
        }

        private void DrawMarkers()
        {
            //clear stuff to prep for drawing new markers
            SampleToChunk.wave.ClearAllMarker();
            double markerpos = Starttime;
            if (!chkPosEnd.Checked) {
                Endtime = SampleToChunk.alteredtime;
            }
            int markernum = 1;
            //draw nothing if the chunk size is 0 (effectively making infinity)
            if (ChunkSize != 0) {
                //there will always be at least 1 split. So "do" first before the while
                do {
                    SampleToChunk.wave.AddMarker($"chunk {markernum}", markerpos);
                    markerpos += ChunkTime;
                    markernum++;
                    if (chkLimit.Checked && markernum > Chunklimit) {
                        break;
                    }
                }
                while (markerpos < Endtime);
                SampleToChunk.wave.AddMarker($"chunk end", markerpos);
            }

            DrawManualSplits();
            //draw all the markers and the wave
            DrawWave();
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
            //initialize the sample
            int channel = Bass.BASS_StreamCreateFile(SampleToChunk.TempFile, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
            //pitch shift, pan, other fx
            float initialfreq = 0;
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, ref initialfreq);
            var err = Bass.BASS_ErrorGetCode();
            Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, initialfreq * (float)SampleToChunk.pitch);
            err = Bass.BASS_ErrorGetCode();
            //get byte buffer
            long bytelength = Bass.BASS_ChannelGetLength(channel, BASSMode.BASS_POS_BYTE);
            byte[] buffer = new byte[bytelength];

            //get markers
            string[] markers = SampleToChunk.wave.GetMarkers();
            List<long> markerpos = markers.Select(x => SampleToChunk.wave.GetMarker(x)).Order().ToList();
            //loop over markers to start splitting them
            for (int x = 0; x < markerpos.Count; x++) {
                //pos and pos2 are the cutoffs for the split. pos2 = -1 means end of file
                long pos = markerpos[x];
                long pos2 = -1;
                if (x + 1 < markerpos.Count)
                    pos2 = SampleToChunk.wave.GetMarker(markers[x + 1]);

                Bass.BASS_ChannelGetData(channel, buffer);
                err = Bass.BASS_ErrorGetCode();
            }
        }
    }
}
