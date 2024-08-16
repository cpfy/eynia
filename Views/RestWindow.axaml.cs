using Avalonia.Input; // for keyeventargs
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;  // for Dispatcher

using System;

using DragDemo;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class RestWindow : Window
    {
        private RestWindowViewModel? ViewModel => DataContext as RestWindowViewModel;
        public RestWindow()
        {
            InitializeComponent();
            var vm = new RestWindowViewModel();
            DataContext = vm;
            vm.OnRequestClose += (sender, e) => Close();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ExitWindows_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        // 禁用键盘输入
        // protected override void OnKeyDown(KeyEventArgs e)
        // {
        //     e.Handled = true;
        //     base.OnKeyDown(e);
        // }

        // protected override void OnKeyUp(KeyEventArgs e)
        // {
        //     e.Handled = true;
        //     base.OnKeyUp(e);
        // }
    }
}