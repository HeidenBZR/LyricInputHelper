using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VAtlas;

namespace Avis
{
    class Program
    {
        public static bool IsDebug;
        public static string[] Args;
        public static NumberFormatInfo NFI;
        public static Encoding Encoding;
        public static AvisManager Manager;

        [STAThread]
        static void Main(string[] args)
        {
            Args = args;
#if DEBUG
            IsDebug = true;
#endif

            AvisManager manager = null;
            Try(() =>
            {
                NFI = new CultureInfo("en-US", false).NumberFormat;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding = Encoding.GetEncoding(932);

                var ust = new Ust(args[0], Encoding);
                if (!ust.IsLoaded)
                    throw new Exception("Failed to read UST");

                var singer = new Singer(ust.VoiceDir);
                if (!singer.IsLoaded)
                    throw new Exception("Failed to read Singer");

                var atlas = new Atlas(ust.VoiceDir);
                if (!atlas.IsLoaded)
                    throw new Exception("Failed to read Atlas");
                ust.SetAtlas(atlas);

                manager = new AvisManager(ust, atlas, singer);

                Errors.Log("Ust loaded");
            }, "Failed to load data");

            if (manager == null)
                return;
            Manager = manager;
            Manager.Start();
        }

        public static void Try(Action action, string errorMessage = "An error occured.")
        {
#if !DEBUG
            try
            {
#endif
            action.Invoke();
#if !DEBUG
            }
            catch (Exception ex)
            {
                ErrorMessage(ex, message);
            }
#endif
        }
    }
}
