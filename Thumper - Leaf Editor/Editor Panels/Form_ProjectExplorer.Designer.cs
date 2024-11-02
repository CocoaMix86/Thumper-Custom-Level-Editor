namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_ProjectExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ProjectExplorer));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.toolstripExplorer = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ForeColor = System.Drawing.Color.White;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageKey = "xfm";
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 20;
            this.treeView1.LabelEdit = true;
            this.treeView1.LineColor = System.Drawing.Color.White;
            this.treeView1.Location = new System.Drawing.Point(0, 45);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageKey = "xfm";
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(357, 405);
            this.treeView1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            this.imageList1.Images.SetKeyName(1, "LEVEL DETAILS");
            this.imageList1.Images.SetKeyName(2, "leaf");
            this.imageList1.Images.SetKeyName(3, "lvl");
            this.imageList1.Images.SetKeyName(4, "gate");
            this.imageList1.Images.SetKeyName(5, "master");
            this.imageList1.Images.SetKeyName(6, "samp");
            this.imageList1.Images.SetKeyName(7, "xfm");
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(0, 25);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(357, 20);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Text = "Search Project Explorer (Ctrl+;)";
            // 
            // toolstripExplorer
            // 
            this.toolstripExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.toolstripExplorer.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolstripExplorer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripExplorer.Location = new System.Drawing.Point(0, 0);
            this.toolstripExplorer.MaximumSize = new System.Drawing.Size(0, 50);
            this.toolstripExplorer.Name = "toolstripExplorer";
            this.toolstripExplorer.Padding = new System.Windows.Forms.Padding(0);
            this.toolstripExplorer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolstripExplorer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolstripExplorer.Size = new System.Drawing.Size(357, 25);
            this.toolstripExplorer.Stretch = true;
            this.toolstripExplorer.TabIndex = 142;
            this.toolstripExplorer.Text = "titlebar";
            // 
            // Form_ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(357, 450);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.toolstripExplorer);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ProjectExplorer";
            this.Text = "Project Explorer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStrip toolstripExplorer;
    }
}