using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_WorkSpace : DockContentEx
    {
        private DeserializeDockContent m_deserializeDockContent;

        #region Form Construction
        public Form_WorkSpace(string configtoload)
        {
            InitializeComponent();
            this.Text = configtoload;
            this.TabText = configtoload;
            dockMain.Theme = new VS2015DarkTheme();
            dockMain.Theme.Extender.FloatWindowFactory = new CustomFloatWindowFactory();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            if (TCLE.IsLoadingProject && !string.IsNullOrEmpty(configtoload)) {
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

        private void dockMain_ContentAdded(object sender, DockContentEventArgs e)
        {
            e.Content.DockHandler.TabPageContextMenuStrip = TCLE.TabRightClickMenu;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void dockMain_Enter(object sender, EventArgs e)
        {
            if (TCLE.DontSwitchGAD)
                return;
            if (this.Disposing)
                return;
            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = dockMain.ActiveDocument;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }
        private void dockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (TCLE.DontSwitchGAD)
                return;
            if (this.Disposing)
                return;
            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = dockMain.ActiveDocument;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }
        private void dockMain_ActiveContentChanged(object sender, EventArgs e)
        {
            if (TCLE.DontSwitchGAD)
                return;
            if (this.Disposing)
                return;
            if (dockMain.ActiveDocument != null && TCLE.GlobalActiveDocument != dockMain.ActiveDocument)
                TCLE.GlobalActiveDocument = dockMain.ActiveDocument;
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void dockMain_ActivePaneChanged(object sender, EventArgs e)
        {
            if (TCLE.DontSwitchGAD)
                return;
            if (this.Disposing)
                return;
            //if (dockMain.ActivePane == null) {
            //if (dockMain.ActiveContent == null)
            //if (dockMain.Panes.Count > 0)
            //    dockMain.Panes[0].Activate();
            //TCLE.GlobalActiveDocument = dockMain.ActiveContent;
            //}
            dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
        }

        private void dockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            IDockContent DocClosing = e.Content;
            FileInfo filetoclose = null;
            if (!TCLE.Instance.Disposing) {
                if (DocClosing.GetType() == typeof(Form_MasterEditor))
                    filetoclose = (DocClosing as Form_MasterEditor).MasterProperties.LoadedMaster;
                else if (DocClosing.GetType() == typeof(Form_GateEditor))
                    filetoclose = (DocClosing as Form_GateEditor).loadedgate;
                else if (DocClosing.GetType() == typeof(Form_LvlEditor))
                    filetoclose = (DocClosing as Form_LvlEditor).loadedlvl;
                else if (DocClosing.GetType() == typeof(Form_SampleEditor))
                    filetoclose = (DocClosing as Form_SampleEditor).loadedsample;
                else if (DocClosing.GetType() == typeof(Form_LeafEditor))
                    filetoclose = (DocClosing as Form_LeafEditor).LeafProperties.LoadedLeaf;
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
                dockMain.SaveAsXml($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
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
            //check for unsaved files
            if (TCLE.AnyUnsaved()) {
                if (MessageBox.Show("Some files in this workspace are unsaved. Do you still want to close this?", "Thumper Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                    return;
                }
            }
            //When workspace closes, close the file lock on all the files inside it
            foreach (IDockContent doc in this.dockMain.Documents) {
                FileInfo filetoclose = null;
                if (!TCLE.Instance.Disposing) {
                    if (doc.GetType() == typeof(Form_MasterEditor))
                        filetoclose = (doc as Form_MasterEditor).MasterProperties.LoadedMaster;
                    else if (doc.GetType() == typeof(Form_GateEditor))
                        filetoclose = (doc as Form_GateEditor).loadedgate;
                    else if (doc.GetType() == typeof(Form_LvlEditor))
                        filetoclose = (doc as Form_LvlEditor).loadedlvl;
                    else if (doc.GetType() == typeof(Form_SampleEditor))
                        filetoclose = (doc as Form_SampleEditor).loadedsample;
                    else if (doc.GetType() == typeof(Form_LeafEditor))
                        filetoclose = (doc as Form_LeafEditor).LeafProperties.LoadedLeaf;
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
                File.Delete($@"{TCLE.AppLocation}\settings\projects\{TCLE.WorkingFolder.Name}\layout_{this.Text}.config");
            } catch { }
        }

        private void Form_WorkSpace_Shown(object sender, EventArgs e)
        {
        }
    }
}