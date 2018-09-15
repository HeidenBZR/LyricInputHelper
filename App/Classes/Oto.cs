using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
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
    }
}
