namespace CPS1.Functions
{
    using System;

    public class NormalDistribution : IFunction
    {
        public static int NumbersPerSample { get; set; } = 100;

        public void GeneratePoints(FunctionData data)
        {
            var random = new Random();
            data.Points.Clear();
            var interval = (data.MaxArgument - data.MinArgument) / data.Samples;
            for (var i = 0; i < data.Samples; i++)
            {
                var y = 0d;
                for (var j = 0; j < NumbersPerSample; j++)
                {
                    y += random.NextDouble() * (data.MaxValue - data.MinValue) + data.MinValue;
                }

                y /= NumbersPerSample;
                data.Points.Add(new Point(i * interval + data.MinArgument, y));
            }

            data.SetMinMaxValue();
        }
    }
}