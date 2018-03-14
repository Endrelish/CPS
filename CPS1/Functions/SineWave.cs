namespace CPS1.Functions
{
    using System;

    public class SineWave : IFunction
    {
        public SineWave()
        {
            this.SinusoidFunction = (A, T, t, t1) => A * Math.Sin(Math.PI * 2 * (t - t1) / T);
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, false, true);

        private Func<double, double, double, double, double> SinusoidFunction { get; }

        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var x = i * interval + data.StartTime;
                var y = this.SinusoidFunction(data.Amplitude, data.Period, x, data.StartTime);
                data.Points.Add(new Point(x, y));
            }
        }
    }
}