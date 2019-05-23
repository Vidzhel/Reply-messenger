using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using PropertyChanged;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.InversionOfControl;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{

    /// <summary>
    /// View modelfor Sign Up page
    /// </summary>
    public class SignUpPageViewModel : BaseViewModel
    {

        #region Public Members

        public ControlStates FieldState { get; set; } = ControlStates.NormalGray;

        public string ErrorMessage { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        //Commands for system buttons
        [DoNotNotify]
        public ICommand SignUpCommand { get; set; }

        [DoNotNotify]
        // Opens new page
        public ICommand OpenNextPageCommand { get; set; }


        public bool SignUpIsRunning { get; set; } = false;

        #endregion
        
        #region Constructor

        public SignUpPageViewModel()
        {

            SignUpCommand = new RelayCommandParametrized( async (parametr) => await signUpAsync(parametr) );

            OpenNextPageCommand = new RelayCommandParametrized((parametr) => changePage(parametr));
        }

        #endregion

        #region Private Methods

        void changePage(object parametr)
        {
            ApplicationService.ChangeCurrentApplicationPage((ApplicationPages)Enum.Parse(typeof(ApplicationPages), (string)parametr, true));
        }

        /// <summary>
        /// Check data from form and registrate user
        /// </summary>
        /// <param name="param">Password, RepeatPassword</param>
        /// <returns></returns>
        async Task signUpAsync(object param)
        {
            //If user few times chicks Sign Up, but the server does not response yet
            if (SignUpIsRunning)
                return;

            //Get secure strings from passwrod boxes
            var pass = (param as IHavePasswords).StringPassword;
            var repeatPass = (param as IHavePasswords).RepeatStringPassword;

            if (!FieldChecker(new SecureString[] { pass, repeatPass}))
                return;


            //Registration
            try
            {
                SignUpIsRunning = true;

                var res = UnitOfWork.SignUp(new CommonLibs.Data.User(UserName, pass, Email, null));

                if (res)
                    //If all right open SignIn pag
                    changePage("SignInPage");

                ErrorMessage = "Something went wrong";
            }
            catch {
                ErrorMessage = "Something went wrong";
            }
            finally
            {
                SignUpIsRunning = false;
            }
        }

        #endregion

        #region Helpers

        bool FieldChecker(SecureString[] values)
        {
            string temp;

            if((temp = ValidateUserData.ValidateUserName(UserName, true)) != null){
                ErrorMessage = temp;
                FieldState = ControlStates.UserNameError;
                return false;
            }
            
            if ((temp = ValidateUserData.ValidateEmail(Email, true)) != null)
            {
                FieldState = ControlStates.EmailError;
                ErrorMessage = temp;
                return false;
            }

            if ((temp = ValidateUserData.ValidatePassword(values, true)) != null)
            {
                FieldState = ControlStates.PasswordError;
                ErrorMessage = temp;
                return false;
            }

            ErrorMessage = "";
            FieldState = ControlStates.NormalGray;
            return true;
        }

        #endregion
    }
}
