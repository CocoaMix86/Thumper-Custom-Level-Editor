using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    class ResizeablePanel : Panel
    {
        public ResizeablePanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.Gray;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var p = new Pen(Color.Black, 4)) {
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }
        }
        const int WM_NCHITTEST = 0x84;
        const int WM_SETCURSOR = 0x20;
        const int WM_NCLBUTTONDBLCLK = 0xA3;
        protected override void WndProc(ref Message m)
        {
            int borderWidth = 15;
            if (m.Msg == WM_SETCURSOR)  /*Setting cursor to SizeAll*/
            {
                if ((m.LParam.ToInt32() & 0xffff) == 0x2 /*Move*/) {
                    //Cursor.Current = Cursors.SizeAll;
                    m.Result = (IntPtr)1;
                    return;
                }
            }
            if ((m.Msg == WM_NCLBUTTONDBLCLK)) /*Disable Mazimiz on Double click*/
            {
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
            if (this.Parent.GetType() == typeof(SplitterPanel))
                return;
            if (m.Msg == WM_NCHITTEST) {
                var pos = PointToClient(new Point(m.LParam.ToInt32() & 0xffff,
                    m.LParam.ToInt32() >> 16));
                if (pos.X <= ClientRectangle.Left + borderWidth &&
                    pos.Y <= ClientRectangle.Top + borderWidth)
                    m.Result = new IntPtr(13); //TOPLEFT
                else if (pos.X >= ClientRectangle.Right - borderWidth &&
                    pos.Y <= ClientRectangle.Top + borderWidth)
                    m.Result = new IntPtr(14); //TOPRIGHT
                else if (pos.X <= ClientRectangle.Left + borderWidth &&
                    pos.Y >= ClientRectangle.Bottom - borderWidth)
                    m.Result = new IntPtr(16); //BOTTOMLEFT
                else if (pos.X >= ClientRectangle.Right - borderWidth &&
                    pos.Y >= ClientRectangle.Bottom - borderWidth)
                    m.Result = new IntPtr(17); //BOTTOMRIGHT
                else if (pos.X <= ClientRectangle.Left + borderWidth)
                    m.Result = new IntPtr(10); //LEFT
                else if (pos.Y <= ClientRectangle.Top + borderWidth)
                    m.Result = new IntPtr(12); //TOP
                else if (pos.X >= ClientRectangle.Right - borderWidth)
                    m.Result = new IntPtr(11); //RIGHT
                else if (pos.Y >= ClientRectangle.Bottom - borderWidth)
                    m.Result = new IntPtr(15); //Bottom
                //else
                   // m.Result = new IntPtr(2); //Move
            }
        }
    }
}
