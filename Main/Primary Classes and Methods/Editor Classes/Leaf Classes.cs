using Newtonsoft.Json.Linq;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

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
        public decimal default_value { get; set; }
        public string footer { get; set; }
        public Color defaultcolor { get; set; }
        public bool favorite { get; set; }
    }

    public static class TraitValidator
    {
        public static decimal Sanitize(string trait, decimal value)
        {
            return trait switch
            {
                "kTraitBool" => value is 0 or 1 ? value : 1,
                "kTraitAction" => value is 0 or 1 ? value : 1,
                "kTraitInt" => Math.Truncate(value),
                "kTraitColor" => Math.Truncate(value),
                _ => value
            };
        }
    }

    public class Sequencer_Object : DataGridViewRow
    {
        public LeafProperties ParentLeaf;

        public Sequencer_Object()
        {
            this.DividerHeight = 0;
            this.HeaderCell.Style.BackColor = Color.Black;
        }

        public JObject ConvertToJson()
        {
            //if saving a leaf as a new name, obj_name's have to be updated, otherwise it saves with the old file's name
            if (this.ObjName == "leafname" || this.ObjName.Contains(".leaf") || string.IsNullOrEmpty(this.ObjName))
                this.ObjName = this.ParentLeaf.ParentEditor.WorkingFile.Name;
            //add all the required fields to the json object
            JObject s = new() {
                { "obj_name", this.ObjName },
                { (this.ParamPath.StartsWith("0x") ? "param_path_hash" : "param_path"), this.ParamPath.Replace("0x", "") },
                { "trait_type", this.TraitType },
                { "step", this.Step },
                { "default", this.DefaultValue },
                { "footer", this.Footer },
                { "editor_data", new JArray() { new object[] { this.HighlightColor.ToArgb(), this.highlight_value } } },
                { "enabled", this.EnabledInEditor },
            };
            //add all the datapoints of object
            JArray datapoints = new();
            for (int _in = EditorLeaf.FrozenColumnOffset; _in < this.ParentLeaf.Beats + EditorLeaf.FrozenColumnOffset; _in++) {
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
        public string ParamPath { get; set; }
        public string ParamPathLane => this.ParamPath == null ? "none" : (this.ParamPath.Contains('.') ? this.ParamPath.Split('.')[1] : "none");
        public string FriendlyLane => TCLE.TrackLaneFriendly[this.ParamPathLane];
        public string TraitType { get; set; }
        public bool Step { get; set; } = true;
        public decimal DefaultValue
        {
            get => _defaultvalue;
            set {
                //standardize values based on the type
                _defaultvalue = TraitValidator.Sanitize(TraitType, value);
            }
        }
        private decimal _defaultvalue;
        public string Footer { get; set; }
        public string Category { get; set; }
        public string FriendlyParam { get; set; }
        public Color HighlightColor
        {
            get => _highlightcolor;
            set {
                _highlightcolor = value;
                HighlightBrush = new(value);
            }
        }
        private Color _highlightcolor;
        public SolidBrush HighlightBrush;
        public float highlight_value { get; set; }
        public bool EnabledInEditor
        {
            get => _enabledineditor;
            set {
                if (value != _enabledineditor)
                    WaveBitmap = null;
                _enabledineditor = value;
            }
        }
        private bool _enabledineditor = true;
        public bool IsDefault { get; set; }

        public Bitmap WaveBitmap;
        public int id { get; set; }
        public bool MuteInEditor { get; set; }
        public bool ExpandLanesInEditor
        {
            get => _expandlanes;
            set {
                _expandlanes = value;
                if (Playback.Generating)
                    return;
                if (this.FriendlyLane is not "lane center" and not "none")
                    this.Visible = value;
                EditorLeaf.ChangeTrackName(this, this.Category);
            }
        }
        private bool _expandlanes;
        public bool HasShownError { get; set; }
        public Bitmap TuningLayer { get; set; }
        public List<Sequencer_Object?> Lanes
        {
            get {
                if (FriendlyLane is "none")
                    return new() { this };
                //
                return ParentLeaf.SequencerObjects.GetRange(this.Index + this.LaneOffsetFromTop, 5).Select(x => x.FriendlyParam == this.FriendlyParam ? x : null).ToList();
            }
        }
        private static readonly Dictionary<string, int> LaneOffsets = new() { ["a01"] = 0, ["a02"] = -1, ["ent"] = -2, ["z01"] = -3, ["z02"] = -4 };
        public int LaneOffsetFromTop => LaneOffsets.TryGetValue(ParamPathLane, out int offset) ? offset : 0;

        public void ClearDataPoints()
        {
            foreach (SeqDataPoint sdp in this.Cells) {
                sdp.Value = null;
                sdp.Ease = "Ease In Out";
                sdp.Interpolation = "Linear";
            }
        }

        public Sequencer_Object Clone(int CellsToClone = -1)
        {
            //Sequencer_Object clone = (Sequencer_Object)MemberwiseClone();Sequencer_Object clone = new(this.parent) {
            Sequencer_Object clone = new() {
                ParentLeaf = null,
                ObjName = this.ObjName,
                ParamPath = this.ParamPath,
                TraitType = this.TraitType,
                Step = this.Step,
                DefaultValue = this.DefaultValue,
                Footer = this.Footer,
                Category = this.Category,
                FriendlyParam = this.FriendlyParam,
                HighlightColor = this.HighlightColor,
                highlight_value = this.highlight_value,
                EnabledInEditor = true,
                IsDefault = false,
                MuteInEditor = false,
                id = TCLE.rng.Next(),
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
            Sequencer_Object clone = new() {
                ParentLeaf = this.ParentLeaf,
                ObjName = this.ObjName,
                ParamPath = this.ParamPath.Split('.')[0] + lane,
                TraitType = this.TraitType,
                //skip data points
                Step = this.Step,
                DefaultValue = this.DefaultValue,
                Footer = this.Footer,
                Category = this.Category,
                FriendlyParam = this.FriendlyParam,
                HighlightColor = this.HighlightColor,
                highlight_value = this.highlight_value,
                EnabledInEditor = true,
                IsDefault = true,
                MuteInEditor = false,
                id = TCLE.rng.Next()
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
        public void Reset()
        {
            this._interp = "Linear";
            this._ease = "Ease In Out";
            this.Value = null;
        }

        public JObject ConvertToJson()
        {
            object value = ParentSeqObj.TraitType == "kTraitFloat" ? Value : Convert.ToInt32(Value);

            return new()
            {
                ["beat"] = beat,
                ["value"] = (JToken)value,
                ["interp"] = $"kTraitInterp{Interpolation ?? "Linear"}",
                ["ease"] = $"k{Ease?.Replace(" ", "") ?? "EaseInOut"}"
            };
        }

        [Browsable(false)]
        public Sequencer_Object ParentSeqObj => (Sequencer_Object)this.OwningRow;
        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Beat #")]
        public int beat { 
            get {
                return this.ColumnIndex - EditorLeaf.FrozenColumnOffset; 
            } 
        }
        /*
        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Value")]
        public object Value
        {
            get => _value;
            set {
                _value = value;
                if (ParentSeqObj == null)
                    return;
                //if value changing on a tuning layer, recalc the values
                if (((Sequencer_Object)this.OwningRow).obj_name == "_TuningLayerX") {
                    Form_LeafEditor.CalculateTuningLayers(ParentSeqObj.parent, ParentSeqObj);
                    ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
                }

                ParentSeqObj.isdefault = false;
            }
        }
        private object _value;*/
        protected override bool SetValue(int rowIndex, object value)
        {
            //sanitize inputs based on the trait type
            //skipping header row
            if (rowIndex is not -1 && this.OwningRow.Index is not -1) {
                value = TraitValidator.Sanitize(ParentSeqObj.TraitType, Convert.ToDecimal(value));
            }

            bool _set = base.SetValue(rowIndex, value);

            if (rowIndex is -1 || this.OwningRow.Index is -1)
                return _set;

            this.OwningRow.DataGridView.InvalidateRow(this.RowIndex);

            if (((Sequencer_Object)this.OwningRow).Category == "PLAY SAMPLE")
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            //if value changing on a tuning layer, recalc the values
            if (((Sequencer_Object)this.OwningRow).ObjName == "_TuningLayerX") {
                EditorLeaf.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            }
            ParentSeqObj.IsDefault = false;
            return _set;
        }

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("In Game Value")]
        [Description("If cell has no value, this instead shows the default value of the sequencer object")]
        public decimal InGameValue => Value != null ? Convert.ToDecimal(Value) : ParentSeqObj.DefaultValue;

        [CategoryAttribute("Selected Data Point(s)")]
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

        [CategoryAttribute("Selected Data Point(s)")]
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

        public int OriginalRow { get; set; }
        public int OriginalColumn { get; set; }
        public SeqDataPoint Clone()
        {
            //SeqDataPoint sdp = (SeqDataPoint)MemberwiseClone();
            return new() { 
                Value = Value,
                Ease = Ease,
                Interpolation = Interpolation,
                OriginalRow = RowIndex,
                OriginalColumn = ColumnIndex
            };
        }
    }

    public class LeafProperties
    {
        public LeafProperties(EditorLeaf Parent)
        {
            ParentEditor = Parent;
            selectedobj = new() { ParentLeaf = this };
        }

        public JObject ConvertToJson()
        {
            //start building JSON output
            JObject _save = new() {
                { "obj_type", "SequinLeaf" },
                { "obj_name", this.ParentEditor.WorkingFile.Name },
                { "beat_cnt", this.Beats },
                { "time_sig", this.timesignature }
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
        public EditorLeaf ParentEditor;
        [Browsable(false)]
        public DataGridView trackEditor => ParentEditor.trackEditor;
        [Browsable(false)]
        public List<Sequencer_Object> SequencerObjects {
            get => _seqobjs;
            set {
                _seqobjs = value;
                ParentEditor.EnableLeafButtons();
            }
        }
        private List<Sequencer_Object> _seqobjs = new();
        [Browsable(false)]
        public Sequencer_Object selectedobj { get; set; }
        [Browsable(false)]
        public string SequencerType { get; set; } = ".leaf";

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string filepath => this.ParentEditor.WorkingFile.FullName;
        /*
        [Browsable(false)]
        public FileInfo LoadedLeaf
        {
            get => _loadedleaf;
            set {
                if (_loadedleaf != value) {
                    if (_loadedleaf != null)
                        TCLE.CloseFileLock(_loadedleaf);
                    _loadedleaf = value;
                    if (!_loadedleaf.Exists) {
                        using (StreamWriter sw = _loadedleaf.CreateText()) {
                            sw.Write(' ');
                            sw.Close();
                        }
                    }
                    TCLE.AddFileLock(_loadedleaf);
                }
            }
        }
        private FileInfo _loadedleaf;
        */
        [CategoryAttribute("Leaf Options")]
        [DisplayName("Leaf Length")]
        [Description("How many beats long this sequencer/leaf is.")]
        [Editor(typeof(LeafBeatLength), typeof(UITypeEditor))]
        public int Beats
        {
            get => _beats;
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
                _beats = (int)value;
                BeatsChangedSinceSave = true;
                if (!ParentEditor.EditorIsLoading)
                    ParentEditor.LeafLengthChanged();
            }
        }
        [Browsable(false)]
        public int _beats;
        [Browsable(false)]
        public int BeatsAndFrozen => _beats + EditorLeaf.FrozenColumnOffset;
        [Browsable(false)]
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
                ParentEditor.SaveCheckAndWrite(false, "Time signature changed");
                if (!ParentEditor.EditorIsLoading)
                    ParentEditor.TrackTimeSigHighlighting();
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
                ParentEditor.SaveCheckAndWrite(false, "Time signature changed");
                if (!ParentEditor.EditorIsLoading)
                    ParentEditor.TrackTimeSigHighlighting();
            }
        }
        private string TimeSignature;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Category")]
        [Description("")]
        public string category => selectedobj.Category;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Parameter")]
        [Description("")]
        public string parameter => selectedobj.FriendlyParam;

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Trait Type")]
        [Description("BOOL: accepts values 1 (on) or 0 (off); ACTION: accepts values 1 (activate); FLOAT: accepts float values; INT: accepts integer (no decimal) values; COLOR: accepts an integer representation of an ARGB color. Use the color wheel button to insert colors.")]
        public string traittype => selectedobj.TraitType?.Replace("kTrait", "");

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
        public float highlightvalue
        {
            get => selectedobj.highlight_value;
            set {
                selectedobj.highlight_value = value;
            }
        }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 0")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue0 { get => TCLE.LeafQuickValues[0]; set => TCLE.LeafQuickValues[0] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 1")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue1 { get => TCLE.LeafQuickValues[1]; set => TCLE.LeafQuickValues[1] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 2")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue2 { get => TCLE.LeafQuickValues[2]; set => TCLE.LeafQuickValues[2] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 3")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue3 { get => TCLE.LeafQuickValues[3]; set => TCLE.LeafQuickValues[3] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 4")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue4 { get => TCLE.LeafQuickValues[4]; set => TCLE.LeafQuickValues[4] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 5")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue5 { get => TCLE.LeafQuickValues[5]; set => TCLE.LeafQuickValues[5] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 6")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue6 { get => TCLE.LeafQuickValues[6]; set => TCLE.LeafQuickValues[6] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 7")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue7 { get => TCLE.LeafQuickValues[7]; set => TCLE.LeafQuickValues[7] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 8")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue8 { get => TCLE.LeafQuickValues[8]; set => TCLE.LeafQuickValues[8] = value; }

        [CategoryAttribute("Values (use hotkeys)")]
        [DisplayName("Quick 9")]
        [Description("Use hotkey to insert this value into selected cells.")]
        [Editor(typeof(LeafDecimalQuickValues), typeof(UITypeEditor))]
        public decimal quickvalue9 { get => TCLE.LeafQuickValues[9]; set => TCLE.LeafQuickValues[9] = value; }
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
