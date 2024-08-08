using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity; // for RoutedEventArgs
using Avalonia.Controls;

using System; // for 'EventArgs'
using System.Windows.Input;

namespace eynia
{
    public partial class App : Application
    {

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // 设定启动时的窗口
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // desktop.MainWindow = new MainWindow();
                desktop.MainWindow = new BubbleWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        // 绑定system tray点击事件
        private void OpenRestWindow(object? sender, EventArgs e)
        {
            var restWindow = new RestWindow();
            restWindow.Show();
        }

        private void ExitApp(object? sender, EventArgs e)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}