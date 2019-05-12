using System;
using System.Globalization;

namespace UI.UIPresenter.Converters
{
    public class DateToTextHoursConverter : BaseValueConverter<DateToTextHoursConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).TimeOfDay.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
