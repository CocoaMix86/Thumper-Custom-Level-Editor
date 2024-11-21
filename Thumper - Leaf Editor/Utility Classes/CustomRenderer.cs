using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    class ContextMenuColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder { get { return Color.FromArgb(112, 112, 112); } }
        public override Color MenuItemBorder { get { return Color.Red; } }
        public static Color MenuItemEnabledBorder { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ToolStripDropDownBackground { get { return Color.FromArgb(46, 46, 46); } }

        public override Color ImageMarginGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientEnd { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(46, 46, 46); } }
        public override Color MenuItemSelected { get { return Color.FromArgb(61, 61, 61); } }

        public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(46, 46, 46); } }

        public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ButtonSelectedBorder { get { return Color.FromArgb(112, 112, 112); } }
    }

    class EditorToolstripColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder { get { return Color.FromArgb(112, 112, 112); } }
        public override Color MenuItemBorder { get { return Color.Red; } }
        public static Color MenuItemEnabledBorder { get { return Color.FromArgb(112, 112, 112); } }
        public override Color ToolStripDropDownBackground { get { return Color.FromArgb(46, 46, 46); } }

        public override Color ImageMarginGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientEnd { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(46, 46, 46); } }
        public override Color MenuItemSelected { get { return Color.FromArgb(61, 61, 61); } }

        public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }
        public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(46, 46, 46); } }

        public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }
        public override Color ButtonSelectedBorder { get { return Color.FromArgb(112, 112, 112); } }

        public override Color ButtonCheckedGradientBegin { get { return Color.FromArgb(61, 61, 61); } }
        public override Color ButtonCheckedGradientEnd { get { return Color.FromArgb(61, 61, 61); } }
        public override Color ButtonCheckedHighlightBorder { get { return Color.Purple; } }

        public override Color ButtonPressedGradientBegin { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ButtonPressedGradientEnd { get { return Color.FromArgb(46, 46, 46); } }
        public override Color ButtonPressedBorder { get { return Color.Purple; } }
    }

    public class ToolStripOverride : ToolStripProfessionalRenderer
    {
        public ToolStripOverride() : base(new EditorToolstripColorTable()) { }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.BackColor != Color.FromArgb(46, 46, 46))
                base.OnRenderSplitButtonBackground(e);
            else {
                var sb = e.Item as ToolStripSplitButton;
                var button = sb.ButtonBounds;

                button.Width--;
                button.Height--;

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(46, 46, 46)), button);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(113, 96, 232)), button);

                OnRenderArrow(new ToolStripArrowRenderEventArgs(
                    e.Graphics, e.Item, sb.DropDownButtonBounds, e.Item.ForeColor,
                    ArrowDirection.Down));
            }
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
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Enabled && e.Item.Selected) {
                return;
            }
            base.OnRenderMenuItemBackground(e);
        }
    }
}
