namespace CPS1
{
    using System.ComponentModel;

    using LiveCharts;

    public class MainViewModel : INotifyPropertyChanged
    {
        private SeriesCollection a;

        private FunctionData histogram;

        private FunctionData signal;

        public MainViewModel()
        {
            this.Generator = new RandomNoise();
            this.signal = new FunctionData();

            this.Generator.GeneratePoints(this.signal);
            this.histogram = new FunctionData();
            this.histogram.Points.AddRange(CPS1.Histogram.GetHistogram(this.signal, 10));
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}