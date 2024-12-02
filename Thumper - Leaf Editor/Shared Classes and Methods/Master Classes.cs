using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Thumper_Custom_Level_Editor
{
    public class MasterLvlData
    {
        public MasterLvlData() { }

        public string type { get; set; }
        private string Name;
        public string name
        {
            get { return Name; }
            set {
                int idx = value.LastIndexOf('.');
                Name = idx != -1 ? value[..idx] : value;
            }
        }
        public string lvlname { get { return $"{name}.lvl"; } }
        public string gatename { get { return $"{name}.gate"; } }
        public bool playplus { get; set; }
        public bool checkpoint { get; set; }
        public bool isolate { get; set; }
        public string rest { get; set; }
        public string gatesectiontype { get; set; }
        public string checkpoint_leader { get; set; }
        public int id { get; set; }

        public MasterLvlData Clone()
        {
            return (MasterLvlData)MemberwiseClone();
        }
    }

    public class MasterProperties
    {
        [Browsable(false)]
        public Form_MasterEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public List<JObject> undoItems { get; set; }
        [Browsable(false)]
        public ObservableCollection<MasterLvlData> masterlvls;
        [Browsable(false)]
        public MasterLvlData sublevel { get; set; }

        public MasterProperties(Form_MasterEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            sublevel = new();
            undoItems = new();
            masterlvls = new();
            masterlvls.CollectionChanged += parent.masterlvls_CollectionChanged;
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath { get {return FilePath.FullName; } }
        private FileInfo FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        public string skybox { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Intro Lvl")]
        [Description("This lvl will play at the beginning of your level, and whenever you restart.")]
        [TypeConverter(typeof(LvlList))]
        public string introlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        [TypeConverter(typeof(LvlList))]
        public string checkpointlvl { get; set; }

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int beats { get { return TCLE.CalculateMasterRuntime(parent); } }

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime { 
            get {
                parent.RecalculateRuntime();
                return TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.dockProjectProperties.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
            }
        }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Sublevel Name")]
        public string sublevelname { get { return sublevel.name; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool playplus { get { return sublevel.playplus; } set { sublevel.playplus = value; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool checkpoint { get { return sublevel.checkpoint; } set { sublevel.checkpoint = value; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool isolate { get { return sublevel.isolate; } set { sublevel.isolate = value; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Rest Lvl")]
        [Description("The rest lvl will play before the sublevel.")]
        [TypeConverter(typeof(LvlList))]
        public string rest { get { return sublevel.rest; } set { sublevel.rest = value; } }
    }

    public class LvlList : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            TCLE.ReloadLvlsInProject();
            return new StandardValuesCollection(TCLE.lvlsinworkfolder);
        }
    }
}
