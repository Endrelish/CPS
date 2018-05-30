using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation.Filters;
using CPS1.Model.ConvolutionFiltrationCorrelation.Windows;
using CPS1.Model.SignalData;
using CPS1.Properties;

namespace CPS1.ViewModel
{
    public class ConvolutionFiltrationCorrelationViewModel : INotifyPropertyChanged
    {
        private Dictionary<FilterType, string> filters;

        private FilterType selectedFilter;
        private IWindow window;

        private Filter filter;

        public ConvolutionFiltrationCorrelationViewModel(SignalViewModel first)
        {
            this.firstSignalViewModel = first;
            this.SecondSignalData = new FunctionData();
            this.filter = new Filter();

            this.filters = new Dictionary<FilterType, string>();
            this.filters.Add(FilterType.LowPassFilter, "LOW-PASS FILTER");
            this.filters.Add(FilterType.HighPassFilter, "HIGH-PASS FILTER");

            this.selectedFilter = FilterType.LowPassFilter;
            this.window = new RectangularWindow();
            this.windows = new List<IWindow>(new IWindow[]{new RectangularWindow(), new HanningWindow()/*, new HammingWindow(), new BlackmanWindow()*/});

            this.filterOrder = new FunctionAttribute<int>(5, true, 1, 500, "FILTER ORDER");
            this.cutoffFrequency = new FunctionAttribute<double>(400.0d, true, 10.0d, 25000.0d, "CUTOFF FREQUENCY");

            this.Attributes = new List<object>(new []{(object)filterOrder, (object)cutoffFrequency});

            this.firstSignalViewModel.SignalGenerated += ((sender, args) => this.FilterCommand.RaiseCanExecuteChanged());
        }

        private CommandHandler filterCommand;

        public CommandHandler FilterCommand => this.filterCommand ?? (this.filterCommand =
                                                   new CommandHandler(this.Filtration,
                                                       () => firstSignalViewModel.IsSignalGenerated));

        private void Filtration(object obj)
        {
            this.SecondSignalData.Points.Clear();
            this.SecondSignalData.Points = filter.Filtration(filterOrder.Value, cutoffFrequency.Value,
                FirstSignalData.Points, window, selectedFilter).ToList();
        }

        public IEnumerable<string> Filters => filters.Select(f => f.Value);

        private FunctionAttribute<int> filterOrder;
        private FunctionAttribute<double> cutoffFrequency;

        public IEnumerable<object> Attributes { get; }

        public string SelectedFilter
        {
            get => filters[selectedFilter];
            set
            {
                selectedFilter = filters.First(f => f.Value.Equals(value)).Key;
                OnPropertyChanged();
            }
        }

        private SignalViewModel firstSignalViewModel;
        public FunctionData FirstSignalData => firstSignalViewModel.SignalData;
        public FunctionData SecondSignalData { get; }

        public string Window
        {
            get => window.Name;
            set
            {
                if (value.Equals(window.Name))
                {
                    return;
                }

                window = windows.First(w => w.Name.Equals(value));
                OnPropertyChanged();
            }
        }

        private IEnumerable<IWindow> windows { get; }

        public IEnumerable<string> Windows
        {
            get { return windows.Select(w => w.Name); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}