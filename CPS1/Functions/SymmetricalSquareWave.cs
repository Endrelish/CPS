namespace CPS1.Functions
{
    using System;

    public class SymmetricalSquareWave : IFunction
    {
        public SymmetricalSquareWave()
        {
            this.Function = (A, T, t1, kw, t) =>
                {
                    var k = (int)Math.Floor((t - t1) / T);
                    var result = t - t1 - k * T;
                    if (result < kw * T)
                    {
                        return A;
                    }

                    return -A;
                };
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true);

        private Func<double, double, double, double, double, double> Function { get; }

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

            data.RequiredAttributes = SymmetricalSquareWave.RequiredAttributes;
        }
    }
}