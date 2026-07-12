using Avalonia; // for Application
using Avalonia.Threading;
using Avalonia.Media; // for StreamGeometry

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

            // Appearance of bubble window
            SetAppearanceSize();

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

            PinnedOnTopCommand = ReactiveCommand.Create(ToggleTopMostState);

            // this field must contain a non-null value when exiting constructor
            _restWindow = new RestWindow(userConfig);
        }

        // Windows width、height，文字大小 参数变量
        private int _EDGE = 60; // 实际上用从userconfig读入的数据覆盖
        public int EDGE
        {
            get { return _EDGE; }
            set { this.RaiseAndSetIfChanged(ref _EDGE, value); }
        }
        private double _TEXT_SIZE = 17;
        public double TEXT_SIZE
        {
            get { return _TEXT_SIZE; }
            set { this.RaiseAndSetIfChanged(ref _TEXT_SIZE, value); }
        }

        private void SetAppearanceSize()
        {
            double ratio = userConfig.UIScale;
            // double ratio = size switch
            // {
            //     "非常小" => 0.5,
            //     "小" => 0.75,
            //     "中" => 1,
            //     "大" => 1.25,
            //     "非常大" => 1.5,
            //     _ => 1,
            // };
            EDGE = (int)(60 * ratio);
            TEXT_SIZE = 17 * ratio;
        }

        // public void updateUIScale()
        // {
        //     SetAppearanceSize();
        //     // 重新设置窗口大小
        //     // if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //     // {
        //     //     if (desktop.MainWindow is BubbleWindow bubbleWindow)
        //     //     {
        //     //         bubbleWindow.Width = EDGE;
        //     //         bubbleWindow.Height = EDGE;
        //     //         bubbleWindow.FontSize = TEXT_SIZE;
        //     //     }
        //     // }
        // }

        /*
        在 MVVM 模式下明确要求:当属性的值发生变化时，必须通知 UI。这通常是通过实现 INotifyPropertyChanged 接口来实现的。
        如果想要动态绑定并自动更新 UI，则必须实现 INotifyPropertyChanged
        */




        // vm逻辑参数
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
        private int _saveConfigTickCounter = 0;

        public EventHandler<UserConfig>? OnConfigUpdated;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (userConfig.IsEnableDailyLimit)
            {
                string today = DateTime.Today.ToString("yyyy-MM-dd");
                bool dateChanged = false;
                bool limitReached = false;

                if (userConfig.DailyLimitDate != today)
                {
                    userConfig.DailyLimitDate = today;
                    userConfig.DailyLimitAccumulatedSeconds = 0;
                    dateChanged = true;
                }
                else
                {
                    userConfig.DailyLimitAccumulatedSeconds += 1.0; // 计时器每秒触发一次
                }

                if (userConfig.DailyLimitAccumulatedSeconds >= (double)(userConfig.DailyLimitTime * 60))
                {
                    limitReached = true;
                }

                // 定期存盘，避免频繁写入导致磁盘损耗；若跨天或到达上限则立即存盘
                _saveConfigTickCounter++;
                if (dateChanged || limitReached || _saveConfigTickCounter >= 10)
                {
                    _saveConfigTickCounter = 0;
                    try
                    {
                        new UserConfigService().SaveConfig(userConfig.SaveToDictionary());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving config in timer tick: {ex.Message}");
                    }
                }

                if (limitReached)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        OpenDailyLimitRestWindow();
                    });
                    return; // 达到限时，不更新正常的工作倒计时UI
                }
            }

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
            if (sender is RestWindow rw)
            {
                rw.Closed -= RestWindow_Closed;
            }
            else
            {
                _restWindow.Closed -= RestWindow_Closed;
            }

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

        private void OpenDailyLimitRestWindow()
        {
            if (_isRestWindowOpen)
            {
                return;
            }
            _isRestWindowOpen = true;

            // 重置并暂停正常计时器
            _timer.Reset();
            _timer.Pause();

            // 打开每日限时锁屏窗口
            var dlWindow = new RestWindow(userConfig, isDailyLimit: true);
            dlWindow.Closed += RestWindow_Closed; // 订阅关闭事件
            dlWindow.Show();
        }

        public void AddMinutes(int minutes)
        {
            _timer.AddMinutes(minutes);
        }

        private void OpenSettingWindow()
        {
            var settingWindow = new SettingWindow(userConfig);
            var svm = settingWindow.DataContext as SettingWindowViewModel;
            if(svm != null)
            {
                svm.OnConfigUpdated += HandleConfigUpdated;
            }
            settingWindow.Show();
        }

        // 设置更新保存后，更正timer时间、UI大小等显示状态
        private void HandleConfigUpdated(object? sender, UserConfig updatedConfig)
        {
            userConfig = updatedConfig;

            // 更新 BubbleWindow 的 UI 大小
            SetAppearanceSize();
            Console.WriteLine($"updateUIScale: {userConfig.UIScale}");

            // 更新计时器的当前时间
            int new_total_minutes = (int)userConfig.BreakIntervalTime;
            _timer.ChangeIntervalTime(new_total_minutes);

            OnConfigUpdated?.Invoke(this, userConfig);
        }

        // 置于顶层
        public ICommand PinnedOnTopCommand { get; }
        private bool _IsPinnedTop = true;
        public bool IsPinnedTop
        {
            get { return _IsPinnedTop; }
            set
            {
                this.RaiseAndSetIfChanged(ref _IsPinnedTop, value);
                this.RaisePropertyChanged(nameof(PinnedStateStr));  // 手动通知 PinnedStateStr 改变
            }
        }

        public event EventHandler<bool>? OnPinnedTopChanged;

        public string PinnedStateStr
        {
            get { return _IsPinnedTop ? "取消置顶" : "置于顶层"; }
        }

        private void ToggleTopMostState()
        {
            IsPinnedTop = !IsPinnedTop;
            OnPinnedTopChanged?.Invoke(this, IsPinnedTop);
        }

        // 以下是fail方案
        // public string PinnedIconResourceKey
        // {
        //     get { return _IsPinnedTop ? "pin_off_regular" : "pin_regular"; }
        // }

        // private StreamGeometry? _PinIconData;
        // public StreamGeometry? PinIconData
        // {
        //     get
        //     {
        //         string key = _IsPinnedTop ? "pin_off_regular" : "pin_regular";
        //         return GetIconForName(key);
        //     }
        // }

        // public static StreamGeometry? GetIconForName(string name)
        // {
        //     /*
        //     Console.WriteLine($"GetIconForName: {name}");
        //     return (StreamGeometry?)Application.Current.Resources[name];
        //     Console.WriteLine($"GetIconForName: {name}");
        //     Console.WriteLine($"Resource count: {Application.Current == null}");
        //     Console.WriteLine($"ss:{Application.Current.Styles}");
        //     // Console.WriteLine($"ss:{Application.Current.Styles.Resources}");
        //     Console.WriteLine($"ss:{Application.Current.Styles.Resources.Count}");
        //     Console.WriteLine($"Resource count: {Application.Current.Resources.Count}");
        //     Console.WriteLine($"Resource keys: {string.Join(", ", Application.Current.Resources.Keys)}");
        //     */
        //     if (Application.Current.Resources.ContainsKey(name))
        //     {
        //         var resource = Application.Current.Resources[name];
        //         Console.WriteLine($"Resource type: {resource.GetType()}");
        //         return resource as StreamGeometry;
        //     }
        //     else
        //     {
        //         Console.WriteLine($"Resource not found: {name}");
        //     }
        //     return null;
        // }
    }
}