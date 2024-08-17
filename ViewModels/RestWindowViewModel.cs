using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand


using eynia.Models;
using eynia.Views;
using eynia.ViewModels;

// alias
using Timer = eynia.Models.Timer;


namespace eynia.ViewModels
{
    public class RestWindowViewModel : ViewModelBase
    {
        private Timer _timer;

        public RestWindowViewModel()
        {
            _timer = new Timer(TimeSpan.FromSeconds(10));
            _timer.Tick += Timer_Tick;
            _timer.Completed += (sender, e) => ExitWindow(); // 订阅 Timer 完成事件：当 _timer 的 Completed 事件被触发时，忽略事件提供的 sender 和 e 参数，直接调用 ExitWindow() 方法
            // StartTimer();

            _RemainingTimeStr = _timer.RemainingTimeStr;

            ExitCommand = ReactiveCommand.Create(ExitWindow);
        }
        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                // Point centerPoint = GetWindowCenter();
                // SetCursorPos((int)centerPoint.X, (int)centerPoint.Y);
                RemainingTimeStr = _timer.RemainingTimeStr;
            });
        }


        // 直接在 ViewModel 中维护这个值：
        private string _RemainingTimeStr;
        public string RemainingTimeStr
        {
            get { return _RemainingTimeStr; }
            set { this.RaiseAndSetIfChanged(ref _RemainingTimeStr, value); }
        }

        // 在vm中Close窗口
        public event EventHandler? OnRequestClose;

        public ICommand ExitCommand { get; }
        private void ExitWindow()
        {
            if(OnRequestClose != null){
                OnRequestClose(this, new EventArgs());
            }
        }
    }
}