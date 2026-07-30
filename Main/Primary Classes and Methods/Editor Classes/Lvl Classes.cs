using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;

namespace Thumper_Custom_Level_Editor
{
    public class LvlLeafData : NotifyBase
    {
        public LvlLeafData()
        {
        }

        private string _leafname;
        public string Leaf { 
            get => _leafname;
            set => SetField(ref _leafname, value);
        }
        //
        private int _beats;
        public int Beats { 
            get => _beats;
            set {
                SetField(ref _beats, value);
                if (_beats == -1) {
                    Runtime = "file not found";
                    BackColor = Color.Maroon;
                }
                else {
                    Runtime = $"{Beats} beats -- " + TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    BackColor = Color.Green;
                }
            } 
        }
        //
        public string Runtime { get; set; } = "file not found";
        public Color BackColor { get; set; } = Color.Green;
        //
        public List<string> Paths { get; set; }
        public int id { get; set; }
        public int BeatStart;

        public LvlLeafData Clone()
        {
            LvlLeafData leaf = (LvlLeafData)MemberwiseClone();
            leaf.Paths = new List<string>(Paths);
            return leaf;
        }
    }

    public class LvlLoop : DataGridViewRow
    {
        public LvlLoop()
        {
            this.Cells.Add(new DataGridViewTextBoxCell());
            this.Cells.Add(new DataGridViewTextBoxCell());
        }

        public string SampleName
        {
            get => _samplename;
            set {
                _samplename = value;
                Cells[0].Value = _samplename;
            }
        }
        private string _samplename;
        public decimal Beats
        {
            get => _beats;
            set {
                if (value < 1)
                    value = 1;
                if (value > 99999)
                    value = 99999;
                _beats = value;
                Cells[1].Value = _beats;
            }
        }
        private decimal _beats;
    }

    public class LvlProperties
    {
        public LvlProperties(EditorLvl Parent)
        {
            ParentEditor = Parent;
            SelectedLeaf = new();
            SequencerObjects = new();
            Leafs = new();
            //Leafs.CollectionChanged += ParentEditor.lvlleaf_CollectionChanged;
            LvlLoops = new();
            LvlLoops.CollectionChanged += ParentEditor.lvlloop_CollectionChanged;
        }

        [Browsable(false)]
        public EditorLvl ParentEditor;
        [Browsable(false)]
        public BindingList<LvlLeafData> Leafs;
        [Browsable(false)]
        public dynamic seqJSON;
        [Browsable(false)]
        public List<Sequencer_Object> SequencerObjects {
            get => _seqobjs;
            set {
                _seqobjs = value;
                ParentEditor.SaveCheckAndWrite(true, "Sequencer saved", false);
            }
        }
        private List<Sequencer_Object> _seqobjs;
        [Browsable(false)]
        public ObservableCollection<LvlLoop> LvlLoops { get; set; }
        [Browsable(false)]
        public LvlLeafData SelectedLeaf { get; set; }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => this.ParentEditor.WorkingFile.FullName;

        [CategoryAttribute("Options")]
        [DisplayName("Approach Beats")]
        [Description("How many beats ahead of this lvl starting do the loops start playing.")]
        public int ApproachBeats
        {
            get => _approachBeats;
            set
            {
                if (value < 0)
                    value = 0;
                _approachBeats = value;
            }
        }
        private int _approachBeats;

        [CategoryAttribute("Options")]
        [DisplayName("Volume")]
        [Description("1.0 is default. Affects all loops.")]
        public decimal Volume { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Allow Input")]
        [Description("Enable/disable player input")]
        public bool AllowInput { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Tutorial Type")]
        [Description("Shows on-screen input hints for different objects as they approach.")]
        [TypeConverter(typeof(LvlTutorialType))]
        public string TutorialType { get; set; }

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int Beats => Leafs.Sum(x => x.Beats);

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string Runtime => TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");

    }

    public class LvlTutorialType : StringConverter
    {
        private List<string> tutorialtypes = new() { 
            "TUTORIAL_NONE",
            "TUTORIAL_THUMP",
            "TUTORIAL_THUMP_REMINDER",
            "TUTORIAL_TURN_RIGHT",
            "TUTORIAL_TURN_LEFT",
            "TUTORIAL_GRIND",
            "TUTORIAL_POWER_GRIND",
            "TUTORIAL_POUND",
            "TUTORIAL_POUND_REMINDER",
            "TUTORIAL_LANES",
            "TUTORIAL_JUMP" };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(tutorialtypes);
        }
    }
}
