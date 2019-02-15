using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LyricInputHelper.Classes;

namespace LyricInputHelper
{
    class Parser
    {
        public static void Split()
        {
            if (!Atlas.KeepCC && !Atlas.KeepWordsBeginnigs && !Atlas.KeepWordsEndings)
            {
                SplitFull();
                return;
            }
            var note = Ust.Notes[1];
            var next = Ust.GetNextNote(note);
            var prev = Ust.GetPrevNote(note);
            while (next != null)
            {
                if (!note.IsRest())
                {
                    if (note.Syllable is null)
                        note.Syllable = new Syllable(note.ParsedLyric.Split(' '));
                    var syll = note.Syllable;
                    if (!syll.ContainsVowel)
                    {
                        if (Atlas.KeepCC)
                        {
                            prev = note;
                            note = next;
                            next = Ust.GetNextNote(note);
                            continue;
                        }
                        // split CC
                        note.ParsedLyric = syll.Phonemes.Last();
                        for (int k = 0; k < syll.Phonemes.Length - 1; k++)
                            Ust.InsertNote(note, syll.Phonemes[k], Insert.Before, note);
                    }
                    else
                    {
                        // Words beginnings
                        if (note.Word.Name != null && !Atlas.KeepWordsBeginnigs)
                        {
                            if (syll.ExtraConsonantBeginning.Length > 0)
                            {
                                if (Atlas.KeepCC)
                                {
                                    Ust.InsertNote(note, Syllable.ToString(syll.ExtraConsonantBeginning), Insert.Before, prev, prev);
                                    var ccc = Ust.GetPrevNote(note);
                                    ccc.Syllable = new Syllable(ccc.ParsedLyric.Split(' '));
                                    if (ccc.Syllable.Phonemes.Length == 2)
                                        ccc.ParsedLyric = Atlas.GetAlias("CC", ccc.Syllable.Phonemes);
                                    if (ccc.Syllable.Phonemes.Length == 3)
                                        ccc.ParsedLyric = Atlas.GetAlias("CCC", ccc.Syllable.Phonemes);
                                    if (ccc.Syllable.Phonemes.Length == 4)
                                        ccc.ParsedLyric = Atlas.GetAlias("CCCC", ccc.Syllable.Phonemes);
                                }
                                else
                                    for (int k = 0; k < syll.ExtraConsonantBeginning.Length - 1; k++)
                                        Ust.InsertNote(note, syll.ExtraConsonantBeginning[k], Insert.Before, prev, prev);
                            }

                            if (!Atlas.KeepCV && syll.VowelIndex > 0)
                                Ust.InsertNote(note, syll.CV[0], Insert.Before, prev);
                        }
                        // Word endings
                        if (syll.ConsonantEnding.Length > 0)
                        {
                            if (Atlas.KeepCC)
                            {
                                Ust.InsertNote(note, Syllable.ToString(syll.ConsonantEnding), Insert.After, note);
                                var ccc = Ust.GetNextNote(note);
                                ccc.Syllable = new Syllable(ccc.ParsedLyric.Split(' '));
                                if (ccc.Syllable.Phonemes.Length == 2)
                                    ccc.ParsedLyric = Atlas.GetAlias("CC", ccc.Syllable.Phonemes);
                                if (ccc.Syllable.Phonemes.Length == 3)
                                    ccc.ParsedLyric = Atlas.GetAlias("CCC", ccc.Syllable.Phonemes);
                                if (ccc.Syllable.Phonemes.Length == 4)
                                    ccc.ParsedLyric = Atlas.GetAlias("CCCC", ccc.Syllable.Phonemes);
                            }
                            else
                                for (int k = syll.ConsonantEnding.Length; k > 0; k--)
                                    Ust.InsertNote(note, syll.ConsonantEnding[k - 1], Insert.Before, note);
                        }
                    }
                    if (Atlas.KeepCV)
                        note.Syllable = new Syllable(syll.Beginning);
                    else
                        note.Syllable = new Syllable(new List<string> { syll.Vowel });
                    note.ParsedLyric = note.Syllable.ToString();
                    if (note.Syllable.Phonemes.Length > 1)
                    {
                        if (note.Syllable.Beginning.Length == 2)
                            note.ParsedLyric = Atlas.GetAlias("CV", note.Syllable.Beginning);
                        if (note.Syllable.Beginning.Length == 3)
                            note.ParsedLyric = Atlas.GetAlias("CCV", note.Syllable.Beginning);
                        if (note.Syllable.Beginning.Length == 4)
                            note.ParsedLyric = Atlas.GetAlias("CCCV", note.Syllable.Beginning);
                        if (note.Syllable.Beginning.Length == 5)
                            note.ParsedLyric = Atlas.GetAlias("CCCCV", note.Syllable.Beginning);

                    }

                }

                prev = note;
                note = next;
                next = Ust.GetNextNote(note);
            }
        }

        public static void SplitFull()
        {
            // int i = Ust.Notes.First().Number == Number.PREV ? 2 : 1; // always must be 1
            //int stop = Ust.Notes.Last().Number == Number.NEXT ? 1 : 0; // always must be 1
            int i = 1;
            int stop = 1;
            for (; i < Ust.Notes.Length - stop; i++)
            {
                Note note = Ust.Notes[i];
                Note prev = i > 0 ? Ust.Notes[i - 1] : null;
                Note next = i + 1 < Ust.Notes.Length ? Ust.Notes[i + 1] : null;
                var members = note.ParsedLyric.Trim().Split(' ');
                int inserted = 0;
                if (members.Length > 1)
                {
                    int vowelI = members.ToList().FindIndex(n => Atlas.GetAliasType(n) != "C");
                    if (vowelI > 0 && Atlas.KeepCV)
                        note.Syllable = new Syllable(new List<string> { members[vowelI - 1], members[vowelI] });
                    else 
                        note.Syllable = new Syllable(new List<string> { members[vowelI] });
                    note.ParsedLyric = note.Syllable.ToString();
                    if (note.Syllable.Beginning.Length == 2)
                        note.ParsedLyric = Atlas.GetAlias("CV", note.Syllable.Beginning);
                    if (vowelI > 0)
                    {
                        for (int k = vowelI - 1; k >= 0; k--)
                        {
                            if (Atlas.KeepCV && k == vowelI - 1)
                                continue;
                            if (prev != null)
                                if (prev.IsRest())
                                    Ust.InsertNote(prev, members[k], Insert.After, note);
                                else
                                    Ust.InsertNote(prev, members[k], Insert.After, prev);
                            else { }
                            inserted++;
                        }
                    }
                    i += inserted;
                    if (vowelI < members.Length - 1)
                    {
                        if (Ust.Notes[i + 1].IsRest())
                        {
                            for (int k = members.Length - 1; k > vowelI; k--)
                            {
                                Ust.InsertNote(note, members[k], Insert.After, note, next);
                                i++;
                            }
                        }
                        else
                        {
                            for (int k = members.Length - 1; k > vowelI; k--)
                            {
                                var result = Ust.InsertNote(note, members[k], Insert.After, note);
                                i++;
                            }
                        }
                    }
                }
            }
        }

        public static void AtlasConverting()
        {

            //int i = Ust.Notes.First().Number == Number.PREV ? 1 : 0;
            int i = 1; /// Всегда нужна первая нота, но будут нюансы со вставкой
            //int stop = Ust.Notes.Last().Number == Number.NEXT ? 1 : 0;
            int stop = 0;

            for  (; i < Ust.Notes.Length - stop; i++)
            {
                string lyric = Ust.Notes[i].ParsedLyric;
                string lyricPrev = Ust.Notes[i - 1].ParsedLyric;
                string aliasType = "";
                string aliasTypePrev = "";
                bool tookAliases = false;
                try
                {
                    aliasType = Atlas.GetAliasType(lyric);
                    aliasTypePrev = Atlas.GetAliasType(lyricPrev);
                    tookAliases = true;
                }
                catch (KeyNotFoundException ex) { Program.Log(ex.Message); }
                if (!tookAliases) { i++; continue; }

                if (lyricPrev == "b'e" && lyric == "po")
                    Console.WriteLine();

                Classes.Rule rule = Classes.Rule.GetRule($"{aliasTypePrev},{aliasType}");
                if (rule == null)
                {
                    Ust.Notes[i].ParsedLyric = lyric;
                    continue;
                }

                if (rule.MustConvert && Ust.Notes[i].Number != Number.NEXT)
                {
                    RuleResult result = rule.FormatConvert.GetResult(lyricPrev, lyric);
                    Ust.Notes[i].ParsedLyric = result.Alias;
                    if (Ust.Notes[i].Parent != null)
                    {
                        Oto oto = Singer.Current.FindOto(Ust.Notes[i].ParsedLyric);
                        if (oto != null)
                        {
                            int ilength = MusicMath.MillisecondToTick(oto.InnerLength, Ust.Tempo);
                            Ust.Notes[i].Length += ilength;
                            Ust.Notes[i].Parent.Length -= ilength;
                        }
                    }
                }
                else Ust.Notes[i].ParsedLyric = lyric;
                Console.WriteLine(Ust.Notes[i].ParsedLyric);
                if (rule.MustInsert)
                {
                    var next = Ust.GetNextNote(Ust.Notes[i]);
                    bool isRest = Atlas.IsRest(lyric);
                    bool prevIsRest = Atlas.IsRest(lyricPrev);
                    RuleResult result = rule.FormatInsert.GetResult(lyricPrev, lyric);
                    Note pitchparent = prevIsRest ? Ust.Notes[i] : Ust.Notes[i - 1];
                    Insert insert = isRest  ? Insert.Before : Insert.After ;
                    Note parent = isRest  ? Ust.Notes[i] : Ust.Notes[i - 1];
                    if (PluginWindow.MakeVR || result.AliasType != "V-")
                        if (Ust.InsertNote(parent, result.Alias, insert, pitchparent))
                            Console.WriteLine(Ust.Notes[i].ParsedLyric);
                    if (new[] { "-C" }.Contains(result.AliasType))
                        i++;
                }
            }
            foreach (var note in Ust.Notes)
            {
                note.AliasType = Atlas.GetAliasType(note.ParsedLyric);
                note.ParsedLyric = Atlas.AliasReplace(note.ParsedLyric);
            }
        }

        public static void ToCV()
        {
            ToCVC();

            bool c_left = true;
            while (c_left)
            {
                c_left = false;
                int i = Ust.Notes.First().Number == Number.PREV ? 1 : 0;
                int stop = Ust.Notes.Last().Number == Number.NEXT ? 1 : 0;
                for (; i < Ust.Notes.Length - stop; i++)
                {
                    // remove C
                    var members = Ust.Notes[i].ParsedLyric.Split(' ');
                    bool doit = members.All(n => Atlas.GetAliasType(n) == "C");
                    if (doit)
                    {
                        c_left = true;
                        if (i > 0 && Ust.Notes[i - 1].IsRest())
                        {
                            /// R [-C] -CV
                            if (Ust.Notes[i + 1].ParsedLyric == Number.DELETE)
                            {
                                /// R [-C] -C -CV
                                Ust.Notes[i + 1].ParsedLyric = Ust.Notes[i].ParsedLyric;
                                Ust.Notes[i + 1].Length = Ust.Notes[i].Length;
                            }
                            else
                            {
                                Ust.Notes[i + 1].ParsedLyric = $"{Ust.Notes[i].ParsedLyric} {Ust.Notes[i + 1].ParsedLyric}";
                                Ust.Notes[i - 1].Length += Ust.Notes[i].Length;
                            }
                            Ust.Notes[i].ParsedLyric = Number.DELETE;
                            Ust.Notes[i].Number = Number.DELETE;
                            Ust.Notes[i].Length = 0;
                        }
                        else if (Ust.Notes[i + 1].IsRest())
                        {
                            /// VC- [C] R
                            if (Ust.Notes[i - 1].ParsedLyric == Number.DELETE)
                            {
                                /// VC- C [C] R
                                Ust.Notes[i - 1].ParsedLyric = Ust.Notes[i].ParsedLyric;
                                Ust.Notes[i - 1].Length = Ust.Notes[i].Length;
                            }
                            else
                            {
                                Ust.Notes[i - 1].ParsedLyric += $" {Ust.Notes[i].ParsedLyric}";
                                Ust.Notes[i + 1].Length += Ust.Notes[i].Length;
                            }
                            Ust.Notes[i].ParsedLyric = Number.DELETE;
                            Ust.Notes[i].Number = Number.DELETE;
                            Ust.Notes[i].Length = 0;
                        }
                        else if (i > 0 && Ust.Notes[i - 1].ParsedLyric.Split(' ').Any(n => Atlas.GetAliasType(n).Contains("V")))
                        {
                            /// CV [(VC-)] (-CV)
                            Ust.Notes[i - 1].Length += Ust.Notes[i].Length;
                            Ust.Notes[i + 1].ParsedLyric = $"{Ust.Notes[i].ParsedLyric} {Ust.Notes[i + 1].ParsedLyric}";
                            Ust.Notes[i].ParsedLyric = Number.DELETE;
                            Ust.Notes[i].Number = Number.DELETE;
                            Ust.Notes[i].Length = 0;
                        }
                        else
                        {
                            int k = 1;
                            while (Ust.Notes[i - k].Number == Number.DELETE) k++;
                            if (Ust.Notes[i - k].IsRest())
                            {
                                /// (R) (С)* [C] (!R)
                                Ust.Notes[i - k].Length += Ust.Notes[i].Length;
                                Ust.Notes[i + 1].ParsedLyric = $"{Ust.Notes[i].ParsedLyric} {Ust.Notes[i + 1].ParsedLyric}";
                                Ust.Notes[i].ParsedLyric = Number.DELETE;
                                Ust.Notes[i].Number = Number.DELETE;
                                Ust.Notes[i].Length = 0;
                            }
                            else
                            {
                                /// (CV) (VC-) (С)* [C] (!R)
                                Ust.Notes[i - k].Length += Ust.Notes[i].Length;
                                Ust.Notes[i - k].ParsedLyric = $"{Ust.Notes[i - k].ParsedLyric} {Ust.Notes[i].ParsedLyric}";
                                Ust.Notes[i].ParsedLyric = Number.DELETE;
                                Ust.Notes[i].Number = Number.DELETE;
                                Ust.Notes[i].Length = 0;
                            }
                        }
                    }
                }

            }
        }

        public static void ToCVC()
        {
            int i = Ust.Notes.First().Number == Number.PREV ? 1 : 0;
            int stop = Ust.Notes.Last().Number == Number.NEXT ? 1 : 0;
            for (; i < Ust.Notes.Length - stop; i++)
            {
                Note note = Ust.Notes[i];
                switch (Atlas.GetAliasType(note.ParsedLyric))
                {
                    case "-V":
                        note.ParsedLyric = Atlas.GetAlias("V", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "`V":
                        note.ParsedLyric = Atlas.GetAlias("V", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "V`":
                        if (i > 0)
                        {
                            Ust.Notes[i - 1].Length += note.Length;
                            note.ParsedLyric = Number.DELETE;
                            note.Number = Number.DELETE;
                        }
                        break;
                    case "V-":
                        note.ParsedLyric = "R";
                        Note container = Ust.GetNextNote(note);
                        if (container != null || container.IsRest())
                        {
                            container.Length += note.Length;
                            note.ParsedLyric = Number.DELETE;
                            note.Number = Number.DELETE;
                        }
                        break;
                    case "-C":
                        note.ParsedLyric = Atlas.GetAlias("C", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "-CV":
                        note.ParsedLyric = Atlas.GetAlias("CV", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "VC-":
                        note.ParsedLyric = Atlas.GetAlias("C", new[] { Atlas.GetPhonemes(note.ParsedLyric).ToList().Last() });
                        break;
                    case "VC":
                        if (i > 0)
                        {
                            Ust.Notes[i - 1].Length += note.Length;
                            note.ParsedLyric = Number.DELETE;
                            note.Number = Number.DELETE;
                        }
                        break;
                }

            }

        }
    }
}
