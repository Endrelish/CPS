namespace CPS1.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using CPS1.Annotations;
    using CPS1.Model;
    using CPS1.View;

    public class MainViewModel : INotifyPropertyChanged
    {
        [NonSerialized]
        private static readonly Func<double, string> Formatter1 = value => value.ToString("N");

        private readonly IFileDialog fileDialog;

        private readonly IFileSerializer serializer;

        private ICommand addCommand;

        private string adOperation;

        private ICommand computeCommand;

        private string daOperation;

        private ICommand divideCommand;

        private Signal firstSignalType;

        private ICommand generateSignalCommand;

        private ICommand multiplyCommand;

        private ICommand openCommand;

        private bool quantizationLevelsVisibility;

        private bool samplingFrequencyVisibility;

        private ICommand saveCommand;

        private Signal secondSignalType;

        private ICommand subtractCommand;

        public MainViewModel()
        {
            this.SignalFirst = new FunctionData();
            this.SignalSecond = new FunctionData();
            this.firstSignalType = Signal.Sine;
            this.secondSignalType = Signal.Sine;
            this.SignalFirst.Type = this.firstSignalType;
            this.SignalSecond.Type = this.secondSignalType;
            this.SignalFirst.Continuous.Value = true;
            this.SignalSecond.Continuous.Value = true;

            this.SetRequiredParameters(this.SignalFirst);
            this.SetRequiredParameters(this.SignalSecond);

            this.fileDialog = new FileDialogWpf();
            this.serializer = new FileBinarySerializer();

            this.SignalFirst.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SignalSecond.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SamplingFrequency = 10 / this.SignalFirst.Period.Value;
            this.QuantizationLevels = 25;

            this.AdOperations = new List<string> { "UNIFORM SAMPLING", "UNIFORM QUANTIZATION" };
            this.DaOperations = new List<string> { "ZERO-ORDER HOLD", "SINC RECONSTRUCTION" };
            this.AdOperation = string.Empty;
            this.DaOperation = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Func<double, string> Formatter => Formatter1;

        public ICommand AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.SignalsGenerated));

        public string AdOperation
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
                ((CommandHandler)this.ComputeCommand).RaiseCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        public List<string> AdOperations { get; }

        public ICommand ComputeCommand
        {
            get
            {
                return this.computeCommand ?? (this.computeCommand = new CommandHandler(
                                                   obj => this.Compute(obj),
                                                   () => (this.SignalFirst.Continuous.Value
                                                         && this.AdOperation.Length > 0)
                                                         || (!this.SignalFirst.Continuous.Value
                                                         && this.DaOperation.Length > 0)));
            }
        }

        public string DaOperation
        {
            get => this.daOperation;
            set
            {
                if (value == this.daOperation)
                {
                    return;
                }
                SetParametersVisibility(value);
                this.daOperation = value;
                ((CommandHandler)this.ComputeCommand).RaiseCanExecuteChanged();
                this.OnPropertyChanged();
            }
        }

        public List<string> DaOperations { get; }

        public ICommand DivideCommand => this.divideCommand
                                         ?? (this.divideCommand = new CommandHandler(
                                                 this.DivideSignals,
                                                 this.SignalsGenerated));

        public string FirstSignalType
        {
            get => AvailableFunctions.GetDescription(this.firstSignalType);
            set
            {
                this.firstSignalType = AvailableFunctions.GetTypeByDescription(value);
                this.SetRequiredParameters(this.SignalFirst);
            }
        }

        public ICommand GenerateSignalCommand => this.generateSignalCommand
                                                 ?? (this.generateSignalCommand = new CommandHandler(
                                                         this.GenerateSignal,
                                                         () => true));

        public ICommand MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.SignalFirst.Points.Count > 0
                                                                              && this.SignalSecond.Points.Count > 0));

        public ICommand OpenCommand =>
            this.openCommand ?? (this.openCommand = new CommandHandler(this.OpenSignal, () => true));

        public int Param { get; } = 2;

        public int QuantizationLevels { get; set; }

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

        public ICommand SaveCommand =>
            this.saveCommand ?? (this.saveCommand = new CommandHandler(this.SaveSignal, () => true));

        public string SecondSignalType
        {
            get => AvailableFunctions.GetDescription(this.secondSignalType);
            set
            {
                this.secondSignalType = AvailableFunctions.GetTypeByDescription(value);
                this.SetRequiredParameters(this.SignalSecond);
            }
        }

        public FunctionData SignalFirst { get; set; }

        public FunctionData SignalSecond { get; set; }

        public IEnumerable<string> SignalsLabels
        {
            get
            {
                return AvailableFunctions.Functions.Values.Select(p => p.Item3);
            }
        }

        public ICommand SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
                                                                        this.SubtractSignals,
                                                                        () => this.SignalFirst.Points.Count > 0
                                                                              && this.SignalSecond.Points.Count > 0));

        public bool SignalsGenerated()
        {
            return this.SignalFirst.Points.Count > 0 && this.SignalSecond.Points.Count > 0;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.SignalFirst.Compose(this.SignalSecond, Operation.Add);
                }
                else
                {
                    this.SignalSecond.Compose(this.SignalFirst, Operation.Add);
                }
            }
        }

        private void Compute(object parameter)
        {
            if (parameter is string type)
            {
                if (type.Equals("AD"))
                {
                    switch (this.AdOperation)
                    {
                        case "UNIFORM SAMPLING":
                            this.Sampling();
                            break;
                        case "UNIFORM QUANTIZATION":
                            this.Quantization();
                            break;
                    }
                }
            }
        }

        private void DivideSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.SignalFirst.Compose(this.SignalSecond, Operation.Divide);
                }
                else
                {
                    this.SignalSecond.Compose(this.SignalFirst, Operation.Divide);
                }
            }
        }

        private void GenerateSignal(object parameter)
        {
            if (parameter is short chart)
            {
                if (chart == 1)
                {
                    this.SignalFirst.Type = this.firstSignalType;
                    this.SignalFirst.Function = AvailableFunctions.GetFunction(this.SignalFirst.Type);
                    Generator.GenerateSignal(this.SignalFirst);
                    Histogram.GetHistogram(this.SignalFirst);
                }
                else if (chart == 2)
                {
                    this.SignalSecond.Type = this.secondSignalType;
                    this.SignalSecond.Function = AvailableFunctions.GetFunction(this.SignalSecond.Type);
                    Generator.GenerateSignal(this.SignalSecond);
                    Histogram.GetHistogram(this.SignalSecond);
                }

                ((CommandHandler)this.ComputeCommand).RaiseCanExecuteChanged();
            }
        }

        private void Md()
        {
            throw new NotImplementedException();
        }

        private void Mse()
        {
            throw new NotImplementedException();
        }

        private void MultiplySignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.SignalFirst.Compose(this.SignalSecond, Operation.Multiply);
                }
                else
                {
                    this.SignalSecond.Compose(this.SignalFirst, Operation.Multiply);
                }
            }
        }

        private void OpenSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = this.fileDialog.GetOpenFilePath(this.serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                var data = this.serializer.Deserialize(filename);

                if (chart == 1)
                {
                    this.SignalFirst.AssignSignal(data);

                    // Histogram.GetHistogram(this.SignalFirst);
                    // this.SignalFirst.HistogramPointsUpdate();
                    // this.SignalFirst.PointsUpdate();
                    // this.SignalFirst.AllChanged();
                }
                else if (chart == 2)
                {
                    this.SignalSecond.AssignSignal(data);

                    // Histogram.GetHistogram(this.SignalSecond);
                    // this.SignalSecond.HistogramPointsUpdate();
                    // this.SignalSecond.PointsUpdate();
                    // this.SignalSecond.AllChanged();
                }
            }
        }

        private void Psnr()
        {
            throw new NotImplementedException();
        }

        private void Quantization()
        {
            var levels = new List<double>();
            var step = this.SignalFirst.Amplitude.Value * 2.0d / this.QuantizationLevels;
            for (var i = 0; i <= this.QuantizationLevels; i++)
            {
                levels.Add(-this.SignalFirst.Amplitude.Value + i * step);
            }

            this.SignalSecond.AssignSignal(this.SignalFirst);

            foreach (var point in this.SignalSecond.Points)
            {
                point.Y = levels.OrderBy(l => Math.Abs(point.Y - l)).First(); // TODO This might be wrong
            }

            this.SignalSecond.PointsUpdate();
        }

        private void Sampling()
        {
            this.SignalSecond.AssignSignal(this.SignalFirst);
            this.SignalSecond.Continuous.Value = false;
            //this.SignalSecond.Samples.Value = (int)(SignalSecond.Duration.Value * this.SamplingFrequency);
            Generator.GenerateSignal(this.SignalSecond);
        }

        private void SaveSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = this.fileDialog.GetSaveFilePath(this.serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                if (chart == 1)
                {
                    this.serializer.Serialize(this.SignalFirst, filename);
                }
                else if (chart == 2)
                {
                    this.serializer.Serialize(this.SignalSecond, filename);
                }
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
            }
        }

        private void SetRequiredParameters(FunctionData signal)
        {
            var choice = this.firstSignalType;
            if (signal == this.SignalSecond)
            {
                choice = this.secondSignalType;
            }

            signal.RequiredAttributes = AvailableFunctions.GetRequiredParameters(choice);
        }

        private void Sinc()
        {
            throw new NotImplementedException();
        }

        private void Snr()
        {
            throw new NotImplementedException();
        }

        private void SubtractSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.SignalFirst.Compose(this.SignalSecond, Operation.Subtract);
                }
                else
                {
                    this.SignalSecond.Compose(this.SignalFirst, Operation.Subtract);
                }
            }
        }

        private void ZeroOrderHold()
        {
            this.SignalSecond.AssignSignal(this.SignalFirst);
        }
    }
}