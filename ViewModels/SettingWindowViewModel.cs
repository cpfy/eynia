
using Avalonia;
using ReactiveUI;

using System.Windows.Input; // for ICommand
using System.Runtime.Serialization; // for [DataMember]

namespace eynia.ViewModels
{
    public class SettingWindowViewModel : ViewModelBase
    {

        // save&load configs
        private readonly NewtonsoftJsonSuspensionDriver _suspensionDriver;

        public SettingWindowViewModel()
        {
            SaveConfigCommand = ReactiveCommand.Create(SaveConfig);

            // Load config from file
            _suspensionDriver = new NewtonsoftJsonSuspensionDriver("user_setting.json");
            LoadState();
        }

        // [DataMember]
        private decimal? _BreakIntervalTime = 45;
        public decimal? BreakIntervalTime
        {
            get { return _BreakIntervalTime; }
            set { this.RaiseAndSetIfChanged(ref _BreakIntervalTime, value); }
        }

        private decimal? _BreakLengthTime = 5;
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

        private decimal? _PostponeCount = 3;
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

        private bool _IsAllowAutoCheckUpdate = false;
        public bool IsAllowAutoCheckUpdate
        {
            get { return _IsAllowAutoCheckUpdate; }
            set { this.RaiseAndSetIfChanged(ref _IsAllowAutoCheckUpdate, value); }
        }

        private bool _IsAllowAutoDownloadUpdate = false;
        public bool IsAllowAutoDownloadUpdate
        {
            get { return _IsAllowAutoDownloadUpdate; }
            set { this.RaiseAndSetIfChanged(ref _IsAllowAutoDownloadUpdate, value); }
        }

        public ICommand SaveConfigCommand { get; }
        private void SaveConfig()
        {
            // Save config to file
            //  _suspensionDriver.SaveState(this).Subscribe();
        }

        public void LoadState()
        {
        //     _suspensionDriver.LoadState().Subscribe(state =>
        //     {
        //         if (state is xxViewModel loadedState)
        //         {
        //             this.BreakIntervalTime = loadedState.BreakIntervalTime;
        //             this.BreakLengthTime = loadedState.BreakLengthTime;
        //         }
        //     });
        }
    }
}
