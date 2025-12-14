using System;
using System.Globalization;
using System.Windows.Data;

namespace Library.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && parameter is string paramString && double.TryParse(paramString, out double margin))
            {
                // Zwraca szerokość ListView pomniejszoną o marginesy (np. 10)
                return width - margin;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}