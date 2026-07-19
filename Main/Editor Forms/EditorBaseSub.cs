using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public class EditorBaseSub : DockContent
    {
        public EditorBaseSub() { }

        protected override string GetPersistString()
        {
            return base.GetPersistString() + ";" + (this.TabText ?? this.Text).Replace("*", "");
        }
    }        
}
