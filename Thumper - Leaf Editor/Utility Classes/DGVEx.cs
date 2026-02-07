using System.Drawing;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;
using Windows.Media.Playback;

namespace Thumper_Custom_Level_Editor.Utility_Classes
{
    public class DGVEx : DataGridView
    {
        public Bitmap MasterBG { get; set; }
        public DGVEx()
        {

        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            graphics.DrawImage(MasterBG, 0 - this.HorizontalScrollingOffset + 3, 0);
        }

    }

    public class DGVPlayback : DataGridView
    {
        public double PlaybackPosition { get; set; } = -1;
        private int _positionpixels => this.RowHeadersWidth + 80 - this.HorizontalScrollingOffset + (int)(PlaybackPosition * this.Columns[3].Width);
        public DGVPlayback()
        {
            this.PlaybackPosition = -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (PlaybackPosition > 0 && _positionpixels > this.RowHeadersWidth + 75)
                e.Graphics.DrawLine(LeafCellPainting.PenVioletThick, _positionpixels, 0, _positionpixels, this.Height);
        }

        public void ResetPlayback()
        {
            this.PlaybackPosition = -1;
        }
    }
}
