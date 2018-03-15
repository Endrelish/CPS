namespace CPS1
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Windows.Input;

    using CPS1.Functions;

    public class MainViewModel
    {
        private Signal firstSignalType = Signal.Sine;

        private ICommand generateSignalCommand;

        private ICommand saveCommand;

        private Signal secondSignalType = Signal.Sine;

        public MainViewModel()
        {
            this.SignalFirst = new FunctionData();
            this.SignalSecond = new FunctionData();

            var signals = new List<Tuple<Signal, string>>();
            signals.Add(new Tuple<Signal, string>(Signal.FullyRectifiedSine, "Fully rectified sine signal"));
            signals.Add(new Tuple<Signal, string>(Signal.NormalDistribution, "Gaussian distribution signal"));
            signals.Add(new Tuple<Signal, string>(Signal.HalfRectifiedSine, "Half rectified sine signal"));
            signals.Add(new Tuple<Signal, string>(Signal.RandomNoise, "Random noise signal"));
            signals.Add(new Tuple<Signal, string>(Signal.Sine, "Sine signal"));
            signals.Add(new Tuple<Signal, string>(Signal.Square, "Square signal"));
            signals.Add(new Tuple<Signal, string>(Signal.SymmetricalSquare, "Symmetrical square signal"));
            signals.Add(new Tuple<Signal, string>(Signal.Triangle, "Triangle signal"));

            this.AvailableSignals = ImmutableList.CreateRange(signals);

            this.SetRequiredParameters(this.SignalFirst);
            this.SetRequiredParameters(this.SignalSecond);
        }

        public ImmutableList<Tuple<Signal, string>> AvailableSignals { get; }

        public string FirstSignalType
        {
            get => this.AvailableSignals.Where(s => s.Item1.Equals(this.firstSignalType)).Select(s => s.Item2)
                .FirstOrDefault();
            set
            {
                this.firstSignalType = this.AvailableSignals.Where(s => s.Item2.Equals(value)).Select(s => s.Item1)
                    .FirstOrDefault();
                this.SetRequiredParameters(this.SignalFirst);
            }
        }

        public ICommand GenerateSignalCommand => this.generateSignalCommand
                                                 ?? (this.generateSignalCommand = new CommandHandler(
                                                         this.GenerateSignal,
                                                         () => true));

        public ICommand SaveCommand =>
            this.saveCommand ?? (this.saveCommand = new CommandHandler(this.SaveSignal, () => true));

        public string SecondSignalType
        {
            get => this.AvailableSignals.Where(s => s.Item1.Equals(this.secondSignalType)).Select(s => s.Item2)
                .FirstOrDefault();
            set
            {
                this.secondSignalType = this.AvailableSignals.Where(s => s.Item2.Equals(value)).Select(s => s.Item1)
                    .FirstOrDefault();
                this.SetRequiredParameters(this.SignalSecond);
            }
        }

        public FunctionData SignalFirst { get; }

        public FunctionData SignalSecond { get; }

        public IEnumerable<string> SignalsLabels
        {
            get
            {
                return this.AvailableSignals.Select(p => p.Item2);
            }
        }

        private void GenerateSignal(object parameter)
        {
            if (parameter is short chart)
            {
                if (chart == 1)
                {
                    Generator.GenerateSignal(this.SignalFirst, this.firstSignalType);
                    Histogram.GetHistogram(this.SignalFirst);
                }
                else if (chart == 2)
                {
                    Generator.GenerateSignal(this.SignalSecond, this.secondSignalType);
                    Histogram.GetHistogram(this.SignalSecond);
                }
            }
        }

        private void SaveSignal(object parameter)
        {
            if (parameter is short chart)
            {
                if (chart == 1)
                {
                    Writer.Write(this.SignalFirst.Points, "chart1.txt");
                }
                else if (chart == 2)
                {
                    Writer.Write(this.SignalSecond.Points, "chart2.txt");
                }
            }
        }

        private void SetRequiredParameters(FunctionData signal)
        {
            var choice = this.firstSignalType;
            if (signal == this.SignalSecond)
            {
                choice = this.secondSignalType;
            }

            switch (choice)
            {
                case Signal.FullyRectifiedSine:
                    signal.RequiredAttributes = FullyRectifiedSineWave.RequiredAttributes;
                    break;
                case Signal.HalfRectifiedSine:
                    signal.RequiredAttributes = HalfRectifiedSineWave.RequiredAttributes;
                    break;
                case Signal.NormalDistribution:
                    signal.RequiredAttributes = NormalDistributionWave.RequiredAttributes;
                    break;
                case Signal.RandomNoise:
                    signal.RequiredAttributes = RandomNoiseWave.RequiredAttributes;
                    break;
                case Signal.Sine:
                    signal.RequiredAttributes = SineWave.RequiredAttributes;
                    break;
                case Signal.Square:
                    signal.RequiredAttributes = SquareWave.RequiredAttributes;
                    break;
                case Signal.SymmetricalSquare:
                    signal.RequiredAttributes = SymmetricalSquareWave.RequiredAttributes;
                    break;
                case Signal.Triangle:
                    signal.RequiredAttributes = TriangleWave.RequiredAttributes;
                    break;
            }
        }
    }
}