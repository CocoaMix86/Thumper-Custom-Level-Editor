using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Windows.Networking.NetworkOperators;

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
            get { return ProjectProperties; } 
            set { ProjectProperties = value; } }
        private static ProjectProperties ProjectProperties;
        public decimal BPM { get { return ProjectProperties.bpm; } }
        #endregion
        #region Methods
        #endregion
    }
}
