﻿global using TaskManager.Models;
global using TaskManager.ViewModels;
global using System.Windows.Input;
global using Microsoft.Toolkit.Uwp.Notifications;
global using Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM viewmodel = new();
            DataContext = viewmodel;
            Closing += viewmodel.addTaskToJson;
        }
    }
}
