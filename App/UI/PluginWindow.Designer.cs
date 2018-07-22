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
            this.lyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parsedLyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uNoteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonAtlasConvert = new System.Windows.Forms.Button();
            this.buttonSetText = new System.Windows.Forms.Button();
            this.checkBoxVR = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxVCLength = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxInsertShort = new System.Windows.Forms.CheckBox();
            this.buttonReload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).BeginInit();
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
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // lyricView
            // 
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
            // lyricDataGridViewTextBoxColumn
            // 
            this.lyricDataGridViewTextBoxColumn.DataPropertyName = "Lyric";
            resources.ApplyResources(this.lyricDataGridViewTextBoxColumn, "lyricDataGridViewTextBoxColumn");
            this.lyricDataGridViewTextBoxColumn.Name = "lyricDataGridViewTextBoxColumn";
            // 
            // parsedLyricDataGridViewTextBoxColumn
            // 
            this.parsedLyricDataGridViewTextBoxColumn.DataPropertyName = "ParsedLyric";
            resources.ApplyResources(this.parsedLyricDataGridViewTextBoxColumn, "parsedLyricDataGridViewTextBoxColumn");
            this.parsedLyricDataGridViewTextBoxColumn.Name = "parsedLyricDataGridViewTextBoxColumn";
            // 
            // uNoteBindingSource
            // 
            this.uNoteBindingSource.DataSource = typeof(App.Classes.UNote);
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
            // statusBar
            // 
            this.statusBar.AcceptsReturn = true;
            this.statusBar.AcceptsTab = true;
            resources.ApplyResources(this.statusBar, "statusBar");
            this.statusBar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusBar.Name = "statusBar";
            this.statusBar.ReadOnly = true;
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
            resources.ApplyResources(this.checkBoxInsertShort, "checkBoxInsertShort");
            this.checkBoxInsertShort.Name = "checkBoxInsertShort";
            this.toolTip.SetToolTip(this.checkBoxInsertShort, resources.GetString("checkBoxInsertShort.ToolTip"));
            this.checkBoxInsertShort.UseVisualStyleBackColor = true;
            // 
            // buttonReload
            // 
            resources.ApplyResources(this.buttonReload, "buttonReload");
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // PluginWindow
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.checkBoxInsertShort);
            this.Controls.Add(this.checkBoxVR);
            this.Controls.Add(this.textBoxVCLength);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.lyricView);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAtlasConvert);
            this.Controls.Add(this.buttonSetText);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.buttonOK);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginWindow";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.PluginWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uNoteBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TextBox statusBar;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.BindingSource uNoteBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn lyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parsedLyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.CheckBox checkBoxInsertShort;
    }
}