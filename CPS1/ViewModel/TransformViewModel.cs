using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CPS1.Model.CommandHandlers;
using CPS1.Model.SignalData;
using CPS1.Model.Transform.FourierTransform;

namespace CPS1.ViewModel
{
    public class TransformViewModel
    {
        public FunctionData SignalData { get; }
        public FunctionData Transform { get; }

        public TransformViewModel(FunctionData data)
        {
            SignalData = data;
            Transform = new FunctionData(continuous:false);
        }
        
        private ICommand fourierTransformCommand;

        public ICommand FourierTransformCommand => fourierTransformCommand ??
                                                   (fourierTransformCommand =
                                                       new CommandHandler(FourierTransform, () => true));

        private void FourierTransform(object obj)
        {
            var fft = new DiscreteFourierTransform();
            Transform.Points = fft.Transform(SignalData.Points).ToList();
        }
    }
}