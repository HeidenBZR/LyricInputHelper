
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{
    public partial class Ust
    {
        public void TakeOut(string line, string name, out string value) { value = line.Substring(name.Length + 1); }
        public void TakeOut(string line, string name, out int value) { value = int.Parse(line.Substring(name.Length + 1), new CultureInfo("ja-JP")); }
        public void TakeOut(string line, string name, out double value) { value = double.Parse(line.Substring(name.Length + 1), new CultureInfo("ja-JP")); }
        public string TakeIn(string name, dynamic value) { return $"{name}={value}"; }


        private void Read(Encoding encoding)
        {
            string[] lines = System.IO.File.ReadAllLines(Dir, encoding);
            int i = 0;
            // Reading version
            if (lines[0] == NumberManager.VERSION)
            {
                Version = 1.2;
                i++;
                i++;
            }
            if (lines[i] != NumberManager.SETTING)
                throw new Exception("Error UST reading");
            else i++;

            while (i < lines.Length && !NumberManager.IsNote(lines[i]))
            {
                if (lines[i].StartsWith("UstVersion")) TakeOut(lines[i], "UstVersion", out Version);
                if (lines[i].StartsWith("Tempo")) TakeOut(lines[i], "Tempo", out Tempo);
                if (lines[i].StartsWith("VoiceDir")) TakeOut(lines[i], "VoiceDir", out VoiceDir);
                i++;
            }

            List<Note> notes = new List<Note>();
            Note note = new Note();
            while (i + 1 < lines.Length)
            {
                note = new Note();
                note.Number = lines[i];
                i++;
                while (!NumberManager.IsNote(lines[i]))
                {
                    string line = lines[i];
                    if (lines[i].StartsWith("Length"))
                        if (int.TryParse(lines[i].Substring("Length=".Length), out int length))
                        {
                            note.Length = length;
                            note.FinalLength = length;
                        }
                    if (lines[i].StartsWith("Intensity="))
                        if (int.TryParse(lines[i].Substring("Intensity=".Length), out int intensity))
                            note.Intensity = intensity;
                    if (lines[i].StartsWith("NoteNum="))
                        if (int.TryParse(lines[i].Substring("NoteNum=".Length), out int noteNum))
                            note.NoteNum = noteNum;
                    if (lines[i].StartsWith("Flags="))
                        note.Flags = lines[i].Substring("Flags=".Length);
                    if (lines[i].StartsWith("Lyric="))
                    {
                        note.Lyric = lines[i].Substring("Lyric=".Length);
                        note.ParsedLyric = note.Lyric;
                    }
                    i++;
                    if (i == lines.Length) break;
                }
                notes.Add(note);
            }
            Notes = notes.ToArray();

            Console.WriteLine("Read UST successfully");
            IsLoaded = true;
            // Console.WriteLine(String.Join("\r\n", GetText()));
        }

        public string[] GetText()
        {
            List<string> text = new List<string> { };
            if (Version == 1.2)
            {
                text.Add(NumberManager.VERSION);
                text.Add("UST Version " + Version.ToString());
                text.Add(NumberManager.SETTING);
            }
            else
            {
                text.Add(NumberManager.SETTING);
                text.Add(TakeIn("Version", Version));
            }
            text.Add(TakeIn("Tempo", Tempo));
            text.Add(TakeIn("VoiceDir", VoiceDir));
            foreach (Note note in Notes)
                text.AddRange(note.GetText(Atlas));
            return text.ToArray();
        }



        public string[] Save(AtlasSettings settings)
        {
            SetLength(settings);
            if (settings.MakeFade)
                GetEnvelope();
            string[] text = GetText();
            File.WriteAllLines(Dir, text, Encoding.GetEncoding(932));
            Console.WriteLine("Successfully saved UST.");
            return text;
        }

        public void Save(string dir, AtlasSettings settings)
        {
            SetLength(settings);
            string[] text = GetText();
            File.WriteAllLines(dir, text, Encoding.GetEncoding(932));
            Console.WriteLine("Successfully saved debug UST.");
        }
    }
}
