using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CPS1.Converters;
using CPS1.Model.Generation;
using CPS1.Model.Parameters;
using CPS1.Properties;
using LiveCharts;

namespace CPS1.Model.SignalData
{
    [Serializable]
    [DataContract]
    public class FunctionData : INotifyPropertyChanged, IParametersProvider
    {
        [NonSerialized] private Func<FunctionData, double, double> function;

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
            Amplitude = new FunctionAttribute<double>(
                amplitude,
                false,
                Settings.Default.AmplitudeMin,
                Settings.Default.AmplitudeMax,
                "AMPLITUDE");
            Period = new FunctionAttribute<double>(
                period,
                false,
                Settings.Default.PeriodMin,
                Settings.Default.PeriodMax,
                "PERIOD");
            Frequency = new FunctionAttribute<double>(
                1.0d / period,
                false,
                1.0d / Settings.Default.PeriodMax,
                1.0d / Settings.Default.PeriodMin,
                "FREQUENCY");
            Samples = new FunctionAttribute<int>(
                samples,
                false,
                Settings.Default.SamplesMin,
                Settings.Default.SamplesMax,
                "NUMBER OF SAMPLES");
            StartTime = new FunctionAttribute<double>(
                startTime,
                false,
                Settings.Default.StartTimeMin,
                Settings.Default.StartTimeMax,
                "STARTING TIME");
            Duration = new FunctionAttribute<double>(
                duration,
                false,
                Settings.Default.DurationMin,
                Settings.Default.DurationMax,
                "DURATION");
            DutyCycle = new FunctionAttribute<double>(
                dutyCycle,
                false,
                Settings.Default.DutyCycleMin,
                Settings.Default.DutyCycleMax,
                "DUTY CYCLE");
            HistogramIntervals = new FunctionAttribute<int>(
                histogramIntervals,
                true,
                Settings.Default.HistogramIntervalMin,
                Settings.Default.HistogramIntervalMax,
                "NUMBER OF INTERVALS");
            Probability = new FunctionAttribute<double>(probability, false, 0, 1, "PROBABILITY");
            Continuous = new FunctionAttribute<bool>(true, false, false, true, "CONTINUITY");
            AverageValue = new Parameter(0, "AVERAGE VALUE");
            AbsoluteAverageValue = new Parameter(0, "ABSOLUTE AVERAGE VALUE");
            AveragePower = new Parameter(0, "AVERAGE POWER");
            Variance = new Parameter(0, "VARIANCE");
            RootMeanSquare = new Parameter(0, "ROOT MEAN SQUARE");

            Parameters = new List<Parameter>(
                new[]
                {
                    AverageValue, AbsoluteAverageValue, AveragePower, Variance,
                    RootMeanSquare
                });

            Attributes = new List<object>(
                new object[]
                {
                    Amplitude, Period,
                    Frequency, Samples,
                    StartTime, Duration,
                    DutyCycle, Probability
                });
            HistogramAttributes =
                new List<object>(new object[] {HistogramIntervals});

            Points = new List<Point>();
            HistogramPoints = new List<Point>();


            AttributesBinding.BindAttributesTwoWay(Frequency, Period, new FrequencyPeriodConverter());
            RequiredAttributes = new Required(false, false, false, false, false, false, false, false);

            Continuous.Value = continuous;
            Type = type;
        }

        [DataMember] public Parameter AbsoluteAverageValue { get; set; }

        [DataMember] public FunctionAttribute<double> Amplitude { get; set; }

        public IEnumerable<object> Attributes { get; }

        [DataMember] public Parameter AveragePower { get; set; }

        [DataMember] public Parameter AverageValue { get; set; }

        [DataMember] public FunctionAttribute<bool> Continuous { get; set; }

        public FunctionData Copy
        {
            get
            {
                var fd = new FunctionData();
                fd.AssignSignal(this);
                return fd;
            }
        }

        [DataMember] public FunctionAttribute<double> Duration { get; set; }

        [DataMember] public FunctionAttribute<double> DutyCycle { get; set; }

        [IgnoreDataMember] public Func<double, string> Formatter => value => value.ToString("N");

        public FunctionAttribute<double> Frequency { get; set; }

        [IgnoreDataMember]
        public Func<FunctionData, double, double> Function
        {
            get => function;
            set => function = value;
        }

        public IEnumerable<object> HistogramAttributes { get; }

        [DataMember] public FunctionAttribute<int> HistogramIntervals { get; set; }

        [IgnoreDataMember]
        public string[] HistogramLabels
        {
            get { return HistogramPoints.Select(p => p.X.ToString("N")).ToArray(); }
        }

        [DataMember] public List<Point> HistogramPoints { get; set; }

        [IgnoreDataMember]
        public ChartValues<double> HistogramValues
        {
            get { return new ChartValues<double>(HistogramPoints.Select(p => p.Y)); }
        }

        [IgnoreDataMember]
        public string[] Labels
        {
            get { return Points.Select(p => p.X.ToString("N")).ToArray(); }
        }

        [DataMember] public FunctionAttribute<double> Period { get; set; }

        [DataMember] public List<Point> Points { get; set; }

        [DataMember] public FunctionAttribute<double> Probability { get; set; }

        [IgnoreDataMember]
        public Required RequiredAttributes
        {
            set
            {
                Amplitude.Visibility = value.Amplitude;
                Period.Visibility = value.Period;
                Samples.Visibility = value.Samples;
                StartTime.Visibility = value.StartTime;
                Duration.Visibility = value.Duration;
                DutyCycle.Visibility = value.DutyCycle;
                Samples.Visibility = value.Samples;
                Probability.Visibility = value.Probability;
                Continuous.Visibility = value.Continuous;
            }
        }

        [DataMember] public Parameter RootMeanSquare { get; set; }

        [DataMember] public FunctionAttribute<int> Samples { get; set; }

        [DataMember] public FunctionAttribute<double> StartTime { get; set; }

        [DataMember]
        public Signal Type
        {
            get => type;
            set
            {
                if (value == type)
                {
                    return;
                }

                type = value;
                if (type != Signal.Composite)
                {
                    RequiredAttributes = AvailableFunctions.GetRequiredParameters(type);
                    Function = AvailableFunctions.GetFunction(type);
                }
            }
        }

        [IgnoreDataMember]
        public ChartValues<double> Values
        {
            get { return new ChartValues<double>(Points.Select(p => p.Y)); }
        }

        [DataMember] public Parameter Variance { get; set; }

        [IgnoreDataMember]
        private double Max
        {
            get
            {
                var max = StartTime.Value + Duration.Value;
                if (Period.Visibility)
                {
                    var k = Math.Floor(Duration.Value / Period.Value);
                    max = StartTime.Value + k * Period.Value;
                }

                return max;
            }
        }

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Parameter> Parameters { get; }

        public void AssignSignal(FunctionData data)
        {
            Amplitude.AssignAttribute(data.Amplitude);
            Period.AssignAttribute(data.Period);
            Samples.AssignAttribute(data.Samples);
            StartTime.AssignAttribute(data.StartTime);
            Duration.AssignAttribute(data.Duration);
            DutyCycle.AssignAttribute(data.DutyCycle);
            Samples.AssignAttribute(data.Samples);
            Probability.AssignAttribute(data.Probability);
            Continuous.AssignAttribute(data.Continuous);
            HistogramIntervals.AssignAttribute(data.HistogramIntervals);
            AverageValue.AssingParameter(data.AverageValue);
            AbsoluteAverageValue.AssingParameter(data.AbsoluteAverageValue);
            Variance.AssingParameter(data.Variance);
            RootMeanSquare.AssingParameter(data.RootMeanSquare);
            AveragePower.AssingParameter(data.AveragePower);
            if (data.Function != null)
            {
                Function = (Func<FunctionData, double, double>) data.Function.Clone();
            }

            Points.Clear();
            Points.AddRange(data.Points);
            PointsUpdate();
            HistogramPoints.Clear();
            HistogramPoints.AddRange(data.HistogramPoints);
            HistogramPointsUpdate();
            Type = data.Type;
        }

        public void CalculateParameters()
        {
            AverageValue.Value = Points.Where(p => p.X <= Max).Select(p => p.Y).Sum()
                                 / Points.Count(p => p.X <= Max);
            AbsoluteAverageValue.Value = Points.Where(p => p.X <= Max).Select(p => Math.Abs(p.Y)).Sum()
                                         / Points.Count(p => p.X <= Max);
            AveragePower.Value = Points.Where(p => p.X <= Max).Select(p => p.Y * p.Y).Sum()
                                 / Points.Count(p => p.X <= Max);
            Variance.Value =
                Points.Where(p => p.X <= Max)
                    .Select(p => (p.X - AverageValue.Value) * (p.X - AverageValue.Value)).Sum()
                / Points.Count(p => p.X <= Max);
            RootMeanSquare.Value = Math.Sqrt(AveragePower.Value);
        }

        public void HistogramPointsUpdate()
        {
            OnPropertyChanged(nameof(HistogramValues));
            OnPropertyChanged(nameof(HistogramLabels));
        }

        public void PointsUpdate()
        {
            OnPropertyChanged(nameof(Values));
            OnPropertyChanged(nameof(Labels));
        }

        public void SetAmplitude()
        {
            Amplitude.Value =
                Math.Ceiling(Math.Max(Points.Max(p => p.Y), Math.Abs(Points.Min(p => p.Y))));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}