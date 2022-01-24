using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TaskManager.Models
{
    internal class TaskModel : INotifyPropertyChanged
    {
        public bool _reminder { get; set; }

        public event EventHandler? DeleteTaskEvent;
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        [JsonIgnore]
        public Brush Fill => _reminder? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Transparent);
        [JsonIgnore]
        public ToggleButton DeleteTask => new(raiseDelete);
        public TaskModel( string Name, DateTime DateTime, bool _reminder)
        {
            this.Name = Name;
            this.DateTime = DateTime;
            this._reminder = _reminder;
        }

        public void toggleReminder()
        {
            _reminder = !_reminder;
            RaisePropertyChanged("Fill");
        }

        private void raiseDelete(object? o)
        {
            DeleteTaskEvent?.Invoke(this,EventArgs.Empty);
        }

        //Propertychanged eventhandler + raise
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string? caller = null)
        {
            PropertyChanged?.Invoke(caller, new PropertyChangedEventArgs(caller));
        }

    }
}
