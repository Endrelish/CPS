using System.Linq;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation;

namespace CPS1.ViewModel
{
    public class MainViewModel
    {
        private CommandHandler compositeSineCommand;

        private CommandHandler correlationTestCommand;

        public MainViewModel()
        {
            FirstSignalViewModel = new SignalViewModel();
            SecondSignalViewModel = new SignalViewModel();

            CompositionViewModel = new CompositionViewModel(FirstSignalViewModel, SecondSignalViewModel);
            ConversionViewModel = new ConversionViewModel(FirstSignalViewModel);
            ConvolutionFiltrationCorrelationViewModel =
                new ConvolutionFiltrationCorrelationViewModel(FirstSignalViewModel);
            DistanceSensorViewModel = new DistanceSensorViewModel();
            TransformViewModel = new TransformViewModel(FirstSignalViewModel.SignalData);
        }

        public CommandHandler CompositeSineCommand =>
            compositeSineCommand ?? (compositeSineCommand = new CommandHandler(CompositeSine, () => true));

        public CommandHandler CorrelationTestcommand => correlationTestCommand ??
                                                        (correlationTestCommand =
                                                            new CommandHandler(ConvolutionTest, () => true));

        public CompositionViewModel CompositionViewModel { get; }

        public DistanceSensorViewModel DistanceSensorViewModel { get; }

        public ConversionViewModel ConversionViewModel { get; }

        public ConvolutionFiltrationCorrelationViewModel ConvolutionFiltrationCorrelationViewModel { get; }

        public SignalViewModel FirstSignalViewModel { get; }

        public SignalViewModel SecondSignalViewModel { get; }
        public TransformViewModel TransformViewModel { get; }

        private void CompositeSine(object obj)
        {
            FirstSignalViewModel.SignalData.Frequency.Value = 460.0d;
            FirstSignalViewModel.SignalData.Duration.Value = 0.02d;
            FirstSignalViewModel.SignalData.Samples.Value = 128;
            FirstSignalViewModel.GenerateSignalCommand.Execute(null);
            SecondSignalViewModel.SignalData.Frequency.Value = 3000.0d;
            SecondSignalViewModel.SignalData.Duration.Value = 0.009d;
            SecondSignalViewModel.SignalData.Amplitude.Value = 20.0d;
            SecondSignalViewModel.SignalData.Samples.Value = 128;
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);
            CompositionViewModel.AddCommand.Execute((short) 1);
        }

        private void ConvolutionTest(object obj)
        {
            var first = FirstSignalViewModel.SignalData;
            var second = SecondSignalViewModel.SignalData;

            first.Duration.Value = 5;
            second.Duration.Value = 5;
            //first.StartTime.Value = 0.3d;
            FirstSignalViewModel.GenerateSignalCommand.Execute(null);
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);

            SecondSignalViewModel.SignalData.Points = Convolution.Convolute(first.Points, second.Points).ToList();
        }
    }
}
