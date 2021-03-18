using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
		bool _savemaster;
		string _loadedmaster;

		ObservableCollection<LvlLeafData> _masterlvls = new ObservableCollection<LvlLeafData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS ///
		///         ///
		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///
		private void btnMasterLvlAdd_Click(object sender, EventArgs e)
		{

		}

		private void btnMasterLvlDelete_Click(object sender, EventArgs e)
		{

		}

		private void btnMasterLvlUp_Click(object sender, EventArgs e)
		{

		}

		private void btnMasterLvlDown_Click(object sender, EventArgs e)
		{

		}
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///
		#endregion
	}
}