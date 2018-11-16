using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.UI
{
    public partial class NewLyricDialog : Form
    {
        public string NewLyric;

        public NewLyricDialog(string lyric)
        {
            InitializeComponent();
            NewLyric = lyric;
            labelLyric.Text = lyric;
            textBoxNewLyric.Text = lyric;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            NewLyric = textBoxNewLyric.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
