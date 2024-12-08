using System;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_DrawScene : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_DrawScene()
        {
            InitializeComponent();
        }
        #endregion

        private void btnDoTheThing_Click(object sender, EventArgs e) => Native.tcle_native_reload();
        private void Form_DrawScene_SizeChanged(object sender, EventArgs e) => DoDraw();

        // Creating multiple instances of this panel causes a double free situation
        // which is likely to trigger a crash
        private void DoDraw()
        {
            int targetWidth = this.panel1.Width;
            int targetHeight = this.panel1.Height;

            // Enforce minimum size to avoid nullptr allocation
            if (targetWidth < 32) targetWidth = 32;
            if (targetHeight < 32) targetHeight = 32;

            IntPtr pixelBuffer = Native.tcle_native_draw(targetWidth, targetHeight);
            this.panel1.BackgroundImage = new Bitmap(targetWidth, targetHeight, targetWidth * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, pixelBuffer);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DoDraw();
        }
    }
}