using System;
using System.Windows.Forms;

namespace App.UI
{
    public partial class StandaloneWindow : Form
    {
        public StandaloneWindow()
        {
            InitializeComponent();
            SetLang();
        }

        void SetLang()
        {
            label1.Text = Classes.Lang.Get("standalone_window_message");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
