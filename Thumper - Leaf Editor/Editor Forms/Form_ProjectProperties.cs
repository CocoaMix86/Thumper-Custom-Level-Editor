using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectProperties : DockContentEx
    {
        #region Form Construction
        public Form_ProjectProperties()
        {
            InitializeComponent();
        }

        public void LoadProjectProperties()
        {
            propertyGridProject.PropertyValueChanged -= propertyGridProject_PropertyValueChanged;
            propertyGridProject.SelectedObject = TCLE.ProjectProperties;
            propertyGridProject.PropertyValueChanged += propertyGridProject_PropertyValueChanged;
        }
        #endregion

        private void propertyGridProject_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (this.TabText != "Project Properties")
                return;
            TCLE.SaveTCL();
        }
    }
}
