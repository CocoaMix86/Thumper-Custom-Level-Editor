using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public class ToolStripEx : ToolStrip
    {
        private const int WM_MOUSEACTIVATE = 0x21;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE && this.CanFocus && !this.Focused)
                this.Focus();

            base.WndProc(ref m);
        }
    }
}
