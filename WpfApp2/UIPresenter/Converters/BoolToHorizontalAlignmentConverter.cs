using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.UIPresenter.Converters
{
    public class BoolToHorizontalAlignmentConverter : BaseValueConverter<BoolToHorizontalAlignmentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                if(((string)parameter).Equals("true"))
                    return HorizontalAlignment.Left;
                else
                    return HorizontalAlignment.Right;
            else
                if (((string)parameter).Equals("true"))
                    return HorizontalAlignment.Right;

            return HorizontalAlignment.Left;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
