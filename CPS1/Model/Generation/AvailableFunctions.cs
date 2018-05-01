using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CPS1.Model.Composition;
using CPS1.Model.Exceptions;
using CPS1.Model.SignalData;
using CPS1.Properties;

namespace CPS1.Model.Generation
{
    public static class AvailableFunctions
    {
        private static readonly Random random = new Random();

        static AvailableFunctions()
        {
            var list =
                new Dictionary<Signal, Tuple<Func<FunctionData, double, double>, Required, string>>();

            list.Add(
                Signal.FullyRectifiedSine,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var ret = data.Amplitude.Value *
                                  Math.Sin(Math.PI * 2 * (t - data.StartTime.Value) / data.Period.Value);
                        if (ret.CompareTo(0) < 0)
                        {
                            ret *= -1;
                        }

                        return ret;
                    },
                    new Required(true, true, true, true, false, true, true, true),
                    "Fully rectified sine signal"));
            list.Add(
                Signal.HalfRectifiedSine,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var ret = data.Amplitude.Value *
                                  Math.Sin(Math.PI * 2 * (t - data.StartTime.Value) / data.Period.Value);
                        if (ret.CompareTo(0) < 0)
                        {
                            ret = 0;
                        }

                        return ret;
                    },
                    new Required(true, true, true, true, false, true, true, false),
                    "Half rectified sine signal"));
            list.Add(
                Signal.ImpulseNoise,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var threshold = random.NextDouble();
                        if (data.Probability.Value <= threshold)
                        {
                            return data.Amplitude.Value;
                        }

                        return 0;
                    },
                    new Required(true, true, false, true, false, true, false, true),
                    "Impulse noise signal"));
            list.Add(
                Signal.KroneckerDelta,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        if (Math.Abs(t - data.StartTime.Value) < double.Epsilon)
                        {
                            return data.Amplitude.Value;
                        }

                        return 0;
                    },
                    new Required(true, true, false, true, false, true, false, false),
                    "Kronecker delta signal"));
            list.Add(
                Signal.NormalDistribution,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var y = 0d;
                        for (var j = 0; j < Settings.Default.NumbersPerSample; j++)
                        {
                            y += random.NextDouble() * (data.Amplitude.Value * 2) - data.Amplitude.Value;
                        }

                        y /= Settings.Default.NumbersPerSample;

                        return y;
                    },
                    new Required(true, true, false, true, false, true, false, false),
                    "Gaussian distribution signal"));
            list.Add(
                Signal.RandomNoise,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var y = random.NextDouble() * (data.Amplitude.Value * 2) - data.Amplitude.Value;

                        return y;
                    },
                    new Required(true, true, false, true, false, true, false, false),
                    "Random noise signal"));
            list.Add(
                Signal.Sine,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                        data.Amplitude.Value * Math.Sin(Math.PI * 2 * (t - data.StartTime.Value) / data.Period.Value),
                    new Required(true, true, true, true, false, true, true, false),
                    "Sine signal"));
            list.Add(
                Signal.Square,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var k = (int) Math.Floor((t - data.StartTime.Value) / data.Period.Value);
                        var result = t - data.StartTime.Value - k * data.Period.Value;
                        if (result < data.DutyCycle.Value * data.Period.Value)
                        {
                            return data.Amplitude.Value;
                        }

                        return 0;
                    },
                    new Required(true, true, true, true, true, true, true, false),
                    "Square signal"));
            list.Add(
                Signal.SymmetricalSquare,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var k = (int) Math.Floor((t - data.StartTime.Value) / data.Period.Value);
                        var result = t - data.StartTime.Value - k * data.Period.Value;
                        if (result < data.DutyCycle.Value * data.Period.Value)
                        {
                            return data.Amplitude.Value;
                        }

                        return -data.Amplitude.Value;
                    },
                    new Required(true, true, true, true, true, true, true, false),
                    "Symmetrical square signal"));
            list.Add(
                Signal.Triangle,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        var k = (int) Math.Floor((t - data.StartTime.Value) / data.Period.Value);
                        var result = t - data.StartTime.Value - k * data.Period.Value;
                        if (result < data.DutyCycle.Value * data.Period.Value)
                        {
                            return data.Amplitude.Value * (t - k * data.Period.Value - data.StartTime.Value) /
                                   (data.DutyCycle.Value * data.Period.Value);
                        }

                        return -data.Amplitude.Value * (t - k * data.Period.Value - data.StartTime.Value) /
                               ((1 - data.DutyCycle.Value) * data.Period.Value) +
                               data.Amplitude.Value / (1 - data.DutyCycle.Value);
                    },
                    new Required(true, true, true, true, true, true, true, false),
                    "Triangle signal"));
            list.Add(
                Signal.UnitStep,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) =>
                    {
                        if (t < data.StartTime.Value)
                        {
                            return 0d;
                        }

                        if (Math.Abs(t - data.StartTime.Value) < double.Epsilon)
                        {
                            return data.Amplitude.Value * 0.5d;
                        }

                        return data.Amplitude.Value;
                    },
                    new Required(true, true, false, true, false, true, true, false),
                    "Unit step signal"));
            list.Add(Signal.Composite,
                new Tuple<Func<FunctionData, double, double>, Required, string>(
                    (data, t) => throw new InvalidFunctionException("Cannot get composite function formula."),
                    new Required(false, false, false, false, false, false, false, false),
                    "Composite signal"));

            Functions = ImmutableDictionary.CreateRange(list);
        }

        public static
            ImmutableDictionary<Signal, Tuple<Func<FunctionData, double, double>, Required, string>> Functions { get; }

        public static string GetDescription(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found)
            {
                throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));
            }

            return tuple.Item3;
        }

        public static Func<FunctionData, double, double> GetFunction(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found)
            {
                throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));
            }

            return tuple.Item1;
        }

        public static Required GetRequiredParameters(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found)
            {
                throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));
            }

            return tuple.Item2;
        }

        public static Signal GetTypeByDescription(string description)
        {
            Signal signal;
            try
            {
                signal = Functions.First(s => s.Value.Item3.Equals(description)).Key;
            }
            catch (InvalidOperationException e)
            {
                throw new SignalNotFoundException(
                    string.Format("Could not find signal with description: \"{0}\"", description),
                    e);
            }
            catch (ArgumentNullException e)
            {
                throw new SignalNotFoundException(
                    "Oh no! Something is terribly wrong. Contact the developer, it\'s all their fault!",
                    e);
            }

            return signal;
        }

        public static Func<FunctionData, double, double> GetComposite(List<Tuple<Operation, FunctionData>> composite)
        {
            var compositeFunction = GetFunction(composite[0].Item2.Type);
            for (var i = 1; i < composite.Count; i++)
            {
                compositeFunction = Compose(compositeFunction, composite[i].Item1, GetFunction(composite[i].Item2.Type),
                    composite[i].Item2);
            }

            return compositeFunction;
        }

        private static Func<FunctionData, double, double> Compose(
            Func<FunctionData, double, double> composite,
            Operation operation,
            Func<FunctionData, double, double> function,
            FunctionData functionData)
        {
            switch (operation)
            {
                case Operation.Add:
                    return (data, d) => composite(data, d) + function(functionData, d);
                case Operation.Multiply:
                    return (data, d) => composite(data, d) * function(functionData, d);
                case Operation.Subtract:
                    return (data, d) => composite(data, d) - function(functionData, d);
                case Operation.Divide:
                    return (data, d) => composite(data, d) / function(functionData, d);
            }

            return composite;
        }
    }
}