using System;
using System.Collections.Generic;
using CPS1.Model.CommandHandlers;
using CPS1.Model.Composition;
using CPS1.Model.SignalData;

namespace CPS1.ViewModel
{
    public class CompositionViewModel
    {
        private readonly SignalViewModel firstSignalViewModel;

        private readonly SignalViewModel secondSignalViewModel;
        private CommandHandler addCommand;
        private CommandHandler convolutionCommand;
        private CommandHandler correlationCommand;
        private CommandHandler divideCommand;
        private CommandHandler multiplyCommand;
        private CommandHandler subtractCommand;
        private CommandHandler swapCommand;

        public CompositionViewModel(SignalViewModel first, SignalViewModel second)
        {
            firstSignalViewModel = first;
            secondSignalViewModel = second;

            firstSignalViewModel.SignalGenerated += SignalsGenerated;
            secondSignalViewModel.SignalGenerated += SignalsGenerated;
        }

        public CommandHandler AddCommand => addCommand
                                            ?? (addCommand = new CommandHandler(AddSignals, AreSignalsGenerated));

        public CommandHandler CorrelationCommand => correlationCommand
                                                    ?? (correlationCommand =
                                                        new CommandHandler(Correlation, AreSignalsGenerated));

        public CommandHandler ConvolutionCommand => convolutionCommand
                                                    ?? (convolutionCommand =
                                                        new CommandHandler(Convolution, AreSignalsGenerated));

        public CommandHandler DivideCommand => divideCommand
                                               ?? (divideCommand = new CommandHandler(
                                                   DivideSignals,
                                                   AreSignalsGenerated));

        public CommandHandler MultiplyCommand => multiplyCommand ?? (multiplyCommand = new CommandHandler(
                                                     MultiplySignals,
                                                     () => AreSignalsGenerated()));

        public CommandHandler SubtractCommand => subtractCommand ?? (subtractCommand = new CommandHandler(
                                                     SubtractSignals,
                                                     () => AreSignalsGenerated()));

        public CommandHandler SwapCommand => swapCommand
                                             ?? (swapCommand = new CommandHandler(
                                                 SwapSignals,
                                                 AreSignalsGenerated));

        private void Correlation(object obj)
        {
            var points = new List<Point>();
            points.AddRange(secondSignalViewModel.SignalData.Points);
            secondSignalViewModel.SignalData.Points.Clear();
            secondSignalViewModel.SignalData.Points.AddRange(
                Model.ConvolutionFiltrationCorrelation.Correlation.Correlate(firstSignalViewModel.SignalData.Points,
                    points));
            secondSignalViewModel.SignalData.PointsUpdate();
        }

        private void Convolution(object obj)
        {
            var points = new List<Point>();
            points.AddRange(secondSignalViewModel.SignalData.Points);
            secondSignalViewModel.SignalData.Points.Clear();
            secondSignalViewModel.SignalData.Points.AddRange(
                Model.ConvolutionFiltrationCorrelation.Convolution.Convolute(firstSignalViewModel.SignalData.Points,
                    points));
            secondSignalViewModel.SignalData.PointsUpdate();
        }

        private void SignalsGenerated(object sender, EventArgs args)
        {
            AddCommand.RaiseCanExecuteChanged();
            DivideCommand.RaiseCanExecuteChanged();
            MultiplyCommand.RaiseCanExecuteChanged();
            SubtractCommand.RaiseCanExecuteChanged();
            SwapCommand.RaiseCanExecuteChanged();
            CorrelationCommand.RaiseCanExecuteChanged();
            ConvolutionCommand.RaiseCanExecuteChanged();
        }

        public bool AreSignalsGenerated()
        {
            return firstSignalViewModel.IsSignalGenerated && secondSignalViewModel.IsSignalGenerated;
        }

        private void AddSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(firstSignalViewModel.SignalData, secondSignalViewModel.SignalData,
                        Operation.Add);
                }
                else
                {
                    Operations.Compose(secondSignalViewModel.SignalData, firstSignalViewModel.SignalData,
                        Operation.Add);
                }
            }
        }

        private void DivideSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(firstSignalViewModel.SignalData, secondSignalViewModel.SignalData,
                        Operation.Divide);
                }
                else
                {
                    Operations.Compose(secondSignalViewModel.SignalData, firstSignalViewModel.SignalData,
                        Operation.Divide);
                }
            }
        }

        private void MultiplySignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(firstSignalViewModel.SignalData, secondSignalViewModel.SignalData,
                        Operation.Multiply);
                }
                else
                {
                    Operations.Compose(secondSignalViewModel.SignalData, firstSignalViewModel.SignalData,
                        Operation.Multiply);
                }
            }
        }

        private void SubtractSignals(object obj)
        {
            if (obj is short no)
            {
                if (no == 1)
                {
                    Operations.Compose(firstSignalViewModel.SignalData, secondSignalViewModel.SignalData,
                        Operation.Subtract);
                }
                else
                {
                    Operations.Compose(secondSignalViewModel.SignalData, firstSignalViewModel.SignalData,
                        Operation.Subtract);
                }
            }
        }

        private void SwapSignals(object obj)
        {
            var fd = firstSignalViewModel.SignalData.Copy;
            firstSignalViewModel.SignalData.AssignSignal(secondSignalViewModel.SignalData);
            secondSignalViewModel.SignalData.AssignSignal(fd);
            firstSignalViewModel.SignalData.PointsUpdate();
            secondSignalViewModel.SignalData.PointsUpdate();
        }
    }
}