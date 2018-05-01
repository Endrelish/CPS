using System.Collections.Concurrent;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class MainViewModel
    {
        public static FunctionAttribute<bool> SecondSignalContinuous;

        public MainViewModel()
        {
            FirstSignalViewModel = new SignalViewModel();
            SecondSignalViewModel = new SignalViewModel();

            SecondSignalContinuous = SecondSignalViewModel.SignalData.Continuous;

            CompositionViewModel = new CompositionViewModel(FirstSignalViewModel, SecondSignalViewModel);
            ConversionViewModel = new ConversionViewModel(FirstSignalViewModel);
            ConvolutionFiltrationCorrelationViewModel = new ConvolutionFiltrationCorrelationViewModel(FirstSignalViewModel, SecondSignalViewModel);
        }

        public CompositionViewModel CompositionViewModel { get; }

        public ConversionViewModel ConversionViewModel { get; }

        public ConvolutionFiltrationCorrelationViewModel ConvolutionFiltrationCorrelationViewModel { get; }

        public SignalViewModel FirstSignalViewModel { get; }

        public SignalViewModel SecondSignalViewModel { get; }
    }
}