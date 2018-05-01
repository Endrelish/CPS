using System;
using System.Collections.Generic;
using System.Linq;
using CPS1.Model.ConvolutionFiltrationCorrelation;
using CPS1.Model.ConvolutionFiltrationCorrelation.Windows;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class ConvolutionFiltrationCorrelationViewModel
    {
        public SignalViewModel FirstSignalViewModel { get; }
        public SignalViewModel SecondSignalViewModel { get; }
        public SignalViewModel ConvolutionVM { get; private set; }
        private IWindow window;
        public string Window { get; set; }
        private IEnumerable<IWindow> windows { get; }

        public IEnumerable<string> Windows
        {
            get { return windows.Select(w => w.Name); }
        }

        public ConvolutionFiltrationCorrelationViewModel(SignalViewModel first, SignalViewModel second)
        {
            FirstSignalViewModel = first;
            SecondSignalViewModel = second;
        }

        private void ConvolutionTest()
        {
            FirstSignalViewModel.SignalData.Points.Clear();
            SecondSignalViewModel.SignalData.Points.Clear();
            FirstSignalViewModel.SignalData.Continuous.Value = false;
            SecondSignalViewModel.SignalData.Continuous.Value = false;

            var points = FirstSignalViewModel.SignalData.Points;
            points.AddRange(new[] { new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4) });
            points = SecondSignalViewModel.SignalData.Points;
            points.AddRange(new[] { new Point(0, 5), new Point(1, 6), new Point(2, 7) });

            ConvolutionVM = new SignalViewModel();
            ConvolutionVM.SignalData.AssignSignal(Convolution.Convolute(FirstSignalViewModel.SignalData, SecondSignalViewModel.SignalData));
        }

        // K = fp / f0 fp - cz. probkowania, f0 - cz. odciecia
        public static double ImpulseResponse(double n, double K, double M)
        {
            if (Math.Abs(n - (M - 1) / 2.0d) < double.Epsilon) return 2.0d / K;
            return (Math.Sin((2 * Math.PI * (n - (M - 1) / 2.0d)) / K) / (Math.PI * (n - (M - 1) / 2.0d)));
        }
    }
}