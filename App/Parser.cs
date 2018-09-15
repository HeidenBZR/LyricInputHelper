using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using App.Classes;

namespace App
{
    class Parser
    {
        public static void Split()
        {
            for (int i = 1; i < Ust.Notes.Length; i++)
            {
                UNote note = Ust.Notes[i];
                UNote prev = i > 0 ? Ust.Notes[i - 1] : null;
                var members = note.ParsedLyric.Trim().Split(' ');
                int inserted = 0;
                if (i >= 34)
                    Console.WriteLine();
                if (members.Length > 1)
                {
                    int vowelI = members.ToList().FindIndex(n => Atlas.GetAliasType(n) != "C");
                    if (vowelI > 0)
                    {
                        for (int k = vowelI - 1; k >= 0; k--)
                        {
                            if (prev != null)
                                if (prev.IsRest())
                                    Ust.InsertNote(prev, members[k], Insert.After, note);
                                else
                                    Ust.InsertNote(prev, members[k], Insert.After, prev);
                            else { }
                            inserted++;
                        }
                    }
                    if (vowelI < members.Length - 1)
                    {
                        for (int k = members.Length - 1; k > vowelI; k--)
                        {
                            Ust.InsertNote(note, members[k], Insert.After, note);
                            inserted++;
                        }
                    }
                    i += inserted;
                    note.ParsedLyric = members[vowelI];
                }
            }
        }

        public static void AtlasConverting()
        {
            for  (int i = 1; i < Ust.Notes.Length; i++)
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

                if (rule.MustConvert)
                {
                    RuleResult result = rule.FormatConvert.GetResult(lyricPrev, lyric);
                    Ust.Notes[i].ParsedLyric = result.Alias;
                }
                else Ust.Notes[i].ParsedLyric = lyric;
                Console.WriteLine(Ust.Notes[i].ParsedLyric);
                if (rule.MustInsert)
                {
                    RuleResult result = rule.FormatInsert.GetResult(lyricPrev, lyric);
                    Insert insert = result.AliasType == "V-" ? Insert.Before : Insert.After;
                    UNote pitchparent = Ust.Notes[i - 1];
                    UNote parent = result.AliasType == "VC" ? Ust.Notes[i - 1] : Ust.Notes[i];
                    if (PluginWindow.MakeVR || result.AliasType != "V-")
                    {
                        if (Ust.InsertNote(parent, result.Alias, insert, pitchparent))
                        {
                            Console.WriteLine(Ust.Notes[i].ParsedLyric);
                        }
                    }
                }
            }
        }

        public static void ToCV()
        {
            Ust.Reload();
            string lyric;
            for (int i = 0; i < Ust.Notes.Length; i++)
            {
                UNote note = Ust.Notes[i];
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
                            note.Number = Number.Delete;
                        }
                        break;
                    case "V-":
                        note.ParsedLyric = "R";
                        UNote container = Ust.GetNextNote(note);
                        if (container != null || container.IsRest())
                        {
                            container.Length += note.Length;
                            note.ParsedLyric = Number.Delete;
                            note.Number = Number.Delete;
                        }
                        break;
                    case "-C":
                        lyric = Atlas.GetAlias("C", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "-CV":
                        note.ParsedLyric = Atlas.GetAlias("CV", Atlas.GetPhonemes(note.ParsedLyric));
                        break;
                    case "VC-":
                        lyric = Atlas.GetAlias("C", new[] { Atlas.GetPhonemes(note.ParsedLyric).ToList().Last() });
                        if (i > 0)
                        {
                            Ust.Notes[i - 1].ParsedLyric += $" {lyric}";
                            Ust.Notes[i - 1].Length += note.Length;
                            note.ParsedLyric = Number.Delete;
                            note.Number = Number.Delete;
                        }
                        break;
                    case "VC":
                        if (i > 0)
                        {
                            Ust.Notes[i - 1].Length += note.Length;
                            note.ParsedLyric = Number.Delete; ;
                            note.Number = Number.Delete;
                        }
                        break;
                }

            }

            for (int i = 0; i < Ust.Notes.Length; i++)
            {
                // remove C
                if (Atlas.GetAliasType(Ust.Notes[i].ParsedLyric) == "C")
                {
                    if (i > 0 && !Ust.Notes[i - 1].IsRest())
                    {
                        Ust.Notes[i + 1].Length += Ust.Notes[i].Length;
                        Ust.Notes[i + 1].ParsedLyric = $"{Ust.Notes[i].ParsedLyric} {Ust.Notes[i + 1].ParsedLyric}";
                        Ust.Notes[i].ParsedLyric = Number.Delete;
                        Ust.Notes[i].Number = Number.Delete;
                    }
                    else
                    {
                        Ust.Notes[i - 1].Length += Ust.Notes[i].Length;
                        Ust.Notes[i - 1].ParsedLyric += $" {Ust.Notes[i].ParsedLyric}";
                        Ust.Notes[i].ParsedLyric = Number.Delete;
                        Ust.Notes[i].Number = Number.Delete;
                    }
                }
            }
        }

        public static void ToCVC()
        {
            Ust.Reload();
            for (int i = 0; i < Ust.Notes.Length; i++)
            {
                UNote note = Ust.Notes[i];
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
                            note.ParsedLyric = Number.Delete;
                            note.Number = Number.Delete;
                        }
                        break;
                    case "V-":
                        note.ParsedLyric = "R";
                        UNote container = Ust.GetNextNote(note);
                        if (container != null || container.IsRest())
                        {
                            container.Length += note.Length;
                            note.ParsedLyric = Number.Delete;
                            note.Number = Number.Delete;
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
                            note.ParsedLyric = Number.Delete;
                            note.Number = Number.Delete;
                        }
                        break;
                }

            }

        }
    }
}
