using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
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

        private void txtBeatChunk_TextChanged(object sender, EventArgs e)
        {
            ParseInputs();
            DrawMarkers();
        }

        private void ParseInputs()
        {
            if (radioBeats.Checked) {
                double.TryParse(txtBeatChunk.Text, out ChunkSize);
            }
            Chunklimit = (int)numChunks.Value;
        }

        private void DrawMarkers()
        {
            if (ChunkSize == 0)
                return;

            SampleToChunk.wave.ClearAllMarker();
            double markerpos = Starttime;
            if (!chkPosEnd.Checked) {
                Endtime = SampleToChunk.alteredtime;
            }
            int markernum = 1;
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

            DrawWave();
        }

        private void DrawWave()
        {
            Bitmap WaveToDraw = SampleToChunk.wave.CreateBitmap(pictureWave.Width, pictureWave.Height - 20, -1, -1, true);
            pictureWave.Image = WaveToDraw;
        }
    }
}
