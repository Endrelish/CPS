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
        
        private Thread simulationThread;

        public DistanceSensorViewModel()
        {
            SentSignal = new ChartValues<ObservableValue>();
            ReceivedSignal = new ChartValues<ObservableValue>();
            CorrelationData = new ChartValues<ObservableValue>();
            SentSignalData = new SensorSignal();
            Object = new TrackedObject();
            SensorParameters = new SensorParameters(this.Object);
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
        }
        
        public CommandHandler StartSimulationCommand =>
            startSimulationCommand ?? (startSimulationCommand = new CommandHandler(Simulation, () => true));

        private void Simulation(object arg)
        {
            SentSignal.Clear();
            ReceivedSignal.Clear();
            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;

            for (int i = 0; i < SentSignalData.NumberOfSamples.Value; i++)
            {
                SentSignal.Add(new ObservableValue(Signal(i * samplingPeriod)));
                ReceivedSignal.Add(new ObservableValue(Signal(i * samplingPeriod)));
            }

            CorrelationData.AddRange(Correlation
                .Correlate(SentSignal.Select(p => new Point(0, p.Value)),
                    ReceivedSignal.Select(p => new Point(0, p.Value))).Select(p => new ObservableValue(p.Y)));

            simulationThread = new Thread(RunSimulation);
            simulationThread.Start();
        }

        private int CorrelationDistance(IEnumerable<Point> correlation)
        {
            var list = new List<double>(correlation.Select(p => p.Y));

            var max = list.Take(list.Count / 2).Max();
            return list.Count / 2 - list.IndexOf(max);
        }

        private void RunSimulation()
        {
            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;
            var currentTime = 0.0d;
            var coefficient = SentSignalData.SignalSpeed.Value / SentSignalData.SamplingFrequency.Value / 2.0d;
            while (true)
            {
                Thread.Sleep((int)SentSignalData.ReportPeriod.Value * 1000);
                currentTime += SentSignalData.ReportPeriod.Value;
                Object.Position.Value += Object.Velocity.Value * SentSignalData.ReportPeriod.Value;
                var delay = 2.0d * Object.Position.Value / SentSignalData.SignalSpeed.Value;
                var second = new List<double>();

                for (int i = 0; i < SentSignalData.NumberOfSamples.Value; i++)
                {
                    second.Add(Signal(i * samplingPeriod - delay));
                }

                var correlation = Correlation.Correlate(SentSignal.Select(s => new Point(0, s.Value)),
                    second.Select(s => new Point(0, s))).ToList();
                var distance = CorrelationDistance(correlation);


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
                SensorParameters.SensedObjectPosition.Value = distance * samplingPeriod * SentSignalData.SignalSpeed.Value / 2.0d;
            }
        }
    }
}