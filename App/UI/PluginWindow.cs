using System;
using System.Windows.Forms;
using App.Classes;
using System.Collections.Generic;
using App.UI;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Globalization;

namespace App
{
    public partial class PluginWindow : Form
    {
        public static PluginWindow window;
        public static Dictionary<string, string> DefaultLyric = new Dictionary<string, string> { };
        public static int MinSize = 120;
        public static double Velocity = 1;
        public static bool MakeVR = true;
        public static bool MakeShort = true;
        public static bool IsParsed = false;
        public static bool IsUnparsed = false;
        private static int VCLengthDefault;
        public static int VCLength;
        public static string LOG_Dir = @"log.txt";
        public static Dictionary<string, string> parents = new Dictionary<string, string>();

        public static string Message;

        static string[] lastText;

        public PluginWindow()
        {
            InitializeComponent();
            window = this;
            VCLengthDefault = 110 * (int)Ust.Tempo / 120;
            VCLength = VCLengthDefault;
            textBoxVCLength.Text = VCLength.ToString();
            SetInitMessage();
            SetLyric();
            // Set Version and title
            Text = $"autoCVC v.0.3.3 ({Singer.Current.Name} - {Atlas.VoicebankType})";
        }


        public void SetInitMessage()
        {
            Message = "";

            if (Atlas.IsDefault) Message += "Информация о типе голосового банка не найдена или отсутствует атлас " +
                    "для текущего типа банка, поэтому будет использоваться атлас по умолчанию.\r\n" +
                    "Чтобы внести информацию о типе войсбанка, добавьте информацию о нем в файл character.txt, " +
                    "добавив строку вида:\r\n" +
                    "type=MyVoicebankType\r\n" +
                    "Например:\r\n" +
                    "type=CVC RUS\r\n" +
                    "Убедитесь, что в папке atlas в директории плагина присутствует файл атласа с именем в формате " +
                    "VoicebankType.atlas, например, \"CVC RUS.atlas\".\r\n\r\n";
            Message += $"Атлас: {Path.GetFileName(Atlas.AtlasPath)}.\r\n";
            if (Atlas.HasDict) Message += $"Словарь: {Path.GetFileName(Atlas.DictPath)}.\r\n\r\n";
            if (Atlas.HasDict) Message += $"Для этого типа войсбанка доступен словарь, поэтому вы " +
                    $"можете вводить алиасы на кириллице, или иным способом, предусмотренным словарем. " +
                    $"Для уточнения, откройте файл {Path.GetFileName(Atlas.DictPath)} в папке atlas " +
                    $"в директории плагина с помощью любого текстового редактора.\r\n";
            
            Message +=
                "\r\nВозможно несколько режимов работы: \r\n" +
                "1. Вы работаете с импортированный MIDI-файлом или CV-устом (без отдельных выдохов и C-нот). " +
                "Нажмите кнопку \"Ввести текст\", введите текст (если доступен словарь) либо фонетические " +
                "единицы (фонемы для arpasing и CV/V/C для остальных типов банка). Проверьте правильность " +
                "введенных единиц. При необходимости замените текст, нажав на нужную ячейку. Если все правильно, " +
                "нажмите \"Разбить\", затем \"Конвертировать\".\r\n" +
                "2. Вы работаете с presamp-устом. В этом случае достаточно нажать кнопку \"Конвертировать\".\r\n" +
                "3. Вы работаете с готовым устом на основе другого реклиста. \r\n" +
                "3.1. Для этого реклиста есть atlas: запустите плагин с установленным голосовым банком " +
                "на основе этого реклиста и нажмите кнопку \"Преобразовать в CV\" (для arpasing) или " +
                "\"Преобразовать в CV + C\" (для остальных типов). Примените изменения, после чего снова запустите " +
                "плагин с вашим голосовым банком, и перейдите к пункту 1, если это был arpasing уст, " +
                "или к пункту 2 для других типов реклиста. \r\n" +
                "3.2. Файла atlas нет: если возможно, преобразуйте уст к CV-виду посредством других плагинов " +
                "и перейдите к пункту 1. " +
                "Если нет, бог вам в помощь. \r\n\r\n" +
                "" +
                "Используйте кнопку \"Сбросить все изменения\", если всё пошло не так.\r\n\r\n" +
                "Используйте кнопку \"Перезагрузить ресурсы\", если вам нужно, чтобы плагин заново прочитал " +
                "конфигурацию голосового банка, реклиста и словарь.";

        }

        public void SetLyric()
        {
            lyricView.DataSource = Ust.Notes;
            lyricView.Refresh();
            Recolor(lyricView);
        }

        public static void Recolor(DataGridView lyricView)
        {
            Color SelectionForeColor = Color.DarkGray;
            Color LightColor = Color.FromArgb(220, 230, 240);
            Color MediumColor = Color.FromArgb(180, 190, 190);
            Color DarkColor = Color.FromArgb(80, 110, 120);
            DataGridViewCellStyle defaultCellStyle = lyricView.DefaultCellStyle.Clone();
            defaultCellStyle.SelectionBackColor = Color.LightBlue;
            defaultCellStyle.SelectionForeColor = DarkColor;

            DataGridViewCellStyle darkCellStyle = defaultCellStyle.Clone();
            darkCellStyle.BackColor = DarkColor;
            darkCellStyle.SelectionBackColor = DarkColor;
            darkCellStyle.SelectionForeColor = MediumColor;
            darkCellStyle.ForeColor = MediumColor;

            DataGridViewCellStyle mediumCellStyle = defaultCellStyle.Clone();
            mediumCellStyle.BackColor = LightColor;
            mediumCellStyle.SelectionBackColor = Color.LightSkyBlue;
            mediumCellStyle.SelectionForeColor = DarkColor;

            DataGridViewCellStyle deleteCellStyle = defaultCellStyle.Clone();
            deleteCellStyle.ForeColor = Color.DarkGray;
            deleteCellStyle.BackColor = Color.LightGray;
            deleteCellStyle.SelectionBackColor = Color.DimGray;
            deleteCellStyle.SelectionForeColor = Color.LightGray;


            lyricView.Columns[0].DefaultCellStyle = darkCellStyle;
            lyricView.DefaultCellStyle = defaultCellStyle;

            for (int i = 0; i < lyricView.Rows.Count; i++)
            {
                UNote note = Ust.Notes[i];
                if (note.Length <= MinSize)
                {
                    lyricView[1,i].Style = mediumCellStyle;
                    lyricView[2,i].Style = mediumCellStyle;
                }
                if (note.IsRest())
                {
                    lyricView[1, i].Style = darkCellStyle;
                    lyricView[2, i].Style = darkCellStyle;
                }
                if (note.Number == Classes.Number.DELETE)
                {
                    lyricView[1, i].Style = deleteCellStyle;
                    lyricView[2, i].Style = deleteCellStyle;
                }
            }
        }

        public static void SetStatus(string text, bool appendTextbox = false)
        {
            if (appendTextbox) Message += "\r\n" + text;
            else Message = text;
        }

        public static void CheckAccess()
        {
            window.buttonSetText.Enabled = !IsUnparsed;
            window.buttonSplit.Enabled = !IsUnparsed;
            window.buttonAtlasConvert.Enabled = !IsUnparsed;
            window.buttonCV.Enabled = !IsParsed;
            window.buttonToCVC.Enabled = !IsParsed;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Ust.Save();
            Application.Exit();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lyricView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textMinSize_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int newMinSize)) MinSize = newMinSize;
            Recolor(lyricView);
        }

        private void PluginWindow_Load(object sender, EventArgs e)
        {
            Recolor(lyricView);
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            Ust.Reload();
            IsParsed = false;
            IsUnparsed = false;
            CheckAccess();
            SetLyric();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
            //if (lastText == null)
                lastText = Ust.GetLyrics(skipRest: false);
            int[] sizes = Ust.GetLengths();
            SetTextWindow setTextWindow = new SetTextWindow(lastText.ToList(), sizes);
            DialogResult result = setTextWindow.ShowDialog(this);
            if (setTextWindow.Cancel) return;
            try
            {
                var phonemes = setTextWindow.CurrentText.ToArray();
                Ust.SetLyric(phonemes);
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

        private void buttonSplit_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
            try
            {
                Parser.Split();
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

        private void buttonAtlasConvert_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
            MakeVR = checkBoxVR.Checked;
            MakeShort = checkBoxInsertShort.Checked;
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

        private void buttonToCVC_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
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

        private void buttonToCV_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
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

        private void buttonWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lyricView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.ColumnIndex;
            int y = e.RowIndex;
            if (x == 2)
            {
                // if (x == 1) Ust.Notes[y].Lyric = (string)lyricView[x,y].Value;
                var dialog = new NewLyricDialog(Ust.Notes[y].ParsedLyric);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Ust.Notes[y].ParsedLyric = dialog.NewLyric.Trim(' ');
                    SetLyric();
                }
            }

        }

        private void buttonReloadResources_Click(object sender, EventArgs e)
        {
            Singer.Current.Reload();
            Atlas.Reload();
            MessageBox.Show("Вокалист, атлас и словарь были перезагружены.", "Обновление ресурсов");
        }
    }
}
