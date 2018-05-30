using System;
using System.Collections.Generic;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public sealed class DiscreteFourierTransform : FourierTransform
    {
        protected override Complex TransformValue(int m, int N, List<Point> signal)
        {
            var value = new Complex(0, 0);
            for (var i = 0; i < N; i++)
            {
                value += signal[i].Y * Complex.Exp(new Complex(0, -2 * Math.PI * m * i / N));
            }

            value /= N;
            return value;
        }
    }
}