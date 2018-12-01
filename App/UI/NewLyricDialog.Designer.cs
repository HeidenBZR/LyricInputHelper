namespace App.UI
{
    partial class NewLyricDialog
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
            if (disposing && (components != null))
            {
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxNewLyric = new System.Windows.Forms.TextBox();
            this.labelOfNewText = new System.Windows.Forms.Label();
            this.labelOfCurrentText = new System.Windows.Forms.Label();
            this.labelLyric = new System.Windows.Forms.Label();
            this.labelOfWarning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(58, 143);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "button_ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(169, 143);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "button_cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxNewLyric
            // 
            this.textBoxNewLyric.Location = new System.Drawing.Point(118, 60);
            this.textBoxNewLyric.Name = "textBoxNewLyric";
            this.textBoxNewLyric.Size = new System.Drawing.Size(136, 20);
            this.textBoxNewLyric.TabIndex = 1;
            // 
            // labelOfNewText
            // 
            this.labelOfNewText.Location = new System.Drawing.Point(12, 60);
            this.labelOfNewText.Name = "labelOfNewText";
            this.labelOfNewText.Size = new System.Drawing.Size(100, 23);
            this.labelOfNewText.TabIndex = 2;
            this.labelOfNewText.Text = "new_text";
            this.labelOfNewText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelOfCurrentText
            // 
            this.labelOfCurrentText.Location = new System.Drawing.Point(12, 22);
            this.labelOfCurrentText.Name = "labelOfCurrentText";
            this.labelOfCurrentText.Size = new System.Drawing.Size(100, 23);
            this.labelOfCurrentText.TabIndex = 2;
            this.labelOfCurrentText.Text = "current_text";
            this.labelOfCurrentText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLyric
            // 
            this.labelLyric.Location = new System.Drawing.Point(118, 22);
            this.labelLyric.Name = "labelLyric";
            this.labelLyric.Size = new System.Drawing.Size(136, 23);
            this.labelLyric.TabIndex = 2;
            this.labelLyric.Text = "_";
            this.labelLyric.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOfWarning
            // 
            this.labelOfWarning.Location = new System.Drawing.Point(12, 98);
            this.labelOfWarning.Name = "labelOfWarning";
            this.labelOfWarning.Size = new System.Drawing.Size(253, 42);
            this.labelOfWarning.TabIndex = 2;
            this.labelOfWarning.Text = "no_text_validation_warning";
            // 
            // NewLyricDialog
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(268, 178);
            this.Controls.Add(this.labelLyric);
            this.Controls.Add(this.labelOfCurrentText);
            this.Controls.Add(this.labelOfWarning);
            this.Controls.Add(this.labelOfNewText);
            this.Controls.Add(this.textBoxNewLyric);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Name = "NewLyricDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "change_lyric_dialog_title";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxNewLyric;
        private System.Windows.Forms.Label labelOfNewText;
        private System.Windows.Forms.Label labelOfCurrentText;
        private System.Windows.Forms.Label labelLyric;
        private System.Windows.Forms.Label labelOfWarning;
    }
}