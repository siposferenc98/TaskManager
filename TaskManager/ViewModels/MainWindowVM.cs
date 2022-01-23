using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskManager.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        private bool _stackpanelVisible = false;
        public BindingList<TaskModel> Tasks { get; } = new();

        public string? TaskName { get; set; }
        public DateTime TaskDate { get; set; } = DateTime.Now;
        public bool SetReminder { get; set; }
        public ICommand AddTask => new ButtonCE(addTask,addTaskCanExecute);
        public ICommand ToggleButton => new ToggleButton(toggleTaskAddStackPanel);
        public ICommand ToggleReminder => new ToggleButton(toggleReminderOnTask);
        public TaskModel? SelectedTask { get; set; }

        public MainWindowVM()
        {
            jsonReadAll();
        }

        #region Toggle task add visibility
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
        #endregion

        private void addTask(object? o)
        {
            TaskModel taskModel = new(TaskName!,TaskDate,SetReminder);
            taskModel.DeleteTaskEvent += deleteTaskFromList!;
            Tasks.Add(taskModel);
        }

        private bool addTaskCanExecute()
        {
            if (TaskName is not null and not "")
                return true;

            return false;
        }

        private void toggleReminderOnTask(object? o)
        {
            if(SelectedTask is not null)
                SelectedTask.toggleReminder();
            
        }

        private void deleteTaskFromList(object sender, EventArgs e)
        {
            TaskModel taskModel = (TaskModel)sender;
            Tasks.Remove(taskModel);
        }

        private void jsonReadAll()
        {
            string jsonfile = File.ReadAllText("./Tasks.json");

            JsonRootClass tasks = JsonSerializer.Deserialize<JsonRootClass>(jsonfile)!;
            foreach (TaskModel t in tasks.TaskModels)
            {
                t.DeleteTaskEvent += deleteTaskFromList!;
                Tasks.Add(t);
            }
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged(string? sender = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sender));
        }
    }
}
