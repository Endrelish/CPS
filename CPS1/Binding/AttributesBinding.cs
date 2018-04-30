namespace CPS1.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using CPS1.Model.SignalData;

    public static class AttributesBinding
    {
        private static void ConverterMethods(
            IValueConverter converter,
            out Func<object, Type, object, CultureInfo, object> convert,
            out Func<object, Type, object, CultureInfo, object> convertBack)
        {
            convert = (o, type, arg3, arg4) => o;
            convertBack = (o, type, arg3, arg4) => o;
            if (converter != null)
            {
                convert = converter.Convert;
                convertBack = converter.ConvertBack;
            } 
        }
        private static void BindAttributes<T>(
            FunctionAttribute<T> source,
            FunctionAttribute<T> target,
            Func<object, Type, object, CultureInfo, object> convert)
            where T : struct
        {
            source.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(nameof(source.Value)))
                    {
                        target.Value = (T)(convert(
                                               source.Value,
                                               target.Value.GetType(),
                                               null,
                                               CultureInfo.InvariantCulture) ?? target.Value);
                        return;
                    }

                    if (args.PropertyName.Equals(nameof(source.Visibility)))
                    {
                        target.Visibility = source.Visibility;
                        return;
                    }
                };
        }
        public static void BindAttributesOneWay<T>(
            FunctionAttribute<T> source,
            FunctionAttribute<T> target,
            IValueConverter converter = null)
            where T : struct
        {
            ConverterMethods(converter, out var convert, out var convertBack);
            
            BindAttributes(source, target, convert);
        }

        public static void BindAttributesTwoWay<T>(
            FunctionAttribute<T> source,
            FunctionAttribute<T> target,
            IValueConverter converter = null)
            where T : struct
        {
           ConverterMethods(converter, out var convert, out var convertBack);

            BindAttributes(source, target, convert);
            BindAttributes(target, source, convertBack);
        }
        
    }
}