using CommonLibs.Data;
using Ninject;
using System.Collections.Generic;
using System.Windows.Controls;
using UI.Pages;
using UI.UIPresenter.ViewModels;

namespace UI.InversionOfControl
{
    /// <summary>
    /// Service to work with <see cref="ApplicationViewModel"/> and <see cref="ChatViewModel"/>, Pages of this
    /// </summary>
    public static class ApplicationService
    {
        #region Public Members

        ///Gets Application view model
        public static ApplicationViewModel GetApplicationViewModel => IoCController.Kernel.Get<ApplicationViewModel>();

        /// <summary>
        /// Gets chat view model
        /// </summary>
        public static ChatViewModel GetChatViewModel => IoCController.Kernel.Get<ChatViewModel>();

        /// <summary>
        /// Do Not Notify Changes, Get current page
        /// </summary>
        public static ApplicationPages GetCurrentPage => IoCController.Kernel.Get<ApplicationViewModel>().ApplicationCurrentPage;

        /// <summary>
        /// Do Not Notify Changes, Get current chat page
        /// </summary>
        public static ChatPages GetCurrentChatPage => IoCController.Kernel.Get<ChatViewModel>().CurrentChatPage;

        /// <summary>
        /// Do Not Notify Changes, Get current chat page
        /// </summary>
        public static Group GetCurrentChoosenChat => IoCController.Kernel.Get<ChatViewModel>().CurrentChat;

        /// <summary>
        /// Do Not Notify Changes, Get current chat page
        /// </summary>
        public static Contact GetCurrentChoosenContact => IoCController.Kernel.Get<ChatViewModel>().CurrentUserInfo;

        /// <summary>
        /// Gets and sets search results
        /// </summary>
        public static List<Contact> UsersSearchResult
        {
            get => GetChatViewModel.UsersSearchResult;
            set => GetChatViewModel.UsersSearchResult = value;
        }

        /// <summary>
        /// Get and sets search results
        /// </summary>
        public static List<Group> GroupsSearchResult {
            get => GetChatViewModel.GroupsSearchResult;
            set => GetChatViewModel.GroupsSearchResult = value;
        }

        #endregion

        #region Public Methods
        

        /// <summary>
        /// Set new application page
        /// </summary>
        /// <param name="nextPage">new page</param>
        public static void ChangeCurrentApplicationPage(ApplicationPages nextPage)
        {
            GetApplicationViewModel.ApplicationCurrentPage = nextPage;
        }

        /// <summary>
        /// Set new chat page
        /// </summary>
        /// <param name="nextPage">new page</param>
        public static void ChangeCurrentChatPage(ChatPages nextPage)
        {
            GetChatViewModel.CurrentChatPage = nextPage;
        }

        /// <summary>
        /// Set new contact 
        /// </summary>
        /// <param name="nextPage">new page</param>
        public static void ChangeCurrentContact(Contact contact)
        {
            GetChatViewModel.CurrentUserInfo = contact;
        }

        /// <summary>
        /// Set new chat page
        /// </summary>
        /// <param name="nextPage">new page</param>
        public static void ChangeCurrentChat(Group group)
        {
            GetChatViewModel.CurrentChat = group;
        }

        #endregion

    }
}
