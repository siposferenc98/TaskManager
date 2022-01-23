using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskManager.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        private bool _stackpanelVisible = false;
        public BindingList<TaskModel> Tasks { get; } = new() { new("Elsotest", DateTime.Now) };
        public ICommand ToggleButton => new ToggleButton(toggleTaskAddStackPanel);
        public ICommand ToggleReminder => new ToggleButton(toggleReminderOnTask);
        public TaskModel? SelectedTask { get; set; }

        public MainWindowVM()
        {
            
        }

        private void toggleTaskAddStackPanel(object o)
        {
            Window window = Application.Current.MainWindow;
            StackPanel sp = (StackPanel)o;
            _stackpanelVisible = !_stackpanelVisible;
            switch(_stackpanelVisible)
            {
                case true:
                    window.MinHeight += 200;
                    sp.Visibility = Visibility.Visible;
                    break;
                case false:
                    window.MinHeight -= 200;
                    window.Height = window.ActualHeight <= window.MinHeight + 200 ? window.MinHeight : window.ActualHeight;
                    sp.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void toggleReminderOnTask(object? o)
        {
            if(SelectedTask is not null)
                SelectedTask.toggleReminder();
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged(string? sender = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sender));
        }
    }
}
