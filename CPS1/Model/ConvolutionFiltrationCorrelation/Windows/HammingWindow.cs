using System;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Windows
{
    public class HammingWindow : IWindow
    {
        public string Name { get; } = "HAMMING WINDOW";

        public double Window(int n, double M)
        {
            return 0.53836d - 0.46164 * Math.Cos(2.0d * Math.PI * n / M);
        }
    }
}