using System.Collections.Concurrent;
using System.Linq;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class MainViewModel
    {
        private CommandHandler compositeSineCommand;

        public CommandHandler CompositeSineCommand =>
            compositeSineCommand ?? (compositeSineCommand = new CommandHandler(CompositeSine, () => true));

        private void CompositeSine(object obj)
        {
            FirstSignalViewModel.SignalData.Frequency.Value = 440.0d;
            FirstSignalViewModel.SignalData.Duration.Value = 0.02d;
            FirstSignalViewModel.SignalData.Samples.Value = 500;
            FirstSignalViewModel.GenerateSignalCommand.Execute(null);
            SecondSignalViewModel.SignalData.Frequency.Value = 3000.0d;
            SecondSignalViewModel.SignalData.Duration.Value = 0.009d;
            SecondSignalViewModel.SignalData.Amplitude.Value = 20.0d;
            SecondSignalViewModel.SignalData.Samples.Value = 1500;
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);
            CompositionViewModel.AddCommand.Execute((short)1);
        }

        private CommandHandler correlationTestCommand;

        public CommandHandler CorrelationTestcommand => correlationTestCommand ??
                                                        (correlationTestCommand =
                                                            new CommandHandler(CorrelationTest, () => true));

        private void CorrelationTest(object obj)
        {
            var first = FirstSignalViewModel.SignalData;
            var second = SecondSignalViewModel.SignalData;

            first.Duration.Value = 5;
            second.Duration.Value = 5;
            first.StartTime.Value = 0.3d;
            FirstSignalViewModel.GenerateSignalCommand.Execute(null);
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);

            SecondSignalViewModel.SignalData.Points = Correlation.Correlate(first.Points, second.Points).ToList();
        }

        public MainViewModel()
        {
            FirstSignalViewModel = new SignalViewModel();
            SecondSignalViewModel = new SignalViewModel();

            CompositionViewModel = new CompositionViewModel(FirstSignalViewModel, SecondSignalViewModel);
            ConversionViewModel = new ConversionViewModel(FirstSignalViewModel);
            ConvolutionFiltrationCorrelationViewModel = new ConvolutionFiltrationCorrelationViewModel(FirstSignalViewModel);
            DistanceSensorViewModel = new DistanceSensorViewModel();
        }

        public CompositionViewModel CompositionViewModel { get; }

        public DistanceSensorViewModel DistanceSensorViewModel { get; }

        public ConversionViewModel ConversionViewModel { get; }

        public ConvolutionFiltrationCorrelationViewModel ConvolutionFiltrationCorrelationViewModel { get; }

        public SignalViewModel FirstSignalViewModel { get; }

        public SignalViewModel SecondSignalViewModel { get; }
    }
}