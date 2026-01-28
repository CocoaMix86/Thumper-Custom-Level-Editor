using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods;

namespace Thumper_Custom_Level_Editor.Utility_Classes
{
    public class DGVEx : DataGridView
    {
        public DGVEx()
        {

        }

        protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        {
            base.PaintBackground(graphics, clipBounds, gridBounds);
            graphics.DrawImage(LeafMasterView.Master, 0 - this.HorizontalScrollingOffset, 0);
        }
    }
}
