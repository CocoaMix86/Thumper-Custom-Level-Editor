using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using NAudio.Vorbis;
using NAudio.Wave;
using System.Linq;

namespace Thumper_Custom_Level_Editor
{
	public partial class FormLeafEditor
	{
		Multimedia.Timer _playbacktimer = new Multimedia.Timer();
		List<List<CachedSound>> vorbis;
		bool _playing = false;
		int _playbackbeat;
		int _sequence;

		CachedSound ring;
		CachedSound ring_approach;
		CachedSound bar_approach;
		CachedSound bar;
		CachedSound spikes;
		CachedSound thump_approach;
		CachedSound thump;
		CachedSound turn_approachR;
		CachedSound turn_approachL;
		CachedSound turn;

		private void InitializeSounds()
        {
			/*ring = new CachedSound(@"temp\coin_collect.ogg");
			ring_approach = new CachedSound(@"temp\ducker_ring_approach.ogg");
			bar_approach = new CachedSound(@"temp\grindable_birth2.ogg");
			bar = new CachedSound(@"temp\hammer_two_handed_hit.ogg");
			spikes = new CachedSound(@"temp\high_jump.ogg");
			thump_approach = new CachedSound(@"temp\thump_birth1.ogg");
			thump = new CachedSound(@"temp\thump1b.ogg");
			turn_approachR = new CachedSound(@"temp\turn_birth.ogg");
			turn_approachL = new CachedSound(@"temp\turn_birth_lft.ogg");
			turn = new CachedSound(@"temp\turn_hit_perfect2.ogg");*/
		}

		private void btnTrackPlayback_Click(object sender, EventArgs e)
		{
			//if the playback is active, stop it. Otherwise, continue below
			if (_playing) {
				_playing = false;
				_playbacktimer.Stop();
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
							vorbis[_sequence].Add(new CachedSound(@"temp\thump1b.ogg"));
							vorbis[_sequence - 8].Add(new CachedSound(@"temp\thump_birth1.ogg"));
						}
						_sequence++;
					}
				}
				//Takes care of Turns
				if (dgvr.HeaderCell.Value.ToString().Contains("(turn)")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(new CachedSound(@"temp\turn_hit_perfect2.ogg"));
							vorbis[_sequence - 8].Add(new CachedSound(@"temp\turn_birth.ogg"));
						}
						_sequence++;
					}
				}
				//Takes care of Bars
				if (dgvr.HeaderCell.Value.ToString().Contains("BARS")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(new CachedSound(@"temp\hammer_two_handed_hit.ogg"));
							vorbis[_sequence - 8].Add(new CachedSound(@"temp\grindable_birth2.ogg"));
						}
						_sequence++;
					}
				}
				//Takes care of Rings
				if (dgvr.HeaderCell.Value.ToString().Contains("RING")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							vorbis[_sequence].Add(new CachedSound(@"temp\coin_collect.ogg"));
							vorbis[_sequence].Add(new CachedSound(@"temp\hammer_two_handed_hit.ogg"));
							vorbis[_sequence - 8].Add(new CachedSound(@"temp\ducker_ring_approach.ogg"));
						}
						_sequence++;
					}
				}

				//Takes care of PLAY SAMPLE
				if (dgvr.HeaderCell.Value.ToString().Contains("PLAY SAMPLE")) {
					_sequence = 8;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null) {
							//if the audio file doesn't exist in the temp folder, we need to extract it first
							if (!File.Exists($@"temp\{_tracks[dgvr.Index].obj_name.Replace(".samp", "")}.ogg")) {
								//using LINQ, I can enumerate over the sample list, locate the obj_name, and then pull out the entire object from that!
								var _samplocate = _lvlsamples.First(item => item.obj_name == _tracks[dgvr.Index].obj_name.Replace(".samp", ""));
								PCtoOGG(_samplocate);
							}
							vorbis[_sequence].Add(new CachedSound($@"temp\{_tracks[dgvr.Index].obj_name.Replace(".samp", "")}.ogg"));
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
			_playbacktimer.Period = (int)Math.Round(1000 * ((float)60 / (float)NUD_ConfigBPM.Value), MidpointRounding.AwayFromZero);
			_playbacktimer.Start();
		}

		private void _playbacktimer_Tick(object source, EventArgs e)
		{
			foreach (var _sample in vorbis[_playbackbeat]) {
				AudioPlaybackEngine.Instance.PlaySound(_sample);
			}
			try {
				trackEditor.ClearSelection();
				trackEditor.Columns[_playbackbeat - 8].Selected = true;
			} catch { }

			_playbackbeat++;
			if (_playbackbeat >= vorbis.Count) {
				_playbacktimer.Stop();
				_playing = false;
				btnTrackPlayback.ForeColor = Color.Green;
				btnTrackPlayback.Image = Properties.Resources.icon_play;
				trackEditor.SelectionMode = DataGridViewSelectionMode.CellSelect;
			}
		}
	}
}