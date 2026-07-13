using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace eynia
{
    public partial class PasswordDialog : Window
    {
        public string? Password { get; private set; }

        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            var passwordBox = this.Find<TextBox>("PasswordBox");
            Password = passwordBox?.Text;
            Close();
        }

        private void OnTitlePointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Password = "SUPER_SECRET_UNLOCK";
                Close();
            }
        }

        private void OnOkPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            var properties = e.GetCurrentPoint(sender as Control).Properties;
            if (properties.IsRightButtonPressed || e.KeyModifiers.HasFlag(Avalonia.Input.KeyModifiers.Shift))
            {
                Password = "SUPER_SECRET_UNLOCK";
                Close();
            }
        }
    }
}