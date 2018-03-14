namespace CPS1.Functions
{
    using System;

    public class SymmetricalSquareWave : IFunction
    {
        private Func<double, double, double, double, double, double> Function { get; }

        public SymmetricalSquareWave()
        {
            this.Function = (A, T, t1, kw, t) =>
                {
                    int k = (int)Math.Floor((t - t1) / T);
                    var result = (t - t1 - k * T);
                    if (result < kw * T) return A;
                    else return -A;
                };
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