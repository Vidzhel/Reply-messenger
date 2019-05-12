using System.Windows;
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
        public ApplicationPages ApplicationCurrentPage { get; set; }

        /// <summary>
        /// Actual Width of the wind
        /// </summary>
        public Thickness WindowHeight { get; set; }

        /// <summary>
        /// Defoult constructor
        /// </summary>
        public ApplicationViewModel()
        {
            ApplicationCurrentPage = ApplicationPages.ChatPage;
        }

    }
}
