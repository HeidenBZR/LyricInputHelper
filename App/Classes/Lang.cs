using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
{
    public static class Lang
    {
        static Dictionary<string, string> messages;

        public static void Init(string lang)
        {
            messages = new Dictionary<string, string>();
            string[] lines;
            try
            {
                lines = File.ReadAllLines($"lang/{lang}.txt");
            }
            catch (Exception) { return; }
            foreach (var line in lines)
            {
                if (line == "") continue;
                try
                {
                    var l = line.Split('=');
                    messages[l[0]] = l[1];
                }
                catch (Exception) { }
            }
        }

        public static string Get(string code)
        {
            if (messages.ContainsKey(code))
                return messages[code];
            else
                return code;
        }
    }
}
