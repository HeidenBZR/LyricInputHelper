namespace LyricInputHelper.UI
{
    partial class AddWordDialog
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
            this.textBoxWord = new System.Windows.Forms.TextBox();
            this.labelWord = new System.Windows.Forms.Label();
            this.textBoxPhonemes = new System.Windows.Forms.TextBox();
            this.labelPhonemes = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(151, 162);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(120, 29);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "button1";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(286, 162);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(125, 29);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "button2";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxWord
            // 
            this.textBoxWord.Location = new System.Drawing.Point(24, 34);
            this.textBoxWord.Name = "textBoxWord";
            this.textBoxWord.Size = new System.Drawing.Size(387, 20);
            this.textBoxWord.TabIndex = 2;
            // 
            // labelWord
            // 
            this.labelWord.Location = new System.Drawing.Point(24, 8);
            this.labelWord.Name = "labelWord";
            this.labelWord.Size = new System.Drawing.Size(387, 23);
            this.labelWord.TabIndex = 3;
            this.labelWord.Text = "label_word";
            this.labelWord.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxPhonemes
            // 
            this.textBoxPhonemes.Location = new System.Drawing.Point(24, 90);
            this.textBoxPhonemes.Name = "textBoxPhonemes";
            this.textBoxPhonemes.Size = new System.Drawing.Size(387, 20);
            this.textBoxPhonemes.TabIndex = 2;
            // 
            // labelPhonemes
            // 
            this.labelPhonemes.Location = new System.Drawing.Point(24, 64);
            this.labelPhonemes.Name = "labelPhonemes";
            this.labelPhonemes.Size = new System.Drawing.Size(387, 23);
            this.labelPhonemes.TabIndex = 3;
            this.labelPhonemes.Text = "label_phonemes";
            this.labelPhonemes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelStatus
            // 
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelStatus.Location = new System.Drawing.Point(0, 203);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Padding = new System.Windows.Forms.Padding(5);
            this.labelStatus.Size = new System.Drawing.Size(423, 23);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // AddWordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 226);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelPhonemes);
            this.Controls.Add(this.labelWord);
            this.Controls.Add(this.textBoxPhonemes);
            this.Controls.Add(this.textBoxWord);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Name = "AddWordDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddWordDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxWord;
        private System.Windows.Forms.Label labelWord;
        private System.Windows.Forms.TextBox textBoxPhonemes;
        private System.Windows.Forms.Label labelPhonemes;
        private System.Windows.Forms.Label labelStatus;
    }
}