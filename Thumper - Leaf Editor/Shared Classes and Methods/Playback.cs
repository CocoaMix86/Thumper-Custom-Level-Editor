using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;
using Windows.Devices.Bluetooth.Advertisement;

namespace Thumper_Custom_Level_Editor
{ 
    public static class Playback
    {
        public static decimal BPM => TCLE.BPM;
        public static int MidiStream;
        public static Dictionary<string, int> PlaybackChannels = new() {
            { "thump", 0 },
            { "turn", 0 },
            { "ring", 0 },
            { "bar", 0 },
            { "sentryclose", 0 },
            { "sentrywoomp", 0 }
            };
        public static int MidiSoundfontHandle;
        public static BASS_MIDI_FONT[] MidiSoundFonts;

        public static void Initialize()
        {
            //initialize midi stream
            MidiStream = BassMidi.BASS_MIDI_StreamCreate(128, BASSFlag.BASS_SAMPLE_FLOAT, (int)BPM);
            //load soundfont
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Sequencer.sf2"))
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Sequencer.sf2", Properties.Resources.Thumper_Sequencer);
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Sequencer.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, -1, 0)};
            //apply soundfont to stream
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts, 1);
        }

        public static void CreatePlayback(List<Sequencer_Object> SeqObjs)
        {
            /*
            BassMidi.
            int channel = 0;
            List<BASS_MIDI_EVENT> events;
            foreach (Sequencer_Object Seq in SeqObjs) {
                foreach (SeqDataPoint DataPoint in Seq.data_points) {
                    if (DataPoint.value == null)
                        continue;
                    events.Add(new(,));
                }
            }
            */
        }
    }
}
