using Ninject;
using PropertyChanged;
using System;
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

            SignInCommand = new RelayCommandParametrized(async (parametr) => await signInAsync(parametr));
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
            //If user few times chicks Sign Up, but the server does not response yet
            if (SignInIsRunning)
                return;

            string value = (string)parameter;
            if (!FieldChecker(value))
                return;


            //Registration
            try
            {
                SignInIsRunning = true;

                //TODO SignIn
                //FieldState = ControlStates.Error;
                //ShowErrorMessage("Invalid Email or Password");

                //If all right open chat page
                changePage("ChatPage");
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

            #region Check on emptyness

            if (Email == String.Empty)
            {
                FieldState = ControlStates.EmailError;
                ErrorMessage = "Email field is empty";
                return false;
            }

            if ((string)value == String.Empty)
            {
                FieldState = ControlStates.PasswordError;
                ErrorMessage = "Password field is empty";
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
