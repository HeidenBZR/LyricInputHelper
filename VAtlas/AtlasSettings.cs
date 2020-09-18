using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{
    public class AtlasSettings
    {
        public bool LengthByOto;
        public Dictionary<string, string> DefaultLyric = new Dictionary<string, string> { };
        public double Velocity = 1;
        public bool MakeVR = true;
        public bool MakeShort = true;
        public bool IsParsed = false;
        public bool IsUnparsed = false;
        public int MinLengthDefault;
        public int MinLength;
        public double CompressionRatio = 1;
        public double LastChildCompressionRatio = 1;
        public bool MakeFade = false;
    }
}
