using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Thumper_Custom_Level_Editor
{
    class Native
    {
        public const int TCLE_ERR_GEN = 0;
        public const int TCLE_OK = 1;
        public const int TCLE_ERR_WIN = 2;

        [DllImport("tcle_native")] public static extern int tcle_native_init();
        [DllImport("tcle_native")] public static extern void tcle_native_reload();
        [DllImport("tcle_native")] public static extern IntPtr tcle_native_draw(int width, int height);
        public static Bitmap tcle_native_bitmap(int width, int height)
        {
            IntPtr pixelBuffer = Native.tcle_native_draw(width, height);
            return new Bitmap(width, height, width * 4, PixelFormat.Format32bppRgb, pixelBuffer);
        }
            
        [DllImport("tcle_native")] public static extern void tcle_native_uninit();
    }
}
