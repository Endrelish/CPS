using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CPS1.Converters
{
    public class VisibilityInversionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility vis)
            {
                if (vis == Visibility.Collapsed || vis == Visibility.Hidden)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}