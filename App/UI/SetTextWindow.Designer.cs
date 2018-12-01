namespace App.UI
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonWhat = new System.Windows.Forms.Button();
            this.setTextWindowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxHyphen = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(433, 218);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "button_ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(433, 247);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "button_cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox.Location = new System.Drawing.Point(13, 17);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(371, 250);
            this.textBox.TabIndex = 4;
            // 
            // buttonWhat
            // 
            this.buttonWhat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWhat.Location = new System.Drawing.Point(433, 173);
            this.buttonWhat.Name = "buttonWhat";
            this.buttonWhat.Size = new System.Drawing.Size(75, 23);
            this.buttonWhat.TabIndex = 3;
            this.buttonWhat.Text = "?";
            this.buttonWhat.UseVisualStyleBackColor = true;
            this.buttonWhat.Click += new System.EventHandler(this.buttonWhat_Click);
            // 
            // setTextWindowBindingSource
            // 
            this.setTextWindowBindingSource.DataSource = typeof(App.UI.SetTextWindow);
            // 
            // checkBoxHyphen
            // 
            this.checkBoxHyphen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxHyphen.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkBoxHyphen.Location = new System.Drawing.Point(395, 37);
            this.checkBoxHyphen.Name = "checkBoxHyphen";
            this.checkBoxHyphen.Size = new System.Drawing.Size(106, 34);
            this.checkBoxHyphen.TabIndex = 7;
            this.checkBoxHyphen.Text = "checkbox_hyphen";
            this.checkBoxHyphen.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkBoxHyphen.UseVisualStyleBackColor = true;
            // 
            // SetTextWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 282);
            this.Controls.Add(this.checkBoxHyphen);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonWhat);
            this.Controls.Add(this.buttonOK);
            this.Name = "SetTextWindow";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.BindingSource setTextWindowBindingSource;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button buttonWhat;
        private System.Windows.Forms.CheckBox checkBoxHyphen;
    }
}