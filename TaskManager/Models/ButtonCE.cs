using System;

namespace TaskManager.Models
{
    internal class ButtonCE : ICommand
    {
        Action<object> Action;
        Func<bool> canExecute;

        public ButtonCE(Action<object> action, Func<bool> ce)
        {
            Action = action;
            canExecute = ce;
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute.Invoke();
        }

        public void Execute(object? parameter)
        {
            Action.Invoke(parameter!);
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
