using System;
using LiveCharts;

namespace CPS1.Model.Transform.WalshHadamardTransform
{
    public class WalshHadamardTransform
    {
        private Matrix<double> WalshHadamardMatrix(int m)
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
            for (int i = 0; i < sX; i++)
            {
                for (int j = 0; j < sY; j++)
                {
                    matrix[i, j] = matrix[i, j + sY] =
                        matrix[i + sX, j] = previousMatrix[i, j];
                    matrix[i + sX, j + sY] = -previousMatrix[i, j];
                }
            }

            return matrix;
        }
    }
}