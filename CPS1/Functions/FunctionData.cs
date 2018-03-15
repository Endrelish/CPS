namespace CPS1.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CPS1.Properties;

    using LiveCharts;

    public class FunctionData
    {
        public FunctionData(
            double startTime = 0,
            double amplitude = 50,
            double period = Math.PI,
            double duration = Math.PI * 10,
            double dutyCycle = Math.PI / 5,
            int samples = 500)
        {
            this.Amplitude = new FunctionAttribute<double>(amplitude, false, Settings.Default.AmplitudeMin, Settings.Default.AmplitudeMax);
            this.Period = new FunctionAttribute<double>(period, false, Settings.Default.PeriodMin, Settings.Default.PeriodMax);
            this.Samples = new FunctionAttribute<int>(samples, false, Settings.Default.SamplesMin, Settings.Default.SamplesMax);
            this.StartTime = new FunctionAttribute<double>(startTime, false, Settings.Default.StartTimeMin, Settings.Default.StartTimeMax);
            this.Duration = new FunctionAttribute<double>(
                duration,
                false,
                Settings.Default.DurationMin,
                Settings.Default.DurationMax);
            this.DutyCycle = new FunctionAttribute<double>(
                dutyCycle,
                false,
                Settings.Default.DutyCycleMin,
                Settings.Default.DutyCycleMax);
            this.Points = new List<Point>();

            this.RequiredAttributes = new Required(false, false, false, false, false, false);

            this.Formatter = value => value.ToString("N");
        }

        public FunctionAttribute<double> Amplitude { get; private set; }

        public FunctionAttribute<double> Duration { get; }

        public FunctionAttribute<double> DutyCycle { get; }

        public Func<double, string> Formatter { get; set; }

        public string[] Labels
        {
            get
            {
                return this.Points.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        public FunctionAttribute<double> Period { get; }

        public List<Point> Points { get; }

        public Required RequiredAttributes { get; set; }

        public FunctionAttribute<int> Samples { get; }

        public FunctionAttribute<double> StartTime { get; }

        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        public void SetAmplitude()
        {
            this.Amplitude.Value = Math.Max(this.Points.Max(p => p.Y), Math.Abs(this.Points.Min(p => p.Y)));
        }
    }
}