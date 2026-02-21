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
    {/*
        public static void AddFileLock(FileInfo file)
        {
            if (file == null)
                return;
            if (!TCLE.lockedfiles.Any(x => x.Key.FullName == file.FullName)) {
                lockedfiles.Add(file, new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite));
            }
        }*/

        public static void WriteFileLock(FileStream fs, JObject _save)
        {
            string tosave = JsonConvert.SerializeObject(_save, Formatting.Indented);
            using (StreamWriter sr = new(fs, System.Text.Encoding.UTF8, tosave.Length, true)) {
                fs.SetLength(0);
                sr.Write(tosave);
            }
        }

        public static void WriteFileLock(string fs, string _save)
        {
            using (StreamWriter sr = new(new FileStream(fs, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), System.Text.Encoding.UTF8, _save.Length, true)) {
                sr.Write(_save);
            }
        }

        public static void WriteFileLock(FileStream fs, string _save)
        {
            string tosave = _save;
            using (StreamWriter sr = new(fs, System.Text.Encoding.UTF8, tosave.Length, true)) {
                fs.SetLength(0);
                sr.Write(tosave);
            }
        }

        public static void WriteFileLock(string fs, JObject _save)
        {
            string tosave = JsonConvert.SerializeObject(_save, Formatting.Indented);
            File.Delete(fs);
            using (StreamWriter sr = new(new FileStream(fs, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite), System.Text.Encoding.UTF8, tosave.Length, true)) {
                sr.Write(tosave);
            }
        }

        public static dynamic LoadFileLock(string _selectedfilename, bool LoadText = false)
        {
            object _load;
            if (!File.Exists(_selectedfilename))
                return null;
            ///reference:
            ///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
            using (FileStream fileStream = new(_selectedfilename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader textReader = new(fileStream)) {
                if (LoadText) {
                    _load = textReader.ReadToEnd();
                }
                else {
                    try {
                        _load = JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
                    } catch (Exception) {
                        MessageBox.Show($"Failed to parse JSON in {_selectedfilename}.", "File load error");
                        _load = null;
                    }
                }
            }

            return _load;
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
        /// 
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

        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
            return files.Where(f => extensions.Contains(f.Extension));
        }
    }
}
