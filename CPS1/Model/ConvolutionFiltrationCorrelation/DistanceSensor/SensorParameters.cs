using System.Collections.Generic;
using CPS1.Model.Parameters;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.DistanceSensor
{
    public class SensorParameters
    {
        public SensorParameters(TrackedObject obj)
        {
            SensedObjectPosition = new Parameter(0.0d, "COMPUTED OBJECT POSITION");
            Parameters = new[] {obj.Position, SensedObjectPosition};
        }

        public Parameter SensedObjectPosition { get; }
        public IEnumerable<Parameter> Parameters { get; }
    }
}