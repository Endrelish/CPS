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

    [DataContract]
    public class FunctionData : INotifyPropertyChanged
    {
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
            this.CompositeFunctionComponents = new List<Tuple<Operation, FunctionData>>();
            this.CompositeFunctionComponents.Add(new Tuple<Operation, FunctionData>(Operation.Add, this));

            this.RequiredAttributes = new Required(false, false, false, false, false, false, false, false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FunctionAttribute<double> AbsoluteAverageValue { get; set; }

        [DataMember]
        public FunctionAttribute<double> Amplitude { get; set; }

        public FunctionAttribute<double> AveragePower { get; set; }

        public FunctionAttribute<double> AverageValue { get; set; }

        public List<Tuple<Operation, FunctionData>> CompositeFunctionComponents { get; }

        [DataMember]
        public FunctionAttribute<bool> Continuous { get; set; }

        [DataMember]
        public FunctionAttribute<double> Duration { get; set; }

        [DataMember]
        public FunctionAttribute<double> DutyCycle { get; set; }

        [IgnoreDataMember]
        public Func<double, string> Formatter => MainViewModel.Formatter;

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
            }
        }

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

        public FunctionAttribute<double> Variance { get; set; }

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
            // TODO Change signals composing. Right now when composing already composed signal, the operation only applies to the first component, the rest is composed based on their previous operations. This is an unwanted situation, because the rest of the operations need to change accordingly.
            this.Continuous.Value = this.Continuous.Value && data.Continuous.Value;

            // this.Type = Signal.Composite;
            var comp = new Tuple<Operation, FunctionData>(operation, data.CompositeFunctionComponents[0].Item2);
            this.CompositeFunctionComponents.Add(comp);
            for (var i = 1; i < data.CompositeFunctionComponents.Count; i++)
            {
                this.CompositeFunctionComponents.Add(data.CompositeFunctionComponents[i]);
            }

            Generator.GenerateSignal(this, this.Type, this.CompositeFunctionComponents);

            this.PointsUpdate();
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
            this.Amplitude.Value = Math.Max(this.Points.Max(p => p.Y), Math.Abs(this.Points.Min(p => p.Y)));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}