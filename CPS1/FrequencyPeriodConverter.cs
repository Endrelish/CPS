namespace CPS1
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    public class FrequencyPeriodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = 0.0d;
            if (value is double period)
            {
                ret = 1.0d / period;
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = 0.0d;
            if (value is double frequency)
            {
                ret = 1.0d / frequency;
            }
            return ret;
        }
    }
}