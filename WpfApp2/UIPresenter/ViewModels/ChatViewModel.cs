using ClientLibs.Core.DataAccess;
using CommonLibs.Data;
using System;
using UI.Pages;

namespace UI.UIPresenter.ViewModels
{

    /// <summary>
    /// Contains Chat page view model current page (user info, chat or search results)
    /// </summary>
    public class ChatViewModel : BaseViewModel
    {

        #region Private Members

        //Events
        event EventHandler<Group> CurrentChatChanged;
        event EventHandler<Contact> CurrentUserInfoChanged;

        Group currentChat;

        Contact currentContactInfo;

        #endregion

        public ChatPages CurrentChatPage { get; set; }

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
