namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    using CPS1.Properties;

    public static class AvailableFunctions
    {
        static AvailableFunctions()
        {
            var list =
                new List<Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>>();

            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.FullyRectifiedSine,
                    (A, T, t, t1, p) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.HalfRectifiedSine,
                    (A, T, t, t1, p) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.ImpulseNoise,
                    (A, T, t, t1, p) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.KroneckerDelta,
                    (A, T, t, t1, p) =>
                        {
                            if (Math.Abs(t - t1) < double.Epsilon)
                            {
                                return A;
                            }

                            return 0;
                        },
                    new Required(true, true, false, true, false, true, false, false),
                    "Kronecker delta signal"));
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.NormalDistribution,
                    (A, T, t, t1, p) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.RandomNoise,
                    (A, T, t, t1, p) =>
                        {
                            var random = new Random();
                            var y = random.NextDouble() * (A * 2) - A;

                            return y;
                        },
                    new Required(true, true, false, true, false, true, false, false),
                    "Random noise signal"));
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.Sine,
                    (A, T, t, t1, p) => A * Math.Sin(Math.PI * 2 * (t - t1) / T),
                    new Required(true, true, true, true, false, true, true, false),
                    "Sine signal"));
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.Square,
                    (A, T, t1, kw, t) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.SymmetricalSquare,
                    (A, T, t1, kw, t) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.Triangle,
                    (A, T, t1, kw, t) =>
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
            list.Add(
                new Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>(
                    Signal.UnitStep,
                    (A, T, t1, kw, t) =>
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

            Functions = ImmutableList.CreateRange(list);
        }

        public static
            ImmutableList<Tuple<Signal, Func<double, double, double, double, double, double>, Required, string>> Functions { get; }

        public static string GetDescription(Signal signal)
        {
            return Functions.Where(a => a.Item1 == signal).Select(a => a.Item4).FirstOrDefault();
        }

        public static Func<double, double, double, double, double, double> GetFunction(Signal signal)
        {
            return Functions.Where(a => a.Item1 == signal).Select(a => a.Item2).FirstOrDefault();
        }

        public static Required GetRequiredParameters(Signal signal)
        {
            return Functions.Where(a => a.Item1 == signal).Select(a => a.Item3).FirstOrDefault();
        }

        public static Signal GetTypeByDescription(string description)
        {
            return Functions.Where(f => f.Item4 == description).Select(f => f.Item1).FirstOrDefault();
        }
    }
}