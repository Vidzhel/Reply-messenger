using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UI.UIPresenter.ViewModels;

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
                    return Application.Current.FindResource("DarkBlueBrush");

                case ControlStates.EmailError:
                    if(fieldType != "E-mail")
                        return Application.Current.FindResource("DarkBlueBrush");
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.PasswordError:
                    if(fieldType != "Password" && fieldType != "Repeat Password")
                        return Application.Current.FindResource("DarkBlueBrush");
                    return Application.Current.FindResource("LightRedBrush");

                case ControlStates.UserNameError:
                    if(fieldType != "User name")
                        return Application.Current.FindResource("DarkBlueBrush");
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
