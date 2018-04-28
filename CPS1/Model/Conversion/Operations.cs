namespace CPS1.Model.Conversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CPS1.Model.Generation;
    using CPS1.Model.SignalData;

    public static class Operations
    {

        private static readonly Func<double, double> sincFunc = t =>
            {
                if (t == 0)
                {
                    return 1;
                }

                return Math.Sin(Math.PI * t) / (Math.PI * t);
            };
        public static void ZeroOrderHold(FunctionData first, FunctionData second)
        {
            first.AssignSignal(first);
            second.Continuous.Value = true;
            second.Samples.Value = 500;

            second.Function = (data, t) =>
                {
                    var ret = 0.0d;
                    if (first.Points.Any(p => p.X < t))
                    {
                        ret = first.Points.Where(p => p.X < t).OrderBy(p => -p.X).First().Y;
                    }
                    else
                    {
                        ret = first.Points.OrderBy(p => Math.Abs(p.X - t)).First().Y;
                    }

                    return ret;
                };
            Generator.GenerateSignal(second);
        }


        public static void SincReconstruction(FunctionData first, FunctionData second, int samples)
        {
            second.AssignSignal(first);
            second.Continuous.Value = true;
            second.Samples.Value = 500;

            second.Function = (data, t) =>
                {
                    var n = first.Points.Count(p => p.X < t);

                    var sum = 0.0d;
                    var ts = first.Duration.Value / first.Samples.Value;
                    for (var i = n - samples; i < n + samples; i++)
                    {
                        if (i >= 0 && i < first.Points.Count)
                        {
                            sum += first.Points[i].Y
                                   * sincFunc((t - first.Points[i].X) / ts);
                        }
                    }

                    return sum;
                };

            Generator.GenerateSignal(second);
        }


        public static void Sampling(FunctionData first, FunctionData second, double samplingFrequency)
        {
            second.AssignSignal(first);
            second.Continuous.Value = false;
            second.Samples.Value = (int)(second.Duration.Value * samplingFrequency);
            Generator.GenerateSignal(second);
        }


        public static void Quantization(FunctionData first, FunctionData second, double quantizationLevels)
        {
            var levels = new List<double>();
            var step = first.Amplitude.Value * 2.0d / (quantizationLevels - 1);
            for (var i = 0; i <= quantizationLevels - 1; i++)
            {
                levels.Add(-first.Amplitude.Value + i * step);
            }

            second.AssignSignal(first);

            foreach (var point in second.Points)
            {
                point.Y = levels.OrderBy(l => Math.Abs(point.Y - l)).First();
            }

            second.PointsUpdate();
        }

    }
}