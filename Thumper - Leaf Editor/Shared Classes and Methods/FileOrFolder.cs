using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{
    public class FileOrFolder
    {
        public FileInfo File { get; set; }
        public DirectoryInfo Folder { get; set; }
        public bool IsFile => File != null;
        public bool IsFolder => Folder != null;
        public string Name => File?.Name ?? Folder.Name;
        public string FullPath => File?.FullName ?? Folder.FullName;

        public FileOrFolder()
        {
        }

        public FileOrFolder(DirectoryInfo _dir = null)
        {
            File = null;
            Folder = _dir;
        }

        public FileOrFolder(FileInfo _file = null)
        {
            File = _file;
            Folder = null;
        }
    }
}
