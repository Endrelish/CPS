namespace CPS1.Functions
{
    using System;

    public class FullyRectifiedSineWave : IFunction
    {
        public FullyRectifiedSineWave()
        {
            this.Function = (A, T, t, t1) =>
                {
                    var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                    if (ret.CompareTo(0) < 0)
                    {
                        ret *= -1;
                    }

                    return ret;
                };
        }

        public static Required RequiredAttributes { get; set; } = new Required(true, true, true, true, true, true);

        private Func<double, double, double, double, double> Function { get; }

        public void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / data.Samples.Value;
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
                var y = this.Function(data.Amplitude.Value, data.Period.Value, x, data.StartTime.Value);
                data.Points.Add(new Point(x, y));
            }

            data.RequiredAttributes = FullyRectifiedSineWave.RequiredAttributes;
        }

        // TODO Make a list of known generators, choose one from list 
        
    }
}