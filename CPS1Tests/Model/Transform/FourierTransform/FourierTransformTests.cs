using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS1.Model.Transform.FourierTransform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CPS1.Model.Generation;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.FourierTransform.Tests
{
    [TestClass()]
    public class FourierTransformTests
    {
        [TestMethod()]
        public void SineSignalTransform()
        {
            var data = new FunctionData(samples:256);
            Generator.GenerateSignal(data);

            var fft = new FastFourierTransform();
            var transform = fft.Transform(data.Points.ToArray());
        }
        [TestMethod()]
        public void TransformTest()
        {
            var r = new Random();
            var t1 = new DiscreteFourierTransform();
            var t2 = new FastFourierTransform();

            var points = new List<Point>();
            var points2 = new List<Point>();
            var x = 0;

            points.Add(new Point(x++, 1));
            points.Add(new Point(x++, 2));
            points.Add(new Point(x++, 3));
            points.Add(new Point(x, 1));
            x = 0;
            points2.Add(new Point(x, 1));
            points2.Add(new Point(x, 1));
            points2.Add(new Point(x, 1));
            points2.Add(new Point(x, 1));
            points2.Add(new Point(x, 0));
            points2.Add(new Point(x, 0));
            points2.Add(new Point(x, 0));
            points2.Add(new Point(x, 0));
            

            var first = t1.Transform(points.ToArray()).ToList();
            var second = t2.Transform(points.ToArray()).ToList();
            var f2 = t1.Transform(points2.ToArray()).ToList();
            var s2 = t2.Transform(points2.ToArray()).ToList();
            
        
            for (int i = 0; i < first.Count; i++)
            {
                Assert.AreEqual(first[i] , second[i]);
            }
        }
    }
}