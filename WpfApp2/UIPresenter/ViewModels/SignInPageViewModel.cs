using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using Common.Data.Security;
using CommonLibs.Data;
using PropertyChanged;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.InversionOfControl;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{
    public class SignInPageViewModel : BaseViewModel
    {

        #region Public Members

        //Store email info from Sign In form
        public string Email { get; set; }

        // Represent field states
        public ControlStates FieldState { get; set; } = ControlStates.NormalGray;

        // Display error message
        public string ErrorMessage { get; set; } = string.Empty;

        [DoNotNotify]
        public ICommand SignInCommand { get; set; }

        [DoNotNotify]
        // Opens new page
        public ICommand OpenNextPageCommand { get; set; }

        //Show that Sign In button has pressed, and data loading
        public bool SignInIsRunning { get; set; } = false;

        #endregion


        #region Constructor

        public SignInPageViewModel()
        {

            SignInCommand = new RelayCommandParametrized(async (parametr) => await Task.Run(() => signInAsync(parametr)));
            OpenNextPageCommand = new RelayCommandParametrized((parametr) => changePage(parametr));
        }

        #endregion

        #region Private Methods

        void changePage(object parametr)
        {
            ApplicationService.ChangeCurrentApplicationPage((ApplicationPages)Enum.Parse(typeof(ApplicationPages), (string)parametr, true));
        }

        async Task signInAsync(object parameter)
        {
            if (parameter == null)
                return;

            //If user few times chicks Sign Up, but the server does not response yet
            if (SignInIsRunning)
                return;

            var value = (parameter as IHavePassword).StringPassword;
            if (value == null)
                return;

            if (!FieldChecker(value))
                return;


            //Registration
            try
            {
                SignInIsRunning = true;
                
                var res = await UnitOfWork.SignIn(new User(null, value, Email, null));

                if (res == true)
                {
                    //If all right open chat page
                    changePage("ChatPage");
                    await UnitOfWork.SyncData();
                }

                //If data is wrong show erroro message
                FieldState = ControlStates.Error;
                ShowErrorMessage("Invalid Email or Password");

            }
            finally
            {
                SignInIsRunning = false;
            }
        }

        #endregion

        #region Helpers

        bool FieldChecker(object value)
        {

            #region Check data

            string temp;

            if ((temp = ValidateUserData.ValidateEmail(Email, true)) != null)
            {
                FieldState = ControlStates.EmailError;
                ErrorMessage = temp;
                return false;
            }

            if ((temp = ValidateUserData.ValidatePassword((SecureString)value, true)) != null)
            {
                FieldState = ControlStates.PasswordError;
                ErrorMessage = temp;
                return false;
            }

            #endregion


            ShowErrorMessage("");
            FieldState = ControlStates.NormalGray;
            return true;
        }

        void ShowErrorMessage(string message)
        {
            ErrorMessage = message;
        }

        #endregion
    }
}
