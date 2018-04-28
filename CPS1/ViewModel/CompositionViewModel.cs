namespace CPS1.ViewModel
{
    using System.Windows.Input;

    using CPS1.Model;

    public class CompositionViewModel
    {
        private ICommand addCommand;
        private ICommand divideCommand;
        private ICommand multiplyCommand;
        private ICommand subtractCommand;

        private SignalViewModel firstSignalViewModel;

        private SignalViewModel secondSignalViewModel;

        public CompositionViewModel(SignalViewModel first, SignalViewModel second)
        {
            this.firstSignalViewModel = first;
            this.secondSignalViewModel = second;

            this.firstSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };

            this.secondSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    ((CommandHandler)this.AddCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.SubtractCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.MultiplyCommand).RaiseCanExecuteChanged();
                    ((CommandHandler)this.DivideCommand).RaiseCanExecuteChanged();
                };
        }

        public bool SignalsGenerated()
        {
            return this.firstSignalViewModel.SignalGenerated && this.secondSignalViewModel.SignalGenerated;
        }

        public ICommand AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.SignalsGenerated));
        public ICommand DivideCommand => this.divideCommand
                                         ?? (this.divideCommand = new CommandHandler(
                                                 this.DivideSignals,
                                                 this.SignalsGenerated));
        public ICommand MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.SignalsGenerated()));
        public ICommand SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
                                                                        this.SubtractSignals,
                                                                        () => this.SignalsGenerated()));
        private void AddSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.firstSignalViewModel.Compose(this.secondSignalViewModel, Operation.Add);
                }
                else
                {
                    this.secondSignalViewModel.Compose(this.firstSignalViewModel, Operation.Add);
                }
            }
        }
        private void DivideSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.firstSignalViewModel.Compose(this.secondSignalViewModel, Operation.Divide);
                }
                else
                {
                    this.secondSignalViewModel.Compose(this.firstSignalViewModel, Operation.Divide);
                }
            }
        }
        private void MultiplySignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.firstSignalViewModel.Compose(this.secondSignalViewModel, Operation.Multiply);
                }
                else
                {
                    this.secondSignalViewModel.Compose(this.firstSignalViewModel, Operation.Multiply);
                }
            }
        }

        private void SubtractSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    this.firstSignalViewModel.Compose(this.secondSignalViewModel, Operation.Subtract);
                }
                else
                {
                    this.secondSignalViewModel.Compose(this.firstSignalViewModel, Operation.Subtract);
                }
            }
        }

    }
}