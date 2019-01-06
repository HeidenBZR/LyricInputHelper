using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricInputHelper.Classes
{
    public class Note
    {
        private Note _parent;

        public int FinalLength;
        public int Length;
        public int NoteNum;
        public Note Parent
        {
            get => _parent;
            set
            {
                var temp = value;
                while (temp.Parent != null)
                    temp = temp.Parent;
                _parent = temp;
                Children = null;
            }
        }
        private string number;
        private string lyric;
        private string parsedLyric = "";
        private string envelope;

        public Word Word { get; set; }
        public Syllable Syllable { get; set; }
        public string WordName { get { return Word.Name; } }

        public int Intensity = 100;
        public string Flags = "";

        public bool HadVelocity = false;

        public List<Note> Children;
        private double _velocity = 1;

        public string Lyric { get => lyric; set => lyric = ValidateLyric(value); }
        public string Number { get => number; set => number = value; }
        public double Velocity { get => _velocity; set { _velocity = value; HadVelocity = true; } }
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

        public Note()
        {
            Children = new List<Note>();
        }

        public override string ToString()
        {
            return ParsedLyricView;
        }

        public string[] GetText()
        {
            string lyric = Lyric != ParsedLyric && ParsedLyric == "" ? Lyric : ParsedLyric;
            if (Atlas.IsLoaded && Atlas.IsRest(lyric)) lyric = "R";
            if (lyric == "r") lyric = "rr";
            if (lyric == Classes.Number.DELETE) lyric = "";
            var note = this;
            if (FinalLength == 0)
                Number = Classes.Number.DELETE;
            List<string> text = new List<string>
            {
                Number,
                $"Length={FinalLength}",
                $"Lyric={lyric}",
                $"NoteNum={NoteNum}",
            };
            if (Atlas.IsLoaded && !Atlas.IsRest(parsedLyric))
            {
                text.Add($"Intensity={Intensity}");
                if (Number == Classes.Number.INSERT) text.Add("Modulation=0");
                var velocity = (int)(Velocity * 100);
                if (HadVelocity || Velocity != 1)
                    text.Add($"Velocity={(velocity > 200 ? 200 : velocity)}");
                string alias_type = Atlas.GetAliasType(ParsedLyric);
                //text.Add($"Flags={Flags}{(alias_type is null || alias_type.Contains("V") ? "" : "P10" ) }");
            }
            if (envelope != null)
                text.Add($"Envelope={envelope}");
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
            if (!Atlas.IsLoaded)
                return false;
            else if (parsedLyric != null)
                return Atlas.IsRest(ParsedLyric);
            else
                return false;
        }

        public void GetEnvelope()
        {
            try
            {

                if (!Ust.IsLoaded || Singer.Current is null || !Atlas.IsLoaded || Atlas.IsRest(ParsedLyric))
                    return;
                var next = Ust.GetNextNote(this);
                double length = MusicMath.TickToMillisecond(FinalLength, Ust.Tempo);
                length += Singer.Current.FindOto(this).Preutterance / Velocity;
                if (next != null && Singer.Current.FindOto(next) != null)
                    length -= Singer.Current.FindOto(next).StraightPreutterance / next.Velocity;
                double this_o = Singer.Current.FindOto(this).Overlap / Velocity;
                double next_o = next is null || Atlas.IsRest(next.parsedLyric) ? 20 : Singer.Current.FindOto(next).Overlap / next.Velocity;
                if (this_o > length)
                    this_o = length / 2;
                if (length < this_o + next_o)
                    next_o = length - this_o;
                if (next_o < 0)
                    throw new Exception($"negative next-overlap from {next.parsedLyric} on {parsedLyric}");
                if (length < this_o + next_o)
                    throw new Exception($"Обязательно что-то пойдет не так. Блять.");
                var e = new double[10]
                {
                Math.Truncate(this_o * 100) / 100, //p1 -> self
                0, //p2 -> p1
                0, //p3 -> p4
                100, //v1
                100, //v2
                100, //v3
                100, //v4
                Math.Truncate(next_o * 100) / 100, //p4 -> self
                0, //p5 -> p2
                100, //v5
                };
                envelope = $"{e[0]} {e[1]} {e[2]} {e[3]} {e[4]} {e[5]} {e[6]} % {e[7]} {e[8]} {e[9]}";
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, $"Error on GetEnvelope for {Number} [{parsedLyric}]");
            }
        }

        public void MergeIntoLeft()
        {
            try
            {
                var prev = Ust.GetPrevNote(this);
                if (prev is null)
                    return;
                Number = Classes.Number.DELETE;
                prev.FinalLength += FinalLength;
                FinalLength = 0;
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, $"Error on MergeIntoLeft: {Number}");
            }
        }

        public void MergeIntoRight()
        {
            try
            {
                var next = Ust.GetNextNote(this);
                if (next is null)
                    return;
                Number = Classes.Number.DELETE;
                next.FinalLength += FinalLength;
                FinalLength = 0;
            }
            catch (Exception ex)
            {
                Program.ErrorMessage(ex, $"Error on MergeIntoRight: {Number}");
            }
        }
    }

}
