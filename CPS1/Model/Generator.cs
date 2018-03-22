namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;

    public static class Generator
    {
        public static void GenerateSignal(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
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

            data.CalculateParameters();
            data.PointsUpdate();
            Histogram.GetHistogram(data);
        }
    }
}