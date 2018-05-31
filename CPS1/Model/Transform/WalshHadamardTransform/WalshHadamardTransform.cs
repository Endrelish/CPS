using System;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;
using CPS1.Model.Transform.FourierTransform;

namespace CPS1.Model.Transform.WalshHadamardTransform
{
    public abstract class WalshHadamardTransform : ITransform
    {
        protected Matrix<double> WalshHadamardMatrix(int m)
        {
            if (m == 0)
            {
                var singleMatrix = new Matrix<double>(1, 1);
                singleMatrix[0, 0] = 1.0d;
            }

            var previousMatrix = WalshHadamardMatrix(m - 1);
            var sX = previousMatrix.SizeX;
            var sY = previousMatrix.SizeY;

            previousMatrix *= 1.0d / Math.Sqrt(2);

            var matrix = new Matrix<double>(previousMatrix.SizeX * 2, previousMatrix.SizeY * 2);
            for (var i = 0; i < sX; i++)
            {
                for (var j = 0; j < sY; j++)
                {
                    matrix[i, j] = matrix[i, j + sY] =
                        matrix[i + sX, j] = previousMatrix[i, j];
                    matrix[i + sX, j + sY] = -previousMatrix[i, j];
                }
            }

            return matrix;
        }

        public abstract IEnumerable<Point> Transform(IEnumerable<Point> signal);
    }
}