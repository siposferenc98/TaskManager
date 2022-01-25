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
        public event EventHandler? DeleteTaskEvent;

        //Json properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public bool Reminder { get; set; }

        //We are ignoring these in the json parsing
        [JsonIgnore]
        public Brush Fill => Reminder? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Transparent);
        [JsonIgnore]
        public ToggleButton DeleteTask => new(raiseDelete);
        public TaskModel(Guid Id, string Name, DateTime DateTime, bool Reminder)
        {
            this.Id = Id;
            this.Name = Name;
            this.DateTime = DateTime;
            this.Reminder = Reminder;
            checkNotification();
        }

        public void toggleReminder()
        {
            Reminder = !Reminder;
            checkNotification();
            RaisePropertyChanged("Fill");
        }

        public void checkNotification(bool deletingTask = false)
        {
            IReadOnlyList<ScheduledToastNotification> scheduledToastNotifications  = ToastNotifications.notifier.GetScheduledToastNotifications();
            ScheduledToastNotification? notificationForThisTask = scheduledToastNotifications.FirstOrDefault(x => x.Tag == Id.ToString());

            if(!deletingTask)
            {
                if (Reminder && notificationForThisTask is null && DateTime > DateTime.Now)
                {
                    ToastNotifications.addToastNotification(this);
                }
                else if(!Reminder && notificationForThisTask is not null)
                {
                    ToastNotifications.notifier.RemoveFromSchedule(notificationForThisTask);
                }
            }
            else if(deletingTask && notificationForThisTask is not null)
            {
                ToastNotifications.notifier.RemoveFromSchedule(notificationForThisTask);
            }
            
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
