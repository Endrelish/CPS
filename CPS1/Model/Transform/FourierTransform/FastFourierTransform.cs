using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class FastFourierTransform : FourierTransform
    {
        private List<Point> _even;
        private List<Point> _odd;

        public FastFourierTransform() : base("Fast Fourier Transform")
        {
        }

        public override IEnumerable<Point> Transform(IEnumerable<Point> signal)
        {
            var enumerable = signal as Point[] ?? signal.ToArray();
            if (enumerable.Count() <= 1)
            {
                return enumerable;
            }

            var currentFt = new FastFourierTransform();
            var points = enumerable.ToList();
            var N = points.Count;
            var o = new List<Point>();
            var e = new List<Point>();
            for (var i = 0; i < N; i += 2)
            {
                e.Add(points[i]);
            }

            for (var i = 1; i < N; i += 2)
            {
                o.Add(points[i]);
            }

            _odd = currentFt.Transform(o).ToList();
            _even = currentFt.Transform(e).ToList();

            return base.Transform(points);
        }

        protected override Complex TransformValue(int m, int N, Point[] signal)
        {
            Complex ret;
            if (m - N / 2 >= 0)
            {
                var tf = TwiddleFactor(m - N / 2, 1, N);
                ret = _even[m - N / 2].ToComplex() - tf * _odd[m - N / 2].ToComplex();
            }
            else
            {
                var tf = TwiddleFactor(m, 1, N);
                ret = _even[m].ToComplex() + tf * _odd[m].ToComplex();
            }

            return ret;
        }

        protected override Complex ReverseTransformValue(int m, int N, List<Point> signal)
        {
            throw new NotImplementedException();
        }
    }
}