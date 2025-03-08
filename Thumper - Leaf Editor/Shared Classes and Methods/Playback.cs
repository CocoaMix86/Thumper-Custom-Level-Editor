using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;
using Windows.Devices.Bluetooth.Advertisement;

namespace Thumper_Custom_Level_Editor
{ 
    public static class Playback
    {
        public static bool IsPlaying;
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
        public static List<BASS_MIDI_EVENT>[] SequencerEvents = new List<BASS_MIDI_EVENT>[20];
        private static int LastBeat;
        private static BASSError Error;

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

        ///SOUNDFONT DETAILS
        ///Keys
        ///0 = 
        ///1 = bar appear
        ///2 = millipede appear
        ///3 = millipede full land
        ///4 = millipede half land
        ///5 = millipede quarter land
        ///6 = mushroom (jump) appear
        ///7 = ring appear
        ///8 = thump hit
        ///9 = turn 360 horn
        ///10= turn appear left
        ///11= turn long appear
        ///12= turn appear right
        ///13= turn perfect
        ///14= sentry appear
        ///15= sentry end
        ///16= sentry thump
        ///17= spike (jump) appear
        ///18= thump appear
        ///19= bar collect (rising semitons)
        ///20= ring collect (rising semitones)

        public static void CreatePlaybackFromLeaf(LeafProperties Leaf)
        {
            SequencerEvents = new List<BASS_MIDI_EVENT>[21];
            for (int x = 0; x < 21; x++) {
                // +8 for lead time
                SequencerEvents[x] = new(Leaf.beats + CallOffset);
            }
            SequencerEvents[19].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 12, 19, 2, 0));
            SequencerEvents[20].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 12, 20, 2, 0));
            LastBeat = Leaf.beats + CallOffset;

            foreach (Sequencer_Object Seq in Leaf.seq_objs)
            {
                //don't playback disabled items
                if (Seq.enabled == false)
                    continue;

                int Key = 0;
                int Call = 0;
                int CallKey = 0;
                switch (Seq.obj_name)
                {
                    case "thump.spn":
                        Key = 8;
                        Call = 8;
                        CallKey = 18;
                        break;
                    case "grindable.spn":
                        Key = 19;
                        Call = 8;
                        CallKey = 1;
                        break;
                    case "ducker.spn":
                        Key = 20;
                        Call = 8;
                        CallKey = 7;
                        break;
                }

                if (Key == 0)
                    continue;

                foreach (SeqDataPoint sdp in Seq.data_points) {
                    if (sdp.beat > LastBeat)
                        LastBeat = sdp.beat;
                    if (sdp.value != null)
                        AddNoteToChannel(sdp.beat, Key, Call, CallKey);
                }
            }
            PitchShiftingBarsRings(Leaf);
        }

        /// Key and Channel are the same thing
        private static int Pitch = 8192;
        private static int CallOffset = 9;
        public static void AddNoteToChannel(int beat, int key, int call, int callkey)
        {
            //beats land on multiples of 100 ticks.
            //to handle offsetting calls, increase beats by 8.
            beat = (beat + CallOffset) * 100;
            call *= 100;
            if (call > 0) {
                SequencerEvents[callkey].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)callkey, 100), callkey, beat - call, 0));
            }

            //handle pitch shifting for bar/ring sequential collection
            /*
            if (key is 19 or 20) {
                if ((SequencerEvents[19].Count > 0 && beat - SequencerEvents[19].Last().tick < 500) || (SequencerEvents[20].Count > 0 && beat - SequencerEvents[20].Last().tick < 500)) {
                    if (Pitch < 15701) {
                        Pitch += 682;
                        SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, key, beat, 0));
                    }
                }
                else {
                    Pitch = 8192;
                    SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, key, beat, 0));
                }
            }
            */
            SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)key, 100), key, beat, 0));
            //bar collect also plays ring collect noise
            if (key == 19)
                SequencerEvents[20].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)20, 100), 20, beat, 0));
        }

        public static void PitchShiftingBarsRings(LeafProperties Leaf)
        {
            Pitch = 8192;
            int Missed = 0;
            List<BASS_MIDI_EVENT> EventsToAdd19 = new();
            List<BASS_MIDI_EVENT> EventsToAdd20 = new();
            List<BASS_MIDI_EVENT> ComboList = SequencerEvents[19].Concat(SequencerEvents[20]).ToList();
            ComboList.Sort((event1, event2) => event1.tick.CompareTo(event2.tick));

            for (int x = 1; x < ComboList.Count; x++) {
                if (ComboList[x].tick - ComboList[x - 1].tick is < 500 and > 0) {
                    if (Pitch < 15701) {
                        Pitch += 682;
                        EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, ComboList[x].tick - 1, 0));
                        EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, ComboList[x].tick - 1, 0));
                    }
                    Missed = 0;
                }
                else
                    Missed++;

                if (Missed == 5) {
                    Missed = 0;
                    Pitch = 8192;
                    EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, ComboList[x].tick - 1, 0));
                    EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, ComboList[x].tick - 1, 0));
                }
            }

            SequencerEvents[19] = SequencerEvents[19].Concat(EventsToAdd19).ToList();
            SequencerEvents[19].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            SequencerEvents[20] = SequencerEvents[20].Concat(EventsToAdd20).ToList();
            SequencerEvents[20].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        public static void ChannelEnd()
        {
            //set instrument to use and tempo
            //These need to be at tick 0, on channel 0
            SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 0, 0, 0, 0));
            SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 36, 0, 0, 0));
            SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_TEMPO, (int)Microseconds, 0, 0, 0));
            //cap off each channel with an END event
            for (int x = 0; x < SequencerEvents.Length; x++) {
                if (SequencerEvents[x].Count > 0) {
                    int tickend = SequencerEvents[x].Last().tick;
                    SequencerEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, x, (LastBeat * 100) + 100, 0));
                }
            }
        }

        public static void Play()
        {
            ChannelEnd();
            //merge all channels to a single array of events
            List<BASS_MIDI_EVENT> events = SequencerEvents.SelectMany(x => x).ToList();
            //the very last midi event needs to be EVENT_END
            events.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, LastBeat, 0));
            //play the sequence
            MidiStream = BassMidi.BASS_MIDI_StreamCreateEvents(events.ToArray(), 100, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            Error = Bass.BASS_ErrorGetCode();
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts, 1);
            if (Bass.BASS_ChannelPlay(MidiStream, true))
                IsPlaying = true;
            else
                Error = Bass.BASS_ErrorGetCode();
        }

        private static uint MakeWord(byte low, byte high)
        {
            return ((uint)high << 8) | low;
        }
    }
}
