using System;
using System.Globalization;
using System.Windows.Data;

namespace CPS1.Converters
{
    public class FrequencyPeriodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double period)
            {
                return 1.0d / period;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value as string, NumberStyles.Any, culture, out var frequency))
            {
                return 1.0d / frequency;
            }

            if (value is double freq)
            {
                return 1.0d / freq;
            }

            return null;
        }
    }
}