using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace App.Classes
{
    enum Insert : int
    {
        Before = -1,
        After = 0
    }

    class Ust
    {
        public static string VoiceDir;
        public static double Tempo;
        public static double Version;
        public static UNote[] Notes { get; set; }

        public static bool IsLoaded = false;
        public static string Dir;

        public static void TakeOut(string line, string name, out string value) { value = line.Substring(name.Length + 1); }
        public static void TakeOut(string line, string name, out int value) { value = int.Parse(line.Substring(name.Length + 1), new CultureInfo("ja-JP")); }
        public static void TakeOut(string line, string name, out double value) { value = double.Parse(line.Substring(name.Length + 1), new CultureInfo("ja-JP")); }
        public static string TakeIn(string name, dynamic value) { return $"{name}={value}"; }
        
        public Ust(string dir)
        {
            Dir = dir;
            string[] lines = File.ReadAllLines(Dir, System.Text.Encoding.GetEncoding(932));
            Read(lines);
        }

        public static void Reload()
        {
            string[] lines = File.ReadAllLines(Dir);
            Read(lines);
        }

        public static string[] Save()
        {
            string[] text = GetText();
            File.WriteAllLines(Dir, text);
            Console.WriteLine("Successfully saved UST.");
            return text;
        }

        public static void Save(string dir)
        {
            string[] text = GetText();
            File.WriteAllLines(dir, text);
            Console.WriteLine("Successfully saved debug UST.");
        }


        private static void Read(string[] lines)
        {
            int i = 0;
            // Reading version
            if (lines[0] == Number.Version)
            {
                Version = 1.2;
                i++;
                i++;
            }
            if (lines[i] != Number.Setting) throw new Exception("Error UST reading");
            else i++;

            while (i < lines.Length && !Number.IsNote(lines[i]))
            {
                if (lines[i].StartsWith("UstVersion")) TakeOut(lines[i], "UstVersion", out Version);
                if (lines[i].StartsWith("Tempo")) TakeOut(lines[i], "Tempo", out Tempo);
                if (lines[i].StartsWith("VoiceDir")) TakeOut(lines[i], "VoiceDir", out VoiceDir);
                i++;
            }

            List<UNote> notes = new List<UNote>();
            UNote note = new UNote();
            while (i + 1 < lines.Length)
            {
                note = new UNote();
                note.Number = lines[i];
                i++;
                while (!Number.IsNote(lines[i]))
                {
                    string line = lines[i];
                    if (lines[i].StartsWith("Length")) TakeOut(line, "Length", out note.Length);
                    if (lines[i].StartsWith("NoteNum")) TakeOut(line, "NoteNum", out note.NoteNum);
                    if (lines[i].StartsWith("Lyric"))
                    {
                        TakeOut(line, "Lyric", out string lyric);
                        note.Lyric = lyric;
                        note.ParsedLyric = note.Lyric;
                    }
                    i++;
                    Console.WriteLine(i);
                    if (i == lines.Length) break;
                }
                notes.Add(note);
            }
            Notes = notes.ToArray();

            Console.WriteLine("Read UST successfully");
            IsLoaded = true;
            Console.WriteLine(String.Join("\r\n", GetText()));
        }

        public static void ValidateLyrics()
        {
            foreach (UNote note in Notes) note.Lyric = note.Lyric;
        }

        public static bool IsTempUst(string Dir)
        {
            string filename = Dir.Split('\\').Last();
            return filename.StartsWith("tmp") && filename.EndsWith("tmp");
        }

        public static string[] GetText()
        {
            List<string> text = new List<string> { };
            if (Version == 1.2)
            {
                text.Add(Number.Version);
                text.Add("UST Version " + Version.ToString());
                text.Add(Number.Setting);
            }
            else
            {
                text.Add(Number.Setting);
                text.Add(TakeIn("Version", Version));
            }
            text.Add(TakeIn("Tempo", Tempo));
            text.Add(TakeIn("VoiceDir", VoiceDir));
            foreach (UNote note in Notes) text.AddRange(note.GetText());
            return text.ToArray();
        }

        public static void SetLyric(string[] newlyric, bool skipRest = true)
        {
            List<string> notelyrics = new List<string>();
            int k = 0;
            bool notEnoughWords = false;
            for (int i = 0; i < newlyric.Length; i++)
            {
                while (Notes[k].Number == Number.Delete)
                    k++;
                if (k >= Notes.Length)
                    break;
                while (Atlas.IsRest(Ust.Notes[k].ParsedLyric) && skipRest)
                {
                    if (notelyrics.Count > 0)
                    {
                        Ust.Notes[k].ParsedLyric += " " + String.Join(" ", notelyrics);
                        notelyrics = new List<string>();
                    } else { }
                    k++;
                    if (k >= Notes.Length)
                    {
                        notEnoughWords = true;
                        break;
                    }
                    else { }
                }
                if (notEnoughWords) break;
                while (Notes[k].Number == Number.Delete)
                    k++;
                if (Atlas.GetAliasType(newlyric[i]) == "C")
                    notelyrics.Add(newlyric[i]);
                else
                {
                    if (k >= Notes.Length)
                        break;
                    if (notEnoughWords) break;
                    notelyrics.Add(newlyric[i]);
                    Ust.Notes[k].ParsedLyric = String.Join(" ", notelyrics);
                    notelyrics = new List<string>();
                    k++;
                }
            }
            if (notelyrics.Count > 0)
            {
                UNote last = Ust.Notes.Last();
                for (int i = 0; i < notelyrics.Count; i++)
                {
                    Ust.InsertNote(Ust.Notes.Last(), notelyrics[i], Insert.After, last);
                }
            }
        }

        public static UNote GetNextNote(UNote note)
        {
            List<UNote> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind + 1;
            if (newInd >= notes.Count) return null;
            return notes[newInd];
        }

        public static UNote GetPrevNote(UNote note)
        {
            List<UNote> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind - 1;
            if (newInd == -1) return null;
            return notes[newInd];
        }

        public static bool InsertNote(UNote parent, string lyric, Insert insert, UNote pitchparent, UNote container = null)
        {
            UNote prev = GetPrevNote(parent);
            UNote next = GetNextNote(parent);
            UNote note = new UNote()
            {
                ParsedLyric = lyric,
                Number = Number.Insert,
                NoteNum = pitchparent.NoteNum,
                Parent = parent
            };
            if (insert == Insert.After && next != null && next.IsRest())
                note.Parent = next;
            if (insert == Insert.Before && prev != null &&  prev.IsRest())
                note.Parent = prev;
            if (container != null) note.Parent = container;

            List<UNote> notes = Notes.ToList();
            int indParent = notes.IndexOf(parent);
            int ind = notes.IndexOf(parent) + 1 + (int)insert;
            notes.Insert(ind, note);
            Notes = notes.ToArray();

            if (!SetLength(note)) return false;
            return true;
        }

        public static bool SetLength(UNote note)
        {
            UNote container = note.Parent;
            UNote next = GetNextNote(note);

            int length = PluginWindow.VCLength;
            var nextmembers = next.ParsedLyric.Trim().Split(' ');
            int vowelI = nextmembers.ToList().FindIndex(n => Atlas.GetAliasType(n) != "C");
            string nextlyric = vowelI == -1 ? nextmembers[0] : nextmembers[vowelI];
            string alias_type = Atlas.GetAliasType(note.ParsedLyric);
            string containterAliasType = Atlas.GetAliasType(container.ParsedLyric);
            string nextAliasType = next == null ? "" : Atlas.GetAliasType(next.ParsedLyric);
            if (alias_type == "C" || alias_type == "VC")
            {
                Oto oto = null;
                if (alias_type == "C")
                {
                    if (nextAliasType.Contains("CV"))
                    {
                        string al = Atlas.GetAlias("-CV", Atlas.GetPhonemes(nextlyric));
                        oto = Singer.Current.FindOto(al, container.NoteNum);
                    }
                    else if (nextAliasType.Contains("C"))
                    {
                        if (containterAliasType == "" || containterAliasType == "R")
                        {
                            oto = Singer.Current.FindOto(nextlyric, container.NoteNum);
                        }
                        else
                        {
                            string al = Atlas.GetAlias("-C", Atlas.GetPhonemes(nextlyric));
                            oto = Singer.Current.FindOto(al, container.NoteNum);
                        }
                    }
                    else if (nextAliasType.Contains("V"))
                    {
                        string al = Atlas.GetAlias("-V", Atlas.GetPhonemes(nextlyric));
                        oto = Singer.Current.FindOto(al, container.NoteNum);
                    }
                }
                else if (alias_type == "VC")
                {
                    oto = Singer.Current.FindOto(nextlyric, next.NoteNum);
                }
                if (oto != null)
                    length = MusicMath.MillisecondToTick(oto.Attack, Tempo);
            }
            length = (int) (length / PluginWindow.Velocity);

            if (container.Length < length + 10)
            {
                if (PluginWindow.MakeShort)
                {
                    note.Length = container.Length / 2;
                    container.Length -= note.Length;
                }
                else return false;
            }
            else
            {
                note.Length = length;
                container.Length -= note.Length;
            }
            return true;
        }

        public static int[] GetLengths()
        {
            List<int> sizes = new List<int>();
            foreach (UNote note in Notes)
            {
                sizes.Add(note.Length);
            }
            return sizes.ToArray();
        }


        public static string[] GetLyrics(bool skipRest = true)
        {
            List<string> lyrics = new List<string>();
            foreach (UNote note in Notes)
            {
                if (skipRest && Atlas.IsRest(note.Lyric)) continue;
                if (note.Lyric == null) continue;
                lyrics.Add(note.Lyric);
            }
            return lyrics.ToArray();
        }
    }
}
