using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public struct Word
    {
        public string Name;
        private string[] _phonemes;
        public List<Syllable> Syllables;

        public string[] Phonemes { get => _phonemes; set => _phonemes = value; }

        public Word(string name, string[] phonemes)
        {
            Name = name;
            _phonemes = null;
            Syllables = null;
            SetPhonemes(phonemes);
        }

        void SetPhonemes(string[] phonemes)
        {
            if (phonemes is null)
                return;
            Syllables = Atlas.GetSyllables(phonemes);
            _phonemes = phonemes;
        }
    }
}
