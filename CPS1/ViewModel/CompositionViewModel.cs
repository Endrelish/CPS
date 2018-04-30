namespace CPS1.ViewModel
{
    using System;
    using System.Windows.Input;

    using CPS1.Model;
    using CPS1.Model.CommandHandlers;
    using CPS1.Model.Composition;

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

            this.firstSignalViewModel.SignalGenerated += this.SignalsGenerated;
            this.secondSignalViewModel.SignalGenerated += this.SignalsGenerated;
        }

        private void SignalsGenerated(object sender, EventArgs args)
        {
            this.AddCommand.RaiseCanExecuteChanged();
            this.DivideCommand.RaiseCanExecuteChanged();
            this.MultiplyCommand.RaiseCanExecuteChanged();
            this.SubtractCommand.RaiseCanExecuteChanged();
            this.SwapCommand.RaiseCanExecuteChanged();
        }

        public bool AreSignalsGenerated()
        {
            return this.firstSignalViewModel.IsSignalGenerated && this.secondSignalViewModel.IsSignalGenerated;
        }

        public CommandHandler AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.AreSignalsGenerated));
        public CommandHandler DivideCommand => this.divideCommand
                                         ?? (this.divideCommand = new CommandHandler(
                                                 this.DivideSignals,
                                                 this.AreSignalsGenerated));
        public CommandHandler MultiplyCommand => this.multiplyCommand ?? (this.multiplyCommand = new CommandHandler(
                                                                        this.MultiplySignals,
                                                                        () => this.AreSignalsGenerated()));
        public CommandHandler SubtractCommand => this.subtractCommand ?? (this.subtractCommand = new CommandHandler(
                                                                        this.SubtractSignals,
                                                                        () => this.AreSignalsGenerated()));
        private void AddSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(this.firstSignalViewModel.SignalData, this.secondSignalViewModel.SignalData, Operation.Add);
                }
                else
                {
                    Operations.Compose(this.secondSignalViewModel.SignalData, this.firstSignalViewModel.SignalData, Operation.Add);
                }
            }
        }
        private void DivideSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(this.firstSignalViewModel.SignalData, this.secondSignalViewModel.SignalData, Operation.Divide);
                }
                else
                {
                    Operations.Compose(this.secondSignalViewModel.SignalData, this.firstSignalViewModel.SignalData, Operation.Divide);
                }
            }
        }
        private void MultiplySignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(this.firstSignalViewModel.SignalData, this.secondSignalViewModel.SignalData, Operation.Multiply);
                }
                else
                {
                    Operations.Compose(this.secondSignalViewModel.SignalData, this.firstSignalViewModel.SignalData, Operation.Multiply);
                }
            }
        }

        private void SubtractSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(this.firstSignalViewModel.SignalData, this.secondSignalViewModel.SignalData, Operation.Subtract);
                }
                else
                {
                    Operations.Compose(this.secondSignalViewModel.SignalData, this.firstSignalViewModel.SignalData, Operation.Subtract);
                }
            }
        }

        public CommandHandler SwapCommand => this.swapCommand
                                       ?? (this.swapCommand = new CommandHandler(
                                               this.SwapSignals,
                                               this.AreSignalsGenerated));

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