using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Thumper_Custom_Level_Editor
{
    public partial class ProjectPropertiesForm : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false, InitialDirectory = Application.StartupPath, Title = "Choose where to save the custom level" };
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };
        public FileInfo ProjectToLoad;
        private nint WindowHandle => this.Handle;

        public ProjectPropertiesForm()
        {
            InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnCustomFolder_Click(object sender, EventArgs e)
        {
            if (cfd_lvl.ShowDialog(WindowHandle) == CommonFileDialogResult.Ok) {
                if (cfd_lvl.FileName.Length > 255) {
                    MessageBox.Show("Folder path too long, due to Windows limits. Max length 255.\nChoose a different path.", "Thumper Custom Level Editor");
                    return;
                }
                txtCustomPath.Text = cfd_lvl.FileName;
                txtCustomName_TextChanged(null, null);
                SaveButtonCheck();
            }
        }

        private void lblCustomDiffHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("difficultyhelp").Show();
        }

        private void txtCustomDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image diff = (Image)Properties.Resources.ResourceManager.GetObject($"difficulty_{txtCustomDiff.Text}");
            pictureDifficulty.Image = diff;
            SaveButtonCheck();
        }

        private void btnCustomCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnCustomSave_Click(object sender, EventArgs e)
        {
            FileInfo NewProject = CreateCustomLevelFolder();
            if (NewProject != null) {
                //if this application already has a TCL loaded, open a new application and pass in the TCL name to load it
                if (TCLE.WorkingFolder != null) {
                    ProcessStartInfo info = new(Application.ExecutablePath, NewProject.FullName);
                    Process.Start(info);
                }
                ProjectToLoad = NewProject;
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        public FileInfo CreateCustomLevelFolder()
        {
            FileInfo NewProject = new($@"{txtCustomPath.Text}\{txtCustomName.Text}\{txtCustomName.Text}.TCL");
            ProjectProperties NewProjectProperties = new() {
                ProjectName = txtCustomName.Text,
                difficulty = txtCustomDiff.Text,
                description = txtDesc.Text,
                authornames = txtCustomAuthor.Text,
                BPM = 400,
                rail = Color.White,
                railglow = Color.White,
                path = Color.White,
                WorkingFile = NewProject
            };

            if (!NewProjectProperties.WorkingFolder.Exists)
                NewProjectProperties.WorkingFolder.Create();

            ///Initialize lists based on checkboxes and the new levelpath
            Dictionary<string, FileInfo> defaultFiles = new() {
                {"defaultsamp", new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.samp")},
                {"defaultspn", new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.spn")},
                {"defaultxfm", new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.xfm")}
            };

            ///these 4 files below are required defaults of new levels.
            ///create them if they don't exist
            if (!NewProjectProperties.WorkingFolder.GetFiles(defaultFiles["defaultsamp"].Name, SearchOption.AllDirectories).Any()) {
                using (StreamWriter sw = defaultFiles["defaultsamp"].CreateText()) {
                    sw.Write(Properties.Resources.samp_default);
                }
            }
            if (!NewProjectProperties.WorkingFolder.GetFiles(defaultFiles["defaultspn"].Name, SearchOption.AllDirectories).Any()) {
                using (StreamWriter sw = defaultFiles["defaultspn"].CreateText()) {
                    sw.Write(Properties.Resources.spn_default);
                }
            }
            if (!NewProjectProperties.WorkingFolder.GetFiles(defaultFiles["defaultxfm"].Name, SearchOption.AllDirectories).Any()) {
                using (StreamWriter sw = defaultFiles["defaultxfm"].CreateText()) {
                    sw.Write(Properties.Resources.xfm_default);
                }
            }

            JObject save = TCLE.BuildSave(NewProjectProperties);
            File.WriteAllText(NewProject.FullName, JsonConvert.SerializeObject(save, Formatting.Indented));

            return NewProject;
        }

        bool nameok;
        private void txtCustomName_TextChanged(object sender, EventArgs e)
        {
            lblNameError.Visible = false;
            btnCustomSave.Enabled = false;
            nameok = false;
            bool illegal = illegalchars.Any(c => txtCustomName.Text.Contains(c));
            bool exists = Directory.Exists($@"{txtCustomPath}\{txtCustomName.Text}") && txtCustomName.Text != TCLE.WorkingFolder.Name;
            bool endsindot = txtCustomName.Text.TrimEnd().EndsWith('.');
            bool endsinspace = txtCustomName.Text.EndsWith(' ');

            if (illegal) {
                lblNameError.Visible = true;
                lblNameError.Text = "Illegal characters in name (\\, /, :, *, ?, <, >, |)";
            }
            else if (exists) {
                lblNameError.Visible = true;
                lblNameError.Text = "A folder with this name already exists";
            }
            else if (endsindot) {
                lblNameError.Visible = true;
                lblNameError.Text = "A level name cannot end with '.'";
            }
            else if (endsinspace) {
                lblNameError.Visible = true;
                lblNameError.Text = "A level name cannot end with ' ' (space)";
            }
            else if (txtCustomName.Text.Length + txtCustomPath.Text.Length > 255) {
                lblNameError.Visible = true;
                lblNameError.Text = "The folder path + level name is longer than 256 characters (Windows limit).";
            }
            else {
                nameok = true;
                SaveButtonCheck();
            }
        }

        private void SaveButtonCheck()
        {
            if (nameok && txtCustomDiff.Text.Length > 1 && txtCustomPath.Text.Length > 1)
                btnCustomSave.Enabled = true;
            else
                btnCustomSave.Enabled = false;
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

        private void txtCustomPath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
