using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

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

        public Sequencer_Object()
        {
        }

        public Sequencer_Object Clone()
        {
            return (Sequencer_Object)MemberwiseClone();
        }
    }

    class LvlLeafData
	{
		public string leafname { get; set; }
		public int beats { get; set; }
		public List<string> paths { get; set; }
		public int id { get; set; }

		public LvlLeafData()
        {

		}

        public LvlLeafData Clone()
        {
            LvlLeafData leaf = (LvlLeafData)MemberwiseClone();
            leaf.paths = new List<string>(paths);
            return leaf;
        }
    }

    public class GateLvlData
    {
        public string lvlname { get; set; }
        public string sentrytype { get; set; }
        public int bucket { get; set; }
    }

    public class BossData
    {
        public string boss_name { get; set; }
        public string boss_spn { get; set; }
        public string boss_ent { get; set; }
    }

    class MasterLvlData
    {
        public string lvlname { get; set; }
        public string gatename { get; set; }
        public bool playplus { get; set; }
        public bool checkpoint { get; set; }
        public bool isolate { get; set; }
        public string checkpoint_leader { get; set; }
        public string rest { get; set; }
        public int id { get; set; }

        public MasterLvlData()
        {

        }

        public MasterLvlData Clone()
        {
            return (MasterLvlData)MemberwiseClone();
        }
    }

    public class SampleData
    {
        public string obj_name { get; set; }
        public string path { get; set; }
        public decimal volume { get; set; }
        public decimal pitch { get; set; }
        public decimal pan { get; set; }
        public decimal offset { get; set; }
        public string channel_group { get; set; }

        public override string ToString()
        {
            return obj_name;
        }
    }

    class WorkingFolderFileItem
    {
        public string type { get; set; }
        public string filename { get; set; }
        public int index { get; set; }
    }

    class MasterProperties
    {
        [CategoryAttribute("Options")]
        [DisplayName("Skybox")]
        [Description("")]
        public string skybox { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Intro Lvl")]
        [Description("This lvl will play at the beginning of your level, and whenever you restart.")]
        public string introlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Checkpoint Lvl")]
        [Description("This lvl will play immediately after each checkpoint.")]
        public string checkpointlvl { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("BPM")]
        [Description("Beats Per Minute. If your song is at 100bpm, you'll likely want to map at either 200 or 400, so you can place objects on half note and quarter note intervals.")]
        public decimal bpm { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Rail Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color rail { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Rail Glow Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color railglow { get; set; }

        [CategoryAttribute("Options")]
        [DisplayName("Path Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color path { get; set; }
    }
}
