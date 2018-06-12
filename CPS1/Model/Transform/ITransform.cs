using System.Collections.Generic;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public interface ITransform
    {
        string Name { get; }
        IEnumerable<Point> Transform(Point [] signal);
    }
}