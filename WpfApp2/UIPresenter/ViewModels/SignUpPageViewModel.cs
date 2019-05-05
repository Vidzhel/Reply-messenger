using PropertyChanged;
using System;
using System.Text.RegularExpressions;
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

        #region Private Members


        //Regexp for validation
        private Regex userNameRegex = new Regex(@"^.{3,15}$");
        private Regex emailRegex = new Regex(@"^\w+[._-]?\w+@\w+.\w+$");
        private Regex passwordRegex = new Regex(@"^[\w.\-,/]{8,}$");

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
            ApplicationService.ChangeCurrentApplicationPage((ApplicationPage)Enum.Parse(typeof(ApplicationPage), (string)parametr, true));
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

            object[] values = (object[])param;
            if (!FieldChecker(values))
                return;


            //Registration
            try
            {
                SignUpIsRunning = true;

                //TODO registration


                //If all right open SignIn pag
                changePage("SignInPage");
            }
            finally
            {
                SignUpIsRunning = false;
            }
        }

        #endregion

        #region Helpers

        bool FieldChecker(object[] values)
        {

            #region Check on emptyness

            if (UserName == String.Empty)
            {
                FieldState = ControlStates.UserNameError;
                ErrorMessage = "User name field is empty";
                return false;
            }
            if (Email == String.Empty)
            {
                FieldState = ControlStates.EmailError;
                ErrorMessage = "Email field is empty";
                return false;
            }

            if ((string)values[0] == String.Empty)
            {
                FieldState = ControlStates.PasswordError;
                ErrorMessage = "Password field is empty";
                return false;
            }
            if ((string)values[1] == String.Empty)
            {
                FieldState = ControlStates.PasswordError;
                ErrorMessage = "Repeat password field is empty";
                return false;
            }

            #endregion

            //Check data correct
            if (!userNameRegex.IsMatch(UserName))
            {
                FieldState = ControlStates.UserNameError;
                ShowErrorMessage("User name should be 3-15 symbols");
                return false;
            }

            if (!emailRegex.IsMatch(Email))
            {
                FieldState = ControlStates.EmailError;
                ShowErrorMessage("Wrong email");
                return false;
            }

            if (!passwordRegex.IsMatch((string)values[0]))
            {
                FieldState = ControlStates.PasswordError;
                ShowErrorMessage("Password should have at less 8 symbols");
                return false;
            }
            if((string)values[0] != (string)values[1])
            {
                FieldState = ControlStates.PasswordError;
                ShowErrorMessage("Paswords don't match");
                return false;
            }

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
