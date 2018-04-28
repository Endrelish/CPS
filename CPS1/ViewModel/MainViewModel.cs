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

    public class MainViewModel : INotifyPropertyChanged
    {
       


        public MainViewModel()
        {
            this.FirstSignalViewModel = new SignalViewModel();
            this.SecondSignalViewModel = new SignalViewModel();
            this.CompositionViewModel = new CompositionViewModel(this.FirstSignalViewModel, this.SecondSignalViewModel);

           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CompositionViewModel CompositionViewModel { get; }

        
        public SignalViewModel FirstSignalViewModel { get; }


        public SignalViewModel SecondSignalViewModel { get; }

        public IEnumerable<string> SignalsLabels
        {
            get
            {
                return AvailableFunctions.Functions.Values.Select(p => p.Item3);
            }
        }

        public bool SignalsGenerated()
        {
            return this.CompositionViewModel.SignalsGenerated();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}