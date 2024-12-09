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
    public class GateLvlData
    {
        public string lvlname { get; set; }
        public string sentrytype { get; set; }
        public int bucket { get; set; }
    }

    public class BossData
    {
        public string boss_name { get; set; }
        public string boss_spn { get; set; }
        public string boss_ent { get; set; }
    }

    public class GateProperties
    {
        [Browsable(false)]
        public Form_GateEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public List<JObject> undoItems { get; set; }
        [Browsable(false)]
        public ObservableCollection<GateLvlData> gatelvls;
        [Browsable(false)]
        public GateLvlData sublevel { get; set; }

        public GateProperties(Form_GateEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            sublevel = new();
            undoItems = new();
            gatelvls = new();
            gatelvls.CollectionChanged += parent.gatelvls_CollectionChanged;
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => FilePath.FullName;
        [Browsable(false)]
        public FileInfo FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Boss")]
        [Description("The boss to fight.")]
        [TypeConverter(typeof(GateBossList))]
        public string boss { 
            get => Boss;
            set {
                if (random)
                    return;
                Boss = value;
                parent.RecalculateRuntime();
                if (Boss == "Level 9 - pyramid" && !parent.EditorLoading)
                    MessageBox.Show("Pyramid requires 5 phases to function. 4 for the fight, 1 for the death sequence. Otherwise the level will crash.", "Thumper Custom Level Editor");
            } }
        private string Boss;

        [CategoryAttribute("Options")]
        [DisplayName("Level Subtitle")]
        [Description("This changes how the gate appears as a sublevel in game, adding 'boss', omega, or infinity")]
        [TypeConverter(typeof(GateSectionType))]
        public string sectiontype { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Pre Lvl")]
        [Description("THis lvl will play when the gate starts, before the boss spawns in.")]
        [TypeConverter(typeof(LvlList))]
        public string prelvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Post Lvl")]
        [Description("This lvl plays after the boss spawns in, before phase0 starts.")]
        [TypeConverter(typeof(LvlList))]
        public string postlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Restart Lvl")]
        [Description("This lvl plays when you restart the boss.")]
        [TypeConverter(typeof(LvlList))]
        public string restartlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Random")]
        [Description("When TRUE, only Spirograph boss can be used. Each phase can hold up to 4 lvls in its 'bucket'. Then in game, every time a phase is repeated, it will use one of the lvls in its bucket randomly.")]
        public bool random
        {
            get => Random;
            set {
                if (value == true) {
                    boss = "Level 6 - spirograph";
                }
                Random = value;
                parent.RecalculateRuntime();
            }
        }
        private bool Random;

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int beats => parent.RecalculateRuntime();

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime => TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.dockProjectProperties.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sublevel Name")]
        public string sublevelname => sublevel.lvlname;

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sentry")]
        [TypeConverter(typeof(GateSentryList))]
        [Description("Does this sublevel use a sentry. The multilane option is wider than the single lane")]
        public string sentrytype { get => sublevel.sentrytype; set => sublevel.sentrytype = value; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Bucket")]
        [TypeConverter(typeof(GateBucket))]
        [Description("Which phase's bucket should this go in. If random FALSE, always use 1.")]
        public int bucket { get => sublevel.bucket + 1; set => sublevel.bucket = value - 1; }
    }

    public class GateBossList : StringConverter
    {
        private readonly List<string> bossdata = new() {
            "Level 1 - circle",
            "Level 1 - crakhed",
            "Level 2 - circle",
            "Level 2 - crakhed",
            "Level 3 - array",
            "Level 3 - crakhed",
            "Level 4 - triangle",
            "Level 4 - zillapede",
            "Level 4 - crakhed",
            "Level 5 - spiral",
            "Level 5 - crakhed",
            "Level 6 - spirograph",
            "Level 6 - crakhed",
            "Level 7 - tube",
            "Level 7 - crakhed",
            "Level 8 - starfish",
            "Level 8 - crakhed",
            "Level 9 - fractal",
            "Level 9 - crakhed",
            "Level 9 - pyramid"
        };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(bossdata);
        }
    }

    public class GateSentryList : StringConverter
    {
        private List<string> gatesentrynames = new() { "None", "Single Lane", "Multi Lane" };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(gatesentrynames);
        }
    }

    public class GateSectionType : StringConverter
    {
        private List<string> sectiontypes = new() { "None", "Boss", "Final Boss", "Infinity" };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(sectiontypes);
        }
    }

    public class GateBucket : Int32Converter
    {
        private List<int> sectiontypes = new() { 1, 2, 3, 4 };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(sectiontypes);
        }
    }
}
