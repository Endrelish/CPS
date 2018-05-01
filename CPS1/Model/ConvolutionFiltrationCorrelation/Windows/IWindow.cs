namespace CPS1.Model.ConvolutionFiltrationCorrelation.Windows
{
    public interface IWindow
    {
        string Name { get; }
        double Window(int n, double M);
    }
}