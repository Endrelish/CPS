namespace CPS1
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;

    using CPS1.Functions;

    using org.mariuszgromada.math.mxparser;

    public class MainViewModel : INotifyPropertyChanged
    {
        private ICommand clickCommand;

        private FunctionData histogramFirst;

        private FunctionData signalFirst;

        public MainViewModel()
        {
            this.Generator = new SquareWave();
            //this.signal = new FunctionData();

            //this.Generator.GeneratePoints(this.signal);
            //this.histogram = new FunctionData();
            //this.histogram.Points.AddRange(CPS1.Histogram.GetHistogram(this.signal, 10));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ClickCommand
        {
            get
            {
                return this.clickCommand ?? (this.clickCommand = new CommandHandler(() => this.Compute(), true));
            }
        }

        private FunctionData histogramSecond;

        private FunctionData signalSecond;

        public FunctionData HistogramSecond
        {
            get => this.histogramSecond;
            set
            {
                this.histogramSecond = value;
                OnPropertyChanged("HistogramSecond");
            } 
        }

        public FunctionData SignalSecond
        {
            get => this.signalSecond;
            set
            {
                this.signalSecond = value;
                OnPropertyChanged("SignalSecond");
            }
        }

        public IFunction Generator { get; set; }

        public FunctionData HistogramFirst
        {
            get => this.histogramFirst;
            set
            {
                this.histogramFirst = value;
                this.OnPropertyChanged("HistogramFirst");
            }
        }

        public FunctionData SignalFirst
        {
            get => this.signalFirst;
            set
            {
                this.signalFirst = value;
                this.OnPropertyChanged("SignalFirst");
            }
        }

        public string Text { get; set; } = "PI / 2";

        public double Value { get; set; } = 3;

        public void Compute()
        {
            this.Value = 9;
            var pi = new Constant("PI", Math.PI);
            var e1 = new Expression(this.Text);
            e1.addConstants(pi);
            this.Value = e1.calculate();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}