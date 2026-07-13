
using Avalonia;
using ReactiveUI;

using Microsoft.Win32; // for RegistryKey

using System.Windows.Input; // for ICommand
using System.Runtime.Serialization;
using System;
using System.Reflection;
using System.Diagnostics; // for [DataMember]

using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System.Threading.Tasks;
using eynia.Views;
using eynia;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace eynia.ViewModels
{
    public class SettingWindowViewModel : ViewModelBase
    {
        private UserConfig _userConfig;

        public SettingWindowViewModel(UserConfig userConfig)
        {
            _userConfig = userConfig;
            // _cfg_bak = new UserConfig(userConfig); // 备份
            SaveConfigCommand = ReactiveCommand.Create(SaveConfig);
            ResetConfigCommand = ReactiveCommand.Create(ResetConfig);
            UnlockParentalControlsCommand = ReactiveCommand.CreateFromTask(UnlockParentalControls);

            // init fields from userConfig
            ResetConfig();
        }

        private decimal? _BreakIntervalTime;
        public decimal? BreakIntervalTime
        {
            get { return _BreakIntervalTime; }
            set { this.RaiseAndSetIfChanged(ref _BreakIntervalTime, value); }
        }

        private decimal? _BreakLengthTime;
        public decimal? BreakLengthTime
        {
            get { return _BreakLengthTime; }
            set { this.RaiseAndSetIfChanged(ref _BreakLengthTime, value); }
        }

        private bool _IsForceBreak = false;
        public bool IsForceBreak
        {
            get { return _IsForceBreak; }
            set { this.RaiseAndSetIfChanged(ref _IsForceBreak, value); }
        }

        private string _ForceBreakType = "一般强制";
        public string ForceBreakType
        {
            get { return _ForceBreakType; }
            set { this.RaiseAndSetIfChanged(ref _ForceBreakType, value); }
        }

        public string[] AvailableForceBreakTypes { get; } = new string[]
        {
            "一般强制", "完全强制"
        };

        private decimal? _PostponeCount;
        public decimal? PostponeCount
        {
            get { return _PostponeCount; }
            set { this.RaiseAndSetIfChanged(ref _PostponeCount, value); }
        }

        private bool _IsAllowPostpone = true;
        public bool IsAllowPostpone
        {
            get { return _IsAllowPostpone; }
            set { this.RaiseAndSetIfChanged(ref _IsAllowPostpone, value); }
        }

        private bool _IsAllowShowAlert = false;
        public bool IsAllowShowAlert
        {
            get { return _IsAllowShowAlert; }
            set { this.RaiseAndSetIfChanged(ref _IsAllowShowAlert, value); }
        }

        // appearance
        // public enum Size
        // {
        //     VerySmall,  // 对应 0.5x
        //     Small,      // 对应 0.75x
        //     Medium,     // 对应 1x
        //     Large,      // 对应 1.25x
        //     VeryLarge   // 对应 1.5x
        // }
        // private string _BubbleSize = "中";
        // public string BubbleSize
        // {
        //     get { return _BubbleSize; }
        //     set { this.RaiseAndSetIfChanged(ref _BubbleSize, value); }
        // }

        // private Size _BubbleSizeEnum;   // 在后面ResetConfig中一起更新
        // public Size BubbleSizeEnum
        // {
        //     get { return _BubbleSizeEnum; }
        //     set { this.RaiseAndSetIfChanged(ref _BubbleSizeEnum, value); }
        // }

        private double _UIScale = 1.0; // 默认1.0
        public double UIScale
        {
            get { return _UIScale; }
            set { this.RaiseAndSetIfChanged(ref _UIScale, value); }
        }

        // advanced
        private bool _IsAllowAutoStart = false;
        public bool IsAllowAutoStart
        {
            get { return _IsAllowAutoStart; }
            set { this.RaiseAndSetIfChanged(ref _IsAllowAutoStart, value); }
        }

        // private bool _IsAllowAutoCheckUpdate = false;
        // public bool IsAllowAutoCheckUpdate
        // {
        //     get { return _IsAllowAutoCheckUpdate; }
        //     set { this.RaiseAndSetIfChanged(ref _IsAllowAutoCheckUpdate, value); }
        // }

        // private bool _IsAllowAutoDownloadUpdate = false;
        // public bool IsAllowAutoDownloadUpdate
        // {
        //     get { return _IsAllowAutoDownloadUpdate; }
        //     set { this.RaiseAndSetIfChanged(ref _IsAllowAutoDownloadUpdate, value); }
        // }

        private bool _IsEnableDailyLimit;
        public bool IsEnableDailyLimit
        {
            get { return _IsEnableDailyLimit; }
            set { this.RaiseAndSetIfChanged(ref _IsEnableDailyLimit, value); }
        }

        private decimal? _DailyLimitTime;
        public decimal? DailyLimitTime
        {
            get { return _DailyLimitTime; }
            set { this.RaiseAndSetIfChanged(ref _DailyLimitTime, value); }
        }

        private bool _IsParentalControlsVisible = App.IsParentalModeUnlocked;
        public bool IsParentalControlsVisible
        {
            get { return _IsParentalControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref _IsParentalControlsVisible, value); }
        }

        public ICommand SaveConfigCommand { get; }
        public ICommand ResetConfigCommand { get; }
        public ICommand UnlockParentalControlsCommand { get; }

        private async Task UnlockParentalControls()
        {
            var passwordDialog = new PasswordDialog();
            Window? owner = null;
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var w in desktop.Windows)
                {
                    if (w is SettingWindow)
                    {
                        owner = w;
                        break;
                    }
                }
                if (owner == null)
                {
                    owner = desktop.MainWindow;
                }
            }

            if (owner != null)
            {
                await passwordDialog.ShowDialog(owner);
                if (passwordDialog.Password == "885988")
                {
                    App.IsParentalModeUnlocked = true;
                    IsParentalControlsVisible = true;
                }
                else if (passwordDialog.Password != null)
                {
                    var messageBox = MessageBoxManager
                        .GetMessageBoxStandard("提示", "密码错误，解锁失败！", ButtonEnum.Ok);
                    await messageBox.ShowWindowDialogAsync(owner);
                }
            }
        }

        public event EventHandler<UserConfig>? OnConfigUpdated; // 传递UserConfig示例

        private void SaveConfig()
        {
            _userConfig.BreakIntervalTime = BreakIntervalTime ?? 45;
            _userConfig.BreakLengthTime = BreakLengthTime ?? 5;
            _userConfig.IsForceBreak = IsForceBreak;
            _userConfig.ForceBreakType = ForceBreakType;
            _userConfig.PostponeCount = PostponeCount ?? 3;
            _userConfig.IsAllowPostpone = IsAllowPostpone;
            // _userConfig.BubbleSize = BubbleSize ?? "中";
            // _userConfig.UIScale = UIScale ?? 1.0;   // ?? 只能用于可空类型nullable types，这里用会报错
            _userConfig.UIScale = UIScale > 0 ? UIScale : 1.0;
            _userConfig.IsAllowShowAlert = IsAllowShowAlert;

            // 家长控制
            _userConfig.IsEnableDailyLimit = IsEnableDailyLimit;
            _userConfig.DailyLimitTime = DailyLimitTime ?? 150;

            // advanced
            _userConfig.IsAllowAutoStart = IsAllowAutoStart;
            if(OperatingSystem.IsWindows()){
                OperateWindowsRegKey();
            }

            // 事件的第一个参数是 sender，通常是引发事件的对象本身（通常使用 this）
            OnConfigUpdated?.Invoke(this, _userConfig);
        }

        private void ResetConfig()
        {
            /*
            如果已经保存了、则无法修改
            */

            // 用初始化时的参数再load一遍。既是init、也是reload
            // 如果直接赋值/设置私有字段（如 _BreakIntervalTime、_BreakLengthTime 等），而没有通过属性的 set 方法。这意味着 RaiseAndSetIfChanged 没有被调用，因此不会触发属性变更通知，导致视图没有更新
            BreakIntervalTime = _userConfig.BreakIntervalTime;
            BreakLengthTime = _userConfig.BreakLengthTime;
            IsForceBreak = _userConfig.IsForceBreak;
            ForceBreakType = _userConfig.ForceBreakType;
            PostponeCount = _userConfig.PostponeCount;
            IsAllowPostpone = _userConfig.IsAllowPostpone;
            IsAllowShowAlert = _userConfig.IsAllowShowAlert;

            // 家长控制
            IsEnableDailyLimit = _userConfig.IsEnableDailyLimit;
            DailyLimitTime = _userConfig.DailyLimitTime;
            IsParentalControlsVisible = App.IsParentalModeUnlocked;

            // appearance
            // BubbleSize = _userConfig.BubbleSize;
            // BubbleSizeEnum = BubbleSize switch
            // {
            //     "非常小" => Size.VerySmall,
            //     "小" => Size.Small,
            //     "中" => Size.Medium,
            //     "大" => Size.Large,
            //     "非常大" => Size.VeryLarge,
            //     _ => Size.Medium,
            // };
            // Console.WriteLine($"BubbleSize: {BubbleSize}");
            // Console.WriteLine($"BubbleSizeEnum: {BubbleSizeEnum}");
            UIScale = _userConfig.UIScale;
            Console.WriteLine($"UIScale: {UIScale}");

            // advanced
            IsAllowAutoStart = _userConfig.IsAllowAutoStart;
        }

        private void OperateWindowsRegKey()
        {
            if(OperatingSystem.IsWindows()) // 双保险
            {
                // 操作注册表，最后的true是指writable
                RegistryKey? rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if(rk != null){
                    var AppName = "Eynia";
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    if (IsAllowAutoStart)
                        rk.SetValue(AppName, exePath);
                    else
                        rk.DeleteValue(AppName,false);
                }
            }
        }
    }
}
