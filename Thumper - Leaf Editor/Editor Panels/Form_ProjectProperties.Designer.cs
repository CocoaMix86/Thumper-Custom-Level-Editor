namespace Thumper_Custom_Level_Editor.Editor_Panels
{
    partial class Form_ProjectProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ProjectProperties));
            this.propertyGridProject = new PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGridProject
            // 
            this.propertyGridProject.BackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridProject.CategoryForeColor = Color.White;
            this.propertyGridProject.CategorySplitterColor = Color.FromArgb(46, 46, 46);
            this.propertyGridProject.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            this.propertyGridProject.Dock = DockStyle.Fill;
            this.propertyGridProject.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.propertyGridProject.HelpBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridProject.HelpBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridProject.HelpForeColor = Color.White;
            this.propertyGridProject.LineColor = Color.FromArgb(46, 46, 46);
            this.propertyGridProject.Location = new Point(0, 0);
            this.propertyGridProject.Margin = new Padding(4, 3, 4, 3);
            this.propertyGridProject.Name = "propertyGridProject";
            this.propertyGridProject.PropertySort = PropertySort.Categorized;
            this.propertyGridProject.RightToLeft = RightToLeft.No;
            this.propertyGridProject.SelectedItemWithFocusBackColor = Color.FromArgb(113, 96, 232);
            this.propertyGridProject.SelectedItemWithFocusForeColor = Color.White;
            this.propertyGridProject.Size = new Size(416, 519);
            this.propertyGridProject.TabIndex = 148;
            this.propertyGridProject.ToolbarVisible = false;
            this.propertyGridProject.ViewBackColor = Color.FromArgb(31, 31, 31);
            this.propertyGridProject.ViewBorderColor = Color.FromArgb(61, 61, 61);
            this.propertyGridProject.ViewForeColor = Color.White;
            // 
            // Form_ProjectProperties
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(31, 31, 31);
            this.ClientSize = new Size(416, 519);
            this.Controls.Add(this.propertyGridProject);
            this.DoubleBuffered = true;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form_ProjectProperties";
            this.Text = "Project Properties";
            this.ResumeLayout(false);
        }

        #endregion
        private Thumper_Custom_Level_Editor.TreeViewEx treeView1;
        private PropertyGrid propertyGridProject;
    }
}