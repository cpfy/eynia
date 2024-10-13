using Avalonia.Data;    // for BindingOperations
using Avalonia.Data.Converters;
using System; // for Type, NotImplementedException
using System.Globalization; // for CultureInfo, IValueConverter

/*
from enum to boolean as a RatioButton's databinding
Refer: https://github.com/AvaloniaUI/Avalonia/issues/3016#issuecomment-706492175
*/

namespace eynia.Converter
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null || parameter == null)
            {
                return false;
            }

            Console.WriteLine($"value: {value}, parameter: {parameter}");

            return value.Equals(parameter);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null || parameter == null)
            {
                return BindingOperations.DoNothing;
            }
            return value?.Equals(true) == true ? parameter : BindingOperations.DoNothing;
        }
    }
}
