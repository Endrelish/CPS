namespace CPS1.Model.ConvolutionFiltrationCorrelation.Windows
{
    public class RectangularWindow : IWindow
    {
        public string Name { get; } = "RECTANGULAR WINDOW";

        public double Window(int n, double M)
        {
            return 1.0d;
        }
    }
}