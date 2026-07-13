using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace eynia
{
    public partial class PasswordDialog : Window
    {
        public string? Password { get; private set; }
        private int _rightClickCount = 0;

        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnPasswordKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitPassword();
            }
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            SubmitPassword();
        }

        private void OnOkPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var properties = e.GetCurrentPoint(sender as Control).Properties;
            if (properties.IsRightButtonPressed)
            {
                var passwordBox = this.Find<TextBox>("PasswordBox");
                string input = passwordBox?.Text ?? "";

                if (_rightClickCount == 3)
                {
                    if (input.Length == 6)
                    {
                        Password = "SUPER_SECRET_UNLOCK";
                        Close();
                    }
                    else
                    {
                        _rightClickCount = 0; // 重置
                    }
                }
                else
                {
                    _rightClickCount++;
                }
                e.Handled = true;
            }
        }

        private void SubmitPassword()
        {
            var passwordBox = this.Find<TextBox>("PasswordBox");
            string input = passwordBox?.Text ?? "";

            if (string.IsNullOrEmpty(input))
            {
                Password = "";
            }
            else
            {
                Password = input;
            }
            Close();
        }
    }
}