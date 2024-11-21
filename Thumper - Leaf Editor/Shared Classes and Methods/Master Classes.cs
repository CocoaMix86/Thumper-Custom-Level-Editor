using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Windows.Foundation.Collections;

namespace Thumper_Custom_Level_Editor
{
    public class MasterLvlData
    {
        public MasterLvlData() { }

        [Browsable(false)]
        public string type { get; set; }

        private string Name;
        [Browsable(false)]
        public string name
        {
            get { return Name; }
            set {
                int idx = value.LastIndexOf('.');
                Name = value[..idx];
            }
        }
        [CategoryAttribute("General")]
        [DisplayName("Sublevel Name")]
        [Description("")]
        public string noneditname { get { return Name; } }

        [Browsable(false)]
        public string lvlname { get { return $"{name}.lvl"; } }
        [Browsable(false)]
        public string gatename { get { return $"{name}.gate"; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool playplus { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool checkpoint { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool isolate { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Rest Lvl")]
        [Description("The rest lvl will play before the sublevel.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string rest { get; set; }

        [Browsable(false)]
        public string gatesectiontype { get; set; }

        [Browsable(false)]
        public string checkpoint_leader { get; set; }
        [Browsable(false)]
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

        public MasterProperties(Form_MasterEditor Parent, string path)
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
        public string filepath { get {return FilePath; } }
        private string FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        public string skybox { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Intro Lvl")]
        [Description("This lvl will play at the beginning of your level, and whenever you restart.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string introlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string checkpointlvl { get; set; }

        [CategoryAttribute("Runtime")]
        [DisplayName("Beats")]
        [Description("Total number of beats across all lvls and gates included in the master.")]
        public int beats { get { return TCLE.CalculateMasterRuntime(TCLE.WorkingFolder, parent); } }

        [CategoryAttribute("Runtime")]
        [DisplayName("Runtime")]
        [Description("Calculated based on Beats and the current BPM. (Beats/BPM)")]
        public string runtime { get { return TimeSpan.FromMilliseconds((int)TimeSpan.FromMinutes(beats / (double)TCLE.dockProjectProperties.BPM).TotalMilliseconds).ToString(@"hh\:mm\:ss\.fff"); } }

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
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string rest { get { return sublevel.rest; } set { sublevel.rest = value; } }
    }

    class LvlPicker : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext? context, System.IServiceProvider? provider, object? value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            string foo = value as string;
            if (svc != null && foo != null) {
                TCLE.ReloadLvlsInProject();
                using (FileListBox form = new(TCLE.lvlsinworkfolder)) {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(Cursor.Position.X - 100, Cursor.Position.Y - 50);
                    if (svc.ShowDialog(form) == DialogResult.OK) {
                        return form.Value; // update object
                    }
                }
            }
            return value; // can also replace the wrapper object here
        }
    }
}
