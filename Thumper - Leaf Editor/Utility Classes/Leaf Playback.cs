using System;
using System.IO;
using System.Windows.Forms;
using System.Media;

namespace Thumper___Leaf_Editor
{
	public partial class FormLeafEditor
	{
		Tuple<int, int>[] _toplay;
		int _playbackbeat;
		int _sequence;

		private void btnTrackPlayback_Click(object sender, EventArgs e)
		{
			_toplay = new Tuple<int, int>[(int)numericUpDown_LeafLength.Value];
			for (int x = 0; x < _toplay.Length; x++)
				_toplay[x] = new Tuple<int, int>(0, 0);

			foreach (DataGridViewRow dgvr in trackEditor.Rows)
			{
				if (dgvr.HeaderCell.Value.ToString().Contains("THUMPS"))
				{
					_sequence = 0;
					foreach (DataGridViewCell dgvc in dgvr.Cells) {
						if (dgvc.Value != null)
							_toplay[_sequence] = new Tuple<int, int>(150, 50);
						//else
							//_toplay[_sequence] = new Tuple<int, int>(37, 1);
						_sequence++;
					}
				}
				if (dgvr.HeaderCell.Value.ToString().Contains("(turn)"))
				{
					_sequence = 0;
					foreach (DataGridViewCell dgvc in dgvr.Cells)
					{
						decimal _s = dgvc.Value == null ? 0 : decimal.Parse(dgvc.Value.ToString());
						if (Math.Abs(_s) >= 15)
							_toplay[_sequence] = new Tuple<int, int>(400, 50);
						//else
							//_toplay[_sequence] = new Tuple<int, int>(37, 1);
						_sequence++;
					}
				}
			}

			_playbackbeat = 0;
			timer1.Interval = (int)(1000 * (60 / NUD_ConfigBPM.Value));
			timer1.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{

			//Console.Beep(_toplay[_playbackbeat].Item1, _toplay[_playbackbeat].Item2);
			Beep.BeepBeep(100, _toplay[_playbackbeat].Item1, _toplay[_playbackbeat].Item2);
			_playbackbeat++;
			if (_playbackbeat >= _toplay.Length)
				timer1.Enabled = false;
		}
	}


	/// CREDIT: https://social.msdn.microsoft.com/Forums/vstudio/en-US/18fe83f0-5658-4bcf-bafc-2e02e187eb80/beep-beep
	public class Beep
	{
		public static void BeepBeep(int Amplitude, int Frequency, int Duration)
		{
			double A = ((Amplitude * (System.Math.Pow(2, 15))) / 1000) - 1;
			double DeltaFT = 2 * Math.PI * Frequency / 44100.0;

			int Samples = 441 * Duration / 10;
			int Bytes = Samples * 4;
			int[] Hdr = { 0X46464952, 36 + Bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, Bytes };
			using (MemoryStream MS = new MemoryStream(44 + Bytes))
			{
				using (BinaryWriter BW = new BinaryWriter(MS))
				{
					for (int I = 0; I < Hdr.Length; I++)
					{
						BW.Write(Hdr[I]);
					}
					for (int T = 0; T < Samples; T++)
					{
						short Sample = System.Convert.ToInt16(A * Math.Sin(DeltaFT * T));
						BW.Write(Sample);
						BW.Write(Sample);
					}
					BW.Flush();
					MS.Seek(0, SeekOrigin.Begin);
					using (SoundPlayer SP = new SoundPlayer(MS))
					{
						SP.PlaySync();
					}
				}
			}
		}
	}
}