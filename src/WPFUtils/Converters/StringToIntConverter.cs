using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFUtils.Converters
{
    // This is a quick sketch, not yet production-ready.
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue == null)
                return null;

            int result;
            if (Int32.TryParse(stringValue, out result))
                return result;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : value.ToString();
        }
    }
}