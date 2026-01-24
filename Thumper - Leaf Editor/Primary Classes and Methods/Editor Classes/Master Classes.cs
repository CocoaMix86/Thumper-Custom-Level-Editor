using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class MasterLvlData
    {
        public MasterLvlData() { }

        [Browsable(false)]
        public string type { get; set; }
        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Name")]
        public string name2 => $"{Name}.{type}";
        [Browsable(false)]
        public string name
        {
            get => name2;
            set {
                int idx = value.LastIndexOf('.');
                Name = idx != -1 ? value[..idx] : value;
            }
        }
        [Browsable(false)]
        public string Name;

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool playplus { get; set; }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool checkpoint { get; set; }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool isolate { get; set; }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Rest Lvl")]
        [Description("The rest lvl will play before the sublevel.")]
        [TypeConverter(typeof(LvlList))]
        public string rest { get; set; }

        [Browsable(false)]
        public string gatesectiontype { get; set; }
        [Browsable(false)]
        public string checkpoint_leader { get; set; }
        [Browsable(false)]
        public int id { get; set; }

        [Browsable(false)]
        public int Beats
        {
            get => _beats;
            set { _beats = value; }
        }
        [Browsable(false)]
        private int _beats;
        [Browsable(false)]
        public string runtime
        {
            get {
                return TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
            }
        }
        [Browsable(false)]
        public int beatstart;
        [Browsable(false)]
        public int restlevelbeats = 0;
        [Browsable(false)]
        public int restlevelbeatstart;

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
        public ObservableCollection<MasterLvlData> masterlvls;

        public MasterProperties(Form_MasterEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            masterlvls = new();
            masterlvls.CollectionChanged += parent.masterlvls_CollectionChanged;
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => FilePath.FullName;
        private FileInfo FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        [TypeConverter(typeof(SkyboxList))]
        public string skybox { get; set; } = "skybox_cube";

        [CategoryAttribute("Options")]
        [DisplayName("Intro Lvl")]
        [Description("This lvl will play at the beginning of your level, and whenever you restart.")]
        [TypeConverter(typeof(LvlList))]
        public string introlvl { get; set; }
        [Browsable(false)]
        public int introlevelbeats = 0;

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        [TypeConverter(typeof(LvlList))]
        public string checkpointlvl { get; set; }
        [Browsable(false)]
        public int checkpointbeats;

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int Beats => introlevelbeats + masterlvls.Sum(x => x.Beats) + masterlvls.Sum(x => x.restlevelbeats) + (masterlvls.Count(x => x.checkpoint) * checkpointbeats);

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime { 
            get {
                //parent.RecalculateRuntime();
                return TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff");
            }
        }
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

    public class SkyboxList : StringConverter
    {
        List<string> skyboxes = new() { "<none>", "skybox_cube" };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            TCLE.ReloadLvlsInProject();
            return new StandardValuesCollection(skyboxes);
        }
    }
}
