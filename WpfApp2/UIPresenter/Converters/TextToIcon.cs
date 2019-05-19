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
                    return "";
                case "Change E-mail":
                    return "";
                case "User name":
                    return "";
                case "Change Name":
                    return "";
                case "Change Bio":
                    return "";
                case "Password":
                    return "";
                case "Repeat Password":
                    return "";
                case "Old Password":
                    return "";
                case "New Password":
                    return "";
                case "Repeat New Password":
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
