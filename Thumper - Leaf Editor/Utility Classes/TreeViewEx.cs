using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Thumper_Custom_Level_Editor
{
    [DesignerCategory("Code")]
    public class TreeViewEx : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_REFLECT_NOTIFY) {
                NMHDR st = Marshal.PtrToStructure<NMHDR>(m.LParam);

                if (st.code == TVN_BEGINLABELEDITW) {
                    IntPtr editPtr = SendMessage(Handle, TVM_GETEDITCONTROL, IntPtr.Zero, IntPtr.Zero);

                    if (editPtr != IntPtr.Zero && SelectedNode != null) {
                        int selStart = 0;
                        int selEnd = Path.GetFileNameWithoutExtension(SelectedNode.Text).Length;
                        SendMessage(editPtr, EM_SETSEL, selStart, selEnd);
                    }
                }
            }
        }

        const int WM_REFLECT_NOTIFY = 0x204E;
        const int TVM_GETEDITCONTROL = 0x0000110F;
        const int TVN_BEGINLABELEDITW = 0 - 400 - 59;
        const int EM_SETSEL = 0xB1;

        [StructLayout(LayoutKind.Sequential)]
        struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
