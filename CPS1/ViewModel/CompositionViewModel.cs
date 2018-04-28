namespace CPS1.ViewModel
{
    using System.Windows.Input;

    public class CompositionViewModel
    {
        private ICommand addCommand;
        private ICommand divideCommand;
        private ICommand multiplyCommand;
        private ICommand subtractCommand;
        public ICommand AddCommand => this.addCommand
                                      ?? (this.addCommand = new CommandHandler(this.AddSignals, this.SignalsGenerated));
        public ICommand DivideCommand => this.divideCommand
                                         ?? (this.divideCommand = new CommandHandler(
                                                 this.DivideSignals,
                                                 this.SignalsGenerated));
    }
}