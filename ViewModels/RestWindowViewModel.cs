using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand


// using eynia.Models;
using eynia.Views;
using eynia.ViewModels;


namespace eynia.ViewModels
{
    public class RestWindowViewModel : ViewModelBase
    {
        // private Timer _timer;
        public RestWindowViewModel()
        {
            // _timer = new Timer(TimeSpan.FromMilliseconds(50));
            // _timer.Tick += Timer_Tick;
            // this.Opened += RestWindow_Opened;
            // this.Closed += RestWindow_Closed;

            _RemainingTime = TotalTime;
            _RemainingTimeStr = TotalTime.ToString(@"mm\:ss");
            _cancellationTokenSource = new CancellationTokenSource();

            ExitCommand = ReactiveCommand.Create(ExitWindow);

            StartTimer(_cancellationTokenSource.Token);
        }
        // private void RestWindow_Opened(object sender, EventArgs e)
        // {
        //     _timer.Start();
        // }

        // private void RestWindow_Closed(object sender, EventArgs e)
        // {
        //     _timer.Stop();
        // }

        // private void Timer_Tick(object sender, EventArgs e)
        // {
        //     Dispatcher.UIThread.InvokeAsync(() =>
        //     {
        //         Point centerPoint = GetWindowCenter();
        //         SetCursorPos((int)centerPoint.X, (int)centerPoint.Y);
        //     });
        // }




        // private TimeSpan TotalTime = TimeSpan.FromMinutes(1);
        private TimeSpan TotalTime = TimeSpan.FromSeconds(5);


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
                // ExecuteExitAndResetCommand();
                ExitWindow();
            }
        }

        private void UpdateUI()
        {
            RemainingTimeStr = RemainingTime.ToString(@"mm\:ss");
            RemainTimeBarValue = RemainingTime.TotalSeconds / TotalTime.TotalSeconds * 100;
        }

        public void CancelTimer()
        {
            _cancellationTokenSource.Cancel();
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