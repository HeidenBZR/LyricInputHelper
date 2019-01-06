using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace LyricInputHelper.Classes
{

    partial class Atlas
    {
        #region Variables
        public static string Version;
        public static string[] Vowels;
        public static string[] Consonants;
        public static string[] Rests;
        public static List<string> AliasTypes;
        public static Dictionary<string, string> Format;
        public static List<string[]> AliasReplaces;
        public static List<string[]> PhonemeReplaces;
        public static Dict Dict;

        public static bool KeepWordsEndings = false;
        public static bool KeepWordsBeginnigs = false;
        public static bool KeepCC = false;
        public static bool KeepCV = true;

        public static string Melisma = "&m;";


        public static string ExampleWord = "привет";

        public static bool IsLoaded = false;
        public static bool HasDict { get { return Dict.IsEnabled; } }
        public static bool IsDefault = false;
        public static string DefaultVoicebankType { get { return "CVC RUS"; } }
        public static string VoicebankType;
        public static string DictPath
        {
            get
            {
                return Program.GetResourceFile(Path.Combine(@"atlas", VoicebankType + ".dict"));
            }
        }
        public static string AtlasPath
        {
            get
            {
                return Program.GetResourceFile(Path.Combine(@"atlas", VoicebankType + ".atlas"));
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
                            return alias_type;
                }
            }
            return "";
            //throw new Exception("Это говно какое-то а не алиас ясно?");
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
                    string value = Regex.Match(alias, pattern).Value;
                    if (value == alias)
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
                throw new Exception($"Cannot found alias type format for alias type: {alias_type} for {Singer.Current.VoicebankType}");
            string format = Format[alias_type];
            int i = 0;
            while (Regex.IsMatch(format, ph))
            {
                if (i >= phonemes.Length)
                    throw new Exception($"Not enough phonemes to format alias {alias_type} for {Singer.Current.VoicebankType}");
                string ph_type = Regex.Match(format, ph).Value;
                if (MatchPhonemeType(ph_type, phonemes[i]))
                {
                    var f = new Regex(ph_type);
                    format = f.Replace(format, phonemes[i], 1);
                    i++;
                }
                else throw new Exception($"Wrong phonemes [ {String.Join(" ", phonemes)} ] to format alias {alias_type} for {Singer.Current.VoicebankType}");
            }

            return format;
        }

        public static string AliasReplace(string line)
        {
            foreach (string[] pair in AliasReplaces)
            {
                try
                {
                    var pattern = pair[0];
                    var replacement = pair[1];
                    if (Regex.IsMatch(line, pattern))
                        line = Regex.Replace(line, pattern, replacement);
                    //if (line.Contains(pair[0])) line = line.Replace(pair[0], pair[1]);
                }
                catch (Exception ex)
                {
                    Program.ErrorMessage(ex, "Error on AliasReplace");
                }
            }
            return line;
        }

        public static string PhonemeReplace(string phonemes)
        {
            foreach (string[] pair in PhonemeReplaces)
            {
                var pattern = pair[0];
                var replacement = pair[1];
                if (Regex.IsMatch(phonemes, pattern))
                    phonemes = Regex.Replace(phonemes, pattern, replacement);
            }
            return phonemes;
        }

        public static Syllable PhonemeReplace(Syllable syllable)
        {
            var phonemes = PhonemeReplace(syllable.ToString());
            return new Syllable(phonemes.Split(' '));
        }


        public static string[] DictAnalysis(string lyric)
        {
            // DEPRECATED
            List<string> aliases = new List<string>();
            int k;
            string l = "";
            List<string> syll = new List<string>();
            for (int i = 0; i < lyric.Length; )
            {
                for (k = lyric.Length - i; k > 0; k--)
                {
                    l = lyric.Substring(i, k);
                    if (Dict.Has(l))
                        break;
                    var last = l.Last().ToString();
                    if (Dict.Has(last) && Dict.Get(last)[0] == "#SKIP")
                        k--;
                }
                if (k == 0)
                {
                    return new[] { lyric };
                }
                else
                {
                    aliases.AddRange(Dict.Get(l));
                    i += k;
                }
            }
            int vs = aliases.Select(n => GetAliasType(n) != "C").ToArray().Length;
            if (vs == 1)
                return new[] { String.Join(" ", Dict.Get(l)) };
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

        public static bool AddWord(string word, string phonemes, bool toSendMail = false)
        {
            if (HasDict)
            {
                string line = word + "=" + phonemes;
                string old_phonemes = "";
                bool wasInDict = Dict.Has(word);
                if (wasInDict)
                    old_phonemes = String.Join(" ", Dict.Get(word));
                if (Dict.Add(line))
                {
                    if (!wasInDict || old_phonemes != phonemes)
                    {
                        writeWord(line, wasInDict);
                        if (toSendMail)
                        {
                            sendMail(word, phonemes, wasInDict);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        static void writeWord(string line, bool wasInDict)
        {
            if (wasInDict)
            {
                // Better not
                try
                {
                    File.AppendAllText(DictPath, line + "\r\n");
                }
                catch (Exception ex)
                {
                    Program.ErrorMessage(ex, "Cant modify dict file");
                }
            }
            else
            {
                try
                {
                    File.AppendAllText(DictPath, line + "\r\n");
                }
                catch (Exception ex)
                {
                    Program.ErrorMessage(ex, "Cant modify dict file");
                }
            }
        }

        static void sendMail(string word, string phonemes, bool wasInDict)
        {
            try
            {
                MailAddress from = new MailAddress("wavconfigtool@gmail.com", "WavConfigTool");
                MailAddress to = new MailAddress("wavconfigtool@gmail.com", "WavConfigTool");
                MailMessage m = new MailMessage(from, to);
                m.Subject = (wasInDict ? "Word correction " : "New word") + $" [{word}]";
                m.Body = $"<h1>{VoicebankType}</h1><p>{word}={phonemes}</p>";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("wavconfigtool@gmail.com", "wavconfig99231");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on sending mail");
            }
        }

        public static List<Syllable> GetSyllables(string[] phonemes)
        {
            var sylls = new List<Syllable>();
            var syll_phonemes = new List<string>();
            int i = 0;

            // non-vowel or 1 vowel word
            if (!phonemes.Any(n => IsVowel(n)) || phonemes.Count(n => IsVowel(n)) == 1)
                return new[] { new Syllable(phonemes) }.ToList();

            for (; i < phonemes.Length; )
            {
                // add beginning CC
                for (; IsConsonant(phonemes[i]); i++)
                    syll_phonemes.Add(phonemes[i]);

                // make last syll if only 1 vowel left
                var rest = phonemes.ToList().GetRange(i, phonemes.Length - i);
                if (rest.Count(n => IsVowel(n)) == 1)
                    for (; i < phonemes.Length; i++)
                        syll_phonemes.Add(phonemes[i]);
                else
                {
                    // add vowels itself
                    syll_phonemes.Add(phonemes[i]);
                    i++;

                    // add as [C*VC] [CV...] if there is more than 1 consonant between two vowels
                    if (i + 2 < phonemes.Length && IsConsonant(phonemes[i]) && IsConsonant(phonemes[i + 1]))
                    {
                        syll_phonemes.Add(phonemes[i]);
                        i++;
                    }
                }

                sylls.Add(new Syllable(syll_phonemes));
                syll_phonemes = new List<string>();
            }
            return sylls;
        }
    }
}
