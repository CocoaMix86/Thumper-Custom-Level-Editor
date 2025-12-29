using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;
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
        public Color defaultcolor { get; set; }
    }

    public class Sequencer_Object
    {
        public LeafProperties parent;

        public string obj_name { get; set; }
        public string param_path { get; set; }
        public string param_path_lane { get; set; }
        public string trait_type { get; set; }
        public List<SeqDataPoint> data_points { get; set; }
        public bool step { get; set; } = true;
        public float defaultvalue
        {
            get => DefVal;
            set {
                //standardize values based on the type
                if (this.trait_type == "kTraitBool") {
                    if ((decimal)value is not 1 and not 0)
                        value = 1;
                }
                else if (this.trait_type == "kTraitColor") {
                    value = (float)Math.Truncate(value);
                }
                else if (this.trait_type == "kTraitAction") {
                    if ((decimal)value is not 1 and not 0)
                        value = 1;
                }
                else if (this.trait_type == "kTraitInt") {
                    value = (float)Math.Truncate(value);
                }

                DefVal = value;
            }
        }
        private float DefVal;
        public string footer { get; set; }

        public string category { get; set; }
        public string friendly_param { get; set; }
        public Color highlight_color
        {
            get => HighCol;
            set {
                HighCol = value;
                HighlightBrush = new(value);
            }
        }
        private Color HighCol;
        public SolidBrush HighlightBrush;
        public float highlight_value { get; set; }
        public bool enabled
        {
            get => Enabled;
            set {
                if (value != Enabled)
                    WaveBitmap = null;
                Enabled = value;
            }
        }
        private bool Enabled = true;
        public bool isdefault { get; set; }

        public Bitmap WaveBitmap;
        public int id { get; set; }
        public bool mute { get; set; }
        public DataGridViewRow editor_row { get; set; }
        public bool expandlanes
        {
            get => ExpandLanes;
            set {
                ExpandLanes = value;
                if (this.editor_row == null)
                    return;
                if (this.friendly_lane is not "lane center" and not "none")
                    editor_row.Visible = value;
                Form_LeafEditor.ChangeTrackName(this, Properties.Settings.Default.LeafOptionShowCategory ? $"[{this.category}] " : "");
                Form_LeafEditor.TrackUpdateHighlighting(this);
            }
        }
        private bool ExpandLanes;
        public string friendly_lane { get; set; }
        public bool HasShownError { get; set; }
        public bool HasTuningLayer { get; set; }
        public Bitmap TuningLayer { get; set; }

        public Sequencer_Object(LeafProperties Parent)
        {
            parent = Parent;
            ClearDataPoints();
        }

        public Sequencer_Object(Sequencer_Object ToClone)
        {
            data_points = new SeqDataPoint[ToClone.data_points.Count].ToList();
            for (int x = 0; x < this.data_points.Count; x++) {
                this.data_points[x] = ToClone.data_points[x].CloneWithOwner(this, ToClone.data_points[x].beat);
            }
        }

        public void ClearDataPoints()
        {
            int maxbeats = parent.SequencerType == ".leaf" ? 255 : parent.beats;
            data_points = new SeqDataPoint[maxbeats].ToList();
            for (int x = 0; x < maxbeats; x++)
            {
                data_points[x] = new() { Owner = this, Beat = x };
            }
            if (editor_row != null)
                for (int x = 0; x < editor_row.Cells.Count; x++)
                    editor_row.Cells[x].Value = null;
        }

        public Sequencer_Object Clone()
        {
            //Sequencer_Object clone = (Sequencer_Object)MemberwiseClone();Sequencer_Object clone = new(this.parent) {
            Sequencer_Object clone = new(this) {
                obj_name = this.obj_name,
                param_path = this.param_path,
                trait_type = this.trait_type,
                //skip data points
                step = this.step,
                defaultvalue = this.defaultvalue,
                footer = this.footer,
                category = this.category,
                friendly_param = this.friendly_param,
                highlight_color = this.highlight_color,
                highlight_value = this.highlight_value,
                enabled = true,
                isdefault = false,
                mute = false,
                id = TCLE.rng.Next(),
                param_path_lane = this.param_path_lane,
                friendly_lane = this.friendly_lane,
                expandlanes = false,
                editor_row = null
            };
            clone.parent = null;
            return clone;
        }

        public Sequencer_Object CloneAsDefault(string lane, string friendlylane, DataGridViewRow dgvr)
        {
            Sequencer_Object clone = new(this.parent) {
                obj_name = this.obj_name,
                param_path = this.param_path,
                trait_type = this.trait_type,
                //skip data points
                step = this.step,
                defaultvalue = this.defaultvalue,
                footer = this.footer,
                category = this.category,
                friendly_param = this.friendly_param,
                highlight_color = this.highlight_color,
                highlight_value = this.highlight_value,
                enabled = true,
                isdefault = true,
                mute = false,
                id = TCLE.rng.Next(),
                param_path_lane = lane,
                friendly_lane = friendlylane,
                editor_row = dgvr,
                expandlanes = false,
            };
            return clone;
        }
    }

    public class SeqDataPoint
    {
        public void Reset()
        {
            this.interpolation = "Linear";
            this.ease = "Ease In Out";
            this.value = null;
        }

        [Browsable(false)]
        public Sequencer_Object Owner { get; set; }
        [Browsable(false)]
        public int index { get; set; }
        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Beat #")]
        public int beat { get => Beat; }
        [Browsable(false)]
        public int Beat;

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Value")]
        public decimal? value
        {
            get => (decimal?)Value;
            set {
                if (value != null) {
                }
                Value = value;
                //check if the beat being set is within the leaf's beatcount.
                //also check if Owner and the editing row exists.
                //and one final check to see if the value is the same. If it's the same, don't bother setting it.
                if (this.beat < Owner.parent.beats && Owner != null && Owner.editor_row != null) {
                    if ((decimal?)Owner.editor_row.Cells[beat + 3].Value != (decimal?)Value) {
                        Owner.editor_row.Cells[beat + 3].Value = Value;
                        Owner.parent.parent.CellValueChanged(Owner.editor_row.Index, beat + 3);
                    }
                    //if value changing on a tuning layer, recalc the values
                    if (Owner.obj_name == "_TuningLayerX") {
                        Form_LeafEditor.CalculateTuningLayers(Owner.parent, Owner);
                        Owner.parent.parent.trackEditor.InvalidateRow(Owner.editor_row.Index);
                    }

                    Owner.isdefault = false;
                }
            }
        }
        private object Value = null;

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Interp")]
        [TypeConverter(typeof(LeafInterpolations))]
        public string interpolation { 
            get => Interp;
            set {
                Interp = value;
                if (Owner.parent.parent.EditorIsLoading)
                    return;
                if (Owner.obj_name == "_TuningLayerX") {
                    Form_LeafEditor.CalculateTuningLayers(Owner.parent, Owner);
                    Owner.parent.parent.trackEditor.InvalidateRow(Owner.editor_row.Index);
                }
            } 
        }
        private string Interp = "Linear";

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Easing")]
        [TypeConverter(typeof(LeafEasings))]
        public string ease
        {
            get => Ease;
            set {
                Ease = value;
                if (Owner.parent.parent.EditorIsLoading)
                    return;
                if (Owner.obj_name == "_TuningLayerX") {
                    Form_LeafEditor.CalculateTuningLayers(Owner.parent, Owner);
                    Owner.parent.parent.trackEditor.InvalidateRow(Owner.editor_row.Index);
                }
            }
        }
        private string Ease = "Ease In Out";

        public void UpdateCell()
        {
            if (this.beat < Owner.parent.beats && Owner != null && Owner.editor_row != null) {
                if ((decimal?)Owner.editor_row.Cells[beat + 3].Value != (decimal?)Value) {
                    Owner.editor_row.Cells[beat + 3].Value = Value;
                    Owner.parent.parent.CellValueChanged(Owner.editor_row.Index, beat + 3);
                }

                Owner.isdefault = false;
            }
        }

        public SeqDataPoint Clone()
        {
            SeqDataPoint sdp = (SeqDataPoint)MemberwiseClone();
            sdp.index = Owner.parent.seq_objs.IndexOf(Owner);
            sdp.Owner = null;
            return sdp;
        }

        public SeqDataPoint CloneWithOwner(Sequencer_Object Owner, int newbeat)
        {
            SeqDataPoint sdp = (SeqDataPoint)MemberwiseClone();
            sdp.Beat = newbeat;
            sdp.Owner = Owner;
            return sdp;
        }
    }

    public class LeafProperties
    {
        [Browsable(false)]
        public Form_LeafEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public ObservableCollection<Sequencer_Object> seq_objs {
            get => _SeqObjs;
            set {
                _SeqObjs = value;
                _SeqObjs.CollectionChanged += parent.seqobjs_CollectionChanged;
                parent.EnableLeafButtons();
            }
        }
        private ObservableCollection<Sequencer_Object> _SeqObjs = new();
        [Browsable(false)]
        public Sequencer_Object selectedobj { get; set; }
        [Browsable(false)]
        public string SequencerType { get; set; } = ".leaf";

        public LeafProperties(Form_LeafEditor Parent, FileInfo path, int _beats)
        {
            parent = Parent;
            Beats = _beats;
            selectedobj = new(this);
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => FilePath.FullName;
        [Browsable(false)]
        public FileInfo FilePath => parent.loadedleaf;

        [CategoryAttribute("Leaf Options")]
        [DisplayName("Leaf Length")]
        [Description("How many beats long this sequencer/leaf is.")]
        [Editor(typeof(LeafBeatLength), typeof(UITypeEditor))]
        public int beats
        {
            get => Beats;
            set {
                if (SequencerType == ".leaf") {
                    if (value > 255)
                        value = 255;
                    else if (value < 1)
                        value = 1;
                }
                //cannot change beats if editing a non-leaf sequencer
                else
                    return;
                Beats = (int)value;
                BeatsChangedSinceSave = true;
                if (!parent.EditorIsLoading)
                    parent.LeafLengthChanged();
            }
        }
        [Browsable(false)]
        public int Beats;
        public bool BeatsChangedSinceSave = false;

        [Category​Attribute("Editor")]
        [DisplayName("Set Time Sig")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        public string timeedit
        {
            get => timesignature;
            set {
                //check if incoming value matches time sig pattern #/#
                Match reg = Regex.Match(value, "(^\\d+\\/\\d+$)");
                if (!reg.Success) {
                    MessageBox.Show("Time signature input was not in a valid form.\nIt should follow \"#/#\".", "Custom Editor Thumper Level");
                    return;
                }

                if (!TCLE.TimeSignatures.Contains(value))
                    TCLE.TimeSignatures.Add(value);
                TimeSignature = value;
                parent.SaveCheckAndWrite(false, "Time signature changed");
                if (!parent.EditorIsLoading)
                    parent.TrackTimeSigHighlighting();
            }
        }
        [Category​Attribute("Editor")]
        [DisplayName("Preset Time Sigs")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        [TypeConverter(typeof(LeafTimeSignatures))]
        public string timesignature
        {
            get => TimeSignature; 
            set {
                TimeSignature = value;
                parent.SaveCheckAndWrite(false, "Time signature changed");
                if (!parent.EditorIsLoading)
                    parent.TrackTimeSigHighlighting();
            }
        }
        private string TimeSignature;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Category")]
        [Description("")]
        public string category => selectedobj.category;

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
        [Description("FALSE: Blank cells use the last known set value. Some trait types will automatically interpolate between set values too. TRUE: Blank cells use the Default Value")]
        public bool step { get => selectedobj.step; set => selectedobj.step = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Default Value")]
        [Description("If Step TRUE, blank cells will use this value")]
        public float defaultvalue { get => selectedobj.defaultvalue; set => selectedobj.defaultvalue = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Color")]
        [Description("When Highlight Value is met, color the cell this color")]
        public Color highlightcolor
        {
            get => selectedobj.highlight_color;
            set { 
                selectedobj.highlight_color = value;
                Form_LeafEditor.TrackUpdateHighlighting(selectedobj);
            }
        }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Value")]
        [Description("When this value is met (+/-), color the cell the Highlight Color. Set to 0 to highlight all.")]
        public float highlightvalue
        {
            get => selectedobj.highlight_value;
            set {
                selectedobj.highlight_value = value;
                Form_LeafEditor.TrackUpdateHighlighting(selectedobj);
            }
        }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 0")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue0 { get => TCLE.LeafQuickValue0; set => TCLE.LeafQuickValue0 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 1")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue1 { get => TCLE.LeafQuickValue1; set => TCLE.LeafQuickValue1 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 2")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue2 { get => TCLE.LeafQuickValue2; set => TCLE.LeafQuickValue2 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 3")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue3 { get => TCLE.LeafQuickValue3; set => TCLE.LeafQuickValue3 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 4")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue4 { get => TCLE.LeafQuickValue4; set => TCLE.LeafQuickValue4 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 5")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue5 { get => TCLE.LeafQuickValue5; set => TCLE.LeafQuickValue5 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 6")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue6 { get => TCLE.LeafQuickValue6; set => TCLE.LeafQuickValue6 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 7")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue7 { get => TCLE.LeafQuickValue7; set => TCLE.LeafQuickValue7 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 8")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue8 { get => TCLE.LeafQuickValue8; set => TCLE.LeafQuickValue8 = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 9")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
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

    public class LeafInterpolations : StringConverter
    {
        List<string> interpolations = new() { "Linear", "Quadratic", "Cubic", "Quartic", "Quintic", "Sine", "Step", "None" };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(interpolations);
        }
    }

    public class LeafEasings : StringConverter
    {
        List<string> easings = new() { "Ease In Out", "Ease In", "Ease Out" };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(easings);
        }
    }

    public class LeafDecimalQuickValues : UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;
            if (provider != null) {
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (editorService != null) {
                NumericUpDown udControl = new() {
                    DecimalPlaces = 3,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    Value = (decimal)value
                };
                editorService.DropDownControl(udControl);
                value = (decimal)udControl.Value;
            }

            return value;
        }
    }

    public class LeafBeatLength : UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;
            if (provider != null) {
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (editorService != null) {
                NumericUpDown udControl = new() {
                    DecimalPlaces = 0,
                    Minimum = 1,
                    Maximum = 255,
                    Value = Decimal.Parse(value.ToString()),
                    Increment = 1
                };
                editorService.DropDownControl(udControl);
                value = (int)udControl.Value;
            }

            return value;
        }
    }
}
