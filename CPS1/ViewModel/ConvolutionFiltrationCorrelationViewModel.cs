using CPS1.Model.ConvolutionFiltrationCorrelation;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class ConvolutionFiltrationCorrelationViewModel
    {
        public SignalViewModel FirstSignalViewModel { get; }
        public SignalViewModel SecondSignalViewModel { get; }
        public SignalViewModel ConvolutionVM { get; }

        public ConvolutionFiltrationCorrelationViewModel(SignalViewModel first, SignalViewModel second)
        {
            FirstSignalViewModel = first;
            SecondSignalViewModel = second;

            FirstSignalViewModel.SignalData.Points.Clear();
            SecondSignalViewModel.SignalData.Points.Clear();
            FirstSignalViewModel.SignalData.Continuous.Value = false;
            SecondSignalViewModel.SignalData.Continuous.Value = false;

            var points = FirstSignalViewModel.SignalData.Points;
            points.AddRange(new []{new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4) });
            points = SecondSignalViewModel.SignalData.Points;
            points.AddRange(new[] { new Point(0, 5), new Point(1, 6), new Point(2, 7) });

            ConvolutionVM = new SignalViewModel();
            ConvolutionVM.SignalData.AssignSignal(Convolution.Convolute(FirstSignalViewModel.SignalData, SecondSignalViewModel.SignalData));
        }
    }
}