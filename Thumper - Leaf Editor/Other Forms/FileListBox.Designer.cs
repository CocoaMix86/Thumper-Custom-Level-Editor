namespace Thumper_Custom_Level_Editor
{
    partial class FileListBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileListBox));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new Thumper_Custom_Level_Editor.TreeViewEx();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, "LEVEL DETAILS.txt");
            this.imageList1.Images.SetKeyName(2, "leaf");
            this.imageList1.Images.SetKeyName(3, "lvl");
            this.imageList1.Images.SetKeyName(4, "gate");
            this.imageList1.Images.SetKeyName(5, "master");
            this.imageList1.Images.SetKeyName(6, "samp");
            this.imageList1.Images.SetKeyName(7, "xfm");
            this.imageList1.Images.SetKeyName(8, "project");
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageKey = "xfm";
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 20;
            this.treeView1.LabelEdit = true;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageKey = "xfm";
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(217, 203);
            this.treeView1.TabIndex = 1;
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            // 
            // FileListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 203);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileListBox";
            this.ShowIcon = false;
            this.Text = "Select A File";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx treeView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}