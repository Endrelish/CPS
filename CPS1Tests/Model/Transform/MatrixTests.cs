using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPS1.Model.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS1.Model.Transform.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public void MultiplicationTest()
        {
            var first = new Matrix<int>(4, 4);
            int i = 0, j = 0;
            first[i, j++ % 4] = 1;
            first[i, j++ % 4] = 3;
            first[i, j++ % 4] = 1;
            first[i++, j++ % 4] = 1;

            first[i, j++ % 4] = 2;
            first[i, j++ % 4] = 2;
            first[i, j++ % 4] = 3;
            first[i++, j++ % 4] = 3;

            first[i, j++ % 4] = 1;
            first[i, j++ % 4] = 2;
            first[i, j++ % 4] = 2;
            first[i++, j++ % 4] = 1;

            first[i, j++ % 4] = 3;
            first[i, j++ % 4] = 2;
            first[i, j++ % 4] = 1;
            first[i, j % 4] = 3;

            var second = new Matrix<int>(2, 4);

            i = 0;
            j = 0;
            second[i, j++ % 4] = 2;
            second[i, j++ % 4] = 1;
            second[i, j++ % 4] = 3;
            second[i++, j++ % 4] = 1;

            second[i, j++ % 4] = 1;
            second[i, j++ % 4] = 3;
            second[i, j++ % 4] = 3;
            second[i, j % 4] = 1;

            var product = first * second;

        }
    }
}