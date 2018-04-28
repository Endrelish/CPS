namespace CPS1.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using CPS1.Model;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.FirstSignalViewModel = new SignalViewModel();
            this.SecondSignalViewModel = new SignalViewModel();

            this.CompositionViewModel = new CompositionViewModel(this.FirstSignalViewModel, this.SecondSignalViewModel);
            this.ConversionViewModel = new ConversionViewModel(this.FirstSignalViewModel, this.SecondSignalViewModel);
        }

        public CompositionViewModel CompositionViewModel { get; }

        public ConversionViewModel ConversionViewModel { get; }

        public SignalViewModel FirstSignalViewModel { get; }

        public SignalViewModel SecondSignalViewModel { get; }

    }
}