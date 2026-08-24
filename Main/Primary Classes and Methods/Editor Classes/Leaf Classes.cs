using Newtonsoft.Json.Linq;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public class DefaultSequencerObject
    {
        public string Name { get; set; }
        public string ParamPath { get; set; }
        public bool Step { get; set; }
        public decimal DefaultValue { get; set; }
        public string Footer { get; set; }
        public Color DefaultColor { get; set; }
        public bool Favorite { get; set; }

        public enum Trait { Bool, Action, Int, Color, Float, None }
        public static readonly Dictionary<string, Trait> TraitLookup = new(StringComparer.OrdinalIgnoreCase) {
            { "kTraitBool", Trait.Bool },
            { "kTraitAction", Trait.Action },
            { "kTraitInt", Trait.Int },
            { "kTraitColor", Trait.Color },
            { "kTraitFloat", Trait.Float },
            { "", Trait.None },
        };
        public Trait TraitType;
        public string TraitTypeString => TraitType == Trait.None ? string.Empty : $"kTrait{TraitType}";

        public string Category
        {
            get => _category;
            set {
                _category = value;
                if (!Playback.Generating)
                    CategoryIcon = TCLE.Instance.imageListCategoryIcons.Images[$"{value}.png"];
            }
        }
        private string _category;
        public Image CategoryIcon;

        private string _paramdisplay;
        public string ParamDisplayName { 
            get => _paramdisplay;
            set { 
                _paramdisplay = value;
                _ = TrailLength;
            } 
        }
        private int? _trailLength;
        public int TrailLength
        {
            get {
                if (_trailLength.HasValue)
                    return _trailLength.Value;

                _trailLength = ParseTrailLength();
                return _trailLength.Value;
            }
        }
        public int ParseTrailLength()
        {
            string param = this.ParamDisplayName;

            int start = param.IndexOf('[');
            if (start == -1)
                return 0;

            start++;

            int end = param.IndexOf(' ', start);
            if (end == -1)
                end = param.IndexOf(']', start);

            if (end == -1)
                return 0;

            return int.TryParse(param.AsSpan(start, end - start), out int length) ? length : 0;
        }
    }

    public static class TraitValidator
    {
        public static object Sanitize(DefaultSequencerObject.Trait? trait, object value)
        {
            if (value == null || trait == null)
                return value;
            return trait switch
            {
                DefaultSequencerObject.Trait.Bool => (decimal)value is 0 or 1 ? value : 1m,
                DefaultSequencerObject.Trait.Action => (decimal)value is 0 or 1 ? value : 1m,
                DefaultSequencerObject.Trait.Int => Math.Truncate((decimal)value),
                DefaultSequencerObject.Trait.Color => Math.Truncate((decimal)value),
                _ => value
            };
        }
    }

    public class Sequencer_Object : SimpleSequencerObject
    {
        public LeafProperties ParentLeaf;
        public DefaultSequencerObject Default;
        public Sequencer_Object(LeafProperties _parent, DefaultSequencerObject _default) : base(_parent, _default)
        {
            ParentLeaf = _parent;
            Default = _default;
            this.DividerHeight = 0;
            this.HeaderCell.Style.BackColor = Color.Black;
            this.Height = ((EditorLeaf)ParentLeaf?.ParentEditor)?.trackZoomVert?.Value ?? 0;
            //for (int x = 0; x < ParentLeaf.Beats + EditorLeaf.FrozenColumnOffset; x++)
                //this.Cells.Add(new SeqDataPoint());
        }

        public JObject ConvertToJson()
        {
            //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
            //Object should NEVER be named leafname.
            if (this.ObjName == "leafname" || this.ObjName.Contains(".leaf") || string.IsNullOrEmpty(this.ObjName))
                this.ObjName = this.ParentLeaf.ParentEditor.WorkingFile.Name;
            //add all the required fields to the json object
            JObject s = new() {
                { "obj_name", this.ObjName },
                { (this.ParamPath.StartsWith("0x") ? "param_path_hash" : "param_path"), this.ParamPath.Replace("0x", "") },
                { "trait_type", Default.TraitTypeString },
                { "step", this.Step },
                { "default", this.DefaultValue },
                { "footer", this.Default?.Footer },
                { "editor_data", new JArray() { new object[] { this.HighlightColor.ToArgb(), this.highlight_value } } },
                { "enabled", this.EnabledInEditor },
            };
            //add all the datapoints of object
            JArray datapoints = new();
            for (int _in = EditorLeaf.FrozenColumnOffset; _in < this.ParentLeaf.LeafLength + EditorLeaf.FrozenColumnOffset; _in++) {
                if (this[_in]?.Value == null)
                    continue;
                datapoints.Add(this[_in].ConvertToJson());
            }
            s.Add("data_points", datapoints);

            return s;
        }       

        public SeqDataPoint this[int index]
        {
            get {
                if (index == -1)
                    return null;
                if (index >= this.Cells.Count)
                    return null;
                return (SeqDataPoint)this.Cells[index];
            }
            set {
                this.Cells[index] = value;
            }
        }

        public string ObjName { get; set; }

        public string FriendlyParam { get; set; } = "";
        private string _parampath;
        public string ParamPath { 
            get => _parampath;
            set {
                if (value == null)
                    return;
                _parampath = value;
                string[] _split = ParamPath.Split('.');
                ParamPathBase = _split[0];
                if (this.ParamPath.Contains('.'))
                    ParamPathLane = _split[1];
                else
                    ParamPathLane = "none";
            }
        }
        public string ParamPathBase { get; set; }
        public string ParamPathLane { get; set; } = "none";
        public string FriendlyLane => TCLE.TrackLaneFriendly[this.ParamPathLane];
        public int LaneOffsetFromTop => TCLE.LaneOffsets.TryGetValue(ParamPathLane, out int offset) ? offset : 0;
        //
        public bool Step { get; set; }

        private decimal _defaultvalue;
        public decimal DefaultValue
        {
            get => _defaultvalue;
            set {
                //standardize values based on the type
                _defaultvalue = Convert.ToDecimal(TraitValidator.Sanitize(Default?.TraitType, value));
            }
        }

        private Color _highlightcolor = Color.Purple;
        public Color HighlightColor
        {
            get => _highlightcolor;
            set {
                _highlightcolor = value;
                HighlightBrush.Color = value;
            }
        }
        public SolidBrush HighlightBrush = new(Color.Purple);
        public decimal highlight_value { get; set; }

        private bool _enabledineditor = true;
        public bool EnabledInEditor
        {
            get => _enabledineditor;
            set {
                if (value != _enabledineditor)
                    WaveBitmap = null;
                _enabledineditor = value;
            }
        }

        public bool IsDefault { get; set; }

        private Bitmap _waveBitmap;
        public Bitmap WaveBitmap
        {
            get => _waveBitmap;
            set {
                _waveBitmap?.Dispose();
                _waveBitmap = value;
            }
        }

        public bool MuteInEditor { get; set; }
        private bool _expandlanes;
        public bool ExpandLanesInEditor
        {
            get => _expandlanes;
            set {
                _expandlanes = value;
                if (Playback.Generating)
                    return;
                if (this.FriendlyLane is not "lane center" and not "none")
                    this.Visible = value;
                EditorLeaf.SetRowHeaderText(this);
            }
        }
        public bool HasShownError { get; set; }
        public Bitmap TuningLayer { get; set; }

        public void ClearDataPoints()
        {
            //set EditorIsLoading to prevent every SDP from triggering recalcs
            ParentLeaf.ParentEditor.EditorIsLoading = true;
            try {
                foreach (SeqDataPoint sdp in this.Cells)
                    sdp.Reset();
            }
            finally {
                ParentLeaf.ParentEditor.EditorIsLoading = false;
            }
        }

        public Sequencer_Object Clone(int CellsToClone = -1)
        {
            //Sequencer_Object clone = (Sequencer_Object)MemberwiseClone();Sequencer_Object clone = new(this.parent) {
            Sequencer_Object clone = new(null, this.Default) {
                ParentLeaf = null,
                ObjName = this.ObjName,
                ParamPath = this.ParamPath,
                Step = this.Step,
                DefaultValue = this.DefaultValue,
                FriendlyParam = this.FriendlyParam,
                HighlightColor = this.HighlightColor,
                highlight_value = this.highlight_value,
                EnabledInEditor = true,
                IsDefault = false,
                MuteInEditor = false,
                ExpandLanesInEditor = false
            };
            clone.CloneCells(this, CellsToClone);
            return clone;
        }

        private void CloneCells(DataGridViewRow rowTemplate, int CellsToClone)
        {
            int cellsCount = CellsToClone == -1 ? rowTemplate.Cells.Count : CellsToClone;
            if (cellsCount > rowTemplate.Cells.Count)
                cellsCount = rowTemplate.Cells.Count;
            if (cellsCount > 0) {
                SeqDataPoint[] cells = new SeqDataPoint[cellsCount];
                for (int i = 0; i < cellsCount; i++) {
                    SeqDataPoint dataGridViewCell = rowTemplate.Cells[i] as SeqDataPoint;
                    SeqDataPoint dgvcNew = dataGridViewCell.Clone();
                    cells[i] = dgvcNew;
                }

                Cells.AddRange(cells);
            }
        }

        public Sequencer_Object CloneAsLane(string lane, bool showlane = false)
        {
            Sequencer_Object clone = new(this.ParentLeaf, this.Default) {
                ParentLeaf = this.ParentLeaf,
                ObjName = this.ObjName,
                ParamPath = this.ParamPathBase + lane,
                //skip data points
                Step = this.Step,
                DefaultValue = this.DefaultValue,
                FriendlyParam = this.FriendlyParam,
                HighlightColor = this.HighlightColor,
                highlight_value = this.highlight_value,
                EnabledInEditor = true,
                IsDefault = true,
                MuteInEditor = false,
                ExpandLanesInEditor = showlane
            };
            return clone;
        }
    }    

    public class SequencerColumn : DataGridViewTextBoxColumn
    {
        public SequencerColumn()
        {
            this.CellTemplate = new SeqDataPoint();
        }
    }

    public class SeqDataPoint : DataGridViewTextBoxCell
    {
        [Browsable(false)]
        public Sequencer_Object ParentSeqObj => (Sequencer_Object)this.OwningRow;

        public void Reset()
        {
            this._interp = "Linear";
            this._ease = "Ease In Out";
            this.Value = null;
        }

        public JObject ConvertToJson()
        {
            if (this.ParentSeqObj.Default?.TraitType == DefaultSequencerObject.Trait.Float) { 
                return new() { 
                    { "beat", this.beat }, 
                    { "value", (decimal)this.Value }, 
                    { "interp", $"kTraitInterp{this.Interpolation ?? "Linear"}" }, 
                    { "ease", $"k{this.Ease?.Replace(" ", "") ?? "EaseInOut"}" } 
                }; 
            } 
            else { 
                return new() { 
                    { "beat", this.beat }, 
                    { "value", (int)(decimal)this.Value }, 
                    { "interp", $"kTraitInterp{this.Interpolation ?? "Linear"}" }, 
                    { "ease", $"k{this.Ease?.Replace(" ", "") ?? "EaseInOut"}" } 
                }; 
            }
        }

        protected override bool SetValue(int rowIndex, object value)
        {
            //sanitize inputs based on the trait type
            //skipping header row
            if (rowIndex is not -1 && this.OwningRow.Index is not -1) {
                value = TraitValidator.Sanitize(ParentSeqObj.Default?.TraitType, value);
            }

            bool _set = base.SetValue(rowIndex, value);

            if (rowIndex is -1 || this.OwningRow.Index is -1)
                return _set;

            this.OwningRow.DataGridView.InvalidateRow(this.RowIndex);

            if (((Sequencer_Object)this.OwningRow).Default?.Category == "PLAY SAMPLE")
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            //if value changing on a tuning layer, recalc the values
            if (((Sequencer_Object)this.OwningRow).ObjName == "_TuningLayerX") {
                EditorLeaf.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            }
            ParentSeqObj.IsDefault = false;
            return _set;
        }

        [CategoryAttribute(" Selected Data Point(s)")]
        [DisplayName("Beat #")]
        public int beat => this.ColumnIndex - EditorLeaf.FrozenColumnOffset;

        [CategoryAttribute(" Selected Data Point(s)")]
        [DisplayName("In Game Value")]
        [Description("If cell has no value, this instead shows the default value of the sequencer object")]
        public decimal InGameValue => Value != null ? Convert.ToDecimal(Value) : ParentSeqObj.DefaultValue;

        [CategoryAttribute(" Selected Data Point(s)")]
        [DisplayName("Interp")]
        [TypeConverter(typeof(LeafInterpolations))]
        public string Interpolation { 
            get => _interp;
            set {
                _interp = value;
                if (ParentSeqObj == null || ParentSeqObj.ParentLeaf.ParentEditor.EditorIsLoading)
                    return;
                if (ParentSeqObj.ObjName == "_TuningLayerX") {
                    EditorLeaf.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                    ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
                }
            } 
        }
        private string _interp = "Linear";

        [CategoryAttribute(" Selected Data Point(s)")]
        [DisplayName("Easing")]
        [TypeConverter(typeof(LeafEasings))]
        public string Ease
        {
            get => _ease;
            set {
                _ease = value;
                if (ParentSeqObj == null || ParentSeqObj.ParentLeaf.ParentEditor.EditorIsLoading)
                    return;
                if (ParentSeqObj.ObjName == "_TuningLayerX") {
                    EditorLeaf.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                    ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
                }
            }
        }
        private string _ease = "Ease In Out";

        [Browsable(false)]
        public int OriginalRow { get; set; }
        [Browsable(false)]
        public int OriginalColumn { get; set; }
        public SeqDataPoint Clone()
        {
            return new() { 
                Value = Value,
                Ease = Ease,
                Interpolation = Interpolation,
                OriginalRow = RowIndex,
                OriginalColumn = ColumnIndex
            };
        }
    }

    public class LeafProperties : SimpleLeafProperties
    {
        public LeafProperties(EditorLeaf Parent)
        {
            ParentEditor = Parent;
            selectedobj = new(this, null);
        }

        public JObject ConvertToJson()
        {
            //start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLeaf" },
                { "obj_name", this.ParentEditor.WorkingFile.Name },
                { "beat_cnt", this.LeafLength },
                { "time_sig", this.TimeSignature }
            };

            JArray seq_objs = new();
            //isdefault = true means object has not been changed in any way.
            //friendly_param = null means the object wasn't initialized properly and will have errors when it comes time to save.
            foreach (Sequencer_Object seq_obj in this.SequencerObjects.Where(x => !x.IsDefault && x.FriendlyParam != null)) {
                seq_objs.Add(seq_obj.ConvertToJson());
            }
            //add all seq_objs to the overall leaf
            _save.Add("seq_objs", seq_objs);

            return _save;
        }

        [Browsable(false)]
        public List<Sequencer_Object> SequencerObjects {
            get => _seqobjs;
            set {
                _seqobjs = value;
                ((EditorLeaf)ParentEditor).EnableLeafButtons();
            }
        }
        private List<Sequencer_Object> _seqobjs = new();
        [Browsable(false)]
        public Sequencer_Object selectedobj { get; set; }
        [Browsable(false)]
        public string SequencerType { get; set; } = ".leaf";

        [CategoryAttribute("Leaf Options")]
        [DisplayName("Leaf Length")]
        [Description("How many beats long this sequencer/leaf is.")]
        [Editor(typeof(LeafBeatLength), typeof(UITypeEditor))]
        public int LeafLength
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
                else if (Beats == value)
                    return;

                Beats = (int)value;
                if (!ParentEditor.EditorIsLoading) {
                    BeatsChangedSinceSave = true;
                    ((EditorLeaf)ParentEditor).LeafLengthChanged();
                }
            }
        }
        [Browsable(false)]
        public int BeatsAndFrozen => Beats + EditorLeaf.FrozenColumnOffset;
        [Browsable(false)]
        public bool BeatsChangedSinceSave = false;

        [CategoryAttribute("Editor")]
        [DisplayName("Set Time Sig")]
        [Description("Editor only. Affects the column highlighting so you can see the measures")]
        public string TimeEdit
        {
            get => TimeSignature;
            set {
                //check if incoming value matches time sig pattern #/#
                Match reg = Regex.Match(value, "(^\\d+\\/\\d+$)");
                if (!reg.Success) {
                    MessageBox.Show("Time signature input was not in a valid form.\nIt should follow the pattern: \"#/#\".", "Custom Editor Thumper Level");
                    return;
                }

                if (!TCLE.TimeSignatures.Contains(value))
                    TCLE.TimeSignatures.Add(value);
                TimeSignature = value;
            }
        }
        [CategoryAttribute("Editor")]
        [DisplayName("Preset Time Sigs")]
        [Description("Editor only. Affects the column highlighting so you can see the measuers")]
        [TypeConverter(typeof(LeafTimeSignatures))]
        public string TimeSignature
        {
            get => _timesig; 
            set {
                if (!int.TryParse(value.Split('/')[0], out int timesigbeats))
                    return;
                _timesig = value;
                TimeTopBeat = timesigbeats;
                if (!ParentEditor.EditorIsLoading)
                    ((EditorLeaf)ParentEditor).TrackTimeSigHighlighting();
                ((EditorLeaf)ParentEditor).SaveCheckAndWrite(false, "Time signature changed");
            }
        }
        private string _timesig;
        public int TimeTopBeat = 4;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Category")]
        [Description("")]
        public string category => selectedobj.Default?.Category;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Parameter")]
        [Description("")]
        public string parameter => selectedobj.FriendlyParam;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Trait Type")]
        [Description("BOOL: accepts values 1 (on) or 0 (off); ACTION: accepts values 1 (activate); FLOAT: accepts float values; INT: accepts integer (no decimal) values; COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors.")]
        public string traittype => selectedobj.Default?.TraitTypeString;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Step")]
        [Description("FALSE: Blank cells use the last known set value. Some trait types will automatically interpolate between set values too. TRUE: Blank cells use the Default Value")]
        public bool step { get => selectedobj.Step; set => selectedobj.Step = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Default Value")]
        [Description("If Step TRUE, blank cells will use this value")]
        public decimal defaultvalue { get => selectedobj.DefaultValue; set => selectedobj.DefaultValue = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Color")]
        [Description("When Highlight Value is met, color the cell this color")]
        public Color highlightcolor
        {
            get => selectedobj.HighlightColor;
            set { 
                selectedobj.HighlightColor = value;
            }
        }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Value")]
        [Description("When this value is met (+/-), color the cell the Highlight Color. Set to 0 to highlight all.")]
        public decimal highlightvalue
        {
            get => selectedobj.highlight_value;
            set {
                selectedobj.highlight_value = value;
            }
        }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 1")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue1 { get => TCLE.LeafQuickValues[1]; set => TCLE.LeafQuickValues[1] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 2")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue2 { get => TCLE.LeafQuickValues[2]; set => TCLE.LeafQuickValues[2] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 3")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue3 { get => TCLE.LeafQuickValues[3]; set => TCLE.LeafQuickValues[3] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 4")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue4 { get => TCLE.LeafQuickValues[4]; set => TCLE.LeafQuickValues[4] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 5")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue5 { get => TCLE.LeafQuickValues[5]; set => TCLE.LeafQuickValues[5] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 6")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue6 { get => TCLE.LeafQuickValues[6]; set => TCLE.LeafQuickValues[6] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 7")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue7 { get => TCLE.LeafQuickValues[7]; set => TCLE.LeafQuickValues[7] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 8")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue8 { get => TCLE.LeafQuickValues[8]; set => TCLE.LeafQuickValues[8] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 9")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue9 { get => TCLE.LeafQuickValues[9]; set => TCLE.LeafQuickValues[9] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 0")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal QuickValue0 { get => TCLE.LeafQuickValues[0]; set => TCLE.LeafQuickValues[0] = value; }
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
