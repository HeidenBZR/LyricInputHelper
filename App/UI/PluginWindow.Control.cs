using LyricInputHelper.Classes;
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

        public static Dictionary<string, string> DefaultLyric = new Dictionary<string, string> { };
        public static double Velocity = 1;
        public static bool MakeVR = true;
        public static bool MakeShort = true;
        public static bool IsParsed = false;
        public static bool IsUnparsed = false;
        private static int MinLengthDefault;
        public static int MinLength;
        public static string LOG_Dir = @"log.txt";
        public static Dictionary<string, string> parents = new Dictionary<string, string>();
        public static Version VERSION = new Version(0, 4, 1, 0);
        public static double CompressionRatio = 1;
        public static double LastChildCompressionRatio = 1;
        public static bool LengthByOto = false;
        public static bool MakeFade = false;

        void Init()
        {
            MinLengthDefault = 110 * (int)Ust.Tempo / 120;
            MinLength = MinLengthDefault;
            textBoxMinLength.Text = MinLength.ToString();
            ImportDict();
            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.AddRange(Lang.Languages);
            comboBoxLanguage.SelectedItem = Lang.Current;
            SetLyric();
            buttonSetText.Enabled = false;
            SetTextWindow = new SetTextWindow();
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
                Atlas.Dict = new Dict(Atlas.DictPath);
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
            IsParsed = false;
            IsUnparsed = false;
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
                IsParsed = true;
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
            if (int.TryParse(textBoxMinLength.Text, out int vcLength)) MinLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
            try
            {
                Parser.Split();
                IsParsed = true;
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
                Parser.AtlasConverting();
                IsParsed = true;
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
                IsUnparsed = true;
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
                IsUnparsed = true;
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
                Ust.Notes[i].Syllable = new Syllable(dialog.NewLyric.Trim().Split(' '));
                Ust.Notes[i].ParsedLyric = Ust.Notes[i].Syllable.ToString();
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
                var window = new AddWordDialog();
                window.ShowDialog();
                if (window.DialogResult == DialogResult.OK)
                {
                    if (Atlas.AddWord(window.Word, window.Phonemes, window.IsToSendMail))
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
