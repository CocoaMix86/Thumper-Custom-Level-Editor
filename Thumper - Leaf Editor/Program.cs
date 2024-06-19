using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Thumper_Custom_Level_Editor
{
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
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
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.Run(new FormLeafEditor(args[0]));
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string name = args.Name[..args.Name.IndexOf(',')] + ".dll";
            System.Collections.Generic.IEnumerable<string> resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));

            if (resources.Count() > 0) {
                string resourceName = resources.First();

                using Stream stream = thisAssembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                    return null;
                byte[] block = new byte[stream.Length - 1 + 1];
                stream.Read(block, 0, block.Length);
                return Assembly.Load(block);
            }

            return null;
        }
    }
}
