using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CPS1.Annotations;
using CPS1.Model.CommandHandlers;
using CPS1.Model.Generation;
using CPS1.Model.Serialization;
using CPS1.Model.SignalData;
using CPS1.Properties;
using CPS1.View;

namespace CPS1.ViewModel
{
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
            SignalData = new FunctionData();
            fileDialog = new FileDialogWpf();
            serializer = new FileBinarySerializer();
        }

        public CommandHandler ClearCommand =>
            clearCommand ?? (clearCommand = new CommandHandler(Clear, () => true));

        public CommandHandler GenerateSignalCommand => generateSignalCommand
                                                       ?? (generateSignalCommand = new CommandHandler(
                                                           GenerateSignal,
                                                           () => true));

        public CommandHandler OpenCommand =>
            openCommand ?? (openCommand = new CommandHandler(OpenSignal, () => true));

        public CommandHandler SaveCommand =>
            saveCommand ?? (saveCommand = new CommandHandler(SaveSignal, () => true));

        public FunctionData SignalData { get; }


        public bool IsSignalGenerated => SignalData.Points.Count > 0;

        public string SignalType
        {
            get => AvailableFunctions.GetDescription(SignalData.Type);
            set => SignalData.Type = AvailableFunctions.GetTypeByDescription(value);
        }

        public IEnumerable<string> SignalsLabels
        {
            get { return AvailableFunctions.Functions.Values.Select(p => p.Item3); }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Clear(object parameter)
        {
            SignalData.AssignSignal(new FunctionData());
            SignalData.PointsUpdate();
        }

        private void GenerateSignal(object parameter)
        {
            Generator.GenerateSignal(SignalData);
            Histogram.GetHistogram(SignalData);

            SignalGenerated?.Invoke(this, null);
        }

        public event EventHandler SignalGenerated;


        private void OpenSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = fileDialog.GetOpenFilePath(serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                var data = serializer.Deserialize(filename);

                SignalData.AssignSignal(data);
            }
        }

        private void SaveSignal(object parameter)
        {
            if (parameter is short chart)
            {
                var filename = fileDialog.GetSaveFilePath(serializer.Format);
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }

                serializer.Serialize(SignalData, filename);
            }
        }
    }
}