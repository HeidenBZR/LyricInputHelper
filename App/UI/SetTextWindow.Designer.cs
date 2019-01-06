namespace LyricInputHelper.UI
{
    partial class SetTextWindow
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
            this.components = new System.ComponentModel.Container();
            this.setTextWindowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxInputMode = new System.Windows.Forms.ComboBox();
            this.buttonLastText = new System.Windows.Forms.Button();
            this.buttonLastPhonemes = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // setTextWindowBindingSource
            // 
            this.setTextWindowBindingSource.DataSource = typeof(LyricInputHelper.UI.SetTextWindow);
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(377, 325);
            this.textBox.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxInputMode);
            this.panel1.Controls.Add(this.buttonLastText);
            this.panel1.Controls.Add(this.buttonLastPhonemes);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(379, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 325);
            this.panel1.TabIndex = 10;
            // 
            // comboBoxInputMode
            // 
            this.comboBoxInputMode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBoxInputMode.FormattingEnabled = true;
            this.comboBoxInputMode.Items.AddRange(new object[] {
            "Lyric Input",
            "Phonetic Input"});
            this.comboBoxInputMode.Location = new System.Drawing.Point(5, 165);
            this.comboBoxInputMode.Name = "comboBoxInputMode";
            this.comboBoxInputMode.Size = new System.Drawing.Size(106, 21);
            this.comboBoxInputMode.TabIndex = 8;
            // 
            // buttonLastText
            // 
            this.buttonLastText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLastText.Location = new System.Drawing.Point(21, 14);
            this.buttonLastText.Name = "buttonLastText";
            this.buttonLastText.Size = new System.Drawing.Size(90, 43);
            this.buttonLastText.TabIndex = 3;
            this.buttonLastText.Text = "GET LAST TEXT";
            this.buttonLastText.UseVisualStyleBackColor = true;
            this.buttonLastText.Click += new System.EventHandler(this.buttonLastText_Click);
            // 
            // buttonLastPhonemes
            // 
            this.buttonLastPhonemes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLastPhonemes.Location = new System.Drawing.Point(21, 63);
            this.buttonLastPhonemes.Name = "buttonLastPhonemes";
            this.buttonLastPhonemes.Size = new System.Drawing.Size(90, 43);
            this.buttonLastPhonemes.TabIndex = 3;
            this.buttonLastPhonemes.Text = "GET LAST PHONEMES";
            this.buttonLastPhonemes.UseVisualStyleBackColor = true;
            this.buttonLastPhonemes.Click += new System.EventHandler(this.buttonLastPhonemes_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(21, 293);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "button_cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(21, 264);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(90, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "button_ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // SetTextWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 325);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox);
            this.Name = "SetTextWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource setTextWindowBindingSource;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxInputMode;
        private System.Windows.Forms.Button buttonLastText;
        private System.Windows.Forms.Button buttonLastPhonemes;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
    }
}