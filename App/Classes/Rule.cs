using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    struct Format
    {
        public Format(string line)
        {
            /// line looks like -V[][0], i.e. AliasType[MembersPrev][Members]
            /// 
            Members = new int[] { };
            MembersPrev = new int[] { };
            UseAllPrevPhonemes = false;

            if (Atlas.AliasTypes.Contains(line))
            {
                AliasType = line;
                UseAllPhonemes = true;
                return;
            }
            
            UseAllPhonemes = false;
            if (line.Count(n => n == '[') != 2 || line.Count(n => n == ']') != 2)
                throw new Exception($"Error reading rule format: {line}");
            string[] splitted = line.Split(new char[] { '[', ']' });
            AliasType = splitted[0];
            string membersPrev = splitted[1];
            string members = splitted[3];


            if (membersPrev.Length != 0)
            {
                if (membersPrev.Trim() == "*")
                    UseAllPrevPhonemes = true;
                else
                    MembersPrev = membersPrev.Split(',').Select(n => int.Parse(n)).ToArray();
            }
            if (members.Length != 0)
            {
                if (members.Trim() == "*")
                    UseAllPhonemes = true;
                else
                    Members = members.Split(',').Select(n => int.Parse(n)).ToArray();
            }
        }

        public string AliasType;
        public int[] Members;
        public int[] MembersPrev;
        public bool IsLastPhoneme(int[] phonemes) { return phonemes[0] == -1; }
        public bool IsEmptyPhoneme(int[] phonemes) { return phonemes.Length == 0; }
        public bool UseAllPhonemes;
        public bool UseAllPrevPhonemes;

        public RuleResult GetResult(string lyricPrev, string lyric)
        {
            string[] phonemesPrev = Atlas.GetPhonemes(lyricPrev);
            string[] phonemes = Atlas.GetPhonemes(lyric);
            string[] phonemesNew = GetNewPhonemes(phonemesPrev, phonemes);
            string alias = Atlas.GetAlias(AliasType, phonemesNew);
            RuleResult ruleResult = new RuleResult(alias, AliasType);
            return ruleResult;
        }

        string[] GetNewPhonemes(string[] phonemesPrev, string[] phonemes)
        {
            if (UseAllPrevPhonemes && UseAllPhonemes)
                return (string[])phonemesPrev.Concat(phonemesPrev);
            if (UseAllPrevPhonemes)
                return phonemesPrev;
            if (UseAllPhonemes)
                return phonemes;
            List<string> phonemesNew = new List<string>();
            if (IsEmptyPhoneme(MembersPrev)) { }
            else if (IsLastPhoneme(MembersPrev)) phonemesNew.Add(phonemesPrev.Last());
            else foreach (int ind in MembersPrev) phonemesNew.Add(phonemesPrev[ind]);

            if (IsEmptyPhoneme(Members)) { }
            else if (IsLastPhoneme(Members)) phonemesNew.Add(phonemes.Last());
            else foreach (int ind in Members) phonemesNew.Add(phonemes[ind]);
            if (Members.Length + MembersPrev.Length != phonemesNew.Count)
                throw new Exception($"Error formating phonemes from [{Syllable.ToString(phonemesPrev)}] and [{Syllable.ToString(phonemes)}]");
            return phonemesNew.ToArray();
        }
    }

    struct RuleResult
    {
        public string Alias;
        public string AliasType;

        public RuleResult(string alias, string aliasType)
        {
            Alias = alias;
            AliasType = aliasType;
        }
    }

    class Rule
    {
        public bool MustConvert = false;
        public bool MustInsert = false;

        // for converting
        public Format FormatConvert;
        public Format FormatInsert;

        private static Dictionary<string, Rule> Links = new Dictionary<string, Rule>();

        public Rule(string ruleline)
        {
            if (ruleline.Contains(";"))
            {
                MustConvert = true;
                MustInsert = true;
                var tl = ruleline.Split(';');
                string toconvert = tl[0].StartsWith("INSERT") ? tl[1] : tl[0];
                string toinsert = tl[0].StartsWith("INSERT") ? tl[0] : tl[1];
                toinsert = toinsert.Substring("INSERT(".Length);
                toinsert = toinsert.TrimEnd(')');
                FormatConvert = new Format( toconvert);
                FormatInsert = new Format(toinsert);
            }
            else if (ruleline.StartsWith("INSERT"))
            {
                MustInsert = true;
                string toinsert = ruleline.Substring("INSERT(".Length);
                toinsert = toinsert.TrimEnd(')');
                FormatInsert = new Format(toinsert);
            }
            else
            {
                MustConvert = true;
                FormatConvert = new Format(ruleline);
            }
        }

        public static Rule GetRule(string subject)
        {
            if (!Links.ContainsKey(subject)) return null;
            return Links[subject];
        }

        public static void Read(string line)
        {
            if (line.Contains("="))
            {
                var t = line.Split('=');
                if (t.Length != 2) return;
                string subject = t[0];
                string rule = t[1];
                Links[subject] = new Rule(rule);
            }
            else if (line.Contains(">"))
            {
                var t = line.Split('>');
                if (t.Length != 2) return;
                string subject = t[0];
                string reference = t[1];
                FindLinks(subject, reference);
            }
        }

        public static void Read04(string line)
        {
            if (line.Contains("="))
            {
                var t = line.Split('=');
                if (t.Length != 2) return;
                string subject = t[0];
                string rule = t[1];

                if (subject.Contains("C*") || rule.Contains("C*"))
                {
                    string r = rule;
                    string s1 = subject.Split(',')[0];
                    string s2 = subject.Split(',')[1];

                    if (s1.Contains("C*") && s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        { 
                            for (int k = 0; k < Atlas.MULTICONSONANT_LIMIT; k++)
                            {
                                var final_s1 = s1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                                var final_s2 = s2.Replace("C*", String.Concat(Enumerable.Repeat("C", k + 1)));
                                var final_r = r.Replace("C*", String.Concat(Enumerable.Repeat("C", k + 1)));

                                Links[$"{final_s1},{final_s2}"] = new Rule(final_r);
                            }
                        }
                    }
                    else if (s1.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var final_s1 = s1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                            var final_s2 = s2;
                            var final_r = r;

                            Links[$"{final_s1},{final_s2}"] = new Rule(final_r);
                        }
                    }
                    else if (s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var final_s1 = s1;
                            var final_s2 = s2.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                            var final_r = r.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));

                            Links[$"{final_s1},{final_s2}"] = new Rule(final_r);
                        }
                    }
                }
                else
                {
                    Links[subject] = new Rule(rule);
                }
            }
            else if (line.Contains(">"))
            {
                var t = line.Split('>');
                if (t.Length != 2) return;
                string subject = t[0];
                string reference = t[1];
                if (subject.Contains("C*"))
                {
                    string s1 = subject.Split(',')[0];
                    string s2 = subject.Split(',')[1];
                    string r1 = reference.Split(',')[0];
                    string r2 = reference.Split(',')[1];

                    if (s1.Contains("C*") && s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            for (int k = 0; k < Atlas.MULTICONSONANT_LIMIT; k++)
                            {
                                var final_s1 = s1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                                var final_s2 = s2.Replace("C*", String.Concat(Enumerable.Repeat("C", k + 1)));
                                var final_r1 = r1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                                var final_r2 = r2.Replace("C*", String.Concat(Enumerable.Repeat("C", k + 1)));

                                FindLinks($"{final_s1},{final_s2}", $"{final_r1},{final_r2}");
                            }
                        }
                    }
                    else if (s1.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var final_s1 = s1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                            var final_s2 = s2;
                            var final_r1 = r1.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                            var final_r2 = r2.Replace("C*", "C");

                            FindLinks($"{final_s1},{final_s2}", $"{final_r1},{final_r2}");
                        }
                    }
                    else if (s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var final_s1 = s1;
                            var final_s2 = s2.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));
                            var final_r1 = r1.Replace("C*", "C");
                            var final_r2 = r2.Replace("C*", String.Concat(Enumerable.Repeat("C", i + 1)));

                            FindLinks($"{final_s1},{final_s2}", $"{final_r1},{final_r2}");
                        }
                    }
                }
                else
                    FindLinks(subject, reference.Replace("C*", "C"));
            }
        }
        static void FindLinks(string subject, string link)
        {
            if (Links.ContainsKey(link))
                Links[subject] = Links[link];
            else
                Program.Log($"Referensed rule {subject} was not defined");
        }
    }
}
