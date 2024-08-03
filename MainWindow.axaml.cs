using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System;


namespace eynia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // new
    public void ButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("Click!");
        Debug.WriteLine($"Click! Celsius={celsius.Text}");

        if (Double.TryParse(celsius.Text, out double C))
        {
            var F = C * (9d / 5d) + 32;
            fahrenheit.Text = F.ToString("0.0");
        }
        else
        {
            celsius.Text = "0";
            fahrenheit.Text = "0";
        }
    }
}