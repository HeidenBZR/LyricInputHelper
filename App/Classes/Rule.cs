using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public class Rule
    {
        public bool MustConvert = false;
        public bool MustInsert = false;

        // for converting
        public Format FormatConvert;
        public Format FormatInsert;
    }

    public struct Format
    {

        public string AliasType;
        public int[] Members;
        public int[] MembersPrev;
        public bool UseAllPhonemes;
        public bool UseAllPrevPhonemes;

        public Format(string line, bool isInAliasTypes)
        {
            /// line looks like -V[][0], i.e. AliasType[MembersPrev][Members]
            /// 
            Members = new int[] { };
            MembersPrev = new int[] { };
            UseAllPrevPhonemes = false;

            if (isInAliasTypes)
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

        public string[] GetNewPhonemes(string[] phonemesPrev, string[] phonemes)
        {
            if (UseAllPrevPhonemes && UseAllPhonemes)
                return (string[])phonemesPrev.Concat(phonemesPrev);
            List<string> phonemesNew = new List<string>();
            if (UseAllPrevPhonemes)
                phonemesNew.AddRange(phonemesPrev);
            else if (IsEmptyPhoneme(MembersPrev))
                { }
            else if (IsLastPhoneme(MembersPrev))
                phonemesNew.Add(phonemesPrev.Last());
            else
                foreach (int ind in MembersPrev)
                    phonemesNew.Add(phonemesPrev[ind]);
            if (UseAllPhonemes)
                phonemesNew.AddRange(phonemes);
            else if (IsEmptyPhoneme(Members))
                { }
            else if (IsLastPhoneme(Members))
                phonemesNew.Add(phonemes.Last());
            else
                foreach (int ind in Members)
                    phonemesNew.Add(phonemes[ind]);
            //if (Members.Length + MembersPrev.Length != phonemesNew.Count)
            //    throw new Exception($"Error formating phonemes from [{Syllable.ToString(phonemesPrev)}] and [{Syllable.ToString(phonemes)}]");
            return phonemesNew.ToArray();
        }

        public bool IsLastPhoneme(int[] phonemes) 
        { 
            return phonemes[0] == -1; 
        }

        public bool IsEmptyPhoneme(int[] phonemes) 
        { 
            return phonemes.Length == 0; 
        }
    }

    public struct RuleResult
    {
        public string Alias;
        public string AliasType;

        public RuleResult(string alias, string aliasType)
        {
            Alias = alias;
            AliasType = aliasType;
        }
    }

}
