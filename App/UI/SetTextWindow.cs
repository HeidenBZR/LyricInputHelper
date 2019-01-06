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
    public enum InputMode
    {
        LyricInput,
        PhoneticInput
    }

    public partial class SetTextWindow : Form
    {
        public List<Syllable> Syllables;
        public List<Word> Words;
        public int[] sizes;
        public bool Cancel = true;
        public InputMode InputMode { get; set; } = InputMode.LyricInput;

        public SetTextWindow()
        {
            InitializeComponent();
            SetLang();

            SetInputMode();
        }

        public void SetTitle()
        {
            string dict = Atlas.HasDict ? Lang.Get("dict_enabled") : Lang.Get("dict_disabled");
            SetTitle(Lang.Get("set_text_dialog_title"), dict);
        }

        void SetTitle(string title, string dict)
        {
            Text = $"{title} [{Atlas.VoicebankType}] [{dict}]";
        }

        public void SetInputMode()
        {
            if (!Atlas.Dict.IsEnabled)
            {
                InputMode = InputMode.PhoneticInput;
                comboBoxInputMode.Enabled = false;
            }
            else
            {
                comboBoxInputMode.Enabled = true;
                InputMode = InputMode.LyricInput;
            }
            comboBoxInputMode.SelectedIndex = (int)InputMode;
        }

        public void SetLang()
        {
            buttonOk.Text = Lang.Get("button_ok");
            buttonCancel.Text = Lang.Get("button_cancel");
            string dict = Atlas.HasDict ? Lang.Get("dict_enabled") : Lang.Get("dict_disabled");
            SetTitle(Lang.Get("set_text_dialog_title"), dict);
        }

        private bool ReDict(List<string> texts)
        {
            if (!Atlas.HasDict) return true;
            Words = new List<Word>();
            for (int i = 0; i < texts.Count; i++)
            {
                Word word = new Word(texts[i], Atlas.Dict.Get(texts[i]));
                while (word.Phonemes is null)
                {
                    // добавление слова
                    bool success = false;
                    while (!success)
                    {
                        var window = new AddWordDialog(texts[i]);
                        window.SetStatus($"Слово \"{texts[i]}\" отсутствует в словаре. Добавьте слово");
                        window.ShowDialog();
                        if (window.DialogResult == DialogResult.OK)
                        {
                            if (Atlas.AddWord(window.Word, window.Phonemes, window.IsToSendMail))
                            {
                                window.SetStatus($"Слово \"{window.Word}\" успешно добавлено.");
                                word = new Word(texts[i], Atlas.Dict.Get(texts[i]));
                                success = true;
                            }
                            else
                                window.SetStatus("Ошибка при добавлении слова.");
                        }
                        else return false;
                    }
                    word.Phonemes = Atlas.Dict.Get(texts[i]);
                }
                Words.Add(word);
            }
            Syllables = texts.Select(n => new Syllable(n.Split(' '))).ToList();
            return true;
        }

        string EcranLyric(string text)
        {
            text = text.Replace("\r\n", " ");
            text = text.Replace("-", " ");
            text = text.Replace("—", "");
            text = text.Replace(".", "");
            text = text.Replace("?", "");
            text = text.Replace("!", "");
            text = text.Replace(",", "");
            text = text.Replace(":", "");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("\"", "");
            text = text.Replace("\"", "");
            text = text.Replace("_", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, " {2,}", " ");
            text = text.ToLower();
            text = text.Trim();
            return text;
        }

        string EcranPhonetics(string text)
        {
            text = text.Trim();
            text = text.Trim('\r', '\n');
            text = text.Replace(" \r\n", "\r\n");
            text = text.Replace("\r\n ", "\r\n");
            text = System.Text.RegularExpressions.Regex.Replace(text, " {2,}", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, "(\r\n){2,}", "\r\n");
            text = text.Replace("\r", "");
            return text;
        }

        public string PrintLyric()
        {
            StringBuilder text = new StringBuilder();
            foreach (var note in Ust.Notes)
            {
                if (Atlas.IsRest(note.ParsedLyric))
                {
                    if (text.Length > 0)
                    {
                        if (text[text.Length - 1] == ' ')
                            text[text.Length - 1] = '\n';
                        else
                            text.Append("\n");
                    }
                }
                else if (note.Word.Name is null) { }
                else
                {
                    text.Append(note.WordName);
                    text.Append(" ");
                }
            }
            if (text.Length > 0)
                while (text[0] == '\n')
                    text.Remove(0, 1);
            if (text.Length > 0)
                while (text[text.Length - 1] == '\n')
                    text.Remove(text.Length - 1, 1);
            text.Replace("\n", "\r\n");
            text = new StringBuilder(System.Text.RegularExpressions.Regex.Replace(text.ToString(), 
                "(\r\n){3,}", "\r\n\r\n"));
            return text.ToString();
        }

        public string PrintSyllables()
        {
            StringBuilder text = new StringBuilder();
            foreach (var note in Ust.Notes)
            {
                if (Atlas.IsRest(note.ParsedLyric))
                {
                    if (text.Length > 0)
                    {
                        text.Append("\n");
                    }
                }
                else
                {
                    text.Append(note.ParsedLyric);
                    text.Append("\n");
                }
            }
            if (text.Length > 0)
                while (text[0] == '\n')
                    text.Remove(0, 1);
            if (text.Length > 0)
                while (text[text.Length - 1] == '\n')
                    text.Remove(text.Length - 1, 1);
            text.Replace("\n", "\r\n");
            text = new StringBuilder(System.Text.RegularExpressions.Regex.Replace(text.ToString(),
                "(\r\n){3,}", "\r\n\r\n"));
            return text.ToString();
        }

        public bool LoseTextWarning()
        {
            if (textBox.Text.Trim().Length == 0)
                return true;
            var result = MessageBox.Show(Lang.Get("lose_text_warning"), "", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                InputMode = (InputMode)comboBoxInputMode.SelectedIndex;
                string text = textBox.Text;
                List<string> texts;
                if (InputMode == InputMode.PhoneticInput)
                {
                    text = EcranPhonetics(text);
                    texts = text.Split('\n').ToList();
                    Syllables = texts.Select(n => new Syllable(n.Split(' '))).ToList();
                    for (int i = 0; i < Syllables.Count; i++)
                        Syllables[i] = Atlas.PhonemeReplace(Syllables[i]);
                    Cancel = false;
                    Close();
                }
                else
                {
                    text = EcranLyric(text);
                    texts = text.Split(' ').ToList();
                    if (ReDict(texts))
                    {
                        foreach (var word in Words)
                            for (int i = 0; i < word.Syllables.Count; i++)
                                word.Syllables[i] = Atlas.PhonemeReplace(word.Syllables[i]);
                        Cancel = false;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on set text");
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void buttonLastText_Click(object sender, EventArgs e)
        {
            if (Words is null)
                return;
            if (LoseTextWarning())
            {
                textBox.Text = PrintLyric();
                comboBoxInputMode.SelectedIndex = (int)InputMode.LyricInput;
            }
        }

        private void buttonLastPhonemes_Click(object sender, EventArgs e)
        {
            if (LoseTextWarning())
            {
                textBox.Text = PrintSyllables();
                comboBoxInputMode.SelectedIndex = (int)InputMode.PhoneticInput;
            }
        }

    }
}
