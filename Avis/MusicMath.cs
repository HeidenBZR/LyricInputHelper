using System;
using System.Collections.Generic;
using System.Text;

namespace Avis
{
    class MusicMath
    {
        #region singleton base

        private static MusicMath current;
        private MusicMath()
        {

        }

        public static MusicMath Current
        {
            get
            {
                if (current == null)
                {
                    current = new MusicMath();
                }
                return current;
            }
        }

        #endregion
        
        
        public const int RESOLUTION = 480;

        public string[] noteStrings =
            new string[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        public string GetNoteString(int noteNum)
        {
            return noteNum < 0 ? "" : noteStrings[noteNum % 12] + (noteNum / 12 - 1);
        }

        public int[] BlackNoteNums = { 1, 3, 6, 8, 10 };

        public bool IsCenterKey(int noteNum)
        {
            return noteNum % 12 == 0;
        }

        public double[] zoomRatios = { 4.0, 2.0, 1.0, 1.0 / 2, 1.0 / 4, 1.0 / 8, 1.0 / 16, 1.0 / 32, 1.0 / 64 };

        public double getZoomRatio(double quarterWidth, int beatPerBar, int beatUnit, double minWidth)
        {
            int i;

            switch (beatUnit)
            {
                case 2:
                    i = 0;
                    break;
                case 4:
                    i = 1;
                    break;
                case 8:
                    i = 2;
                    break;
                case 16:
                    i = 3;
                    break;
                default:
                    throw new Exception("Invalid beat unit.");
            }

            if (beatPerBar % 4 == 0) i--; // level below bar is half bar, or 2 beatunit
            // else // otherwise level below bar is beat unit

            if (quarterWidth * beatPerBar * 4 <= minWidth * beatUnit) return beatPerBar / beatUnit * 4;

            while (i + 1 < zoomRatios.Length && quarterWidth * zoomRatios[i + 1] > minWidth) i++;
            return zoomRatios[i];
        }

        public double TickToMillisecond(double tick, double tempo)
        {
            double BeatTicks = RESOLUTION;
            var TempoCoeff = 60 / tempo;
            var size = tick / BeatTicks;
            return size * TempoCoeff * 1000;
        }

        public int MillisecondToTick(double ms, double tempo)
        {
            double BeatTicks = RESOLUTION;
            var TempoCoeff = 60 / tempo;
            return (int)(ms / TempoCoeff / 1000 * BeatTicks);
        }

        public double SinEasingInOut(double x0, double x1, double y0, double y1, double x)
        {
            var z = y0 + (y1 - y0) * (1 - Math.Cos((x - x0) / (x1 - x0) * Math.PI)) / 2;
            return y0 + (y1 - y0) * (1 - Math.Cos((x - x0) / (x1 - x0) * Math.PI)) / 2;
        }

        public double SinEasingInOutX(double x0, double x1, double y0, double y1, double y)
        {
            return Math.Acos(1 - (y - y0) * 2 / (y1 - y0)) / Math.PI * (x1 - x0) + x0;
        }

        public double SinEasingIn(double x0, double x1, double y0, double y1, double x)
        {
            return y0 + (y1 - y0) * (1 - Math.Cos((x - x0) / (x1 - x0) * Math.PI / 2));
        }

        public double SinEasingInX(double x0, double x1, double y0, double y1, double y)
        {
            return Math.Acos(1 - (y - y0) / (y1 - y0)) / Math.PI * 2 * (x1 - x0) + x0;
        }

        public double SinEasingOut(double x0, double x1, double y0, double y1, double x)
        {
            return y0 + (y1 - y0) * Math.Sin((x - x0) / (x1 - x0) * Math.PI / 2);
        }

        public double SinEasingOutX(double x0, double x1, double y0, double y1, double y)
        {
            return Math.Asin((y - y0) / (y1 - y0)) / Math.PI * 2 * (x1 - x0) + x0;
        }

        public double Linear(double x0, double x1, double y0, double y1, double x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        public double LinearX(double x0, double x1, double y0, double y1, double y)
        {
            return (y - y0) / (y1 - y0) * (x1 - x0) + x0;
        }

        public double InterpolateShape(double x0, double x1, double y0, double y1, double x,
            PitchPointShape shape)
        {
            switch (shape)
            {
                case PitchPointShape.io:
                    return SinEasingInOut(x0, x1, y0, y1, x);
                case PitchPointShape.i:
                    return SinEasingIn(x0, x1, y0, y1, x);
                case PitchPointShape.o:
                    return SinEasingOut(x0, x1, y0, y1, x);
                default:
                    return Linear(x0, x1, y0, y1, x);
            }
        }

        public double InterpolateShapeX(double x0, double x1, double y0, double y1, double y,
            PitchPointShape shape)
        {
            switch (shape)
            {
                case PitchPointShape.io:
                    return SinEasingInOutX(x0, x1, y0, y1, y);
                case PitchPointShape.i:
                    return SinEasingInX(x0, x1, y0, y1, y);
                case PitchPointShape.o:
                    return SinEasingOutX(x0, x1, y0, y1, y);
                default:
                    return LinearX(x0, x1, y0, y1, y);
            }
        }

        public double DecibelToLinear(double db)
        {
            return Math.Pow(10, db / 20);
        }

        public double LinearToDecibel(double v)
        {
            return Math.Log10(v) * 20;
        }

        public string NoteNum2String(int noteNum)
        {
            var octave = noteNum / 12 + 1;
            string note;
            switch (11 - noteNum % 12)
            {
                case 0:
                    note = "B";
                    break;
                case 1:
                    note = "A#";
                    break;
                case 2:
                    note = "A";
                    break;
                case 3:
                    note = "G#";
                    break;
                case 4:
                    note = "G";
                    break;
                case 5:
                    note = "F#";
                    break;
                case 6:
                    note = "F";
                    break;
                case 7:
                    note = "E";
                    break;
                case 8:
                    note = "D#";
                    break;
                case 9:
                    note = "D";
                    break;
                case 10:
                    note = "C#";
                    break;
                default:
                    note = "C";
                    break;
            }

            return $"{note}{octave}";
        }

        /// <summary>
        ///     Snap tick ON RENDER
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public long SnapTickInt(int tick)
        {
            var intervalTick = PitchController.INTERVAL_TICK;
            tick = (int)(tick + intervalTick * 0.25) / intervalTick * intervalTick;
            if (tick % intervalTick != 0) 
                throw new Exception();
            return tick;
        }

        public long SnapTickInt(dynamic tick)
        {
            return SnapTickInt((int)tick);
        }

        /// <summary>
        ///     Snap tick ON RENDER
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public long SnapTick(long tick)
        {
            var intervalTick = PitchController.INTERVAL_TICK;
            tick = (long)(tick + intervalTick * 0.25) / intervalTick * intervalTick;
            if (tick % intervalTick != 0) 
                throw new Exception();
            return tick;
        }

        public long SnapTick(dynamic tick)
        {
            return SnapTick((int)tick);
        }

        /// <summary>
        ///     Snap ms ON RENDER
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public double SnapMs(double ms, double tempo)
        {
            var tick = MillisecondToTick(ms, tempo);
            tick = (int)SnapTick(tick);
            return MillisecondToTick(tick, tempo);
        }
    }
}
