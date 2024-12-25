namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    { 
        private void RecentFiles(List<string> recentfiles)
        {
            dgvRecentFiles.Rows.Clear();
            panelRecentFiles.Visible = true;
            panelRecentFiles.BringToFront();
            foreach (string level in recentfiles) {
                dgvRecentFiles.Rows.Add("", Path.GetFileName(level), level);
            }
            dgvRecentFiles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void dgvRecentFiles_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //button is in column 0, so that's where to draw the image
            if (e.ColumnIndex == 0) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //get dimensions
                int w = Properties.Resources.icon_openedfolders.Width;
                int h = Properties.Resources.icon_openedfolders.Height;
                int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
                int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
                //paint the image
                e.Graphics.DrawImage(Properties.Resources.icon_openedfolders, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
            //button is in column 3, so that's where to draw the image
            if (e.ColumnIndex == 3) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //get dimensions
                int w = Properties.Resources.icon_remove2.Width;
                int h = Properties.Resources.icon_remove2.Height;
                int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
                int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
                //paint the image
                e.Graphics.DrawImage(Properties.Resources.icon_remove2, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void dgvRecentFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string level = dgvRecentFiles.Rows[e.RowIndex].Cells[2].Value.ToString();
            //handle column 0 clicks only as that's where the button is
            if (e.ColumnIndex == 0) {
                if (workingfolder?.FullName == level) {
                    panelRecentFiles.Visible = false;
                    return;
                }
                if (!Directory.Exists(level)) {
                    if (MessageBox.Show($"Recent Level selected no longer exists at that location\n{level}\n\nDo you want to remove this entry?", "Level Custom Thumper Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        RemoveRecentLevel(e.RowIndex);
                    return;
                }
                //set working folder to the path
                workingfolder = new DirectoryInfo(level);
                panelRecentFiles.Visible = false;
                PlaySound("UIfolderclose");
            }
            //if remove column button clicked, run this
            if (e.ColumnIndex == 3) {
                RemoveRecentLevel(e.RowIndex);
            }
        }
        private void btnRecentClose_Click(object sender, EventArgs e)
        {
            PlaySound("UIfolderclose");
            panelRecentFiles.Visible = false;
        }

        private void panelRecentClick(object sender, EventArgs e)
        {
            panelRecentFiles.BringToFront();
        }

        private void RemoveRecentLevel(int index)
        {
            dgvRecentFiles.Rows.RemoveAt(index);
            PlaySound("UIselect");
            Properties.Settings.Default.Recentfiles.RemoveAt(index);
            Properties.Settings.Default.Save();
        }
    }
}