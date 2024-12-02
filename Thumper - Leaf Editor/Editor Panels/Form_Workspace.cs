using FastColoredTextBoxNS.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_WorkSpace : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_WorkSpace()
        {
            InitializeComponent();
            dockMain.Theme = new VS2015DarkTheme();
        }
        #endregion
    }
}