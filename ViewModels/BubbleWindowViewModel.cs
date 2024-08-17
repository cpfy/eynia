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

        public BubbleWindowViewModel()
        {
            _timer = new Timer(TimeSpan.FromSeconds(10));
            _timer.Tick += Timer_Tick;
            _timer.Completed += (sender, e) => TimerFinished(); // 订阅 Timer 完成事件
            // StartTimer();

            _RemainingTimeStr = _timer.RemainingTimeStr;
            _RemainTimeBarValue = _timer.RemainTimeBarValue;


            AddMinutesCommand = ReactiveCommand.Create<int>(AddMinutes);

            _restWindow = new RestWindow();
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


        private RestWindow _restWindow;

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
        }

        private void TimerFinished()
        {
            // 重置+暂停计时器
            _timer.Reset();
            _timer.Pause();

            // 打开 RestWindow
            var _restWindow = new RestWindow();
            _restWindow.Closed += RestWindow_Closed; // 订阅关闭事件
            _restWindow.Show();
        }

        private void AddMinutes(int minutes)
        {
            _timer.AddMinutes(minutes);
        }
    }
}