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
        public static double BPM => (double)TCLE.BPM;
        public static double Microseconds => (60 / BPM) * 1_000_000;
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
                new(BASSMIDIEvent.MIDI_EVENT_TEMPO, (int)Microseconds, 0, 0, 0), // set the tempo to 0.5 seconds per quarter note
                new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 0, 0, 0, 0), // select the first instrument in soundfont
            };

            for (byte x = 1; x < 44; x++)
            {
                if (x % 2 == 0)
                    continue;
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, 40, 0, x * 10, 0));
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord(40, 100), 0, x*10 + 1, 0));
            }
            events.Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, 0, 5000, 0));
            for (byte x = 1; x < 44; x++)
            {
                if (x % 2 == 1)
                    continue;
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, 41, 1, x * 10, 0));
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord(41, 100), 1, x * 10 + 1, 0));
            }
            events.Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, 0, 5000, 0));

            int Channel = 0;
            foreach (Sequencer_Object Seq in SeqObjs)
            {
                int Key = 0;
                switch (Seq.obj_name)
                {
                    case "thump.spn":
                        break;
                }

                foreach (SeqDataPoint sdp in Seq.data_points)
                {

                }
                events.Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, Channel, 5000, 0));
            }

            events.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, 5000, 0));

            //play the sequence
            int stream = BassMidi.BASS_MIDI_StreamCreateEvents(events.ToArray(), 1, BASSFlag.BASS_SAMPLE_FLOAT, 0);
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
