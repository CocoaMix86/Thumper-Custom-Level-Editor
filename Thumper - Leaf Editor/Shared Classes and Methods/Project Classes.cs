using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace Thumper_Custom_Level_Editor
{
    public class ProjectProperties
    {
        [Browsable(false)]
        public FileInfo TCL;

        public ProjectProperties()
        {

        }

        [Category("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public string folder => WorkingFolder.FullName;
        [Browsable(false)]
        public DirectoryInfo WorkingFolder; 

        [Category("General Project Info")]
        [DisplayName("Level Name")]
        public string projectname { get; set; }

        [Category("General Project Info")]
        [DisplayName("Author(s)")]
        public string authornames { get; set; }

        [Category("General Project Info")]
        [DisplayName("Difficulty")]
        [Description("")]
        [DefaultValue("D0")]
        [TypeConverter(typeof(DifficultyOptions))]
        public string difficulty { get; set; }

        [Category("General Project Info")]
        [DisplayName("Description")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string description { get; set; }

        [Category("Level Properties")]
        [DisplayName("BPM")]
        [Description("Beats Per Minute. If your song is at 100bpm, you'll likely want to map at either 200 or 400, so you can place objects on half note and quarter note intervals.")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal bpm
        {
            get { return Bpm; }
            set {
                if (value < 1)
                    value = 1;
                if (value > 99999.99m)
                    value = 99999.99m;
                Bpm = value;
                foreach (IDockContent dc in TCLE.Instance.dockMain.Documents) {
                    if (dc.DockHandler.TabText.Contains(".master")) (dc as Form_MasterEditor).propertyGridMaster.Refresh();
                    if (dc.DockHandler.TabText.Contains(".lvl")) (dc as Form_LvlEditor).RecalculateRuntime();
                }
                foreach (SampleData samp in TCLE.ProjectSamples.Where(x => x.Editor != null)) {
                    samp.UpdateRuntime();
                }
            }
        }
        private decimal Bpm;
        
        [Category("Level Properties")]
        [DisplayName("Rail Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color rail { get; set; }

        [Category("Level Properties")]
        [DisplayName("Rail Glow Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color railglow { get; set; }

        [Category("Level Properties")]
        [DisplayName("Path Color")]
        [Description("Affects the rail color on the title screen.")]
        public Color path { get; set; }
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
