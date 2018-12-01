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
        public string Message;
        public bool Cancel = true;

        public SetTextWindow(List<string> text, int[] sizes)
        {
            // if (Atlas.HasDict) Atlas.ReloadDict();
            InitializeComponent();
            this.sizes = sizes;
            List<string> normalized = Normalize(text);
            if (CurrentText == null) CurrentText = normalized;
            textBox.Text = String.Join(" ", normalized);
            textBox.Text = textBox.Text.Replace("\r\n ", "\r\n");
            textBox.Text = textBox.Text.Replace(" \r\n", "\r\n");
            textBox.Text = textBox.Text.Trim(new char[] { '\r','\n' });
            //labelNotesSelected.Text = normalized.Count.ToString();
            SetMinSize();
            SetLang();
        }
        void SetTitle(string title, string dict)
        {
            Text = $"{title} [{Atlas.VoicebankType}] [{dict}]";
        }

        void SetLang()
        {
            Message = $"{Lang.Get("set_text_message")}\r\n\r\n" +
                $"{Lang.Get("set_text_message2")}\r\n\r\n" +
                $"{Lang.Get("set_text_message3")}\r\n\r\n" ;
            if (Atlas.HasDict)
            {
                string key = Atlas.Dict.Keys.ToArray()[Atlas.Dict.Count / 2];
                if (Atlas.VoicebankType == "CVC RUS")
                    key = "привет";
                Message += $"{Lang.Get("set_text_has_dict_message")}\r\n\r\n" +
                    $"{key} => {String.Join(" ", Atlas.Dict[key].Select(n => $"[{n}]"))}\r\n\r\n" +
                    $"{Lang.Get("set_text_has_dict_message2")}";
            }
            buttonOK.Text = Lang.Get("button_ok");
            buttonCancel.Text = Lang.Get("button_cancel");
            checkBoxHyphen.Text = Lang.Get("checkbox_hyphen");
            string dict = Atlas.HasDict ? Lang.Get("dict_enabled") : Lang.Get("dict_disabled");
            SetTitle(Lang.Get("set_text_dialog_title"), dict);
        }

        private void SetMinSize()
        {
            int syls = sizes.Count(n => n >= PluginWindow.MinSize);
            //labelSylSelected.Text = syls.ToString();
            //labelCSelected.Text = (sizes.Length - syls).ToString();
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
                redicted.AddRange(Atlas.DictAnalysis(text));
            return redicted;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CurrentText.Clear();
            string text = textBox.Text.Replace("\r\n", " ");
            if (!checkBoxHyphen.Checked)
            {
                text = text.Replace("-", " ");
            }
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
            text = System.Text.RegularExpressions.Regex.Replace(text, " {2,}", " ");
            CurrentText = ReDict(Denormalize(text.ToLower().Split(' ').ToList()));
            Cancel = false;
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

        private void buttonWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
