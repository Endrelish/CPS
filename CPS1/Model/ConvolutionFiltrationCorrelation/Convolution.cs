using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation
{
    public static class Convolution
    {
        public static IEnumerable<Point> Convolute(IEnumerable<Point> first, IEnumerable<Point> second)
        {
            var M = first.Count();
            var N = second.Count();
            var convolution = new List<Point>();

            for (int i = 0; i < M + N - 1; i++)
            {
                var y = 0.0d;

                for (int j = 0; j <M; j++)
                {
                    if (j < first.Count() && i - j >= 0 && i - j < second.Count())
                    {
                        y += first.ElementAt(j).Y * second.ElementAt(i - j).Y;
                    }
                }

                convolution.Add(new Point(i, y));
            }

            return convolution;
        }
    }
}