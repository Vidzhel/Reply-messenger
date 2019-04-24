using System;
using System.Globalization;

namespace UI.UIPresenter.Converters
{
    public class TextToIcon : BaseValueConverter<TextToIcon>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "E-mail":
                    return "";
                case "User name":
                    return "";
                case "Password":
                    return "";
                case "Repeat Password":
                    return "";
                default:
                    return "";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
