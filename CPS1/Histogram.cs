namespace CPS1
{
    using System.Collections.Generic;

    using CPS1.Functions;

    public static class Histogram
    {
        public static IEnumerable<Point> GetHistogram(FunctionData data, int intervals)
        {
            var histogramList = new List<Point>();
            var step = (data.Amplitude * 2) / intervals;
            for (var i = 0; i <= intervals; i++)
            {
                histogramList.Add(new Point(i * step, 0));
            }

            foreach (var point in data.Points)
            {
                var index = (int)((point.Y + data.Amplitude) / step);
                histogramList[index].Y++;
            }

            return histogramList;
        }
    }
}