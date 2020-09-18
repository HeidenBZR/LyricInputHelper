using System;
using System.Collections.Generic;
using System.Text;

namespace Avis
{
    public class PitchBendExpression : UExpression
    {
        public PitchBendExpression() : base("pitch", "PIT")
        {
        }

        protected List<PitchPoint> _data = new List<PitchPoint>();
        protected bool _snapFirst = true;

        public override string Type => "pitch";

        public override object Data
        {
            set => _data = (List<PitchPoint>)value;
            get => _data;
        }

        public List<PitchPoint> Points => _data;

        public bool SnapFirst
        {
            set => _snapFirst = value;
            get => _snapFirst;
        }

        public void AddPoint(PitchPoint p)
        {
            _data.Add(p);
            _data.Sort();
        }

        public void RemovePoint(PitchPoint p)
        {
            _data.Remove(p);
        }

        public int[] Array { get; set; }
    }


    public class PitchPoint : ExpPoint
    {
        public PitchPointShape Shape;

        public PitchPoint(double x, double y, PitchPointShape shape = PitchPointShape.io) : base(x, y)
        {
            Shape = shape;
        }

        public new PitchPoint Clone()
        {
            return new PitchPoint(X, Y, Shape);
        }
    }

    public enum PitchPointShape
    {
        /// <summary>
        ///     SineInOut
        /// </summary>
        io,

        /// <summary>
        ///     Linear
        /// </summary>
        l,

        /// <summary>
        ///     SineIn
        /// </summary>
        i,

        /// <summary>
        ///     SineOut
        /// </summary>
        o
    }
}
