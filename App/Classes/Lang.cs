﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public static class Lang
    {
        public static string[] Languages { get; set; }
        static Dictionary<string, string> messages;
        static string _default = "ru";
        public static string Current;

        public static void Init()
        {
            Languages = Directory.EnumerateFiles(Program.GetResourceFolder("LyricInputHelper", "lang"), 
                "*.txt", SearchOption.TopDirectoryOnly).Select(n => Path.GetFileNameWithoutExtension(n)).ToArray();
            var lang_file = Program.GetResourceFile("LyricInputHelper", "lang", "lang");
            if (File.Exists(lang_file))
            {
                string filename = File.ReadAllText(lang_file).Trim(' ', '\n', '\r');
                if (Languages.Contains(filename))
                {
                    Set(filename);
                    return;
                }
            }
            Set(_default);
        }

        static void Save(string filename)
        {
            var lang_file = Program.GetResourceFile("LyricInputHelper", "lang", "lang");
            try
            {
                if (!File.Exists(lang_file))
                    File.Create(lang_file);
                File.WriteAllText(lang_file, filename);
            }
            catch (Exception ex)
            {
                Program.Log($"Failed to save lang file. \n\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void Set(string lang)
        {
            messages = new Dictionary<string, string>();
            string[] lines;
            try
            {
                lines = File.ReadAllLines(Program.GetResourceFile("LyricInputHelper", "lang", $"{lang}.txt"), Encoding.UTF8);
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
            Current = lang;
            Save(lang);
        }

        public static string Get(string code)
        {
            if (messages.ContainsKey(code))
                return messages[code];
            else
                return code;
        }

        public static bool Has(string code)
        {
            return messages.ContainsKey(code);
        }
    }
}
