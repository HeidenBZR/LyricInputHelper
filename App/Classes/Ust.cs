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
        public string VoiceDir;
        public double Tempo;
        public double Version;
        public Note[] Notes { get; set; }

        public bool IsLoaded = false;
        public string Dir;

        public Atlas Atlas;
        
        public Ust(string dir)
        {
            Dir = dir;
            Read();
        }

        public void Reload()
        {
            Read();
        }

        public void SetAtlas(Atlas atlas)
        {
            Atlas = atlas;
            foreach (var note in Notes)
            {
                note.Lyric = note.ValidateLyric(Atlas, note.Lyric);
                note.SetParsedLyric(Atlas, note.ParsedLyric);
            }
        }

        public void ValidateLyrics()
        {
        }

        public void SetLyric(List<Syllable> syllables, bool skipRest = true)
        {
            int? n = Notes[0].Number == NumberManager.PREV ? 1 : 0; // нота
            int s = 0; // слог
            string cc = "";
            while (s < syllables.Count && n < Notes.Length)
            {
                n = SkipNotes(n.Value);
                if (!n.HasValue)
                    break;


                Notes[n.Value].SetParsedLyric(Atlas, syllables[s].ToString());
                Notes[n.Value].Syllable = syllables[s];
                n++;
                s++;
            }
            if (s < syllables.Count - 1)
                System.Windows.Forms.MessageBox.Show("Недостаточно нот для введенных слогов. Часть слогов была утеряна.", "Предупреждение");

            foreach (var note in Notes)
                note.SetParsedLyric(Atlas, Atlas.PhonemeReplace(note.ParsedLyric));
        }

        public void SetLyric(List<Word> words, bool skipRest = true)
        {
            int? n = Notes[0].Number == NumberManager.PREV ? 1 : 0; // нота
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
                    Notes[n.Value].SetParsedLyric(Atlas, $"{cc} {words[w].Syllables[s]}");
                else
                    Notes[n.Value].SetParsedLyric(Atlas, words[w].Syllables[s].ToString());
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
                note.SetParsedLyric(Atlas, Atlas.PhonemeReplace(note.ParsedLyric));
        }

        public Note GetNextNote(Note note)
        {
            List<Note> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind + 1;
            if (newInd >= notes.Count) return null;
            return notes[newInd];
        }

        public Note GetPrevNote(Note note)
        {
            List<Note> notes = Notes.ToList();
            int ind = notes.IndexOf(note);
            if (ind == -1) throw new Exception();
            int newInd = ind - 1;
            if (newInd == -1) return null;
            return notes[newInd];
        }

        public bool InsertNote(Note parent, string lyric, Insert insert, Note pitchparent, Note container = null)
        {
            Note prev = GetPrevNote(parent);
            Note next = GetNextNote(parent);
            Note note = new Note()
            {
                Number = NumberManager.INSERT,
                NoteNum = pitchparent.NoteNum,
                Intensity = pitchparent.Intensity,
                Flags = pitchparent.Flags,
                Parent = parent
            };
            note.SetParsedLyric(Atlas, lyric);
            if (insert == Insert.After && next != null && next.IsRest)
                note.Parent = next;
            if (insert == Insert.Before && prev != null &&  prev.IsRest)
                note.Parent = prev;
            if (container != null) note.Parent = container;



            List<Note> notes = Notes.ToList();
            int indParent = notes.IndexOf(parent);
            int ind = notes.IndexOf(parent) + 1 + (int)insert;
            notes.Insert(ind, note);
            Notes = notes.ToArray();
            return true;
        }

        public string[] GetLyrics(bool skipRest = true)
        {
            List<string> lyrics = new List<string>();
            foreach (Note note in Notes)
            {
                if (note.Number == NumberManager.NEXT || note.Number == NumberManager.PREV)
                    continue;
                if (skipRest && Atlas.IsRest(note.Lyric))
                    continue;
                if (note.Lyric == null) continue;
                lyrics.Add(note.ParsedLyricView);
            }
            return lyrics.ToArray();
        }

        public int GetLength(Note note)
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

        public void GetEnvelope()
        {
            if (!IsLoaded || Singer.Current is null || !Atlas.IsLoaded)
                return;
            for (var index = 0; index < Notes.Length; index++)
            {
                var note = Notes[index];
                var next = Notes.ElementAtOrDefault(index + 1);
                if (!note.IsRest && note.Number != NumberManager.NEXT && note.Number != NumberManager.PREV &&
                    !Atlas.IsRest(note.ParsedLyric))
                {
                    var isNextRest = next != null && Atlas.IsRest(next.ParsedLyric);
                    note.GetEnvelope(next, Tempo, isNextRest);
                }
            }
        }

        public void SetLength()
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

                    if (note.IsRest && note.FinalLength < PluginWindow.MinLength)
                    {
                        var prev = GetPrevNote(note);
                        note.MergeIntoLeft(prev);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on SetLength");
            }
        }

        #region private

        private int? SkipNotes(int k, bool skipRest = true)
        {
            while (Notes[k].Number == NumberManager.DELETE ||
                   skipRest && Atlas.IsRest(Notes[k].ParsedLyric))
            {
                k++;
                if (k >= Notes.Length)
                    return null;
            }
            return k;
        }

        #endregion
    }
}
