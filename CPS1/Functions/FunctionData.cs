namespace CPS1.Functions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Data;

    using CPS1.Annotations;
    using CPS1.Properties;

    using LiveCharts;

    [Serializable]
    public class FunctionData : INotifyPropertyChanged
    {
        public FunctionData(
            double startTime = 0,
            double amplitude = 50,
            double period = Math.PI,
            double duration = Math.PI * 10,
            double dutyCycle = Math.PI / 5,
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
            this.Points = new List<Point>();
            this.HistogramPoints = new List<Point>();

            this.RequiredAttributes = new Required(false, false, false, false, false, false, false, false);

            this.Formatter = value => value.ToString("N");

            this.Duration.PropertyChanged += (sender, args) =>
                {
                    this.DurationInterval = this.Duration.Value / 100.0d;
                };
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FunctionAttribute<double> Amplitude { get; }

        public FunctionAttribute<bool> Continuous { get; set; }

        public FunctionAttribute<double> Duration { get; }

        public double DurationInterval { get; private set; }

        public FunctionAttribute<double> DutyCycle { get; }

        public Func<double, string> Formatter { get; }

        public FunctionAttribute<int> HistogramIntervals { get; }

        public string[] HistogramLabels
        {
            get
            {
                return this.HistogramPoints.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        public List<Point> HistogramPoints { get; }

        public ChartValues<double> HistogramValues
        {
            get
            {
                return new ChartValues<double>(this.HistogramPoints.Select(p => p.Y));
            }
        }

        public string[] Labels
        {
            get
            {
                return this.Points.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        public FunctionAttribute<double> Period { get; }

        public List<Point> Points { get; }

        public FunctionAttribute<double> Probability { get; }

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

        public FunctionAttribute<int> Samples { get; }

        public FunctionAttribute<double> StartTime { get; }
        
        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        private Func<double, double, double, double, double, double, double> Function { get; set; }

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