using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;
using Windows.ApplicationModel.Calls;

namespace Thumper_Custom_Level_Editor
{
    public class MasterLvlData : NotifyBase
    {
        public MasterLvlData(MasterProperties parent)
        {
            Parent = parent;
        }
        public MasterProperties Parent;

        [Browsable(false)]
        public string name
        {
            get => name2;
            set {
                int idx = value.LastIndexOf('.');
                Name = idx != -1 ? value[..idx] : value;
                Type = idx != -1 ? value[(idx + 1)..] : "";
            }
        }
        [Browsable(false)]
        public string Name;
        [Browsable(false)]
        public string Type { get; set; }
        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Name")]
        public string name2 => $"{Name}.{Type}";

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool Playplus { get => _playplus; set => SetField(ref _playplus, value); }
        private bool _playplus;

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool Checkpoint { get => _checkpoint; set => SetField(ref _checkpoint, value); }
        private bool _checkpoint;

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool Isolate { get => _isolate; set => SetField(ref _isolate, value); }
        private bool _isolate;

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

        private string _sublevelnum;
        public string SublevelNumber
        {
            get => _sublevelnum;
            set {
                string levelnum;
                int index = Parent.MasterLvls.IndexOf(this);
                int checkpoints = Parent.MasterLvls.Take(index).Count(x => x.Checkpoint);
                if (this.gatesectiontype is "SECTION_BOSS_CRAKHED" or "SECTION_BOSS_CRAKHED_FINAL")
                    levelnum = "Ω";
                else if (this.gatesectiontype is "SECTION_BOSS_PYRAMID")
                    levelnum = "∞";
                //check if previous sublevel had a checkpoint. If not, these are merged.
                else if (index != 0 && !Parent.MasterLvls[index - 1].Checkpoint)
                    levelnum = Parent.MasterLvls[index - 1].SublevelNumber;
                else
                    levelnum = $"{checkpoints + 1}";// $"{Parent.MasterLvls.IndexOf(this) + 1}";
                _sublevelnum = levelnum;
                //SetField(ref _sublevelnum, levelnum);
            }
        }

        [Browsable(false)]
        public int Beats
        {
            get => _beats;
            set {
                if (_beats != value) {
                    _beats = value;
                    if (_beats == -1) {
                        Runtime = "file not found";
                        RowColor = Color.Maroon;
                    }
                    else
                        Runtime = $"{this.Beats} beats -- {TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(Beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff")}";
                    RowColor = Color.Green;
                }
            }
        }
        private int _beats;
        public string Runtime { get => _runtime; set => SetField(ref _runtime, value); }
        private string _runtime;

        public Color RowColor = Color.Green;
        [Browsable(false)]
        public int BeatStart;
        [Browsable(false)]
        public int restlevelbeats = 0;
        [Browsable(false)]
        public int restlevelbeatstart;

        public MasterLvlData Clone()
        {
            return (MasterLvlData)MemberwiseClone();
        }
    }

    public class MasterProperties : EditorPropertiesBase
    {
        public MasterProperties(EditorMaster Parent)
        {
            ParentEditor = Parent;
            MasterLvls = new();
            MasterLvls.ListChanged += ((EditorMaster)ParentEditor).masterlvls_CollectionChanged;
        }

        [Browsable(false)]
        public BindingList<MasterLvlData> MasterLvls;

        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        [TypeConverter(typeof(SkyboxList))]
        public string Skybox { get; set; } = "skybox_cube";

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
