using System;
using System.Collections.Generic;
using CPS1.Model.Exceptions;
using CPS1.Model.SignalData;
using CPS1.Model.Transform.FourierTransform;

namespace CPS1.Model.Transform.WalshHadamardTransform
{
    public abstract class WalshHadamardTransform : ITransform
    {
        protected WalshHadamardTransform(string name)
        {
            Name = name;
        }

        protected void CheckPointsNumber(Point[] points)
        {
            var N = points.Length;
            if ((N != 0) && ((N & (N - 1)) != 0)) throw new InvalidSamplesNumberException("The number of samples must be a power of 2.");
        }
        public abstract IEnumerable<Point> Transform(Point[] signal);
        public string Name { get; }

        protected Matrix<double> WalshHadamardMatrix(int m)
        {
            if (m == 0)
            {
                var singleMatrix = new Matrix<double>(1, 1);
                singleMatrix[0, 0] = 1.0d;
                return singleMatrix;
            }

            var previousMatrix = WalshHadamardMatrix(m - 1);
            var sX = previousMatrix.SizeX;
            var sY = previousMatrix.SizeY;

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

            matrix *= 1 / Math.Sqrt(2);
            return matrix;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}