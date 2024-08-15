using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;  // for Dispatcher

using System.Diagnostics;
using System;


namespace eynia.Views
{
    public partial class RestWindow : Window
    {
        public RestWindow()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ExitWindows_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}