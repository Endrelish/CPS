namespace CPS1.Model.SignalData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Windows.Data;

    using CPS1.Converters;
    using CPS1.Model.Generation;
    using CPS1.Model.Parameters;
    using CPS1.Properties;

    using LiveCharts;

    [Serializable]
    [DataContract]
    public class FunctionData : INotifyPropertyChanged, IParametersProvider
    {
        [NonSerialized]
        private Func<FunctionData, double, double> function;

        private Signal type;

        public FunctionData(
            double startTime = 0,
            double amplitude = 50,
            double period = 1,
            double duration = 2,
            double dutyCycle = 0.5,
            int samples = 500,
            int histogramIntervals = 10,
            double probability = 0.5,
            bool continuous = true,
            Signal type = Signal.Sine)
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
            this.Period = new FunctionAttribute<double>(
                period,
                false,
                1.0d / Settings.Default.PeriodMax,
                1.0d / Settings.Default.PeriodMin,
                "FREQUENCY");
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
            this.AverageValue = new Parameter(0, "AVERAGE VALUE");
            this.AbsoluteAverageValue = new Parameter(0, "ABSOLUTE AVERAGE VALUE");
            this.AveragePower = new Parameter(0, "AVERAGE POWER");
            this.Variance = new Parameter(0, "VARIANCE");
            this.RootMeanSquare = new Parameter(0, "ROOT MEAN SQUARE");

            this.Parameters = new List<Parameter>(
                new[]
                    {
                        this.AverageValue, this.AbsoluteAverageValue, this.AveragePower, this.Variance,
                        this.RootMeanSquare
                    });

            this.Attributes = new List<IFunctionAttribute>(
                new IFunctionAttribute[]
                    {
                        (IFunctionAttribute)Amplitude,
                        (IFunctionAttribute)Period,
                        (IFunctionAttribute)Frequency,
                        (IFunctionAttribute)Samples,
                        (IFunctionAttribute)StartTime,
                        (IFunctionAttribute)Duration,
                        (IFunctionAttribute)DutyCycle,
                        (IFunctionAttribute)Probability,
                        (IFunctionAttribute)Continuous,
                    });
            this.HistogramAttributes = new List<IFunctionAttribute>(new IFunctionAttribute[] { (IFunctionAttribute)this.HistogramIntervals });

            this.Points = new List<Point>();
            this.HistogramPoints = new List<Point>();

            this.RequiredAttributes = new Required(false, false, false, false, false, false, false, false);
            this.Continuous.Value = continuous;
            this.Type = type;

            this.Period.PropertyChanged += (sender, args) =>
                {
                    if (sender.Equals(Period.Value))
                    {
                        this.Frequency.Value = 1.0d / this.Period.Value;
                        return;
                    }
                    if (sender.Equals(Period.Visibility))
                    {
                        this.Frequency.Visibility = this.Period.Visibility;
                        return;
                    }
                    if (sender.Equals(Period.MaxValue))
                    {
                        this.Frequency.MinValue = 1.0d / this.Period.MaxValue;
                        return;
                    }
                    if (sender.Equals(Period.MinValue))
                    {
                        this.Frequency.MaxValue = 1.0d / this.Period.MinValue;
                        return;
                    }
                };

            this.Frequency.PropertyChanged += (sender, args) =>
                {
                    if (sender.Equals(Frequency.Value))
                    {
                        this.Period.Value = 1.0d / this.Frequency.Value;
                        return;
                    }
                    if (sender.Equals(Frequency.Visibility))
                    {
                        this.Period.Visibility = this.Frequency.Visibility;
                        return;
                    }
                    if (sender.Equals(Frequency.MaxValue))
                    {
                        this.Period.MinValue = 1.0d / this.Frequency.MaxValue;
                        return;
                    }
                    if (sender.Equals(Frequency.MinValue))
                    {
                        this.Period.MaxValue = 1.0d / this.Frequency.MinValue;
                        return;
                    }
                };
        }

        private void BindAttributesOneWay<T>(FunctionAttribute<T> source, FunctionAttribute<T> target, IValueConverter converter = null) where T : struct
        {
            source.PropertyChanged += (sender, args) =>
                {
                    if (sender.Equals(source.Value))
                    {
                        target.Value = 1.0d / source.Value;
                        return;
                    }
                    if (sender.Equals(source.Visibility))
                    {
                        target.Visibility = source.Visibility;
                        return;
                    }
                    if (sender.Equals(source.MaxValue))
                    {
                        target.MinValue = 1.0d / source.MaxValue;
                        return;
                    }
                    if (sender.Equals(source.MinValue))
                    {
                        target.MaxValue = 1.0d / source.MinValue;
                        return;
                    }
                };
        }

        public FunctionAttribute<double> Frequency { get; set; }
        public List<IFunctionAttribute> HistogramAttributes { get; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public Parameter AbsoluteAverageValue { get; set; }

        [DataMember]
        public FunctionAttribute<double> Amplitude { get; set; }

        [DataMember]
        public Parameter AveragePower { get; set; }

        [DataMember]
        public Parameter AverageValue { get; set; }

        [DataMember]
        public FunctionAttribute<bool> Continuous { get; set; }

        public FunctionData Copy
        {
            get
            {
                var fd = new FunctionData();
                fd.AssignSignal(this);
                return fd;
            }
        }

        [DataMember]
        public FunctionAttribute<double> Duration { get; set; }

        [DataMember]
        public FunctionAttribute<double> DutyCycle { get; set; }

        [IgnoreDataMember]
        public Func<double, string> Formatter => value => value.ToString("N");

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

        public IEnumerable<Parameter> Parameters { get; }

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
        public Parameter RootMeanSquare { get; set; }

        [DataMember]
        public FunctionAttribute<int> Samples { get; set; }

        [DataMember]
        public FunctionAttribute<double> StartTime { get; set; }

        [DataMember]
        public Signal Type
        {
            get => this.type;
            set
            {
                if (value == this.type)
                {
                    return;
                }

                this.type = value;
                if (this.type != Signal.Composite)
                {
                    this.RequiredAttributes = AvailableFunctions.GetRequiredParameters(this.type);
                }
            }
        }

        [IgnoreDataMember]
        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        [DataMember]
        public Parameter Variance { get; set; }

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
            this.AverageValue.AssingParameter(data.AverageValue);
            this.AbsoluteAverageValue.AssingParameter(data.AbsoluteAverageValue);
            this.Variance.AssingParameter(data.Variance);
            this.RootMeanSquare.AssingParameter(data.RootMeanSquare);
            this.AveragePower.AssingParameter(data.AveragePower);
            if (data.Function != null)
            {
                this.Function = (Func<FunctionData, double, double>)data.Function.Clone();
            }

            this.Points.Clear();
            this.Points.AddRange(data.Points);
            this.PointsUpdate();
            this.HistogramPoints.Clear();
            this.HistogramPoints.AddRange(data.HistogramPoints);
            this.HistogramPointsUpdate();
            this.Type = data.Type;
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
            this.Amplitude.Value =
                Math.Ceiling(Math.Max(this.Points.Max(p => p.Y), Math.Abs(this.Points.Min(p => p.Y))));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IFunctionAttribute> Attributes { get; }
    }
}