using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_WorkSpace : DockContentEx
    {
        private DeserializeDockContent m_deserializeDockContent;

        #region Form Construction
        public Form_WorkSpace(string configtoload = "")
        {
            InitializeComponent();
            dockMain.Theme = new VS2015DarkTheme();
            dockMain.Theme.Extender.FloatWindowFactory = new CustomFloatWindowFactory();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            if (TCLE.IsLoadingProject && configtoload != "") {
                try {
                    dockMain.LoadFromXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{configtoload}.config", m_deserializeDockContent);
                } catch { }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            persistString = persistString.Split(';')[1];
            bool raw = persistString.Contains(" [Raw]");
            persistString = persistString.Replace(" [Raw]", "");

            FileInfo _topopen = ProjectExplorer.Files.FirstOrDefault(x => x.Name.Equals(persistString, StringComparison.OrdinalIgnoreCase));
            if (_topopen != null)
                return TCLE.OpenFile(_topopen, raw, true);
            return null;

            throw new NotImplementedException();
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
            if (this.Disposing)
                return;
            if (dockMain.ActiveContent != null)
                TCLE.GlobalActiveDocument = dockMain.ActiveContent;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.TabText}.config");
        }

        private void dockMain_ActivePaneChanged(object sender, EventArgs e)
        {

        }

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            IDockContent DocClosing = e.Content;
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
                else if (DocClosing.GetType() == typeof(Form_LeafEditor))
                    filetoclose = (DocClosing as Form_LeafEditor).loadedleaf;
                else if (DocClosing.GetType() == typeof(Form_RawText))
                    filetoclose = (DocClosing as Form_RawText).loadedfile;

                if (TCLE.GlobalLastGate == DocClosing)
                    TCLE.GlobalLastGate = null;
                if (TCLE.GlobalLastLvl == DocClosing)
                    TCLE.GlobalLastLvl = null;
                if (TCLE.GlobalLastMaster == DocClosing)
                    TCLE.GlobalLastMaster = null;
                //check if any other tab is open that is the same file
                //if it is, we don't want to close the file lock
                if (filetoclose == null)
                    return;
                foreach (IDockContent document in TCLE.Documents.Where(x => x.DockHandler.TabText.StartsWith(filetoclose.Name))) {
                    return;
                }

                TCLE.CloseFileLock(filetoclose);
                ///TCLE.ProjectExplorer.FindNode(filetoclose.Name, TCLE.ProjectExplorer.treeView1.Nodes[0].Nodes).ForeColor = Properties.Settings.Default.ColorProjExpText;
            }
        }

        private void Form_WorkSpace_Load(object sender, EventArgs e)
        {

        }

        private void Form_WorkSpace_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form_WorkSpace_FormClosing(object sender, FormClosingEventArgs e)
        {
            //When workspace closes, close the file lock on all the files inside it
            foreach (IDockContent doc in this.dockMain.Documents) {
                FileInfo filetoclose = null;
                if (!TCLE.Instance.Disposing) {
                    if (doc.GetType() == typeof(Form_MasterEditor))
                        filetoclose = (doc as Form_MasterEditor).loadedmaster;
                    else if (doc.GetType() == typeof(Form_GateEditor))
                        filetoclose = (doc as Form_GateEditor).loadedgate;
                    else if (doc.GetType() == typeof(Form_LvlEditor))
                        filetoclose = (doc as Form_LvlEditor).loadedlvl;
                    else if (doc.GetType() == typeof(Form_SampleEditor))
                        filetoclose = (doc as Form_SampleEditor).loadedsample;
                    else if (doc.GetType() == typeof(Form_LeafEditor))
                        filetoclose = (doc as Form_LeafEditor).loadedleaf;
                    else if (doc.GetType() == typeof(Form_RawText))
                        filetoclose = (doc as Form_RawText).loadedfile;

                    if (filetoclose == null)
                        continue;

                    TCLE.CloseFileLock(filetoclose);
                }
            }
        }

        private void Form_WorkSpace_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                File.Delete($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.TabText}.config");
            } catch { }
        }

        private void Form_WorkSpace_Shown(object sender, EventArgs e)
        {
        }
    }
}