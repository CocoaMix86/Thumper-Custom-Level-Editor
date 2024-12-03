namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectProperties : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_ProjectProperties()
        {
            InitializeComponent();
        }

        public void LoadProjectProperties(dynamic _load)
        {
            projectproperties = new ProjectProperties(_load);
            propertyGridProject.SelectedObject = projectproperties;
        }
        #endregion
        #region Variables
        public ProjectProperties projectproperties {
            get => ProjectProperties;
            set => ProjectProperties = value; }
        private static ProjectProperties ProjectProperties;
        public decimal BPM => ProjectProperties.bpm;
        #endregion
        #region Methods
        #endregion
    }
}
