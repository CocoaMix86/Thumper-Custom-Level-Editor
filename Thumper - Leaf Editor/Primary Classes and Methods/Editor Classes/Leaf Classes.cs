using System.Collections;
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
        public decimal default_value { get; set; }
        public string footer { get; set; }
        public Color defaultcolor { get; set; }
        public bool favorite { get; set; }
    }

    public class Sequencer_Object : DataGridViewRow
    {
        public LeafProperties ParentLeaf;

        public Sequencer_Object()
        {
            this.DividerHeight = 0;
            this.HeaderCell.Style.BackColor = Color.Black;
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

        public string obj_name { get; set; }
        public string param_path { get; set; }
        public string param_path_lane => this.param_path.Contains('.') ? this.param_path.Split('.')[1] : "none";
        public string friendly_lane => TCLE.TrackLaneFriendly[this.param_path_lane];
        public string trait_type { get; set; }
        public bool step { get; set; } = true;
        public decimal defaultvalue
        {
            get => DefVal;
            set {
                //standardize values based on the type
                if (this.trait_type == "kTraitBool") {
                    if (value is not 1 and not 0)
                        value = 1;
                }
                else if (this.trait_type == "kTraitColor") {
                    value = Math.Truncate(value);
                }
                else if (this.trait_type == "kTraitAction") {
                    if (value is not 1 and not 0)
                        value = 1;
                }
                else if (this.trait_type == "kTraitInt") {
                    value = Math.Truncate(value);
                }

                DefVal = value;
            }
        }
        private decimal DefVal;
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
        public bool expandlanes
        {
            get => ExpandLanes;
            set {
                ExpandLanes = value;
                if (Playback.Generating)
                    return;
                if (this.friendly_lane is not "lane center" and not "none")
                    this.Visible = value;
                Form_LeafEditor.ChangeTrackName(this, this.category);
            }
        }
        private bool ExpandLanes;
        public bool HasShownError { get; set; }
        public Bitmap TuningLayer { get; set; }
        public List<Sequencer_Object?> Lanes
        {
            get {
                if (friendly_lane is "none")
                    return new() { this };
                //
                return ParentLeaf.seq_objs.GetRange(this.Index + this.LaneOffsetFromTop, 5).Select(x => x.friendly_param == this.friendly_param ? x : null).ToList();
            }
        }
        public int LaneOffsetFromTop { 
            get {
                switch (param_path_lane) {
                    case "a01":
                        return 0;
                    case "a02":
                        return -1;
                    case "ent":
                        return -2;
                    case "z01":
                        return -3;
                    case "z02":
                        return -4;
                }
                return 0;
            }
        }

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
                obj_name = this.obj_name,
                param_path = this.param_path,
                trait_type = this.trait_type,
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
                expandlanes = false
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
                obj_name = this.obj_name,
                param_path = this.param_path.Split('.')[0] + lane,
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
            this.Interpolation = "Linear";
            this.Ease = "Ease In Out";
            this.Value = null;
        }

        [Browsable(false)]
        public Sequencer_Object ParentSeqObj => (Sequencer_Object)this.OwningRow;
        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Beat #")]
        public int beat { 
            get {
                return this.ColumnIndex - Form_LeafEditor.FrozenColumnOffset; 
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
                if (ParentSeqObj.trait_type == "kTraitBool") {
                    if ((decimal?)value is not null and not 1 and not 0)
                        value = 1m;
                }
                else if (ParentSeqObj.trait_type == "kTraitColor") {
                    value = TCLE.TruncateDecimal((decimal?)value, 0);
                }
                else if (ParentSeqObj.trait_type == "kTraitAction") {
                    if ((decimal?)value is not null or 1 or 0)
                        value = 1m;
                }
                else if (ParentSeqObj.trait_type == "kTraitInt") {
                    value = TCLE.TruncateDecimal((decimal?)value, 0);
                }
            }

            bool _set = base.SetValue(rowIndex, value);

            if (rowIndex is -1 || this.OwningRow.Index is -1)
                return _set;

            this.OwningRow.DataGridView.InvalidateRow(this.RowIndex);

            if (((Sequencer_Object)this.OwningRow).category == "PLAY SAMPLE")
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            //if value changing on a tuning layer, recalc the values
            if (((Sequencer_Object)this.OwningRow).obj_name == "_TuningLayerX") {
                Form_LeafEditor.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
            }
            ParentSeqObj.isdefault = false;
            return _set;
        }

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("In Game Value")]
        [Description("If cell has no value, this instead shows the default value of the sequencer object")]
        public decimal InGameValue
        {
            get {
                return this.Value != null ? (decimal)this.Value : (decimal)ParentSeqObj.defaultvalue;
            }
            set {
                this.Value = value;
            }
        }

        [CategoryAttribute("Selected Data Point(s)")]
        [DisplayName("Interp")]
        [TypeConverter(typeof(LeafInterpolations))]
        public string Interpolation { 
            get => _interp;
            set {
                _interp = value;
                if (ParentSeqObj == null || ParentSeqObj.ParentLeaf.ParentEditor.EditorIsLoading)
                    return;
                if (ParentSeqObj.obj_name == "_TuningLayerX") {
                    Form_LeafEditor.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
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
                if (ParentSeqObj.obj_name == "_TuningLayerX") {
                    Form_LeafEditor.CalculateTuningLayers(ParentSeqObj.ParentLeaf, ParentSeqObj);
                    ParentSeqObj.DataGridView.InvalidateRow(ParentSeqObj.Index);
                }
            }
        }
        private string _ease = "Ease In Out";

        public int OriginalRow;
        public int OriginalColumn;
        public SeqDataPoint Clone()
        {
            //SeqDataPoint sdp = (SeqDataPoint)MemberwiseClone();
            return new() { 
                Value = this.Value,
                Ease = this.Ease,
                Interpolation = this.Interpolation,
                OriginalRow = this.RowIndex,
                OriginalColumn = this.ColumnIndex
            };
        }
    }

    public class LeafProperties
    {
        public LeafProperties(Form_LeafEditor Parent)
        {
            ParentEditor = Parent;
            selectedobj = new() { ParentLeaf = this };
        }

        [Browsable(false)]
        public Form_LeafEditor ParentEditor;
        [Browsable(false)]
        public DataGridView trackEditor => ParentEditor.trackEditor;
        [Browsable(false)]
        public List<Sequencer_Object> seq_objs {
            get => _SeqObjs;
            set {
                _SeqObjs = value;
                ParentEditor.EnableLeafButtons();
            }
        }
        private List<Sequencer_Object> _SeqObjs = new();
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
        public int BeatsAndFrozen => _beats + Form_LeafEditor.FrozenColumnOffset;
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
        public decimal defaultvalue { get => selectedobj.defaultvalue; set => selectedobj.defaultvalue = value; }

        [CategoryAttribute("Sequencer Object")]
        [DisplayName("Highlight Color")]
        [Description("When Highlight Value is met, color the cell this color")]
        public Color highlightcolor
        {
            get => selectedobj.highlight_color;
            set { 
                selectedobj.highlight_color = value;
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
