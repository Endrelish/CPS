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
            var interval = data.Duration / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var y = random.NextDouble() * (data.Amplitude * 2) - data.Amplitude;
                data.Points.Add(new Point(i * interval + data.StartTime, y));
            }

            data.SetAmplitude();
        }
    }
}