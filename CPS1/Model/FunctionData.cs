namespace CPS1.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    using CPS1.Properties;
    using CPS1.ViewModel;

    using LiveCharts;

    [Serializable]
    [DataContract]
    public class FunctionData : INotifyPropertyChanged
    {
        [NonSerialized]
        private Func<FunctionData, double, double> function;

        public FunctionData(
            double startTime = 0,
            double amplitude = 50,
            double period = 1,
            double duration = 2,
            double dutyCycle = 0.5,
            int samples = 500,
            int histogramIntervals = 10,
            double probability = 0.5)
        {
            this.Amplitude = new FunctionAttribute<double>(
                amplitude,
                false,
                Settings.Default.AmplitudeMin,
                Settings.Default.AmplitudeMax,
                "AMPLITUDE");
            this.Period = new FunctionAttribute<double>(
                period,
                false,
                Settings.Default.PeriodMin,
                Settings.Default.PeriodMax,
                "PERIOD");
            this.Samples = new FunctionAttribute<int>(
                samples,
                false,
                Settings.Default.SamplesMin,
                Settings.Default.SamplesMax,
                "NUMBER OF SAMPLES");
            this.StartTime = new FunctionAttribute<double>(
                startTime,
                false,
                Settings.Default.StartTimeMin,
                Settings.Default.StartTimeMax,
                "STARTING TIME");
            this.Duration = new FunctionAttribute<double>(
                duration,
                false,
                Settings.Default.DurationMin,
                Settings.Default.DurationMax,
                "DURATION");
            this.DutyCycle = new FunctionAttribute<double>(
                dutyCycle,
                false,
                Settings.Default.DutyCycleMin,
                Settings.Default.DutyCycleMax,
                "DUTY CYCLE");
            this.HistogramIntervals = new FunctionAttribute<int>(
                histogramIntervals,
                true,
                Settings.Default.HistogramIntervalMin,
                Settings.Default.HistogramIntervalMax,
                "NUMBER OF INTERVALS");
            this.Probability = new FunctionAttribute<double>(probability, false, 0, 1, "PROBABILITY");
            this.Continuous = new FunctionAttribute<bool>(true, false, false, true, "CONTINUITY");
            this.AverageValue = new FunctionAttribute<double>(0, true, 0, 0, "AVERAGE VALUE");
            this.AbsoluteAverageValue = new FunctionAttribute<double>(0, true, 0, 0, "ABSOLUTE AVERAGE VALUE");
            this.AveragePower = new FunctionAttribute<double>(0, true, 0, 0, "AVERAGE POWER");
            this.Variance = new FunctionAttribute<double>(0, true, 0, 0, "VARIANCE");
            this.RootMeanSquare = new FunctionAttribute<double>(0, true, 0, 0, "ROOT MEAN SQUARE");

            this.Points = new List<Point>();
            this.HistogramPoints = new List<Point>();

            this.RequiredAttributes = new Required(false, false, false, false, false, false, false, false);
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public FunctionAttribute<double> AbsoluteAverageValue { get; set; }

        [DataMember]
        public FunctionAttribute<double> Amplitude { get; set; }

        [DataMember]
        public FunctionAttribute<double> AveragePower { get; set; }

        [DataMember]
        public FunctionAttribute<double> AverageValue { get; set; }

        [DataMember]
        public FunctionAttribute<bool> Continuous { get; set; }

        [DataMember]
        public FunctionAttribute<double> Duration { get; set; }

        [DataMember]
        public FunctionAttribute<double> DutyCycle { get; set; }

        [IgnoreDataMember]
        public Func<double, string> Formatter => MainViewModel.Formatter;

        [IgnoreDataMember]
        public Func<FunctionData, double, double> Function
        {
            get => this.function;
            set => this.function = value;
        }

        [DataMember]
        public FunctionAttribute<int> HistogramIntervals { get; set; }

        [IgnoreDataMember]
        public string[] HistogramLabels
        {
            get
            {
                return this.HistogramPoints.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        [DataMember]
        public List<Point> HistogramPoints { get; set; }

        [IgnoreDataMember]
        public ChartValues<double> HistogramValues
        {
            get
            {
                return new ChartValues<double>(this.HistogramPoints.Select(p => p.Y));
            }
        }

        [IgnoreDataMember]
        public string[] Labels
        {
            get
            {
                return this.Points.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        [DataMember]
        public FunctionAttribute<double> Period { get; set; }

        [DataMember]
        public List<Point> Points { get; set; }

        [DataMember]
        public FunctionAttribute<double> Probability { get; set; }

        [IgnoreDataMember]
        public Required RequiredAttributes
        {
            set
            {
                this.Amplitude.Visibility = value.Amplitude;
                this.Period.Visibility = value.Period;
                this.Samples.Visibility = value.Samples;
                this.StartTime.Visibility = value.StartTime;
                this.Duration.Visibility = value.Duration;
                this.DutyCycle.Visibility = value.DutyCycle;
                this.Samples.Visibility = value.Samples;
                this.Probability.Visibility = value.Probability;
                this.Continuous.Visibility = value.Continuous;
                this.Continuous.Value = value.Continuous;
            }
        }

        [DataMember]
        public FunctionAttribute<double> RootMeanSquare { get; set; }

        [DataMember]
        public FunctionAttribute<int> Samples { get; set; }

        [DataMember]
        public FunctionAttribute<double> StartTime { get; set; }

        [DataMember]
        public Signal Type { get; set; }

        [IgnoreDataMember]
        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        [DataMember]
        public FunctionAttribute<double> Variance { get; set; }

        [IgnoreDataMember]
        private double Max
        {
            get
            {
                var max = this.StartTime.Value + this.Duration.Value;
                if (this.Period.Visibility)
                {
                    var k = Math.Floor(this.Duration.Value / this.Period.Value);
                    max = this.StartTime.Value + k * this.Period.Value;
                }

                return max;
            }
        }

        public void AssignSignal(FunctionData data)
        {
            this.Amplitude.AssignAttribute(data.Amplitude);
            this.Period.AssignAttribute(data.Period);
            this.Samples.AssignAttribute(data.Samples);
            this.StartTime.AssignAttribute(data.StartTime);
            this.Duration.AssignAttribute(data.Duration);
            this.DutyCycle.AssignAttribute(data.DutyCycle);
            this.Samples.AssignAttribute(data.Samples);
            this.Probability.AssignAttribute(data.Probability);
            this.Continuous.AssignAttribute(data.Continuous);
            this.HistogramIntervals.AssignAttribute(data.HistogramIntervals);
            this.AverageValue.AssignAttribute(data.AverageValue);
            this.AbsoluteAverageValue.AssignAttribute(data.AbsoluteAverageValue);
            this.Variance.AssignAttribute(data.Variance);
            this.RootMeanSquare.AssignAttribute(data.RootMeanSquare);
            this.AveragePower.AssignAttribute(data.AveragePower);
            this.Function = data.Function;
            this.Points.Clear();
            this.Points.AddRange(data.Points);
            this.PointsUpdate();
            this.HistogramPoints.Clear();
            this.HistogramPoints.AddRange(data.HistogramPoints);
            this.HistogramPointsUpdate();
        }

        public void CalculateParameters()
        {
            this.AverageValue.Value = this.Points.Where(p => p.X <= this.Max).Select(p => p.Y).Sum()
                                      / this.Points.Count(p => p.X <= this.Max);
            this.AbsoluteAverageValue.Value = this.Points.Where(p => p.X <= this.Max).Select(p => Math.Abs(p.Y)).Sum()
                                              / this.Points.Count(p => p.X <= this.Max);
            this.AveragePower.Value = this.Points.Where(p => p.X <= this.Max).Select(p => p.Y * p.Y).Sum()
                                      / this.Points.Count(p => p.X <= this.Max);
            this.Variance.Value =
                this.Points.Where(p => p.X <= this.Max)
                    .Select(p => (p.X - this.AverageValue.Value) * (p.X - this.AverageValue.Value)).Sum()
                / this.Points.Count(p => p.X <= this.Max);
            this.RootMeanSquare.Value = Math.Sqrt(this.AveragePower.Value);
        }

        public void Compose(FunctionData data, Operation operation)
        {
            
            if (operation == Operation.Divide && data.Points.Any(p => Math.Abs(p.Y) < double.Epsilon))
                this.Continuous.Value = false;
            if (this.Type != Signal.Composite)
            {
                this.Function = AvailableFunctions.GetFunction(this.Type);
            }

            if (data.Type != Signal.Composite)
            {
                data.Function = AvailableFunctions.GetFunction(data.Type);
            }

            if (data.Function != null && this.Function != null) // TODO Think about a proper way of implementing this feature
            {
                this.Type = Signal.Composite;
                this.Continuous.Value = this.Continuous.Value && data.Continuous.Value;
                this.Function = FunctionComposer.ComposeFunction(this.Function, data.Function, data, operation);
                Generator.GenerateSignal(this);
            }
            else
            {
                this.SimpleCompose(data, operation);
                this.PointsUpdate();
                Histogram.GetHistogram(this);
            }
        }

        public void HistogramPointsUpdate()
        {
            this.OnPropertyChanged(nameof(this.HistogramValues));
            this.OnPropertyChanged(nameof(this.HistogramLabels));
        }

        public void PointsUpdate()
        {
            this.OnPropertyChanged(nameof(this.Values));
            this.OnPropertyChanged(nameof(this.Labels));
        }

        public void SetAmplitude()
        {
            this.Amplitude.Value = Math.Ceiling(Math.Max(this.Points.Max(p => p.Y), Math.Abs(this.Points.Min(p => p.Y))));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SimpleAdd(FunctionData data)
        {
            if (this.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => p.X == point.X))
                    {
                        point.Y += data.Points.First(p => p.X == point.X).Y;
                    }
                    else
                    {
                        point.Y += (data.Points.First(a => a.X == data.Points.Where(p => p.X < point.X).Max(p => p.X)).Y
                                    + data.Points.First(a => a.X == data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                        .Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => p.X == point.X))
                    {
                        point.Y += data.Points.First(p => p.X == point.X).Y;
                    }
                }

                this.Points.AddRange(data.Points.Where(p => !this.Points.Select(a => a.X).Contains(p.X)));
            }
        }

        private void SimpleCompose(FunctionData data, Operation operation)
        {
            switch (operation)
            {
                case Operation.Add:
                    this.SimpleAdd(data);
                    break;
                case Operation.Subtract:
                    this.SimpleSubtract(data);
                    break;
                case Operation.Multiply:
                    this.SimpleMultiply(data);
                    break;
                case Operation.Divide:
                    this.SimpleDivide(data);
                    break;
            }
        }

        private void SimpleDivide(FunctionData data)
        {
            if (this.Continuous.Value && data.Continuous.Value)
            {
                for (var i = 0; i < this.Points.Count; i++)
                {
                    var point = this.Points[i];
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            this.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        try
                        {
                            point.Y /=
                                (data.Points.First(
                                     a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                          < double.Epsilon).Y + data.Points.First(
                                     a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                          < double.Epsilon).Y) / 2.0d;
                        }
                        catch (DivideByZeroException)
                        {
                            this.Points.RemoveAt(i--);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < this.Points.Count; i++)
                {
                    var point = this.Points[i];
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            this.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        this.Points.RemoveAt(i--);
                    }
                }
            }
        }

        private void SimpleMultiply(FunctionData data)
        {
            if (this.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y *=
                            (data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y = 0;
                    }
                }
            }
        }

        private void SimpleSubtract(FunctionData data)
        {
            if (this.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y -=
                            (data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                }

                this.Points.AddRange(
                    data.Points.Where(p => !this.Points.Select(a => a.X).Contains(p.X))
                        .Select(p => new Point(p.X, p.Y * -1)));
            }

            foreach (var point in Points)
            {
                if (Math.Abs(point.Y) < 10E-10) point.Y = 0;
            }
        }
    }
}