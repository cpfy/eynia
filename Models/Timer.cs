using System;
using System.Threading;
using System.Threading.Tasks;

namespace eynia.Models
{
    public class Timer
    {
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);  // 单次更新计时器间隔，默认1s
        private CancellationTokenSource _cts;
        private Task? _timerTask;
        private DateTime _startTime;
        private TimeSpan _elapsedTime; // 已经过时间
        public TimeSpan TotalDuration { get; set; }
        public TimeSpan CurrentTime => _elapsedTime;

        // 状态属性
        private bool _isPaused;
        public bool IsRunning => _timerTask != null && !_timerTask.IsCompleted && !_isPaused;
        public bool IsPaused => _isPaused;

        public event EventHandler? Tick;
        public event EventHandler? Completed; // 新增 Completed 事件

        public Timer(TimeSpan totalDuration)
        {
            TotalDuration = totalDuration;
            _elapsedTime = TimeSpan.Zero;

            _cts = new CancellationTokenSource();
            Start();
        }

        public void Start()
        {
            if (_timerTask != null && !_isPaused)
            {
                return; // Timer is already running
            }

            _cts = new CancellationTokenSource();
            _startTime = DateTime.Now;
            _isPaused = false;

            _timerTask = RunTimer(_cts.Token);
        }

        public void Pause()
        {
            _isPaused = true;
            _cts?.Cancel();
        }

        public void Resume()
        {
            if (!_isPaused)
            {
                return;
            }


            _isPaused = false;
            _cts = new CancellationTokenSource();
            _timerTask = RunTimer(_cts.Token);

            // Start();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _timerTask = null;
            _elapsedTime = TimeSpan.Zero;
            _isPaused = false;
            Completed?.Invoke(this, EventArgs.Empty); // 在 Stop 方法中也触发 Completed 事件
        }

        public void Reset()
        {
            _startTime = DateTime.Now;
            _elapsedTime = TimeSpan.Zero;
        }

        public void AddMinutes(int minutes)
        {
            _startTime = _startTime.Add(TimeSpan.FromMinutes(minutes));
            if(_startTime > DateTime.Now)
            {
                _startTime = DateTime.Now;
            }
            _elapsedTime = DateTime.Now - _startTime;
        }

        // UI显示用
        public string RemainingTimeStr => (TotalDuration - _elapsedTime).ToString(@"mm\:ss");
        // public decimal RemainTimeBarValue =>(
        // {
        //     var value = _elapsedTime.TotalSeconds / TotalDuration.TotalSeconds * 100;
        //     // 由于progressbar UI显示不全，value再次归一化到[30,70]。估计因为minimum=100，此时width=height=40
        //     value = value * 0.4 + 30;
        //     return (decimal)value;
        // })
        public double RemainTimeBarValue => (70 - _elapsedTime.TotalSeconds / TotalDuration.TotalSeconds * 40);

        private async Task RunTimer(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _elapsedTime < TotalDuration)
            {
                await Task.Delay(_interval, cancellationToken);

                if (!_isPaused)
                {
                    _elapsedTime = DateTime.Now - _startTime;
                    Tick?.Invoke(this, EventArgs.Empty);
                }
            }

            if (_elapsedTime >= TotalDuration)
            {
                Stop();
                Completed?.Invoke(this, EventArgs.Empty); // 触发 Completed 事件
            }
        }

        // if被Setting修改了时间
        public void ChangeIntervalTime(int updatedMinute)
        {
            Pause();
            TotalDuration = TimeSpan.FromMinutes(updatedMinute);
            Resume();
        }
    }
}