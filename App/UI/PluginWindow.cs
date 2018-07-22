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
        static Dictionary<string, string> DefaultLyric = new Dictionary<string, string> { };
        static string[] lastText;
        public static int MinSize = 120;
        public static bool makeVR = true;
        public static bool makeShort = true;
        public static bool parsed = false;
        private static int VCLengthDefault;
        public static int VCLength;
        public static string LOG_Dir = @"log.txt";
        public static Dictionary<string, string> parents = new Dictionary<string, string>();
        
        public PluginWindow()
        {
            InitializeComponent();
            window = this;
            VCLengthDefault = 150 * (int)Ust.Tempo / 120;
            VCLength = VCLengthDefault;
            textBoxVCLength.Text = VCLength.ToString();
            SetInitMessage();
            SetLyric();
        }


        public void SetInitMessage()
        {
            string message = "";

            if (Atlas.IsDefault) message += "Информация о типе голосового банка не найдена или отсутствует атлас " +
                    "для текущего типа банка, поэтому будет использоваться атлас по умолчанию.\r\n" +
                    "Чтобы внести информацию о типе войсбанка, добавьте информацию о нем в файл character.txt, " +
                    "добавив строку вида:\r\n" +
                    "VoicebankType=MyVoicebankType\r\n" +
                    "Например:\r\n" +
                    "VoicebankType=CVC RUS\r\n" +
                    "Убедитесь, что в папке atlas в директории плагина присутствует файл атласа с именем в формате " +
                    "VoicebankType.atlas, например, \"CVC RUS.atlas\".\r\n\r\n";
            message += $"Атлас: {Path.GetFileName(Atlas.AtlasPath)}.\r\n";
            if (Atlas.HasDict) message += $"Словарь: {Path.GetFileName(Atlas.DictPath)}.\r\n";
            message +=
                "\r\nВходной уст должен представлять собой CV уст, содержащий CV, V и C ноты. " +
                "Обратите внимание, что C ноты в усте должны стоять отдельно, в этой версии " +
                "конвертер еще не умеет их разделять.\r\n\r\n" +
                "Конвертер создает VC ноты заданной длины, подобрать их точную длину нужно " +
                "самостоятельно.\r\n\r\n" +
                "Вы можете ввести текст, нажав кнопку \"Ввести текст\". Введенный текст можно " +
                "конвертировать, а можно сохранить без конвертирования, нажав кнопку OK. \r\n\r\n";
            if (Atlas.HasDict) message += $"Для этого типа войсбанка доступен словарь, поэтому вы " +
                    $"можете вводить алиасы на кириллице, или иным способом, предусмотренным словарем. " +
                    $"Для уточнения, откройте файл {Path.GetFileName(Atlas.DictPath)} в папке atlas " +
                    $"в директории плагина.\r\n";
            statusBar.Text = message;
        }

        public void SetLyric()
        {
            lyricView.DataSource = Ust.Notes;
            lyricView.Refresh();
            Recolor(lyricView);
        }

        public static void Recolor(DataGridView lyricView)
        {
            Color SelectionForeColor = Color.Gray;
            Color LightColor = Color.FromArgb(220, 230, 240);
            Color MediumColor = Color.FromArgb(180, 190, 190);
            Color DarkColor = Color.FromArgb(80, 110, 120);
            DataGridViewCellStyle defaultCellStyle = lyricView.DefaultCellStyle.Clone();
            defaultCellStyle.SelectionBackColor = lyricView.DefaultCellStyle.BackColor;
            defaultCellStyle.SelectionForeColor = DarkColor;

            DataGridViewCellStyle darkCellStyle = defaultCellStyle.Clone();
            darkCellStyle.BackColor = DarkColor;
            darkCellStyle.SelectionBackColor = DarkColor;
            darkCellStyle.SelectionForeColor = MediumColor;
            darkCellStyle.ForeColor = MediumColor;

            DataGridViewCellStyle mediumCellStyle = defaultCellStyle.Clone();
            mediumCellStyle.BackColor = LightColor;
            mediumCellStyle.SelectionBackColor = LightColor;
            mediumCellStyle.SelectionForeColor = MediumColor;

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
                if (note.IsRest()) lyricView.Rows[i].DefaultCellStyle = darkCellStyle;
            }
        }

        public static void SetStatus(string text, bool appendTextbox = false)
        {
            if (window != null && window.statusBar != null)
            {
                if (appendTextbox) window.statusBar.Text += "\r\n" + text;
                else window.statusBar.Text = text;
            }
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

        private void buttonAtlasConvert_Click(object sender, EventArgs e)
        {
            if (parsed) Ust.Reload();
            if (!int.TryParse(textBoxVCLength.Text, out VCLength)) VCLength = VCLengthDefault;
            makeVR = checkBoxVR.Checked;
            makeShort = checkBoxInsertShort.Checked;
            parsed = true;
            try
            {
                Parser.AtlasConverting();
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex);
                return;
            }

            SetLyric();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            if (parsed)
            {
                Ust.Reload();
                parsed = false;
            }
            if (lastText == null) lastText = Ust.GetLyrics(skipRest:false);
            int[] sizes = Ust.GetLengths();
            SetTextWindow setTextWindow = new SetTextWindow(lastText.ToList(), sizes);
            DialogResult result = setTextWindow.ShowDialog(this);
            Ust.SetLyric(setTextWindow.CurrentText.ToArray());
            SetLyric();
        }

        private void lyricView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.ColumnIndex;
            int y = e.RowIndex;
            if (x == 1) Ust.Notes[y].Lyric = (string)lyricView[x,y].Value;
            if (x == 2) Ust.Notes[y].ParsedLyric = (string)lyricView[x,y].Value;
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
            parsed = false;
            SetLyric();
        }
    }
}
