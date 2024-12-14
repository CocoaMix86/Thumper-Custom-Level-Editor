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
            string filetoclose = "";
            if (!TCLE.Instance.Disposing) {
                if (TCLE.GlobalActiveDocument.GetType() == typeof(Form_MasterEditor))
                    filetoclose = (TCLE.GlobalActiveDocument as Form_MasterEditor).loadedmaster.Name;
                else if (TCLE.GlobalActiveDocument.GetType() == typeof(Form_GateEditor))
                    filetoclose = (TCLE.GlobalActiveDocument as Form_GateEditor).loadedgate.Name;
                else if (TCLE.GlobalActiveDocument.GetType() == typeof(Form_LvlEditor))
                    filetoclose = (TCLE.GlobalActiveDocument as Form_LvlEditor).loadedlvl.Name;
                else if (TCLE.GlobalActiveDocument.GetType() == typeof(Form_SampleEditor))
                    filetoclose = (TCLE.GlobalActiveDocument as Form_SampleEditor).loadedsample.Name;
                else if (TCLE.GlobalActiveDocument.GetType() == typeof(Form_RawText))
                    filetoclose = (TCLE.GlobalActiveDocument as Form_RawText).loadedfile.Name;

                //check if any other tab is open that is the same file
                //if it is, we don't want to close the file lock
                foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(filetoclose))) {
                    return;
                }

                TCLE.CloseFileLock(TCLE.dockProjectExplorer.projectfiles.First(x => x.Key.EndsWith($@"\{filetoclose}")).Value);
            }
        }
    }
}