namespace CPS1.Functions
{
    using System;

    public abstract class Function
    {
        protected Func<double, double, double, double, double, double> SignalFunction { get; set; }

        public virtual void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
                var y = this.SignalFunction(data.Amplitude.Value, data.Period.Value, x, data.StartTime.Value, data.Probability.Value);
                data.Points.Add(new Point(x, y));
            }
        }
    }
}