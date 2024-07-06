using System.Collections.Generic;
using System;

namespace Thumper_Custom_Level_Editor
{
    class LvlLeafData
	{
		public string leafname { get; set; }
		public int beats { get; set; }
		public List<string> paths { get; set; }
		public int id { get; set; }

		public LvlLeafData()
        {

		}

		public LvlLeafData(string LEAFNAME, int BEATS, List<string> PATHS)
		{
			this.leafname = LEAFNAME;
			this.beats = BEATS;
			this.paths = PATHS;
			this.id = new Random().Next(0, 1000000);
		}


		public LvlLeafData Clone()
		{
			LvlLeafData copy = new(this.leafname, this.beats, this.paths);
			return copy;
		}
	}
}
