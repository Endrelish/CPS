namespace CPS1.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using CPS1.Model;
    using CPS1.View;

    public class MainViewModel
    {
        [NonSerialized]
        private static readonly Func<double, string> Formatter1 = value => value.ToString("N");

        private readonly IFileDialog fileDialog;

        private readonly IFileSerializer serializer;

        private ICommand addCommand;

        private ICommand divideCommand;

        private Signal firstSignalType = Signal.Sine;

        private ICommand generateSignalCommand;

        private ICommand holdCommand;

        private ICommand mdCommand;

        private ICommand mseCommand;

        private ICommand multiplyCommand;

        private ICommand openCommand;

        private ICommand psnrCommand;

        private ICommand quantizationCommand;

        private ICommand samplingCommand;

        private ICommand saveCommand;

        private Signal secondSignalType = Signal.Sine;

        private ICommand sincCommand;

        private ICommand snrCommand;

        private ICommand subtractCommand;

        public MainViewModel()
        {
            this.SignalFirst = new FunctionData();
            this.SignalSecond = new FunctionData();

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

            this.SamplingFrequency = 0.1d / this.SignalFirst.Period.Value;
        }

        public static Func<double, string> Formatter => Formatter1;

        public ICommand AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.SignalsGenerated));

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

        public ICommand HoldCommand
        {
            get
            {
                return this.holdCommand ?? (this.holdCommand = new CommandHandler(
                                                obj => this.ZeroOrderHold(),
                                                () => this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand MdCommand
        {
            get
            {
                return this.mdCommand ?? (this.mdCommand = new CommandHandler(
                                              obj => this.Md(),
                                              () => !this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand MseCommand
        {
            get
            {
                return this.mseCommand ?? (this.mseCommand = new CommandHandler(
                                               obj => this.Mse(),
                                               () => !this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.SignalFirst.Points.Count > 0
                                                                              && this.SignalSecond.Points.Count > 0));

        public ICommand OpenCommand =>
            this.openCommand ?? (this.openCommand = new CommandHandler(this.OpenSignal, () => true));

        public int Param { get; } = 2;

        public ICommand PsnrCommand
        {
            get
            {
                return this.psnrCommand ?? (this.psnrCommand = new CommandHandler(
                                                obj => this.Psnr(),
                                                () => !this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand QuantizationCommand
        {
            get
            {
                return this.quantizationCommand ?? (this.quantizationCommand = new CommandHandler(
                                                        obj => this.Quantization(),
                                                        () => this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand SamplingCommand
        {
            get
            {
                return this.samplingCommand ?? (this.samplingCommand = new CommandHandler(
                                                    obj => this.Sampling(),
                                                    () => this.SignalFirst.Continuous.Value));
            }
        }

        public double SamplingFrequency { get; set; }

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

        public ICommand SincCommand
        {
            get
            {
                return this.sincCommand ?? (this.sincCommand = new CommandHandler(
                                                obj => this.Sinc(),
                                                () => this.SignalFirst.Continuous.Value));
            }
        }

        public ICommand SnrCommand
        {
            get
            {
                return this.snrCommand ?? (this.snrCommand = new CommandHandler(
                                               obj => this.Snr(),
                                               () => !this.SignalFirst.Continuous.Value));
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
            throw new NotImplementedException();
        }

        private void Sampling()
        {
            this.SignalSecond.AssignSignal(this.SignalFirst);
            this.SignalSecond.Continuous.Value = false;
            this.SignalSecond.Period.Value = 1.0d / this.SamplingFrequency;
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
            throw new NotImplementedException();
        }
    }
}