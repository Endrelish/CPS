using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CPS1.Model.CommandHandlers;
using CPS1.Model.SignalData;
using CPS1.Model.Transform.FourierTransform;
using CPS1.Model.Transform.WalshHadamardTransform;

namespace CPS1.ViewModel
{
    public class TransformViewModel
    {
        private ICommand fourierTransformCommand;

        private ICommand walshHadamardTransformCommand;

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
            FourierTransformResult.Points = SelectedFourierTransform.Transform(SignalData.Points).ToList();
        }

        private void WalshHadamardTransform(object obj)
        {
            WalshHadamardTransformResult.Points = SelectedWalshHadamardTransform.Transform(SignalData.Points).ToList();
        }
    }
}