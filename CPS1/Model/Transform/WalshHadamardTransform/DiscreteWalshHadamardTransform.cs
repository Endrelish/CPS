using System;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.WalshHadamardTransform
{
    public sealed class DiscreteWalshHadamardTransform : WalshHadamardTransform
    {
        public override IEnumerable<Point> Transform(IEnumerable<Point> signal)
        {
            var signalList = signal.ToList();
            var count = signalList.Count();
            var points = new List<Point>(count);
            var matrix = new Matrix<double>(1, count);
            for (var i = 0; i < count; i++)
            {
                matrix[0, i] = signalList[i].Y;
            }

            var exp = Convert.ToInt32(Math.Log(count, 2));
            var whMatrix = WalshHadamardMatrix(exp);
            var transformMatrix = whMatrix * matrix;

            var transformList = transformMatrix.ToList();
            var index = 0;

            foreach (var d in transformList)
            {
                points.Add(new Point(index++, d));
            }

            return points;
        }
    }
}