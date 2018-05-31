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
        private ITransform fft;
        
        public override IEnumerable<Point> Transform(IEnumerable<Point> signal, bool reversed = false)
        {
            if (signal.Count() == 2) fft = new DiscreteFourierTransform();
                else fft = new FastFourierTransform();
            var enumerable = signal as Point[] ?? signal.ToArray();
            odd = fft.Transform(enumerable.Where(s => enumerable.ToList().IndexOf(s) % 2 == 1)).ToList();
            even = fft.Transform(enumerable.Where(s => enumerable.ToList().IndexOf(s) % 2 == 0)).ToList();
            return base.Transform(enumerable);
        }

        protected override Complex TransformValue(int m, int N, Point[] signal)
        {
            if (m - N / 2 >= 0) return TransformValue(m - N / 2, N, signal);

            return even[m].ToComplex() - TwiddleFactor(m, N) * odd[m].ToComplex();
        }

        protected override Complex ReverseTransformValue(int m, int N, List<Point> signal)
        {
            throw new NotImplementedException();
        }

        private Complex TwiddleFactor(int k, int n)
        {
            return Complex.Exp(-2 * Math.PI * k / n);
        }
    }
}