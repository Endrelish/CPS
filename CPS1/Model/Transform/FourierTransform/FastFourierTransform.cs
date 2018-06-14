using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.Exceptions;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class FastFourierTransform : FourierTransform
    {
        private List<Point> _even;
        private List<Point> _odd;
        private Dictionary<int, Complex> _twiddleFactors;

        public FastFourierTransform() : base("Fast Fourier Transform")
        {
            _twiddleFactors = new Dictionary<int, Complex>();
        }

        public override IEnumerable<Point> Transform(Point[] signal)
        {
            var enumerable = signal as Point[] ?? signal.ToArray();
            if (enumerable.Count() <= 1)
            {
                return enumerable;
            }

            var currentFt = new FastFourierTransform();
            var points = enumerable.ToArray();
            var N = points.Length;
            var o = new Point[points.Length / 2];
            var e = new Point[points.Length / 2];
            for (var i = 0; i < N - 1; i += 2)
            {
                e[i / 2] = points[i];
                o[i / 2] = points[i + 1];
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
                if(!_twiddleFactors.ContainsKey(m - N / 2))
                    _twiddleFactors[m - N / 2] = TwiddleFactor(m - N / 2, 1, N);
                    ret = _even[m - N / 2].ToComplex() - _twiddleFactors[m - N / 2] * _odd[m - N / 2].ToComplex();
            }
            else
            {
                if(!_twiddleFactors.ContainsKey(m))
                    _twiddleFactors[m] = TwiddleFactor(m, 1, N);
                ret = _even[m].ToComplex() + _twiddleFactors[m] * _odd[m].ToComplex();
            }

            return ret;
        }

        protected override Complex ReverseTransformValue(int m, int N, List<Point> signal)
        {
            throw new NotImplementedException();
        }
    }
}