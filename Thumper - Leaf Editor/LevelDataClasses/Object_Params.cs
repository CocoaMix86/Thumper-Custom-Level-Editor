using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Thumper_Custom_Level_Editor
{
	public class Object_Params
	{
		public string category { get; set; }
		/*
		public List<string> obj_name { get; set; }
		public List<string> param_displayname { get; set; }
		public List<string> param_path { get; set; }
		public List<string> trait_type { get; set; }
		public List<string> step { get; set; }
		public List<string> def { get; set; }
		public List<string> footer { get; set; }
		*/
		public string obj_name { get; set; }
		public string param_displayname { get; set; }
		public string param_path { get; set; }
		public string trait_type { get; set; }
		public string step { get; set; }
		public string def { get; set; }
		public string footer { get; set; }
	}
}
