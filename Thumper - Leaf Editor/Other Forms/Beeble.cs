using System.Windows.Media.Imaging;

namespace Thumper_Custom_Level_Editor
{
    public partial class Beeble : Form
    {
        private static List<Image> beebleimages = new() { Properties.Resources.beeblehappy, Properties.Resources.beebleconfuse, Properties.Resources.beeblecool, Properties.Resources.beeblederp, Properties.Resources.beeblelaugh, Properties.Resources.beeblestare, Properties.Resources.beeblethink, Properties.Resources.beebletiny, Properties.Resources.beeblelove, Properties.Resources.beeblespin, Properties.Resources.beebleflesh, Properties.Resources.beebleuwu };
        private Random rng = new();
        private static Image BeebleDance = Properties.Resources.beebledance;

        public Beeble()
        {
            InitializeComponent();
            if (!Directory.Exists($@"{TCLE.AppLocation}\beeble") || Directory.GetFiles($@"{TCLE.AppLocation}\beeble").Length == 0) {
                Directory.CreateDirectory($@"{TCLE.AppLocation}\beeble");
                foreach (Image img in beebleimages)
                    img.Save($@"{TCLE.AppLocation}\beeble\beeble{beebleimages.IndexOf(img)}.png");
            }
            else {
                beebleimages.Clear();
                foreach (string img in Directory.GetFiles($@"{TCLE.AppLocation}\beeble", "*.png")) {
                    beebleimages.Add(Image.FromFile(img));
                }
                foreach (string img in Directory.GetFiles($@"{TCLE.AppLocation}\beeble", "*.jpg")) {
                    beebleimages.Add(Image.FromFile(img));
                }
                foreach (string img in Directory.GetFiles($@"{TCLE.AppLocation}\beeble", "*.gif")) {
                    beebleimages.Add(Image.FromFile(img));
                }
            }
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
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public void MakeFace()
        {
            int i = new Random().Next(0, 1001);
            if (i == 1000) {
                pictureBeeble.Image = Properties.Resources.beeblegold;
                TCLE.PlaySound("UIbeetleclickGOLD");
            }
            else {
                pictureBeeble.Image = BeebleDance;// beebleimages[i % beebleimages.Count];
            }
            timerBeeble.Start();
        }

        private void timerBeeble_Tick(object sender, EventArgs e)
        {
            timerBeeble.Stop();
            pictureBeeble.Image = Properties.Resources.beeble;
        }

        public void Dance(bool dance)
        {
            if (dance)
                pictureBeeble.Image = BeebleDance;
            else
                pictureBeeble.Image = Properties.Resources.beeble;
        }
    }
}
