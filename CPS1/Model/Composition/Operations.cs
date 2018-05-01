using System;
using System.Linq;
using CPS1.Model.Generation;
using CPS1.Model.SignalData;

namespace CPS1.Model.Composition
{
    public static class Operations
    {
        public static void Compose(FunctionData first, FunctionData second, Operation operation)
        {
            if (operation == Operation.Divide && second.Points.Any(p => Math.Abs(p.Y) < double.Epsilon))
            {
                first.Continuous.Value = false;
            }

            if (first.Type != Signal.Composite)
            {
                first.Function = AvailableFunctions.GetFunction(first.Type);
            }

            if (second.Type != Signal.Composite)
            {
                second.Function = AvailableFunctions.GetFunction(second.Type);
            }

            first.Type = Signal.Composite;
            if (second.Function != null && first.Function != null)
            {
                // TODO Think about a proper way of implementing this feature
                first.Continuous.Value = first.Continuous.Value && second.Continuous.Value;
                first.Function = FunctionComposer.ComposeFunction(
                    first.Function,
                    second.Function,
                    second,
                    operation);
                Generator.GenerateSignal(first);
            }
            else
            {
                SimpleCompose(first, second, operation);
                first.PointsUpdate();
                Histogram.GetHistogram(first);
            }
        }

        private static void SimpleAdd(FunctionData first, FunctionData second)
        {
            if (first.Continuous.Value && second.Continuous.Value)
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => p.X == point.X))
                    {
                        point.Y += second.Points.First(p => p.X == point.X).Y;
                    }
                    else
                    {
                        point.Y += (second.Points
                                        .First(a => a.X == second.Points.Where(p => p.X < point.X).Max(p => p.X)).Y
                                    + second.Points.First(a =>
                                            a.X == second.Points.Where(p => p.X > point.X).Min(p => p.X))
                                        .Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => p.X == point.X))
                    {
                        point.Y += second.Points.First(p => p.X == point.X).Y;
                    }
                }

                first.Points.AddRange(
                    second.Points.Where(p => !first.Points.Select(a => a.X).Contains(p.X)));
            }
        }

        private static void SimpleCompose(FunctionData first, FunctionData second, Operation operation)
        {
            switch (operation)
            {
                case Operation.Add:
                    SimpleAdd(first, second);
                    break;
                case Operation.Subtract:
                    SimpleSubtract(first, second);
                    break;
                case Operation.Multiply:
                    SimpleMultiply(first, second);
                    break;
                case Operation.Divide:
                    SimpleDivide(first, second);
                    break;
            }
        }

        private static void SimpleDivide(FunctionData first, FunctionData second)
        {
            if (first.Continuous.Value && second.Continuous.Value)
            {
                for (var i = 0; i < first.Points.Count; i++)
                {
                    var point = first.Points[i];
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            first.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        try
                        {
                            point.Y /=
                                (second.Points.First(
                                     a => Math.Abs(a.X - second.Points.Where(p => p.X < point.X).Max(p => p.X))
                                          < double.Epsilon).Y + second.Points.First(
                                     a => Math.Abs(a.X - second.Points.Where(p => p.X > point.X).Min(p => p.X))
                                          < double.Epsilon).Y) / 2.0d;
                        }
                        catch (DivideByZeroException)
                        {
                            first.Points.RemoveAt(i--);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < first.Points.Count; i++)
                {
                    var point = first.Points[i];
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            first.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        first.Points.RemoveAt(i--);
                    }
                }
            }
        }

        private static void SimpleMultiply(FunctionData first, FunctionData second)
        {
            if (first.Continuous.Value && second.Continuous.Value)
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y *=
                            (second.Points.First(
                                 a => Math.Abs(a.X - second.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + second.Points.First(
                                 a => Math.Abs(a.X - second.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y = 0;
                    }
                }
            }
        }

        private static void SimpleSubtract(FunctionData first, FunctionData second)
        {
            if (first.Continuous.Value && second.Continuous.Value)
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y -=
                            (second.Points.First(
                                 a => Math.Abs(a.X - second.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + second.Points.First(
                                 a => Math.Abs(a.X - second.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in first.Points)
                {
                    if (second.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= second.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                }

                first.Points.AddRange(
                    second.Points.Where(p => !first.Points.Select(a => a.X).Contains(p.X))
                        .Select(p => new Point(p.X, p.Y * -1)));
            }

            foreach (var point in first.Points)
            {
                if (Math.Abs(point.Y) < 10E-10)
                {
                    point.Y = 0;
                }
            }
        }
    }
}