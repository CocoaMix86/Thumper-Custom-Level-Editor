using System.Diagnostics;
using Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE
    {
        private void label2_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = "https://github.com/CocoaMix86/Thumper-Custom-Level-Editor/wiki/TCLE-3.0#first-time-in-tcle", UseShellExecute = true });

        private void RecentFiles(List<string> recentfiles)
        {
            dgvRecentFiles.Rows.Clear();
            panelRecentFiles.Visible = true;
            panelRecentFiles.BringToFront();
            foreach (string _projectpath in recentfiles) {
                FileInfo tcl = new(_projectpath);
                dynamic _tclinfo = UtilFile.LoadFileLock(_projectpath);
                if (_tclinfo is null) {
                    dgvRecentFiles.Rows.Add("D0", Path.GetFileName(_projectpath), "some error occurred when trying to load the project details.");
                }
                else
                    dgvRecentFiles.Rows.Add((string)_tclinfo["difficulty"], tcl.Name.Replace(".TCL", "", StringComparison.OrdinalIgnoreCase), _projectpath);
            }
            dgvRecentFiles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private int _imagetarget = rng.Next(10, 30);
        private int _imagecounter = 0;
        private void MainMenu_MouseEnter(object sender, EventArgs e)
        {
            if (sender == pictureProjectOpen) {
                pictureProjectOpen.Image = Properties.Resources.projectopen;
                labelProjectOpen.ForeColor = Color.Gold;
                labelProjectOpen.Font = new Font("Futura PT Heavy", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
                labelProjectOpen.Location = new(159 - (labelProjectOpen.Width / 2), labelProjectOpen.Location.Y);
            }
            else if (sender == pictureProjectNew) {
                pictureProjectNew.Image = Properties.Resources.projectnew;
                labelProjectNew.ForeColor = Color.Green;
                labelProjectNew.Font = new Font("Futura PT Heavy", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
                labelProjectNew.Location = new(319 - (labelProjectNew.Width / 2), labelProjectOpen.Location.Y);
            }
            else if (sender == pictureOptions) {
                _imagecounter++;
                pictureOptions.Image = _imagecounter == _imagetarget ? Properties.Resources.thisisforberry : Properties.Resources.options;
                labelOptions.ForeColor = Color.Aquamarine;
                labelOptions.Font = new Font("Futura PT Heavy", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
                labelOptions.Location = new(478 - (labelOptions.Width / 2), labelProjectOpen.Location.Y);
            }
        }

        private void MainMenu_MouseLeave(object sender, EventArgs e)
        {
            if (sender == pictureProjectOpen) {
                pictureProjectOpen.Image = Properties.Resources.projectopen_gray;
                labelProjectOpen.ForeColor = Color.FromArgb(150, 150, 150);
                labelProjectOpen.Font = new Font("Futura PT Book", 12F);
                labelProjectOpen.Location = new(159 - (labelProjectOpen.Width / 2), labelProjectOpen.Location.Y);
            }
            else if (sender == pictureProjectNew) {
                pictureProjectNew.Image = Properties.Resources.projectnew_gray;
                labelProjectNew.ForeColor = Color.FromArgb(150, 150, 150);
                labelProjectNew.Font = new Font("Futura PT Book", 12F);
                labelProjectNew.Location = new(319 - (labelProjectNew.Width / 2), labelProjectOpen.Location.Y);
            }
            else if (sender == pictureOptions) {
                pictureOptions.Image = Properties.Resources.options_gray;
                labelOptions.ForeColor = Color.FromArgb(150, 150, 150);
                labelOptions.Font = new Font("Futura PT Book", 12F);
                labelOptions.Location = new(478 - (labelOptions.Width / 2), labelProjectOpen.Location.Y);
            }
        }

        private int RecentFilesRowHover = -1;
        private int RecentFilesColumnHover = -1;
        private void dgvRecentFiles_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            RecentFilesRowHover = e.RowIndex;
            RecentFilesColumnHover = e.ColumnIndex;
            dgvRecentFiles.Invalidate();
        }

        private void dgvRecentFiles_MouseLeave(object sender, EventArgs e)
        {
            RecentFilesRowHover = -1;
            dgvRecentFiles.Invalidate();
        }

        private void dgvRecentFiles_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            //button is in column 0, so that's where to draw the image
            
            if (e.ColumnIndex == 0) {
                //get dimensions
                int w = 30;
                int h = 30;
                int x = e.CellBounds.Left + ((e.CellBounds.Width - w) / 2);
                int y = e.CellBounds.Top + ((e.CellBounds.Height - h) / 2);
                //paint the image
                e.Graphics.DrawImage((Bitmap)Properties.Resources.ResourceManager.GetObject($"difficulty_{dgvRecentFiles[e.ColumnIndex, e.RowIndex].Value}"), new Rectangle(x, y, w, h));
                e.Handled = true;
            }
            
            if (e.ColumnIndex == 2) {
                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
                e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
            }
            //button is in column 3, so that's where to draw the image
            if (e.ColumnIndex == 3) {
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

        private void dgvRecentFiles_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.Handled = true;
            Rectangle bounds = e.RowBounds;
            bounds.X += 2;
            bounds.Y += 2;
            bounds.Width -= 4;
            bounds.Height -= 4;
            e.Graphics.FillRectangle(Brushes.Black, e.RowBounds);
            DataGridView dgv = sender as DataGridView;

            if (e.RowIndex == RecentFilesRowHover)
                e.Graphics.FillRoundedRectangle(Brushes.White, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 2), 8);
            e.Graphics.FillRoundedRectangle(new SolidBrush(UtilMath.Blend((RecentFilesColumnHover == 3 && RecentFilesRowHover == e.RowIndex) ? Color.Crimson : Color.Aqua, Color.Black, (e.RowIndex == RecentFilesRowHover ? 1 : 0.6))), bounds, 8);

            e.PaintCells(e.RowBounds, DataGridViewPaintParts.ContentForeground);
        }

        private void dgvRecentFiles_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex is -1 || e.ColumnIndex is 3)
                return;

            FileInfo level = new((string)dgvRecentFiles.Rows[e.RowIndex].Cells[2].Value ?? "");
            //don't reopen the project if it's already open
            if (WorkingFolder?.FullName == level.DirectoryName) {
                panelRecentFiles.Visible = false;
                return;
            }
            //check if the folder still exists.
            if (!level.Exists) {
                if (MessageBox.Show($"Recent Level selected no longer exists at that location\n{level.FullName}\n\nDo you want to remove this entry?", "Level Custom Thumper Editor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    RemoveRecentLevel(e.RowIndex);
                return;
            }
            //open the project if everything else above clears
            panelRecentFiles.Visible = false;
            UtilAudio.PlaySound("UIfolderclose");
            OpenProject(level);

        }

        private void dgvRecentFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            FileInfo level = new(dgvRecentFiles.Rows[e.RowIndex].Cells[2].Value.ToString());
            //if remove column button clicked, run this
            if (e.ColumnIndex == 3) {
                RemoveRecentLevel(e.RowIndex);
            }
        }

        private void btnRecentClose_Click(object sender, EventArgs e)
        {
            UtilAudio.PlaySound("UIfolderclose");
            MenusVisible(true);
        }

        private void panelRecentClick(object sender, EventArgs e)
        {
            panelRecentFiles.BringToFront();
        }

        private void RemoveRecentLevel(int index)
        {
            dgvRecentFiles.Rows.RemoveAt(index);
            Properties.Settings.Default.Recentfiles.RemoveAt(index);
            Properties.Settings.Default.Save();
            UtilAudio.PlaySound("UIselect");
        }
    }
}