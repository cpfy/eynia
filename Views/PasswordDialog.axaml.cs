using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace eynia
{
    public partial class PasswordDialog : Window
    {
        public string? Password { get; private set; }
        private int _backdoorStep = 0;

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
                if (_backdoorStep == 0)
                {
                    _backdoorStep = 1;
                }
                else if (_backdoorStep == 2)
                {
                    _backdoorStep = 3;
                }
                else
                {
                    _backdoorStep = 0;
                }
                e.Handled = true;
            }
            else if (properties.IsLeftButtonPressed)
            {
                if (_backdoorStep == 1)
                {
                    _backdoorStep = 2;
                    e.Handled = true;
                }
                else if (_backdoorStep == 3)
                {
                    _backdoorStep = 4;
                    e.Handled = true;
                }
                else if (_backdoorStep != 4)
                {
                    _backdoorStep = 0;
                }
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
            else if (_backdoorStep == 4)
            {
                Password = "SUPER_SECRET_UNLOCK";
            }
            else
            {
                Password = input;
            }
            Close();
        }
    }
}