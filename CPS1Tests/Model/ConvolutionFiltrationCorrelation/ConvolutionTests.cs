using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Tests
{
    [TestClass()]
    public class ConvolutionTests
    {
        private static List<Point> first = new List<Point>(new[] { new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4) });
        private static List<Point> second = new List<Point>(new[] { new Point(0, 5), new Point(1, 6), new Point(2, 7) });

        private static List<Point> result = new List<Point>(new []{new Point(0, 5), new Point(1, 16), new Point(2, 34), new Point(3, 52), new Point(4, 45), new Point(5, 28)});
        [TestMethod()]
        public void ConvoluteTest()
        {
            var convolution = Convolution.Convolute(first, second).ToList();

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(convolution[i].Y, result[i].Y);
            }
        }
    }
}