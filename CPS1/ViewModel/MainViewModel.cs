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
        public static readonly Func<double, string> Formatter = value => value.ToString("N");



        private readonly Func<double, double> sincFunc = t =>
            {
                if (Math.Abs(t) < double.Epsilon)
                {
                    return 1;
                }

                return Math.Sin(Math.PI * t) / (Math.PI * t);
            };

        private ICommand addCommand;

        private string adOperation;

        private ICommand computeCommand;

        private string daOperation;

        private ICommand divideCommand;
        

        private FunctionAttribute<double> meanSquaredError;

        private ICommand multiplyCommand;

        private ICommand openCommand;

        private bool quantizationLevelsVisibility;

        private bool samplingFrequencyVisibility;

        private ICommand saveCommand;
        
        private ICommand subtractCommand;

        private FunctionAttribute<double> signalToNoiseRatio;

        private FunctionAttribute<double> peakSignalToNoiseRatio;

        private FunctionAttribute<double> maximumDifference;

        public MainViewModel()
        {
            this.FirstSignal = new GeneratorViewModel();
            this.SecondSignal = new GeneratorViewModel();
            

            this.FirstSignal.FunctionData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SecondSignal.FunctionData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SamplingFrequency = 10 / this.FirstSignal.FunctionData.Period.Value;
            this.QuantizationLevels = 25;

            this.AdOperations = new List<string> { "UNIFORM SAMPLING", "UNIFORM QUANTIZATION" };
            this.DaOperations = new List<string> { "ZERO-ORDER HOLD", "SINC RECONSTRUCTION" };
            this.AdOperation = string.Empty;
            this.DaOperation = string.Empty;

            this.MaximumDifference = new FunctionAttribute<double>(0.0d, true, 0, 0, "MAXIMUM DIFFERENCE");
            this.SignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "SIGNAL TO NOISE RATIO");
            this.MeanSquaredError = new FunctionAttribute<double>(0, true, 0, 0, "MEAN SQUARED ERROR");
            this.PeakSignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "PEAK SIGNAL TO NOISE RATIO");
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

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
                                                   () => this.FirstSignal.FunctionData.Continuous.Value
                                                         && this.AdOperation.Length > 0
                                                         || !this.FirstSignal.FunctionData.Continuous.Value
                                                         && this.DaOperation.Length > 0));
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

                this.SetParametersVisibility(value);
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

        public ICommand MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.FirstSignal.Points.Count > 0
                                                                              && this.SecondSignal.Points.Count > 0));

        public ICommand OpenCommand =>
            this.openCommand ?? (this.openCommand = new CommandHandler(this.OpenSignal, () => true));
        
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

        

        public GeneratorViewModel FirstSignal { get; set; }

        public GeneratorViewModel SecondSignal { get; set; }

        public IEnumerable<string> SignalsLabels
        {
            get
            {
                return AvailableFunctions.Functions.Values.Select(p => p.Item3);
            }
        }

        public ICommand SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
                                                                        this.SubtractSignals,
                                                                        () => this.FirstSignal.Points.Count > 0
                                                                              && this.SecondSignal.Points.Count > 0));

        public bool SignalsGenerated()
        {
            return this.FirstSignal.Points.Count > 0 && this.SecondSignal.Points.Count > 0;
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
                    this.FirstSignal.Compose(this.SecondSignal, Operation.Add);
                }
                else
                {
                    this.SecondSignal.Compose(this.FirstSignal, Operation.Add);
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
                else
                {
                    switch (this.DaOperation)
                    {
                        case "ZERO-ORDER HOLD":
                            this.ZeroOrderHold();
                            break;
                        case "SINC RECONSTRUCTION":
                            this.Sinc();
                            break;
                    }
                }

                this.CalculateMetrics();
            }
        }

        private void DivideSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.FirstSignal.Compose(this.SecondSignal, Operation.Divide);
                }
                else
                {
                    this.SecondSignal.Compose(this.FirstSignal, Operation.Divide);
                }
            }
        }


        public FunctionAttribute<double> MaximumDifference
        {
            get
            {
                return this.maximumDifference;
            }
            set
            {
                if (value.Equals(this.maximumDifference)) return;
                this.maximumDifference = value;
                this.OnPropertyChanged();
            }
        }

        private void Md()
        {
            this.MaximumDifference.Value = this.SecondSignal.Points
                .Select(p => Math.Abs(p.Y - this.FirstSignal.Function(this.FirstSignal, p.X))).Max();
        }

        private void Mse()
        {
            this.MeanSquaredError.Value =
                this.SecondSignal.Points
                    .Select(p => Math.Pow(this.FirstSignal.Function(this.FirstSignal, p.X) - p.Y, 2)).Sum()
                / this.SecondSignal.Points.Count;
        }

        private void MultiplySignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.FirstSignal.Compose(this.SecondSignal, Operation.Multiply);
                }
                else
                {
                    this.SecondSignal.Compose(this.FirstSignal, Operation.Multiply);
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
                    this.FirstSignal.AssignSignal(data);

                    // Histogram.GetHistogram(this.FirstSignal);
                    // this.FirstSignal.HistogramPointsUpdate();
                    // this.FirstSignal.PointsUpdate();
                    // this.FirstSignal.AllChanged();
                }
                else if (chart == 2)
                {
                    this.SecondSignal.AssignSignal(data);

                    // Histogram.GetHistogram(this.SecondSignal);
                    // this.SecondSignal.HistogramPointsUpdate();
                    // this.SecondSignal.PointsUpdate();
                    // this.SecondSignal.AllChanged();
                }
            }
        }

        private void Psnr()
        {
            PeakSignalToNoiseRatio.Value = 10 * Math.Log10(this.FirstSignal.Points.Max(p => p.Y) / this.MeanSquaredError.Value);
        }

        private void CalculateMetrics()
        {
            this.Mse();
            this.Snr();
            this.Psnr();
            this.Md();
        }

        private void Quantization()
        {
            // TODO Number of bits in GUI
            // BUG Quantization after sampling
            var levels = new List<double>();
            var step = this.FirstSignal.Amplitude.Value * 2.0d / (this.QuantizationLevels - 1);
            for (var i = 0; i <= this.QuantizationLevels - 1; i++)
            {
                levels.Add(-this.FirstSignal.Amplitude.Value + i * step);
            }

            this.SecondSignal.AssignSignal(this.FirstSignal);

            foreach (var point in this.SecondSignal.Points)
            {
                point.Y = levels.OrderBy(l => Math.Abs(point.Y - l)).First();
            }

            this.SecondSignal.PointsUpdate();
        }

        private void Sampling()
        {
            this.SecondSignal.AssignSignal(this.FirstSignal);
            this.SecondSignal.Continuous.Value = false;
            this.SecondSignal.Samples.Value = (int)(this.SecondSignal.Duration.Value * this.SamplingFrequency);
            Generator.GenerateSignal(this.SecondSignal);
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

        

        private void Sinc()
        {
            this.SecondSignal.AssignSignal(this.FirstSignal);
            this.SecondSignal.Continuous.Value = true;
            this.SecondSignal.Samples.Value = 500;

            this.SecondSignal.Function = (data, t) =>
                {
                    var n = this.FirstSignal.Points.Count(p => p.X < t);

                    var sum = 0.0d;
                    var ts = this.FirstSignal.Duration.Value / this.FirstSignal.Samples.Value;
                    for (var i = n - this.QuantizationLevels; i < n + this.QuantizationLevels; i++)
                    {
                        ////for (int i = 0; i < FirstSignal.Points.Count; i++)
                        if (i < 0 || i > this.FirstSignal.Points.Count)
                        {
                            continue;
                        }

                        try
                        {
                            sum += this.FirstSignal.Points[i].Y
                                   * this.sincFunc((t - this.FirstSignal.Points[i].X) / ts);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // Do nothing
                        }
                    }

                    ////foreach (var point in FirstSignal.Points)
                    ////{
                    ////    sum += point.Y * this.sincFunc((t - point.X) / ts);
                    ////}
                    return sum;
                };
            SecondSignal.GenerateSignalCommand.Execute(null);
        }

        public FunctionAttribute<double> SignalToNoiseRatio
        {
            get
            {
                return this.signalToNoiseRatio;
            }
            set
            {
                if (value.Equals(this.signalToNoiseRatio)) return;
                this.signalToNoiseRatio = value;
                this.OnPropertyChanged();
            }
        }

        private void Snr()
        {
            var numerator = this.FirstSignal.Points.Select(p => p.Y * p.Y).Sum();
            var denominator = this.SecondSignal.Points.Select(p => Math.Pow(this.FirstSignal.Function(this.FirstSignal, p.X) - p.Y, 2))
                .Sum();

            this.SignalToNoiseRatio.Value = 10 * Math.Log10(numerator / denominator);
        }

        public FunctionAttribute<double> PeakSignalToNoiseRatio
        {
            get
            {
                return this.peakSignalToNoiseRatio;
            }
            set
            {
                if (value.Equals(this.peakSignalToNoiseRatio)) return;
                this.peakSignalToNoiseRatio = value;
                this.OnPropertyChanged();
            }
        }

        private void SubtractSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.FirstSignal.Compose(this.SecondSignal, Operation.Subtract);
                }
                else
                {
                    this.SecondSignal.Compose(this.FirstSignal, Operation.Subtract);
                }
            }
        }

        private void ZeroOrderHold()
        {
            this.SecondSignal.AssignSignal(this.FirstSignal);
            this.SecondSignal.Continuous.Value = true;
            this.SecondSignal.Samples.Value = 500;

            this.SecondSignal.Function = (data, t) =>
                {
                    return this.FirstSignal.Points.OrderBy(p => Math.Abs(p.X - t)).First().Y;
                };
            Generator.GenerateSignal(this.SecondSignal);
        }
    }
}