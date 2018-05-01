using System;

namespace CPS1.Model.ConvolutionFiltrationCorrelation.Windows
{
    public class HanningWindow : IWindow
    {
        public string Name { get; } = "HANNING WINDOW";

        public double Window(int n, double M)
        {
            return 0.5d - 0.5d * Math.Cos(2 * Math.PI * n / M);
        }
    }
}