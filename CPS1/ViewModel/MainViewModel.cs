using System.Linq;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation;
using CPS1.Model.SignalData;

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
            var signalFirst = FirstSignalViewModel.SignalData;
            var signalSecond = SecondSignalViewModel.SignalData;
            signalFirst.Amplitude.Value = 2.0d;
            signalFirst.Period.Value = 2.0d;
            signalFirst.Duration.Value = 4.0d;
            signalFirst.Samples.Value = 64;
            FirstSignalViewModel.GenerateSignalCommand.Execute(null);

            signalSecond.Amplitude.Value = 1.0d;
            signalSecond.Period.Value = 1.0d;
            signalSecond.Duration.Value = 4.0d;
            signalSecond.Samples.Value = 64;
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);
            CompositionViewModel.AddCommand.Execute((short) 1);

            signalSecond.AssignSignal(new FunctionData());

            signalSecond.Amplitude.Value = 5.0d;
            signalSecond.Period.Value = 0.5d;
            signalSecond.Duration.Value = 4.0d;
            signalSecond.Samples.Value = 64;
            SecondSignalViewModel.GenerateSignalCommand.Execute(null);
            CompositionViewModel.AddCommand.Execute((short)1);
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
