namespace CPS1
{
    using System;

    public class RandomNoise : IFunction
    {
        public void GeneratePoints(FunctionData data)
        {
            var random = new Random();
            data.Points.Clear();
            var interval = (data.MaxArgument - data.MinArgument) / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var y = (random.NextDouble() * (data.MaxValue - data.MinValue)) + data.MinValue;
                data.Points.Add(new Point((i * interval) + data.MinArgument, y));
            }
        }
    }
}