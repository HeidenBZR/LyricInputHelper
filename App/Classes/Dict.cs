using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public class Dict
    {
        private ConcurrentDictionary<string, string> _convert_rules;
        private ConcurrentDictionary<string, string[]> _words;
        private string _path;
        private bool _isMergeCV = true;
        private string _source;
        private StringBuilder _memory;

        public bool IsEnabled = false;
        public bool IsGenerated = false;

        public delegate void DictLoadEndHandler(bool enabled);
        public event DictLoadEndHandler OnDictLoadEnd;
        public Atlas Atlas;

        public Dict(string path, Atlas atlas)
        {
            _path = path;
            Atlas = atlas;
            OnDictLoadEnd += (bool x) => { IsGenerated = true;  };
        }

        bool ReadAdditional()
        {
            try
            {
                if (!File.Exists(_path))
                    return false;
                if (_words is null)
                    _words = new ConcurrentDictionary<string, string[]>();
                string[] dicts = File.ReadAllLines(_path, Encoding.UTF8);
                foreach (string line in dicts)
                    Add(line);
                return true;
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on reading additional dict");
                return false;
            }
        }

        public async void Import()
        {
            bool result = await GenerateAsync();
            IsEnabled = result;
            OnDictLoadEnd(IsEnabled); 
        }

        Task<bool> GenerateAsync()
        {
            return Task<bool>.Run(() =>
            {
                IsEnabled = Generate() | ReadAdditional();
                return IsEnabled;
            });
        }

        bool Generate()
        {
            var generate_file = _path + ".import";
            try
            {
                if (!ReadImportRules())
                    return false;

                _words = new ConcurrentDictionary<string, string[]>();
                bool was_errors = false;
                Parallel.ForEach(File.ReadLines(_source), (line, _, lineNumber) =>
                {
                    try
                    {
                        Add(line, import: true);
                    }
                    catch (Exception ex)
                    {
                        Program.Log($"Error on import dict.\r\n{ex.Message}\r\n{ex.StackTrace}");
                        was_errors = true;
                    }
                });
                if (was_errors)
                    Program.Log("Some errors occured on dict import. Check the log file.");
                return true;
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error on import dict");
                return false;
            }
        }

        bool ReadImportRules()
        {
            var generate_file = _path + ".import";

            if (!File.Exists(generate_file))
                return false;

            string[] info = File.ReadAllLines(generate_file, Encoding.UTF8);
            _source = info[0].Substring("source=".Length).Replace("\r\n", "");
            if (!Path.IsPathRooted(_source))
                _source = Path.Combine(Program.GetResourceFile("Atlas"), _source);
            var converts = info.Skip(1).Select(n => n.Split('\t'));
            _convert_rules = new ConcurrentDictionary<string, string>();
            foreach (var pair in converts)
            {
                if (pair.Length < 2)
                    continue;
                _convert_rules[pair[0]] = pair[1];
            }
            return true;
        }

        public string[] MergeCV(string[] phonemes)
        {
            List<string> phs = phonemes.ToList();
            for (int i = 1; i < phs.Count; i++)
            {
                if (_isMergeCV && Atlas.IsConsonant(phs[i - 1]) && Atlas.IsVowel(phs[i]))
                {
                    //phs[i - 1] = Atlas.GetAlias("CV", new[] { phs[i - 1], phs[i] }); // Слишком долго блет
                    phs[i - 1] = String.Join("", phs[i - 1], phs[i]);
                    phs.RemoveAt(i);
                    i--;
                }
            }
            return phs.ToArray();
        }

        public bool Add(string word, string[] phonemes, bool import = false)
        {
            //phonemes.Last().Replace("\r", "");
            //phonemes.Last().Replace("\n", "");
            bool had_word = _words.ContainsKey(word);
            if (import)
            {
                for (int i = 0; i < phonemes.Length; i++)
                    if (_convert_rules.ContainsKey(phonemes[i]))
                        phonemes[i] = _convert_rules[phonemes[i]];
                    //else
                    //    throw new Exception("No convert rule found for phoneme: " + phonemes[i]);
                /// CV теперь объединяется не в словаре, а при составлении слогов после окна SetText
                //if (_isMergeCV)
                //    phonemes = MergeCV(phonemes);
                //Memorize(word, phonemes);
            }

            _words[word] = phonemes;
            return true;
        }

        public bool Add(string word, string phonemes, bool import = false)
        {
            var phs = phonemes.Split(' ');
            if (phs.Length == 0)
                return false;
            else
                return Add(word, phs, import);
        }

        public bool Add(string line, bool import=false)
        {
            var pair = line.Split('=');
            if (pair.Length != 2)
                return false;
            else
                return Add(pair[0], pair[1], import);
        }

        public bool Has(string word)
        {
            return _words.ContainsKey(word);
        }

        public string[] Get(string word)
        {
            if (_words.ContainsKey(word))
                return _words[word];
            else
                return null;
        }

        void Memorize(string word, string[] phonemes)
        {
            _memory.Append($"{word}={String.Join(" ", phonemes)}\r\n");
        }

        public void Write()
        {
            try
            {
                File.WriteAllText(_path, _memory.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, "Error writing dict");
            }
        }
    }
}
