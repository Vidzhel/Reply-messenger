using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;

namespace UI.UIPresenter.ViewModels
{
    public class ContactsListItemViewModel : BaseViewModel
    {

        #region Public Members
        
        /// <summary>
        /// Contact info to display
        /// </summary>
        public Contact UserInfo { get; set; }

        /// <summary>
        /// True if the user is in your contact list
        /// </summary>
        public bool IsYourContact => UnitOfWork.User.contactsIdList.Contains(UserInfo.Id);


        /// <summary>
        /// If true profide button to delate user from a group
        /// </summary>
        public bool IsGroupContactsList { get; set; }

        /// <summary>
        /// If true provide button to add user to a group
        /// </summary>
        public bool IsInviteList { get; set; }

        /// <summary>
        /// If true provide Remove from chat botton
        /// </summary>
        public bool AreYouAdmin { get; set; }

        /// <summary>
        /// Gets user status
        /// </summary>
        public bool IsOnline => UserInfo.Online.Equals("true", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Retur true if it's you
        /// </summary>
        public bool IfYou => UserInfo.Id == UnitOfWork.User.Id;

        /// <summary>
        /// Gets user name
        /// </summary>
        public string UserName => UserInfo.UserName;

        #endregion

        #region Constructor

        public ContactsListItemViewModel(Contact user, bool isGroupContactsList = false, bool isInviteList = false, bool areYouAdmin = false)
        {
            IsGroupContactsList = isGroupContactsList;
            IsInviteList = isInviteList;
            AreYouAdmin = areYouAdmin;

            UpdateUser(user);
        }

        #endregion

        #region Public Methods

        public void UpdateUser(Contact user)
        {
            UserInfo = user;

            //Check data base

            //update UI
            OnPropertyChanged("UserName");
            OnPropertyChanged("IsOnline");
            OnPropertyChanged("IsYourContact");
            OnPropertyChanged("IfYou");
        }

        #endregion

    }
}
