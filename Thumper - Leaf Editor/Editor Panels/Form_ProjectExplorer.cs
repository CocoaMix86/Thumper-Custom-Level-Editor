using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
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
            //SetTreeViewTheme(treeView1.Handle);
            CreateTreeView();
        }
        #endregion

        bool filterenabled = false;
        private void CreateTreeView()
        {
            treeView1.Nodes.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(@"X:\Thumper\levels\Basics3");
            if (directoryInfo.Exists) {
                BuildTree(directoryInfo, treeView1.Nodes);
                treeView1.Nodes[0].ImageKey = "project";
                treeView1.Nodes[0].SelectedImageKey = "project";
            }
            if (filterenabled)
                treeView1.ExpandAll();
            else
                treeView1.Nodes[0].Expand();
        }
        private void BuildTree(DirectoryInfo directoryInfo, TreeNodeCollection addInMe)
        {
            TreeNode curNode = addInMe.Add(directoryInfo.Name, directoryInfo.Name, "folder", "folder");

            foreach (FileInfo file in directoryInfo.GetFiles()) {
                if (file.Name.Contains("xfm_") || file.Name.Contains("spn_") || file.Name.Contains("config_") || file.Name.Contains(".color"))
                    continue;
                if (!filterenabled)
                    curNode.Nodes.Add(file.FullName, file.Name, file.Name.Split('_')[0], file.Name.Split('_')[0]);
                else if ((filterLeaf.Checked && file.Name.Contains("leaf_")) || (filterLvl.Checked && file.Name.Contains("lvl_")) || (filterGate.Checked && file.Name.Contains("gate_")) || (filterMaster.Checked && file.Name.Contains("master_")) || (filterSample.Checked && file.Name.Contains("samp_")))
                    curNode.Nodes.Add(file.FullName, file.Name, file.Name.Split('_')[0], file.Name.Split('_')[0]);
            }
            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories()) {
                BuildTree(subdir, curNode.Nodes);
            }
        }

        private void filter_CheckChanged(object sender, EventArgs e) => CreateTreeView();

        private void btnFilter_ButtonClick(object sender, EventArgs e)
        {
            filterenabled = !filterenabled;
            if (filterenabled)
                btnFilter.BackColor = Color.LightBlue;
            else
                btnFilter.BackColor = Color.FromArgb(35, 35, 35);
            CreateTreeView();
        }

        private void contextMenuFilters_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCollapse_Click(object sender, EventArgs e) => treeView1.CollapseAll();
        private void btnExpand_Click(object sender, EventArgs e) => treeView1.ExpandAll();
        private void btnRefresh_Click(object sender, EventArgs e) => CreateTreeView();
    }
}
