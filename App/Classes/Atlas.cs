using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace App.Classes
{

    class Atlas
    {
        #region Variables
        public static string Version;
        public static string[] Vowels;
        public static string[] Consonants;
        public static string[] Rests;
        public static List<string> AliasTypes;
        public static Dictionary<string, string> Format;
        public static List<string[]> Replaces;
        public static List<string[]> PhonemeReplaces;
        public static Dictionary<string, string[]> Dict;

        public static bool IsLoaded = false;
        public static bool HasDict = true;
        public static bool IsDefault = false;
        public static string DefaultVoicebankType { get { return "CVC RUS"; } }
        public static string VoicebankType;
        public static string DictPath
        {   get
            {
                if (!HasDict) return null;
                else if (IsDefault) return Path.Combine(@"atlas", DefaultVoicebankType + ".dict");
                else return Path.Combine(@"atlas", VoicebankType + ".dict");
            }
        }
        public static string AtlasPath
        {   get
            {
                if (IsDefault) return Path.Combine(@"atlas", DefaultVoicebankType + ".atlas");
                else return Path.Combine(@"atlas", VoicebankType + ".atlas");
            }
        }

        private static string VowelPattern;
        private static string ConsonantPattern;
        private static string RestPattern;

        public static Atlas Current;

        string _dir;

        #endregion

        public Atlas(string dir)
        {
            _dir = dir;
            Current = this;
            Load();
            Ust.ValidateLyrics();
        }

        public static void Reload()
        {
            Load();
        }

        static void Load()
        {
            if (File.Exists(Path.Combine(Current._dir, "character.txt")))
            {
                string[] character = File.ReadAllLines(Path.Combine(Current._dir, "character.txt"));
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
            if (!File.Exists(DictPath))
                HasDict = false;
            if (HasDict) Current.ReadDict();
        }

        static void SetDefault()
        {
            IsDefault = true;
            VoicebankType = DefaultVoicebankType;
        }

        #region Reading

        static void ReadVersion(string line) { Version = line.Substring("Version".Length + 1); }
        static void ReadVowels(string line) { Vowels = line.Substring("Vowels".Length + 1).Split(','); }
        static void ReadConsonants(string line) { Consonants = line.Substring("Consonants".Length + 1).Split(','); }
        static void ReadRest(string line) { Rests = line.Substring("Rests".Length + 1).Split(','); }
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
        static void ReadReplace(string line)
        {
            string[] replace = line.Split('=');
            if (replace.Length != 2) return;
            Replaces.Add(replace);
        }
        static void ReadPhonemeReplace(string line)
        {
            string[] replace = line.Split('=');
            if (replace.Length != 2) return;
            replace[0] = replace[0].Trim();
            replace[1] = replace[1].Trim();
            PhonemeReplaces.Add(replace);
        }
        static void ReadDict(string line)
        {
            string[] dict = line.Split('=');
            if (dict.Length != 2) return;
            Dict[dict[0]] = dict[1].Split(' ');
        }
        static void ReadAtlas()
        {
            string[] atlas = File.ReadAllLines(AtlasPath);
            int i = 0;
            if (atlas[0] != "[MAIN]") throw new Exception("Can't find main section"); ;
            i++;

            while (atlas[i] != "[FORMAT]")
            {
                if (atlas[i].StartsWith("Version")) ReadVersion(atlas[i]);
                if (atlas[i].StartsWith("Vowels")) ReadVowels(atlas[i]);
                if (atlas[i].StartsWith("Consonants")) ReadConsonants(atlas[i]);
                if (atlas[i].StartsWith("Rests")) ReadRest(atlas[i]);
                i++;
            }

            if (Rests == null) throw new Exception("Can't find Rests definition");
            if (Consonants == null) throw new Exception("Can't find Consonants definition");
            if (Vowels == null) throw new Exception("Can't find Vowels definition");

            VowelPattern = $"({String.Join("|", Vowels)})";
            ConsonantPattern = $"({String.Join("|", Consonants)})";
            RestPattern = $"({String.Join("|", Rests)})";

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
            Replaces = new List<string[]>();
            while (atlas[i] != "[PHONEME_REPLACE]")
            {
                ReadReplace(atlas[i]);
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

        void ReadDict()
        {
            Dict = new Dictionary<string, string[]>();
            string[] dicts = File.ReadAllLines(DictPath);
            foreach (string line in dicts) ReadDict(line);
        }

        public static void ReloadDict()
        {
            Dict = new Dictionary<string, string[]>();
            string[] dicts = File.ReadAllLines(DictPath);
            foreach (string line in dicts) ReadDict(line);
        }

        #endregion

        public static bool IsRest(string phoneme) { return phoneme.Trim(' ') == "" || Rests.Contains(phoneme) || phoneme == "R"; }
        public static bool IsConsonant(string phoneme) { return Consonants.Contains(phoneme); }
        public static bool IsVowel(string phoneme) { return Vowels.Contains(phoneme); }

        public static string GetAliasType(string alias)
        {
            if (alias == Number.DELETE) return "";
            if (IsRest(alias)) return "R";
            if (IsVowel(alias)) return "V";
            if (IsConsonant(alias)) return "C";
            foreach (string alias_type in AliasTypes)
            {
                string pattern = Format[alias_type].Replace("%V%", VowelPattern);
                pattern = pattern.Replace("%C%", ConsonantPattern);
                pattern = pattern.Replace("%R%", RestPattern);
                if (Regex.IsMatch(alias, pattern))
                {
                    var value = Regex.Match(alias, pattern).Value;
                    if (value == alias)
                    {
                        string attempt = Regex.Match(alias, pattern).Value;
                        return alias_type;
                    }
                }
            }
            return "";
        }

        public static string[] GetPhonemes(string alias)
        {
            if (IsRest(alias)) return new string[] {};
            if (IsVowel(alias) || IsConsonant(alias)) return new string[] { alias };
            foreach (string alias_type in AliasTypes)
            {

                string pattern = Format[alias_type].Replace("%V%", VowelPattern);
                pattern = pattern.Replace("%C%", ConsonantPattern);
                pattern = pattern.Replace("%R%", RestPattern);
                if (Regex.IsMatch(alias, pattern))
                {
                    string attempt = Regex.Match(alias, pattern).Value;
                    if (attempt == alias)
                    {
                        List<string> st = Regex.Split(alias, pattern).ToList();
                        st.Remove(st.First());
                        st.Remove(st.Last());
                        return st.ToArray();
                    }
                }
            }
            throw new Exception($"Can't extract phonemes from [{alias}]");
        }

        public static string[] GetPhonemes(string alias, string alias_type)
        {
            if (IsRest(alias)) return new string[] { };
            string pattern = Format[alias_type];
            pattern = pattern.Replace("%V%", VowelPattern);
            pattern = pattern.Replace("%C%", ConsonantPattern);
            pattern = pattern.Replace("%R%", RestPattern);
            if (Regex.IsMatch(alias, pattern))
            {
                string attempt = Regex.Match(alias, pattern).Value;
                if (attempt == alias)
                {
                    List<string> st = Regex.Split(alias, pattern).ToList();
                    st.Remove(st.First());
                    st.Remove(st.Last());
                    return st.ToArray();
                }
            }
            throw new Exception($"Can't extract phonemes from {alias}");
        }

        public static bool MatchPhonemeType(string PhonemeType, string phoneme)
        {
            switch (PhonemeType)
            {
                case "%C%":
                    if (Consonants.Contains(phoneme)) return true;
                    else return false;
                case "%V%":
                    if (Vowels.Contains(phoneme)) return true;
                    else return false;
                default:
                    throw new Exception($"Unknown phoneme: {phoneme}");
            }
        }

        public static string GetAlias(string alias_type, string[] phonemes)
        {
            if (alias_type == "R") return " ";
            string ph = "%.%";
            if (!Format.ContainsKey(alias_type))
                throw new Exception($"Cannot found alias type format for alias type: {alias_type}");
            string format = Format[alias_type];
            int i = 0;
            while (Regex.IsMatch(format, ph))
            {
                if (i >= phonemes.Length) throw new Exception($"Not enough phonemes to format alias {alias_type}");
                string ph_type = Regex.Match(format, ph).Value;
                if (MatchPhonemeType(ph_type, phonemes[i]))
                {
                    var f = new Regex(ph_type);
                    format = f.Replace(format, phonemes[i], 1);
                    i++;
                }
                else throw new Exception($"Wrong phonemes to format alias {alias_type}");
            }

            return Replace(format);
        }

        static string Replace(string line)
        {
            foreach (string[] pair in Replaces)
            {
                if (line.Contains(pair[0])) line = line.Replace(pair[0], pair[1]);
            }
            return line;
        }

        public static string PhonemeReplace(string phonemes)
        {
            foreach (string[] pair in PhonemeReplaces)
            {
                var search = pair[0];
                var replacement = pair[1];
                if (phonemes.Contains(search))
                    phonemes = phonemes.Replace(search, replacement);
            }
            return phonemes;
        }

        public static string[] DictAnalysis(string lyric)
        {
            List<string> aliases = new List<string>();
            int k;
            string l = "";
            List<string> syll = new List<string>();
            for (int i = 0; i < lyric.Length; )
            {
                for (k = lyric.Length - i; k > 0; k--)
                {
                    l = lyric.Substring(i, k);
                    if (Dict.ContainsKey(l))
                        break;
                    var last = l.Last().ToString();
                    if (Dict.ContainsKey(last) && Dict[last][0] == "#SKIP")
                        k--;
                }
                if (k == 0)
                {
                    return new[] { lyric };
                }
                else
                {
                    aliases.AddRange(Dict[l]);
                    i += k;
                }
            }
            int vs = aliases.Select(n => GetAliasType(n) != "C").ToArray().Length;
            if (vs == 1)
                return new[] { String.Join(" ", Dict[l]) };
            var sylls = new List<string>();
            int lastv = aliases.Select(n => GetAliasType(n) != "C").ToList().FindLastIndex(n => n);
            int prevv = -1;
            for (int ph = 0; ph <= lastv; ph++)
            {
                string alias_type = GetAliasType(aliases[ph]);
                while (alias_type == "C" && ph < lastv)
                {
                    ph++;
                    alias_type = GetAliasType(aliases[ph]);
                }
                if (ph == lastv)
                {
                    sylls.Add(String.Join(" ", aliases.ToList().GetRange(prevv + 1, aliases.Count - 1 - prevv)));
                    prevv = ph;
                }
                else
                {
                    sylls.Add(String.Join(" ", aliases.ToList().GetRange(prevv + 1, ph - prevv)));
                    prevv = ph;
                }
            }
            string t = String.Join(" ", sylls);
            return sylls.ToArray();
        }

        public static int FindVowel(string[] aliases)
        {
            for (int i = 0; i < aliases.Length; i++)
            {
                if (IsVowel(aliases[i])) return i;
            }
            return 0;
        }

        public static int VowelsCount(string[] aliases)
        {
            return aliases.Count(n => new[] { "CV", "V", "-CV", "`V", "-V"}.Contains(GetAliasType(n)));
        }
    }
}
