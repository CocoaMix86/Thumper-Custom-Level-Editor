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
			_playbackbeat++;
			if (_playbackbeat >= vorbis.Count) {
				_playbacktimer.Stop();
				_playing = false;
				btnTrackPlayback.ForeColor = Color.Green;
				btnTrackPlayback.Image = Properties.Resources.icon_play;
			}
		}
			/*
			Tuple<int, int>[] _toplay;
			int _playbackbeat;
			int _sequence;
			bool turning = false;
			bool playing = false;

			Multimedia.Timer timer = new Multimedia.Timer();

			private void btnTrackPlayback_Click(object sender, EventArgs e)
			{
				//if the playback is active, stop it. Otherwise, continue below
				if (playing) {
					playing = false;
					timer.Stop();
					btnTrackPlayback.ForeColor = Color.Green;
					return;
				}

				//initialize a new array of the sounds to play
				_toplay = new Tuple<int, int>[(int)numericUpDown_LeafLength.Value];
				for (int x = 0; x < _toplay.Length; x++)
					_toplay[x] = new Tuple<int, int>(0, 0);

				//iterate over each row in the lead to find the tracks with the important sound-making objects
				foreach (DataGridViewRow dgvr in trackEditor.Rows)
				{
					//Takes care of Thumps
					if (dgvr.HeaderCell.Value.ToString().Contains("THUMPS"))
					{
						_sequence = 0;
						foreach (DataGridViewCell dgvc in dgvr.Cells) {
							if (dgvc.Value != null)
								_toplay[_sequence] = new Tuple<int, int>(150, 60);
							_sequence++;
						}
					}
					//Takes care of turns
					if (dgvr.HeaderCell.Value.ToString().Contains("(turn)"))
					{
						_sequence = 0;
						foreach (DataGridViewCell dgvc in dgvr.Cells)
						{
							decimal _s = dgvc.Value == null ? 0 : decimal.Parse(dgvc.Value.ToString());
							//only makes a sound if the turn is 15deg or larger
							if (Math.Abs(_s) >= 15) {
								//if the previous beat was already a turn, don't make another beep.
								if (turning == false) {
									_toplay[_sequence] = new Tuple<int, int>(400, 60);
									turning = true;
								}
							}
							else
								turning = false;
							_sequence++;
						}
					}
					//Takes care of bars and rings
					if (dgvr.HeaderCell.Value.ToString().Contains("BARS") || dgvr.HeaderCell.Value.ToString().Contains("RINGS")) {
						_sequence = 0;
						foreach (DataGridViewCell dgvc in dgvr.Cells) {
							if (dgvc.Value != null)
								_toplay[_sequence] = new Tuple<int, int>(500, 60);
							_sequence++;
						}
					}
				}
				//enable the timer and start playback
				playing = true;
				btnTrackPlayback.ForeColor = Color.Red;
				_playbackbeat = 0;
				//the speed of the timer is reliant on the level's BPM
				timer.Period = (int)(1000 * (60 / NUD_ConfigBPM.Value));
				timer.Start();
			}

			private void timer_Tick(object source, EventArgs e)
			{
				//Task.Run(() => BeepBeep(_toplay[_playbackbeat].Item1, _toplay[_playbackbeat].Item2)).ConfigureAwait(false);
				BeepBeep(_toplay[_playbackbeat].Item1, _toplay[_playbackbeat].Item2);
				_playbackbeat++;
				//once playback reaches the end of the leaf, stop it
				if (_playbackbeat >= _toplay.Length - 1) {
					timer.Stop();
					playing = false;
					btnTrackPlayback.ForeColor = Color.Green;
				}
			}

			/// CREDIT: https://social.msdn.microsoft.com/Forums/vstudio/en-US/18fe83f0-5658-4bcf-bafc-2e02e187eb80/beep-beep
			private static void BeepBeep(int Frequency, int Duration)
			{
				double A = ((100 * Math.Pow(2, 15)) / 1000) - 1;
				double DeltaFT = 2 * Math.PI * Frequency / 44100.0;

				int Samples = 441 * Duration / 10;
				int Bytes = Samples * 4;
				int[] Hdr = { 0X46464952, 36 + Bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, Bytes };
				using (MemoryStream MS = new MemoryStream(44 + Bytes)) {
					using (BinaryWriter BW = new BinaryWriter(MS)) {
						for (int I = 0; I < Hdr.Length; I++) {
							BW.Write(Hdr[I]);
						}
						for (int T = 0; T < Samples; T++) {
							short Sample = System.Convert.ToInt16(A * Math.Sin(DeltaFT * T));
							BW.Write(Sample);
							BW.Write(Sample);
						}
						BW.Flush();
						MS.Seek(0, SeekOrigin.Begin);
						using (SoundPlayer SP = new SoundPlayer(MS)) {
							SP.Play();
						}
					}
				}
			}
			*/
		}
}