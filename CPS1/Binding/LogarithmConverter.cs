using System;
using System.Globalization;
using System.Windows.Data;

namespace CPS1.Converters
{
    public class LogarithmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int integer)
            {
                return (int) Math.Ceiling(Math.Log(integer, 2));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int integer)
            {
                return (int) Math.Pow(2, integer);
            }

            return null;
        }
    }
}