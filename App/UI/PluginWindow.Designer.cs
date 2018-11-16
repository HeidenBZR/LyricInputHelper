namespace App
{
    partial class PluginWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginWindow));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lyricView = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAtlasConvert = new System.Windows.Forms.Button();
            this.buttonSetText = new System.Windows.Forms.Button();
            this.checkBoxVR = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxVCLength = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxInsertShort = new System.Windows.Forms.CheckBox();
            this.buttonSplit = new System.Windows.Forms.Button();
            this.buttonToCVC = new System.Windows.Forms.Button();
            this.buttonCV = new System.Windows.Forms.Button();
            this.textBoxVelocity = new System.Windows.Forms.TextBox();
            this.buttonReload = new System.Windows.Forms.Button();
            this.buttonWhat = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parsedLyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uNoteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonReloadResources = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uNoteBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // lyricView
            // 
            this.lyricView.AllowUserToAddRows = false;
            this.lyricView.AllowUserToDeleteRows = false;
            this.lyricView.AllowUserToResizeColumns = false;
            this.lyricView.AllowUserToResizeRows = false;
            this.lyricView.AutoGenerateColumns = false;
            this.lyricView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.lyricView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lyricView.ColumnHeadersVisible = false;
            this.lyricView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.lyricDataGridViewTextBoxColumn,
            this.parsedLyricDataGridViewTextBoxColumn});
            this.lyricView.DataSource = this.uNoteBindingSource;
            resources.ApplyResources(this.lyricView, "lyricView");
            this.lyricView.Name = "lyricView";
            this.lyricView.RowHeadersVisible = false;
            this.lyricView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lyricView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lyricView.Tag = "";
            this.lyricView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lyricView_CellClick);
            this.lyricView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.lyricView_CellEndEdit);
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.Frozen = true;
            resources.ApplyResources(this.Number, "Number");
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Number.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // buttonAtlasConvert
            // 
            resources.ApplyResources(this.buttonAtlasConvert, "buttonAtlasConvert");
            this.buttonAtlasConvert.Name = "buttonAtlasConvert";
            this.toolTip.SetToolTip(this.buttonAtlasConvert, resources.GetString("buttonAtlasConvert.ToolTip"));
            this.buttonAtlasConvert.UseVisualStyleBackColor = true;
            this.buttonAtlasConvert.Click += new System.EventHandler(this.buttonAtlasConvert_Click);
            // 
            // buttonSetText
            // 
            resources.ApplyResources(this.buttonSetText, "buttonSetText");
            this.buttonSetText.Name = "buttonSetText";
            this.toolTip.SetToolTip(this.buttonSetText, resources.GetString("buttonSetText.ToolTip"));
            this.buttonSetText.UseVisualStyleBackColor = true;
            this.buttonSetText.Click += new System.EventHandler(this.buttonSetText_Click);
            // 
            // checkBoxVR
            // 
            resources.ApplyResources(this.checkBoxVR, "checkBoxVR");
            this.checkBoxVR.Checked = true;
            this.checkBoxVR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVR.Name = "checkBoxVR";
            this.toolTip.SetToolTip(this.checkBoxVR, resources.GetString("checkBoxVR.ToolTip"));
            this.checkBoxVR.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxVCLength
            // 
            resources.ApplyResources(this.textBoxVCLength, "textBoxVCLength");
            this.textBoxVCLength.Name = "textBoxVCLength";
            this.toolTip.SetToolTip(this.textBoxVCLength, resources.GetString("textBoxVCLength.ToolTip"));
            this.textBoxVCLength.TextChanged += new System.EventHandler(this.textMinSize_TextChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Подсказка";
            // 
            // checkBoxInsertShort
            // 
            this.checkBoxInsertShort.Checked = true;
            this.checkBoxInsertShort.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.checkBoxInsertShort, "checkBoxInsertShort");
            this.checkBoxInsertShort.Name = "checkBoxInsertShort";
            this.toolTip.SetToolTip(this.checkBoxInsertShort, resources.GetString("checkBoxInsertShort.ToolTip"));
            this.checkBoxInsertShort.UseVisualStyleBackColor = true;
            // 
            // buttonSplit
            // 
            resources.ApplyResources(this.buttonSplit, "buttonSplit");
            this.buttonSplit.Name = "buttonSplit";
            this.toolTip.SetToolTip(this.buttonSplit, resources.GetString("buttonSplit.ToolTip"));
            this.buttonSplit.UseVisualStyleBackColor = true;
            this.buttonSplit.Click += new System.EventHandler(this.buttonSplit_Click);
            // 
            // buttonToCVC
            // 
            resources.ApplyResources(this.buttonToCVC, "buttonToCVC");
            this.buttonToCVC.Name = "buttonToCVC";
            this.toolTip.SetToolTip(this.buttonToCVC, resources.GetString("buttonToCVC.ToolTip"));
            this.buttonToCVC.UseVisualStyleBackColor = true;
            this.buttonToCVC.Click += new System.EventHandler(this.buttonToCVC_Click);
            // 
            // buttonCV
            // 
            resources.ApplyResources(this.buttonCV, "buttonCV");
            this.buttonCV.Name = "buttonCV";
            this.toolTip.SetToolTip(this.buttonCV, resources.GetString("buttonCV.ToolTip"));
            this.buttonCV.UseVisualStyleBackColor = true;
            this.buttonCV.Click += new System.EventHandler(this.buttonToCV_Click);
            // 
            // textBoxVelocity
            // 
            resources.ApplyResources(this.textBoxVelocity, "textBoxVelocity");
            this.textBoxVelocity.Name = "textBoxVelocity";
            this.toolTip.SetToolTip(this.textBoxVelocity, resources.GetString("textBoxVelocity.ToolTip"));
            // 
            // buttonReload
            // 
            resources.ApplyResources(this.buttonReload, "buttonReload");
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // buttonWhat
            // 
            resources.ApplyResources(this.buttonWhat, "buttonWhat");
            this.buttonWhat.Name = "buttonWhat";
            this.buttonWhat.UseVisualStyleBackColor = true;
            this.buttonWhat.Click += new System.EventHandler(this.buttonWhat_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.textBoxVelocity);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonWhat);
            this.panel1.Controls.Add(this.checkBoxInsertShort);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.checkBoxVR);
            this.panel1.Controls.Add(this.buttonReloadResources);
            this.panel1.Controls.Add(this.buttonReload);
            this.panel1.Controls.Add(this.textBoxVCLength);
            this.panel1.Controls.Add(this.buttonToCVC);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonCV);
            this.panel1.Controls.Add(this.buttonSetText);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonSplit);
            this.panel1.Controls.Add(this.buttonAtlasConvert);
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lyricDataGridViewTextBoxColumn
            // 
            this.lyricDataGridViewTextBoxColumn.DataPropertyName = "Lyric";
            resources.ApplyResources(this.lyricDataGridViewTextBoxColumn, "lyricDataGridViewTextBoxColumn");
            this.lyricDataGridViewTextBoxColumn.Name = "lyricDataGridViewTextBoxColumn";
            this.lyricDataGridViewTextBoxColumn.ReadOnly = true;
            this.lyricDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // parsedLyricDataGridViewTextBoxColumn
            // 
            this.parsedLyricDataGridViewTextBoxColumn.DataPropertyName = "ParsedLyricView";
            resources.ApplyResources(this.parsedLyricDataGridViewTextBoxColumn, "parsedLyricDataGridViewTextBoxColumn");
            this.parsedLyricDataGridViewTextBoxColumn.Name = "parsedLyricDataGridViewTextBoxColumn";
            this.parsedLyricDataGridViewTextBoxColumn.ReadOnly = true;
            this.parsedLyricDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // uNoteBindingSource
            // 
            this.uNoteBindingSource.DataSource = typeof(App.Classes.UNote);
            // 
            // buttonReloadResources
            // 
            resources.ApplyResources(this.buttonReloadResources, "buttonReloadResources");
            this.buttonReloadResources.Name = "buttonReloadResources";
            this.buttonReloadResources.UseVisualStyleBackColor = true;
            this.buttonReloadResources.Click += new System.EventHandler(this.buttonReloadResources_Click);
            // 
            // PluginWindow
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lyricView);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginWindow";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.PluginWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uNoteBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView lyricView;
        private System.Windows.Forms.Button buttonAtlasConvert;
        private System.Windows.Forms.Button buttonSetText;
        private System.Windows.Forms.CheckBox checkBoxVR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxVCLength;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.BindingSource uNoteBindingSource;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.CheckBox checkBoxInsertShort;
        private System.Windows.Forms.Button buttonToCVC;
        private System.Windows.Forms.Button buttonCV;
        private System.Windows.Forms.Button buttonSplit;
        private System.Windows.Forms.Button buttonWhat;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxVelocity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn lyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parsedLyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonReloadResources;
    }
}