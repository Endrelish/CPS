namespace CPS1.Model.Generation
{
    using System;

    using CPS1.Model.SignalData;

    public static class Histogram
    {
        public static void GetHistogram(FunctionData data)
        {
            var amp = data.Amplitude.Value;
            data.SetAmplitude();
            var max = data.StartTime.Value + data.Duration.Value;
            if (data.Period.Visibility)
            {
                max = data.Period.Value * Math.Floor(data.Duration.Value / data.Period.Value) + data.StartTime.Value;
            }

            data.HistogramPoints.Clear();
            var step = data.Amplitude.Value * 2 / (data.HistogramIntervals.Value - 1);
            for (var i = 0; i < data.HistogramIntervals.Value; i++)
            {
                data.HistogramPoints.Add(new Point(i * step - data.Amplitude.Value, 0));
            }

            foreach (var point in data.Points)
            {
                if (point.X > max)
                {
                    break;
                }

                var index = (int)((point.Y + data.Amplitude.Value) / step);
                if (Math.Abs(step) < double.Epsilon) index = 0;

                try
                {
                    data.HistogramPoints[index].Y++;
                } catch(ArgumentOutOfRangeException) { }
            }

            foreach (var point in data.HistogramPoints)
            {
                var x = point.X;
                var y = point.Y;
            }
            data.Amplitude.Value = amp;
            data.HistogramPointsUpdate();
        }
    }
}