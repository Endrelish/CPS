using System;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Windows
{
    public class BlackmanWindow : IWindow
    {
        public string Name { get; } = "BLACKMAN WINDOW";

        public double Window(int n, double M)
        {
            return 0.42d - 0.5d * Math.Cos(2.0d * Math.PI * n / M) + 0.08d * Math.Cos(4 * Math.PI * n / M);
        }
    }
}