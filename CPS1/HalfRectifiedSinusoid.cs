namespace CPS1
{
    using System;

    using CPS1.Functions;

    public class HalfRectifiedSinusoid : IFunction
    {
        public HalfRectifiedSinusoid()
        {
            this.SinusoidFunction = (A, T, t, t1) =>
                {
                    var ret = A * Math.Sin(Math.PI * 2 * (t - t1) / T);
                    if (ret.CompareTo(0) < 0)
                    {
                        ret = 0;
                    }

                    return ret;
                };
        }

        private Func<double, double, double, double, double> SinusoidFunction { get; }

        public void GeneratePoints(FunctionData data)
        {
            throw new NotImplementedException();
        }
    }
}