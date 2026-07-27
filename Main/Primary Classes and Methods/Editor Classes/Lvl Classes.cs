using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.WebSockets;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Un4seen.Bass;

namespace Thumper_Custom_Level_Editor
{
    public class LvlLeafData : DataGridViewRow
    {
        public LvlLeafData()
        {
            this.Height = 20;
            this.Cells.Add(new DataGridViewImageCell());
            this.Cells.Add(new DataGridViewTextBoxCell());
            this.Cells.Add(new DataGridViewTextBoxCell());
            Cells[0].Value = Properties.Resources.editor_lvl;
        }

        public string LeafName { 
            get => _leafname;
            set {
                _leafname = value;
                Cells[1].Value = LeafName;
            } 
        }
        private string _leafname;
        //
        public int Beats { 
            get => _beats;
            set {
                _beats = value;
                if (_beats == -1) {
                    Runtime = "file not found";
                    this.DefaultCellStyle.BackColor = Color.Maroon;
                }
                else {
                    Runtime = $"{Beats} beats -- " + TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
                    this.DefaultCellStyle = null;
                }
                Cells[2].Value = Runtime;
            } 
        }
        private int _beats;
        [Browsable(false)]
        public string Runtime;
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

    public class LvlLoop
    {
        public string sample { get; set; }
        public decimal beats
        {
            get => Beats;
            set {
                if (value < 1)
                    value = 1;
                if (value > 99999)
                    value = 99999;
                Beats = value;
            }
        }
        private decimal Beats;
    }

    public class LvlProperties
    {
        public LvlProperties(EditorLvl Parent)
        {
            ParentEditor = Parent;
            SelectedLeaf = new();
            SequencerObjects = new();
            lvlleafs = new();
            lvlleafs.CollectionChanged += ParentEditor.lvlleaf_CollectionChanged;
            lvlloops = new();
            lvlloops.CollectionChanged += ParentEditor.lvlloop_CollectionChanged;
        }

        [Browsable(false)]
        public EditorLvl ParentEditor;
        [Browsable(false)]
        public ObservableCollection<LvlLeafData> lvlleafs;
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
        public ObservableCollection<LvlLoop> lvlloops { get; set; }
        [Browsable(false)]
        public LvlLeafData SelectedLeaf { get; set; }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => this.ParentEditor.WorkingFile.FullName;

        [CategoryAttribute("Options")]
        [DisplayName("Approach Beats")]
        [Description("How many beats ahead of this lvl starting do the loops start playing.")]
        public int approachbeats
        {
            get => ApproachBeats;
            set
            {
                if (value < 0)
                    value = 0;
                ApproachBeats = value;
            }
        }
        private int ApproachBeats;

        [CategoryAttribute("Options")]
        [DisplayName("Volume")]
        [Description("1.0 is default. Affects all loops.")]
        public decimal volume { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Allow Input")]
        [Description("Enable/disable player input")]
        public bool allowinput { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Tutorial Type")]
        [Description("Shows on-screen input hints for different objects as they approach.")]
        [TypeConverter(typeof(LvlTutorialType))]
        public string tutorialtype { get; set; }

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int beats => lvlleafs.Sum(x => x.Beats);

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime => TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");

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
