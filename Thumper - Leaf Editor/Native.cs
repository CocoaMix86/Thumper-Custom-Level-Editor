using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{
    class Native
    {
        public const int TCLE_ERR_GEN = 0;
        public const int TCLE_OK = 1;
        public const int TCLE_ERR_WIN = 2;

        [DllImport("tcle_native")] public static extern int tcle_native_init();
        [DllImport("tcle_native")] public static extern IntPtr tcle_native_draw(int width, int height);
        [DllImport("tcle_native")] public static extern void tcle_native_uninit();
    }
}
