using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{

    public class Oto
    {
        public string File;
        public string Alias;
        public string Pitch;
        public double Offset;
        public double Consonant;
        public double Cutoff;
        public double Preutterance;
        public double Overlap;
        public double FullLength
        {
            get
            {
                if (Cutoff < 0) return Math.Abs(Cutoff);
                Wav wav = new Wav(File);
                return wav.Length - Cutoff - Offset;
            }
        }
        public double StraightPreutterance
        {
            get
            {
                return Math.Abs(Preutterance - Overlap);
            }
        }
        public double Length
        {
            get
            {
                if (Preutterance > Overlap)
                    return FullLength - Preutterance;
                else
                    return FullLength - Preutterance + Overlap;
            }
        }
        public double InnerLength
        {
            get
            {
                if (Preutterance > Overlap)
                    return 0;
                else
                    return Overlap - Preutterance;
            }
        }
        public double FixedVowel
        {
            get
            {
                return Consonant - Preutterance;
            }
        }
        public double Attack
        {
            get
            {
                if (Preutterance < Overlap)
                    return Overlap;
                else return Preutterance;
            }
        }

        public static Oto CreateDefault()
        {
            var oto = new Oto()
            {
                Preutterance = 100,
                Overlap = 50,
                Cutoff = -300,
                Consonant = 150,
                Offset = 100,
                File = ""
            };
            return oto;
        }
    }
}
