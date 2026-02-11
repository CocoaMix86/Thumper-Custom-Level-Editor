using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public class DockContentEx : DockContent
    {
        public bool NoLock { get; set; }
        public FileInfo WorkingFile { 
            get => _workfile; 
            set {
                _workfile = value;
                if (value is not null && !NoLock)
                    FileLock = new FileStream(_workfile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            } 
        }
        private FileInfo _workfile;
        public FileStream FileLock { get; set; }
        public DockContentEx(FileInfo _filetolock)
        {
            if (_filetolock == null)
                return;
            WorkingFile = _filetolock;
        }

        protected override string GetPersistString()
        {
            return base.GetPersistString() + ";" + (this.TabText ?? this.Text).Replace("*", "");
        }

        private void InitializeComponent()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            FileLock.Close();
        }
    }

    public class CustomFloatWindow : FloatWindow
    {
        public CustomFloatWindow(DockPanel dockPanel, DockPane pane)
            : base(dockPanel, pane)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            this.Enter += Float_Enter;
            this.GotFocus += Float_Enter;
        }

        public CustomFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            : base(dockPanel, pane, bounds)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            this.Enter += Float_Enter;
            this.GotFocus += Float_Enter;
        }

        public void Float_Enter(object sender, EventArgs e)
        {
            IDockContent focus = this.NestedPanes.Select(x => x.ActiveContent).FirstOrDefault();
            if (focus != null)
                TCLE.GlobalActiveDocument = focus;
        }
    }

    public class CustomFloatWindowFactory : DockPanelExtender.IFloatWindowFactory
    {
        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            return new CustomFloatWindow(dockPanel, pane, bounds);
        }

        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane)
        {
            return new CustomFloatWindow(dockPanel, pane);
        }
    }
}
