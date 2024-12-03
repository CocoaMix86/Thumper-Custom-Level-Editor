﻿using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_WorkSpace : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        public Form_WorkSpace()
        {
            InitializeComponent();
            dockMain.Theme = new VS2015DarkTheme();
        }
        #endregion

        private void dockMain_ContentAdded(object sender, DockContentEventArgs e) => e.Content.DockHandler.TabPageContextMenuStrip = TCLE.TabRightClickMenu;
        private void dockMain_Enter(object sender, EventArgs e) => TCLE.GlobalActiveDocument = dockMain.ActiveContent;
        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e) => TCLE.GlobalActiveDocument = dockMain.ActiveContent;
        private void dockMain_ActiveContentChanged(object sender, EventArgs e) => TCLE.GlobalActiveDocument = dockMain.ActiveContent;

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (!TCLE.Instance.Disposing)
                TCLE.CloseFileLock(TCLE.dockProjectExplorer.projectfiles.Where(x => x.Value.Name == e.Content.DockHandler.TabText).First().Value);
        }
    }
}