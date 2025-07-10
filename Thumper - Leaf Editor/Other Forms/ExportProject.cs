using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using Thumper_Custom_Level_Editor.Shared_Classes_and_Methods;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Thumper_Custom_Level_Editor.Other_Forms
{
    public partial class ExportProject : Form
    {
        public readonly CommonOpenFileDialog cfd_lvl = new() { IsFolderPicker = true, Multiselect = false };
        private List<string> FilesInProject = new();
        private string ProjectZIP;

        public ExportProject()
        {
            InitializeComponent();
        }

        private void ExportProject_Load(object sender, EventArgs e)
        {
            FilesInProject  = Directory.GetFiles($@"{TCLE.ProjectProperties.folder}", "*.*", SearchOption.AllDirectories).ToList();
            int _countleaf = FilesInProject.Count(x => x.EndsWith(".leaf"));
            int _countlvl = FilesInProject.Count(x => x.EndsWith(".lvl"));
            int _countgate = FilesInProject.Count(x => x.EndsWith(".gate"));
            int _countmaster = FilesInProject.Count(x => x.EndsWith(".master"));
            int _countsamp = FilesInProject.Count(x => x.EndsWith(".samp"));
            int _countpc = FilesInProject.Count(x => x.EndsWith(".pc"));
            int _countother = FilesInProject.Count - _countleaf - _countlvl - _countgate - _countmaster - _countsamp - _countpc;

            lblStuff.Text = $"  .leaf: {_countleaf}\n   .lvl: {_countlvl}\n  .gate: {_countgate}\n.master: {_countmaster}\n  .samp: {_countsamp}\n  other: {_countother}\n\ncustom samples: {_countpc}";

            panelThumb.Location = new Point(panelThumb.Location.X, lblStuff.Location.Y + lblStuff.Height + 5);
            if (TCLE.ProjectProperties.Thumbnail != null) {
                pictureThumb.Width = TCLE.ProjectProperties.Thumbnail.Width;
                pictureThumb.Height = TCLE.ProjectProperties.Thumbnail.Height;
                pictureThumb.Image = TCLE.ProjectProperties.Thumbnail;
                double ratio = 0;
                if (pictureThumb.Width > 200) {
                    ratio = pictureThumb.Width / 200d;
                    pictureThumb.Width = (int)(pictureThumb.Width / ratio);
                    pictureThumb.Height = (int)(pictureThumb.Height / ratio);
                }
                if (pictureThumb.Height > 200) {
                    ratio = pictureThumb.Height / 200d;
                    pictureThumb.Width = (int)(pictureThumb.Width / ratio);
                    pictureThumb.Height = (int)(pictureThumb.Height / ratio);
                }
                panelThumb.Width = pictureThumb.Width + 5;
                panelThumb.Height = pictureThumb.Height + label3.Height + 10;
            }

            txtCustomPath.Text = TCLE.ProjectProperties.WorkingFolder.FullName;
            btnExport.Enabled = true;
        }

        private void btnCustomFolder_Click(object sender, EventArgs e)
        {
            cfd_lvl.InitialDirectory = TCLE.ProjectProperties.folder;
            cfd_lvl.Title = "Choose where to save the project ZIP export";
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok) {
                if (cfd_lvl.FileName.Length > 255) {
                    MessageBox.Show("Folder path too long, due to Windows limits. Max length 255.\nChoose a different path.", "Thumper Custom Level Editor");
                    return;
                }
                txtCustomPath.Text = cfd_lvl.FileName;
                btnExport.Enabled = true;
                btnExport.BackColor = Color.Green;
                ProjectZIP = cfd_lvl.FileName + $"\\{TCLE.ProjectProperties.projectname}.zip";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //resave the .TCL so it's accurate before export
            //mainly to make sure Level Sections is good
            dynamic _saveTCL = TCLE.BuildSave(TCLE.projectProperties);
            File.WriteAllText($"{TCLE.ProjectProperties.TCL.FullName}", JsonConvert.SerializeObject(_saveTCL, Formatting.Indented));
            //build the objlib and sec files
            //these will get stored in \temp
            BuildObjlib.Make_Custom_Level(TCLE.ProjectProperties);
            //list of files to zip
            List<string> FilesToZip = new();
            FilesToZip.AddRange(FilesInProject.Where(x => x.EndsWith(".pc")));
            FilesToZip.Add($@"{TCLE.AppLocation}\temp\{TCLE.ProjectProperties.projectname}.objlib");
            FilesToZip.Add($@"{TCLE.AppLocation}\temp\{TCLE.ProjectProperties.projectname}.sec");
            FilesToZip.Add(TCLE.ProjectProperties.TCL.FullName);
            if (TCLE.ProjectProperties.Thumbnail != null)
                FilesToZip.Add(FilesInProject.First(x => x.Contains("\\thumbnail.")));
            //zip relevant files together
            int exists = 0;
            if (File.Exists($@"{txtCustomPath.Text}\{TCLE.ProjectProperties.projectname}.zip")) {
                exists = Directory.GetFiles($@"{txtCustomPath.Text}\", $"{TCLE.ProjectProperties.projectname}*.zip").Count();
            }
            using (ZipArchive archive = ZipFile.Open($@"{txtCustomPath.Text}\{TCLE.ProjectProperties.projectname}{(exists > 0 ? $" {exists + 1}" : "")}.zip", ZipArchiveMode.Create)) {
                foreach (string fPath in FilesToZip) {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }

            if (MessageBox.Show($"Level export is complete.\nDo you want to open the containing folder?", "Foxo Custom Level Editor", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                Process.Start("explorer.exe", $@"/select, ""{txtCustomPath.Text}\{TCLE.ProjectProperties.projectname}.zip""");
            }

            this.Close();
        }
    }
}
