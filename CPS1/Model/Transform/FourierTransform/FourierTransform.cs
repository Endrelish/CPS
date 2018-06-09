using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public abstract class FourierTransform : ITransform
    {
        public virtual IEnumerable<Point> Transform(IEnumerable<Point> signal)
        {
            var signalList = signal.ToArray();
            var N = signalList.Length;
            var transform = new Point[N];
            

            for (int i = 0; i < N; i++)
            {
                transform[i] = new Point(i, TransformValue(i, N, signalList));
                transform[i].Round(10E-15);
            }

            var duration = (N - 1) * (signalList[1].X - signalList[0].X);

            foreach (var point in transform)
            {
                point.X /= duration;
            }

            return transform;
        }

        protected Complex TwiddleFactor(int k, int m, int N)
        {
            return Complex.Exp(new Complex(0, -2 * Math.PI * m * k / N));
        }

        protected abstract Complex TransformValue(int m, int N, Point[] signal);
        protected abstract Complex ReverseTransformValue(int m, int N, List<Point> signal);
    }
}