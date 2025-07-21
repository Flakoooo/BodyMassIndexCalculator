using System.Globalization;

namespace BodyMassIndexCalculator.src.Converters
{
    class BoolInverseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null) 
                if (value is bool boolean)
                    return !boolean;

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
                if (value is bool boolean)
                    return !boolean;

            return value;
        }
    }
}
