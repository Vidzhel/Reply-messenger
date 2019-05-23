using System;
using System.Security;
using System.Threading.Tasks;
using UI.Animations;
using UI.UIPresenter.ViewModels;

namespace UI.Pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : BasePage<SignInPageViewModel>, IHavePassword
    {
        public SignInPage()
        {
            InitializeComponent();

            //Starts all page animations
            startAnimationsAsync();
        }

        public SecureString StringPassword => SignInPassword.SecurePassword;

        #region Animation Helpers

        private async Task startAnimationsAsync()
        {
            //Wait for Page animation done
            await Task.Delay(TimeSpan.FromMilliseconds((int)SlideDurationSec * 1000));

            //Element Animations
            AnimationPresets.SlideAndFadeFromUp(SignInPanel, 1, 150);
        }

        #endregion

    }
}
