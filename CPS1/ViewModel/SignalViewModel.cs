namespace CPS1.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Linq;
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

        public event PropertyChangedEventHandler PropertyChanged;

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

        public bool SignalGenerated => this.SignalData.Points.Count > 0;

        public string SignalType
        {
            get => AvailableFunctions.GetDescription(this.SignalData.Type);
            set
            {
                this.SignalData.Type = AvailableFunctions.GetTypeByDescription(value);
                this.SetRequiredParameters();
            }
        }

        public void Compose(SignalViewModel viewModel, Operation operation)
        {
            var data = viewModel.SignalData;
            if (operation == Operation.Divide && data.Points.Any(p => Math.Abs(p.Y) < double.Epsilon))
            {
                this.SignalData.Continuous.Value = false;
            }

            if (this.SignalData.Type != Signal.Composite)
            {
                this.SignalData.Function = AvailableFunctions.GetFunction(this.SignalData.Type);
            }

            if (data.Type != Signal.Composite)
            {
                data.Function = AvailableFunctions.GetFunction(data.Type);
            }

            this.SignalData.Type = Signal.Composite;
            if (data.Function != null && this.SignalData.Function != null)
            {
                // TODO Think about a proper way of implementing this feature
                this.SignalData.Continuous.Value = this.SignalData.Continuous.Value && data.Continuous.Value;
                this.SignalData.Function = FunctionComposer.ComposeFunction(
                    this.SignalData.Function,
                    data.Function,
                    data,
                    operation);
                Generator.GenerateSignal(this.SignalData);
            }
            else
            {
                this.SimpleCompose(data, operation);
                this.SignalData.PointsUpdate();
                Histogram.GetHistogram(this.SignalData);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            this.SignalData.RequiredAttributes = AvailableFunctions.GetRequiredParameters(this.SignalData.Type);
        }

        private void SimpleAdd(FunctionData data)
        {
            if (this.SignalData.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => p.X == point.X))
                    {
                        point.Y += data.Points.First(p => p.X == point.X).Y;
                    }
                    else
                    {
                        point.Y += (data.Points.First(a => a.X == data.Points.Where(p => p.X < point.X).Max(p => p.X)).Y
                                    + data.Points.First(a => a.X == data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                        .Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => p.X == point.X))
                    {
                        point.Y += data.Points.First(p => p.X == point.X).Y;
                    }
                }

                this.SignalData.Points.AddRange(
                    data.Points.Where(p => !this.SignalData.Points.Select(a => a.X).Contains(p.X)));
            }
        }

        private void SimpleCompose(FunctionData data, Operation operation)
        {
            switch (operation)
            {
                case Operation.Add:
                    this.SimpleAdd(data);
                    break;
                case Operation.Subtract:
                    this.SimpleSubtract(data);
                    break;
                case Operation.Multiply:
                    this.SimpleMultiply(data);
                    break;
                case Operation.Divide:
                    this.SimpleDivide(data);
                    break;
            }
        }

        private void SimpleDivide(FunctionData data)
        {
            if (this.SignalData.Continuous.Value && data.Continuous.Value)
            {
                for (var i = 0; i < this.SignalData.Points.Count; i++)
                {
                    var point = this.SignalData.Points[i];
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            this.SignalData.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        try
                        {
                            point.Y /=
                                (data.Points.First(
                                     a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                          < double.Epsilon).Y + data.Points.First(
                                     a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                          < double.Epsilon).Y) / 2.0d;
                        }
                        catch (DivideByZeroException)
                        {
                            this.SignalData.Points.RemoveAt(i--);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < this.SignalData.Points.Count; i++)
                {
                    var point = this.SignalData.Points[i];
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        try
                        {
                            point.Y /= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                        }
                        catch (DivideByZeroException)
                        {
                            this.SignalData.Points.RemoveAt(i--);
                        }
                    }
                    else
                    {
                        this.SignalData.Points.RemoveAt(i--);
                    }
                }
            }
        }

        private void SimpleMultiply(FunctionData data)
        {
            if (this.SignalData.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y *=
                            (data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y *= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y = 0;
                    }
                }
            }
        }

        private void SimpleSubtract(FunctionData data)
        {
            if (this.SignalData.Continuous.Value && data.Continuous.Value)
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                    else
                    {
                        point.Y -=
                            (data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X < point.X).Max(p => p.X))
                                      < double.Epsilon).Y + data.Points.First(
                                 a => Math.Abs(a.X - data.Points.Where(p => p.X > point.X).Min(p => p.X))
                                      < double.Epsilon).Y) / 2.0d;
                    }
                }
            }
            else
            {
                foreach (var point in this.SignalData.Points)
                {
                    if (data.Points.Any(p => Math.Abs(p.X - point.X) < double.Epsilon))
                    {
                        point.Y -= data.Points.First(p => Math.Abs(p.X - point.X) < double.Epsilon).Y;
                    }
                }

                this.SignalData.Points.AddRange(
                    data.Points.Where(p => !this.SignalData.Points.Select(a => a.X).Contains(p.X))
                        .Select(p => new Point(p.X, p.Y * -1)));
            }

            foreach (var point in this.SignalData.Points)
            {
                if (Math.Abs(point.Y) < 10E-10)
                {
                    point.Y = 0;
                }
            }
        }
    }
}