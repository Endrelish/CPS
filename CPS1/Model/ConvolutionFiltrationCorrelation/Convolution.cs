using System.Linq;
using CPS1.Model.SignalData;

namespace CPS1.Model.ConvolutionFiltrationCorrelation
{
    public static class Convolution
    {
        public static FunctionData Convolute(FunctionData first, FunctionData second)
        {
            var M = first.Points.Count;
            var N = second.Points.Count;
            var convolution = new FunctionData();
            convolution.AssignSignal(first);
            convolution.Points.Clear();
            convolution.HistogramPoints.Clear();

            for (int i = 0; i < M + N - 1; i++)
            {
                var y = 0.0d;

                for (int j = 0; j <M; j++)
                {
                    if (j < first.Points.Count && i - j >= 0 && i - j < second.Points.Count)
                    {
                        y += first.Points[j].Y * second.Points[i - j].Y;
                    }
                }

                convolution.Points.Add(new Point(i, y));
            }

            convolution.StartTime.Value = 0;
            convolution.Duration.Value = M + N - 1;
            convolution.SetAmplitude();

            return convolution;
        }
    }
}