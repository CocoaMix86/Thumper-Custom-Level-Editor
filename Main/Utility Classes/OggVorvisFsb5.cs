using System.Runtime.InteropServices;
using System.Text;

namespace Thumper_Custom_Level_Editor
{
    // This is a custom binding to the following code
    // https://github.com/uyjulian/oggvorbis2fsb5/tree/03bd46a46981f20a345df559dfc862f0666469ef
    // The c code is modified, to make a half decent binding, the changes are visible in `oggvorbisfsb5.patch`
    //
    // --- BASIC USAGE ---
    // The only function you need from here is `OggVorvisFsb5.convert_ogg_vorbis_fsb5`
    // You give this function bytes from a .ogg file and it'll return the bytes for a .fsb file
    // Example:
    //
    // byte[] input = File.ReadAllBytes("input.ogg");
    // byte[] output = OggVorvisFsb5.convert_ogg_vorbis_fsb5(input); // <-- !!! IMPORTANT !!! Check for exceptions !!!
    // File.WriteAllBytes("output.fsb", output);
    internal class OggVorvisFsb5
    {

        // C function signature: int(void* inData, int inSize, void** outData, int* outSize)
        //
        // This function takes raw data and converts it to fsb5, the bytes are returned from function parameters
        // Invoking this function will invalidate the error log and info log pointers
        // If `1` is returned then everything was successful, if `0` then a failure occured
        //
        // If this function fails you should query the error log, on success an info log is also available to query
        //
        // Upon success the outData pointer is filled with the data, you now own this pointer and must call `ns_anthofoxo_tcle_free` when you're done with it
        [DllImport("oggvorbis2fsb5.dll")] public static extern int ns_anthofoxo_tcle_ogg_vorbis_fsb5_convert(IntPtr inData, int inSize, out IntPtr outData, out int outSize);

        // C function signature: void(void const** pData, int* size)
        //
        // When converting a file successfully, an optional info log may be fetched,
        // The pData pointer is invalidated upon the next conversion, the info log should be copied to prevent issues with this
        [DllImport("oggvorbis2fsb5.dll")] public static extern void ns_anthofoxo_tcle_get_info_log(out IntPtr pData, out int pSize);

        // C function signature: void(void const** pData, int* size)
        //
        // When converting a file fails, the error log will be available here
        // The pData pointer is invalidated upon the next conversion, the info log should be copied to prevent issues with this
        [DllImport("oggvorbis2fsb5.dll")] public static extern void ns_anthofoxo_tcle_get_error_log(out IntPtr pData, out int pSize);

        // C function signature: void(void* ptr)
        //
        // When converting you are given ownership of the data pointer, call this function when you are done with that pointer
        [DllImport("oggvorbis2fsb5.dll")] public static extern void ns_anthofoxo_tcle_free(IntPtr ptr);

        // Nice wrapper around the native api, call this after converting to get the error log
        public static String get_error_log()
        {
            ns_anthofoxo_tcle_get_error_log(out IntPtr ptr, out int size);
            if (ptr == IntPtr.Zero || size == 0) return string.Empty;
            byte[] buf = new byte[size];
            Marshal.Copy(ptr, buf, 0, size);
            return Encoding.UTF8.GetString(buf);
        }

        // Nice wrapper around the native api, call this after converting to get the info log
        public static String get_info_log()
        {
            ns_anthofoxo_tcle_get_info_log(out IntPtr ptr, out int size);
            if (ptr == IntPtr.Zero || size == 0) return string.Empty;
            byte[] buf = new byte[size];
            Marshal.Copy(ptr, buf, 0, size);
            return Encoding.UTF8.GetString(buf);
        }

        // Nice weapper around the native api, call this with your input bytes and the output bytes are returned
        // If an error occurs, InvalidDataException is thrown with the attached error log
        // The info log is not attached
        public static byte[] convert_ogg_vorbis_fsb5(byte[] inBytes)
        {
            IntPtr nativeBytes = Marshal.AllocHGlobal(inBytes.Length);
            Marshal.Copy(inBytes, 0, nativeBytes, inBytes.Length);

            IntPtr outBytes = IntPtr.Zero;
            int outSize = 0;

            try
            {
                int ok = ns_anthofoxo_tcle_ogg_vorbis_fsb5_convert(nativeBytes, inBytes.Length, out outBytes, out outSize);

                if (ok == 0)
                {
                    string error = get_error_log();
                    if (string.IsNullOrWhiteSpace(error)) throw new InvalidDataException("OGG -> FSB conversion failed (no error log returned)");
                    throw new InvalidDataException($"OGG -> FSB conversion failed:\n{error}");
                }

                byte[] output = new byte[outSize];
                Marshal.Copy(outBytes, output, 0, outSize);
                return output;
            }
            finally
            {
                Marshal.FreeHGlobal(nativeBytes);

                if (outBytes != IntPtr.Zero)
                {
                    ns_anthofoxo_tcle_free(outBytes);
                }
            }
        }
    }
}
