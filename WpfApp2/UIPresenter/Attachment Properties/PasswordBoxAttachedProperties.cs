using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.UIPresenter.Attachment_Properties
{
    /// <summary>
    /// The HasText Attached property for a <see cref="PasswordBox"/>, return true if Secture Password isn't empty string
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>{

        /// <summary>
        /// Shortcut for <see cref="HasTextProperty.SetValue"/>
        /// </summary>
        /// <param name="element"></param>
        public static void SetValue(DependencyObject element) {

            HasTextProperty.SetValue((PasswordBox)element, ((PasswordBox)element).SecurePassword.Length > 0);

        }
    }

    /// <summary>
    /// The Monitor Attached property for a <see cref="PasswordBox"/>, monitor secure password if set true
    /// </summary>
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        /// <summary>
        /// Occurs on MonitorPasswordBox property changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get passwordbox(caller)
            var passwordBox = (d as PasswordBox);


            if (passwordBox == null)
                return;

            //Delete previous event handlers
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            //If new value equals true (MonitorPasswordBox)
            if ((bool)e.NewValue)
            {
                //Set HasText property true if the passwordBox has text
                HasTextProperty.SetValue(passwordBox);

                //Add new event handler
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        /// <summary>
        /// Sets new value of HasText property on MonitorPasswordBox set true and password box password has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HasTextProperty.SetValue((DependencyObject)sender);
        }
    }
}
