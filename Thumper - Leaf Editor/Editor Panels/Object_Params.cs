using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Thumper___Leaf_Editor
{
	public class Object_Params
	{
		public List<string> obj_name { get; set; }
		public string obj_displayname { get; set; }
		public List<string> param_displayname { get; set; }
		public List<string> param_path { get; set; }
		public List<string> trait_type { get; set; }
		public List<string> step { get; set; }
		public List<string> def { get; set; }
		public List<string> footer { get; set; }
	}
}
