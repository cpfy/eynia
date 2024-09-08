using Avalonia.Threading;

using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; // for ICommand

using Avalonia.Input; // for keyeventargs


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

        public RestWindowViewModel(UserConfig userConfig)
        {
            int t_rest = (int)userConfig.BreakLengthTime;  // default:5
            _timer = new Timer(TimeSpan.FromMinutes(t_rest));
            _timer.Tick += Timer_Tick;

            // 订阅 Timer 完成事件：当 _timer 的 Completed 事件被触发时，忽略事件提供的 sender 和 e 参数，直接调用 ExitWindow() 方法
            _timer.Completed += (sender, e) => ExitWindow();
            // StartTimer();

            _RemainingTimeStr = _timer.RemainingTimeStr;

            // Unlock Button 可见性
            // this.KeyDown += OnKeyDown;
            // KeyPressCommand = ReactiveCommand.Create<KeyEventArgs>(OnKeyDown);
            UnlockCommand = ReactiveCommand.Create(ExitWindow);
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

        public ICommand UnlockCommand { get; }
        private void ExitWindow()
        {
            if(OnRequestClose != null){
                OnRequestClose(this, new EventArgs());
            }
            // var passwordDialog = new PasswordDialog();
            // await passwordDialog.ShowDialog();

            // // 校验password
            // if (passwordDialog.Password == "885988")
            // {
            //     // Pass! Close the window
            //     if(OnRequestClose != null){
            //         OnRequestClose(this, new EventArgs());
            //     }
            // }
            // else
            // {
            //     // Show an error message or handle incorrect password
            //     var messageBox = MessageBox.Avalonia.MessageBoxManager
            //         .GetMessageBoxStandardWindow("Error", "Incorrect password");
            //     await messageBox.ShowDialog(this);
            // }
        }


        // 控制Button Visible状态
        private bool _CanUnlock = false;
        public bool CanUnlock
        {
            get { return _CanUnlock; }
            set { this.RaiseAndSetIfChanged(ref _CanUnlock, value); }
        }

        public void ChangeUnlockBtnState(){
            CanUnlock = !CanUnlock;
        }

        // 不可！Keyboard好像被ban了
        // private string _currentInput = string.Empty;

        //  public ICommand KeyPressCommand { get; }

        // private void OnKeyDown(object sender, KeyEventArgs e)
        // {
        //     // Convert Key to char
        //     char keyChar = KeyToChar(e.Key);
        //     if (keyChar != '\0')
        //     {
        //         _currentInput += keyChar;

        //         // Check if the current input matches "magic"
        //         if (_currentInput.EndsWith("magic"))
        //         {
        //             CanUnlock = !CanUnlock; // Toggle the boolean state
        //             _currentInput = string.Empty; // Reset the input
        //         }
        //         else if (_currentInput.Length > 5)
        //         {
        //             // Keep the input length manageable
        //             _currentInput = _currentInput.Substring(_currentInput.Length - 5);
        //         }
        //     }
        // }

        // private char KeyToChar(Key key)
        // {
        //     // Simple conversion from Key to char
        //     // This can be expanded to handle more keys and modifiers
        //     if (key >= Key.A && key <= Key.Z)
        //     {
        //         return (char)('a' + (key - Key.A));
        //     }
        //     return '\0';
        // }


    }
}