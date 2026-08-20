using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace Thumper_Custom_Level_Editor
{ 
    public class SpeedEvent
    {
        public BASSMIDIEvent EventType { get; set; }
        public int Value { get; set; }
        public int Tick { get; set; }
        public int Channel { get; set; }
        public string Interpolation { get; set; }
    }

    public static class Playback
    {
        public static bool IsPlaying {
            get => _isplay;
            set {
                _isplay = value;
                TCLE.MainBeeble.Dance(value);
                foreach (Beeble beeb in TCLE.ExistingBeebles)
                    beeb.Dance(value);
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
        public static Dictionary<int, int> VolumeKeys = new();

        public static void Initialize(string _Type)
        {
            TCLE.Instance.lblLoadingPlayback.Text = "Generating Playback";
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
            SpeedTempoEvents.Clear();
            //write soundfont to file if it doesn't exist
            if (!File.Exists($@"{TCLE.AppLocation}\temp\Thumper Sequencer.sf2")) {
                File.WriteAllBytes($@"{TCLE.AppLocation}\temp\Thumper Sequencer.zip", Properties.Resources.ThumperSequencerZip);
                ZipFile.ExtractToDirectory($@"{TCLE.AppLocation}\temp\Thumper Sequencer.zip", $@"{TCLE.AppLocation}\temp\");
            }
            //load soundfont
            MidiSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\Thumper Sequencer.sf2", BASSFlag.BASS_MIDI_FONT_MMAP);
            MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0)};
            //cache volumes
            VolumeKeys = new() {
                [1] = Properties.Settings.Default.VolKey1,
                [2] = Properties.Settings.Default.VolKey2,
                [3] = Properties.Settings.Default.VolKey3,
                [4] = Properties.Settings.Default.VolKey4,
                [5] = Properties.Settings.Default.VolKey5,
                [6] = Properties.Settings.Default.VolKey6,
                [7] = Properties.Settings.Default.VolKey7,
                [8] = Properties.Settings.Default.VolKey8,
                [9] = Properties.Settings.Default.VolKey9,
                [10] = Properties.Settings.Default.VolKey10,
                [11] = Properties.Settings.Default.VolKey11,
                [12] = Properties.Settings.Default.VolKey12,
                [13] = Properties.Settings.Default.VolKey13,
                [14] = Properties.Settings.Default.VolKey14,
                [15] = Properties.Settings.Default.VolKey15,
                [16] = Properties.Settings.Default.VolKey16,
                [17] = Properties.Settings.Default.VolKey17,
                [18] = Properties.Settings.Default.VolKey18,
                [19] = Properties.Settings.Default.VolKey19,
                [20] = Properties.Settings.Default.VolKey20,
                [21] = Properties.Settings.Default.VolKey21
            };
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
            TCLE.Instance.lblLoadingLeaf.Text = $"Leaf: {Leaf.ParentEditor.WorkingFile?.Name ?? "Lvl Sequencer"}";
            TCLE.Instance.lblLoadingLeaf.Invalidate();
            TCLE.Instance.lblLoadingLeaf.Update();
            TCLE.Instance.lblLoadingLeaf.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            BeatOffset = _BeatOffset;
            SequencerEvents = new List<BASS_MIDI_EVENT>[23];
            for (int x = 0; x < SequencerEvents.Length; x++) {
                SequencerEvents[x] = new(Leaf.LeafLength + CallOffset);
            }

            LeafLastBeat = Leaf.LeafLength;
            if (BeatStop >= 0) {
                //BeatStop += 1;
                LeafLastBeat = Math.Min(Leaf.LeafLength, BeatStop);
            }
            if (Leaf.ParentEditor.WorkingFile != null)
                GlobalLeafQueue.Add(new Tuple<string, int>(Leaf.ParentEditor.WorkingFile.Name, (BeatOffset) * 100));

            foreach (Sequencer_Object Seq in Leaf.SequencerObjects)
            {
                //don't playback disabled items
                if (Seq.EnabledInEditor == false || Seq.MuteInEditor)
                    continue;

                int Key = 0;
                int Call = 0;
                int CallKey = 0;
                if (Seq.ObjName.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase) || Seq.ObjName == "leafname")
                {
                    if (Seq.FriendlyParam == "turn") {
                        MidiEventsForTurns(Seq);
                    }
                    else if (Seq.FriendlyParam is "lane left 2" or "lane left 1" or "lane center" or "lane right 1" or "lane right 2") {
                        MidiEventsForLanes(Seq);
                    }
                }
                else if (Seq.ObjName == "avatar.lib" && Seq.FriendlyParam == "speed") {                    
                   // MidiEventsForSpeed(Seq);
                }
                else if (Seq.ObjName.EndsWith(".samp", StringComparison.OrdinalIgnoreCase)) {
                    MidiEventPlaySample(Seq);
                }
                else {
                    switch (Seq.ObjName) {
                        case "thump.spn":
                            Key = 8;
                            Call = 8;
                            CallKey = 18;
                            if (Seq.FriendlyParam == "thump[fast]")
                                Call = 4;
                            break;
                        case "grindable.spn":
                            Key = 19;
                            Call = 8;
                            CallKey = 1;
                            break;
                        case "grindable_multi.spn":
                            if (Seq.FriendlyParam == "thump and bar") {
                                Key = 8;
                                Call = 8;
                                CallKey = 18;
                            }
                            else {
                                Key = 19;
                                Call = 8;
                                CallKey = 1;
                            }
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
                    if (Seq.Default.TraitType is DefaultSequencerObject.Trait.Bool or DefaultSequencerObject.Trait.Action && Seq.DefaultValue is 1) {
                        for (int beat = EditorLeaf.FrozenColumnOffset; beat <= LeafLastBeat + EditorLeaf.FrozenColumnOffset; beat++) {
                            if (Seq[beat]?.Value == null || (Seq[beat].Value != null && (decimal)Seq[beat].Value != 0)) {
                                AddNoteToChannel(Seq[beat].beat, Key, Call, CallKey, Seq.MuteInEditor);
                                if (Seq.ObjName == "grindable_multi.spn") {
                                    if (Seq.FriendlyParam == "bar[double]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[triple]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.3333d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.6666d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[quad]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.25d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.50d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.75d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "thump and bar") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, 19, 8, 1, Seq.MuteInEditor);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        for (int beat = EditorLeaf.FrozenColumnOffset; beat <= LeafLastBeat + EditorLeaf.FrozenColumnOffset; beat++) {
                            if (Seq[beat]?.Value != null && (decimal?)Seq[beat].Value != 0) {
                                AddNoteToChannel(Seq[beat].beat, Key, Call, CallKey, Seq.MuteInEditor);
                                if (Seq.ObjName == "grindable_multi.spn") {
                                    if (Seq.FriendlyParam == "bar[double]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[triple]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.3333d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.6666d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[quad]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.25d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.50d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.75d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "thump and bar") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, 19, 8, 1, Seq.MuteInEditor);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            MidiEventsForSentry(Leaf);
            MidiEventsForSpeed(Leaf.SequencerObjects.FirstOrDefault(x => x.ObjName == "avatar.lib" && x.FriendlyParam == "speed"));
            //copy over single leaf results to global so it doesn't get cleared
            for (int x = 0; x < SequencerEvents.Length; x++) {
                if (SequencerEvents[x].Count > 0)
                    GlobalSequencerEvents[x] = GlobalSequencerEvents[x].Concat(SequencerEvents[x]).ToList();
            }
        }
        public static void CreatePlaybackFromLeaf(SimpleLeafProperties Leaf, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingLeaf.Text = $"Leaf: {Leaf.LeafName}";
            TCLE.Instance.lblLoadingLeaf.Invalidate();
            TCLE.Instance.lblLoadingLeaf.Update();
            TCLE.Instance.lblLoadingLeaf.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            BeatOffset = _BeatOffset;
            SequencerEvents = new List<BASS_MIDI_EVENT>[23];
            for (int x = 0; x < SequencerEvents.Length; x++) {
                SequencerEvents[x] = new(Leaf.LeafLength + CallOffset);
            }

            LeafLastBeat = Leaf.LeafLength;
            if (BeatStop >= 0) {
                //BeatStop += 1;
                LeafLastBeat = Math.Min(Leaf.LeafLength, BeatStop);
            }
            //if (Leaf.ParentEditor.WorkingFile != null)
                GlobalLeafQueue.Add(new Tuple<string, int>(Leaf.LeafName, (BeatOffset) * 100));

            foreach (SimpleSequencerObject Seq in Leaf.SequencerObjects) {
                //don't playback disabled items
                if (Seq.EnabledInEditor == false || Seq.MuteInEditor)
                    continue;

                int Key = 0;
                int Call = 0;
                int CallKey = 0;
                if (Seq.ObjName.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase) || Seq.ObjName == "leafname") {
                    if (Seq.FriendlyParam == "turn") {
                        MidiEventsForTurns(Seq);
                    }
                    else if (Seq.FriendlyParam is "lane left 2" or "lane left 1" or "lane center" or "lane right 1" or "lane right 2") {
                        MidiEventsForLanes(Seq);
                    }
                }
                else if (Seq.ObjName == "avatar.lib" && Seq.FriendlyParam == "speed") {
                    // MidiEventsForSpeed(Seq);
                }
                else if (Seq.ObjName.EndsWith(".samp", StringComparison.OrdinalIgnoreCase)) {
                    MidiEventPlaySample(Seq);
                }
                else {
                    switch (Seq.ObjName) {
                        case "thump.spn":
                            Key = 8;
                            Call = 8;
                            CallKey = 18;
                            if (Seq.FriendlyParam == "thump[fast]")
                                Call = 4;
                            break;
                        case "grindable.spn":
                            Key = 19;
                            Call = 8;
                            CallKey = 1;
                            break;
                        case "grindable_multi.spn":
                            if (Seq.FriendlyParam == "thump and bar") {
                                Key = 8;
                                Call = 8;
                                CallKey = 18;
                            }
                            else {
                                Key = 19;
                                Call = 8;
                                CallKey = 1;
                            }
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
                    if (Seq.Default.TraitType is DefaultSequencerObject.Trait.Bool or DefaultSequencerObject.Trait.Action && Seq.DefaultValue is 1) {
                        for (int beat = EditorLeaf.FrozenColumnOffset; beat <= LeafLastBeat + EditorLeaf.FrozenColumnOffset; beat++) {
                            if (Seq[beat]?.Value == null || (Seq[beat].Value != null && (decimal)Seq[beat].Value != 0)) {
                                AddNoteToChannel(Seq[beat].beat, Key, Call, CallKey, Seq.MuteInEditor);
                                if (Seq.ObjName == "grindable_multi.spn") {
                                    if (Seq.FriendlyParam == "bar[double]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[triple]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.3333d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.6666d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[quad]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.25d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.50d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.75d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "thump and bar") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, 19, 8, 1, Seq.MuteInEditor);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        for (int beat = EditorLeaf.FrozenColumnOffset; beat <= LeafLastBeat + EditorLeaf.FrozenColumnOffset; beat++) {
                            if (Seq[beat]?.Value != null && (decimal?)Seq[beat].Value != 0) {
                                AddNoteToChannel(Seq[beat].beat, Key, Call, CallKey, Seq.MuteInEditor);
                                if (Seq.ObjName == "grindable_multi.spn") {
                                    if (Seq.FriendlyParam == "bar[double]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[triple]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.3333d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.6666d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "bar[quad]") {
                                        AddNoteToChannel(Seq[beat].beat + 0.25d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.50d, Key, Call, CallKey, Seq.MuteInEditor);
                                        AddNoteToChannel(Seq[beat].beat + 0.75d, Key, Call, CallKey, Seq.MuteInEditor);
                                    }
                                    else if (Seq.FriendlyParam == "thump and bar") {
                                        AddNoteToChannel(Seq[beat].beat + 0.5d, 19, 8, 1, Seq.MuteInEditor);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            MidiEventsForSentry(Leaf);
            MidiEventsForSpeed(Leaf.SequencerObjects.FirstOrDefault(x => x.ObjName == "avatar.lib" && x.FriendlyParam == "speed"));
            //copy over single leaf results to global so it doesn't get cleared
            for (int x = 0; x < SequencerEvents.Length; x++) {
                if (SequencerEvents[x].Count > 0)
                    GlobalSequencerEvents[x] = GlobalSequencerEvents[x].Concat(SequencerEvents[x]).ToList();
            }
        }

        public static void CreatePlaybackFromLvl(LvlProperties Lvl, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingLvl.Text = $"Lvl: {Lvl.ParentEditor.WorkingFile.Name}";
            TCLE.Instance.lblLoadingLvl.Invalidate();
            TCLE.Instance.lblLoadingLvl.Update();
            TCLE.Instance.lblLoadingLvl.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            GlobalLvlQueue.Add(new Tuple<string, int>(Lvl.ParentEditor.WorkingFile.Name, (_BeatOffset) * 100));
            Playback.CallOffset = 0;
            int beatoffset = _BeatOffset;
            if (_BeatOffset == 0)
                beatoffset = Lvl.ApproachBeats < 8 ? 8 : Lvl.ApproachBeats;
            //create playback of the lvl sequencer
            EditorLeaf lvlseq = new(Lvl, null, true);
            Playback.CreatePlaybackFromLeaf(lvlseq.LeafProperties, lvlseq.LeafProperties.LeafLength + EditorLeaf.FrozenColumnOffset, beatoffset - Lvl.ApproachBeats);
            lvlseq.Dispose();
            //create playback for each leaf
            foreach (LvlLeafData leaf in Lvl.Leafs) {
                SimpleLeafProperties _leafload = UtilCreate.SimpleLeaf(UtilFile.LoadFileLock(ProjectExplorer.TryGetFile(leaf.Leaf, out FileInfo _file) ? _file : null), _file);
                Playback.CreatePlaybackFromLeaf(_leafload, _leafload.LeafLength + EditorLeaf.FrozenColumnOffset, beatoffset);
                /*
                EditorLeaf leaftoplay = (EditorLeaf)TCLE.OpenFile(ProjectExplorer.GetFile(leaf.Leaf), false, true);
                if (leaftoplay == null)
                    continue;
                Playback.CreatePlaybackFromLeaf(leaftoplay.LeafProperties, leaftoplay.LeafProperties.LeafLength + EditorLeaf.FrozenColumnOffset, beatoffset);
                
                beatoffset += leaf.Beats;
                leaftoplay.Dispose();
                */
                beatoffset += _leafload.LeafLength;
            }
            //create midi events for the loop tracks
            Playback.MidiEventLoopTracks(Lvl, (_BeatOffset == 0 ? 0 : Lvl.ApproachBeats), _BeatOffset);
        }

        public static void CreatePlaybackFromGate(GateProperties Gate, int BeatStop = -1, int _BeatOffset = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingGate.Text = $"Gate: {Gate.ParentEditor.WorkingFile.Name}";
            TCLE.Instance.lblLoadingGate.Invalidate();
            TCLE.Instance.lblLoadingGate.Update();
            TCLE.Instance.lblLoadingGate.Refresh();
            Application.DoEvents();
            //
            Generating = true;
            GlobalGateQueue.Add(new Tuple<string, int>(Gate.ParentEditor.WorkingFile.Name, (_BeatOffset) * 100));
            Playback.CallOffset = 0;
            int beatoffset = _BeatOffset;
            //create playback of the pre lvl
            EditorLvl lvlpre = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(Gate.prelvl), false, true);
            if (lvlpre != null) {
                Playback.CreatePlaybackFromLvl(lvlpre.LvlProperties, lvlpre.LvlProperties.Beats, beatoffset);
                beatoffset += lvlpre.LvlProperties.Beats;
                lvlpre.Dispose();
            }
            //create playback of the pre lvl
            EditorLvl lvlpost = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(Gate.postlvl), false, true);
            if (lvlpost != null) {
                Playback.CreatePlaybackFromLvl(lvlpost.LvlProperties, lvlpost.LvlProperties.Beats, beatoffset);
                beatoffset += lvlpost.LvlProperties.Beats;
                lvlpost.Dispose();
            }
            //create playback for each lvl phase
            foreach (GateLvlData lvl in Gate.GateLvls) {
                EditorLvl lvltoplay = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(lvl.Lvlname), false, true);
                Playback.CreatePlaybackFromLvl(lvltoplay.LvlProperties, lvltoplay.LvlProperties.Beats, beatoffset);
                beatoffset += lvl.Beats;
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
            EditorLvl lvlcheckpoint = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(Master.checkpointlvl), false, true);
            //create playback of the intro lvl
            EditorLvl lvlintro = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(Master.introlvl), false, true);
            if (lvlintro != null) {
                Playback.CreatePlaybackFromLvl(lvlintro.LvlProperties);
                beatoffset += lvlintro.LvlProperties.Beats + (lvlintro.LvlProperties.ApproachBeats < 8 ? 8 : lvlintro.LvlProperties.ApproachBeats);
                lvlintro.Dispose();
            }
            //create playback for each lvl
            foreach (MasterLvlData lvl in Master.MasterLvls) {
                //int index = Master.MasterLvls.IndexOf(lvl);
                //load rest lvl first
                EditorLvl lvlrest = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(lvl.rest), false, true);
                if (lvlrest != null) {
                    Playback.CreatePlaybackFromLvl(lvlrest.LvlProperties, lvlrest.LvlProperties.Beats, beatoffset);
                    if (beatoffset == 0)
                        beatoffset += (lvlrest.LvlProperties.ApproachBeats < 8 ? 8 : lvlrest.LvlProperties.ApproachBeats);
                    beatoffset += lvl.restlevelbeats;
                    lvlrest.Dispose();
                }
                //load main lvl
                if (lvl.Type == "gate") {
                    EditorGate gatetoplay = (EditorGate)TCLE.OpenFile(ProjectExplorer.GetFile(lvl.name), false, true);
                    Playback.CreatePlaybackFromGate(gatetoplay.GateProperties, gatetoplay.GateProperties.Beats, beatoffset);
                    beatoffset += gatetoplay.GateProperties.Beats;
                    gatetoplay.Dispose();
                }
                else {
                    EditorLvl lvltoplay = (EditorLvl)TCLE.OpenFile(ProjectExplorer.GetFile(lvl.name), false, true);
                    Playback.CreatePlaybackFromLvl(lvltoplay.LvlProperties, lvltoplay.LvlProperties.Beats, beatoffset);
                    if (beatoffset == 0)
                        beatoffset += (lvltoplay.LvlProperties.ApproachBeats < 8 ? 8 : lvltoplay.LvlProperties.ApproachBeats);
                    beatoffset += lvltoplay.LvlProperties.Beats;
                    lvltoplay.Dispose();
                }
                //load checkpoint
                if (lvl.Checkpoint && lvlcheckpoint != null) {
                    Playback.CreatePlaybackFromLvl(lvlcheckpoint.LvlProperties, lvlcheckpoint.LvlProperties.Beats, beatoffset);
                    beatoffset += lvlcheckpoint.LvlProperties.Beats;
                }
            }
        }

        /// Key and Channel are the same thing
        public static int CallOffset = 8;
        public static int BeatOffset = 0;
        public static void AddNoteToChannel(double beat, int key, int call, int callkey, bool mute = false)
        {
            //beats land on multiples of 100 ticks.
            //to handle offsetting calls, increase beats by 8.
            beat = (beat + CallOffset + BeatOffset) * 100;
            call *= 100;
            if (call > 0) {
                SequencerEvents[callkey].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)callkey, (byte)(mute ? 0 : VolumeKeys[callkey])), callkey, (int)beat - call, 0));
            }
            //key is -1 if there's no note to play
            if (key != -1) {
                SequencerEvents[key].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)key, (byte)(mute ? 0 : VolumeKeys[key])), key, (int)beat, 0));
                //bar collect key 20 also needs to play ring collect sound
                if (key == 20)
                    SequencerEvents[19].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)19, (byte)(mute ? 0 : VolumeKeys[key])), 19, (int)beat, 0));
            }
        }

        public static int Pitch = 8192;
        enum BeetleState { Normal, Grind, Fly, FlyGrind, LongFly, LongFlyGrind };
        public static void PitchShiftingBarsRings()
        {
            Pitch = 8192;
            List<BASS_MIDI_EVENT> EventsToAdd19 = new();
            List<BASS_MIDI_EVENT> EventsToAdd20 = new();
            //combine ring and bar hit events to get a single track of events, in choronological order
            List<BASS_MIDI_EVENT> ComboList = GlobalSequencerEvents[19];//.Concat(SequencerEvents[20]).ToList();
            //concat turn and thump hits, as they contribute to keeping a combo going
            ComboList = ComboList.Concat(GlobalSequencerEvents[8]).Concat(GlobalSequencerEvents[13]).Concat(GlobalSequencerEvents[20]).Concat(GlobalSequencerEvents[22]).ToList();
            ComboList.Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            //if no events to pitch shift for, skip method
            if (ComboList.Count == 0)
                return;
            //precompute all unique ring events to lookup later
            HashSet<int> ringTicks = GlobalSequencerEvents[20].Select(x => x.tick).ToHashSet();

            int TimerGrindState = 0;
            int TimerCombo = 0;
            bool resetPitch = false;
            HashSet<int> PitchShiftsToProcess = new();

            //8 = thump, 13 = turn, 19 = bar, 20 = ring, 22 = turn long
            int currentTickStart = 0;
            int eventIndex = 0;
            int lastTick = ComboList.Last().tick;

            while (currentTickStart <= lastTick) {
                //go over list in 100 tick increments to simulate progressing beats
                while (eventIndex < ComboList.Count && ComboList[eventIndex].tick < currentTickStart + 100) {
                    //thumps
                    //can initiate the grind state. And if combo is active, re-ups the timer
                    if (ComboList[eventIndex].chan is 8) {
                        TimerGrindState = 12;
                        if (TimerCombo > 0)
                            TimerCombo = 3;
                    }
                    //turns and long turns
                    //re-ups grindstate and combo timers
                    else if (ComboList[eventIndex].chan is 13 or 22) {
                        if (TimerGrindState > 0)
                            TimerGrindState = 12;
                        if (TimerCombo > 0)
                            TimerCombo = 3;
                    }
                    //find bar only (since rings also play bar sound)
                    //starts the combo timer if in grindstate, and re-ups grindstate
                    else if (ComboList[eventIndex].chan is 19 && !ringTicks.Contains(ComboList[eventIndex].tick)) {
                        if (TimerGrindState > 0) {
                            TimerGrindState = 12;
                            TimerCombo = 3;
                            PitchShiftsToProcess.Add(ComboList[eventIndex].tick);
                        }
                        else if (TimerCombo > 0) {
                            TimerCombo = 3;
                            PitchShiftsToProcess.Add(ComboList[eventIndex].tick);
                        }
                        //if not grinding or combo, play bad breaking sound. Requires pitch shifting the normal sound down an octave, and then reset to normal
                        else {
                            EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, 8192 - (136 * 12), 19, ComboList[eventIndex].tick - 1, 0));
                            EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, 8192, 19, ComboList[eventIndex].tick + 50, 0));
                        }
                    }
                    //rings
                    //stops grindstate and starts combo timer no matter what
                    else if (ComboList[eventIndex].chan is 20) {
                        TimerGrindState = 0;
                        TimerCombo = 3;
                        PitchShiftsToProcess.Add(ComboList[eventIndex].tick);
                    }

                    eventIndex++;
                }

                if (TimerGrindState > 0) 
                    TimerGrindState--;
                //if combo reaches 0, reset pitch
                if (TimerCombo > 0) 
                    TimerCombo--;
                else if (resetPitch) {
                    Pitch = 8192;
                    EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, currentTickStart - 1, 0));
                    EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, currentTickStart - 1, 0));
                    resetPitch = false;
                }

                foreach (int tick in PitchShiftsToProcess) {
                    resetPitch = true;
                    if (Pitch < 9824) {
                        Pitch += 136;
                        EventsToAdd19.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 19, tick - 1, 0));
                        EventsToAdd20.Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, Pitch, 20, tick - 1, 0));
                    }
                }

                PitchShiftsToProcess.Clear();
                currentTickStart += 100;
            }
            
            //concat new events into lists, then sort by tick to make them chronological
            GlobalSequencerEvents[19].AddRange(EventsToAdd19);
            GlobalSequencerEvents[19].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            GlobalSequencerEvents[20].AddRange(EventsToAdd20);
            GlobalSequencerEvents[20].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        public enum TurnState { None, Left, LongLeft, Right, LongRight };
        public static void MidiEventsForTurns(SimpleSequencerObject Seq)
        {
            //int _IsTurning = 0;
            TurnState _turnstate = TurnState.None;
            SimpleSeqDataPoint _lastprocessedbeat = null;
            for (int x = EditorLeaf.FrozenColumnOffset; x < LeafLastBeat + EditorLeaf.FrozenColumnOffset; x++)
            {
                _lastprocessedbeat = Seq[x];
                //account for default value being +-15
                decimal _turndegree = (decimal?)Seq[x].Value ?? Seq.DefaultValue;
                //current beat turning left
                if (_turndegree >= 15) {
                    //-1 = previous beat was turning the otherway. Play the turn left appear sound
                    if (_turnstate is TurnState.Right or TurnState.LongRight) {
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 10);
                        _turnstate = TurnState.Left;
                    }
                    //1 = turn is ongoing. Play the long turn sound
                    else if (_turnstate == TurnState.Left) {
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 11);
                        _turnstate = TurnState.LongLeft;
                    }
                    //2 = long turn still going. Add blank note
                    else if (_turnstate == TurnState.LongLeft) {
                        AddNoteToChannel(Seq[x].beat - 1, 22, 0, 0, true);
                    }
                    //0 = turn has not started. Update to 1 to trigger on next loop
                    else if (_turnstate == TurnState.None)
                        _turnstate = TurnState.Left;
                }
                //current beat turning right
                else if (_turndegree <= -15) {
                    //1 = previous beat was turning the otherway. Play the turn left appear sound
                    if (_turnstate is TurnState.Left or TurnState.LongLeft) {
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 12);
                        _turnstate = TurnState.Right;
                    }
                    else if (_turnstate == TurnState.Right) {
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 11);
                        _turnstate = TurnState.LongRight;
                    }
                    else if (_turnstate == TurnState.LongRight) {
                        AddNoteToChannel(Seq[x].beat - 1, 22, 0, 0, true);
                    }
                    else if (_turnstate == TurnState.None)
                        _turnstate = TurnState.Right;
                }
                //current beat not turning
                else {
                    //if last beat turned, play the turn right sound
                    if (_turnstate == TurnState.Left)
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 10);
                    //if last beat turned, play the turn left sound
                    else if (_turnstate == TurnState.Right)
                        AddNoteToChannel(Seq[x].beat - 1, 13, 8, 12);
                    //if last beat was long turning, do nothing
                    else if (_turnstate is TurnState.LongRight or TurnState.LongLeft)
                        AddNoteToChannel(Seq[x].beat - 1, 22, 0, 0, true);
                    //reset turning tracker
                    _turnstate = TurnState.None;
                }
            }
            //handle last beat
            if (_turnstate == TurnState.Left)
                AddNoteToChannel(_lastprocessedbeat.beat, 13, 8, 10);
            else if (_turnstate == TurnState.Right)
                AddNoteToChannel(_lastprocessedbeat.beat, 13, 8, 12);
            else if (_turnstate is TurnState.LongRight or TurnState.LongLeft)
                AddNoteToChannel(_lastprocessedbeat.beat, 22, 0, 0, true);
        }

        public static void MidiEventsForLanes(SimpleSequencerObject Seq)
        {
            //All Seqs that come through here will always be lanes. I don't have to test for other things.
            for (int x = 1; x < LeafLastBeat; x++)
            {
                //get the current beat value
                decimal? _laneState = (decimal?)Seq[x].Value;
                //switch logic depending if the default per beat is 0 or 1.
                //if lane's default is off...
                if (Seq.DefaultValue == 0) {
                    //if the current value is default (0) and the beat BEHIND was filled (1), play the lane end sound.
                    if (_laneState is 0 or null) {
                        if ((decimal?)Seq[x - 1].Value == 1)
                            AddNoteToChannel(Seq[x].beat - 1, -1, 8, 21);
                    }
                }
                //if lane's default is on...
                else if (Seq.DefaultValue == 1) {
                    //if the current value is 0 the beat BEHIND is default, play the lane end sound.
                    if (_laneState is 0) {
                        if ((decimal?)Seq[x - 1].Value is 1 or null)
                            AddNoteToChannel(Seq[x].beat - 1, -1, 8, 21);
                    }
                }
            }
        }

        public static void MidiEventsForSentry(SimpleLeafProperties Leaf)
        {
            List<BASS_MIDI_EVENT> EventsToAdd15 = new();
            List<BASS_MIDI_EVENT> EventsToAdd16 = new();
            foreach (SimpleSequencerObject Seq in Leaf.SequencerObjects.Where(x => x.ObjName == "sentry.spn"))
            {
                int length = Seq.Default.TrailLength;
                /*switch (Seq.FriendlyParam) {
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
                }*/
                //Get all datapoints for the sentry that are 1
                foreach (SimpleSeqDataPoint sdp in Seq.Cells.Cast<SimpleSeqDataPoint>().Where(x => x.beat < LeafLastBeat && x.InGameValue == 1)) {
                    //find thump events that fall inside the sentry activation time
                    foreach (BASS_MIDI_EVENT _event in SequencerEvents[8].Where(x => x.tick > ((sdp.beat + BeatOffset + CallOffset) *100) && x.tick <= (sdp.beat + length + BeatOffset + CallOffset) *100)) {
                        //if the sentry call event doesn't exist yet, add it (so we don't duplicate on sounds)
                        if (!EventsToAdd15.Any(x => x.tick == _event.tick - 400)) {
                            //Sentry call happens 4 beats ahead (400 ticks)
                            EventsToAdd15.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)16, (byte)VolumeKeys[16]), 16, _event.tick - 400, 0));
                        }
                    }
                    //if the sentry's end time is within the playback bounds, add the sentry exit sound
                    if (sdp.beat + length < LeafLastBeat) {
                        EventsToAdd16.Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)15, (byte)VolumeKeys[15]), 15, (sdp.beat + length + CallOffset + BeatOffset) * 100, 0));
                    }
                }
            }
            //sort all the added events, as sometimes the get added out of order with repeated calls to this function
            SequencerEvents[15] = EventsToAdd15;
            SequencerEvents[15].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            SequencerEvents[16] = EventsToAdd16;
            SequencerEvents[16].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
        }

        private static int SpeedPitch = 8192;
        private static List<SpeedEvent> SpeedTempoEvents = new();
        public static void MidiEventsForSpeed(SimpleSequencerObject Seq)
        {
            //skipping for now since the logic doesn't quite work
            //return;
            if (Seq == null)
                return;

            for (int x = EditorLeaf.FrozenColumnOffset; x < LeafLastBeat + EditorLeaf.FrozenColumnOffset; x++) {
                SimpleSeqDataPoint sdp = Seq[x];
                if (sdp.Value == null)
                    continue;
                SpeedTempoEvents.Add(new() {
                    EventType = BASSMIDIEvent.MIDI_EVENT_TEMPO,
                    //Tempo is calculated in Microseconds/beat, so smaller number = faster.
                    //casting to int because the Midi event param field takes only int
                    Value = (int)(Microseconds / (double)(decimal)sdp.Value),
                    Channel = 0,
                    Tick = (sdp.beat + CallOffset + BeatOffset) * 100,
                    Interpolation = $"{sdp.Interpolation} {sdp.Ease}"
                });
            }
        }

        public static void ApplySpeedEvents()
        {
            if (SpeedTempoEvents.Count == 0)
                return;
            List<BASS_MIDI_EVENT> TempoInterp = new();
            if (SpeedTempoEvents.Count > 1) {
                TempoInterp = InterpolateSpeedEvents(SpeedTempoEvents);
            }
            else {
                TempoInterp.Add(new(BASSMIDIEvent.MIDI_EVENT_TEMPO, SpeedTempoEvents[0].Value, 0, SpeedTempoEvents[0].Tick, 0));
            }
            //add all tempo events to channel 0.
            GlobalSequencerEvents[0].AddRange(TempoInterp);

            //now we apply all the pitch shifting events to match the tempo changes.
            //Importantly, pitch shifting increases the playback speed of samples         
            for (int x = 0; x < TempoInterp.Count; x++) {
                BASS_MIDI_EVENT _speedevent = TempoInterp[x];
                //we can divide Microseconds by the tempo param to get back the original value set in the sequencer
                double basevalue = Microseconds / _speedevent.param;
                //log 2 speed value to get octaves, times 12 for semitones
                double semitones = Math.Log2(basevalue) * 12;
                //each semitone takes 136.5 units on the pitchwheel
                double pitchadjust = semitones * 136.5;
                for (int i = 0; i < GlobalSampleEvents.Count; i++) {
                    GlobalSampleEvents[i].Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, SpeedPitch+(int)pitchadjust, SequencerEvents.Length + i, _speedevent.tick, 0));
                }
                for (int y = 1; y < SequencerEvents.Length; y++) {
                    //skip pitch shifting thumps
                    if (y == 8)
                        continue;
                    GlobalSequencerEvents[y].Add(new(BASSMIDIEvent.MIDI_EVENT_PITCH, SpeedPitch + (int)pitchadjust, y, _speedevent.tick, 0));
                }
            }            
        }

        public static List<BASS_MIDI_EVENT> InterpolateSpeedEvents(List<SpeedEvent> EventsToInterp)
        {
            List<BASS_MIDI_EVENT> AllInterps = new();
            for (int _eventind = 0; _eventind < EventsToInterp.Count - 1; _eventind++) {
                SpeedEvent _event = EventsToInterp[_eventind];
                //if interpolation is None or Step, or current and next value are the same, do not interpolate to the next value
                if (_event.Interpolation.Contains("Step") || _event.Interpolation.Contains("None") || _event.Value == EventsToInterp[_eventind + 1].Value) {
                    AllInterps.Add(new(_event.EventType, _event.Value, 0, _event.Tick, 0));
                    continue;
                }
                double _start = _event.Value;
                double _end = EventsToInterp[_eventind + 1].Value;
                double max = Math.Max(_start, _end);
                double min = Math.Min(_start, _end);
                int _beats = (EventsToInterp[_eventind + 1].Tick - _event.Tick) / 10;
                //initialize array = to beats, fill with linear values between 0 and 1
                //these will be transformed by the formulas below
                //double[] interp = new double[_beats];
                for (int x = 0; x < _beats; x++) {
                    double t = (double)x / (_beats - 1);
                    //interp[x] = (double)(x) / (double)(interp.Length - 1);
                    //depending on interp option chosen, run a different calculation per value in interp[]
                    switch (_event.Interpolation) {
                        case "Linear":
                            //no changes needed
                            break;
                        case "Quadratic Ease In":
                            t = t * t;
                            break;
                        case "Quadratic Ease Out":
                            t = 1 - (1 - t) * (1 - t);
                            break;
                        case "Quadratic Ease In Out":
                            t = t < 0.5 ? (2 * t * t) : (1 - (Math.Pow(-2 * t + 2, 2) / 2));
                            break;
                        case "Cubic Ease In":
                            t = t * t * t;
                            break;
                        case "Cubic Ease Out":
                            t = 1 - Math.Pow(1 - t, 3);
                            break;
                        case "Cubic Ease In Out":
                            t = t < 0.5 ? (4 * t * t * t) : (1 - (Math.Pow(-2 * t + 2, 3) / 2));
                            break;
                        case "Quartic Ease In":
                            t = t * t * t * t;
                            break;
                        case "Quartic Ease Out":
                            t = 1 - Math.Pow(1 - t, 4);
                            break;
                        case "Quartic Ease In Out":
                            t = t < 0.5 ? (8 * t * t * t * t) : (1 - (Math.Pow(-2 * t + 2, 4) / 2));
                            break;
                        case "Quintic Ease In":
                            t = t * t * t * t * t;
                            break;
                        case "Quintic Ease Out":
                            t = 1 - Math.Pow(1 - t, 5);
                            break;
                        case "Quintic Ease In Out":
                            t = t < 0.5 ? (16 * t * t * t * t) : (1 - (Math.Pow(-2 * t + 2, 5) / 2));
                            break;
                        case "Sine Ease In":
                            t = 1 - Math.Cos((t * Math.PI) / 2);
                            break;
                        case "Sine Ease Out":
                            t = Math.Sin((t * Math.PI) / 2);
                            break;
                        case "Sine Ease In Out":
                            t = -(Math.Cos(Math.PI * t) - 1) / 2;
                            break;
                    }

                    //if the first cell is actually the maximum, each value needs to be flipped across the range 0 to 1
                    if (_start == max)
                        t = 1 - t;
                    //convert interp[] range of 0 to 1 into range between selected beats
                    double _finalvalue = t * (max - min) + min;
                    AllInterps.Add(new(_event.EventType, (int)_finalvalue, 0, _event.Tick + (x * 10), 0));
                }
            }
            return AllInterps;
        }

        public static void MidiEventPlaySample(SimpleSequencerObject Seq)
        {
            if (!GlobalSamplesToPlay.Contains(Seq.ObjName)) {
                GlobalSamplesToPlay.Add(Seq.ObjName);
                GlobalSampleEvents.Add(new());
            }
            int _sampleIndex = GlobalSamplesToPlay.IndexOf(Seq.ObjName);
            //get the sampledata to calculate the volume it should be played at
            SampleData SampToPlay = TCLE.ProjectSamples[Seq.ObjName];
            //default to 100 if volume is somehow not set
            int velocity = (int?)(SampToPlay?.Volume * 100) ?? 100;
            //then further tune velocity using the master volume setting
            velocity = (int)(velocity * (((float)(int)Properties.Settings.Default[$"VolKey99"]) / 100f));
            //clamp
            velocity = Math.Clamp(velocity, 0, 127);
            //write each data point as a sample event
            foreach (SimpleSeqDataPoint sdp in Seq.Cells.Cast<SimpleSeqDataPoint>()) {
                if (sdp.Value == null || sdp.beat >= LeafLastBeat)
                    continue;
                GlobalSampleEvents[_sampleIndex].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)(_sampleIndex + 1), (byte)velocity), SequencerEvents.Length + GlobalSamplesToPlay.Count - 1, (sdp.beat + CallOffset + BeatOffset) * 100, 0));
            }
        }

        public static void MidiEventLoopTracks(LvlProperties Lvl, int offset = 0, int lvloffset = 0)
        {
            foreach (LvlLoop loop in Lvl.LvlLoops) {
                //skip loops with no lnegth, or less than 0.
                if (loop.Beats <= 0)
                    continue;
                //add new entry to the loops
                GlobalLoopTracks.Add(new(Lvl.ParentEditor.WorkingFile.Name, loop.SampleName, loop.Beats));
                GlobalLoopEvents.Add(new());
                //get sample data and its volume
                SampleData SampToPlay = TCLE.ProjectSamples[loop.SampleName];
                int velocity = (int?)(SampToPlay?.Volume * 100) ?? 100;
                velocity = (int)(velocity * (((float)(int)Properties.Settings.Default[$"VolKey99"]) / 100f));
                //clamp
                velocity = Math.Clamp(velocity, 0, 127);
                //
                for (decimal x = 0; x < Lvl.Beats + Lvl.ApproachBeats; x += loop.Beats) {
                    GlobalLoopEvents[^1].Add(new(BASSMIDIEvent.MIDI_EVENT_NOTE, (int)MakeWord((byte)GlobalLoopTracks.Count, (byte)velocity), 50 + GlobalLoopEvents.Count - 1, (int)((x + CallOffset - offset + lvloffset) * 100), 0));
                }
            }
        }

        public static void CreateSampleSoundfont()
        {
            string path = $@"{TCLE.AppLocation}\temp\";
            //initialize file beginnings
            string _out = $"<control>\r\ndefault_path={path}\r\n\r\n<group>\r\n\r\n";
            string _outloops = $"<control>\r\ndefault_path={path}\r\n\r\n<group>\r\n\r\n";
            //output every pc file to a playable audio file
            foreach (string sample in GlobalSamplesToPlay) {
                string FileName = UtilAudio.PCtoAudioFile(TCLE.ProjectSamples[sample]);
                _out += $"<region> sample={Path.GetFileName(FileName)} key={GlobalSamplesToPlay.IndexOf(sample) + 1}\r\n";
            }
            //same for loop tracks
            foreach (Tuple<string, string, decimal> loop in GlobalLoopTracks) {
                string FileName = UtilAudio.PCtoAudioFile(TCLE.ProjectSamples[loop.Item2]);
                _outloops += $"<region> sample={Path.GetFileName(FileName)} key={GlobalLoopTracks.IndexOf(loop) + 1}\r\n";
            }
            //close out files
            _out += "\r\n\r\n";
            _outloops += "\r\n\r\n";
            //write sound fonts to file
            File.WriteAllText($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz", _out);
            File.WriteAllText($@"{TCLE.AppLocation}\temp\SamplesSoundfontLoops.sfz", _outloops);
            //import sound fonts to BASS.Midi for playback
            int SamplesSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\SamplesSoundfont.sfz");
            int SamplesLoopsSoundfontHandle = BassMidi.BASS_MIDI_FontInit($@"{TCLE.AppLocation}\temp\SamplesSoundfontLoops.sfz");
            //depending on what sound fonts are loaded, the MidiSoundFonts array is loaded differently
            if (GlobalSamplesToPlay.Count > 0 && GlobalLoopTracks.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesSoundfontHandle, 1, 0), new BASS_MIDI_FONT(SamplesLoopsSoundfontHandle, 2, 0) };
            else if (GlobalSamplesToPlay.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesSoundfontHandle, 1, 0) };
            else if (GlobalLoopTracks.Count > 0)
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0), new BASS_MIDI_FONT(SamplesLoopsSoundfontHandle, 1, 0) };
            else
                MidiSoundFonts = new[] { new BASS_MIDI_FONT(MidiSoundfontHandle, 0, 0) };
        }

        public static void RemoveNegativeTickEvents()
        {
            for (int x = 0; x < GlobalSequencerEvents.Length; x++) {
                GlobalSequencerEvents[x].RemoveAll(x => x.tick < 0);
            }
            for (int x = 0; x < GlobalSampleEvents.Count; x++) {
                GlobalSampleEvents[x].RemoveAll(x => x.tick < 0);
            }
            for (int x = 0; x < GlobalLoopEvents.Count; x++) {
                GlobalLoopEvents[x].RemoveAll(x => x.tick < 0);
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
                //add pitch range as first event to the sample channelw
                GlobalSequencerEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, x, 2, 0));
                GlobalSequencerEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, x, (EndBeat * 100) + 1, 0));
                //make sure all events are in proper tick order
                GlobalSequencerEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }

            //for sample play events, insert EVENT_PROGRAM at tick 0 so it uses the correct soundfont
            for (int x = 0; x < GlobalSampleEvents.Count; x++) {
                int channeloffset = GlobalSequencerEvents.Length + x;
                GlobalSampleEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, 1, channeloffset, 0, 0));
                //add pitch range as first event to the sample channel
                GlobalSampleEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, channeloffset, 2, 0));
                GlobalSampleEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, channeloffset, (EndBeat * 100) + 1, 0));
                //make sure all events are in proper tick order
                GlobalSampleEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }

            //for ;v; loop events, insert EVENT_PROGRAM at tick 0 so it uses the correct soundfont
            for (int x = 0; x < GlobalLoopEvents.Count; x++) {
                int channeloffset = 50 + x;
                GlobalLoopEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PROGRAM, GlobalSampleEvents.Count > 0 ? 2 : 1, channeloffset, 0, 0));
                //add pitch range as first event to the sample channel
                GlobalLoopEvents[x].Insert(0, new(BASSMIDIEvent.MIDI_EVENT_PITCHRANGE, 60, channeloffset, 2, 0));
                GlobalLoopEvents[x].Add(new(BASSMIDIEvent.MIDI_EVENT_END_TRACK, 0, channeloffset, (EndBeat * 100) + 1, 0));
                //make sure all events are in proper tick order
                GlobalLoopEvents[x].Sort((event1, event2) => event1.tick.CompareTo(event2.tick));
            }
        }

        public static List<BASS_MIDI_EVENT> MergeAllEvents()
        {
            //merge all channels to a single array of events
            List<BASS_MIDI_EVENT> _SequencerEvents = Playback.GlobalSequencerEvents.SelectMany(x => x).Distinct().ToList();
            List<BASS_MIDI_EVENT> _SampleEvents = Playback.GlobalSampleEvents.SelectMany(x => x).Distinct().ToList();
            List<BASS_MIDI_EVENT> _SampleLoopEvents = Playback.GlobalLoopEvents.SelectMany(x => x).Distinct().ToList();
            return _SequencerEvents.Concat(_SampleEvents)
                .Concat(_SampleLoopEvents)
                .OrderBy(e => e.tick)
                .ToList();
        }

        public static void SetupPlaybackStream(List<BASS_MIDI_EVENT> AllEvents, double StartTime, bool Loop)
        {
            //create the stream
            MidiStream = BassMidi.BASS_MIDI_StreamCreateEvents(AllEvents.ToArray(), 100, BASSFlag.BASS_SAMPLE_FLOAT, 0);
            ///List<BASS_MIDI_EVENT> BadTick = AllEvents.Where(x => x.tick > (EndBeat + 1)*100).ToList();
            //set ending sync
            channelsync = Bass.BASS_ChannelSetSync(MidiStream, BASSSync.BASS_SYNC_END, 0, EndingProc, IntPtr.Zero);
            //setup channel for looping
            if (Loop) {
                Bass.BASS_ChannelFlags(MidiStream, BASSFlag.BASS_SAMPLE_LOOP, BASSFlag.BASS_SAMPLE_LOOP);
                IsLooping = true;
                //adding +6 to skip call sounds before the first beat
                if (StartTime > -1)
                    LoopingStartTime = (60 / (double)TCLE.BPM) * (StartTime + 6);
                else
                    LoopingStartTime = (60 / (double)TCLE.BPM) * (StartTime);
            }
            else {
                IsLooping = false;
            }
            //apply soundfonts
            BassMidi.BASS_MIDI_StreamSetFonts(MidiStream, MidiSoundFonts.ToArray(), MidiSoundFonts.Length);
            //apply master volume level to playback
            Bass.BASS_ChannelSetAttribute(MidiStream, BASSAttribute.BASS_ATTRIB_VOL, (int)Properties.Settings.Default.VolKey100 / 100f);
        }

        public static int channelsync;
        public static void Play(double StartTime, int EndBeat, bool Loop = false, int _ApproachBeats = 0)
        {
            //show the loading message
            TCLE.Instance.lblLoadingLeaf.Text = $"";
            TCLE.Instance.lblLoadingLvl.Text = $"Finalizing";
            TCLE.Instance.lblLoadingGate.Text = $"";
            TCLE.Instance.panelLoadingMessage.Invalidate();
            TCLE.Instance.panelLoadingMessage.Update();
            TCLE.Instance.panelLoadingMessage.Refresh();
            Application.DoEvents();
            //
            EndBeat += 7; //+ call offset
            PitchShiftingBarsRings();
            ApplySpeedEvents();
            RemoveNegativeTickEvents();
            ChannelEnd(EndBeat);
            CreateSampleSoundfont();

            List<BASS_MIDI_EVENT> AllEvents = MergeAllEvents();
            //the very last midi event needs to be EVENT_END
            AllEvents.Add(new(BASSMIDIEvent.MIDI_EVENT_END, 0, 0, (EndBeat * 100) + 1, 0));

            SetupPlaybackStream(AllEvents, StartTime, Loop);
            //at this point we can clear the generating flag and hide the loading screen
            Generating = false;
            TCLE.Instance.panelLoadingMessage.Visible = false;            
            //calculate where playback should start
            PlaybackBeat = -9;
            if (StartTime > -1) {
                PlaybackBeat = (int)StartTime - 3;
                Bass.BASS_ChannelSetPosition(MidiStream, (60 / (double)TCLE.BPM) * (PlaybackBeat + CallOffset));
            }
            ApproachBeats = _ApproachBeats;
            //play the sequence
            if (Bass.BASS_ChannelPlay(MidiStream, PlaybackBeat < 0)) {
                //SyncTimer = new(new TimerCallback(SyncTimer_Tick), null, 0, (int)((60 / TCLE.BPM) * (1000 / BeatSubdivisions)));
                SyncTimer?.Dispose();
                SyncTimer = new(new TimerCallback(SyncTimer_Tick), null, 0, 20);
                IsPlaying = true;
            }
            else {
                ///Error = Bass.BASS_ErrorGetCode();
            }
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
                SyncTimer?.Dispose();
                //SyncTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _ = Bass.BASS_ChannelStop(channel);
                _ = Bass.BASS_ChannelFree(channel);
                //TCLE.alzheimer();
            }
        }

        public static void StopPlayback()
        {
            Playback.IsPlaying = false;
            _ = Bass.BASS_ChannelStop(Playback.MidiStream);
            _ = Bass.BASS_ChannelFree(Playback.MidiStream);
            _ = Bass.BASS_ErrorGetCode();
            Playback.SyncTimer?.Dispose();
            Playback.PlaybackTick = -1;
            //
            GlobalCurrentLeaf = "???";
            GlobalCurrentLvl = "???";
            GlobalCurrentGate = "???";
            GlobalCurrentOffset = -1;
            GlobalCurrentOffsetLvl = -1;
            GlobalCurrentOffsetGate = -1;
            //
            TCLE.Instance.lblRuntime.Text = "00h 00m 00s 000ms - beat 0";
        }

        public static int BeatSubdivisions = 4;
        public static long PlaybackTimeBytes;
        public static double PlaybackTimeSec;
        public static double PlaybackTick = -1;
        public static int PlaybackBeat = -1;
        public static double PlaybackSubBeat = -1;
        public static int ApproachBeats;
        private static void SyncTimer_Tick(object sender)
        {
            PlaybackTimeBytes = Bass.BASS_ChannelGetPosition(MidiStream, BASSMode.BASS_POS_BYTE);
            PlaybackTimeSec = Bass.BASS_ChannelBytes2Seconds(MidiStream, PlaybackTimeBytes);
            //
            PlaybackTick = Bass.BASS_ChannelGetPosition(MidiStream, BASSMode.BASS_POS_MIDI_TICK);
            PlaybackBeat = (int)(PlaybackTick / 100d) - CallOffset;
            PlaybackSubBeat = (PlaybackTick % 100) / 100.0d;
            //
            TCLE.Instance.lblRuntime.Text = $"{TimeSpan.FromSeconds(PlaybackTimeSec).ToString("hh'h 'mm'm 'ss's 'fff'ms'")} - beat {PlaybackBeat}";

            while (GlobalLeafQueue.Count > 0 && PlaybackTick > GlobalLeafQueue[0].Item2) {
                GlobalCurrentOffset = GlobalLeafQueue[0].Item2 / 100;
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
