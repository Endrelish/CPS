using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPS1.Model.Transform.WalshHadamardTransform.Tests
{
    [TestClass]
    public class DiscreteWalshHadamardTransformTests
    {
        [TestMethod]
        public void TransformTest()
        {
            var dwht = new DiscreteWalshHadamardTransform();
            var fwht = new FastWalshHadamardTransform();
            var points = new List<double>(new double[] {1, 2, 1, 2, 1, 3, 1, 5, 1, 1, 1, 8, 2, 1, 3, 4});
            var r1 = dwht.Transform(points.Select(d => new Point(0, d))).ToArray();
            var r2 = fwht.Transform(points.Select(d => new Point(0, d))).ToArray();

            for (var i = 0; i < 16; i++)
            {
                Assert.AreEqual(r1[i], r2[i]);
            }
        }
    }
}