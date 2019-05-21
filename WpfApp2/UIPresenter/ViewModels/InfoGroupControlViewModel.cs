using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class InfoGroupControlViewModel : BaseViewModel
    {
        #region Public Members

        /// <summary>
        /// Change group info button
        /// </summary>
        public ICommand ChangeGroupInfo { get; set; }

        public Group GroupInfo { get;
            set; }

        /// <summary>
        /// Group members list
        /// </summary>
        public ContactsListViewModel ContactsList { get; set; } = new ContactsListViewModel();

        /// <summary>
        /// Group files list
        /// </summary>
        public ContactsListViewModel FilesList { get; set; } = new ContactsListViewModel();

        /// <summary>
        /// Group photos list
        /// </summary>
        public ContactsListViewModel PhotosList { get; set; } = new ContactsListViewModel();
        
        /// <summary>
        /// Gets user name
        /// </summary>
        public string GroupName => GroupInfo?.Name;

        /// <summary>
        /// Return true if you an admin of the group
        /// </summary>
        public bool AreYouAdmin => GroupInfo.AdminsIdList.Contains(UnitOfWork.User.Id);

        /// <summary>
        /// Return count of online users
        /// </summary>
        public int Online => GroupInfo.UsersOnline;

        /// <summary>
        /// Is any user can join a group
        /// </summary>
        public bool IsPrivate => GroupInfo.IsPrivate.Equals("true", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Is any user can send messages
        /// </summary>
        public bool IsChat => GroupInfo.IsChannel.Equals("false", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Return true if the user is in your contact list
        /// </summary>
        public bool IsYourGroup => UnitOfWork.User.chatsIdList.Contains(GroupInfo.Id);

        /// <summary>
        /// Displays error message on group info change error
        /// </summary>
        public string ChangeGroupInfoErrorMessage { get; set; } = String.Empty;


        /// <summary>
        /// Represent change group info fields state
        /// </summary>
        public ControlStates FieldState { get; set; } = ControlStates.NormalGray;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates group info page
        /// </summary>
        /// <param name="contact">Contact to display info</param>
        public InfoGroupControlViewModel()
        {
            //Setup handlers
            UnitOfWork.GroupsTableRepo.AddDataChangedHandler((sender, args) => OnGroupUpdates(sender, args));
            UnitOfWork.MessagesTableRepo.AddDataChangedHandler((sender, args) => OnMessageUpdates(sender, args));
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnPropertyChanged("IsYourGroup"));
            ApplicationService.GetChatViewModel.OnCurrentChatChanged((sender, args) => loadInfo(args));

            ChangeGroupInfo = new RelayCommandParametrized((data) => changeGroupInfo(data));

            //Load data
            loadInfo(ApplicationService.GetCurrentChoosenChat);
        }


        #endregion

        #region Private Methods

        void changeGroupInfo(object data)
        {
            var groupName = (((object[])data)[0] as string);
            var isPrivate = (bool)((object[])data)[1];
            var isChat = (bool)((object[])data)[2];

            bool update = false;

            //Validate data

            if(ValidateUserData.ValidateUserName(groupName, false) != null)
            {
                ChangeGroupInfoErrorMessage = "Wrong group name";
                FieldState = ControlStates.UserNameError;
                return;
            }
            else
                if(groupName != null)
                {
                    update = true;
                    GroupInfo.Name = groupName;
                }

            if(IsPrivate != isPrivate)
            {
                update = true;
                GroupInfo.IsPrivate = isPrivate.ToString();
            }

            if(IsChat != IsChat)
            {
                update = true;
                GroupInfo.IsPrivate = IsChat.ToString();
            }

            //Update db
            if(update)
                UnitOfWork.GroupsTableRepo.Update(GroupsTableFields.Id.ToString(), GroupInfo.Id.ToString(), GroupInfo);

            ChangeGroupInfoErrorMessage = "";
            FieldState = ControlStates.NormalGray;
        }

        void OnMessageUpdates(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {

        }

        void OnGroupUpdates(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
            //Check specific action 
            if (args.Action == RepositoryActions.Update)
            {
                foreach (var group in args.Data)
                {

                    if (GroupInfo.Id == group.Id)
                    {

                        //Find differences in member list
                        var addedUsers = group.MembersIdList.Except(GroupInfo.MembersIdList).ToList();
                        var deletedUsers = GroupInfo.MembersIdList.Except(group.MembersIdList).ToList();

                        ContactsList.AddContacts(UnitOfWork.GetUsersInfo(addedUsers));
                        ContactsList.RemoveContacts(UnitOfWork.GetUsersInfo(deletedUsers));

                        GroupInfo = group;

                        //Update UI
                        OnPropertyChanged("GroupName");
                        OnPropertyChanged("IsOnline");
                        OnPropertyChanged("AreYouAdmin");
                        OnPropertyChanged("IsYourGroup");

                        break;

                    }

                }
            }
        }

        void OnUserUpdates(object sender, EventArgs args)
        {
            //Update UI
            OnPropertyChanged("IsYourGroup");

        }

        void loadInfo(Group group)
        {
            //Set group info
            GroupInfo = group;

            //add contacts to contact list all members list
            if (GroupInfo.MembersIdList != null)
            {
                var membersInfo = UnitOfWork.GetUsersInfo(new List<int>(GroupInfo.MembersIdList));

                ContactsList = new ContactsListViewModel(membersInfo, true, false, AreYouAdmin);
            }

            //Update UI
            OnPropertyChanged("GroupName");
            OnPropertyChanged("IsYourGroup");
            OnPropertyChanged("IsOnline");
            OnPropertyChanged("AreYouAdmin");
        }

        #endregion
    }
}
