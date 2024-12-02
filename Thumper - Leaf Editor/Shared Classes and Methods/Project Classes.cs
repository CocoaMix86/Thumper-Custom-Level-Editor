using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor
{
    public class ProjectProperties
    {
        public ProjectProperties(dynamic _load)
        {
            projectname = (string)_load["level_name"];
            difficulty = (string)_load["difficulty"];
            description = (string)_load["description"];
            authornames = (string)_load["author"];
            bpm = (decimal)_load["bpm"];
            dynamic railcolor = _load["rails_color"];
            rail = Color.FromArgb((int)(railcolor[0] * 255), (int)(railcolor[1] * 255), (int)(railcolor[2] * 255));
            dynamic railglowcolor = _load["rails_glow_color"];
            railglow = Color.FromArgb((int)(railglowcolor[0] * 255), (int)(railglowcolor[1] * 255), (int)(railglowcolor[2] * 255));
            dynamic pathcolor = _load["path_color"];
            path = Color.FromArgb((int)(pathcolor[0] * 255), (int)(pathcolor[1] * 255), (int)(pathcolor[2] * 255));
        }
        [Category("General")]
        [DisplayName("File Path")]
        [Description("The full path to this file.")]
        public static string filepath { get { return TCLE.WorkingFolder.FullName; } }

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
        public string difficulty { get { return _difficulty; } set { _difficulty = value; } }
        private string _difficulty = null;

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
                if (value < 0)
                    value = 1;
                if (value > 99999.99m)
                    value = 99999.99m;
                Bpm = value;
                foreach (var dc in TCLE.Instance.dockMain.Documents) {
                    if (dc.DockHandler.TabText.Contains(".master")) (dc as Form_MasterEditor).propertyGridMaster.Refresh();
                    if (dc.DockHandler.TabText.Contains(".lvl")) (dc as Form_LvlEditor).RecalculateRuntime();
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
            List<string> list = new List<string>() { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7" };
            return new StandardValuesCollection(list);
        }
    }

}
