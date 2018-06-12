using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using CPS1.Annotations;
using CPS1.Model.CommandHandlers;
using CPS1.Model.Parameters;
using CPS1.Model.SignalData;
using CPS1.Model.Transform.FourierTransform;
using CPS1.Model.Transform.WalshHadamardTransform;

namespace CPS1.ViewModel
{
    public class TransformViewModel : INotifyPropertyChanged, IParametersProvider
    {
        private ICommand fourierTransformCommand;

        private ICommand walshHadamardTransformCommand;
        private Parameter _elapsedTime;

        public TransformViewModel(FunctionData data)
        {
            SignalData = data;
            FourierTransformResult = new FunctionData(continuous: false);
            WalshHadamardTransformResult = new FunctionData(continuous: false);

            FourierTransforms = new List<FourierTransform>(new FourierTransform[]
                {new DiscreteFourierTransform(), new FastFourierTransform()});
            WalshHadamardTransforms = new List<WalshHadamardTransform>(new WalshHadamardTransform[]
                {new DiscreteWalshHadamardTransform(), new FastWalshHadamardTransform()});

            SelectedFourierTransform = FourierTransforms.ElementAt(0);
            SelectedWalshHadamardTransform = WalshHadamardTransforms.ElementAt(0);

            ElapsedTime = new Parameter(0.0d, "LAST OPERATION DURATION");
            Parameters = new List<Parameter>(new []{ElapsedTime});
        }

        public Parameter ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (Equals(value, _elapsedTime)) return;
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }

        public FunctionData SignalData { get; }
        public FunctionData FourierTransformResult { get; }
        public FunctionData WalshHadamardTransformResult { get; }

        public IEnumerable<FourierTransform> FourierTransforms { get; set; }
        public IEnumerable<WalshHadamardTransform> WalshHadamardTransforms { get; set; }

        public FourierTransform SelectedFourierTransform { get; set; }
        public WalshHadamardTransform SelectedWalshHadamardTransform { get; set; }

        public ICommand FourierTransformCommand => fourierTransformCommand ??
                                                   (fourierTransformCommand =
                                                       new CommandHandler(FourierTransform, () => true));

        public ICommand WalshHadamardTransformCommand => walshHadamardTransformCommand ??
                                                         (walshHadamardTransformCommand =
                                                             new CommandHandler(WalshHadamardTransform, () => true));

        private void FourierTransform(object obj)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            FourierTransformResult.Points = SelectedFourierTransform.Transform(SignalData.Points.ToArray()).ToList();
            s.Stop();
            ElapsedTime.Value = s.ElapsedMilliseconds / 1000.0d;
        }

        private void WalshHadamardTransform(object obj)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            WalshHadamardTransformResult.Points = SelectedWalshHadamardTransform.Transform(SignalData.Points.ToArray()).ToList();
            s.Stop();
            ElapsedTime.Value = s.ElapsedMilliseconds / 1000.0d;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<Parameter> Parameters { get; }
    }
}