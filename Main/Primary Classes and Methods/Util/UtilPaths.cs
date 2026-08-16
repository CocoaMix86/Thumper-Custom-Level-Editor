namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilPaths
    {
        public static DirectoryInfo DirTemp = new(Path.Combine(TCLE.AppLocation, "temp"));
        public static string Temp => DirTemp.FullName;
        public static DirectoryInfo DirTemplates = new(Path.Combine(TCLE.AppLocation, "templates"));
        public static string Templates => DirTemplates.FullName;
        public static DirectoryInfo DirSettings = new(Path.Combine(TCLE.AppLocation, "settings"));
        public static string Settings => DirSettings.FullName;
        public static DirectoryInfo DirSettingsProjects = new(Path.Combine(TCLE.AppLocation, "settings", "projects"));
        public static string SettingsProjects => DirSettingsProjects.FullName;
        public static DirectoryInfo DirCurrentProjectSettings => new(Path.Combine(SettingsProjects, TCLE.WorkingFolder?.Name));
        public static string CurrentProjectSettings => DirCurrentProjectSettings.FullName;
        public static string ProjectNotes => Path.Combine(TCLE.WorkingFolder.FullName, "ProjectNotes.txt");
    }
}
