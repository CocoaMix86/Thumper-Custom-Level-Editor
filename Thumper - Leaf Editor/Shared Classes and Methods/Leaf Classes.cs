using ICSharpCode.TextEditor.Actions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class Object_Params
    {
        public string category { get; set; }
        public string obj_name { get; set; }
        public string param_displayname { get; set; }
        public string param_path { get; set; }
        public string trait_type { get; set; }
        public string step { get; set; }
        public string def { get; set; }
        public string footer { get; set; }
    }

    public class Sequencer_Object
    {
        public string obj_name { get; set; }
        public string param_path { get; set; }
        public string trait_type { get; set; }
        public dynamic data_points { get; set; }
        public string step { get; set; }
        public float _default { get; set; }
        public string footer { get; set; }
        public string default_interp { get; set; }

        public string friendly_type { get; set; }
        public string friendly_param { get; set; }
        public string highlight_color { get; set; }
        public float highlight_value { get; set; }

        public int id { get; set; }
        public bool mute { get; set; }

        public Sequencer_Object()
        {
        }

        public Sequencer_Object Clone()
        {
            return (Sequencer_Object)MemberwiseClone();
        }
    }

    public class LeafProperties
    {
        [Browsable(false)]
        public Form_LeafEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public List<JObject> undoItems { get; set; }
        [Browsable(false)]
        public ObservableCollection<Sequencer_Object> seq_objs;
        [Browsable(false)]
        public Sequencer_Object selectedobj { get; set; }

        public LeafProperties(Form_LeafEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            selectedobj = new();
            undoItems = new();
            seq_objs = new();
            //seq_objs.CollectionChanged += parent.seqobjs_CollectionChanged;
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => FilePath.FullName;
        [Browsable(false)]
        public FileInfo FilePath;

        [CategoryAttribute("Options")]
        [DisplayName("Leaf Length")]
        [Description("How many beats long this sequencer/leaf is.")]
        public int beats
        {
            get => Beats;
            set {
                if (value > 255)
                    value = 255;
                else if (value < 1)
                    value = 1;
                Beats = value;

            }
        }
        private int Beats;

        [CategoryAttribute("Options")]
        [DisplayName("Time Signature")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        public string timesignature { get; set; }
    }
}
