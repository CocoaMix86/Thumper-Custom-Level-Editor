namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectProperties : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_ProjectProperties()
        {
            InitializeComponent();
        }

        public void LoadProjectProperties()
        {
            propertyGridProject.SelectedObject = TCLE.ProjectProperties;
        }
        #endregion
    }
}
