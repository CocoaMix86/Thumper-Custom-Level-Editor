using System.DirectoryServices.ActiveDirectory;
using System.Windows.Documents;
using WeifenLuo.WinFormsUI.Docking;

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
        private void dockMain_Enter(object sender, EventArgs e)
        {
            if (dockMain.ActiveContent != null)
                TCLE.GlobalActiveDocument = dockMain.ActiveContent;
        }
        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (dockMain.ActiveContent != null)
                TCLE.GlobalActiveDocument = dockMain.ActiveContent;
        }
        private void dockMain_ActiveContentChanged(object sender, EventArgs e)
        {
            if (dockMain.ActiveContent != null)
                TCLE.GlobalActiveDocument = dockMain.ActiveContent;
        }

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            var DocClosing = e.Content;
            FileInfo filetoclose = null;
            if (!TCLE.Instance.Disposing) {
                if (DocClosing.GetType() == typeof(Form_MasterEditor))
                    filetoclose = (DocClosing as Form_MasterEditor).loadedmaster;
                else if (DocClosing.GetType() == typeof(Form_GateEditor))
                    filetoclose = (DocClosing as Form_GateEditor).loadedgate;
                else if (DocClosing.GetType() == typeof(Form_LvlEditor))
                    filetoclose = (DocClosing as Form_LvlEditor).loadedlvl;
                else if (DocClosing.GetType() == typeof(Form_SampleEditor))
                    filetoclose = (DocClosing as Form_SampleEditor).loadedsample;
                else if (DocClosing.GetType() == typeof(Form_RawText))
                    filetoclose = (DocClosing as Form_RawText).loadedfile;

                //check if any other tab is open that is the same file
                //if it is, we don't want to close the file lock
                foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(filetoclose.Name))) {
                    return;
                }

                TCLE.CloseFileLock(filetoclose);
            }
        }
    }
}