using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public ObservableCollection<TaskModel> Tasks { get; } = new() { new("Elsotest", DateTime.Now) };
        public ICommand ToggleButton => new ToggleButton(testaction);

        private void testaction(object o)
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

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
