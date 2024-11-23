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
        private TCLE mainform { get; set; }
        private bool isthisnew;
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };

        public ProjectPropertiesForm(TCLE form, bool newlevel)
		{
            isthisnew = newlevel;
			InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
            mainform = form;
            //set the form text fields
            txtCustomPath.Text = Path.GetDirectoryName(mainform.workingfolder);
            btnCustomSave.Enabled = true;
            //set samp pack checkboxes
            if (!newlevel) {
                chkLevel1.Checked = File.Exists($@"{mainform.workingfolder}\samp_level1_320bpm.txt");
                chkLevel2.Checked = File.Exists($@"{mainform.workingfolder}\samp_level2_340bpm.txt");
                chkLevel3.Checked = File.Exists($@"{mainform.workingfolder}\samp_level3_360bpm.txt");
                chkLevel4.Checked = File.Exists($@"{mainform.workingfolder}\samp_level4_380bpm.txt");
                chkLevel5.Checked = File.Exists($@"{mainform.workingfolder}\samp_level5_400bpm.txt");
                chkLevel6.Checked = File.Exists($@"{mainform.workingfolder}\samp_level6_420bpm.txt");
                chkLevel7.Checked = File.Exists($@"{mainform.workingfolder}\samp_level7_440bpm.txt");
                chkLevel8.Checked = File.Exists($@"{mainform.workingfolder}\samp_level8_460bpm.txt");
                chkLevel9.Checked = File.Exists($@"{mainform.workingfolder}\samp_level9_480bpm.txt");
                chkDissonance.Checked = File.Exists($@"{mainform.workingfolder}\samp_dissonant.txt");
                chkGlobal.Checked = File.Exists($@"{mainform.workingfolder}\samp_globaldrones.txt");
                chkRests.Checked = File.Exists($@"{mainform.workingfolder}\samp_rests.txt");
                chkMisc.Checked = File.Exists($@"{mainform.workingfolder}\samp_misc.txt");
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
            //check if any files are unsaved before moving folders
            if (mainform.workingfolder != levelpath) {
                if (/*!mainform._saveleaf || !mainform._savelvl || !mainform._savemaster || !mainform._savegate || !mainform._savesample*/ true) {
                    if (MessageBox.Show("Some files are unsaved. Changing the level folder will close them and unsaved changes will be lost?\nDo you want to continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                        return false;
                    }
                }
                ///mainform._saveleaf = mainform._savelvl = mainform._savemaster = mainform._savegate = mainform._savesample = true;
            }

            JObject level_details = new() {
                { "level_name", input.txtCustomName.Text },
                { "difficulty", input.txtCustomDiff.Text },
                { "description", input.txtDesc.Text },
                { "author", input.txtCustomAuthor.Text }
            };

            mainform.toolstripLevelName.Text = input.txtCustomName.Text;
            mainform.toolstripLevelName.Image = (Image)Properties.Resources.ResourceManager.GetObject(txtCustomDiff.Text);

            if (mainform.workingfolder != null && isthisnew == false && mainform.workingfolder != levelpath) {
                foreach (KeyValuePair<string, FileStream> fs in TCLE.lockedfiles) {
                    fs.Value.Close();
                }
                TCLE.lockedfiles.Clear();
                //using a random suffix on the end to avoid any folders with same name
                Directory.Move(mainform.workingfolder, $"{levelpath}1029");
                Directory.Move($"{levelpath}1029", $"{levelpath}");
                //if level name changes, should update the config file
                if (File.Exists($@"{levelpath}\config_{Path.GetFileName(mainform.workingfolder)}.txt"))
                    File.Move($@"{levelpath}\config_{Path.GetFileName(mainform.workingfolder)}.txt", $@"{levelpath}\config_{input.txtCustomName.Text}.txt");
            }

            if (!Directory.Exists(levelpath)) {
                Directory.CreateDirectory(levelpath);
            }

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
            ///create samp_ files if any boxes are checked
            //level 1
            if (input.chkLevel1.Checked && !File.Exists($@"{levelpath}\samp_level1_320bpm.txt"))
                File.WriteAllText($@"{levelpath}\samp_level1_320bpm.txt", Properties.Resources.samp_level1_320bpm);
            else if (input.chkLevel1.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level1_320bpm.txt", "samp");
            
            //level 2
            if (input.chkLevel2.Checked && !File.Exists($@"{levelpath}\samp_level2_340bpm.txt")) 
                    File.WriteAllText($@"{levelpath}\samp_level2_340bpm.txt", Properties.Resources.samp_level2_340bpm);
            else if (input.chkLevel2.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level2_340bpm.txt", "samp");
            
            //level 3
            if (input.chkLevel3.Checked && !File.Exists($@"{levelpath}\samp_level3_360bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level3_360bpm.txt", Properties.Resources.samp_level3_360bpm);
            else if (input.chkLevel3.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level3_360bpm.txt", "samp");
            
            //level 4
            if (input.chkLevel4.Checked && !File.Exists($@"{levelpath}\samp_level4_380bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level4_380bpm.txt", Properties.Resources.samp_level4_380bpm);
            else if (input.chkLevel4.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level4_380bpm.txt", "samp");
            
            //level 5
            if (input.chkLevel5.Checked && !File.Exists($@"{levelpath}\samp_level5_400bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level5_400bpm.txt", Properties.Resources.samp_level5_400bpm);
            else if (input.chkLevel5.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level5_400bpm.txt", "samp");
            
            //level 6
            if (input.chkLevel6.Checked && !File.Exists($@"{levelpath}\samp_level6_420bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level6_420bpm.txt", Properties.Resources.samp_level6_420bpm);
            else if (input.chkLevel6.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level6_420bpm.txt", "samp");
            
            //level 7
            if (input.chkLevel7.Checked && !File.Exists($@"{levelpath}\samp_level7_440bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level7_440bpm.txt", Properties.Resources.samp_level7_440bpm);
            else if (input.chkLevel7.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level7_440bpm.txt", "samp");
            
            //level 8
            if (input.chkLevel8.Checked && !File.Exists($@"{levelpath}\samp_level8_460bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level8_460bpm.txt", Properties.Resources.samp_level8_460bpm);
            else if (input.chkLevel8.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level8_460bpm.txt", "samp");
            
            //level 9
            if (input.chkLevel9.Checked && !File.Exists($@"{levelpath}\samp_level9_480bpm.txt")) 
                File.WriteAllText($@"{levelpath}\samp_level9_480bpm.txt", Properties.Resources.samp_level9_480bpm);
            else if (input.chkLevel9.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_level9_480bpm.txt", "samp");
            
            //Dissonance
            if (input.chkDissonance.Checked && !File.Exists($@"{levelpath}\samp_dissonant.txt")) 
                File.WriteAllText($@"{levelpath}\samp_dissonant.txt", Properties.Resources.samp_dissonant);
            else if (input.chkDissonance.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_dissonant.txt", "samp");
            
            //Global Drones
            if (input.chkGlobal.Checked && !File.Exists($@"{levelpath}\samp_globaldrones.txt")) 
                File.WriteAllText($@"{levelpath}\samp_globaldrones.txt", Properties.Resources.samp_globaldrones);
            else if (input.chkGlobal.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_globaldrones.txt", "samp");
            
            //Rests
            if (input.chkRests.Checked && !File.Exists($@"{levelpath}\samp_rests.txt")) 
                File.WriteAllText($@"{levelpath}\samp_rests.txt", Properties.Resources.samp_rests);
            else if (input.chkRests.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_rests.txt", "samp");
            
            //Misc
            if (input.chkMisc.Checked && !File.Exists($@"{levelpath}\samp_misc.txt")) 
                File.WriteAllText($@"{levelpath}\samp_misc.txt", Properties.Resources.samp_misc);
            else if (input.chkMisc.Checked == false)
                TCLE.DeleteFileLock($@"{levelpath}\samp_misc.txt", "samp");


            if (isthisnew || (isthisnew == false && mainform.workingfolder != levelpath)) {
                File.WriteAllText($@"{levelpath}\LEVEL DETAILS.txt", JsonConvert.SerializeObject(level_details, Formatting.Indented));
            }
            else {
                TCLE.WriteFileLock(TCLE.lockedfiles[$@"{levelpath}\LEVEL DETAILS.txt"], level_details);
            }
            TCLE.projectjson = level_details;
            mainform.workingfolder = levelpath;

            ///
            ///create a default master file and open it
            if (!File.Exists($@"{levelpath}\master_sequin.txt")) {
                ///mainform._loadedmaster = $@"{levelpath}\master_sequin.txt";
                ///mainform.WriteMaster();
            }
            if (mainform.workingfolder != levelpath) {
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
            bool exists = Directory.Exists($@"{Path.GetDirectoryName(mainform.workingfolder)}\{txtCustomName.Text}") && txtCustomName.Text != Path.GetFileName(mainform.workingfolder);
            bool samefolder = $@"{txtCustomPath.Text.ToLower()}\{txtCustomName.Text.ToLower()}" == mainform.workingfolder?.ToLower();
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
