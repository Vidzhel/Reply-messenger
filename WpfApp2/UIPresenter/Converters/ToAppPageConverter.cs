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
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.SignInPage:
                    return new SignInPage();

                case ApplicationPages.SignUpPage:
                    return new SignUpPage();

                case ApplicationPages.ChatPage:
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
