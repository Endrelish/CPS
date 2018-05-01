using System;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.Conversion
{
    public static class Metrics
    {
        public static double MaximumDifference(FunctionData first, FunctionData second)
        {
            return second.Points
                .Select(p => Math.Abs(p.Y - first.Function(second, p.X))).Max();
        }

        public static double MeanSquaredError(FunctionData first, FunctionData second)
        {
            return second.Points
                       .Select(p => Math.Pow(first.Function(first, p.X) - p.Y, 2)).Sum()
                   / second.Points.Count;
        }

        public static double PeakSignalToNoiseRatio(FunctionData first, FunctionData second)
        {
            return
                10 * Math.Log10(first.Points.Max(p => p.Y) / MeanSquaredError(first, second));
        }

        public static double SignalToNoiseRatio(FunctionData first, FunctionData second)
        {
            var numerator = second.Points.Select(p => first.Function(first, p.X))
                .Sum();
            var denominator = second.Points
                .Select(p => Math.Pow(first.Function(first, p.X) - p.Y, 2)).Sum();

            return 10 * Math.Abs(Math.Log10(Math.Abs(numerator / denominator)));
        }
    }
}