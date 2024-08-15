using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand



namespace eynia.ViewModels
{
    public class BubbleWindowViewModel : ViewModelBase
    {

        private TimeSpan TotalTime = TimeSpan.FromMinutes(1);


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

        private void UpdateUI()
        {
            RemainingTimeStr = RemainingTime.ToString(@"mm\:ss");
            RemainTimeBarValue = RemainingTime.TotalSeconds / TotalTime.TotalSeconds * 100;
        }

        public void CancelTimer()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}