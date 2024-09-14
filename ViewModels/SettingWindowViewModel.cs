
using Avalonia;
using ReactiveUI;

using Microsoft.Win32; // for RegistryKey

using System.Windows.Input; // for ICommand
using System.Runtime.Serialization;
using System;
using System.Reflection; // for [DataMember]

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

        public ICommand SaveConfigCommand { get; }
        public ICommand ResetConfigCommand { get; }

        public event EventHandler<UserConfig>? OnConfigUpdated; // 传递UserConfig示例

        private void SaveConfig()
        {
            _userConfig.BreakIntervalTime = BreakIntervalTime ?? 45;
            _userConfig.BreakLengthTime = BreakLengthTime ?? 5;
            _userConfig.IsForceBreak = IsForceBreak;
            _userConfig.ForceBreakType = ForceBreakType;
            _userConfig.PostponeCount = PostponeCount ?? 3;
            _userConfig.IsAllowPostpone = IsAllowPostpone;
            _userConfig.IsAllowShowAlert = IsAllowShowAlert;

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
