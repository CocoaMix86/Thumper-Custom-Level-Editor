namespace Thumper_Custom_Level_Editor.Other_Forms
{
    partial class DragDropItemList
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DragDropItemList));
            this.dgvPathsList = new DataGridView();
            this.listName = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)this.dgvPathsList).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPathsList
            // 
            this.dgvPathsList.AllowDrop = true;
            this.dgvPathsList.AllowUserToAddRows = false;
            this.dgvPathsList.AllowUserToDeleteRows = false;
            this.dgvPathsList.AllowUserToResizeColumns = false;
            this.dgvPathsList.AllowUserToResizeRows = false;
            this.dgvPathsList.BackgroundColor = Color.FromArgb(20, 20, 20);
            this.dgvPathsList.BorderStyle = BorderStyle.Fixed3D;
            this.dgvPathsList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvPathsList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgvPathsList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPathsList.ColumnHeadersHeight = 20;
            this.dgvPathsList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPathsList.Columns.AddRange(new DataGridViewColumn[] { this.listName });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.DarkBlue;
            dataGridViewCellStyle2.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(150, 150, 255);
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = Color.CornflowerBlue;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgvPathsList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPathsList.Dock = DockStyle.Fill;
            this.dgvPathsList.EnableHeadersVisualStyles = false;
            this.dgvPathsList.GridColor = Color.Black;
            this.dgvPathsList.Location = new Point(0, 0);
            this.dgvPathsList.Margin = new Padding(4, 3, 4, 3);
            this.dgvPathsList.Name = "dgvPathsList";
            this.dgvPathsList.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(90, 90, 90);
            dataGridViewCellStyle3.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.dgvPathsList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPathsList.RowHeadersVisible = false;
            this.dgvPathsList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = Color.DarkBlue;
            dataGridViewCellStyle4.ForeColor = Color.White;
            this.dgvPathsList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPathsList.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvPathsList.RowTemplate.DefaultCellStyle.BackColor = Color.DarkBlue;
            this.dgvPathsList.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            this.dgvPathsList.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
            this.dgvPathsList.RowTemplate.Height = 20;
            this.dgvPathsList.RowTemplate.Resizable = DataGridViewTriState.False;
            this.dgvPathsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPathsList.Size = new Size(296, 456);
            this.dgvPathsList.TabIndex = 162;
            this.dgvPathsList.CellDoubleClick += this.dgvPathsList_CellDoubleClick;
            this.dgvPathsList.CellPainting += this.lvlLeafList_CellPainting;
            this.dgvPathsList.RowPrePaint += this.lvlLeafList_RowPrePaint;
            this.dgvPathsList.SelectionChanged += this.dgvPathsList_SelectionChanged;
            this.dgvPathsList.DragEnter += this.lvlLeafPaths_DragEnter;
            this.dgvPathsList.MouseDown += this.lvlLeafList_MouseDown;
            this.dgvPathsList.MouseMove += this.lvlLeafList_MouseMove;
            // 
            // listName
            // 
            this.listName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.listName.HeaderText = "Name";
            this.listName.Name = "listName";
            this.listName.ReadOnly = true;
            // 
            // DragDropItemList
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(296, 456);
            this.Controls.Add(this.dgvPathsList);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DragDropItemList";
            this.TopMost = true;
            this.FormClosing += this.DragDropItemList_FormClosing;
            this.Load += this.DragDropItemList_Load;
            ((System.ComponentModel.ISupportInitialize)this.dgvPathsList).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvPathsList;
        private DataGridViewTextBoxColumn listName;
    }
}