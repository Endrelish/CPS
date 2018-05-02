using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS1.Model.ConvolutionFiltrationCorrelation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Tests
{
    [TestClass()]
    public class CorrelationTests
    {
        private static List<Point> first = new List<Point>(new [] {new Point(0,1), new Point(1, 2), new Point(2, 3), new Point(3, 4)});
        private static List<Point> second = new List<Point>(new [] {new Point(0,5), new Point(1, 6), new Point(2, 7)});

        private static List<Point> result = new List<Point>(new[]
        {
            new Point(-2, 7), new Point(-1, 20), new Point(0, 38), new Point(1, 56), new Point(2, 39), new Point(3, 20)
        });

        [TestMethod()]
        public void CorrelateUsingConvolutionTest()
        {
            var correlation = Correlation.CorrelateUsingConvolution(first, second).ToList();
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(correlation[i].Y, result[i].Y);
            }
        }

        [TestMethod()]
        public void CorrelateTest()
        {
            var correlation = Correlation.Correlate(first, second).ToList();
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(correlation[i].Y, result[i].Y);
            }
        }
    }
}