using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class InfoUserControlViewModel : BaseViewModel
    {
        
        #region Public Members

        public Contact UserInfo { get; set; }

        public ContactsListViewModel ContactsList { get; set; }

        /// <summary>
        /// True if this is your accaunt info page
        /// </summary>
        public bool IsYourAccaunt => UnitOfWork.User.Id == UserInfo?.Id;

        /// <summary>
        /// Gets user name
        /// </summary>
        public string UserName => UserInfo?.UserName;

        /// <summary>
        /// Return true if user online
        /// </summary>
        public string IsOnline => UserInfo?.Online;

        /// <summary>
        /// Return true if the user is in your contact list
        /// </summary>
        public bool IsYourContact => UnitOfWork.ContactsTableRepo.IsExists(ContactsTableFields.Id.ToString(), UserInfo?.Id.ToString());

        /// <summary>
        /// Gets user bio
        /// </summary>
        public string Bio => UserInfo?.Bio;

        /// <summary>
        /// Gets user email
        /// </summary>
        public string Email => UserInfo?.Email;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates user info page
        /// </summary>
        /// <param name="contact">Contact to display info</param>
        public InfoUserControlViewModel()
        {
            //Setup handlers
            UnitOfWork.ContactsTableRepo.AddDataChangedHandler((sender, args) => OnUserUpdates(sender, args));
            ApplicationService.GetChatViewModel.OnCurrentContactInfoChanged((sender, args) => loadInfo(args));

            //Load data
            loadInfo(ApplicationService.GetCurrentChoosenContact);
        }
        

        #endregion

        #region Private Methods

        void OnUserUpdates(object sender, DataChangedArgs<IEnumerable<Contact>> args)
        {
            if(args.Action == RepositoryActions.Update)
            {
                foreach (var contact in args.Data)
                {
                    
                    if(UserInfo.Id == contact.Id)
                    {

                        UserInfo = contact;

                        //Update UI
                        OnPropertyChanged("UserName");
                        OnPropertyChanged("Bio");
                        OnPropertyChanged("Email");
                        OnPropertyChanged("IsYourAccaunt");
                        OnPropertyChanged("IsYourContact");

                        break;

                    }

                }
            }
        }

        void OnUserUpdates(object sender, EventArgs args)
        {
            var user = UnitOfWork.User;
            UserInfo = new Contact(user.UserName, user.Email, user.Bio, user.ProfilePhoto, user.Online, user.Id);

            //Update UI
            OnPropertyChanged("UserName");
            OnPropertyChanged("Bio");
            OnPropertyChanged("Email");
            OnPropertyChanged("IsYourAccaunt");
            OnPropertyChanged("IsYourContact");

        }

        void loadInfo(Contact contact)
        {
            //Set user info
            UserInfo = contact;

            //If it's your account, than add contacts list
            if (IsYourAccaunt)
            {
                //Add on user data changed handler
                UnitOfWork.OnUserInfoUpdated((sender, args) => OnUserUpdates(sender, args));

                var contactsList = UnitOfWork.ContactsTableRepo.GetAll();

                if (contactsList != null)
                    ContactsList = new ContactsListViewModel((List<Contact>)contactsList);
            }


            //Update UI
            OnPropertyChanged("UserName");
            OnPropertyChanged("Bio");
            OnPropertyChanged("Email");
            OnPropertyChanged("IsYourContact");
            OnPropertyChanged("IsYourAccaunt");
        }

        #endregion
    }
}
