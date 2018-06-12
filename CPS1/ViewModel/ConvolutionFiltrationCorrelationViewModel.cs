using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CPS1.Annotations;
using CPS1.Model.CommandHandlers;
using CPS1.Model.ConvolutionFiltrationCorrelation.Filters;
using CPS1.Model.ConvolutionFiltrationCorrelation.Windows;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class ConvolutionFiltrationCorrelationViewModel : INotifyPropertyChanged
    {
        private readonly FunctionAttribute<double> cutoffFrequency;

        private readonly Filter filter;

        private CommandHandler filterCommand;

        private readonly FunctionAttribute<int> filterOrder;
        private readonly Dictionary<FilterType, string> filters;

        private readonly SignalViewModel firstSignalViewModel;

        private FilterType selectedFilter;
        private IWindow window;

        public ConvolutionFiltrationCorrelationViewModel(SignalViewModel first)
        {
            firstSignalViewModel = first;
            SecondSignalData = new FunctionData();
            filter = new Filter();

            filters = new Dictionary<FilterType, string>();
            filters.Add(FilterType.LowPassFilter, "LOW-PASS FILTER");
            filters.Add(FilterType.HighPassFilter, "HIGH-PASS FILTER");

            selectedFilter = FilterType.LowPassFilter;
            window = new RectangularWindow();
            windows = new List<IWindow>(new IWindow[]
                {new RectangularWindow(), new HanningWindow() /*, new HammingWindow(), new BlackmanWindow()*/});

            filterOrder = new FunctionAttribute<int>(5, true, 1, 500, "FILTER ORDER");
            cutoffFrequency = new FunctionAttribute<double>(400.0d, true, 10.0d, 25000.0d, "CUTOFF FREQUENCY");

            Attributes = new List<object>(new[] {filterOrder, (object) cutoffFrequency});

            firstSignalViewModel.SignalGenerated += (sender, args) => FilterCommand.RaiseCanExecuteChanged();
        }

        public CommandHandler FilterCommand => filterCommand ?? (filterCommand =
                                                   new CommandHandler(Filtration,
                                                       () => firstSignalViewModel.IsSignalGenerated));

        public IEnumerable<string> Filters => filters.Select(f => f.Value);

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

        private void Filtration(object obj)
        {
            SecondSignalData.Points.Clear();
            SecondSignalData.Points = filter.Filtration(filterOrder.Value, cutoffFrequency.Value,
                FirstSignalData.Points, window, selectedFilter).ToList();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}