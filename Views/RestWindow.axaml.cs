using Avalonia.Input; // for keyeventargs
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;  // for Dispatcher

using System;
// using System.Collections; // wrong solution! for Queue
using System.Collections.Generic; // for Queue
using System.Diagnostics; // for Process
using System.Runtime.InteropServices;

using DragDemo;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class RestWindow : Window
    {
        private RestWindowViewModel? vm => DataContext as RestWindowViewModel;
        private KeyboardHook? _keyboardHook;

        public RestWindow()
        {
            InitializeComponent();
            var vm = new RestWindowViewModel();
            DataContext = vm;
            vm.OnRequestClose += (sender, e) => Close();

            // fullscreen
            // this.WindowState = WindowState.FullScreen;


            // 禁用键盘输入: 不可在实例化时启用 KeyboardHook！因为bubble启动时已经实例化，仅在OnOpened窗口打开时启用
            // _keyboardHook = new KeyboardHook();
            // _keyboardHook.KeyIntercepted += OnKeyIntercepted;

            // 订阅鼠标点击事件
            this.PointerPressed += OnPointerPressed;

            // 订阅滚轮事件
            this.PointerWheelChanged += OnPointerWheelChanged;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // 处理键盘拦截事件
        private void OnKeyIntercepted(KeyboardHookEventArgs e)
        {
            // 检查是否按下了左侧或右侧的 Windows 键
            if (e.KeyCode == Key.LWin || e.KeyCode == Key.RWin)
            {
                // 阻止 Windows 键事件传递给系统
                e.PassThrough = false;
            }

            // 检查是否按下了 Alt+Tab 组合键
            // if (e.KeyCode == Key.Tab && (IsKeyDown(Key.LeftAlt) || IsKeyDown(Key.RightAlt)))
            // fail!
            // if (e.KeyCode == Key.Tab)
            // {
            //     // 阻止 Alt+Tab 事件传递给系统
            //     e.PassThrough = false;
            // }

            // 你可以在这里添加更多的键盘处理逻辑
        }

        // 仅在窗口打开时启用 KeyboardHook
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            if (OperatingSystem.IsWindows()) // 仅在 Windows 平台上启用 KeyboardHook
            {
                _keyboardHook = new KeyboardHook();
                _keyboardHook.KeyIntercepted += OnKeyIntercepted;
            }
        }

        // 确保在窗口关闭时释放钩子
        protected override void OnClosed(EventArgs e)
        {
            if (OperatingSystem.IsWindows() && _keyboardHook != null) // 仅在 Windows 平台上禁用 KeyboardHook
            {
                _keyboardHook.Dispose();
                _keyboardHook = null;
            }

            base.OnClosed(e);
        }


        // 暗号: 开启Unlock解锁键
        private Queue<string> _inputSequence = new Queue<string>();
        private readonly string[] _targetSequence = { "LeftClick", "RightClick", "LeftClick", "RightClick", "Scroll" }; // 暗号序列

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                AddInput("LeftClick");
            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                AddInput("RightClick");
            }
        }

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            AddInput("Scroll");
        }

        private void AddInput(string input)
        {
            _inputSequence.Enqueue(input);

            // 保持队列长度不超过目标序列长度
            if (_inputSequence.Count > _targetSequence.Length)
            {
                _inputSequence.Dequeue();
            }

            // 检查输入序列是否匹配
            if (_inputSequence.Count == _targetSequence.Length)
            {
                bool isMatch = true;
                int i = 0;
                foreach (var item in _inputSequence)
                {
                    if (item != _targetSequence[i])
                    {
                        isMatch = false;
                        break;
                    }
                    i++;
                }

                if (isMatch)
                {
                    // 这里可以执行你想要的操作，比如触发某个事件或方法
                    OnCodeActivated();
                }
            }
        }
        private void OnCodeActivated()
        {
            // 暗号match后的处理逻辑
            vm?.ChangeUnlockBtnState();
        }
    }
}