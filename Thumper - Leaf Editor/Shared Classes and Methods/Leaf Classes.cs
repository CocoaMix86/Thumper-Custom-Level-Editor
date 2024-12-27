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
        public bool step { get; set; }
        public string def { get; set; }
        public string footer { get; set; }
    }

    public class Sequencer_Object
    {
        public string obj_name { get; set; }
        public string param_path { get; set; }
        public string trait_type { get; set; }
        public dynamic data_points { get; set; }
        public bool step { get; set; }
        public float defaultvalue { get; set; }
        public string footer { get; set; }
        public string default_interp { get; set; }

        public string friendly_type { get; set; }
        public string friendly_param { get; set; }
        public Color highlight_color { get; set; }
        public float highlight_value { get; set; }
        public bool enabled { get; set; }

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

        [Category​Attribute("Options")]
        [DisplayName("Time Signature")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        [TypeConverter(typeof(LeafTimeSignatures))]
        public string timesignature
        {
            get => TimeSignature; 
            set {
                TimeSignature = value;
                if (!parent.EditorIsLoading)
                    parent.TrackTimeSigHighlighting();
            }
        }
        private string TimeSignature;

        [Category​Attribute("Options")]
        [DisplayName("Show Category Name")]
        [Description("Shows/Hides the category names on the sequencer row headers.")]
        public bool showcategory
        {
            get => ShowCategory; 
            set {
                ShowCategory = value;
                foreach (DataGridViewRow dgvr in parent.trackEditor.Rows) {
                    parent.ChangeTrackName(dgvr);
                }
            }
        }
        private bool ShowCategory;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Category")]
        [Description("")]
        public string category => selectedobj.friendly_type;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Parameter")]
        [Description("")]
        public string parameter => selectedobj.friendly_param;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Trait Type")]
        [Description("BOOL: accepts values 1 (on) or 0 (off); ACTION: accepts values 1 (activate); FLOAT: accepts float values; INT: accepts integer (no decimal) values; COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors.")]
        public string traittype => selectedobj.trait_type?.Replace("kTrait", "");

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Step")]
        [Description("FALSE: Blank cells use the last known set value. Some trait types will automatically interpolate between set values too. TRUE: Blank cells use the Default value")]
        public bool step { get => selectedobj.step; set => selectedobj.step = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Default Value")]
        [Description("If Step FALSE, blank cells will use this value")]
        public float defaultvalue { get => selectedobj.defaultvalue; set => selectedobj.defaultvalue = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Color")]
        [Description("When Highlight Value is met, color the cell this color")]
        public Color highlightcolor
        {
            get => selectedobj.highlight_color;
            set { 
                selectedobj.highlight_color = value;
                Form_LeafEditor.TrackUpdateHighlighting(parent.trackEditor.CurrentRow, selectedobj);
            }
        }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Value")]
        [Description("When this value is met (+/-), color the cell the Highlight Color")]
        public float highlightvalue
        {
            get => selectedobj.highlight_value;
            set {
                selectedobj.highlight_value = value;
                Form_LeafEditor.TrackUpdateHighlighting(parent.trackEditor.CurrentRow, selectedobj);
            }
        }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 0")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue0 { get => TCLE.LeafQuickValue0; set => TCLE.LeafQuickValue0 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 1")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue1 { get => TCLE.LeafQuickValue1; set => TCLE.LeafQuickValue1 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 2")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue2 { get => TCLE.LeafQuickValue2; set => TCLE.LeafQuickValue2 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 3")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue3 { get => TCLE.LeafQuickValue3; set => TCLE.LeafQuickValue3 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 4")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue4 { get => TCLE.LeafQuickValue4; set => TCLE.LeafQuickValue4 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 5")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue5 { get => TCLE.LeafQuickValue5; set => TCLE.LeafQuickValue5 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 6")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue6 { get => TCLE.LeafQuickValue6; set => TCLE.LeafQuickValue6 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 7")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue7 { get => TCLE.LeafQuickValue7; set => TCLE.LeafQuickValue7 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 8")]
        [Description("Use hotkey to insert this value into selected cells.")]
        public decimal quickvalue8 { get => TCLE.LeafQuickValue8; set => TCLE.LeafQuickValue8 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
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
