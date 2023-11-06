using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor
    { 
        private void RecentFiles(List<string> recentfiles)
        {
            dgvRecentFiles.Rows.Clear();
            panelRecentFiles.Visible = true;
            panelRecentFiles.BringToFront();
            foreach (string level in recentfiles) {
                dgvRecentFiles.Rows.Add("", Path.GetFileName(level), level);
            }
        }
        private void dgvRecentFiles_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //button is in column 0, so that's where to draw the image
            if (e.ColumnIndex == 0) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //get dimensions
                var w = Properties.Resources.icon_openedfolders.Width;
                var h = Properties.Resources.icon_openedfolders.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                //paint the image
                e.Graphics.DrawImage(Properties.Resources.icon_openedfolders, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
            //button is in column 3, so that's where to draw the image
            if (e.ColumnIndex == 3) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //get dimensions
                var w = Properties.Resources.icon_remove2.Width;
                var h = Properties.Resources.icon_remove2.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                //paint the image
                e.Graphics.DrawImage(Properties.Resources.icon_remove2, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void dgvRecentFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //handle column 0 clicks only as that's where the button is
            if (e.ColumnIndex == 0) {
                if (workingfolder == dgvRecentFiles.Rows[e.RowIndex].Cells[2].Value.ToString()) {
                    panelRecentFiles.Visible = false;
                    return;
                }
                //set working folder to the path
                ClearPanels();
                workingfolder = dgvRecentFiles.Rows[e.RowIndex].Cells[2].Value.ToString();
                panelRecentFiles.Visible = false;
                PlaySound("UIfolderclose");
            }
            //if remove column button clicked, run this
            if (e.ColumnIndex == 3) {
                dgvRecentFiles.Rows.RemoveAt(e.RowIndex);
                PlaySound("UIselect");
                Properties.Settings.Default.Recentfiles.RemoveAt(e.RowIndex);
                Properties.Settings.Default.Save();
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
    }
}