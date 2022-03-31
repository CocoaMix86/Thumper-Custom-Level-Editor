using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor
	{
		#region Variables

		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS  ///
		///         ///

		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///
		
		private uint Hash32(string s)
		{
			//this hashes stuff. Don't know why it does it this why.
			//this is ripped directly from the game's code
			uint h = 0x811c9dc5;
			foreach (char c in s)
				h = ((h ^ c) * 0x1000193) & 0xffffffff;
			h = (h * 0x2001) & 0xffffffff;
			h = (h ^ (h >> 0x7)) & 0xffffffff;
			h = (h * 0x9) & 0xffffffff;
			h = (h ^ (h >> 0x11)) & 0xffffffff;
			h = (h * 0x21) & 0xffffffff;

			return h;
		}
		#endregion
	}
}