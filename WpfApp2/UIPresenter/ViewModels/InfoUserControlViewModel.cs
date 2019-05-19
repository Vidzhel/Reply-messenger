using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class InfoUserControlViewModel : BaseViewModel
    {

        #region Public Members

        /// <summary>
        /// Changes user ingormation
        /// </summary>
        public ICommand ChangeUserInfo { get; set; }

        /// <summary>
        /// Changes user password
        /// </summary>
        public ICommand ChangeUserPass { get; set; }

        /// <summary>
        /// Displays error message on user info change error
        /// </summary>
        public string ChangeUserInfoErrorMessage { get; set; } = String.Empty;

        /// <summary>
        /// Displays error message on user change pass error
        /// </summary>
        public string ChangePassErrorMessage { get; set; } = String.Empty;

        /// <summary>
        /// Represent change user info fields state
        /// </summary>
        public ControlStates FieldState { get; set; } = ControlStates.NormalGray;

        public Contact UserInfo { get; set; }

        public ContactsListViewModel ContactsList { get; set; }

        public GroupsListViewModel GroupsList { get; set; }

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
        public DateTime Online => UserInfo.LocalLastTimeOnline;

        /// <summary>
        /// Return true if the user is in your contact list
        /// </summary>
        public bool IsYourContact => UnitOfWork.User.contactsIdList.Contains(UserInfo.Id);

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
            //Setup Commands
            ChangeUserInfo = new RelayCommandParametrized((objects) => changeUserInfo(objects));
            ChangeUserPass = new RelayCommandParametrized((objects) => changeUserPass(objects));

            //Setup handlers
            UnitOfWork.ContactsTableRepo.AddDataChangedHandler((sender, args) => OnContactsUpdate(sender, args));
            ApplicationService.GetChatViewModel.OnCurrentContactInfoChanged((sender, args) => loadInfo(args));

            //Load data
            loadInfo(ApplicationService.GetCurrentChoosenContact);
        }
        

        #endregion

        #region Private Methods

        void changeUserInfo(object objects)
        {
            var data = (object[])objects;
            var name = (string)data[0];
            var email = (string)data[1];
            var bio = (string)data[2];

            string temp;

            //Check and change user name
            if(name != String.Empty)
            {

                if((temp = ValidateUserData.VilidateUserName(name, false)) != null)
                {
                    ChangeUserInfoErrorMessage = temp;
                    FieldState = ControlStates.UserNameError;
                    return;
                }

                //Change
                else
                {
                    UnitOfWork.User.UserName = name;
                    UnitOfWork.OnUserUpdated(this, new DataChangedArgs<IEnumerable<object>>(new List<object>() { name }, UsersTableFields.UserName.ToString(), RepositoryActions.Update));
                }
            }


            //Check user email
            if (email != String.Empty)
            {

                if ((temp = ValidateUserData.VilidateEmail(email, false)) != null)
                {
                    ChangeUserInfoErrorMessage = temp;
                    FieldState = ControlStates.EmailError;
                    return;
                }

                //Change
                else
                {

                    UnitOfWork.User.Email = email;
                    UnitOfWork.OnUserUpdated(this, new DataChangedArgs<IEnumerable<object>>(new List<object>() { email }, UsersTableFields.Email.ToString(), RepositoryActions.Update));
                }

                //Check Bio
                if(bio != String.Empty)
                {
                    //Delate excess spaces
                    var replaced = System.Text.RegularExpressions.Regex.Replace(bio, @"^(\s*)(\S*)(\s*)$", "$2");

                    UnitOfWork.User.Bio = replaced;
                    UnitOfWork.OnUserUpdated(this, new DataChangedArgs<IEnumerable<object>>(new List<object>() { bio }, UsersTableFields.Bio.ToString(), RepositoryActions.Update));
                }
            }

            FieldState = ControlStates.NormalGray;
            ChangeUserInfoErrorMessage = "";
        }

        void changeUserPass(object objects)
        {
            var data = (object[])objects;

            var oldPass = (string)data[0];
            var newPass = (string)data[1];
            var repeatNewPass = (string)data[2];

            // Check old password
            if(UnitOfWork.User.Password == oldPass)
            {
                string temp;
                // Validate new password
                if ((temp = ValidateUserData.VilidatePassword(new string[] { newPass, repeatNewPass }, true)) != null)
                {
                    ChangePassErrorMessage = temp;
                    FieldState = ControlStates.PasswordError;
                    return;
                }

                //Change password
                UnitOfWork.User.Password = newPass;
                UnitOfWork.OnUserUpdated(this, new DataChangedArgs<IEnumerable<object>>(new List<object>() { newPass }, UsersTableFields.Password.ToString(), RepositoryActions.Update));
            }
            else
            {
                ChangePassErrorMessage = "Wrong old password";
                FieldState = ControlStates.PasswordError;
                return;
            }

            //If all ok 
            ChangePassErrorMessage = "";
            FieldState = ControlStates.NormalGray;
        }

        /// <summary>
        /// On contacts table updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnContactsUpdate(object sender, DataChangedArgs<IEnumerable<Contact>> args)
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

        /// <summary>
        /// On your profile updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnUserUpdates(object sender, DataChangedArgs<IEnumerable<object>> args)
        {
            //Update UI
            OnPropertyChanged("IsYourContact");

            //If open not your profile close
            if (!IsYourAccaunt)
                return;

            UserInfo = UnitOfWork.Contact;

            //Update UI
            OnPropertyChanged("UserName");
            OnPropertyChanged("Bio");
            OnPropertyChanged("Email");
            OnPropertyChanged("IsYourAccaunt");

        }

        void loadInfo(Contact contact)
        {
            //Set user info
            UserInfo = contact;

            //If it's your account, than add contacts list
            if (IsYourAccaunt)
            {
                //Add on user data changed handler
                UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnUserUpdates(sender, args));

                //Load contacts info 
                var contactsList = UnitOfWork.GetUsersInfo(new List<int>(UnitOfWork.User.contactsIdList));

                if (contactsList != null)
                    ContactsList = new ContactsListViewModel(contactsList);
            }

            //If it's not your account, add groups list
            else
            {
                //Make request to the server and get groups list
                GroupsList = new GroupsListViewModel(UnitOfWork.GetUserGroupsInfo(UserInfo));

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
