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
        public static Version VERSION = new Version(0, 3, 4, 1);

        public static string Message;

        static string[] lastText;

        public PluginWindow()
        {
            InitializeComponent();
            window = this;
            VCLengthDefault = 110 * (int)Ust.Tempo / 120;
            VCLength = VCLengthDefault;
            textBoxVCLength.Text = VCLength.ToString();
            SetLang();
            SetLyric();
            SetTitle();
        }

        void SetTitle()
        {
            Text = $"autoCVC v.{VERSION.ToString()} ({Singer.Current.Name} - {Atlas.VoicebankType})";
        }

        void SetLang()
        {
            Message = "";
            if (Atlas.IsDefault)
                Message = $"{Lang.Get("plugin_window_message_no_dict")}\r\n\r\n" +
                    $"{Lang.Get("plugin_window_message_no_dict2")}\r\n\r\n" +
                    "type=CVC RUS\r\n" +
                    $"{Lang.Get("plugin_window_message_no_dict3")}\r\n\r\n";
            Message += $"{Lang.Get("plugin_window_message_atlas")}: {Path.GetFileName(Atlas.AtlasPath)}.\r\n";
            if (Atlas.HasDict)
                Message += $"{ Lang.Get("plugin_window_message_dict")}: { Path.GetFileName(Atlas.DictPath)}.\r\n\r\n";
            if (Atlas.HasDict) Message += $"{Lang.Get("plugin_window_message_has_dict")} " +
                    $"{Path.GetFileName(Atlas.DictPath)} " +
                    $"{Lang.Get("plugin_window_message_has_dict")}\r\n\r\n";
            Message += $"{Lang.Get("tutorial")}\r\n";
            Message += $"{Lang.Get("tutorial2")}\r\n";
            Message += $"{Lang.Get("tutorial3")}\r\n";
            Message += $"{Lang.Get("tutorial4")}\r\n";
            Message += $"{Lang.Get("tutorial5")}\r\n";
            Message += $"{Lang.Get("tutorial6")}\r\n";
            Message += $"{Lang.Get("tutorial7")}\r\n";
            Message += $"{Lang.Get("tutorial8")}\r\n\r\n";
            Message += $"{Lang.Get("plugin_window_message_reset")}\r\n";
            Message += $"{Lang.Get("plugin_window_message_reload")}\r\n";

            checkBoxInsertShort.Text = Lang.Get("checkbox_insert_short");
            checkBoxVR.Text = Lang.Get("checkbox_insert_vr");
            labelOfVrLength.Text = Lang.Get("vr_length");
            labelOfMultiplayer.Text = Lang.Get("multiplayer");

            buttonSetText.Text = Lang.Get("button_set_text");
            buttonSplit.Text = Lang.Get("button_split");
            buttonAtlasConvert.Text = Lang.Get("button_atlas_convert");

            buttonReload.Text = Lang.Get("button_reload_ust");
            buttonToCVC.Text = Lang.Get("button_to_cv_c");
            buttonToCV.Text = Lang.Get("button_to_cv");

            buttonReloadResources.Text = Lang.Get("button_reload_resources");

            buttonOK.Text = Lang.Get("button_ok");
            buttonCancel.Text = Lang.Get("button_cancel");
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
            window.buttonToCV.Enabled = !IsParsed;
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
            SetLang();
            SetTitle();
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
