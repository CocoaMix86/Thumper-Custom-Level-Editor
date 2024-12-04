namespace Thumper_Custom_Level_Editor
{
    internal class ContextMenuColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder => Color.FromArgb(112, 112, 112);
        public override Color MenuItemBorder => Color.Red;
        public static Color MenuItemEnabledBorder => Color.FromArgb(112, 112, 112);
        public override Color ToolStripDropDownBackground => Color.FromArgb(46, 46, 46);

        public override Color ImageMarginGradientBegin => Color.FromArgb(46, 46, 46);
        public override Color ImageMarginGradientEnd => Color.FromArgb(46, 46, 46);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(46, 46, 46);
        public override Color MenuItemSelected => Color.FromArgb(61, 61, 61);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(61, 61, 61);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(61, 61, 61);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(46, 46, 46);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(46, 46, 46);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(112, 112, 112);
        public override Color ButtonSelectedGradientEnd => Color.FromArgb(112, 112, 112);
        public override Color ButtonSelectedBorder => Color.FromArgb(112, 112, 112);
    }

    internal class EditorToolstripColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder => Color.FromArgb(112, 112, 112);
        public override Color MenuItemBorder => Color.Red;
        public static Color MenuItemEnabledBorder => Color.FromArgb(112, 112, 112);
        public override Color ToolStripDropDownBackground => Color.FromArgb(46, 46, 46);

        public override Color ImageMarginGradientBegin => Color.FromArgb(46, 46, 46);
        public override Color ImageMarginGradientEnd => Color.FromArgb(46, 46, 46);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(46, 46, 46);
        public override Color MenuItemSelected => Color.FromArgb(61, 61, 61);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(61, 61, 61);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(61, 61, 61);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(46, 46, 46);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(46, 46, 46);

        public override Color ButtonSelectedGradientBegin => Color.FromArgb(61, 61, 61);
        public override Color ButtonSelectedGradientEnd => Color.FromArgb(61, 61, 61);
        public override Color ButtonSelectedBorder => Color.FromArgb(112, 112, 112);

        public override Color ButtonCheckedGradientBegin => Color.FromArgb(61, 61, 61);
        public override Color ButtonCheckedGradientEnd => Color.FromArgb(61, 61, 61);
        public override Color ButtonCheckedHighlightBorder => Color.Purple;

        public override Color ButtonPressedGradientBegin => Color.FromArgb(26, 26, 26);
        public override Color ButtonPressedGradientEnd => Color.FromArgb(26, 26, 26);
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
                ToolStripSplitButton? sb = e.Item as ToolStripSplitButton;
                Rectangle button = sb.ButtonBounds;

                button.Width--;
                button.Height--;

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(46, 46, 46)), button);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(113, 96, 232)), button);

                OnRenderArrow(new ToolStripArrowRenderEventArgs(
                    e.Graphics, e.Item, sb.DropDownButtonBounds, e.Item.ForeColor,
                    ArrowDirection.Down));
            }
        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripButton btn && btn.CheckOnClick && btn.Checked) {
                Rectangle bounds = new(Point.Empty, e.Item.Size);
                bounds.Width--;
                bounds.Height--;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(46, 46, 46)), bounds);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(113, 96, 232)), bounds);
            }
            else
                base.OnRenderButtonBackground(e);
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
