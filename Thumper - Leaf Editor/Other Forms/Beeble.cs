using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class Beeble : Form
    {
        static List<Image> beebleimages = new() { Properties.Resources.beeblehappy, Properties.Resources.beebleconfuse, Properties.Resources.beeblecool, Properties.Resources.beeblederp, Properties.Resources.beeblelaugh, Properties.Resources.beeblestare, Properties.Resources.beeblethink, Properties.Resources.beebletiny, Properties.Resources.beeblelove, Properties.Resources.beeblespin, Properties.Resources.beebleflesh, Properties.Resources.beebleuwu };
        Random rng = new Random();

        public Beeble()
        {
            InitializeComponent();
        }

        private void Beeble_Load(object sender, EventArgs e)
        {

        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Beeble_MouseDown(object sender, MouseEventArgs e)
        {
            TCLE.PlaySound($"UIbeetleclick{rng.Next(1, 9)}");
            this.BackColor = Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            MakeFace();
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void MakeFace()
        {
            int i = new Random().Next(0, 1001);
            if (i == 1000) {
                this.BackgroundImage = Properties.Resources.beeblegold;
                TCLE.PlaySound("UIbeetleclickGOLD");
            }
            else {
                this.BackgroundImage = beebleimages[i % 12];
            }
            timerBeeble.Start();
        }

        private void timerBeeble_Tick(object sender, EventArgs e)
        {
            timerBeeble.Stop();
            this.BackgroundImage = Properties.Resources.beeble;
        }
    }
}
