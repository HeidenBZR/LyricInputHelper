using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public struct Flag
    {
        public string Code;
        public int Value;
        public int DefaultValue;


        public Flag(string code, int value)
        {
            DefaultValue = value;
            Value = value;
            Code = code;
            Flags[code] = this;
        }

        public bool Check()
        {
            return Value >= 0 && Value <= 100;
        }

        public void Fix()
        {
            if (Value > 100)
                Value = 100;
            if (Value < 0)
                Value = 0;
        }

        public string Write()
        {
            if (Value == DefaultValue)
                return "";
            else
                return $"{Code}{Value}";
        }


        public static Dictionary<string, Flag> Flags = new Dictionary<string, Flag>();

        public static void Init()
        {
            new Flag("INT", 100);
            new Flag("MOD", 10);
            new Flag("BRE", 0);

        }

        public static Flag Read(string code, int value)
        {
            Flag flag;
            if (Flags.ContainsKey(code))
                throw new Exception($"Unknown flag code: {code}");
            flag = Flags[code];
            flag.Value = value;
            return flag;
        }

    }

}
