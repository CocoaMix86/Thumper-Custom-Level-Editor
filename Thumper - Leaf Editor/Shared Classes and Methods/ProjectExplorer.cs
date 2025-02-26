using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{ 
    public static class ProjectExplorer
    {
        public static Dictionary<string, FileInfo> Files = new();
        public static Dictionary<string, DirectoryInfo> Folders = new();
    }
}
