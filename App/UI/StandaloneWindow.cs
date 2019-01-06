using LyricInputHelper.Classes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LyricInputHelper.UI
{
    public partial class StandaloneWindow : Form
    {
        public StandaloneWindow()
        {
            InitializeComponent();
            Lang.Init();
            SetLang();
            Setup();
        }

        void SetLang()
        {
            labelSetupMessage.Text = "LyricInputHelper instalatiion started.";
            labelSetupAtlas.Text = "";
            labelSetupResources.Text = "";
            labelSetupTemp.Text = "";
        }

        bool SetupAtlas()
        {
            try
            {
                Program.GetResourceFolder("atlas");
                if (Directory.Exists("atlas"))
                {
                    foreach (var filename in Directory.GetFiles("atlas"))
                        if (!File.Exists(Program.GetResourceFile(filename)))
                            File.Move(filename, Program.GetResourceFile(filename));
                        else
                            File.Delete(filename);

                    Directory.Delete("atlas");
                }
                labelSetupAtlas.Text = "Atlases are successfully installed.";
                return true;
            }
            catch (Exception ex)
            {
                labelSetupAtlas.Text = "Error on aliases installation.";
                labelSetupMessage.Text = "Installation failed.";
                Program.ErrorMessage(ex, "Error on aliases installation.");
                return false;
            }
        }

        bool SetupResources()
        {
            try
            {
                Program.GetResourceFolder("LyricInputHelper", "lang");
                if (Directory.Exists("lang"))
                {
                    foreach (var filename in Directory.GetFiles("lang"))
                        if (!File.Exists(Program.GetResourceFile("LyricInputHelper", filename)))
                            File.Move(filename, Program.GetResourceFile("LyricInputHelper", filename));
                        else
                            File.Delete(filename);
                    Directory.Delete("lang");
                }
                labelSetupResources.Text = "Resources are successfully installed.";
                return true;
            }
            catch (Exception ex)
            {
                labelSetupResources.Text = "Error on resources installation.";
                labelSetupMessage.Text = "Installation failed.";
                Program.ErrorMessage(ex, "Error on resources installation.");
                return false;
            }
        }

        bool SetupTemp()
        {

            try
            {
                Program.GetTempFolder("LyricInputHelper");
                if (!File.Exists(Program.GetTempFile("LyricInputHelper", "log.txt")))
                    File.Create(Program.GetTempFile("LyricInputHelper", "log.txt")).Close();
                if (!File.Exists(Program.GetTempFile("LyricInputHelper", "ust.tmp")))
                    File.Create(Program.GetTempFile("LyricInputHelper", "ust.tmp")).Close();
                //labelSetupTemp.Text = "Temp folder  successfully created.";
                return true;
            }
            catch (Exception ex)
            {
                labelSetupTemp.Text = "Error on temp folder creation.";
                labelSetupMessage.Text = "Installation failed.";
                Program.ErrorMessage(ex, "Error on temp folder creation.");
                return false;
            }
        }

        void Setup()
        {
            if (SetupAtlas() & SetupResources() & SetupTemp())
                labelSetupMessage.Text = "Plugin is successfully installed!";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void urlShortcutToDesktop(string linkName, string linkUrl, string ToolTip)
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(linkName + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + linkUrl);
                writer.Flush();
            }
        }
    }
}
