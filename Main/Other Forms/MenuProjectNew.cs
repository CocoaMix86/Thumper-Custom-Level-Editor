using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class MenuProjectNew : Form
    {
        public static readonly CommonOpenFileDialog FolderDialog = new() { IsFolderPicker = true, Multiselect = false, InitialDirectory = Application.StartupPath, Title = "Choose where to save the custom level" };
        private readonly static char[] IllegalChars = Path.GetInvalidFileNameChars();
        public FileInfo ProjectToLoad;
        private nint WindowHandle => this.Handle;

        public MenuProjectNew()
        {
            InitializeComponent();
            pictureDifficulty.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //this handles dragging the window by the toolbar along the top
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        private void toolStripTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //

        private void btnCustomFolder_Click(object sender, EventArgs e)
        {
            if (FolderDialog.ShowDialog(WindowHandle) != CommonFileDialogResult.Ok)
                return;

            if (FolderDialog.FileName.Length > 255) {
                MessageBox.Show("Folder path too long, due to Windows limits. Max length 255.\nChoose a different path.", "Thumper Custom Level Editor");
                return;
            }
            txtCustomPath.Text = FolderDialog.FileName;
            txtCustomName_TextChanged(null, null);
            SaveButtonCheck();
        }

        private void lblCustomDiffHelp_Click(object sender, EventArgs e)
        {
            new ImageMessageBox("difficultyhelp").Show();
        }

        private void txtCustomDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureDifficulty.Image = (Image)Properties.Resources.ResourceManager.GetObject($"difficulty_{txtCustomDiff.Text}");
            SaveButtonCheck();
        }

        private void btnCustomCancel_Click(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIfolderclose");
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
            string projectpath = Path.Join(txtCustomPath.Text, txtCustomName.Text, $"{txtCustomName.Text}.TCL");
            FileInfo NewProject = new(projectpath);
            if (!NewProject.Directory.Exists)
                NewProject.Directory.Create();

            ProjectProperties NewProjectProperties = new() {
                ProjectName = txtCustomName.Text,
                Difficulty = txtCustomDiff.Text,
                Description = txtDesc.Text,
                AuthorNames = txtCustomAuthor.Text,
                BPM = 400,
                RailColor = Color.White,
                RailGlowColor = Color.White,
                PathColor = Color.White,
                WorkingFile = NewProject
            };

            //Setup default files
            FileInfo defaultsamp = new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.samp");
            FileInfo defaultspn = new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.spn");
            FileInfo defaultxfm = new FileInfo($@"{NewProjectProperties.WorkingFolder}\default.xfm");
            CreateDefaultFile(defaultsamp, Properties.Resources.samp_default);
            CreateDefaultFile(defaultspn, Properties.Resources.spn_default);
            CreateDefaultFile(defaultxfm, Properties.Resources.xfm_default);
            //
            JObject save = TCLE.BuildSave(NewProjectProperties);
            UtilFile.WriteFileLock(NewProjectProperties.FileLock, save);
            NewProjectProperties.FileLock.Close();

            return NewProject;
        }
        private static void CreateDefaultFile(FileInfo file, string contents)
        {
            if (file.Exists)
                return;
            using StreamWriter sw = file.CreateText();
            sw.Write(contents);
        }

        bool ProjectNameValid;
        private void txtCustomName_TextChanged(object sender, EventArgs e)
        {
            lblNameError.Visible = false;
            btnCustomSave.Enabled = false;
            ProjectNameValid = false;
            bool illegal = txtCustomName.Text.IndexOfAny(IllegalChars) >= 0;
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
            else if (txtCustomName.TextLength + txtCustomPath.TextLength > 255) {
                lblNameError.Visible = true;
                lblNameError.Text = "The folder path + level name is longer than 256 characters (Windows limit).";
            }
            else {
                ProjectNameValid = true;
                SaveButtonCheck();
            }
        }

        private void SaveButtonCheck()
        {
            btnCustomSave.Enabled = ProjectNameValid && txtCustomDiff.Text.Length > 1 && txtCustomPath.Text.Length > 1;
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

        private void btnCustomSave_EnabledChanged(object sender, EventArgs e)
        {
            btnCustomSave.BackColor = btnCustomSave.Enabled ? Color.Green : Color.Gray;
        }
    }
}
