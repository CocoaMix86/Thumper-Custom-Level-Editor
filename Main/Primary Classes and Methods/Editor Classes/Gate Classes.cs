using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public class GatePropertyGrid
    {
        public GateLvlData Parent;

        [CategoryAttribute("General")]
        [DisplayName("Phase Lvl Name")]
        public string LvlName => Parent.LvlName;

        [CategoryAttribute("General")]
        [DisplayName("Phase Number")]
        public int Phase => Parent.Phase;

        [CategoryAttribute("General")]
        [DisplayName("Beats")]
        public int Beats => Parent.Beats;
        [CategoryAttribute("General")]
        [DisplayName("Runtime")]
        public string Runtime => Parent.Runtime;

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sentry")]
        [Description("Does this sublevel use a sentry. The multilane option is wider than the single lane")]
        [TypeConverter(typeof(GateSentryList))]
        public string SentryType
        {
            get => Parent.SentryType;
            set => Parent.SentryType = value;
        }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Bucket #")]
        [Description("Which phase's bucket should this go in. If random FALSE, always use 1.")]
        [TypeConverter(typeof(GateBucket))]
        public int Bucket
        {
            get => Parent.Bucket;
            set => Parent.Bucket = value;
        }
    }

    public class GateLvlData : NotifyBase
    {
        public GateProperties Parent;
        public GatePropertyGrid PropertyGrid;
        public Image Icon = Properties.Resources.editor_gate;
        public GateLvlData(GateProperties parent)
        {
            Parent = parent;
            PropertyGrid = new() { Parent = this };
        }

        private string _lvlname;
        public string LvlName
        {
            get => _lvlname;
            set => SetField(ref _lvlname, value);
        }

        private string _sentrytype;
        public string SentryType { 
            get => _sentrytype; 
            set => SetField(ref _sentrytype, value); 
        }

        private int _bucket;
        public int Bucket { 
            get => _bucket; 
            set => SetField(ref _bucket, value); 
        }
        public int Phase => Parent.Random ? Bucket : Parent.GateLvls.IndexOf(this);

        public int Beats
        {
            get {
                if (!TCLE.CachedRuntimes.TryGetValue(LvlName, out int _run)) {
                    _run = UtilMath.CalculateLvlRuntime(ProjectExplorer.Files[LvlName]);
                }
                if (_beats != _run) {
                    _beats = _run;
                    if (_beats == -1) {
                        Runtime = "file not found";
                        RowColor = Color.Maroon;
                    }
                    else
                        Runtime = $"{this.Beats} beats -- {TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff")}";
                    RowColor = Color.Green;
                }
                return _run;
            }
        }
        private int _beats;
        
        private string _runtime;
        public string Runtime
        {
            get => _runtime;
            set => SetField(ref _runtime, value);
        }

        public Color RowColor = Color.Green;
        public int BeatStart { get; set; } = 0;

        private string _runtimemessage;
        public string RuntimeMessage
        {
            get => _runtimemessage;
            set => SetField(ref _runtimemessage, value);
        }

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
        public BindingList<GateLvlData> GateLvls;

        public GateProperties(EditorGate Parent)
        {
            ParentEditor = Parent;
            GateLvls = new();
            GateLvls.ListChanged += ((EditorGate)ParentEditor).GateLvls_ListChanges;
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
        public int prebeats
        {
            get {
                if (prelvl is null or "<none>")
                    return 0;
                if (ProjectExplorer.TryGetFile(prelvl, out ProjectItem _prelvl)) {
                    return _prelvl.Runtime;
                }
                return 0;
            }
        }

        [CategoryAttribute("Options")]
        [DisplayName("Post Lvl")]
        [Description("This lvl plays after the boss spawns in, before phase0 starts.")]
        [TypeConverter(typeof(LvlList))]
        public string postlvl { get; set; }
        [Browsable(false)]
        public int postbeats
        {
            get {
                if (postlvl is null or "<none>")
                    return 0;
                if (ProjectExplorer.TryGetFile(postlvl, out ProjectItem _postlvl)) {
                    return _postlvl.Runtime;
                }
                return 0;
            }
        }

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
