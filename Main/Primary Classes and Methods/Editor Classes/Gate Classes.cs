using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;
using Un4seen.Bass;

namespace Thumper_Custom_Level_Editor
{
    public class GateLvlData
    {
        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sublevel Name")]
        public string Lvlname { get => _lvlname; }
        [Browsable(false)]
        public string _lvlname;

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sentry")]
        [Description("Does this sublevel use a sentry. The multilane option is wider than the single lane")]
        [TypeConverter(typeof(GateSentryList))]
        public string SentryType { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Bucket #")]
        [Description("Which phase's bucket should this go in. If random FALSE, always use 1.")]
        [TypeConverter(typeof(GateBucket))]
        public int Bucket { get; set; }

        [Browsable(false)]
        public int Beats { get; set; } = 0;
        [Browsable(false)]
        public int BeatStart { get; set; } = 0;
        [Browsable(false)]
        public string Runtime => TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");

        public GateLvlData Clone()
        {
            return (GateLvlData)MemberwiseClone();
        }
    }

    public class BossData
    {
        public string boss_name { get; set; }
        public string boss_spn { get; set; }
        public string boss_ent { get; set; }
    }

    public class GateProperties : EditorPropertiesBase
    {
        [Browsable(false)]
        public ObservableCollection<GateLvlData> GateLvls;

        public GateProperties(EditorGate Parent)
        {
            ParentEditor = Parent;
            GateLvls = new();
            GateLvls.CollectionChanged += ((EditorGate)ParentEditor).gatelvls_CollectionChanged;
        }

        [CategoryAttribute("Options")]
        [DisplayName("Boss")]
        [Description("The boss to fight.")]
        [TypeConverter(typeof(GateBossList))]
        public string Boss { 
            get => _boss;
            set {
                if (Random)
                    return;
                _boss = value;
                ((EditorGate)ParentEditor).RecalculateRuntime();
                if (_boss == "Level 9 - pyramid" && !ParentEditor.EditorIsLoading)
                    MessageBox.Show("Pyramid requires 5 phases to function. 4 for the fight, 1 for the death sequence. Otherwise the level will crash.", "Thumper Custom Level Editor");
            } }
        private string _boss;

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
        [Browsable(false)]
        public int prebeats = 0;

        [CategoryAttribute("Options")]
        [DisplayName("Post Lvl")]
        [Description("This lvl plays after the boss spawns in, before phase0 starts.")]
        [TypeConverter(typeof(LvlList))]
        public string postlvl { get; set; }
        [Browsable(false)]
        public int postbeats = 0;

        [CategoryAttribute("Options")]
        [DisplayName("Restart Lvl")]
        [Description("This lvl plays when you restart the boss.")]
        [TypeConverter(typeof(LvlList))]
        public string restartlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Random")]
        [Description("When TRUE, only Spirograph boss can be used. Each phase can hold up to 4 lvls in its 'bucket'. Then in game, every time a phase is repeated, it will use one of the lvls in its bucket randomly.")]
        public bool Random
        {
            get => _random;
            set {
                _random = value;
                if (_random == true) {
                    Boss = "Level 6 - spirograph";
                }
                ((EditorGate)ParentEditor).RecalculateRuntime();
            }
        }
        private bool _random;
        [Browsable(false)]
        public int MaximumLvls;
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
