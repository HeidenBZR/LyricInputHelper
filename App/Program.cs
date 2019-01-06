using System;
using System.IO;
using LyricInputHelper.Classes;
using LyricInputHelper.UI;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LyricInputHelper
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
        public static string LOG_DIR { get { return GetTempFile("LyricInputHelper", @"log.txt"); } }
        public static string[] args;
        public static NumberFormatInfo NFI;
        public static Settings settings;

        static void InitLang()
        {
            try
            {
                Lang.Init();
            }
            catch(ExecutionEngineException ex)
            {
                Program.ErrorMessage(ex, "Error on Lang Init");
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            InitLang();
            try
            {
                NFI = new CultureInfo("en-US", false).NumberFormat;
                Program.args = args;
                mode = DetectMode();
            }
            catch (Exception ex)
            {
                try
                {
                    File.WriteAllText(LOG_DIR,
                        $"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n{ex.Message}\r\n",
                        Encoding.UTF8
                        );

                }
                catch (Exception ex2)
                {

                    MessageBox.Show($"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n" +
                        $"{ex.Message}\r\n\r\n" +
                        $"{ex2.Message}\r\n{ex2.Source}\r\n{ex2.TargetSite.ToString()}\r\n" +
                        $"{ex2.Message}\r\n", "Error");
                }
            }
            Init();
        }

        static void CheckFolder(params string[] path)
        {
            try
            {
                string dir;
                for (int i = 0; i < path.Length - 2; i++)
                {
                    dir = Path.Combine(path.ToList().Take(i + 2).ToArray());
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on file access");
            }
        }

        public static string GetResourceFile(params string[] path)
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            return Path.Combine(path);
        }

        public static string GetResourceFolder(params string[] path)
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            var dir = Path.Combine(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetTempFile(params string[] path)
        {
            var root = Path.GetTempPath();
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            return Path.Combine(path);
        }

        public static string GetTempFolder(params string[] path)
        {
            var root = Path.GetTempPath();
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            var dir = Path.Combine(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
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
                if (Ust.IsLoaded)
                    Log("Ust loaded");
                else Log($"Error reading UST");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on reading ust");
                return;
            }
            try
            {
                if (Ust.IsLoaded)
                    atlas = new Atlas(Ust.VoiceDir);
                if (Atlas.IsLoaded)
                    Log("Atlas loaded");
                else
                    Log($"Error reading atlas {Atlas.AtlasPath}");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, $"Error on reading atlas {Atlas.AtlasPath}");
                return;
            }
            try
            {
                var Singer = new Singer(Ust.VoiceDir);
                if (Singer.IsLoaded) Log("Singer loaded"); else Log($"Error reading singer {Ust.VoiceDir}");
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on reading singer");
                return;
            }
            Log("All files loaded successfully");
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, "Error on end init");
                return;
            }
            var window = new PluginWindow();
            Application.Run(window);
        }

        public static void ErrorMessage (Exception ex, string name = "Error")
        {
            Log(ex.Message);
            MessageBox.Show(ex.TargetSite + ": " + ex.Message +"\r\n" + ex.StackTrace, name, 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                if (!File.Exists(LOG_DIR))
                    File.Create(LOG_DIR).Close();
                using (StreamWriter log = new StreamWriter(LOG_DIR, false, System.Text.Encoding.UTF8))
                {
                    log.WriteLine($"====== Mode: {type} ======");
                    log.WriteLine(DateTime.Now.ToString(format: "d.MMM.yyyy, HH:mm:ss"));
                    log.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n" +
                    $"{ex.StackTrace}\r\n", "Error");
            }
        }

        public static void Log(string text, bool saveUST = true, bool appendTextbox = false)
        {
            try
            {
                using (StreamWriter log = new StreamWriter(LOG_DIR, true, System.Text.Encoding.UTF8))
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n" +
                    $"{ex.StackTrace}\r\n", "Error");
            }

            try
            {
                if (saveUST && Ust.IsLoaded)
                {
                    File.Copy(args[0], GetTempFile("autocvc", "ust.tmp"), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n" +
                    $"{ex.StackTrace}\r\n", "Error");
            }

            try
            {
                PluginWindow.SetStatus(text, appendTextbox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.Source}\r\n{ex.TargetSite.ToString()}\r\n" +
                    $"{ex.StackTrace}\r\n", "Error");
            }

        }

    }
}
