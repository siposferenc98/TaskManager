﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TaskManager.Models
{
    internal class TaskModel : INotifyPropertyChanged
    {
        private bool _reminder = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public Brush Fill => _reminder? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Transparent);
        public TaskModel(string name, DateTime datetime)
        {
            Name = name;
            DateTime = datetime;
        }

        public void toggleReminder()
        {
            _reminder = !_reminder;
            RaisePropertyChanged("Fill");
        }
        public void RaisePropertyChanged(string? caller = null)
        {
            PropertyChanged?.Invoke(caller, new PropertyChangedEventArgs(caller));
        }

    }
}