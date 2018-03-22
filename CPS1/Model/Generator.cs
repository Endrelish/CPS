namespace CPS1.Model
{
    public static class Generator
    {
        public static void GenerateSignal(FunctionData data, Signal signal)
        {
            data.Points.Clear();
            data.Type = signal;

            var function = AvailableFunctions.GetFunction(signal);

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
                var y = function(
                    data,
                    x);
                data.Points.Add(new Point(x, y));
            }

            data.PointsUpdate();
        }
    }
}