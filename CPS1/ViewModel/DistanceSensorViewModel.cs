using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation;
using CPS1.Model.ConvolutionFiltrationCorrelation.DistanceSensor;
using CPS1.Model.Generation;
using CPS1.Model.SignalData;
using LiveCharts;
using LiveCharts.Defaults;
using Point = CPS1.Model.SignalData.Point;
using Timer = System.Timers.Timer;

namespace CPS1.ViewModel
{
    public class DistanceSensorViewModel
    {
        private CommandHandler startSimulationCommand;
        private CommandHandler testCommand;

        private FunctionData firstComponent;
        private FunctionData secondComponent;
        public CommandHandler TestCommand => testCommand ?? (testCommand = new CommandHandler(Test, () => true));

        private void Test(object obj)
        {
            firstComponent.Period.Value = SentSignalData.FirstPeriod.Value;
            secondComponent.Period.Value = SentSignalData.SecondPeriod.Value;
            SentSignal.Clear();
            ReceivedSignal.Clear();
            this.Object.Position.Value += Object.Velocity.Value * SentSignalData.ReportPeriod.Value;

            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;
            var delay = 2.0d * Object.Position.Value / SentSignalData.SignalSpeed.Value;

            for (int i = 0; i < SentSignalData.NumberOfSamples.Value; i++)
            {
                SentSignal.Add(new ObservableValue(Signal(i * samplingPeriod)));
                ReceivedSignal.Add(new ObservableValue(Signal(i * samplingPeriod - delay)));
            }

            var correlation = Correlation.Correlate(SentSignal.Select(s => new Point(0, s.Value)),
                ReceivedSignal.Select(s => new Point(0, s.Value)));
            var distance = CorrelationDistance(correlation);
            this.CorrelationData.Clear();
            this.CorrelationData.AddRange(correlation.Select(p => new ObservableValue(p.Y)));

            var a = SentSignalData.NumberOfSamples.Value / (double) (SentSignalData.NumberOfSamples.Value * 2 - 1);
            var b = samplingPeriod;
            var c = SentSignalData.SignalSpeed.Value;


            SensorParameters.SensedObjectPosition.Value = distance * b * c / 2.0d;
        }

        private Thread simulationThread;

        public DistanceSensorViewModel()
        {
            SentSignal = new ChartValues<ObservableValue>();
            ReceivedSignal = new ChartValues<ObservableValue>();
            CorrelationData = new ChartValues<ObservableValue>();
            SentSignalData = new SensorSignal();
            Object = new TrackedObject();
            SensorParameters = new SensorParameters(this.Object);

            firstComponent = new FunctionData();
            secondComponent = new FunctionData();
            firstComponent.Type = Model.Generation.Signal.Square;
            secondComponent.Type = Model.Generation.Signal.Square;
            firstComponent.DutyCycle.Value = 0.001d;
            secondComponent.DutyCycle.Value = 0.001d;
        }
        public SensorParameters SensorParameters { get; }
        public SensorSignal SentSignalData { get; }
        public ChartValues<ObservableValue> ReceivedSignal { get; }
        public ChartValues<ObservableValue> SentSignal { get; }
        public ChartValues<ObservableValue> CorrelationData { get; }
        public TrackedObject Object { get; }

        private double Signal(double t)
        {

            return 50.0d * Math.Sin(2.0d * Math.PI * t / SentSignalData.FirstPeriod.Value) +
                   50.0d * Math.Sin(2.0d * Math.PI * t / SentSignalData.SecondPeriod.Value);
            //return firstComponent.Function(firstComponent, t) + secondComponent.Function(secondComponent, t);
        }
        
        public CommandHandler StartSimulationCommand =>
            startSimulationCommand ?? (startSimulationCommand = new CommandHandler(Simulation, () => true));

        private void Simulation(object arg)
        {

            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;

            for (int i = -SentSignalData.NumberOfSamples.Value+1; i <= 0; i++)
            {
                var y = Signal(i * samplingPeriod);
                SentSignal.Add(new ObservableValue(y));
                ReceivedSignal.Add(new ObservableValue(y));
            }

            CorrelationData.AddRange(Correlation.Correlate(SentSignal.Select(s => new Point(0, s.Value)),
                ReceivedSignal.Select(s => new Point(0, s.Value))).Select(p => new ObservableValue(p.Y)));
            simulationThread = new Thread(RunSimulation);
            simulationThread.Start();
        }

        private int CorrelationDistance(IEnumerable<Point> correlation)
        {
            var list = new List<double>(correlation.Select(p => p.Y));
            var firstHalf = list.Take(list.Count / 2 - 1);
            var secondHalf = list.Skip(list.Count / 2);

            var firstMax = firstHalf.Max();
            var secondMax = secondHalf.Max();
            return list.Count / 2 - list.IndexOf(firstMax);
            return list.IndexOf(secondMax) - list.Count / 2;
        }

        private void RunSimulation()
        {
            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;
            var interval = (int)SentSignalData.ReportPeriod.Value * 1000;
            var currentTime = 0.0d;
            var coefficient = SentSignalData.NumberOfSamples.Value /
                              (double)(SentSignalData.NumberOfSamples.Value * 2 - 1) * SentSignalData.SignalSpeed.Value / SentSignalData.SamplingFrequency.Value / 2.0d;
            while (true)
            {
                Thread.Sleep(interval);
                currentTime += interval;
                Object.Position.Value += Object.Velocity.Value * interval / 1000.0d;
                var delay = 2.0d * Object.Position.Value / SentSignalData.SignalSpeed.Value;
                var second = new List<double>();

                for (int i = 0; i < SentSignalData.NumberOfSamples.Value; i++)
                {
                    second.Add(Signal(i * samplingPeriod - delay));
                }

                var correlation = Correlation.Correlate(SentSignal.Select(s => new Point(0, s.Value)),
                    second.Select(s => new Point(0, s))).ToList();
                
                var distance = CorrelationDistance(correlation);

                SensorParameters.SensedObjectPosition.Value = currentTime == 0 ? 0 : distance * coefficient;

                if (Application.Current == null) break;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    int i = 0;
                    foreach (var observableValue in ReceivedSignal)
                    {
                        observableValue.Value = second[i++];
                    }

                    i = 0;
                    foreach (var observableValue in CorrelationData)
                    {
                        observableValue.Value = correlation[i++].Y;
                    }
                });

            }
        }
    }
}