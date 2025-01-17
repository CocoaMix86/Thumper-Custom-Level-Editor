using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
            propertyGridProject.PropertyValueChanged -= propertyGridProject_PropertyValueChanged;
            propertyGridProject.SelectedObject = TCLE.ProjectProperties;
            propertyGridProject.PropertyValueChanged += propertyGridProject_PropertyValueChanged;
        }
        #endregion

        private void propertyGridProject_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //build the JSON to write to file
            JObject _saveJSON = TCLE.BuildSave(TCLE.ProjectProperties);
            //write JSON to file
            File.WriteAllText($"{TCLE.ProjectProperties.TCL.FullName}", JsonConvert.SerializeObject(_saveJSON, Formatting.Indented));
        }
    }
}
