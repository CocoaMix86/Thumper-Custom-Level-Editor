namespace Thumper_Custom_Level_Editor
{
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
	}
}
