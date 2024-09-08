using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class SettingWindow : Window
    {
        private SettingWindowViewModel? ViewModel => DataContext as SettingWindowViewModel;

        public SettingWindow() : this(new())  // new() 相当于null
        {
        }
        public SettingWindow(UserConfig userConfig)
        {
            InitializeComponent();
            DataContext = new SettingWindowViewModel(userConfig);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}