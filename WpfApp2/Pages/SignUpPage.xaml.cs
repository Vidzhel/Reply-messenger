using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Pages
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Make ruquest to server
        /// </summary>
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            //var user = UserFactory.Create();
            //var service = ServiceFactory.Create(ServiceType.UIControl);

            //user.UserName = SignInUserName.Text;
            //user.Password = SignInPassword.Text;

            //var isAutorizated = service.MakeRequest(Target.Remote, Request.Authorization, User);

            //if ((bool)isAutorizated) ;
            ////TODO Open Chat Window
            //else
            //    //TODO popup window
        }


        private async void SignInLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(200);
            SignInPanel.Visibility = Visibility.Collapsed;
        }
    }
}
