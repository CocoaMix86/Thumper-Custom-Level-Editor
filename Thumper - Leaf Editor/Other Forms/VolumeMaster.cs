using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass.AddOn.Midi;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class VolumeMaster : Form
    {
        public static int MidiSoundfontHandle;
        public static BASS_MIDI_FONT[] MidiSoundFonts;
        public static int MidiStream;
        BASSTimer _updateTimer = new(50);
        public Visuals _vis = new();

        public VolumeMaster()
        {
            InitializeComponent();
            //write soundfont to file if it doesn't exist
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Sequencer.sf2"))
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Sequencer.sf2", Properties.Resources.Thumper_Sequencer);
            //load soundfont
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Sequencer.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0) };

            MidiStream = BassMidi.BASS_MIDI_StreamCreate(1, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts, 1);
            Bass.BASS_ChannelPlay(MidiStream, true);
            _updateTimer.Tick += new EventHandler(timerUpdate_Tick);
            _updateTimer.Start();
        }

        private void trackMix1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void PlayKeyAtVolume(object sender, MouseEventArgs e)
        {
            TrackBar mixer = sender as TrackBar;
            int key = int.Parse((sender as Control).Tag.ToString());
            BassMidi.BASS_MIDI_StreamEvent(MidiStream, 0, BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)key, (byte)mixer.Value));
            var error = Bass.BASS_ErrorGetCode();
        }

        private static uint MakeWord(byte low, byte high)
        {
            return ((uint)high << 8) | low;
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            //these 2 show different spectrums visually while the sample plays
            pictureWaveL.Image = _vis.CreateSpectrumLinePeak(MidiStream, pictureWaveL.Width, pictureWaveL.Height, Color.Green, Color.Red, Color.Red, Properties.Settings.Default.ColorWaveformBG, 2, 1, 1, 100000, false, false, false);
        }
    }
}
