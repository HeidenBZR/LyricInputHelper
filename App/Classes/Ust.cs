using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace LyricInputHelper.Classes
{
    public enum Insert : int
    {
        Before = -1,
        After = 0
    }

    public partial class Ust
    {
        public static string VoiceDir;
        public static double Tempo;
        public static double Version;
        public static Note[] Notes { get; set; }

        public static bool IsLoaded = false;
        public static string Dir;
        
        public Ust(string dir)
        {
            Dir = dir;
            Read();
        }

        public static void Reload()
        {
            Read();
        }


        public static void ValidateLyrics()
        {
            foreach (Note note in Notes) note.Lyric = note.Lyric;
        }

        public static bool IsTempUst(string Dir)
        {
            string filename = Dir.Split('\\').Last();
            return filename.StartsWith("tmp") && filename.EndsWith("tmp");
        }

        static int? SkipNotes(int k, bool skipRest = true)
        {
            while (Notes[k].Number == Number.DELETE ||
                skipRest && Atlas.IsRest(Notes[k].ParsedLyric))
            {
                k++;
                if (k >= Notes.Length)
                    return null;
            }
            return k;
        }

        public static void SetLyric(List<Syllable> syllables, bool skipRest = true)
        {
            int? n = Notes[0].Number == Number.PREV ? 1 : 0; // нота
            int s = 0; // слог
            string cc = "";
            while (s < syllables.Count && n < Notes.Length)
            {
                n = SkipNotes(n.Value);
                if (!n.HasValue)
                    break;


                Notes[n.Value].ParsedLyric = syllables[s].ToString();
                Notes[n.Value].Syllable = syllables[s];
                n++;
                s++;
            }
            if (s < syllables.Count - 1)
                System.Windows.Forms.MessageBox.Show("Недостаточно нот для введенных слогов. Часть слогов была утеряна.", "Предупреждение");

            foreach (var note in Notes)
                note.ParsedLyric = Atlas.PhonemeReplace(note.ParsedLyric);
        }

        public static void SetLyric(List<Word> words, bool skipRest = true)
        {
            int? n = Notes[0].Number == Number.PREV ? 1 : 0; // нота
            int s = 0; // слог
            int w = 0; // слово
            string cc = "";
            while (w < words.Count && n < Notes.Length)
            {
                n = SkipNotes(n.Value);
                if (!n.HasValue)
                    break;

                /// Прелоги типа к, с, ф
                if (words[w].Phonemes.All(p => !Atlas.IsVowel(p)))
                {
                    var new_cc = string.Join(" ", words[w].Phonemes);
                    cc = cc.Length > 0? string.Join(" ", cc, new_cc) : new_cc;
                    w++;
                    continue;
                }

                if (cc.Length > 0)
                    Notes[n.Value].ParsedLyric = $"{cc} {words[w].Syllables[s]}";
                else
                    Notes[n.Value].ParsedLyric = words[w].Syllables[s].ToString();
                Notes[n.Value].Syllable = words[w].Syllables[s];
                if (s == 0)
                    Notes[n.Value].Word = words[w];
                n++;

                if (words[w].Phonemes.Any(p => Atlas.IsVowel(p)))
                    cc = "";
                s++;
                if (s == words[w].Syllables.Count)
                {
                    s = 0;
                    w++;
                }
            }
            if (w < words.Count - 1)
                System.Windows.Forms.MessageBox.Show("Недостаточно нот для введенного текста. Часть текста была утеряна.", "Предупреждение");

            foreach (var note in Notes)
                note.ParsedLyric = Atlas.PhonemeReplace(note.ParsedLyric);
        }

        public static Note GetNextNote(Note note)
        {
            List<Note> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind + 1;
            if (newInd >= notes.Count) return null;
            return notes[newInd];
        }

        public static Note GetPrevNote(Note note)
        {
            List<Note> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind - 1;
            if (newInd == -1) return null;
            return notes[newInd];
        }

        public static bool InsertNote(Note parent, string lyric, Insert insert, Note pitchparent, Note container = null)
        {
            Note prev = GetPrevNote(parent);
            Note next = GetNextNote(parent);
            Note note = new Note()
            {
                ParsedLyric = lyric,
                Number = Number.INSERT,
                NoteNum = pitchparent.NoteNum,
                Intensity = pitchparent.Intensity,
                Flags = pitchparent.Flags,
                Parent = parent
            };
            if (insert == Insert.After && next != null && next.IsRest())
                note.Parent = next;
            if (insert == Insert.Before && prev != null &&  prev.IsRest())
                note.Parent = prev;
            if (container != null) note.Parent = container;



            List<Note> notes = Notes.ToList();
            int indParent = notes.IndexOf(parent);
            int ind = notes.IndexOf(parent) + 1 + (int)insert;
            notes.Insert(ind, note);
            Notes = notes.ToArray();
            return true;
        }

        public static string[] GetLyrics(bool skipRest = true)
        {
            List<string> lyrics = new List<string>();
            foreach (Note note in Notes)
            {
                if (note.Number == Number.NEXT || note.Number == Number.PREV)
                    continue;
                if (skipRest && Atlas.IsRest(note.Lyric))
                    continue;
                if (note.Lyric == null) continue;
                lyrics.Add(note.ParsedLyricView);
            }
            return lyrics.ToArray();
        }

        public static int GetLength(Note note)
        {
            if (!PluginWindow.LengthByOto)
                return PluginWindow.MinLength;
            var next = GetNextNote(note);
            if (next is null)
                return PluginWindow.MinLength;
            if (Atlas.IsRest(next.ParsedLyric))
                return PluginWindow.MinLength;
            var oto = Singer.Current.FindOto(next);
            if (oto is null)
                return PluginWindow.MinLength;
            var length = MusicMath.MillisecondToTick(oto.Preutterance, Tempo) + 1;
            return length;
        }

        public static void GetEnvelope()
        {
            foreach (var note in Notes)
                if (!note.IsRest() && note.Number != Number.NEXT && note.Number != Number.PREV)
                    note.GetEnvelope();
        }

        public static void SetLength()
        {
            try
            {

                //int start = Notes[0].Number == Number.PREV ? 1 : 0;
                //int stop = Notes[Notes.Length - 1].Number == Number.NEXT ? 1 : 0;
                int start = 0;
                int stop = 0;
                List<Note> parents = new List<Note>();
                for (int i = 0; i < Notes.Length - stop; i++)
                {
                    var note = Notes[i];
                    if (note.Parent is null)
                        parents.Add(note);
                    else
                    {
                        note.FinalLength = GetLength(Notes[i]) + 1;
                        note.Parent.Children.Add(note);
                    }
                }
                foreach (var note in parents)
                {
                    bool isRest = Atlas.IsRest(note.ParsedLyric);
                    var minSize = isRest ? 0 : PluginWindow.MinLength;

                    if (note.Children.Count == 0)
                    {
                        note.FinalLength = note.Length;
                        continue;
                    }

                    int children_length = note.Children.Sum(n => n.FinalLength);
                    if (note.FinalLength - children_length >= minSize)
                        note.FinalLength -= children_length;
                    else
                    {
                        // initial velocity
                        double velocity = 1 / ((double)note.Length / (minSize + children_length)) * PluginWindow.CompressionRatio;
                        // 2 kind of velocity
                        var velocity_last = velocity;
                        var velocity_children = velocity;
                        var last_child = note.Children.Last();
                        var next = GetNextNote(last_child);
                        if (note.Children.Count > 1)
                        {
                            /// watch length_formula.png
                            children_length = note.Children.Take(note.Children.Count - 1).Sum(n => n.FinalLength);
                            var LCCR = PluginWindow.LastChildCompressionRatio;
                            var CR = PluginWindow.CompressionRatio;
                            velocity = (minSize + children_length * LCCR / CR + last_child.FinalLength / CR) / note.Length;
                            velocity_last = velocity * CR;
                            velocity_children = velocity_last / LCCR;
                            velocity = velocity > 1 ? velocity : 1;
                            velocity_children = velocity_children > 1 ? velocity_children : 1;
                            velocity_last = velocity_last > 1 ? velocity_last : 1;
                        }
                        for (int i = 0; i < note.Children.Count - 1; i++)
                        {
                            note.Children[i].FinalLength = (int)((double)note.Children[i].FinalLength / velocity_children);
                            note.Children[i].Velocity *= velocity_children + 0.1;
                            if (note.Children[i].Velocity < 1)
                                throw new Exception($"Че блять. Velocity {note.Children[i].Number}[{note.Children[i].ParsedLyric}]: " +
                                    $"{next.Velocity}. " +
                                    $"\nvelocity_children: {velocity_children}");
                        }
                        last_child.Velocity *= velocity_children + 0.1;
                        last_child.FinalLength = (int)((double)last_child.FinalLength / velocity_last);
                        if (last_child.Velocity < 1)
                            throw new Exception($"Че блять. Velocity {last_child.Number}[{last_child.ParsedLyric}]: " +
                                $"{next.Velocity}. " +
                                $"\nvelocity_children: {velocity_children}");
                        next.Velocity *= velocity_last + 0.1;
                        if (next.Velocity < 1)
                            throw new Exception($"Че блять. Velocity {next.Number}[{next.ParsedLyric}]: {next.Velocity}. " +
                                $"\nvelocity_last: {velocity_last}");
                        //if (next.Parent != null)
                        //    throw new Exception("Эм");
                        children_length = note.Children.Sum(n => n.FinalLength);
                        note.FinalLength -= children_length;
                        if (note.FinalLength < minSize / velocity)
                            throw new Exception($"Так бля. Это несмотря на всю магию вне хогвартса длина {note.Number}[{note.ParsedLyric}] меньше минимальной.");
                        if (note.Length != note.FinalLength + children_length)
                            throw new Exception($"Что за херня. Это несмотря на всю магию вне хогвартса длина ноты {note.Number}[{note.ParsedLyric}] не равна сумме ее итоговой длины и длин ее деток ДА ЭТО ПРОСТО НЕВОЗМОЖНО");
                    }
                    if (note.IsRest() && note.FinalLength < PluginWindow.MinLength)
                        note.MergeIntoLeft();
                }
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on SetLength");
            }
        }
    }
}
