using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.Models
{
    internal class ToastNotifications
    {
        public static ToastNotifierCompat notifier = ToastNotificationManagerCompat.CreateToastNotifier();

        public static void addToastNotification(TaskModel taskModel)
        {
            new ToastContentBuilder()
                .AddText(taskModel.Name)
                .Schedule(new DateTimeOffset(taskModel.DateTime), notif =>
                {
                    notif.Tag = taskModel.Id.ToString();
                });
        }
    }
}
