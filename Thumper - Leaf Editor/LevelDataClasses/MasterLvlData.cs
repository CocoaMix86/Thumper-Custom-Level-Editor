using System;

namespace Thumper_Custom_Level_Editor
{
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

		public MasterLvlData(string LVLNAME, string GATENAME, bool PLAYPLUS, bool CHECKPOINT, bool ISOLATE, string CHECKPOINT_LEADER, string REST)
        {
			this.lvlname = LVLNAME;
			this.gatename = GATENAME;
			this.playplus = PLAYPLUS;
			this.checkpoint = CHECKPOINT;
			this.isolate = ISOLATE;
			this.checkpoint_leader = CHECKPOINT_LEADER;
			this.rest = REST;
			this.id = new Random().Next(0, 1000000);
        }

		public MasterLvlData Clone()
		{
			MasterLvlData copy = new(this.lvlname, this.gatename, this.playplus, this.checkpoint, this.isolate, this.checkpoint_leader, this.rest);
			return copy;
		}
	}
}