namespace Thumper_Custom_Level_Editor.Other_Forms
{
    partial class CheckboxDialog
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
            this.btnYes = new Button();
            this.btnNo = new Button();
            this.labelSequencer = new Label();
            this.checkAsk = new CheckBox();
            this.SuspendLayout();
            // 
            // btnYes
            // 
            this.btnYes.BackColor = Color.DarkGreen;
            this.btnYes.Cursor = Cursors.Hand;
            this.btnYes.DialogResult = DialogResult.Yes;
            this.btnYes.FlatStyle = FlatStyle.Flat;
            this.btnYes.ForeColor = Color.White;
            this.btnYes.ImageAlign = ContentAlignment.TopCenter;
            this.btnYes.Location = new Point(155, 161);
            this.btnYes.Margin = new Padding(0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new Size(70, 28);
            this.btnYes.TabIndex = 45;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            // 
            // btnNo
            // 
            this.btnNo.BackColor = Color.DarkRed;
            this.btnNo.Cursor = Cursors.Hand;
            this.btnNo.DialogResult = DialogResult.No;
            this.btnNo.FlatStyle = FlatStyle.Flat;
            this.btnNo.ForeColor = Color.White;
            this.btnNo.ImageAlign = ContentAlignment.TopCenter;
            this.btnNo.Location = new Point(230, 161);
            this.btnNo.Margin = new Padding(0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new Size(70, 28);
            this.btnNo.TabIndex = 46;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            // 
            // labelSequencer
            // 
            this.labelSequencer.AutoSize = true;
            this.labelSequencer.BackColor = Color.FromArgb(60, 60, 60);
            this.labelSequencer.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelSequencer.ForeColor = Color.White;
            this.labelSequencer.Location = new Point(9, 9);
            this.labelSequencer.Margin = new Padding(4, 0, 4, 0);
            this.labelSequencer.MinimumSize = new Size(117, 0);
            this.labelSequencer.Name = "labelSequencer";
            this.labelSequencer.Size = new Size(296, 108);
            this.labelSequencer.TabIndex = 97;
            this.labelSequencer.Text = "Do you want to pre-calculate all sample\r\nruntimes now? This may take a few minutes\r\ndepending on how many samples are in\r\nthis project. You can do this later from\r\nthe Project menu.\r\n\r\n";
            this.labelSequencer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // checkAsk
            // 
            this.checkAsk.AutoSize = true;
            this.checkAsk.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.checkAsk.ForeColor = Color.White;
            this.checkAsk.Location = new Point(93, 120);
            this.checkAsk.Name = "checkAsk";
            this.checkAsk.Size = new Size(111, 19);
            this.checkAsk.TabIndex = 98;
            this.checkAsk.Text = "Don't ask again";
            this.checkAsk.UseVisualStyleBackColor = true;
            // 
            // CheckboxDialog
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(60, 60, 60);
            this.ClientSize = new Size(309, 198);
            this.ControlBox = false;
            this.Controls.Add(this.checkAsk);
            this.Controls.Add(this.labelSequencer);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckboxDialog";
            this.Text = "Thumper Custom Level Editor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btnRawImport;
        private Label labelSequencer;
        public CheckBox checkAsk;
        public Button btnYes;
        public Button btnNo;
    }
}