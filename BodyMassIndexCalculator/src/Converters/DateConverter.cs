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
            var yesterday = today.AddDays(-1);

            if (date.Date == today)
                return $"{_today}{date.ToString("HH:mm", culture)}";

            if (date.Date == yesterday)
                return $"{_yesterday}{date.ToString("HH:mm", culture)}";

            if (date.Year == now.Year)
                return date.ToString("dd.MM.yyyy, HH:mm", culture);

            return date.ToString("dd.MM.yyyy, HH:mm", culture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string dateStr) return value;

            var now = DateTime.Now;
            try
            {
                if (dateStr.StartsWith(_today))
                {
                    var timeStr = dateStr[_today.Length..];
                    if (TimeSpan.TryParse(timeStr, culture, out var time))
                        return new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0);
                }
                else if (dateStr.StartsWith(_yesterday))
                {
                    var timeStr = dateStr[_yesterday.Length..];
                    if (TimeSpan.TryParse(timeStr, culture, out var time))
                    {
                        var yesterday = now.Date.AddDays(-1);
                        return new DateTime(yesterday.Year, yesterday.Month, yesterday.Day,
                                            time.Hours, time.Minutes, 0);
                    }
                }
                else if (DateTime.TryParse(dateStr, culture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
            }
            catch
            {
                
            }

            return value;
        }
    }
}
