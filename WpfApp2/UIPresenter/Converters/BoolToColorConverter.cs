using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.UIPresenter.Converters
{
    public class BoolToColorBrushConverter : BaseValueConverter<BoolToColorBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Application.Current.FindResource((bool)value ? "GrayBGHoverBrush" : "DarkBgBrush"); 
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
