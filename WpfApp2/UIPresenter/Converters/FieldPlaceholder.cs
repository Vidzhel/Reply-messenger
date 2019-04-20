using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WpfApp2.UIPresenter.Converters
{

    public class FieldPlaceholder : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var IsFocused = (bool)values[0];
            var IsTextEmpty = (bool)values[1];

            if (IsFocused || !IsTextEmpty)
                return true;
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
