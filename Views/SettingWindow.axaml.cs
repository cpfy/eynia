using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace eynia.Views
{
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
        AvaloniaXamlLoader.Load(this);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // // 获取用户输入的值
            // int breakInterval = (int)BreakInterval.Value;
            // int breakLength = (int)BreakLength.Value;
            // bool isForceBreak = IsForceBreak.IsChecked ?? false;
            // string forceBreakType = (ForceBreakType.SelectedItem as ComboBoxItem)?.Content.ToString();
            // bool isAllowPostpone = IsAllowPostpone.IsChecked ?? false;
            // int postponeCount;

            // // 尝试解析推迟次数
            // if (!int.TryParse(PostponeCount.Text, out postponeCount))
            // {
            //     MessageBox.Show(this, "请输入有效的推迟次数。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //     return;
            // }

            // // 处理保存逻辑
            // // 你可以在这里保存设置到文件、数据库，或者其他存储
            // MessageBox.Show(this, "设置已保存。", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}