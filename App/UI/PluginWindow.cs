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
        }


        public void SetInitMessage()
        {
            Message = "";

            if (Atlas.IsDefault) Message += "Информация о типе голосового банка не найдена или отсутствует атлас " +
                    "для текущего типа банка, поэтому будет использоваться атлас по умолчанию.\r\n" +
                    "Чтобы внести информацию о типе войсбанка, добавьте информацию о нем в файл character.txt, " +
                    "добавив строку вида:\r\n" +
                    "VoicebankType=MyVoicebankType\r\n" +
                    "Например:\r\n" +
                    "VoicebankType=CVC RUS\r\n" +
                    "Убедитесь, что в папке atlas в директории плагина присутствует файл атласа с именем в формате " +
                    "VoicebankType.atlas, например, \"CVC RUS.atlas\".\r\n\r\n";
            Message += $"Атлас: {Path.GetFileName(Atlas.AtlasPath)}.\r\n";
            if (Atlas.HasDict) Message += $"Словарь: {Path.GetFileName(Atlas.DictPath)}.\r\n";
            Message +=
                "\r\nВходной уст должен представлять собой CV уст, содержащий CV, V и C ноты. " +
                "Обратите внимание, что C ноты в усте должны стоять отдельно, в этой версии " +
                "конвертер еще не умеет их разделять.\r\n\r\n" +
                "Конвертер создает VC ноты заданной длины, подобрать их точную длину нужно " +
                "самостоятельно.\r\n\r\n" +
                "Вы можете ввести текст, нажав кнопку \"Ввести текст\". Введенный текст можно " +
                "конвертировать, а можно сохранить без конвертирования, нажав кнопку OK. \r\n\r\n";
            if (Atlas.HasDict) Message += $"Для этого типа войсбанка доступен словарь, поэтому вы " +
                    $"можете вводить алиасы на кириллице, или иным способом, предусмотренным словарем. " +
                    $"Для уточнения, откройте файл {Path.GetFileName(Atlas.DictPath)} в папке atlas " +
                    $"в директории плагина.\r\n";
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
                if (note.Number == Classes.Number.Delete)
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
            IsParsed = false;
            IsUnparsed = false;
            CheckAccess();
            SetLyric();
        }

        private void buttonSetText_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxVCLength.Text, out int vcLength)) VCLength = vcLength;
            if (double.TryParse(textBoxVelocity.Text, out double velocity)) Velocity = velocity;
            if (lastText == null) lastText = Ust.GetLyrics(skipRest: false);
            int[] sizes = Ust.GetLengths();
            SetTextWindow setTextWindow = new SetTextWindow(lastText.ToList(), sizes);
            DialogResult result = setTextWindow.ShowDialog(this);
            if (setTextWindow.Cancel) return;
            try
            {
                Ust.SetLyric(setTextWindow.CurrentText.ToArray());
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
    }
}
