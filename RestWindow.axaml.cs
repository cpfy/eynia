using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;  // for Dispatcher

using System.Diagnostics;
using System;


namespace eynia
{
    public partial class RestWindow : Window
    {
        public RestWindow()
        {
            InitializeComponent();
        }

        private void ExitWindows_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}