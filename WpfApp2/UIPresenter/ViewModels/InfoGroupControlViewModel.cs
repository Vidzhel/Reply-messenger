﻿using ClientLibs.Core;
using ClientLibs.Core.DataAccess;
using CommonLibs.Connections.Repositories;
using CommonLibs.Connections.Repositories.Tables;
using CommonLibs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using UI.InversionOfControl;
using WpfApp2;

namespace UI.UIPresenter.ViewModels
{
    public class InfoGroupControlViewModel : BaseViewModel
    {
        #region Public Members

        /// <summary>
        /// Change group info button
        /// </summary>
        public ICommand ChangeGroupInfo { get; set; }

        //Deletes group
        public ICommand DeleteGroup { get; set; }

        /// <summary>
        /// Changes group photo
        /// </summary>
        public ICommand ChangeGroupPhoto { get; set; }

        public Group GroupInfo { get; set; }

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

        public FilesListViewModel SharedMedia { get; set; } = new FilesListViewModel();

        public string GroupPhoto
        {
            get
            {
                var list = UnitOfWork.GetFilesByName(new List<string>() { GroupInfo.Image });
                return (list.Result)[0];
            }
        }

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
            UnitOfWork.Database.GroupsTableRepo.AddDataChangedHandler((sender, args) => OnGroupUpdates(sender, args));
            UnitOfWork.Database.MessagesTableRepo.AddDataChangedHandler((sender, args) => OnMessageUpdates(sender, args));
            UnitOfWork.AddUserInfoUpdatedHandler((sender, args) => OnPropertyChanged("IsYourGroup"));
            ApplicationService.GetChatViewModel.OnCurrentChatChanged((sender, args) => loadInfo(args));


            ChangeGroupPhoto = new RelayCommand(changeGroupPhoto);
            ChangeGroupInfo = new RelayCommandParametrized((data) => changeGroupInfo(data));
            DeleteGroup = new RelayCommand(deleteGroup);

            //Load data
            loadInfo(ApplicationService.GetCurrentChoosenChat);
        }


        #endregion

        #region Private Methods


        async void changeGroupPhoto()
        {
            if (!AreYouAdmin)
                return;

            var photoPath = FileManager.OpenFileDialogForm("Image files(*.png;*jpg) | *.png;*jpg");

            if (photoPath == String.Empty)
                return;

            //Copy photo to
            var name = await UnitOfWork.SendFile(photoPath);
            var newImageDest = Directory.GetCurrentDirectory() + @"\Reply Messenger\Saved Files\" + name;
            if (!System.IO.File.Exists(newImageDest))
                System.IO.File.Copy(photoPath, newImageDest);

            var newGroup = new Group(GroupInfo);
            newGroup.Image = Path.GetFileName(newImageDest);

            UnitOfWork.ChangeGroupInfo(newGroup);

            OnPropertyChanged("ProfilePhoto");
        }


        async void deleteGroup()
        {
            await UnitOfWork.RemoveGroup(GroupInfo);
            await UnitOfWork.LeaveGroup(GroupInfo);
            ApplicationService.ChangeCurrentChatPage(Pages.ChatPages.UserInfo);
        }

        void changeGroupInfo(object data)
        {
            var groupName = (((object[])data)[0] as string);
            var isPrivate = (bool)((object[])data)[1];
            var isChat = (bool)((object[])data)[2];

            bool update = false;

            //Validate data

            if (ValidateUserData.ValidateUserName(groupName, false) != null)
            {
                ChangeGroupInfoErrorMessage = "Wrong group name";
                FieldState = ControlStates.UserNameError;
                return;
            }
            else
            {
                if (groupName == null)
                {
                    groupName = GroupInfo.Name;
                }
                else
                {
                    update = true;
                }
            }

            if (IsPrivate != isPrivate)
                update = true;

            if(IsChat != isChat)
                update = true;

            if(update)
                UnitOfWork.ChangeGroupInfo(new Group(isPrivate, groupName, !isChat, GroupInfo.Image, GroupInfo.Id, GroupInfo.AdminsIdList, GroupInfo.MembersIdList, GroupInfo.UsersOnline));

            ChangeGroupInfoErrorMessage = "";
            FieldState = ControlStates.NormalGray;
        }

        async void OnGroupUpdates(object sender, DataChangedArgs<IEnumerable<Group>> args)
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

                        ContactsList.AddContacts(await UnitOfWork.GetUsersInfo(addedUsers));
                        ContactsList.RemoveContacts(await UnitOfWork.GetUsersInfo(deletedUsers));

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

        async void loadInfo(Group group)
        {
            //Set group info
            GroupInfo = group;

            //add contacts to contact list all members list
            if (GroupInfo.MembersIdList != null)
            {
                var membersInfo = await UnitOfWork.GetUsersInfo(new List<int>(GroupInfo.MembersIdList));

                ContactsList = new ContactsListViewModel(membersInfo, true, false, AreYouAdmin);
            }

            //Load shared files
            List<string> filesPath = new List<string>();
            var messages = UnitOfWork.Database.MessagesTableRepo.Find(MessagesTableFields.ReceiverId.ToString(), GroupInfo.Id.ToString());
            foreach (var mess in messages)
            {
                if (mess.AttachmentsList.Count != 0)
                    foreach (var attachment in mess.AttachmentsList)
                        filesPath.Add(attachment);
            }

            foreach (var file in await UnitOfWork.GetFilesByName(filesPath))
             //Becouse Items is ObservableCollection we should update elements from the main thread
                    await App.Current.Dispatcher.Invoke(async () =>
                    {
                        SharedMedia.Items.Add(new FilesListItemViewModel(file, false, false));
                    });

            //Update UI
            OnPropertyChanged("GroupName");
            OnPropertyChanged("IsYourGroup");
            OnPropertyChanged("IsOnline");
            OnPropertyChanged("AreYouAdmin");
        }

        #endregion
    }
}
