using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.UIPresenter.Converters
{
    public class BoolToChackedConverter : BaseValueConverter<BoolToChackedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var @checked = (bool)value;

            //Reverse
            if (((string)parameter).Equals("true", StringComparison.CurrentCultureIgnoreCase))
                @checked = !@checked;

                return @checked;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
