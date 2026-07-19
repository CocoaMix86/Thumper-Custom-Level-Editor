using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilFile
    {
        public static void WriteFileLock(string fs, string _save)
        {
            using (StreamWriter sr = new(new FileStream(fs, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), System.Text.Encoding.UTF8, _save.Length, true)) {
                sr.Write(_save);
            }
        }

        public static void WriteFileLock(FileStream fs, string _save)
        {
            using (StreamWriter sr = new(fs, System.Text.Encoding.UTF8, _save.Length, true)) {
                fs.Position = 0;
                fs.SetLength(0);
                sr.Write(_save);
            }
        }

        public static void WriteFileLock(string fs, JObject _save)
        {
            WriteFileLock(fs, JsonConvert.SerializeObject(_save, Formatting.Indented));
        }

        public static void WriteFileLock(FileStream fs, JObject _save)
        {
            WriteFileLock(fs, JsonConvert.SerializeObject(_save, Formatting.Indented));
        }

        public static dynamic LoadFileLock(string _selectedfilename, bool LoadText = false)
        {
            if (!File.Exists(_selectedfilename))
                return null;
            ///reference:
            ///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
            using (FileStream fileStream = new(_selectedfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader textReader = new(fileStream)) {
                if (LoadText) {
                    return textReader.ReadToEnd();
                }
                else {
                    try {
                        return JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
                    } catch (Exception) {
                        MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.", "File load error");
                        return null;
                    }
                }
            }
        }
        /*
        public static void DeleteFileLock(FileInfo filetodelete)
        {
            if (lockedfiles.TryGetValue(filetodelete, out FileStream? value)) {
                value.Close();
                lockedfiles.Remove(filetodelete);
            }
            filetodelete.Delete();
            TCLE.FindEditorRunMethod(typeof(Form_LvlEditor), "RecalculateRuntime");
            TCLE.FindEditorRunMethod(typeof(Form_GateEditor), "RecalculateRuntime");
            TCLE.FindEditorRunMethod(typeof(Form_MasterEditor), "RecalculateRuntime");
        }*/
        /*
        public static void CloseFileLock(FileInfo filetoclose)
        {
            if (filetoclose == null)
                return;
            if (lockedfiles.TryGetValue(filetoclose, out FileStream? value)) {
                value.Close();
                lockedfiles.Remove(filetoclose);
            }
        }*/
        /*
        public static void ClearFileLock()
        {
            //clear previously locked files
            foreach (KeyValuePair<FileInfo, FileStream> i in lockedfiles) {
                i.Value.Close();
            }
            lockedfiles.Clear();
        }*/
        /// 

        public static string CopyToWorkingFolderCheck(string filepath)
        {
            if (TCLE.WorkingFolder == null)
                return filepath;

            FileInfo _input = new(filepath);
            if (!_input.DirectoryName.Contains(TCLE.WorkingFolder.FullName, StringComparison.OrdinalIgnoreCase)) {
                DialogResult result = MessageBox.Show("That file does not exist in the current Project. Do you want to copy it here?", "Bumper Custom Level Editor", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) {
                    string dest = null;
                    if (!File.Exists($@"{TCLE.WorkingFolder}\{_input.Name}"))
                        dest = $@"{TCLE.WorkingFolder}\{_input.Name}";
                    else
                        dest = $@"{TCLE.WorkingFolder}\{_input.Name} ({TCLE.WorkingFolder.GetFiles($"{Path.GetFileNameWithoutExtension(filepath)}*").Length + 1})";
                    File.Copy(filepath, dest);
                    filepath = dest;
                    ProjectExplorer.CreateTreeView();
                }
                else
                    filepath = null;
            }

            return filepath;
        }

        ///
        ///https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            DirectoryInfo dir = new(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles()) {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive) {
                foreach (DirectoryInfo subDir in dirs) {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public static string SearchReferences(string searchreference)
        {
            string referencefiles = "";
            //search all files in the project folder
            //looking only in approved extensions as to not search massive audio files
            foreach (FileInfo file in TCLE.WorkingFolder.GetFiles("*", SearchOption.AllDirectories).Where(x => TCLE.ProjectExtensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase))) {
                //skip self to not include self
                if (file.Name == searchreference)
                    continue;
                string text = ((JObject)UtilFile.LoadFileLock(file.FullName)).ToString(Formatting.None);
                //check if the file we're searching contains the obj_name
                if (text.Contains(searchreference)) {
                    referencefiles += file.Name + '\n';
                }
            }

            return referencefiles.Length > 1 ? referencefiles : "<none>";
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            IEnumerable<FileInfo> files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
            return files.Where(f => extensions.Contains(f.Extension));
        }
    }
}
