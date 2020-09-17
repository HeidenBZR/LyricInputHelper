using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{
    public class PathResolver
    {
        public static string GetResourceFile(params string[] path)
        {
            Errors.DebugLog("GetResourceFile: " + string.Join(", ", path));
            var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            return Path.Combine(path);
        }

        public static string GetResourceFolder(params string[] path)
        {
            Errors.DebugLog("GetResourceFolder: " + string.Join(", ", path));
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
            Errors.DebugLog("GetTempFile: " + string.Join(", ", path));
            var root = Path.GetTempPath();
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            return Path.Combine(path);
        }

        public static string GetTempFolder(params string[] path)
        {
            Errors.DebugLog("GetTempFolder: " + string.Join(", ", path));
            var root = Path.GetTempPath();
            path = path.ToList().Prepend(root).ToArray();
            CheckFolder(path);
            var dir = Path.Combine(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
        static void CheckFolder(params string[] path)
        {
            Errors.DebugLog("CheckFolder: " + string.Join(", ", path));
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
                Errors.ErrorMessage(ex, "Error on file access");
            }
        }

    }
}
