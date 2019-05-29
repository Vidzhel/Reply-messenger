using ClientLibs.Core.DataAccess;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{

    /// <summary>
    /// Contains Chat page view model current page (user info, chat or search results)
    /// </summary>
    public class ChatViewModel : BaseViewModel
    {

        #region Private Members

        event EventHandler<Group> CurrentChatChanged;
        event EventHandler<Contact> CurrentUserInfoChanged;

        //Search Results changed
        event EventHandler<List<Contact>> UsersSearchResultChanged;
        event EventHandler<List<Group>> GroupSearchResultChanged;

        Group currentChat;
        Contact currentContactInfo;

        List<Contact> usersSearchResult;
        List<Group> groupsSearchResult;

        #endregion

        /// <summary>
        /// Attached files
        /// </summary>
        public List<string> Attachments { get; set; }
        public FilesListViewModel AttachmentsList { get; set; } = new FilesListViewModel(true);

        public ChatPages CurrentChatPage { get; set; }

        /// <summary>
        /// Result of Search request
        /// </summary>
        public List<Contact> UsersSearchResult
        {
            get => usersSearchResult;
            set
            {
                usersSearchResult = value;
                UsersSearchResultChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Result of Search request
        /// </summary>
        public List<Group> GroupsSearchResult
        {
            get => groupsSearchResult;
            set
            {
                groupsSearchResult = value;
                GroupSearchResultChanged?.Invoke(this, groupsSearchResult);
            }
        }

        /// <summary>
        /// Currently opened group in Chat User Control
        /// </summary>
        public Group CurrentChat {
            get
            {
                return currentChat;
            }
            set
            {
                currentChat = value;
                CurrentChatChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Currently opened contact in Chat User Control
        /// </summary>
        public Contact CurrentUserInfo {
            get
            {
                if(currentContactInfo != null)
                    return currentContactInfo;

                currentContactInfo = UnitOfWork.User;
                return currentContactInfo;
            }
            set
            {
                currentContactInfo = value;
                CurrentUserInfoChanged?.Invoke(this, value);
            }
        }


        #region Constructor

        public ChatViewModel()
        {
            //Startup user control
            CurrentChatPage = ChatPages.UserInfo;
            CurrentUserInfo = UnitOfWork.Contact;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs on search result changed
        /// </summary>
        /// <param name="handler"></param>
        public void OnUsersSearchResultChanged(EventHandler<List<Contact>> handler)
        {
            UsersSearchResultChanged += handler;
        }

        /// <summary>
        /// Occurs on search result changed
        /// </summary>
        /// <param name="handler"></param>
        public void OnGroupSearchResultChanged(EventHandler<List<Group>> handler)
        {
            GroupSearchResultChanged += handler;
        }

        /// <summary>
        /// Adds handler to <see cref="CurrentChatChanged"/>
        /// </summary>
        /// <param name="handler"></param>
        public void OnCurrentChatChanged(EventHandler<Group> handler)
        {
            CurrentChatChanged += handler;
        }

        /// <summary>
        /// Adds handler to <see cref="CurrentUserInfoChanged"/>
        /// </summary>
        /// <param name="handler"></param>
        public void OnCurrentContactInfoChanged(EventHandler<Contact> handler)
        {
            CurrentUserInfoChanged += handler;
        }

        #endregion

    }
}
