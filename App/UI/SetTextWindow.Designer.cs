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
            this.label1 = new System.Windows.Forms.Label();
            this.labelNotesSelected = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCSelected = new System.Windows.Forms.Label();
            this.labelSylSelected = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonWhat = new System.Windows.Forms.Button();
            this.setTextWindowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(433, 161);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(433, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отмена";
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
            this.textBox.Size = new System.Drawing.Size(371, 193);
            this.textBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Нот выделено:";
            // 
            // labelNotesSelected
            // 
            this.labelNotesSelected.AutoSize = true;
            this.labelNotesSelected.Location = new System.Drawing.Point(91, 51);
            this.labelNotesSelected.Name = "labelNotesSelected";
            this.labelNotesSelected.Size = new System.Drawing.Size(16, 13);
            this.labelNotesSelected.TabIndex = 5;
            this.labelNotesSelected.Text = "-1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Слогов:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Согласных:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Visible = false;
            // 
            // labelCSelected
            // 
            this.labelCSelected.AutoSize = true;
            this.labelCSelected.Location = new System.Drawing.Point(91, 31);
            this.labelCSelected.Name = "labelCSelected";
            this.labelCSelected.Size = new System.Drawing.Size(16, 13);
            this.labelCSelected.TabIndex = 5;
            this.labelCSelected.Text = "-1";
            this.labelCSelected.Visible = false;
            // 
            // labelSylSelected
            // 
            this.labelSylSelected.AutoSize = true;
            this.labelSylSelected.Location = new System.Drawing.Point(91, 6);
            this.labelSylSelected.Name = "labelSylSelected";
            this.labelSylSelected.Size = new System.Drawing.Size(16, 13);
            this.labelSylSelected.TabIndex = 5;
            this.labelSylSelected.Text = "-1";
            this.labelSylSelected.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelSylSelected);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelCSelected);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labelNotesSelected);
            this.panel1.Location = new System.Drawing.Point(395, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(113, 73);
            this.panel1.TabIndex = 6;
            // 
            // buttonWhat
            // 
            this.buttonWhat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWhat.Location = new System.Drawing.Point(433, 116);
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
            // SetTextWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 225);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonWhat);
            this.Controls.Add(this.buttonOK);
            this.Name = "SetTextWindow";
            this.ShowIcon = false;
            this.Text = "Ввести текст";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setTextWindowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.BindingSource setTextWindowBindingSource;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelNotesSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCSelected;
        private System.Windows.Forms.Label labelSylSelected;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonWhat;
    }
}