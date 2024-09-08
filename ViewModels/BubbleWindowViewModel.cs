using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand

using eynia.Models;
using eynia.Views;
using Timer = eynia.Models.Timer;

namespace eynia.ViewModels
{
    public class BubbleWindowViewModel : ViewModelBase
    {
        private Timer _timer;
        private UserConfig userConfig;

        public BubbleWindowViewModel(UserConfig userConfig)
        {
            this.userConfig = userConfig;
            int t_interval = (int)userConfig.BreakIntervalTime;
            _timer = new Timer(TimeSpan.FromMinutes(t_interval)); // default:35
            // _timer = new Timer(TimeSpan.FromSeconds(10));  // Test & Debug
            _timer.Tick += Timer_Tick;
            _timer.Completed += (sender, e) => TimerFinished(); // 订阅 Timer 完成事件

            // init binding fields
            _RemainingTimeStr = _timer.RemainingTimeStr;
            _RemainTimeBarValue = _timer.RemainTimeBarValue;

            // init binding command
            AddMinutesCommand = ReactiveCommand.Create<int>(AddMinutes);
            OpenSettingWindowCommand = ReactiveCommand.Create(OpenSettingWindow);
            ImmRestCommand = ReactiveCommand.Create(TimerFinished);
            ExitAppCommand = ReactiveCommand.Create(() => Environment.Exit(0));

            // this field must contain a non-null value when exiting constructor
            _restWindow = new RestWindow(userConfig);
        }



        private string _RemainingTimeStr;
        public string RemainingTimeStr
        {
            get { return _RemainingTimeStr; }
            set { this.RaiseAndSetIfChanged(ref _RemainingTimeStr, value); }
        }

        private double _RemainTimeBarValue;
        public double RemainTimeBarValue
        {
            get { return _RemainTimeBarValue; }
            set { this.RaiseAndSetIfChanged(ref _RemainTimeBarValue, value); }
        }

        public ICommand AddMinutesCommand { get; }

        public ICommand OpenSettingWindowCommand { get; }
        public ICommand ImmRestCommand { get; }
        public ICommand ExitAppCommand { get; }

        private RestWindow _restWindow;
        private bool _isRestWindowOpen = false; // 确保在计时器完成时只打开一个窗口

        private void Timer_Tick(object? sender, EventArgs e)
        {

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                // Point centerPoint = GetWindowCenter();
                // SetCursorPos((int)centerPoint.X, (int)centerPoint.Y);
                RemainingTimeStr = _timer.RemainingTimeStr;
                RemainTimeBarValue = _timer.RemainTimeBarValue;
            });
        }

        private void RestWindow_Closed(object? sender, EventArgs e)
        {
            // 取消订阅事件:为了避免内存泄漏或不必要的事件订阅
            _restWindow.Closed -= RestWindow_Closed;

            // RestWindow 关闭时触发 Timer Reset+Resume 方法
            _timer.Reset();
            _timer.Resume();

            _isRestWindowOpen = false;
        }

        private void TimerFinished()
        {
            if (_isRestWindowOpen)
            {
                return;
            }
            _isRestWindowOpen = true;

            // 重置+暂停计时器
            _timer.Reset();
            _timer.Pause();

            // 打开 RestWindow
            var _restWindow = new RestWindow(userConfig);
            _restWindow.Closed += RestWindow_Closed; // 订阅关闭事件
            _restWindow.Show();
        }

        private void AddMinutes(int minutes)
        {
            _timer.AddMinutes(minutes);
        }

        private void OpenSettingWindow()
        {
            var settingWindow = new SettingWindow(userConfig);
            settingWindow.Show();
        }
    }
}