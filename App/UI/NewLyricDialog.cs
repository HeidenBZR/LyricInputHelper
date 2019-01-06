using LyricInputHelper.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LyricInputHelper.Classes;

namespace LyricInputHelper.UI
{
    public partial class NewLyricDialog : Form
    {
        public string NewLyric;

        public NewLyricDialog(string lyric)
        {
            InitializeComponent();
            NewLyric = lyric;
            labelLyric.Text = lyric;
            textBoxNewLyric.Text = lyric;
            SetLang();
        }

        void SetLang()
        {
            Text = Lang.Get("change_lyric_dialog_title");
            labelOfCurrentText.Text = Lang.Get("current_text");
            labelOfNewText.Text = Lang.Get("new_text");
            labelOfWarning.Text = Lang.Get("no_text_validation_warning");
            buttonOk.Text = Lang.Get("button_ok");
            buttonCancel.Text = Lang.Get("button_cancel");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            NewLyric = textBoxNewLyric.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
