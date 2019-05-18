using System;
using System.Diagnostics;
using System.Globalization;
using UI.Pages;
using UI.UserControls;

namespace UI.UIPresenter.Converters
{
    public class ToChatPageConverter : BaseValueConverter<ToChatPageConverter>
    {
        //Convert to page
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ChatPages)value)
            {
                case ChatPages.Chat:
                    return new ChatUserControl();
                case ChatPages.UserInfo:
                    return new InfoUserControl();

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
