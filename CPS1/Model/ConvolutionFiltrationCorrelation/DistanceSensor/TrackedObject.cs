using System.Collections.Generic;
using CPS1.Model.Parameters;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.DistanceSensor
{
    public class TrackedObject
    {
        public TrackedObject()
        {
            Position = new Parameter(0.0d, "POSITION");
            Velocity = new FunctionAttribute<double>(1.0d, true, 0.5d, 20.0d, "VELOCITY");
            Attributes = new List<FunctionAttribute<double>>(new []{Velocity});
        }
        public List<FunctionAttribute<double>> Attributes { get; }
        public Parameter Position { get; }
        public FunctionAttribute<double> Velocity { get; }

        public void Move(double time)
        {
            Position.Value = time * Velocity.Value;
        }
    }
}