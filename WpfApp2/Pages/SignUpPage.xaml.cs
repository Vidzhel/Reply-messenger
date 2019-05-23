using System;
using System.Security;
using System.Threading.Tasks;
using UI.Animations;
using UI.UIPresenter.ViewModels;

namespace UI.Pages
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : BasePage<SignUpPageViewModel>, IHavePasswords
    {
        public SignUpPage()
        {
            InitializeComponent();

            //Starts all page animations
            startAnimationsAsync();
        }
        
        public SecureString StringPassword => SignUpPassword.SecurePassword;

        public SecureString RepeatStringPassword => SignUpRepeatPassword.SecurePassword;

        #region Animation Helpers

        private async Task startAnimationsAsync()
        {
            //Wait for Page animation done
            await Task.Delay(TimeSpan.FromMilliseconds((int)SlideDurationSec * 1000));

            //Element Animations
            AnimationPresets.SlideAndFadeFromUp(SignUpPanel, 1, 150);
        }

        #endregion
    }
}
