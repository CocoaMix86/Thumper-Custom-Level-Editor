using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;

namespace Thumper_Custom_Level_Editor
{
    public partial class ProjectPropertiesForm : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
        private bool isthisnew;
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };

        public ProjectPropertiesForm(bool newlevel)
		{
			InitializeComponent();

            isthisnew = newlevel;
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
            //set the form text fields
            txtCustomPath.Text = Path.GetDirectoryName(TCLE.WorkingFolder.FullName);
            btnCustomSave.Enabled = true;
            //set samp pack checkboxes
            if (!newlevel) {
                chkLevel1.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level1_320bpm.txt");
                chkLevel2.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level2_340bpm.txt");
                chkLevel3.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level3_360bpm.txt");
                chkLevel4.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level4_380bpm.txt");
                chkLevel5.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level5_400bpm.txt");
                chkLevel6.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level6_420bpm.txt");
                chkLevel7.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level7_440bpm.txt");
                chkLevel8.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level8_460bpm.txt");
                chkLevel9.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_level9_480bpm.txt");
                chkDissonance.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_dissonant.txt");
                chkGlobal.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_globaldrones.txt");
                chkRests.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_rests.txt");
                chkMisc.Checked = File.Exists($@"{TCLE.WorkingFolder}\samp_misc.txt");
            }
        }
        
        private void btnCustomFolder_Click(object sender, EventArgs e)
        {
            cfd_lvl.InitialDirectory = Application.StartupPath;
            cfd_lvl.Title = "Choose where to save the custom level";
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok) {
                //if (Directory.EnumerateFileSystemEntries(cfd_lvl.FileName).Any() && MessageBox.Show("The selected path is not empty. Do you still wish to save level data to this location?", "Confirm choice", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    //return;
                txtCustomPath.Text = cfd_lvl.FileName;
                btnCustomSave.Enabled = true;
            }
        }

		private void lblCustomDiffHelp_Click(object sender, EventArgs e)
		{
			new ImageMessageBox("difficultyhelp").Show();
		}

        private void combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            if (sender is ComboBox cbx) {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0) {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new() {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void txtCustomDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image diff = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);
            pictureDifficulty.Image = diff;
        }

        private void btnCustomCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnCustomSave_Click(object sender, EventArgs e)
        {
            bool success = CreateCustomLevelFolder();
            if (success) {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        public bool CreateCustomLevelFolder()
        {
            string levelpath = $@"{txtCustomPath.Text}\{txtCustomName.Text}";

            JObject level_details = new() {
                { "level_name", txtCustomName.Text },
                { "difficulty", txtCustomDiff.Text },
                { "description", txtDesc.Text },
                { "author", txtCustomAuthor.Text }
            };

            if (isthisnew) {
                ///PUT CODE HERE TO OPEN A NEW APP WINDOW AND LOAD THE NEW PROJECT


                ///
                return true;
            }

            ///mainform.toolstripLevelName.Text = input.txtCustomName.Text;
            ///mainform.toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);

            if (TCLE.WorkingFolder != null && TCLE.WorkingFolder.FullName != levelpath) {
                foreach (KeyValuePair<FileInfo, FileStream> fs in TCLE.lockedfiles) {
                    fs.Value.Close();
                }
                TCLE.lockedfiles.Clear();
                //using a random suffix on the end to avoid any folders with same name
                TCLE.WorkingFolder.MoveTo($"{levelpath}1029");
                TCLE.WorkingFolder.MoveTo($"{levelpath}");
                ///Directory.Move(TCLE.WorkingFolder, $"{levelpath}1029");
                ///Directory.Move($"{levelpath}1029", $"{levelpath}");
                //if level name changes, should update the config file
                if (File.Exists($@"{levelpath}\{TCLE.WorkingFolder.Name}.TCL"))
                    File.Move($@"{levelpath}\{TCLE.WorkingFolder.Name}.TCL", $@"{levelpath}\{txtCustomName.Text}.TCL");
            }

            if (!Directory.Exists(levelpath)) {
                Directory.CreateDirectory(levelpath);
            }

            ///Initialize lists based on checkboxes and the new levelpath
            Dictionary<string, FileInfo> defaultFiles = new() {
                {"defaultsamp", new FileInfo($@"{levelpath}\default.samp")},
                {"defaultspn", new FileInfo($@"{levelpath}\default.spn")},
                {"defaultxfm", new FileInfo($@"{levelpath}\default.xfm")},
                {"pyramidoutro", new FileInfo($@"{levelpath}\pyramid_outro.leaf")}
            };
            List<Tuple<FileInfo, bool, string>> samplePacks = new() {                 
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level1_320bpm.samp"), chkLevel1.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level2_340bpm.samp"), chkLevel2.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level3_360bpm.samp"), chkLevel3.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level4_380bpm.samp"), chkLevel4.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level5_400bpm.samp"), chkLevel5.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level6_420bpm.samp"), chkLevel6.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level7_440bpm.samp"), chkLevel7.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level8_460bpm.samp"), chkLevel8.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\level9_480bpm.samp"), chkLevel9.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\dissonant.samp"), chkDissonance.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\globaldrones.samp"), chkGlobal.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\rests.samp"), chkRests.Checked, Properties.Resources.samp_level1_320bpm),
                new Tuple<FileInfo, bool, string>(new FileInfo($@"{levelpath}\misc.samp"), chkMisc.Checked, Properties.Resources.samp_level1_320bpm)};

            ///these 4 files below are required defaults of new levels.
            ///create them if they don't exist
            if (!Directory.GetFiles(levelpath, defaultFiles["defaultsamp"].Name, SearchOption.AllDirectories).Any()) {
                defaultFiles["defaultsamp"].CreateText().Write(Properties.Resources.samp_default);
            }
            if (!Directory.GetFiles(levelpath, defaultFiles["defaultspn"].Name, SearchOption.AllDirectories).Any()) {
                defaultFiles["defaultspn"].CreateText().Write(Properties.Resources.spn_default);
            }
            if (!Directory.GetFiles(levelpath, defaultFiles["defaultxfm"].Name, SearchOption.AllDirectories).Any()) {
                defaultFiles["defaultxfm"].CreateText().Write(Properties.Resources.xfm_default);
            }
            if (!Directory.GetFiles(levelpath, defaultFiles["pyramidoutro"].Name, SearchOption.AllDirectories).Any()) {
                defaultFiles["pyramidoutro"].CreateText().Write(Properties.Resources.leaf_pyramid_outro);
            }
            ///create samp_ files if any boxes are checked
            foreach (Tuple<FileInfo, bool, string> pack in samplePacks) {
                if (pack.Item2) {
                    if (!Directory.GetFiles(levelpath, pack.Item1.Name, SearchOption.AllDirectories).Any())
                        pack.Item1.CreateText().Write(pack.Item3);
                }
                else
                    TCLE.DeleteFileLock(new FileInfo(Directory.GetFiles(levelpath, pack.Item1.Name, SearchOption.AllDirectories).First()));
            }

            
            //if (isthisnew || (isthisnew == false && TCLE.WorkingFolder != levelpath)) {
            //    File.WriteAllText($@"{levelpath}\{txtCustomName.Text}.TCL", JsonConvert.SerializeObject(level_details, Formatting.Indented));
            //}
            //else {
                TCLE.WriteFileLock(TCLE.lockedfiles[new FileInfo($@"{levelpath}\{txtCustomName.Text}.TCL")], level_details);
            //}
            TCLE.ProjectJson = level_details;
            TCLE.WorkingFolder = new DirectoryInfo(levelpath);

            ///
            ///create a default master file and open it
            if (!File.Exists($@"{levelpath}\sequin.master")) {
                ///mainform._loadedmaster = $@"{levelpath}\master_sequin.txt";
                ///mainform.WriteMaster();
            }
            if (TCLE.WorkingFolder.FullName != levelpath) {
                MessageBox.Show("New level folder was created, but not loaded.", "Something went wrong...");
                return false;
            }
            ///mainform.btnWorkRefresh_Click(null, null);
            return true;
        }

        private void txtCustomName_TextChanged(object sender, EventArgs e)
        {
            lblNameError.Visible = false;
            btnCustomSave.Enabled = false;
            bool illegal = illegalchars.Any(c => txtCustomName.Text.Contains(c));
            bool exists = Directory.Exists($@"{TCLE.WorkingFolder.Parent}\{txtCustomName.Text}") && txtCustomName.Text != TCLE.WorkingFolder.Name;
            bool samefolder = $@"{txtCustomPath.Text.ToLower()}\{txtCustomName.Text.ToLower()}" == TCLE.WorkingFolder.FullName.ToLower();
            bool endsindot = txtCustomName.Text.TrimEnd().EndsWith('.');

            if (illegal) {
                lblNameError.Visible = true;
                lblNameError.Text = "Illegal characters in name";
            }
            else if (samefolder) {
                btnCustomSave.Enabled = true;
            }
            else if (exists) {
                lblNameError.Visible = true;
                lblNameError.Text = "A folder with this name already exists";
            }
            else if (endsindot) {
                lblNameError.Visible = true;
                lblNameError.Text = "A level name cannot end with '.'";
            }
            else if (txtCustomName.Text.Length + txtCustomPath.Text.Length > 240) {
                lblNameError.Visible = true;
                lblNameError.Text = "The folder path + level name is longer than 256 characters (Windows limit).";
            }
            else
                btnCustomSave.Enabled = true;
        }
    }
}
