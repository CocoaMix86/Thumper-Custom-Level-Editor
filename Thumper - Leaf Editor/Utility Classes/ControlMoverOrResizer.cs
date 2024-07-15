using System;
using System.Windows.Forms;
using System.Drawing;

namespace Thumper_Custom_Level_Editor
{
    class ControlMover
    {
        public enum direction
        {
            Any,
            Horizontal,
            Vertical
        }

        public static void Init(Control control)
        {
            Init(control, direction.Any);
        }

        public static void Init(Control control, direction direction)
        {
            Init(control, control, direction);
        }

        public static void Init(Control control, Control container, direction direction)
        {
            bool Dragging = false;
            Point DragStart = Point.Empty;
            control.MouseDown += delegate (object sender, MouseEventArgs e) {
                Dragging = true;
                DragStart = new Point(e.X, e.Y);
                control.Capture = true;
            };

            control.MouseUp += delegate (object sender, MouseEventArgs e) {
                Dragging = false;
                control.Capture = false;
            };

            control.MouseMove += delegate (object sender, MouseEventArgs e) {
                if (Dragging) {
                    if (direction != direction.Vertical)
                        container.Left = Math.Max(0, e.X + container.Left - DragStart.X);
                    if (direction != direction.Horizontal)
                        container.Top = Math.Max(0, e.Y + container.Top - DragStart.Y);
                }
            };
        }
    }
}