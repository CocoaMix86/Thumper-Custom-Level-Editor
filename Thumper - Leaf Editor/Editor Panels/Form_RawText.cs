using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_RawText : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public Form_RawText(dynamic _load)
        {
            InitializeComponent();
            textEditor.Language = FastColoredTextBoxNS.Text.Language.JSON;
            textEditor.Text = JsonConvert.SerializeObject(_load, Formatting.Indented);
            textEditor.ClearUndo();
            textEditor.SetSelectedLine(-1);
        }
    }
}
