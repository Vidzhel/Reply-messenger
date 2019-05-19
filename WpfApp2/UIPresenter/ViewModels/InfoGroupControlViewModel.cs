using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.InversionOfControl;

namespace UI.UIPresenter.ViewModels
{
    public class InfoGroupControlViewModel : BaseViewModel
    {
        #region Public Members


        public Group GroupInfo { get; set; }

        public ContactsListViewModel ContactsList { get; set; } = new ContactsListViewModel();

        public ContactsListViewModel FilesList { get; set; } = new ContactsListViewModel();

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
        public int IsOnline => GroupInfo.UsersOnline;

        /// <summary>
        /// Return true if the user is in your contact list
        /// </summary>
        public bool IsYourGroup => UnitOfWork.User.chatsIdList.Contains(GroupInfo.Id);
        
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

            //Load data
            loadInfo(ApplicationService.GetCurrentChoosenChat);
        }


        #endregion

        #region Private Methods

        void OnMessageUpdates(object sender, DataChangedArgs<IEnumerable<Message>> args)
        {

        }

        void OnGroupUpdates(object sender, DataChangedArgs<IEnumerable<Group>> args)
        {
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

                ContactsList = new ContactsListViewModel(membersInfo);
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
