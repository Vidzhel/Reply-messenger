using Common.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.Converters
{
    public class DateToTextConverter : BaseValueConverter<DateToTextConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var date = (DateTime)value;

            var yearsPassed = date.Year - DateTime.Now.Year;
            if (yearsPassed != 0)
                return $"{date.Day} {Enum.GetName(typeof(MonthsOfYear), date.Month) } {date.Year}";

            var mounthsPassed = Math.Abs(((DateTime)value).Month - DateTime.Now.Month);
            var daysPassed = Math.Abs(((DateTime)value).Day - DateTime.Now.Day);

            if (mounthsPassed != 0 || daysPassed > 7)
                return $"{date.Day} { Enum.GetName(typeof(MonthsOfYear), date.Month) }";

            if (daysPassed <= 7 && daysPassed > 1)
                return $"{Enum.GetName(typeof(DaysOfWeek), date.DayOfWeek)}";

            if (daysPassed == 1)
                return "Yes";

            if (daysPassed == 0)
                return $"{date.ToShortTimeString()}";

            return "error";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
