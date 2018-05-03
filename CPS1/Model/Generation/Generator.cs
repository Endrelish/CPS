using System;
using CPS1.Model.SignalData;

namespace CPS1.Model.Generation
{
    public static class Generator
    {
        public static void GenerateSignal(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval;
                try
                {
                    var y = data.Function(data, x);

                    if (Math.Abs(y) < 10E-10)
                    {
                        y = 0d;
                    }

                    data.Points.Add(new Point(x, y));
                }
                catch (DivideByZeroException)
                {
                    // Everything is fine, we just don't add this point
                }
            }

            data.Points.Sort(); // No idea why I put it here, seems not to have any influence, but better don't remove.

            data.CalculateParameters();
            data.PointsUpdate();
            Histogram.GetHistogram(data);
        }
    }
}