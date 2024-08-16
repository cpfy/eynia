using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand


using eynia.Views;

namespace eynia.ViewModels
{
    public class BubbleWindowViewModel : ViewModelBase
    {

        private TimeSpan TotalTime = TimeSpan.FromMinutes(1);
        // private TimeSpan TotalTime = TimeSpan.FromSeconds(10);


        private CancellationTokenSource _cancellationTokenSource;

        private TimeSpan _RemainingTime;
        public TimeSpan RemainingTime
        {
            get { return _RemainingTime; }
            set { this.RaiseAndSetIfChanged(ref _RemainingTime, value); }
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


        public BubbleWindowViewModel()
        {
            _RemainingTime = TotalTime;
            _RemainingTimeStr = TotalTime.ToString(@"mm\:ss");
            _cancellationTokenSource = new CancellationTokenSource();
            AddMinutesCommand = ReactiveCommand.Create<int>(AddMinutes);
            StartTimer(_cancellationTokenSource.Token);

            _restWindow = new RestWindow();
        }

        private RestWindow _restWindow;

        private async void StartTimer(CancellationToken token)
        {
            while (RemainingTime > TimeSpan.Zero)
            {
                await Task.Delay(1000, token);
                if (token.IsCancellationRequested)
                    break;

                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateUI();
            }

            if (RemainingTime <= TimeSpan.Zero)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var _restWindow = new RestWindow();
                    _restWindow.Closed += RestWindow_Closed; // 订阅关闭事件
                    _restWindow.Show();

                });
            }
        }

        private void RestWindow_Closed(object? sender, EventArgs e)
        {
            // 取消订阅事件:为了避免内存泄漏或不必要的事件订阅
            _restWindow.Closed -= RestWindow_Closed;

            // RestWindow 关闭时自动触发 ResetTimer 方法
            ResetTimer();
        }

        private void AddMinutes(int minutes)
        {
            RemainingTime += TimeSpan.FromMinutes(minutes);
            if (RemainingTime > TotalTime)
            {
                RemainingTime = TotalTime;
            }
            UpdateUI();
        }

        public void ResetTimer()
        {
            RemainingTime = TotalTime;
            UpdateUI();
            StartTimer(_cancellationTokenSource.Token);
        }

        private void UpdateUI()
        {
            RemainingTimeStr = RemainingTime.ToString(@"mm\:ss");
            RemainTimeBarValue = RemainingTime.TotalSeconds / TotalTime.TotalSeconds * 100;
            // 由于progressbar UI显示不全，value再次归一化到[30,70]。估计因为minimum=100，此时width=height=40
            RemainTimeBarValue = RemainTimeBarValue * 0.4 + 30;
        }

        public void CancelTimer()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}