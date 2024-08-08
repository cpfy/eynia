using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;  // for Dispatcher

using System;
using System.Threading;
using System.Threading.Tasks;
using DragDemo;

namespace eynia
{
    public partial class BubbleWindow : Window
    {
        private TimeSpan TotalTime = TimeSpan.FromMinutes(1); // 1 minute timer
        private TimeSpan RemainingTime;
        private CancellationTokenSource cancellationTokenSource;

        public BubbleWindow()
        {
            InitializeComponent();

            // 扩展客户区到装饰区
            // ExtendClientAreaToDecorationsHint = true;

            RemainingTime = TotalTime;
            cancellationTokenSource = new CancellationTokenSource();
            StartTimer(cancellationTokenSource.Token);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // 使用 Dispatcher 确保在 UI 线程上执行
            Dispatcher.UIThread.Post(() =>
            {
                // 查找名为 "Border" 的 Border 控件
                var border = this.Find<Border>("Border");
                if (border != null)
                {
                    // 启动拖拽
                    DragControlHelper.StartDrag(border);
                }
            });
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            // 查找名为 "Border" 的 Border 控件
            var border = this.Find<Border>("Border");
            if (border != null)
            {
                // 停止拖拽
                DragControlHelper.StopDrag(border);
            }

            base.OnUnloaded(e);
        }

        private async void StartTimer(CancellationToken token)
        {
            while (RemainingTime > TimeSpan.Zero)
            {
                await Task.Delay(1000, token); // wait for 1 second
                if (token.IsCancellationRequested)
                    break;

                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            TimeTextBlock.Text = RemainingTime.ToString(@"mm\:ss");
            ProgressBar.Value = RemainingTime.TotalSeconds / TotalTime.TotalSeconds * 100;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            cancellationTokenSource.Cancel(); // Cancel the timer when the window is closed
        }
    }
}