using System;
using System.Globalization;
using System.Windows.Data;

namespace CPS1.Binding
{
    public class PointSizeContinuousConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool cont)
            {
                if (cont == false)
                {
                    return 5.0d;
                }

                return 0.0d;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size)
            {
                if (Math.Abs(size) < double.Epsilon)
                {
                    return true;
                }

                return false;
            }

            return null;
        }
    }
}