using Thumper_Custom_Level_Editor.Editor_Panels;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace Thumper_Custom_Level_Editor
{ 
    public static class Playback
    {
        public static bool IsPlaying {
            get => _isplay;
            set {
                _isplay = value;
                TCLE.MainBeeble.Dance(value);
            }
        }
        private static bool _isplay;
        public static string Type;
        public static bool IsLooping;
        public static bool Generating;
        public static double LoopingStartTime;
        public static double BPM => (double)TCLE.BPM;
        public static double Microseconds => (60 / BPM) * 1_000_000;
        public static int MidiStream = -1;
        public static int MidiSoundfontHandle = -1;
        public static BASS_MIDI_FONT[] MidiSoundFonts;
        private static BASSError Error;
        public static System.Threading.Timer SyncTimer;
        //
        public static List<BASS_MIDI_EVENT>[] SequencerEvents = new List<BASS_MIDI_EVENT>[23];
        private static int LeafLastBeat;
        //
        public static List<string> GlobalSamplesToPlay = new();
        public static List<List<BASS_MIDI_EVENT>> GlobalLoopEvents = new();
        public static List<BASS_MIDI_EVENT>[] GlobalSequencerEvents = new List<BASS_MIDI_EVENT>[23];
        public static List<List<BASS_MIDI_EVENT>> GlobalSampleEvents = new();
        public static List<SeqDataPoint> GlobalSpeedEvents = new();
        public static List<Tuple<string, int>> GlobalLeafQueue = new();
        public static List<Tuple<string, int>> GlobalLvlQueue = new();
        public static List<Tuple<string, int>> GlobalGateQueue = new();
        public static List<Tuple<string, string, decimal>> GlobalLoopTracks = new();
        public static string GlobalCurrentLeaf = "???";
        public static string GlobalCurrentLvl = "???";
        public static string GlobalCurrentGate = "???";
        public static int GlobalCurrentOffset = -1;
        public static int GlobalCurrentOffsetLvl = -1;
        public static int GlobalCurrentOffsetGate = -1;

        public static void Initialize(string _Type)
        {
            CallOffset = 9;
            GlobalCurrentLeaf = "???";
            GlobalCurrentLvl = "???";
            GlobalCurrentGate = "???";
            GlobalCurrentOffset = -1;
            GlobalCurrentOffsetLvl = -1;
            GlobalCurrentOffsetGate = -1;
            Type = _Type;
            //show the loading message
            TCLE.Instance.panelLoadingMessage.Visible = true;
            TCLE.Instance.panelLoadingMessage.Invalidate();
            TCLE.Instance.panelLoadingMessage.Update();
            TCLE.Instance.panelLoadingMessage.Refresh();
            Application.DoEvents();
            //
            GlobalSampleEvents = new();
            GlobalLoopEvents = new();
            GlobalLoopTracks = new();
            GlobalSpeedEvents = new();
            GlobalSamplesToPlay = new();
            GlobalLeafQueue = new();
            GlobalLvlQueue = new();
            GlobalGateQueue = new();
            GlobalSequencerEvents = new List<BASS_MIDI_EVENT>[23];
            for (int x = 0; x < GlobalSequencerEvents.Length; x++) {
                // +8 for lead time
                GlobalSequencerEvents[x] = new();
            }
            //write soundfont to file if it doesn't exist
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Sequencer_21.sf2"))
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Sequencer_21.sf2", Properties.Resources.Thumper_Sequencer);
            //load soundfont
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Sequencer_21.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0)};
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
        ///21= lane end
        ///22= turn silent (for long turns)

        public static void CreatePlaybackFromLeaf(LeafProperties Leaf, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingLeaf.Text = $"Leaf: {Leaf.FilePath.Name}";
            TCLE.Instance.lblLoadingLeaf.Invalidate();
            TCLE.Instance.lblLoadingLeaf.Update();
            TCLE.Instance.lblLoadingLeaf.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            BeatOffset = _BeatOffset;
            SequencerEvents = new List<BASS_MIDI_EVENT>[23];
            for (int x = 0; x < SequencerEvents.Length; x++) {
                SequencerEvents[x] = new(Leaf.beats + CallOffset);
            }

            LeafLastBeat = Leaf.beats;
            if (BeatStop > 0) {
                BeatStop += 1;
                LeafLastBeat = Math.Min(Leaf.beats, BeatStop);
            }
            GlobalLeafQueue.Add(new Tuple<string, int>(Leaf.FilePath.Name, (BeatOffset) * 100));

            foreach (Sequencer_Object Seq in Leaf.seq_objs)
            {
                //don't playback disabled items
                if (Seq.enabled == false || Seq.mute)
                    continue;

                int Key = 0;
                int Call = 0;
                int CallKey = 0;
                if (Seq.obj_name.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase) || Seq.obj_name == "leafname")
                {
                    if (Seq.friendly_param == "turn") {
                        MidiEventsForTurns(Seq);
                    }
                    else if (Seq.friendly_param is "lane left 2" or "lane left 1" or "lane center" or "lane right 1" or "lane right 2") {
                        MidiEventsForLanes(Seq);
                    }
                }
                else if (Seq.obj_name == "avatar.lib" && Seq.friendly_param == "speed") {                    
                   // MidiEventsForSpeed(Seq);
                }
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
                        case "grindable_multi.spn":
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

                    //If the default for bools and actions is 1, every beat will trigger, so don't check for null.
                    //instead, check for any beat set to 0.
                    if (Seq.trait_type is "kTraitBool" or "kTraitAction" && Seq.defaultvalue is 1) {
                        for (int beat = 0; beat < LeafLastBeat; beat++) {
                            if (Seq.data_points[beat].value == null || (Seq.data_points[beat].value != null && (decimal)Seq.data_points[beat].value != 0)) {
                                AddNoteToChannel(Seq.data_points[beat].beat, Key, Call, CallKey, Seq.mute);
                                if (Seq.obj_name == "grindable_multi.spn") {
                                    if (Seq.friendly_param == "bar[double]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.5d, Key, Call, CallKey, Seq.mute);
                                    }
                                    else if (Seq.friendly_param == "bar[triple]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.3333d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.6666d, Key, Call, CallKey, Seq.mute);
                                    }
                                    else if (Seq.friendly_param == "bar[quad]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.25d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.50d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.75d, Key, Call, CallKey, Seq.mute);
                                    }
                                    else if (Seq.friendly_param == "thump and bar") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.5d, 8, 8, 18, Seq.mute);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        for (int beat = 0; beat < LeafLastBeat; beat++) {
                            if (Seq.data_points[beat].value != null) {
                                AddNoteToChannel(Seq.data_points[beat].beat, Key, Call, CallKey, Seq.mute);
                                if (Seq.obj_name == "grindable_multi.spn") {
                                    if (Seq.friendly_param == "bar[double]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.5d, Key, Call, CallKey, Seq.mute);
                                    }
                                    else if (Seq.friendly_param == "bar[triple]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.3333d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.6666d, Key, Call, CallKey, Seq.mute);
                                    }
                                    else if (Seq.friendly_param == "bar[quad]") {
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.25d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.50d, Key, Call, CallKey, Seq.mute);
                                        AddNoteToChannel(Seq.data_points[beat].beat + 0.75d, Key, Call, CallKey, Seq.mute);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            MidiEventsForSentry(Leaf);
            MidiEventsForSpeed(Leaf.seq_objs.FirstOrDefault(x => x.obj_name == "avatar.lib" && x.friendly_param == "speed"));
            //copy over single leaf results to global so it doesn't get cleared
            for (int x = 0; x < SequencerEvents.Length; x++) {
                if (SequencerEvents[x].Count > 0)
                    GlobalSequencerEvents[x] = GlobalSequencerEvents[x].Concat(SequencerEvents[x]).ToList();
            }
        }

        public static void CreatePlaybackFromLvl(LvlProperties Lvl, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingLvl.Text = $"Lvl: {Lvl.FilePath.Name}";
            TCLE.Instance.lblLoadingLvl.Invalidate();
            TCLE.Instance.lblLoadingLvl.Update();
            TCLE.Instance.lblLoadingLvl.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            GlobalLvlQueue.Add(new Tuple<string, int>(Lvl.FilePath.Name, (_BeatOffset) * 100));
            Playback.CallOffset = 0;
            int beatoffset = _BeatOffset;
            if (_BeatOffset == 0)
                beatoffset = Lvl.approachbeats < 8 ? 8 : Lvl.approachbeats;
            //create playback of the lvl sequencer
            Form_LeafEditor lvlseq = new(Lvl);
            Playback.CreatePlaybackFromLeaf(lvlseq.leafProperties, lvlseq.leafProperties.beats, beatoffset - Lvl.approachbeats);
            lvlseq.Dispose();
            //create playback for each leaf
            foreach (LvlLeafData leaf in Lvl.lvlleafs) {
                Form_LeafEditor leaftoplay = (Form_LeafEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == leaf.leafname), false, true);
                if (leaftoplay == null)
                    continue;
                Playback.CreatePlaybackFromLeaf(leaftoplay.leafProperties, leaftoplay.leafProperties.beats, beatoffset);
                beatoffset += leaf.beats;
                leaftoplay.Dispose();
            }
            //create midi events for the loop tracks
            Playback.MidiEventLoopTracks(Lvl, (_BeatOffset == 0 ? 0 : Lvl.approachbeats), _BeatOffset);
        }

        public static void CreatePlaybackFromGate(GateProperties Gate, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingGate.Text = $"Gate: {Gate.FilePath.Name}";
            TCLE.Instance.lblLoadingGate.Invalidate();
            TCLE.Instance.lblLoadingGate.Update();
            TCLE.Instance.lblLoadingGate.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            GlobalGateQueue.Add(new Tuple<string, int>(Gate.FilePath.Name, (_BeatOffset) * 100));
            Playback.CallOffset = 0;
            int beatoffset = _BeatOffset;
            //create playback of the pre lvl
            Form_LvlEditor lvlpre = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == Gate.prelvl), false, true);
            if (lvlpre != null) {
                Playback.CreatePlaybackFromLvl(lvlpre.lvlProperties, lvlpre.lvlProperties.beats, beatoffset);
                beatoffset += lvlpre.lvlProperties.beats;
                lvlpre.Dispose();
            }
            //create playback of the pre lvl
            Form_LvlEditor lvlpost = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == Gate.postlvl), false, true);
            if (lvlpost != null) {
                Playback.CreatePlaybackFromLvl(lvlpost.lvlProperties, lvlpost.lvlProperties.beats, beatoffset);
                beatoffset += lvlpost.lvlProperties.beats;
                lvlpost.Dispose();
            }
            //create playback for each lvl phase
            foreach (GateLvlData lvl in Gate.gatelvls) {
                Form_LvlEditor lvltoplay = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == lvl.lvlname), false, true);
                Playback.CreatePlaybackFromLvl(lvltoplay.lvlProperties, lvltoplay.lvlProperties.beats, beatoffset);
                beatoffset += lvl.beats;
                lvltoplay.Dispose();
            }
            //clear the gate name after loading it
            TCLE.Instance.lblLoadingGate.Text = $"Gate:";
            TCLE.Instance.lblLoadingGate.Invalidate();
            TCLE.Instance.lblLoadingGate.Update();
            TCLE.Instance.lblLoadingGate.Refresh();
            Application.DoEvents();
            //
        }

        public static void CreatePlaybackFromMaster(MasterProperties Master, int BeatStop = -1, int _BeatOffset = 0)
        {
            Generating = true;
            Playback.CallOffset = 0;
            int beatoffset = 0;
            //setup checkpoint lvl so we can call it later if needed
            Form_LvlEditor lvlcheckpoint = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == Master.checkpointlvl), false, true);
            //create playback of the intro lvl
            Form_LvlEditor lvlintro = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == Master.introlvl), false, true);
            if (lvlintro != null) {
                Playback.CreatePlaybackFromLvl(lvlintro.lvlProperties);
                beatoffset += lvlintro.lvlProperties.beats + (lvlintro.lvlProperties.approachbeats < 8 ? 8 : lvlintro.lvlProperties.approachbeats);
                lvlintro.Dispose();
            }
            //create playback for each lvl
            foreach (MasterLvlData lvl in Master.masterlvls) {
                //load rest lvl first
                Form_LvlEditor lvlrest = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == lvl.rest), false, true);
                if (lvlrest != null) {
                    Playback.CreatePlaybackFromLvl(lvlrest.lvlProperties, lvlrest.lvlProperties.beats, beatoffset);
                    if (beatoffset == 0)
                        beatoffset += (lvlrest.lvlProperties.approachbeats < 8 ? 8 : lvlrest.lvlProperties.approachbeats);
                    beatoffset += lvl.restlevelbeats;
                    lvlrest.Dispose();
                }
                //load main lvl
                if (lvl.type == "gate") {
                    Form_GateEditor gatetoplay = (Form_GateEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == lvl.name), false, true);
                    Playback.CreatePlaybackFromGate(gatetoplay.gateproperties, gatetoplay.gateproperties.beats, beatoffset);
                    beatoffset += gatetoplay.gateproperties.beats;
                    gatetoplay.Dispose();
                }
                else {
                    Form_LvlEditor lvltoplay = (Form_LvlEditor)TCLE.OpenFile(ProjectExplorer.Files.FirstOrDefault(x => x.Name == lvl.name), false, true);
                    Playback.CreatePlaybackFromLvl(lvltoplay.lvlProperties, lvltoplay.lvlProperties.beats, beatoffset);
                    if (beatoffset == 0)
                        beatoffset += (lvltoplay.lvlProperties.approachbeats < 8 ? 8 : lvltoplay.lvlProperties.approachbeats);
                    beatoffset += lvltoplay.lvlProperties.beats;
                    lvltoplay.Dispose();
                }
                //load checkpoint
                if (lvl.checkpoint && lvlcheckpoint != null) {
                    Playback.CreatePlaybackFromLvl(lvlcheckpoint.lvlProperties, lvlcheckpoint.lvlProperties.beats, beatoffset);
                    beatoffset += lvlcheckpoint.lvlProperties.beats;
                }
            }
        }

        /// Key and Channel are the same thing
        public static int Pitch = 8192;
        public static int CallOffset = 8;
        public static int BeatOffset = 0;
        public static void AddNoteToChannel(double beat, int key, int call, int callkey, bool mute = false)
        {
            //beats land on multiples of 100 ticks.
            //to handle offsetting calls, increase beats by 8.
            beat = (beat + CallOffset + BeatOffset) * 100;
            call *= 100;
            if (call > 0) {
                SequencerEvents[callkey].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)callkey, (byte)(mute ? 0 : (int)Properties.Settings.Default[$"VolKey{callkey}"])), callkey, (int)beat - call, 0));
            }

            if (key != -1) {
                SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)key, (byte)(mute ? 0 : (int)Properties.Settings.Default[$"VolKey{key}"])), key, (int)beat, 0));
                //bar collect also plays ring collect noise
                if (key == 20)
                    SequencerEvents[19].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)19, (byte)(mute ? 0 : (int)Properties.Settings.Default[$"VolKey{key}"])), 19, (int)beat, 0));
            }
        }

        public static void PitchShiftingBarsRings()
        {
            Pitch = 8192;
            int Missed = 0;
            List<BASS_MIDI_EVENT> EventsToAdd19 = new();
            List<BASS_MIDI_EVENT> EventsToAdd20 = new();
            //combine ring and bar hit events to get a single track of events, in choronological order
            List<BASS_MIDI_EVENT> ComboList = GlobalSequencerEvents[19];/*.Concat(SequencerEvents[20]).ToList();*/
            //concat turn and thump hits, as they contribute to keeping a combo going
            ComboList = ComboList.Concat(GlobalSequencerEvents[8]).ToList();
            ComboList = ComboList.Concat(GlobalSequencerEvents[13]).ToList();
            ComboList = ComboList.Concat(GlobalSequencerEvents[22]).ToList();
            ComboList.Sort((event1, event2) => event1.tick.CompareTo(event2.tick));

            for (int x = 1; x < ComboList.Count; x++) {
                if (ComboList[x].chan is 8 or 13 or 22)
                    continue;
                //test if the event behind current is within 500 ticks (5 beats) of the current event
                if (ComboList[x].tick - ComboList[x - 1].tick is < 400 and not 0) {
                    //if found, pitch up next sound.
                    //add the pitch events to the lists.
                    if (Pitch < 9824) {
                        Pitch += 136;
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
            GlobalSequencerEvents[19] = GlobalSequencerEvents[19].Concat(EventsToAdd19).ToList();
            GlobalSequencerEvents[19].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            GlobalSequencerEvents[20] = GlobalSequencerEvents[20].Concat(EventsToAdd20).ToList();
            GlobalSequencerEvents[20].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        public static void MidiEventsForTurns(Sequencer_Object Seq)
        {
            int IsTurning = 0;
            SeqDataPoint lastprocessed = null;
            for (int x = 0; x < LeafLastBeat; x++)
            {
                lastprocessed = Seq.data_points[x];
                //account for default value being +-15
                decimal valuetotest = Seq.data_points[x].value == null ? (decimal)Seq.defaultvalue : (decimal)Seq.data_points[x].value;
                if (valuetotest >= 15) {
                    if (IsTurning == -1) {
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 10);
                        IsTurning = 1;
                    }
                    else if (IsTurning == 1) {
                        IsTurning = 2;
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 11);
                    }
                    else if (IsTurning == 2) {
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 22, 0, 0, true);
                    }
                    else if (IsTurning == 0)
                        IsTurning = 1;
                }
                else if (valuetotest <= -15) {
                    if (IsTurning == 1) {
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 12);
                        IsTurning = -1;
                    }
                    else if (IsTurning == -1) {
                        IsTurning = -2;
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 11);
                    }
                    else if (IsTurning == -2) {
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 22, 0, 0, true);
                    }
                    else if (IsTurning == 0)
                        IsTurning = -1;
                }
                else {
                    if (IsTurning == -1)
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 10);
                    else if (IsTurning == 1)
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 13, 8, 12);
                    else if (IsTurning is 2 or -2)
                        AddNoteToChannel(Seq.data_points[x].beat - 1, 22, 0, 0, true);
                    IsTurning = 0;
                }
            }
            //handle last beat
            if (IsTurning == -1)
                AddNoteToChannel(lastprocessed.beat, 13, 8, 10);
            else if (IsTurning == 1)
                AddNoteToChannel(lastprocessed.beat, 13, 8, 12);
            else if (IsTurning is 2 or -2)
                AddNoteToChannel(lastprocessed.beat, 22, 0, 0, true);
        }

        public static void MidiEventsForLanes(Sequencer_Object Seq)
        {
            for (int x = 1; x < LeafLastBeat; x++)
            {
                decimal? value = (decimal?)Seq.data_points[x].value;
                if (Seq.defaultvalue == 0) {
                    if (value is 0 or null) {
                        if ((decimal?)Seq.data_points[x - 1].value == 1)
                            AddNoteToChannel(Seq.data_points[x].beat - 1, -1, 8, 21);
                    }
                }
                else if (Seq.defaultvalue == 1) {
                    if (value is 0) {
                        if ((decimal?)Seq.data_points[x - 1].value is 1 or null)
                            AddNoteToChannel(Seq.data_points[x].beat - 1, -1, 8, 21);
                    }
                }
            }
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

                foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.beat < LeafLastBeat && x.value != null)) {
                    //find events that fall inside the sentry activation time
                    foreach (BASS_MIDI_EVENT _event in SequencerEvents[8].Where(x => x.tick > ((sdp.beat + BeatOffset) *100)+400 && x.tick <= (sdp.beat + length + BeatOffset) *100)) {
                        //if the sentry call event doesn't exist yet, add it (so we don't duplicate on sounds)
                        if (!EventsToAdd15.Any(x => x.tick == _event.tick - 400)) {
                            //Sentry call happens 4 beats ahead (400 ticks)
                            EventsToAdd15.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)16, (byte)(int)Properties.Settings.Default[$"VolKey16"]), 16, _event.tick - 400, 0));
                        }
                    }
                    if (sdp.beat + length < LeafLastBeat) {
                        EventsToAdd16.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)15, (byte)(int)Properties.Settings.Default[$"VolKey15"]), 15, (sdp.beat + length + CallOffset + BeatOffset) * 100, 0));
                    }
                }
            }

            SequencerEvents[15] = EventsToAdd15;
            SequencerEvents[15].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            SequencerEvents[16] = EventsToAdd16;
            SequencerEvents[16].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        private static int SpeedPitch = 8192;
        public static void MidiEventsForSpeed(Sequencer_Object Seq)
        {
            return;
            if (Seq == null)
                return;

            foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.beat < LeafLastBeat && x.value != null)) {
                SequencerEvents[0].Add(new(BASSMIDIEvent.MIDI_EVENT_TEMPO, (int)(Microseconds / (double)(decimal)sdp.value), 0, (sdp.beat + CallOffset) * 100, 0));
                //log 2 speed value to get octaves, times 12 for semitones
                double semitones = Math.Log2((double)(decimal)sdp.value) * 12;
                //each semitone takes 136.5 units on the pitchwheel
                double pitchadjust = semitones * 136.5;
                foreach (List<BASS_MIDI_EVENT> listevents in GlobalSampleEvents) {
                    listevents.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, (int)(SpeedPitch + pitchadjust), SequencerEvents.Length + GlobalSampleEvents.IndexOf(listevents), ((sdp.beat + CallOffset) * 100) - 1, 0));
                }
                for (int x = 1; x < SequencerEvents.Length; x++) {
                    //skip pitch shifting thumps
                    if (x == 8)
                        continue;
                    SequencerEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, (int)(SpeedPitch + pitchadjust), x, ((sdp.beat + CallOffset) * 100) - 1, 0));
                }
                //SequencerEvents[0].Add(new(BASSMIDIEvent.MIDI_EVENT_SPEED, (int)(10_000 * (decimal)sdp.value), 0, (sdp.beat + CallOffset) * 100, 0));
            }
        }

        public static void MidiEventPlaySample(Sequencer_Object Seq)
        {
            if (!GlobalSamplesToPlay.Contains(Seq.obj_name)) {
                GlobalSamplesToPlay.Add(Seq.obj_name);
                GlobalSampleEvents.Add(new());
            }
            //get the sampledata to calculate the volume it should be played at
            SampleData SampToPlay = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == Seq.obj_name);
            int velocity = (int?)(SampToPlay?.volume * 100) ?? 100;
            velocity = (int)(velocity * (((float)(int)Properties.Settings.Default[$"VolKey99"]) / 100f));
            if (velocity > 127)
                velocity = 127;
            //write each data point as a sample event
            foreach (SeqDataPoint sdp in Seq.data_points.Where(x => x.beat < LeafLastBeat && x.value != null)) {
                GlobalSampleEvents[GlobalSamplesToPlay.IndexOf(Seq.obj_name)].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)(GlobalSamplesToPlay.IndexOf(Seq.obj_name) + 1), (byte)velocity), SequencerEvents.Length + GlobalSamplesToPlay.Count - 1, (sdp.beat + CallOffset + BeatOffset) * 100, 0));
            }
        }

        public static void MidiEventLoopTracks(LvlProperties Lvl, int offset = 0, int lvloffset = 0)
        {
            foreach (LvlLoop loop in Lvl.lvlloops) {
                //add new entry to the loops
                GlobalLoopTracks.Add(new(Lvl.FilePath.Name, loop.sample, loop.beats));
                GlobalLoopEvents.Add(new());
                //get sample data and its volume
                SampleData SampToPlay = TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == loop.sample);
                int velocity = (int?)(SampToPlay?.volume * 100) ?? 100;
                velocity = (int)(velocity * (((float)(int)Properties.Settings.Default[$"VolKey99"]) / 100f));
                if (velocity > 127)
                    velocity = 127;
                //
                if (200 + GlobalLoopEvents.Count() - 1 == 316)
                    ;
                for (decimal x = 0; x < Lvl.beats + Lvl.approachbeats; x += loop.beats) {
                    GlobalLoopEvents[^1].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)GlobalLoopTracks.Count, (byte)velocity), 50 + GlobalLoopEvents.Count() - 1, (int)((x + CallOffset - offset + lvloffset) * 100), 0));
                }
            }
        }

        public static void CreateSampleSoundfont()
        {
            //skip this if there are no samples
            //if (SamplesToPlay.Count == 0)
            //    return;

            string path = $@"{TCLE.AppLocation}\temp\";
            string _out = $"<control>\r\ndefault_path={path}\r\n\r\n<group>\r\n\r\n";
            string _outloops = $"<control>\r\ndefault_path={path}\r\n\r\n<group>\r\n\r\n";
            foreach (string sample in GlobalSamplesToPlay) {
                string FileName = TCLE.PCtoAudioFile(TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == sample));
                _out += $"<region> sample={Path.GetFileName(FileName)} key={GlobalSamplesToPlay.IndexOf(sample) + 1}\r\n";
            }

            foreach (Tuple<string, string, decimal> loop in GlobalLoopTracks) {
                string FileName = TCLE.PCtoAudioFile(TCLE.ProjectSamples.FirstOrDefault(x => x.obj_name == loop.Item2));
                _outloops += $"<region> sample={Path.GetFileName(FileName)} key={GlobalLoopTracks.IndexOf(loop) + 1}\r\n";
            }

            _out += "\r\n\r\n";
            _outloops += "\r\n\r\n";
            File.WriteAllText($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz", _out);
            File.WriteAllText($@"{TCLE.AppLocation}\temp\SamplesSoundfontLoops.sfz", _outloops);

            int SamplesSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz");
            int SamplesLoopsSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\SamplesSoundfontLoops.sfz");

            if (GlobalSamplesToPlay.Count > 0 && GlobalLoopTracks.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesSoundfontHandle, 1, 0), new BASS_MIDI_FONT(SamplesLoopsSoundfontHandle, 2, 0) };
            else if (GlobalSamplesToPlay.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesSoundfontHandle, 1, 0) };
            else if (GlobalLoopTracks.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesLoopsSoundfontHandle, 1, 0) };
        }

        public static void RemoveNegativeTickEvents()
        {
            for (int x = 0; x < GlobalSequencerEvents.Length; x++) {
                GlobalSequencerEvents[x] = GlobalSequencerEvents[x].Where(x => x.tick > -1).ToList();
            }
            for (int x = 0; x < GlobalSampleEvents.Count; x++) {
                GlobalSampleEvents[x] = GlobalSampleEvents[x].Where(x => x.tick > -1).ToList();
            }
            for (int x = 0; x < GlobalLoopEvents.Count; x++) {
                GlobalLoopEvents[x] = GlobalLoopEvents[x].Where(x => x.tick > -1).ToList();
            }
        }

        public static void ChannelEnd(int EndBeat)
        {
            //set instrument to use and tempo
            //These need to be at tick 0, on channel 0
            //SequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 0, 0, 0, 0));
            GlobalSequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 36, 0, 0, 0));
            GlobalSequencerEvents[0].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_TEMPO, (int)Microseconds, 0, 0, 0));
            //cap off each channel with an END event
            for (int x = 0; x < GlobalSequencerEvents.Length; x++) {
                GlobalSequencerEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, x, 2, 0));
                if (GlobalSequencerEvents[x].Count > 0) {
                    GlobalSequencerEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, x, (EndBeat + 1) * 100, 0));
                }
                //make sure all events are in proper tick order
                GlobalSequencerEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }

            //for sample play events, insert EVENT_PROGRAM at tick 0 so it uses the correct soundfont
            for (int x = 0; x < GlobalSampleEvents.Count; x++) {
                int channeloffset = GlobalSequencerEvents.Length + x;
                GlobalSampleEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 1, channeloffset, 0, 0));
                //add pitch range as first event to the sample channel
                GlobalSampleEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, channeloffset, 2, 0));
                if (GlobalSampleEvents[x].Count > 0) {
                    GlobalSampleEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, channeloffset, (EndBeat + 1) * 100, 0));
                }
                //make sure all events are in proper tick order
                GlobalSampleEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }

            //for ;v; loop events, insert EVENT_PROGRAM at tick 0 so it uses the correct soundfont
            for (int x = 0; x < GlobalLoopEvents.Count; x++) {
                int channeloffset = 50 + x;
                GlobalLoopEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, GlobalSampleEvents.Count() > 0 ? 2 : 1, channeloffset, 0, 0));
                //add pitch range as first event to the sample channel
                GlobalLoopEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, channeloffset, 2, 0));
                if (GlobalLoopEvents[x].Count > 0) {
                    GlobalLoopEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, channeloffset, (EndBeat + 1) * 100, 0));
                }
                //make sure all events are in proper tick order
                GlobalLoopEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }
        }

        public static int channelsync;
        public static void Play(double StartTime, int EndBeat, bool Loop = false, int _ApproachBeats = 0)
        {
            EndBeat += 8; //+ call offset
            RemoveNegativeTickEvents();
            PitchShiftingBarsRings();
            ChannelEnd(EndBeat);
            CreateSampleSoundfont();
            Generating = false;
            TCLE.Instance.panelLoadingMessage.Visible = false;
            //merge all channels to a single array of events
            List<BASS_MIDI_EVENT> _SequencerEvents = Playback.GlobalSequencerEvents.SelectMany(x => x).Distinct().ToList();
            List<BASS_MIDI_EVENT> _SampleEvents = Playback.GlobalSampleEvents.SelectMany(x => x).Distinct().ToList();
            List<BASS_MIDI_EVENT> _SampleLoopEvents = Playback.GlobalLoopEvents.SelectMany(x => x).Distinct().ToList();
            List<BASS_MIDI_EVENT> AllEvents = _SequencerEvents.Concat(_SampleEvents).Concat(_SampleLoopEvents).ToList();
            //the very last midi event needs to be EVENT_END
            AllEvents.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, ((EndBeat + 1) * 100), 0));
            //create the stream
            MidiStream = BassMidi.BASS_MIDI_StreamCreateEvents(AllEvents.ToArray(), 100, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            Error = Bass.BASS_ErrorGetCode();
            List<BASS_MIDI_EVENT> BadTick = AllEvents.Where(x => x.tick > (EndBeat + 1)*100).ToList();
            channelsync = Bass.BASS_ChannelSetSync(MidiStream, BASSSync.BASS_SYNC_END, 0, EndingProc, IntPtr.Zero);
            //setup channel for looping
            if (Loop) {
                Bass.BASS_ChannelFlags(MidiStream, BASSFlag.BASS_SAMPLE_LOOP, BASSFlag.BASS_SAMPLE_LOOP);
                IsLooping = true;
                if (StartTime != -1)
                    LoopingStartTime = (60 / (double)TCLE.BPM) * (StartTime + 9);
                else
                    LoopingStartTime = (60 / (double)TCLE.BPM) * (StartTime);
            }
            else {
                IsLooping = false;
            }
            Error = Bass.BASS_ErrorGetCode();
            //apply soundfonts
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts.ToArray(), MidiSoundFonts.Length);
            //set ending sync
            Bass.BASS_ChannelSetAttribute(MidiStream, BASSAttribute.BASS_ATTRIB_VOL, (int)Properties.Settings.Default.VolKey100 / 100f);
            //calculate where playback should start
            PlaybackBeat = -9;
            if (StartTime != -1) {
                PlaybackBeat = (int)StartTime;
                Bass.BASS_ChannelSetPosition(MidiStream, (60 / (double)TCLE.BPM) * (StartTime + CallOffset));
                Error = Bass.BASS_ErrorGetCode();
            }
            ApproachBeats = _ApproachBeats;
            //play the sequence
            if (Bass.BASS_ChannelPlay(MidiStream, PlaybackBeat < 0)) {
                SyncTimer = new(new TimerCallback(SyncTimer_Tick), null, 0, (int)((60 / TCLE.BPM) * (1000 / BeatSubdivisions)));
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
            if (IsLooping) {
                Bass.BASS_ChannelSetPosition(MidiStream, LoopingStartTime);
            }
            else {
                IsPlaying = false;
                SyncTimer.Dispose();
                //SyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _ = Bass.BASS_ChannelStop(channel);
                _ = Bass.BASS_ChannelFree(channel);
                //TCLE.alzheimer();
            }
        }

        public static void StopPlayback()
        {
            Playback.IsPlaying = false;
            Bass.BASS_ChannelStop(Playback.MidiStream);
            _ = Bass.BASS_ErrorGetCode();
            Playback.SyncTimer.Dispose();
            Playback.PlaybackTick = -1;
            //
            GlobalCurrentLeaf = "???";
            GlobalCurrentOffset = -1;
        }

        public static int BeatSubdivisions = 4;
        public static double PlaybackTick = -1;
        public static int PlaybackBeat = -1;
        public static double PlaybackSubBeat = -1;
        public static int ApproachBeats;
        private static void SyncTimer_Tick(object sender)
        {
            PlaybackTick = Bass.BASS_ChannelGetPosition(MidiStream, BASSMode.BASS_POS_MIDI_TICK);
            PlaybackBeat = (int)(PlaybackTick / 100d) - CallOffset;
            PlaybackSubBeat = (PlaybackTick % 100) / 100;

            while (GlobalLeafQueue.Count > 0 && PlaybackTick > GlobalLeafQueue[0].Item2) {
                GlobalCurrentOffset = GlobalLeafQueue[0].Item2;
                GlobalCurrentLeaf = GlobalLeafQueue[0].Item1;
                GlobalLeafQueue.RemoveAt(0);
            }
            while (GlobalLvlQueue.Count > 0 && PlaybackTick > GlobalLvlQueue[0].Item2) {
                GlobalCurrentOffsetLvl = GlobalLvlQueue[0].Item2 / 100;
                GlobalCurrentLvl = GlobalLvlQueue[0].Item1;
                GlobalLvlQueue.RemoveAt(0);
            }
            while (GlobalGateQueue.Count > 0 && PlaybackTick > GlobalGateQueue[0].Item2) {
                GlobalCurrentOffsetGate = GlobalGateQueue[0].Item2 / 100;
                GlobalCurrentGate = GlobalGateQueue[0].Item1;
                GlobalGateQueue.RemoveAt(0);
            }
        }
    }
}
