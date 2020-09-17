using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
{
    public class NumberManager
    {

        public const string NEXT = "[#NEXT]";
        public const string PREV = "[#PREV]";
        public const string INSERT = "[#INSERT]";
        public const string DELETE = "[#DELETE]";
        public const string VERSION = "[#VERSION]";
        public const string SETTING = "[#SETTING]";

        public static bool IsNote(string number)
        {
            if (number.Length < 6) 
                return false;
            if (number == NEXT) 
                return true;
            if (number == PREV) 
                return true;
            return int.TryParse(number.Substring(2, 4), out int i);
        }

        public static string GetNumber(int i)
        {
            return $"[#{i.ToString().PadLeft(4, '0')}]";
        }

        public static int GetInt(string number, Ust ust)
        {
            if (number == "[#NEXT]")
                return ust.Notes.Count();
            return 
                int.Parse(number.Substring(2, 4));
        }
    }

}
