namespace CPS1.ViewModel
{
    using System.Windows.Input;

    using CPS1.Model;

    public class CompositionViewModel
    {
        private CommandHandler addCommand;
        private CommandHandler divideCommand;
        private CommandHandler multiplyCommand;
        private CommandHandler subtractCommand;
        private CommandHandler swapCommand;

        private SignalViewModel firstSignalViewModel;

        private SignalViewModel secondSignalViewModel;

        public CompositionViewModel(SignalViewModel first, SignalViewModel second)
        {
            this.firstSignalViewModel = first;
            this.secondSignalViewModel = second;

            this.firstSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    this.AddCommand.RaiseCanExecuteChanged();
                    this.SubtractCommand.RaiseCanExecuteChanged();
                    this.MultiplyCommand.RaiseCanExecuteChanged();
                    this.DivideCommand.RaiseCanExecuteChanged();
                };

            this.secondSignalViewModel.SignalData.PropertyChanged += (sender, args) =>
                {
                    this.AddCommand.RaiseCanExecuteChanged();
                    this.SubtractCommand.RaiseCanExecuteChanged();
                    this.MultiplyCommand.RaiseCanExecuteChanged();
                    this.DivideCommand.RaiseCanExecuteChanged();
                };
        }

        public bool SignalsGenerated()
        {
            return this.firstSignalViewModel.SignalGenerated && this.secondSignalViewModel.SignalGenerated;
        }

        public CommandHandler AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.SignalsGenerated));
        public CommandHandler DivideCommand => this.divideCommand
                                         ?? (this.divideCommand = new CommandHandler(
                                                 this.DivideSignals,
                                                 this.SignalsGenerated));
        public CommandHandler MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.SignalsGenerated()));
        public CommandHandler SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
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

        public CommandHandler SwapCommand => this.swapCommand
                                       ?? (this.swapCommand = new CommandHandler(
                                               this.SwapSignals,
                                               this.SignalsGenerated));

        private void SwapSignals(object obj)
        {
            var fd = this.firstSignalViewModel.SignalData.Copy;
            this.firstSignalViewModel.SignalData.AssignSignal(this.secondSignalViewModel.SignalData);
            this.secondSignalViewModel.SignalData.AssignSignal(fd);
            this.firstSignalViewModel.SignalData.PointsUpdate();
            this.secondSignalViewModel.SignalData.PointsUpdate();
        }


    }
}