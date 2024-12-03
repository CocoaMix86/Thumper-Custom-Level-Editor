using System.Collections.Generic;

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
