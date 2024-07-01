using System;

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

		public int id { get; set; }

		public Sequencer_Object()
        {
        }

		public Sequencer_Object(string OBJ_NAME, string PARAM_PATH, string TRAIT_TYPE, dynamic DATA_POINTS, string STEP, float _DEFAULT, string FOOTER, string DEFAULT_INTERP, string FRIENDLY_TYPE, string FRIENDLY_PARAM, string HIGHLIGHT_COLOR, float HIGHLIGHT_VALUE)
        {
			this.obj_name = OBJ_NAME;
			this.param_path = PARAM_PATH;
			this.trait_type = TRAIT_TYPE;
			this.data_points = DATA_POINTS;
			this.step = STEP;
			this._default = _DEFAULT;
			this.footer = FOOTER;
			this.default_interp = DEFAULT_INTERP;
			this.friendly_type = FRIENDLY_TYPE;
			this.friendly_param = FRIENDLY_PARAM;
			this.highlight_color = HIGHLIGHT_COLOR;
			this.highlight_value = HIGHLIGHT_VALUE;
			this.id = new Random().Next(0, 1000000);
		}

		public Sequencer_Object Clone()
        {
			Sequencer_Object copy = new(this.obj_name, this.param_path, this.trait_type, this.data_points, this.step, this._default, this.footer, this.default_interp, this.friendly_type, this.friendly_param, this.highlight_color, this.highlight_value);
			return copy;
        }
	}
}
