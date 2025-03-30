using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public class DockContentEx : DockContent
    {
        public DockContentEx()
        {

        }

        protected override string GetPersistString()
        {
            return base.GetPersistString() + ";" + this.TabText;
        }
    }
}
