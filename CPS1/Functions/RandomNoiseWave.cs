namespace CPS1.Functions
{
    using System;

    public class RandomNoiseWave : IFunction
    {
        public static Required RequiredAttributes { get; set; } = new Required(true, true, false, true, false, true);

        public void GeneratePoints(FunctionData data)
        {
            var random = new Random();
            data.Points.Clear();
            var interval = data.Duration.Value / data.Samples.Value;
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var y = random.NextDouble() * (data.Amplitude.Value * 2) - data.Amplitude.Value;
                data.Points.Add(new Point(i * interval + data.StartTime.Value, y));
            }

            data.RequiredAttributes = RandomNoiseWave.RequiredAttributes;

            data.SetAmplitude();
        }
    }
}