using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using NAudio.Vorbis;
using NAudio.Wave;
using System.Runtime.InteropServices;

namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    public partial class Form_ProjectExplorer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Form Construction
        private TCLE _mainform { get; set; }
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        public static void SetTreeViewTheme(IntPtr treeHandle)
        {
            SetWindowTheme(treeHandle, "explorer", null);
        }
        public Form_ProjectExplorer(TCLE form)
        {
            _mainform = form;
            InitializeComponent();
            toolstripExplorer.Renderer = new ToolStripOverride();
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
            SetTreeViewTheme(treeView1.Handle);

            DirectoryInfo directoryInfo = new DirectoryInfo(@"X:\Thumper\levels\Basics3");
            if (directoryInfo.Exists) {
                BuildTree(directoryInfo, treeView1.Nodes);
            }
        }
        #endregion

        private void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {
            TreeNode curNode = addInMe.Add(directoryInfo.Name, directoryInfo.Name, "folder", "folder");

            foreach (FileInfo file in directoryInfo.GetFiles()) {
                if (file.Name.Contains("xfm_") || file.Name.Contains("spn_") || file.Name.Contains("config_") || file.Name.Contains(".color"))
                    continue;
                curNode.Nodes.Add(file.FullName, file.Name, file.Name.Split('_')[0], file.Name.Split('_')[0]);
            }
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories()) {
                BuildTree(subdir, curNode.Nodes);
            }
        }

        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search Project Explorer (Ctrl+;)")
                txtSearch.Text = "";
        }
        private void txtSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
                txtSearch.Text = "Search Project Explorer (Ctrl+;)";
        }
    }
}
