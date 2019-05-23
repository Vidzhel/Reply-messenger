using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using Common.Data.Security;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Security;
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
        /// Creates new group
        /// </summary>
        public ICommand CreateGroup { get; set; }

        /// <summary>
        /// Displays error message on user info change error
        /// </summary>
        public string ChangeUserInfoErrorMessage { get; set; } = String.Empty;

        /// <summary>
        /// Displays error message on user change pass error
        /// </summary>
        public string ChangePassErrorMessage { get; set; } = String.Empty;

        /// <summary>
        /// Displays error message on create group error
        /// </summary>
        public string CreateGroupErrorMessage { get; set; } = String.Empty;

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
            CreateGroup = new RelayCommandParametrized((objects) => createGroup(objects));

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
            if (name != String.Empty)
            {
                if ((temp = ValidateUserData.ValidateUserName(name, false)) != null)
                {
                    ChangeUserInfoErrorMessage = temp;
                    FieldState = ControlStates.UserNameError;
                    return;
                }
            }
            else
                name = UnitOfWork.User.UserName;


            //Check user email
            if (email != String.Empty)
            {
                if ((temp = ValidateUserData.ValidateEmail(email, false)) != null)
                {
                    ChangeUserInfoErrorMessage = temp;
                    FieldState = ControlStates.EmailError;
                    return;
                }
            }
            else
                email = UnitOfWork.User.Email;

            //Check Bio
            if (bio != String.Empty)
                //Delate excess spaces
                bio = System.Text.RegularExpressions.Regex.Replace(bio, @"^(\s*)(\S*)(\s*)$", "$2");
            else
                bio = UnitOfWork.User.Bio;

            //Make request to server
            var errorMess = UnitOfWork.ChangeUserInfo(new User(name, UnitOfWork.User.Password, email, bio, UnitOfWork.User.ProfilePhoto, "true", UnitOfWork.User.chatsIdList, UnitOfWork.User.contactsIdList, UnitOfWork.User.Id));

            if(errorMess != null)
                ChangeUserInfoErrorMessage = errorMess;

            FieldState = ControlStates.NormalGray;
            ChangeUserInfoErrorMessage = "";
        }

        void createGroup(object objects)
        {
            var groupName = (string)((object[])objects)[0];
            var isPrivate = (bool)((object[])objects)[1];
            var isPublic = (bool)((object[])objects)[2];
            var isChat = (bool)((object[])objects)[3];
            var isChannel = (bool)((object[])objects)[4];

            string temp;

            if((temp = ValidateUserData.ValidateUserName(groupName, true)) != null)
            {
                CreateGroupErrorMessage = temp;
                FieldState = ControlStates.GroupName;
                return;
            }

            //If chat accessability isn't specified
            if(!isPublic && !isPrivate)
            {
                CreateGroupErrorMessage = "Chat accessability isn't specified";
                FieldState = ControlStates.AccessabilityError;
                return;
            }

            //If chat type isn't specified
            if(!isChat && !isChannel)
            {
                CreateGroupErrorMessage = "Chat type isn't specified";
                FieldState = ControlStates.ChatType;
                return;
            }

            UnitOfWork.CreateGroup(new Group(isPrivate, groupName, isChannel, "", 0, new List<int>() { UnitOfWork.User.Id }, new List<int>() { UnitOfWork.User.Id }, 1));

            CreateGroupErrorMessage = "";
            FieldState = ControlStates.NormalGray;
        }

        void changeUserPass(object objects)
        {
            var oldPass = (objects as IHaveThreePasswords).OldStringPassword;
            var newPass = (objects as IHaveThreePasswords).StringPassword;
            var repeatNewPass = (objects as IHaveThreePasswords).RepeatStringPassword;

            string temp;
            // Validate new password
            if ((temp = ValidateUserData.ValidatePassword(new SecureString[] { newPass, repeatNewPass }, true)) != null)
            {
                ChangePassErrorMessage = temp;
                FieldState = ControlStates.PasswordError;
                return;
            }

            var errorMess = UnitOfWork.ChangePassword(oldPass.GetHash(), newPass.GetHash());

            if (errorMess != null)
            {
                ChangePassErrorMessage = errorMess;
                FieldState = ControlStates.PasswordError;
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
            if (contact == null)
                return;

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
