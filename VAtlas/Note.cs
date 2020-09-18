using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAtlas
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

        public string envelope;

        public Word Word { get; set; }
        public Syllable Syllable { get; set; }
        public string WordName { get { return Word.Name; } }
        public string AliasType { get; set; }

        public int Intensity = 100;
        public string Flags = "";

        public bool HadVelocity = false;

        public List<Note> Children;

        public string Lyric { get; set; }
        public string Number { get; set; }
        public double Velocity { get; set; } = 1;
        public string ParsedLyric { get ; set; }
        public string ParsedLyricView => IsRest ? "" : ParsedLyric;
        public bool IsRest { get; set; }

        public Note()
        {
            Children = new List<Note>();
        }

        public override string ToString()
        {
            return ParsedLyricView;
        }

        public void SetParsedLyric(Atlas atlas, string lyric)
        {
            // TODO: Wtf is parsed lyric and why normal lyric is not parsed??? Make better
            ParsedLyric = atlas.ValidateLyric(lyric);
            if (!atlas.IsLoaded)
                IsRest = false;
            else if (ParsedLyric != null)
                IsRest = atlas.IsRest(ParsedLyric);
            else
                IsRest = false;
        }

        public void SetVelocity(double velocity)
        {
            Velocity = velocity;
            HadVelocity = true;
        }

        public void MultiplyVelocity(double value)
        {
            Velocity *= value;
            HadVelocity = true;
        }

        public void MarkAsDelete()
        {
            ParsedLyric = NumberManager.DELETE;
            Number = NumberManager.DELETE;
            Length = 0;
        }

        public string[] GetText(Atlas atlas)
        {
            string lyric = Lyric != ParsedLyric && ParsedLyric == "" ? Lyric : ParsedLyric;
            if (atlas.IsLoaded && atlas.IsRest(lyric)) lyric = "R";
            if (lyric == "r") lyric = "rr";
            if (lyric == VAtlas.NumberManager.DELETE) lyric = "";
            var note = this;
            if (FinalLength == 0)
                Number = VAtlas.NumberManager.DELETE;
            List<string> text = new List<string>
            {
                Number,
                $"Length={FinalLength}",
                $"Lyric={lyric}",
                $"NoteNum={NoteNum}",
            };
            if (atlas.IsLoaded && !atlas.IsRest(ParsedLyric))
            {
                text.Add($"Intensity={Intensity}");
                if (Number == VAtlas.NumberManager.INSERT) text.Add("Modulation=0");
                var velocity = (int)(Velocity * 100);
                if (HadVelocity || Velocity != 1)
                    text.Add($"Velocity={(velocity > 200 ? 200 : velocity)}");
                string alias_type = atlas.GetAliasType(ParsedLyric);
                //text.Add($"Flags={Flags}{(alias_type is null || alias_type.Contains("V") ? "" : "P10" ) }");
            }
            if (envelope != null)
                text.Add($"Envelope={envelope}");
            return text.ToArray();
        }


        public void GetEnvelope(Note next, double tempo, bool isNextRest)
        {
            try
            {
                double length = MusicMath.TickToMillisecond(FinalLength, tempo);
                var oto = Singer.Current.FindOto(this);
                if (oto != null)
                    length += Singer.Current.FindOto(this).Preutterance / Velocity;
                if (next != null && Singer.Current.FindOto(next) != null)
                    length -= Singer.Current.FindOto(next).StraightPreutterance / next.Velocity;
                if (Velocity < 1 || (next != null && next.Velocity < 1))
                    throw new Exception($"Something is wrong with velocity. {Number}[{ParsedLyric}]: {Velocity}; " +
                        $"next {next.Number}[{next.ParsedLyric}]: {next.Velocity}.");
                if (length <= 0)
                    throw new Exception($"Got negative length on {Number}[{ParsedLyric}]. Please check oto " +
                        $"of next {next.Number}[{next.ParsedLyric}]. It has {Singer.Current.FindOto(next).Preutterance} " +
                        $"Preutterance and {Singer.Current.FindOto(next).Overlap}");
                double this_o = oto is null ? 20 : Singer.Current.FindOto(this).Overlap / Velocity;
                double next_o = 20;
                if (next != null && !isNextRest)
                {
                    var next_oto = Singer.Current.FindOto(next.ParsedLyric);
                    if (next_oto != null)
                        next_o = Singer.Current.FindOto(next).Overlap / next.Velocity;
                }
                if (this_o > length)
                    this_o = length / 2;
                if (length < this_o + next_o)
                    next_o = length - this_o;
                if (next_o < 0)
                    throw new Exception($"negative next-overlap from [{next.ParsedLyric}] on [{ParsedLyric}]");
                if (length < this_o + next_o)
                    throw new Exception($"Note [{ParsedLyric}] has negative length. Please, check the oto. If it's ok, temporally lower the BGM (tempo) and run the plugin again.");
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
            catch (EntryPointNotFoundException ex)
            {
                Errors.ErrorMessage(ex, $"Error on GetEnvelope for {Number} [{ParsedLyric}]");
            }
        }

        public void MergeIntoLeft(Note prev)
        {
            try
            {
                if (prev is null)
                    return;
                Number = VAtlas.NumberManager.DELETE;
                prev.FinalLength += FinalLength;
                FinalLength = 0;
            }
            catch (Exception ex)
            {
                Errors.ErrorMessage(ex, $"Error on MergeIntoLeft: {Number}");
            }
        }

        public void MergeIntoRight(Note next)
        {
            try
            {
                if (next is null)
                    return;
                Number = VAtlas.NumberManager.DELETE;
                next.FinalLength += FinalLength;
                FinalLength = 0;
            }
            catch (Exception ex)
            {
                Errors.ErrorMessage(ex, $"Error on MergeIntoRight: {Number}");
            }
        }
    }

}
