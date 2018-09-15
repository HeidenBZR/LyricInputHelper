using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
{
    public class Pitchmap
    {
        public List<string> Pitches;
        public string Dir;
        public Dictionary<string, string> Suffixes;
        public Dictionary<string, string> Prefixes;

        public Pitchmap(string dir)
        {
            Pitches = new List<string>();
            Suffixes = new Dictionary<string, string>();
            Prefixes = new Dictionary<string, string>();
            string filename = dir;
            if (!File.Exists(filename)) filename = Path.Combine(dir, @"prefix.map");
            if (!File.Exists(filename)) return;
            var lines = File.ReadAllLines(filename);
            Dir = filename;
            foreach (string line in lines)
            {
                var pr = line.Trim('\r', '\n').Split('\t');
                var pitch = pr[1];
                if (pitch.Length > 0 && !Pitches.Contains(pitch)) Pitches.Add(pitch);
                pitch = pr[2];
                if (pitch.Length > 0 && !Pitches.Contains(pitch)) Pitches.Add(pitch);
                Prefixes[pr[0]] = pr[1];
                Suffixes[pr[0]] = pr[2];
            }
        }

        public Oto Substract(Oto oto)
        {
            foreach (var pitch in Pitches)
            {
                if (oto.Alias.Contains(pitch))
                {
                    oto.Pitch = pitch;
                    oto.Alias = oto.Alias.Replace(pitch, "");
                    return oto;
                }
            }
            return oto;
        }
    }

}
