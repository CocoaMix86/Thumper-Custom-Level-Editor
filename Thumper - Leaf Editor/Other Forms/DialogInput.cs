using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Thumper_Custom_Level_Editor
{
    public partial class DialogInput : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
        private FormLeafEditor mainform { get; set; }
        private bool isthisnew;
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };

        public DialogInput(FormLeafEditor form, bool newlevel)
		{
            isthisnew = newlevel;
			InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
            mainform = form;
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
            this.Close();
        }

        private void btnCustomSave_Click(object sender, EventArgs e)
        {
            CreateCustomLevelFolder(this);
            this.Close();
        }

        public void CreateCustomLevelFolder(DialogInput input)
        {
            JObject level_details = new() {
                { "level_name", input.txtCustomName.Text },
                { "difficulty", input.txtCustomDiff.Text },
                { "description", input.txtDesc.Text },
                { "author", input.txtCustomAuthor.Text }
            };
            mainform.toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);
            string levelpath = $@"{input.txtCustomPath.Text}\{input.txtCustomName.Text}";
            if (mainform.workingfolder != null && isthisnew == false && mainform.workingfolder != levelpath) {
                Directory.Move(mainform.workingfolder, levelpath);
                //if level name changes, should update the config file
                if (File.Exists($@"{levelpath}\config_{Path.GetFileName(mainform.workingfolder)}.txt"))
                    File.Move($@"{levelpath}\config_{Path.GetFileName(mainform.workingfolder)}.txt", $@"{levelpath}\config_{input.txtCustomName.Text}.txt");
            }
            if (!Directory.Exists(levelpath)) {
                Directory.CreateDirectory(levelpath);
            }
            //then write the file to the new folder that was created from the form
            File.WriteAllText($@"{levelpath}\LEVEL DETAILS.txt", JsonConvert.SerializeObject(level_details, Formatting.Indented));
            //these 4 files below are required defaults of new levels.
            //create them if they don't exist
            if (!File.Exists($@"{levelpath}\samp_default.txt")) {
                File.WriteAllText($@"{levelpath}\samp_default.txt", Properties.Resources.samp_default);
            }
            if (!File.Exists($@"{levelpath}\leaf_pyramid_outro.txt")) {
                File.WriteAllText($@"{levelpath}\leaf_pyramid_outro.txt", Properties.Resources.leaf_pyramid_outro);
            }
            if (!File.Exists($@"{levelpath}\spn_default.txt")) {
                File.WriteAllText($@"{levelpath}\spn_default.txt", Properties.Resources.spn_default);
            }
            if (!File.Exists($@"{levelpath}\xfm_default.txt")) {
                File.WriteAllText($@"{levelpath}\xfm_default.txt", Properties.Resources.xfm_default);
            }
            //finally, set workingfolder
            string workingfolder = levelpath;
            ///create samp_ files if any boxes are checked
            //level 1
            if (input.chkLevel1.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level1_320bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level1_320bpm.txt", Properties.Resources.samp_level1_320bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level1_320bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level1_320bpm.txt");
            }
            //level 2
            if (input.chkLevel2.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level2_340bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level2_340bpm.txt", Properties.Resources.samp_level2_340bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level2_340bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level2_340bpm.txt");
            }
            //level 3
            if (input.chkLevel3.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level3_360bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level3_360bpm.txt", Properties.Resources.samp_level3_360bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level3_360bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level3_360bpm.txt");
            }
            //level 4
            if (input.chkLevel4.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level4_380bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level4_380bpm.txt", Properties.Resources.samp_level4_380bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level4_380bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level4_380bpm.txt");
            }
            //level 5
            if (input.chkLevel5.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level5_400bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level5_400bpm.txt", Properties.Resources.samp_level5_400bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level5_400bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level5_400bpm.txt");
            }
            //level 6
            if (input.chkLevel6.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level6_420bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level6_420bpm.txt", Properties.Resources.samp_level6_420bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level6_420bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level6_420bpm.txt");
            }
            //level 7
            if (input.chkLevel7.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level7_440bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level7_440bpm.txt", Properties.Resources.samp_level7_440bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level7_440bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level7_440bpm.txt");
            }
            //level 8
            if (input.chkLevel8.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level8_460bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level8_460bpm.txt", Properties.Resources.samp_level8_460bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level8_460bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level8_460bpm.txt");
            }
            //level 9
            if (input.chkLevel9.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_level9_480bpm.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_level9_480bpm.txt", Properties.Resources.samp_level9_480bpm);
            }
            else if (File.Exists($@"{workingfolder}\samp_level9_480bpm.txt")) {
                File.Delete($@"{workingfolder}\samp_level9_480bpm.txt");
            }
            //Dissonance
            if (input.chkDissonance.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_dissonant.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_dissonant.txt", Properties.Resources.samp_dissonant);
            }
            else if (File.Exists($@"{workingfolder}\samp_dissonant.txt")) {
                File.Delete($@"{workingfolder}\samp_dissonant.txt");
            }
            //Global Drones
            if (input.chkGlobal.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_globaldrones.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_globaldrones.txt", Properties.Resources.samp_globaldrones);
            }
            else if (File.Exists($@"{workingfolder}\samp_globaldrones.txt")) {
                File.Delete($@"{workingfolder}\samp_globaldrones.txt");
            }
            //Rests
            if (input.chkRests.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_rests.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_rests.txt", Properties.Resources.samp_rests);
            }
            else if (File.Exists($@"{workingfolder}\samp_rests.txt")) {
                File.Delete($@"{workingfolder}\samp_rests.txt");
            }
            //Misc
            if (input.chkMisc.Checked) {
                if (!File.Exists($@"{workingfolder}\samp_misc.txt"))
                    File.WriteAllText($@"{workingfolder}\samp_misc.txt", Properties.Resources.samp_misc);
            }
            else if (File.Exists($@"{workingfolder}\samp_misc.txt")) {
                File.Delete($@"{workingfolder}\samp_misc.txt");
            }
            ///
            ///create a default master file and open it
            mainform.workingfolder = workingfolder;
            if (!File.Exists($@"{levelpath}\master_sequin.txt")) {
                mainform._loadedmaster = $@"{levelpath}\master_sequin.txt";
                mainform.WriteMaster();
            }
            if (mainform.workingfolder != workingfolder) {
                MessageBox.Show("New level folder was created, but not loaded.");
                return;
            }
            mainform.btnWorkRefresh_Click(null, null);
        }

        private void txtCustomName_TextChanged(object sender, EventArgs e)
        {
            lblNameError.Visible = false;
            btnCustomSave.Enabled = false;
            bool illegal = illegalchars.Any(c => txtCustomName.Text.Contains(c));
            bool exists = Directory.Exists($@"{Path.GetDirectoryName(mainform.workingfolder)}\{txtCustomName.Text}") && txtCustomName.Text != Path.GetFileName(mainform.workingfolder);
            bool endsindot = txtCustomName.Text.TrimEnd().EndsWith(".");

            if (illegal) {
                lblNameError.Visible = true;
                lblNameError.Text = "Illegal characters in name";
            }
            else if (exists) {
                lblNameError.Visible = true;
                lblNameError.Text = "A folder with this name already exists";
            }
            else if (endsindot) {
                lblNameError.Visible = true;
                lblNameError.Text = "A level name cannot end with '.'";
            }
            else
                btnCustomSave.Enabled = true;
        }
    }
}
