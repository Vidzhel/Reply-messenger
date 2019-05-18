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
        public bool IsYourContact { get; set; }

        /// <summary>
        /// Gets user status
        /// </summary>
        public bool IsOnline => UserInfo.Online.Equals("true", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Gets user name
        /// </summary>
        public string UserName => UserInfo.UserName;

        #endregion

        #region Constructor

        public ContactsListItemViewModel(Contact user)
        {
            UpdateUser(user);
        }

        #endregion

        #region Public Methods

        public void UpdateUser(Contact user)
        {
            UserInfo = user;

            //Check data base
            IsYourContact = UnitOfWork.ContactsTableRepo.IsExists(ContactsTableFields.Id.ToString(), user.Id.ToString());

            //update UI
            OnPropertyChanged("UserName");
            OnPropertyChanged("IsOnline");
        }

        #endregion

    }
}
