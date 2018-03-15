namespace CPS1.Functions
{
    using System;

    public class NormalDistributionWave : IFunction
    {
        public static int NumbersPerSample { get; set; } = 100;

        public static Required RequiredAttributes { get; set; } = new Required(true, true, false, true, false, true);

        public void GeneratePoints(FunctionData data)
        {
            var random = new Random();
            data.Points.Clear();
            var interval = data.Duration.Value / data.Samples.Value;
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var y = 0d;
                for (var j = 0; j < NumbersPerSample; j++)
                {
                    y += random.NextDouble() * (data.Amplitude.Value * 2) - data.Amplitude.Value;
                }

                y /= NumbersPerSample;
                data.Points.Add(new Point(i * interval + data.StartTime.Value, y));
            }
            

            data.SetAmplitude();
        }
    }
}