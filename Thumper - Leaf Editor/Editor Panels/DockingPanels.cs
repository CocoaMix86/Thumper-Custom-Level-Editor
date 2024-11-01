﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    public partial class TCLE : Form
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
        }
        //assign this to the SplitContainer's MouseMove event
        private void splitCont_MouseMoveHorz(object sender, MouseEventArgs e)
        {
            // Make sure that the button used to move the splitter
            // is the left mouse button
            if (e.Button.Equals(MouseButtons.Left)) {
                // Only move the splitter if the mouse is within
                // the appropriate bounds
                if (e.Y > 0 && e.Y < splitHorizontal.Height) {
                    // Move the splitter & force a visual refresh
                    splitHorizontal.SplitterDistance = e.Y;
                    splitHorizontal.SuspendLayout();
                    splitHorizontal.Refresh();
                    splitHorizontal.ResumeLayout();
                }
            }
        }

        public void ShowPanel(bool visible, Control panel)
        {
            panel.Visible = visible;
            panel.BringToFront();
        }

        private void lblPopout_Click(object sender, EventArgs e)
        {
            ToolStripButton lbl = (ToolStripButton)sender;
            //get parent panel where lbl was clicked
            Control parent = lbl.Owner.Parent;
            UndockPanel(parent, true);
        }
        private void lblPopin_Click(object sender, EventArgs e)
        {
            ToolStripButton lbl = (ToolStripButton)sender;
            //get parent panel where lbl was clicked
            Control parent = lbl.Owner.Parent;
            //re-add panel to a splitter panel so it is now docked.
            //check all 6 and add to the earliest one
            //also set what dock the panel is in, so it is remembered on restart
            if (splitTop1.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitTop1.Panel1, true);
                Properties.Settings.Default.dock1 = parent.Name;
            }
            else if (splitTop2.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitTop2.Panel1, true);
                Properties.Settings.Default.dock2 = parent.Name;
            }
            else if (splitTop2.Panel2.Controls.Count == 1) {
                DockPanel(parent, splitTop2.Panel2, true);
                Properties.Settings.Default.dock3 = parent.Name;
            }
            else if (splitBottom1.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitBottom1.Panel1, true);
                Properties.Settings.Default.dock4 = parent.Name;
            }
            else if (splitBottom2.Panel1.Controls.Count == 1) {
                DockPanel(parent, splitBottom2.Panel1, true);
                Properties.Settings.Default.dock5 = parent.Name;
            }
            else if (splitBottom2.Panel2.Controls.Count == 1) {
                DockPanel(parent, splitBottom2.Panel2, true);
                Properties.Settings.Default.dock6 = parent.Name;
            }
            Properties.Settings.Default.Save();
        }

        private void dockbtn_Click(object sender, EventArgs e)
        {
            PlaySound("UIselect");
            Control c = (Control)sender;
            c.ContextMenuStrip.Show(c, new Point(5, 5));
        }

        private void contextdockLvl_Click(object sender, EventArgs e)
        {
            Control parentdock = null;
            ToolStripItem item = (sender as ToolStripItem);
            string text = item.Text;
            //this block finds what button called the contextmenu, and gets its parent
            //will use the parent to then dock the selected panel
            if (item != null) {
                if (item.Owner is ContextMenuStrip owner) {
                    parentdock = owner.SourceControl;
                }
            }
            if (parentdock.GetType() == typeof(Label))
                parentdock = parentdock.Parent;
            //set text to the exact panel name
            if (text == "Leaf Editor")
                text = "panelLeaf";
            if (text == "Lvl Editor")
                text = "panelLevel";
            if (text == "Gate Editor")
                text = "panelGate";
            if (text == "Master Editor")
                text = "panelMaster";
            if (text == "Sample Editor")
                text = "panelSample";
            if (text == "Working Folder")
                text = "panelWorkingFolder";
            //search for panel and add it to the dock
            DockPanel(this.Controls.Find(text, true).First(), parentdock, true);
        }

        private void resetMenuPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndockPanel(panelLeaf);
            UndockPanel(panelLevel);
            UndockPanel(panelGate);
            UndockPanel(panelMaster);
            UndockPanel(panelWorkingFolder);
            UndockPanel(panelSample);

            panelMaster.Location = new Point(0, 30);
            panelLevel.Location = new Point(this.Width / 3, 30);
            panelGate.Location = new Point((int)(this.Width * 0.66), 30);
            panelWorkingFolder.Location = new Point(0, this.Height / 2);
            panelLeaf.Location = new Point(this.Width / 3, this.Height / 2);
            panelSample.Location = new Point((int)(this.Width * 0.66), this.Height / 2);

            panelLeaf.Visible = panelLevel.Visible = panelGate.Visible = panelMaster.Visible = panelWorkingFolder.Visible = panelSample.Visible = true;

            ResetBeeble(null, null);
        }

        private void resetDocksStripMenuItem_Click(object sender, EventArgs e)
        {
            DockPanel(panelMaster, splitTop1.Panel1);
            DockPanel(panelLevel, splitTop2.Panel1);
            DockPanel(panelGate, splitTop2.Panel2);
            DockPanel(panelWorkingFolder, splitBottom1.Panel1);
            DockPanel(panelLeaf, splitBottom2.Panel1);
            DockPanel(panelSample, splitBottom2.Panel2);

            splitHorizontal.SplitterDistance = (int)(splitHorizontal.Height / 2.5);
            splitTop1.SplitterDistance = (int)(splitTop1.Width / 4);
            splitTop2.SplitterDistance = (int)(splitTop2.Width / 1.3);
            splitBottom1.SplitterDistance = (int)(splitBottom1.Width / 6);
            splitBottom2.SplitterDistance = (int)(splitBottom2.Width / 1.3);

            leafEditorToolStripMenuItem.Checked = true;
            levelEditorToolStripMenuItem.Checked = true;
            gateEditorToolStripMenuItem.Checked = true;
            masterEditorToolStripMenuItem.Checked = true;
            workingFolderToolStripMenuItem.Checked = true;
            sampleEditorToolStripMenuItem.Checked = true;
            //splitter distances
            Properties.Settings.Default.splitterHorz1 = splitHorizontal.SplitterDistance;
            Properties.Settings.Default.splitterVert1 = splitTop1.SplitterDistance;
            Properties.Settings.Default.splitterVert2 = splitTop2.SplitterDistance;
            Properties.Settings.Default.splitterVert3 = splitBottom1.SplitterDistance;
            Properties.Settings.Default.splitterVert4 = splitBottom2.SplitterDistance;
            Properties.Settings.Default.Save();
            settings = Properties.Settings.Default;

            ResetBeeble(null, null);
            panelRecentFiles.Location = new Point(100, 100);
        }

        private void ResetBeeble(object sender, EventArgs e)
        {
            pictureBeeble.Location = new Point(50, 50);
            pictureBeeble.Size = new Size(96, 76);
            PlaySound("UIwindowopen");
        }

        private void editorpanelDoubleClick(object sender, EventArgs e)
        {
            Control _editorpanel = ((Control)sender).Parent;
            if (!this.Controls.Contains(_editorpanel))
                UndockPanel(_editorpanel, true);
        }
        #endregion


        #region Methods
        public void DockPanel(Control panel, Control dock, bool playsound = false)
        {
            dock.Controls.Add(panel);
            dock.Paint -= splitPanel_Paint;
            dock.ContextMenuStrip = null;
            panel.Dock = DockStyle.Fill;
            //locate the dock button in the panel
            //ToolStripButton dockbtn = panel.Controls.OfType<ToolStrip>().Where(x => x.Text == "titlebar").First().Items[3] as ToolStripButton;
            //then change its click event and tooltip
            //dockbtn.Click -= lblPopin_Click;
            //dockbtn.Click += lblPopout_Click;
            //change tooltip
            //dockbtn.ToolTipText = "Undock panel";
            //toolTip1.SetToolTip(dockbtn, "Undock panel");
            ShowPanel(true, panel);

            //save settings
            if (dock == splitTop1.Panel1)
                Properties.Settings.Default.dock1 = panel.Name;
            if (dock == splitTop2.Panel1)
                Properties.Settings.Default.dock2 = panel.Name;
            if (dock == splitTop2.Panel2)
                Properties.Settings.Default.dock3 = panel.Name;
            if (dock == splitBottom1.Panel1)
                Properties.Settings.Default.dock4 = panel.Name;
            if (dock == splitBottom2.Panel1)
                Properties.Settings.Default.dock5 = panel.Name;
            if (dock == splitBottom2.Panel2)
                Properties.Settings.Default.dock6 = panel.Name;
            Properties.Settings.Default.Save();

            if (playsound) PlaySound("UIdock");
            //ControlMoverOrResizer.Dispose(panel);
            //ControlMoverOrResizer.Dispose(dockbtn.Owner);
            //((Panel)panel).BorderStyle = BorderStyle.None;
        }

        public void UndockPanel(Control panel, bool playsound = false)
        {
            if (panel.Parent.GetType() != typeof(SplitterPanel))
                return;

            Control dock = panel.Parent;
            dock.Paint += splitPanel_Paint;
            dock.ContextMenuStrip = contextMenuDock;
            panel.Dock = DockStyle.None;
            this.Controls.Add(panel);
            panel.Size = new Size(dock.Width, dock.Height);
            panel.Location = new Point(this.PointToClient(Cursor.Position).X - panel.Width + 150, this.PointToClient(Cursor.Position).Y - 10);
            //locate the dock button in the panel
            //ToolStripButton dockbtn = panel.Controls.OfType<ToolStrip>().Where(x => x.Text == "titlebar").First().Items[3] as ToolStripButton;
            //then change its click event and tooltip
            //dockbtn.Click += lblPopin_Click;
            //dockbtn.Click -= lblPopout_Click;
            //change tooltip
            //dockbtn.ToolTipText = "Dock panel";
            ShowPanel(true, panel);

            //save settings
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
            Properties.Settings.Default.Save();

            if (playsound) PlaySound("UIdockun");
            //ControlMoverOrResizer.InitResizer(panel);
            //ControlMoverOrResizer.InitMover(dockbtn.Owner);
            //((Panel)panel).BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetDockLocations()
        {
            if (settings.firstrun) {
                resetDocksStripMenuItem_Click(null, null);
                return;
            }
            //set dock locations for panels
            /*
            if (settings.dock1 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock1, true).First();
                DockPanel(_c, splitTop1.Panel1);
            }
            if (settings.dock2 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock2, true).First();
                DockPanel(_c, splitTop2.Panel1);
            }
            if (settings.dock3 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock3, true).First();
                DockPanel(_c, splitTop2.Panel2);
            }
            if (settings.dock4 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock4, true).First();
                DockPanel(_c, splitBottom1.Panel1);
            }
            if (settings.dock5 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock5, true).First();
                DockPanel(_c, splitBottom2.Panel1);
            }
            if (settings.dock6 != "empty") {
                Control _c = this.Controls.Find(Properties.Settings.Default.dock6, true).First();
                DockPanel(_c, splitBottom2.Panel2);
            }
            */
        }
        #endregion
    }
}