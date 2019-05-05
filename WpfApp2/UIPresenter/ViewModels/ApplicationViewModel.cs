using UI.Pages;

namespace UI.UIPresenter.ViewModels
{

    /// <summary>
    /// Store properties for whole application
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {

        /// <summary>
        /// A curent open page in the main window
        /// </summary>
        public ApplicationPage ApplicationCurrentPage { get; set; }

        /// <summary>
        /// Defoult constructor
        /// </summary>
        public ApplicationViewModel()
        {
            ApplicationCurrentPage = ApplicationPage.SignInPage;
        }

    }
}
