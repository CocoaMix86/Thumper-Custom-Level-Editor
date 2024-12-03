namespace Thumper_Custom_Level_Editor
{
    public partial class FileNameDialog : Form
    {
        private string WorkingFolder { get; set; }
        private string Filetype { get; set; }
        private string[] illegalchars = new[] { "\\", "/", ":", "*", "?", "<", ">", "|" };

        public FileNameDialog(string workingfolder, string filetype)
        {
            InitializeComponent();
            WorkingFolder = workingfolder;
            Filetype = filetype;
        }

        private void FileNameDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnWorkRenameYes.PerformClick();
        }

        private void btnWorkRenameYes_Click(object sender, EventArgs e)
        {

        }

        private void txtWorkingRename_TextChanged(object sender, EventArgs e)
        {
            string newfilepath = $@"{WorkingFolder}\{Filetype}_{txtWorkingRename.Text}.txt";
            bool fileexists = File.Exists(newfilepath);
            bool illegal = illegalchars.Any(c => txtWorkingRename.Text.Contains(c));
            btnWorkRenameYes.Enabled = (fileexists || illegal) ? false : true;
            lblExists.Visible = (fileexists || illegal) ? true : false;

            if (illegal)
                lblExists.Text = "Illegal character in name!";
            if (fileexists)
                lblExists.Text = "That file name exists already!";
        }
    }
}