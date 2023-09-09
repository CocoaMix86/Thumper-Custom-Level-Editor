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
using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Thumper_Custom_Level_Editor
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

		private void sampleList_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;
			//remove event handlers so they don't trigger when the values change
			txtSampPath.TextChanged -= txtSampPath_TextChanged;
			//update values with selected data
			txtSampPath.Text = _samplelist[e.RowIndex].path;
			btnSampEditorPlaySamp.Enabled = true;
			//re-add the event handlers
			txtSampPath.TextChanged += txtSampPath_TextChanged;
		}

		private void sampleList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			//When a cell is finished editing, resave the row to _samplelist
			int _index = e.RowIndex;
			switch (e.ColumnIndex) {
				case 0:
					_samplelist[_index].obj_name = (string)sampleList.Rows[_index].Cells[0].Value;
					break;
				case 1:
					_samplelist[_index].volume = (decimal)sampleList.Rows[_index].Cells[1].Value;
					break;
				case 2:
					_samplelist[_index].pitch = (decimal)sampleList.Rows[_index].Cells[2].Value;
					break;
				case 3:
					_samplelist[_index].pan = (decimal)sampleList.Rows[_index].Cells[3].Value;
					break;
				case 4:
					_samplelist[_index].offset = (decimal)sampleList.Rows[_index].Cells[4].Value;
					break;
				case 5:
					_samplelist[_index].channel_group = sampleList.Rows[_index].Cells[5].Value.ToString();
					break;
			}
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
			//sort the list alphabetically
			_samplelist = new ObservableCollection<SampleData>(_samplelist.OrderBy(x => x.obj_name).ToList());
			_samplelist.CollectionChanged += _samplelist_CollectionChanged;
			//clear dgv
			sampleList.RowCount = 0;
			//repopulate dgv from list
			sampleList.RowEnter -= sampleList_RowEnter;
			foreach (SampleData _samp in _samplelist) {
				sampleList.Rows.Add(new object[] { _samp.obj_name, _samp.volume, _samp.pitch, _samp.pan, _samp.offset, _samp.channel_group });
			}
			sampleList.RowEnter += sampleList_RowEnter;
			//enable certain buttons if there are enough items for them
			btnSampleDelete.Enabled = _samplelist.Count > 0;

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
			using (var sfd = new SaveFileDialog()) {
				//filter .txt only
				sfd.Filter = "Thumper Sample File (*.txt)|*.txt";
				sfd.FilterIndex = 1;
				sfd.InitialDirectory = workingfolder;
				if (sfd.ShowDialog() == DialogResult.OK) {
					//separate path and filename
					string storePath = Path.GetDirectoryName(sfd.FileName);
					string tempFileName = Path.GetFileName(sfd.FileName);
					//check if user input "gate_", and deny save if so
					if (Path.GetFileName(sfd.FileName).Contains("samp_")) {
						MessageBox.Show("File not saved. Do not include 'samp_' in your file name.", "File not saved");
						return;
					}
					//get contents to save
					var _save = SampleBuildSave();
					//serialize JSON object to a string, and write it to the file
					sfd.FileName = $@"{storePath}\samp_{tempFileName}";
					File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(_save, Formatting.Indented));
					//set a few visual elements to show what file is being worked on
					workingfolder = Path.GetDirectoryName(sfd.FileName);
					_loadedsample = sfd.FileName;
					lblSampleEditor.Text = $"Sample Editor - {_loadedsample}";
					//set save flag
					SaveSample(true);
				}
			}
		}

		///Detect dragon-and-drop of files and then load them to Sample files
		private void sampleList_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (File.Exists(data[0])) {
					e.Effect = DragDropEffects.Copy;
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}
		///Detect dragon-and-drop of files and then load them to Sample files
		private void sampleList_DragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
			string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string dir in data) {
				if (File.Exists(dir) && Path.GetExtension(dir) == ".fsb")
					FSBtoSAMP(dir);
				else
					MessageBox.Show($@"{dir} is not an .fsb file. It was {Path.GetExtension(dir)}. File not added to sample list.", "Sample load error");
			}
		}

		#endregion

		#region Buttons
		///         ///
		/// BUTTONS ///
		///         ///
		//add and remove sample entries
		private void btnSampleDelete_Click(object sender, EventArgs e) => _samplelist.RemoveAt(sampleList.CurrentRow.Index);
		private void btnSampleAdd_Click(object sender, EventArgs e)
		{
			SampleData newsample = new SampleData { 
				obj_name = "new", volume = 1, pitch = 1, pan = 0, offset = 0, path = "samples/levels/custom/new.wav", channel_group = "sequin.ch"
			};
			_samplelist.Add(newsample);
			int _index = _samplelist.IndexOf(newsample);
			sampleList.Rows[_index].Cells[0].Selected = true;
		}
		//open or new sample file
		private void btnSampPanelNew_Click(object sender, EventArgs e) => SamplenewToolStripMenuItem.PerformClick();
		private void btnSampPanelOpen_Click(object sender, EventArgs e) => SampleopenToolStripMenuItem.PerformClick();

		//Opens an .FSB audio file, hashes the name, and adds it to the loaded SAMP_ file
		private void FSBtoSamp_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog()) {
				ofd.Filter = "FSB Audio File (*.fsb)|*.fsb";
				ofd.Title = "Load a FSB Audio file";
				if (ofd.ShowDialog() == DialogResult.OK) {
					FSBtoSAMP(ofd.FileName);
				}
			}
		}

		private void btnSampEditorPlaySamp_Click(object sender, EventArgs e)
		{
			var _samp = _samplelist[sampleList.CurrentRow.Index];
			string _filetype = "";

			if (!File.Exists($@"temp\{_samp.obj_name}.ogg") && !File.Exists($@"temp\{_samp.obj_name}.wav")) {
				var _result = PCtoOGG(_samp);
				if (_result == null)
					return;
			}
			//check extension of the sample to play
			if (File.Exists($@"temp\{_samp.obj_name}.ogg"))
				_filetype = "ogg";
			if (File.Exists($@"temp\{_samp.obj_name}.wav"))
				_filetype = "wav";

			AudioPlaybackEngine.Instance.PlaySound(new CachedSound($@"temp\{_samp.obj_name}.{_filetype}"));
			//VorbisWaveReader vorbis = new VorbisWaveReader($@"temp\{_samp.obj_name}.{_filetype}");
			//WaveOut oggPlayer = WaveOutInit(vorbis);
			//oggPlayer.Play();
		}
		#endregion

		#region Methods
		///         ///
		/// METHODS ///
		///         ///

		public void InitializeSampleStuff()
        {
			///Customize Lvl list a bit
			sampleList.RowsDefaultCellStyle = new DataGridViewCellStyle() {
				ForeColor = Color.White,
				Font = new Font("Arial", 12, GraphicsUnit.Pixel)
			};
			sampleList.Columns[0].ValueType = typeof(string);
			sampleList.Columns[1].ValueType = typeof(decimal);
			sampleList.Columns[2].ValueType = typeof(decimal);
			sampleList.Columns[3].ValueType = typeof(decimal);
			sampleList.Columns[4].ValueType = typeof(decimal);
			sampleList.Columns[5].ValueType = typeof(string);
		}
		
		public void LoadSample(dynamic _load)
        {
			//if Sample Editor is hidden, show it when selecting a gate
			if (panelSample.Visible == false)
				sampleEditorToolStripMenuItem.PerformClick();
			//detect if file is actually Gate or not
			if (!_load.ContainsKey("items")) {
				MessageBox.Show("This does not appear to be a sample file!");
				return;
			}
			//if the check above succeeds, then set the _loadedlvl to the string temp saved from ofd.filename
			workingfolder = Path.GetDirectoryName(_loadedsampletemp);
			_loadedsample = _loadedsampletemp;
			//set some visual elements
			lblSampleEditor.Text = $"Sample Editor - {Path.GetFileNameWithoutExtension(loadedsample)}";

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
					offset = _samp["offset"],
					channel_group = _samp["channel_group"]
				});
			}
			_samplelist.CollectionChanged += _samplelist_CollectionChanged;
			_samplelist_CollectionChanged(null, null);

			///set save flag (samples just loaded, has no changes)
			SaveSample(true);
		}

		public void SaveSample(bool save)
		{
			//make the beeble emote
			pictureBox1_Click(null, null);

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
                    { "channel_group", _sample.channel_group }
                };
				_items.Add(_samp);
            }
			_save.Add("items", _items);
			return _save;
		}

		public void SampleEditorVisible(bool visible)
		{
			panelSample.Visible = visible;
		}

		private void FSBtoSAMP(string filepath)
		{
			string _filename;
			byte[] _bytes;
			byte[] _header = new byte[] { 0x0d, 0x00, 0x00, 0x00 };
			string _hashedname = "";

			//save relevant data of the chosen file
			_filename = Path.GetFileNameWithoutExtension(filepath);
			_bytes = File.ReadAllBytes(filepath);
			//get the hash of the FSB filename. This will be used to name the final .PC file
			byte[] hashbytes = BitConverter.GetBytes(Hash32($"Asamples/levels/custom/{_filename}.wav"));
			Array.Reverse(hashbytes);
			foreach (byte b in hashbytes)
				_hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
			//if the hashed name starts with a '0', remove it
			if (_hashedname[0] == '0')
				_hashedname = _hashedname.Substring(1);

			///With hashing complete, can now save the file to a .PC
			//if the `extras` folder doesn't exist, make it
			Directory.CreateDirectory($@"{workingfolder}\extras");
			//write header and bytes of fsb to new file
			using (FileStream f = File.Open($@"{workingfolder}\extras\{_hashedname}.pc", FileMode.Create, FileAccess.Write, FileShare.None)) {
				f.Write(_header, 0, _header.Length);
				f.Write(_bytes, 0, _bytes.Length);
			}

			//Add new sample entry to the loaded samp_ file
			SampleData newsample = new SampleData {
				obj_name = $"{_filename}",
				volume = 1,
				pitch = 1,
				pan = 0,
				offset = 0,
				path = $"samples/levels/custom/{_filename}.wav",
				channel_group = "sequin.ch"
			};
			_samplelist.Add(newsample);
			int _index = _samplelist.IndexOf(newsample);
			sampleList.Rows[_index].Cells[0].Selected = true;
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

		public string PCtoOGG(SampleData _samp)
        {
			//check if the gamedir has been set so the method can find the .pc files
			if (Properties.Settings.Default.game_dir == "none") {
				Read_Config();
            }

			byte[] _bytes;
			//get the hash of this filename. This will be used to locate the sample's .PC file
			string _hashedname = "";
			byte[] hashbytes = BitConverter.GetBytes(Hash32($"A{_samp.path}"));
			Array.Reverse(hashbytes);
			foreach (byte b in hashbytes)
				_hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
			//if the hashed name starts with a '0', remove it
			if (_hashedname[0] == '0')
				_hashedname = _hashedname.Substring(1);

			//check if sample is custom or not. This changes where we load audio from
			if (_samp.path.Contains("custom")) {
				//attempt to locate file. But error and return safely if nothing found
				try {
					//read the .pc file as bytes, and skip the first 4 header bytes
					_bytes = File.ReadAllBytes($@"{workingfolder}\extras\{_hashedname}.pc");
				} catch {
					MessageBox.Show($@"Unable to locate file {workingfolder}\extras\{_hashedname}.pc to play sample. Is the custom audio file in the extras folder?");
					return null;
                }
			}
			else {
				//attempt to locate file. But error and return safely if nothing found
				try {
					//read the .pc file as bytes, and skip the first 4 header bytes
					_bytes = File.ReadAllBytes($@"{Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc");
				} catch {
					MessageBox.Show($@"Unable to locate file {Properties.Settings.Default.game_dir}\{_hashedname}.pc to play sample. If you need to change your Game Directory, go to the the Help menu.");
					return null;
				}
			}
			_bytes = _bytes.Skip(4).ToArray();

			// credit to https://github.com/SamboyCoding/Fmod5Sharp
			FmodSoundBank bank = FsbLoader.LoadFsbFromByteArray(_bytes);
			List<FmodSample> samples = bank.Samples;
			samples[0].RebuildAsStandardFileFormat(out var dataBytes, out var fileExtension);

			File.WriteAllBytes($@"temp\{_samp.obj_name}.{fileExtension}", dataBytes);
			return fileExtension;
		}

		/// These are specifically for audio playback. Don't touch them
		/// IDK how they work
		/// https://stackoverflow.com/questions/74605784/using-vorbis-and-naudio-to-play-ogg-files-in-c-sharp
		private WaveOut WaveOutInit(IWaveProvider reader)
		{
			var waveOut = new WaveOut();
			waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
			waveOut.Init(reader);
			return waveOut;
		}
		private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
		{
			//WaveOutReset(oggPlayer, vorbis);
			//btnSampEditorPlaySamp.Enabled = true;
		}
		private void WaveOutReset(WaveOut? player, VorbisWaveReader? reader)
		{
			if (player != null) {
				player.PlaybackStopped -= WaveOut_PlaybackStopped;
				player.Dispose();
			}
			reader?.Dispose();
		}

		#endregion
	}
}