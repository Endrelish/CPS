namespace CPS1.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ExpanderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var expand = true;
            foreach (var value in values)
            {
                if (value is bool b)
                {
                    if (b)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}