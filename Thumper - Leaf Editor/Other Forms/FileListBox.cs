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
    public partial class FileListBox : Form
    {
        public FileListBox(List<string> files)
        {
            InitializeComponent();
            //files = new() { "a", "b", "c", "d", "e", "f" };
            foreach (string file in files) {
                TreeNode tn = new() {
                    Name = file,
                    Text = file,
                    ImageKey = "lvl",
                    SelectedImageKey = "lvl"
                };
                treeView1.Nodes.Add(tn);
            }
        }
        public string Value
        {
            get { return treeView1.SelectedNode.Text; }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
