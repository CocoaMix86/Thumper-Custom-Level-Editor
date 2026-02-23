using System.Reflection;

namespace Thumper_Custom_Level_Editor
{
    public partial class ImageMessageBox : Form
    {
        private System.Drawing.Size _size;
        private TCLE tcle;

        public ImageMessageBox()
        {
            InitializeComponent();
            lblVersion.Text = $"Thumper Custom Level Editor -- {TCLE.VersionNumber}";
        }

        public ImageMessageBox(string path, TCLE _tcle = null)
        {
            InitializeComponent();
            tcle = _tcle;
            lblVersion.Visible = false;
            this.ControlBox = true;

            if (path == "railcolorhelp") {
                this.BackgroundImage = Properties.Resources.help_railcolor;
                this.BackgroundImageLayout = ImageLayout.Center;
                this.Text = "Rail Color Explanation";
                _size = this.BackgroundImage.Size;
            }
            if (path == "difficultyhelp") {
                this.BackgroundImage = Properties.Resources.help_difficulty;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.Text = "Difficulty Explanation";
                _size = new System.Drawing.Size(700, 690);
            }
            if (path == "bosssectionhelp") {
                this.BackgroundImage = Properties.Resources.help_bosssection;
                this.BackgroundImageLayout = ImageLayout.Center;
                this.Text = "Boss Section Explanation";
                _size = this.BackgroundImage.Size;
            }
            if (path == "splashscreen") {
                lblVersion.Visible = true;
                lblVersion.Text = $"Thumper Custom Level Editor -- {TCLE.VersionNumber}";
                this.BackgroundImage = Properties.Resources.Thumper_Splash;
                this.BackgroundImageLayout = ImageLayout.Center;
                this.Text = "";
                this.ControlBox = false;
                this.FormBorderStyle = FormBorderStyle.None;
                _size = this.BackgroundImage.Size;
                timer1.Enabled = true;
            }
            this.Size = _size;
            this.Height += 40;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tcle != null) {
                tcle.MaximizeScreenBounds();
                TCLE.MainBeeble.Visible = true;
                TCLE.MainBeeble.Location = Properties.Settings.Default.beebleloc;
            }
            this.Close();
        }

        private void ImageMessageBox_Load(object sender, EventArgs e)
        {

        }
    }
}
