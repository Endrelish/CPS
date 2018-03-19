namespace CPS1.Functions
{
    using System;
    using System.Linq;

    public class KroneckerDelta : Function
    {
        public KroneckerDelta()
        {
            this.SignalFunction = (A, T, t, t1, p) =>
                {
                    if (Math.Abs(t - t1) < double.Epsilon)
                    {
                        return A;
                    }

                    return 0;
                };
        }

        public static Required RequiredAttributes { get; } =
            new Required(true, true, false, true, false, true, false, false);

        public override void GeneratePoints(FunctionData data)
        {
            data.Points.Clear();

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value - data.Duration.Value / 2.0d;
                var y = this.SignalFunction(
                    data.Amplitude.Value,
                    data.Period.Value,
                    x,
                    data.StartTime.Value,
                    data.Probability.Value);
                data.Points.Add(new Point(x, y));
            }

            if (data.Points.Count(p => Math.Abs(p.X - data.StartTime.Value) < double.Epsilon) == 0)
            {
                for (var i = 0; i < data.Points.Count; i++)
                {
                    if (data.Points[i].X > data.StartTime.Value)
                    {
                        data.Points.Insert(
                            i,
                            new Point(
                                data.StartTime.Value,
                                this.SignalFunction(
                                    data.Amplitude.Value,
                                    data.Period.Value,
                                    data.StartTime.Value,
                                    data.StartTime.Value,
                                    data.Probability.Value)));
                        break;
                    }
                }
            }
        }
    }
}