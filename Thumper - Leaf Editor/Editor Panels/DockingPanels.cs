using ControlManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor
    {
        #region Event Handlers
        ///https://stackoverflow.com/questions/6521731/refresh-the-panels-of-a-splitcontainer-as-the-splitter-moves
        //assign this to the SplitContainer's MouseDown event
        private void splitCont_MouseDown(object sender, MouseEventArgs e)
        {
            // This disables the normal move behavior
            ((SplitContainer)sender).IsSplitterFixed = true;
        }
        //assign this to the SplitContainer's MouseUp event
        private void splitCont_MouseUp(object sender, MouseEventArgs e)
        {
            // This allows the splitter to be moved normally again
            ((SplitContainer)sender).IsSplitterFixed = false;
        }
        //assign this to the SplitContainer's MouseMove event
        private void splitCont_MouseMove(object sender, MouseEventArgs e)
        {
            // Check to make sure the splitter won't be updated by the
            // normal move behavior also
            if (((SplitContainer)sender).IsSplitterFixed) {
                // Make sure that the button used to move the splitter
                // is the left mouse button
                if (e.Button.Equals(MouseButtons.Left)) {
                    // Checks to see if the splitter is aligned Vertically
                    if (((SplitContainer)sender).Orientation.Equals(Orientation.Vertical)) {
                        // Only move the splitter if the mouse is within
                        // the appropriate bounds
                        if (e.X > 0 && e.X < ((SplitContainer)sender).Width) {
                            // Move the splitter & force a visual refresh
                            ((SplitContainer)sender).SplitterDistance = e.X;
                            ((SplitContainer)sender).Refresh();
                        }
                    }
                    // If it isn't aligned vertically then it must be
                    // horizontal
                    else {
                        // Only move the splitter if the mouse is within
                        // the appropriate bounds
                        if (e.Y > 0 && e.Y < ((SplitContainer)sender).Height) {
                            // Move the splitter & force a visual refresh
                            ((SplitContainer)sender).SplitterDistance = e.Y;
                            ((SplitContainer)sender).Refresh();
                        }
                    }
                }
                // If a button other than left is pressed or no button
                // at all
                else {
                    // This allows the splitter to be moved normally again
                    ((SplitContainer)sender).IsSplitterFixed = false;
                }
            }
        }
        private void lblPopout_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).BackColor = Color.Aqua;
            (sender as Label).BorderStyle = BorderStyle.Fixed3D;
        }

        private void lblPopout_MouseLeave(object sender, EventArgs e)
        {
            (sender as Label).BackColor = Color.FromArgb(55, 55, 55);
            (sender as Label).BorderStyle = BorderStyle.FixedSingle;
        }

        private void lblPopout_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            //get parent panel where lbl was clicked
            var parent = lbl.Parent;
            //find what dock it came from and remove the setting
            var dock = parent.Parent;
            if (dock == splitTop1.Panel1) {
                Properties.Settings.Default.dock1 = "empty";
            }
            if (dock == splitTop2.Panel1) {
                Properties.Settings.Default.dock2 = "empty";
            }
            if (dock == splitTop2.Panel2) {
                Properties.Settings.Default.dock3 = "empty";
            }
            if (dock == splitBottom1.Panel1) {
                Properties.Settings.Default.dock4 = "empty";
            }
            if (dock == splitBottom2.Panel1) {
                Properties.Settings.Default.dock5 = "empty";
            }
            if (dock == splitBottom2.Panel2) {
                Properties.Settings.Default.dock6 = "empty";
            }
            //remove panel from splitter
            parent.Dock = DockStyle.None;
            parent.Size = new Size(dock.Width, dock.Height);
            this.Controls.Add(parent);
            parent.Location = new Point(MousePosition.X - parent.Width + 35, MousePosition.Y - 25);
            //
            parent.BringToFront();
            lbl.Click -= lblPopout_Click;
            lbl.Click += lblPopin_Click;
            //change tooltip
            toolTip1.SetToolTip(lbl, "Dock panel");
        }
        private void lblPopin_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            //get parent panel where lbl was clicked
            var parent = lbl.Parent;
            //re-add panel to a splitter panel so it is now docked.
            //check all 6 and add to the earliest one
            //also set what dock the panel is in, so it is remembered on restart
            if (splitTop1.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitTop1.Panel1);
                Properties.Settings.Default.dock1 = parent.Name;
            }
            else if (splitTop2.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitTop2.Panel1);
                Properties.Settings.Default.dock2 = parent.Name;
            }
            else if (splitTop2.Panel2.Controls.Count == 1) {
                DockPanel(parent, splitTop2.Panel2);
                Properties.Settings.Default.dock3 = parent.Name;
            }
            else if (splitBottom1.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitBottom1.Panel1);
                Properties.Settings.Default.dock4 = parent.Name;
            }
            else if (splitBottom2.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitBottom2.Panel1);
                Properties.Settings.Default.dock5 = parent.Name;
            }
            else if (splitBottom2.Panel2.Controls.Count == 1) {
                DockPanel(parent, splitBottom2.Panel2);
                Properties.Settings.Default.dock6 = parent.Name;
            }
            Properties.Settings.Default.Save();
        }
        #endregion


        #region Methods
        private void DockPanel(Control panel, Control dock)
        {
            dock.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
            //locate the dock button in the panel
            var dockbtn = panel.Controls.OfType<Label>().Where(x => x.Text == "▲").First();
            //then change its click event and tooltip
            dockbtn.Click -= lblPopin_Click;
            dockbtn.Click += lblPopout_Click;
            //change tooltip
            toolTip1.SetToolTip(dockbtn, "Undock panel");
        }

        private void dockbtn_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            c.ContextMenuStrip.Show(c, new Point(5, 5));
        }

        private void contextdockLvl_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}