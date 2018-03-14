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

        private FunctionData histogram;

        private FunctionData signal;

        public MainViewModel()
        {
            this.Generator = new SquareWave();
            this.signal = new FunctionData();

            this.Generator.GeneratePoints(this.signal);
            this.histogram = new FunctionData();
            this.histogram.Points.AddRange(CPS1.Histogram.GetHistogram(this.signal, 10));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ClickCommand
        {
            get
            {
                return this.clickCommand ?? (this.clickCommand = new CommandHandler(() => this.Compute(), true));
            }
        }

        public IFunction Generator { get; set; }

        public FunctionData Histogram
        {
            get => this.histogram;
            set
            {
                this.histogram = value;
                this.OnPropertyChanged("Histogram");
            }
        }

        public FunctionData Signal
        {
            get => this.signal;
            set
            {
                this.signal = value;
                this.OnPropertyChanged("Signal");
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