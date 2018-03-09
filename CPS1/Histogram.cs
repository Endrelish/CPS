namespace CPS1
{
    using System.Collections;
    using System.Collections.Generic;

    public static class Histogram
    {
        public static IEnumerable<Point> GetHistogram(FunctionData data, int intervals)
        {
            var histogramList = new List<Point>();
            double treshold = (data.MaxValue - data.MinValue) / intervals;
            for (int i = 0; i < intervals; i++)
            {
                histogramList.Add(new Point(i * treshold, 0));
            }

            foreach (var point in data.Points)
            {
                var index = (int)((point.Y - data.MinValue) / treshold);
                histogramList[index].Y++;
            }

            return histogramList;
        }
    }
}