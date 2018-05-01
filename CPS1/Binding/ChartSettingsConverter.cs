namespace CPS1.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ChartSettingsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = 0;
            if (parameter.Equals("line"))
            {
                ret = System.Convert.ToInt32(value) * 2;
            }
            else if (parameter.Equals("point"))
            {
                ret = Math.Abs(System.Convert.ToInt32(value) - 1) * 5;
            }

            return ret.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}