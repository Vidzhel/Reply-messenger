using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.UIPresenter.ViewModels;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Window.DataContext = new WindowsViewModel(this);

            ////Create an UIControl service and make request
            //var service = ServiceFactory.Create(ServiceType.UIControl);
            //var isConnected = service.MakeRequest(Target.Remote, Request.Connect);
            //if (!(bool)isConnected)
            //    return;
            ////TODO add code

            ////Meke ruquest to local data to get user info
            //var user = service.MakeRequest(Target.Local, Request.GetUserData);

            ////Trying to aytorizate user
            //if (user)
            //    var isAutorizated = service.MakeRequest(Target.Remote, Request.Authorization, User);

            //if ((bool)isAutorizated) ;
            //    //TODO Open Chat Window
        }

        /// <summary>
        /// Make ruquest to server
        /// </summary>
        private void SignInButton_Click(object sender, RoutedEventArgs e)
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

        private void SignInUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
