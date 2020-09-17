using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public class RuleManager
    {

        public Dictionary<string, Rule> Links = new Dictionary<string, Rule>();

        public RuleManager(Atlas atlas)
        {
            this.atlas = atlas;
        }

        public void ReadRule(string line)
        {
            if (line.Contains("="))
            {
                var t = line.Split('=');
                if (t.Length != 2) return;
                string subject = t[0];
                string ruleString = t[1];
                Links[subject] = CreateRule(ruleString);
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

        public void ReadRule04(string line)
        {
            if (line.Contains("="))
            {
                var t = line.Split('=');
                if (t.Length != 2) return;
                string subject = t[0];
                string ruleString = t[1];

                if (subject.Contains("C*") || ruleString.Contains("C*"))
                {
                    string r = ruleString;
                    string s1 = subject.Split(',')[0];
                    string s2 = subject.Split(',')[1];

                    if (s1.Contains("C*") && s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            for (int k = 0; k < Atlas.MULTICONSONANT_LIMIT; k++)
                            {
                                var finalSubject1 = s1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                                var finalSubject2 = s2.Replace("C*", string.Concat(Enumerable.Repeat("C", k + 1)));
                                var finalRuleString = r.Replace("C*", string.Concat(Enumerable.Repeat("C", k + 1)));

                                Links[$"{finalSubject1},{finalSubject2}"] = CreateRule(finalRuleString);
                            }
                        }
                    }
                    else if (s1.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var finalSubject1 = s1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                            var finalSubject2 = s2;
                            var finalRuleString = r;

                            Links[$"{finalSubject1},{finalSubject2}"] = CreateRule(finalRuleString);
                        }
                    }
                    else if (s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var finalSubject1 = s1;
                            var finalSubject2 = s2.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                            var finalRuleString = r.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));

                            Links[$"{finalSubject1},{finalSubject2}"] = CreateRule(finalRuleString);
                        }
                    }
                }
                else
                {
                    Links[subject] = CreateRule(ruleString);
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
                                var finalSubject1 = s1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                                var finalSubject2 = s2.Replace("C*", string.Concat(Enumerable.Repeat("C", k + 1)));
                                var finalRuleString1 = r1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                                var finalRuleString2 = r2.Replace("C*", string.Concat(Enumerable.Repeat("C", k + 1)));

                                FindLinks($"{finalSubject1},{finalSubject2}", $"{finalRuleString1},{finalRuleString2}");
                            }
                        }
                    }
                    else if (s1.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var finalSubject1 = s1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                            var finalSubject2 = s2;
                            var finalRuleString1 = r1.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                            var finalRuleString2 = r2.Replace("C*", "C");

                            FindLinks($"{finalSubject1},{finalSubject2}", $"{finalRuleString1},{finalRuleString2}");
                        }
                    }
                    else if (s2.Contains("C*"))
                    {
                        for (int i = 0; i < Atlas.MULTICONSONANT_LIMIT; i++)
                        {
                            var finalSubject1 = s1;
                            var finalSubject2 = s2.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));
                            var finalRuleString1 = r1.Replace("C*", "C");
                            var finalRuleString2 = r2.Replace("C*", string.Concat(Enumerable.Repeat("C", i + 1)));

                            FindLinks($"{finalSubject1},{finalSubject2}", $"{finalRuleString1},{finalRuleString2}");
                        }
                    }
                }
                else
                    FindLinks(subject, reference.Replace("C*", "C"));
            }
        }


        public Rule GetRule(string subject)
        {
            return Links.ContainsKey(subject) ? Links[subject] : null;
        }


        public void FindLinks(string subject, string link)
        {
            if (Links.ContainsKey(link))
                Links[subject] = Links[link];
            else
                Program.Log($"Referensed rule {link} for {subject} was not defined");
        }

        #region private

        private Atlas atlas;

        private Rule CreateRule(string ruleString)
        {
            var rule = new Rule();
            InitRule(rule, ruleString);
            return rule;
        }

        private void InitRule(Rule rule, string ruleString)
        {
            if (ruleString.Contains(";"))
            {
                rule.MustConvert = true;
                rule.MustInsert = true;
                var tl = ruleString.Split(';');
                string toconvert = tl[0].StartsWith("INSERT") ? tl[1] : tl[0];
                string toinsert = tl[0].StartsWith("INSERT") ? tl[0] : tl[1];
                toinsert = toinsert.Substring("INSERT(".Length);
                toinsert = toinsert.TrimEnd(')');
                rule.FormatConvert = new Format(toconvert, atlas.AliasTypes.Contains(toconvert));
                rule.FormatInsert = new Format(toinsert, atlas.AliasTypes.Contains(toconvert));
            }
            else if (ruleString.StartsWith("INSERT"))
            {
                rule.MustInsert = true;
                string toinsert = ruleString.Substring("INSERT(".Length);
                toinsert = toinsert.TrimEnd(')');
                rule.FormatInsert = new Format(toinsert, atlas.AliasTypes.Contains(toinsert));
            }
            else
            {
                rule.MustConvert = true;
                rule.FormatConvert = new Format(ruleString, atlas.AliasTypes.Contains(ruleString));
            }
        }

        #endregion
    }
}
