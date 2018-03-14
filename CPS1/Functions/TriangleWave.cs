namespace CPS1.Functions
{
    using System;
    using System.Windows.Controls;

    public class TriangleWave : IFunction
    {
        private Func<double, double, double, double, double, double> Function { get; }
        public TriangleWave()
        {
            this.Function = (A, T, t1, kw, t) => { return 0; };
        }
        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var x = i * interval + data.StartTime;
                var y = this.Function(data.Amplitude, data.Period, data.StartTime, data.DutyCycle, x);
                data.Points.Add(new Point(x, y));
            }
        }
    }
}