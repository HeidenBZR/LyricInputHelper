using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public class Syllable
    {
        public string[] Phonemes;
        public readonly bool ContainsVowel = false;
        public readonly string Vowel;
        public readonly int VowelIndex;
        public readonly string[] Beginning = new string[0];
        public readonly string[] Ending = new string[0];
        public readonly string[] ConsonantBeginning = new string[0];
        public readonly string[] ConsonantEnding = new string[0];
        public readonly string[] VC = new string[0];
        public readonly string[] CV = new string[0];
        public readonly string[] ExtraConsonantBeginning = new string[0];
        public readonly string[] ExtraConsonantEnding = new string[0];
                
        public Syllable(IEnumerable<string> phonemes)
        {
            Phonemes = phonemes.ToArray();
            try
            {
                List<string> phs = phonemes.ToList();
                if (Phonemes.Any(n => Atlas.IsVowel(n)))
                {
                    ContainsVowel = true;
                    int v = phs.FindIndex(n => Atlas.IsVowel(n));
                    VowelIndex = v;
                    Vowel = phs[v];
                    int count = phs.Count;
                    if (v > 0)
                    {
                        CV = phs.GetRange(v - 1, 2).ToArray();
                        Beginning = phs.GetRange(0, v + 1).ToArray();
                        ConsonantBeginning = phs.GetRange(0, v).ToArray();
                    }
                    else
                    {
                        CV = new[] { Vowel };
                        Beginning = new[] { Vowel };
                    }
                    if (v > 2)
                    {
                        ExtraConsonantBeginning = phs.GetRange(0, v - 1).ToArray();
                    }
                    if (v < count - 1)
                    {
                        VC = phs.GetRange(v, 2).ToArray();
                        Ending = phs.GetRange(v, count - v).ToArray();
                        ConsonantEnding = phs.GetRange(v + 1, count - v - 1).ToArray();
                    }
                    else
                        Ending = new string[] { Vowel };
                    if (v < count - 2)
                        ExtraConsonantEnding = phs.GetRange(v + 2, count - v - 2).ToArray();
                }
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on Syllable parsing");
            }
                
        }


        public override string ToString()
        {
            if (Phonemes is null)
                return "";
            else
                return String.Join(" ", Phonemes);
        }

        public static string ToString(IEnumerable<string> phonemes)
        {
            if (phonemes is null)
                return "(null)";
            else
                return string.Join(" ", phonemes);
        }
    }
}
