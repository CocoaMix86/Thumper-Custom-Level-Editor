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
    public partial class FileNameDialog : Form
    {
        public FileNameDialog()
        {
            InitializeComponent();
        }

        private void FileNameDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnWorkRenameYes.PerformClick();
        }
    }
}
