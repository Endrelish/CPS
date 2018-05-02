using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation
{
    public static class Correlation
    {
        public static IEnumerable<Point> Correlate(IEnumerable<Point> first, IEnumerable<Point> second)
        {
            int M = first.Count();
            int N = second.Count();

            var correlation = new List<Point>();

            for (int i = 1 - N; i < M; i++)
            {
                var y = 0.0d;

                for (int j = 0; j < M; j++)
                {
                    if (j < first.Count() && j - i >= 0 && j - i < second.Count())
                    {
                        y += first.ElementAt(j).Y * second.ElementAt(j - i).Y;
                    }
                }

                correlation.Add(new Point(i, y));
            }

            return correlation;
        }

        public static IEnumerable<Point> CorrelateUsingConvolution(IEnumerable<Point> first, IEnumerable<Point> second)
        {
            int M = first.Count();
            int N = second.Count();

            var secondReversed = second.ToList();
            secondReversed.Reverse();

            return Convolution.Convolute(first, secondReversed);
        }

    }
}