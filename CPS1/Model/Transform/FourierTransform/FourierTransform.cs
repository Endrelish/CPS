using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public abstract class FourierTransform : ITransform
    {
        public virtual IEnumerable<Point> Transform(IEnumerable<Point> signal, bool reversed = false)
        {
            var signalList = signal.ToArray();
            var N = signalList.Length;
            var transform = new Point[N];
            

            for (int i = 0; i < N; i++)
            {
                transform[i] = new Point(i, TransformValue(i, N, signalList));
            }

            return transform;
        }

        protected abstract Complex TransformValue(int m, int N, Point[] signal);
        protected abstract Complex ReverseTransformValue(int m, int N, List<Point> signal);
    }
}