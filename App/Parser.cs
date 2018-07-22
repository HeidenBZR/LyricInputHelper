using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using App.Classes;

namespace App
{
    class Parser
    {

        public static void AtlasConverting()
        {
            int i = 1;
            while (i < Ust.Notes.Length)
            {
                string lyric = Ust.Notes[i].Lyric;
                string lyricPrev = Ust.Notes[i - 1].Lyric;
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

                Classes.Rule rule = Classes.Rule.GetRule($"{aliasTypePrev},{aliasType}");
                if (rule == null)
                {
                    Ust.Notes[i].ParsedLyric = lyric;
                    i++;
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
                    UNote parent = result.AliasType == "V-" ? Ust.Notes[i] : Ust.Notes[i - 1];
                    if (PluginWindow.makeVR || result.AliasType != "V-")
                    {
                        if (Ust.InsertNote(parent, result.Alias, insert))
                        {
                            Console.WriteLine(Ust.Notes[i].ParsedLyric);
                            i++;
                        }
                    }
                }
                i++;
            }
        }
    }
}
