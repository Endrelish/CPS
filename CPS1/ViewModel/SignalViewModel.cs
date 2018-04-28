﻿namespace CPS1.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using CPS1.Annotations;
    using CPS1.Model;
    using CPS1.View;

    public class SignalViewModel : INotifyPropertyChanged
    {

        private readonly IFileDialog fileDialog;

        private readonly IFileSerializer serializer;


        private CommandHandler clearCommand;

        private CommandHandler generateSignalCommand;

        private CommandHandler openCommand;

        private CommandHandler saveCommand;

        public SignalViewModel()
        {
            this.SignalData = new FunctionData();
            this.fileDialog = new FileDialogWpf();
            this.serializer = new FileBinarySerializer();
        }

        public CommandHandler ClearCommand =>
            this.clearCommand ?? (this.clearCommand = new CommandHandler(this.Clear, () => true));

        public CommandHandler GenerateSignalCommand => this.generateSignalCommand
                                                       ?? (this.generateSignalCommand = new CommandHandler(
                                                               this.GenerateSignal,
                                                               () => true));

        public CommandHandler OpenCommand =>
            this.openCommand ?? (this.openCommand = new CommandHandler(this.OpenSignal, () => true));

        public CommandHandler SaveCommand =>
            this.saveCommand ?? (this.saveCommand = new CommandHandler(this.SaveSignal, () => true));

        public FunctionData SignalData { get; set; }

        public string SignalType
        {
            get => AvailableFunctions.GetDescription(this.SignalData.Type);
            set
            {
                this.SignalData.Type = AvailableFunctions.GetTypeByDescription(value);
                this.SetRequiredParameters();
            }
        }

        private void Clear(object parameter)
        {
            this.SignalData.AssignSignal(new FunctionData());
            this.SignalData.PointsUpdate();
        }

        private void GenerateSignal(object parameter)
        {
            if (this.SignalData.Type != Signal.Composite)
            {
                this.SignalData.Function = AvailableFunctions.GetFunction(this.SignalData.Type);
            }

            Generator.GenerateSignal(this.SignalData);
            Histogram.GetHistogram(this.SignalData);

            // TODO
            // ((CommandHandler)this.ComputeCommand).RaiseCanExecuteChanged();
            // ((CommandHandler)this.SwapCommand).RaiseCanExecuteChanged();
        }

        private void OpenSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = this.fileDialog.GetOpenFilePath(this.serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                var data = this.serializer.Deserialize(filename);
                
                    this.SignalData.AssignSignal(data);

                    // Histogram.GetHistogram(this.SignalFirst);
                    // this.SignalFirst.HistogramPointsUpdate();
                    // this.SignalFirst.PointsUpdate();
                    // this.SignalFirst.AllChanged();
            }
        }

        private void SaveSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = this.fileDialog.GetSaveFilePath(this.serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }
                
                    this.serializer.Serialize(this.SignalData, filename);
            }
        }
        
        private void SetRequiredParameters()
        {
            this.SignalData.RequiredAttributes = AvailableFunctions.GetRequiredParameters(SignalData.Type);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}