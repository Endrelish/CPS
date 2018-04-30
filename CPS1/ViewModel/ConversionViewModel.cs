namespace CPS1.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using CPS1.Converters;
    using CPS1.Model.CommandHandlers;
    using CPS1.Model.Conversion;
    using CPS1.Model.Parameters;
    using CPS1.Model.SignalData;
    using CPS1.Properties;

    public class ConversionViewModel : INotifyPropertyChanged
    {
        private readonly SignalViewModel firstSignalViewModel;

        private readonly SignalViewModel secondSignalViewModel;

        public FunctionData FirstSignalData
        {
            get { return firstSignalViewModel.SignalData; }
        }

        public FunctionData SecondSignalData
        {
            get { return secondSignalViewModel.SignalData; }
        }

        private CommandHandler computeCommand;

        private string operation;

        public ConversionViewModel(SignalViewModel first, SignalViewModel second)
        {
            this.firstSignalViewModel = first;
            this.secondSignalViewModel = second;

            this.SamplingFrequency = new FunctionAttribute<int>(10, false, 1, 5000, "SAMPLING FREQUENCY");
            this.QuantizationBits = new FunctionAttribute<int>(3, false, 2, 10, "QUANTIZATION BITS");
            this.QuantizationLevels = new FunctionAttribute<int>(8, false, 2, 1024, "QUANTIZATION LEVELS");

            AttributesBinding.BindAttributesTwoWay<int>(QuantizationLevels, QuantizationBits, new LogarithmConverter());
            this.NumberOfSamples = new FunctionAttribute<int>(5, false, 2, 500, "NUMBER OF SAMPLES");
            
            this.AnalogToDigitalConversionOperations = new List<string> { "UNIFORM SAMPLING", "UNIFORM QUANTIZATION" };
            this.DigitalToAnalogConversionOperations = new List<string> { "ZERO-ORDER HOLD", "SINC RECONSTRUCTION" };
            this.Operation = string.Empty;

            this.MaximumDifference = new Parameter(0.0d, "MAXIMUM DIFFERENCE", true);
            this.SignalToNoiseRatio = new Parameter(0, "SIGNAL TO NOISE RATIO", true);
            this.MeanSquaredError = new Parameter(0, "MEAN SQUARED ERROR", true);
            this.PeakSignalToNoiseRatio = new Parameter(0, "PEAK SIGNAL TO NOISE RATIO", true);


            this.Attributes = new List<FunctionAttribute<int>>(
                new[] { this.SamplingFrequency, this.QuantizationLevels, this.QuantizationBits, this.NumberOfSamples });
            this.ConversionMetrics = new List<Parameter>(new[]
                {MeanSquaredError, MaximumDifference, SignalToNoiseRatio, PeakSignalToNoiseRatio});

            this.FirstSignalData.Continuous.PropertyChanged += (sender, args) => OnPropertyChanged(nameof(IsSignalAnalog));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> AnalogToDigitalConversionOperations { get; }

        public IEnumerable<FunctionAttribute<int>> Attributes { get; }

        public CommandHandler ComputeCommand
        {
            get
            {
                return this.computeCommand ?? (this.computeCommand = new CommandHandler(
                                                   obj => this.Compute(obj),
                                                   () => this.CanCompute()));
            }
        }

        public List<string> DigitalToAnalogConversionOperations { get; }

        public bool IsSignalAnalog => this.firstSignalViewModel.SignalData.Continuous.Value;

        public Parameter MaximumDifference { get; }

        public Parameter MeanSquaredError { get; }

        public FunctionAttribute<int> NumberOfSamples { get; }

        public string Operation
        {
            get => this.operation;
            set
            {
                if (value == this.operation)
                {
                    return;
                }

                this.operation = value;
                this.SetParametersVisibility(value);
                this.ComputeCommand.RaiseCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        public Parameter PeakSignalToNoiseRatio { get; }

        public FunctionAttribute<int> QuantizationLevels { get; }
        public FunctionAttribute<int> QuantizationBits { get; }

        public FunctionAttribute<int> SamplingFrequency { get; }

        public Parameter SignalToNoiseRatio { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<Parameter> ConversionMetrics { get; }

        private void CalculateMetrics()
        {
            this.MeanSquaredError.Value = Metrics.MeanSquaredError(
                this.firstSignalViewModel.SignalData,
                this.secondSignalViewModel.SignalData);
            this.PeakSignalToNoiseRatio.Value = Metrics.PeakSignalToNoiseRatio(
                this.firstSignalViewModel.SignalData,
                this.secondSignalViewModel.SignalData);
            this.SignalToNoiseRatio.Value = Metrics.SignalToNoiseRatio(
                this.firstSignalViewModel.SignalData,
                this.secondSignalViewModel.SignalData);
            this.MaximumDifference.Value = Metrics.MaximumDifference(
                this.firstSignalViewModel.SignalData,
                this.secondSignalViewModel.SignalData);
        }

        private bool CanCompute()
        {
            if (this.firstSignalViewModel.SignalData.Continuous.Value)
            {
                return this.AnalogToDigitalConversionOperations.Contains(this.Operation);
            }

            return this.DigitalToAnalogConversionOperations.Contains(this.Operation);
        }

        private void Compute(object parameter)
        {
                switch (this.Operation)
                {
                    case "UNIFORM SAMPLING":
                        Operations.Sampling(
                            this.FirstSignalData,
                            this.SecondSignalData,
                            this.SamplingFrequency.Value);
                        break;
                    case "UNIFORM QUANTIZATION":
                        Operations.Quantization(
                            this.FirstSignalData,
                            this.SecondSignalData,
                            this.QuantizationLevels.Value);
                        break;
                    case "ZERO-ORDER HOLD":
                        Operations.ZeroOrderHold(
                            this.FirstSignalData,
                            this.SecondSignalData);
                        break;
                    case "SINC RECONSTRUCTION":
                        Operations.SincReconstruction(
                            this.FirstSignalData,
                            this.SecondSignalData,
                            this.NumberOfSamples.Value);
                        break;
                }

                this.CalculateMetrics();
        }

        private void SetParametersVisibility(string value)
        {
            this.SamplingFrequency.Visibility = value == "UNIFORM SAMPLING";
            this.QuantizationLevels.Visibility = value == "UNIFORM QUANTIZATION";
            this.NumberOfSamples.Visibility = value == "SINC RECONSTRUCTION";
        }
    }
}