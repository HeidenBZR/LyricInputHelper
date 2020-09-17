using VAtlas;
using LyricInputHelper.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LyricInputHelper
{
    public partial class PluginWindow
    {


        public Ust Ust;
        public Atlas Atlas;
        public Parser Parser;

        public void Init(Ust ust, Atlas atlas)
        {
            Atlas = atlas;
            Ust = ust;
            Parser = new Parser(Atlas, Ust);
            Settings.MinLengthDefault = 110 * (int)Ust.Tempo / 120;
            Settings.MinLength = Settings.MinLengthDefault;
            textBoxMinLength.Text = Settings.MinLength.ToString();
            ImportDict();
            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.AddRange(Lang.Languages);
            comboBoxLanguage.SelectedItem = Lang.Current;
            SetLyric();
            buttonSetText.Enabled = false;
            SetTextWindow = new SetTextWindow(Atlas, Ust);
            if (Singer.Current.IsLoaded)
                checkBoxLengthByOto.Checked = true;
        }

        public void SetLyric()
        {
            lyricView.DataSource = Ust.Notes;
            lyricView.Refresh();
            Recolor(lyricView);
        }

        public void ImportDict()
        {
            try
            {
                Atlas.Dict = new Dict(Atlas.GetDictPath(), Atlas);
                Atlas.Dict.OnDictLoadEnd += (bool success) =>
                {
                    if (success)
                        labelStatus.Text = Lang.Get("import_dict");
                    else
                        labelStatus.Text = Lang.Get("import_dict_fail");
                    buttonSetText.Enabled = true;
                    SetTextWindow.SetInputMode();
                };
                labelStatus.Text = Lang.Get("import_dict_before");
                Atlas.Dict.Import();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on end init");
                return;
            }
        }

        void Reset()
        {
            Ust.Reload();
            Settings.IsParsed = false;
            Settings.IsUnparsed = false;
            CheckAccess();
            SetLyric();
            SetLang();
            SetTitle();
        }

        void SetText()
        {
            GetValues();
            SetTextWindow.SetTitle();
            DialogResult result = SetTextWindow.ShowDialog(this);
            if (SetTextWindow.Cancel) return;
            try
            {
                if (SetTextWindow.InputMode == InputMode.LyricInput)
                    Ust.SetLyric(SetTextWindow.Words);
                else
                    Ust.SetLyric(SetTextWindow.Syllables);
                Settings.IsParsed = true;
                CheckAccess();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }
            SetLyric();
        }

        void Split()
        {
            GetValues();
            if (int.TryParse(textBoxMinLength.Text, out int vcLength)) Settings.MinLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Settings.Velocity = velocity;
            try
            {
                Parser.Split();
                Settings.IsParsed = true;
                CheckAccess();
                labelStatus.Text = Lang.Get("button_reload_resources");
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }
            SetLyric();
        }

        void Convert()
        {
            GetValues();
            try
            {
                Parser.AtlasConverting(Settings);
                Settings.IsParsed = true;
                CheckAccess();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }
            SetLyric();
        }

        void ToCV()
        {
            GetValues();
            try
            {
                Parser.ToCV();
                Settings.IsUnparsed = true;
                CheckAccess();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }
            SetLyric();
        }

        void ToCVC()
        {
            GetValues();
            try
            {
                Parser.ToCVC();
                Parser.Split();
                Settings.IsUnparsed = true;
                CheckAccess();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }
            SetLyric();
        }

        void EditNote(int i)
        {
            // if (x == 1) Ust.Notes[y].Lyric = (string)lyricView[x,y].Value;
            var dialog = new NewLyricDialog(Ust.Notes[i].ParsedLyric);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Ust.Notes[i].Syllable = new Syllable(dialog.NewLyric.Trim().Split(' '), Atlas);
                Ust.Notes[i].SetParsedLyric(Atlas, Ust.Notes[i].Syllable.ToString());
                SetLyric();
            }
        }

        void Reload()
        {
            buttonSetText.Enabled = false;
            Singer.Current.Reload();
            Atlas.Reload();
            Atlas.Dict.Import();
        }

        void AddWord()
        {
            try
            {
                var window = new AddWordDialog(Atlas);
                window.ShowDialog();
                if (window.DialogResult == DialogResult.OK)
                {
                    if (Atlas.AddWord(window.Word, window.Phonemes))
                        labelStatus.Text = $"Слово \"{window.Word}\" успешно добавлено.";
                    else
                        labelStatus.Text = "Ошибка при добавлении слова.";
                }

            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Ошибка при добавлении слова.");
                labelStatus.Text = "Ошибка при добавлении слова.";
                return;
            }
        }

    }
}
