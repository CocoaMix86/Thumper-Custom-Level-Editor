﻿namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_WorkSpace
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_WorkSpace));
            this.dockMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.SuspendLayout();
            // 
            // dockMain
            // 
            this.dockMain.BackColor = Color.FromArgb(31, 31, 31);
            this.dockMain.BackgroundImageLayout = ImageLayout.None;
            this.dockMain.Dock = DockStyle.Fill;
            this.dockMain.DockBottomPortion = 0.33D;
            this.dockMain.DockLeftPortion = 0.33D;
            this.dockMain.DockRightPortion = 0.15D;
            this.dockMain.DockTopPortion = 0.33D;
            this.dockMain.Location = new Point(0, 0);
            this.dockMain.Name = "dockMain";
            this.dockMain.ShowAutoHideContentOnHover = false;
            this.dockMain.ShowDocumentIcon = true;
            this.dockMain.Size = new Size(476, 423);
            this.dockMain.TabIndex = 148;
            // 
            // Form_WorkSpace
            // 
            this.ClientSize = new Size(476, 423);
            this.Controls.Add(this.dockMain);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "Form_WorkSpace";
            this.ShowIcon = false;
            this.Text = "Workspace";
            this.ResumeLayout(false);
        }

        #endregion

        public WeifenLuo.WinFormsUI.Docking.DockPanel dockMain;
    }
}