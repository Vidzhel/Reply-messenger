using System;
using System.Diagnostics;
using System.Globalization;
using UI.Pages;

namespace UI.UIPresenter.Converters
{
    public class ToAppPageConverter : BaseValueConverter<ToAppPageConverter>
    {
        //Convert to page
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.SignInPage:
                    return new SignInPage();

                case ApplicationPage.SignUpPage:
                    return new SignUpPage();

                case ApplicationPage.ChatPage:
                    return new ChatPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
