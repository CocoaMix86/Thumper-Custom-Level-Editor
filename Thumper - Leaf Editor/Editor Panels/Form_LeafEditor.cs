using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_LeafEditor : Form, IMessageFilter
    {
        private FormLeafEditor _mainform { get; set; }
        #region Form Construction
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> controlsToMove = new HashSet<Control>();

        public Form_LeafEditor(FormLeafEditor form)
        {
            Application.AddMessageFilter(this);
            _mainform = form;

            InitializeComponent();
            toolstripTitleMaster.Renderer = new ToolStripOverride();
            masterToolStrip.Renderer = new ToolStripOverride();
            controlsToMove.Add(this);
            controlsToMove.Add(panelMover);

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd))) {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                this.BringToFront();
                return true;
            }
            return false;
        }
        private int tolerance = 16;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg) {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.panelMaster.Region = region;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }
        #endregion

        private void lblPopoutMaster_Click(object sender, EventArgs e)
        {
            panelDockOptions.Visible = !panelDockOptions.Visible;
            /*
            ((FormLeafEditor)Parent).splitBottom1.Panel1.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            this.BringToFront();
            */
        }

        private void btnDock_Click(object sender, EventArgs e)
        {
            var parentform = this.Parent;
            panelDockOptions.Visible = false;
            string _docklocation = (sender as Button).Text;

            if (_docklocation == "1") {
                _mainform.DockPanel(this, _mainform.splitTop1.Panel1);
            }
            if (_docklocation == "2") {
                _mainform.DockPanel(this, _mainform.splitTop2.Panel1);
            }
            if (_docklocation == "3") {
                _mainform.DockPanel(this, _mainform.splitTop2.Panel2);
            }
            if (_docklocation == "4") {
                _mainform.DockPanel(this, _mainform.splitBottom1.Panel1);
            }
            if (_docklocation == "5") {
                _mainform.DockPanel(this, _mainform.splitBottom2.Panel1);
            }
            if (_docklocation == "6") {
                _mainform.DockPanel(this, _mainform.splitBottom2.Panel2);
            }
            if (_docklocation == "0") {
                _mainform.UndockPanel(this);
            }
        }

        private void btnDock1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Blue;
        }

        private void btnDock1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Black;
        }
    }
}
