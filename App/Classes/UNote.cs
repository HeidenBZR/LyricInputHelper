using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Classes
{
    class Number
    {

        public const string Next = "[#NEXT]";
        public const string Prev = "[#PREV]";
        public const string Insert = "[#INSERT]";
        public const string Delete = "[#DELETE]";
        public const string Version = "[#VERSION]";
        public const string Setting = "[#SETTING]";

        public static bool IsNote(string number)
        {
            if (number.Length < 6) return false;
            if (number == Next) return true;
            if (number == Prev) return true;
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
        public int Length;
        public int NoteNum;
        public UNote Parent;
        private string number;
        private string lyric;
        private string parsedLyric = "";

        public string Lyric { get => lyric; set => lyric = ValidateLyric(value); }
        public string Number { get => number; set => number = value; }
        public string ParsedLyric { get => parsedLyric; set => parsedLyric = ValidateLyric(value); }

        public string[] GetText()
        {
            string lyric = Lyric != ParsedLyric && ParsedLyric == "" ? Lyric : ParsedLyric;
            if (Atlas.IsLoaded && Atlas.IsRest(lyric)) lyric = "R";
            if (lyric == "r") lyric = "rr";
            List<string> text = new List<string>
            {
                Number,
                $"Length={Length}",
                $"Lyric={lyric}",
                $"NoteNum={NoteNum}"
            };
            if (Number == Classes.Number.Insert) text.Add("Modulation=0");
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
            else if (Lyric != null) return Atlas.IsRest(Lyric);
            else return Atlas.IsRest(ParsedLyric);
        }

    }

}
