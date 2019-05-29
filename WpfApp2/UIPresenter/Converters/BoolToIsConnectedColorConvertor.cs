using System;
using System.Globalization;
using System.Windows;

namespace UI.UIPresenter.Converters
{
    public class BoolToIsConnectedColorConvertor : BaseValueConverter<BoolToIsConnectedColorConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
                return Application.Current.FindResource("LightGreenBrush");
            return Application.Current.FindResource("LightRedBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
