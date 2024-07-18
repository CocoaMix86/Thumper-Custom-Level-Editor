using System;
using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    class ResizeablePanel : Panel
    {
        public ResizeablePanel()
        {
            //this.ResizeRedraw = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Parent.GetType() == typeof(SplitterPanel))
                return;
            Rectangle rc = new Rectangle(this.ClientSize.Width - grab, this.ClientSize.Height - grab, grab, grab);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84) {  // Trap WM_NCHITTEST
                if (this.Parent.GetType() == typeof(SplitterPanel))
                    return;
                Point pos = this.PointToClient(new Point(m.LParam.ToInt32()));
                if (pos.X >= this.ClientSize.Width - grab && pos.Y >= this.ClientSize.Height - grab)
                    m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
            }
        }
        private const int grab = 16;
    }
}
