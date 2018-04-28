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

        private readonly Func<double, double> sincFunc = t =>
            {
                if (t == 0)
                {
                    return 1;
                }

                return Math.Sin(Math.PI * t) / (Math.PI * t);
            };


        private string adOperation;
        
        private ICommand computeCommand;

        private string daOperation;

        
        private FunctionAttribute<double> maximumDifference;

        private FunctionAttribute<double> meanSquaredError;

        
        private FunctionAttribute<double> peakSignalToNoiseRatio;

        private int quantizationLevels;

        private bool quantizationLevelsVisibility;

        private bool samplingFrequencyVisibility;
        
        private FunctionAttribute<double> signalToNoiseRatio;


        private ICommand swapCommand;

        public SignalViewModel FirstSignalViewModel { get; set; }
        public SignalViewModel SecondSignalViewModel { get; set; }

        public MainViewModel()
        {
            this.FirstSignalViewModel = new SignalViewModel();
            this.SecondSignalViewModel = new SignalViewModel();


            this.FirstSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SecondSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.SamplingFrequency = 10;
            this.QuantizationLevels = 25;

            this.AdOperations = new List<string> { "UNIFORM SAMPLING", "UNIFORM QUANTIZATION" };
            this.DaOperations = new List<string> { "ZERO-ORDER HOLD", "SINC RECONSTRUCTION" };
            this.AdOperation = string.Empty;
            this.DaOperation = string.Empty;

            this.MaximumDifference = new FunctionAttribute<double>(0.0d, true, 0, 0, "MAXIMUM DIFFERENCE");
            this.SignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "SIGNAL TO NOISE RATIO");
            this.MeanSquaredError = new FunctionAttribute<double>(0, true, 0, 0, "MEAN SQUARED ERROR");
            this.PeakSignalToNoiseRatio = new FunctionAttribute<double>(0, true, 0, 0, "PEAK SIGNAL TO NOISE RATIO");

            // TUTAJ TERAZ TAKIE COŚ

            // SignalSecond.Period.Value = 0.5;
            // GenerateSignal((short)1);
            // GenerateSignal((short)2);

            // AddSignals((short)1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

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
                                                   () => this.SignalFirst.Continuous.Value
                                                         && this.AdOperation.Length > 0
                                                         || !this.SignalFirst.Continuous.Value
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

        public ICommand MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.SignalFirst.Points.Count > 0
                                                                              && this.SignalSecond.Points.Count > 0));

        
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
        
        public IEnumerable<string> SignalsLabels
        {
            get
            {
                return AvailableFunctions.Functions.Values.Select(p => p.Item3);
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

        public ICommand SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
                                                                        this.SubtractSignals,
                                                                        () => this.SignalsGenerated()));

        public ICommand SwapCommand => this.swapCommand
                                       ?? (this.swapCommand = new CommandHandler(
                                               this.SwapSignals,
                                               this.SignalsGenerated));

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

        private void CalculateMetrics()
        {
            this.Mse();
            this.Snr();
            this.Psnr();
            this.Md();
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
                    this.SignalFirst.Compose(this.SignalSecond, Operation.Divide);
                }
                else
                {
                    this.SignalSecond.Compose(this.SignalFirst, Operation.Divide);
                }
            }
        }

       
        private void Md()
        {
            this.MaximumDifference.Value = this.SignalSecond.Points
                .Select(p => Math.Abs(p.Y - this.SignalFirst.Function(this.SignalFirst, p.X))).Max();
        }

        private void Mse()
        {
            this.MeanSquaredError.Value =
                this.SignalSecond.Points
                    .Select(p => Math.Pow(this.SignalFirst.Function(this.SignalFirst, p.X) - p.Y, 2)).Sum()
                / this.SignalSecond.Points.Count;
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

        

        private void Psnr()
        {
            this.PeakSignalToNoiseRatio.Value =
                10 * Math.Log10(this.SignalFirst.Points.Max(p => p.Y) / this.MeanSquaredError.Value);
        }

        private void Quantization()
        {
            var levels = new List<double>();
            var step = this.SignalFirst.Amplitude.Value * 2.0d / (this.QuantizationLevels - 1);
            for (var i = 0; i <= this.QuantizationLevels - 1; i++)
            {
                levels.Add(-this.SignalFirst.Amplitude.Value + i * step);
            }

            this.SignalSecond.AssignSignal(this.SignalFirst);

            foreach (var point in this.SignalSecond.Points)
            {
                point.Y = levels.OrderBy(l => Math.Abs(point.Y - l)).First();
            }

            this.SignalSecond.PointsUpdate();
        }

        private void Sampling()
        {
            this.SignalSecond.AssignSignal(this.SignalFirst);
            this.SignalSecond.Continuous.Value = false;
            this.SignalSecond.Samples.Value = (int)(this.SignalSecond.Duration.Value * this.SamplingFrequency);
            Generator.GenerateSignal(this.SignalSecond);
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
            this.SignalSecond.AssignSignal(this.SignalFirst);
            this.SignalSecond.Continuous.Value = true;
            this.SignalSecond.Samples.Value = 500;

            this.SignalSecond.Function = (data, t) =>
                {
                    var n = this.SignalFirst.Points.Count(p => p.X < t);

                    var sum = 0.0d;
                    var ts = this.SignalFirst.Duration.Value / this.SignalFirst.Samples.Value;
                    for (var i = n - this.QuantizationLevels; i < n + this.QuantizationLevels; i++)
                    {
                        if (i >= 0 && i < this.SignalFirst.Points.Count)
                        {
                            
                            sum += this.SignalFirst.Points[i].Y
                                   * this.sincFunc((t - this.SignalFirst.Points[i].X) / ts);
                        }
                    }
                    
                    return sum;
                };

            Generator.GenerateSignal(this.SignalSecond);
        }

        private void Snr()
        {
            var numerator = this.SignalSecond.Points.Select(p => this.SignalFirst.Function(this.SignalFirst, p.X))
                .Sum();
            var denominator = this.SignalSecond.Points
                .Select(p => Math.Pow(this.SignalFirst.Function(this.SignalFirst, p.X) - p.Y, 2)).Sum();

            this.SignalToNoiseRatio.Value = 10 * Math.Abs(Math.Log10(Math.Abs(numerator / denominator)));
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

        private void SwapSignals(object obj)
        {
            var fd = this.SignalFirst.Copy;
            this.SignalFirst.AssignSignal(this.SignalSecond);
            this.SignalSecond.AssignSignal(fd);
            this.SignalFirst.PointsUpdate();
            this.SignalSecond.PointsUpdate();
        }

        private void ZeroOrderHold()
        {
            this.SignalSecond.AssignSignal(this.SignalFirst);
            this.SignalSecond.Continuous.Value = true;
            this.SignalSecond.Samples.Value = 500;

            this.SignalSecond.Function = (data, t) =>
                {
                    var ret = 0.0d;
                    if (this.SignalFirst.Points.Any(p => p.X < t))
                    {
                        ret = this.SignalFirst.Points.Where(p => p.X < t).OrderBy(p => -p.X).First().Y;
                    }
                    else
                    {
                        ret = this.SignalFirst.Points.OrderBy(p => Math.Abs(p.X - t)).First().Y;
                    }

                    return ret;
                };
            Generator.GenerateSignal(this.SignalSecond);
        }
    }
}