using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class LvlLeafData
    {
        public string leafname { get; set; }
        public int beats { get; set; }
        public List<string> paths { get; set; }
        public int id { get; set; }

        public LvlLeafData()
        {

        }

        public LvlLeafData Clone()
        {
            LvlLeafData leaf = (LvlLeafData)MemberwiseClone();
            leaf.paths = new List<string>(paths);
            return leaf;
        }
    }

    public class LvlLoop
    {
        public string sample { get; set; }
        public decimal beats { get; set; }
    }

    public class LvlProperties
    {
        [Browsable(false)]
        public Form_LvlEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public List<JObject> undoItems { get; set; }
        [Browsable(false)]
        public ObservableCollection<LvlLeafData> lvlleafs;
        [Browsable(false)]
        public LvlLeafData sublevel { get; set; }

        public LvlProperties(Form_LvlEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            sublevel = new();
            undoItems = new();
            lvlleafs = new();
            lvlleafs.CollectionChanged += parent.lvlleaf_CollectionChanged;
        }

        [Browsable(false)]
        public List<LvlLoop> lvlloops { get; set; }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => FilePath.FullName;
        [Browsable(false)]
        public FileInfo FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Approach Beats")]
        [Description("How many beats ahead of this lvl starting do the loops start playing.")]
        public int approachbeats { get; set; }

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
        public int beats => parent.RecalculateRuntime();

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime => TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.dockProjectProperties.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");

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
