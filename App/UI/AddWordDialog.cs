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

namespace LyricInputHelper.UI
{
    public partial class AddWordDialog : Form
    {
        public string Word;
        public string Phonemes;
        public bool IsToSendMail;
        public Atlas Atlas;

        public AddWordDialog(Atlas atlas)
        {
            Atlas = atlas;
            InitializeComponent();
            SetLang();
            DialogResult = DialogResult.Cancel;
        }

        public AddWordDialog(Atlas atlas, string word) : this(atlas)
        {
            textBoxWord.Text = word;
        }

        public AddWordDialog(Atlas atlas, string word, string phonemes) : this(atlas, word)
        {
            textBoxPhonemes.Text = phonemes;
        }

        void SetLang()
        {
            if (Atlas.ExampleWord != null && Atlas.Dict.Has(Atlas.ExampleWord))
            {
                string word = $" ({Atlas.ExampleWord}).";
                string phonemes = $" ({Syllable.ToString(Atlas.Dict.Get(Atlas.ExampleWord))}).";
                labelWord.Text = Lang.Get("addword_word_example_word") + word;
                labelPhonemes.Text = Lang.Get("addword_phonemes_example_word") + phonemes;
            }
            else
            {
                labelWord.Text = Lang.Get("addword_word");
                labelPhonemes.Text = Lang.Get("addword_phonemes");
            }
            Text = Lang.Get("addword_title");
            buttonOk.Text = Lang.Get("button_ok");
            buttonCancel.Text = Lang.Get("button_cancel");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (textBoxWord.Text.Trim() == "" || textBoxPhonemes.Text.Trim() == "")
                return;
            Word = textBoxWord.Text;
            Phonemes = textBoxPhonemes.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetStatus(string text)
        {
            labelStatus.Text = text;
        }
    }
}
