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
        }

        public void toggleReminder()
        {
            Reminder = !Reminder;
            RaisePropertyChanged("Fill");
            if (Reminder)
            {
                addToastNotification();
            }
            else
            {
                removeToastNotification();
            }

        }

        private void addToastNotification()
        {
            new ToastContentBuilder()
                .AddText(Name)
                .Schedule(DateTime.Now.AddSeconds(60), notif =>
                {
                    notif.Tag = Id.ToString();
                });
        }

        private void removeToastNotification()
        {
            ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();

            IReadOnlyList<ScheduledToastNotification> scheduledNotifications = notifier.GetScheduledToastNotifications();

            ScheduledToastNotification? removedNotif = scheduledNotifications.FirstOrDefault(x => x.Tag == Id.ToString());
            if (removedNotif is not null)
            {
                notifier.RemoveFromSchedule(removedNotif);
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
