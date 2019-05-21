using ClientLibs.Core;
using System;
using System.Globalization;
using System.Windows;

namespace UI.UIPresenter.Converters
{
    public class ControlStateToColorBrush : BaseMultiValueConverter<ControlStateToColorBrush>
    {
        public override object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var fieldType = (string)value[1];

            switch ((ControlStates)value[0])
            {
                case ControlStates.NormalGray:
                    return Application.Current.FindResource("MiddleGrayBgBrush");

                case ControlStates.EmailError:
                    if(fieldType != "E-mail" && fieldType != "Change E-mail")
                        return Application.Current.FindResource("MiddleGrayBgBrush");
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.PasswordError:
                    if(fieldType != "Password" && fieldType != "Repeat Password" && fieldType != "Old Password" && fieldType != "New Password" && fieldType != "Repeat New Password")
                        return Application.Current.FindResource("MiddleGrayBgBrush");
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.UserNameError:
                    if(fieldType != "User name" && fieldType != "Change Name" && fieldType != "Group Name")
                        return Application.Current.FindResource("MiddleGrayBgBrush");
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.Error:
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.Ok:
                    return Application.Current.FindResource("LightGreen");

                default:
                    return Application.Current.FindResource("DarkBlueBrush");
            }
        }

        public override object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
