using System;
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
	public partial class FormLeafEditor : Form
	{
		#region Variables
		bool _savegate = true;
		string _loadedgate
		{
			get { return loadedgate; }
			set
			{
				if (loadedgate != value) {
					loadedgate = value;
					MasterEditorVisible();
					mastersaveAsToolStripMenuItem.Enabled = true;
					mastersaveToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string loadedgate;
		string _loadedgatetemp;

		ObservableCollection<MasterLvlData> _gatelvls = new ObservableCollection<MasterLvlData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS ///
		///         ///
		#endregion
	}
}