namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    [Serializable]
    public static class FunctionComposer
    {
        public static Func<FunctionData, double, double> ComposeFunction(Func<FunctionData, double, double> first, Func<FunctionData, double, double> second, FunctionData secondData, Operation operation)
        {
            var fd = new FunctionData(secondData.StartTime.Value, secondData.Amplitude.Value, secondData.Period.Value, secondData.Duration.Value, secondData.DutyCycle.Value, secondData.Samples.Value, secondData.HistogramIntervals.Value, secondData.Probability.Value);
            fd.Function = (Func<FunctionData, double, double>)secondData.Function.Clone();
            var composition = first;
                switch (operation)
                {
                    case Operation.Add:
                        composition = (data, x) => first(data, x) + second(fd, x);
                        break;
                    case Operation.Subtract:
                        composition = (data, x) => first(data, x) - second(fd, x);
                        break;
                    case Operation.Multiply:
                        composition = (data, x) => first(data, x) * second(fd, x);
                        break;
                    case Operation.Divide:
                        composition = (data, x) =>
                            {
                                var y1 = first(data, x);
                                var y2 = second(fd, x);

                                if (double.IsPositiveInfinity(y1))
                                {
                                    return double.MaxValue;
                                }
                                else if (double.IsNegativeInfinity(y1))
                                {
                                    return double.MinValue;
                                }
                                else if (double.IsNaN(y1))
                                {
                                    return 0;
                                }

                                if (double.IsInfinity(y2))
                                {
                                    return 0;
                                }
                                else if (double.IsNaN(y2))
                                {
                                    return y1 > 0 ? double.MaxValue : double.MinValue;
                                }
                                return y1 / y2;
                            };
                        break;
            }

            return composition;
        }
    }
}