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
using System.Text.Json.Serialization;

namespace TaskManager.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        #region Private fields
        private bool _stackpanelVisible = false;
        private string _name = string.Empty;
        private string _date = string.Empty;
        private int _time = 12;
        #endregion

        //All of our tasks
        public BindingList<TaskModel> Tasks { get; } = new();

        #region Add task properties
        public string TaskName 
        { 
            get => _name;
            set 
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        public string TaskDate
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged();
            }
        }
        public int TaskTime 
        {
            get => _time; 
            set 
            {
                _time = value;
                RaisePropertyChanged();
            } 
        }
        public bool SetReminder { get; set; }
        #endregion

        #region Icommands
        public ICommand AddTask => new ButtonCE(addTask,addTaskCanExecute);
        public ICommand ToggleButton => new ToggleButton(toggleTaskAddStackPanel);
        public ICommand ToggleReminder => new ToggleButton(toggleReminderOnTask);
        #endregion

        //Binded to listbox selecteditem
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
                    window.MinHeight += 250;
                    sp.Visibility = Visibility.Visible;
                    break;
                case false:
                    window.MinHeight -= 250;
                    window.Height = window.ActualHeight <= window.MinHeight + 250 ? window.MinHeight : window.ActualHeight;
                    sp.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        #endregion

        #region Closing method
        public void addTaskToJson(object? sender, CancelEventArgs e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string jsonTask = JsonSerializer.Serialize(Tasks,options);
            File.WriteAllText("./Tasks.json", jsonTask);
        }
        #endregion

        #region Add Task
        private void addTask(object? o)
        {
            DateTime dateTime = DateTime.Parse($"{TaskDate} {TaskTime}:00");
            TaskModel taskModel = new(Guid.NewGuid(),TaskName!,dateTime,SetReminder);
            taskModel.DeleteTaskEvent += deleteTaskFromList!;
            Tasks.Add(taskModel);

            TaskName = string.Empty;
            TaskTime = 12;

        }

        private bool addTaskCanExecute()
        {
            if (TaskName is not null and not "")
                return true;

            return false;
        }
        #endregion

        #region Toggle reminder
        private void toggleReminderOnTask(object? o)
        {
            if(SelectedTask is not null)
                SelectedTask.toggleReminder();
        }
        #endregion

        #region Delete Task
        private void deleteTaskFromList(object sender, EventArgs e)
        {
            TaskModel taskModel = (TaskModel)sender;
            Tasks.Remove(taskModel);
            if (taskModel.Reminder)
                taskModel.checkNotification(deletingTask: true);
        }
        #endregion

        #region Startup method
        private void jsonReadAll()
        {
            string jsonfile = File.ReadAllText("./Tasks.json");

            List<TaskModel> tasks = JsonSerializer.Deserialize<List<TaskModel>>(jsonfile)!;
            foreach (TaskModel t in tasks)
            {
                t.DeleteTaskEvent += deleteTaskFromList!;
                Tasks.Add(t);
            }
            
        }
        #endregion

        #region Property changed region
        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string? sender = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sender));
        }
        #endregion
    }
}
