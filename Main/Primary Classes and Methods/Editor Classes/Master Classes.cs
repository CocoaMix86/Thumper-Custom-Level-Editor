using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public class MasterPropertyGrid
    {
        public MasterLvlData Parent;
        
        [CategoryAttribute("General")]
        [DisplayName("Sublevel Name")]
        public string LvlName => Parent.WholeName;

        [CategoryAttribute("General")]
        [DisplayName("Sublevel Number")]
        public string Phase => Parent.SublevelNumber;

        [CategoryAttribute("General")]
        [DisplayName("Beats")]
        public int Beats => Parent.Beats;
        [CategoryAttribute("General")]
        [DisplayName("Runtime")]
        public string Runtime => Parent.Runtime;

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Name")]
        public string Name => $"{Parent.NameWithoutExt}.{Parent.Type}";

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool Playplus { 
            get => Parent.Playplus; 
            set => Parent.Playplus = value; 
        }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool Checkpoint { 
            get => Parent.Checkpoint;
            set => Parent.Checkpoint = value;
        }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool Isolate { 
            get => Parent.Isolate;
            set => Parent.Isolate = value;
        }

        [CategoryAttribute("Selected Sublevel(s)")]
        [DisplayName("Rest Lvl")]
        [Description("The rest lvl will play before the sublevel.")]
        [TypeConverter(typeof(LvlList))]
        public string Rest {
            get => Parent.RestLvl; 
            set => Parent.RestLvl = value;
        }
    }

    public class MasterLvlData : NotifyBase
    {
        public MasterProperties Parent;
        public MasterPropertyGrid PropertyGrid;
        public MasterLvlData(MasterProperties parent)
        {
            Parent = parent;
            PropertyGrid = new() { Parent = this };
        }

        public string NameSplitter
        {
            get => WholeName;
            set {
                int idx = value.LastIndexOf('.');
                NameWithoutExt = idx != -1 ? value[..idx] : value;
                Type = idx != -1 ? value[(idx + 1)..] : "";
            }
        }
        public string NameWithoutExt;
        public string Type { get; set; }
        public string WholeName => $"{NameWithoutExt}.{Type}";

        public bool Playplus { 
            get => _playplus; 
            set => SetField(ref _playplus, value); 
        }
        private bool _playplus;

        public bool Checkpoint { 
            get => _checkpoint; 
            set => SetField(ref _checkpoint, value); 
        }
        private bool _checkpoint;

        public bool Isolate { 
            get => _isolate; 
            set => SetField(ref _isolate, value); 
        }
        private bool _isolate;

        public string RestLvl { get; set; }
        public int restlevelbeats { 
            get {
                if (RestLvl is null or "<none>")
                    return 0;
                if (ProjectExplorer.TryGetFile(RestLvl, out ProjectItem _run)) {
                    return _run.Runtime;
                }
                return 0;
            }
        }
        public int restlevelbeatstart;

        public string gatesectiontype { get; set; }

        public string checkpoint_leader { get; set; }

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

        private int _beats = -1;
        public int Beats
        {
            get {
                int _b = -1;
                bool exists = false;
                if (ProjectExplorer.TryGetFile(WholeName, out ProjectItem _run)) {
                    _b = _run.Runtime;
                    exists = true;
                }
                if (_beats != _b) {
                    _beats = _b;
                    if (!exists) {
                        UpdateRuntime("File not found");
                        RowColor = Color.Maroon;
                    }
                    else if (exists && _beats <= 0) {
                        UpdateRuntime($"Item exists, but nothing in it");
                        RowColor = Color.Orange;
                    }
                    else {
                        UpdateRuntime(TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(_beats / (double)TCLE.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff"));
                        RowColor = Color.Green;
                    }
                }
                return _b + restlevelbeats;
            }
        }
        //
        private string _runtime = "File not found";
        public string Runtime
        {
            get {
                if (_beats <= 0)
                    return _runtime;
                return $"{Beats} beats -- {_runtime}";
            }
        }
        public void UpdateRuntime(string value)
        {
            _runtime = value;
            OnPropertyChanged(nameof(Runtime));
        }

        public Color RowColor = Color.Red;
        public int BeatStart;

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
        public int introlevelbeats
        {
            get {
                if (introlvl is null or "<none>")
                    return 0;
                if (ProjectExplorer.TryGetFile(introlvl, out ProjectItem _run)) {
                    return _run.Runtime;
                }
                return 0;
            }
        }

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        [TypeConverter(typeof(LvlList))]
        public string checkpointlvl { get; set; }
        [Browsable(false)]
        public int checkpointbeats
        {
            get {
                if (checkpointlvl is null or "<none>")
                    return 0;
                if (ProjectExplorer.TryGetFile(checkpointlvl, out ProjectItem _run)) {
                    return _run.Runtime;
                }
                return 0;
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
            return new StandardValuesCollection(TCLE.LvlsInProject);
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
