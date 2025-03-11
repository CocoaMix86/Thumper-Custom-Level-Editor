using NAudio.Wave.SampleProviders;
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
        public static int MidiSoundfontHandle = -1;
        public static BASS_MIDI_FONT[] MidiSoundFonts;
        public static List<BASS_MIDI_EVENT>[] SequencerEvents = new List<BASS_MIDI_EVENT>[20];
        private static int LastBeat;
        private static BASSError Error;
        public static System.Threading.Timer SyncTimer;

        public static void Initialize()
        {
            if (MidiSoundfontHandle != -1)
                return;
            //write soundfont to file if it doesn't exist
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Sequencer.sf2"))
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Sequencer.sf2", Properties.Resources.Thumper_Sequencer);
            //load soundfont
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Sequencer.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, -1, 0)};
        }

        ///SOUNDFONT DETAILS
        ///Keys
        ///0 = control stuff (non-instrument)
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
            SamplesToPlay = new();
            SampleEvents = new();
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
                if (Seq.enabled == false || Seq.mute)
                    continue;

                int Key = 0;
                int Call = 0;
                int CallKey = 0;
                if (Seq.obj_name.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase))
                {
                    if (Seq.friendly_param == "turn") {
                        MidiEventsForTurns(Seq);
                    }
                }
                else if (Seq.obj_name == "avatar.lib" && Seq.friendly_param == "speed")
                    MidiEventsForSpeed(Seq);
                else if (Seq.obj_name.EndsWith(".samp", StringComparison.OrdinalIgnoreCase)) {
                    MidiEventPlaySample(Seq);
                }
                else {
                    switch (Seq.obj_name) {
                        case "thump.spn":
                            Key = 8;
                            Call = 8;
                            CallKey = 18;
                            if (Seq.friendly_param == "thump[fast]")
                                Call = 4;
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
                        case "sentry.spn":
                            Key = 14;
                            Call = 0;
                            CallKey = 0;
                            break;
                        case "millipede_quarter.spn":
                            Key = 4;
                            Call = 8;
                            CallKey = 2;
                            break;
                        case "millipede_half.spn":
                        case "millipede_half_decorative.spn":
                        case "millipede_half_decorative_b.spn":
                            Key = 5;
                            Call = 8;
                            CallKey = 2;
                            break;
                        case "jump.spn":
                            Key = -1;
                            Call = 8;
                            CallKey = 6;
                            break;
                        case "jump_high.spn":
                        case "jump_high_big_trees_set.spn":
                        case "jump_high_spikes.spn":
                            Key = -1;
                            Call = 8;
                            CallKey = 17;
                            break;
                    }

                if (Key == 0)
                    continue;

                foreach (SeqDataPoint sdp in Seq.data_points) {
                    if (sdp.value != null)
                        AddNoteToChannel(sdp.beat, Key, Call, CallKey);
                    }
                }
            }
            PitchShiftingBarsRings(Leaf);
            MidiEventsForSentry(Leaf);
            CreateSampleSoundfont();
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
                SequencerEvents[callkey].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)callkey, (byte)(key == -1 ? 100 : 50)), callkey, beat - call, 0));
            }

            if (key != -1) {
                SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)key, 100), key, beat, 0));
                //bar collect also plays ring collect noise
                if (key == 19)
                    SequencerEvents[20].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)20, 100), 20, beat, 0));
            }
        }

        public static void PitchShiftingBarsRings(LeafProperties Leaf)
        {
            Pitch = 8192;
            int Missed = 0;
            List<BASS_MIDI_EVENT> EventsToAdd19 = new();
            List<BASS_MIDI_EVENT> EventsToAdd20 = new();
            //combine ring and bar hit events to get a single track of events, in choronological order
            List<BASS_MIDI_EVENT> ComboList = SequencerEvents[20];/*.Concat(SequencerEvents[20]).ToList();*/
            ComboList.Sort((event1, event2) => event1.tick.CompareTo(event2.tick));

            for (int x = 1; x < ComboList.Count; x++) {
                //test if the event behind current is within 500 ticks (5 beats) of the current event
                if (ComboList[x].tick - ComboList[x - 1].tick is < 500 and not 0) {
                    //if found, pitch up next sound.
                    //add the pitch events to the lists.
                    if (Pitch < 15701) {
                        Pitch += 682;
                        EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, ComboList[x].tick - 1, 0));
                        EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, ComboList[x].tick - 1, 0));
                    }
                    Missed = 0;
                }
                else if (ComboList[x].tick - ComboList[x - 1].tick == 0) { }
                //else, reset pitch and start again
                else if (Missed == 0) {
                    Pitch = 8192;
                    Missed = 1;
                    EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, ComboList[x].tick - 1, 0));
                    EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, ComboList[x].tick - 1, 0));
                }
            }
            //concat new events into lists, then sort by tick to make them chronological
            SequencerEvents[19] = SequencerEvents[19].Concat(EventsToAdd19).ToList();
            SequencerEvents[19].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            SequencerEvents[20] = SequencerEvents[20].Concat(EventsToAdd20).ToList();
            SequencerEvents[20].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        public static void MidiEventsForTurns(Sequencer_Object Seq)
        {
            int IsTurning = 0;
            for (int x = 0; x < Seq.data_points.Count; x++)
            {
                if (Seq.data_points[x].value != null)
                {
                    if ((decimal)Seq.data_points[x].value >= 15) {
                        if (IsTurning == -1) {
                            AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 10);
                            IsTurning = 1;
                        }
                        else if (IsTurning == 1) {
                            IsTurning = 2;
                            AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 11);
                        }
                        else if (IsTurning == 0)
                            IsTurning = 1;
                    }
                    else if ((decimal)Seq.data_points[x].value <= -15) {
                        if (IsTurning == 1) {
                            AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 12);
                            IsTurning = -1;
                        }
                        else if (IsTurning == -1) {
                            IsTurning = -2;
                            AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 11);
                        }
                        else if (IsTurning == 0)
                            IsTurning = -1;
                    }
                }
                else {
                    if (IsTurning == -1) 
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 10);                    
                    else if (IsTurning == 1) 
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 12);
                    IsTurning = 0;
                }
            }
            //handle last beat
            if (IsTurning == -1)
                AddNoteToChannel(Seq.data_points[^1].beat - 1, 13, 8, 10);
            else if (IsTurning == 1)
                AddNoteToChannel(Seq.data_points[^1].beat - 1, 13, 8, 12);
        }

        public static void MidiEventsForSentry(LeafProperties Leaf)
        {
            List<BASS_MIDI_EVENT> EventsToAdd15 = new();
            List<BASS_MIDI_EVENT> EventsToAdd16 = new();
            foreach (Sequencer_Object Seq in Leaf.seq_objs.Where(x => x.obj_name == "sentry.spn"))
            {
                int length = 0;
                switch (Seq.friendly_param) {
                    case "single lane [55 beats]":
                        length = 55;
                        break;
                    case "single lane [79 beats]":
                        length = 79;
                        break;
                    case "single lane [114 beats]":
                        length = 114;
                        break;
                    case "multi lane [126 beats]":
                        length = 126;
                        break;
                    case "single lane [129 beats]":
                        length = 129;
                        break;
                    case "single lane [150 beats]":
                    case "multi lane [150 beats]":
                        length = 150;
                        break;
                }

                foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.value != null)) {
                    //find events that fall inside the sentry activation time
                    foreach (BASS_MIDI_EVENT _event in SequencerEvents[8].Where(x => x.tick > (sdp.beat*100)+400 && x.tick <= (sdp.beat + length)*100)) {
                        //if the sentry call event doesn't exist yet, add it (so we don't duplicate on sounds)
                        if (!EventsToAdd15.Any(x => x.tick == _event.tick - 400)) {
                            //Sentry call happens 4 beats ahead (400 ticks)
                            EventsToAdd15.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)16, 100), 16, _event.tick - 400, 0));
                        }
                    }
                    if (sdp.beat + length < LastBeat) {
                        EventsToAdd16.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)15, 100), 15, (sdp.beat + length + CallOffset) * 100, 0));
                    }
                }
            }

            SequencerEvents[15] = EventsToAdd15;
            SequencerEvents[15].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            SequencerEvents[16] = EventsToAdd16;
            SequencerEvents[16].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        public static void MidiEventsForSpeed(Sequencer_Object Seq)
        {
            foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.value != null)) {
                //SequencerEvents[0].Add(new(BASSMIDIEvent.MIDI_EVENT_SPEED, (int)(10_000 * (decimal)sdp.value), 0, (sdp.beat + CallOffset) * 100, 0));
            }
        }

        public static List<string> SamplesToPlay = new();
        public static List<List<BASS_MIDI_EVENT>> SampleEvents = new();
        public static void MidiEventPlaySample(Sequencer_Object Seq)
        {
            SamplesToPlay.Add(Seq.obj_name);
            SampleEvents.Add(new());
            SampleData SampToPlay = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == Seq.obj_name);
            int velocity = (int?)(SampToPlay?.volume * 100) ?? 100;
            if (velocity > 127)
                velocity = 127;
            foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.value != null)) {
                SampleEvents[^1].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)SamplesToPlay.Count, (byte)velocity), SequencerEvents.Length + SamplesToPlay.Count - 1, (sdp.beat + CallOffset) * 100, 0));
            }
        }

        public static void CreateSampleSoundfont()
        {
            string path = $@"{TCLE.AppLocation}\temp\";
            string _out = $"<control>\r\ndefault_path={path}\r\n\r\n<group>\r\n\r\n";
            foreach (string sample in SamplesToPlay) {
                string FileName = TCLE.PCtoAudioFile(TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == sample));
                _out += $"<region> sample={Path.GetFileName(FileName)} key={SamplesToPlay.IndexOf(sample) + 1}\r\n";
            }
            _out += "\r\n\r\n";
            File.WriteAllText($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz", _out);

            int SamplesSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz");
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesSoundfontHandle, 1, 0) };
        }

        public static void ChannelEnd()
        {
            //set instrument to use and tempo
            //These need to be at tick 0, on channel 0
            //SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 0, 0, 0, 0));
            SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 36, 0, 0, 0));
            SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_TEMPO, (int)Microseconds, 0, 0, 0));
            //cap off each channel with an END event
            for (int x = 0; x < SequencerEvents.Length; x++) {
                if (SequencerEvents[x].Count > 0) {
                    int tickend = SequencerEvents[x].Last().tick;
                    SequencerEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, x, (LastBeat * 100) + 100, 0));
                }
                //make sure all events are in proper tick order
                SequencerEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }

            for (int x = 0; x < SampleEvents.Count; x++) {
                int channeloffset = SequencerEvents.Length + x;
                SampleEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 1, channeloffset, 0, 0));
                if (SampleEvents[x].Count > 0) {
                    int tickend = SampleEvents[x].Last().tick;
                    SampleEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, channeloffset, (LastBeat * 100) + 100, 0));
                }
                //make sure all events are in proper tick order
                SampleEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }
        }

        public static void Play(double StartTime)
        {
            ChannelEnd();
            //merge all channels to a single array of events
            List<BASS_MIDI_EVENT> _SequencerEvents = Playback.SequencerEvents.SelectMany(x => x).ToList();
            List<BASS_MIDI_EVENT> _SampleEvents = Playback.SampleEvents.SelectMany(x => x).ToList();
            var AllEvents = _SequencerEvents.Concat(_SampleEvents).ToList();
            //the very last midi event needs to be EVENT_END
            AllEvents.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, (LastBeat * 100) + 50, 0));
            //create the stream
            MidiStream = BassMidi.BASS_MIDI_StreamCreateEvents(AllEvents.ToArray(), 100, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            Error = Bass.BASS_ErrorGetCode();
            //apply soundfonts
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts, 2);
            //set ending sync
            int ee = Bass.BASS_ChannelSetSync(MidiStream, BASSSync.BASS_SYNC_END, 0, EndingProc, 0);
            ColumnPlaybackHead = -9;
            if (StartTime != 0) {
                ColumnPlaybackHead = (int)StartTime;
                Bass.BASS_ChannelSetPosition(MidiStream, (60 / (double)TCLE.BPM) * (StartTime + 9));
                Error = Bass.BASS_ErrorGetCode();
            }
            //play the sequence
            if (Bass.BASS_ChannelPlay(MidiStream, ColumnPlaybackHead > 0 ? false : true)) {
                SyncTimer = new(new TimerCallback(SyncTimer_Tick), null, 0, (int)((60 / TCLE.BPM) * 1000));
                IsPlaying = true;
            }
            else
                Error = Bass.BASS_ErrorGetCode();
        }

        private static uint MakeWord(byte low, byte high)
        {
            return ((uint)high << 8) | low;
        }

        public static SYNCPROC EndingProc = new(OnEnding);
        public static void OnEnding(int handle, int channel, int data, IntPtr user)
        {
            IsPlaying = false;
            SyncTimer.Dispose();
            //SyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
            bool free1 = Bass.BASS_ChannelStop(channel);
            bool free2 = Bass.BASS_ChannelFree(channel);
            TCLE.alzheimer();
        }

        public static int ColumnPlaybackHead;
        private static void SyncTimer_Tick(object sender)
        {
            ColumnPlaybackHead = (int)Math.Floor(Bass.BASS_ChannelGetPosition(MidiStream, BASSMode.BASS_POS_MIDI_TICK) / 100d) - 8;
            //ColumnPlaybackHead++;
        }
    }
}
