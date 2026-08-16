using System.Diagnostics;
using System.Windows.Controls;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class ProjectNotes : EditorBase
    {
        public ProjectNotes(string Notes)
        {
            InitializeComponent();
            txtNotes.Text = Notes;
        }

        private void Form_WorkSpace_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(UtilPaths.ProjectNotes, txtNotes.Text);
            TCLE.Documents.Remove("ProjectNotes");
        }

        private void Form_WorkSpace_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}