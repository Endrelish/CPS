using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CPS1.Annotations;
using CPS1.Binding;
using CPS1.Model.CommandHandlers;
using CPS1.Model.Conversion;
using CPS1.Model.Parameters;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class ConversionViewModel : INotifyPropertyChanged
    {
        private readonly SignalViewModel firstSignalViewModel;

        private readonly SignalViewModel secondSignalViewModel;

        private CommandHandler computeCommand;

        private string operation;

        public ConversionViewModel(SignalViewModel first)
        {
            firstSignalViewModel = first;
            secondSignalViewModel = new SignalViewModel();

            SamplingFrequency = new FunctionAttribute<int>(10, false, 1, 5000, "SAMPLING FREQUENCY");
            QuantizationBits = new FunctionAttribute<int>(3, false, 2, 10, "QUANTIZATION BITS");
            QuantizationLevels = new FunctionAttribute<int>(8, false, 2, 1024, "QUANTIZATION LEVELS");

            AttributesBinding.BindAttributesTwoWay(QuantizationLevels, QuantizationBits, new LogarithmConverter());
            NumberOfSamples = new FunctionAttribute<int>(5, false, 2, 500, "NUMBER OF SAMPLES");

            AnalogToDigitalConversionOperations = new List<string> {"UNIFORM SAMPLING", "UNIFORM QUANTIZATION"};
            DigitalToAnalogConversionOperations = new List<string> {"ZERO-ORDER HOLD", "SINC RECONSTRUCTION"};
            Operation = string.Empty;

            MaximumDifference = new Parameter(0.0d, "MAXIMUM DIFFERENCE");
            SignalToNoiseRatio = new Parameter(0, "SIGNAL TO NOISE RATIO");
            MeanSquaredError = new Parameter(0, "MEAN SQUARED ERROR");
            PeakSignalToNoiseRatio = new Parameter(0, "PEAK SIGNAL TO NOISE RATIO");


            Attributes = new List<FunctionAttribute<int>>(
                new[] {SamplingFrequency, QuantizationLevels, QuantizationBits, NumberOfSamples});
            ConversionMetrics = new List<Parameter>(new[]
                {MeanSquaredError, MaximumDifference, SignalToNoiseRatio, PeakSignalToNoiseRatio});

            FirstSignalData.Continuous.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(IsSignalAnalog));
        }

        public FunctionData FirstSignalData => firstSignalViewModel.SignalData;

        public FunctionData SecondSignalData => secondSignalViewModel.SignalData;

        public List<string> AnalogToDigitalConversionOperations { get; }

        public IEnumerable<FunctionAttribute<int>> Attributes { get; }

        public CommandHandler ComputeCommand
        {
            get
            {
                return computeCommand ?? (computeCommand = new CommandHandler(
                           obj => Compute(obj),
                           () => CanCompute()));
            }
        }

        public List<string> DigitalToAnalogConversionOperations { get; }

        public bool IsSignalAnalog => firstSignalViewModel.SignalData.Continuous.Value;

        public Parameter MaximumDifference { get; }

        public Parameter MeanSquaredError { get; }

        public FunctionAttribute<int> NumberOfSamples { get; }

        public string Operation
        {
            get => operation;
            set
            {
                if (value == operation)
                {
                    return;
                }

                operation = value;
                SetParametersVisibility(value);
                ComputeCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public Parameter PeakSignalToNoiseRatio { get; }

        public FunctionAttribute<int> QuantizationLevels { get; }
        public FunctionAttribute<int> QuantizationBits { get; }

        public FunctionAttribute<int> SamplingFrequency { get; }

        public Parameter SignalToNoiseRatio { get; }

        public IEnumerable<Parameter> ConversionMetrics { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CalculateMetrics()
        {
            MeanSquaredError.Value = Metrics.MeanSquaredError(
                firstSignalViewModel.SignalData,
                secondSignalViewModel.SignalData);
            PeakSignalToNoiseRatio.Value = Metrics.PeakSignalToNoiseRatio(
                firstSignalViewModel.SignalData,
                secondSignalViewModel.SignalData);
            SignalToNoiseRatio.Value = Metrics.SignalToNoiseRatio(
                firstSignalViewModel.SignalData,
                secondSignalViewModel.SignalData);
            MaximumDifference.Value = Metrics.MaximumDifference(
                firstSignalViewModel.SignalData,
                secondSignalViewModel.SignalData);
        }

        private bool CanCompute()
        {
            if (firstSignalViewModel.SignalData.Continuous.Value)
            {
                return AnalogToDigitalConversionOperations.Contains(Operation);
            }

            return DigitalToAnalogConversionOperations.Contains(Operation);
        }

        private void Compute(object parameter)
        {
            switch (Operation)
            {
                case "UNIFORM SAMPLING":
                    Operations.Sampling(
                        FirstSignalData,
                        SecondSignalData,
                        SamplingFrequency.Value);
                    break;
                case "UNIFORM QUANTIZATION":
                    Operations.Quantization(
                        FirstSignalData,
                        SecondSignalData,
                        QuantizationLevels.Value);
                    break;
                case "ZERO-ORDER HOLD":
                    Operations.ZeroOrderHold(
                        FirstSignalData,
                        SecondSignalData);
                    break;
                case "SINC RECONSTRUCTION":
                    Operations.SincReconstruction(
                        FirstSignalData,
                        SecondSignalData,
                        NumberOfSamples.Value);
                    break;
            }

            CalculateMetrics();
        }

        private void SetParametersVisibility(string value)
        {
            SamplingFrequency.Visibility = value == "UNIFORM SAMPLING";
            QuantizationLevels.Visibility = value == "UNIFORM QUANTIZATION";
            NumberOfSamples.Visibility = value == "SINC RECONSTRUCTION";
        }
    }
}