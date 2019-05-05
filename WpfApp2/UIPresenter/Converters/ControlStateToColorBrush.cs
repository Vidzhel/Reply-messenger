using System;
using System.Globalization;
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
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA4C0"));

                case ControlStates.EmailError:
                    if(fieldType != "E-mail")
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA4C0"));
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6666"));

                case ControlStates.PasswordError:
                    if(fieldType != "Password" && fieldType != "Repeat Password")
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA4C0"));
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6666"));

                case ControlStates.UserNameError:
                    if(fieldType != "User name")
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA4C0"));
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6666"));

                case ControlStates.Error:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6666"));

                case ControlStates.Ok:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#56FF50"));

                default:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA4C0"));
            }
        }

        public override object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
