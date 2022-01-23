using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TaskManager.Models
{
    internal class TaskModel
    {
        private bool _reminder = false;
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public Brush Fill => _reminder ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Transparent);
        public TaskModel(string name, DateTime datetime)
        {
            Name = name;
            DateTime = datetime;
        }
    }
}
