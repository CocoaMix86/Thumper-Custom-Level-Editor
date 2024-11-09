using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    class ContextMenuColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder { get { return Color.FromArgb(66, 66, 66); } }
        public override Color ImageMarginGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientEnd { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(46, 46, 46); } }
        public override Color MenuItemSelected { get { return Color.FromArgb(61, 61, 61); } }
        public Color MenuItemEnabledBorder { get { return Color.FromArgb(112, 112, 112); } }
        public override Color MenuItemBorder { get { return Color.FromArgb(112, 112, 112); } }

        public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }

        public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ButtonSelectedBorder { get { return Color.FromArgb(112, 112, 112); } }
    }

    public class ToolStripOverride : ToolStripProfessionalRenderer
    {
        public ToolStripOverride() { }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripButton btn && btn.CheckOnClick && btn.Checked) {
                Rectangle bounds = new(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(Brushes.PaleTurquoise, bounds);
            }
            else base.OnRenderButtonBackground(e);
        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
    }

    public class ToolStripMainForm : ToolStripProfessionalRenderer
    {
        public ToolStripMainForm() : base(new ContextMenuColorTable()) { }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
    }

    public class ContextMenuColors : ToolStripProfessionalRenderer
    {
        public ContextMenuColors() : base(new ContextMenuColorTable()) { }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
    }
}
