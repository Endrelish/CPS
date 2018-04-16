namespace CPS1.ViewModel
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using CPS1.Annotations;
    using CPS1.Model;
    using CPS1.View;

    public class GeneratorViewModel : INotifyPropertyChanged
    {
        private readonly IFileDialog fileDialog;

        private readonly IFileSerializer serializer;

        private ICommand generateSignalCommand;

        public GeneratorViewModel()
        {
            this.FunctionData = new FunctionData();
            this.serializer = new FileBinarySerializer();
            this.fileDialog = new FileDialogWpf();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FunctionData FunctionData { get; set; }

        public ICommand GenerateSignalCommand => this.generateSignalCommand
                                                 ?? (this.generateSignalCommand = new CommandHandler(
                                                         this.GenerateSignal,
                                                         () => true));

        public string SignalType
        {
            get => AvailableFunctions.GetDescription(this.FunctionData.Type);
            set => this.FunctionData.Type = AvailableFunctions.GetTypeByDescription(value);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GenerateSignal(object parameter)
        {
            this.FunctionData.Function = AvailableFunctions.GetFunction(this.FunctionData.Type);
            Generator.GenerateSignal(this.FunctionData);
            Histogram.GetHistogram(this.FunctionData);

            this.OnPropertyChanged(nameof(this.FunctionData));
        }

        private void SaveSignal(object parameter)
        {
            var filename = this.fileDialog.GetSaveFilePath(this.serializer.Format);
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            this.serializer.Serialize(this.FunctionData, filename);
        }
    }
}