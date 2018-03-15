namespace CPS1
{
    using System;
    using System.Windows.Input;

    public class CommandHandler : ICommand
    {
        private readonly Action<object> action;

        private readonly Func<bool> canExecute;

        public CommandHandler(Action<object> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}