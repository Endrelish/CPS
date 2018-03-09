namespace CPS1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LiveCharts;
    using LiveCharts.Wpf;

    public class FunctionData
    {
        public FunctionData(
            double maxArgument = 100,
            double minArgument = -100,
            double maxValue = 50,
            double minValue = -50,
            double period = Math.PI,
            int samples = 500)
        {
            this.MaxArgument = maxArgument;
            this.MinArgument = minArgument;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
            this.Period = period;
            this.Samples = samples;
            this.Points = new List<Point>();

            this.Formatter = value => value.ToString("N");
        }

        public Func<double, string> Formatter { get; set; }

        public string[] Labels
        {
            get
            {
                return this.Points.Select(p => p.X.ToString("N")).ToArray();
            }
        }

        public double MaxArgument { get; }

        public double MaxValue { get; }

        public double MinArgument { get; }

        public double MinValue { get; }

        public double Period { get; }

        public List<Point> Points { get; }

        public ChartValues<double> Values
        {
            get
            {
                return new ChartValues<double>(this.Points.Select(p => p.Y));
            }
        }
        public int Samples { get; }
        
    }
}