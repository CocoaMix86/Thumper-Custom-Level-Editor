using System;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class Interpolator : Form
	{
		public Interpolator()
		{
			InitializeComponent();
		}

		private void Interpolator_Load(object sender, EventArgs e)
		{

		}

		///
		/// FORM EVENT HANDLERS
		/// Mainly for filtering inputs
		///
		private void txtConstant_bStart_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}

		private void txtConstant_bEnd_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}

		private void txtNBeats_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}

		private void txtSmoothTurn_bStart_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}

		private void txtSmoothTurn_bEnd_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}

		private void txtSmooth_angleStart_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
				(e.KeyChar != '.' && e.KeyChar != '-')) {
				e.Handled = true;
			}
			// only allow one decimal point
			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) {
				e.Handled = true;
			}
		}

		private void txtSmooth_angleTarget_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
				(e.KeyChar != '.' && e.KeyChar != '-')) {
				e.Handled = true;
			}
			// only allow one decimal point
			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) {
				e.Handled = true;
			}
		}

		private void txtOffset_beat_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-')) {
				e.Handled = true;
			}
		}

		///
		///TAB 1 - Turn Constant
		///
		private void btnConstant_output_Click(object sender, EventArgs e)
		{
			if (txtConstant_bStart.Text == "" || txtConstant_bEnd.Text == "" || txtConstant_angle.Text == "")
				return;

			bool _skipNo = radioNone.Checked;
			bool _skipNthBeat = radionth.Checked;
			bool _skipNRow = radionrow.Checked;
			int _nbeats = (txtNBeats.Text == "") ? 1 : int.Parse(txtNBeats.Text);
			if (_nbeats == 0) _nbeats = 1;

			int _start = int.Parse(txtConstant_bStart.Text);
			int _end = int.Parse(txtConstant_bEnd.Text);
			string _pitch = txtConstant_angle.Text;
			string _output = " ";

			for (int x = 1; _start <= _end; _start++) {
				if (!_skipNo) {
					//check if beat should be skipped. If skipNthbeat is on, and beat%N = 0, skip it
					//if SkipNRow is on, write the beat
					if ((_skipNthBeat && x % _nbeats != 0) || _skipNRow)
						_output += _start + ":" + _pitch + ",";
					//Also if SkipNRow is on, add N beats to the loop to skip that many
					if (_skipNRow)
						_start += _nbeats;
					x++;
				}
				//if no skip is specified, write the beat normally
				else
					_output += _start + ":" + _pitch + ",";
			}

			_output = _output.Remove(_output.Length - 1);
			richTextBox1.Text = _output;
		}


		///
		///TAB 2 - Smooth Turns
		///
		private void btnSmooth_output_Click(object sender, EventArgs e)
		{
			if (txtSmoothTurn_bEnd.Text == "" || txtSmoothTurn_bStart.Text == "" || txtSmooth_angleStart.Text == "" || txtSmooth_angleTarget.Text == "")
				return;

			int _beatStart = int.Parse(txtSmoothTurn_bStart.Text);
			int _beatEnd = int.Parse(txtSmoothTurn_bEnd.Text);
			float _beats = 1 + _beatEnd - _beatStart;
			int _beatindex = 0;

			string _output = "";

			float _angleStart = float.Parse(txtSmooth_angleStart.Text);
			float _angleTarget = float.Parse(txtSmooth_angleTarget.Text);

			//calculate how much the angle needs to change per beat to reach target
			float _angledif = _angleTarget - _angleStart;
			float _anglechange = _angledif / _beats;


			for (int x = 0; x < _beats; x++) {
				_angleStart += _anglechange;
				_output += (_beatStart + _beatindex) + ":" + _angleStart.ToString("#0.0000") + ",";
				_beatindex++;
			}

			if (radioReturn_start.Checked) {
				_anglechange *= -1;
				for (int x = 0; x < _beats; x++) {
					_angleStart += _anglechange;
					_output += (_beatStart + _beatindex) + ":" + _angleStart.ToString("#0.0000") + ",";
					_beatindex++;
				}
			}

			richTextBox1.Text = _output;
		}

		///
		///TAB 3 - Offsets
		///
		private void btnOffset_Out_Click(object sender, EventArgs e)
		{
			int _offset;
			try {
				_offset = int.Parse(txtOffset_beat.Text);
			}
			catch (Exception) {
				_offset = 0;
				MessageBox.Show("Offset was not an integer. Defaulting to '0'");
			}
			string _in = txtOffset_In.Text;
			string _out = "";
			var _split = _in.Split(',');

			foreach (string _s in _split) {
				try {
					var _v = _s.Split(':');
					_v[0] = (int.Parse(_v[0]) + _offset).ToString();
					_out += _v[0] + ':' + _v[1] + ',';
				}
				catch (Exception) {
					_out += "";
				}
			}

			richTextBox1.Text = _out;
			/// egg
		}
	}
}
