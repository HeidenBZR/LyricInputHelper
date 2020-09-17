using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{
    public struct Word
    {
        public string Name;
        private string[] _phonemes;
        public List<Syllable> Syllables;

        public string[] Phonemes { get => _phonemes; set => _phonemes = value; }

        public Word( string name, string[] phonemes, Atlas atlas)
        {
            Name = name;
            _phonemes = null;
            Syllables = null;
            SetPhonemes(phonemes, atlas);
        }

        void SetPhonemes(string[] phonemes, Atlas atlas)
        {
            if (phonemes is null)
                return;
            Syllables = atlas.GetSyllables(phonemes);
            _phonemes = phonemes;
        }
    }
}
