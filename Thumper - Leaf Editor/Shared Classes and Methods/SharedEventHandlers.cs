namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        private void combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            if (sender is ComboBox cbx) {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0) {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new() {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void dropTrackLane_DataSourceChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int maxWidth = 0;
            foreach (object obj in cb.Items) {
                int temp = TextRenderer.MeasureText(obj.ToString(), cb.Font).Width;
                if (temp > maxWidth) {
                    maxWidth = temp;
                }
            }
            cb.DropDownWidth = maxWidth != 0 ? maxWidth : cb.DropDownWidth;
        }

        private void dropTrackLane_DropDown(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = 0;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            if (senderComboBox.Items[0].GetType() == typeof(SampleData)) {
                foreach (SampleData s in senderComboBox.Items) {
                    newWidth = (int)g.MeasureString(s.obj_name, font).Width;
                    if (width < newWidth) {
                        width = newWidth;
                    }
                }
            }
            else {
                foreach (object? s in senderComboBox.Items) {
                    newWidth = (int)g.MeasureString(s.ToString(), font).Width;
                    if (width < newWidth) {
                        width = newWidth;
                    }
                }
            }
            senderComboBox.DropDownWidth = width + vertScrollBarWidth;
        }
    }
}