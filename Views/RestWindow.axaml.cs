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
        private RestWindowViewModel? vm => DataContext as RestWindowViewModel;
        public RestWindow()
        {
            InitializeComponent();
            var vm = new RestWindowViewModel();
            DataContext = vm;
            vm.OnRequestClose += (sender, e) => Close();


            // this.Opened += RestWindow_Opened;
            // this.Closed += RestWindow_Closed;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // private void RestWindow_Opened(object? sender, EventArgs e)
        // {
        //     vm?.StartTimer();
        // }

        // private void RestWindow_Closed(object? sender, EventArgs e)
        // {
        //     vm?.StopTimer();
        // }

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