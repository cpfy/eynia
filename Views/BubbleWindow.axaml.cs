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
        private BubbleWindowViewModel? ViewModel => DataContext as BubbleWindowViewModel;

        public BubbleWindow()
        {
            InitializeComponent();
            DataContext = new BubbleWindowViewModel();
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
    }
}