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
        public static int MidiStream = -1;
        public static Dictionary<string, int> PlaybackChannels = new() {
            { "thump", 0 },
            { "turn", 0 },
            { "ring", 0 },
            { "bar", 0 },
            { "sentryclose", 0 },
            { "sentrywoomp", 0 }
            };
        public static int MidiSoundfontHandle = -1;
        public static BASS_MIDI_FONT[] MidiSoundFonts;

        public static void Initialize()
        {
            if (MidiStream != -1)
                return;
            //initialize midi stream
            //MidiStream = BassMidi.BASS_MIDI_StreamCreate(10, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            //load soundfont
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Sequencer.sf2"))
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Sequencer.sf2", Properties.Resources.Thumper_Sequencer);
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Sequencer.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, -1, 0)};
            //apply soundfont to stream
            //BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts, 1);
        }

        public static void CreatePlayback(List<Sequencer_Object> SeqObjs)
        {
            List<BASS_MIDI_EVENT> events = new() {
                new(BASSMIDIEvent.MIDI_EVENT_TEMPO, 500000, 0, 0, 0), // set the tempo to 0.5 seconds per quarter note
                new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 0, 0, 0, 0), // select the first instrument in soundfont
                //new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord(1, 100), 0, 0, 0), // press the key
                //new (BASSMIDIEvent.MIDI_EVENT_NOTE, 1, 0, 400, 0), // release the key after 200 ticks
                //new (BASSMIDIEvent.MIDI_EVENT_END, 0, 0, 500, 0) // end after 400 ticks
            };

            for (byte x = 1; x < 44; x++)
            {
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, x, 0, x * 100, 0));
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord(x, 100), 0, x*100 + 1, 0));
            }

            events.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, 5000, 0));

            //play the sequence
            int stream = BassMidi.BASS_MIDI_StreamCreateEvents(events.ToArray(), 100, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            BassMidi.BASS_MIDI_StreamSetFonts(stream, MidiSoundFonts, 1);
            Bass.BASS_ChannelPlay(stream, true);


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

        public static uint MakeWord(byte low, byte high)
        {
            return ((uint)high << 8) | low;
        }
    }
}
