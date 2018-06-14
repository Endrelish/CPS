using System;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.WalshHadamardTransform
{
    public sealed class FastWalshHadamardTransform : WalshHadamardTransform
    {
        public FastWalshHadamardTransform() : base("Fast Walsh-Hadamard Transform")
        {
        }

        public override IEnumerable<Point> Transform(Point[] signal)
        {
            CheckPointsNumber(signal);
            var count = signal.Length;
            var exp = Convert.ToInt32(Math.Log(count, 2));

            var whTransform = WalshHadamardMatrix(exp - 1);
            var points = new List<Point>(count);

            var vector = new Matrix<double>(1, count / 2);

            for (var i = 0; i < count / 2; i++)
            {
                vector[0, i] = signal[i].Y + signal[i + count / 2].Y;
            }

            var half = whTransform * vector;
            var j = 0;
            foreach (var d in half.ToList())
            {
                points.Add(new Point(j++, d));
            }

            for (var i = 0; i < count / 2; i++)
            {
                vector[0, i] = signal[i].Y - signal[i + count / 2].Y;
            }

            half = whTransform * vector;
            foreach (var d in half.ToList())
            {
                points.Add(new Point(j++, d));
            }

            return points;
        }
    }
}