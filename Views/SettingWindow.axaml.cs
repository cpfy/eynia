using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class SettingWindow : Window
    {
        private SettingWindowViewModel? ViewModel => DataContext as SettingWindowViewModel;
        public SettingWindow()
        {
            InitializeComponent();
            DataContext = new SettingWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}