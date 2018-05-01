using System;
using System.Windows.Input;

namespace CPS1.Model.CommandHandlers
{
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
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}