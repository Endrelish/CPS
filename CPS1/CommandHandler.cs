namespace CPS1
{
    using System;
    using System.Windows.Input;

    public class CommandHandler : ICommand
    {
        private readonly Action _action;

        private readonly bool _canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            this._action = action;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this._canExecute;
        }

        public void Execute(object parameter)
        {
            this._action();
        }
    }
}