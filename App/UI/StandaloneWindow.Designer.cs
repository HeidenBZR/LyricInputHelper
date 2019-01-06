namespace LyricInputHelper.UI
{
    partial class StandaloneWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandaloneWindow));
            this.labelSetupMessage = new System.Windows.Forms.Label();
            this.labelSetupAtlas = new System.Windows.Forms.Label();
            this.labelSetupResources = new System.Windows.Forms.Label();
            this.labelSetupTemp = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSetupMessage
            // 
            this.labelSetupMessage.Location = new System.Drawing.Point(12, 9);
            this.labelSetupMessage.Name = "labelSetupMessage";
            this.labelSetupMessage.Size = new System.Drawing.Size(281, 27);
            this.labelSetupMessage.TabIndex = 0;
            this.labelSetupMessage.Text = "setup_message";
            // 
            // labelSetupAtlas
            // 
            this.labelSetupAtlas.Location = new System.Drawing.Point(12, 36);
            this.labelSetupAtlas.Name = "labelSetupAtlas";
            this.labelSetupAtlas.Size = new System.Drawing.Size(281, 27);
            this.labelSetupAtlas.TabIndex = 0;
            this.labelSetupAtlas.Text = "setup_atlas";
            // 
            // labelSetupResources
            // 
            this.labelSetupResources.Location = new System.Drawing.Point(12, 63);
            this.labelSetupResources.Name = "labelSetupResources";
            this.labelSetupResources.Size = new System.Drawing.Size(281, 27);
            this.labelSetupResources.TabIndex = 0;
            this.labelSetupResources.Text = "setup_resourses";
            // 
            // labelSetupTemp
            // 
            this.labelSetupTemp.Location = new System.Drawing.Point(12, 90);
            this.labelSetupTemp.Name = "labelSetupTemp";
            this.labelSetupTemp.Size = new System.Drawing.Size(281, 27);
            this.labelSetupTemp.TabIndex = 0;
            this.labelSetupTemp.Text = "setup_temp";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(292, 136);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // StandaloneWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 171);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelSetupTemp);
            this.Controls.Add(this.labelSetupResources);
            this.Controls.Add(this.labelSetupAtlas);
            this.Controls.Add(this.labelSetupMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StandaloneWindow";
            this.Text = "Standalone";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSetupMessage;
        private System.Windows.Forms.Label labelSetupAtlas;
        private System.Windows.Forms.Label labelSetupResources;
        private System.Windows.Forms.Label labelSetupTemp;
        private System.Windows.Forms.Button buttonOk;
    }
}