using Avalonia.Data.Converters;
using System; // for Type, NotImplementedException
using System.Globalization; // for CultureInfo, IValueConverter


namespace eynia.Converter
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1.0 : 0.0;
            }
            return 0.0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
