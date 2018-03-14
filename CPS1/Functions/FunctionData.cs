namespace CPS1.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiveCharts;

    public class FunctionData
    {
        public FunctionData(
            double maxArgument = 100,
            double minArgument = -100,
            double maxValue = 50,
            double minValue = -50,
            double period = Math.PI,
            double startTime = 0,
            double duration = Math.PI * 10,
            double dutyCycle = Math.PI / 4,
            int samples = 500)
        {
            this.MaxArgument = maxArgument;
            this.MinArgument = minArgument;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
            this.Period = period;
            this.Samples = samples;
            this.StartTime = startTime;
            this.Duration = duration;
            this.DutyCycle = dutyCycle;
            this.Points = new List<Point>();

            this.Formatter = value => value.ToString("N");
        }

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

        public double MaxArgument { get; }

        public double MaxValue { get; private set; }

        public double MinArgument { get; }

        public double MinValue { get; }

        public double Period { get; }

        public List<Point> Points { get; }

        public int Samples { get; }

        public double StartTime { get; }

        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }

        public void SetMinMaxValue()
        {
            this.MaxValue = this.Points.Max(p => p.Y);
            this.MaxValue = this.Points.Min(p => p.Y);
        }
    }
}