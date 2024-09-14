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
        // main window
        public BubbleWindow? bubbleWindow { get; private set; }

        // user config
        private UserConfigService? _userConfigService;
        private UserConfig? _userConfig;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // Initialize后执行
        public override void OnFrameworkInitializationCompleted()
        {
            // 加载配置数据并初始化 UserConfig
            _userConfigService = new UserConfigService();
            var data = _userConfigService.LoadConfig();
            _userConfig = new UserConfig();
            _userConfig.LoadFromDictionary(data);

            // 设定启动时的窗口
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                bubbleWindow = new BubbleWindow(_userConfig);
                desktop.MainWindow = bubbleWindow;
            }


            base.OnFrameworkInitializationCompleted();
        }

        // 保存配置数据
        private void SaveConfigData()
        {
            if(_userConfig == null || _userConfigService == null)
                return;

            var data = _userConfig.SaveToDictionary();
            _userConfigService.SaveConfig(data);
        }

        // 绑定system tray点击事件
        private void OpenRestWindow(object sender, EventArgs e)
        {
            if(_userConfig == null)
                return;

            //TODO: 少了暂停BubbleWindow的逻辑
            var restWindow = new RestWindow(_userConfig);
            restWindow.Show();
        }

        private void OpenSettingWindow(object sender, EventArgs e)
        {
            if(_userConfig == null)
                return;

            var settingWindow = new SettingWindow(_userConfig);
            settingWindow.Show();
        }

        private void ExitApp(object sender, EventArgs e)
        {
            SaveConfigData();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
        private void DelayRest(object sender, EventArgs e)
        {
            // 也可：var menuItem = sender as NativeMenuItem;
            if (sender is NativeMenuItem menuItem)
            {
                string header = menuItem.Header!.ToString(); // 格式形如：x分钟。使用空条件运算符
                int delayminutes = int.Parse(header[0].ToString()); // 将第一个字符转换为字符串再解析为 int

                if (bubbleWindow != null)
                {
                    bubbleWindow.AddMinutes(delayminutes);
                }
            }
        }
    }
}