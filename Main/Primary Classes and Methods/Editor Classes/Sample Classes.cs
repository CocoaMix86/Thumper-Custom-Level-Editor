using System.Collections.ObjectModel;
using System.ComponentModel;
using Thumper_Custom_Level_Editor.Editor_Panels;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace Thumper_Custom_Level_Editor
{
    public class SampleData
    {
        public EditorSample Editor;
        public FileInfo File { get; set; }
        public string TempFile { get; set; }

        private string _objname;
        public string ObjName
        {
            get => _objname;
            set {
                if (!value.EndsWith(".samp"))
                    value += ".samp";
                _objname = value;
            }
        }

        public string Path { get; set; }
        public decimal Volume { get; set; }

        private decimal _pitch;
        public decimal Pitch
        {
            get => _pitch;
            set {
                _pitch = value;
                if (Editor != null)
                    UpdateRuntime();
            }
        }

        private decimal _pan;
        public decimal Pan
        {
            get => _pan;
            set {
                _pan = value;
            }
        }

        private int _offset;
        public int Offset
        {
            get => _offset;
            set {
                _offset = value;
                if (Editor != null)
                    UpdateRuntime();
            }
        }

        public string ChannelGroup { get; set; }
        public WaveForm Wave;
        public double Runtime = -1;
        public double AlteredRuntime => (this.Runtime - ((double)this.Offset / 1000d)) / (double)this.Pitch;
        public double Beats => (this.AlteredRuntime / 60) * (double)TCLE.BPM;
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return ObjName;
        }

        public SampleData Clone(EditorSample parent = null)
        {
            SampleData clone = new() {
                Editor = null,
                File = new(File.FullName),
                TempFile = TempFile,
                ObjName = ObjName,
                Path = Path,
                Volume = Volume,
                Pitch = Pitch,
                Pan = Pan,
                Offset = Offset,
                ChannelGroup = ChannelGroup,
                Wave = Wave.Clone(false),
                Runtime = Runtime,
            };
            clone.Editor = parent;
            return clone;
        }

        public void CalculateRuntime(int channel = -1, bool free = true)
        {
            if (this.TempFile == null)
                UtilAudio.PCtoAudioFile(this);
            if (channel == -1) 
                channel = Bass.BASS_StreamCreateFile(this.TempFile, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
            //pitch shift, pan, other fx
            float initialfreq = 0;
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, ref initialfreq);
            Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_FREQ, initialfreq * (float)this.Pitch);
            //after fx are done, generate the new wave and runtime
            UtilAudio.GenerateSampWave(this, channel);
            if (free) {
                Bass.BASS_ChannelFree(channel);
                Bass.BASS_StreamFree(channel);
            }
        }

        public void UpdateRuntime()
        {
            if (this.Editor != null) {
                int rowindex = this.Editor.SampleProperties.samplelist.IndexOf(this);
                this.Editor.sampleList.Rows[rowindex].Cells[2].Value = $"{this.Beats.ToString("0.##")} beats -- {TimeSpan.FromSeconds(this.AlteredRuntime).ToString(@"hh\:mm\:ss\.fff")}";
            }
        }
    }

    public class SampleProperties
    {
        public SampleProperties(EditorSample Parent)
        {
            ParentEditor = Parent;
            sample = new();
            samplelist = new();
            samplelist.CollectionChanged += ParentEditor._samplelist_CollectionChanged;
        }

        [Browsable(false)]
        public EditorSample ParentEditor;
        [Browsable(false)]
        public ObservableCollection<SampleData> samplelist;
        [Browsable(false)]
        public SampleData sample { get; set; }

        [CategoryAttribute("General")]
        [DisplayName("File Path")]
        [Description("The full path to this sample file.")]
        public string filepath => this.ParentEditor.WorkingFile.FullName;

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Sample Name")]
        [Description("")]
        public string name
        {
            get => sample.ObjName;
            set {
                if (!value.EndsWith(".samp"))
                    value += ".samp";
                //need to change the sample name in the playing channels
                for (int x = 0; x < UtilAudio.PlayingChannels.Count; x++) {
                    if (UtilAudio.PlayingChannels[x].Item2 == sample.ObjName)
                        UtilAudio.PlayingChannels[x] = new Tuple<DataGridView, string, int>(UtilAudio.PlayingChannels[x].Item1, value, UtilAudio.PlayingChannels[x].Item3);
                }
                sample.ObjName = value;
                ParentEditor._samplelist_CollectionChanged(null, null);
            }
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Volume")]
        [Description("1 is default.")]
        public decimal volume { get => sample.Volume; set => sample.Volume = value; }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Pitch")]
        [Description("1 is default.")]
        public decimal pitch { 
            get => sample.Pitch; 
            set {
                if (value <= 0) value = 0.1m;
                if (value > 10) value = 10.0m;
                sample.Pitch = value;
            } 
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Pan")]
        [Description("0 is default. Negative pans left, positive pans right.")]
        public decimal pan { 
            get => sample.Pan;
            set {
                if (value < -1) value = -1;
                if (value > 1) value = 1;
                sample.Pan = value;
            }
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Offset")]
        [Description("0 is default. Offsets the playback start position, measured in milliseconds. Can't be negative.")]
        public int offset
        {
            get => sample.Offset;
            set {
                if (value < 0)
                    value = 0;
                sample.Offset = value;
            }
        }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Channel")]
        [Description("There are several audio channels in the game that apply various EQ to the audio. Default is sequin.ch. Don't change this unless you know what you're doing.")]
        [TypeConverter(typeof(SampleChannels))]
        public string channel { get => sample.ChannelGroup; set => sample.ChannelGroup = value; }

        [CategoryAttribute("Sample Settings")]
        [DisplayName("Path")]
        [Description("The physical file path to the file that contains this audio sample. This path is hashed and exists in the Thumper cache folder.")]
        public string path { get => sample.Path; set => sample.Path = value; }
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
