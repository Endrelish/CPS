namespace CPS1.Functions
{
    using System;

    using CPS1.Properties;

    public class NormalDistributionWave : Function
    {
        public static int NumbersPerSample { get; } = Settings.Default.NumbersPerSample;

        public static Required RequiredAttributes { get; } = new Required(true, true, false, true, false, true, false, false);

        public NormalDistributionWave()
        {
            SignalFunction = (A, T, t, t1, p) =>
                {
                    var random = new Random();
                    var y = 0d;
                    for (var j = 0; j < NormalDistributionWave.NumbersPerSample; j++)
                    {
                        y += random.NextDouble() * (A * 2) - A;
                    }

                    y /= NumbersPerSample;

                    return y;
                };
        }
    }
}