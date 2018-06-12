using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS1.Model.Transform.WalshHadamardTransform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS1.Model.SignalData;

namespace CPS1.Model.Transform.WalshHadamardTransform.Tests
{
    [TestClass()]
    public class WalshHadamardTransformTests
    {
        [TestMethod()]
        public void TransformTest()
        {
            var r = new Random();
            var transformD = new DiscreteWalshHadamardTransform();
            var transformF = new FastWalshHadamardTransform();

            var points = new List<Point>();

            for (int i = 0; i < 256; i++)
            {
                points.Add(new Point(i, r.Next()));
            }

            var first = transformD.Transform(points.ToArray()).ToList();
            var second = transformF.Transform(points.ToArray()).ToList();

            for (int i = 0; i < first.Count; i++)
            {
                Assert.AreEqual(first[i], second[i]);
            }
        }
    }
}