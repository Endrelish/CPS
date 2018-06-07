using System.Collections.Generic;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public interface ITransform
    {
        IEnumerable<Point> Transform(IEnumerable<Point> signal);
    }
}