using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    partial class Atlas
    {

        static void Load()
        {
            if (File.Exists(Path.Combine(Current._dir, "character.txt")))
            {
                string[] character = File.ReadAllLines(Path.Combine(Current._dir, "character.txt"), Encoding.GetEncoding(932));
                if (character.ToList().Exists(n => n.StartsWith("type")))
                {
                    VoicebankType = character.ToList().Find(n => n.StartsWith("type="));
                    VoicebankType = VoicebankType.Substring("type=".Length);
                    if (!File.Exists(AtlasPath))
                        SetDefault();
                }
                else if (character.ToList().Exists(n => n.StartsWith("VoicebankType")))
                {
                    VoicebankType = character.ToList().Find(n => n.StartsWith("VoicebankType="));
                    VoicebankType = VoicebankType.Substring("VoicebankType=".Length);
                    if (!File.Exists(AtlasPath))
                        SetDefault();
                    else
                        IsDefault = false;
                }
                else SetDefault();
            }
            else SetDefault();
            ReadAtlas();
        }

        static void SetDefault()
        {
            IsDefault = true;
            VoicebankType = DefaultVoicebankType;
        }

        static void ReadVersion(string line) { Version = line.Substring("Version".Length + 1); }
        static void ReadVowels(string line) { Vowels = line.Substring("Vowels".Length + 1).Trim(',').Split(','); }
        static void ReadConsonants(string line) { Consonants = line.Substring("Consonants".Length + 1).Trim(',').Split(','); }
        static void ReadRest(string line) { Rests = line.Substring("Rests".Length + 1).Trim(',').Split(','); }

        static void ReadFormat(string line)
        {
            string[] format = line.Split('=');
            if (AliasTypes.Contains(format[0])) return;
            Format[format[0]] = format[1];
            AliasTypes.Add(format[0]);
        }

        static void ReadRule(string line)
        {
            Rule.Read(line);
        }

        static void ReadAliasReplace(string line)
        {
            try
            {
                if (line is null || line.Length == 0)
                    return;
                string[] replace = line.Split('=');
                if (replace.Length != 2) return;
                replace[0] = replace[0].Trim();
                replace[1] = replace[1].Trim();
                replace[0] = replace[0].Replace("%V%", VowelPattern);
                replace[0] = replace[0].Replace("%C%", ConsonantPattern);
                replace[0] = replace[0].Replace("%R%", RestPattern);
                AliasReplaces.Add(replace);
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, $"Error on reading Alias Replace: {line}");
            }
        }


        static void ReadPhonemeReplace(string line)
        {
            try
            {
                if (line is null || line.Length == 0)
                    return;
                string[] replace = line.Split('=');
                if (replace.Length != 2) return;
                replace[0] = replace[0].Trim();
                replace[1] = replace[1].Trim();
                replace[0] = replace[0].Replace("%V%", VowelPattern);
                replace[0] = replace[0].Replace("%C%", ConsonantPattern);
                replace[0] = replace[0].Replace("%R%", RestPattern);
                PhonemeReplaces.Add(replace);
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, $"Error on reading Phoneme Replace: {line}");
            }
        }

        static void ReadSplit(string line)
        {
            if (line.StartsWith("KeepWordsEndings="))
                KeepWordsEndings = line.Substring("KeepWordsEndings=".Length).ToLower() == "true";
            if (line.StartsWith("KeepWordsBeginnigs="))
                KeepWordsBeginnigs = line.Substring("KeepWordsBeginnigs=".Length).ToLower() == "true";
            if (line.StartsWith("KeepCC="))
                KeepCC = line.Substring("KeepCC=".Length).ToLower() == "true";
            if (line.StartsWith("KeepCV="))
                KeepCV = line.Substring("KeepCV=".Length).ToLower() == "true";
        }

        static void ReadAtlas()
        {
            string[] atlas = File.ReadAllLines(AtlasPath, Encoding.UTF8);
            int i = 0;
            if (atlas[0] != "[MAIN]") throw new Exception("Can't find main section"); ;
            i++;

            while (!atlas[i].StartsWith("["))
            {
                if (atlas[i].StartsWith("Version")) ReadVersion(atlas[i]);
                if (atlas[i].StartsWith("Vowels")) ReadVowels(atlas[i]);
                if (atlas[i].StartsWith("Consonants")) ReadConsonants(atlas[i]);
                if (atlas[i].StartsWith("Rests")) ReadRest(atlas[i]);
                if (atlas[i].StartsWith("ExampleWord")) ExampleWord = atlas[i].Substring("ExampleWord=".Length).Trim().ToLower();
                i++;
            }

            if (Rests == null) throw new Exception("Can't find Rests definition");
            if (Consonants == null) throw new Exception("Can't find Consonants definition");
            if (Vowels == null) throw new Exception("Can't find Vowels definition");

            BuildPatterns();


            if (Version != "0.3")
                throw new Exception("Current plugin version requests 0.3 atlas version.");
            // Read02();
            Read03(atlas, i);

        }

        static void BuildPatterns()
        {
            /// Phonemes must be ordered by string length, else there will be false 
            /// positive results which ends as wrong recognition

            var vowels_for_pattern = Vowels.OrderByDescending(n => n.Length).ToList();
            var consonants_for_pattern = Consonants.OrderByDescending(n => n.Length).ToList();
            var rests_for_pattern = Rests.OrderByDescending(n => n.Length).ToList();


            VowelPattern = $"{String.Join("|", vowels_for_pattern)}";
            ConsonantPattern = $"{String.Join("|", consonants_for_pattern)}";
            RestPattern = $"{String.Join("|", rests_for_pattern)}";
            foreach (var shit in ". ? ( ) [ ] * \\".Split(' '))
            {
                VowelPattern = VowelPattern.Replace(shit, "\\" + shit);
                ConsonantPattern = ConsonantPattern.Replace(shit, "\\" + shit);
                RestPattern = RestPattern.Replace(shit, "\\" + shit);
            }
            VowelPattern = $"({VowelPattern})";
            ConsonantPattern = $"({ConsonantPattern})";
            RestPattern = $"({RestPattern})";
        }

        static void Read02(string[] atlas, int i)
        {
            AliasTypes = new List<string>();
            Format = new Dictionary<string, string>();
            i++;
            while (atlas[i] != "[RULES]")
            {
                ReadFormat(atlas[i]);
                i++;
            }
            i++;
            while (atlas[i] != "[REPLACE]")
            {
                ReadRule(atlas[i]);
                i++;
            }
            i++;
            AliasReplaces = new List<string[]>();
            while (atlas[i] != "[PHONEME_REPLACE]")
            {
                ReadAliasReplace(atlas[i]);
                i++;
            }
            PhonemeReplaces = new List<string[]>();
            while (i < atlas.Length)
            {
                ReadPhonemeReplace(atlas[i]);
                i++;
            }
            IsLoaded = true;
        }
        static void Read03(string[] atlas, int i)
        {
            AliasTypes = new List<string>();
            Format = new Dictionary<string, string>();
            i++;
            ReadFormat("C=%C%");
            ReadFormat("V=%V%");
            while (atlas[i] != "[SPLIT]")
            {
                ReadFormat(atlas[i]);
                i++;
            }
            i++;
            while (atlas[i] != "[CONVERT]")
            {
                ReadSplit(atlas[i]);
                i++;
            }
            i++;
            while (atlas[i] != "[PHONEME_REPLACE]")
            {
                ReadRule(atlas[i]);
                i++;
            }
            i++;
            PhonemeReplaces = new List<string[]>();
            while (atlas[i] != "[ALIAS_REPLACE]")
            {
                ReadPhonemeReplace(atlas[i]);
                i++;
            }
            AliasReplaces = new List<string[]>();
            while (i < atlas.Length)
            {
                ReadAliasReplace(atlas[i]);
                i++;
            }
            IsLoaded = true;
        }
    }
}
