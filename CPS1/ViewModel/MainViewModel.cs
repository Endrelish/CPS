namespace CPS1.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Windows.Input;

    using CPS1.Model;
    using CPS1.View;

    public class MainViewModel
    {
        private readonly IFileDialog fileDialog;

        private readonly IFileSerializer serializer;

        private Signal firstSignalType = Signal.Sine;

        private ICommand generateSignalCommand;

        private ICommand openCommand;

        private ICommand saveCommand;

        private Signal secondSignalType = Signal.Sine;

        public MainViewModel()
        {
            this.SignalFirst = new FunctionData();
            this.SignalSecond = new FunctionData();

            this.SetRequiredParameters(this.SignalFirst);
            this.SetRequiredParameters(this.SignalSecond);

            this.fileDialog = new FileDialogWpf();
            this.serializer = new FileXmlSerializer();
        }

        public static Func<double, string> Formatter { get; } = value => value.ToString("N");

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

        public ICommand OpenCommand =>
            this.openCommand ?? (this.openCommand = new CommandHandler(this.OpenSignal, () => true));

        public int Param { get; } = 2;

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
                return AvailableFunctions.Functions.Select(p => p.Item4);
            }
        }

        private void GenerateSignal(object parameter)
        {
            if (parameter is short chart)
            {
                if (chart == 1)
                {
                    Generator.GenerateSignal(this.SignalFirst, this.firstSignalType);
                    Histogram.GetHistogram(this.SignalFirst);
                }
                else if (chart == 2)
                {
                    Generator.GenerateSignal(this.SignalSecond, this.secondSignalType);
                    Histogram.GetHistogram(this.SignalSecond);
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
                    //Histogram.GetHistogram(this.SignalFirst);
                    //this.SignalFirst.HistogramPointsUpdate();
                    //this.SignalFirst.PointsUpdate();
                    //this.SignalFirst.AllChanged();
                }
                else if (chart == 2)
                {
                    this.SignalSecond.AssignSignal(data);
                    //Histogram.GetHistogram(this.SignalSecond);
                    //this.SignalSecond.HistogramPointsUpdate();
                    //this.SignalSecond.PointsUpdate();
                    //this.SignalSecond.AllChanged();
                }
            }
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
    }
}