using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity; // for RoutedEventArgs
using Avalonia.Controls;

using System; // for 'EventArgs'
using System.Windows.Input;

using ReactiveUI;

using eynia.Views;
using eynia.ViewModels;

namespace eynia
{
    public partial class App : Application
    {
        public BubbleWindow? bubbleWindow { get; private set; }
        // public ICommand AddMinutesCommand { get; } = ReactiveCommand.Create(AddMinutes);

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // 设定启动时的窗口
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                bubbleWindow = new BubbleWindow();
                desktop.MainWindow = bubbleWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        // 绑定system tray点击事件
        private void OpenRestWindow(object sender, EventArgs e)
        {
            var restWindow = new RestWindow();
            restWindow.Show();
        }

        private void OpenSettingWindow(object sender, EventArgs e)
        {
            var settingWindow = new SettingWindow();
            settingWindow.Show();
        }

        private void ExitApp(object sender, EventArgs e)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
            // var app = (Apps)Application.Current;
            // if(app is IClassicDesktopStyleApplicationLifetime desktop){
            //     desktop.Shutdown();
            // }

        }

        private void AddMinutes(object sender, EventArgs e)
        {
            // if (bubbleWindow != null)
            // {
            //     bubbleWindow.AddMinutes(5);
            // }
        }
        private void DelayRest(object sender, EventArgs e)
        {
            var menuItem = sender as NativeMenuItem;
            if (menuItem != null)
            {
                string header = menuItem.Header!.ToString(); // 格式形如：x分钟。使用空条件运算符
                int delayminutes = int.Parse(header[0].ToString()); // 将第一个字符转换为字符串再解析为 int
                // bubbleWindow.AddMinutes(delayminutes);

                // if (DataContext is BubbleWindowViewModel viewModel)
                // {
                //     viewModel.AddMinutes(delayminutes);
                // }
            }
        }
    }
}