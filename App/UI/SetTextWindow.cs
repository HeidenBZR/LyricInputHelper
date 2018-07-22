using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Classes;

namespace App.UI
{
    public partial class SetTextWindow : Form
    {
        public List<string> CurrentText;
        public int[] sizes;

        public SetTextWindow(List<string> text, int[] sizes)
        {
            if (Atlas.HasDict) Atlas.ReloadDict();
            InitializeComponent();
            this.sizes = sizes;
            List<string> normalized = Normalize(text);
            if (CurrentText == null) CurrentText = normalized;
            textBox.Text = String.Join(" ", normalized);
            textBox.Text = textBox.Text.Replace("\r\n ", "\r\n");
            textBox.Text = textBox.Text.Replace(" \r\n", "\r\n");
            textBox.Text = textBox.Text.Trim(new char[] { '\r','\n' });
            labelNotesSelected.Text = normalized.Count.ToString();
            SetMinSize();

            labelTip.Text = "Введите алиасы. Можно ввести как алиасы CV, C и V, которые в последствии " +
                    "будут преобразованы по атласу, так и сразу конечные алиасы.\r\n";
            labelTip.Text += " Алиасы разделяются пробелами, символом дефиса или переносами строк. " +
                "Нижние подчеркивания будут преобразованы в пробелы, например, введите \"-_va\" " +
                "вместо - va.\r\n" +
                "Можно вводить символы в любом регистре.\r\n" +
                "Будут стерты символы: .,:\"()!? \r\n\r\n";
            if (Atlas.HasDict)
            {
                string key = Atlas.Dict.Keys.First();
                labelTip.Text += "Для этого войсбанка доступен словарь. Пример преобразования:\r\n";
                if (Atlas.VoicebankType == "CVC RUS") labelTip.Text += "ку => ku";
                else labelTip.Text += $"{key} => {Atlas.Dict[key]}";
                labelTip.Text += "\r\nОбратите внимание, что словарь предосталяет правила транслитерации, " +
                    "но не делает фонетический анализ.";
            }
        }

        private void SetMinSize()
        {
            int syls = sizes.Count(n => n >= PluginWindow.MinSize);
            labelSylSelected.Text = syls.ToString();
            labelCSelected.Text = (sizes.Length - syls).ToString();
        }

        private List<string> Normalize(List<string> texts)
        {
            List<string> normalized = new List<string>();
            foreach (string text in texts)
            {
                string t = text;
                if (t == " ") t = "\r\n";
                t = t.Replace(" ", "_");
                normalized.Add(t);
            }
            return normalized;
        }

        private List<string> Denormalize(List<string> texts)
        {
            List<string> denormalized = new List<string>();
            foreach (string text in texts)
            {
                denormalized.Add(text.Replace("_", " "));
            }
            return denormalized;
        }

        private List<string> ReDict(List<string> texts)
        {
            if (!Atlas.HasDict) return texts;
            List<string> redicted = new List<string>();
            foreach (string text in texts)
            {
                if (Atlas.Dict.ContainsKey(text))
                    redicted.Add(Atlas.Dict[text]);
                else
                {
                    redicted.Add(text);
                    Program.Log($"Cant find dict rule for \"{text}\"");
                }
            }
            return redicted;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CurrentText.Clear();
            string text = textBox.Text.Replace("\r\n", " ");
            text = text.Replace("-", " ");
            text = text.Replace(".", "");
            text = text.Replace("?", "");
            text = text.Replace("!", "");
            text = text.Replace(",", "");
            text = text.Replace(":", "");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("'", "");
            text = text.Replace("\"", "");
            text = text.ToLower();
            CurrentText = ReDict(Denormalize(text.Split(' ').ToList()));
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxMinSize_TextChanged(object sender, EventArgs e)
        {
            SetMinSize();
        }
    }
}
