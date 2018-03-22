namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;

    public static class Generator
    {
        public static void GenerateSignal(
            FunctionData data,
            Signal signal,
            List<Tuple<Operation, FunctionData>> composite = null)
        {
            data.Points.Clear();
            data.Type = signal;
            Func<FunctionData, double, double> function;
            if (composite == null || composite.Count == 1)
            {
                function = AvailableFunctions.GetFunction(signal);
                data.CompositeFunctionComponents.Clear();
                data.CompositeFunctionComponents.Add(new Tuple<Operation, FunctionData>(Operation.Add, data));
            }
            else
            {
                function = AvailableFunctions.GetComposite(composite);
            }

            var interval = data.Duration.Value / (data.Samples.Value - 1);
            for (var i = 0; i < data.Samples.Value; i++)
            {
                var x = i * interval + data.StartTime.Value;
                try
                {
                    var y = function(data, x);
                    if (Math.Abs(y) < 10E-10)
                    {
                        y = 0d;
                    }
                    data.Points.Add(new Point(x, y));
                }
                catch (DivideByZeroException)
                {
                    // Everything is fine, we just don't add this point
                }
            }

            data.PointsUpdate();
        }
    }
}