using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Media;
using System;

using DragDemo;
using eynia.ViewModels;

namespace eynia.Views
{
    public partial class BubbleWindow : Window
    {
        private BubbleWindowViewModel? vm => DataContext as BubbleWindowViewModel;

        // 无参数构造函数,给xaml用
        public BubbleWindow() : this(new())  // new() 相当于null
        {
        }

        public BubbleWindow(UserConfig userConfig)
        {
            InitializeComponent();
            DataContext = new BubbleWindowViewModel(userConfig);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            Dispatcher.UIThread.Post(() =>
            {
                var border = this.Find<Border>("Border");
                if (border != null)
                {
                    DragControlHelper.StartDrag(border);
                }
            });
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            var border = this.Find<Border>("Border");
            if (border != null)
            {
                DragControlHelper.StopDrag(border);
            }

            base.OnUnloaded(e);
        }

        // 从App.xaml.cs传递一个函数到vm
        public void AddMinutes(int minutes)
        {
            vm?.AddMinutes(minutes);
        }
    }
}