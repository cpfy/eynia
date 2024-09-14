using System.Reflection.Metadata;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using eynia.ViewModels;

namespace eynia.Views
{
    public partial class SettingWindow : Window
    {
        private SettingWindowViewModel? vm => DataContext as SettingWindowViewModel;

        public SettingWindow() : this(new())  // new() 相当于null
        {
        }
        public SettingWindow(UserConfig userConfig)
        {
            InitializeComponent();
            DataContext = new SettingWindowViewModel(userConfig);
            // DataContext.OnConfigUpdated += HandleConfigUpdated;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}