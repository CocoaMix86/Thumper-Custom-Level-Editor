using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class SampleData
    {
        public string obj_name { get; set; }
        public string path { get; set; }
        public decimal volume { get; set; }
        public decimal pitch { get; set; }
        public decimal pan { get; set; }
        public int offset { get; set; }
        public string channel_group { get; set; }

        public override string ToString()
        {
            return obj_name;
        }
    }

    public class SampleProperties
    {
        [Browsable(false)]
        public Form_SampleEditor parent;
        [Browsable(false)]
        public JObject revertPoint { get; set; }
        [Browsable(false)]
        public List<JObject> undoItems { get; set; }
        [Browsable(false)]
        public ObservableCollection<SampleData> samplelist;
        [Browsable(false)]
        public SampleData sample { get; set; }

        public SampleProperties(Form_SampleEditor Parent, FileInfo path)
        {
            parent = Parent;
            FilePath = path;
            sample = new();
            undoItems = new();
            samplelist = new();
            samplelist.CollectionChanged += parent._samplelist_CollectionChanged;
        }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this sample file.")]
        public string filepath => FilePath.FullName;
        [Browsable(false)]
        public FileInfo FilePath;

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Sample Name")]
        [Description("")]
        public string name
        {
            get => sample.obj_name;
            set {
                if (!value.EndsWith(".samp"))
                    value += ".samp";
                sample.obj_name = value;
                parent._samplelist_CollectionChanged(null, null);
            }
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Volume")]
        [Description("1 is default.")]
        public decimal volume { get => sample.volume; set => sample.volume = value; }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Pitch")]
        [Description("1 is default.")]
        public decimal pitch { 
            get => sample.pitch; 
            set {
                if (value <= 0) value = 1;
                if (value > 100) value = 1;
                sample.pitch = value;
            } 
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Pan")]
        [Description("0 is default. Negative pans left, positive pans right.")]
        public decimal pan { get => sample.pan; set => sample.pan = value; }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Offset")]
        [Description("0 is default. Offsets the playback start position, measured in milliseconds. Can't be negative.")]
        public int offset
        {
            get => sample.offset;
            set {
                if (value < 0)
                    value = 0;
                sample.offset = value;
            }
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Channel")]
        [Description("There are several audio channels in the game that apply various EQ to the audio. Default is sequin.ch. Don't change this unless you know what you're doing.")]
        [TypeConverter(typeof(SampleChannels))]
        public string channel { get => sample.channel_group; set => sample.channel_group = value; }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Path")]
        [Description("The physical file path to the file that contains this audio sample. This path is hashed and exists in the Thumper cache folder.")]
        public string path { get => sample.path; set => sample.path = value; }
    }


    public class SampleChannels : StringConverter
    {
        private List<string> channels = new() { "base.ch", "base_credits.ch", "bass_cut.ch", "beat_time.ch", "beneath_ice.ch", "carve.ch", "checkpoint_hud.ch", "death_sfx.ch", "DF.ch", "dissonant_bursts.ch", "effects.ch", "effects_echo.ch", "effects_echoflange.ch", "effects_flanger.ch", "effects_loud.ch", "effects_tremelo_2hz.ch", "flutter_grind_wet.ch", "french_horn_swells.ch", "grind_thump_pitch.ch", "hI.ch", "i.ch", "master.ch", "master_realtime.ch", "Master_uncompressed.ch", "music_fade.ch", "once_rises.ch", "pound_hit.ch", "rail_drone_left.ch", "rail_drone_right.ch", "rises.ch", "rises_1_1.ch", "rise_delay.ch", "rise_delay_1_1.ch", "rumble.ch", "scrape_drone.ch", "scrape_sfx.ch", "sequin.ch", "streak_layer.ch", "swooshes.ch", "thumps.ch", "thumps_accents.ch", "thumps_realtime.ch", "thump_hit.ch", "tunnel_whooshes.ch", "turn_anticipation.ch", "turn_auto.ch", "turn_strike.ch", "ui.ch", "wail_delay.ch", "white_noise.ch", "wind.ch", "_m.ch" };

        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(channels);
        }
    }
}
