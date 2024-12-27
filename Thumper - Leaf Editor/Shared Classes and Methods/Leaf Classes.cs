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
                if (!parent.EditorIsLoading)
                    parent.LeafLengthChanged();
            }
        }
        private int Beats;

        [CategoryAttribute("Options")]
        [DisplayName("Time Signature")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        [TypeConverter(typeof(LeafTimeSignatures))]
        public string timesignature { get; set; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 0")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue0 { get => TCLE.LeafQuickValue0; set => TCLE.LeafQuickValue0 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 1")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue1 { get => TCLE.LeafQuickValue1; set => TCLE.LeafQuickValue1 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 2")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue2 { get => TCLE.LeafQuickValue2; set => TCLE.LeafQuickValue2 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 3")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue3 { get => TCLE.LeafQuickValue3; set => TCLE.LeafQuickValue3 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 4")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue4 { get => TCLE.LeafQuickValue4; set => TCLE.LeafQuickValue4 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 5")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue5 { get => TCLE.LeafQuickValue5; set => TCLE.LeafQuickValue5 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 6")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue6 { get => TCLE.LeafQuickValue6; set => TCLE.LeafQuickValue6 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 7")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue7 { get => TCLE.LeafQuickValue7; set => TCLE.LeafQuickValue7 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 8")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue8 { get => TCLE.LeafQuickValue8; set => TCLE.LeafQuickValue8 = value; }

        [CategoryAttribute("Quick Values")]
        [DisplayName("Quick 9")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue9 { get => TCLE.LeafQuickValue9; set => TCLE.LeafQuickValue9 = value; }
    }

    public class LeafTimeSignatures : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(TCLE.TimeSignatures);
        }
    }
}
