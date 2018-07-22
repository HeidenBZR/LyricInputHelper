using System;
using System.IO;
using App.Classes;
using App.UI;
using System.Windows.Forms;
using System.Globalization;

namespace App
{

    class Program
    {
        public enum Mode
        {
            Standalone,
            Plugin,
            Resampler=12,
            Wavtool
        };
        public static Mode mode;
        public static Ust ust;
        public static Oto oto;
        public static Atlas atlas;
        public static string LOG_Dir = @"log.txt";
        public static string[] args;
        public static NumberFormatInfo NFI;
        public static Settings settings;

        [STAThread]
        static void Main(string[] args)
        {
            NFI = new CultureInfo("en-US", false).NumberFormat;
            Program.args = args;
            mode = DetectMode();
            // SaveArgs();
            Init();
            // Save
        }

        static void SaveArgs()
        {
            string text = "";
            for (int i = 0; i < args.Length; i++)
            {
                text += $"Arg #{i}: \"{args[i]}\"\n";
            }
            Log(text);
        }

        static void Init()
        {
            switch (mode)
            {
                case Mode.Plugin:
                    InitLog("Plugin");
                    PluginModeInit();
                    break;
                case Mode.Resampler:
                    InitLog("Resampler");
                    Console.WriteLine("Hi stranger");
                    Console.WriteLine("It's been a while");
                    break;
                case Mode.Standalone:
                    InitLog("Standalone");
                    StandaloneModeInit();
                    break;
            }
        }

        static void PluginModeInit()
        {
            try
            {
                ust = new Ust(args[0]);
                if (Ust.IsLoaded) Log("Ust loaded"); else Log($"Error reading UST");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on reading atlas");
                return;
            }
            try
            {
                atlas = new Atlas(Ust.VoiceDir);
                if (Atlas.IsLoaded) Log("Atlas loaded"); else Log($"Error reading atlas");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on reading ust");
                return;
            }
            Log("All files loaded successfully");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginWindow());
        }

        public static void ErrorMessage (Exception ex, string name = "Error")
        {
            string oops = "Упс, что-то пошло не так. Напишите автору, приложив скриншот ошибки, приложив " +
                "файлы log.txt и ust.tmp из папки плагина.\r\n\r\n";
            Log(ex.Message);
            MessageBox.Show(ex.TargetSite + ": " + ex.Message +"\r\n" + ex.StackTrace, name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void StandaloneModeInit()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StandaloneWindow());

        }

        static Mode DetectMode()
        {
            Mode mode;
            switch (args.Length)
            {
                case 1:
                    if (Ust.IsTempUst(args[0]))
                    {
                        mode = Mode.Plugin;
                    } else
                    {
                        mode = Mode.Standalone;
                    }
                    break;
                case 6:
                case 12:
                    mode = Mode.Wavtool;
                    break;
                case 13:
                    mode = Mode.Resampler;
                    break;
                default:
                    mode = Mode.Standalone;
                    break;
            }
            Console.WriteLine($"{mode} mode");
            return mode;

        }

        static void InitLog(string type)
        {
            using (StreamWriter log = new StreamWriter(LOG_Dir, false, System.Text.Encoding.UTF8))
            {
                log.WriteLine($"====== Mode: {type} ======");
                log.WriteLine(DateTime.Now.ToString(format: "d.MMM.yyyy, HH:mm:ss"));
                log.Close();
            }
        }

        public static void Log(string text, bool saveUST = true, bool appendTextbox = false)
        {
            using (StreamWriter log = new StreamWriter(LOG_Dir, true, System.Text.Encoding.UTF8))
            {
                string type;
                switch (args.Length)
                {
                    case 6:
                        type = "wavtool for R";
                        break;
                    default:
                        type = mode.ToString();
                        break;
                }
                log.WriteLine(text);
                log.Close();

            }

            if (saveUST && Ust.IsLoaded)
            {
                File.Copy(args[0], "ust.tmp", true);
            }

            PluginWindow.SetStatus(text, appendTextbox);
        }

    }
}
