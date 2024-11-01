using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_GateEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private FormLeafEditor _mainform { get; set; }
        public Form_GateEditor(FormLeafEditor form)
        {
            _mainform = form;
            InitializeComponent();
            toolstripTitleGate.Renderer = new ToolStripOverride();
            gateToolStrip.Renderer = new ToolStripOverride();
        }
    }
}
