using System.Collections.Generic;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class DiscreteFourierTransform : FourierTransform
    {
        public DiscreteFourierTransform() : base("Discrete Fourier Transform")
        {
        }

        protected override Complex TransformValue(int m, int N, Point[] signal)
        {
            Complex value = 0;
            for (var i = 0; i < N; i++)
            {
                value += new Complex(signal[i].Y, signal[i].Z) * TwiddleFactor(i, m, N);
            }

            return value;
        }

        protected override Complex ReverseTransformValue(int n, int N, List<Point> transform)
        {
            var value = new Complex(0, 0);
            for (var i = 0; i < N; i++)
            {
                value += transform[i].Y * TwiddleFactor(i, 1, N);
            }

            return value;
        }
    }
}