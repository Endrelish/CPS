namespace CPS1.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using CPS1.Model;
    using CPS1.Model.Conversion;
    using CPS1.Model.SignalData;
    using CPS1.Properties;

    using Operations = CPS1.Model.Conversion.Operations;

    public class ConversionViewModel : INotifyPropertyChanged
    {
        private string adOperation;

        private CommandHandler computeCommand;

        private string daOperation;

        private readonly SignalViewModel firstSignalViewModel;

        private FunctionAttribute<double> maximumDifference;

        private FunctionAttribute<double> meanSquaredError;

        private FunctionAttribute<double> peakSignalToNoiseRatio;

        private int quantizationLevels;

        private bool quantizationLevelsVisibility;

        private bool samplingFrequencyVisibility;

        private readonly SignalViewModel secondSignalViewModel;

        private FunctionAttribute<double> signalToNoiseRatio;

        public ConversionViewModel(SignalViewModel first, SignalViewModel second)
        {
            this.firstSignalViewModel = first;
            this.secondSignalViewModel = second;
            this.SamplingFrequency = 10;
            this.QuantizationLevels = 25;

            this.AnalogToDigitalConversionOperations = new List<string> { "UNIFORM SAMPLING", "UNIFORM QUANTIZATION" };
            this.DigitalToAnalogConversionOperations = new List<string> { "ZERO-ORDER HOLD", "SINC RECONSTRUCTION" };
            this.Operation = string.Empty;

            this.MaximumDifference = new FunctionAttribute<double>(0.0d, true, 0, 0, "MAXIMUM DIFFERENCE");
            this.SignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "SIGNAL TO NOISE RATIO");
            this.MeanSquaredError = new FunctionAttribute<double>(0, true, 0, 0, "MEAN SQUARED ERROR");
            this.PeakSignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "PEAK SIGNAL TO NOISE RATIO");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> AnalogToDigitalConversionOperations { get; }

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

        public FunctionAttribute<double> MaximumDifference
        {
            get => this.maximumDifference;
            set
            {
                if (value.Equals(this.maximumDifference))
                {
                    return;
                }

                this.maximumDifference = value;
                this.OnPropertyChanged();
            }
        }

        public FunctionAttribute<double> MeanSquaredError
        {
            get => this.meanSquaredError;
            set
            {
                if (value.Equals(this.meanSquaredError))
                {
                    return;
                }

                this.meanSquaredError = value;
                this.OnPropertyChanged();
            }
        }

        public string Operation
        {
            get => this.adOperation;
            set
            {
                if (value == this.adOperation)
                {
                    return;
                }

                this.SetParametersVisibility(value);
                this.adOperation = value;
                this.ComputeCommand.RaiseCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        public FunctionAttribute<double> PeakSignalToNoiseRatio
        {
            get => this.peakSignalToNoiseRatio;
            set
            {
                if (value.Equals(this.peakSignalToNoiseRatio))
                {
                    return;
                }

                this.peakSignalToNoiseRatio = value;
                this.OnPropertyChanged();
            }
        }

        public int QuantizationLevels
        {
            get => this.quantizationLevels;
            set
            {
                if (value == this.quantizationLevels)
                {
                    return;
                }

                this.quantizationLevels = value;
                this.OnPropertyChanged();
            }
        }

        public bool QuantizationLevelsVisibility
        {
            get => this.quantizationLevelsVisibility;
            set
            {
                if (value == this.quantizationLevelsVisibility)
                {
                    return;
                }

                this.quantizationLevelsVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public double SamplingFrequency { get; set; }

        public bool SamplingFrequencyVisibility
        {
            get => this.samplingFrequencyVisibility;
            set
            {
                if (value == this.samplingFrequencyVisibility)
                {
                    return;
                }

                this.samplingFrequencyVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public FunctionAttribute<double> SignalToNoiseRatio
        {
            get => this.signalToNoiseRatio;
            set
            {
                if (value.Equals(this.signalToNoiseRatio))
                {
                    return;
                }

                this.signalToNoiseRatio = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            if (parameter is string type)
            {
                switch (this.Operation)
                {
                    case "UNIFORM SAMPLING":
                        Operations.Sampling(
                            this.firstSignalViewModel.SignalData,
                            this.secondSignalViewModel.SignalData,
                            this.SamplingFrequency);
                        break;
                    case "UNIFORM QUANTIZATION":
                        Operations.Quantization(
                            this.firstSignalViewModel.SignalData,
                            this.secondSignalViewModel.SignalData,
                            this.QuantizationLevels);
                        break;
                    case "ZERO-ORDER HOLD":
                        Operations.ZeroOrderHold(
                            this.firstSignalViewModel.SignalData,
                            this.secondSignalViewModel.SignalData);
                        break;
                    case "SINC RECONSTRUCTION":
                        Operations.SincReconstruction(
                            this.firstSignalViewModel.SignalData,
                            this.secondSignalViewModel.SignalData,
                            this.QuantizationLevels);
                        break;
                }

                this.CalculateMetrics();
            }
        }

        private void SetParametersVisibility(string value)
        {
            switch (value)
            {
                case "UNIFORM SAMPLING":
                    this.SamplingFrequencyVisibility = true;
                    this.QuantizationLevelsVisibility = false;
                    break;
                case "UNIFORM QUANTIZATION":
                    this.SamplingFrequencyVisibility = false;
                    this.QuantizationLevelsVisibility = true;
                    break;
                case "ZERO-ORDER HOLD":
                    this.SamplingFrequencyVisibility = false;
                    this.QuantizationLevelsVisibility = false;
                    break;
                case "SINC RECONSTRUCTION":
                    this.SamplingFrequencyVisibility = false;
                    this.QuantizationLevelsVisibility = true;
                    break;
            }
        }
    }
}