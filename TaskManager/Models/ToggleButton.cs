using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskManager.Models
{
    internal class ToggleButton : ICommand
    {
        Action<object> Action;
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
