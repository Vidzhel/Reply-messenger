using System;
using System.Globalization;
using System.Windows;

namespace UI.UIPresenter.Converters
{
    public class ResourceKeyToColorBrushConverter : BaseValueConverter<ResourceKeyToColorBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Application.Current.TryFindResource((string)value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
