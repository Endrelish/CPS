namespace CPS1
{
    using System.Collections.Generic;

    using CPS1.Functions;

    public static class Histogram
    {
        public static void GetHistogram(FunctionData data)
        {
            data.HistogramPoints.Clear();
            var step = data.Amplitude.Value * 2 / data.HistogramIntervals.Value;
            for (var i = 0; i <= data.HistogramIntervals.Value; i++)
            {
                data.HistogramPoints.Add(new Point(i * step, 0));
            }

            foreach (var point in data.Points)
            {
                var index = (int)((point.Y + data.Amplitude.Value) / step);
                data.HistogramPoints[index].Y++;
            }
            
        }
    }
}