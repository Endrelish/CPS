namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    using CPS1.Model.Exceptions;
    using CPS1.Properties;

    public static class AvailableFunctions
    {
        static AvailableFunctions()
        {
            var list =
                new Dictionary<Signal, Tuple<Func<double, double, double, double, double, double, double>, Required, string>>();

            list.Add(Signal.FullyRectifiedSine,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                            if (ret.CompareTo(0) < 0)
                            {
                                ret *= -1;
                            }

                            return ret;
                        },
                    new Required(true, true, true, true, false, true, true, true),
                    "Fully rectified sine signal"));
            list.Add(Signal.HalfRectifiedSine,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                            if (ret.CompareTo(0) < 0)
                            {
                                ret = 0;
                            }

                            return ret;
                        },
                    new Required(true, true, true, true, false, true, true, false),
                    "Half rectified sine signal"));
            list.Add(Signal.ImpulseNoise,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var random = new Random();
                            var threshold = random.NextDouble();
                            if (p <= threshold)
                            {
                                return A;
                            }

                            return 0;
                        },
                    new Required(true, true, false, true, false, true, false, true),
                    "Impulse noise signal"));
            list.Add(Signal.KroneckerDelta,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            if (Math.Abs(t - t1) < double.Epsilon)
                            {
                                return A;
                            }

                            return 0;
                        },
                    new Required(true, true, false, true, false, true, false, false),
                    "Kronecker delta signal"));
            list.Add(Signal.NormalDistribution,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var random = new Random();
                            var y = 0d;
                            for (var j = 0; j < Settings.Default.NumbersPerSample; j++)
                            {
                                y += random.NextDouble() * (A * 2) - A;
                            }

                            y /= Settings.Default.NumbersPerSample;

                            return y;
                        },
                    new Required(true, true, false, true, false, true, false, false),
                    "Gaussian distribution signal"));
            list.Add(Signal.RandomNoise,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var random = new Random();
                            var y = random.NextDouble() * (A * 2) - A;

                            return y;
                        },
                    new Required(true, true, false, true, false, true, false, false),
                    "Random noise signal"));
            list.Add(Signal.Sine,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) => A * Math.Sin(Math.PI * 2 * (t - t1) / T),
                    new Required(true, true, true, true, false, true, true, false),
                    "Sine signal"));
            list.Add( Signal.Square,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                   
                    (A, T, t1, kw, p, t) =>
                        {
                            var k = (int)Math.Floor((t - t1) / T);
                            var result = t - t1 - k * T;
                            if (result < kw * T)
                            {
                                return A;
                            }

                            return 0;
                        },
                    new Required(true, true, true, true, true, true, true, false),
                    "Square signal"));
            list.Add(Signal.SymmetricalSquare,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var k = (int)Math.Floor((t - t1) / T);
                            var result = t - t1 - k * T;
                            if (result < kw * T)
                            {
                                return A;
                            }

                            return -A;
                        },
                    new Required(true, true, true, true, true, true, true, false),
                    "Symmetrical square signal"));
            list.Add(Signal.Triangle,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            var k = (int)Math.Floor((t - t1) / T);
                            var result = t - t1 - k * T;
                            if (result < kw * T)
                            {
                                return A * (t - k * T - t1) / (kw * T);
                            }

                            return -A * (t - k * T - t1) / ((1 - kw) * T) + A / (1 - kw);
                        },
                    new Required(true, true, true, true, true, true, true, false),
                    "Triangle signal"));
            list.Add(Signal.UnitStep,
                new Tuple<Func<double, double, double, double, double, double, double>, Required, string>(
                    
                    (A, T, t1, kw, p, t) =>
                        {
                            if (t < t1)
                            {
                                return 0d;
                            }

                            if (Math.Abs(t - t1) < double.Epsilon)
                            {
                                return A * 0.5d;
                            }

                            return A;
                        },
                    new Required(true, true, false, true, false, true, true, false),
                    "Unit step signal"));

            Functions = ImmutableDictionary.CreateRange(list);
        }

        public static
            ImmutableDictionary<Signal, Tuple<Func<double, double, double, double, double, double, double>, Required, string>> Functions { get; }

        public static string GetDescription(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found) throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));

            return tuple.Item3;
        }

        public static Func<double, double, double, double, double, double, double> GetFunction(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found) throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));

            return tuple.Item1;
        }

        public static Required GetRequiredParameters(Signal signal)
        {
            var found = Functions.TryGetValue(signal, out var tuple);
            if (!found) throw new SignalNotFoundException(string.Format("Could not find {0}.", signal.ToString()));

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
                    string.Format("Could not find signal with description: \"{0}\"", description), e);
            }
            catch (ArgumentNullException e)
            {
                throw new SignalNotFoundException(
                    "Oh no! Something is terribly wrong. Contact the developer, it\'s all their fault!", e);
            }

            return signal;
        }
    }
}