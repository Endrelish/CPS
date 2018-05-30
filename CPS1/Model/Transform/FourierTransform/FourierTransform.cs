using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform
{
    public abstract class FourierTransform : IFourierTransform
    {
        protected List<Point> transform;
        public virtual IEnumerable<Point> Transform(IEnumerable<Point> signal)
        {
            transform = new List<Point>();
            var signalList = signal.ToList();
            var N = signalList.Count();

            for (int i = 0; i < N; i++)
            {
                transform.Add(new Point(i, TransformValue(i, N, signalList)));
            }

            return transform;
        }

        protected abstract Complex TransformValue(int m, int N, List<Point> signal);
    }
}