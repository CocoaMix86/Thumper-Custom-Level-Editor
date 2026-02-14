using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public class DockContentEx : DockContent
    {
        public bool RawText { get; set; }
        public bool NoLock { get; set; }
        public FileInfo WorkingFile { 
            get => _workfile; 
            set {
                if (!RawText && value != null && _workfile != null) {
                    TCLE.Documents.Remove(_workfile.Name);
                }

                _workfile = value;

                if (value is not null && !NoLock) {
                    FileLock?.Close();
                    if (!RawText)
                        TCLE.Documents.TryAdd(value.Name, this);
                    try {
                        FileLock = new FileStream(_workfile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    } catch (Exception) {
                        FileLock = null;
                    }
                }
            } 
        }
        private FileInfo _workfile;
        public FileStream FileLock { get; set; }
        public DockContentEx(FileInfo _filetolock, bool rawtext = false)
        {
            if (_filetolock == null)
                return;
            RawText = rawtext;
            WorkingFile = _filetolock;
        }

        protected override string GetPersistString()
        {
            return base.GetPersistString() + ";" + (this.TabText ?? this.Text).Replace("*", "");
        }

        protected override void Dispose(bool disposing)
        {
            FileLock?.Close();
            TCLE.Documents.Remove(WorkingFile?.Name ?? "");
            base.Dispose(disposing);
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
