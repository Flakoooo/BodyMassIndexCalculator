using System.Globalization;

namespace BodyMassIndexCalculator.src.Converters
{
    public class DateConverter : IValueConverter
    {
        private const string _today = "Сегодня, ";
        private const string _yesterday = "Вчера, ";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not DateTime date) return value;

            var now = DateTime.Now;
            var today = now.Date;

            if (date.Date == today) return $"{_today}{date.ToString("HH:mm", culture)}";

            if (date.Date == today.AddDays(-1)) return $"{_yesterday}{date.ToString("HH:mm", culture)}";

            if (date.Year == now.Year) return date.ToString("dd.MM.yyyy, HH:mm", culture);

            return date.ToString("dd.MM.yyyy, HH:mm", culture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
    }
}
