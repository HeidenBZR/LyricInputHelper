using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
{
    class Number
    {

        public const string NEXT = "[#NEXT]";
        public const string PREV = "[#PREV]";
        public const string INSERT = "[#INSERT]";
        public const string DELETE = "[#DELETE]";
        public const string VERSION = "[#VERSION]";
        public const string SETTING = "[#SETTING]";

        public static bool IsNote(string number)
        {
            if (number.Length < 6) return false;
            if (number == NEXT) return true;
            if (number == PREV) return true;
            return int.TryParse(number.Substring(2, 4), out int i);
        }

        public static string GetNumber(int i)
        {
            return $"[#{i.ToString().PadLeft(4, '0')}]";
        }

        public static int GetInt(string number)
        {
            if (number == "[#NEXT]") return Ust.Notes.Count();
            return int.Parse(number.Substring(2, 4));
        }
    }

    public class UNote
    {
        private UNote _parent;

        public int Length;
        public int NoteNum;
        public UNote Parent
        {
            get => _parent;
            set
            {
                var temp = value;
                while (temp.Parent != null)
                    temp = temp.Parent;
                _parent = temp;
            }
        }
        private string number;
        private string lyric;
        private string parsedLyric = "";

        public string Lyric { get => lyric; set => lyric = ValidateLyric(value); }
        public string Number { get => number; set => number = value; }
        public string ParsedLyric { get => parsedLyric; set => parsedLyric = ValidateLyric(value); }
        public string ParsedLyricView
        {
            get
            {
                if (IsRest())
                    return "";
                else
                    return parsedLyric;
            }
        }

        public string[] GetText()
        {
            string lyric = Lyric != ParsedLyric && ParsedLyric == "" ? Lyric : ParsedLyric;
            if (Atlas.IsLoaded && Atlas.IsRest(lyric)) lyric = "R";
            if (lyric == "r") lyric = "rr";
            if (lyric == Classes.Number.DELETE) lyric = "";
            List<string> text = new List<string>
            {
                Number,
                $"Length={Length}",
                $"Lyric={lyric}",
                $"NoteNum={NoteNum}"
            };
            if (Number == Classes.Number.INSERT) text.Add("Modulation=0");
            return text.ToArray();
        }

        public string ValidateLyric(string lyric)
        {
            if (!Atlas.IsLoaded) return lyric;
            if (Atlas.IsRest(lyric)) return " ";
            if (lyric == "rr") return "r";
            else return lyric;
        }

        public bool IsRest()
        {
            if (!Atlas.IsLoaded) return false;
            else if (parsedLyric != null) return Atlas.IsRest(parsedLyric);
            else return Atlas.IsRest(ParsedLyric);
        }

    }

}
