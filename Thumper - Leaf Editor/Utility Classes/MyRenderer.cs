using System.Drawing;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    /// <summary>
    /// https://stackoverflow.com/questions/9260303/how-to-change-menu-hover-color
    /// </summary>
    class MyRenderer : ToolStripProfessionalRenderer
    {
        public MyRenderer() : base(new MyColors()) { }
        //credit to https://stackoverflow.com/questions/47241021/prevent-showing-border-of-a-disabled-menu-item-on-mouse-hover
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled && e.Item.Selected && (e.Item.Name is not "toolstripAppIcon" and not "toolstripLevelName")) {
                base.OnRenderMenuItemBackground(e);
                using Pen pen = new(((MyColors)ColorTable).MenuItemEnabledBorder);
                Rectangle r = new(0, 0, e.Item.Width - 2, e.Item.Height - 1);
                e.Graphics.DrawRectangle(pen, r);
            }
            else if (e.Item.Selected && e.Item.Name is "toolstripAppIcon" or "toolstripLevelName") {
                using Pen pen = new(Color.Transparent);
                Rectangle r = new(0, 0, e.Item.Width - 2, e.Item.Height - 1);
                e.Graphics.DrawRectangle(pen, r);
            }
            else if (e.Item.Enabled) {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }

    class MyColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected { get { return Color.BlueViolet; } }
        public override Color MenuItemSelectedGradientBegin { get { return Color.DarkRed; } }
        public override Color MenuItemSelectedGradientEnd { get { return Color.Red; } }
        public override Color MenuItemBorder { get { return Color.Transparent; } }
        public Color MenuItemEnabledBorder { get { return Color.Magenta; } }
        public override Color MenuItemPressedGradientBegin { get { return Color.DarkRed; } }
        public override Color MenuItemPressedGradientEnd { get { return Color.Red; } }
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
    }
}
