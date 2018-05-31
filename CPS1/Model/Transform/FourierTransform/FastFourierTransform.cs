using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class FastFourierTransform : FourierTransform
    {
        private List<Point> even;
        private List<Point> odd;
        private ITransform dft;

        public FastFourierTransform()
        {
            dft = new DiscreteFourierTransform();
        }

        public override IEnumerable<Point> Transform(IEnumerable<Point> signal)
        {
            var enumerable = signal as Point[] ?? signal.ToArray();
            odd = dft.Transform(enumerable.Where(s => enumerable.ToList().IndexOf(s) % 2 == 1)).ToList();
            even = dft.Transform(enumerable.Where(s => enumerable.ToList().IndexOf(s) % 2 == 0)).ToList();
            return base.Transform(enumerable);
        }

        protected override Complex TransformValue(int m, int N, List<Point> signal)
        {
            if (m - N / 2 > 0) return TransformValue(m - N / 2, N, signal);

            return even[m].ToComplex() - TwiddleFactor(m, N) * odd[m].ToComplex();
        }

        private Complex TwiddleFactor(int k, int n)
        {
            return Complex.Exp(-2 * Math.PI * k / n);
        }
    }
}