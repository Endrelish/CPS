namespace CPS1.Functions
{
    using System;

    public class Sinusoid : IFunction
    {
        public Sinusoid()
        {
            this.SinusoidFunction = (A, T, t, t1) => A * Math.Sin(Math.PI * 2 * (t - t1) / T);
        }

        private Func<double, double, double, double, double> SinusoidFunction { get; }

        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = (data.MaxArgument - data.MinArgument) / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var x = i * interval + data.MinArgument;
                var y = this.SinusoidFunction(data.MaxValue, data.Period, x, data.StartTime);
                data.Points.Add(new Point(x, y));
            }

            data.SetMinMaxValue();
        }
    }
}