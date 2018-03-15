namespace CPS1.Functions
{
    using System;

    public class TriangleWave : IFunction
    {
        public TriangleWave()
        {
            this.Function = (A, T, t1, kw, t) => { return 0; };
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true);

        private Func<double, double, double, double, double, double> Function { get; }

        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / data.Samples.Value;
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
                var y = this.Function(data.Amplitude.Value, data.Period.Value, data.StartTime.Value, data.DutyCycle.Value, x);
                data.Points.Add(new Point(x, y));
            }
            
        }
    }
}