using System;

namespace TaskManager.Models
{
    internal class ToggleButton : ICommand
    {
        Action<object> Action;

        //This button will execute anyways so this eventhandler is unused.
        public event EventHandler? CanExecuteChanged;

        public ToggleButton(Action<object> a)
        {
            Action = a;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Action?.Invoke(parameter!);
        }
    }
}
