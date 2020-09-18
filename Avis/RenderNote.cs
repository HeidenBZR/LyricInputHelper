using System;
using System.Collections.Generic;
using System.Text;
using VAtlas;

namespace Avis
{
    public class RenderNote : Note
    {
        public PitchInfo PitchInfo;
        public VibratoInfo VibratoInfo;

        public PitchBendExpression PitchBend;
        public VibratoExpression Vibrato;
        public Oto Oto;
        public Envelope Envelope;

        public RenderNote()
        {
            PitchBend = new PitchBendExpression();
        }

        public static RenderNote CreateFromNote(Note note)
        {
            var renderNote = new RenderNote();
            renderNote.FinalLength = note.FinalLength;
            renderNote.NoteNum = note.NoteNum;
            renderNote.Intensity = note.Intensity;
            renderNote.ParsedLyric = note.ParsedLyric;
            renderNote.Lyric = note.Lyric;
            renderNote.IsRest = note.IsRest;
            renderNote.Flags = note.Flags;
            renderNote.Velocity = note.Velocity;

            return renderNote;
        }


    }


    public struct Envelope
    {
        public double p1;
        public double p2;
        public double p3;
        public double p4;
        public double p5;
        public double v1;
        public double v2;
        public double v3;
        public double v4;
        public double v5;

        public Envelope(RenderNote note, RenderNote next = null)
        {
            p1 = note.Oto.Overlap;
            p2 = note.Oto.Preutterance;
            p3 = 30;
            p4 = 0;
            p5 = 0;
            v1 = 60;
            v2 = 100;
            v3 = 60;
            v4 = 0;
            v5 = 100;
            if (next != null)
                p3 = next.Oto.Overlap;
        }

    }
}
