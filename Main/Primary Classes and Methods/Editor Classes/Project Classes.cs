using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing.Imaging;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class ProjectProperties
    {
        public ProjectProperties()
        {

        }

        [Browsable(false)]
        public FileInfo WorkingFile { 
            get => _workfile;
            set {
                _workfile = value;
                if (value is not null) {
                    FileLock?.Close();
                    FileLock = new FileStream(_workfile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
            } 
        }
        private FileInfo _workfile;
        [Browsable(false)]
        public FileStream FileLock;

        [Category("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string folder => WorkingFolder.FullName;
        [Browsable(false)]
        public DirectoryInfo WorkingFolder => WorkingFile?.Directory; 

        [Category("General Project Info")]
        [DisplayName("Level Name")]
        public string ProjectName
        {
            get => _projectName;
            set {
                if (String.IsNullOrEmpty(value))
                    return;
                _projectName = value;
                TCLE.Instance.toolstripLevelName.Text = value;
            }
        }
        private string _projectName;

        [Category("General Project Info")]
        [DisplayName("Author(s)")]
        public string AuthorNames { get; set; }

        [Category("General Project Info")]
        [DisplayName("Difficulty")]
        [Description("")]
        [DefaultValue("D0")]
        [TypeConverter(typeof(DifficultyOptions))]
        public string Difficulty { get; set; }

        [Category("General Project Info")]
        [DisplayName("Description")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description { get; set; }

        [Category("General Project Info")]
        [DisplayName("Thumbnail")]
        [Description("The image to use in the mod loader when loading this level.")]
        public Image Thumbnail
        {
            get => _thumbnail;
            set {
                _thumbnail = value;
                if (value != null) {
                    if (File.Exists($@"{WorkingFolder}\thumbnail.png"))
                        File.Delete($@"{WorkingFolder}\thumbnail.png");
                    value.Save($@"{WorkingFolder}\thumbnail.png", ImageFormat.Png);
                }
            }
        }
        private Image _thumbnail;

        [Category("Level Properties")]
        [DisplayName("BPM")]
        [Description("Beats Per Minute. If your song is at 100bpm, you'll likely want to map at either 200 or 400, so you can place objects on half note and quarter note intervals.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal BPM
        {
            get { return _bpm; }
            set {
                if (value < 1)
                    value = 1;
                if (value > 999999.99m)
                    value = 999999.99m;
                _bpm = value;
                foreach (EditorBase _tab in TCLE.Documents.Values) {
                    if (_tab is EditorLvl lvl) lvl.RecalculateRuntime();
                    else if (_tab is EditorGate gate) gate.RecalculateRuntime();
                    else if (_tab is EditorMaster master) master.RecalculateRuntime();
                    else if (_tab is EditorLeaf leaf) {
                        foreach (Sequencer_Object seq in leaf.LeafProperties.SequencerObjects) {
                            seq.WaveBitmap = null;
                        }
                        TCLE.alzheimer();
                        leaf.trackEditor.Invalidate();
                    }
                }
                foreach (var samp in TCLE.ProjectSamples.Where(x => x.Value.Editor != null)) {
                    samp.Value.UpdateRuntime();
                }
            }
        }
        private decimal _bpm;
        
        [Category("Level Properties")]
        [DisplayName("Rail Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color RailColor { get; set; }

        [Category("Level Properties")]
        [DisplayName("Rail Glow Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color RailGlowColor { get; set; }

        [Category("Level Properties")]
        [DisplayName("Path Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color PathColor { get; set; }

        [Browsable(false)]
        public List<string> LevelSections = new();
    }

    public class DifficultyOptions : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            List<string> list = new() { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7" };
            return new StandardValuesCollection(list);
        }
    }

}
