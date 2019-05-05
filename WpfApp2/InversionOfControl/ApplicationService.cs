using Ninject;
using System.Windows.Controls;
using UI.Pages;
using UI.UIPresenter.ViewModels;

namespace UI.InversionOfControl
{
    /// <summary>
    /// Service to work with <see cref="ApplicationViewModel"/>, Pages of this
    /// </summary>
    public static class ApplicationService
    {
        //public static ApplicationService Instance { get; private set; } = new ApplicationService();


        ///Get Application view model
        public static ApplicationViewModel GetApplicationViewModel => IoCController.Kernel.Get<ApplicationViewModel>();

        /// <summary>
        /// Do Not Notify Changes, Get current page
        /// </summary>
        public static ApplicationPage GetCurrentPage => IoCController.Kernel.Get<ApplicationViewModel>().ApplicationCurrentPage;


        /// <summary>
        /// Set new application page
        /// </summary>
        /// <param name="nextPage">new page</param>
        public static void ChangeCurrentApplicationPage(ApplicationPage nextPage)
        {
            GetApplicationViewModel.ApplicationCurrentPage = nextPage;
        }
            
    }
}
