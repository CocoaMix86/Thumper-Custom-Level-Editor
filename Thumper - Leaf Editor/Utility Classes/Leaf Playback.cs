using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
	{
		AccurateTimer mTimer1;
		List<List<CachedSound>> vorbis;
		bool _playing = false;
		int _playbackbeat;
		int _sequence;

		CachedSound ring;
		CachedSound ring_approach;
		CachedSound bar_approach;
		CachedSound bar;
		CachedSound spikes;
		CachedSound mushroom;
		CachedSound thump_approach;
		CachedSound thump;
		CachedSound turn_approachR;
		CachedSound turn_approachL;
		CachedSound turn;
		CachedSound turn_long;

		private void InitializeSounds()
        {
			ring = new CachedSound(@"temp\coin_collect.ogg");
			ring_approach = new CachedSound(@"temp\ducker_ring_approach.ogg");
			bar_approach = new CachedSound(@"temp\grindable_birth2.ogg");
			bar = new CachedSound(@"temp\hammer_two_handed_hit.ogg");
			spikes = new CachedSound(@"temp\high_jump.ogg");
			mushroom = new CachedSound(@"temp\jumper_approach.ogg");
			thump_approach = new CachedSound(@"temp\thump_birth1.ogg");
			thump = new CachedSound(@"temp\thump1b.ogg");
			turn_approachR = new CachedSound(@"temp\turn_birth.ogg");
			turn_approachL = new CachedSound(@"temp\turn_birth_lft.ogg");
			turn = new CachedSound(@"temp\turn_hit_perfect2.ogg");
			turn_long = new CachedSound(@"temp\turn_long_lft.ogg");
		}

		private void btnTrackPlayback_Click(object sender, EventArgs e)
		{
			//if the playback is active, stop it. Otherwise, continue below
			if (_playing) {
				_playing = false;
				btnTrackPlayback.ForeColor = Color.Green;
				btnTrackPlayback.Image = Properties.Resources.icon_play;
				trackEditor.SelectionMode = DataGridViewSelectionMode.CellSelect;
				return;
			}
			//check if master file is loaded
			if (_loadedmaster == null) {
				MessageBox.Show("A master file needs to be loaded to get the BPM", "Cannot start playback");
				return;
            }

			//make sure the sample list is up to date
			LvlReloadSamples();
			//for each beat in the leaf, fill the list with new lists to hold playable data
			vorbis = new List<List<CachedSound>>((int)numericUpDown_LeafLength.Value + 8);
			for (int x = 0; x < numericUpDown_LeafLength.Value + 8; x++) {
				vorbis.Add(new List<CachedSound>());
			}

			//iterate over each row in the lead to find the tracks with the important sound-making objects
			foreach (DataGridViewRow dgvr in trackEditor.Rows) {

				//Takes care of Thumps
				if (dgvr.HeaderCell.Value.ToString().Contains("THUMPS")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(thump);
							//vorbis[_sequence - 8].Add(thump_approach);
						}
						_sequence++;
					}
				}
				//Takes care of Turns
				else if (dgvr.HeaderCell.Value.ToString().Contains("(turn)")) {
					_sequence = 8;
					bool _turning = false;
					bool _played = false;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						//check if NOT turning. This marks the first beat of the turn
						if (dgvc.Value != null && Math.Abs(decimal.Parse(dgvc.Value.ToString())) >= 15 && !_turning) {
							_played = false;
							_turning = true;
							vorbis[_sequence].Add(turn);
						}
						//check if still turning
						else if (dgvc.Value != null && Math.Abs(decimal.Parse(dgvc.Value.ToString())) >= 15) {
							//only need the turn call sound one time
							if (!_played) vorbis[_sequence - 9].Add(turn_long);
							_played = true;
						}
						//if no longer turning
						else if (_turning) {
							//check if long turn played. If not, add regular turn sound.
							if (!_played) { //vorbis[_sequence - 9].Add(turn_approachR);
											}
							_turning = false;
							_played = false;
						}
						_sequence++;
					}
				}
				//Takes care of Bars
				else if (dgvr.HeaderCell.Value.ToString().Contains("BARS")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(bar);
							vorbis[_sequence - 8].Add(bar_approach);
						}
						_sequence++;
					}
				}
				//Takes care of Rings
				else if (dgvr.HeaderCell.Value.ToString().Contains("RING")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(ring);
							vorbis[_sequence].Add(bar);
							vorbis[_sequence - 8].Add(ring_approach);
						}
						_sequence++;
					}
				}
				//Takes care of Spikes
				else if (dgvr.HeaderCell.Value.ToString().Contains("JUMPS")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null && dgvr.HeaderCell.Value.ToString().Contains("spike")) {
							vorbis[_sequence - 8].Add(spikes);
						}
						if (dgvc.Value != null && dgvr.HeaderCell.Value.ToString().Contains("mushroom")) {
							vorbis[_sequence - 8].Add(mushroom);
						}
						_sequence++;
					}
				}

				//Takes care of PLAY SAMPLE
				else if (dgvr.HeaderCell.Value.ToString().Contains("PLAY SAMPLE")) {
					_sequence = 8;
					string _samplename = $@"temp\{_tracks[dgvr.Index].obj_name.Replace(".samp", "")}";
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							//if the audio file doesn't exist in the temp folder, we need to extract it first
							if (!File.Exists($@"{_samplename}.ogg") && !File.Exists($@"{_samplename}.wav")) {
								//using LINQ, I can enumerate over the sample list, locate the obj_name, and then pull out the entire object from that!
								var _samplocate = _lvlsamples.First(item => item.obj_name == _tracks[dgvr.Index].obj_name.Replace(".samp", ""));
								PCtoOGG(_samplocate);
							}

							if (File.Exists($@"{_samplename}.ogg"))
								vorbis[_sequence].Add(new CachedSound($@"{_samplename}.ogg"));
							if (File.Exists($@"{_samplename}.wav"))
								vorbis[_sequence].Add(new CachedSound($@"{_samplename}.wav"));
						}
						_sequence++;
					}
				}
			}

			//set this to allow columns to be highlighted as the player moves
			trackEditor.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
			//enable the timer and start playback
			_playing = true;
			btnTrackPlayback.ForeColor = Color.Red;
			btnTrackPlayback.Image = Properties.Resources.icon_pause16;
			_playbackbeat = 0;
			//the speed of the timer is reliant on the level's BPM
			int _period = (int)Math.Round(60000f / (float)NUD_ConfigBPM.Value, MidpointRounding.AwayFromZero);
			mTimer1 = new AccurateTimer(this, new Action(_playbacktimer_Tick), _period);
		}

		private void _playbacktimer_Tick()
		{
			foreach (var _sample in vorbis[_playbackbeat]) {
				AudioPlaybackEngine.Instance.PlaySound(_sample);
			}
			try {
				trackEditor.ClearSelection();
				trackEditor.Columns[_playbackbeat - 8].Selected = true;
			}
			catch { }

			_playbackbeat++;
			if (_playbackbeat >= vorbis.Count) {
				mTimer1.Stop();
				_playing = false;
				btnTrackPlayback.ForeColor = Color.Green;
				btnTrackPlayback.Image = Properties.Resources.icon_play;
				trackEditor.SelectionMode = DataGridViewSelectionMode.CellSelect;
			}
		}
	}
}