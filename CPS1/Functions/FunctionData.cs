namespace CPS1.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            this.Amplitude = amplitude;
            this.Period = period;
            this.Samples = samples;
            this.StartTime = startTime;
            this.Duration = duration;
            this.DutyCycle = dutyCycle;
            this.Points = new List<Point>();

            this.RequiredAttributes = new Required(false, false, false, false, false, false);

            this.Formatter = value => value.ToString("N");
        }

        public double Amplitude { get; private set; }

        public double Duration { get; }

        public double DutyCycle { get; }

        public Func<double, string> Formatter { get; set; }

        public string[] Labels
        {
            get
            {
                return this.Points.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        public double Period { get; }

        public List<Point> Points { get; }

        public Required RequiredAttributes { get; set; }

        public int Samples { get; }

        public double StartTime { get; }

        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        public void SetAmplitude()
        {
            this.Amplitude = Math.Max(this.Points.Max(p => p.Y), Math.Abs(this.Points.Min(p => p.Y)));
        }
    }
}