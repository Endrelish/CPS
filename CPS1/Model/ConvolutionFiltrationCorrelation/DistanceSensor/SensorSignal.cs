using System.Collections.Generic;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.DistanceSensor
{
    public class SensorSignal
    {
        public SensorSignal(double startingTime = 0)
        {
            FirstPeriod = new FunctionAttribute<double>(10.1d, true, 1.0d, 100.0d, "PERIOD OF FIRST COMPONENT");
            SecondPeriod = new FunctionAttribute<double>(7.3d, true, 1.0d, 100.0d, "PERIOD OF SECOND COMPONENT");
            SamplingFrequency = new FunctionAttribute<double>(0.5d, true, 0.5d, 50.0d, "SAMPLING FREQUENCY");
            NumberOfSamples = new FunctionAttribute<int>(100, true, 50, 300, "NUMBER OF SAMPLES");
            SignalSpeed = new FunctionAttribute<double>(100.0d, true, 10.0d, 5000.0d, "SPEED OF SIGNAL");
            ReportPeriod = new FunctionAttribute<double>(0.5d, true, 0.2d, 5.0d, "SENSOR REPORTING PERIOD");

            Attributes = new List<object>(new object[] {FirstPeriod, SecondPeriod, SamplingFrequency, NumberOfSamples, SignalSpeed});
        }
        
        public List<object> Attributes { get; }
        public FunctionAttribute<double> FirstPeriod { get; }
        public FunctionAttribute<double> SecondPeriod { get; }
        public FunctionAttribute<double> SamplingFrequency { get; }
        public FunctionAttribute<int> NumberOfSamples { get; }
        public FunctionAttribute<double> SignalSpeed { get; }
        public FunctionAttribute<double> ReportPeriod { get; }
    }
}