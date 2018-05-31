using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS1.Model.Transform.FourierTransform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform.Tests
{
    [TestClass()]
    public class FourierTransformTests
    {
        [TestMethod()]
        public void TransformTest()
        {
            var r = new Random();
            var t1 = new DiscreteFourierTransform();
            var t2 = new FastFourierTransform();

            var points = new List<Point>();
            var x = 0;

            points.Add(new Point(x++, 1));
            points.Add(new Point(x++, 2));
            points.Add(new Point(x++, 3));
            points.Add(new Point(x, 1));

            var complex = points.Select(p => new Complex(p.Y, 0)).ToArray();

            var first = t1.Transform(points).ToList();
            var comp = DiscreteFourierTransform.computeDft(complex);
            var second = t2.Transform(points).ToList();
        
            for (int i = 0; i < first.Count; i++)
            {
                Assert.AreEqual(first[i], second[i]);
            }
        }
    }
}