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
		bool _savesample = true;
		string _loadedsample
		{
			get { return loadedsample; }
			set {
				if (loadedsample != value) {
					loadedsample = value;
					SampleEditorVisible(true);
					SamplesaveAsToolStripMenuItem.Enabled = true;
					SamplesaveToolStripMenuItem.Enabled = true;
				}
			}
		}
		private string loadedsample;
		string _loadedsampletemp;
		ObservableCollection<SampleData> _samplelist = new ObservableCollection<SampleData>();
		#endregion

		#region EventHandlers
		///         ///
		/// EVENTS  ///
		///         ///

		private void panelSample_SizeChanged(object sender, EventArgs e)
		{
			lblSampleEditor.MaximumSize = new Size(panelSample.Width - 16, 0);
			btnSaveSample.Location = new Point(lblSampleEditor.Location.X + lblSampleEditor.Size.Width, btnSaveSample.Location.Y);
		}

		private void sampleList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			//remove event handlers so they don't trigger when the values change
			txtSampPath.TextChanged -= txtSampPath_TextChanged;
			//update values with selected data
			txtSampPath.Text = _samplelist[e.RowIndex].path;
			//re-add the event handlers
			txtSampPath.TextChanged += txtSampPath_TextChanged;
		}

		private void sampleList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			//When a cell is finished editing, resave the row to _samplelist
			_samplelist[e.RowIndex] = new SampleData() {
				obj_name = (string)sampleList.Rows[e.RowIndex].Cells[0].Value.ToString(),
				volume = Decimal.Parse(sampleList.Rows[e.RowIndex].Cells[1].Value.ToString()),
				pitch = Decimal.Parse(sampleList.Rows[e.RowIndex].Cells[2].Value.ToString()),
				pan = Decimal.Parse(sampleList.Rows[e.RowIndex].Cells[3].Value.ToString()),
				offset = Decimal.Parse(sampleList.Rows[e.RowIndex].Cells[4].Value.ToString()),
				path = txtSampPath.Text
			};
			SaveSample(false);
		}

		private void sampleList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			//during editing of a cell in sampleList, check and sanitize input so it's numeric only
			e.Control.KeyPress -= new KeyPressEventHandler(NumericInputSanitize);
			if (sampleList.CurrentCell.ColumnIndex == 1 || sampleList.CurrentCell.ColumnIndex == 2 || sampleList.CurrentCell.ColumnIndex == 3 || sampleList.CurrentCell.ColumnIndex == 4) //Desired Column
			{
				TextBox tb = e.Control as TextBox;
				if (tb != null) {
					tb.KeyPress += new KeyPressEventHandler(NumericInputSanitize);
				}
			}
		}

		private void sampleList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (_dgfocus != "sampleList") {
				_dgfocus = "sampleList";
			}
		}

		public void _samplelist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
			//clear dgv
			sampleList.RowCount = 0;
			//repopulate dgv from list
			sampleList.RowEnter -= sampleList_RowEnter;
			foreach (SampleData _samp in _samplelist) {
				sampleList.Rows.Add(new object[] { _samp.obj_name, _samp.volume, _samp.pitch, _samp.pan, _samp.offset });
			}
			sampleList.RowEnter += sampleList_RowEnter;
			//enable certain buttons if there are enough items for them
			btnGateLvlDelete.Enabled = _gatelvls.Count > 0;

			//set lvl save flag to false
			SaveSample(false);
		}

		private void txtSampPath_TextChanged(object sender, EventArgs e)
		{
			_samplelist[sampleList.CurrentRow.Index].path = txtSampPath.Text;
		}


		private void SamplenewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savesample && MessageBox.Show("Current Samples is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savesample) {
				//reset things to default values
				_samplelist.Clear();
				lblSampleEditor.Text = "Sample Editor";
				//set saved flag to true, because nothing is loaded
				SaveSample(true);
				if (e != null)
					SamplesaveAsToolStripMenuItem_Click(null, null);
			}
		}

		private void SampleopenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ((!_savesample && MessageBox.Show("Current Samples is not saved. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) || _savesample) {
				using (var ofd = new OpenFileDialog()) {
					ofd.Filter = "Thumper Sample File (*.txt)|samp_*.txt";
					ofd.Title = "Load a Thumper Sample file";
					if (ofd.ShowDialog() == DialogResult.OK) {
						//storing the filename in temp so it doesn't overwrite _loadedsample in case it fails the check in LoadSample()
						_loadedsampletemp = ofd.FileName;
						//load json from file into _load. The regex strips any comments from the text.
						dynamic _load = JsonConvert.DeserializeObject(Regex.Replace(File.ReadAllText(ofd.FileName), "#.*", ""));
						LoadSample(_load);
					}
				}
			}
		}

		private void SamplesaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
			//if _loadedgate is somehow not set, force Save As instead
			if (_loadedsample == null) {
				SamplesaveAsToolStripMenuItem.PerformClick();
				return;
			}
			//write contents direct to file without prompting save dialog
			var _save = SampleBuildSave();
			File.WriteAllText(_loadedsample, JsonConvert.SerializeObject(_save, Formatting.Indented));
			SaveSample(true);
			lblSampleEditor.Text = $"Sample Editor - {_loadedsample}";
		}

		private void SamplesaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///

		private void btnSampleDelete_Click(object sender, EventArgs e) => _samplelist.RemoveAt(sampleList.CurrentRow.Index);
		private void btnSampleAdd_Click(object sender, EventArgs e)
		{

		}

		private void btnSampPanelNew_Click(object sender, EventArgs e) => SamplenewToolStripMenuItem.PerformClick();
		private void btnSampPanelOpen_Click(object sender, EventArgs e) => SampleopenToolStripMenuItem.PerformClick();

		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void InitializeSampleStuff()
        {
			_samplelist.CollectionChanged += _samplelist_CollectionChanged;
			///Customize Lvl list a bit
			sampleList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 12, GraphicsUnit.Pixel)
			};
		}
		
		public void LoadSample(dynamic _load)
        {
			//if Sample Editor is hidden, show it when selecting a gate
			if (panelSample.Visible == false)
				sampleEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Gate or not
			if (_load.ContainsKey("items") && _load["items"][0]["obj_type"] != "Sample") {
				MessageBox.Show("This does not appear to be a sample file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedsampletemp);
			_loadedsample = _loadedsampletemp;
			//set some visual elements
			lblSampleEditor.Text = $"Sample Editor - {loadedsample}";

			///Clear form elements so new data can load
			_samplelist.Clear();
			///load lvls associated with this master
			foreach (dynamic _samp in _load["items"]) {
				_samplelist.Add(new SampleData() {
					obj_name = ((string)_samp["obj_name"]).Replace(".samp", ""),
					path = _samp["path"],
					volume = _samp["volume"],
					pitch = _samp["pitch"],
					pan = _samp["pan"],
					offset = _samp["offset"]
				});
			}

			///set save flag (samples just loaded, has no changes)
			SaveSample(true);
		}

		public void SaveSample(bool save)
		{
			_savesample = save;
			if (!save) {
				if (!lblSampleEditor.Text.Contains("(unsaved)"))
					lblSampleEditor.Text += " (unsaved)";
				btnSaveSample.Location = new Point(lblSampleEditor.Location.X + lblSampleEditor.Size.Width, btnSaveSample.Location.Y);
				btnSaveSample.Enabled = true;
				lblSampleEditor.BackColor = Color.Maroon;
			}
			else {
				lblSampleEditor.Text = lblSampleEditor.Text.Replace(" (unsaved)", "");
				btnSaveSample.Location = new Point(lblSampleEditor.Location.X + lblSampleEditor.Size.Width, btnSaveSample.Location.Y);
				btnSaveSample.Enabled = false;
				lblSampleEditor.BackColor = Color.FromArgb(40, 40, 40);
			}
		}

		public JObject SampleBuildSave()
        {
			JObject _save = new JObject();
			JArray _items = new JArray();
			foreach (SampleData _sample in _samplelist) {
				JObject _samp = new JObject {
					{ "obj_type", "Sample"},
					{ "obj_name", _sample.obj_name + ".samp" },
					{ "mode", "kSampleOneOff" },
					{ "path", _sample.path },
					{ "volume", _sample.volume },
					{ "pitch", _sample.pitch },
					{ "pan", _sample.pan },
					{ "offset", _sample.offset },
					{ "channel_group", "" }
				};
				_items.Add(_samp);
            }
			_save.Add("items", _items);
			return _save;
		}

		public void SampleEditorVisible(bool visible)
		{
			if (workingfolder != null) {
				foreach (Control c in panelSample.Controls)
					c.Visible = visible;
				btnSampPanelNew.Visible = !visible;
				btnSampPanelOpen.Visible = !visible;
			}
		}

		public uint Hash32(string s)
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