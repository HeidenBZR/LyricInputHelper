namespace LyricInputHelper
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lyricView = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Word = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAtlasConvert = new System.Windows.Forms.Button();
            this.buttonSetText = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxInsertShort = new System.Windows.Forms.CheckBox();
            this.buttonSplit = new System.Windows.Forms.Button();
            this.buttonToCVC = new System.Windows.Forms.Button();
            this.buttonToCV = new System.Windows.Forms.Button();
            this.textBoxVelocity = new System.Windows.Forms.TextBox();
            this.checkBoxVR = new System.Windows.Forms.CheckBox();
            this.textBoxMinLength = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAddWord = new System.Windows.Forms.Button();
            this.buttonReloadResources = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelOfMultiplayer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.label = new System.Windows.Forms.Label();
            this.textBoxLastChildCompressionRatio = new System.Windows.Forms.TextBox();
            this.labelOfLastChildCompressionRatio = new System.Windows.Forms.Label();
            this.textBoxCompressionRatio = new System.Windows.Forms.TextBox();
            this.labelOfCompressionRatio = new System.Windows.Forms.Label();
            this.labelOfMinLength = new System.Windows.Forms.Label();
            this.checkBoxLengthByOto = new System.Windows.Forms.CheckBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.lyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parsedLyricDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Syllable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uNoteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxFade = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uNoteBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
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
            this.parsedLyricDataGridViewTextBoxColumn,
            this.Syllable,
            this.Word});
            this.lyricView.DataSource = this.uNoteBindingSource;
            resources.ApplyResources(this.lyricView, "lyricView");
            this.lyricView.Name = "lyricView";
            this.lyricView.RowHeadersVisible = false;
            this.lyricView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lyricView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lyricView.Tag = "";
            this.lyricView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lyricView_CellClick);
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
            // Word
            // 
            this.Word.DataPropertyName = "WordName";
            resources.ApplyResources(this.Word, "Word");
            this.Word.Name = "Word";
            this.Word.ReadOnly = true;
            // 
            // buttonAtlasConvert
            // 
            resources.ApplyResources(this.buttonAtlasConvert, "buttonAtlasConvert");
            this.buttonAtlasConvert.Name = "buttonAtlasConvert";
            this.buttonAtlasConvert.UseVisualStyleBackColor = true;
            this.buttonAtlasConvert.Click += new System.EventHandler(this.buttonAtlasConvert_Click);
            this.buttonAtlasConvert.MouseEnter += new System.EventHandler(this.buttonAtlasConvert_MouseEnter);
            // 
            // buttonSetText
            // 
            resources.ApplyResources(this.buttonSetText, "buttonSetText");
            this.buttonSetText.Name = "buttonSetText";
            this.buttonSetText.UseVisualStyleBackColor = true;
            this.buttonSetText.Click += new System.EventHandler(this.buttonSetText_Click);
            this.buttonSetText.MouseEnter += new System.EventHandler(this.buttonSetText_MouseEnter);
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
            this.buttonSplit.UseVisualStyleBackColor = true;
            this.buttonSplit.Click += new System.EventHandler(this.buttonSplit_Click);
            this.buttonSplit.MouseEnter += new System.EventHandler(this.buttonSplit_MouseEnter);
            // 
            // buttonToCVC
            // 
            resources.ApplyResources(this.buttonToCVC, "buttonToCVC");
            this.buttonToCVC.Name = "buttonToCVC";
            this.buttonToCVC.UseVisualStyleBackColor = true;
            this.buttonToCVC.Click += new System.EventHandler(this.buttonToCVC_Click);
            this.buttonToCVC.MouseEnter += new System.EventHandler(this.buttonToCVC_MouseEnter);
            // 
            // buttonToCV
            // 
            resources.ApplyResources(this.buttonToCV, "buttonToCV");
            this.buttonToCV.Name = "buttonToCV";
            this.buttonToCV.UseVisualStyleBackColor = true;
            this.buttonToCV.Click += new System.EventHandler(this.buttonToCV_Click);
            this.buttonToCV.MouseEnter += new System.EventHandler(this.buttonToCV_MouseEnter);
            // 
            // textBoxVelocity
            // 
            resources.ApplyResources(this.textBoxVelocity, "textBoxVelocity");
            this.textBoxVelocity.Name = "textBoxVelocity";
            // 
            // checkBoxVR
            // 
            resources.ApplyResources(this.checkBoxVR, "checkBoxVR");
            this.checkBoxVR.Checked = true;
            this.checkBoxVR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVR.Name = "checkBoxVR";
            this.checkBoxVR.UseVisualStyleBackColor = true;
            this.checkBoxVR.MouseEnter += new System.EventHandler(this.checkBoxVR_MouseEnter);
            // 
            // textBoxMinLength
            // 
            resources.ApplyResources(this.textBoxMinLength, "textBoxMinLength");
            this.textBoxMinLength.Name = "textBoxMinLength";
            this.textBoxMinLength.MouseEnter += new System.EventHandler(this.textBoxMinLength_MouseEnter);
            // 
            // buttonReset
            // 
            resources.ApplyResources(this.buttonReset, "buttonReset");
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            this.buttonReset.MouseEnter += new System.EventHandler(this.buttonReset_MouseEnter);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonAddWord);
            this.panel1.Controls.Add(this.buttonReloadResources);
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.buttonToCVC);
            this.panel1.Controls.Add(this.buttonToCV);
            this.panel1.Controls.Add(this.buttonSetText);
            this.panel1.Controls.Add(this.buttonSplit);
            this.panel1.Controls.Add(this.buttonAtlasConvert);
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonAddWord
            // 
            resources.ApplyResources(this.buttonAddWord, "buttonAddWord");
            this.buttonAddWord.Name = "buttonAddWord";
            this.buttonAddWord.UseVisualStyleBackColor = true;
            this.buttonAddWord.Click += new System.EventHandler(this.buttonAddWord_Click);
            this.buttonAddWord.MouseEnter += new System.EventHandler(this.buttonAddWord_MouseEnter);
            // 
            // buttonReloadResources
            // 
            resources.ApplyResources(this.buttonReloadResources, "buttonReloadResources");
            this.buttonReloadResources.Name = "buttonReloadResources";
            this.buttonReloadResources.UseVisualStyleBackColor = true;
            this.buttonReloadResources.Click += new System.EventHandler(this.buttonReloadResources_Click);
            this.buttonReloadResources.MouseEnter += new System.EventHandler(this.buttonReloadResources_MouseEnter);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageMain);
            this.tabControl.Controls.Add(this.tabPageOptions);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.lyricView);
            this.tabPageMain.Controls.Add(this.panel1);
            resources.ApplyResources(this.tabPageMain, "tabPageMain");
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.panel3);
            this.tabPageOptions.Controls.Add(this.panel2);
            resources.ApplyResources(this.tabPageOptions, "tabPageOptions");
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBoxInsertShort);
            this.panel3.Controls.Add(this.textBoxVelocity);
            this.panel3.Controls.Add(this.labelOfMultiplayer);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // labelOfMultiplayer
            // 
            resources.ApplyResources(this.labelOfMultiplayer, "labelOfMultiplayer");
            this.labelOfMultiplayer.Name = "labelOfMultiplayer";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxFade);
            this.panel2.Controls.Add(this.comboBoxLanguage);
            this.panel2.Controls.Add(this.label);
            this.panel2.Controls.Add(this.textBoxLastChildCompressionRatio);
            this.panel2.Controls.Add(this.labelOfLastChildCompressionRatio);
            this.panel2.Controls.Add(this.textBoxCompressionRatio);
            this.panel2.Controls.Add(this.labelOfCompressionRatio);
            this.panel2.Controls.Add(this.textBoxMinLength);
            this.panel2.Controls.Add(this.labelOfMinLength);
            this.panel2.Controls.Add(this.checkBoxLengthByOto);
            this.panel2.Controls.Add(this.checkBoxVR);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxLanguage, "comboBoxLanguage");
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            this.comboBoxLanguage.MouseEnter += new System.EventHandler(this.comboBoxLanguage_MouseEnter);
            // 
            // label
            // 
            resources.ApplyResources(this.label, "label");
            this.label.Name = "label";
            // 
            // textBoxLastChildCompressionRatio
            // 
            resources.ApplyResources(this.textBoxLastChildCompressionRatio, "textBoxLastChildCompressionRatio");
            this.textBoxLastChildCompressionRatio.Name = "textBoxLastChildCompressionRatio";
            this.textBoxLastChildCompressionRatio.MouseEnter += new System.EventHandler(this.textBoxLastChildCompressionRatio_MouseEnter);
            // 
            // labelOfLastChildCompressionRatio
            // 
            resources.ApplyResources(this.labelOfLastChildCompressionRatio, "labelOfLastChildCompressionRatio");
            this.labelOfLastChildCompressionRatio.Name = "labelOfLastChildCompressionRatio";
            // 
            // textBoxCompressionRatio
            // 
            resources.ApplyResources(this.textBoxCompressionRatio, "textBoxCompressionRatio");
            this.textBoxCompressionRatio.Name = "textBoxCompressionRatio";
            this.textBoxCompressionRatio.MouseEnter += new System.EventHandler(this.textBoxCompressionRatio_MouseEnter);
            // 
            // labelOfCompressionRatio
            // 
            resources.ApplyResources(this.labelOfCompressionRatio, "labelOfCompressionRatio");
            this.labelOfCompressionRatio.Name = "labelOfCompressionRatio";
            // 
            // labelOfMinLength
            // 
            resources.ApplyResources(this.labelOfMinLength, "labelOfMinLength");
            this.labelOfMinLength.Name = "labelOfMinLength";
            // 
            // checkBoxLengthByOto
            // 
            resources.ApplyResources(this.checkBoxLengthByOto, "checkBoxLengthByOto");
            this.checkBoxLengthByOto.Checked = true;
            this.checkBoxLengthByOto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLengthByOto.Name = "checkBoxLengthByOto";
            this.checkBoxLengthByOto.UseVisualStyleBackColor = true;
            this.checkBoxLengthByOto.MouseEnter += new System.EventHandler(this.checkBoxLengthByOto_MouseEnter);
            // 
            // labelStatus
            // 
            resources.ApplyResources(this.labelStatus, "labelStatus");
            this.labelStatus.Name = "labelStatus";
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
            // Syllable
            // 
            this.Syllable.DataPropertyName = "Syllable";
            resources.ApplyResources(this.Syllable, "Syllable");
            this.Syllable.Name = "Syllable";
            // 
            // uNoteBindingSource
            // 
            this.uNoteBindingSource.DataSource = typeof(LyricInputHelper.Classes.Note);
            // 
            // checkBoxFade
            // 
            resources.ApplyResources(this.checkBoxFade, "checkBoxFade");
            this.checkBoxFade.Checked = true;
            this.checkBoxFade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFade.Name = "checkBoxFade";
            this.checkBoxFade.UseVisualStyleBackColor = true;
            this.checkBoxFade.MouseEnter += new System.EventHandler(this.checkBoxFade_MouseEnter);
            // 
            // PluginWindow
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginWindow";
            this.Load += new System.EventHandler(this.PluginWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lyricView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uNoteBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView lyricView;
        private System.Windows.Forms.Button buttonAtlasConvert;
        private System.Windows.Forms.Button buttonSetText;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.BindingSource uNoteBindingSource;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonToCVC;
        private System.Windows.Forms.Button buttonToCV;
        private System.Windows.Forms.Button buttonSplit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonReloadResources;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Button buttonAddWord;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxMinLength;
        private System.Windows.Forms.TextBox textBoxVelocity;
        private System.Windows.Forms.Label labelOfMinLength;
        private System.Windows.Forms.Label labelOfMultiplayer;
        private System.Windows.Forms.CheckBox checkBoxVR;
        private System.Windows.Forms.CheckBox checkBoxInsertShort;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxCompressionRatio;
        private System.Windows.Forms.Label labelOfCompressionRatio;
        private System.Windows.Forms.TextBox textBoxLastChildCompressionRatio;
        private System.Windows.Forms.Label labelOfLastChildCompressionRatio;
        private System.Windows.Forms.CheckBox checkBoxLengthByOto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn lyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parsedLyricDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Syllable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Word;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.CheckBox checkBoxFade;
    }
}