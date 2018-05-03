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
        private CommandHandler correlationCommand;
        private CommandHandler startSimulationCommand;

        public DistanceSensorViewModel()
        {
            SentSignalData = new SensorSignal();
            SentSignal = new ChartValues<ObservableValue>();
            ReceivedSignal = new ChartValues<ObservableValue>();

            Object = new TrackedObject();
        }

        public SensorSignal SentSignalData { get; }
        public ChartValues<ObservableValue> ReceivedSignal { get; }
        public ChartValues<ObservableValue> SentSignal { get; set; }
        public ChartValues<ObservableValue> CorrelationData { get; }
        public TrackedObject Object { get; }

        private double Signal(double t)
        {
            //return 0;
            return 50.0d * Math.Sin(2.0d * Math.PI * t / SentSignalData.FirstPeriod.Value) +
                   50.0d * Math.Sin(2.0d * Math.PI * t / SentSignalData.SecondPeriod.Value);
        }

        public CommandHandler CorrelationCommand =>
            correlationCommand ?? (correlationCommand = new CommandHandler(Correlate, () => true));
        public CommandHandler StartSimulationCommand =>
            startSimulationCommand ?? (startSimulationCommand = new CommandHandler(Simulation, () => true));

        private void Simulation(object arg)
        {
            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;
            for (int i = -SentSignalData.NumberOfSamples.Value+1; i <= 0; i++)
            {
                SentSignal.Add(new ObservableValue(Signal(i * samplingPeriod)));
            }

            Thread sentSimulation = new Thread(SentSignalSimulation);
            sentSimulation.Start();
        }

        private void SentSignalSimulation()
        {
            var samplingPeriod = 1.0d / SentSignalData.SamplingFrequency.Value;
            var currentTime = 0.0d;
            while (true)
            {
                Thread.Sleep((int) (samplingPeriod * 1000));
                Thread.Sleep(100);
                currentTime += samplingPeriod;
                var y = Signal(currentTime);
                if (Application.Current == null) break;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SentSignal.Add(new ObservableValue(y));
                    SentSignal.RemoveAt(0);
                });
            }
        }

        public ChartValues<ObservableValue> Values { get; set; }

        private void Correlate(object obj)
        {
            CorrelationData.Clear();
            CorrelationData.AddRange(Correlation.Correlate(SentSignal.Select(y => new Point(0, y.Value)),
                ReceivedSignal.Select(y => new Point(0, y.Value)))
                .Select(p => new ObservableValue(p.Y)));
        }
    }
}