namespace CPS1.Model.Functions
{
    using System;

    public class TriangleWave : Function
    {
        public TriangleWave()
        {
            this.SignalFunction = (A, T, t1, kw, t) =>
                {
                    var k = (int)Math.Floor((t - t1) / T);
                    var result = t - t1 - k * T;
                    if (result < kw * T)
                    {
                        return A * (t - k * T - t1) / (kw * T);
                    }

                    return -A * (t - k * T - t1) / ((1 - kw) * T) + A / (1 - kw);
                };
        }

        public static Required RequiredAttributes { get; } =
            new Required(true, true, true, true, true, true, true, false);
    }
}