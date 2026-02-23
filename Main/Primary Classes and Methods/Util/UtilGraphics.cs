using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilGraphics
    {
        private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius == 0) {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            // top left arc
            path.AddArc(arc, 180, 90);

            // top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius)) {
                graphics.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectangle(bounds, cornerRadius)) {
                graphics.FillPath(brush, path);
            }
        }
    }
}
