using System;

namespace CPS1.Model.ConvolutionFiltrationCorrelation
{
    public static class ImpulseResponse
    {
        // K = fp / fo ---- fp - cz. probkowania, fo - cz. odciecia
        public static double Response(int n, double K, double M)
        {
            if (Math.Abs(n - (M - 1) / 2.0d) < double.Epsilon)
            {
                return 2.0d / K;
            }

            return Math.Sin(2 * Math.PI * (n - (M - 1) / 2.0d) / K) / (Math.PI * (n - (M - 1) / 2.0d));
        }
    }
}