using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avis
{
    class PitchController
    {
        private readonly double Tempo;

        public PitchController(double tempo)
        {
            Tempo = tempo;
        }

        private double InterpolateVibrato(VibratoExpression vibrato, double posMs, long noteLength)
        {
            var lengthMs = vibrato.Length / 100 * MusicMath.Current.TickToMillisecond(noteLength, Tempo);
            var inMs = lengthMs * vibrato.In / 100;
            var outMs = lengthMs * vibrato.Out / 100;

            var value = -Math.Sin(2 * Math.PI * (posMs / vibrato.Period + vibrato.Shift / 100)) * vibrato.Depth;

            if (posMs < inMs)
                value *= posMs / inMs;
            else if (posMs > lengthMs - outMs) value *= (lengthMs - posMs) / outMs;

            return value;
        }

        public void BuildPitchData(RenderNote note, RenderNote prevNote, RenderNote nextNote)
        {
            BuildVibratoInfo(note, prevNote, nextNote, out var vibratoInfo, out var vibratoPrevInfo);
            var pitches = BuildVibrato(vibratoInfo, vibratoPrevInfo);
            var pitchInfo = BuildPitchInfo(note, prevNote, nextNote);
            var pitchesP = BuildPitch(pitchInfo);
            if (pitchInfo.Start > 0)
                throw new Exception();
            var offset = -pitchInfo.Start / INTERVAL_TICK;
            pitches = Interpolate(pitchesP, pitches, offset);
            note.PitchBend.Array = pitches;
            var renderNote = (RenderNote)note;
            renderNote.PitchInfo = pitchInfo;
            renderNote.VibratoInfo = vibratoInfo;
        }

        public PitchInfo BuildPitchInfo(RenderNote note, RenderNote prevNote, RenderNote nextNote)
        {
            var pitchInfo = new PitchInfo();
            var autoPitchLength = 40;
            var autoPitchOffset = -20;
            var firstNoteRaise = 10;

            var oto = note.Oto;
            var pps = new List<PitchPoint>();
            foreach (var pp in note.PitchBend.Points)
                pps.Add(pp);
            if (pps.Count == 0)
            {
                var offsetY = prevNote == null ? firstNoteRaise : GetPitchDiff(note.NoteNum, prevNote.NoteNum);
                pps.Add(new PitchPoint(MusicMath.Current.TickToMillisecond(autoPitchOffset, Tempo), -offsetY));
                pps.Add(new PitchPoint(MusicMath.Current.TickToMillisecond(autoPitchOffset + autoPitchLength, Tempo), 0));
                note.PitchBend.Data = pps;
            }

            var cutoffFromNext = nextNote != null ? nextNote.Oto.StraightPreutterance : 0;

            // end and start ms
            var startMs = pps.First().X < -oto.Preutterance ? pps.First().X : -oto.Preutterance;
            double endMs = MusicMath.Current.TickToMillisecond(note.FinalLength, Tempo) - cutoffFromNext;

            // if not all the length involved, add end and/or start pitch points
            if (pps.First().X > startMs)
                pps.Insert(0, new PitchPoint(startMs, pps.First().Y));
            if (pps.Last().X < endMs)
                pps.Add(new PitchPoint(endMs, pps.Last().Y));

            var start = (int)MusicMath.Current.SnapMs(pps.First().X, Tempo);
            var end = (int)MusicMath.Current.SnapMs(pps.Last().X, Tempo);

            // combine all
            pitchInfo.Start = start;
            pitchInfo.End = end;
            pitchInfo.PitchPoints = pps.ToArray();
            return pitchInfo;
        }

        public int GetOnePitchValue()
        {
            return 10;
        }

        public double GetPitchDiff(int noteNum1, int noteNum2)
        {
            return (noteNum1 - noteNum2) * GetOnePitchValue();
        }

        public void BuildVibratoInfo(RenderNote note, RenderNote prevNote, RenderNote nextNote, out VibratoInfo vibratoInfo, out VibratoInfo vibratoPrevInfo)
        {
            vibratoInfo = new VibratoInfo { Start = 0, End = 0, Vibrato = note.Vibrato, Length = note.FinalLength };
            if (prevNote != null)
                vibratoPrevInfo = new VibratoInfo
                {
                    Start = 0,
                    End = 0,
                    Vibrato = prevNote.Vibrato,
                    Length = prevNote.FinalLength
                };
            else
                vibratoPrevInfo = new VibratoInfo { Start = 0, End = 0, Vibrato = null, Length = 0 };

            if (note.Vibrato != null && note.Vibrato.Depth != 0)
            {
                vibratoInfo.End = MusicMath.Current.TickToMillisecond(note.FinalLength, Tempo);
                vibratoInfo.Start = vibratoInfo.End * (1 - note.Vibrato.Length / 100);
            }

            if (prevNote != null && prevNote.Vibrato != null && prevNote.Vibrato.Depth != 0)
            {
                vibratoPrevInfo.Start = -MusicMath.Current.TickToMillisecond(prevNote.FinalLength, Tempo) * prevNote.Vibrato.Length / 100;
                vibratoPrevInfo.End = 0;
            }
        }

        public int[] BuildPitch(PitchInfo pitchInfo)
        {
            var pitches = new List<int>();
            var interv = INTERVAL_TICK;
            var i = -1;
            double xl = 0; // x local
            var dir = -99; // up or down
            double y = -99; // point value
            double xk = -99; // sin width
            double yk = -99; // sin height
            double C = -99; // normalize to zero
            bool IsNextPointTime;
            bool IsLastPoint;
            int nextX;
            int thisX;
            for (var x = pitchInfo.Start; x <= pitchInfo.End; x += interv)
            {
                // only S shape is allowed
                nextX = (int)pitchInfo.PitchPoints[i + 1].X;
                thisX = x;
                IsNextPointTime = thisX < 0 ? thisX + interv >= nextX : thisX + interv >= nextX;
                IsLastPoint = i + 2 == pitchInfo.PitchPoints.Length;
                while (IsNextPointTime && !IsLastPoint || i < 0)
                {
                    // goto next pitch points pair
                    i++;
                    xk = pitchInfo.PitchPoints[i + 1].X - pitchInfo.PitchPoints[i].X;
                    yk = pitchInfo.PitchPoints[i + 1].Y - pitchInfo.PitchPoints[i].Y;
                    dir = pitchInfo.PitchPoints[i + 1].Y > pitchInfo.PitchPoints[i].Y ? 1 : -1;
                    dir = pitchInfo.PitchPoints[i + 1].Y == pitchInfo.PitchPoints[i].Y ? 0 : dir;
                    C = pitchInfo.PitchPoints[i + 1].Y;
                    xl = 0;
                    nextX = (int)(pitchInfo.PitchPoints[i + 1].X / interv - 0.5 * IntervalMs);
                    nextX -= (int)(nextX % IntervalMs);
                    thisX = (int)Math.Ceiling((double)x / interv);
                    IsNextPointTime = thisX >= nextX;
                    IsLastPoint = i + 2 == pitchInfo.PitchPoints.Length;
                }

                if (dir == -99) throw new Exception();
                yk = Math.Round(yk, 3);
                var X = Math.Abs(xk) < 5 ? 0 : 1 / xk * 10 * xl / Math.PI;
                y = -yk * (0.5 * Math.Cos(X) + 0.5) + C;
                y *= 10;
                pitches.Add((int)Math.Round(y));
                xl += interv;
            }

            //if (i < pitchInfo.PitchPoints.Length - 2)
            //    throw new Exception("Some points was not processed");
            return pitches.ToArray();
        }

        public const int INTERVAL_TICK = 5;
        public double IntervalMs => (int)MusicMath.Current.TickToMillisecond(INTERVAL_TICK, Tempo);

        public int[] BuildVibrato(VibratoInfo vibratoInfo, VibratoInfo vibratoPrevInfo)
        {
            var pitches = new List<int>();
            var interv = INTERVAL_TICK;
            for (var x = 0; x <= vibratoInfo.Length; x += interv)
            {
                double y = 0;
                //Apply vibratos
                if (MusicMath.Current.TickToMillisecond(x, Tempo) < vibratoPrevInfo.End &&
                    MusicMath.Current.TickToMillisecond(x, Tempo) >= vibratoPrevInfo.Start && vibratoPrevInfo.Vibrato != null)
                    y += InterpolateVibrato(vibratoPrevInfo.Vibrato,
                        MusicMath.Current.TickToMillisecond(x, Tempo) - vibratoPrevInfo.Start, vibratoPrevInfo.Length);

                if (MusicMath.Current.TickToMillisecond(x, Tempo) < vibratoInfo.End &&
                    MusicMath.Current.TickToMillisecond(x, Tempo) >= vibratoInfo.Start)
                    y += InterpolateVibrato(vibratoInfo.Vibrato, MusicMath.Current.TickToMillisecond(x, Tempo) - vibratoInfo.Start,
                        vibratoInfo.Length);

                pitches.Add((int)Math.Round(y));
            }

            return pitches.ToArray();
        }

        public int[] Interpolate(int[] pitches1, int[] pitches2, int offset = 0)
        {
            var pitches = new List<int>();
            var len = pitches1.Length > pitches2.Length + offset ? pitches1.Length : pitches2.Length;
            for (var i = -offset; i < len; i++)
            {
                var y1 = 0;
                var y2 = 0;
                if (pitches1.Length > i + offset && i + offset >= 0) y1 = pitches1[i + offset];
                if (pitches2.Length > i && i >= 0) y2 = pitches2[i];
                var z = y1 + y2;
                pitches.Add(z);
            }

            return pitches.ToArray();
        }

    }


    public struct VibratoInfo
    {
        // vibrato start ms
        public double Start;

        // vibrato end ms
        public double End;

        // note length tick
        public long Length;

        // vibrato
        public VibratoExpression Vibrato;
    }
    public class VibratoExpression : UExpression
    {
        public VibratoExpression() : base("vibrato", "VBR")
        {
        }

        private double _length;
        private double _period;
        private double _depth;
        private double _in;
        private double _out;
        private double _shift;
        private double _drift;

        public double Length
        {
            set => _length = Math.Max(0, Math.Min(100, value));
            get => _length;
        }

        public double Period
        {
            set => _period = Math.Max(64, Math.Min(512, value));
            get => _period;
        }

        public double Depth
        {
            set => _depth = Math.Max(5, Math.Min(200, value));
            get => _depth;
        }

        public double In
        {
            set
            {
                _in = Math.Max(0, Math.Min(100, value));
                _out = Math.Min(_out, 100 - value);
            }
            get => _in;
        }

        public double Out
        {
            set
            {
                _out = Math.Max(0, Math.Min(100, value));
                _in = Math.Min(_in, 100 - value);
            }
            get => _out;
        }

        public double Shift
        {
            set => _shift = Math.Max(0, Math.Min(100, value));
            get => _shift;
        }

        public double Drift
        {
            set => _drift = Math.Max(-100, Math.Min(100, value));
            get => _drift;
        }

        public override string Type => "pitch";

        public override object Data { set; get; }
    }
    public struct PitchInfo
    {
        public PitchPoint[] PitchPoints;

        // pitch start tick
        public int Start;

        // pitch end tick
        public int End;
    }

    public class ExpPoint : IComparable<ExpPoint>
    {
        public double X;
        public double Y;

        public int CompareTo(ExpPoint other)
        {
            if (X > other.X)
                return 1;
            if (X == other.X)
                return 0;
            return -1;
        }

        public ExpPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ExpPoint Clone()
        {
            return new ExpPoint(X, Y);
        }
    }
}
