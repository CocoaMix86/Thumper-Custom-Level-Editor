using Newtonsoft.Json.Linq;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Editor_Classes;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilCreate
    {
        public static SimpleLeafProperties SimpleLeaf(dynamic LeafToLoad, FileInfo File)
        {
            SimpleLeafProperties LeafProps = new() {
                LeafName = File.Name,
                LeafLength = (int?)LeafToLoad["beat_cnt"] ?? 1
            };
            LoadSequencer(LeafToLoad["seq_objs"], LeafProps);

            return LeafProps;
        }

        private static void LoadSequencer(dynamic LeafToLoad, SimpleLeafProperties LeafProps) 
        {
            DataGridView _setupdgv = new();
            _setupdgv.ColumnCount = LeafProps.LeafLength + EditorLeaf.FrozenColumnOffset;
            //
            List<SimpleSequencerObject> LoadedObjects = new();
            int audiochannels = 0;
            //each object in the seq_objs[] list
            foreach (dynamic seq_obj in LeafToLoad) {
                SimpleSequencerObject ObjectToImport = new(LeafProps, null) {
                    ObjName = ((string)seq_obj["obj_name"]),
                    DefaultValue = seq_obj["default"],
                    //if the leaf has definitions for these, add them. If not, set to defaults
                    ParamPath = seq_obj.ContainsKey("param_path_hash") ? $"0x{(string)seq_obj["param_path_hash"]}" : ((string)seq_obj["param_path"]),
                    EnabledInEditor = ((string)seq_obj["enabled"] ?? "True").Equals("true", StringComparison.OrdinalIgnoreCase),
                };
                for (int x = 0; x < LeafProps.Beats + EditorLeaf.FrozenColumnOffset; x++)
                    ObjectToImport.Cells.Add(new SimpleSeqDataPoint());

                //if object is a layer volume, we "reset" its index to x so it can be renumbered in case its out of order.
                if (ObjectToImport.ParamPath.StartsWith("layer_volume"))
                    ObjectToImport.ParamPath = "layer_volume,x";
                //if the object is a tuning layer, handle it here
                if (ObjectToImport.ObjName == "_TuningLayerX") {
                    ObjectToImport.FriendlyParam = ObjectToImport.ParamPath;
                    ObjectToImport.Default = TCLE.LeafObjects["_TuningLayerX;⮝ Tuning Layer X"];
                }
                //if object is a .samp, set category and friendly_param since they don't exist in LeafObjects
                else if (ObjectToImport.ObjName.EndsWith(".samp") && ObjectToImport.ParamPath == "play") {
                    ObjectToImport.FriendlyParam = "play";
                    ObjectToImport.Default = TCLE.LeafObjects["sample.samp;play"];
                }
                //otherwise, search LeafObjects for the friendly names for display purposes
                else {
                    try {
                        string normalizeParam = $"{(ObjectToImport.ObjName.EndsWith(".leaf", StringComparison.OrdinalIgnoreCase) ? "leafname" : ObjectToImport.ObjName)};{ObjectToImport.ParamPath.Replace(ObjectToImport.ParamPathLane, "ent")}";
                        DefaultSequencerObject objmatch = TCLE.LeafObjects[$"{normalizeParam}"]/* && obj.obj_name == ObjectToImport.obj_name.Replace(ParentLeaf.FilePath.Name, "leafname")*/;
                        ObjectToImport.Default = objmatch;
                        ObjectToImport.FriendlyParam = objmatch?.ParamDisplayName ?? "";
                    } catch (Exception) { }
                }

                LoadDataPoints(ObjectToImport, seq_obj);
                LoadedObjects.Add(ObjectToImport);
                _setupdgv.Rows.Add(ObjectToImport);
            }
            string _ee = string.Join(',', _setupdgv.Rows.Cast<DataGridViewRow>().Select(x => x.Index));
            //return Seq_Objs;
            LeafProps.SequencerObjects = LoadedObjects;
        }

        private static void LoadDataPoints(SimpleSequencerObject ObjectToImport, dynamic seq_obj)
        {
            //There are 2 methods here for backwards compat
            foreach (JObject data_point in seq_obj["data_points"]) {
                //if imported data is greater than leaf's beat count, skip it.
                //but don't break, as beats could have been written in the file out of order
                if ((int)data_point["beat"] >= ObjectToImport.ParentLeaf.LeafLength)
                    continue;
                SimpleSeqDataPoint data = ObjectToImport[(int)data_point["beat"] + EditorLeaf.FrozenColumnOffset];
                data.Interpolation = ((string)data_point["interp"])?.Replace("kTraitInterp", "") ?? "Linear";
                data.Ease = TCLE.Easings[(string)data_point["ease"] ?? "kEaseInOut"];
                data.Value = (decimal)data_point["value"];
            }
        }
    }

    public class SimpleLeafProperties : EditorPropertiesBase
    {
        public SimpleLeafProperties() { }

        public string LeafName { get; set; }
        public List<SimpleSequencerObject> SequencerObjects
        {
            get => _seqobjs;
            set => _seqobjs = value;
        }
        private List<SimpleSequencerObject> _seqobjs = new();
        public string SequencerType { get; set; } = ".leaf";

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
                else
                    return;
                Beats = (int)value;
            }
        }
        public int BeatsAndFrozen => Beats + EditorLeaf.FrozenColumnOffset;
    }

    public class SimpleSequencerObject : DataGridViewRow
    {
        public SimpleLeafProperties ParentLeaf;
        public DefaultSequencerObject Default;
        public SimpleSequencerObject(SimpleLeafProperties _parent, DefaultSequencerObject _default)
        {
            ParentLeaf = _parent;
            Default = _default;
        }        

        public SimpleSeqDataPoint this[int index]
        {
            get {
                if (index == -1)
                    return null;
                if (index >= this.Cells.Count)
                    return null;
                return (SimpleSeqDataPoint)this.Cells[index];
            }
            set {
                this.Cells[index] = value;
            }
        }

        public string ObjName { get; set; }

        private string _parampath;
        public string ParamPath
        {
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

        private decimal _defaultvalue;
        public decimal DefaultValue
        {
            get => _defaultvalue;
            set => _defaultvalue = value;
        }

        public string FriendlyParam { get; set; }

        private bool _enabledineditor = true;
        public bool EnabledInEditor
        {
            get => _enabledineditor;
            set {
                _enabledineditor = value;
            }
        }

        public bool MuteInEditor { get; set; }

        public SimpleSequencerObject CloneAsLane(string lane, bool showlane = false)
        {
            SimpleSequencerObject clone = new(this.ParentLeaf, this.Default) {
                ParentLeaf = this.ParentLeaf,
                ObjName = this.ObjName,
                ParamPath = this.ParamPath.Split('.')[0] + lane,
                //skip data points
                DefaultValue = this.DefaultValue,
                FriendlyParam = this.FriendlyParam,
                EnabledInEditor = true,
                MuteInEditor = false
            };
            return clone;
        }
    }

    public class SimpleSeqDataPoint : DataGridViewTextBoxCell
    {
        public SimpleSequencerObject ParentSeqObj => (SimpleSequencerObject)this.OwningRow;
        public int beat => this.ColumnIndex - EditorLeaf.FrozenColumnOffset;

        public decimal InGameValue => Value != null ? Convert.ToDecimal(Value) : ParentSeqObj.DefaultValue;
        protected override bool SetValue(int rowIndex, object value)
        {
            //sanitize inputs based on the trait type
            //skipping header row
            if (rowIndex is not -1 && this.OwningRow.Index is not -1) {
                value = TraitValidator.Sanitize(ParentSeqObj.Default.TraitType, value);
            }
            bool _set = base.SetValue(rowIndex, value);
            return _set;
        }

        private string _interp = "Linear";
        public string Interpolation
        {
            get => _interp;
            set => _interp = value;
        }

        private string _ease = "Ease In Out";
        public string Ease
        {
            get => _ease;
            set => _ease = value;
        }
    }
}
