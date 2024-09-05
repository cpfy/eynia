using Avalonia.Input; // for keyeventargs
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;  // for Dispatcher

using System;
using System.Diagnostics; // for Process
using System.Runtime.InteropServices;

// public static KeyboardHook kh;

using DragDemo;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class RestWindow : Window
    {
        private RestWindowViewModel? vm => DataContext as RestWindowViewModel;

        private KeyboardHook _keyboardHook;

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
            if (OperatingSystem.IsWindows()) // 仅在 Windows 平台上禁用 KeyboardHook
            {
                _keyboardHook.Dispose();
                _keyboardHook = null;
            }

            base.OnClosed(e);
        }
    }
}