namespace CPS1.Functions
{
    using System;

    public class HalfRectifiedSineWave : IFunction
    {
        public HalfRectifiedSineWave()
        {
            this.Function = (A, T, t, t1) =>
                {
                    var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                    if (ret.CompareTo(0) < 0)
                    {
                        ret = 0;
                    }

                    return ret;
                };
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true);

        private Func<double, double, double, double, double> Function { get; }

        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var x = i * interval + data.StartTime;
                var y = this.Function(data.Amplitude, data.Period, x, data.StartTime);
                data.Points.Add(new Point(x, y));
            }
        }
    }
}