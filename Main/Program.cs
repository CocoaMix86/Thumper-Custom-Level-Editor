using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Thumper_Custom_Level_Editor
{
    internal static class Program
	{
        public static TCLE tcle
        {
            get => _tcle;
            set {
                _tcle = value;
            }
        }
        private static TCLE _tcle;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length <= 0)
                args = new string[] { "" };
            else
                args[0] = string.Join(" ", args);
            // Force culture info, ensures periods . for decimals
            CultureInfo ci = new("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            tcle = new(args[0]) { WindowState = FormWindowState.Normal, Width = 20, Height = 20, StartPosition = FormStartPosition.CenterScreen };
            ImageMessageBox splash = new("splashscreen", tcle) { TopMost = true, TopLevel = true };
            splash.Show();
            tcle.Location = new Point(splash.Location.X, splash.Location.Y);
            tcle.Size = splash.Size;

            Application.Run(tcle);
        }

        public static List<string> total = new();
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string name = args.Name[..args.Name.IndexOf(',')] + ".dll";
            List<string> resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name)).ToList();

            if (resources.Count > 0) {
                string resourceName = resources.First();
                total.Add(resourceName);

                using Stream stream = thisAssembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                    return null;
                byte[] block = new byte[stream.Length - 1 + 1];
                stream.ReadExactly(block, 0, block.Length);
                return Assembly.Load(block);
            }

            return null;
        }
    }
}
