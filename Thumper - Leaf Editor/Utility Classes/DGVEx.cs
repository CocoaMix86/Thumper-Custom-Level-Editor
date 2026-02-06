using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;

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
}
