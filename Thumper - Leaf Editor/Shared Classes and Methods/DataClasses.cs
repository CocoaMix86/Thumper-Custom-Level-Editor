using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

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

    public class LvlLeafData
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

    public class MasterLvlData
    {
        [Browsable(false)]
        public string type { get; set; }

        private string Name;
        [Browsable(false)]
        public string name { 
            get { return Name; } 
            set {
                int idx = value.LastIndexOf('.');
                Name = value[..idx]; 
            }
        }
        [CategoryAttribute("General")]
        [DisplayName("Sublevel Name")]
        [Description("")]
        public string noneditname { get { return Name; } }

        [Browsable(false)]
        public string lvlname { get { return $"{name}.lvl"; } }
        [Browsable(false)]
        public string gatename { get { return $"{name}.gate"; } }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Play Plus")]
        [Description("When True, the sublevel shows up in Play+. Useful to have a tutorial sublevel in Play and then have it not show up in Play+.")]
        public bool playplus { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Checkpoint")]
        [Description("Enables the checkpoint that follows this sublevel.")]
        public bool checkpoint { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Isolate")]
        [Description("If True, only isolated sublevels will play in game. Mainly used for testing your level.")]
        public bool isolate { get; set; }

        [CategoryAttribute("Sublevel Options")]
        [DisplayName("Rest Lvl")]
        [Description("The rest lvl will play before the sublevel.")]
        [Editor(typeof(LvlPicker), typeof(UITypeEditor))]
        public string rest { get; set; }

        [Browsable(false)]
        public string checkpoint_leader { get; set; }
        [Browsable(false)]
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

    public class WorkingFolderFileItem
    {
        public string type { get; set; }
        public string filename { get; set; }
        public int index { get; set; }
    }

}
