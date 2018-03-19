namespace CPS1
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = 0.0d;
            if (value is string str)
            {
                double.TryParse(str, out ret);
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                return val.ToString();
            }
            return null;
        }
    }
}