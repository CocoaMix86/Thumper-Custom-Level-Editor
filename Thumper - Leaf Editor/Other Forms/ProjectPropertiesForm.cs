using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Thumper_Custom_Level_Editor
{
    public partial class ProjectPropertiesForm : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
        private bool isthisnew;
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };

        public ProjectPropertiesForm(bool newlevel)
		{
            isthisnew = newlevel;
			InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
            //set the form text fields
            txtCustomPath.Text = Path.GetDirectoryName(TCLE.WorkingFolder);
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
            bool success = CreateCustomLevelFolder(this);
            if (success) {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        public bool CreateCustomLevelFolder(ProjectPropertiesForm input)
        {
            string levelpath = $@"{input.txtCustomPath.Text}\{input.txtCustomName.Text}";

            JObject level_details = new() {
                { "level_name", input.txtCustomName.Text },
                { "difficulty", input.txtCustomDiff.Text },
                { "description", input.txtDesc.Text },
                { "author", input.txtCustomAuthor.Text }
            };

            if (isthisnew) {
                ///PUT CODE HERE TO OPEN A NEW APP WINDOW AND LOAD THE NEW PROJECT


                ///
                return true;
            }

            ///mainform.toolstripLevelName.Text = input.txtCustomName.Text;
            ///mainform.toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);

            if (TCLE.WorkingFolder != null && TCLE.WorkingFolder != levelpath) {
                foreach (KeyValuePair<FileInfo, FileStream> fs in TCLE.lockedfiles) {
                    fs.Value.Close();
                }
                TCLE.lockedfiles.Clear();
                //using a random suffix on the end to avoid any folders with same name
                Directory.Move(TCLE.WorkingFolder, $"{levelpath}1029");
                Directory.Move($"{levelpath}1029", $"{levelpath}");
                //if level name changes, should update the config file
                if (File.Exists($@"{levelpath}\config_{Path.GetFileName(TCLE.WorkingFolder)}.txt"))
                    File.Move($@"{levelpath}\config_{Path.GetFileName(TCLE.WorkingFolder)}.txt", $@"{levelpath}\config_{input.txtCustomName.Text}.txt");
            }

            if (!Directory.Exists(levelpath)) {
                Directory.CreateDirectory(levelpath);
            }
            Dictionary<string, FileInfo> defaultFiles = new() { 
                {"defaultsamp", new FileInfo($@"{levelpath}\default.samp")},
                {"defaultspn", new FileInfo($@"{levelpath}\default.spn")},
                {"defaultxfm", new FileInfo($@"{levelpath}\default.xfm")},
                {"pyramidoutro", new FileInfo($@"{levelpath}\pyramid_outro.leaf")},
                {"level1", new FileInfo($@"{levelpath}\level1_320bpm.samp")},
                {"level2", new FileInfo($@"{levelpath}\level2_340bpm.samp")},
                {"level3", new FileInfo($@"{levelpath}\level3_360bpm.samp")},
                {"level4", new FileInfo($@"{levelpath}\level4_380bpm.samp")},
                {"level5", new FileInfo($@"{levelpath}\level5_400bpm.samp")},
                {"level6", new FileInfo($@"{levelpath}\level6_420bpm.samp")},
                {"level7", new FileInfo($@"{levelpath}\level7_440bpm.samp")},
                {"level8", new FileInfo($@"{levelpath}\level8_460bpm.samp")},
                {"level9", new FileInfo($@"{levelpath}\level9_480bpm.samp")},
                {"dissonant", new FileInfo($@"{levelpath}\dissonant.samp")},
                {"drones", new FileInfo($@"{levelpath}\globaldrones.samp")},
                {"rests", new FileInfo($@"{levelpath}\rests.samp")},
                {"misc", new FileInfo($@"{levelpath}\misc.samp")}};

            //these 4 files below are required defaults of new levels.
            //create them if they don't exist
            if (!defaultFiles["defaultsamp"].Exists) {
                defaultFiles["defaultsamp"].CreateText().Write(Properties.Resources.samp_default);
            }
            if (!defaultFiles["defaultspn"].Exists) {
                defaultFiles["defaultspn"].CreateText().Write(Properties.Resources.spn_default);
            }
            if (!defaultFiles["defaultxfm"].Exists) {
                defaultFiles["defaultxfm"].CreateText().Write(Properties.Resources.xfm_default);
            }
            if (!defaultFiles["pyramidoutro"].Exists) {
                defaultFiles["pyramidoutro"].CreateText().Write(Properties.Resources.leaf_pyramid_outro);
            }
            ///create samp_ files if any boxes are checked
            //level 1
            if (input.chkLevel1.Checked) {
                if (!defaultFiles["level1"].Exists)
                    defaultFiles["level1"].CreateText().Write(Properties.Resources.samp_level1_320bpm);
            }
            else
                TCLE.DeleteFileLock(defaultFiles["level1"]);
            
            //level 2
            if (input.chkLevel2.Checked) {
                if (!defaultFiles["level2"].Exists)
                    defaultFiles["level2"].CreateText().Write(Properties.Resources.samp_level2_340bpm);
            }
            else
                TCLE.DeleteFileLock(defaultFiles["level2"]);

            //level 3
            if (input.chkLevel3.Checked && !File.Exists($@"{levelpath}\samp_level3_360bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level3_360bpm.txt", Properties.Resources.samp_level3_360bpm);
            else if (input.chkLevel3.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level3_360bpm.txt");
            
            //level 4
            if (input.chkLevel4.Checked && !File.Exists($@"{levelpath}\samp_level4_380bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level4_380bpm.txt", Properties.Resources.samp_level4_380bpm);
            else if (input.chkLevel4.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level4_380bpm.txt");
            
            //level 5
            if (input.chkLevel5.Checked && !File.Exists($@"{levelpath}\samp_level5_400bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level5_400bpm.txt", Properties.Resources.samp_level5_400bpm);
            else if (input.chkLevel5.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level5_400bpm.txt");
            
            //level 6
            if (input.chkLevel6.Checked && !File.Exists($@"{levelpath}\samp_level6_420bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level6_420bpm.txt", Properties.Resources.samp_level6_420bpm);
            else if (input.chkLevel6.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level6_420bpm.txt");
            
            //level 7
            if (input.chkLevel7.Checked && !File.Exists($@"{levelpath}\samp_level7_440bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level7_440bpm.txt", Properties.Resources.samp_level7_440bpm);
            else if (input.chkLevel7.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level7_440bpm.txt");
            
            //level 8
            if (input.chkLevel8.Checked && !File.Exists($@"{levelpath}\samp_level8_460bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level8_460bpm.txt", Properties.Resources.samp_level8_460bpm);
            else if (input.chkLevel8.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level8_460bpm.txt");
            
            //level 9
            if (input.chkLevel9.Checked && !File.Exists($@"{levelpath}\samp_level9_480bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level9_480bpm.txt", Properties.Resources.samp_level9_480bpm);
            else if (input.chkLevel9.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level9_480bpm.txt");
            
            //Dissonance
            if (input.chkDissonance.Checked && !File.Exists($@"{levelpath}\samp_dissonant.txt")) 
                File.WriteAllText($@"{levelpath}\samp_dissonant.txt", Properties.Resources.samp_dissonant);
            else if (input.chkDissonance.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_dissonant.txt");
            
            //Global Drones
            if (input.chkGlobal.Checked && !File.Exists($@"{levelpath}\samp_globaldrones.txt")) 
                File.WriteAllText($@"{levelpath}\samp_globaldrones.txt", Properties.Resources.samp_globaldrones);
            else if (input.chkGlobal.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_globaldrones.txt");
            
            //Rests
            if (input.chkRests.Checked && !File.Exists($@"{levelpath}\samp_rests.txt")) 
                File.WriteAllText($@"{levelpath}\samp_rests.txt", Properties.Resources.samp_rests);
            else if (input.chkRests.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_rests.txt");
            
            //Misc
            if (input.chkMisc.Checked && !File.Exists($@"{levelpath}\samp_misc.txt")) 
                File.WriteAllText($@"{levelpath}\samp_misc.txt", Properties.Resources.samp_misc);
            else if (input.chkMisc.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_misc.txt");


            if (isthisnew || (isthisnew == false && TCLE.WorkingFolder != levelpath)) {
                File.WriteAllText($@"{levelpath}\LEVEL DETAILS.txt", JsonConvert.SerializeObject(level_details, Formatting.Indented));
            }
            else {
                TCLE.WriteFileLock(TCLE.lockedfiles[$@"{levelpath}\LEVEL DETAILS.txt"], level_details);
            }
            TCLE.projectjson = level_details;
            TCLE.WorkingFolder = levelpath;

            ///
            ///create a default master file and open it
            if (!File.Exists($@"{levelpath}\master_sequin.txt")) {
                ///mainform._loadedmaster = $@"{levelpath}\master_sequin.txt";
                ///mainform.WriteMaster();
            }
            if (TCLE.WorkingFolder != levelpath) {
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
            bool exists = Directory.Exists($@"{Path.GetDirectoryName(TCLE.WorkingFolder)}\{txtCustomName.Text}") && txtCustomName.Text != Path.GetFileName(TCLE.WorkingFolder);
            bool samefolder = $@"{txtCustomPath.Text.ToLower()}\{txtCustomName.Text.ToLower()}" == TCLE.WorkingFolder?.ToLower();
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
                lblNameError.Text = "The folder path + level name is longer than 256 characters\n                                               (Windows limit).";
            }
            else
                btnCustomSave.Enabled = true;
        }
    }
}
